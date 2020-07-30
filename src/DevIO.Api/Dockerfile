FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build

WORKDIR /src

COPY ["src/DevIO.Api/DevIO.Api.csproj", "DevIO.Api/"]
COPY ["src/DevIO.Business/DevIO.Business.csproj", "DevIO.Business/"]
COPY ["src/DevIO.Data/DevIO.Data.csproj", "DevIO.Data/"]

COPY ["src/DevIO.Api/entrypoint.sh", "DevIO.Api/entrypoint.sh"]
RUN chmod +x ./DevIO.Api/entrypoint.sh
COPY . .

RUN dotnet restore DevIO.Api/DevIO.Api.csproj

WORKDIR "src/DevIO.Api"

RUN dotnet build DevIO.Api.csproj -c Release -o /app/build

FROM build AS publish
RUN dotnet publish DevIO.Api.csproj -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DevIO.Api.dll"]

CMD /bin/bash ./DevIO.Api/entrypoint.sh