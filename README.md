# Shopping Automation Project - SauceDemo

This project is an end-to-end automation solution for the SauceDemo store. It includes a robust Backend API using Playwright for web scraping/automation, and a professional Unit Testing suite.

## Project Structure
- **ShoppingAutomation.Api**: Core logic, Playwright automation, and REST API.
- **ShoppingAutomation.Tests**: Unit tests for price normalization and product selection logic.
- **UI**: Integrated frontend for triggering automation tasks.

## Key Features
- **Smart Product Selection**: Automatically finds the cheapest item based on price normalization.
- **Robust Automation**: Uses Playwright with explicit waits and error handling.
- **Unit Tested**: 100% pass rate on logic-heavy components (8/8 tests passed).
- **Dynamic Port Allocation**: Configured to use port 5199 with a fallback to a dynamic port if busy.

## How to Run

### Prerequisites
- .NET 8.0 or higher
- Playwright browsers (will be installed automatically or via CLI)

### Installation
1. Clone the repository.
2. From the root directory, install Playwright dependencies:
   ```bash
   playwright install --with-deps
Running the Application
From the root directory, run:

Bash
dotnet run --project ShoppingAutomation.Api/ShoppingAutomation.Api.csproj
The API will be available at http://localhost:5199 (or a dynamic port if 5199 is occupied).

Running Tests
To verify the business logic, execute:

Bash
dotnet test
Troubleshooting
If you see an "Address already in use" error, the system will automatically attempt to bind to a different port. Check the terminal output for the active "Now listening on" URL.