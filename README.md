# Giang Michael Phan Web App

An ASP.NET Core MVC portfolio application with visitor pages for the resume, projects, posts, quotes, about information, and contact form. Administrative pages and sign-in are provided through separate MVC and Identity areas. The application uses Entity Framework Core with PostgreSQL and ASP.NET Core Identity.

## Technology

- .NET 8 / ASP.NET Core MVC
- Entity Framework Core and PostgreSQL
- ASP.NET Core Identity
- Serilog
- Docker for container builds and deployment

## Repository Layout

- `src/GmphanMvc` - web application, MVC areas, configuration, and views
- `src/Gmphan.BusinessAccessLib` - application services
- `src/Gmphan.DataAccessLib` - EF Core context, repositories, and migrations
- `src/Gmphan.ModelLib` - entities and view models
- `src/Gmphan.UtilityLib` - shared utilities
- `Ubuntu/database` - PostgreSQL Docker Compose configuration
- `Ubuntu/app` - production application Docker Compose configuration
- `https` - local HTTPS certificate files; keep these private

## Prerequisites

- .NET 8 SDK
- PostgreSQL, either installed locally or started with Docker Compose
- Docker only if building or running the container image

## Run Locally

1. Configure a PostgreSQL database and set the `ConnectionStrings__DefaultConnection` environment variable to its connection string. For example, use your own local database credentials:

	```sh
	export ConnectionStrings__DefaultConnection='Host=localhost;Port=5432;Database=<database>;Username=<user>;Password=<password>'
	```

2. Apply the EF Core migrations. Install the `dotnet-ef` tool if it is not already available:

	```sh
	dotnet tool install --global dotnet-ef
	dotnet ef database update \
	  --project src/Gmphan.DataAccessLib \
	  --startup-project src/GmphanMvc
	```

3. Start the application:

	```sh
	dotnet watch --project src/GmphanMvc run
	```

	The HTTPS launch profile is configured for `https://localhost:7090` and `http://localhost:5095`. The HTTP profile is available with:

	```sh
	dotnet run --project src/GmphanMvc --launch-profile http
	```

## Build and Publish

From the repository root:

```sh
dotnet build -c Release src/GmphanMvc
dotnet publish -c Release src/GmphanMvc
```

The `buildAndPublish.sh` script performs these commands and then builds and pushes the Linux AMD64 image `ghcr.io/gmphan/gmphan-webapp:latest`. Authenticate to GitHub Container Registry before running the script if you intend to push the image.

## Docker

The root `Dockerfile` uses the ASP.NET 8 runtime image and copies the published output from `src/GmphanMvc/bin/Release/net8.0/publish`. Build the application first, then build the image:

```sh
dotnet publish -c Release src/GmphanMvc
docker build -t gmphan-webapp .
```

The container listens on ports 80 and 443. Configure its PostgreSQL connection with `ConnectionStrings__DefaultConnection` and configure HTTPS using an ASP.NET Core Kestrel certificate path and password. The production Compose file expects an environment file at `Ubuntu/app/variables.prd.env`, a certificate directory mounted at `/https`, and a PostgreSQL service reachable as `postgres`; configure those deployment resources before starting it.

Start the production stack from the repository root with:

```sh
docker compose --env-file Ubuntu/app/variables.prd.env \
	-f Ubuntu/app/docker-compose.prd.yml up -d
```

The Compose file reads the PostgreSQL credentials from that env file and supplies the matching connection string to the web app. `variables.prd.env` is ignored by Git and should remain private.

To deploy an updated image, first publish it to the registry. Then run these commands on the server from the stack directory:

```sh
cd ~/development/giang-michael-phan-webapp/app/stack-prd
docker compose --env-file variables.prd.env -f docker-compose.prd.yml pull webapp
docker compose --env-file variables.prd.env -f docker-compose.prd.yml up -d
```

Compose recreates the web container when the pulled image or configuration has changed. For env-file or Compose-only changes, run just the `up -d` command. There is no need to run `down`; avoid `down -v`, which removes the persistent PostgreSQL volume.

View the latest 100 application log lines and continue following new output with:

```sh
docker compose --env-file variables.prd.env -f docker-compose.prd.yml logs --tail=100 -f webapp
```

Press `Ctrl+C` to stop following logs; this does not stop the application container.

## Configuration and Security

- Keep database passwords, admin credentials, identity-provider secrets, and certificate passwords in environment variables or a local secret store. Do not add them to source control or paste them into this README.
- Do not commit `.pfx` files or other private keys. The Docker build context currently includes the `https` directory, so ensure it contains no production secrets before building or publishing an image.
- If a credential or private key has already been committed or shared, rotate it.
- If rotating the PostgreSQL password, update both `POSTGRES_PASSWORD` in `Ubuntu/app/variables.prd.env` and the password for the PostgreSQL role in the running database. The official PostgreSQL image only applies `POSTGRES_PASSWORD` when initializing an empty data directory; changing the env file alone does not update credentials in an existing persistent volume.
- The application seeds its initial admin identity during startup. Review the admin settings for the selected environment and replace any default credentials before exposing the app publicly.

