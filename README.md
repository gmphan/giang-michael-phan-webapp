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

The container now serves **HTTP on port 80 only**. Public HTTPS is terminated by Cloudflare at the edge; the Cloudflare Tunnel forwards to the origin over plain HTTP on the LAN. Kestrel no longer listens on `443` and does not load a certificate. Configure the PostgreSQL connection with `ConnectionStrings__DefaultConnection`. The production Compose file expects an environment file at `Ubuntu/app/variables.prd.env` and a PostgreSQL service reachable as `postgres`; configure those deployment resources before starting it.

The relevant `variables.prd.env` application settings are:

```sh
ASPNETCORE_ENVIRONMENT=Production
ASPNETCORE_URLS=http://*:80
```

The former `ASPNETCORE_Kestrel__Certificates__Default__Path` and `...__Password` settings and the `https://*:443` Kestrel URL have been removed, along with the `443:443` port mapping and the `./certs:/https` volume mount, because the origin no longer serves TLS directly.

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

- Keep database passwords, admin credentials, and identity-provider secrets in environment variables or a local secret store. Do not add them to source control or paste them into this README.
- The origin no longer serves TLS directly (Cloudflare terminates HTTPS at the edge), so no production Kestrel certificate is mounted in the container. The `https` directory holds local development certificates only; do not commit `.pfx` files or other private keys if they contain any real secrets.
- If a credential or private key has already been committed or shared, rotate it.
- If rotating the PostgreSQL password, update both `POSTGRES_PASSWORD` in `Ubuntu/app/variables.prd.env` and the password for the PostgreSQL role in the running database. The official PostgreSQL image only applies `POSTGRES_PASSWORD` when initializing an empty data directory; changing the env file alone does not update credentials in an existing persistent volume.
- The application seeds its initial admin identity during startup. Review the admin settings for the selected environment and replace any default credentials before exposing the app publicly.

## Deployment Files

- `Ubuntu/app/docker-compose.prd.yml` describes the production stack: the `webapp` container (HTTP on `80`), the `postgres` container, the environment file, and the `postgres_data` volume. It contains no certificate mount or `443` mapping because TLS is terminated at the Cloudflare edge.
- `Ubuntu/database/docker-compose.yml` describes a PostgreSQL 16 container with a persistent volume.
- `runDemo.md` contains additional certificate, image, and container examples.

### Cloudflare DNS and tunnel cutover

DNS for `giangmichaelphan.com` is hosted at **Cloudflare**. Its relevant records are all tunnel-backed, so they are independent of the server's public IP:

| Type | Name | Value | Proxied | Notes |
| --- | --- | --- | --- | --- |
| CNAME (flattened) | `@` (`giangmichaelphan.com`) | tunnel target (`*.cfargotunnel.com`) | Proxied (tunnel) | Portfolio via tunnel. |
| CNAME | `www` | tunnel target (`*.cfargotunnel.com`) | Proxied (tunnel) | Portfolio via tunnel. |
| CNAME | `signage` | `*.cfargotunnel.com` | Proxied (tunnel) | Signage via tunnel. |
| CNAME | `portainer` | `*.cfargotunnel.com` | Proxied (tunnel) | Portainer via tunnel (Access-gated). |

None of these depend on the public IP, so **no DDNS record is needed**. The old apex `A` record (→ public IP), the IP-dependent DDNS setup, and the `2222` SSH forward have been retired.

#### Cutover runbook (direct → tunnel)

1. **Deploy the proxy-aware app.** Publish and deploy the `GmphanMvc` image that includes the `UseForwardedHeaders` change, so the origin is ready before DNS flips.
2. **Add tunnel hostnames** on `gp-signage`: `giangmichaelphan.com` and `www.giangmichaelphan.com` → `http://192.168.68.130:80`. Cloudflare creates the flattened apex + `www` records pointing at the tunnel.
3. **Verify** through Cloudflare: `https://giangmichaelphan.com` and `https://www.giangmichaelphan.com` load with a valid certificate and no redirect loop. Confirm `curl -sI https://giangmichaelphan.com` shows a `200`, not a `307` to itself.
4. **Remove the router forwards** for `443`, `80` (and `2222` if it was ever added). Confirm `https://<public-ip>` no longer reaches the site (expected).
5. **Retire ddns-updater:** remove the `ddns-updater` service and `ddns-data` from the production Compose stack, drop `CLOUDFLARE_API_TOKEN` from `variables.prd.env`, and remove the `CLOUDFLARE_API_TOKEN` secret. Nothing points at the public IP anymore, so DDNS has no job.
6. **Update DNS:** the apex A record is replaced by the tunnel CNAME automatically when the tunnel hostname is added; confirm no leftover apex A record remains in Cloudflare.

