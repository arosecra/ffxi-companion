using System;
using System.IO;
using System.Threading;
using System.Xml;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.Collections.Concurrent;

using FFXICompanion.KeyMapper;
using FFXICompanion.Settings;

namespace FFXICompanion
{
    class Program
    {

        static void Main(string[] args)
        {
            CompanionSettings settings = new CompanionSettings();

            settings.states.Add(new State("Default", "The default state"));
            settings.states.Add(new State("Macro Type State", null));
            settings.states.Add(new State("Macro State 1", null));
            settings.states.Add(new State("Macro State 2", null));
            settings.states.Add(new State("Macro State 3", null));
            settings.states.Add(new State("Macro State 4", null));
            settings.states.Add(new State("Macro State 4", null));
            
            addStateTransition(settings, "Macro Type State", Button.BACK, Settings.Action.PRESSED);
            addStateTransition(settings, "Macro State 1", Button.LB, Settings.Action.PRESSED);
            addStateTransition(settings, "Macro State 2", Button.LT, Settings.Action.PRESSED);
            addStateTransition(settings, "Macro State 3", Button.RB, Settings.Action.PRESSED);
            addStateTransition(settings, "Macro State 4", Button.RT, Settings.Action.PRESSED);
            addStateTransition(settings, "Macro State 5", Button.GUIDE, Settings.Action.PRESSED);

            StateControllerMapping defaultStateCM = new StateControllerMapping();
            defaultStateCM.stateName = "Default";

            addButtonToAltTabCommand(defaultStateCM, Button.START, Settings.Action.PRESSED);
            
            //xyab
            addButtonToOneKeyPressAndReleaseMapping(defaultStateCM, Button.X, Settings.Action.PRESSED, Settings.Key.F);
            addButtonToOneKeyPressAndReleaseMapping(defaultStateCM, Button.Y, Settings.Action.PRESSED, Settings.Key.DashUnderscore);
            addButtonToOneKeyPressAndReleaseMapping(defaultStateCM, Button.A, Settings.Action.PRESSED, Settings.Key.Enter);
            addButtonToOneKeyPressAndReleaseMapping(defaultStateCM, Button.B, Settings.Action.PRESSED, Settings.Key.Escape);
            
            //d pad
            addButtonToKeyPressMapping(defaultStateCM, Button.DL, Settings.Action.PRESSED, Settings.Key.CommaLeftArrow,   Settings.Action.PRESSED);
            addButtonToKeyPressMapping(defaultStateCM, Button.DR, Settings.Action.PRESSED, Settings.Key.PeriodRightArrow, Settings.Action.PRESSED);
            addButtonToKeyPressMapping(defaultStateCM, Button.DU, Settings.Action.PRESSED, Settings.Key.N,  Settings.Action.PRESSED);
            addButtonToKeyPressMapping(defaultStateCM, Button.DD, Settings.Action.PRESSED, Settings.Key.M,  Settings.Action.PRESSED);
            addButtonToKeyPressMapping(defaultStateCM, Button.DL, Settings.Action.RELEASED, Settings.Key.CommaLeftArrow,  Settings.Action.RELEASED);
            addButtonToKeyPressMapping(defaultStateCM, Button.DR, Settings.Action.RELEASED, Settings.Key.PeriodRightArrow,  Settings.Action.RELEASED);
            addButtonToKeyPressMapping(defaultStateCM, Button.DU, Settings.Action.RELEASED, Settings.Key.N,  Settings.Action.RELEASED);
            addButtonToKeyPressMapping(defaultStateCM, Button.DD, Settings.Action.RELEASED, Settings.Key.M,  Settings.Action.RELEASED);
            
            //left stick
            addButtonToKeyPressMapping(defaultStateCM, Button.LSU, Settings.Action.PRESSED,  Settings.Key.W, Settings.Action.PRESSED );
            addButtonToKeyPressMapping(defaultStateCM, Button.LSU, Settings.Action.RELEASED, Settings.Key.W, Settings.Action.RELEASED);
            addButtonToKeyPressMapping(defaultStateCM, Button.LSL, Settings.Action.PRESSED,  Settings.Key.A, Settings.Action.PRESSED );
            addButtonToKeyPressMapping(defaultStateCM, Button.LSL, Settings.Action.RELEASED, Settings.Key.A, Settings.Action.RELEASED);
            addButtonToKeyPressMapping(defaultStateCM, Button.LSD, Settings.Action.PRESSED,  Settings.Key.S, Settings.Action.PRESSED );
            addButtonToKeyPressMapping(defaultStateCM, Button.LSD, Settings.Action.RELEASED, Settings.Key.S, Settings.Action.RELEASED);
            addButtonToKeyPressMapping(defaultStateCM, Button.LSR, Settings.Action.PRESSED,  Settings.Key.D, Settings.Action.PRESSED );
            addButtonToKeyPressMapping(defaultStateCM, Button.LSR, Settings.Action.RELEASED, Settings.Key.D, Settings.Action.RELEASED);
            
            addButtonToKeyPressMapping(defaultStateCM, Button.LS, Settings.Action.PRESSED,  Settings.Key.R, Settings.Action.PRESSED );
            addButtonToKeyPressMapping(defaultStateCM, Button.LS, Settings.Action.RELEASED, Settings.Key.R, Settings.Action.RELEASED);

            //right stick
            addButtonToKeyPressMapping(defaultStateCM, Button.RSU, Settings.Action.PRESSED,  Settings.Key.I, Settings.Action.PRESSED );
            addButtonToKeyPressMapping(defaultStateCM, Button.RSU, Settings.Action.RELEASED, Settings.Key.I, Settings.Action.RELEASED);
            addButtonToKeyPressMapping(defaultStateCM, Button.RSL, Settings.Action.PRESSED,  Settings.Key.J, Settings.Action.PRESSED );
            addButtonToKeyPressMapping(defaultStateCM, Button.RSL, Settings.Action.RELEASED, Settings.Key.J, Settings.Action.RELEASED);
            addButtonToKeyPressMapping(defaultStateCM, Button.RSD, Settings.Action.PRESSED,  Settings.Key.K, Settings.Action.PRESSED );
            addButtonToKeyPressMapping(defaultStateCM, Button.RSD, Settings.Action.RELEASED, Settings.Key.K, Settings.Action.RELEASED);
            addButtonToKeyPressMapping(defaultStateCM, Button.RSR, Settings.Action.PRESSED,  Settings.Key.L, Settings.Action.PRESSED );
            addButtonToKeyPressMapping(defaultStateCM, Button.RSR, Settings.Action.RELEASED, Settings.Key.L, Settings.Action.RELEASED);

            addButtonToKeyPressMapping(defaultStateCM, Button.LB, Settings.Action.PRESSED, Settings.Key.RightAlt  , Settings.Action.PRESSED );
            addButtonToKeyPressMapping(defaultStateCM, Button.LT, Settings.Action.PRESSED, Settings.Key.LOGI_WIN  , Settings.Action.PRESSED );
            addButtonToKeyPressMapping(defaultStateCM, Button.RB, Settings.Action.PRESSED, Settings.Key.Control   , Settings.Action.PRESSED );
            addButtonToKeyPressMapping(defaultStateCM, Button.RT, Settings.Action.PRESSED, Settings.Key.LOGI_MENU , Settings.Action.PRESSED );
            addButtonToKeyPressMapping(defaultStateCM, Button.GUIDE, Settings.Action.PRESSED, Settings.Key.LeftShift, Settings.Action.PRESSED);
            addButtonToKeyPressMapping(defaultStateCM, Button.LB, Settings.Action.RELEASED, Settings.Key.RightAlt , Settings.Action.RELEASED);
            addButtonToKeyPressMapping(defaultStateCM, Button.LT, Settings.Action.RELEASED, Settings.Key.LOGI_WIN , Settings.Action.RELEASED);
            addButtonToKeyPressMapping(defaultStateCM, Button.RB, Settings.Action.RELEASED, Settings.Key.Control  , Settings.Action.RELEASED);
            addButtonToKeyPressMapping(defaultStateCM, Button.RT, Settings.Action.RELEASED, Settings.Key.LOGI_MENU, Settings.Action.RELEASED);
            addButtonToKeyPressMapping(defaultStateCM, Button.GUIDE, Settings.Action.RELEASED, Settings.Key.LeftShift, Settings.Action.RELEASED);

            settings.stateMappings.Add(defaultStateCM);

            
            StateControllerMapping macroTypeStateCM = new StateControllerMapping();
            macroTypeStateCM.stateName = "Macro Type State";

            addButtonToOneKeyPressAndReleaseMapping(macroTypeStateCM, Button.LB, Settings.Action.PRESSED, Settings.Key.Numpad1);
            addButtonToOneKeyPressAndReleaseMapping(macroTypeStateCM, Button.LT, Settings.Action.PRESSED, Settings.Key.Numpad2);
            addButtonToOneKeyPressAndReleaseMapping(macroTypeStateCM, Button.RB, Settings.Action.PRESSED, Settings.Key.Numpad3);
            addButtonToOneKeyPressAndReleaseMapping(macroTypeStateCM, Button.RT, Settings.Action.PRESSED, Settings.Key.Numpad4);
            addButtonToOneKeyPressAndReleaseMapping(macroTypeStateCM, Button.X,  Settings.Action.PRESSED, Settings.Key.Numpad5);
            addButtonToOneKeyPressAndReleaseMapping(macroTypeStateCM, Button.Y,  Settings.Action.PRESSED, Settings.Key.Numpad6);
            addButtonToOneKeyPressAndReleaseMapping(macroTypeStateCM, Button.A,  Settings.Action.PRESSED, Settings.Key.Numpad7);
            addButtonToOneKeyPressAndReleaseMapping(macroTypeStateCM, Button.B,  Settings.Action.PRESSED, Settings.Key.Numpad8);

            settings.stateMappings.Add(macroTypeStateCM);

            addMacroState(settings, "Macro State 1");
            addMacroState(settings, "Macro State 2");
            addMacroState(settings, "Macro State 3");
            addMacroState(settings, "Macro State 4");
            addMacroState(settings, "Macro State 5");

            writeSettings(settings);

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
            threads.Add(new Thread(new ThreadStart(srvr.start)));
            threads.Add(new Thread(new ThreadStart(sock.start)));
            threads.Add(new Thread(new ThreadStart(pipeServer.start)));

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

        public static void addMacroState(CompanionSettings settings, String stateName) {
            StateControllerMapping macroState = new StateControllerMapping();
            macroState.stateName = stateName;

            addButtonToOneKeyPressAndReleaseMapping(macroState, Button.DL, Settings.Action.PRESSED, Settings.Key.Numpad1);
            addButtonToOneKeyPressAndReleaseMapping(macroState, Button.DU, Settings.Action.PRESSED, Settings.Key.Numpad2);
            addButtonToOneKeyPressAndReleaseMapping(macroState, Button.DD, Settings.Action.PRESSED, Settings.Key.Numpad3);
            addButtonToOneKeyPressAndReleaseMapping(macroState, Button.DR, Settings.Action.PRESSED, Settings.Key.Numpad4);
            addButtonToOneKeyPressAndReleaseMapping(macroState, Button.X,  Settings.Action.PRESSED, Settings.Key.Numpad5);
            addButtonToOneKeyPressAndReleaseMapping(macroState, Button.Y,  Settings.Action.PRESSED, Settings.Key.Numpad6);
            addButtonToOneKeyPressAndReleaseMapping(macroState, Button.A,  Settings.Action.PRESSED, Settings.Key.Numpad7);
            addButtonToOneKeyPressAndReleaseMapping(macroState, Button.B,  Settings.Action.PRESSED, Settings.Key.Numpad8);
            addButtonToOneKeyPressAndReleaseMapping(macroState, Button.BACK,  Settings.Action.PRESSED, Settings.Key.Numpad9);
            addButtonToOneKeyPressAndReleaseMapping(macroState, Button.START,  Settings.Action.PRESSED, Settings.Key.Numpad0);
            
            //left stick
            addButtonToKeyPressMapping(macroState, Button.LSU, Settings.Action.PRESSED,  Settings.Key.W, Settings.Action.PRESSED );
            addButtonToKeyPressMapping(macroState, Button.LSU, Settings.Action.RELEASED, Settings.Key.W, Settings.Action.RELEASED);
            addButtonToKeyPressMapping(macroState, Button.LSL, Settings.Action.PRESSED,  Settings.Key.A, Settings.Action.PRESSED );
            addButtonToKeyPressMapping(macroState, Button.LSL, Settings.Action.RELEASED, Settings.Key.A, Settings.Action.RELEASED);
            addButtonToKeyPressMapping(macroState, Button.LSD, Settings.Action.PRESSED,  Settings.Key.S, Settings.Action.PRESSED );
            addButtonToKeyPressMapping(macroState, Button.LSD, Settings.Action.RELEASED, Settings.Key.S, Settings.Action.RELEASED);
            addButtonToKeyPressMapping(macroState, Button.LSR, Settings.Action.PRESSED,  Settings.Key.D, Settings.Action.PRESSED );
            addButtonToKeyPressMapping(macroState, Button.LSR, Settings.Action.RELEASED, Settings.Key.D, Settings.Action.RELEASED);

            //right stick
            addButtonToKeyPressMapping(macroState, Button.RSU, Settings.Action.PRESSED,  Settings.Key.I, Settings.Action.PRESSED );
            addButtonToKeyPressMapping(macroState, Button.RSU, Settings.Action.RELEASED, Settings.Key.I, Settings.Action.RELEASED);
            addButtonToKeyPressMapping(macroState, Button.RSL, Settings.Action.PRESSED,  Settings.Key.J, Settings.Action.PRESSED );
            addButtonToKeyPressMapping(macroState, Button.RSL, Settings.Action.RELEASED, Settings.Key.J, Settings.Action.RELEASED);
            addButtonToKeyPressMapping(macroState, Button.RSD, Settings.Action.PRESSED,  Settings.Key.K, Settings.Action.PRESSED );
            addButtonToKeyPressMapping(macroState, Button.RSD, Settings.Action.RELEASED, Settings.Key.K, Settings.Action.RELEASED);
            addButtonToKeyPressMapping(macroState, Button.RSR, Settings.Action.PRESSED,  Settings.Key.L, Settings.Action.PRESSED );
            addButtonToKeyPressMapping(macroState, Button.RSR, Settings.Action.RELEASED, Settings.Key.L, Settings.Action.RELEASED);

            addButtonToKeyPressMapping(macroState, Button.LB, Settings.Action.PRESSED, Settings.Key.RightAlt  , Settings.Action.PRESSED );
            addButtonToKeyPressMapping(macroState, Button.LT, Settings.Action.PRESSED, Settings.Key.LOGI_WIN  , Settings.Action.PRESSED );
            addButtonToKeyPressMapping(macroState, Button.RB, Settings.Action.PRESSED, Settings.Key.Control   , Settings.Action.PRESSED );
            addButtonToKeyPressMapping(macroState, Button.RT, Settings.Action.PRESSED, Settings.Key.LOGI_MENU , Settings.Action.PRESSED );
            addButtonToKeyPressMapping(macroState, Button.GUIDE, Settings.Action.PRESSED, Settings.Key.LeftShift, Settings.Action.PRESSED);
            addButtonToKeyPressMapping(macroState, Button.LB, Settings.Action.RELEASED, Settings.Key.RightAlt , Settings.Action.RELEASED);
            addButtonToKeyPressMapping(macroState, Button.LT, Settings.Action.RELEASED, Settings.Key.LOGI_WIN , Settings.Action.RELEASED);
            addButtonToKeyPressMapping(macroState, Button.RB, Settings.Action.RELEASED, Settings.Key.Control  , Settings.Action.RELEASED);
            addButtonToKeyPressMapping(macroState, Button.RT, Settings.Action.RELEASED, Settings.Key.LOGI_MENU, Settings.Action.RELEASED);
            addButtonToKeyPressMapping(macroState, Button.GUIDE, Settings.Action.RELEASED, Settings.Key.LeftShift, Settings.Action.RELEASED);

            settings.stateMappings.Add(macroState);
        }

        public static void addStateTransition(CompanionSettings settings, string stateName, Settings.Button button, Settings.Action action) {
            StateTransition stateTransition = new StateTransition();
            stateTransition.stateName = stateName;
            ControllerButton controllerButton = new ControllerButton(button, action);
            stateTransition.transitionButtons.Add(controllerButton);
            settings.stateTransitions.Add(stateTransition);
        }

        public static void addButtonToKeyPressMapping(StateControllerMapping scm, 
                                            Button controllerButton, 
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
                                            Button controllerButton, 
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
                                            Button controllerButton, 
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
                                            Button controllerButton, 
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
                                            Button controllerButton, 
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
