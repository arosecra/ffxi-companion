using System;
using System.IO;
using System.Threading;
using System.Xml;
using System.Xml.Serialization;
using System.Collections.Generic;


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
            
            addStateTransition(settings, "Macro Type State", Button.BACK, Settings.Action.PRESSED);
            addStateTransition(settings, "Macro State 1", Button.RT, Settings.Action.PRESSED);
            addStateTransition(settings, "Macro State 2", Button.LT, Settings.Action.PRESSED);
            addStateTransition(settings, "Macro State 3", Button.RB, Settings.Action.PRESSED);
            addStateTransition(settings, "Macro State 4", Button.LB, Settings.Action.PRESSED);

            StateControllerMapping defaultStateCM = new StateControllerMapping();
            defaultStateCM.stateName = "Default";

            addButtonToAltTabCommand(defaultStateCM, Button.START, Settings.Action.PRESSED);
            
            //xyab
            addButtonToOneKeyPressAndReleaseMapping(defaultStateCM, Button.X, Settings.Action.PRESSED, Settings.Key.F);
            addButtonToOneKeyPressAndReleaseMapping(defaultStateCM, Button.Y, Settings.Action.PRESSED, Settings.Key.DashUnderscore);
            addButtonToOneKeyPressAndReleaseMapping(defaultStateCM, Button.A, Settings.Action.PRESSED, Settings.Key.Enter);
            addButtonToOneKeyPressAndReleaseMapping(defaultStateCM, Button.B, Settings.Action.PRESSED, Settings.Key.Escape);
            
            //d pad
            addButtonToOneKeyPressAndReleaseMapping(defaultStateCM, Button.DL, Settings.Action.PRESSED, Settings.Key.CommaLeftArrow);
            addButtonToOneKeyPressAndReleaseMapping(defaultStateCM, Button.DR, Settings.Action.PRESSED, Settings.Key.PeriodRightArrow);
            addButtonToOneKeyPressAndReleaseMapping(defaultStateCM, Button.DU, Settings.Action.PRESSED, Settings.Key.N);
            addButtonToOneKeyPressAndReleaseMapping(defaultStateCM, Button.DD, Settings.Action.PRESSED, Settings.Key.M);
            

            //left stick
            addButtonToKeyPressMapping(defaultStateCM, Button.LSU, Settings.Action.PRESSED,  Settings.Key.W, Settings.Action.PRESSED );
            addButtonToKeyPressMapping(defaultStateCM, Button.LSU, Settings.Action.RELEASED, Settings.Key.W, Settings.Action.RELEASED);
            addButtonToKeyPressMapping(defaultStateCM, Button.LSL, Settings.Action.PRESSED,  Settings.Key.A, Settings.Action.PRESSED );
            addButtonToKeyPressMapping(defaultStateCM, Button.LSL, Settings.Action.RELEASED, Settings.Key.A, Settings.Action.RELEASED);
            addButtonToKeyPressMapping(defaultStateCM, Button.LSD, Settings.Action.PRESSED,  Settings.Key.S, Settings.Action.PRESSED );
            addButtonToKeyPressMapping(defaultStateCM, Button.LSD, Settings.Action.RELEASED, Settings.Key.S, Settings.Action.RELEASED);
            addButtonToKeyPressMapping(defaultStateCM, Button.LSR, Settings.Action.PRESSED,  Settings.Key.D, Settings.Action.PRESSED );
            addButtonToKeyPressMapping(defaultStateCM, Button.LSR, Settings.Action.RELEASED, Settings.Key.D, Settings.Action.RELEASED);

            //right stick
            addButtonToKeyPressMapping(defaultStateCM, Button.RSU, Settings.Action.PRESSED,  Settings.Key.I, Settings.Action.PRESSED );
            addButtonToKeyPressMapping(defaultStateCM, Button.RSU, Settings.Action.RELEASED, Settings.Key.I, Settings.Action.RELEASED);
            addButtonToKeyPressMapping(defaultStateCM, Button.RSL, Settings.Action.PRESSED,  Settings.Key.J, Settings.Action.PRESSED );
            addButtonToKeyPressMapping(defaultStateCM, Button.RSL, Settings.Action.RELEASED, Settings.Key.J, Settings.Action.RELEASED);
            addButtonToKeyPressMapping(defaultStateCM, Button.RSD, Settings.Action.PRESSED,  Settings.Key.K, Settings.Action.PRESSED );
            addButtonToKeyPressMapping(defaultStateCM, Button.RSD, Settings.Action.RELEASED, Settings.Key.K, Settings.Action.RELEASED);
            addButtonToKeyPressMapping(defaultStateCM, Button.RSR, Settings.Action.PRESSED,  Settings.Key.L, Settings.Action.PRESSED );
            addButtonToKeyPressMapping(defaultStateCM, Button.RSR, Settings.Action.RELEASED, Settings.Key.L, Settings.Action.RELEASED);

            settings.stateMappings.Add(defaultStateCM);

            
            StateControllerMapping macroTypeStateCM = new StateControllerMapping();
            macroTypeStateCM.stateName = "Macro Type State";

            addButtonToOneKeyPressAndReleaseMapping(defaultStateCM, Button.LB, Settings.Action.PRESSED, Settings.Key.Numpad0);
            addButtonToOneKeyPressAndReleaseMapping(defaultStateCM, Button.RB, Settings.Action.PRESSED, Settings.Key.Numpad1);
            addButtonToOneKeyPressAndReleaseMapping(defaultStateCM, Button.LT, Settings.Action.PRESSED, Settings.Key.Numpad2);
            addButtonToOneKeyPressAndReleaseMapping(defaultStateCM, Button.RT, Settings.Action.PRESSED, Settings.Key.Numpad3);
            addButtonToOneKeyPressAndReleaseMapping(defaultStateCM, Button.X,  Settings.Action.PRESSED, Settings.Key.Numpad4);
            addButtonToOneKeyPressAndReleaseMapping(defaultStateCM, Button.Y,  Settings.Action.PRESSED, Settings.Key.Numpad5);
            addButtonToOneKeyPressAndReleaseMapping(defaultStateCM, Button.A,  Settings.Action.PRESSED, Settings.Key.Numpad6);
            addButtonToOneKeyPressAndReleaseMapping(defaultStateCM, Button.B,  Settings.Action.PRESSED, Settings.Key.Numpad7);

            settings.stateMappings.Add(macroTypeStateCM);

            StateControllerMapping macroState1 = new StateControllerMapping();
            macroState1.stateName = "Macro State 1";

            addButtonToTwoKeyPressAndReleaseMapping(defaultStateCM, Button.DL, Settings.Action.PRESSED, Settings.Key.RightAlt, Settings.Key.Numpad0);
            addButtonToTwoKeyPressAndReleaseMapping(defaultStateCM, Button.DU, Settings.Action.PRESSED, Settings.Key.RightAlt, Settings.Key.Numpad1);
            addButtonToTwoKeyPressAndReleaseMapping(defaultStateCM, Button.DR, Settings.Action.PRESSED, Settings.Key.RightAlt, Settings.Key.Numpad2);
            addButtonToTwoKeyPressAndReleaseMapping(defaultStateCM, Button.DD, Settings.Action.PRESSED, Settings.Key.RightAlt, Settings.Key.Numpad3);
            addButtonToTwoKeyPressAndReleaseMapping(defaultStateCM, Button.X, Settings.Action.PRESSED, Settings.Key.RightAlt, Settings.Key.Numpad4);
            addButtonToTwoKeyPressAndReleaseMapping(defaultStateCM, Button.Y, Settings.Action.PRESSED, Settings.Key.RightAlt, Settings.Key.Numpad5);
            addButtonToTwoKeyPressAndReleaseMapping(defaultStateCM, Button.A, Settings.Action.PRESSED, Settings.Key.RightAlt, Settings.Key.Numpad6);
            addButtonToTwoKeyPressAndReleaseMapping(defaultStateCM, Button.B, Settings.Action.PRESSED, Settings.Key.RightAlt, Settings.Key.Numpad7);

            settings.stateMappings.Add(macroState1);

            writeSettings(settings);

            CancellationTokenSource cts = new CancellationTokenSource();
            ModuleData md = new ModuleData();
            md.companionSettings = settings;
            md.cancellationToken = cts.Token;

            Mapper keyMapper = new Mapper(md);
            FFXICompanion.Server.HttpServer srvr = new FFXICompanion.Server.HttpServer(md);

            List<Thread> threads = new List<Thread>();
            threads.Add(new Thread(new ThreadStart(keyMapper.start)));
            // threads.Add(new Thread(new ThreadStart(srvr.start)));

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

    // public interface Module {

    // }

    public class ModuleData {
        public CompanionSettings companionSettings {get; set;}
        public CancellationToken cancellationToken {get; set;}

    }


}
