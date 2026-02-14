using System;
using System.Collections;
using UnityEngine;
using static Constants;
using static UnityEditor.Rendering.InspectorCurveEditor;

public class DemoTurnStateManager : MonoBehaviour
{

    private Coroutine counterRoutine;

    private DemoBaseState currenState;

    public Action<DemoBaseState> endGameDelegate;

    public void SetState(DemoBaseState newState/*, PlayerType playerType*/)
    {
        currenState = newState;
        if(counterRoutine !=null)
            StopCoroutine(counterRoutine);
        counterRoutine = StartCoroutine(CounterRoutine(newState));
    }

    IEnumerator CounterRoutine(DemoBaseState playerState)
    {
        int timeLimit = TIME_LIMIT;
        
        while(timeLimit>0)
        {
            timeLimit--;
            Debug.Log($"{timeLimit}초");
            yield return new WaitForSeconds(1f);
        }
        //TODO: gameManager에서 게임 끝 함수 호출
        endGameDelegate?.Invoke(playerState);
    }
}
