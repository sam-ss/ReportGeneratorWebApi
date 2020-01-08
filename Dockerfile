# NuGet restore
FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /src
COPY . .

CMD ASPNETCORE_URLS=http://*:$PORT dotnet ReportGeneratorWebApi.dll 