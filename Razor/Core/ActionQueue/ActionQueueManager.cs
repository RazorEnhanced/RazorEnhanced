using System.Threading.Tasks;

namespace Assistant.Core.ActionQueue
{
    internal class ActionQueueManager
    {
        public static Item HoldingItem;

        internal static Task DragDrop(Item i, Serial to)
        {
            return Queue.EnqueueDragDrop(i.Serial, i.Amount, to, Point3D.MinusOne);
        }

        internal static Task DragDrop(Item i, int amount, Serial to)
        {
            return Queue.EnqueueDragDrop(i.Serial, amount, to, Point3D.MinusOne);
        }

        internal static Task DragDrop(Item i, Item to)
        {
            return Queue.EnqueueDragDrop(i.Serial, i.Amount, to.Serial, Point3D.MinusOne);
        }

        internal static Task DragDrop(Item i, Point3D dest)
        {
            return Queue.EnqueueDragDrop(i.Serial, i.Amount, Serial.MinusOne, dest);
        }

        internal static Task DragDrop(Item i, Point3D dest, int amount)
        {
            return Queue.EnqueueDragDrop(i.Serial, amount, Serial.MinusOne, dest);
        }

        internal static Task DragDrop(Item i, int amount, Item to)
        {
            return Queue.EnqueueDragDrop(i.Serial, amount, to.Serial, Point3D.MinusOne);
        }

        internal static Task DragDrop(Item i, int amount, Item to, Point3D dest)
        {
            return Queue.EnqueueDragDrop(i.Serial, amount, to.Serial, dest);
        }

        internal static Task DragDrop(Item i, Mobile to, Layer layer, int amount)
        {
            return Queue.EnqueueEquip(i.Serial, amount, to, layer);
        }

        internal static Task DragDrop(Item i, Mobile to, Layer layer, bool doLast)
        {
            return Queue.EnqueueEquip(i.Serial, i.Amount, to, layer);
        }

        internal static Task DragDrop(Item i, Mobile to, Layer layer)
        {
            return Queue.EnqueueEquip(i.Serial, i.Amount, to, layer);
        }

        internal static Task Drag(Item i, int amount)
        {
            return Queue.EnqueueDrag(i.Serial, amount);
        }
        //============================================================
        internal static Task Drag(Item i, int amount, bool fromClient)
        {
            return Drag(i, amount, fromClient, false);
        }

        internal static Task Drag(Item i, int amount, bool fromClient, bool doLast)
        {
            return Queue.EnqueueDrag(i.Serial, amount);

        }

        internal static Task Drop(Item i, Serial dest, Point3D pt)
        {
            return Queue.EnqueueDropRelative(i.Serial, dest, pt);
        }
        //============================================================

        internal static Task Drop(Item i, Item to)
        {
            return Queue.EnqueueDrop(i.Serial, to.Serial);
        }

        internal static Task Drop(Item i, Mobile to, Layer layer)
        {
            return Queue.EnqueueEquipDrop(i.Serial, to, layer);
        }

        internal static Task DoubleClick(Serial s)
        {
            return Queue.EnqueueDoubleClick(s);
        }

    }




}
