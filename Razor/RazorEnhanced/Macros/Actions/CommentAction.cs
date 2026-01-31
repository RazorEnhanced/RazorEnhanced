using System;

namespace RazorEnhanced.Macros.Actions
{
    public class CommentAction : MacroAction
    {
        public string Comment { get; set; }

        public CommentAction()
        {
            Comment = "";
        }

        public CommentAction(string comment)
        {
            Comment = comment ?? "";
        }

        public override string GetActionName() => "Comment";

        public override void Execute()
        {
            // Comments don't execute - they're just for documentation
        }

        public override int GetDelay() => 0; // No delay for comments

        public override string Serialize()
        {
            return $"Comment|{Escape(Comment)}";
        }

        public override void Deserialize(string data)
        {
            var parts = data.Split('|');
            if (parts.Length >= 2)
            {
                Comment = Unescape(parts[1]);
            }
        }

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

        public override bool IsValid()
        {
            return true; // Comments are always valid
        }
    }
}