using UnityEngine;
//큐로 기보 가능할지도 (포지션, 돌 타입)

public static class GomokuLibrary
{
    private static readonly Vector2Int[] directions = { new Vector2Int(0, 1), new Vector2Int(1, 0), new Vector2Int(1, 1), new Vector2Int(1, -1) };
    public static bool CheckGameWin(Constants.PlayerType playerType, Constants.PlayerType[,] board, int inRow, int inCol)
    {
        int totalCount = 0;
        int boardSize = board.GetLength(0); //보드 사이즈는 가로세로 고정

        foreach (var dir in directions)
        {
            // dir은 순서대로 가로, 세로, 우상향, 우하향 대각선 검사

            int curRow = inRow + dir[0];
            int curCol = inCol + dir[1];

            //정방향
            while (IsInRange(curRow, curCol, boardSize) && board[curRow, curCol] == playerType)
            {
                totalCount++;
                curRow += dir[0];
                curCol += dir[1];
            }

            while (IsInRange(curRow, curCol, boardSize) && board[curRow, curCol] == playerType)
            {
                totalCount++;
                curRow -= dir[0];
                curCol -= dir[1];
            }

            //흑돌은 정확히 5개면 승리
            if (playerType == Constants.PlayerType.Black)
            {
                return totalCount == 5;

            }
            //백돌은 5개 이상이면 승리
            else
            {
                return totalCount >= 5;
            }
        }
        return false;
    }
    //금수 체크함수
    public static Constants.ForbiddenType IsForbidden(Constants.PlayerType playerType, Constants.PlayerType[,] board, int inRow, int inCol)
    {
        //백돌은 금수없음
        if (playerType == Constants.PlayerType.White)
        {
            return Constants.ForbiddenType.None;
        }
        //TODO: 3-3인 경우 

        //TODO: 4-4

        

        return Constants.ForbiddenType.None;
    }

    //범위함수
    public static bool IsInRange(int inRow, int inCol, int boardRange)
    {
        return inRow >= 0 && inRow < boardRange && inCol >= 0 && inCol < boardRange;
    }
}

