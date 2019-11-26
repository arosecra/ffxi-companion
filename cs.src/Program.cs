using System;
using System.IO;
using System.Threading;
using System.Xml;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Windows.Forms;
using System.Drawing;

using FFXICompanion.KeyMapper;
using FFXICompanion.Settings;

namespace FFXICompanion
{
    class Program
    {

        static void Main(string[] args)
        {

            CompanionSettings settings = new CompanionSettings();

            //default -> select button -> 

            //default -> LB, LT, RB, RT, LB+LT, RB+RT, GUIDE -> macro state
            //
        


            settings.states.Add(new State("Default", "The default state"));
            settings.states.Add(new State("Macro Book State", null));
            settings.states.Add(new State("Macro State", null));

// Settings.Key.LeftShift  ,
// Settings.Key.Control  , S
// Settings.Key.LOGI_WIN   ,
// Settings.Key.LeftAlt , Se
// D, Settings.Key.RightAlt,
// Settings.Key.LeftShift ,
// Settings.Key.Control , S
// Settings.Key.LOGI_WIN  ,
// Settings.Key.LeftAlt, Se
// ED, Settings.Key.RightAlt
            
            addStateTransition(settings, "Macro Book State", "Macro Book State Transition", FFXICompanion.Settings.Button.BACK);
            addStateTransition(settings, "Macro State", "1", FFXICompanion.Settings.Button.GUIDE, Settings.Key.LeftShift);
            addStateTransition(settings, "Macro State", "2", FFXICompanion.Settings.Button.LB, Settings.Key.Control);
            addStateTransition(settings, "Macro State", "3", FFXICompanion.Settings.Button.LT, Settings.Key.LOGI_WIN);
            addStateTransition(settings, "Macro State", "4", FFXICompanion.Settings.Button.RB, Settings.Key.LeftAlt);
            addStateTransition(settings, "Macro State", "5", FFXICompanion.Settings.Button.RT, Settings.Key.RightAlt);
            addTwoButtonStateTransition(settings, "Macro State", "6", FFXICompanion.Settings.Button.LB, FFXICompanion.Settings.Button.LT, Settings.Key.LOGI_MENU);
            addTwoButtonStateTransition(settings, "Macro State", "7", FFXICompanion.Settings.Button.RB, FFXICompanion.Settings.Button.RT, Settings.Key.RightControl);

            StateControllerMapping defaultStateCM = new StateControllerMapping();
            defaultStateCM.stateName = "Default";

            addButtonToAltTabCommand(defaultStateCM, FFXICompanion.Settings.Button.START, Settings.Action.PRESSED);
            
            //xyab
            addButtonToOneKeyPressAndReleaseMapping(defaultStateCM, FFXICompanion.Settings.Button.X, Settings.Action.PRESSED, Settings.Key.F);
            addButtonToOneKeyPressAndReleaseMapping(defaultStateCM, FFXICompanion.Settings.Button.Y, Settings.Action.PRESSED, Settings.Key.DashUnderscore);
            addButtonToOneKeyPressAndReleaseMapping(defaultStateCM, FFXICompanion.Settings.Button.A, Settings.Action.PRESSED, Settings.Key.Enter);
            addButtonToOneKeyPressAndReleaseMapping(defaultStateCM, FFXICompanion.Settings.Button.B, Settings.Action.PRESSED, Settings.Key.Escape);
            
            //d pad
            addButtonToKeyPressMapping(defaultStateCM, FFXICompanion.Settings.Button.DL, Settings.Action.PRESSED, Settings.Key.CommaLeftArrow,   Settings.Action.PRESSED);
            addButtonToKeyPressMapping(defaultStateCM, FFXICompanion.Settings.Button.DR, Settings.Action.PRESSED, Settings.Key.PeriodRightArrow, Settings.Action.PRESSED);
            addButtonToKeyPressMapping(defaultStateCM, FFXICompanion.Settings.Button.DU, Settings.Action.PRESSED, Settings.Key.N,  Settings.Action.PRESSED);
            addButtonToKeyPressMapping(defaultStateCM, FFXICompanion.Settings.Button.DD, Settings.Action.PRESSED, Settings.Key.M,  Settings.Action.PRESSED);
            addButtonToKeyPressMapping(defaultStateCM, FFXICompanion.Settings.Button.DL, Settings.Action.RELEASED, Settings.Key.CommaLeftArrow,  Settings.Action.RELEASED);
            addButtonToKeyPressMapping(defaultStateCM, FFXICompanion.Settings.Button.DR, Settings.Action.RELEASED, Settings.Key.PeriodRightArrow,  Settings.Action.RELEASED);
            addButtonToKeyPressMapping(defaultStateCM, FFXICompanion.Settings.Button.DU, Settings.Action.RELEASED, Settings.Key.N,  Settings.Action.RELEASED);
            addButtonToKeyPressMapping(defaultStateCM, FFXICompanion.Settings.Button.DD, Settings.Action.RELEASED, Settings.Key.M,  Settings.Action.RELEASED);
            
            //left stick
            addButtonToKeyPressMapping(defaultStateCM, FFXICompanion.Settings.Button.LSU, Settings.Action.PRESSED,  Settings.Key.W, Settings.Action.PRESSED );
            addButtonToKeyPressMapping(defaultStateCM, FFXICompanion.Settings.Button.LSU, Settings.Action.RELEASED, Settings.Key.W, Settings.Action.RELEASED);
            addButtonToKeyPressMapping(defaultStateCM, FFXICompanion.Settings.Button.LSL, Settings.Action.PRESSED,  Settings.Key.A, Settings.Action.PRESSED );
            addButtonToKeyPressMapping(defaultStateCM, FFXICompanion.Settings.Button.LSL, Settings.Action.RELEASED, Settings.Key.A, Settings.Action.RELEASED);
            addButtonToKeyPressMapping(defaultStateCM, FFXICompanion.Settings.Button.LSD, Settings.Action.PRESSED,  Settings.Key.S, Settings.Action.PRESSED );
            addButtonToKeyPressMapping(defaultStateCM, FFXICompanion.Settings.Button.LSD, Settings.Action.RELEASED, Settings.Key.S, Settings.Action.RELEASED);
            addButtonToKeyPressMapping(defaultStateCM, FFXICompanion.Settings.Button.LSR, Settings.Action.PRESSED,  Settings.Key.D, Settings.Action.PRESSED );
            addButtonToKeyPressMapping(defaultStateCM, FFXICompanion.Settings.Button.LSR, Settings.Action.RELEASED, Settings.Key.D, Settings.Action.RELEASED);
            
            addButtonToKeyPressMapping(defaultStateCM, FFXICompanion.Settings.Button.LS, Settings.Action.PRESSED,  Settings.Key.R, Settings.Action.PRESSED );
            addButtonToKeyPressMapping(defaultStateCM, FFXICompanion.Settings.Button.LS, Settings.Action.RELEASED, Settings.Key.R, Settings.Action.RELEASED);

            //right stick
            addButtonToKeyPressMapping(defaultStateCM, FFXICompanion.Settings.Button.RSU, Settings.Action.PRESSED,  Settings.Key.I, Settings.Action.PRESSED );
            addButtonToKeyPressMapping(defaultStateCM, FFXICompanion.Settings.Button.RSU, Settings.Action.RELEASED, Settings.Key.I, Settings.Action.RELEASED);
            addButtonToKeyPressMapping(defaultStateCM, FFXICompanion.Settings.Button.RSL, Settings.Action.PRESSED,  Settings.Key.J, Settings.Action.PRESSED );
            addButtonToKeyPressMapping(defaultStateCM, FFXICompanion.Settings.Button.RSL, Settings.Action.RELEASED, Settings.Key.J, Settings.Action.RELEASED);
            addButtonToKeyPressMapping(defaultStateCM, FFXICompanion.Settings.Button.RSD, Settings.Action.PRESSED,  Settings.Key.K, Settings.Action.PRESSED );
            addButtonToKeyPressMapping(defaultStateCM, FFXICompanion.Settings.Button.RSD, Settings.Action.RELEASED, Settings.Key.K, Settings.Action.RELEASED);
            addButtonToKeyPressMapping(defaultStateCM, FFXICompanion.Settings.Button.RSR, Settings.Action.PRESSED,  Settings.Key.L, Settings.Action.PRESSED );
            addButtonToKeyPressMapping(defaultStateCM, FFXICompanion.Settings.Button.RSR, Settings.Action.RELEASED, Settings.Key.L, Settings.Action.RELEASED);

            // createControlKeyButtonMappings(defaultStateCM);

            settings.stateMappings.Add(defaultStateCM);

            
            StateControllerMapping macroTypeStateCM = new StateControllerMapping();
            macroTypeStateCM.stateName = "Macro Book State";

            addButtonToOneKeyPressAndReleaseMapping(macroTypeStateCM, FFXICompanion.Settings.Button.LB, Settings.Action.PRESSED, Settings.Key.Numpad1);
            addButtonToOneKeyPressAndReleaseMapping(macroTypeStateCM, FFXICompanion.Settings.Button.LT, Settings.Action.PRESSED, Settings.Key.Numpad2);
            addButtonToOneKeyPressAndReleaseMapping(macroTypeStateCM, FFXICompanion.Settings.Button.RB, Settings.Action.PRESSED, Settings.Key.Numpad3);
            addButtonToOneKeyPressAndReleaseMapping(macroTypeStateCM, FFXICompanion.Settings.Button.RT, Settings.Action.PRESSED, Settings.Key.Numpad4);
            addButtonToOneKeyPressAndReleaseMapping(macroTypeStateCM, FFXICompanion.Settings.Button.X,  Settings.Action.PRESSED, Settings.Key.Numpad5);
            addButtonToOneKeyPressAndReleaseMapping(macroTypeStateCM, FFXICompanion.Settings.Button.Y,  Settings.Action.PRESSED, Settings.Key.Numpad6);
            addButtonToOneKeyPressAndReleaseMapping(macroTypeStateCM, FFXICompanion.Settings.Button.A,  Settings.Action.PRESSED, Settings.Key.Numpad7);
            addButtonToOneKeyPressAndReleaseMapping(macroTypeStateCM, FFXICompanion.Settings.Button.B,  Settings.Action.PRESSED, Settings.Key.Numpad8);

            settings.stateMappings.Add(macroTypeStateCM);

            addMacroState(settings, "Macro State");

            writeSettings(settings);

            // NotifyIcon tray = new NotifyIcon();
            // tray.Icon = new Icon("2.ico");;
            // tray.Visible = true;

            CancellationTokenSource cts = new CancellationTokenSource();
            ModuleData md = ModuleData.getInstance();
            md.companionSettings = settings;
            md.cancellationToken = cts.Token;


            Mapper keyMapper = new Mapper();
            FFXICompanion.Server.HttpServer srvr = new FFXICompanion.Server.HttpServer();
            FFXICompanion.Server.SocketServer sock = new FFXICompanion.Server.SocketServer();
            FFXICompanion.Server.NamedPipeServer pipeServer = new FFXICompanion.Server.NamedPipeServer();

            List<Thread> threads = new List<Thread>();
            threads.Add(new Thread(new ThreadStart(keyMapper.start)));
            // threads.Add(new Thread(new ThreadStart(srvr.start)));
            // threads.Add(new Thread(new ThreadStart(sock.start)));
            // threads.Add(new Thread(new ThreadStart(pipeServer.start)));

            foreach(Thread thread in threads) {
                thread.Start();
            }

            Console.CancelKeyPress += delegate {
                cts.Cancel();
            };

            foreach(Thread thread in threads) {
                thread.Join();
            }

        }

