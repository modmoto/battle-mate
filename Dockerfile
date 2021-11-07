FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["battle-mate/battle-mate.csproj", "battle-mate/"]
RUN dotnet restore "battle-mate/battle-mate.csproj"
COPY . .
WORKDIR "/src/battle-mate"
RUN dotnet build "battle-mate.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "battle-mate.csproj" -c Release -o /app/publish

FROM nginx:alpine AS final
WORKDIR /usr/share/nginx/html
COPY --from=publish /app/publish/ .
COPY default.conf /etc/nginx/conf.d/
