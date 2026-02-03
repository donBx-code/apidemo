FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build

WORKDIR /app/

# Copy project file and restore dependencies
COPY *.csproj ./
RUN dotnet restore

# Copy the rest of the application code
COPY . ./

# Publish the application
RUN dotnet publish -c Release -o /app/out


# Build the runtime image
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app
COPY --from=build /app/out .

# Expose the port your application will run on
EXPOSE 5070

# Start the application
ENTRYPOINT ["dotnet", "apidemo.dll"]