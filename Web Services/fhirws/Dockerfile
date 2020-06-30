FROM mcr.microsoft.com/dotnet/core/aspnet:3.1
WORKDIR /fhir_ws
COPY fhir_ws/ /fhir_ws/
RUN ls -la /fhir_ws/*
ENTRYPOINT ["dotnet", "FhirWS.dll"]

