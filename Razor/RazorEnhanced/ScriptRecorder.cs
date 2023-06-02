using Assistant;
using IronPython.Runtime.Exceptions;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RazorEnhanced
{
    public delegate void ScriptRecorderOutput(string code);

    public class ScriptRecorderService{ 
        public readonly static ScriptRecorderService Instance = new ScriptRecorderService();
        private readonly List<ScriptRecorder> m_ScriptRecorderList = new List<ScriptRecorder>();
        public List<ScriptRecorder> ScriptRecorderList { get { return new List<ScriptRecorder>(m_ScriptRecorderList); } }

        private int m_Count=0;

        internal static ScriptRecorder RecorderForLanguage(ScriptLanguage language)
        {
            switch (language)
            {
                default:
                case ScriptLanguage.PYTHON: return new PyScriptRecorder();
                case ScriptLanguage.CSHARP: return new CsScriptRecorder();
                case ScriptLanguage.UOSTEAM: return new UosScriptRecorder();
            }
        }

        public bool Active() { 
            return m_Count > 0;
        }

        public int RecorderCount()
        {
            return m_Count;
        }
        public void Add(ScriptRecorder scriptRecorder)
        {
            m_ScriptRecorderList.Add(scriptRecorder);
            m_Count = m_ScriptRecorderList.Count;
        }
        public void Remove(ScriptRecorder scriptRecorder)
        {
            m_ScriptRecorderList.Remove(scriptRecorder);
            m_Count = m_ScriptRecorderList.Count;
        }
        public void RemoveAll()
        {
            ScriptRecorderList.ForEach(recorder => recorder.Stop());
            m_Count = m_ScriptRecorderList.Count;
        }


        // -------------------------------------------------- RECORDER METHODS ---------------------------------------------------------------------------
        internal void Record_AttackRequest(uint serial)
        {
            if (!Active()) { return; }
            ScriptRecorderList.ForEach(recorder => new Task(() => { recorder.Record_AttackRequest(serial); } ).Start());
        }

        internal void Record_ClientDoubleClick(Assistant.Serial ser)
        {
            if (!Active()) { return; }
            ScriptRecorderList.ForEach(recorder => new Task(() => { recorder.Record_ClientDoubleClick(ser); }).Start());
        }
        internal void Record_DropRequest(Assistant.Item i, Assistant.Serial dest)
        {
            if (!Active()) { return; }
            ScriptRecorderList.ForEach(recorder => new Task(() => { recorder.Record_DropRequest(i,dest); }).Start());
        }
        internal void Record_ClientTextCommand(int type, int id)
        {
            if (!Active()) { return; }
            ScriptRecorderList.ForEach(recorder => new Task(() => { recorder.Record_ClientTextCommand(type, id); }).Start());
        }
        internal void Record_EquipRequest(Assistant.Item item, Assistant.Layer l, Assistant.Mobile m)
        {
            if (!Active()) { return; }
            ScriptRecorderList.ForEach(recorder => new Task(() => { recorder.Record_EquipRequest(item, l, m); }).Start());
        }
        internal void Record_RenameMobile(int serial, string name)
        {
            if (!Active()) { return; }
            ScriptRecorderList.ForEach(recorder => new Task(() => { recorder.Record_RenameMobile(serial, name); }).Start());
        }
        internal void Record_AsciiPromptResponse(uint type, string text)
        {
            if (!Active()) { return; }
            ScriptRecorderList.ForEach(recorder => new Task(() => { recorder.Record_AsciiPromptResponse(type, text); }).Start());
        }
        internal void Record_UnicodeSpeech(MessageType type, string text, int hue)
        {
            if (!Active()) { return; }
            ScriptRecorderList.ForEach(recorder => new Task(() => { recorder.Record_UnicodeSpeech(type, text, hue); }).Start());
        }
        internal void Record_GumpsResponse(uint id, int operation)
        {
            if (!Active()) { return; }
            ScriptRecorderList.ForEach(recorder => new Task(() => { recorder.Record_GumpsResponse(id, operation); }).Start());
        }
        internal void Record_SADisarm()
        {
            if (!Active()) { return; }
            ScriptRecorderList.ForEach(recorder => new Task(() => { recorder.Record_SADisarm(); }).Start());
        }
        internal void Record_SAStun()
        {
            if (!Active()) { return; }
            ScriptRecorderList.ForEach(recorder => new Task(() => { recorder.Record_SAStun(); }).Start());
        }
        internal void Record_ContextMenuResponse(int serial, ushort idx)
        {
            if (!Active()) { return; }
            ScriptRecorderList.ForEach(recorder => new Task(() => { recorder.Record_ContextMenuResponse(serial, idx); }).Start());
        }
        internal void Record_ResponseStringQuery(byte yesno, string text)
        {
            if (!Active()) { return; }
            ScriptRecorderList.ForEach(recorder => new Task(() => { recorder.Record_ResponseStringQuery(yesno, text); }).Start());
        }
        internal void Record_MenuResponse(int index)
        {
            if (!Active()) { return; }
            ScriptRecorderList.ForEach(recorder => new Task(() => { recorder.Record_MenuResponse(index); }).Start());
        }
        internal void Record_Movement(Direction dir)
        {
            if (!Active()) { return; }
            ScriptRecorderList.ForEach(recorder => new Task(() => { recorder.Record_Movement(dir); }).Start());
        }
        internal void Record_Target(TargetInfo info)
        {
            if (!Active()) { return; }
            ScriptRecorderList.ForEach(recorder => new Task(() => { recorder.Record_Target(info); }).Start());
        }


    }

    public class ScriptRecorder
    {
        public ScriptRecorderOutput Output;

        private bool m_Recording = false;

        public bool IsRecording()
        { return m_Recording; }

        public void Start()
        {
            ScriptRecorderService.Instance.Add(this);
            m_Recording = true;
        }

        public void Stop()
        {
            ScriptRecorderService.Instance.Remove(this);
            m_Recording = false;
        }

        ~ScriptRecorder(){
            ScriptRecorderService.Instance.Remove(this);
        }

        internal void AddLog(string code)
        {
            if (m_Recording &&  Output != null) { Output(code); }
        }



        internal virtual void Record_AttackRequest(uint serial) { }
        internal virtual void Record_ClientDoubleClick(Assistant.Serial ser){ }
        internal virtual void Record_DropRequest(Assistant.Item i, Assistant.Serial dest){ }
        internal virtual void Record_ClientTextCommand(int type, int id){ }
        internal virtual void Record_EquipRequest(Assistant.Item item, Assistant.Layer l, Assistant.Mobile m){ }
        internal virtual void Record_RenameMobile(int serial, string name){ }
        internal virtual void Record_AsciiPromptResponse(uint type, string text){ }
        internal virtual void Record_UnicodeSpeech(MessageType type, string text, int hue){ }
        internal virtual void Record_GumpsResponse(uint id, int operation){ }
        internal virtual void Record_SADisarm(){ }
        internal virtual void Record_SAStun(){ }
        internal virtual void Record_ContextMenuResponse(int serial, ushort idx){ }
        internal virtual void Record_ResponseStringQuery(byte yesno, string text){ }
        internal virtual void Record_MenuResponse(int index){ }
        internal virtual void Record_Movement(Direction dir){ }
        internal virtual void Record_Target(TargetInfo info){ }
    }
    public class PyScriptRecorder : ScriptRecorder
    {
        internal override void Record_AttackRequest(uint serial)
        {
                AddLog("Player.Attack(0x" + serial.ToString("X8") + ")");

        }

        internal override void Record_ClientDoubleClick(Assistant.Serial ser)
        {
                if (ser.IsItem)
                    AddLog("Items.UseItem(0x" + ser.Value.ToString("X8") + ")");
                else
                    AddLog("Mobiles.UseMobile(0x" + ser.Value.ToString("X8") + ")");
        }

        internal override void Record_DropRequest(Assistant.Item i, Assistant.Serial dest)
        {
                if (dest != 0xFFFFFFFF)
                    AddLog("Items.Move(0x" + i.Serial.Value.ToString("X8") + ", 0x" + dest.Value.ToString("X8") + ", " + i.Amount + ")");
                else
                    AddLog("Items.DropItemGroundSelf(0x" + i.Serial.Value.ToString("X8") + ", " + i.Amount + ")");
        }
        /*internal static void Record_ClientSingleClick(Assistant.Serial ser)
        {
            int serint = ser;
            if (ser.IsItem)
                AddLog("Items.SingleClick(0x" + serint.ToString("X8") + ")");
            else
                AddLog("Mobiles.SingleClick(0x" + serint.ToString("X8") + ")");
        }
        */
        internal override void Record_ClientTextCommand(int type, int id)
        {
            if (type == 1) // Use Skill
            {
                switch (id)
                {
                    case 2:
                        AddLog("Player.UseSkill(\"Animal Lore\")");
                        break;

                    case 3:
                        AddLog("Player.UseSkill(\"Item ID\")");
                        break;

                    case 4:
                        AddLog("Player.UseSkill(\"Arms Lore\")");
                        break;

                    case 6:
                        AddLog("Player.UseSkill(\"Begging\")");
                        break;

                    case 9:
                        AddLog("Player.UseSkill(\"Peacemaking\")");
                        break;

                    case 12:
                        AddLog("Player.UseSkill(\"Cartography\")");
                        break;

                    case 14:
                        AddLog("Player.UseSkill(\"Detect Hidden\")");
                        break;

                    case 15:
                        AddLog("Player.UseSkill(\"Discordance\")");
                        break;

                    case 16:
                        AddLog("Player.UseSkill(\"Eval Int\")");
                        break;

                    case 19:
                        AddLog("Player.UseSkill(\"Forensics\")");
                        break;

                    case 21:
                        AddLog("Player.UseSkill(\"Hiding\")");
                        break;

                    case 22:
                        AddLog("Player.UseSkill(\"Provocation\")");
                        break;

                    case 30:
                        AddLog("Player.UseSkill(\"Poisoning\")");
                        break;

                    case 32:
                        AddLog("Player.UseSkill(\"Spirit Speak\")");
                        break;

                    case 33:
                        AddLog("Player.UseSkill(\"Stealing\")");
                        break;

                    case 35:
                        AddLog("Player.UseSkill(\"Animal Taming\")");
                        break;

                    case 36:
                        AddLog("Player.UseSkill(\"Taste ID\")");
                        break;

                    case 38:
                        AddLog("Player.UseSkill(\"Tracking\")");
                        break;

                    case 46:
                        AddLog("Player.UseSkill(\"Meditation\")");
                        break;

                    case 47:
                        AddLog("Player.UseSkill(\"Stealth\")");
                        break;

                    case 48:
                        AddLog("Player.UseSkill(\"Remove Trap\")");
                        break;

                    case 23:
                        AddLog("Player.UseSkill(\"Inscribe\")");
                        break;

                    case 1:
                        AddLog("Player.UseSkill(\"Anatomy\")");
                        break;

                    default:
                        break;
                }

            }
            else if (type == 2) // Cast Spell
            {
                Spell s = Spell.Get(id);
                if (id >= 1 && id <= 64)
                    AddLog("Spells.CastMagery(\"" + Utility.CapitalizeAllWords(Language.GetString(s.Name)) + "\")");
                else if (id >= 101 && id <= 117)
                    AddLog("Spells.CastNecro(\"" + Utility.CapitalizeAllWords(Language.GetString(s.Name)) + "\")");
                else if (id >= 201 && id <= 210)
                    AddLog("Spells.CastChivalry(\"" + Utility.CapitalizeAllWords(Language.GetString(s.Name)) + "\")");
                else if (id >= 401 && id <= 406)
                    AddLog("Spells.CastBushido(\"" + Utility.CapitalizeAllWords(Language.GetString(s.Name)) + "\")");
                else if (id >= 501 && id <= 508)
                    AddLog("Spells.CastNinjitsu(\"" + Utility.CapitalizeAllWords(Language.GetString(s.Name)) + "\")");
                else if (id >= 601 && id <= 616)
                    AddLog("Spells.CastSpellweaving(\"" + Utility.CapitalizeAllWords(Language.GetString(s.Name)) + "\")");
                else if (id >= 678 && id <= 693)
                    AddLog("Spells.CastMysticism(\"" + Utility.CapitalizeAllWords(Language.GetString(s.Name)) + "\")");
                else if (id >= 701 && id <= 745)
                {
                    if (id == 732)
                        AddLog("Spells.CastMastery(\"Called Shot\")");
                    else if (id == 715)
                        AddLog("Spells.CastMastery(\"Enchanted Summoning\")");
                    else
                        AddLog("Spells.CastMastery(\"" + Utility.CapitalizeAllWords(Language.GetString(s.Name)) + "\")");
                }
                else
                    AddLog("ERROR Spell not listed " + id);
            }
            else // InvokeVirtue
            {
                switch (id)
                {
                    case 1:
                        AddLog("Player.InvokeVirtue(\"Honor\")");
                        break;
                    case 2:
                        AddLog("Player.InvokeVirtue(\"Sacrifice\")");
                        break;
                    case 3:
                        AddLog("Player.InvokeVirtue(\"Valor\")");
                        break;
                    case 4:
                        AddLog("Player.InvokeVirtue(\"Compassion\")");
                        break;
                    case 5:
                        AddLog("Player.InvokeVirtue(\"Honesty\")");
                        break;
                    case 6:
                        AddLog("Player.InvokeVirtue(\"Humility\")");
                        break;
                    case 7:
                        AddLog("Player.InvokeVirtue(\"Justice\")");
                        break;
                    case 8:
                        AddLog("Player.InvokeVirtue(\"Spirituality\")");
                        break;
                }
            }
        }

        internal override void Record_EquipRequest(Assistant.Item item, Assistant.Layer l, Assistant.Mobile m)
        {
            if (m == World.Player)
                AddLog("Player.EquipItem(0x" + item.Serial.Value.ToString("X8") + ")");
            else
                AddLog("Player.UnEquipItemByLayer(" + l.ToString() + ")");
        }

        internal override void Record_RenameMobile(int serial, string name)
        {
            AddLog("Misc.PerRename(0x" + serial.ToString("X8") + ", " + name + " )");
        }

        internal override void Record_AsciiPromptResponse(uint type, string text)
        {
            AddLog("Misc.WaitForPrompt(10000)");
            if (type == 0)
                AddLog("Misc.WaitForPrompt(10000)");
            else
                AddLog("Misc.ResponsePrompt(\"" + text + "\")");
        }

        internal override void Record_UnicodeSpeech(MessageType type, string text, int hue)
        {
            switch (type)
            {
                case MessageType.Guild:
                    AddLog("Player.ChatGuild(\"" + text + "\")");
                    break;
                case MessageType.Alliance:
                    AddLog("Player.ChatAlliance(\"" + text + "\")");
                    break;
                case MessageType.Emote:
                    AddLog("Player.ChatEmote(" + hue + ", \"" + text + "\")");
                    break;
                case MessageType.Whisper:
                    AddLog("Player.ChatWhisper(" + hue + ", \"" + text + "\")");
                    break;
                case MessageType.Yell:
                    AddLog("Player.ChatYell(" + hue + ", \"" + text + "\")");
                    break;
                default:
                    AddLog("Player.ChatSay(" + hue + ", \"" + text + "\")");
                    break;
            }
        }

        internal override void Record_GumpsResponse(uint id, int operation)
        {
            AddLog("Gumps.WaitForGump(" + id + ", 10000)");
            AddLog("Gumps.SendAction(" + id + ", " + operation + ")");
        }

        internal override void Record_SADisarm()
        {
            AddLog("Player.WeaponDisarmSA( )");
        }

        internal override void Record_SAStun()
        {
            AddLog("Player.WeaponStunSA( )");
        }

        internal override void Record_ContextMenuResponse(int serial, ushort idx)
        {
            AddLog("Misc.WaitForContext(0x" + serial.ToString("X8") + ", 10000)");
            AddLog("Misc.ContextReply(0x" + serial.ToString("X8") + ", " + idx + ")");
        }

        internal override void Record_ResponseStringQuery(byte yesno, string text)
        {
            AddLog("Misc.WaitForQueryString(10000)");
            if (yesno != 0)
                AddLog("Misc.QueryStringResponse(True, " + text + ")");
            else
                AddLog("Misc.QueryStringResponse(False, " + text + ")");
        }

        internal override void Record_MenuResponse(int index)
        {
            AddLog("Misc.WaitForMenu(10000)");
            string text = string.Empty;
            try
            {
                text = World.Player.MenuEntry[index - 1].ModelText;
            }
            catch { }
            AddLog("Misc.MenuResponse(\"" + text + "\")");
        }

        internal override void Record_Movement(Direction dir)
        {
            if ((dir & Direction.running) == Direction.running)
            {
                switch (World.Player.Direction & Direction.mask)
                {
                    case Direction.north: AddLog("Player.Run(\"North\")"); break;
                    case Direction.south: AddLog("Player.Run(\"South\")"); break;
                    case Direction.west: AddLog("Player.Run(\"West\")"); break;
                    case Direction.east: AddLog("Player.Run(\"East\")"); break;
                    case Direction.right: AddLog("Player.Run(\"Right\")"); break;
                    case Direction.left: AddLog("Player.Run(\"Left\")"); break;
                    case Direction.down: AddLog("Player.Run(\"Down\")"); break;
                    case Direction.up: AddLog("Player.Run(\"Up\")"); break;
                    default: break;
                }
            }
            else
            {
                switch (World.Player.Direction & Direction.mask)
                {
                    case Direction.north: AddLog("Player.Walk(\"North\")"); break;
                    case Direction.south: AddLog("Player.Walk(\"South\")"); break;
                    case Direction.west: AddLog("Player.Walk(\"West\")"); break;
                    case Direction.east: AddLog("Player.Walk(\"East\")"); break;
                    case Direction.right: AddLog("Player.Walk(\"Right\")"); break;
                    case Direction.left: AddLog("Player.Walk(\"Left\")"); break;
                    case Direction.down: AddLog("Player.Walk(\"Down\")"); break;
                    case Direction.up: AddLog("Player.Walk(\"Up\")"); break;
                    default: break;
                }
            }
        }

        internal override void Record_Target(TargetInfo info)
        {
            AddLog("Target.WaitForTarget(10000, False)");
            if (info.X == 0xFFFF && info.X == 0xFFFF && (info.Serial <= 0 || info.Serial >= 0x80000000))
            {
                AddLog("Target.Cancel( )");
                return;
            }

            if (info.Serial == 0)
            {
                if (info.Gfx == 0)
                    AddLog("Target.TargetExecute(" + info.X + ", " + info.Y + " ," + info.Z + ")");
                else
                    AddLog("Target.TargetExecute(" + info.X + ", " + info.Y + " ," + info.Z + " ," + info.Gfx + ")");
            }
            else
                AddLog("Target.TargetExecute(" + info.Serial + ")");

        }
    }
    public class UosScriptRecorder : ScriptRecorder
    {
        internal override void Record_AttackRequest(uint serial)
        {
                AddLog("attack " + "0x" + serial.ToString("X8"));
        }

        internal override void Record_ClientDoubleClick(Assistant.Serial ser)
        {
                AddLog("useobject 0x" + ser.Value.ToString("X8"));
        }

        internal override void Record_DropRequest(Assistant.Item i, Assistant.Serial dest)
        {
                if (dest != 0xFFFFFFFF)
                    AddLog("moveitem 0x" + i.Serial.Value.ToString("X8") + " 0x" + dest.Value.ToString("X8") + " " + i.Amount);
                else
                    AddLog("moveitem 0x" + i.Serial.Value.ToString("X8") + " ground " + i.Amount);
        }
        /*internal static void Record_ClientSingleClick(Assistant.Serial ser)
        {
            int serint = ser;
            if (ser.IsItem)
                AddLog("Items.SingleClick(0x" + serint.ToString("X8") + ")");
            else
                AddLog("Mobiles.SingleClick(0x" + serint.ToString("X8") + ")");
        }
        */
        internal override void Record_ClientTextCommand(int type, int id)
        {
            if (type == 1) // Use Skill
            {
                string skillName = string.Empty;
                switch (id)
                {
                    case 2:
                        skillName = "Animal Lore";
                        break;

                    case 3:
                        skillName = "Item ID";
                        break;

                    case 4:
                        skillName = "Arms Lore";
                        break;

                    case 6:
                        skillName = "Begging";
                        break;

                    case 9:
                        skillName = "Peacemaking";
                        break;

                    case 12:
                        skillName = "Cartography";
                        break;

                    case 14:
                        skillName = "Detect Hidden";
                        break;

                    case 15:
                        skillName = "Discordance";
                        break;

                    case 16:
                        skillName = "Eval Int";
                        break;

                    case 19:
                        skillName = "Forensics";
                        break;

                    case 21:
                        skillName = "Hiding";
                        break;

                    case 22:
                        skillName = "Provocation";
                        break;

                    case 30:
                        skillName = "Poisoning";
                        break;

                    case 32:
                        skillName = "Spirit Speak";
                        break;

                    case 33:
                        skillName = "Stealing";
                        break;

                    case 35:
                        skillName = "Animal Taming";
                        break;

                    case 36:
                        skillName = "Taste ID";
                        break;

                    case 38:
                        skillName = "Tracking";
                        break;

                    case 46:
                        skillName = "Meditation";
                        break;

                    case 47:
                        skillName = "Stealth";
                        break;

                    case 48:
                        skillName = "Remove Trap";
                        break;

                    case 23:
                        skillName = "Inscribe";
                        break;

                    case 1:
                        skillName = "Anatomy";
                        break;

                    default:
                        break;
                }
                if (skillName != string.Empty)
                    AddLog(string.Format("useskill \"{0}\"", skillName));

            }
            else if (type == 2) // Cast Spell
            {
                Spell s = Spell.Get(id);
                if (s != null)
                {

                    if (id >= 1 && id <= 693)
                        AddLog("cast \"" + Language.GetString(s.Name) + "\"");
                    else if (id >= 701 && id <= 745)
                    {
                        if (id == 732)
                            AddLog("cast \"Called Shot\"");
                        else if (id == 715)
                            AddLog("cast \"Enchanted Summoning\"");
                        else
                            AddLog("cast \"" + Language.GetString(s.Name) + "\"");
                    }
                    else
                        AddLog("ERROR Spell not listed " + id);
                }
                else
                    AddLog("ERROR Spell not known " + id);
            }
            else // InvokeVirtue
            {
                string virtue = string.Empty;
                switch (id)
                {
                    case 1:
                        virtue ="Honor";
                        break;
                    case 2:
                        virtue ="Sacrifice";
                        break;
                    case 3:
                        virtue ="Valor";
                        break;
                    case 4:
                        virtue ="Compassion";
                        break;
                    case 5:
                        virtue ="Honesty";
                        break;
                    case 6:
                        virtue ="Humility";
                        break;
                    case 7:
                        virtue ="Justice";
                        break;
                    case 8:
                        virtue ="Spirituality";
                        break;
                }
                if (virtue != string.Empty)
                    AddLog("virtue \"" + virtue + "\"");
            }

        }

        internal override void Record_EquipRequest(Assistant.Item item, Assistant.Layer l, Assistant.Mobile m)
        {
            if (m == World.Player)
                AddLog(string.Format("equipitem \"0x{0}\"", item.Serial.Value.ToString("X8")));
            else
                AddLog(string.Format("unequipitem \"{0}\"", l.ToString()));
        }

        internal override void Record_RenameMobile(int serial, string name)
        {
            AddLog(string.Format("rename 0x{0} \"{1}\"", serial.ToString("X8"), name));
        }

        internal override void Record_AsciiPromptResponse(uint type, string text)
        {
            //AddLog("waitforprompt 10000");
            if (type == 0)
                AddLog("waitforprompt 10000");
            else
                AddLog("promptmsg \"" + text + "\"");
        }

        internal override void Record_UnicodeSpeech(MessageType type, string text, int hue)
        {
            switch (type)
            {
                case MessageType.Guild:
                    AddLog("guildmsg \"" + text + "\"");
                    break;
                case MessageType.Alliance:
                    AddLog("allymsg \"" + text + "\"");
                    break;
                case MessageType.Emote:
                    AddLog("emotemsg \"" + text + "\"");
                    break;
                case MessageType.Whisper:
                    AddLog("whispermsg \"" + text + "\"");
                    break;
                case MessageType.Yell:
                    AddLog("yellmsg \"" + text + "\"");
                    break;
                default:
                    AddLog("msg \"" + text + "\"");
                    break;
            }
        }

        internal override void Record_GumpsResponse(uint id, int operation)
        {
            AddLog("waitforgump " + id + " 10000");
            AddLog("replygump " + id + " " + operation);
        }

        internal override void Record_SADisarm()
        {
            AddLog("disarm");
        }

        internal override void Record_SAStun()
        {
            AddLog("stun");
        }

        internal override void Record_ContextMenuResponse(int serial, ushort idx)
        {
            AddLog("waitforcontext 0x" + serial.ToString("X8") + " 10000");
            AddLog("contextmenu 0x" + serial.ToString("X8") + " " + idx);
        }

        internal override void Record_ResponseStringQuery(byte yesno, string text)
        {
            AddLog("Misc.WaitForQueryString(10000)");
            if (yesno != 0)
                AddLog("Misc.QueryStringResponse(True, " + text + ")");
            else
                AddLog("Misc.QueryStringResponse(False, " + text + ")");
        }

        internal override void Record_MenuResponse(int index)
        {
            AddLog("Misc.WaitForMenu(10000)");
            string text = string.Empty;
            try
            {
                text = World.Player.MenuEntry[index - 1].ModelText;
            }
            catch { }
            AddLog("Misc.MenuResponse(\"" + text + "\")");
        }

        internal override void Record_Movement(Direction dir)
        {
            if ((dir & Direction.running) == Direction.running)
            {
                switch (World.Player.Direction & Direction.mask)
                {
                    case Direction.north: AddLog("run \"North\""); break;
                    case Direction.south: AddLog("run \"South\""); break;
                    case Direction.west: AddLog("run \"West\""); break;
                    case Direction.east: AddLog("run \"East\""); break;
                    case Direction.right: AddLog("run \"Right\""); break;
                    case Direction.left: AddLog("run \"Left\""); break;
                    case Direction.down: AddLog("run \"Down\""); break;
                    case Direction.up: AddLog("run \"Up\""); break;
                    default: break;
                }
            }
            else
            {
                switch (World.Player.Direction & Direction.mask)
                {
                    case Direction.north: AddLog("walk \"North\""); break;
                    case Direction.south: AddLog("walk \"South\""); break;
                    case Direction.west: AddLog("walk \"West\""); break;
                    case Direction.east: AddLog("walk \"East\""); break;
                    case Direction.right: AddLog("walk \"Right\""); break;
                    case Direction.left: AddLog("walk \"Left\""); break;
                    case Direction.down: AddLog("walk \"Down\""); break;
                    case Direction.up: AddLog("walk \"Up\""); break;
                    default: break;
                }
            }
        }

        internal override void Record_Target(TargetInfo info)
        {
            AddLog("waitfortarget 10000");
            if (info.X == 0xFFFF && info.X == 0xFFFF && (info.Serial <= 0 || info.Serial >= 0x80000000))
            {
                AddLog("canceltarget");
                return;
            }

            if (info.Serial == 0)
            {
                if (info.Gfx == 0)
                    AddLog("targettile " + info.X + " " + info.Y + " " + info.Z );
                else
                    AddLog("targettile " + info.X + " " + info.Y + " " + info.Z + " " + info.Gfx);
            }
            else
                AddLog("target 0x" + ((int)info.Serial).ToString("X8"));

        }

    }

    public class CsScriptRecorder : ScriptRecorder
    {
        // no recorder for CS yet
    }

}
