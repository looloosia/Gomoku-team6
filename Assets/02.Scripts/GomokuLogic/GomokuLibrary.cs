using Unity.VisualScripting;
using UnityEngine;
using System.Collections;
using static Constants;
using System;
//큐로 기보 가능할지도 (포지션, 돌 타입)

public static class GomokuLibrary
{
    private static Queue forbiddenPositions;

    private static readonly Vector2Int[] directions = { new Vector2Int(0, 1), new Vector2Int(1, 0), new Vector2Int(1, 1), new Vector2Int(1, -1) };
    public static bool CheckGameWin(Constants.PlayerType[,] board, Constants.PlayerType playerType, int inRow, int inCol)
    {
        if (IsOverline(board, playerType, inRow, inCol))
        {
            return false;
        }
        if (IsGomoku(board, playerType, inRow, inCol))
            return true;
        else
            return false;
    }

    //금수 체크함수
    public static Constants.ForbiddenType IsForbidden(Constants.PlayerType[,] board, Constants.PlayerType playerType, int inRow, int inCol)
    {
        //백돌은 금수없음
        if (playerType == Constants.PlayerType.White)
        {
            return Constants.ForbiddenType.None;
        }

        //TODO: 3-3인 경우 
        if (IsDoubleThree(board, playerType, inRow, inCol))
        {
            //TODO: 3-3이라도 오목 완성하면 None반환
            return ForbiddenType.DoubleThree;
        }
        //TODO: 4-4
        if (IsDoubleFour(board, playerType, inRow, inCol))
        {
            //TODO: 4-4라도 오목 완성하면 None반환
            return ForbiddenType.DoubleFour;
        }
        return Constants.ForbiddenType.None;
    }

    //TODO: 모든 칸에 IsDoubleThree, IsDoubleFour, IsOverline다 둘러보기

    public static void CheckForbiddenPostions(Constants.PlayerType[,] board, PlayerType playerType, int boardRange)
    {
        for (int r = 0; r < boardRange; r++)
        {
            for (int c = 0; c < boardRange; c++)
            {
                if (IsForbidden(board, playerType, r, c) != ForbiddenType.None)
                {
                    forbiddenPositions.Enqueue(new Vector2Int(r, c));
                }
            }
        }
    }

    public static void ClearForbiddenPositionCheck(Constants.PlayerType[,] board)
    {
        while (forbiddenPositions.Count > 0)
        {
            Vector2Int position = (Vector2Int)forbiddenPositions.Dequeue();
            //board[position[0], position[1]]; //forbiddenCheck된 것 해제
        }
    }
    //범위함수
    public static bool IsInRange(int inRow, int inCol, int boardRange)
    {
        return inRow >= 0 && inRow < boardRange && inCol >= 0 && inCol < boardRange;
    }

    //3-3
    private static bool IsDoubleThree(Constants.PlayerType[,] board, PlayerType playerType, int inRow, int inCol)
    {
        int openThreeChecked = 0;    //열린 삼 개수

        for (int i = 0; i < 4; i++) //방향 탐색 위한 반복문
        {
            if (IsOpenThree(board, playerType, inRow, inCol, i))
            {
                openThreeChecked++;
            }
        }
        return true;
    }

