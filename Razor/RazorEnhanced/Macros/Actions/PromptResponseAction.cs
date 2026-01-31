using System;

namespace RazorEnhanced.Macros.Actions
{
    public class PromptResponseAction : MacroAction
    {
        public string Response { get; set; }
        public int Timeout { get; set; }

        public PromptResponseAction()
        {
            Timeout = 10000;
            Response = "";
        }

        public PromptResponseAction(string response, int timeout = 10000)
        {
            Response = response;
            Timeout = timeout;
        }

        public override string GetActionName() => "Prompt Response";

        public override void Execute()
        {
            // Wait for prompt to appear
            Misc.WaitForPrompt(Timeout);

            // Send response
            if (!string.IsNullOrEmpty(Response))
            {
                Misc.ResponsePrompt(Response);
            }
        }

        public override int GetDelay() => 250;

        public override string Serialize()
        {
            return $"PromptResponse|{Escape(Response)}|{Timeout}";
        }

        public override void Deserialize(string data)
        {
            var parts = data.Split('|');
            if (parts.Length >= 2)
            {
                Response = Unescape(parts[1]);

                if (parts.Length >= 3 && int.TryParse(parts[2], out int timeout))
                    Timeout = timeout;
                else
                    Timeout = 10000;
            }
        }

        // Add these helpers to the class:
        private static string Escape(string value)
        {
            if (value == null) return "";
            return value.Replace("\\", "\\\\").Replace("|", "\\|");
        }

        private static string Unescape(string value)
        {
            if (value == null) return "";
            return value.Replace("\\|", "|").Replace("\\\\", "\\");
        }


    }
}