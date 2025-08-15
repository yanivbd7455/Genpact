## Overview
This repository contains automation tests for both UI and REST API, implemented using Playwright in C#.

## Technologies and Tools
- **Playwright (C#):** Used for both UI and REST API testing.
- **NUnit:** Framework used for organizing and running tests.
- **Page Object Model:** Design pattern used for maintaining test automation code.
- **NLog:** Used for logging purposes - After execution log file: genpac-log-execution.log
            can be found at: 'C:\Genpact\bin\Debug\net8.0'
- **Allure:** Can be managed with Docker-based reporting tool for generating test reports.
              For this particual test will be managed locally. In order to see Allure report
              localy, follow those steps:
  1. Install Allure CLI (you'll need java installation)
  2. Run the tests to generate Allure results
  3. Use the command: C:\Genpact\bin\Debug\net8.0> allure serve allure-results to view the report in your browser.
- **Git:** Version control system for managing the codebase.


### Prerequisites
- .NET SDK
- Playwright Installation
- Allure reporting
- Git
- NLog
