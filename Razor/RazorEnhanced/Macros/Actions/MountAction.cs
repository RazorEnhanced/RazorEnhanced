using Assistant;
using System;
using System.Threading;

namespace RazorEnhanced.Macros.Actions
{
    public class MountAction : MacroAction
    {
        public bool ShouldMount { get; set; }
        public int MountSerial { get; set; }

        public MountAction()
        {
            ShouldMount = true;
            MountSerial = 0;
        }

        public MountAction(bool shouldMount, int mountSerial = 0)
        {
            ShouldMount = shouldMount;
            MountSerial = mountSerial;
        }

        public override string GetActionName()
        {
            return "Mount";
        }

        public override void Execute()
        {
            if (ShouldMount)
            {
                // Mount mode
                if (MountSerial == 0)
                {
                    // Mount last used mount
                    if (World.Player.GetItemOnLayer(Layer.Mount) != null)   // Gia su mount
                        return;

                    Assistant.Item etheralMount = Assistant.World.FindItem(Filters.AutoRemountSerial);
                    if (etheralMount != null && etheralMount.Serial.IsItem)
                    {
                        Items.UseItem(Filters.AutoRemountSerial);
                        Thread.Sleep(Filters.AutoRemountEDelay);
                    }
                    else
                    {
                        Assistant.Mobile mount = Assistant.World.FindMobile(Filters.AutoRemountSerial);
                        if (mount != null && mount.Serial.IsMobile)
                        {
                            Mobiles.UseMobile(Filters.AutoRemountSerial);
                            Thread.Sleep(Filters.AutoRemountEDelay);
                        }
                        else
                        {
                            Misc.SendMessage("There is no mount to remount.", 33);
                        }
                    }
                }
                else
                {

                    if (World.Player.GetItemOnLayer(Layer.Mount) != null)   // Gia su mount
                        return;

                    Assistant.Item etheralMount = Assistant.World.FindItem(MountSerial);
                    if (etheralMount != null && etheralMount.Serial.IsItem)
                    {
                        Items.UseItem(MountSerial);
                        Thread.Sleep(1000);
                    }
                    else
                    {
                        Assistant.Mobile mount = Assistant.World.FindMobile(MountSerial);
                        if (mount != null && mount.Serial.IsMobile)
                        {
                            Mobiles.UseMobile(MountSerial);
                            Thread.Sleep(1000);
                        }
                        else
                        {
                            Misc.SendMessage("There is no mount to remount.", 33);
                        }
                    }
                }
            }
            else
            {
                // Dismount mode
                Mobiles.UseMobile(Player.Serial);
            }
        }

        public override string Serialize()
        {
            return $"Mount|{ShouldMount}|{MountSerial}";
        }

        public override void Deserialize(string data)
        {
            var parts = data.Split('|');

            // Format: Mount|ShouldMount|MountSerial
            if (parts.Length >= 2)
            {
                bool.TryParse(parts[1], out bool shouldMount);
                ShouldMount = shouldMount;
            }
            if (parts.Length >= 3)
            {
                int.TryParse(parts[2], out int mountSerial);
                MountSerial = mountSerial;
            }
        }

        public override bool IsValid()
        {
            return true;
        }

        public override int GetDelay()
        {
            return 250; // Small delay for mount/dismount command
        }
    }
}