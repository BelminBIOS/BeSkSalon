
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src


COPY ["BeSkSalon.csproj", "./"]
RUN dotnet restore "BeSkSalon.csproj"


COPY . .
RUN dotnet build "BeSkSalon.csproj" -c Release -o /app/build


FROM build AS publish
RUN dotnet publish "BeSkSalon.csproj" -c Release -o /app/publish /p:UseAppHost=false


FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
EXPOSE 80
EXPOSE 443

COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BeSkSalon.dll"]