        public static void createControlKeyButtonMappings(StateControllerMapping mapping)
        {
            // addButtonToKeyPressMapping(mapping, FFXICompanion.Settings.Button.LB, Settings.Action.PRESSED, Settings.Key.LeftShift  , Settings.Action.PRESSED );
            // addButtonToKeyPressMapping(mapping, FFXICompanion.Settings.Button.LT, Settings.Action.PRESSED, Settings.Key.Control  , Settings.Action.PRESSED );
            // addButtonToKeyPressMapping(mapping, FFXICompanion.Settings.Button.RB, Settings.Action.PRESSED, Settings.Key.LOGI_WIN   , Settings.Action.PRESSED );
            // addButtonToKeyPressMapping(mapping, FFXICompanion.Settings.Button.RT, Settings.Action.PRESSED, Settings.Key.LeftAlt , Settings.Action.PRESSED );
            // addButtonToKeyPressMapping(mapping, FFXICompanion.Settings.Button.GUIDE, Settings.Action.PRESSED, Settings.Key.RightAlt, Settings.Action.PRESSED);
            // addButtonToKeyPressMapping(mapping, FFXICompanion.Settings.Button.LB, Settings.Action.RELEASED, Settings.Key.LeftShift , Settings.Action.RELEASED);
            // addButtonToKeyPressMapping(mapping, FFXICompanion.Settings.Button.LT, Settings.Action.RELEASED, Settings.Key.Control , Settings.Action.RELEASED);
            // addButtonToKeyPressMapping(mapping, FFXICompanion.Settings.Button.RB, Settings.Action.RELEASED, Settings.Key.LOGI_WIN  , Settings.Action.RELEASED);
            // addButtonToKeyPressMapping(mapping, FFXICompanion.Settings.Button.RT, Settings.Action.RELEASED, Settings.Key.LeftAlt, Settings.Action.RELEASED);
            // addButtonToKeyPressMapping(mapping, FFXICompanion.Settings.Button.GUIDE, Settings.Action.RELEASED, Settings.Key.RightAlt, Settings.Action.RELEASED);
        }

