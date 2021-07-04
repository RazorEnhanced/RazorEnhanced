using Assistant;
using System.Collections.Generic;

namespace RazorEnhanced
{
    /// <summary>
    /// The Spells class allow you to cast any spell and use abilities, via scripting.
    /// </summary>
    public class Spells
    {

        // functions used internally
        internal static string GuessSpellName(string originalName)
        {
            int distance = 99;
            string closest = "";

            foreach (string spell in m_AllSpells.Keys)
            {
                int computeDistance = UOAssist.LevenshteinDistance(spell, originalName);
                if (computeDistance < distance)
                {
                    distance = computeDistance;
                    closest = spell;
                }
            }

            if (distance < 99)
                return closest;
            return originalName;

        }

        
        internal static void CastOnly(string SpellName, bool wait)
        {
            //
            bool success = false;
            string guessedSpellName = GuessSpellName(SpellName);
            if (m_AllSpells.ContainsKey(guessedSpellName))
            {
                success = CastOnlyGeneric(m_AllSpells, guessedSpellName, wait);
            }
            if (!success)
                Scripts.SendMessageScriptError("Script Error: Cast: Invalid spell name: " + SpellName);
        }
        
        internal static bool CastOnlyGeneric(Dictionary<string, int> conversion, string SpellName, bool wait)
        {
            if (World.Player == null)
                return true;
            int id;
            conversion.TryGetValue(SpellName, out id);


            Spell s;
            if (id > 0)
                s = Spell.Get(id);
            else
                return false;

            if (s != null)
                s.OnCast(new CastSpellFromMacro((ushort)s.GetID()), wait);
            return true;
        }

        internal static bool CastTargetedGeneric(Dictionary<string, int> conversion,
            string SpellName, uint target, bool wait)
        {
            if (World.Player == null)
                return true;
            int id;
            conversion.TryGetValue(SpellName, out id);


            Spell s;
            if (id > 0)
                s = Spell.Get(id);
            else
                return false;

            if (s != null)
                s.OnCast(new CastTargetedSpell((ushort)s.GetID(), target), wait);
            return true;
        }



        /// <summary>
        /// Cast spell using the spell name. See the skill-specific functions to get the full list of spell names.
        /// Optionally is possible to specify the Mobile or a Serial as target of the spell. Upon successful casting, the target will be executed automatiaclly by the server.
        /// NOTE: The "automatic" target is not supported by all shards, but you can restort to the Target class to handle it manually.
        /// </summary>
        /// <param name="SpellName">Name of the spell to cast.</param>
        /// <param name="target">Optional: Serial or Mobile to target (default: null)</param>
        /// <param name="wait">Optional: Wait server to confirm. (default: True)</param>
        public static void Cast(string SpellName, uint target, bool wait = true)
        {
            //
            bool success = false;
            string guessedSpellName = GuessSpellName(SpellName);
            if (m_AllSpells.ContainsKey(guessedSpellName))
            {
                success = CastTargetedGeneric(m_AllSpells, guessedSpellName, target, wait);
            }
            if (!success)
                Scripts.SendMessageScriptError("Script Error: Cast: Invalid spell name: " + SpellName);
        }

        public static void Cast(string SpellName, Mobile mobile, bool wait = true)
        {
            Cast(SpellName, (uint)mobile.Serial, wait);
        }

        public static void Cast(string SpellName)
        {
            CastOnly(SpellName, true);
        }

        


        /// <summary>
        /// Cast a Magery spell using the spell name.
        /// Optionally is possible to specify the Mobile or a Serial as target of the spell. Upon successful casting, the target will be executed automatiaclly by the server.
        /// NOTE: The "automatic" target is not supported by all shards, but you can restort to the Target class to handle it manually.
        /// </summary>
        /// <param name="SpellName">
        ///    Clumsy
        ///    Create Food
        ///    Feeblemind
        ///    Heal
        ///    Magic Arrow
        ///    Night Sight
        ///    Reactive Armor
        ///    Weaken
        ///    Agility
        ///    Cunning
        ///    Cure
        ///    Harm
        ///    Magic Trap
        ///    Magic Untrap
        ///    Protection
        ///    Strength
        ///    Bless
        ///    Fireball
        ///    Magic Lock
        ///    Poison
        ///    Telekinesis
        ///    Teleport
        ///    Unlock
        ///    Wall of Stone
        ///    Arch Cure
        ///    Arch Protection
        ///    Curse
        ///    Fire Field
        ///    Greater Heal
        ///    Lightning
        ///    Mana Drain
        ///    Recall
        ///    Blade Spirits
        ///    Dispel Field
        ///    Incognito
        ///    Magic Reflection
        ///    Mind Blast
        ///    Paralyze
        ///    Poison Field
        ///    Summon Creature
        ///    Dispel
        ///    Energy Bolt
        ///    Explosion
        ///    Invisibility
        ///    Mark
        ///    Mass Curse
        ///    Paralyze Field
        ///    Reveal
        ///    Chain Lightning
        ///    Energy Field
        ///    Flamestrike
        ///    Gate Travel
        ///    Mana Vampire
        ///    Mass Dispel
        ///    Meteor Swarm
        ///    Polymorph
        ///    Earthquake
        ///    Energy Vortex
        ///    Resurrection
        ///    Summon Air Elemental
        ///    Summon Daemon
        ///    Summon Earth Elemental
        ///    Summon Fire Elemental
        ///    Summon Water Elemental
        /// </param>
        /// <param name="target">Optional: Serial or Mobile to target (default: null)</param>
        /// <param name="wait">Optional: Wait server to confirm. (default: True)</param>
        public static void CastMagery(string SpellName, uint target, bool wait = true)
        {
            string guessedSpellName = GuessSpellName(SpellName);
            bool success = CastTargetedGeneric(m_MagerySpellName, guessedSpellName, target, wait);
            if (!success)
                Scripts.SendMessageScriptError("Script Error: CastMagery: Invalid spell name: " + SpellName);
        }

