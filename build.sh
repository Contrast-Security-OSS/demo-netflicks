#!/bin/bash

rm ./deploy/deploy.zip

dotnet publish "DotNetFlicks.Web/Web.csproj" -c Release -o ./deploy

cd ./deploy

zip -r deploy.zip .