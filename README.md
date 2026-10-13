## ⛔Never push sensitive information such as client id's, secrets or keys into repositories including in the README file⛔

# SFA.DAS.RequestApprenticeTraining.Api

<img src="https://avatars.githubusercontent.com/u/9841374?s=200&v=4" align="right" alt="UK Government logo">


[![Build Status](https://dev.azure.com/sfa-gov-uk/Digital%20Apprenticeship%20Service/_apis/build/status/_projectname_?branchName=master)](https://dev.azure.com/sfa-gov-uk/Digital%20Apprenticeship%20Service/_build/latest?definitionId=_projectid_&branchName=master)
[![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=_projectId_&metric=alert_status)](https://sonarcloud.io/dashboard?id=_projectId_)
[![Jira Project](https://img.shields.io/badge/Jira-Project-blue)](https://skillsfundingagency.atlassian.net/secure/RapidBoard.jspa?rapidView=564&projectKey=_projectKey_)
[![Confluence Project](https://img.shields.io/badge/Confluence-Project-blue)](https://skillsfundingagency.atlassian.net/wiki/spaces/_pageurl_)
[![License](https://img.shields.io/badge/license-MIT-lightgrey.svg?longCache=true&style=flat-square)](https://en.wikipedia.org/wiki/MIT_License)

```
The Request Apprentice Training API is an API used to store requests for apprentice training by Employers 
which are then actioned by Providers

1. Employers must be authenticated to create requests.
2. Providers can search for requests which are relevant to the services they provide.
```

## How It Works

```
The Request Apprentice Training service consists of a database, Inner API, Outer API's, Employer Portal, 
Provider Portal and Jobs, this repo is the Inner API.
```

## 🚀 Installation

### Pre-Requisites

```
* A clone of this repository
* A code editor that supports Azure functions and .NET 10.0 e.g. Visual Studio 2026
* A SQL server which is either an Azure DB or is Azure DB compatible e.g. SQL Server 2025 Express running locally
* An Azure Service Bus instance (Only required for the API when sending emails)
* Azure Table Storage for config (Azurite and Azure Storage Explorer can be used locally)
```

```
Publish the database to Azure or a local instance by selecting SFA.DAS.RequestApprenticeTraining.Database and 
selecting Publish, no additional properties need to be specified.
```

### Config

<pre>
This service uses the standard Apprenticeship Service configuration.
All configuration can be found in the <a href="https://github.com/SkillsFundingAgency/das-employer-config">das-employer-config repository</a>.
</pre>
```
Add an entry to Azure Table Storage config

1. Start Azurite and open it in Azure Storage Explorer
2. Create a table called Configuration (if it does not already exist)
3. Add a new entry with the following properties

* PartitionKey : LOCAL
* RowKey : SFA.DAS.RequestApprenticeTraining.Api_1.0
* Data : the JSON for this service from the das-employer-config repository: https://github.com/SkillsFundingAgency/das-employer-config

In the data JSON, update the database connection string to point to your local database 
	e.g. Data Source=SERVERNAME\\SQLEXPRESS;Initial Catalog=SFA.DAS.RequestApprenticeTraining.Database
```

## 🔗 External Dependencies

```
* None
```

## Technologies

```
* .NET 10.0
* Azure Table Storage 
* NUnit
* Moq
* FluentAssertions
```

## 🐛 Known Issues

```
* None
```