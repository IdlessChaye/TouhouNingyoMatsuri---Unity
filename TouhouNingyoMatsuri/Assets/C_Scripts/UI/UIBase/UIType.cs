using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIType {
    public string Path { get; private set; }
    public string Name { get; private set; }

    public UIType(string path) {
        Path = path;
        Name = path.Substring(path.LastIndexOf('/') + 1);
    }

    public override string ToString() {
        return string.Format("path: {0} name: {1})", Path, Name);
    }

    public static readonly UIType MainMenuBG = new UIType("View/MainMenuBGView");
    public static readonly UIType MainMenu = new UIType("View/MainMenuView");
    public static readonly UIType GameModeSelect = new UIType("View/GameModeSelectView");
    public static readonly UIType GameInitialSet = new UIType("View/GameInitialSetView");
    public static readonly UIType CharacterSelect = new UIType("View/CharacterSelectView");
    public static readonly UIType MusicRoom = new UIType("View/MusicRoomView");
    public static readonly UIType Result = new UIType("View/ResultView");
    public static readonly UIType Config = new UIType("View/ConfigView");
    public static readonly UIType Loading = new UIType("View/LoadingView");
    public static readonly UIType ESC = new UIType("View/ESCView");
    public static readonly UIType FinalScore = new UIType("View/FinalScoreView");
}