        public static void addMacroState(CompanionSettings settings, String stateName) {
            StateControllerMapping macroState = new StateControllerMapping();
            macroState.stateName = stateName;

            addButtonToOneKeyPressAndReleaseMapping(macroState, FFXICompanion.Settings.Button.DL, Settings.Action.PRESSED, Settings.Key.Numpad1);
            addButtonToOneKeyPressAndReleaseMapping(macroState, FFXICompanion.Settings.Button.DU, Settings.Action.PRESSED, Settings.Key.Numpad2);
            addButtonToOneKeyPressAndReleaseMapping(macroState, FFXICompanion.Settings.Button.DD, Settings.Action.PRESSED, Settings.Key.Numpad3);
            addButtonToOneKeyPressAndReleaseMapping(macroState, FFXICompanion.Settings.Button.DR, Settings.Action.PRESSED, Settings.Key.Numpad4);
            addButtonToOneKeyPressAndReleaseMapping(macroState, FFXICompanion.Settings.Button.X,  Settings.Action.PRESSED, Settings.Key.Numpad5);
            addButtonToOneKeyPressAndReleaseMapping(macroState, FFXICompanion.Settings.Button.Y,  Settings.Action.PRESSED, Settings.Key.Numpad6);
            addButtonToOneKeyPressAndReleaseMapping(macroState, FFXICompanion.Settings.Button.A,  Settings.Action.PRESSED, Settings.Key.Numpad7);
            addButtonToOneKeyPressAndReleaseMapping(macroState, FFXICompanion.Settings.Button.B,  Settings.Action.PRESSED, Settings.Key.Numpad8);
            addButtonToOneKeyPressAndReleaseMapping(macroState, FFXICompanion.Settings.Button.BACK,  Settings.Action.PRESSED, Settings.Key.Numpad9);
            addButtonToOneKeyPressAndReleaseMapping(macroState, FFXICompanion.Settings.Button.START, Settings.Action.PRESSED, Settings.Key.Numpad0);
            
            //left stick
            addButtonToKeyPressMapping(macroState, FFXICompanion.Settings.Button.LSU, Settings.Action.PRESSED,  Settings.Key.W, Settings.Action.PRESSED );
            addButtonToKeyPressMapping(macroState, FFXICompanion.Settings.Button.LSU, Settings.Action.RELEASED, Settings.Key.W, Settings.Action.RELEASED);
            addButtonToKeyPressMapping(macroState, FFXICompanion.Settings.Button.LSL, Settings.Action.PRESSED,  Settings.Key.A, Settings.Action.PRESSED );
            addButtonToKeyPressMapping(macroState, FFXICompanion.Settings.Button.LSL, Settings.Action.RELEASED, Settings.Key.A, Settings.Action.RELEASED);
            addButtonToKeyPressMapping(macroState, FFXICompanion.Settings.Button.LSD, Settings.Action.PRESSED,  Settings.Key.S, Settings.Action.PRESSED );
            addButtonToKeyPressMapping(macroState, FFXICompanion.Settings.Button.LSD, Settings.Action.RELEASED, Settings.Key.S, Settings.Action.RELEASED);
            addButtonToKeyPressMapping(macroState, FFXICompanion.Settings.Button.LSR, Settings.Action.PRESSED,  Settings.Key.D, Settings.Action.PRESSED );
            addButtonToKeyPressMapping(macroState, FFXICompanion.Settings.Button.LSR, Settings.Action.RELEASED, Settings.Key.D, Settings.Action.RELEASED);

            //right stick
            addButtonToKeyPressMapping(macroState, FFXICompanion.Settings.Button.RSU, Settings.Action.PRESSED,  Settings.Key.I, Settings.Action.PRESSED );
            addButtonToKeyPressMapping(macroState, FFXICompanion.Settings.Button.RSU, Settings.Action.RELEASED, Settings.Key.I, Settings.Action.RELEASED);
            addButtonToKeyPressMapping(macroState, FFXICompanion.Settings.Button.RSL, Settings.Action.PRESSED,  Settings.Key.J, Settings.Action.PRESSED );
            addButtonToKeyPressMapping(macroState, FFXICompanion.Settings.Button.RSL, Settings.Action.RELEASED, Settings.Key.J, Settings.Action.RELEASED);
            addButtonToKeyPressMapping(macroState, FFXICompanion.Settings.Button.RSD, Settings.Action.PRESSED,  Settings.Key.K, Settings.Action.PRESSED );
            addButtonToKeyPressMapping(macroState, FFXICompanion.Settings.Button.RSD, Settings.Action.RELEASED, Settings.Key.K, Settings.Action.RELEASED);
            addButtonToKeyPressMapping(macroState, FFXICompanion.Settings.Button.RSR, Settings.Action.PRESSED,  Settings.Key.L, Settings.Action.PRESSED );
            addButtonToKeyPressMapping(macroState, FFXICompanion.Settings.Button.RSR, Settings.Action.RELEASED, Settings.Key.L, Settings.Action.RELEASED);

            // createControlKeyButtonMappings(macroState);

            settings.stateMappings.Add(macroState);
        }

