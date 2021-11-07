FROM mcr.microsoft.com/dotnet/sdk:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["battle-mate/battle-mate.csproj", "battle-mate/"]
RUN dotnet restore "battle-mate/battle-mate.csproj"
COPY . .
WORKDIR "/src/battle-mate"
RUN dotnet build "battle-mate.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "battle-mate.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "battle-mate.dll"]
