# QA Automation Interview Assignment

## Tasks Completed

### Task 1 - Code Review

Reviewed Playwright automation framework and documented code quality, maintainability, and reliability issues.

### Task 2 - Debugging Challenge

Performed root cause analysis using:

* CI logs
* Network logs
* UI evidence

Identified flaky behavior caused by timeout configuration and slow pricing service responses.

### Task 3 - Full Stack Integration Testing

Implemented:

* API Client
* Database Helper
* Checkout Page Object
* Promotion Flow Tests

Validated API → UI → Database workflow.

## Technology Stack

* C#
* .NET 8
* Playwright
* NUnit
* PostgreSQL
* Docker

## Execution

```bash
docker compose up -d
dotnet test
```

All tests pass successfully.
