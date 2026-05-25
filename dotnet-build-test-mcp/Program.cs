using System.Text.Json;
using DotNetBuildTest.Core;
using DotnetBuildTestMcp;
using ModelContextProtocol.Protocol;
using ModelContextProtocol.Server;
using Tool = ModelContextProtocol.Protocol.Tool;

var jobService = new BuildTestJobService();
var toolsList = ToolCatalog.Build();

var options = new McpServerOptions
{
    ServerInfo = new Implementation { Name = "DotnetBuildTestMcp", Version = "0.4.0" },
    ProtocolVersion = "2024-11-05",
    Capabilities = new ServerCapabilities { Tools = new ToolsCapability { ListChanged = false } },
    Handlers = new McpServerHandlers
    {
        ListToolsHandler = (_, _) => ValueTask.FromResult(new ListToolsResult { Tools = toolsList }),

        CallToolHandler = async (request, cancellationToken) =>
        {
            var name = request.Params?.Name ?? "";
            var args = request.Params?.Arguments is IReadOnlyDictionary<string, JsonElement> a
                ? a
                : new Dictionary<string, JsonElement>();

            try
            {
                var text = name switch
                {
                    "build_structured" => await jobService.BuildStructuredAsync(args, cancellationToken).ConfigureAwait(false),
                    "run_tests" => await jobService.RunTestsAsync(args, cancellationToken).ConfigureAwait(false),
                    "publish_structured" => await jobService.PublishStructuredAsync(args, cancellationToken).ConfigureAwait(false),
                    "get_job_status" => jobService.GetJobStatus(args),
                    "get_job_log" => jobService.GetJobLog(args),
                    "cancel_job" => jobService.CancelJob(args),
                    _ => throw new ArgumentException($"Unknown tool: {name}.")
                };

                return new CallToolResult { Content = [new TextContentBlock { Text = text }], IsError = false };
            }
            catch (ArgumentException ex)
            {
                return new CallToolResult
                {
                    Content = [new TextContentBlock { Text = $"Error: {ex.Message}" }],
                    IsError = true
                };
            }
            catch (Exception ex)
            {
                return new CallToolResult
                {
                    Content = [new TextContentBlock { Text = "Error: " + ex.Message }],
                    IsError = true
                };
            }
        }
    }
};

var transport = new StdioServerTransport("DotnetBuildTestMcp");
await using var server = McpServer.Create(transport, options);
await server.RunAsync();
return 0;
