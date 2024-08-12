FROM mcr.microsoft.com/dotnet/aspnet:8.0-nanoserver-1809 AS base
WORKDIR /app
EXPOSE 5143

ENV ASPNETCORE_URLS=http://+:5143

FROM mcr.microsoft.com/dotnet/sdk:8.0-nanoserver-1809 AS build
ARG configuration=Release
WORKDIR /src
COPY ["ShopAPI/ShopAPI.csproj", "ShopAPI/"]
RUN dotnet restore "ShopAPI\ShopAPI.csproj"
COPY . .
WORKDIR "/src/ShopAPI"
RUN dotnet build "ShopAPI.csproj" -c $configuration -o /app/build

FROM build AS publish
ARG configuration=Release
RUN dotnet publish "ShopAPI.csproj" -c $configuration -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ShopAPI.dll"]
