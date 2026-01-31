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

            string actionType = parts[0];

            MacroAction action = actionType switch
            {
                "Arm" => new ArmDisarmAction(),
                "Disarm" => new ArmDisarmAction(),
                "ArmDisarm" => new ArmDisarmAction(),
                "AttackEntity" => new AttackAction(),
                "Bandage" => new BandageAction(),
                "CastSpell" => new CastSpellAction(),
                "ClearJournal" => new ClearJournalAction(),
                "Comment" => new CommentAction(),
                "Disconnect" => new DisconnectAction(),
                "DoubleClick" => new DoubleClickAction(),
                "Drop" => new DropAction(),
                "Fly" => new FlyAction(),
                "GumpResponse" => new GumpResponseAction(),
                "InvokeVirtue" => new InvokeVirtueAction(),
                "Messaging" => new MessagingAction(),
                "Mount" => new MountAction(),
                "MoveItem" => new MoveItemAction(),
                "Movement" => new MovementAction(),
                "PickUp" => new PickUpAction(),
                "PromptResponse" => new PromptResponseAction(),
                "QueryStringResponse" => new QueryStringResponseAction(),
                "RemoveAlias" => new RemoveAliasAction(),
                "RenameMobile" => new RenameMobileAction(),
                "Resync" => new ResyncAction(),
                "RunOrganizerOnce" => new RunOrganizerOnceAction(),
                "SetAbility" => new SetAbilityAction(),
                "SetAlias" => new SetAliasAction(),
                "Target" => new TargetAction(),
                "TargetResource" => new TargetResourceAction(),
                "ToggleWarMode" => new ToggleWarModeAction(),
                "UseContextMenu" => new UseContextMenuAction(),
                "UseEmote" => new UseEmoteAction(),
                "UsePotion" => new UsePotionAction(),
                "UseSkill" => new UseSkillAction(),
                "WaitForTarget" => new WaitForTargetAction(),
                "WaitForGump" => new WaitForGumpAction(),
                "If" => new IfAction(),
                "ElseIf" => new ElseIfAction(),
                "Else" => new ElseAction(),
                "EndIf" => new EndIfAction(),
                "While" => new WhileAction(),
                "EndWhile" => new EndWhileAction(),
                "For" => new ForAction(),
                "EndFor" => new EndForAction(),
                "Pause" => new PauseAction(),

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