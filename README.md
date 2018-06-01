# durable-functions-pipeline

## Background

An example of using durable functions to build an ETL pipeline to take advantage of the simplicity of state management between function calls,
as well as gaining High availability out of the box without the need to code for it. With the additional benefit of being able to take advantage
of the durable functions being paused between calls when using async await to durable activity, hopefully means a cheaper, simpler, more available
ETL process.

## Example flow

![Image of Durable Function ETL Pipeline](https://github.com/JimPaine/images/blob/master/Pipeline.PNG?raw=true)

## Pre-Reqs

| Item      | Reason                              | Source                                               |
|-----------|:-----------------------------------:|-----------------------------------------------------:|
| Terraform | Used to build out environment       | [Downloads](https://www.terraform.io/downloads.html) |
| Azure-Cli | Used to deploy files to environment | sudo apt-get install azure-cli                       |
| npm       | Used to pull down swagger ui        | sudo apt-get install npm                             |
| jq        | Parsing json output of terraform    | sudo apt-get install jq                              |
| git       | For cloning my code :)              | sudo apt-get install git                             |

## Things I need to sort

Login - currently to authenticate run az login, plan to extend script to use service principal

jq output includes quotes within string values. I have included a work around, but being a C# developer and not a bash pro, I am sure there is a better way.

## Step 1

Clone repo
```
$ git clone https://github.com/JimPaine/durable-functions-pipeline.git
```

## Step 2

Login to the Azure cli

```
$ az login
```

## Step 3

Run deploy script

```
$ cd funkyswagger
$ ./deploy.sh {containername} {functionname} {subscriptionid}
```

{containername} is the blob container that is used to store the output from the Egress Function

{functionname} what it says on the tin, will have a suffix added to ensure it is unique

{subscriptionid} so both the Azure cli and terraform can run agaist the right subscription

## Step 4

Either wait for the timed function to be triggered or call the HTTP function to get going.