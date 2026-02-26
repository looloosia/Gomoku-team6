using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using static Constants;
//큐로 기보 가능할지도 (포지션, 돌 타입)


public static class GomokuLibrary
{
    private static List<List<(int, int)>> candidatesList = new List<List<(int, int)>> //DFS용 리스트
    {
        new List<(int, int)>(), // 0번 인덱스에 할당
        new List<(int, int)>(), // 1번 인덱스에 할당
        new List<(int, int)>()  // 2번 인덱스에 할당
    };

    private static Queue forbiddenPositions = new Queue();
    private static readonly Vector2Int[] directions = { new Vector2Int(0, 1), new Vector2Int(1, 0), new Vector2Int(1, 1), new Vector2Int(1, -1) };
    private static readonly Vector2Int[] doubleDirections =
        {
        new Vector2Int(0, 1), new Vector2Int(0, -1), new Vector2Int(1, 0), new Vector2Int(-1, 0),
        new Vector2Int(1, 1), new Vector2Int(-1, -1), new Vector2Int(1, -1), new Vector2Int(-1, 1)
    };

    public static PlayerType[,] experimentBoard = new PlayerType[15, 15];


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
            //Debug.Log("OVERLINE");
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
            //Debug.Log("DOUBLETHREE");
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
    public static Queue CheckForbiddenPostions(Constants.PlayerType[,] board, PlayerType playerType, int boardRange)
    {
               
        Queue retQueue = new Queue();
        for (int r = 0; r < boardRange; r++)
        {
            for (int c = 0; c < boardRange; c++)
            {
                if (board[r, c] != PlayerType.None)//비어있지 않다면 통과
                    continue;

                if (IsForbidden(board, playerType, r, c, boardRange) != ForbiddenType.None) //금수 자리면
                {
                    //Debug.Log($"FORBIDDENCHECK {r}, {c}");
                    //board[r, c] = PlayerType.Forbidden; //둘 수 없도록 금수 위치 체크
                    forbiddenPositions.Enqueue(new Vector2Int(r, c));
                    retQueue.Enqueue(new Vector2Int(r, c));
                }
            }
        }

        //Debug.Log("FORBIDDEN QUEUE START");
        //while (forbiddenPositions.Count!=0)
        //{
        //    Debug.Log(forbiddenPositions.Dequeue());
        //}
        //Debug.Log("FORBIDDEN QUEUE EMPTY");
        return retQueue;
    }

    public static Queue ClearForbiddenPositionCheck(Constants.PlayerType[,] board)// 차례 넘어갈 때 금수 위치 체크한 것 다 해제.
    {
        Queue retQueue = new Queue();
        while (forbiddenPositions.Count > 0)
        {
            Vector2Int position = (Vector2Int)forbiddenPositions.Dequeue();
            //board[position[0], position[1]] = PlayerType.None; //forbiddenCheck된 것 해제
            retQueue.Enqueue(new Vector2Int(position[0], position[1]));
        }
        return retQueue;
    }

    //범위함수
    public static bool IsInRange(int inRow, int inCol, int boardRange)
    {
        return inRow >= 0 && inRow < boardRange && inCol >= 0 && inCol < boardRange;
    }
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    //AI구현

    /// 수정해야할 수도 있는 부분
    ///: Minimax함수에서 isMaximizing이 바뀔 때, playerType매개변수 부분에 otherplayerType을 넣는게 맞는지 확인해봐야 함
    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    private static int EvaluateScore(PlayerType[,] board, PlayerType playerType, int inRow, int inCol, int boardRange)
    {
        return 0;
    }
    public static (int, int)? GetBestMove(PlayerType[,] board, PlayerType playerType, int boardRange)
    {
        int bestScore = int.MinValue;
        int bestRow = -1, bestCol = -1;

        List<(int, int)> candidates = GetCandidateMoves(board, 0, boardRange);

        foreach ((int, int) pos in candidates)
        {
            int row = pos.Item1;
            int col = pos.Item2;

            if (board[row, col] == PlayerType.None) //빈 칸
            {
                board[row, col] = playerType; //돌 놓아보고 미니맥스

                int score = Minimax(board, playerType, 1, false, int.MinValue, int.MaxValue, row, col, boardRange);

                if (score > bestScore)
                {
                    bestScore = score;
                    bestRow = row;
                    bestCol = col;
                }
                board[row, col] = PlayerType.None;//놓은 돌 초기화
            }
        }
        return (bestRow, bestCol);
    }

