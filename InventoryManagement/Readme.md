# ASP.NET Core Web API Application For Booking Service

Web API contains two endpoints which wrap the “Autocomplete a postcode partial” and “Lookup a postcode” methods on the Postcodes.io API (https://postcodes.io).

This API Service has 2 EndPoints
1. <BaseURL>/Prod/api/postcode/autocomplete/{searchParams}
2. <BaseURL>/Prod/api/postcode/{PostCode}

## About Highlevel Project structure

This project shows ASP.NET Core Web API project as an AWS Lambda exposed through Amazon API Gateway. Lambda function that is used to translate requests from API Gateway into the ASP.NET Core framework and then the responses from ASP.NET Core back to API Gateway.

For more information about how the Amazon.Lambda.AspNetCoreServer package works and how to extend its behavior view its [README](https://github.com/aws/aws-lambda-dotnet/blob/master/Libraries/src/Amazon.Lambda.AspNetCoreServer/README.md) file in GitHub.

## InventoryManagement
This is the Main API project which Contains All endpoints.

    ExceptionMiddleware
    Exception Middleware is used to handle All Exception in the project.

    Automaper
    AutoMapper is an object-object mapper. 
    Object-object mapping works by transforming an input object of one type into an output object of a different type.

    Project Files
    * Controllers\{ControllerName} - API controller's

## InventoryManagement.Service
This is the service layer which will be called from InventoryManagementAPI and it has business logic and data validations.

## PostCodeAPI.Model
This project contains Simple Class as a Model files to Deserialize Responce in to object.

## PostCodeAPI.DataTransferModel
This project contains Models which are actuly passed to API layer as a responce.

## PostCodeAPI.Common
This project contains some common functions required in the application.
e.g It provide functions to get AppSetting Keys from Appsetting.Json

## PostCodeAPI.Service.UnitTests
This is Unit test project for Service Layer. NUnit is used with MOQ.

## Deployment Document 
To deploy application on AWS , Please follow steps in attached Document. (PostCode Project Deployment steps.pdf)

## Here are some steps to follow from Visual Studio:

To deploy your Serverless application, right click the project in Solution Explorer and select *Publish to AWS Lambda*.

To view your deployed application open the Stack View window by double-clicking the stack name shown beneath the AWS CloudFormation node in the AWS Explorer tree. The Stack View also displays the root URL to your published application.

## Here are some steps to follow to get started from the command line:

Once you have edited your template and code you can deploy your application using the [Amazon.Lambda.Tools Global Tool](https://github.com/aws/aws-extensions-for-dotnet-cli#aws-lambda-amazonlambdatools) from the command line.

Install Amazon.Lambda.Tools Global Tools if not already installed.
```
    dotnet tool install -g Amazon.Lambda.Tools
```

If already installed check if new version is available.
```
    dotnet tool update -g Amazon.Lambda.Tools
```

Execute unit tests
```
    cd "PostCodeAPI/test/PostCodeAPI.Tests"
    dotnet test
```

Deploy application
```
    cd "PostCodeAPI/src/PostCodeAPI"
    dotnet lambda deploy-serverless
```