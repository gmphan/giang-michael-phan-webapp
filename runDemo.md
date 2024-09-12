### Generate cert and configure local machine:
```
dotnet dev-certs https -ep ./https/aspnetapp.pfx -p crypticpassword
dotnet dev-certs https --trust
```

### Build, tag and push the Docker image for multi-platform.
```
docker buildx build --platform linux/amd64,linux/arm64 -t ocbuuregistry.azurecr.io/gmphan-webapp:latest .
docker push ocbuuregistry.azurecr.io/gmphan-webapp:latest
```

### To verify if the image has been built for multiple platforms, you can inspect the manifest:
```
docker manifest inspect ocbuuregistry.azurecr.io/gmphan-webapp:latest
```

### Run the container image with ASP.NET Core configured for HTTP:
```
docker run --rm -it --name gmphan-webapp -p 8080:8080 --env-file variables.env ocbuuregistry.azurecr.io/gmphan-webapp:latest
```

### Run the container image with ASP.NET Core configured for HTTPS:
```
docker run --rm -it --name gmphan-webapp -p 443:443 --env-file variables.env ocbuuregistry.azurecr.io/gmphan-webapp:latest
```
