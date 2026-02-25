using UnityEngine;

namespace _02.Scripts.States
{
    public class AIState : BaseState
    {
        public AIState(Constants.PlayerType playerType) : base(playerType, Constants.ControllerType.AI)
        {
        }

        public override void OnEnter(GomokuGameLogic gameLogic)
        {
            GameManager.Instance.SetGameTurn(_playerType);
        }

        public override void HandleMove(GomokuGameLogic gameLogic, int inRow, int inCol)
        {
            ProcessMove(gameLogic, inRow, inCol);
        }

        public override void OnExit(GomokuGameLogic gameLogic)
        {
        }

        public override void HandleNextTurn(GomokuGameLogic gameLogic)
        {
            gameLogic.ChangeGameState();
        }
    }
}