## Deployment Files

- `Ubuntu/app/docker-compose.prd.yml` describes the production web container, ports, environment file, and certificate mount.
- `Ubuntu/database/docker-compose.yml` describes a PostgreSQL 16 container with a persistent volume.
- `runDemo.md` contains additional certificate, image, and container examples.

### Namecheap Dynamic DNS

The production Compose stack includes `ddns-updater`, which reads its provider settings from `config.json` in the `ddns-data` directory and updates the Namecheap root-domain DNS record for `giangmichaelphan.com` when the server's public IP changes. Compose mounts the server's `./ddns-data` directory at `/updater/data` in the updater container, so the file must be at `ddns-data/config.json` beside the Compose file on the server. The local secret configuration is kept at `Ubuntu/DdnsContainer/config.json` and is ignored by Git.

The current Namecheap host records are:

| Type | Host | Value | TTL |
| --- | --- | --- | --- |
| A + Dynamic DNS Record | `@` | `104.8.78.220` (current public IP; updated by DDNS) | Automatic |
| CNAME Record | `www` | `giangmichaelphan.com.` | Automatic |

The `@` record points the root domain to the server. The `www` record aliases `www.giangmichaelphan.com` to the root domain.

1. In Namecheap, enable Dynamic DNS for `giangmichaelphan.com` and create an `A + Dynamic DNS Record` with host `@`. Set its initial value to the server's current public IP (`104.8.78.220` in the current setup) and copy the generated Dynamic DNS password. Add a `CNAME Record` with host `www` and value `giangmichaelphan.com.`.
2. Put the Namecheap provider settings and generated Dynamic DNS password in the private `Ubuntu/DdnsContainer/config.json`. Do not commit or share this file.
3. Copy the config file to the server's `ddns-data` directory and recreate the updater from the stack directory:

	```sh
	scp Ubuntu/DdnsContainer/config.json \
		ubuntu-server:~/development/giang-michael-phan-webapp/app/stack-prd/ddns-data/config.json
	ssh ubuntu-server 'cd ~/development/giang-michael-phan-webapp/app/stack-prd && docker compose --env-file variables.prd.env -f docker-compose.prd.yml up -d --force-recreate ddns-updater'
	ssh ubuntu-server 'cd ~/development/giang-michael-phan-webapp/app/stack-prd && docker compose -f docker-compose.prd.yml logs --tail=100 -f ddns-updater'
	```

The DDNS password is stored in `config.json`, not `variables.prd.env`; protect the file on both machines. The updater's status UI is published on port 8000 by the current Compose file. Restrict that port with the server firewall/router, or bind it to localhost and use an SSH tunnel if you need remote access; do not expose the UI publicly.

DNS only points the hostname to the server's public IP. The router/firewall must also forward TCP ports 80 and 443 to the web server, and port 2222 for SSH, and the server should have a stable LAN address. DDNS cannot provide inbound access if the ISP places the connection behind CGNAT.

### Copy Files to the Server

The Mac is configured with an Ed25519 SSH key and the `ubuntu-server` alias. To reproduce that setup on another Mac, generate a key pair and install its public key on the server:

```sh
ssh-keygen -t ed25519 -C "macbook-to-ubuntu"
ssh-copy-id -p 2222 gphan@giangmichaelphan.com
```

The key-generation command prompts for a file location and passphrase. The current Mac setup uses the default `~/.ssh/id_ed25519` location and an empty passphrase for non-interactive transfers. An unencrypted private key requires protecting access to the Mac and the key file.

Add this host entry to `~/.ssh/config`:

```sshconfig
Host ubuntu-server
	HostName giangmichaelphan.com
    User gphan
    Port 2222
    IdentityFile ~/.ssh/id_ed25519
```

Set restrictive permissions and test the alias:

```sh
chmod 600 ~/.ssh/config
ssh ubuntu-server
```

From the repository root, copy the private environment file and Compose stack file to the deployment directory using the alias:

```sh
ssh ubuntu-server 'mkdir -p ~/development/giang-michael-phan-webapp/app/stack-prd/ddns-data && chmod 700 ~/development/giang-michael-phan-webapp/app/stack-prd/ddns-data'

scp Ubuntu/app/variables.prd.env \
	ubuntu-server:~/development/giang-michael-phan-webapp/app/stack-prd/

scp Ubuntu/app/docker-compose.prd.yml \
	ubuntu-server:~/development/giang-michael-phan-webapp/app/stack-prd/

scp Ubuntu/DdnsContainer/config.json \
	ubuntu-server:~/development/giang-michael-phan-webapp/app/stack-prd/ddns-data/config.json
```

The env file and DDNS config contain secrets; transfer them only to a trusted server and keep them out of source control. Keep the env file beside the Compose file and the DDNS config inside `ddns-data` on the server. The SSH alias is local to the Mac and must also be configured on any other machine that uses these commands.

Review the Compose files and environment-specific settings before deployment. In particular, make sure the web container and PostgreSQL share a Docker network and that the configured connection string, certificate path, and published image match the target host.