    public static int Minimax(Constants.PlayerType[,] board, PlayerType playerType, int depth, bool isMaximizing, int alpha, int beta, int inRow, int inCol, int boardRange)
    {
        if (depth == 3 /*|| IsGameOver(board)*/ ) //종료 조건: 최대 깊이 도달 혹은 게임 종료
        {
            return EvaluateScore(board, playerType, inRow, inCol, boardRange);
        }

        PlayerType otherPlayer = playerType == PlayerType.Black ? PlayerType.White : PlayerType.Black;

        List<(int, int)> candidates = GetCandidateMoves(board, depth-1, boardRange); //한 칸 위 후보

        if (isMaximizing) //Maximizing하는 순서
        {
            int maxScore = int.MinValue;

            foreach ((int, int) pos in candidates)
            {
                int row = pos.Item1;
                int col = pos.Item2;

                if (board[row, col] == PlayerType.None)
                {
                    AddCandidates(board, candidatesList[depth], depth, row, col, 2); //현재 depth리스트에 한 칸 위 후보 + row, col주변 부 추가
                    board[row, col] = playerType;   //TODO: 필요 시 보드 복제하는 코드로 대체하기

                    int score = Minimax(board, playerType, depth + 1, false, alpha, beta, row, col, boardRange);

                    maxScore = Math.Max(maxScore, score); // 최댓값 비교

                    alpha = Math.Max(alpha, score);

                    board[row, col] = PlayerType.None;

                    if (beta <= alpha)
                    {
                        break;
                    }

                }

            }
            return maxScore;
        }
        else
        {
            int minScore = int.MaxValue;

            foreach ((int, int) pos in candidates)
            {
                int row = pos.Item1;
                int col = pos.Item2;

                if (board[row, col] == PlayerType.None)
                {
                    AddCandidates(board, candidatesList[depth], depth, row, col, 2); //현재 depth리스트에 한 칸 위 후보 + row, col주변 부 추가
                    board[row, col] = otherPlayer;
                    int score = Minimax(board, otherPlayer, depth + 1, true, alpha, beta, row, col, boardRange);

                    minScore = Math.Min(minScore, score);
                    beta = Math.Min(beta, score);

                    board[row, col] = PlayerType.None;

                    if (beta <= alpha)
                    {
                        break;
                    }
                }
            }

            return minScore;
        }

    }

    //Minimax범위 제한 함수. 돌이 많이 있는 곳이 돌을 놓기 유리할 자리일 확률이 높다는 것을 전제
    private static List<(int, int)> GetCandidateMoves(PlayerType[,] board, int depth, int boardRange)
    {
        const int RADIUS = 2;
        List<(int, int)> candidates = candidatesList[depth];
        bool[,] visited = new bool[boardRange, boardRange];

        if (depth == 0)
        {
            for (int r = 0; r < boardRange; r++)
            {
                for (int c = 0; c < boardRange; c++)
                {
                    if (board[r, c] != PlayerType.None) // 돌이 있는 곳 발견
                    {
                        // 주변 radius 칸을 후보지에 추가
                        for (int dr = -RADIUS; dr <= RADIUS; dr++)
                        {
                            for (int dc = -RADIUS; dc <= RADIUS; dc++)
                            {
                                int rr = r + dr;
                                int cc = c + dc;

                                if (IsInRange(rr, cc, boardRange) && board[rr, cc] == PlayerType.None && !visited[rr, cc])
                                {
                                    candidates.Add((rr, cc));
                                    visited[rr, cc] = true;
                                }
                            }
                        }
                    }
                }

            }
        }
        else
        {
            return candidatesList[depth - 1];
        }

        // 만약 판이 비어있다면 중앙점 반환
        if (candidates.Count == 0) return new List<(int, int)> { (boardRange / 2, boardRange / 2) };

        return candidates;
    }

