using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Agents;
using Microsoft.SemanticKernel.Agents.Chat;
using Microsoft.SemanticKernel.ChatCompletion;
using ModelContextProtocol.Client;
using MultiAgentsOrchestration.Agents;
using MultiAgentsOrchestration.Console;
using MultiAgentsOrchestration.Console.Tools;
using System.Text.Json;
using Azure.Identity;
using Microsoft.Extensions.Configuration;

var kernelBuilder = Kernel.CreateBuilder();
var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", optional: true)
    .AddUserSecrets<Program>() // If using top-level statements, or use assembly reference
    .AddEnvironmentVariables() // Keep environment variables as fallback
    .Build();

var azureOpenAIEndpoint = configuration.GetValue<string>("AzureOpenAI:Url");

kernelBuilder.AddAzureOpenAIChatCompletion("gpt-4o-mini", azureOpenAIEndpoint, new DefaultAzureCredential());


var tools = await (await MCPTools.GetFileSystemToolsAsync()).GetAIFunctionsAsync();



    var kernel = kernelBuilder.Build();
#pragma warning disable SKEXP0001 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
kernel.Plugins.AddFromFunctions("filesystem", tools.Select(aiFunction => aiFunction.AsKernelFunction()));
#pragma warning restore SKEXP0001 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.

var agentFilesExplorer = new ChatAssistantAgent(kernel,"filesreader", "You are a file explorer expert, you know how to list files and to read them, your purpose is to open code source files and return results, use the filesystem tools you have for that and try to be autonomous by selecting the files");
Console.WriteLine("Testing agent 1: ");
var agentResponse = await agentFilesExplorer.InvokeAgentAsync("what is your expertise ?");
Console.WriteLine(agentResponse);
var kernel2 = kernel.Clone();
kernel2.Plugins.Clear();

var agentReviewer = new ChatAssistantAgent(kernel2, "codereviewer","You are a code expert, you will only review code source that is in the contexte, and give the best practices try to outline for each improvments the line number and if possible the fixe");
Console.WriteLine("Testing agent 2: ");
agentResponse = await agentReviewer.InvokeAgentAsync("what is your expertise ?");
Console.WriteLine(agentResponse);

var kernel3 = kernel.Clone();
kernel3.Plugins.Clear();
var finalApprover =  new ChatAssistantAgent(kernel3, "approver", "You are a code expert, you will only review the improvments on the suggested code, do not give improvements only tell if the review is complete or not but just saying : done");
Console.WriteLine("Testing agent 3: ");
agentResponse = await finalApprover.InvokeAgentAsync("what is your expertise ?");
Console.WriteLine(agentResponse);



#pragma warning disable SKEXP0001 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
#pragma warning disable SKEXP0110 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
KernelFunctionSelectionStrategy selectionStrategy =
     new(RunStrategy.GetSelectionFunction(), kernel)
     {
         // Always start with the writer agent.
         InitialAgent = agentFilesExplorer.Agent,
         // Parse the function response.
         ResultParser = (result) => result.GetValue<string>(),
         // The prompt variable name for the history argument.
         HistoryVariableName = "history",
         // Save tokens by not including the entire history in the prompt
         HistoryReducer = new ChatHistoryTruncationReducer(3),
     };




KernelFunctionTerminationStrategy terminationstrategy =
  new(RunStrategy.GetTerminationStrategy(), kernel)
  {
      // only the reviewer may give approval.
      Agents = [finalApprover.Agent],
      // parse the function response.
      ResultParser = (result) =>
        result.GetValue<string>()?.Contains("done", StringComparison.OrdinalIgnoreCase) ?? false,
      
      // the prompt variable name for the history argument.
      HistoryVariableName = "history",
      // save tokens by not including the entire history in the prompt
      HistoryReducer = new ChatHistoryTruncationReducer(1),
      // limit total number of turns no matter what
      MaximumIterations = 10,
  };
#pragma warning restore skexp0001 // type is for evaluation purposes only and is subject to change or removal in future updates. suppress this diagnostic to proceed.
#pragma warning restore skexp0110 // type is for evaluation purposes only and is subject to change or removal in future updates. suppress this diagnostic to proceed.

//#pragma warning disable SKEXP0110 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
AgentGroupChat chat =
            new(agentFilesExplorer.Agent, agentReviewer.Agent, finalApprover.Agent)
            {
                ExecutionSettings =
                    new()
                    {
                        TerminationStrategy = terminationstrategy,
                        SelectionStrategy = selectionStrategy
                    }
            };
#pragma warning restore SKEXP0110 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.




Console.WriteLine("Ready!");



chat.IsComplete = false;

try
{
    await foreach (ChatMessageContent response in chat.InvokeAsync())
    {
        Console.WriteLine();
#pragma warning disable SKEXP0001 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
        Console.WriteLine($"{response.AuthorName.ToUpperInvariant()}:{Environment.NewLine}{response.Content}");

        if (chat.IsComplete) { Console.WriteLine("finished the review");  }
#pragma warning restore SKEXP0001 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
        }
}
catch (HttpOperationException exception)
{
    Console.WriteLine(exception.Message);
    if (exception.InnerException != null)
    {
        Console.WriteLine(exception.InnerException.Message);
        if (exception.InnerException.Data.Count > 0)
        {
            Console.WriteLine(JsonSerializer.Serialize(exception.InnerException.Data, new JsonSerializerOptions() { WriteIndented = true }));
        }
    }
} while (!chat.IsComplete) ;

Console.WriteLine("The review is finished, enter to exit");

Console.ReadLine();