        public static void CastMagery(string SpellName, Mobile mobile, bool wait = true)
        {
            CastMagery(SpellName, (uint)mobile.Serial, wait);
        }

        public static void CastMagery(string SpellName)
        {
            CastOnlyMagery(SpellName, true);
        }
        

        internal static void CastOnlyMagery(string SpellName, bool wait)
        {
            string guessedSpellName = GuessSpellName(SpellName);
            bool success = CastOnlyGeneric(m_MagerySpellName, guessedSpellName, wait);
            if (!success)
                Scripts.SendMessageScriptError("Script Error: CastMagery: Invalid spell name: " + SpellName);
        }


        /// <summary>
        /// Cast a Necromany spell using the spell name.
        /// Optionally is possible to specify the Mobile or a Serial as target of the spell. Upon successful casting, the target will be executed automatiaclly by the server.
        /// NOTE: The "automatic" target is not supported by all shards, but you can restort to the Target class to handle it manually.
        /// </summary>
        /// <param name="SpellName">
        ///    Curse Weapon
        ///    Pain Spike
        ///    Corpse Skin
        ///    Evil Omen
        ///    Blood Oath
        ///    Wraith Form
        ///    Mind Rot
        ///    Summon Familiar
        ///    Horrific Beast
        ///    Animate Dead
        ///    Poison Strike
        ///    Wither
        ///    Strangle
        ///    Lich Form
        ///    Exorcism
        ///    Vengeful Spirit
        ///    Vampiric Embrace
        /// </param>
        /// <param name="target">Optional: Serial or Mobile to target (default: null)</param>
        /// <param name="wait">Optional: Wait server to confirm. (default: True)</param>
        public static void CastNecro(string SpellName, uint target, bool wait = true)
        {
            string guessedSpellName = GuessSpellName(SpellName);
            bool success = CastTargetedGeneric(m_NecroSpellName, guessedSpellName, target, wait);
            if (!success)
                Scripts.SendMessageScriptError("Script Error: CastNecro: Invalid spell name: " + SpellName);
        }

        public static void CastNecro(string SpellName, Mobile mobile, bool wait = true)
        {
            CastNecro(SpellName, (uint)mobile.Serial, wait);
        }

        public static void CastNecro(string SpellName)
        {
            CastOnlyNecro(SpellName);
        }


        internal static void CastOnlyNecro(string SpellName, bool wait = true)
        {
            string guessedSpellName = GuessSpellName(SpellName);
            bool success = CastOnlyGeneric(m_NecroSpellName, guessedSpellName, wait);
            if (!success)
                Scripts.SendMessageScriptError("Script Error: CastNecro: Invalid spell name: " + SpellName);
        }


        /// <summary>
        /// Cast a Chivalry spell using the spell name.
        /// Optionally is possible to specify the Mobile or a Serial as target of the spell. Upon successful casting, the target will be executed automatiaclly by the server.
        /// NOTE: The "automatic" target is not supported by all shards, but you can restort to the Target class to handle it manually.
        /// </summary>
        /// <param name="SpellName">
        ///    Curse Weapon
        ///    Pain Spike
        ///    Corpse Skin
        ///    Evil Omen
        ///    Blood Oath
        ///    Wraith Form
        ///    Mind Rot
        ///    Summon Familiar
        ///    Horrific Beast
        ///    Animate Dead
        ///    Poison Strike
        ///    Wither
        ///    Strangle
        ///    Lich Form
        ///    Exorcism
        ///    Vengeful Spirit
        ///    Vampiric Embrace
        /// </param>
        /// <param name="target">Optional: Serial or Mobile to target (default: null)</param>
        /// <param name="wait">Optional: Wait server to confirm. (default: True)</param>
        public static void CastChivalry(string SpellName, uint target, bool wait = true)
        {
            string guessedSpellName = GuessSpellName(SpellName);
            bool success = CastTargetedGeneric(m_ChivalrySpellName, guessedSpellName, target, wait);
            if (!success)
                Scripts.SendMessageScriptError("Script Error: CastChivalry: Invalid spell name: " + SpellName);
        }

        public static void CastChivalry(string SpellName, Mobile mobile, bool wait = true)
        {
            CastChivalry(SpellName, (uint)mobile.Serial, wait);
        }

        public static void CastChivalry(string SpellName)
        {
            CastOnlyChivalry(SpellName);
        }
        
        internal static void CastOnlyChivalry(string SpellName, bool wait = true)
        {
            string guessedSpellName = GuessSpellName(SpellName);
            bool success = CastOnlyGeneric(m_ChivalrySpellName, guessedSpellName, wait);
            if (!success)
                Scripts.SendMessageScriptError("Script Error: CastChivalry: Invalid spell name: " + SpellName);
        }

        /// <summary>
        /// Cast a Bushido spell using the spell name.
        /// </summary>
        /// <param name="SpellName">
        ///    Honorable Execution
        ///    Confidence
        ///    Counter Attack
        ///    Lightning Strike
        ///    Evasion
        ///    Momentum Strike
        /// </param>
        /// <param name="wait">Optional: Wait server to confirm. (default: True)</param>
        /// 
        public static void CastBushido(string SpellName, bool wait = true)
        {
            CastOnlyBushido(SpellName, wait);
        }

        internal static void CastOnlyBushido(string SpellName, bool wait = true)
        {
            string guessedSpellName = GuessSpellName(SpellName);
            bool success = CastOnlyGeneric(m_BushidoSpellName, guessedSpellName, wait);
            if (!success)
                Scripts.SendMessageScriptError("Script Error: CastBushido: Invalid spell name: " + SpellName);
        }

