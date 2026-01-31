using RazorEnhanced.Macros.Actions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RazorEnhanced.Macros
{
    public class Macro
    {
        public string Name { get; set; }
        public Keys Hotkey { get; set; }
        public bool Loop { get; set; }
        public List<MacroAction> Actions { get; set; }
        public bool IsRunning { get; private set; }

        private int currentActionIndex = 0;
        private bool shouldStop = false;

        // Event to notify when macro state changes
        public event EventHandler StateChanged;

        public Macro()
        {
            Actions = new List<MacroAction>();
            Name = "New Macro";
            Hotkey = Keys.None;
            Loop = false;
        }

        protected virtual void OnStateChanged()
        {
            StateChanged?.Invoke(this, EventArgs.Empty);
        }

        public void Play()
        {
            if (IsRunning)
                return;

            IsRunning = true;
            OnStateChanged();

            Task.Run(() =>
            {
                try
                {
                    do
                    {
                        ExecuteActions();

                        if (Loop && IsRunning)
                        {
                            Misc.Pause(100); // Small delay between loops
                        }
                    } while (Loop && IsRunning);
                }
                catch (Exception ex)
                {
                    Misc.SendMessage($"Macro error: {ex.Message}", 33);
                }
                finally
                {
                    IsRunning = false;
                    OnStateChanged();
                }
            });
        }

        private void ExecuteActions()
        {
            Stack<int> whileStack = new Stack<int>();

            bool skipToEndIf = false;  // Track if we should skip ElseIf/Else blocks
            Stack<(int forIndex, ForAction forAction)> forLoopStack = new Stack<(int, ForAction)>(); // Track nested For loops

            for (int i = 0; i < Actions.Count && IsRunning; i++)
            {
                var action = Actions[i];

                // Handle For loops
                if (action is RazorEnhanced.Macros.Actions.ForAction forAction)
                {
                    if (forAction.CurrentIteration == 0)
                    {
                        // First time entering this For loop
                        forAction.Execute(); // Increments CurrentIteration to 1
                        forLoopStack.Push((i, forAction));
                        continue;
                    }
                    else if (forAction.ShouldContinue())
                    {
                        // Continue looping
                        forAction.Execute(); // Increment counter
                        continue;
                    }
                    else
                    {
                        // Loop complete, skip to EndFor
                        forAction.Reset();
                        i = FindMatchingEndFor(i);
                        if (i == -1)
                        {
                            Misc.SendMessage("Error: No matching EndFor found", 33);
                            break;
                        }
                        continue;
                    }
                }
                // Handle EndFor
                else if (action is RazorEnhanced.Macros.Actions.EndForAction)
                {
                    if (forLoopStack.Count > 0)
                    {
                        var (forIndex, currentFor) = forLoopStack.Peek();

                        if (currentFor.ShouldContinue())
                        {
                            // Jump back to the For statement
                            i = forIndex - 1; // -1 because loop will increment
                            continue;
                        }
                        else
                        {
                            // Loop complete, pop from stack and continue
                            forLoopStack.Pop();
                            currentFor.Reset();
                            continue;
                        }
                    }
                    else
                    {
                        Misc.SendMessage("Error: EndFor without matching For", 33);
                        continue;
                    }
                }
                // Handle If statements
                else if (action is RazorEnhanced.Macros.Actions.IfAction ifAction)
                {
                    ifAction.Execute();

                    if (ifAction.ConditionResult)
                    {
                        // If TRUE: execute block, then skip any ElseIf/Else
                        skipToEndIf = true;
                        continue;
                    }
                    else
                    {
                        // If FALSE: jump to ElseIf/Else/EndIf and evaluate them
                        skipToEndIf = false;
                        i = FindNextElseIfElseOrEndIf(i);
                        if (i == -1)
                        {
                            Misc.SendMessage("Error: No matching EndIf found", 33);
                            break;
                        }
                        i--; // Decrement because loop will increment
                    }
                }
                // Handle ElseIf
                else if (action is RazorEnhanced.Macros.Actions.ElseIfAction elseIfAction)
                {
                    if (skipToEndIf)
                    {
                        // Previous If/ElseIf was TRUE, skip to EndIf
                        i = FindMatchingEndIf(i);
                        if (i == -1)
                        {
                            Misc.SendMessage("Error: No matching EndIf found", 33);
                            break;
                        }
                        continue; // Continue after EndIf
                    }
                    else
                    {
                        // Previous If/ElseIf was FALSE, evaluate this ElseIf
                        elseIfAction.Execute();

                        if (elseIfAction.ConditionResult)
                        {
                            // ElseIf TRUE: execute block, then skip remaining ElseIf/Else
                            skipToEndIf = true;
                            continue;
                        }
                        else
                        {
                            // ElseIf FALSE: jump to next ElseIf/Else/EndIf
                            i = FindNextElseIfElseOrEndIf(i);
                            if (i == -1)
                            {
                                Misc.SendMessage("Error: No matching EndIf found", 33);
                                break;
                            }
                            i--; // Decrement because loop will increment
                        }
                    }
                }
                // Handle Else
                else if (action is RazorEnhanced.Macros.Actions.ElseAction)
                {
                    if (skipToEndIf)
                    {
                        // Previous If/ElseIf was TRUE, skip to EndIf
                        i = FindMatchingEndIf(i);
                        if (i == -1)
                        {
                            Misc.SendMessage("Error: No matching EndIf found", 33);
                            break;
                        }
                        continue; // Continue after EndIf
                    }
                    else
                    {
                        // Previous If/ElseIf were all FALSE, execute Else block
                        // Just continue to next action (which is inside Else block)
                        continue;
                    }
                }
                // Handle EndIf
                else if (action is RazorEnhanced.Macros.Actions.EndIfAction)
                {
                    // Reset flag when exiting If block
                    skipToEndIf = false;
                    continue;
                }
                // Handle While
                else if (action is RazorEnhanced.Macros.Actions.WhileAction whileAction)
                {
                    // Evaluate the while condition using the same logic as IfAction
                    var tempIf = new RazorEnhanced.Macros.Actions.IfAction(
                        whileAction.Type, whileAction.Op, whileAction.Value, whileAction.Graphic, whileAction.Color,
                        whileAction.SkillName, whileAction.ValueToken, whileAction.BooleanValue, whileAction.PresetName,
                        whileAction.BuffName, whileAction.StatType, whileAction.StatusType, whileAction.RangeMode,
                        whileAction.RangeSerial, whileAction.RangeGraphic, whileAction.RangeColor,
                        whileAction.FindEntityMode, whileAction.FindEntityLocation, whileAction.FindContainerSerial,
                        whileAction.FindRange, whileAction.FindStoreSerial
                    );
                    tempIf.Execute();

                    if (tempIf.ConditionResult)
                    {
                        // Condition true: push this While index and continue into the loop
                        whileStack.Push(i);
                        continue;
                    }
                    else
                    {
                        // Condition false: skip to matching EndWhile
                        int endWhileIndex = FindMatchingEndWhile(i);
                        if (endWhileIndex == -1)
                        {
                            Misc.SendMessage("Error: No matching EndWhile found", 33);
                            break;
                        }
                        i = endWhileIndex; // Will increment to next after EndWhile
                        continue;
                    }
                }
                // Handle EndWhile
                else if (action is RazorEnhanced.Macros.Actions.EndWhileAction)
                {
                    if (whileStack.Count > 0)
                    {
                        int whileIndex = whileStack.Peek();

                        // Re-evaluate the While condition
                        var loopWhileAction = Actions[whileIndex] as RazorEnhanced.Macros.Actions.WhileAction;
                        var tempIf = new RazorEnhanced.Macros.Actions.IfAction(
                            loopWhileAction.Type, loopWhileAction.Op, loopWhileAction.Value, loopWhileAction.Graphic, loopWhileAction.Color,
                            loopWhileAction.SkillName, loopWhileAction.ValueToken, loopWhileAction.BooleanValue, loopWhileAction.PresetName,
                            loopWhileAction.BuffName, loopWhileAction.StatType, loopWhileAction.StatusType, loopWhileAction.RangeMode,
                            loopWhileAction.RangeSerial, loopWhileAction.RangeGraphic, loopWhileAction.RangeColor,
                            loopWhileAction.FindEntityMode, loopWhileAction.FindEntityLocation, loopWhileAction.FindContainerSerial,
                            loopWhileAction.FindRange, loopWhileAction.FindStoreSerial
                        );
                        tempIf.Execute();

                        if (tempIf.ConditionResult)
                        {
                            // Loop again: jump back to While
                            i = whileIndex - 1; // -1 because for loop will increment
                            continue;
                        }
                        else
                        {
                            // Exit loop: pop stack and continue after EndWhile
                            whileStack.Pop();
                            continue;
                        }
                    }
                    else
                    {
                        Misc.SendMessage("Error: EndWhile without matching While", 33);
                        continue;
                    }
                }
                else
                {
                    // Normal action execution
                    action.Execute();
                    Misc.Pause(action.GetDelay());
                }

                if (!IsRunning)
                    break;
            }

            // Clean up any remaining For loops
            while (forLoopStack.Count > 0)
            {
                var (_, forAction) = forLoopStack.Pop();
                forAction.Reset();
            }
        }

        // Add this new helper method for For loops
        private int FindMatchingEndFor(int startIndex)
        {
            int depth = 1; // We're inside one For block

            for (int i = startIndex + 1; i < Actions.Count; i++)
            {
                if (Actions[i] is RazorEnhanced.Macros.Actions.ForAction)
                {
                    depth++; // Nested For
                }
                else if (Actions[i] is RazorEnhanced.Macros.Actions.EndForAction)
                {
                    depth--;
                    if (depth == 0)
                    {
                        return i; // Found matching EndFor
                    }
                }
            }

            return -1; // No matching EndFor found
        }

        // UPDATED to handle If/ElseIf/Else/EndIf - called when a condition branch is TRUE
        private int FindMatchingEndIf(int startIndex)
        {
            int depth = 1; // We're inside one If block

            for (int i = startIndex + 1; i < Actions.Count; i++)
            {
                if (Actions[i] is RazorEnhanced.Macros.Actions.IfAction)
                {
                    depth++; // Nested If
                }
                else if (Actions[i] is RazorEnhanced.Macros.Actions.EndIfAction)
                {
                    depth--;
                    if (depth == 0)
                    {
                        return i; // Found matching EndIf
                    }
                }
            }

            return -1; // No matching EndIf found
        }

        // UPDATED to find next ElseIf, Else, or EndIf at the SAME depth level
        private int FindNextElseIfElseOrEndIf(int startIndex)
        {
            int depth = 1; // We're inside one If block

            for (int i = startIndex + 1; i < Actions.Count; i++)
            {
                if (Actions[i] is RazorEnhanced.Macros.Actions.IfAction)
                {
                    depth++; // Nested If
                }
                else if (Actions[i] is RazorEnhanced.Macros.Actions.ElseIfAction && depth == 1)
                {
                    // Found ElseIf at same level
                    return i;
                }
                else if (Actions[i] is RazorEnhanced.Macros.Actions.ElseAction && depth == 1)
                {
                    // Found Else at same level
                    return i;
                }
                else if (Actions[i] is RazorEnhanced.Macros.Actions.EndIfAction)
                {
                    depth--;
                    if (depth == 0)
                    {
                        return i; // Found matching EndIf
                    }
                }
            }

            return -1; // No matching EndIf found (error condition)
        }

        public void Stop()
        {
            shouldStop = true;
            IsRunning = false;
            OnStateChanged();
        }

        private void ExecuteMacro()
        {
            try
            {
                do
                {
                    for (currentActionIndex = 0; currentActionIndex < Actions.Count; currentActionIndex++)
                    {
                        if (shouldStop) break;

                        var action = Actions[currentActionIndex];
                        if (!action.IsValid()) continue;

                        try
                        {
                            action.Execute();

                            int delay = action.GetDelay();
                            if (delay > 0)
                            {
                                System.Threading.Thread.Sleep(delay);
                            }
                        }
                        catch (Exception ex)
                        {
                            Misc.SendMessage($"Macro error: {ex.Message}", 33);
                            shouldStop = true;
                            break;
                        }
                    }

                    // If not looping, break after first iteration
                    if (!Loop) break;

                } while (!shouldStop);
            }
            finally
            {
                // Always set IsRunning to false when macro completes
                IsRunning = false;
                Misc.SendMessage($"Macro '{Name}' stopped.", 88);

                // Notify UI that state changed
                OnStateChanged();
            }
        }

        private int FindMatchingEndWhile(int startIndex)
        {
            int depth = 1;
            for (int i = startIndex + 1; i < Actions.Count; i++)
            {
                if (Actions[i] is RazorEnhanced.Macros.Actions.WhileAction)
                    depth++;
                else if (Actions[i] is RazorEnhanced.Macros.Actions.EndWhileAction)
                {
                    depth--;
                    if (depth == 0)
                        return i;
                }
            }
            return -1;
        }
        public string Serialize()
        {
            var sb = new System.Text.StringBuilder();
            sb.AppendLine($"Name:{Name}");
            sb.AppendLine($"Hotkey:{(int)Hotkey}");
            sb.AppendLine($"Loop:{Loop}");
            sb.AppendLine("Actions:");

            foreach (var action in Actions)
            {
                sb.AppendLine(action.Serialize());
            }

            return sb.ToString();
        }
    }
}