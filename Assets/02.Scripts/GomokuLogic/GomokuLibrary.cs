using Unity.VisualScripting;
using UnityEngine;
using System.Collections;
using static Constants;
using System;
using UnityEngine.Rendering.Universal;
//큐로 기보 가능할지도 (포지션, 돌 타입)

public static class GomokuLibrary
{
    private static Queue forbiddenPositions;

    private static readonly Vector2Int[] directions = { new Vector2Int(0, 1), new Vector2Int(1, 0), new Vector2Int(1, 1), new Vector2Int(1, -1) };
    private static readonly Vector2Int[] doubleDirections =
        {
        new Vector2Int(0, 1), new Vector2Int(0, -1), new Vector2Int(1, 0), new Vector2Int(-1, 0),
        new Vector2Int(1, 1), new Vector2Int(-1, -1), new Vector2Int(1, -1), new Vector2Int(-1, 1)
    };

    //AI 점수 구현 시 필요
    //public static bool CheckGameWin(Constants.PlayerType[,] board, Constants.PlayerType playerType, int inRow, int inCol)
    //{
        
    //}

    //금수 체크함수
    public static Constants.ForbiddenType IsForbidden(Constants.PlayerType[,] board, Constants.PlayerType playerType, int inRow, int inCol, int boardRange)
    {
        //백돌은 금수없음
        if (playerType == Constants.PlayerType.White)
        {
            return Constants.ForbiddenType.None;
        }

        //TODO: 6
        if (CheckOverline(board, playerType, inRow, inCol, boardRange))
        {
            return Constants.ForbiddenType.Overline;
        }

        //오목이면 3-3, 4-4 다 상쇄 가능
        if (CheckGomoku(board, playerType, inRow, inCol, boardRange))
        {
            return Constants.ForbiddenType.None;
        }
        
        //TODO: 3-3인 경우 
        if (CheckDoubleThree(board, playerType, inRow, inCol, boardRange))
        {
            //TODO: 3-3이라도 오목 완성하면 None반환
            return ForbiddenType.DoubleThree;
        }
        //TODO: 4-4
        if (CheckDoubleFour(board, playerType, inRow, inCol, boardRange))
        {
            return ForbiddenType.DoubleFour;
        }
        return Constants.ForbiddenType.None;
    }

    //모든 칸에 IsDoubleThree, IsDoubleFour, IsOverline다 둘러보기
    public static void CheckForbiddenPostions(Constants.PlayerType[,] board, PlayerType playerType, int boardRange)
    {
        for (int r = 0; r < boardRange; r++)
        {
            for (int c = 0; c < boardRange; c++)
            {
                if (board[r, c] != PlayerType.None)//비어있지 않다면 통과
                    continue;

                if (IsForbidden(board, playerType, r, c, boardRange) != ForbiddenType.None) //금수 자리면
                {
                    board[r, c] = PlayerType.Forbidden; //둘 수 없도록 금수 위치 체크
                    forbiddenPositions.Enqueue(new Vector2Int(r, c));
                }
            }
        }
    }

    public static void ClearForbiddenPositionCheck(Constants.PlayerType[,] board)// 차례 넘어갈 때 금수 위치 체크한 것 다 해제.
    {
        while (forbiddenPositions.Count > 0)
        {
            Vector2Int position = (Vector2Int)forbiddenPositions.Dequeue();
            board[position[0], position[1]] = PlayerType.Forbidden; //forbiddenCheck된 것 해제
        }
    }
    //범위함수
    public static bool IsInRange(int inRow, int inCol, int boardRange)
    {
        return inRow >= 0 && inRow < boardRange && inCol >= 0 && inCol < boardRange;
    }
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    //AI구현
    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public static (int, int)? GetBestMove(Constants.PlayerType[,] board, PlayerType playerType, int boardRange)
    {
        int bestScore = int.MinValue;
        int bestRow = -1, bestCol = -1;

        for (int row = 0; row < boardRange; row++)
        {
            for (int col = 0; col < boardRange; col++)
            {
                if (board[row, col] == PlayerType.None) //빈 칸
                {
                    if (IsInRange(row, col, boardRange))
                    {
                        board[row, col] = playerType;

                        int score = Minimax(board, playerType, 3, false, int.MinValue, int.MaxValue, boardRange);

                        if (score > bestScore)
                        {
                            score = bestScore;
                            bestRow = row;
                            bestCol = col;
                        }
                    }
                }
            }
        }
        return (bestRow, bestCol);
    }