    //열린3
    private static bool IsOpenThree(Constants.PlayerType[,] board, PlayerType playerType, int inRow, int inCol, int dir)
    {

        PlayerType otherType = playerType == PlayerType.Black ? PlayerType.White : PlayerType.Black;

        int curRow = inRow;
        int curCol = inCol;

        int rdir = directions[dir][0];
        int cdir = directions[dir][1];

        int forwardBlank = 1;
        int backwardBlank = 0;

        int forwardCount = 0;
        int backwardCount = 0;

        bool isSecondContinuous = false; //두 번 연속 빈 칸 방문 시 걸러내는 부울 변수


        //정방향
        while (true)
        {
            curRow += rdir;
            curCol += cdir;

            if (!IsInRange(curRow, curCol, BOARD_SIZE)) //범위 제한
            {
                break;
            }

            if (board[curRow, curCol] != PlayerType.None) //빈 칸이 아닐 경우
            {
                if (board[curRow, curCol] == playerType) //현재 돌이면
                {
                    isSecondContinuous = false;
                    forwardCount++;
                }

                else//상대 돌이면
                {
                    break;
                }

            }
            else //빈 칸일 경우
            {
                if (!isSecondContinuous) //첫 빈 칸일 경우
                {
                    isSecondContinuous = true;
                }
                else //두 번 연속 빈 칸일 경우
                {
                    //돌 사이에 빈 칸이 없었던 경우로 판단하기 위해 1로 복구
                    forwardBlank++;
                    break;
                }

                if (forwardBlank == 1) //첫 빈 칸일 경우, 1회 차감
                {
                    forwardBlank--;
                }
                else //연속 두 번 빈 칸은 아니지만, 빈 칸 2개인 열린 삼은 없다.
                {
                    break;
                }
            }
        }

        //초기화
        isSecondContinuous = false;
        curRow = inRow;
        curCol = inCol;

        /*
        forwardBlank:
        1. 0이라면 이미 사이에 빈 칸이 하나 있어 더 이상 빈 칸이 있으면 안 된다.
        2. 1이라면,
            2-1. 빈 칸이 연속 두 번 나왔다.
            2-2. 정방향의 끝 쪽에 상대편 돌이 있다.
        한 쪽 끝의 상태를 파악할 때, 그 위치를 파악하기 위해 forwardBlank가 쓰인다.
        forwardBlank == 1인 경우, 빈 칸이 없던 경우로 판단할 것이므로, 0으로 만든다. 
         */
        backwardBlank = forwardBlank;

        if (forwardBlank == 1) //빈 칸이 없는 경우
        {
            forwardBlank = 0; //옆에 더할 때 0
        }
        else//빈 칸이 있는 경우
        {
            forwardBlank = 1;//옆에 더할 때 1
        }

        //역방향
        while (true)
        {
            curRow -= rdir;
            curCol -= cdir;

            if (!IsInRange(curRow, curCol, BOARD_SIZE)) //범위 제한
            {
                break;
            }

            if (board[curRow, curCol] != PlayerType.None) //빈 칸이 아닐 경우
            {
                if (board[curRow, curCol] == playerType) //현재 돌이면
                {
                    isSecondContinuous = false; //연속 두 번 빈 칸이 아니므로 false로 초기화
                    backwardCount++;
                }

                else//상대 돌이면
                {
                    break;
                }

            }
            else //빈 칸일 경우
            {
                if (!isSecondContinuous) //첫 빈 칸일 경우
                {
                    isSecondContinuous = true;
                }
                else //두 번 연속 빈 칸일 경우
                {
                    //돌 사이에 빈 칸이 없었던 경우로 판단하기 위해 연속 빈 칸 2개
                    backwardBlank++;
                    break;
                }

                if (backwardBlank == 1) //첫 빈 칸일 경우, 1회 차감
                {
                    backwardBlank--;
                }
                else //연속 두 번 빈 칸은 아니지만, 빈 칸 2개인 열린 삼은 없다.
                {
                    break;
                }
            }
        }

        int totalCount = forwardCount + backwardCount + 1; //앞, 뒤 포함 총 돌 개수

        if (totalCount != 3) //3개가 딱 맞춰져야 함. 3-4의 예외 처리도 포함된다.
        {
            return false;
        }

        //양 쪽 끝
        Vector2Int forwardEnd = new Vector2Int(inRow, inCol) + new Vector2Int(rdir, cdir) * (forwardCount + forwardBlank);
        Vector2Int backwardEnd = new Vector2Int(inRow, inCol) - new Vector2Int(rdir, cdir) * (backwardCount + backwardBlank);

        if (!IsInRange(forwardEnd[0] + rdir, forwardEnd[1] + cdir, BOARD_SIZE) || !IsInRange(backwardEnd[0] - rdir, backwardEnd[1] - cdir, BOARD_SIZE))
        {
            return false;
        }

        else
        {
            if (board[forwardEnd[0] + rdir, forwardEnd[1] + cdir] == otherType || board[backwardEnd[0] - rdir, backwardEnd[1] - cdir] == otherType) //양 쪽 끝 중 하나라도 다른 돌로 막혀있다면 열린 삼이 아니다. 
            {
                return false;
            }
            else
                return true; //빈 칸이거나 자기 자신이면 열린 삼
        }
    }