#### Reverting to direct (tunnel → direct)

The migration is reversible:

1. **Remove the tunnel hostnames** for `giangmichaelphan.com` and `www` (and any `www` redirect rule).
2. **Re-create the apex A record** → the server's public IP, **DNS only (grey cloud)**, and `www` as a CNAME to the apex (or its own A record).
3. **Re-add the router forwards** for `443` and `80` to the server.
4. **Re-enable ddns-updater** with the apex `@` A record as its `settings` entry (Cloudflare provider, `proxied: false`) and restore `CLOUDFLARE_API_TOKEN` in `variables.prd.env`.
5. Optionally drop the `UseForwardedHeaders` change — it is harmless behind a direct forward but no longer required.

#### Cloudflare API token (only if ddns-updater is used again)

ddns-updater authenticates to Cloudflare with a scoped **API token** (not the tunnel token, which can only connect a connector). If DDNS is re-enabled, create a token with **Zone > DNS > Edit** scoped to **Specific zone > `giangmichaelphan.com`**, and put it in `variables.prd.env` as `CLOUDFLARE_API_TOKEN`. The config uses `"provider": "cloudflare"` with the zone's **Zone ID** as `zone_identifier`, `host: "@"`, `proxied: false`. Never add the tunnel CNAME hostnames to its `settings`.

## Network Access and TLS

Inbound service access is deliberately funneled through a small, well-defined set of entry points. Publicly exposed ports are kept to a minimum; admin and internal tooling is reached through a Cloudflare Tunnel with Cloudflare Access instead of raw port forwards.

### Service and Port Map

| Service | Public hostname / access | Router forward | Notes |
| --- | --- | --- | --- |
| Portfolio web app | `giangmichaelphan.com` | None | Reached through the `gp-signage` Cloudflare Tunnel (origin over HTTP on the LAN). |
| Signage web app | `signage.giangmichaelphan.com` | None | Reached through the `gp-signage` Cloudflare Tunnel. |
| Portainer | `portainer.giangmichaelphan.com` | None | Reached through the `gp-signage` Cloudflare Tunnel, gated by Cloudflare Access. |
| SSH | LAN only (`gphan@192.168.68.130`) | None | Internal access only; no public SSH entry. |

With the portfolio and signage behind the tunnel, **no inbound router ports are required** for web traffic, and SSH is LAN-only. All prior forwards (portfolio `443`/`80`, signage `8080`/`8443`, Portainer `9443`, and `2222`) have been removed. Inbound web access is tunnel-only.

### Cloudflare Tunnel (`gp-signage`)

A single named tunnel, `gp-signage`, provides public HTTPS hostnames for services that should not have router port forwards. Cloudflare terminates TLS at the edge with a real certificate and forwards to the origin over the tunnel, so no inbound router port needs to be open.

Public hostnames configured on the tunnel:

| Hostname | Origin (Service) | Cloudflare Access | TLS to origin |
| --- | --- | --- | --- |
| `giangmichaelphan.com` | `http://192.168.68.130:80` (portfolio) | Public | Standard (origin is HTTP) |
| `www.giangmichaelphan.com` | `http://192.168.68.130:80` (portfolio) | Public | Standard (origin is HTTP) |
| `signage.giangmichaelphan.com` | Signage app on the server | Per deployment | Standard |
| `portainer.giangmichaelphan.com` | `https://192.168.68.130:9443` | Yes: email allow list | `No TLS Verify` required |

Cloudflare creates a DNS `CNAME` for each hostname that points at the tunnel, for example:

