using System;
using System.Collections.Generic;

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
        public static void Move(Assistant.Item item, Assistant.Item bag)
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
            Assistant.DragDropManager.DragDrop(item, bag);
        }
        public static void DropItemGroundSelf(Assistant.Item item)
        {
            if (item == null)
            {
                Player.SendMessage("Script Error: DropItemGroundSelf: Item not found");
                return;
            }
            Assistant.DragDropManager.Drag(item, item.Amount);
            Assistant.DragDropManager.Drop(item, Assistant.Serial.MinusOne, Assistant.World.Player.Position);
        }        
	}
}