    //4-4
    private static bool IsDoubleFour(Constants.PlayerType[,] board, PlayerType playerType, int inRow, int inCol)
    {
        int FourChecked = 0;    //열린 삼 개수

        for (int i = 0; i < 4; i++) //방향 탐색 위한 반복문
        {
            if (IsFour(board, playerType, inRow, inCol, i))
            {
                FourChecked++;
            }
        }
        return true;
    }
    //4
    private static bool IsFour(Constants.PlayerType[,] board, PlayerType playerType, int inRow, int inCol, int dir)
    {
        PlayerType otherType = playerType == PlayerType.Black ? PlayerType.White : PlayerType.Black;

        int curRow = inRow;
        int curCol = inCol;

        int rdir = directions[dir][0];
        int cdir = directions[dir][1];

        int forwardBlank = 1;
        int backwardBlank = 0;

        int forwardCount = 0;
        int backwardCount = 0;

        bool isForwardSecondContinuous = false; //두 번 연속 빈 칸 방문 시 걸러내는 부울 변수
        bool doesBlankExist = false;

        //정방향
        while (true)
        {
            curRow += rdir;
            curCol += cdir;

            if (!IsInRange(curRow, curCol, BOARD_SIZE)) //범위 제한
            {
                break;
            }

            if (board[curRow, curCol] != PlayerType.None) //빈 칸이 아닐 경우
            {
                if (board[curRow, curCol] == playerType) //현재 돌이면
                {
                    isForwardSecondContinuous = false;
                    forwardCount++;
                }

                else//상대 돌이면
                {
                    break;
                }

            }
            else //빈 칸일 경우
            {
                if (!isForwardSecondContinuous) //첫 빈 칸일 경우
                {
                    isForwardSecondContinuous = true;
                }
                else //두 번 연속 빈 칸일 경우
                {
                    //돌 사이에 빈 칸이 없었던 경우로 판단하기 위해 1로 복구
                    forwardBlank++;
                    break;
                }

                if (forwardBlank == 1) //첫 빈 칸일 경우, 차감
                {
                    forwardBlank--;
                }
                else //연속아닌 빈 칸 2개도 되면 안 된다.
                {
                    break;
                }
            }


            //초기화
            bool isBackwardSecondContinuous = false;
            curRow = inRow;
            curCol = inCol;

            backwardBlank = forwardBlank;

            //역방향
            while (true)
            {
                curRow -= rdir;
                curCol -= cdir;

                if (!IsInRange(curRow, curCol, BOARD_SIZE)) //범위 제한
                {
                    break;
                }

                if (board[curRow, curCol] != PlayerType.None) //빈 칸이 아닐 경우
                {
                    if (board[curRow, curCol] == playerType) //현재 돌이면
                    {
                        isBackwardSecondContinuous = false; //연속 두 번 빈 칸이 아니므로 false로 초기화
                        backwardCount++;
                    }

                    else//상대 돌이면
                    {
                        break;
                    }

                }
                else //빈 칸일 경우
                {
                    if (!isBackwardSecondContinuous) //첫 빈 칸일 경우
                    {
                        isBackwardSecondContinuous = true;
                    }
                    else //두 번 연속 빈 칸일 경우
                    {
                        //돌 사이에 빈 칸이 없었던 경우로 판단하기 위해 1로 복구
                        backwardBlank++;
                        break;
                    }

                    if (backwardBlank == 1) //정방향에서 빈 칸이 없었던 경우
                    {
                        backwardBlank--;
                    }
                    else //정방향에서 빈 칸이 있었던 경우 || 정방향에서 빈 칸이 없었는데 연속 아닌 두 빈 칸이 나왔을 경우(isSecondContinuous= false인데 backwardBlank=0일 때)
                    {
                        break;
                    }
                }
            }
        }
        //두 곳 모두 빈 칸이 있으면 ->x

        //한 곳에 빈 칸이 있으면->
        //  ->양쪽에 흰 돌이 있을 경우-> return false;
        //  ->한 쪽이라도 비어있다면->return true;
        //4칸 연속인 경우
        //  ->양쪽에 흰 돌이 있을 경우->return false;
        //  ->한 쪽이라도 비어있다면 -> return true;

        //if(forwardBlank == 1 && backwardBlank == 1)  //사이에 빈 칸이 없고  양측 다 뚫린
        //{

        //}

        int totalCount = forwardCount + backwardCount + 1; //앞, 뒤 포함 총 돌 개수

        if (totalCount != 4) //4개가 딱 맞춰져야 함
        {
            return false;
        }
        else
            return true;
    }