    private static void AddCandidates(PlayerType[,] board, List<(int,int)> list, int depth, int inRow, int inCol, int radius)
    {
        candidatesList[depth].Clear();
        candidatesList[depth].AddRange(candidatesList[depth-1]);

        int rStart = inRow - radius;
        int rEnd = inRow + radius;

        int cStart = inCol - radius;
        int cEnd = inCol + radius;

        for(int r=rStart;r<=rEnd;r++)
        {
            for(int c=cStart;c<=cEnd;c++)
            {
                if ((r == inRow && c == inCol) || !IsInRange(r, c, BOARD_SIZE) || board[r, c] != PlayerType.None)
                    continue;
                else
                {
                    candidatesList[depth + 1].Add((r, c));
                }
            }
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

        //Debug.Log("CountStones START");
        while (true)
        {

            if (!IsInRange(r, c, boardRange))
            {
                return stoneCount;
            }
            if (board[r, c] == playerType)
            {
                //Debug.Log($"R: {r}, C: {c}");
                stoneCount++;

                r += dr;
                c += dc;
                continue;
            }
            else
            {
                //Debug.Log($"OTHER TYPE R: {r}, C: {c}");
                break;
            }
        }
        //Debug.Log($"CountStones END: {stoneCount}");
        return stoneCount;
    }
    ///<summary>
    ///CheckOmok: 오목 판정 bool함수
    ///</summary>
    public static bool CheckGomoku(PlayerType[,] board, PlayerType playerType, int inRow, int inCol, int boardRange) //오목 체크 
    {
        //놓은 돌 주변에 4개의 돌이 있다면 오목
        int stoneCount = 0;
        foreach (Vector2Int dir in directions)
        {
            stoneCount = CountStones(board, playerType, inRow, inCol, boardRange, dir[0], dir[1]) + CountStones(board, playerType, inRow, inCol, boardRange, -1 * dir[0], -1 * dir[1]);
            if (stoneCount == 4)
            {
                //Debug.Log("GOMOKU");
                return true;
            }
            //Debug.Log($"checkGomoku Result: {stoneCount}");
        }
        return false;
    }

    public static bool CheckOverline(PlayerType[,] board, PlayerType playerType, int inRow, int inCol, int boardRange) //장목 체크 
    {
        //Debug.Log("장목 찾기");
        //놓은 돌 주변에 5개의 이상의 돌이 있다면 장목
        int stoneCount = 0;
        foreach (Vector2Int dir in directions)
        {
            stoneCount = CountStones(board, playerType, inRow, inCol, boardRange, dir[0], dir[1]) + CountStones(board, playerType, inRow, inCol, boardRange, -1 * dir[0], -1 * dir[1]);
            if (stoneCount >= 5)
            {
                //      Debug.Log("OVERLINE");
                return true;
            }
            //Debug.Log($"CheckOverline Result: {stoneCount}");
        }
        return false;
    }

    public static bool CheckDoubleThree(PlayerType[,] board, PlayerType playerType, int inRow, int inCol, int boardRange)
    {
        //Debug.Log("3-3 찾기");
        PutStone(board, playerType, inRow, inCol, boardRange);
        int count = 0;

        foreach (Vector2Int dir in directions)
        {
            Vector2Int forwardDir = dir;
            Vector2Int backwardDir = -1 * dir;

            //가로, 세로, 우상향, 우하향 방향에서 두 개 이상 openThree가 나올 경우
            if (CheckOpenThree(board, playerType, inRow, inCol, boardRange, forwardDir[0], forwardDir[1]) || CheckOpenThree(board, playerType, inRow, inCol, boardRange, backwardDir[0], backwardDir[1]))
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

        //Debug.Log("3 찾기");
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

        //Debug.Log("4 찾기");
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
        //Debug.Log("4-4 찾기");
        int fourCount = 0;

        PutStone(board, playerType, inRow, inCol, boardRange);

        foreach (Vector2Int dir in directions)
        {
            bool forwardIsOmok = false;
            bool backwardIsOmok = false;

            Vector2Int forwardDir = dir;
            Vector2Int backwardDir = dir * -1;

            //정방향
            var forwardEmptyPos = FindEmpty(board, playerType, inRow, inCol, boardRange, forwardDir[0], forwardDir[1]);

            if (forwardEmptyPos != (-1, -1))
            {

                //빈 칸 찾았으면 돌 한 번 놓아보고 오목인지 아닌지 체크

                PutStone(board, playerType, forwardEmptyPos.Item1, forwardEmptyPos.Item2, boardRange);

                forwardIsOmok = CheckGomoku(board, playerType, inRow, inCol, boardRange);

                RemoveStone(board, forwardEmptyPos.Item1, forwardEmptyPos.Item2, boardRange);
            }

            //역방향
            var backwardEmptyPos = FindEmpty(board, playerType, inRow, inCol, boardRange, backwardDir[0], backwardDir[1]);

            if (backwardEmptyPos != (-1, -1))
            {

                //빈 칸 찾았으면 돌 한 번 놓아보고 오목인지 아닌지 체크

                PutStone(board, playerType, backwardEmptyPos.Item1, backwardEmptyPos.Item2, boardRange);

                backwardIsOmok = CheckGomoku(board, playerType, inRow, inCol, boardRange);

                RemoveStone(board, backwardEmptyPos.Item1, backwardEmptyPos.Item2, boardRange);
            }

            if (forwardIsOmok && backwardIsOmok) //한 라인에서 5목이 두 번 나온 경우
            {
                PutStone(board, playerType, forwardEmptyPos.Item1, forwardEmptyPos.Item2, boardRange);
                PutStone(board, playerType, backwardEmptyPos.Item1, backwardEmptyPos.Item2, boardRange);

                int oneLineCount = CountStones(board, playerType, inRow, inCol, boardRange, forwardDir[0], forwardDir[1])
                + CountStones(board, playerType, inRow, inCol, boardRange, backwardDir[0], backwardDir[1]);

                if (oneLineCount > 5) //3개의 돌이 놓였을 때에 한 줄에 두 4가 나오는 오류가 있다. 그래서 3개의 경우 empty를 채웠을 때 연속된 돌의 개수가 6이 나온다고 가정하여 세운 if이다.
                {
                    fourCount += 2;
                }
                else
                {
                    fourCount++;
                    //Debug.Log("잘못된 4-4탐지");
                }

                RemoveStone(board, forwardEmptyPos.Item1, forwardEmptyPos.Item2, boardRange);
                RemoveStone(board, backwardEmptyPos.Item1, backwardEmptyPos.Item2, boardRange);
            }
            else if (forwardIsOmok || backwardIsOmok)
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
            if (!IsInRange(r, c, boardRange))
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
    public static void PutStone(PlayerType[,] board, PlayerType playerType, int inRow, int inCol, int boardRange)
    {
        board[inRow, inCol] = playerType;
        //Debug.Log($"PUTSTONE: {inRow}, {inCol}");
    }

    private static void RemoveStone(PlayerType[,] board, int inRow, int inCol, int boardRange)
    {
        board[inRow, inCol] = PlayerType.None;
    }
}