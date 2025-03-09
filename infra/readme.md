The .azcli file will create:
- Az Container Registry (ACR)
- SPN to push to the ACR
- Az App Service Plan
- Az Web App that runs under a System Managed Identity

After running this, you'll need to create/edit github secrets from the output.
(This is copy-paste stuff, it will eventually get automated)

Secrets to update from the .azcli:
AZURE_CREDENTIALS
REGISTRY_LOGIN_SERVER
REGISTRY_USERNAME
REGISTRY_PASSWORD
RESOURCE_GROUP

Secret to update from the WebApp publish profile :
(in the azure portal)
AZURE_WEBAPP_PUBLISH_PROFILE