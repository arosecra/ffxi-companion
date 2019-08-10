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
            
            addStateTransition(settings, "Macro Type State", Button.BACK, Settings.Action.PRESSED);
            addStateTransition(settings, "Macro State 1", Button.LB, Settings.Action.PRESSED);
            addStateTransition(settings, "Macro State 2", Button.LT, Settings.Action.PRESSED);
            addStateTransition(settings, "Macro State 3", Button.RB, Settings.Action.PRESSED);
            addStateTransition(settings, "Macro State 4", Button.RT, Settings.Action.PRESSED);

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

            addButtonToKeyPressMapping(defaultStateCM, Button.LB, Settings.Action.PRESSED, Settings.Key.RightAlt  , Settings.Action.PRESSED );
            addButtonToKeyPressMapping(defaultStateCM, Button.LT, Settings.Action.PRESSED, Settings.Key.LOGI_WIN  , Settings.Action.PRESSED );
            addButtonToKeyPressMapping(defaultStateCM, Button.RB, Settings.Action.PRESSED, Settings.Key.Control   , Settings.Action.PRESSED );
            addButtonToKeyPressMapping(defaultStateCM, Button.RT, Settings.Action.PRESSED, Settings.Key.LOGI_MENU , Settings.Action.PRESSED );
            addButtonToKeyPressMapping(defaultStateCM, Button.LB, Settings.Action.RELEASED, Settings.Key.RightAlt , Settings.Action.RELEASED);
            addButtonToKeyPressMapping(defaultStateCM, Button.LT, Settings.Action.RELEASED, Settings.Key.LOGI_WIN , Settings.Action.RELEASED);
            addButtonToKeyPressMapping(defaultStateCM, Button.RB, Settings.Action.RELEASED, Settings.Key.Control  , Settings.Action.RELEASED);
            addButtonToKeyPressMapping(defaultStateCM, Button.RT, Settings.Action.RELEASED, Settings.Key.LOGI_MENU, Settings.Action.RELEASED);

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

            StateControllerMapping macroState1 = new StateControllerMapping();
            macroState1.stateName = "Macro State 1";

            // addButtonToTwoKeyPressAndReleaseMapping(macroState1, Button.DL, Settings.Action.PRESSED, Settings.Key.RightAlt, Settings.Key.Numpad1);
            // addButtonToTwoKeyPressAndReleaseMapping(macroState1, Button.DU, Settings.Action.PRESSED, Settings.Key.RightAlt, Settings.Key.Numpad2);
            // addButtonToTwoKeyPressAndReleaseMapping(macroState1, Button.DR, Settings.Action.PRESSED, Settings.Key.RightAlt, Settings.Key.Numpad3);
            // addButtonToTwoKeyPressAndReleaseMapping(macroState1, Button.DD, Settings.Action.PRESSED, Settings.Key.RightAlt, Settings.Key.Numpad4);
            // addButtonToTwoKeyPressAndReleaseMapping(macroState1, Button.X,  Settings.Action.PRESSED, Settings.Key.RightAlt, Settings.Key.Numpad5);
            // addButtonToTwoKeyPressAndReleaseMapping(macroState1, Button.Y,  Settings.Action.PRESSED, Settings.Key.RightAlt, Settings.Key.Numpad6);
            // addButtonToTwoKeyPressAndReleaseMapping(macroState1, Button.A,  Settings.Action.PRESSED, Settings.Key.RightAlt, Settings.Key.Numpad7);
            // addButtonToTwoKeyPressAndReleaseMapping(macroState1, Button.B,  Settings.Action.PRESSED, Settings.Key.RightAlt, Settings.Key.Numpad8);
            addButtonToOneKeyPressAndReleaseMapping(macroState1, Button.DL, Settings.Action.PRESSED, Settings.Key.Numpad1);
            addButtonToOneKeyPressAndReleaseMapping(macroState1, Button.DU, Settings.Action.PRESSED, Settings.Key.Numpad2);
            addButtonToOneKeyPressAndReleaseMapping(macroState1, Button.DR, Settings.Action.PRESSED, Settings.Key.Numpad3);
            addButtonToOneKeyPressAndReleaseMapping(macroState1, Button.DD, Settings.Action.PRESSED, Settings.Key.Numpad4);
            addButtonToOneKeyPressAndReleaseMapping(macroState1, Button.X,  Settings.Action.PRESSED, Settings.Key.Numpad5);
            addButtonToOneKeyPressAndReleaseMapping(macroState1, Button.Y,  Settings.Action.PRESSED, Settings.Key.Numpad6);
            addButtonToOneKeyPressAndReleaseMapping(macroState1, Button.A,  Settings.Action.PRESSED, Settings.Key.Numpad7);
            addButtonToOneKeyPressAndReleaseMapping(macroState1, Button.B,  Settings.Action.PRESSED, Settings.Key.Numpad8);
            
            //left stick
            addButtonToKeyPressMapping(macroState1, Button.LSU, Settings.Action.PRESSED,  Settings.Key.W, Settings.Action.PRESSED );
            addButtonToKeyPressMapping(macroState1, Button.LSU, Settings.Action.RELEASED, Settings.Key.W, Settings.Action.RELEASED);
            addButtonToKeyPressMapping(macroState1, Button.LSL, Settings.Action.PRESSED,  Settings.Key.A, Settings.Action.PRESSED );
            addButtonToKeyPressMapping(macroState1, Button.LSL, Settings.Action.RELEASED, Settings.Key.A, Settings.Action.RELEASED);
            addButtonToKeyPressMapping(macroState1, Button.LSD, Settings.Action.PRESSED,  Settings.Key.S, Settings.Action.PRESSED );
            addButtonToKeyPressMapping(macroState1, Button.LSD, Settings.Action.RELEASED, Settings.Key.S, Settings.Action.RELEASED);
            addButtonToKeyPressMapping(macroState1, Button.LSR, Settings.Action.PRESSED,  Settings.Key.D, Settings.Action.PRESSED );
            addButtonToKeyPressMapping(macroState1, Button.LSR, Settings.Action.RELEASED, Settings.Key.D, Settings.Action.RELEASED);

            //right stick
            addButtonToKeyPressMapping(macroState1, Button.RSU, Settings.Action.PRESSED,  Settings.Key.I, Settings.Action.PRESSED );
            addButtonToKeyPressMapping(macroState1, Button.RSU, Settings.Action.RELEASED, Settings.Key.I, Settings.Action.RELEASED);
            addButtonToKeyPressMapping(macroState1, Button.RSL, Settings.Action.PRESSED,  Settings.Key.J, Settings.Action.PRESSED );
            addButtonToKeyPressMapping(macroState1, Button.RSL, Settings.Action.RELEASED, Settings.Key.J, Settings.Action.RELEASED);
            addButtonToKeyPressMapping(macroState1, Button.RSD, Settings.Action.PRESSED,  Settings.Key.K, Settings.Action.PRESSED );
            addButtonToKeyPressMapping(macroState1, Button.RSD, Settings.Action.RELEASED, Settings.Key.K, Settings.Action.RELEASED);
            addButtonToKeyPressMapping(macroState1, Button.RSR, Settings.Action.PRESSED,  Settings.Key.L, Settings.Action.PRESSED );
            addButtonToKeyPressMapping(macroState1, Button.RSR, Settings.Action.RELEASED, Settings.Key.L, Settings.Action.RELEASED);

            addButtonToKeyPressMapping(macroState1, Button.LB, Settings.Action.PRESSED, Settings.Key.RightAlt  , Settings.Action.PRESSED );
            addButtonToKeyPressMapping(macroState1, Button.LT, Settings.Action.PRESSED, Settings.Key.LOGI_WIN  , Settings.Action.PRESSED );
            addButtonToKeyPressMapping(macroState1, Button.RB, Settings.Action.PRESSED, Settings.Key.Control   , Settings.Action.PRESSED );
            addButtonToKeyPressMapping(macroState1, Button.RT, Settings.Action.PRESSED, Settings.Key.LOGI_MENU , Settings.Action.PRESSED );
            addButtonToKeyPressMapping(macroState1, Button.LB, Settings.Action.RELEASED, Settings.Key.RightAlt , Settings.Action.RELEASED);
            addButtonToKeyPressMapping(macroState1, Button.LT, Settings.Action.RELEASED, Settings.Key.LOGI_WIN , Settings.Action.RELEASED);
            addButtonToKeyPressMapping(macroState1, Button.RB, Settings.Action.RELEASED, Settings.Key.Control  , Settings.Action.RELEASED);
            addButtonToKeyPressMapping(macroState1, Button.RT, Settings.Action.RELEASED, Settings.Key.LOGI_MENU, Settings.Action.RELEASED);

            settings.stateMappings.Add(macroState1);

            StateControllerMapping macroState2 = new StateControllerMapping();
            macroState2.stateName = "Macro State 2";

            // addButtonToTwoKeyPressAndReleaseMapping(macroState2, Button.DL, Settings.Action.PRESSED, Settings.Key.LOGI_WIN, Settings.Key.Numpad1);
            // addButtonToTwoKeyPressAndReleaseMapping(macroState2, Button.DU, Settings.Action.PRESSED, Settings.Key.LOGI_WIN, Settings.Key.Numpad2);
            // addButtonToTwoKeyPressAndReleaseMapping(macroState2, Button.DD, Settings.Action.PRESSED, Settings.Key.LOGI_WIN, Settings.Key.Numpad3);
            // addButtonToTwoKeyPressAndReleaseMapping(macroState2, Button.DR, Settings.Action.PRESSED, Settings.Key.LOGI_WIN, Settings.Key.Numpad4);
            // addButtonToTwoKeyPressAndReleaseMapping(macroState2, Button.X,  Settings.Action.PRESSED, Settings.Key.LOGI_WIN, Settings.Key.Numpad5);
            // addButtonToTwoKeyPressAndReleaseMapping(macroState2, Button.Y,  Settings.Action.PRESSED, Settings.Key.LOGI_WIN, Settings.Key.Numpad6);
            // addButtonToTwoKeyPressAndReleaseMapping(macroState2, Button.A,  Settings.Action.PRESSED, Settings.Key.LOGI_WIN, Settings.Key.Numpad7);
            // addButtonToTwoKeyPressAndReleaseMapping(macroState2, Button.B,  Settings.Action.PRESSED, Settings.Key.LOGI_WIN, Settings.Key.Numpad8);
            addButtonToOneKeyPressAndReleaseMapping(macroState2, Button.DL, Settings.Action.PRESSED, Settings.Key.Numpad1);
            addButtonToOneKeyPressAndReleaseMapping(macroState2, Button.DU, Settings.Action.PRESSED, Settings.Key.Numpad2);
            addButtonToOneKeyPressAndReleaseMapping(macroState2, Button.DD, Settings.Action.PRESSED, Settings.Key.Numpad3);
            addButtonToOneKeyPressAndReleaseMapping(macroState2, Button.DR, Settings.Action.PRESSED, Settings.Key.Numpad4);
            addButtonToOneKeyPressAndReleaseMapping(macroState2, Button.X,  Settings.Action.PRESSED, Settings.Key.Numpad5);
            addButtonToOneKeyPressAndReleaseMapping(macroState2, Button.Y,  Settings.Action.PRESSED, Settings.Key.Numpad6);
            addButtonToOneKeyPressAndReleaseMapping(macroState2, Button.A,  Settings.Action.PRESSED, Settings.Key.Numpad7);
            addButtonToOneKeyPressAndReleaseMapping(macroState2, Button.B,  Settings.Action.PRESSED, Settings.Key.Numpad8);
            
            //left stick
            addButtonToKeyPressMapping(macroState2, Button.LSU, Settings.Action.PRESSED,  Settings.Key.W, Settings.Action.PRESSED );
            addButtonToKeyPressMapping(macroState2, Button.LSU, Settings.Action.RELEASED, Settings.Key.W, Settings.Action.RELEASED);
            addButtonToKeyPressMapping(macroState2, Button.LSL, Settings.Action.PRESSED,  Settings.Key.A, Settings.Action.PRESSED );
            addButtonToKeyPressMapping(macroState2, Button.LSL, Settings.Action.RELEASED, Settings.Key.A, Settings.Action.RELEASED);
            addButtonToKeyPressMapping(macroState2, Button.LSD, Settings.Action.PRESSED,  Settings.Key.S, Settings.Action.PRESSED );
            addButtonToKeyPressMapping(macroState2, Button.LSD, Settings.Action.RELEASED, Settings.Key.S, Settings.Action.RELEASED);
            addButtonToKeyPressMapping(macroState2, Button.LSR, Settings.Action.PRESSED,  Settings.Key.D, Settings.Action.PRESSED );
            addButtonToKeyPressMapping(macroState2, Button.LSR, Settings.Action.RELEASED, Settings.Key.D, Settings.Action.RELEASED);

            //right stick
            addButtonToKeyPressMapping(macroState2, Button.RSU, Settings.Action.PRESSED,  Settings.Key.I, Settings.Action.PRESSED );
            addButtonToKeyPressMapping(macroState2, Button.RSU, Settings.Action.RELEASED, Settings.Key.I, Settings.Action.RELEASED);
            addButtonToKeyPressMapping(macroState2, Button.RSL, Settings.Action.PRESSED,  Settings.Key.J, Settings.Action.PRESSED );
            addButtonToKeyPressMapping(macroState2, Button.RSL, Settings.Action.RELEASED, Settings.Key.J, Settings.Action.RELEASED);
            addButtonToKeyPressMapping(macroState2, Button.RSD, Settings.Action.PRESSED,  Settings.Key.K, Settings.Action.PRESSED );
            addButtonToKeyPressMapping(macroState2, Button.RSD, Settings.Action.RELEASED, Settings.Key.K, Settings.Action.RELEASED);
            addButtonToKeyPressMapping(macroState2, Button.RSR, Settings.Action.PRESSED,  Settings.Key.L, Settings.Action.PRESSED );
            addButtonToKeyPressMapping(macroState2, Button.RSR, Settings.Action.RELEASED, Settings.Key.L, Settings.Action.RELEASED);

            addButtonToKeyPressMapping(macroState2, Button.LB, Settings.Action.PRESSED, Settings.Key.RightAlt  , Settings.Action.PRESSED );
            addButtonToKeyPressMapping(macroState2, Button.LT, Settings.Action.PRESSED, Settings.Key.LOGI_WIN  , Settings.Action.PRESSED );
            addButtonToKeyPressMapping(macroState2, Button.RB, Settings.Action.PRESSED, Settings.Key.Control   , Settings.Action.PRESSED );
            addButtonToKeyPressMapping(macroState2, Button.RT, Settings.Action.PRESSED, Settings.Key.LOGI_MENU , Settings.Action.PRESSED );
            addButtonToKeyPressMapping(macroState2, Button.LB, Settings.Action.RELEASED, Settings.Key.RightAlt , Settings.Action.RELEASED);
            addButtonToKeyPressMapping(macroState2, Button.LT, Settings.Action.RELEASED, Settings.Key.LOGI_WIN , Settings.Action.RELEASED);
            addButtonToKeyPressMapping(macroState2, Button.RB, Settings.Action.RELEASED, Settings.Key.Control  , Settings.Action.RELEASED);
            addButtonToKeyPressMapping(macroState2, Button.RT, Settings.Action.RELEASED, Settings.Key.LOGI_MENU, Settings.Action.RELEASED);

            settings.stateMappings.Add(macroState2);

            StateControllerMapping macroState3 = new StateControllerMapping();
            macroState3.stateName = "Macro State 3";

            // addButtonToTwoKeyPressAndReleaseMapping(macroState3, Button.DL, Settings.Action.PRESSED, Settings.Key.Control, Settings.Key.Numpad1);
            // addButtonToTwoKeyPressAndReleaseMapping(macroState3, Button.DU, Settings.Action.PRESSED, Settings.Key.Control, Settings.Key.Numpad2);
            // addButtonToTwoKeyPressAndReleaseMapping(macroState3, Button.DD, Settings.Action.PRESSED, Settings.Key.Control, Settings.Key.Numpad3);
            // addButtonToTwoKeyPressAndReleaseMapping(macroState3, Button.DR, Settings.Action.PRESSED, Settings.Key.Control, Settings.Key.Numpad4);
            // addButtonToTwoKeyPressAndReleaseMapping(macroState3, Button.X,  Settings.Action.PRESSED, Settings.Key.Control, Settings.Key.Numpad5);
            // addButtonToTwoKeyPressAndReleaseMapping(macroState3, Button.Y,  Settings.Action.PRESSED, Settings.Key.Control, Settings.Key.Numpad6);
            // addButtonToTwoKeyPressAndReleaseMapping(macroState3, Button.A,  Settings.Action.PRESSED, Settings.Key.Control, Settings.Key.Numpad7);
            // addButtonToTwoKeyPressAndReleaseMapping(macroState3, Button.B,  Settings.Action.PRESSED, Settings.Key.Control, Settings.Key.Numpad8);
            addButtonToOneKeyPressAndReleaseMapping(macroState3, Button.DL, Settings.Action.PRESSED, Settings.Key.Numpad1);
            addButtonToOneKeyPressAndReleaseMapping(macroState3, Button.DU, Settings.Action.PRESSED, Settings.Key.Numpad2);
            addButtonToOneKeyPressAndReleaseMapping(macroState3, Button.DD, Settings.Action.PRESSED, Settings.Key.Numpad3);
            addButtonToOneKeyPressAndReleaseMapping(macroState3, Button.DR, Settings.Action.PRESSED, Settings.Key.Numpad4);
            addButtonToOneKeyPressAndReleaseMapping(macroState3, Button.X,  Settings.Action.PRESSED, Settings.Key.Numpad5);
            addButtonToOneKeyPressAndReleaseMapping(macroState3, Button.Y,  Settings.Action.PRESSED, Settings.Key.Numpad6);
            addButtonToOneKeyPressAndReleaseMapping(macroState3, Button.A,  Settings.Action.PRESSED, Settings.Key.Numpad7);
            addButtonToOneKeyPressAndReleaseMapping(macroState3, Button.B,  Settings.Action.PRESSED, Settings.Key.Numpad8);
            
            //left stick
            addButtonToKeyPressMapping(macroState3, Button.LSU, Settings.Action.PRESSED,  Settings.Key.W, Settings.Action.PRESSED );
            addButtonToKeyPressMapping(macroState3, Button.LSU, Settings.Action.RELEASED, Settings.Key.W, Settings.Action.RELEASED);
            addButtonToKeyPressMapping(macroState3, Button.LSL, Settings.Action.PRESSED,  Settings.Key.A, Settings.Action.PRESSED );
            addButtonToKeyPressMapping(macroState3, Button.LSL, Settings.Action.RELEASED, Settings.Key.A, Settings.Action.RELEASED);
            addButtonToKeyPressMapping(macroState3, Button.LSD, Settings.Action.PRESSED,  Settings.Key.S, Settings.Action.PRESSED );
            addButtonToKeyPressMapping(macroState3, Button.LSD, Settings.Action.RELEASED, Settings.Key.S, Settings.Action.RELEASED);
            addButtonToKeyPressMapping(macroState3, Button.LSR, Settings.Action.PRESSED,  Settings.Key.D, Settings.Action.PRESSED );
            addButtonToKeyPressMapping(macroState3, Button.LSR, Settings.Action.RELEASED, Settings.Key.D, Settings.Action.RELEASED);

            //right stick
            addButtonToKeyPressMapping(macroState3, Button.RSU, Settings.Action.PRESSED,  Settings.Key.I, Settings.Action.PRESSED );
            addButtonToKeyPressMapping(macroState3, Button.RSU, Settings.Action.RELEASED, Settings.Key.I, Settings.Action.RELEASED);
            addButtonToKeyPressMapping(macroState3, Button.RSL, Settings.Action.PRESSED,  Settings.Key.J, Settings.Action.PRESSED );
            addButtonToKeyPressMapping(macroState3, Button.RSL, Settings.Action.RELEASED, Settings.Key.J, Settings.Action.RELEASED);
            addButtonToKeyPressMapping(macroState3, Button.RSD, Settings.Action.PRESSED,  Settings.Key.K, Settings.Action.PRESSED );
            addButtonToKeyPressMapping(macroState3, Button.RSD, Settings.Action.RELEASED, Settings.Key.K, Settings.Action.RELEASED);
            addButtonToKeyPressMapping(macroState3, Button.RSR, Settings.Action.PRESSED,  Settings.Key.L, Settings.Action.PRESSED );
            addButtonToKeyPressMapping(macroState3, Button.RSR, Settings.Action.RELEASED, Settings.Key.L, Settings.Action.RELEASED);

            addButtonToKeyPressMapping(macroState3, Button.LB, Settings.Action.PRESSED, Settings.Key.RightAlt  , Settings.Action.PRESSED );
            addButtonToKeyPressMapping(macroState3, Button.LT, Settings.Action.PRESSED, Settings.Key.LOGI_WIN  , Settings.Action.PRESSED );
            addButtonToKeyPressMapping(macroState3, Button.RB, Settings.Action.PRESSED, Settings.Key.Control   , Settings.Action.PRESSED );
            addButtonToKeyPressMapping(macroState3, Button.RT, Settings.Action.PRESSED, Settings.Key.LOGI_MENU , Settings.Action.PRESSED );
            addButtonToKeyPressMapping(macroState3, Button.LB, Settings.Action.RELEASED, Settings.Key.RightAlt , Settings.Action.RELEASED);
            addButtonToKeyPressMapping(macroState3, Button.LT, Settings.Action.RELEASED, Settings.Key.LOGI_WIN , Settings.Action.RELEASED);
            addButtonToKeyPressMapping(macroState3, Button.RB, Settings.Action.RELEASED, Settings.Key.Control  , Settings.Action.RELEASED);
            addButtonToKeyPressMapping(macroState3, Button.RT, Settings.Action.RELEASED, Settings.Key.LOGI_MENU, Settings.Action.RELEASED);

            settings.stateMappings.Add(macroState3);

            StateControllerMapping macroState4 = new StateControllerMapping();
            macroState4.stateName = "Macro State 4";

            // addButtonToTwoKeyPressAndReleaseMapping(macroState4, Button.DL, Settings.Action.PRESSED, Settings.Key.LOGI_MENU, Settings.Key.Numpad1);
            // addButtonToTwoKeyPressAndReleaseMapping(macroState4, Button.DU, Settings.Action.PRESSED, Settings.Key.LOGI_MENU, Settings.Key.Numpad2);
            // addButtonToTwoKeyPressAndReleaseMapping(macroState4, Button.DD, Settings.Action.PRESSED, Settings.Key.LOGI_MENU, Settings.Key.Numpad3);
            // addButtonToTwoKeyPressAndReleaseMapping(macroState4, Button.DR, Settings.Action.PRESSED, Settings.Key.LOGI_MENU, Settings.Key.Numpad4);
            // addButtonToTwoKeyPressAndReleaseMapping(macroState4, Button.X,  Settings.Action.PRESSED, Settings.Key.LOGI_MENU, Settings.Key.Numpad5);
            // addButtonToTwoKeyPressAndReleaseMapping(macroState4, Button.Y,  Settings.Action.PRESSED, Settings.Key.LOGI_MENU, Settings.Key.Numpad6);
            // addButtonToTwoKeyPressAndReleaseMapping(macroState4, Button.A,  Settings.Action.PRESSED, Settings.Key.LOGI_MENU, Settings.Key.Numpad7);
            // addButtonToTwoKeyPressAndReleaseMapping(macroState4, Button.B,  Settings.Action.PRESSED, Settings.Key.LOGI_MENU, Settings.Key.Numpad8);
            addButtonToOneKeyPressAndReleaseMapping(macroState4, Button.DL, Settings.Action.PRESSED, Settings.Key.Numpad1);
            addButtonToOneKeyPressAndReleaseMapping(macroState4, Button.DU, Settings.Action.PRESSED, Settings.Key.Numpad2);
            addButtonToOneKeyPressAndReleaseMapping(macroState4, Button.DD, Settings.Action.PRESSED, Settings.Key.Numpad3);
            addButtonToOneKeyPressAndReleaseMapping(macroState4, Button.DR, Settings.Action.PRESSED, Settings.Key.Numpad4);
            addButtonToOneKeyPressAndReleaseMapping(macroState4, Button.X,  Settings.Action.PRESSED, Settings.Key.Numpad5);
            addButtonToOneKeyPressAndReleaseMapping(macroState4, Button.Y,  Settings.Action.PRESSED, Settings.Key.Numpad6);
            addButtonToOneKeyPressAndReleaseMapping(macroState4, Button.A,  Settings.Action.PRESSED, Settings.Key.Numpad7);
            addButtonToOneKeyPressAndReleaseMapping(macroState4, Button.B,  Settings.Action.PRESSED, Settings.Key.Numpad8);
            
            //left stick
            addButtonToKeyPressMapping(macroState4, Button.LSU, Settings.Action.PRESSED,  Settings.Key.W, Settings.Action.PRESSED );
            addButtonToKeyPressMapping(macroState4, Button.LSU, Settings.Action.RELEASED, Settings.Key.W, Settings.Action.RELEASED);
            addButtonToKeyPressMapping(macroState4, Button.LSL, Settings.Action.PRESSED,  Settings.Key.A, Settings.Action.PRESSED );
            addButtonToKeyPressMapping(macroState4, Button.LSL, Settings.Action.RELEASED, Settings.Key.A, Settings.Action.RELEASED);
            addButtonToKeyPressMapping(macroState4, Button.LSD, Settings.Action.PRESSED,  Settings.Key.S, Settings.Action.PRESSED );
            addButtonToKeyPressMapping(macroState4, Button.LSD, Settings.Action.RELEASED, Settings.Key.S, Settings.Action.RELEASED);
            addButtonToKeyPressMapping(macroState4, Button.LSR, Settings.Action.PRESSED,  Settings.Key.D, Settings.Action.PRESSED );
            addButtonToKeyPressMapping(macroState4, Button.LSR, Settings.Action.RELEASED, Settings.Key.D, Settings.Action.RELEASED);

            //right stick
            addButtonToKeyPressMapping(macroState4, Button.RSU, Settings.Action.PRESSED,  Settings.Key.I, Settings.Action.PRESSED );
            addButtonToKeyPressMapping(macroState4, Button.RSU, Settings.Action.RELEASED, Settings.Key.I, Settings.Action.RELEASED);
            addButtonToKeyPressMapping(macroState4, Button.RSL, Settings.Action.PRESSED,  Settings.Key.J, Settings.Action.PRESSED );
            addButtonToKeyPressMapping(macroState4, Button.RSL, Settings.Action.RELEASED, Settings.Key.J, Settings.Action.RELEASED);
            addButtonToKeyPressMapping(macroState4, Button.RSD, Settings.Action.PRESSED,  Settings.Key.K, Settings.Action.PRESSED );
            addButtonToKeyPressMapping(macroState4, Button.RSD, Settings.Action.RELEASED, Settings.Key.K, Settings.Action.RELEASED);
            addButtonToKeyPressMapping(macroState4, Button.RSR, Settings.Action.PRESSED,  Settings.Key.L, Settings.Action.PRESSED );
            addButtonToKeyPressMapping(macroState4, Button.RSR, Settings.Action.RELEASED, Settings.Key.L, Settings.Action.RELEASED);

            addButtonToKeyPressMapping(macroState4, Button.LB, Settings.Action.PRESSED, Settings.Key.RightAlt  , Settings.Action.PRESSED );
            addButtonToKeyPressMapping(macroState4, Button.LT, Settings.Action.PRESSED, Settings.Key.LOGI_WIN  , Settings.Action.PRESSED );
            addButtonToKeyPressMapping(macroState4, Button.RB, Settings.Action.PRESSED, Settings.Key.Control   , Settings.Action.PRESSED );
            addButtonToKeyPressMapping(macroState4, Button.RT, Settings.Action.PRESSED, Settings.Key.LOGI_MENU , Settings.Action.PRESSED );
            addButtonToKeyPressMapping(macroState4, Button.LB, Settings.Action.RELEASED, Settings.Key.RightAlt , Settings.Action.RELEASED);
            addButtonToKeyPressMapping(macroState4, Button.LT, Settings.Action.RELEASED, Settings.Key.LOGI_WIN , Settings.Action.RELEASED);
            addButtonToKeyPressMapping(macroState4, Button.RB, Settings.Action.RELEASED, Settings.Key.Control  , Settings.Action.RELEASED);
            addButtonToKeyPressMapping(macroState4, Button.RT, Settings.Action.RELEASED, Settings.Key.LOGI_MENU, Settings.Action.RELEASED);

            settings.stateMappings.Add(macroState4);

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
