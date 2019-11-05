# Azure Cosmos DB Optimisation

az group create --name az203-batch --location southeastasia

az storage account create --name slaz203batchsa --location southeastasia --resource-group az203-batch --sku Standard_LRS --kind StorageV2

az batch account create --location southeastasia --name slaz203batch --storage-account slaz203batchsa --resource-group az203-batch

az batch account autostorage-keys sync --name slaz203batch --resource-group az203-batch

az batch application package create --application-name ffmpeg --name slaz203batch --package-file https://ffmpeg.zeranoe.com/builds/win64/static/ffmpeg-3.4-win64-static.zip --resource-group az203-batch --version-name 3.4