    public static int Minimax(Constants.PlayerType[,] board, PlayerType playerType, int depth, bool isMaximizing, int alpha, int beta, int boardRange)
    {
        if (depth == 0 /*|| IsGameOver(board)*/ ) //종료 조건: 최대 깊이 도달 혹은 게임 종료
        {

        }

        PlayerType otherPlayer = playerType == PlayerType.Black ? PlayerType.White : PlayerType.Black;

        if (isMaximizing) //Maximizing하는 순서
        {
            int maxScore = int.MinValue;

            for (int row = 0; row < boardRange; row++)
            {
                for (int col = 0; col < boardRange; col++)
                {
                    if (board[row, col] == PlayerType.None)
                    {
                        board[row, col] = playerType;   //TODO: 필요 시 보드 복제하는 코드로 대체하기

                        int score = Minimax(board, playerType, depth - 1, false, alpha, beta, boardRange);

                        maxScore = Math.Max(maxScore, score); // 최댓값 비교

                        alpha = Math.Max(alpha, score);

                        board[row, col] = PlayerType.None;

                        if (beta <= alpha)
                        {
                            break;
                        }
                    }
                }
                if (beta <= alpha)
                {
                    break;
                }
            }
            return maxScore;
        }
        else
        {
            int minScore = int.MaxValue;

            for (int row = 0; row < boardRange; row++)
            {
                for (int col = 0; col < boardRange; col++)
                {
                    int score = Minimax(board, otherPlayer, depth - 1, true, alpha, beta, boardRange);

                    minScore = Math.Min(minScore, score);
                    beta = Math.Min(beta, score);

                    board[row, col] = PlayerType.None;
                    if (beta <= alpha)
                    {
                        break;
                    }
                }
                if (beta <= alpha)
                {
                    break;
                }
            }
            return minScore;
        }

    }
    ///<summary>
    /// TODO: 점수 판정함수
    /// 4방향 돌리기
    /// 0,1,2,3해서
    /// 흑이면 
    /// ->3-3, 4-4 나올 때 X
    /// Hash를 가지고 OPEN_THREE나 FOUR가 있으면 그 자리에서 -점수 줘버리기
    /// 
    /// 백이면 그냥 다 더하기
    /// 
    /// </summary>
    /// 

    ///<summary>
    ///CountStones: 연속적으로 놓여있는 돌 개수 세는 함수
    ///</summary>

    private static int CountStones(PlayerType[,] board, PlayerType playerType, int inRow, int inCol, int boardRange, int dr, int dc)
    {
        int stoneCount = 0;

        int r = inRow + dr;
        int c = inCol + dc;

        while (true)
        {
            if (!IsInRange(r, c, boardRange))
            {
                return stoneCount;
            }

            if (board[r, c] == playerType)
            {
                stoneCount++;

                r += dr;
                c += dc;
                continue;
            }
            break;
        }

        return stoneCount;
    }
    ///<summary>
    ///CheckOmok: 오목 판정 bool함수
    ///</summary>
    public static bool CheckGomoku(PlayerType[,] board, PlayerType playerType, int inRow, int inCol, int boardRange) //오목 체크 
    {
        //놓은 돌 주변에 4개의 돌이 있다면 오목
        foreach (Vector2Int dir in directions)
        {
            if (CountStones(board, playerType, inRow, inCol, boardRange, dir[0], dir[1]) + CountStones(board, playerType, inRow, inCol, boardRange, -1 * dir[0], -1 * dir[1]) == 4)
            {
                return true;
            }
        }
        return false;
    }

    public static bool CheckOverline(PlayerType[,] board, PlayerType playerType, int inRow, int inCol, int boardRange) //장목 체크 
    {
        //놓은 돌 주변에 5개의 이상의 돌이 있다면 장목
        foreach (Vector2Int dir in directions)
        {
            if (CountStones(board, playerType, inRow, inCol, boardRange, dir[0], dir[1]) + CountStones(board, playerType, inRow, inCol, boardRange, -1 * dir[0], -1 * dir[1]) >= 5)
            {
                return true;
            }
        }
        return false;
    }