        /// <summary>
        /// Cast a Ninjitsu spell using the spell name.
        /// Optionally is possible to specify the Mobile or a Serial as target of the spell. Upon successful casting, the target will be executed automatiaclly by the server.
        /// NOTE: The "automatic" target is not supported by all shards, but you can restort to the Target class to handle it manually.
        /// </summary>
        /// <param name="SpellName">
        ///    Animal Form
        ///    Backstab
        ///    Surprise Attack
        ///    Mirror Image
        ///    Shadow jump
        ///    Focus Attack
        ///    Ki Attack
        /// </param>
        /// <param name="target">Optional: Serial or Mobile to target (default: null)</param>
        /// <param name="wait">Optional: Wait server to confirm. (default: True)</param>

        public static void CastNinjitsu(string SpellName, uint target, bool wait = true)
        {
            string guessedSpellName = GuessSpellName(SpellName);
            bool success = CastTargetedGeneric(m_NinjitsuSpellName, guessedSpellName, target, wait);
            if (!success)
                Scripts.SendMessageScriptError("Script Error: CastNinjitsu: Invalid spell name: " + SpellName);
        }

        public static void CastNinjitsu(string SpellName, Mobile mobile, bool wait = true)
        {
            CastNinjitsu(SpellName, (uint)mobile.Serial, wait);
        }

        public static void CastNinjitsu(string SpellName)
        {
            CastOnlyNinjitsu(SpellName);
        }

        internal static void CastOnlyNinjitsu(string SpellName, bool wait = true)
        {
            string guessedSpellName = GuessSpellName(SpellName);
            bool success = CastOnlyGeneric(m_NinjitsuSpellName, guessedSpellName, wait);
            if (!success)
                Scripts.SendMessageScriptError("Script Error: CastNinjitsu: Invalid spell name: " + SpellName);
        }

        /// <summary>
        /// Cast a Spellweaving spell using the spell name.
        /// Optionally is possible to specify the Mobile or a Serial as target of the spell. Upon successful casting, the target will be executed automatiaclly by the server.
        /// NOTE: The "automatic" target is not supported by all shards, but you can restort to the Target class to handle it manually.
        /// </summary>
        /// <param name="SpellName">
        ///    Arcane Circle
        ///    Gift Of Renewal
        ///    Immolating Weapon
        ///    Attune Weapon
        ///    Thunderstorm
        ///    Natures Fury
        ///    Summon Fey
        ///    Summoniend
        ///    Reaper Form
        ///    Wildfire
        ///    Essence Of Wind
        ///    Dryad Allure
        ///    Ethereal Voyage
        ///    Word Of Death
        ///    Gift Of Life
        ///    Arcane Empowerment
        /// </param>
        /// <param name="target">Optional: Serial or Mobile to target (default: null)</param>
        /// <param name="wait">Optional: Wait server to confirm. (default: True)</param>
        public static void CastSpellweaving(string SpellName, uint target, bool wait = true)
        {
            string guessedSpellName = GuessSpellName(SpellName);
            bool success = CastTargetedGeneric(m_SpellweavingSpellName, guessedSpellName, target, wait);
            if (!success)
                Scripts.SendMessageScriptError("Script Error: CastSpellweaving: Invalid spell name: " + SpellName);
        }

        public static void CastSpellweaving(string SpellName, Mobile mobile, bool wait = true)
        {
            CastSpellweaving(SpellName, (uint)mobile.Serial, wait);
        }



        public static void CastSpellweaving(string SpellName)
        {
            CastOnlySpellweaving(SpellName);
        }

        internal static void CastOnlySpellweaving(string SpellName, bool wait = true)
        {
            string guessedSpellName = GuessSpellName(SpellName);
            bool success = CastOnlyGeneric(m_SpellweavingSpellName, guessedSpellName, wait);
            if (!success)
                Scripts.SendMessageScriptError("Script Error: CastSpellweaving: Invalid spell name: " + SpellName);
        }



        /// <summary>
        /// Cast a Mysticism spell using the spell name.
        /// Optionally is possible to specify the Mobile or a Serial as target of the spell. Upon successful casting, the target will be executed automatiaclly by the server.
        /// NOTE: The "automatic" target is not supported by all shards, but you can restort to the Target class to handle it manually. 
        /// </summary>
        /// <param name="SpellName">
        ///    Animated Weapon
        ///    Healing Stone
        ///    Purge
        ///    Enchant
        ///    Sleep
        ///    Eagle Strike
        ///    Stone Form
        ///    SpellTrigger
        ///    Mass Sleep
        ///    Cleansing Winds
        ///    Bombard
        ///    Spell Plague
        ///    Hail Storm
        ///    Nether Cyclone
        ///    Rising Colossus
        /// </param>
        /// <param name="target">Optional: Serial or Mobile to target (default: null)</param>
        /// <param name="wait">Optional: Wait server to confirm. (default: True)</param>
        public static void CastMysticism(string SpellName, uint target, bool wait = true)
        {
            string guessedSpellName = GuessSpellName(SpellName);
            bool success = CastTargetedGeneric(m_MysticismSpellName, guessedSpellName, target, wait);
            if (!success)
                Scripts.SendMessageScriptError("Script Error: CastMysticism: Invalid spell name: " + SpellName);
        }

        public static void CastMysticism(string SpellName, Mobile mobile, bool wait = true)
        {
            CastMysticism(SpellName, (uint)mobile.Serial, wait);
        }

        public static void CastMysticism(string SpellName)
        {
            CastOnlyMysticism(SpellName);
        }
        
        internal static void CastOnlyMysticism(string SpellName, bool wait = true)
        {
            string guessedSpellName = GuessSpellName(SpellName);
            bool success = CastOnlyGeneric(m_MysticismSpellName, guessedSpellName, wait);
            if (!success)
                Scripts.SendMessageScriptError("Script Error: CastMysticism: Invalid spell name: " + SpellName);
        }