        public static void addStateTransition(CompanionSettings settings, string stateName, string transitionName, FFXICompanion.Settings.Button button) {
            StateTransition stateTransition = new StateTransition();
            stateTransition.stateName = stateName;
            stateTransition.transitionName = transitionName;
            stateTransition.key = Settings.Key.NULL;
            ControllerButton controllerButton = new ControllerButton(button, Settings.Action.PRESSED);
            stateTransition.transitionButtons.Add(controllerButton);
            settings.stateTransitions.Add(stateTransition);
        }
        public static void addStateTransition(CompanionSettings settings, string stateName, string transitionName, FFXICompanion.Settings.Button button, Settings.Key key) {
            StateTransition stateTransition = new StateTransition();
            stateTransition.stateName = stateName;
            stateTransition.transitionName = transitionName;
            stateTransition.key = key;
            ControllerButton controllerButton = new ControllerButton(button, Settings.Action.PRESSED);
            stateTransition.transitionButtons.Add(controllerButton);
            settings.stateTransitions.Add(stateTransition);
        }
        public static void addTwoButtonStateTransition(CompanionSettings settings, string stateName, string transitionName, FFXICompanion.Settings.Button button1, FFXICompanion.Settings.Button button2, Settings.Key key) {
            StateTransition stateTransition = new StateTransition();
            stateTransition.stateName = stateName;
            stateTransition.transitionName = transitionName;
            stateTransition.key = key;
            stateTransition.transitionButtons.Add(new ControllerButton(button1, Settings.Action.PRESSED));
            stateTransition.transitionButtons.Add(new ControllerButton(button2, Settings.Action.PRESSED));
            settings.stateTransitions.Add(stateTransition);
        }

