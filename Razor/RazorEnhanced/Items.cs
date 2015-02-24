using System;
using System.Collections.Generic;
using Assistant;

namespace RazorEnhanced
{
	public class Items
	{
        public static Assistant.Item FindBySerial(uint itemserial)
        {
            Assistant.Item item = Assistant.World.FindItem(itemserial);
            if (item == null)
            {
                Player.SendMessage("Script Error: FindBySerial: Item serial: (" + itemserial + ") not found");
                return null;
            }
            else
                return item;
        }
        public static void Move(Assistant.Item item, Assistant.Item bag, int amount)
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
                Assistant.ClientCommunication.SendToServer(new DropRequest(item.Serial, Point3D.MinusOne, bag.Serial));
            }
            else
            {
                if (item.Amount < amount)
                {
                    amount = item.Amount;
                }
                Assistant.ClientCommunication.SendToServer(new LiftRequest(item.Serial, amount));
                Assistant.ClientCommunication.SendToServer(new DropRequest(item.Serial, Point3D.MinusOne, bag.Serial));
            }
        }

        public static void DropItemGroundSelf(Assistant.Item item, int amount)
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
        public static void UseItem(Assistant.Item item)
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

        

        
        
	}
}
