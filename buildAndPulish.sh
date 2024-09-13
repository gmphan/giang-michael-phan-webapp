#!/bin/sh
dotnet build -c Release src/GmphanMvc
dotnet publish -c Release src/GmphanMvc

#About Azure Docker - I use "Azure App Service for Containers" 
#Therefore now need to have the whole stack configuration. 

#build for multi-platform image.
#without --load, the image won't be build locally
docker buildx build --platform linux/amd64,linux/arm64 -t ocbuuregistry.azurecr.io/gmphan-webapp:latest --load .
docker push ocbuuregistry.azurecr.io/gmphan-webapp:latest

