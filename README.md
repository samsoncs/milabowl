# 🏆 Fantasy Milabowl

The most prestigious Fantasy Premier League league in the world! (allegedly)

This repository consists of a .NET console application for FPL data processing, and an Astro-powered frontend for displaying league standings, rules, and statistics.

## 🚀 Quick Start

### Prerequisites

- [The latest version of the .NET SDK](https://dotnet.microsoft.com/download)
- [The latest version of node](https://nodejs.org/)
- [npm](https://www.npmjs.com/)
- [(required for Milalytics): Docker Desktop](https://www.docker.com/products/docker-desktop/)

### Running the Backend (Data Processing)

The backend project can be run in your IDE of choice, or via command line:

```powershell
# Navigate to backend directory
cd milabowl-backend

# Run the console application
dotnet run --project ./Milabowl.Processing/Milabowl.Processing.csproj -- ../milabowl-astro/src/game_state
```

The backend will:

1. Fetch data from FPL APIs (or use snapshots in development)
2. Calculate milapoints for all managers
3. Output JSON state files to the frontend's `game_state` directory

#### Snapshot Mode for Development

The backend includes a snapshot system for development that eliminates the need for live FPL API calls:

- **Read Mode (Development)**: Uses pre-recorded FPL API responses from the `snapshot` folder, instead of HTTP calls
- **Write Mode**: Records live FPL API calls to files for future use
- **None Mode**: Makes live calls to the FPL API (production mode)

This allows for faster development cycles, enables development during down periods for FPL API and consistent test data without hitting FPL rate limits.

### Running Milalytics (Data Analysis)

The Milalytics project loads processed data into SQL Server for deeper analysis:

```powershell
# Navigate to backend directory
cd milabowl-backend

# Run SQL Server as a docker conatiner
docker run -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=!5omeSup3rF4ncyPwd!" -p 1431:1433
-d mcr.microsoft.com/mssql/server:2022-latest

# Run the analytics loader
dotnet run --project ./Milabowl.Milalytics/Milabowl.Milalytics.csproj
```

This will load relevant data into a SQL Server database, enabling:

- Historical trend analysis
- Advanced querying capabilities
- Custom reporting and visualizations

### Running the Frontend (Website)

The frontend project can be run in your IDE of choice, or via command line:

```powershell
# Navigate to frontend directory
cd milabowl-astro

# Install dependencies
npm install

# Start development server
npm run dev
```

Visit `http://localhost:4321` to view the website.

## 🏗️ Project Structure

```
milabowl/
├── milabowl-backend/
│   ├── Milabowl.Processing/           # Main processing application
│   ├── Milabowl.Processing.Tests/     # Unit tests
│   └── Milabowl.Milalytics/          # Database analytics
└── milabowl-astro/       # Astro frontend
```

## 🎯 Implementing New Rules

The rule system is built on the **Open/Closed Principle** - adding new rules requires zero changes to existing code outside of adding a single class in the backend. The rule will automagically show up in the frontend after deployment!

### Step 1: Create Your Rule Class

Create a new file in `Milabowl.Processing/Processing/Rules/`:

```csharp
using Milabowl.Processing.DataImport.Models;

namespace Milabowl.Processing.Processing.Rules;

public class YourAwesomeRule : MilaRule
{
    protected override string ShortName => "YAR"; // 2-6 character abbreviation

    protected override string Description =>
        "Points for doing something awesome in FPL";

    protected override RulePoints CalculatePoints(ManagerGameWeekState userGameWeek)
    {
        // Access all manager data:
        // - userGameWeek.Lineup (all players with stats)
        // - userGameWeek.Opponents (other managers' data)
        // - userGameWeek.TransfersIn/TransfersOut
        // - userGameWeek.HeadToHead
        // - userGameWeek.History

        var points = 0m; // Your calculation logic here
        var reasoning = "Why points were awarded";

        return new RulePoints(points, reasoning);
    }
}
```

### Step 2: That's It!

The rule will be automatically:

- ✅ Discovered by dependency injection
- ✅ Executed by the rules processor
- ✅ Included in JSON output
- ✅ Displayed on the website

### Step 3: Add Tests

Create a test file in `Milabowl.Processing.Tests/Processing/Rules/`:

```csharp
public class YourAwesomeRuleTests : MilaRuleTest<YourAwesomeRule>
{
    [Fact]
    public void Should_award_points_when_awesome_thing_happens()
    {
        // Arrange
        var state = new ManagerGameWeekStateBuilder()
            .WithLineup(/* test data */)
            .Build();

        // Act
        var result = Rule.Calculate(state);

        // Assert
        result.Points.ShouldBe(expectedPoints);
        result.Reasoning.ShouldContain("expected reasoning");
    }
}
```

There is a test that requires all MilaRules to have tests. Create a matching test class implementing `MilaRuleTest<YourRule>` for build to succeed.

## 🧪 CI/CD

The project includes automated build and quality checks that ensure:

- ✅ **Clean Builds**: Project compiles without warnings or errors
- ✅ **Test Coverage**: All tests pass and new rules have corresponding tests
- ✅ **Code Formatting**: Code adheres to [CSharpier](https://csharpier.com/) formatting standards (backend) and [Prettier](https://prettier.io/) standards (frontend)
- ✅ **Code Quality**: Code passes [ESLint](https://eslint.org/) linting checks for TypeScript and Astro files (frontend)

Before submitting changes, ensure your code passes all checks (could be done automatically with git hooks etc.):

**Backend:**

```powershell
# Check formatting
dotnet csharpier --check .

# Format code automatically
dotnet csharpier .

# Build without warnings
dotnet build --no-restore --verbosity normal

# Run all tests
dotnet test --no-build
```

**Frontend:**

```powershell
# Navigate to frontend directory
cd milabowl-astro

# Check formatting and code quality
npm run lint

# Fix formatting and code quality issues automatically
npm run lint:fix
```

## 🤝 Contributing

1. Implement your rule following the pattern above
2. Add comprehensive tests

---

_The most sophisticated FPL scoring system ever created... probably!_ 🎯
