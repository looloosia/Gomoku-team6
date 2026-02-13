using UnityEngine;
using UnityEngine.AdaptivePerformance;

using UnityEngine;

/// <summary>
/// 게임 턴 상태 기본 클래스
/// 현재 Constants(PlayerType), GameLogic(CheckGameResult(), PlaceMarker()) 필요
/// </summary>
public abstract class BaseState
{
    private TurnManager _turnManager;
    public abstract void OnEnter(GameLogic gameLogic);                      // 상태 진입 시 호출
    public abstract void HandleMove(GameLogic gameLogic, int index);        // 플레이어 이동 처리
    public abstract void OnExit(GameLogic gameLogic);                       // 상태 종료 시 호출
    public abstract void HandleNextTurn(GameLogic gameLogic);               // 다음 턴 처리
    
    public void ProcessMove(GameLogic gameLogic, int index, Constants.PlayerType playerType) // Constants 생기면 참조 다시설정
    {
        // 특정 위치에 마커(바둑알) 표시
        if (gameLogic.PlaceMarker(index, playerType))
        {
            // 게임 승패 확인
            var gameResult = gameLogic.CheckGameResult();
            
            if (gameResult == GameLogic.GameResult.None)
            {
                // 턴 전환
                HandleNextTurn(gameLogic);
                Debug.Log("턴 전환");
            }
            else if (gameResult == GameLogic.GameResult.Win)
            {
                // 게임 승리 처리
                gameLogic.EndGame(gameResult);
                Debug.Log("게임 승리");
            }
            else if (gameResult == GameLogic.GameResult.Draw)
            {
                // 게임 무승부 처리
                gameLogic.EndGame(gameResult);
                Debug.Log("게임 무승부");
            }
            else
            {
                // 게임 패배 처리
                gameLogic.EndGame(gameResult);
                Debug.Log("게임 패배");
            }
        }
    }
}
