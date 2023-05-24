using Accord;
using Assistant;
using Assistant.Enums;
using JsonData;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;

namespace RazorEnhanced
{
    /// <summary>
    /// The Target class provides various method for targeting Land, Items and Mobiles in game.
    /// </summary>
    public class Target
    {
        private int m_ptarget;
        private RazorEnhanced.Point3D m_pgtarget;

        private Task<int> promptTargetTask;
        public int PromptTargetAsyncResult { get; private set; }

        /// <summary>
        /// Get status if have in-game cursor has target shape.
        /// </summary>
        /// <returns>True: Cursor has target - False: otherwise</returns>
        public static bool HasTarget()
        {
            return HasTarget("Any");
        }

        /// <summary>
        /// Get status if have in-game cursor has target shape.
        /// U can check if cursor is beneficial, harmful or neutral
        /// </summary>
        /// <returns>True: Cursor has target - False: otherwise</returns>
        public static bool HasTarget(string targetFlagsExists = "Any")
        {
            if (!Enum.TryParse(targetFlagsExists, out Assistant.Enums.TargetFlagsExists enumValue))
            {
                enumValue = Assistant.Enums.TargetFlagsExists.Any;                
            }

            switch (enumValue)
            {
                case Assistant.Enums.TargetFlagsExists.Any:                    
                    return Assistant.Targeting.HasTarget;
                case Assistant.Enums.TargetFlagsExists.Beneficial:
                    return Assistant.Targeting.HasTarget &&
                           Assistant.Enums.TargetFlags.Beneficial.GetHashCode().Equals(Assistant.Targeting.TargetFlags);
                case Assistant.Enums.TargetFlagsExists.Harmful:
                    return Assistant.Targeting.HasTarget &&
                           Assistant.Enums.TargetFlags.Harmful.GetHashCode().Equals(Assistant.Targeting.TargetFlags);
                case Assistant.Enums.TargetFlagsExists.Neutral:
                    return Assistant.Targeting.HasTarget &&
                           Assistant.Enums.TargetFlags.None.GetHashCode().Equals(Assistant.Targeting.TargetFlags);                  
                default:
                    throw new ArgumentOutOfRangeException();
            }            
        }

        /// <summary>
        /// Wait for the cursor to show the target, pause the script for a maximum amount of time. and optional flag True or False. True Not show cursor, false show it
        /// </summary>
        /// <param name="delay">Maximum amount to wait, in milliseconds</param>
        /// <param name="noshow">Pevent the cursor to display the target.</param>
        /// <returns></returns>
        public static bool WaitForTarget(int delay, bool noshow = false)
        {
            int subdelay = delay;
            Assistant.Targeting.NoShowTarget = noshow;
            while (Assistant.Targeting.HasTarget == false)
            {
                Thread.Sleep(2);
                subdelay -= 2;
                if (subdelay <= 0)
                    break;
            }
            Assistant.Targeting.NoShowTarget = false;
            return HasTarget();
        }

        /// <summary>
        /// Execute target on specific serial, item, mobile, X Y Z point.
        /// </summary>
        /// <param name="x">X coordinate.</param>
        /// <param name="y">Y coordinate.</param>
        /// <param name="z">Z coordinate.</param>
        /// <param name="StaticID">ID of Land/Tile</param>
        public static void TargetExecute(int x, int y, int z, int StaticID)
        {
            Assistant.Point3D location = new Assistant.Point3D(x, y, z);
            Assistant.Targeting.Target(location, StaticID, true);
        }

        public static void TargetExecute(int x, int y, int z)
        {
            Assistant.Point3D location = new Assistant.Point3D(x, y, z);
            Assistant.Targeting.Target(location, true);
        }


        /// <param name="serial">Serial of the Target</param>
        public static void TargetExecute(int serial)
        {
            if (!CheckHealPoisonTarg(serial))
            {
                Assistant.Targeting.Target(serial, true);
            }
        }

