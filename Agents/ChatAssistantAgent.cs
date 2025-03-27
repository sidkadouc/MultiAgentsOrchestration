using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Agents;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using System.Text;

namespace MultiAgentsOrchestration.Agents
{
    public class ChatAssistantAgent
    {
        private readonly Kernel _kernel;
        private readonly ChatHistory _chatHistory;
        private readonly ChatCompletionAgent _agent;
   


        public ChatAssistantAgent(Kernel kernel,string agentName, string agentInstructions)
        {
            this._kernel = kernel;
            this._chatHistory = [];

            // Define the agent
#pragma warning disable SKEXP0001 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
            this._agent =
                new()
                {
                    Instructions = agentInstructions,
                    Name = agentName,
                    Kernel = this._kernel,
                    Arguments = new KernelArguments(new OpenAIPromptExecutionSettings()
                    {
                        Temperature = 0,
                        //ServiceId = "gpt-4o-mini",
                        FunctionChoiceBehavior = FunctionChoiceBehavior.Auto(),

                    }),
                };
#pragma warning restore SKEXP0001 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
        }

        public ChatCompletionAgent Agent => this._agent;

        public async Task<string> InvokeAgentAsync(string input)
        {
            ChatMessageContent message = new(AuthorRole.User, input);
            this._chatHistory.Add(message);

            StringBuilder sb = new();
            await foreach (ChatMessageContent response in this._agent.InvokeAsync(this._chatHistory))
            {
                this._chatHistory.Add(response);
                sb.Append(response.Content);
            }

           return sb.ToString();
        }
    }

}