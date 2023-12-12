FROM mcr.microsoft.com/dotnet/sdk:6.0 AS publish
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

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS final
RUN uname -a
RUN apt-get update && apt-get --assume-yes install libnss3-tools
WORKDIR /app
EXPOSE 80
COPY --from=publish /app .

ENTRYPOINT ["dotnet", "DotNetFlicks.Web.dll"]