        /// <param name="item">Item object to Target.</param>
        public static void TargetExecute(RazorEnhanced.Item item)
        {
            Assistant.Targeting.Target(item.Serial, true);
        }

        /// <param name="mobile">Mobile object to Target.</param>
        public static void TargetExecute(RazorEnhanced.Mobile mobile)
        {
            if (!CheckHealPoisonTarg(mobile.Serial))
            {
                Assistant.Targeting.Target(mobile.Serial, true);
            }
        }


        /// <summary>
        /// Execute target on specific land point with offset distance from Mobile. Distance is calculated by target Mobile.Direction.
        /// </summary>
        /// <param name="mobile">Mobile object to target.</param>
        /// <param name="offset">Distance from the target.</param>
        public static void TargetExecuteRelative(Mobile mobile, int offset)
        {
            Assistant.Point2D relpos = new Assistant.Point2D();
            switch (mobile.Direction)
                {
                case "North":
                    relpos.X = mobile.Position.X;
                    relpos.Y = mobile.Position.Y - offset;
                    break;
                case "South":
                    relpos.X = mobile.Position.X;
                    relpos.Y = mobile.Position.Y + offset;
                    break;
                case "West":
                    relpos.X = mobile.Position.X - offset;
                    relpos.Y = mobile.Position.Y;
                    break;
                case "East":
                    relpos.X = mobile.Position.X + offset;
                    relpos.Y = mobile.Position.Y;
                    break;
                case "Up":
                    relpos.X = mobile.Position.X - offset;
                    relpos.Y = mobile.Position.Y - offset;
                    break;
                case "Down":
                    relpos.X = mobile.Position.X + offset;
                    relpos.Y = mobile.Position.Y + offset;
                    break;
                case "Left":
                    relpos.X = mobile.Position.X - offset;
                    relpos.Y = mobile.Position.Y + offset;
                    break;
                case "Right":
                    relpos.X = mobile.Position.X + offset;
                    relpos.Y = mobile.Position.Y - offset;
                    break;
            }
            Assistant.Point3D location = new Assistant.Point3D(relpos.X, relpos.Y, Statics.GetLandZ(relpos.X, relpos.Y, Player.Map));
            Assistant.Targeting.Target(location, true);
        }
        /// <param name="serial">Serial of the mobile</param>
        /// <param name="offset">+- distance to offset from the mobile identified with serial</param>
        public static void TargetExecuteRelative(int serial, int offset)
        {
            Mobile m = Mobiles.FindBySerial(serial);
            if (m != null)
                TargetExecuteRelative(m, offset);
        }

        /// <summary>
        /// Find and target a resource using the specified item.
        /// </summary>
        /// <param name="item_serial">Item object to use.</param>
        /// <param name="resource_number"> Resource as standard name or custom number
        ///     0: ore
        ///     1: sand
        ///     2: wood
        ///     3: graves
        ///     4: red_mushrooms
        ///     n: custom 
        /// </param>
        public static void TargetResource(int item_serial, int resource_number)
        {
            Assistant.Item item = Assistant.World.FindItem(item_serial);
            if (item == null)
            {
                Scripts.SendMessageScriptError("Script Error: UseItem: Invalid Use Serial");
                return;
            }
            Client.Instance.SendToServer(new TargeByResource((uint)item_serial, (uint)resource_number));
        }
        
        public static void TargetResource(int item_serial, string resource_name)
        {
            Assistant.Item item = Assistant.World.FindItem(item_serial);
            if (item == null)
            {
                Scripts.SendMessageScriptError("Script Error: UseItem: Invalid Use Serial");
                return;
            }

            int number;
            switch (resource_name)
            {
                case "ore":
                    number = 0;
                    break;
                case "sand":
                    number = 1;
                    break;
                case "wood":
                    number = 2;
                    break;
                case "graves":
                    number = 3;
                    break;
                case "red_mushroom":
                    number = 4;
                    break;
                default:
                    System.Int32.TryParse(resource_name, out number);
                    break;
            }
            if (number >= 0)
                TargetResource(item_serial, number);
            else
                Misc.SendMessage("Valid resource types are ore, sand, wood, graves, red mushroom, or a number");
        }

