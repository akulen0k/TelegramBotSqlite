FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["TelegramBot-Timetable.csproj", "TelegramBot-Timetable/"]
RUN dotnet restore "TelegramBot-Timetable/TelegramBot-Timetable.csproj"
COPY . .
#WORKDIR "/src/TelegramBot-Timetable"
RUN dotnet build "TelegramBot-Timetable.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TelegramBot-Timetable.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
COPY token.txt .
ENTRYPOINT ["dotnet", "TelegramBot-Timetable.dll"]
