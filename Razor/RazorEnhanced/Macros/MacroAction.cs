using System;

namespace RazorEnhanced.Macros
{
    /// <summary>
    /// Base class for all macro actions
    /// </summary>
    public abstract class MacroAction
    {
        public abstract string GetActionName();
        public abstract void Execute();
        public abstract string Serialize();
        public abstract void Deserialize(string data);

        public virtual bool IsValid() => true;
        public virtual int GetDelay() => 0;
    }

    /// <summary>
    /// Macro action types matching Razor CE
    /// </summary>
    public enum MacroActionType
    {
        // Movement
        Walk,

        // Combat
        Attack,
        LastTarget,
        TargetSelf,

        // Spells & Skills
        CastSpell,
        UseSkill,

        // Items
        UseItem,
        DoubleClick,
        UseItemByType,
        DropItem,
        LiftItem,
        Arm,
        Disarm,
        UseLastWeapon,

        // Targeting
        WaitForTarget,
        TargetRandom,
        TargetClose,
        ClearTargetQueue,

        // Messages
        Say,
        Emote,
        Whisper,
        Yell,

        // Delays & Control
        Pause,
        WaitForGump,
        WaitForMenu,
        WaitForStat,

        // Misc
        Virtue,
        InvokeVirtue,
        UsePotion,
        Bandage,
        BandageSelf,
        Resync,
        Snapshot,
        Hotkey,
        SetAbility
    }
}