        /// <summary>
        /// Cast a Mastery spell using the spell name.
        /// Optionally is possible to specify the Mobile or a Serial as target of the spell. Upon successful casting, the target will be executed automatiaclly by the server.
        /// NOTE: The "automatic" target is not supported by all shards, but you can restort to the Target class to handle it manually. 
        /// </summary>
        /// <param name="SpellName">
        ///    Inspire
        ///    Invigorate
        ///    Resilience
        ///    Perseverance
        ///    Tribulation
        ///    Despair
        ///    Death Ray
        ///    Ethereal Blast
        ///    Nether Blast
        ///    Mystic Weapon
        ///    Command Undead
        ///    Conduit
        ///    Mana Shield
        ///    Summon Reaper
        ///    Enchanted Summoning
        ///    Anticipate Hit
        ///    Warcry
        ///    Intuition
        ///    Rejuvenate
        ///    Holy Fist
        ///    Shadow
        ///    White Tiger Form
        ///    Flaming Shot
        ///    Playing The Odds
        ///    Thrust
        ///    Pierce
        ///    Stagger
        ///    Toughness
        ///    Onslaught
        ///    Focused Eye
        ///    Elemental Fury
        ///    Called Shot
        ///    Saving Throw
        ///    Shield Bash
        ///    Bodyguard
        ///    Heighten Senses
        ///    Tolerance
        ///    Injected Strike
        ///    Potency
        ///    Rampage
        ///    Fists Of Fury
        ///    Knockout
        ///    Whispering
        ///    Combat Training
        ///    Boarding
        /// </param>
        /// <param name="target">Optional: Serial or Mobile to target (default: null)</param>
        /// <param name="wait">Optional: Wait server to confirm. (default: True)</param>

        public static void CastMastery(string SpellName, uint target, bool wait = true)
        {
            string guessedSpellName = GuessSpellName(SpellName);
            bool success = CastTargetedGeneric(m_MasterySpellName, guessedSpellName, target, wait);
            if (!success)
                Scripts.SendMessageScriptError("Script Error: CastMastery: Invalid spell name: " + SpellName);
        }

        public static void CastMastery(string SpellName, Mobile mobile, bool wait = true)
        {
            CastMastery(SpellName, (uint)mobile.Serial, wait);
        }

        public static void CastMastery(string SpellName)
        {
            CastOnlyMastery(SpellName);
        }
        
        internal static void CastOnlyMastery(string SpellName, bool wait = true)
        {
            string guessedSpellName = GuessSpellName(SpellName);
            bool success = CastOnlyGeneric(m_MasterySpellName, guessedSpellName, wait);
            if (!success)
                Scripts.SendMessageScriptError("Script Error: CastMastery: Invalid spell name: " + SpellName);
        }


        /// <summary>
        /// Cast a Cleric spell using the spell name.
        /// Optionally is possible to specify the Mobile or a Serial as target of the spell. Upon successful casting, the target will be executed automatiaclly by the server.
        /// NOTE: The "automatic" target is not supported by all shards, but you can restort to the Target class to handle it manually. 
        /// </summary>
        /// <param name="SpellName">
        ///    Bark Skin : Turns the druid's skin to bark, increasing physical, poison and energy resistence while reducing fire resistence.
        ///    Circle Of Thorns : Creates a ring of thorns preventing an enemy from moving.
        ///    Deadly Spores : The enemy is afflicted by poisonous spores.
        ///    Enchanted Grove : Causes a grove of magical trees to grow, hiding the player for a short time.
        ///    Firefly : Summons a tiny firefly to light the Druid's path. The Firefly is a weak creature with little or no combat skills.
        ///    Forest Kin : Summons from a list of woodland spirits that will fight for the druid and assist him in different ways.
        ///    Grasping Roots : Summons roots from the ground to entangle a single target.
        ///    Hibernate : Causes the target to go to sleep.
        ///    Hollow Reed : Increases both the strength and the intelligence of the Druid.
        ///    Hurricane : Calls forth a violent hurricane that damages any enemies within range.
        ///    Lure Stone : Creates a magical stone that calls all nearby animals to it.
        ///    Mana Spring : Creates a magical spring that restores mana to the druid and any party members within range.
        ///    Mushroom Gateway : A magical circle of mushrooms opens, allowing the Druid to step through it to another location.
        ///    Pack Of Beasts : Summons a pack of beasts to fight for the Druid. Spell length increases with skill.
        ///    Restorative Soil : Saturates a patch of land with power, causing healing mud to seep through . The mud can restore the dead to life.
        ///    Shield Of Earth : A quick-growing wall of foliage springs up at the bidding of the Druid.
        ///    Spring Of Life : Creates a magical spring that heals the Druid and their party.
        ///    Swarm Of Insects : Summons a swarm of insects that bite and sting the Druid's enemies.
        ///    Treefellow : Summons a powerful woodland spirit to fight for the Druid.
        ///    Volcanic Eruption : A blast of molten lava bursts from the ground, hitting every enemy nearby.
        /// </param>
        /// <param name="target">Optional: Serial or Mobile to target (default: null)</param>
        /// <param name="wait">Optional: Wait server to confirm. (default: True)</param>
        public static void CastCleric(string SpellName, uint target, bool wait = true)
        {
            if (RazorEnhanced.Settings.General.ReadBool("DruidClericPackets"))
            {
                string guessedSpellName = GuessSpellName(SpellName);
                bool success = CastTargetedGeneric(m_ClericSpellName, guessedSpellName, target, wait);
                if (!success)
                    Scripts.SendMessageScriptError("Script Error: CastNecro: Invalid spell name: " + SpellName);
            }
            else
            {
                CastOnlyCleric(SpellName, wait);
            }
        }

