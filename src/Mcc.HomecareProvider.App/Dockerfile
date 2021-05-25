#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["test-task-for-qa/test-task-for-qa.csproj", "test-task-for-qa/"]
RUN dotnet restore "test-task-for-qa/test-task-for-qa.csproj"
COPY . .
WORKDIR "/src/test-task-for-qa"
RUN dotnet build "test-task-for-qa.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "test-task-for-qa.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "test-task-for-qa.dll"]