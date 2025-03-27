# MultiAgentsOrchestration

This code sample demonstrates the agents basic orchestration in Semantic Kernel, using the MCP tools to implement a code reviewer solution.

## Overview

The application implements a multi-agent system for code review that consists of three specialized agents:
- **File Explorer Agent**: Navigates and reads source code files from the filesystem
- **Code Reviewer Agent**: Reviews the code and suggests improvements based on best practices
- **Final Approver Agent**: Validates the review and confirms completion

## Setup Instructions

### Prerequisites
- .NET 9.0 SDK
- Azure OpenAI account with access to GPT models (here only gpt-4o-mini is used)
- Visual Studio 2022 or VS Code with C# extension

### Configuration

1. Clone the repository
2. Configure user secrets for Azure OpenAI:

```bash
dotnet user-secrets init
dotnet user-secrets set "AzureOpenAI:Url" "https://your-azure-openai-resource.openai.azure.com/"
```

3. Make sure you have the proper Azure credentials configured for DefaultAzureCredential

### Project Structure

- **Agents/**: Contains the abstract definition of  AI agents supported in semantic kernel : Azure AI Agent, Azure OpenAi Assistant, ChatCompletion Agent
- **Tools/**: Contains the MCP tools implementation to access the filesystem 
- **CodeSample/**: Contains sample code files for testing the review process

### Running the Application

1. Build the solution:
```bash
dotnet build
```

2. Run the application:
```bash
dotnet run
```

3. The agents will orchestrate the code review process automatically, with each agent contributing according to its expertise.

## Notes

- This sample uses experimental Semantic Kernel features (marked with SKEXP warnings)
- The agent orchestration follows a strategic workflow with customizable selection and termination strategies
- The maximum number of iterations is limited to 10 to prevent excessive processing