        /// <param name="item">Item object to use.</param>
        /// <param name="resouce_name">name of the resource to be targeted. ore, sand, wood, graves, red mushroom</param>
        public static void TargetResource(Item item, string resouce_name)
        {
            TargetResource(item.Serial, resouce_name);
        }
        

        public static void TargetResource(Item item, int resoruce_number)
        {
            TargetResource(item.Serial, resoruce_number);
        }

        /// <summary>
        /// Cancel the current target.
        /// </summary>
        public static void Cancel()
        {
            //Assistant.Targeting.CancelClientTarget(true);
            Assistant.Targeting.CancelOneTimeTarget(true);
        }

        /// <summary>
        /// Execute the target on the Player.
        /// </summary>
        public static void Self()
        {
            if (World.Player != null)
                Assistant.Targeting.TargetSelf(false);
        }

        /// <summary>
        /// Enqueue the next target on the Player.
        /// </summary>
        public static void SelfQueued()
        {
            Assistant.Targeting.TargetSelf(true);
        }

        /// <summary>
        /// Execute the target on the last Item or Mobile targeted.
        /// </summary>
        public static void Last()
        {
            if (!CheckHealPoisonTarg(GetLast()))
                Assistant.Targeting.LastTarget();
        }

        /// <summary>
        /// Enqueue the next target on the last Item or Mobile targeted.
        /// </summary>
        public static void LastQueued()
        {
            Assistant.Targeting.LastTarget(true);
        }

        /// <summary>
        /// Returns the serial of last object used by the player.
        /// </summary>
        public static int LastUsedObject()
        {
            if (World.Player == null || World.Player.LastObject == 0)
                return -1;
            return World.Player.LastObject;
        }


        /// <summary>
        /// Get serial number of last target
        /// </summary>
        /// <returns>Serial as number.</returns>
        public static int GetLast()
        {
            return (int)Assistant.Targeting.GetLastTarger;
        }

        /// <summary>
        /// Get serial number of last attack target
        /// </summary>
        /// <returns>Serial as number.</returns>
        public static int GetLastAttack()
        {
            return (int)Assistant.Targeting.LastAttack;
        }

        public static void SetLast(RazorEnhanced.Mobile mob)
        {
            Assistant.Mobile mobile = World.FindMobile(mob.Serial);
            if (mobile != null)
                SetLast(mob.Serial);
        }

        /// <summary>
        /// Set the last target to specific mobile, using the serial.
        /// </summary>
        /// <param name="serial">Serial of the Mobile.</param>
        /// <param name="wait">Wait confirmation from the server.</param>
        public static void SetLast(int serial, bool wait = true)
        {
            TargetMessage(serial, wait); // Process message for highlight
            Assistant.Targeting.SetLastTarget(serial, 0, wait);
        }

        /// <summary>
        /// Clear the last attacked target
        /// </summary>
        public static void ClearLastAttack()
        {
            Assistant.Targeting.LastAttack = 0;
        }

        /// <summary>
        /// Clear Queue Target.
        /// </summary>
        public static void ClearQueue()
        {
            Assistant.Targeting.ClearQueue();
        }

        /// <summary>
        /// Clear the last target.
        /// </summary>
        public static void ClearLast()
        {
            Assistant.Targeting.ClearLast();
        }

        /// <summary>
        /// Clear last target and target queue.
        /// </summary>
        public static void ClearLastandQueue()
        {
            Assistant.Targeting.ClearQueue();
            Assistant.Targeting.ClearLast();
        }