        public static void addButtonToKeyPressMapping(StateControllerMapping scm, 
                                            FFXICompanion.Settings.Button controllerButton, 
                                            Settings.Action buttonAction,
                                            Settings.Key key,
                                            Settings.Action keyPress1) {
            ControllerMapping cm = new ControllerMapping();
            cm.button = new ControllerButton(controllerButton, buttonAction);
            cm.keyPressesCommand = new KeyPressesCommand();
            cm.keyPressesCommand.keyPresses.Add(new KeyPress(key, keyPress1));
            scm.controllerMappings.Add(cm);
        }
        public static void addButtonToOneKeyPressAndReleaseMapping(StateControllerMapping scm, 
                                            FFXICompanion.Settings.Button controllerButton, 
                                            Settings.Action buttonAction,
                                            Settings.Key key) {
            ControllerMapping cm = new ControllerMapping();
            cm.button = new ControllerButton(controllerButton, buttonAction);
            cm.keyPressesCommand = new KeyPressesCommand();
            cm.keyPressesCommand.keyPresses.Add(new KeyPress(key, Settings.Action.PRESSED));
            cm.keyPressesCommand.keyPresses.Add(new KeyPress(key, Settings.Action.RELEASED));
            scm.controllerMappings.Add(cm);
        }
        public static void addButtonToTwoKeyPressAndReleaseMapping(StateControllerMapping scm, 
                                            FFXICompanion.Settings.Button controllerButton, 
                                            Settings.Action buttonAction,
                                            Settings.Key key1,
                                            Settings.Key key2) {
            ControllerMapping cm = new ControllerMapping();
            cm.button = new ControllerButton(controllerButton, buttonAction);
            cm.keyPressesCommand = new KeyPressesCommand();
            cm.keyPressesCommand.keyPresses.Add(new KeyPress(key1, Settings.Action.PRESSED));
            cm.keyPressesCommand.keyPresses.Add(new KeyPress(key2, Settings.Action.PRESSED));
            cm.keyPressesCommand.keyPresses.Add(new KeyPress(key2, Settings.Action.RELEASED));
            cm.keyPressesCommand.keyPresses.Add(new KeyPress(key1, Settings.Action.RELEASED));
            scm.controllerMappings.Add(cm);
        }
        public static void addButtonToThreeKeyPressAndReleaseMapping(StateControllerMapping scm, 
                                            FFXICompanion.Settings.Button controllerButton, 
                                            Settings.Action buttonAction,
                                            Settings.Key key1,
                                            Settings.Key key2,
                                            Settings.Key key3) {
            ControllerMapping cm = new ControllerMapping();
            cm.button = new ControllerButton(controllerButton, buttonAction);
            cm.keyPressesCommand = new KeyPressesCommand();
            cm.keyPressesCommand.keyPresses.Add(new KeyPress(key1, Settings.Action.PRESSED));
            cm.keyPressesCommand.keyPresses.Add(new KeyPress(key2, Settings.Action.PRESSED));
            cm.keyPressesCommand.keyPresses.Add(new KeyPress(key3, Settings.Action.PRESSED));
            cm.keyPressesCommand.keyPresses.Add(new KeyPress(key3, Settings.Action.RELEASED));
            cm.keyPressesCommand.keyPresses.Add(new KeyPress(key2, Settings.Action.RELEASED));
            cm.keyPressesCommand.keyPresses.Add(new KeyPress(key1, Settings.Action.RELEASED));
            scm.controllerMappings.Add(cm);
        }
        