        public static void CastCleric(string SpellName)
        {
            CastOnlyCleric(SpellName);
        }
        public static void CastCleric(string SpellName, Mobile mobile, bool wait = true)
        {
            CastCleric(SpellName, (uint)mobile.Serial, wait);
        }



        //TODO: why is this function much different from the other implementations ?  (ex: CastOnlyMastery )
        internal static void CastOnlyCleric(string SpellName, bool wait = true)
        {
            if (World.Player == null)
                return;
            string guessedSpellName = GuessSpellName(SpellName);

            string spell;
            m_ClericSpellNameText.TryGetValue(guessedSpellName, out spell);

            if (spell == null)
                Scripts.SendMessageScriptError("Script Error: CastCleric: Invalid spell name: " + SpellName);
            else
            {
                if (RazorEnhanced.Settings.General.ReadBool("DruidClericPackets"))
                {
                    bool success = CastOnlyGeneric(m_ClericSpellName, guessedSpellName, wait);
                    if (!success)
                        Scripts.SendMessageScriptError("Script Error: CastCleric: Invalid spell name: " + SpellName);
                }
                else
                {
                    Player.ChatSay(5, spell); //Dalamar: what is this ? ^_^'
                }
            }
        }


        /// <summary>
        /// Cast a Druid spell using the spell name.
        /// Optionally is possible to specify the Mobile or a Serial as target of the spell. Upon successful casting, the target will be executed automatiaclly by the server.
        /// NOTE: The "automatic" target is not supported by all shards, but you can restort to the Target class to handle it manually. 
        /// </summary>
        /// <param name="SpellName">
        ///    Angelic Faith : Turns you into an angel, boosting your stats. At 100 Spirit Speak you get +20 Str/Dex/Int. Every 5 points of SS = +1 point to each stat, at a max of +24. Will also boost your Anatomy, Mace Fighting and Healing, following the same formula.
        ///    Banish Evil : Banishes Undead targets. Auto kills rotting corpses, lich lords, etc. Works well at Doom Champ. Does not produce a corpse however
        ///    Dampen Spirit : Drains the stamina of your target, according to the description
        ///    Divine Focus : Heal for more, but may be broken.
        ///    Hammer of Faith : Summons a War Hammer with Undead Slayer on it for you
        ///    Purge : Cleanses Poison. Better than Cure
        ///    Restoration : Resurrection. Brings the target back with 100% HP/Mana
        ///    Sacred Boon : A HoT, heal over time spell, that heals 10-15 every few seconds
        ///    Sacrifice : Heals your party members when you take damage. Sort of like thorns, but it heals instead of hurts
        ///    Smite : Causes energy damage
        ///    Touch of Life : Heals even if Mortal Strike or poison are active on the target
        ///    Trial by Fire : Attackers receive damage when they strike you, sort of like a temporary RPD buff
        ///</param>
        /// <param name="target">target to use the druid spell on</param>
        /// <param name="wait"></param>

        public static void CastDruid(string SpellName, uint target, bool wait = true)
        {
            string guessedSpellName = GuessSpellName(SpellName);
            if (RazorEnhanced.Settings.General.ReadBool("DruidClericPackets"))
            {
                bool success = CastTargetedGeneric(m_DruidSpellName, guessedSpellName, target, wait);
                if (!success)
                    Scripts.SendMessageScriptError("Script Error: CastNecro: Invalid spell name: " + SpellName);
            }
            else
            {
                CastOnlyDruid(guessedSpellName, wait);
            }
        }

        public static void CastDruid(string SpellName, Mobile mobile, bool wait = true)
        {
            CastDruid(SpellName, (uint)mobile.Serial, wait);
        }


        public static void CastDruid(string SpellName)
        {
            CastOnlyDruid(SpellName);
        }

        //TODO: why is this function much different from the other implementations ?  (ex: CastOnlyMastery )
        internal static void CastOnlyDruid(string SpellName, bool wait = true)
        {
            if (World.Player == null)
                return;

            string guessedSpellName = GuessSpellName(SpellName);
            string spell;
            m_DruidSpellNameText.TryGetValue(guessedSpellName, out spell);

            if (spell == null)
                Scripts.SendMessageScriptError("Script Error: CastDruid: Invalid spell name: " + SpellName);
            else
            {
                if (RazorEnhanced.Settings.General.ReadBool("DruidClericPackets"))
                {
                    bool success = CastOnlyGeneric(m_DruidSpellName, guessedSpellName, wait);
                    if (!success)
                        Scripts.SendMessageScriptError("Script Error: CasDruid: Invalid spell name: " + SpellName);
                }
                else
                {
                    Player.ChatSay(8, spell); //Dalamar: what is this ? ^_^'
                }
            }
        }

        /// <summary>
        /// Interrupt the casting of a spell by performing an equip/unequip.
        /// </summary>
        public static void Interrupt()
        {
            Assistant.Item item = FindUsedLayer();
            if (item != null)
            {
                Assistant.Client.Instance.SendToServerWait(new LiftRequest(item, 1));
                Assistant.Client.Instance.SendToServerWait(new EquipRequest(item.Serial, Assistant.World.Player, item.Layer)); // Equippa
            }
        }


        /// <summary>
        /// Cast again the last casted spell, on last target.
        /// </summary>
        /// <param name="target">Optional: Serial or Mobile to target (default: null)</param>
        /// <param name="wait">Optional: Wait server to confirm. (default: True)</param>
        public static void CastLastSpell(uint target, bool wait = true)
        {
            if (World.Player.LastSpell != 0)
            {
                Spell s = Spell.Get(World.Player.LastSpell);

                if (s != null)
                    s.OnCast(new CastTargetedSpell((ushort)s.GetID(), target), wait);
            }
        }

        public static void CastLastSpell(Mobile m, bool wait = true)
        {
            CastLastSpell((uint)m.Serial, wait);
        }

