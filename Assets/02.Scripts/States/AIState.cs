using UnityEngine;

namespace _02.Scripts.States
{
    public class AIState : BaseState
    {
        public AIState(Constants.PlayerType playerType) : base(playerType, Constants.ControllerType.AI)
        {
            Debug.Log("AI생성자: " + playerType);
        }

        // 한 턴이 시작될 때
        public override void OnEnter(GomokuGameLogic gameLogic)
        {
            //GameManager.Instance.SetGameTurn(_currentPlayerType);
            (int, int)? bestMove = GomokuLibrary.GetBestMove(gameLogic.VirtualBoard, _currentPlayerType, 15);
            Debug.Log("AI: " + bestMove);
            if (bestMove.HasValue)
            {
                HandleMove(gameLogic, bestMove.Value.Item1, bestMove.Value.Item2);
            }
        }

        public override void HandleNextTurn(GomokuGameLogic gameLogic)
        {
            gameLogic.ChangeGameState();
        }

        public override void HandleMove(GomokuGameLogic gameLogic, int inRow, int inCol)
        {
            ProcessMove(gameLogic, inRow, inCol);
        }

        // 한 턴이 끝날 때
        public override void OnExit(GomokuGameLogic gameLogic)
        {
        }


    }
}