        #region PROMPT TARGET ASYNC SUPPORTING METHODS
        private async Task<int> PromptInputAsync(string message = "Select Item or Mobile", int color = 945)
        {
            this.m_ptarget = -1;
            Misc.SendMessage(message, color, true);
            Targeting.OneTimeTarget(false, new Targeting.TargetResponseCallback(this.PromptTargetExex_Callback));

            while (!Targeting.HasTarget)
                await Task.Delay(30);

            while (this.m_ptarget == -1 && Targeting.HasTarget)
                await Task.Delay(30);

            await Task.Delay(100);

            if (this.m_ptarget == 0)
            {
                this.m_ptarget = -2;
            }

            if (this.m_ptarget == -1)
                Misc.SendMessage("Prompt Target Cancelled", color, true);

            return this.m_ptarget;
        }

        private async Task PromptTargetAsyncTask()
        {
            // Start the prompt input task
            this.promptTargetTask = this.PromptInputAsync();

            // Perform other non-blocking operations here

            // Wait for the prompt input task to complete
            var result = await this.promptTargetTask;

            // Continue with the rest of the code here
            this.PromptTargetAsyncResult = result;
        }

        public void ClearPromptTargetAsyncResult()
        {
            this.PromptTargetAsyncResult = 0;
        }
        #endregion

        /// <summary>
        /// Prompt a target in-game, wait for the Player to select an Item or a Mobile. Can also specific a text message for prompt.
        /// </summary>
        /// <param name="message">Hint on what to select.</param>
        /// <param name="color">Color of the message. (default: 945, gray)</param>
        public async Task PromptTargetAsync(string message = "Select Item or Mobile", int color = 945)
        {
            await this.PromptTargetAsyncTask();
        }

        /// <summary>
        /// Prompt a target in-game, wait for the Player to select an Item or a Mobile. Can also specific a text message for prompt.
        /// </summary>
        /// <param name="message">Hint on what to select.</param>
        /// <param name="color">Color of the message. (default: 945, gray)</param>
        /// <returns>Serial of the selected object.</returns>
        public int PromptTarget(string message = "Select Item or Mobile", int color = 945)
        {
            m_ptarget = -1;
            Misc.SendMessage(message, color, true);
            Targeting.OneTimeTarget(false, new Targeting.TargetResponseCallback(PromptTargetExex_Callback));

            while (!Targeting.HasTarget)
                Thread.Sleep(30);

            while (m_ptarget == -1 && Targeting.HasTarget)
                Thread.Sleep(30);

            Thread.Sleep(100);

            if (m_ptarget == -1)
                Misc.SendMessage("Prompt Target Cancelled", color, true);

            return m_ptarget;
        }

        private void PromptTargetExex_Callback(bool loc, Assistant.Serial serial, Assistant.Point3D pt, ushort itemid)
        {
            m_ptarget = serial;
        }
        /// <summary>
        /// Prompt a target in-game, wait for the Player to select the ground. Can also specific a text message for prompt.
        /// </summary>
        /// <param name="message">Hint on what to select.</param>
        /// <param name="color">Color of the message. (default: 945, gray)</param>
        /// <returns>A Point3D object, containing the X,Y,Z coordinate</returns>
        public Point3D PromptGroundTarget(string message = "Select Ground Position", int color = 945)
        {
            m_pgtarget = Point3D.MinusOne;

            Misc.SendMessage(message, color, true);
            Targeting.OneTimeTarget(true, new Targeting.TargetResponseCallback(PromptGroundTargetExex_Callback));

            while (!Targeting.HasTarget)
                Thread.Sleep(30);

            while (m_pgtarget.X == -1 && Targeting.HasTarget)
                Thread.Sleep(30);

            Thread.Sleep(100);

            if (m_pgtarget.X == -1)
                Misc.SendMessage("Prompt Gorund Target Cancelled", color, true);

            return m_pgtarget;
        }

