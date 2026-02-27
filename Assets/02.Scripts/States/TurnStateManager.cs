using System;
using System.Collections;
using Unity.Burst.CompilerServices;
using UnityEngine;
using static Constants;

/// <summary>
/// 타이머 기능과 턴, 타이머 UI 업데이트 기능
/// </summary>
public class TurnStateManager : MonoBehaviour
{ 
    private Coroutine _counterRoutine;

    public Action<BaseState, Constants.GameResult> onEndGame;
    
    // 남은 초
    private int _remainingSeconds;
    public int RemainingSeconds => _remainingSeconds;

    public void SetState(BaseState newState)
    {
        if(_counterRoutine !=null)
            StopCoroutine(_counterRoutine);
        
        var panel = UIManager.Instance.GamePanelController;

        if (panel != null)
        {
            // 턴 UI 변경
            string uiMessage = newState.ControllerType == ControllerType.Human ? "Player의 턴" : "AI의 턴";
            panel.UpdateTurnUI(uiMessage);
        
            // 타이머 UI 변경
            panel.UpdateTimerUI(_remainingSeconds);
        }
        
        _counterRoutine = StartCoroutine(CounterRoutine(newState));
    }

    IEnumerator CounterRoutine(BaseState playerState)
    {
        _remainingSeconds = TIME_LIMIT;
        
        while (_remainingSeconds >= 0)
        {
            _remainingSeconds--;
            
            // 타이머 UI 변경
            UIManager.Instance.GamePanelController.UpdateTimerUI(_remainingSeconds);
            
            yield return new WaitForSeconds(1f);
        }
        onEndGame?.Invoke(playerState, GameResult.Lose);
    }

    public void StopCounterRoutine()
    {
        StopCoroutine(_counterRoutine);
    }
    //TODO: 끝나면 로직에서 호출할 함수: 기능-코루틴 정지, UI숨기기
}