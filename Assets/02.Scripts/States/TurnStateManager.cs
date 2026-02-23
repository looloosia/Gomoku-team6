using System;
using System.Collections;
using UnityEngine;
using static Constants;

public class TurnStateManager : MonoBehaviour
{

    private Coroutine counterRoutine;

    private BaseState currenState;

    public Action<BaseState, Constants.GameResult> onEndGame;

    public void SetState(BaseState newState)
    {
        currenState = newState;
        if(counterRoutine !=null)
            StopCoroutine(counterRoutine);
        counterRoutine = StartCoroutine(CounterRoutine(newState));
    }

    IEnumerator CounterRoutine(BaseState playerState)
    {
        int timeLimit = TIME_LIMIT;
        
        while (timeLimit > 0)
        {
            timeLimit--;
            Debug.Log($"{timeLimit}초");
            yield return new WaitForSeconds(1f);
        }
        onEndGame?.Invoke(playerState, GameResult.Lose);
    }
    //TODO: 끝나면 로직에서 호출할 함수: 기능-코루틴 정지, UI숨기기
}