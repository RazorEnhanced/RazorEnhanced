using System;
using System.Collections.Generic;
using Assistant;

namespace RazorEnhanced
{
	public class Items
	{
        public static Item FindBySerial(int serial)
        {

            Assistant.Item assistantItem = Assistant.World.FindItem((Assistant.Serial)((uint)serial));
			if (assistantItem == null)
			{
				Player.SendMessage("Script Error: FindBySerial: Item serial: (" + serial + ") not found");
				return null;
			}
			else
			{
				RazorEnhanced.Item enhancedItem = new RazorEnhanced.Item(assistantItem);
				return enhancedItem;
			}
        }
        public static void Move(Item item, Item bag, int amount)
        {
            if (item == null)
            {
                Player.SendMessage("Script Error: Move: Source Item  not found");
                return;
            }
            if (bag == null)
            {
                Player.SendMessage("Script Error: Move: Destination Item not found");
                return;
            }
            if (!bag.IsContainer)
            {
                Player.SendMessage("Script Error: Move: Destination Item is not a container");
                return;
            }
            if (amount == 0)
            {
                Assistant.ClientCommunication.SendToServer(new LiftRequest(item.Serial, item.Amount));
				Assistant.ClientCommunication.SendToServer(new DropRequest(item.Serial, Assistant.Point3D.MinusOne, bag.Serial));
            }
            else
            {
                if (item.Amount < amount)
                {
                    amount = item.Amount;
                }
                Assistant.ClientCommunication.SendToServer(new LiftRequest(item.Serial, amount));
				Assistant.ClientCommunication.SendToServer(new DropRequest(item.Serial, Assistant.Point3D.MinusOne, bag.Serial));
            }
        }

        public static void DropItemGroundSelf(Item item, int amount)
        {
            if (item == null)
            {
                Player.SendMessage("Script Error: DropItemGroundSelf: Item not found");
                return;
            }
            if (amount == 0)
            {
                Assistant.ClientCommunication.SendToServer(new LiftRequest(item.Serial, item.Amount));
                Assistant.ClientCommunication.SendToServer(new DropRequest(item.Serial, World.Player.Position, Serial.Zero));
            }
            else
            {
                if (item.Amount < amount)
                {
                    amount = item.Amount;
                }
                Assistant.ClientCommunication.SendToServer(new LiftRequest(item.Serial, amount));
                Assistant.ClientCommunication.SendToServer(new DropRequest(item.Serial, World.Player.Position, Serial.Zero));
            }
        }
        public static void UseItem(Item item)
        {
            if (item.Serial.IsItem)
                Assistant.ClientCommunication.SendToServer(new DoubleClick(item.Serial));
            else
                Player.SendMessage("Script Error: UseItem: (" + item.Serial.ToString() + ") is not a item");
        }
        public static void UseItem(uint itemserial)
        {
            Assistant.Item item = Assistant.World.FindItem(itemserial);
            if (item.Serial.IsItem)
                Assistant.ClientCommunication.SendToServer(new DoubleClick(item.Serial));
            else
                Player.SendMessage("Script Error: UseItem: (" + item.Serial.ToString() + ") is not a item");
            
        }
        public static void UseItemByID(UInt16 ItemID)                   // Da verificare il findbyid dove cerca
        {
            Assistant.Item item = World.Player.FindItemByID(ItemID);
            if (item.Serial.IsItem)
            {
                Player.SendMessage("Script Error: UseItemByID: No item whit ID:("+ ItemID.ToString() +") found!");
                return;
            }
            if (item.Serial.IsItem)
                Assistant.ClientCommunication.SendToServer(new DoubleClick(item.Serial));
            else
                Player.SendMessage("Script Error: UseItem: (" + item.Serial.ToString() + ") is not a item"); 
        }

