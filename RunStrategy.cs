using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Agents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiAgentsOrchestration.Console
{
    public class RunStrategy
    {
        /// Step 2: Define Selection Strategy
        /// This function determines which agent should take the next turn in the collaboration.
        public static KernelFunction GetSelectionFunction()
        {
#pragma warning disable SKEXP0110 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
            return AgentGroupChat.CreatePromptFunctionForStrategy(
                $$$"""
        Determine which participant takes the next turn in a conversation based on the most recent participant.
        State only the name of the participant to take the next turn.
        No participant should take more than one turn in a row.

        Choose only from these participants:
        - filesreader
        - codereviewer
        - approver

           Always follow these rules when selecting the next participant:
        - After filesreader, it is codereviewer's turn.
        - After codereviewer, it is approver's turn.
        - After approver's, it is codereviewer's turn or filesreader's turn depending on the demand
      
        
        History:
        {{$history}}
        """,
                safeParameterNames: "history");
#pragma warning restore SKEXP0110 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
        }


        public static KernelFunction GetTerminationStrategy()
        {
#pragma warning disable SKEXP0110 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
            KernelFunction terminationFunction =
                AgentGroupChat.CreatePromptFunctionForStrategy(
                    $$$"""
            Determine if the you answered to the user request. If so, respond with a single word: done 

            History:
            {{$history}}
            """,
                  safeParameterNames: "history");
#pragma warning restore SKEXP0110 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.

            return terminationFunction;
        }
    }
}