```
portainer.giangmichaelphan.com  CNAME  05e8f5ca-1409-4a6a-ba12-8cb8229389c5.cfargotunnel.com
```

The apex `giangmichaelphan.com` is stored by Cloudflare as a **CNAME-flattened** record to the tunnel (a plain CNAME is not permitted at the zone apex). `www` is a normal tunnel CNAME.

#### Portfolio behind the tunnel

The portfolio is reached by the tunnel over **plain HTTP on the LAN** (`http://192.168.68.130:80`); Cloudflare terminates TLS at the edge with a valid certificate. This avoids the self-signed-certificate problem that Portainer has, because the tunnel connector talks HTTP to the origin and Cloudflare owns the public TLS.

Because the origin is now behind a proxy, the app must honor the `X-Forwarded-*` headers Cloudflare sends. `src/GmphanMvc/Program.cs` registers `ForwardedHeadersOptions` (for `XForwardedFor` and `XForwardedProto`) and calls `app.UseForwardedHeaders()` as the **first** middleware, before `UseHttpsRedirection`, `UseAuthentication`, and `UseRouting`. Without this, the app would see every request as `http` and `UseHttpsRedirection()` would loop (ERR_TOO_MANY_REDIRECTS). `KnownNetworks`/`KnownProxies` are cleared because the tunnel connector reaches the origin from the LAN, not loopback; this is safe only while origin port 80 is not publicly forwarded.

`UseHttpsRedirection()` and `UseHsts()` remain enabled. Behind the tunnel they behave correctly because forwarded headers make the request scheme read as `https`, so no redirect loop occurs and HSTS headers are still sent. Note, however, that a request to the origin on plain **HTTP `:80` without** an `X-Forwarded-Proto: https` header (for example, hitting `http://192.168.68.130` directly on the LAN) will be redirected to `https`, and since Kestrel no longer listens on `443`, that redirect target is unreachable. This only affects direct-to-origin access; all public traffic arrives through the tunnel with the header present.

The portal: Cloudflare Access is not required for the portfolio (it is public), so the apex and `www` have no Access policy. Internal admin/signin pages still rely on the application's own Identity authentication.

#### Portainer behind the tunnel (`No TLS Verify`)

Portainer serves its own **self-signed** certificate on port `9443`. That certificate is issued for `0.0.0.0` (empty subject and issuer), so the tunnel connector cannot validate it against the origin host `192.168.68.130`. Without `No TLS Verify`, the tunnel fails the request and returns `502 Bad Gateway`:

```
ERR Unable to reach the origin service: tls: failed to verify certificate:
    x509: certificate is valid for 0.0.0.0, not 192.168.68.130
    originService=https://192.168.68.130:9443
```

The fix is to disable origin TLS verification on that route only:

- Cloudflare Zero Trust > Networks > Tunnels > `gp-signage` > Public Hostnames > `portainer.giangmichaelphan.com` > Edit.
- Additional application settings > TLS > **No TLS Verify = ON**.

In configuration (YAML) form this is `originRequest.noTLSVerify: true` for that ingress rule:

```yaml
ingress:
  - hostname: portainer.giangmichaelphan.com
    service: https://192.168.68.130:9443
    originRequest:
      noTLSVerify: true
  # ... other hostnames ...
  - service: http_status:404
```

This is safe. `No TLS Verify` only affects the LAN hop between the tunnel connector and the origin. The public leg still uses a valid Cloudflare certificate, traffic stays on the local network, and Cloudflare Access still requires login. The origin Service type must be **HTTPS** (not HTTP) because Portainer serves TLS on `9443`.

#### Cloudflare Access

`portainer.giangmichaelphan.com` is protected by a Cloudflare Access self-hosted application with a single `Allow` policy keyed to an email address (the same identity used for the signage tunnel). Portainer controls every container on the host (start/stop/delete, inspect env vars and secrets, spawn privileged containers), so it must never be exposed without authentication. Keep the session duration for this admin panel short.

To reach Portainer away from home, open `https://portainer.giangmichaelphan.com`, complete the Cloudflare Access login, and use the Portainer UI. On the LAN, `https://192.168.68.130:9443` continues to work directly.

