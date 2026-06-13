using System;
using RazorEnhanced.Macros;

namespace RazorEnhanced.Macros.Actions
{
    public static class MacroActionFactory
    {
        public static MacroAction CreateFromSerialized(string line)
        {
            if (string.IsNullOrWhiteSpace(line))
                return null;

            string[] parts = line.Split('|');
            if (parts.Length == 0)
                return null;

            string actionType = parts[0].ToLower();

            MacroAction action = actionType switch
            {
                "arm" => new ArmDisarmAction(),
                "disarm" => new ArmDisarmAction(),
                "armdisarm" => new ArmDisarmAction(),
                "attackentity" => new AttackAction(),
                "bandage" => new BandageAction(),
                "castspell" => new CastSpellAction(),
                "clearjournal" => new ClearJournalAction(),
                "comment" => new CommentAction(),
                "disconnect" => new DisconnectAction(),
                "doubleclick" => new DoubleClickAction(),
                "drop" => new DropAction(),
                "fly" => new FlyAction(),
                "gumpresponse" => new GumpResponseAction(),
                "invokevirtue" => new InvokeVirtueAction(),
                "messaging" => new MessagingAction(),
                "mount" => new MountAction(),
                "moveitem" => new MoveItemAction(),
                "movement" => new MovementAction(),
                "pickup" => new PickUpAction(),
                "promptresponse" => new PromptResponseAction(),
                "querystringresponse" => new QueryStringResponseAction(),
                "removealias" => new RemoveAliasAction(),
                "renamemobile" => new RenameMobileAction(),
                "resync" => new ResyncAction(),
                "runorganizeronce" => new RunOrganizerOnceAction(),
                "setability" => new SetAbilityAction(),
                "setalias" => new SetAliasAction(),
                "target" => new TargetAction(),
                "targetresource" => new TargetResourceAction(),
                "togglewarmode" => new ToggleWarModeAction(),
                "usecontextmenu" => new UseContextMenuAction(),
                "useemote" => new UseEmoteAction(),
                "usepotion" => new UsePotionAction(),
                "useskill" => new UseSkillAction(),
                "waitfortarget" => new WaitForTargetAction(),
                "waitforgump" => new WaitForGumpAction(),
                "if" => new IfAction(),
                "elseif" => new ElseIfAction(),
                "else" => new ElseAction(),
                "endif" => new EndIfAction(),
                "while" => new WhileAction(),
                "endwhile" => new EndWhileAction(),
                "for" => new ForAction(),
                "endfor" => new EndForAction(),
                "pause" => new PauseAction(),

                // Add any additional MacroAction types here as needed
                _ => null
            };

            if (action != null)
            {
                try
                {
                    action.Deserialize(line);
                }
                catch
                {
                    // Optionally log or handle deserialization errors
                    return null;
                }
            }

            return action;
        }
    }
}