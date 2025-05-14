FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443


FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["TC.Contatos.Produtor.Inclusao.Api/Api.csproj", "./"]
RUN dotnet restore "./Api.csproj"
WORKDIR "/src/."
RUN dotnet build "Api.csproj" -c Release -o /app/build



FROM build AS publish
RUN dotnet publish -c Release -o /app/publish


FROM base AS final
WORKDIR /app
COPY --from=build-env /app/publish .
ENTRYPOINT ["dotnet", "Api.dll"]