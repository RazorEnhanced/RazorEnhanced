using Assistant;
using System.Collections.Generic;

namespace RazorEnhanced
{
    internal class Skills
    {
        internal static Dictionary<int, string> m_SkillNameById = null;
        internal static Dictionary<string, int> m_SkillNameByName = null;

        internal static int GetSkillId(string skillName)
        {
            if (m_SkillNameByName.ContainsKey(skillName))
            {
                return m_SkillNameByName[skillName];
            }
            return -1;
        }

        internal static string GetSkillName(int skillId)
        {
            if (m_SkillNameById.ContainsKey(skillId))
            {
                return m_SkillNameById[skillId];
            }
            return null;
        }

        internal static void InitData()
        {
            var defaults = new List<(int, string)>()
            {
                (0, "Alchemy"),
                (1, "Anatomy"),
                (2, "AnimalLore"),
                (3, "ItemID"),
                (4, "ArmsLore"),
                (5, "Parry"),
                (6, "Begging"),
                (7, "Blacksmith"),
                (8, "Fletching"),
                (9, "Peacemaking"),
                (10, "Camping"),
                (11, "Carpentry"),
                (12, "Cartography"),
                (13, "Cooking"),
                (14, "DetectHidden"),
                (15, "Discordance"),
                (16, "EvalInt"),
                (17, "Healing"),
                (18, "Fishing"),
                (19, "Forensics"),
                (20, "Herding"),
                (21, "Hiding"),
                (22, "Provocation"),
                (23, "Inscribe"),
                (24, "Lockpicking"),
                (25, "Magery"),
                (26, "MagicResist"),
                (27, "Tactics"),
                (28, "Snooping"),
                (29, "Musicianship"),
                (30, "Poisoning"),
                (31, "Archery"),
                (32, "SpiritSpeak"),
                (33, "Stealing"),
                (34, "Tailoring"),
                (35, "AnimalTaming"),
                (36, "TasteID"),
                (37, "Tinkering"),
                (38, "Tracking"),
                (39, "Veterinary"),
                (40, "Swords"),
                (41, "Macing"),
                (42, "Fencing"),
                (43, "Wrestling"),
                (44, "Lumberjacking"),
                (45, "Mining"),
                (46, "Meditation"),
                (47, "Stealth"),
                (48, "RemoveTrap"),
                (49, "Necromancy"),
                (50, "Focus"),
                (51, "Chivalry"),
                (52, "Bushido"),
                (53, "Ninjitsu"),
                (54, "SpellWeaving"),
                (55, "Mysticism"),
                (56, "Imbuing"),
                (57, "Throwing"),
            };

            m_SkillNameById = new Dictionary<int, string>();
            m_SkillNameByName = new Dictionary<string, int>();

            // Add the standard entries by id so no duplicates
            foreach (var entry in defaults)
            {
                m_SkillNameById.Add(entry.Item1, entry.Item2);
            }
            // add the ones from skills.mul data files (does nothing if already exists)
            foreach (var skill in Ultima.Skills.SkillEntries)
            {
                m_SkillNameById[skill.Index] = skill.Name;
            }

            // Use the resulting dictionary to populate the inverse lookup
            foreach (var entry in m_SkillNameById)
            {
                if (!m_SkillNameByName.ContainsKey(entry.Value))
                    m_SkillNameByName.Add(entry.Value, entry.Key);
            }
        }

        internal static string GuessSkillName(string originalName)
        {
            int distance = 99;
            string closest = "";

            foreach (var skill in m_SkillNameById)
            {
                int computeDistance = UOAssist.LevenshteinDistance(skill.Value, originalName);
                if (computeDistance < distance)
                {
                    distance = computeDistance;
                    closest = skill.Value;
                }
            }

            if (distance < 99)
                return closest;
            return originalName;

        }

        internal static int GuessSkillId(string originalName)
        {
            int distance = 99;
            int closest = -1;

            foreach (var skill in m_SkillNameById)
            {
                int computeDistance = UOAssist.LevenshteinDistance(skill.Value, originalName);
                if (computeDistance < distance)
                {
                    distance = computeDistance;
                    closest = skill.Key;
                }
            }

            if (distance < 99)
                return closest;
            return GetSkillId(originalName);

        }
    }
}
