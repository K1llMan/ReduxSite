FROM microsoft/aspnetcore:2.0 AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/aspnetcore-build:2.0 AS build
WORKDIR /src
COPY Redux/Redux.csproj Redux/
COPY Common/Common.csproj Common/
RUN dotnet restore Redux/Redux.csproj
COPY . .
WORKDIR /src/Redux
RUN dotnet build Redux.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish Redux.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Redux.dll"]
