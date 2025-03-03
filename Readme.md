# ASP.NET Core Web API Application For Booking Services

## About Highlevel Project structure

## InventoryManagement
This is the Main API project which Contains All endpoints.

    ExceptionMiddleware
    Exception Middleware is used to handle All Exception in the project.

    Automaper
    AutoMapper is an object-object mapper. 
    Object-object mapping works by transforming an input object of one type into an output object of a different type.

    Project Files

    * Program.cs - In an ASP.NET Core application, the Program.cs file is where the application is configured and run.
    * Controllers\{ControllerName} - API controller's

## InventoryManagement.Service
This is the service layer which will be called from InventoryManagementAPI and it has business logic and data validations.

## InventoryManagement.DataTransferModel
A Data Transfer Model (DTM), also known as a DTO is used to transfer data from Service layer to API layer.

## InventoryManagement.Repository

    Database
    ASP.NET Core provides a built-in InMemoryDatabase. This Application uses InMemoryDatabase.

    Repositories
    Here Generic Repository is used.
    A Generic Repository is a design pattern that provides a way to abstract data access logic in an application. 
    It allows you to write data access code in a generic and reusable way, meaning the repository can work with any entity type 
    without needing to write repetitive code for each entity.

    UnitOfWork
    The Unit of Work (UoW) is a design pattern used to manage business transactions in an application.
    For creating a booking, where multiple tables need to be updated (e.g., Booking, Member, Inventory), 
    the Unit of Work ensures that these changes are committed or rolled back as a single atomic operation. 
    However, when using an in-memory database real database transactions (commit/rollback) are not supported. 
    UoW is used for demonstration purposes, the task will simply complete without transaction management. 
    This shows how the Unit of Work pattern coordinates operations.

## InventoryManagement.Repository.Models
This contains the Database entities.It is used to transfer data from Repository layer to Service layer.

## Tests
This contains Xunit tests for each project. For mocking objects or api calls MOQ is used.

## Here are some steps to follow from on Visual Studio to Run this application
    Open InventoryManagement.sln in Visual Studio 2022.
    Build the solution. The first time you build it, it will take some time to install the required NuGet packages.
    Confirm that InventoryManagement API is set as the startup project.
    Run the project. This will automatically open Swagger in your default web browser.
    You can now test the APIs directly from Swagger.

