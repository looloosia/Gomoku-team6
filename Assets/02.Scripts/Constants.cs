using UnityEngine;

public static class Constants
{

    public const string SCENE_MAIN = "Main";
    public const string SCENE_GAME = "Game";
    public const string SCENE_LOBBY = "Lobby";

    public const int BOARD_SIZE = 15;

    public const int TIME_LIMIT = 5;
    public enum GameType { SinglePlay, LocalDualPlay /*, MultiDualPlay*/ }
    public enum PlayerType { None, Black, White, Forbidden }

    public enum ControllerType { None, Human, AI }

    public enum GameResult { None, Win, Lose }
    public enum ForbiddenType { None, DoubleThree, DoubleFour, Overline } //3-3, 4-4

    public enum MarkerChoice { None, Black, White, Random };
    public enum GameResultType { None, ConnectFive, Surrender }
}