        // Props
        public static int GetPropByCliloc(RazorEnhanced.Item item, int code)
        {
            if (item.Serial.IsItem)
            {
                return GetPropExec(item, code, "GetPropByCliloc");
            }
            else
            {
                Player.SendMessage("Script Error: GetPropByCliloc: (" + item.Serial.ToString() + ") is not a item");
                return 0;
            }
        }
        public static int GetPropByString(RazorEnhanced.Item item, string props)
        {
            if (item.Serial.IsItem)
            {
                switch (props)
                {
                    case "Balanced":
                        return GetPropExec(item, 1072792, "GetPropByString");
                    case "Cold Resist":
                        return GetPropExec(item, 1060445, "GetPropByString");
                    case "Damage Increase":
                        {
                            if (GetPropExec(item, 1060401, "GetPropByString") != 0)
                                return GetPropExec(item, 1060401, "GetPropByString");
                            return GetPropExec(item, 1060402, "GetPropByString");
                        }
                    case "Defense Chance Increase":
                        return GetPropExec(item, 1060408, "GetPropByString");
                    case "Dexterity Bonus":
                        return GetPropExec(item, 1060409, "GetPropByString");
                    case "Energy Resist":
                        return GetPropExec(item, 1060446, "GetPropByString");
                    case "Faster Cast Recovery":
                        return GetPropExec(item, 1060412, "GetPropByString");
                    case "Enhance Potion":
                        return GetPropExec(item, 1060411, "GetPropByString");
                    case "Energy Damage":
                        return GetPropExec(item, 1060407, "GetPropByString");
                    case "Poison Damage":
                        return GetPropExec(item, 1060406, "GetPropByString");
                    case "Fire Damage":
                        return GetPropExec(item, 1060405, "GetPropByString");
                    case "Cold Damage":
                        return GetPropExec(item, 1060404, "GetPropByString");
                    case "Physical Damage":
                        return GetPropExec(item, 1060403, "GetPropByString");
                    case "Faster Casting":
                        return GetPropExec(item, 1060413, "GetPropByString");
                    case "Gold Increase":
                        return GetPropExec(item, 1060414, "GetPropByString");
                    case "Fire Resist":
                        return GetPropExec(item, 1060447, "GetPropByString");
                    case "Hit Chance Increase":
                        return GetPropExec(item, 1060415, "GetPropByString");
                    case "Hit Energy Area":
                        return GetPropExec(item, 1060418, "GetPropByString");
                    case "Hit Dispel":
                        return GetPropExec(item, 1060417, "GetPropByString");
                    case "Hit Cold Area":
                        return GetPropExec(item, 1060416, "GetPropByString");
                    case "Hit Fire Area":
                        return GetPropExec(item, 1060419, "GetPropByString");
                    case "Hit Fireball":
                        return GetPropExec(item, 1060420, "GetPropByString");
                    case "Hit Life Leech":
                        return GetPropExec(item, 1060422, "GetPropByString");
                    case "Hit Point Increase":
                        return GetPropExec(item, 1060431, "GetPropByString");
                    case "Hit Point Regeneration":
                        return GetPropExec(item, 1060444, "GetPropByString");
                    case "Hit Stamina Leech":
                        return GetPropExec(item, 1060430, "GetPropByString");
                    case "Hit Poison Area":
                        return GetPropExec(item, 1060429, "GetPropByString");
                    case "Hit Physical Area":
                        return GetPropExec(item, 1060428, "GetPropByString");
                    case "Hit Mana Leech":
                        return GetPropExec(item, 1060427, "GetPropByString");
                    case "Hit Magic Arrow":
                        return GetPropExec(item, 1060426, "GetPropByString");
                    case "Hit Lower Defence":
                        return GetPropExec(item, 1060425, "GetPropByString");
                    case "Hit Lower Attack":
                        return GetPropExec(item, 1060424, "GetPropByString");
                    case "Hit Lightning":
                        return GetPropExec(item, 1060423, "GetPropByString");
                    case "Hit Harm":
                        return GetPropExec(item, 1060421, "GetPropByString");
                    case "Intelligence Bonus":
                        return GetPropExec(item, 1060432, "GetPropByString");
                    case "Lower Mana Cost":
                        return GetPropExec(item, 1060433, "GetPropByString");
                    case "Lower Reagent Cost":
                        return GetPropExec(item, 1060434, "GetPropByString");
                    case "Lower Requirements":
                        return GetPropExec(item, 1060435, "GetPropByString");
                    case "Luck":
                        return GetPropExec(item, 1060436, "GetPropByString");
                    case "Mana Increase":
                        return GetPropExec(item, 1060439, "GetPropByString");
                    case "Mana Regeneration":
                        return GetPropExec(item, 1060440, "GetPropByString");
                    case "Physical Resist":
                        return GetPropExec(item, 1060448, "GetPropByString");
                    case "Poison Resist":
                        return GetPropExec(item, 1060449, "GetPropByString");
                    case "Nighr Sight":
                        return GetPropExec(item, 1060441, "GetPropByString");
                    case "Spell Channeling":
                        return GetPropExec(item, 1060482, "GetPropByString");
                    case "Spell Damage Increase":
                        return GetPropExec(item, 1060483, "GetPropByString");
                    case "Splintering Weapon":
                        return GetPropExec(item, 1112857, "GetPropByString");
                    case "Stamina Increase":
                        return GetPropExec(item, 1060484, "GetPropByString");
                    case "Stamina Regeneration":
                        return GetPropExec(item, 1060443, "GetPropByString");
                    case "Swing Speed Increase":
                        return GetPropExec(item, 1060486, "GetPropByString");
                    case "Velocity":
                        return GetPropExec(item, 1072792, "GetPropByString");
                    case "Self Repair":
                        return GetPropExec(item, 1060450, "GetPropByString");
                    case "Reflect Physical Damage":
                        return GetPropExec(item, 1060442, "GetPropByString");
                    case "Night Sight":
                        return GetPropExec(item, 1060441, "GetPropByString");
                    case "Mage Armor":
                        return GetPropExec(item, 1060437, "GetPropByString");
                    case "Strenght Bonus":
                        return GetPropExec(item, 1060485, "GetPropByString");
                    case "Water Elemental Slayer":
                        return GetPropExec(item, 1060482, "GetPropByString");
                    case "Troll Slayer":
                        return GetPropExec(item, 1060480, "GetPropByString");
                    case "Undead Slayer":
                        return GetPropExec(item, 1060479, "GetPropByString");
                    case "Terathan Slayer":
                        return GetPropExec(item, 1060478, "GetPropByString");
                    case "Spider Slayer":
                        return GetPropExec(item, 1060477, "GetPropByString");
                    case "Snow Elemental Slayer":
                        return GetPropExec(item, 1060476, "GetPropByString");
                    case "Snake Slayer":
                        return GetPropExec(item, 1060475, "GetPropByString");
                    case "Scorpion Slayer":
                        return GetPropExec(item, 1060474, "GetPropByString");
                    case "Reptile Slayer":
                        return GetPropExec(item, 1060473, "GetPropByString");
                    case "Repond Slayer":
                        return GetPropExec(item, 1060472, "GetPropByString");
                    case "Poison Elemental Slayer":
                        return GetPropExec(item, 1060471, "GetPropByString");
                    case "Orc Slayer":
                        return GetPropExec(item, 1060470, "GetPropByString");
                    case "Ophidian Slayer":
                        return GetPropExec(item, 1060469, "GetPropByString");
                    case "Ogre Slayer":
                        return GetPropExec(item, 1060468, "GetPropByString");
                    case "Lizardman Slayer":
                        return GetPropExec(item, 1060467, "GetPropByString");
                    case "Gargoyle Slayer":
                        return GetPropExec(item, 1060466, "GetPropByString");
                    case "Fire Elemental Slayer":
                        return GetPropExec(item, 1060465, "GetPropByString");
                    case "Elemental Slayer":
                        return GetPropExec(item, 1060464, "GetPropByString");
                    case "Earth Elemental Slayer":
                        return GetPropExec(item, 1060463, "GetPropByString");
                    case "Dragon Slayer":
                        return GetPropExec(item, 1060462, "GetPropByString");
                    case "Demon Slayer":
                        {
                            if (GetPropExec(item, 1060460, "GetPropByString") != 0)
                                return GetPropExec(item, 1060460, "GetPropByString");
                            return GetPropExec(item, 1060461, "GetPropByString");
                        }
                    case "Blood Elemental Slayer":
                        return GetPropExec(item, 1060459, "GetPropByString");
                    case "Arachnid Slayer":
                        return GetPropExec(item, 1060458, "GetPropByString");
                    case "Air Elemental Slayer":
                        return GetPropExec(item, 1060457, "GetPropByString");
                    case "Magic Arrow Charges":
                        return GetPropExec(item, 1060492, "GetPropByString");
                    case "Lightning Charges":
                        return GetPropExec(item, 1060491, "GetPropByString");
                    case "Healing Charges":
                        return GetPropExec(item, 1060490, "GetPropByString");
                    case "Harm Charges":
                        return GetPropExec(item, 1060489, "GetPropByString");
                    case "Greater Healing Charges":
                        return GetPropExec(item, 1060488, "GetPropByString");
                    case "Fireball Charges":
                        return GetPropExec(item, 1060487, "GetPropByString");
                    default:
                        Player.SendMessage("Script Error: GetPropByString: Invalid or not supported props string");
                        return 0;
                }

            }
            else
            {
                Player.SendMessage("Script Error: GetPropByCliloc: (" + item.Serial.ToString() + ") is not a item");
                return 0;
            }
        }
        private static int GetPropExec(RazorEnhanced.Item item, int code, String Fcall)
        {
            Assistant.ObjectPropertyList ItemOPL = item.Properties;
            for (int i = 0; i < ItemOPL.Content.Count; i++)
            {
                Assistant.ObjectPropertyList.OPLEntry ent = (Assistant.ObjectPropertyList.OPLEntry)ItemOPL.Content[i];
                int number = ent.Number;
                string args = ent.Args;
                if (number == code)
                {
                    if (args == null)  // Esiste prop ma senza valore
                        return 1;
                    else
                    {
                        try
                        {
                            return Convert.ToInt32(args);  // Ritorna valore
                        }
                        catch
                        {
                            Player.SendMessage("Script Error: " + Fcall + ": Error to get value of Cliloc:" + code);
                            return 0;  // errore di conversione
                        }
                    }
                }
            }
            return 0;       // Prop inesistente sul item
        }

        
        
	}
}