### SSH Access

SSH is **LAN-only**. There is no public SSH entry point and no router port forward. Connect from inside the network to the server's LAN address:

```sh
ssh gphan@192.168.68.130        # default port 22
ssh -p 2222 gphan@192.168.68.130 # if sshd is configured on 2222
```

Key-based authentication is used (see the SSH alias configuration below). Keep password authentication disabled in `sshd_config`. Because SSH is not reachable from the internet, there is no public `:2222` forward to maintain. If remote SSH is needed later, prefer routing it through the Cloudflare Tunnel with `cloudflared access ssh` rather than opening a port on the router.

### ddns-updater (only needed for direct/IP-dependent records)

With the portfolio and signage behind the Cloudflare Tunnel and SSH LAN-only, **no public DNS record points at the server's public IP**, so **ddns-updater is not needed** in this configuration. When the ISP changes the public IP, the tunnel reconnects outbound (identified by its tunnel token, not the IP) and nothing breaks. The `ddns-updater` container and its config are therefore retired as part of the tunnel cutover; see the cutover runbook below.

ddns-updater would only be required again if some service is reached **directly** by public IP (for example a future DNS-only `ssh.` A-record, or reverting the portfolio to a direct forward). In that case it would manage **only** the IP-dependent A record(s) and must never include the tunnel CNAMEs (`giangmichaelphan.com`, `www`, `signage`, `portainer`) in its `settings`.

### TLS Summary

- **Public edge**: valid certificates issued by Cloudflare at the edge for all tunnel hostnames (portfolio apex/`www`, signage, Portainer).
- **Tunnel origin (portfolio)**: origin reached over HTTP on the LAN; Cloudflare owns public TLS. No origin certificate needed for this path.
- **Tunnel origin (Portainer)**: self-signed `9443` certificate with `No TLS Verify` enabled on that route only.
- **Local development**: ASP.NET Core dev certificate generated with `dotnet dev-certs` (see `runDemo.md`).
- Do not commit `.pfx` files, private keys, Cloudflare API tokens, tunnel credentials, or DDNS `config.json`. The `gp-signage` tunnel token/credentials JSON and any Cloudflare DNS-Edit API token are separate secrets and must both be kept out of source control.

## Copy Files to the Server

The Mac is configured with an Ed25519 SSH key and the `ubuntu-server` alias. SSH is **LAN-only** (there is no public SSH entry point), so the alias targets the server's LAN address. To reproduce that setup on another Mac, generate a key pair and install its public key on the server:

```sh
ssh-keygen -t ed25519 -C "macbook-to-ubuntu"
ssh-copy-id gphan@192.168.68.130
```

The key-generation command prompts for a file location and passphrase. The current Mac setup uses the default `~/.ssh/id_ed25519` location and an empty passphrase for non-interactive transfers. An unencrypted private key requires protecting access to the Mac and the key file.

Add this host entry to `~/.ssh/config`:

```sshconfig
Host ubuntu-server
    HostName 192.168.68.130
    User gphan
    Port 22
    IdentityFile ~/.ssh/id_ed25519
```

Set restrictive permissions and test the alias:

```sh
chmod 600 ~/.ssh/config
ssh ubuntu-server
```

From the repository root, copy the private environment file and Compose stack file to the deployment directory using the alias:

```sh
scp Ubuntu/app/variables.prd.env \
	ubuntu-server:~/development/giang-michael-phan-webapp/app/stack-prd/

scp Ubuntu/app/docker-compose.prd.yml \
	ubuntu-server:~/development/giang-michael-phan-webapp/app/stack-prd/
```

The env file contains secrets; transfer it only to a trusted server and keep it out of source control. Keep the env file beside the Compose file on the server. The SSH alias is local to the Mac and must also be configured on any other machine that uses these commands.

> With the portfolio behind the Cloudflare Tunnel, the `ddns-data` directory and `ddns-updater` service are no longer part of the stack. If you revert to a direct forward, re-add them per the "Reverting to direct" runbook above.

Review the Compose files and environment-specific settings before deployment. In particular, make sure the web container and PostgreSQL share a Docker network and that the configured connection string and published image match the target host.
