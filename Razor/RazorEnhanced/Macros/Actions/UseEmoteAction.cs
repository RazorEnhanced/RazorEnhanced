using RazorEnhanced;

namespace RazorEnhanced.Macros.Actions
{
    public class UseEmoteAction : MacroAction
    {
        public string EmoteName { get; set; }

        public UseEmoteAction()
        {
            EmoteName = "bow";
        }

        public UseEmoteAction(string emoteName)
        {
            EmoteName = emoteName ?? "bow";
        }

        public override string GetActionName() => "Use Emote";

        public override void Execute()
        {
            if (!string.IsNullOrEmpty(EmoteName))
            {
                Player.EmoteAction(EmoteName);
            }
        }

        public override string Serialize()
        {
            return $"UseEmote|{Escape(EmoteName)}";
        }

        public override void Deserialize(string data)
        {
            var parts = data.Split('|');
            if (parts.Length >= 2)
            {
                EmoteName = Unescape(parts[1]);
            }
            else
            {
                EmoteName = "bow"; // Default
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

        public override bool IsValid()
        {
            return !string.IsNullOrEmpty(EmoteName);
        }

        public override int GetDelay()
        {
            return 250; // Small delay for emote animation
        }
    }
}