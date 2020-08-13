# Stage 1
FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /build
COPY . .
RUN dotnet restore
RUN dotnet publish -c Release -o /app
# Stage 2
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS final
RUN apt-get update
RUN apt-get install -y apt-utils
RUN apt-get install -y libgdiplus
RUN apt-get install -y nano
WORKDIR /app
COPY --from=build /app .
ENV ALARM_REPORT=False
ENV CALIBRATION_INSTRUMENT="Indra-FK02GYW-"
ENV CALIBRATION_TYPE="None"
ENV CALIBRATION_METHOD="None"
ENV CALIBRATION_VALUE="37.3"
ENTRYPOINT ["dotnet", "AspStudio.dll"]
