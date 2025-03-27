
using Microsoft.Extensions.Logging.Abstractions;
using ModelContextProtocol.Client;
using ModelContextProtocol.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiAgentsOrchestration.Console.Tools
{
    public class MCPTools
    {
        public static async Task<IMcpClient> GetGitHubToolsAsync()
        {
            McpClientOptions options = new()
            {
                ClientInfo = new() { Name = "GitHub", Version = "1.0.0" }
            };

            var config = new McpServerConfig
            {
                Id = "github",
                Name = "GitHub",
                TransportType = "stdio",
                TransportOptions = new Dictionary<string, string>
                {
                    ["command"] = "npx",
                    ["arguments"] = "-y @modelcontextprotocol/server-github",
                }
            };

            var client = await McpClientFactory.CreateAsync(config, options);

            return client;

        }



        public static async Task<IMcpClient> GetFileSystemToolsAsync()
        {
            McpClientOptions options = new()
            {
                ClientInfo = new() { Name = "GitHub", Version = "1.0.0" }
            };

            var config = new McpServerConfig
            {
                Id = "filesystem",
                Name = "filesystem",
                TransportType = "stdio",
                TransportOptions = new Dictionary<string, string>
                {
                    ["command"] = "npx",
                    ["arguments"] = "-y @modelcontextprotocol/server-filesystem C:\\dev\\Multi-Agents\\MultiAgentsOrchestration.Console\\CodeSample",
                }
            };


            var client = await McpClientFactory.CreateAsync(config, options);

            return client;

        }
    }
}
