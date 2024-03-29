﻿FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
ARG APP_VERSION
WORKDIR /src
COPY ["battle-mate/battle-mate.csproj", "battle-mate/"]
RUN dotnet restore "battle-mate/battle-mate.csproj"
COPY . .
WORKDIR "/src/battle-mate"
RUN rm ./wwwroot/buildVersion.js
RUN echo "buildVersion = () => ${APP_VERSION}" >> ./wwwroot/buildVersion.js
RUN dotnet build "battle-mate.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "battle-mate.csproj" -c Release -o /app/publish

FROM nginx:alpine AS final
WORKDIR /usr/share/nginx/html
COPY --from=publish /app/publish/wwwroot .
COPY nginx.conf /etc/nginx/nginx.conf
