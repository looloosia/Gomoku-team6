using Unity.VisualScripting;
using UnityEngine;
using static Constants;
//큐로 기보 가능할지도 (포지션, 돌 타입)

public static class GomokuLibrary
{
    private static readonly Vector2Int[] directions = { new Vector2Int(0, 1), new Vector2Int(1, 0), new Vector2Int(1, 1), new Vector2Int(1, -1) };
    public static bool CheckGameWin(Constants.PlayerType playerType, Constants.PlayerType[,] board, int inRow, int inCol)
    {
        int totalCount = 0;

        foreach (var dir in directions)
        {
            // dir은 순서대로 가로, 세로, 우상향, 우하향 대각선 검사

            int curRow = inRow + dir[0];
            int curCol = inCol + dir[1];

            //정방향
            while (IsInRange(curRow, curCol, BOARD_SIZE) && board[curRow, curCol] == playerType)
            {
                totalCount++;
                curRow += dir[0];
                curCol += dir[1];
            }

            while (IsInRange(curRow, curCol, BOARD_SIZE) && board[curRow, curCol] == playerType)
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
        if (IsDoubleThree(playerType, board, inRow, inCol))
        {
            return ForbiddenType.DoubleThree;
        }
        //TODO: 4-4
        if (IsDoubleFour(playerType, board, inRow, inCol))
        {
            return ForbiddenType.DoubleFour;
        }


        return Constants.ForbiddenType.None;
    }

    //범위함수
    public static bool IsInRange(int inRow, int inCol, int boardRange)
    {
        return inRow >= 0 && inRow < boardRange && inCol >= 0 && inCol < boardRange;
    }

    //3-3
    private static bool IsDoubleThree(PlayerType playerType, Constants.PlayerType[,] board, int inRow, int inCol)
    {
        int openThreeChecked = 0;    //열린 삼 개수

        for (int i = 0; i < 4; i++) //방향 탐색 위한 반복문
        {
            if (IsOpenThree(playerType, board, inRow, inCol, i))
            {
                openThreeChecked++;
            }
        }
        return true;
    }
    //3
    private static bool IsOpenThree(PlayerType playerType, Constants.PlayerType[,] board, int inRow, int inCol, int dir)
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
        1. 0이라면 이미 빈 칸이 하나 있어 더 이상 빈 칸이 있으면 안 된다.
        2. 1이라면,
            2-1. 빈 칸이 연속 두 번 나왔다.
            2-2. 정방향의 끝 쪽에 상대편 돌이 있다.
        한 쪽 끝의 상태를 파악할 때, 그 위치를 파악하기 위해 forwardBlank가 쓰인다.
        forwardBlank == 1인 경우, 빈 칸이 없던 경우로 판단할 것이므로, 0으로 만든다. 
         */
        backwardBlank = forwardBlank;

        forwardBlank = 0;

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
                    //돌 사이에 빈 칸이 없었던 경우로 판단하기 위해 1로 복구
                    backwardBlank = 1;
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

        if (!IsInRange(forwardEnd[0] + rdir, forwardEnd[1]+cdir, BOARD_SIZE)||!IsInRange(backwardEnd[0]-rdir, backwardEnd[1]-cdir, BOARD_SIZE))
        {
            return false;
        }

        else
        {
            if (board[forwardEnd[0]+rdir, forwardEnd[1]+cdir] == otherType || board[backwardEnd[0]-rdir, backwardEnd[1]-cdir] == otherType) //양 쪽 끝 중 하나라도 다른 돌로 막혀있다면 열린 삼이 아니다. 
            {
                return false;
            }
            else
                return true; //빈 칸이거나 자기 자신이면 열린 삼
        }
    }

    //4-4
    private static bool IsDoubleFour(PlayerType playerType, Constants.PlayerType[,] board, int inRow, int inCol)
    {
        int openThreeChecked = 0;    //열린 삼 개수

        for (int i = 0; i < 4; i++) //방향 탐색 위한 반복문
        {
            if (IsFour(playerType, board, inRow, inCol, i))
            {
                openThreeChecked++;
            }
        }
        return true;
    }
    //4
    private static bool IsFour(PlayerType playerType, Constants.PlayerType[,] board, int inRow, int inCol, int dir)
    {

        return true;
    }

    //6
    public static bool IsOverline(PlayerType playerType, Constants.PlayerType[,] board, int inRow, int inCol)
    {
        int totalCount = 0;
        foreach (var dir in directions)
        {

            int curRow = inRow + dir[0];
            int curCol = inCol + dir[1];

            //정방향
            while (IsInRange(curRow, curCol, BOARD_SIZE) && board[curRow, curCol] == playerType)
            {
                totalCount++;
                curRow += dir[0];
                curCol += dir[1];
            }

            //역방향
            while (IsInRange(curRow, curCol, BOARD_SIZE) && board[curRow, curCol] == playerType)
            {
                totalCount++;
                curRow -= dir[0];
                curCol -= dir[1];
            }

            if (totalCount >= 6)
                return true;
        }
        return false;
    }
}

