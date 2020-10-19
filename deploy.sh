#!/bin/bash
az webapp deployment source config-zip --resource-group $resourcegroupname --name $webappname --src ./deploy/deploy.zip
az webapp restart --name $webappname --resource-group $resourcegroupname

echo "Deploy complete."
