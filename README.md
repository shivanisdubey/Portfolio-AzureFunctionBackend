# Portfolio Azure Function Backend

A .NET 8 Azure Function API that securely processes contact form submissions and sends emails using SendGrid.

This backend powers the Angular portfolio frontend.

---

## Overview

This project implements a serverless contact form API using Azure Functions (Isolated Worker Model).

It receives contact form submissions from the frontend and sends emails via SendGrid.

---

## Tech Stack

- .NET 8 (Isolated Worker)
- Azure Functions
- SendGrid Email API

---

## API Endpoint

POST  
`/api/ContactEmailFunction`

Accepts JSON payload:

```json
{
  "Name": "John Doe",
  "Email": "john@example.com",
  "Message": "Hello"
}
```

---

## Local Setup

### 1. Clone Repository

```bash
git clone https://github.com/shivanisdubey/Portfolio-AzureFunctionBackend.git
cd Portfolio-AzureFunctionBackend
```

### 2. Configure Local Settings

Create a file named:

`local.settings.json`

Add:

```json
{
  "IsEncrypted": false,
  "Values": {
    "AzureWebJobsStorage": "UseDevelopmentStorage=true",
    "FUNCTIONS_WORKER_RUNTIME": "dotnet-isolated",
    "SendGridApiKey": "YOUR_SENDGRID_API_KEY",
    "FromEmail": "your_verified_sendgrid_email"
  }
}
```


### 3. Run Locally

```bash
func start
```

Or via Visual Studio.

---

## Deployment

Deployed as an Azure Function App.

---

## CORS Configuration

CORS is configured in Azure Function App settings to allow requests from the Angular frontend.

---

## Security

- SendGrid API key stored in Azure Function App configuration
- Environment-based configuration

---

## Frontend Repository

 https://github.com/shivanisdubey/Portfolio-AngularUI

---

## Author

Shivani Dubey  
Full Stack .NET Developer
