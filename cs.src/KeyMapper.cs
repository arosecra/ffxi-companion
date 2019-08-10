
using FFXICompanion.Settings;
using FFXICompanion;
using System;
using System.Threading;
using System.Collections.Generic;
using Core_Interception.Lib;
using SharpDX.XInput;
using System.Diagnostics;

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


            var controllers = new[] { new Controller(UserIndex.One), new Controller(UserIndex.Two), new Controller(UserIndex.Three), new Controller(UserIndex.Four) };

            // Get 1st controller available
            Controller controller = null;
            foreach (var selectControler in controllers)
            {
                if (selectControler.IsConnected)
                {
                    controller = selectControler;
                    break;
                }
            }

            if (controller == null)
            {
                // Console.WriteLine("No XInput controller installed");
            }
            else
            {
                Console.WriteLine("KeyMapper loaded");
                // Console.WriteLine("Found a XInput controller available");
                // Console.WriteLine("Press buttons on the controller to display events or escape key to exit... ");

                // Poll events from joystick
                SharpDX.XInput.State previousControllerState = controller.GetState();
                Dictionary<Button, bool> lastSimpleGamepadState = determineSimpleButtonState(previousControllerState);

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

                        //determine if we are transitioning to a new 'state'
                        //  this is based on the current state, the game state and the keys pressed/not pressed
                        //  NOTE: the first state wins, for speed
                        string newState = getNewStateName(simpleGamepadState, ModuleData.getInstance().companionSettings.stateTransitions);


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


                        lastSimpleGamepadState = simpleGamepadState;
                    }
                    Thread.Sleep(10);
                    previousControllerState = controllerState;
                    }

                }
            }
        }

        private void handleCommand(ControllerMapping mapping)
        {
            if (mapping.keyPressesCommand != null)
            {
                foreach (KeyPress kp in mapping.keyPressesCommand.keyPresses)
                {
                    var stroke = new ManagedWrapper.Stroke();
                    stroke.key.code = (ushort)kp.key;
                    if (kp.action == Settings.Action.PRESSED)
                        stroke.key.state = (ushort)ManagedWrapper.KeyState.Down;
                    else
                        stroke.key.state = (ushort)ManagedWrapper.KeyState.Up;
                    int devId = 1;
                    Console.WriteLine(stroke.key.code);
                    ManagedWrapper.Send(deviceContext, devId, ref stroke, 1);
                    System.Threading.Thread.Sleep(50);
                }
            } else if (mapping.altTabCommand != null) {

                
                // List<Process> polProcesses = new List<Process>();
                // foreach (Process process in Process.GetProcesses())
                // {
                //     if(process.ProcessName == "pol") {
                //         polProcesses.Add(process);
                //     }
                // }

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

        private StateControllerMapping findStateControllerMappings(string newState, List<StateControllerMapping> stateMappings)
        {
            StateControllerMapping mapping = null;

            foreach (StateControllerMapping current in stateMappings)
            {
                if (mapping == null && current.stateName == "Default")
                {
                    mapping = current;
                }
                if (current.stateName == newState)
                {
                    mapping = current;
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

        private string getNewStateName(Dictionary<Button, bool> simpleGamepadState, List<StateTransition> stateTransitions)
        {
            string result = "Default";

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
                if(matchedButtons == stateTransition.transitionButtons.Count) {
                    result = stateTransition.stateName;
                    // Console.WriteLine("Changing state to " + result);
                    break;
                }
            }

            return result;
        }
    }
}