using Assistant;
using RazorEnhanced.Macros.Actions;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace RazorEnhanced.Macros
{
    public static class MacroManager
    {
        private static readonly List<Macro> m_Macros = new List<Macro>();
        private static Macro m_CurrentRecording = null;
        private static readonly ConcurrentQueue<MacroAction> m_RecordQueue = new ConcurrentQueue<MacroAction>();
        private static Task m_RecordingTask = null;

        // For tracking pickup/drop amounts
        private static uint _lastPickedUpSerial = 0;
        private static int _lastPickedUpAmount = 0;

        // Index for "record from here" (set by UI, -1 means append)
        public static int RecordInsertIndex { get; set; } = -1;

        public static bool IsRecording => m_CurrentRecording != null;

        public static event EventHandler MacrosChanged;
        public static event EventHandler RecordingStateChanged;
        public static event EventHandler ActionRecorded;

        static MacroManager()
        {
            LoadMacros();
        }

        public static IReadOnlyList<Macro> GetMacros() => m_Macros.AsReadOnly();

        public static void AddMacro(Macro macro)
        {
            m_Macros.Add(macro);
            MacrosChanged?.Invoke(null, EventArgs.Empty);
            SaveMacros();
        }

        public static void RemoveMacro(Macro macro)
        {
            m_Macros.Remove(macro);
            MacrosChanged?.Invoke(null, EventArgs.Empty);
            SaveMacros();
        }

        public static void StartRecording(Macro macro, int insertIndex = -1)
        {
            if (IsRecording) StopRecording();

            m_CurrentRecording = macro;
            RecordInsertIndex = insertIndex;
            RecordingStateChanged?.Invoke(null, EventArgs.Empty);
            Misc.SendMessage($"Recording macro: {macro.Name}", 88);

            m_RecordingTask = Task.Run(ProcessRecordQueue);
            HookRecordingEvents();
        }

        public static void StopRecording()
        {
            if (!IsRecording) return;

            UnhookRecordingEvents();
            Task.Delay(100).Wait();

            Misc.SendMessage($"Recording stopped. {m_CurrentRecording.Actions.Count} actions recorded.", 88);
            SaveMacros();

            m_CurrentRecording = null;
            RecordInsertIndex = -1;
            RecordingStateChanged?.Invoke(null, EventArgs.Empty);
        }

        private static void ProcessRecordQueue()
        {
            while (IsRecording)
            {
                if (m_RecordQueue.TryDequeue(out MacroAction action))
                {
                    try
                    {
                        if (m_CurrentRecording != null)
                        {
                            // Insert at the correct position for "record from here"
                            if (RecordInsertIndex >= 0 && RecordInsertIndex <= m_CurrentRecording.Actions.Count)
                            {
                                m_CurrentRecording.Actions.Insert(RecordInsertIndex, action);
                                RecordInsertIndex++; // Move insertion point forward for next action
                            }
                            else
                            {
                                m_CurrentRecording.Actions.Add(action);
                            }
                        }
                        ActionRecorded?.Invoke(null, EventArgs.Empty);
                    }
                    catch
                    {
                        // Silently fail
                    }
                }
                else
                {
                    System.Threading.Thread.Sleep(10);
                }
            }
        }

        public static void RecordAction(MacroAction action)
        {
            if (!IsRecording) return;

            if (!Player.Connected)
            {
                StopRecording();
                Misc.SendMessage("Disconnected - Recording stopped.", 33);
                return;
            }

            m_RecordQueue.Enqueue(action);
        }




        public static void RecordSpeech(int hue, string text)
        {
            if (!IsRecording || string.IsNullOrWhiteSpace(text)) return;

            // If not connected, stop recording and return
            if (!Player.Connected)
            {
                StopRecording();
                Misc.SendMessage("Disconnected - Recording stopped.", 33);
                return;
            }

            // Record as MessagingAction of type Say, using the new TargetSerialOrAlias pattern (empty for Say)
            var action = new Actions.MessagingAction(
                Actions.MessagingAction.MessageType.Say,
                text,
                hue,
                "" // TargetSerialOrAlias is empty for Say
            );
            RecordAction(action);
        }

        public static void PlayMacro(string macroName)
        {
            var macro = m_Macros.FirstOrDefault(m => m.Name == macroName);
            macro?.Play();
        }

        public static void StopMacro(string macroName)
        {
            var macro = m_Macros.FirstOrDefault(m => m.Name == macroName);
            macro?.Stop();
        }

        public static void StopAllMacros()
        {
            foreach (var macro in m_Macros)
            {
                macro.Stop();
            }
        }

        #region Recording Hooks

        // Remove OnSkillRequestPacket from the hooks:
        private static void HookRecordingEvents()
        {
            // Register packet viewers for recording
            Assistant.PacketHandler.RegisterClientToServerViewer(0x02, OnMovementPacket);      // Movement
            Assistant.PacketHandler.RegisterClientToServerViewer(0x05, OnAttackPacket);        // Attack
            Assistant.PacketHandler.RegisterClientToServerViewer(0x06, OnDoubleClickPacket);   // Double click
            Assistant.PacketHandler.RegisterClientToServerViewer(0x07, OnPickUpItemPacket);    // Pick up item
            Assistant.PacketHandler.RegisterClientToServerViewer(0x08, OnDropItemPacket);      // Drop item
            Assistant.PacketHandler.RegisterClientToServerViewer(0x09, OnSingleClickPacket);   // Single click
                                                                                               // REMOVE THIS LINE: Assistant.PacketHandler.RegisterClientToServerViewer(0x12, OnSkillRequestPacket);
            Assistant.PacketHandler.RegisterClientToServerViewer(0x13, OnEquipItemPacket);     // Equip item
            Assistant.PacketHandler.RegisterClientToServerViewer(0x6C, OnTargetResponsePacket); // Target response
            Assistant.PacketHandler.RegisterClientToServerViewer(0xBF, OnExtendedPacket);      // Extended commands
            Assistant.PacketHandler.RegisterClientToServerViewer(0xD7, OnAbilityPacket);       // Weapon abilities

            // Server to client packets
            Assistant.PacketHandler.RegisterServerToClientViewer(0x6C, OnTargetCursorPacket);  // Target cursor request
        }

        private static void UnhookRecordingEvents()
        {
            // Unregister packet viewers
            Assistant.PacketHandler.RemoveClientToServerViewer(0x02, OnMovementPacket);
            Assistant.PacketHandler.RemoveClientToServerViewer(0x05, OnAttackPacket);
            Assistant.PacketHandler.RemoveClientToServerViewer(0x06, OnDoubleClickPacket);
            Assistant.PacketHandler.RemoveClientToServerViewer(0x07, OnPickUpItemPacket);
            Assistant.PacketHandler.RemoveClientToServerViewer(0x08, OnDropItemPacket);
            Assistant.PacketHandler.RemoveClientToServerViewer(0x09, OnSingleClickPacket);
            // REMOVE THIS LINE: Assistant.PacketHandler.RemoveClientToServerViewer(0x12, OnSkillRequestPacket);
            Assistant.PacketHandler.RemoveClientToServerViewer(0x13, OnEquipItemPacket);
            Assistant.PacketHandler.RemoveClientToServerViewer(0x6C, OnTargetResponsePacket);
            Assistant.PacketHandler.RemoveClientToServerViewer(0xBF, OnExtendedPacket);
            Assistant.PacketHandler.RemoveClientToServerViewer(0xD7, OnAbilityPacket);

            // Server to client packets
            Assistant.PacketHandler.RemoveServerToClientViewer(0x6C, OnTargetCursorPacket);
        }

        public static void RecordRenameMobile(int serial, string name)
        {
            if (!IsRecording) return;

            RecordAction(new Actions.RenameMobileAction(serial, name));
        }

        public static void RecordAsciiPromptResponse(uint type, string text)
        {
            if (!IsRecording) return;

            // Record the prompt response
            RecordAction(new Actions.PromptResponseAction(text, 10000));
        }

        public static void RecordQueryStringResponse(byte yesno, string text)
        {
            if (!IsRecording) return;

            bool accept = yesno != 0;
            RecordAction(new Actions.QueryStringResponseAction(accept, text, 10000));
        }


        // The RecordGumpResponse method:
        public static void RecordGumpResponse(uint gumpId, int buttonId, Gumps.GumpData gumpData)
        {
            if (!IsRecording) return;

            // First, add WaitForGump action (just like ScriptRecorder does)
            //RecordAction(new Actions.WaitForGumpAction(gumpId, 10000));

            // Then, add the response action
            if (gumpData == null || (gumpData.switches.Count == 0 && gumpData.textID.Count == 0))
            {
                // Simple button response
                RecordAction(new Actions.GumpResponseAction(gumpId, buttonId));
            }
            else
            {
                // Advanced response with data
                RecordAction(new Actions.GumpResponseAction(
                    gumpId,
                    buttonId,
                    gumpData.switches,
                    gumpData.textID,
                    gumpData.text
                ));
            }
        }


        public static void RecordContextMenuResponse(int serial, ushort menuIndex)
        {
            if (!IsRecording) return;

            // Use the new TargetSerialOrAlias string property.
            // If serial is not zero, format as hex string; otherwise, leave empty.
            string targetSerialOrAlias = serial != 0 ? $"0x{serial:X8}" : "";

            // Record context menu usage (WaitForContext is built into the action)
            RecordAction(new Actions.UseContextMenuAction(targetSerialOrAlias, menuIndex));
        }

        public static void RecordClientTextCommand(int type, int id)
        {
            if (!IsRecording) return;

            if (type == 1) // Use skill
            {
                string skillName = GetSkillNameFromId((byte)id);
                if (!string.IsNullOrEmpty(skillName))
                {
                    RecordAction(new Actions.UseSkillAction(skillName));
                }
            }
            else if (type == 3) // Invoke Virtue
            {
                string virtue = id switch
                {
                    1 => "Honor",
                    2 => "Sacrifice",
                    3 => "Valor",
                    4 => "Compassion",
                    5 => "Honesty",
                    6 => "Humility",
                    7 => "Justice",
                    8 => "Spirituality",
                    _ => null
                };

                if (virtue != null)
                {
                    RecordAction(new Actions.InvokeVirtueAction(virtue));
                }
            }
            // Type 2 (spells) are handled in OnExtendedPacket (0xBF/0x1C)
        }


        private static void OnAbilityPacket(PacketReader p, PacketHandlerEventArgs args)
        {
            if (!IsRecording) return;

            try
            {
                // 0xD7 packet structure for abilities:
                // uint serial (player serial)
                // ushort subcommand (0x19 for ability)
                // uint unknown (0)
                // byte abilityIndex
                // byte terminator (0x0A)

                uint serial = p.ReadUInt32();
                ushort subcommand = p.ReadUInt16();

                if (subcommand == 0x19) // Use ability
                {
                    uint unknown = p.ReadUInt32();
                    byte abilityIndex = p.ReadByte();

                    string ability = abilityIndex switch
                    {
                        0 => "Primary",
                        1 => "Secondary",
                        2 => "Disarm",
                        3 => "Stun",
                        _ => null
                    };

                    if (ability != null)
                    {
                        RecordAction(new Actions.SetAbilityAction(ability));
                    }
                }
            }
            catch
            {
                // Silently fail
            }
        }


        // Packet handlers for recording
        private static void OnTargetCursorPacket(PacketReader p, PacketHandlerEventArgs args)
        {
            if (!IsRecording) return;
            RecordAction(new Actions.WaitForTargetAction(5000));
        }

        private static void OnMovementPacket(PacketReader p, PacketHandlerEventArgs args)
        {
            if (!IsRecording) return;

            byte directionRaw = p.ReadByte();
            bool isRunning = (directionRaw & (byte)Direction.running) == (byte)Direction.running;
            byte direction = (byte)(directionRaw & (byte)Direction.mask);

            string directionStr = direction switch
            {
                0 => "North",
                1 => "Northeast",
                2 => "East",
                3 => "Southeast",
                4 => "South",
                5 => "Southwest",
                6 => "West",
                7 => "Northwest",
                _ => "North"
            };

            var type = isRunning
                ? RazorEnhanced.Macros.Actions.MovementAction.MovementType.Run
                : RazorEnhanced.Macros.Actions.MovementAction.MovementType.Walk;

            var action = new RazorEnhanced.Macros.Actions.MovementAction(type, directionStr);
            RecordAction(action);
        }

        /*
        private static void OnMovementPacket(PacketReader p, PacketHandlerEventArgs args)
        {
            if (!IsRecording) return;

            byte directionRaw = p.ReadByte();
            byte direction = (byte)(directionRaw & 0x07);

            RecordAction(new Actions.MovementAction(MovementType.Walk, direction));

            //RecordAction(new Actions.WalkAction(direction));
        } 
        */

        private static void OnAttackPacket(PacketReader p, PacketHandlerEventArgs args)
        {
            if (!IsRecording) return;

            uint serial = p.ReadUInt32();
            // Record AttackLastAction with Serial mode instead of deprecated AttackAction
            RecordAction(new Actions.AttackAction(Actions.AttackAction.AttackMode.Serial, (int)serial));
        }

        private static void OnDoubleClickPacket(PacketReader p, PacketHandlerEventArgs args)
        {
            if (!IsRecording) return;

            uint serial = p.ReadUInt32();

            // Quick lookup without blocking
            var item = Items.FindBySerial((int)serial);

           if (item != null)
            {
                string potionType = GetPotionType(item.ItemID);
                if (potionType != null)
                {
                    RecordAction(new Actions.UsePotionAction(potionType));
                }
                else
                {
                    RecordAction(new Actions.DoubleClickAction(serial));
                }
            }
            else
            {
                RecordAction(new Actions.DoubleClickAction(serial));
            }
        }

        private static void OnSingleClickPacket(PacketReader p, PacketHandlerEventArgs args)
        {
            if (!IsRecording) return;
            // Single click handling if needed
        }

        private static string GetSkillNameFromId(byte id)
        {
            return id switch
            {
                1 => "Anatomy",
                2 => "Animal Lore",
                3 => "Item ID",
                4 => "Arms Lore",
                6 => "Begging",
                9 => "Peacemaking",
                12 => "Cartography",
                14 => "Detect Hidden",
                15 => "Discordance",
                16 => "Eval Int",
                19 => "Forensics",
                21 => "Hiding",
                22 => "Provocation",
                23 => "Inscription",
                30 => "Poisoning",
                32 => "Spirit Speak",
                33 => "Stealing",
                35 => "Animal Taming",
                36 => "Taste ID",
                38 => "Tracking",
                46 => "Meditation",
                47 => "Stealth",
                48 => "Remove Trap",
                _ => null
            };
        }

        private static void OnEquipItemPacket(PacketReader p, PacketHandlerEventArgs args)
        {
            if (!IsRecording) return;

            uint serial = p.ReadUInt32();
            byte layer = p.ReadByte();

            // Check if equipping to hand layers (weapon/shield)
            if (layer == 1 || layer == 2) // LeftHand = 1, RightHand = 2
            {
                string hand = layer == 1 ? "Left" : "Right";
                var action = new Actions.ArmDisarmAction("Arm", (int)serial, hand);
                RecordAction(action);
            }
        }

        private static void OnTargetResponsePacket(PacketReader p, PacketHandlerEventArgs args)
        {
            if (!IsRecording) return;

            byte targetType = p.ReadByte();
            uint targetID = p.ReadUInt32();
            byte flags = p.ReadByte();
            uint serial = p.ReadUInt32();
            ushort x = p.ReadUInt16();
            ushort y = p.ReadUInt16();
            short z = p.ReadInt16();
            ushort graphic = p.ReadUInt16();

            // Determine the appropriate target mode
            if (serial != 0)
            {
                // Check if targeting self
                if (World.Player != null && serial == World.Player.Serial)
                {
                    RecordAction(new Actions.TargetAction(Actions.TargetAction.TargetMode.Self));
                }
                else
                {
                    // Targeting a specific serial
                    RecordAction(new Actions.TargetAction(
                        Actions.TargetAction.TargetMode.Serial,
                        serial: (int)serial
                    ));
                }
            }
            else
            {
                // Targeting a location (ground/tile)
                RecordAction(new Actions.TargetAction(
                    Actions.TargetAction.TargetMode.Location,
                    x: (int)x,
                    y: (int)y,
                    z: (int)z
                ));
            }
        }

        private static void OnPickUpItemPacket(PacketReader p, PacketHandlerEventArgs args)
        {

            if (!IsRecording) return;

            uint serial = p.ReadUInt32();
            ushort amount = p.ReadUInt16();

            _lastPickedUpSerial = serial;
            _lastPickedUpAmount = amount;

            return;

            RecordAction(new Actions.PickUpAction(serial, amount));
        }

        private static void OnDropItemPacket(PacketReader p, PacketHandlerEventArgs args)
        {
            if (!IsRecording) return;

            try
            {
                uint serial = p.ReadUInt32();
                ushort x = p.ReadUInt16();
                ushort y = p.ReadUInt16();
                sbyte z = p.ReadSByte();

                if (Engine.UsePostKRPackets)
                {
                    p.ReadByte(); // Skip grid index
                }

                uint containerSerial = p.ReadUInt32();

                int amount = 0;

                // Use the last picked up amount if serial matches
                if (serial == _lastPickedUpSerial)
                {
                    amount = _lastPickedUpAmount;
                }
                else
                {
                    // Fallback: try to get from world (may be 0 or incorrect)
                    Item item = Items.FindBySerial((int)serial);
                    if (item != null)
                        amount = item.Amount;
                }

                // Reset after use
                _lastPickedUpSerial = 0;
                _lastPickedUpAmount = 0;

                Item cont = Items.FindBySerial((int)containerSerial);

                var action = new RazorEnhanced.Macros.Actions.MoveItemAction(containerSerial != 0xFFFFFFFF ? RazorEnhanced.Macros.Actions.MoveItemAction.MoveTargetType.Entity : RazorEnhanced.Macros.Actions.MoveItemAction.MoveTargetType.Ground, serial.ToString(), amount, containerSerial.ToString(), x, y, z);
                RecordAction(action);

            }
            catch (Exception ex)
            {
                Misc.SendMessage($"Error recording drop: {ex.Message}", 33);
            }
        }
        private static void OnExtendedPacket(PacketReader p, PacketHandlerEventArgs args)
        {
            if (!IsRecording) return;

            try
            {
                ushort subcommand = p.ReadUInt16();

                switch (subcommand)
                {
                    case 0x1C: // Cast spell
                        short spellType = p.ReadInt16();
                        if (spellType == 1)
                            p.ReadUInt32(); // book serial - skip it

                        ushort spellID = p.ReadUInt16();
                        RecordAction(new Actions.CastSpellAction(spellID));
                        break;

                        // Remove the 0x19 case - weapon abilities use packet 0xD7 instead
                }
            }
            catch { }
        }
        private static string GetPotionType(int itemID)
        {
            return itemID switch
            {
                0x0F0C => "Heal",
                0x0F07 => "Cure",
                0x0F0B => "Refresh",
                0x0F08 => "Agility",
                0x0F09 => "Strength",
                0x0F0A => "Poison",
                0x0F0D => "Explosion",
                _ => null
            };
        }

        #endregion

        private static string GetMacroPath()
        {
            string path = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "RazorEnhanced",
                "Macros"
            );

            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            return path;
        }

        // Change from private to public:
        public static void SaveMacros()
        {
            try
            {
                string path = GetMacroPath();

                // Clear existing files
                foreach (var file in Directory.GetFiles(path, "*.macro"))
                {
                    File.Delete(file);
                }

                // Save each macro
                foreach (var macro in m_Macros)
                {
                    string filename = Path.Combine(path, $"{macro.Name}.macro");
                    File.WriteAllText(filename, macro.Serialize());
                }
            }
            catch (Exception ex)
            {
                Misc.SendMessage($"Error saving macros: {ex.Message}", 33);
            }
        }
        private static void LoadMacros()
        {
            try
            {
                string path = GetMacroPath();

                foreach (var file in Directory.GetFiles(path, "*.macro"))
                {
                    // TODO: Implement deserialization
                    // For now, just skip
                }
            }
            catch (Exception ex)
            {
                Misc.SendMessage($"Error loading macros: {ex.Message}", 33);
            }
        }






        public static void LoadMacrosFromFiles()
        {
            string macrosFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Macros");
            if (!Directory.Exists(macrosFolder))
                Directory.CreateDirectory(macrosFolder);

            var macroFiles = Directory.GetFiles(macrosFolder, "*.macro");
            foreach (var file in macroFiles)
            {
                string macroName = Path.GetFileNameWithoutExtension(file);
                var macro = new Macro { Name = macroName };

                foreach (var line in File.ReadAllLines(file))
                {
                    if (string.IsNullOrWhiteSpace(line))
                        continue;

                    MacroAction action = MacroActionFactory.CreateFromSerialized(line);
                    if (action != null)
                        macro.Actions.Add(action);
                }

                if (!m_Macros.Any(m => m.Name.Equals(macroName, StringComparison.OrdinalIgnoreCase)))
                    AddMacro(macro);
            }
        }

    }
}