        private void PromptGroundTargetExex_Callback(bool loc, Assistant.Serial serial, Assistant.Point3D pt, ushort itemid)
        {
            if (!loc)
            {
                Mobile target = Mobiles.FindBySerial(serial);
                if (target == null)
                {
                    m_pgtarget = Point3D.MinusOne;
}
                else {
                    m_pgtarget = target.Position;
                }
            }
            else
                m_pgtarget = new Point3D(pt.X, pt.Y, pt.Z);
        }

        // Check Poison
        private static bool CheckHealPoisonTarg(Assistant.Serial ser)
        {
            if (World.Player == null)
                return false;

            if (!RazorEnhanced.Settings.General.ReadBool("BlockHealPoison"))
                return false;

            if (ser.IsMobile && (World.Player.LastSpell == Spell.ToID(1, 4) || World.Player.LastSpell == Spell.ToID(4, 5) || World.Player.LastSpell == 202))
            {
                Assistant.Mobile m = World.FindMobile(ser);

                if (m != null && m.Poisoned)
                {
                    World.Player.SendMessage(MsgLevel.Warning, "Heal blocked, Target is poisoned!");
                    return true;
                }
                else if (m != null && m.Blessed)
                {
                    World.Player.SendMessage(MsgLevel.Warning, "Heal blocked, Target is mortelled!");
                    return true;
                }
                return false;
            }
            else
                return false;
        }

        // Funzioni target per richiamare i target della gui

        private static string GetPlayerName(int s)
        {
            Assistant.Mobile mob = World.FindMobile(s);
            return mob != null ? mob.Name : string.Empty;
        }

        private static readonly int[] m_NotoHues = new int[8]
        {
            1, // black     unused 0
            0x059, // blue      0x0059 1
            0x03F, // green     0x003F 2
            0x3B2, // greyish   0x03b2 3
            0x3B2, // grey         "   4
            0x090, // orange        0x0090 5
            0x022, // red       0x0022 6
            0x035, // yellow        0x0035 7
        };

        private static int GetPlayerColor(Mobile mob)
        {
            if (mob == null)
                return 0;

            return m_NotoHues[mob.Notoriety];
        }

        /// <summary>
        /// Set Last Target from GUI filter selector, in Targetting tab.
        /// </summary>
        /// <param name="target_name">Name of the target filter.</param>
        public static void SetLastTargetFromList(string target_name)
        {
            TargetGUI targetdata = Settings.Target.TargetRead(target_name);
            if (targetdata != null)
            {
                Mobiles.Filter filter = targetdata.TargetGuiObject.Filter.ToMobileFilter();
                string selector = targetdata.TargetGuiObject.Selector;

                List<Mobile> filterresult;
                filterresult = Mobiles.ApplyFilter(filter);

                Mobile mobtarget = Mobiles.Select(filterresult, selector);
                if (mobtarget == null)
                    return;

                RazorEnhanced.Target.SetLast(mobtarget);
            }
        }


        /// <summary>
        /// Get Mobile object from GUI filter selector, in Targetting tab.
        /// </summary>
        /// <param name="target_name">Name of the target filter.</param>
        /// <returns>Mobile object matching. None: not found</returns>
        public static Mobile GetTargetFromList(string target_name)
        {
            TargetGUI targetdata = Settings.Target.TargetRead(target_name);
            if (targetdata == null)
                return null;


            Mobiles.Filter filter = targetdata.TargetGuiObject.Filter.ToMobileFilter();
            string selector = targetdata.TargetGuiObject.Selector;

            List<Mobile> filterresult;
            filterresult = Mobiles.ApplyFilter(filter);

            Mobile mobtarget = Mobiles.Select(filterresult, selector);
            if (mobtarget == null)
                return null;

            return mobtarget;
        }