        public static void CastLastSpell(bool wait=true)
        {
            if (World.Player.LastSpell != 0)
            {
                Spell s = Spell.Get(World.Player.LastSpell);

                if (s != null)
                    s.OnCast(new CastSpellFromMacro((ushort)s.GetID()), wait);
            }
        }


        /// <summary>
        /// Cast again the last casted spell, on last target.
        /// </summary>
        public static void CastLastSpellLastTarget()
        {
            int lastTarget = Target.GetLast();
            Mobile m = Mobiles.FindBySerial(lastTarget);
            if (m != null)
            {
                CastLastSpell((uint)lastTarget, false);
            }
            else
            {
                Scripts.SendMessageScriptError("Last Target no longer exists");
            }
        }


        

        internal static Assistant.Item FindUsedLayer()
        {
            Assistant.Item layeritem = Assistant.World.Player.GetItemOnLayer(Layer.Shoes);
            if (layeritem != null)
                return layeritem;

            layeritem = Assistant.World.Player.GetItemOnLayer(Layer.Pants);
            if (layeritem != null)
                return layeritem;

            layeritem = Assistant.World.Player.GetItemOnLayer(Layer.Shirt);
            if (layeritem != null)
                return layeritem;

            layeritem = Assistant.World.Player.GetItemOnLayer(Layer.Head);
            if (layeritem != null)
                return layeritem;

            layeritem = Assistant.World.Player.GetItemOnLayer(Layer.Gloves);
            if (layeritem != null)
                return layeritem;

            layeritem = Assistant.World.Player.GetItemOnLayer(Layer.Ring);
            if (layeritem != null)
                return layeritem;

            layeritem = Assistant.World.Player.GetItemOnLayer(Layer.Neck);
            if (layeritem != null)
                return layeritem;

            layeritem = Assistant.World.Player.GetItemOnLayer(Layer.Waist);
            if (layeritem != null)
                return layeritem;

            layeritem = Assistant.World.Player.GetItemOnLayer(Layer.InnerTorso);
            if (layeritem != null)
                return layeritem;

            layeritem = Assistant.World.Player.GetItemOnLayer(Layer.Bracelet);
            if (layeritem != null)
                return layeritem;

            layeritem = Assistant.World.Player.GetItemOnLayer(Layer.MiddleTorso);
            if (layeritem != null)
                return layeritem;

            layeritem = Assistant.World.Player.GetItemOnLayer(Layer.Earrings);
            if (layeritem != null)
                return layeritem;

            layeritem = Assistant.World.Player.GetItemOnLayer(Layer.Arms);
            if (layeritem != null)
                return layeritem;

            layeritem = Assistant.World.Player.GetItemOnLayer(Layer.Cloak);
            if (layeritem != null)
                return layeritem;

            layeritem = Assistant.World.Player.GetItemOnLayer(Layer.OuterTorso);
            if (layeritem != null)
                return layeritem;

            layeritem = Assistant.World.Player.GetItemOnLayer(Layer.OuterLegs);
            if (layeritem != null)
                return layeritem;

            layeritem = Assistant.World.Player.GetItemOnLayer(Layer.InnerLegs);
            if (layeritem != null)
                return layeritem;

            layeritem = Assistant.World.Player.GetItemOnLayer(Layer.RightHand);
            if (layeritem != null)
                return layeritem;

            layeritem = Assistant.World.Player.GetItemOnLayer(Layer.LeftHand);
            if (layeritem != null)
                return layeritem;

            return null;
        }

        //////////////////////////////////////////////////////////////
        // Spell Dictionaries
        //////////////////////////////////////////////////////////////

        private static readonly Dictionary<string, int> m_MagerySpellName = new Dictionary<string, int>
        {
			// circle 1 magery
			{ "Clumsy", 1 },
            { "Create Food", 2 },
            { "Feeblemind", 3 },
            { "Heal", 4 },
            { "Magic Arrow", 5 },
            { "Night Sight", 6 },
            { "Reactive Armor", 7 },
            { "Weaken", 8 },

			// circle 2 magery
			{ "Agility", 9 },
            { "Cunning", 10 },
            { "Cure", 11 },
            { "Harm", 12 },
            { "Magic Trap", 13 },
            { "Magic Untrap", 14 },
            { "Protection", 15 },
            { "Strength", 16 },

			// circle 3 magery
			{ "Bless", 17 },
            { "Fireball", 18 },
            { "Magic Lock", 19 },
            { "Poison", 20 },
            { "Telekinesis", 21 },
            { "Teleport", 22 },
            { "Unlock", 23 },
            { "Wall of Stone", 24 },

			// circle 4 magery
			{ "Arch Cure", 25 },
            { "Arch Protection", 26 },
            { "Curse", 27 },
            { "Fire Field", 28 },
            { "Greater Heal", 29 },
            { "Lightning", 30 },
            { "Mana Drain", 31 },
            { "Recall", 32 },

			// circle 5 magery
			{ "Blade Spirits", 33 },
            { "Dispel Field", 34 },
            { "Incognito", 35 },
            { "Magic Reflection", 36 },
            { "Mind Blast", 37 },
            { "Paralyze", 38 },
            { "Poison Field", 39 },
            { "Summon Creature", 40 },

			// circle 6 magery
			{ "Dispel", 41 },
            { "Energy Bolt", 42 },
            { "Explosion", 43 },
            { "Invisibility", 44 },
            { "Mark", 45 },
            { "Mass Curse", 46 },
            { "Paralyze Field", 47 },
            { "Reveal", 48 },

			// circle 7 magery
			{ "Chain Lightning", 49 },
            { "Energy Field", 50 },
            { "Flamestrike", 51 },
            { "Gate Travel", 52 },
            { "Mana Vampire", 53 },
            { "Mass Dispel", 54 },
            { "Meteor Swarm", 55 },
            { "Polymorph", 56 },

			// circle 8 magery
			{ "Earthquake", 57 },
            { "Energy Vortex", 58 },
            { "Resurrection", 59 },
            { "Summon Air Elemental", 60 },
            { "Air Elemental", 60 },
            { "Summon Daemon", 61 },
            { "Summon Earth Elemental", 62 },
            { "Earth Elemental", 62 },
            { "Summon Fire Elemental", 63 },
            { "Fire Elemental", 63 },
            { "Water Elemental", 64 },
            { "Summon Water Elemental", 64 },
            { "Ice Blast", 65 },
            { "Lightning Wave", 66 },
            { "Dust Storm", 67 },
            { "Mass Calm", 68 },
            { "Protean", 69},
            { "Poison Dart", 70},
            { "Void Matter", 71},
            { "Tidal Rush", 72},
            { "Crushing Boulder", 73},
            { "Noxious Fumes", 74},
        };

