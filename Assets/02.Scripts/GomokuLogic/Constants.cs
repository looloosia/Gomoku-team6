public static class Constants
{

    public const string SCENE_MAIN = "Main";
    public const string SCENE_GAME = "Game";

    public const int BOARD_SIZE = 15;

    public const int TIME_LIMIT = 5;
    public enum GameType { SinglePlay, LocalDualPlay /*, MultiDualPlay*/ }
    public enum PlayerType { None, Black, White }

    public enum ForbiddenType { None, Three, Four, Long } //3-3, 4-4, ¿Â∏Ò
}