    //6
    public static bool IsOneLineOverline(Constants.PlayerType[,] board, PlayerType playerType, int inRow, int inCol, int dir)
    {
        PlayerType otherType = playerType == PlayerType.Black ? PlayerType.White : PlayerType.Black;

        int curRow = inRow;
        int curCol = inCol;

        int rdir = directions[dir][0];
        int cdir = directions[dir][1];

        int forwardCount = 0;
        int backwardCount = 0;

        //정방향
        while (true)
        {
            curRow += rdir;
            curCol += cdir;

            if (!IsInRange(curRow, curCol, BOARD_SIZE)) //범위 제한
            {
                break;
            }

            if (board[curRow, curCol] != PlayerType.None) //빈 칸이 아닐 경우
            {
                if (board[curRow, curCol] == playerType) //현재 돌이면
                {
                    forwardCount++;
                }

                else//상대 돌이면
                {
                    break;
                }

            }
            else //빈 칸일 경우
            {
                break;
            }
        }

        //초기화
        curRow = inRow;
        curCol = inCol;

        //역방향
        while (true)
        {
            curRow -= rdir;
            curCol -= cdir;

            if (!IsInRange(curRow, curCol, BOARD_SIZE)) //범위 제한
            {
                break;
            }

            if (board[curRow, curCol] != PlayerType.None) //빈 칸이 아닐 경우
            {
                if (board[curRow, curCol] == playerType) //현재 돌이면
                {
                    backwardCount++;
                }

                else//상대 돌이면
                {
                    break;
                }

            }
            else //빈 칸일 경우
            {
                break;
            }
        }
        int totalCount = forwardCount + backwardCount + 1;

        if (totalCount == 5) //끊김없는 5개일 때
        {
            return true; //오목
        }
        else
        {
            return false;
        }
    }

    public static bool IsOverline(Constants.PlayerType[,] board, PlayerType playerType, int inRow, int inCol)
    {
        for (int dir = 0; dir < 4; dir++)
        {
            if (IsOneLineOverline(board, playerType, inRow, inCol, dir))
            {
                return true;
            }
        }
        return false;
    }


    public static bool IsOneLineGomoku(Constants.PlayerType[,] board, PlayerType playerType, int inRow, int inCol, int dir)
    {
        PlayerType otherType = playerType == PlayerType.Black ? PlayerType.White : PlayerType.Black;

        int curRow = inRow;
        int curCol = inCol;

        int rdir = directions[dir][0];
        int cdir = directions[dir][1];

        int forwardCount = 0;
        int backwardCount = 0;

        //정방향
        while (true)
        {
            curRow += rdir;
            curCol += cdir;

            if (!IsInRange(curRow, curCol, BOARD_SIZE)) //범위 제한
            {
                break;
            }

            if (board[curRow, curCol] != PlayerType.None) //빈 칸이 아닐 경우
            {
                if (board[curRow, curCol] == playerType) //현재 돌이면
                {
                    forwardCount++;
                }

                else//상대 돌이면
                {
                    break;
                }

            }
            else //빈 칸일 경우
            {
                break;
            }
        }

        //초기화
        curRow = inRow;
        curCol = inCol;

        //역방향
        while (true)
        {
            curRow -= rdir;
            curCol -= cdir;

            if (!IsInRange(curRow, curCol, BOARD_SIZE)) //범위 제한
            {
                break;
            }

            if (board[curRow, curCol] != PlayerType.None) //빈 칸이 아닐 경우
            {
                if (board[curRow, curCol] == playerType) //현재 돌이면
                {
                    backwardCount++;
                }

                else//상대 돌이면
                {
                    break;
                }

            }
            else //빈 칸일 경우
            {
                break;
            }
        }
        int totalCount = forwardCount + backwardCount + 1;

        if (totalCount == 5) //끊김없는 5개일 때
        {
            return true; //오목
        }
        else
        {
            return false;
        }
    }
    public static bool IsGomoku(Constants.PlayerType[,] board, PlayerType playerType, int inRow, int inCol)
    {
        for (int dir = 0; dir < 4; dir++)
        {
            if (IsOneLineGomoku(board, playerType, inRow, inCol, dir))
            {
                return true;
            }
        }
        return false;
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
}

