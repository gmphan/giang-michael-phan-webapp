### Generate cert and configure local machine:
```
dotnet dev-certs https -ep ./https/{mycustomcert}.pfx -p {mycustompw}
dotnet dev-certs https --trust
```
### Build and publish the app
dotnet build -c Release src/GmphanMvc
dotnet publish -c Release src/GmphanMvc

### Build, tag and push the Docker image for multi-platform. (without --load, the image won't be build locally)
```
docker buildx build --platform linux/amd64,linux/arm64 -t ocbuuregistry.azurecr.io/gmphan-webapp:latest --load .
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