    public static bool CheckDoubleThree(PlayerType[,] board, PlayerType playerType, int inRow, int inCol, int boardRange)
    {
        PutStone(board, playerType, inRow, inCol, boardRange);
        int count = 0;

        foreach (Vector2Int dir in directions)
        {
            Vector2Int forwardDir = dir;
            Vector2Int backwardDir = -1 * dir;

            if (CheckOpenThree(board, playerType, inRow, inCol, boardRange, forwardDir[0], forwardDir[1]) ||
                CheckOpenThree(board, playerType, inRow, inCol, boardRange, backwardDir[0], backwardDir[1]))
            {
                count++;
            }

            if (count >= 2) //3이 2번 나온 경우
            {
                break;
            }
        }
        RemoveStone(board, inRow, inCol, boardRange);
        return count >= 2;
    }
    private static bool CheckOpenThree(PlayerType[,] board, PlayerType playerType, int inRow, int inCol, int boardRange, int dr, int dc)
    {
        var firstPlaced = FindEmpty(board, playerType, inRow, inCol, boardRange, dr, dc);

        if (firstPlaced == (-1, -1))
            return false;

        PutStone(board, playerType, firstPlaced.Item1, firstPlaced.Item2, boardRange);

        var secondPlaced = FindEmpty(board, playerType, firstPlaced.Item1, firstPlaced.Item2, boardRange, dr, dc);

        if (secondPlaced == (-1, -1))
        {
            RemoveStone(board, firstPlaced.Item1, firstPlaced.Item2, boardRange);
            return false;
        }

        PutStone(board, playerType, secondPlaced.Item1, secondPlaced.Item2, boardRange);

        bool isOmok = CheckGomoku(board, playerType, secondPlaced.Item1, secondPlaced.Item2, boardRange);

        if (isOmok)
        {
            int rr = secondPlaced.Item1 + dr;
            int cc = secondPlaced.Item2 + dc;

            if (IsInRange(rr, cc, boardRange))
            {
                if (board[rr, cc] == playerType)
                {
                    RemoveStone(board, firstPlaced.Item1, firstPlaced.Item2, boardRange);
                    RemoveStone(board, secondPlaced.Item1, secondPlaced.Item2, boardRange);
                    return false;
                }
            }

        }
        //반대방향이 막혔는지 체크
        var reverse = FindEmpty(board, playerType, inRow, inCol, boardRange, -dr, -dc);

        if (reverse == (-1, -1))
        {
            isOmok = false;
        }
        else if (board[reverse.Item1, reverse.Item2] != PlayerType.None)
        {
            isOmok = false;
        }
        else
        {
            //한 칸만 더 움직이기
            int rr = reverse.Item1 - dr;
            int cc = reverse.Item2 - dc;

            //한 칸 더 보냈을 때 같은 색 있으면 4-3
            if (IsInRange(rr, cc, boardRange))
            {
                if (board[rr, cc] == playerType)
                {
                    isOmok = false;
                }
            }
        }


        RemoveStone(board, firstPlaced.Item1, firstPlaced.Item2, boardRange);
        RemoveStone(board, secondPlaced.Item1, secondPlaced.Item2, boardRange);

        return isOmok;
    }

    //나중에 4가 있으면 33이 안 되도록 하기
    public static bool CheckFour(PlayerType[,] board, PlayerType playerType, int inRow, int inCol, int boardRange)//4가 있는지 체크. 나중에 분리하기
    {
        bool foundFour = false;

        PutStone(board, playerType, inRow, inCol, boardRange);
        foreach (Vector2Int dir in doubleDirections)
        {
            int dr = dir[0];
            int dc = dir[1];

            var emptyPos = FindEmpty(board, playerType, inRow, inCol, boardRange, dr, dc);
            if (emptyPos == (-1, -1))
            {
                continue;
            }
            //빈 칸 찾았으면 돌 한 번 놓아보고 오목인지 아닌지 체크

            PutStone(board, playerType, emptyPos.Item1, emptyPos.Item2, boardRange);

            bool isOmok = CheckGomoku(board, playerType, inRow, inCol, boardRange);

            RemoveStone(board, emptyPos.Item1, emptyPos.Item2, boardRange);

            if (isOmok)
            {
                foundFour = isOmok;
                break;
            }
        }
        RemoveStone(board, inRow, inCol, boardRange);
        return foundFour;
    }

    public static bool CheckDoubleFour(PlayerType[,] board, PlayerType playerType, int inRow, int inCol, int boardRange)//4-4가 되는지 체크. 나중에 분리하기
    {
        int fourCount = 0;

        PutStone(board, playerType, inRow, inCol, boardRange);
        foreach (Vector2Int dir in doubleDirections)
        {
            int dr = dir[0];
            int dc = dir[1];

            var emptyPos = FindEmpty(board, playerType, inRow, inCol, boardRange, dr, dc);
            if (emptyPos == (-1, -1))
            {
                continue;
            }
            //빈 칸 찾았으면 돌 한 번 놓아보고 오목인지 아닌지 체크

            PutStone(board, playerType, emptyPos.Item1, emptyPos.Item2, boardRange);

            bool isOmok = CheckGomoku(board, playerType, inRow, inCol, boardRange);

            RemoveStone(board, emptyPos.Item1, emptyPos.Item2, boardRange);

            if (isOmok)
            {
                fourCount++;
            }
        }
        RemoveStone(board, inRow, inCol, boardRange);
        return fourCount >= 2;
    }
    private static (int, int) FindEmpty(PlayerType[,] board, PlayerType playerType, int inRow, int inCol, int boardRange, int dr, int dc)
    {
        int r = inRow + dr;
        int c = inCol + dc;

        while (true)
        {
            if (!IsInRange(dr, dc, boardRange))
            {
                return (-1, -1);
            }

            if (board[r, c] == PlayerType.None) //빈 칸
            {
                return (r, c);
            }

            else if (board[r, c] == playerType) //같은 색
            {
                r += dr;
                c += dc;
                continue;
            }
            else //다른 색깔
            {
                return (-1, -1);
            }

        }
    }
    private static void PutStone(PlayerType[,] board, PlayerType playerType, int inRow, int inCol, int boardRange)
    {
        board[inRow, inCol] = playerType;
    }

    private static void RemoveStone(PlayerType[,] board, int inRow, int inCol, int boardRange)
    {
        board[inRow, inCol] = PlayerType.None;
    }
}