Trial Metadata Processor
========================

A .NET 8 RESTful API for uploading and processing JSON metadata files related to clinical trials.

Features
--------

*   JSON metadata validation
    
*   File upload handling
    
*   Data transformation and storage
    
*   SQL database integration
    
    

Tech Stack
----------

*   .NET 8
    
*   Entity Framework Core
    
*   SQL Server
    
*   Docker
    
*   MediatR
    
*   FluentValidation
    

Getting Started
---------------

#### Prerequisites

*   Docker Desktop
    
*   .NET 8 SDK
    

#### Installation

1.  Clone repository
    
2.  Run docker-compose up --build
    
3.  API available at [http://localhost:8080](http://localhost:8080)
    

#### Usage

Upload JSON file, validate and save:
`   httpCopyPOST /api/clinicaltrials/{upload}   `

Get trial by ID:
`   httpGET /api/clinicaltrials/{id}   `

Get trials by status:
`   httpGET /api/clinicaltrials/{status}   `

Get filtered trials:
`   httpPOST /api/clinicaltrials/{filter}   `

#### Valid JSON Schema
```json
{
    "$schema": "http://json-schema.org/draft-07/schema#",
    "title": "ClinicalTrialMetadata",
    "type": "object",
    "properties": {
        "trialId": "CT236677",
        "title": "Test Trial",
        "startDate": "2024-01-01",
        "participants": 3,
        "status": "Ongoing"
    }
}
 
