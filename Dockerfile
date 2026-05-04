FROM mcr.microsoft.com/dotnet/sdk:6.0@sha256:c8fdd06e430de9f4ddd066b475ea350d771f341b77dd5ff4c2fafa748e3f2ef2 AS publish
ARG TARGETARCH
WORKDIR /src
COPY ./DotNetFlicks.Accessors ./DotNetFlicks.Accessors
COPY ./DotNetFlicks.Common ./DotNetFlicks.Common 
COPY ./DotNetFlicks.Engines ./DotNetFlicks.Engines
COPY ./DotNetFlicks.Managers ./DotNetFlicks.Managers
COPY ./DotNetFlicks.ViewModels ./DotNetFlicks.ViewModels
COPY ./DotNetFlicks.Web ./DotNetFlicks.Web 
COPY ./DotNetFlicks.sln ./DotNetFlicks.sln
RUN dotnet publish "DotNetFlicks.Web/Web.csproj" /p:Platform=$TARGETARCH -c Release -o /app

FROM mcr.microsoft.com/dotnet/aspnet:6.0@sha256:e70c493f8af7f95bf459cb2b15c7e7a6173228929c2b7a9a6836b19377890e78 AS final
RUN uname -a
RUN apt-get update && apt-get --assume-yes install libnss3-tools
WORKDIR /app
EXPOSE 80
COPY --from=publish /app .

ENTRYPOINT ["dotnet", "DotNetFlicks.Web.dll"]