		private static readonly Dictionary<string, int> m_NecroSpellName = new Dictionary<string, int>
		{
			{ "Animate Dead", 101 },
			{ "Blood Oath", 102 },
			{ "Corpse Skin", 103 },
			{ "Curse Weapon", 104 },
			{ "Evil Omen", 105 },
			{ "Horrific Beast", 106 },
			{ "Lich Form", 107 },
			{ "Mind Rot", 108 },
			{ "Pain Spike", 109 },
			{ "Poison Strike", 110 },
			{ "Strangle", 111 },
			{ "Summon Familiar", 112 },
			{ "Vampiric Embrace", 113 },
			{ "Vengeful Spirit", 114 },
			{ "Wither", 115 },
			{ "Wraith Form", 116 },
			{ "Exorcism", 117 }
		};

		private static readonly Dictionary<string, int> m_ChivalrySpellName = new Dictionary<string, int>
		{
			{ "Cleanse By Fire", 201 },
			{ "Close Wounds", 202 },
			{ "Consecrate Weapon", 203 },
			{ "Dispel Evil", 204 },
			{ "Divine Fury", 205 },
			{ "Enemy Of One", 206 },
			{ "Holy Light", 207 },
			{ "Noble Sacrifice", 208 },
			{ "Remove Curse", 209 },
			{ "Sacred Journey", 210 }
		};

		private static readonly Dictionary<string, int> m_BushidoSpellName = new Dictionary<string, int>
		{
			{ "Honorable Execution", 401 },
			{ "Confidence", 402 },
			{ "Evasion", 403 },
			{ "Counter Attack", 404 },
			{ "Lightning Strike", 405 },
			{ "Momentum Strike", 406 }
		};

		private static readonly Dictionary<string, int> m_NinjitsuSpellName = new Dictionary<string, int>
		{
			{ "Focus Attack", 501 },
			{ "Death Strike", 502 },
			{ "Animal Form", 503 },
			{ "Ki Attack", 504 },
			{ "Surprise Attack", 505 },
			{ "Backstab", 506 },
			{ "Shadow jump", 507 },  // Keep old compaibility whit old script
			{ "Shadowjump", 507 },
			{ "Mirror Image", 508 },
		};

		private static readonly Dictionary<string, int> m_SpellweavingSpellName = new Dictionary<string, int>
		{
			{ "Arcane Circle", 601 },
			{ "Gift Of Renewal", 602 },
			{ "Immolating Weapon", 603 },
			{ "Attunement", 604 },
			{ "Attune Weapon", 604 }, // Keep old compaibility whit old script
			{ "Thunderstorm", 605 },
			{ "Natures Fury", 606 },
			{ "Summon Fey", 607 },
			{ "Summon Fiend", 608 },
			{ "Reaper Form", 609 },
			{ "Wildfire", 610 },
			{ "Essence Of Wind", 611 },
			{ "Dryad Allure", 612 },
			{ "Ethereal Voyage", 613 },
			{ "Word Of Death", 614 },
			{ "Gift Of Life", 615 },
			{ "Arcane Empowerment", 616 }
		};

		private static readonly Dictionary<string, int> m_MysticismSpellName = new Dictionary<string, int>
		{
			{ "Nether Bolt", 678 },
			{ "Healing Stone", 679 },
			{ "Purge Magic", 680 },
			{ "Enchant", 681 },
			{ "Sleep", 682 },
			{ "Eagle Strike", 683 },
			{ "Animated Weapon", 684 },
			{ "Stone Form", 685 },
			{ "Spell Trigger", 686 },
			{ "Mass Sleep", 687 },
			{ "Cleansing Winds", 688 },
			{ "Bombard", 689 },
			{ "Spell Plague", 690 },
			{ "Hail Storm", 691 },
			{ "Nether Cyclone", 692 },
			{ "Rising Colossus", 693 },
		};

		private static readonly Dictionary<string, int> m_MasterySpellName = new Dictionary<string, int>
		{
			{ "Inspire", 701 },
			{ "Invigorate", 702 },
			{ "Resilience", 703 },
			{ "Perseverance", 704 },
			{ "Tribulation", 705 },
			{ "Despair", 706 },
			{ "Death Ray", 707 },
			{ "Ethereal Blast", 708 },
			{ "Nether Blast", 709 },
			{ "Mystic Weapon", 710 },
			{ "Command Undead", 711 },
			{ "Conduit", 712 },
			{ "Mana Shield", 713 },
			{ "Summon Reaper", 714 },
			{ "Enchanted Summoning", 715 },
			{ "Anticipate Hit", 716 },
			{ "Warcry", 717 },
			{ "Intuition", 718 },
			{ "Rejuvenate", 719 },
			{ "Holy Fist", 720 },
			{ "Shadow", 721 },
			{ "White Tiger Form", 722 },
			{ "Flaming Shot", 723 },
			{ "Playing The Odds", 724 },
			{ "Thrust", 725 },
			{ "Pierce", 726 },
			{ "Stagger", 727 },
			{ "Toughness", 728 },
			{ "Onslaught", 729 },
			{ "Focused Eye", 730 },
			{ "Elemental Fury", 731 },
			{ "Called Shot", 732 },
			{ "Saving Throw", 733 },
			{ "Shield Bash", 734 },
			{ "Bodyguard", 735 },
			{ "Heighten Senses", 736 },
			{ "Tolerance", 737 },
			{ "Injected Strike", 738 },
			{ "Potency", 739 },
			{ "Rampage", 740 },
			{ "Fists Of Fury", 741 },
			{ "Knockout", 742 },
			{ "Whispering", 743 },
			{ "Combat Training", 744 },
			{ "Boarding", 745 },
		};

