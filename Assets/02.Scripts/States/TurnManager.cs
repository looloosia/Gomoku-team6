using UnityEngine;

/// <summary>
/// GameType에 따른 턴 초기와 작업
/// 
/// </summary>
public class TurnManager : MonoBehaviour
{
    // public BaseState playerAState;
    // public BaseState playerBState;
    // private BaseState _currentState;
    // private GameLogic  _gameLogic;
    //
    // public void InitState(Constants.GameType gameType, GameLogic gameLogic)
    // {
    //     _gameLogic = gameLogic;
    //     switch (gameType)
    //     {
    //         case GameType.SinglePlay:
    //             // 싱글 플레이어 모드 초기화 작업
    //             playerAState = new PlayerState(true);
    //             playerBState = new AIState(false);
    //             
    //             // 초기 상태 설정 (예: 플레이어 A부터 시작)
    //             SetState(playerAState);
    //             break;
    //         case GameType.DualPlay:
    //             // 듀얼 플레이어 모드 초기화 작업
    //             playerAState = new PlayerState(true);
    //             playerBState = new PlayerState(false);
    //
    //             // 초기 상태 설정 (예: 플레이어 A부터 시작)
    //             SetState(playerAState);
    //             break;
    //     }
    // }
    //
    // // 턴 변경
    // public void ChangeGameState()
    // {
    //     if (_currentState == playerAState)
    //     {
    //         SetState(playerBState);
    //     }
    //     else
    //     {
    //         SetState(playerAState);
    //     }
    // }
    //
    //
    // // 턴 바뀔 때 호출되는 메서드 (상태 전환 메서드)
    // public void SetState(BaseState newState)
    // {
    //     _currentState?.OnExit(_gameLogic);
    //     _currentState = newState;
    //     _currentState.OnEnter((_gameLogic));
    // }
}
