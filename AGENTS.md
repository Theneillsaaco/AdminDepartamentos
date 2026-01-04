# AGENT INSTRUCTIONS: AdminDepartamentos Repository

These instructions are tailored for AI coding agents operating within this repository. Adherence to these guidelines is mandatory for all code contributions.

## 1. Development Environment

| Aspect | Value |
| :--- | :--- |
| **Technology Stack** | C#, F#, .NET |
| **Target Framework** | net10.0 |
| **Testing Framework** | xUnit |
| **Primary Tool** | \`dotnet\` CLI |

## 2. Build, Test, and Run Commands

The preferred command runner is the standard \`dotnet\` CLI.

### General Commands

| Task | Command | Description |
| :--- | :--- | :--- |
| **Full Build** | \`dotnet build\` | Compiles all projects in the solution. |
| **Run API** | \`dotnet run --project AdminDepartamentos.API\` | Starts the main API application. |
| **Run All Tests** | \`dotnet test\` | Executes all tests in the \`AdminDepartamentos.Unit.Test\` project. |
| **Entity Framework** | \`dotnet ef [command]\` | Used for database migrations and other EF Core operations. |

### Running a Single Test

Use the xUnit filtering mechanism via the \`--filter\` argument.

**Format:**
\`\`\`bash
dotnet test --filter "FullyQualifiedName~<Namespace>.<TestClassName>.<TestName>"
\`\`\`

**Example:**
To run a specific test method named \`CanCreatePago\` in the \`PagoTests\` class:
\`\`\`bash
dotnet test --filter "FullyQualifiedName~AdminDepartamentos.Unit.Test.PagoTests.CanCreatePago"
\`\`\`

## 3. Code Style and Conventions

All new and modified code must strictly conform to existing project conventions.

### A. C# & General .NET Conventions

1.  **Nullability:** Strict nullability is enforced (\`<Nullable>enable</Nullable>\`). All C# code must handle potential null references explicitly using \`?\` (nullable reference types), null checks, or the bang operator (\`!\`) only when absolute certainty can be proven. **Avoid the bang operator unless strictly necessary.**
2.  **Implicit Usings:** Implicit global usings are enabled (\`<ImplicitUsings>enable</ImplicitUsings>\`). Do not include boilerplate \`using\` statements for common namespaces (e.g., \`System.Collections.Generic\`, \`Microsoft.AspNetCore.Mvc\`) if they are already implicitly included. Only add explicit \`using\` statements for non-implicit namespaces.
3.  **Naming:** Use standard .NET **PascalCase** for public classes, methods, properties, and constants. Use **camelCase** for local variables and method parameters.
4.  **Formatting:** Adhere to the default Visual Studio/Rider C# formatting style. Ensure a consistent use of braces and indentation.

### B. F# Domain Conventions (AdminDepartamentos.Domain.FSharp)

The F# domain layer is critical and follows functional programming principles with an emphasis on robust error handling.

1.  **Error Handling (ROP):** The **Railway Oriented Programming (ROP) / Result Monad** pattern must be used for all fallible domain operations. Functions that may fail must return a type of \`Result<'TSuccess, 'TFailure>\` (or equivalent custom types like \`DomainError\`). **Avoid throwing exceptions** for expected failures; use the \`Result\` type instead.
2.  **Immutability:** Data structures should primarily be defined as immutable records (\`type MyRecord = { ... }\`). State mutation should be localized and minimized.
3.  **Documentation:** XML documentation is required for all public F# members (\`<GenerateDocumentationFile>true</GenerateDocumentationFile>\`).
4.  **F# Idioms:** Prefer F# idioms over C#-style implementations:
    *   Use pattern matching for control flow over \`if/else\` where appropriate.
    *   Prefer pipe-forward operator (\`|>\`) for data flow transformations.
    *   Use modules to organize functions.

### C. Imports and Ordering

1.  **Ordering:** Imports/usings should be grouped and ordered as follows:
    *   System namespaces.
    *   Third-party namespaces.
    *   Project-specific namespaces (current project, followed by other internal projects).
2.  **Alias:** Avoid aliasing imports unless necessary to resolve naming conflicts.

## 4. Agent-Specific Instructions

No specific external agent instruction files (e.g., \`.cursor/rules/\`, \`.github/copilot-instructions.md\`) were found. The primary guidance is to strictly follow the above project conventions.
