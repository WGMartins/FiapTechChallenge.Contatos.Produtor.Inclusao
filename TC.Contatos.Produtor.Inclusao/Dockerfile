FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /app

COPY ./ ./
WORKDIR /app/TC.Contatos.Produtor.Inclusao

RUN dotnet restore --runtime linux-arm64
RUN dotnet publish -c Release -r linux-arm64 -o out

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
COPY --from=build /app/TC.Contatos.Produtor.Inclusao/out .
EXPOSE 8080


ENTRYPOINT ["dotnet", "Api.dll"]
