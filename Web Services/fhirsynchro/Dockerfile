FROM mcr.microsoft.com/dotnet/core/aspnet:3.0
WORKDIR /fhir_client
COPY fhir_client/ /fhir_client/
RUN ls -la /fhir_client/*
ENTRYPOINT ["dotnet", "FhirSyncro.dll"]

# docker build -t aspnetapp .
# docker inspect -f '{{range .NetworkSettings.Networks}}{{.IPAddress}}{{end}}' fhirsynchro_dockerfhir_1
# docker images ls
# docker containers ls
# docker exec -it fhirsynchro_dockerfhir_1 /bin/bash