        private static readonly Dictionary<string, int> m_ClericSpellName = new Dictionary<string, int>
        {
            { "Angelic Faith", 342 },
            {"Banish Evil", 343 },
            { "Dampen Spirit", 344 },
            { "Divine Focus", 345 },
            { "Hammer of Faith", 346 },
            { "Purge", 347 },
            { "Restoration", 348 },
            { "Sacred Boon", 349 },
            { "Sacrifice", 350 },
            { "Smite", 351 },
            { "Touch of Life", 352 },
            { "Trial by Fire", 353 },
        };
        private static readonly Dictionary<string, string> m_ClericSpellNameText = new Dictionary<string, string>
        {
            { "Angelic Faith", "[cs AngelicFaith" },
            {"Banish Evil", "[cs BanishEvil" },
            { "Dampen Spirit", "[cs DampenSpirit" },
            { "Divine Focus", "[cs DivineFocus" },
            { "Hammer of Faith", "[cs HammerofFaith" },
            { "Purge", "[cs Purge" },
            { "Restoration", "[cs Restoration" },
            { "Sacred Boon", "[cs SacredBoon" },
            { "Sacrifice", "[cs Sacrifice" },
            { "Smite", "[cs Smite" },
            { "Touch of Life", "[cs TouchofLife" },
            { "Trial by Fire", "[cs TrialbyFire" },
        };

        private static readonly Dictionary<string, int> m_DruidSpellName = new Dictionary<string, int>
        {
            { "Shield of Earth", 302 },
            { "Hollow Reed", 303 },
            { "Pack of Beast", 304 },
            { "Spring of Life", 305 },
            { "Grasping Roots", 306 },
            { "Circle of Thorns", 307 },
            { "Swarm of Insects", 308 },
            { "Volcanic Eruption", 309 },
            { "Treefellow", 310 },
            { "Deadly Spores", 311 },
            { "Enchanted Grove", 312 },
            { "Lure Stone", 313 },
            { "Hurricane", 314 },
            { "Mushroom Gateway", 315 },
            { "Restorative Soil", 316 },
            { "FireFly", 317 },
            { "Forest Kin", 318 },
            { "BarkSkin", 319 },
            { "ManaSpring", 320 },
            { "Hibernate", 321 },
        };

        private static readonly Dictionary<string, string> m_DruidSpellNameText = new Dictionary<string, string>
        {
            { "Shield of Earth", "[cs ShieldofEarth" },
            { "Hollow Reed", "[cs HollowReed" },
            { "Pack of Beasts", "[cs PackofBeasts" },
            { "Spring of Life", "[cs SpringofLife" },
            { "Grasping Roots", "[cs graspingroots" },
            { "Circle of Thorns", "[cs circleofthorns" },
            { "Swarm of Insects", "[cs SwarmofInsects" },
            { "Volcanic Eruption", "[cs VolcanicEruption" },
            { "Treefellow", "[cs treefellow" },
            { "Deadly Spores", "[cs deadlyspores" },
            { "Enchanted Grove", "[cs EnchantedGrove" },
            { "Lure Stone", "[cs LureStone" },
            { "Hurricane", "[cs Hurricane" },
            { "Mushroom Gateway", "[cs MushroomGateway" },
            { "Restorative Soil", "[cs RestorativeSoil" },
            { "FireFly", "[cs firefly" },
            { "Forest Kin", "[cs forestkin" },
            { "BarkSkin", "[cs barkskin" },
            { "ManaSpring", "[cs manaspring" },
            { "Hibernate", "[cs hibernate" },
        };
        private static readonly Dictionary<string, int> m_AllSpells = AllSpells();

        private static Dictionary<string, int> AllSpells()
        {
            Dictionary<string, int> allSpells = new Dictionary<string, int>();

            foreach (var entry in m_MagerySpellName)
            {
                allSpells.Add(entry.Key, entry.Value);
            }
            foreach (var entry in m_NecroSpellName)
            {
                allSpells.Add(entry.Key, entry.Value);
            }
            foreach (var entry in m_ChivalrySpellName)
            {
                allSpells.Add(entry.Key, entry.Value);
            }
            foreach (var entry in m_BushidoSpellName)
            {
                allSpells.Add(entry.Key, entry.Value);
            }
            foreach (var entry in m_NinjitsuSpellName)
            {
                // because ninjitsu list has 2 entries for same spell
                string tempKey = entry.Key;
                if (!allSpells.ContainsKey(tempKey))
                    allSpells.Add(tempKey, entry.Value);
            }
            foreach (var entry in m_SpellweavingSpellName)
            {
                allSpells.Add(entry.Key, entry.Value);
            }
            foreach (var entry in m_MysticismSpellName)
            {
                allSpells.Add(entry.Key, entry.Value);
            }
            foreach (var entry in m_MasterySpellName)
            {
                allSpells.Add(entry.Key, entry.Value);
            }
            foreach (var entry in m_ClericSpellName)
            {
                allSpells.Add(entry.Key, entry.Value);
            }
            foreach (var entry in m_DruidSpellName)
            {
                allSpells.Add(entry.Key, entry.Value);
            }

            return allSpells;
        }

    }
}
