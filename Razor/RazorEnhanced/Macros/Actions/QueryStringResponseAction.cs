using System;

namespace RazorEnhanced.Macros.Actions
{
    public class QueryStringResponseAction : MacroAction
    {
        public bool Accept { get; set; }
        public string Response { get; set; }
        public int Timeout { get; set; }

        public QueryStringResponseAction()
        {
            Timeout = 10000;
            Response = "";
            Accept = true;
        }

        public QueryStringResponseAction(bool accept, string response, int timeout = 10000)
        {
            Accept = accept;
            Response = response;
            Timeout = timeout;
        }

        public override string GetActionName() => "Query String Response";

        public override void Execute()
        {
            // Wait for query string prompt
            Misc.WaitForQueryString(Timeout);

            // Send response
            Misc.QueryStringResponse(Accept, Response);
        }

        public override int GetDelay() => 250;

        public override string Serialize()
        {
            return $"QueryStringResponse|{Accept}|{Response.Replace("|", "&#124;")}|{Timeout}";
        }

        public override void Deserialize(string data)
        {
            var parts = data.Split('|');
            if (parts.Length >= 3)
            {
                bool.TryParse(parts[1], out bool accept);
                Accept = accept;

                Response = parts[2].Replace("&#124;", "|");

                if (parts.Length >= 4)
                {
                    int.TryParse(parts[3], out int timeout);
                    Timeout = timeout;
                }
                else
                {
                    Timeout = 10000;
                }
            }
        }
    }
}