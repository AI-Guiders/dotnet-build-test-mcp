# dotnet-build-test-mcp

MCP-сервер для структурированной сборки и тестов .NET: **build_structured**, **run_tests**, а также job-инструменты (**get_job_status**, **get_job_log**, **cancel_job**). Агент получает ошибки компиляции и упавшие тесты в JSON без парсинга лога.

- **[dotnet-build-test-mcp/](dotnet-build-test-mcp/)** — сам MCP (тулы, публикация, как подключить в Cursor).
- Парсеры вывода — NuGet **[AIGuiders.DotNetBuildTestParsers](https://www.nuget.org/packages/AIGuiders.DotNetBuildTestParsers)** ([исходники](https://github.com/AI-Guiders/dotnet-build-test-parsers)).
- Ядро jobs/runner — **[dotnet-build-test-core](https://github.com/AI-Guiders/dotnet-build-test-core)** (`AIGuiders.DotNetBuildTest.Core`).
- **[McpVerifyTests/](McpVerifyTests/)** — минимальное решение для проверки всех кейсов (сборка ок/ошибка, тесты ок/падение).

См. [README в dotnet-build-test-mcp](dotnet-build-test-mcp/README.md).
