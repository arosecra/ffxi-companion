
using FFXICompanion.Settings;
using FFXICompanion;
using System;
using System.Text;
using System.Threading;
using System.Collections.Generic;
using Core_Interception.Lib;
using SharpDX.XInput;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace FFXICompanion.KeyMapper
{

    public class Mapper
    {
        static float LDEADZONEX = short.MaxValue * .3f;
        static float LDEADZONEY = short.MaxValue * .3f;
        static float RDEADZONEX = short.MaxValue * .3f;
        static float RDEADZONEY = short.MaxValue * .3f;
        private IntPtr deviceContext;
        public Mapper()
        {
            this.deviceContext = ManagedWrapper.CreateContext();
        }
        
        // public static void GetVidPid(string str, ref int vid, ref int pid)
        // {
        //     var matches = Regex.Matches(str, @"VID_(\w{4})&PID_(\w{4})");
        //     if (matches.Count <= 0 || matches[0].Groups.Count <= 1) return;
        //     vid = Convert.ToInt32(matches[0].Groups[1].Value, 16);
        //     pid = Convert.ToInt32(matches[0].Groups[2].Value, 16);
        // }

        public void start()
        {
            // var stroke = new ManagedWrapper.Stroke();
            // stroke.key.code = (ushort)Keys.J;
            // stroke.key.state = (ushort)ManagedWrapper.KeyState.Down;
            // int devId = 1;
            // ManagedWrapper.Send(_deviceContext, devId, ref stroke, 1);

            //use this code to determine keys
            // ManagedWrapper.SetFilter(deviceContext, ManagedWrapper.IsKeyboard, ManagedWrapper.Filter.All);
            // var stroke = new ManagedWrapper.Stroke();
            // int device = 0;
            // while (ManagedWrapper.Receive(deviceContext, device = ManagedWrapper.Wait(deviceContext), ref stroke, 1) > 0)
            // {
            //     if(ManagedWrapper.IsKeyboard(device) > 0) {
            //         Console.WriteLine(stroke.key.code);
            //         if(stroke.key.code == 1) 
            //             break;
            //     }
            // }
            // ManagedWrapper.SetFilter(deviceContext, ManagedWrapper.IsKeyboard, ManagedWrapper.Filter.None);

            // var ret = new List<DeviceInfo>();
            for (var i = 1; i < 21; i++)
            {
                var handle = ManagedWrapper.GetHardwareStr(deviceContext, i, 1000);
                if (handle == "") continue;
                // int foundVid = 0, foundPid = 0;
                // GetVidPid(handle, ref foundVid, ref foundPid);
                //if (foundVid == 0 || foundPid == 0) continue;
                // Console.WriteLine(i + " " + handle);
                // ret.Add(new DeviceInfo {Id = i, IsMouse = i > 10, Handle = handle});
            }

            // foreach (var device in ret)
            // {
            //     Console.WriteLine(device);
            // }


            var controllers = new[] { new Controller(), new Controller(UserIndex.One), new Controller(UserIndex.Two), new Controller(UserIndex.Three), new Controller(UserIndex.Four) };

            // Get 1st controller available
            Controller controller = controllers[0];
            foreach (var selectControler in controllers)
            {
                // Console.WriteLine(selectControler);
                if (selectControler.IsConnected)
                {
                    controller = selectControler;
                    break;
                }
            }

            if (controller == null)
            {
                Console.WriteLine("No XInput controller installed");
            }
            else
            {
                Console.WriteLine("KeyMapper loaded");
                // Console.WriteLine("Found a XInput controller available");
                // Console.WriteLine("Press buttons on the controller to display events or escape key to exit... ");

                // Poll events from joystick
                SharpDX.XInput.State previousControllerState = controller.GetState();
                Dictionary<Button, bool> lastSimpleGamepadState = determineSimpleButtonState(previousControllerState);

                StateTransition lastState = null;

                while (!ModuleData.getInstance().cancellationToken.IsCancellationRequested)
                {
                    if(controller.IsConnected) {
                    SharpDX.XInput.State controllerState = controller.GetState();
                    if (previousControllerState.PacketNumber != controllerState.PacketNumber)
                    {
                        // Console.WriteLine(controllerState.Gamepad);

                        Dictionary<Button, bool> simpleGamepadState = determineSimpleButtonState(controllerState);
                        Dictionary<Button, Settings.Action> changedState = determineStateDifferences(lastSimpleGamepadState, simpleGamepadState);
                        printStateChanges(changedState);
                        // Console.WriteLine(GetActiveWindowTitle());

                        //determine if we are transitioning to a new 'state'
                        //  this is based on the current state, the game state and the keys pressed/not pressed
                        //  NOTE: the first state wins, for speed
                        StateTransition newState = getNewState(simpleGamepadState, ModuleData.getInstance().companionSettings.stateTransitions);
                        if(newState == null && lastState == null)
                        {
                            //nothing to do
                        } 
                        else if(newState != null && lastState == null)
                        {
                            activateState(newState);
                        }
                        else if(newState == null && lastState != null)
                        {
                            deactivateState(lastState);
                        }
                        else if(newState != null && lastState != null && !newState.transitionName.Equals(lastState.transitionName))
                        {
                            deactivateState(lastState);
                            activateState(newState);
                        }

                        //now that we have the state name determined, load the correct mappings
                        StateControllerMapping stateControllerMappings = findStateControllerMappings(newState, ModuleData.getInstance().companionSettings.stateMappings);

                        //apply button presses and such 
                        foreach (Button key in changedState.Keys)
                        {
                            Settings.Action action = changedState[key];
                            foreach (ControllerMapping controllerMapping in stateControllerMappings.controllerMappings)
                            {
                                if (controllerMapping.button.button == key && action == controllerMapping.button.action)
                                {
                                    handleCommand(controllerMapping);

                                }
                            }
                        }

                        lastState = newState;
                        lastSimpleGamepadState = simpleGamepadState;
                    }
                    Thread.Sleep(10);
                    previousControllerState = controllerState;
                    }

                }
            }
        }

        private void deactivateState(StateTransition transition)
        {
            Console.WriteLine("Deactivating state " + transition.stateName + " using transition " + transition.transitionName);
            if(transition.key != Settings.Key.NULL)
                handleKeyPress(new KeyPress(transition.key, Settings.Action.RELEASED)); 
         
        }

        private void activateState(StateTransition transition)
        {
            Console.WriteLine("Activating state " + transition.stateName + " using transition " + transition.transitionName);
            if(transition.key != Settings.Key.NULL)
                handleKeyPress(new KeyPress(transition.key, Settings.Action.PRESSED));
        }

        [DllImport("user32.dll")]
        static extern int GetWindowText(IntPtr hWnd, StringBuilder text, int count);
        private string GetActiveWindowTitle()
        {
            const int nChars = 256;
            StringBuilder Buff = new StringBuilder(nChars);
            IntPtr handle = WinApi.User32.User32Methods.GetForegroundWindow();

            if (WinApi.User32.User32Methods.GetWindowText(handle, Buff, nChars) > 0)
            {
                return Buff.ToString();
            }
            return null;
        }

        private void handleKeyPressesCommand(KeyPressesCommand command)
        {
            foreach (KeyPress kp in command.keyPresses)
            {
                handleKeyPress(kp);
            }
        }

        private void handleKeyPress(KeyPress kp)
        {
            var stroke = new ManagedWrapper.Stroke();
            stroke.key.code = (ushort)kp.key;
            if (kp.action == Settings.Action.PRESSED)
                stroke.key.state = (ushort)ManagedWrapper.KeyState.Down;
            else
                stroke.key.state = (ushort)ManagedWrapper.KeyState.Up;
            int devId = 4;
            // Console.WriteLine(stroke.key.code + " " + kp.action);
            ManagedWrapper.Send(deviceContext, devId, ref stroke, 1);
            System.Threading.Thread.Sleep(10);
        }

        private void handleCommand(ControllerMapping mapping)
        {
            if (mapping.keyPressesCommand != null)
            {
                handleKeyPressesCommand(mapping.keyPressesCommand);
            } else if (mapping.altTabCommand != null) {

                
                //List<Process> polProcesses = new List<Process>();
                //foreach (Process process in Process.GetProcesses())
               // {
                //    if(process.ProcessName == "pol") {
                 //       polProcesses.Add(process);
                //    }
                //}
                //IntPtr s = polProcesses[0].MainWindowHandle;
                //Console.WriteLine(polProcesses[0].MainWindowTitle);
                
                //WinApi.User32.User32Methods.ShowWindow(s, WinApi.User32.ShowWindowCommands.SW_SHOWMINIMIZED  );
                //WinApi.User32.User32Methods.ShowWindow(s, WinApi.User32.ShowWindowCommands.SW_SHOWNORMAL  );

                // IntPtr activeWindow = WinApi.User32.User32Methods.GetForegroundWindow();
                // int currentIndex = -1;
                // for(int i = 0; i < polProcesses.Count; i++) {
                //     Process p = polProcesses[i];
                //     if(p.Handle == activeWindow) {
                //         currentIndex = i;
                //     }
                // }
                // currentIndex++;
                // Console.WriteLine(currentIndex);
                // currentIndex %= polProcesses.Count;
                // Console.WriteLine(currentIndex);

                // // IntPtr s = process.MainWindowHandle;
                // // SetForegroundWindow(s);
                // // WinApi.User32.User32Methods.BringWindowToTop(polProcesses[currentIndex].Handle);
                // IntPtr newActiveWindow = polProcesses[currentIndex].Handle;
                
                // if (newActiveWindow != activeWindow) {
                //     uint thread1 = WinApi.User32.User32Methods.GetWindowThreadProcessId(activeWindow, IntPtr.Zero);
                //     uint thread2 = WinApi.Kernel32.Kernel32Methods.GetCurrentThreadId();

                //     if (thread1 != thread2)
                //     {
                //         bool attachThread = WinApi.User32.User32Methods.AttachThreadInput(thread1, thread2, true);
                //         bool bringToTop = WinApi.User32.User32Methods.BringWindowToTop(newActiveWindow);
                //         if (WinApi.User32.User32Methods.IsIconic(newActiveWindow))
                //         {
                //             bool showWindow = WinApi.User32.User32Methods.ShowWindow(newActiveWindow, WinApi.User32.ShowWindowCommands.SW_SHOWNORMAL);
                //         }
                //         else
                //         {
                //             bool showWindow = WinApi.User32.User32Methods.ShowWindow(newActiveWindow, WinApi.User32.ShowWindowCommands.SW_SHOW);
                //         }
                //         attachThread = WinApi.User32.User32Methods.AttachThreadInput(thread1, thread2, false);
                //     }
                //     else
                //     {
                //         WinApi.User32.User32Methods.SetForegroundWindow(newActiveWindow);
                //     }
                //     if (WinApi.User32.User32Methods.IsIconic(newActiveWindow))
                //     {
                //         WinApi.User32.User32Methods.ShowWindow(newActiveWindow, WinApi.User32.ShowWindowCommands.SW_SHOWNORMAL);
                //     }
                //     else
                //     {
                //         WinApi.User32.User32Methods.ShowWindow(newActiveWindow, WinApi.User32.ShowWindowCommands.SW_SHOW);
                //     }
                // }

            }
        }

        private StateControllerMapping findStateControllerMappings(StateTransition stateTransition, List<StateControllerMapping> stateMappings)
        {
            StateControllerMapping mapping = null;
            if(stateTransition == null)
            {
                foreach (StateControllerMapping current in stateMappings)
                {
                    if (current.stateName == "Default")
                    {
                        mapping = current;
                        break;
                    }
                }
            }
            else
            {
                foreach (StateControllerMapping current in stateMappings)
                {
                    if (current.stateName == stateTransition.stateName)
                    {
                        mapping = current;
                    }
                }
            }

            return mapping;
        }

        private void printStateChanges(Dictionary<Button, Settings.Action> changedState)
        {
            if (changedState.Count > 0)
            {
                foreach (Button key in changedState.Keys)
                {
                    Console.WriteLine(key + " -> " + changedState[key]);
                }
            }
        }

        private Dictionary<Button, Settings.Action> determineStateDifferences(Dictionary<Button, bool> lastSimpleState, Dictionary<Button, bool> currentSimpleState)
        {
            Dictionary<Button, Settings.Action> result = new Dictionary<Button, Settings.Action>();
            foreach (Button key in lastSimpleState.Keys)
            {
                bool currentValue = currentSimpleState[key];
                bool lastValue = lastSimpleState[key];
                if (currentValue != lastValue)
                {
                    if (!currentValue)
                        result.Add(key, Settings.Action.RELEASED);
                    else
                        result.Add(key, Settings.Action.PRESSED);
                }
            }

            return result;
        }

        [DllImport("xinput1_3.dll", EntryPoint = "#100")]
        static extern int secret_get_gamepad(int playerIndex, out XINPUT_GAMEPAD_SECRET struc);

        [DllImport("xinput1_3.dll")]
        static extern int XInputGetState(int playerIndex, out XINPUT_GAMEPAD_SECRET struc);

        public struct XINPUT_GAMEPAD_SECRET
        {
                public UInt32 eventCount;
                public ushort wButtons;
                public Byte bLeftTrigger;
                public Byte bRightTrigger;
                public short sThumbLX;
                public short sThumbLY;
                public short sThumbRX;
                public short sThumbRY;
        }
        public XINPUT_GAMEPAD_SECRET xgs;


        private Dictionary<Button, bool> determineSimpleButtonState(SharpDX.XInput.State controllerState)
        {
            Dictionary<Button, bool> result = new Dictionary<Button, bool>();
            setButtonState(result, Button.LB,    (controllerState.Gamepad.Buttons & GamepadButtonFlags.LeftShoulder) != 0);
            setButtonState(result, Button.RB,    (controllerState.Gamepad.Buttons & GamepadButtonFlags.RightShoulder) != 0);
            setButtonState(result, Button.X,     (controllerState.Gamepad.Buttons & GamepadButtonFlags.X) != 0);
            setButtonState(result, Button.Y,     (controllerState.Gamepad.Buttons & GamepadButtonFlags.Y) != 0);
            setButtonState(result, Button.A,     (controllerState.Gamepad.Buttons & GamepadButtonFlags.A) != 0);
            setButtonState(result, Button.B,     (controllerState.Gamepad.Buttons & GamepadButtonFlags.B) != 0);
            setButtonState(result, Button.LS,    (controllerState.Gamepad.Buttons & GamepadButtonFlags.LeftThumb) != 0);
            setButtonState(result, Button.RS,    (controllerState.Gamepad.Buttons & GamepadButtonFlags.RightThumb) != 0);
            setButtonState(result, Button.DU,    (controllerState.Gamepad.Buttons & GamepadButtonFlags.DPadUp) != 0);
            setButtonState(result, Button.DD,    (controllerState.Gamepad.Buttons & GamepadButtonFlags.DPadDown) != 0);
            setButtonState(result, Button.DL,    (controllerState.Gamepad.Buttons & GamepadButtonFlags.DPadLeft) != 0);
            setButtonState(result, Button.DR,    (controllerState.Gamepad.Buttons & GamepadButtonFlags.DPadRight) != 0);
            setButtonState(result, Button.START, (controllerState.Gamepad.Buttons & GamepadButtonFlags.Start) != 0);
            setButtonState(result, Button.BACK,  (controllerState.Gamepad.Buttons & GamepadButtonFlags.Back) != 0);

            setJoystickButtonState(result, "LS", "L", "R", (float)controllerState.Gamepad.LeftThumbX, LDEADZONEX);
            setJoystickButtonState(result, "LS", "D", "U", (float)controllerState.Gamepad.LeftThumbY, LDEADZONEY);
            setJoystickButtonState(result, "RS", "L", "R", (float)controllerState.Gamepad.RightThumbX, RDEADZONEX);
            setJoystickButtonState(result, "RS", "D", "U", (float)controllerState.Gamepad.RightThumbY, RDEADZONEY);

            setTriggerButtonState(result, Button.LT, (float)controllerState.Gamepad.LeftTrigger);
            setTriggerButtonState(result, Button.RT, (float)controllerState.Gamepad.RightTrigger);

            int stat = secret_get_gamepad(0, out xgs);
            // Console.WriteLine(xgs.wButtons);
            if (stat == 0) {
                bool value = ((xgs.wButtons & 0x0400) != 0);
                setButtonState(result, Button.GUIDE, value);
            }

            

            return result;
        }

        private void setTriggerButtonState(Dictionary<Button, bool> simpleButtonState, Button button, float value)
        {
            setButtonState(simpleButtonState, button, (value / 255f) > 0.05f);
        }

        private void setJoystickButtonState(Dictionary<Button, bool> simpleButtonState, string prefix, string lowValueSuffix, string highValueSuffix, float value, float deadzone)
        {
            //adapted from https://katyscode.wordpress.com/2013/08/30/xinput-tutorial-part-1-adding-gamepad-support-to-your-windows-game/

            float normalizedValue = maxf(-1, value / 32767f);
            float stickValue = Math.Abs(normalizedValue) < deadzone ? 0 : normalizedValue;
            stickValue = Math.Abs(normalizedValue) < deadzone ? 0 : (Math.Abs(normalizedValue) - deadzone) * (normalizedValue / Math.Abs(normalizedValue));
            if (deadzone > 0) stickValue /= 1 - deadzone;

            // float normalizedValue = 0;
            // if(Math.Abs(value) > deadzone) {
            //     normalizedValue = value / (float)short.MaxValue;
            // }

            setButtonState(simpleButtonState, (Button)Enum.Parse(typeof(Button), prefix + highValueSuffix), isBetween(normalizedValue,  0.1f, 1.00f));
            setButtonState(simpleButtonState, (Button)Enum.Parse(typeof(Button), prefix + lowValueSuffix),  isBetween(normalizedValue, -0.1f, -1.00f));
        }

        private bool isBetween(float value, float min, float max)
        {
            if (value < 0)
                return value <= min && value >= max;
            else 
                return value >= min && value <= max;
        }

        private float maxf(float a, float b)
        {
            if (a > b)
                return a;
            else
                return b;
        }

        private void setButtonState(Dictionary<Button, bool> simpleButtonState, Button key, bool value)
        {
            simpleButtonState.Add(key, value);
        }

        private StateTransition getNewState(Dictionary<Button, bool> simpleGamepadState, List<StateTransition> stateTransitions)
        {
            StateTransition result = null;

            foreach (StateTransition stateTransition in stateTransitions)
            {
                int matchedButtons = 0;
                foreach (var transitionButton in stateTransition.transitionButtons)
                {
                    Button key = transitionButton.button;
                    if(simpleGamepadState[key] && transitionButton.action == Settings.Action.PRESSED)
                        matchedButtons++;
                    if(!simpleGamepadState[key] && transitionButton.action == Settings.Action.RELEASED)
                        matchedButtons++;
                }
                // Console.WriteLine("State " + stateTransition.stateName + " transition " + stateTransition.transitionName + " matched: " + matchedButtons);
                if( matchedButtons == stateTransition.transitionButtons.Count)
                {
                    if(result == null || 
                        (result != null && result.transitionButtons.Count < stateTransition.transitionButtons.Count))
                    {
                
                    result = stateTransition;
                    }
                }
            }

            return result;
        }
    }
}