        public static void addButtonToAltTabCommand(StateControllerMapping scm, 
                                            FFXICompanion.Settings.Button controllerButton, 
                                            Settings.Action buttonAction) {
            ControllerMapping cm = new ControllerMapping();
            cm.button = new ControllerButton(controllerButton, buttonAction);
            cm.altTabCommand = new AltTabCommand();
            scm.controllerMappings.Add(cm);
        }

        public static CompanionSettings readSettings() {
            XmlSerializer mySerializer = new XmlSerializer(typeof(CompanionSettings));
            Stream stream = File.Open("CompanionSettings.xml", FileMode.Open);
            CompanionSettings result = (CompanionSettings) mySerializer.Deserialize(stream);
            stream.Close();
            return result;
        }

        public static void writeSettings(CompanionSettings settings) {
            XmlSerializer mySerializer = new XmlSerializer(typeof(CompanionSettings));
            StreamWriter myWriter = new StreamWriter("CompanionSettings.xml");  
            mySerializer.Serialize(myWriter, settings);  
            myWriter.Close(); 
        }
    }

    

    public class ModuleData {
        private static readonly ModuleData instance = new ModuleData();

        static ModuleData() {}
        private ModuleData() {}

        public static ModuleData getInstance() { return instance; }

        public CompanionSettings companionSettings {get; set;}
        public CancellationToken cancellationToken {get; set;}

        public Dictionary<string, FFXICompanion.Game.Character> party = new Dictionary<string, FFXICompanion.Game.Character>();

        public Mutex mut = new Mutex();
    }


}
