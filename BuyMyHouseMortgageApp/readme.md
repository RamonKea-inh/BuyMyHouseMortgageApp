# BuyMyHouseMortgageApp

## Overview
**BuyMyHouseMortgageApp** is an Azure Functions application designed to streamline the mortgage application process. It uses a email message queue that triggers a function that calls email service SendGrid for notifications and uses a Table Storage database for managing mortgage application and house data. 
A blob storage is used to store the images of the houses.

## Prerequisites
To run this application locally or deploy it to Azure, ensure you have the following:
- **.NET 8 SDK**  
- **Azure Storage Emulator** (for local development) or **Azure Storage Account**  
- **LocalDB** or a **SQL Server instance**  
- **SendGrid account** (for email notifications)  

## Setup Instructions

### 1. Restore NuGet Packages
Run the following command in the root of the project:
```dotnet restore```

### 2. Update Configuration
Update the local.settings.json file to provide necessary configuration values:
```{
  "IsEncrypted": false,
  "Values": {
    "AzureWebJobsStorage": "UseDevelopmentStorage=true",
    "AzureStorage:ConnectionString": "UseDevelopmentStorage=true",
    "DatabaseConnectionString": "Server=(localdb)\\LocalDBInstance;Database=BuyMyHouseDB;Trusted_Connection=True;MultipleActiveResultSets=true",
    "FUNCTIONS_WORKER_RUNTIME": "dotnet-isolated",
    "EmailServiceApiKey": "YourEmailServiceApiKeyHere",
    "EmailServiceSender": "YourEmailServiceSenderHere"
  }
}
```

- Replace YourEmailServiceApiKeyHere with your SendGrid API Key.
- Replace YourEmailServiceSenderHere with the sender email address registered with SendGrid.

### 3. Run the Application
Run the following command in the root of the project:
```func start```

