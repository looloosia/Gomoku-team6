using UnityEngine;

namespace _02.Scripts.States
{
    public class AIState : BaseState
    {
        public AIState(Constants.PlayerType playerType) : base(playerType, Constants.ControllerType.AI)
        {
        }
        
        // 한 턴이 시작될 때
        public override void OnEnter(GomokuGameLogic gameLogic)
        {
            _gameLogic = gameLogic;
            _board = GameManager.Instance.Board;

            _gameLogic.onBlockClicked = OnStonePlace;
        
            _gamePlayerType = GameManager.Instance.GamePlayerType;
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