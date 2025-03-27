using Azure;
using Azure.AI.Projects;
using Azure.Identity;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Agents.AzureAI;
using Microsoft.SemanticKernel.ChatCompletion;

#pragma warning disable SKEXP0110 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
public class AzureAIAgent
{
    private readonly AIProjectClient _client;
    private readonly AgentsClient _agentsClient;
    private readonly Kernel _kernel;
    private readonly Microsoft.SemanticKernel.Agents.AzureAI.AzureAIAgent _agent;







    public AzureAIAgent(Kernel kernel, string connectionString, string? agentId = null)
    {


        _client = Microsoft.SemanticKernel.Agents.AzureAI.AzureAIAgent.CreateAzureAIClient(connectionString, new AzureCliCredential());

        AgentsClient agentsClient = _client.GetAgentsClient();

        Response<Agent> agentResponse = _agentsClient.GetAgent(agentId);
        Agent agent = agentResponse.Value;
        _agent = new(agent, _agentsClient)
        {
            Kernel = _kernel
        };

        // 1. Define an agent on the Azure AI agent service


    }

    public async Task<Microsoft.SemanticKernel.Agents.AzureAI.AzureAIAgent> GetAgent()
    {
        return _agent;
    }
#pragma warning restore SKEXP0110 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
    // Local function to invoke agent and display the conversation messages.
    public async Task InvokeAgentAsync(string input)
    {
        AgentThread thread = await _agentsClient.CreateThreadAsync();
        try
        {
            ChatMessageContent message = new(AuthorRole.User, input);
#pragma warning disable SKEXP0110 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
            await _agent.AddChatMessageAsync(thread.Id, message);

            await foreach (ChatMessageContent response in _agent.InvokeAsync(thread.Id))
            {
                Console.WriteLine(response.Content);
            }
#pragma warning restore SKEXP0110 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
        }
        finally
        {
            await _agentsClient.DeleteThreadAsync(thread.Id);
          
        }
    }
}

