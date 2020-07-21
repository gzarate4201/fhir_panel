FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build-env
WORKDIR /app

# Copy csproj and restore as distinct layers
COPY *.csproj ./
RUN dotnet restore

# Copy everything else and build
COPY . ./
RUN dotnet publish -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1
WORKDIR /app
COPY --from=build-env /app/out .
ENV ALARM_REPORT=Fal
ENV CALIBRATION_INSTRUMENT="Indra-FK02GYW-"
ENV CALIBRATION_TYPE="None"
ENV CALIBRATION_METHOD="None"
ENV CALIBRATION_VALUE="37.3"
ENTRYPOINT ["dotnet", "aspnetapp.dll"]