        internal static void SetLastTargetFromListHotKey(string targetid)
        {
            TargetGUI targetdata = Settings.Target.TargetRead(targetid);

            if (targetdata == null)
                return;

            Mobiles.Filter filter = targetdata.TargetGuiObject.Filter.ToMobileFilter();
            string selector = targetdata.TargetGuiObject.Selector;

            List<Mobile> filterresult;
            filterresult = Mobiles.ApplyFilter(filter);

            Mobile mobtarget = Mobiles.Select(filterresult, selector);

            if (mobtarget == null)
                return;

            TargetMessage(mobtarget.Serial, false); // Process message for highlight

            Assistant.Mobile mobile = World.FindMobile(mobtarget.Serial);
            if (mobile != null)
                Targeting.SetLastTarget(mobile.Serial, 0, false);
        }


        /// <summary>
        /// Execute Target from GUI filter selector, in Targetting tab.
        /// </summary>
        /// <param name="target_name">Name of the target filter.</param>
        public static void PerformTargetFromList(string target_name)
        {
            TargetGUI targetdata = Settings.Target.TargetRead(target_name);

            if (targetdata == null)
                return;

            Mobiles.Filter filter = targetdata.TargetGuiObject.Filter.ToMobileFilter();
            string selector = targetdata.TargetGuiObject.Selector;

            List<Mobile> filterresult;
            filterresult = Mobiles.ApplyFilter(filter);

            Mobile mobtarget = Mobiles.Select(filterresult, selector);

            if (mobtarget == null)
                return;

            TargetExecute(mobtarget.Serial);
            SetLast(mobtarget);
        }

        /// <summary>
        /// Attack Target from gui filter selector, in Targetting tab.
        /// </summary>
        /// <param name="target_name"></param>
        public static void AttackTargetFromList(string target_name)
        {
            TargetGUI targetdata = Settings.Target.TargetRead(target_name);

            if (targetdata == null)
                return;

            Mobiles.Filter filter = targetdata.TargetGuiObject.Filter.ToMobileFilter();
            string selector = targetdata.TargetGuiObject.Selector;

            List<Mobile> filterresult;
            filterresult = Mobiles.ApplyFilter(filter);

            Mobile mobtarget = Mobiles.Select(filterresult, selector);

            if (mobtarget == null)
                return;

            AttackMessage(mobtarget.Serial, true); // Process message for highlight
            if (Targeting.LastAttack != mobtarget.Serial)
            {
                Assistant.Client.Instance.SendToClientWait(new ChangeCombatant(mobtarget.Serial));
                Targeting.LastAttack = (uint)mobtarget.Serial;
            }
            Assistant.Client.Instance.SendToServerWait(new AttackReq(mobtarget.Serial)); // Real attack
        }

        internal static void TargetMessage(int serial, bool wait)
        {
            if (Assistant.Engine.MainWindow.ShowHeadTargetCheckBox.Checked)
            {
                if (Friend.IsFriend(serial))
                    Mobiles.Message(World.Player.Serial, 63, "Target: [" + GetPlayerName(serial) + "]", wait);
                else
                    Mobiles.Message(World.Player.Serial, GetPlayerColor(Mobiles.FindBySerial(serial)), "Target: [" + GetPlayerName(serial) + "]", wait);
            }

            if (Assistant.Engine.MainWindow.HighlightTargetCheckBox.Checked)
                Mobiles.Message(serial, 10, "* Target *", wait);
        }

        internal static void AttackMessage(int serial, bool wait)
        {
            if (Assistant.Engine.MainWindow.ShowHeadTargetCheckBox.Checked)
            {
                if (Friend.IsFriend(serial))
                    Mobiles.Message(World.Player.Serial, 63, "Attack: [" + GetPlayerName(serial) + "]", wait);
                else
                    Mobiles.Message(World.Player.Serial, GetPlayerColor(Mobiles.FindBySerial(serial)), "Attack: [" + GetPlayerName(serial) + "]", wait);
            }

            if (Assistant.Engine.MainWindow.HighlightTargetCheckBox.Checked)
                Mobiles.Message(serial, 10, "* Target *", wait);
        }

    }
}
