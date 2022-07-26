#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["MeetupProject.API/MeetupProject.API.csproj", "MeetupProject.API/"]
COPY ["MeetupProject.DAL/MeetupProject.DAL.csproj", "MeetupProject.DAL/"]
COPY ["MeetupProject.Common/MeetupProject.Common.csproj", "MeetupProject.Common/"]
COPY ["MeetupProject.BLL/MeetupProject.BLL.csproj", "MeetupProject.BLL/"]
RUN dotnet restore "MeetupProject.API/MeetupProject.API.csproj"
COPY . .
WORKDIR "/src/MeetupProject.API"
RUN dotnet build "MeetupProject.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MeetupProject.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MeetupProject.API.dll"]