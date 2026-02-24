using UnityEngine;
using UnityEngine.SceneManagement;
using static Constants;

public class DemoGameManager : DemoSingleton<DemoGameManager>
{

    private GomokuGameLogic gameLogic;
    private PlayerType playerType;
    
    protected override void OnSceneLoad(Scene scene, LoadSceneMode mode)
    {
        if(scene.name == SCENE_GAME)
        {
            DemoTurnStateManager turnStateManager = FindFirstObjectByType<DemoTurnStateManager>();
            //BlockController blockController = FindFirstObjectByType<BlockController>();
            if (turnStateManager == null)
            {
                Debug.LogError("턴 스테이트 매니저가 씬 상에 존재하지 않습니다.");
                return;
            }

            //셀렉트 창에서 고른 PlayerType
            gameLogic = new GomokuGameLogic(GameType.LocalDualPlay/*,blockController*/ ,PlayerType.Black, turnStateManager);
        }
        Debug.Log("씬 로드");
    }

    

}
