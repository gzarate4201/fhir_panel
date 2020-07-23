FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /app

# Copy csproj and restore as distinct layers
COPY *.csproj ./
RUN dotnet restore

# Copy everything else and build
COPY . ./
RUN dotnet build -c Release -o /app/build

FROM build AS publish
RUN dotnet publish -C Release -o /app/publish

# Build runtime image
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1
WORKDIR /app
COPY --from=build /app/publish .
ENV ALARM_REPORT=Fal
ENV CALIBRATION_INSTRUMENT="Indra-FK02GYW-"
ENV CALIBRATION_TYPE="None"
ENV CALIBRATION_METHOD="None"
ENV CALIBRATION_VALUE="37.3"
ENTRYPOINT ["dotnet", "fhirpanel.dll"]
