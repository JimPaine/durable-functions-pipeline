#!/bin/bash

container=$1
functionname=$2
subscription_id=$3

az account set --subscription $subscription_id

terraform init
terraform apply -auto-approve \
    -var "subscription_id=$subscription_id" \
    -var "container=$container" \
    -var "functionname=$functionname"

connectionstring=$(terraform output -json primary_connection_string | jq '.value')
# strip out quotes left from json parse got to be a better way to do this
connectionstring="${connectionstring%\"}"
connectionstring="${connectionstring#\"}"

fileendpoint=$(terraform output -json file_endpoint | jq '.value')
# strip out quotes left from json parse got to be a better way to do this
fileendpoint="${fileendpoint%\"}"
fileendpoint="${fileendpoint#\"}"

functionname=$(terraform output -json function_name | jq '.value')
# strip out quotes left from json parse got to be a better way to do this
functionname="${functionname%\"}"
functionname="${functionname#\"}"

cd src/Pipeline
dotnet clean
dotnet publish -c "Release"

az storage file upload-batch \
    --source ./bin/Release/netstandard2.0/publish/ \
    --destination "$fileendpoint$functionname-content" \
    --destination-path "site/wwwroot" \
    --connection-string $connectionstring