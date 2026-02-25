using System;
using System.Collections;
using Unity.Burst.CompilerServices;
using UnityEngine;
using static Constants;

public class TurnStateManager : MonoBehaviour
{

    private Coroutine _counterRoutine;

    public Action<BaseState, Constants.GameResult> onEndGame;
    
    private int _remainingSeconds;
    public int RemainingSeconds => _remainingSeconds;

    public void SetState(BaseState newState)
    {
        if(_counterRoutine !=null)
            StopCoroutine(_counterRoutine);
        
        // 턴 UI 변경
        string uiMessage = newState.ControllerType == ControllerType.Human ? "Player의 턴" : "AI의 턴";
        GameManager.Instance.GamePanelController.UpdateTurnUI(uiMessage);
        
        // 타이머 UI 변경
        GameManager.Instance.GamePanelController.UpdateTimerUI(_remainingSeconds);
        
        _counterRoutine = StartCoroutine(CounterRoutine(newState));
    }

    IEnumerator CounterRoutine(BaseState playerState)
    {
        _remainingSeconds = TIME_LIMIT;
        
        while (_remainingSeconds > 0)
        {
            _remainingSeconds--;
            yield return new WaitForSeconds(1f);
        }
        onEndGame?.Invoke(playerState, GameResult.Lose);
    }

    public void StopCounterRoutine()
    {
        StopCoroutine(_counterRoutine);
        GameManager.Instance.GameLogic.ChangeGameState();
    }
    //TODO: 끝나면 로직에서 호출할 함수: 기능-코루틴 정지, UI숨기기
}