using Azure.AI.Projects;
using Azure.Identity;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Agents.OpenAI;
using OpenAI.Assistants;

public class AzureOAIAssistantAgent
{
#pragma warning disable OPENAI001 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
    private readonly AssistantClient client;
#pragma warning restore OPENAI001 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
    private readonly Kernel _kernel;


    private readonly Azure.AI.Projects.Agent _agentDefinition;
#pragma warning disable SKEXP0110 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
    private readonly Microsoft.SemanticKernel.Agents.AzureAI.AzureAIAgent _agent;
#pragma warning restore SKEXP0110 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.



    public AzureOAIAssistantAgent(Kernel kernel)
    {
#pragma warning disable OPENAI001 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
        client = Microsoft.SemanticKernel.Agents.OpenAI.OpenAIAssistantAgent.CreateAzureOpenAIClient(new System.ClientModel.ApiKeyCredential("key"), new Uri("endpoint")).GetAssistantClient();
#pragma warning restore OPENAI001 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
    }

#pragma warning disable SKEXP0110 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
    public Microsoft.SemanticKernel.Agents.AzureAI.AzureAIAgent Agent => this._agent;
#pragma warning restore SKEXP0110 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.

    //// Local function to invoke agent and display the conversation messages.
    //public async Task InvokeAgentAsync(string input)
    //{
    //    AgentThread thread = await _agentsClient.CreateThreadAsync();
    //    try
    //    {
    //        ChatMessageContent message = new(AuthorRole.User, input);
    //        await _agent.AddChatMessageAsync(thread.Id, message);
    //        await foreach (ChatMessageContent response in _agent.InvokeAsync(thread.Id))
    //        {
    //            Console.WriteLine(response.Content);
    //        }
    //    }
    //    finally
    //    {
    //        await _agentsClient.DeleteThreadAsync(thread.Id);
    //        //await this._agentsClient.DeleteAgentAsync(agent.Id);
    //    }
    //}

}
    