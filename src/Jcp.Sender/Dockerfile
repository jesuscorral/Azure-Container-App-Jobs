FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 5057


FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:7.0 AS build
ARG configuration=Release
WORKDIR /src
COPY ["src/Jcp.Sender/Jcp.Sender.csproj", "src/Jcp.Sender/"]
RUN dotnet restore "src/Jcp.Sender/Jcp.Sender.csproj"
COPY . .
WORKDIR "/src/src/Jcp.Sender"
RUN dotnet build "Jcp.Sender.csproj" -c $configuration -o /app/build

FROM build AS publish
ARG configuration=Release
RUN dotnet publish "Jcp.Sender.csproj" -c $configuration -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Jcp.Sender.dll"]
