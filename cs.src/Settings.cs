using System;
using System.IO;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace FFXICompanion.Settings {

    [Serializable()]	
    public class CompanionSettings {
        public List<State> states = new List<State>();
        public List<StateTransition> stateTransitions = new List<StateTransition>();
        public List<StateControllerMapping> stateMappings = new List<StateControllerMapping>();
    }

    [Serializable()]	
    public class State {
        public string name {get; set;}
        public string description {get; set;}

        public State() {
            
        }

        public State(String name, String description) {
            this.name = name;
            this.description = description;
        }
    }

    [Serializable()]	
    public class StateTransition {
        public string stateName {get; set;}
        public List<ControllerButton> transitionButtons = new List<ControllerButton>();
    }

    public class StateControllerMapping {
        public string stateName {get; set;}

        public List<ControllerMapping> controllerMappings = new List<ControllerMapping>();
    }

    [Serializable()]	
    public class ControllerMapping {
        public ControllerButton button;
        public KeyPressesCommand keyPressesCommand;
        public AltTabCommand altTabCommand;
    }

    public interface CompanionCommand {

    }

    [Serializable()]
    public class KeyPressesCommand : CompanionCommand {
        public List<KeyPress> keyPresses = new List<KeyPress>();
    }

    [Serializable()]
    public class AltTabCommand : CompanionCommand {

    }

    [Serializable()]	
    public class ControllerButton {
        public Button button;
        public Action action;

        public ControllerButton() {}
        public ControllerButton(Button button, Action action) {
            this.action = action;
            this.button = button;
        }
    }

    [Serializable()]	
    public class KeyPress {
        public Key key;
        public Action action;

        public KeyPress() {}
        public KeyPress(Key key, Action action) {
            this.key = key;
            this.action = action;
        }
    }

    public enum Action {
        NULL,
        PRESSED,
        RELEASED
    }

    public enum Button {
        X,Y,B,A,
        DU,DD,DL,DR,
        LB,RB,
        LS,RS,
        LT, RT,
        LSU,LSD,LSL,LSR,
        RSU,RSD,RSL,RSR,
        START,BACK,
    }

   public enum Key : ushort
    {
        Escape = 1,
        One = 2,
        Two = 3,
        Three = 4,
        Four = 5,
        Five = 6,
        Six = 7,
        Seven = 8,
        Eight = 9,
        Nine = 10,
        Zero = 11,
        DashUnderscore = 12,
        PlusEquals = 13,
        Backspace = 14,
        Tab = 15,
        Q = 16,
        W = 17,
        E = 18,
        R = 19,
        T = 20,
        Y = 21,
        U = 22,
        I = 23,
        O = 24,
        P = 25,
        OpenBracketBrace = 26,
        CloseBracketBrace = 27,
        Enter = 28,
        Control = 29,
        A = 30,
        S = 31,
        D = 32,
        F = 33,
        G = 34,
        H = 35,
        J = 36,
        K = 37,
        L = 38,
        SemicolonColon = 39,
        SingleDoubleQuote = 40,
        Tilde = 41,
        LeftShift = 42,
        BackslashPipe = 43,
        Z = 44,
        X = 45,
        C = 46,
        V = 47,
        B = 48,
        N = 49,
        M = 50,
        CommaLeftArrow = 51,
        PeriodRightArrow = 52,
        ForwardSlashQuestionMark = 53,
        RightShift = 54,
        RightAlt = 56,
        Space = 57,
        CapsLock = 58,
        F1 = 59,
        F2 = 60,
        F3 = 61,
        F4 = 62,
        F5 = 63,
        F6 = 64,
        F7 = 65,
        F8 = 66,
        F9 = 67,
        F10 = 68,
        F11 = 87,
        F12 = 88,
        Up = 72,
        Down = 80,
        Right = 77,
        Left = 75,
        Home = 71,
        End = 79,
        Delete = 83,
        PageUp = 73,
        PageDown = 81,
        Insert = 82,
        PrintScreen = 55, // And break key is 42
        NumLock = 69,
        ScrollLock = 70,
        Menu = 93,
        WindowsKey = 91,
        NumpadDivide = 53,
        NumpadAsterisk = 55,
        Numpad7 = 71,
        Numpad8 = 72,
        Numpad9 = 73,
        Numpad4 = 75,
        Numpad5 = 76,
        Numpad6 = 77,
        Numpad1 = 79,
        Numpad2 = 80,
        Numpad3 = 81,
        Numpad0 = 82,
        NumpadDelete = 83,
        NumpadEnter = 28,
        NumpadPlus = 78,
        NumpadMinus = 74,

        LOGI_MENU = 221,
        LOGI_WIN = 219
    }
}