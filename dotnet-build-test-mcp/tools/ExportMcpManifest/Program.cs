using McpToolManifest;
using DotNetBuildTestMcp;

var tools = ToolCatalog.Build().Select(t => (t.Name!, (string?)t.Description)).ToList();
return McpToolManifestExporter.Run(
    args,
    tools,
    new McpToolManifestExportOptions
    {
        McpId = "dotnet-build-test-mcp",
        Title = "Dotnet Build/Test MCP",
        RepoFolderName = "dotnet-build-test-mcp",
    });
