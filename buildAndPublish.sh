#!/bin/sh
dotnet build -c Release src/GmphanMvc
dotnet publish -c Release src/GmphanMvc

#About Azure Docker - I use "Azure App Service for Containers" 
#Therefore now need to have the whole stack configuration. 

# Build for linux/amd64 and push directly to GitHub Container Registry
docker buildx build --platform linux/amd64 -t ghcr.io/gmphan/gmphan-webapp:latest --push .

#build for multi-platform image and push directly to Azure Container Registry
#docker buildx build --platform linux/amd64,linux/arm64 -t ocbuuregistry.azurecr.io/gmphan-webapp:latest --push .


