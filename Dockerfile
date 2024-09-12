FROM mcr.microsoft.com/dotnet/aspnet:8.0

# Create the application directory
RUN mkdir /App

# Copy the published application files to the container
COPY ./src/GmphanMvc/bin/Release/net8.0/publish /App

# Create the directory for HTTPS certificates
RUN mkdir /https
COPY ./https /https/

# Set the working directory
WORKDIR /App

# Start the application
ENTRYPOINT [ "dotnet", "/App/GmphanMvc.dll" ]