using Unity.VisualScripting;
using UnityEngine;
using System.Collections;
using static Constants;
using System;
using UnityEngine.Rendering.Universal;
//ť�� �⺸ ���������� (������, �� Ÿ��)

public static class GomokuLibrary
{
    private static Queue forbiddenPositions;

    private static readonly Vector2Int[] directions = { new Vector2Int(0, 1), new Vector2Int(1, 0), new Vector2Int(1, 1), new Vector2Int(1, -1) };
    private static readonly Vector2Int[] doubleDirections =
        {
        new Vector2Int(0, 1), new Vector2Int(0, -1), new Vector2Int(1, 0), new Vector2Int(-1, 0),
        new Vector2Int(1, 1), new Vector2Int(-1, -1), new Vector2Int(1, -1), new Vector2Int(-1, 1)
    };

    //AI ���� ���� �� �ʿ�
    //public static bool CheckGameWin(Constants.PlayerType[,] board, Constants.PlayerType playerType, int inRow, int inCol)
    //{
        
    //}

    //�ݼ� üũ�Լ�
    public static Constants.ForbiddenType IsForbidden(Constants.PlayerType[,] board, Constants.PlayerType playerType, int inRow, int inCol, int boardRange)
    {
        //�鵹�� �ݼ�����
        if (playerType == Constants.PlayerType.White)
        {
            return Constants.ForbiddenType.None;
        }

        //TODO: 6
        if (CheckOverline(board, playerType, inRow, inCol, boardRange))
        {
            return Constants.ForbiddenType.Overline;
        }

        //�����̸� 3-3, 4-4 �� ��� ����
        if (CheckGomoku(board, playerType, inRow, inCol, boardRange))
        {
            return Constants.ForbiddenType.None;
        }
        
        //TODO: 3-3�� ��� 
        if (CheckDoubleThree(board, playerType, inRow, inCol, boardRange))
        {
            //TODO: 3-3�̶� ���� �ϼ��ϸ� None��ȯ
            return ForbiddenType.DoubleThree;
        }
        //TODO: 4-4
        if (CheckDoubleFour(board, playerType, inRow, inCol, boardRange))
        {
            return ForbiddenType.DoubleFour;
        }
        return Constants.ForbiddenType.None;
    }

    //��� ĭ�� IsDoubleThree, IsDoubleFour, IsOverline�� �ѷ�����
    public static void CheckForbiddenPostions(Constants.PlayerType[,] board, PlayerType playerType, int boardRange)
    {
        for (int r = 0; r < boardRange; r++)
        {
            for (int c = 0; c < boardRange; c++)
            {
                if (board[r, c] != PlayerType.None)//������� �ʴٸ� ���
                    continue;

                if (IsForbidden(board, playerType, r, c, boardRange) != ForbiddenType.None) //�ݼ� �ڸ���
                {
                    board[r, c] = PlayerType.Forbidden; //�� �� ������ �ݼ� ��ġ üũ
                    forbiddenPositions.Enqueue(new Vector2Int(r, c));
                }
            }
        }
    }

    public static void ClearForbiddenPositionCheck(Constants.PlayerType[,] board)// ���� �Ѿ �� �ݼ� ��ġ üũ�� �� �� ����.
    {
        while (forbiddenPositions.Count > 0)
        {
            Vector2Int position = (Vector2Int)forbiddenPositions.Dequeue();
            board[position[0], position[1]] = PlayerType.Forbidden; //forbiddenCheck�� �� ����
        }
    }
    //�����Լ�
    public static bool IsInRange(int inRow, int inCol, int boardRange)
    {
        return inRow >= 0 && inRow < boardRange && inCol >= 0 && inCol < boardRange;
    }
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    //AI����
    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public static (int, int)? GetBestMove(Constants.PlayerType[,] board, PlayerType playerType, int boardRange)
    {
        int bestScore = int.MinValue;
        int bestRow = -1, bestCol = -1;

        for (int row = 0; row < boardRange; row++)
        {
            for (int col = 0; col < boardRange; col++)
            {
                if (board[row, col] == PlayerType.None) //�� ĭ
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
        if (depth == 0 /*|| IsGameOver(board)*/ ) //���� ����: �ִ� ���� ���� Ȥ�� ���� ����
        {

        }

        PlayerType otherPlayer = playerType == PlayerType.Black ? PlayerType.White : PlayerType.Black;

        if (isMaximizing) //Maximizing�ϴ� ����
        {
            int maxScore = int.MinValue;

            for (int row = 0; row < boardRange; row++)
            {
                for (int col = 0; col < boardRange; col++)
                {
                    if (board[row, col] == PlayerType.None)
                    {
                        board[row, col] = playerType;   //TODO: �ʿ� �� ���� �����ϴ� �ڵ�� ��ü�ϱ�

                        int score = Minimax(board, playerType, depth - 1, false, alpha, beta, boardRange);

                        maxScore = Math.Max(maxScore, score); // �ִ� ��

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
    /// TODO: ���� �����Լ�
    /// 4���� ������
    /// 0,1,2,3�ؼ�
    /// ���̸� 
    /// ->3-3, 4-4 ���� �� X
    /// Hash�� ������ OPEN_THREE�� FOUR�� ������ �� �ڸ����� -���� �������
    /// 
    /// ���̸� �׳� �� ���ϱ�
    /// 
    /// </summary>
    /// 

    ///<summary>
    ///CountStones: ���������� �����ִ� �� ���� ���� �Լ�
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
    ///CheckOmok: ���� ���� bool�Լ�
    ///</summary>
    public static bool CheckGomoku(PlayerType[,] board, PlayerType playerType, int inRow, int inCol, int boardRange) //���� üũ 
    {
        //���� �� �ֺ��� 4���� ���� �ִٸ� ����
        foreach (Vector2Int dir in directions)
        {
            if (CountStones(board, playerType, inRow, inCol, boardRange, dir[0], dir[1]) + CountStones(board, playerType, inRow, inCol, boardRange, -1 * dir[0], -1 * dir[1]) == 4)
            {
                return true;
            }
        }
        return false;
    }

    public static bool CheckOverline(PlayerType[,] board, PlayerType playerType, int inRow, int inCol, int boardRange) //��� üũ 
    {
        //���� �� �ֺ��� 5���� �̻��� ���� �ִٸ� ���
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

            if (count >= 2) //3�� 2�� ���� ���
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
        //�ݴ������ �������� üũ
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
            //�� ĭ�� �� �����̱�
            int rr = reverse.Item1 - dr;
            int cc = reverse.Item2 - dc;

            //�� ĭ �� ������ �� ���� �� ������ 4-3
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

    //���߿� 4�� ������ 33�� �� �ǵ��� �ϱ�
    public static bool CheckFour(PlayerType[,] board, PlayerType playerType, int inRow, int inCol, int boardRange)//4�� �ִ��� üũ. ���߿� �и��ϱ�
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
            //�� ĭ ã������ �� �� �� ���ƺ��� �������� �ƴ��� üũ

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

    public static bool CheckDoubleFour(PlayerType[,] board, PlayerType playerType, int inRow, int inCol, int boardRange)//4-4�� �Ǵ��� üũ. ���߿� �и��ϱ�
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
            //�� ĭ ã������ �� �� �� ���ƺ��� �������� �ƴ��� üũ

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

            if (board[r, c] == PlayerType.None) //�� ĭ
            {
                return (r, c);
            }

            else if (board[r, c] == playerType) //���� ��
            {
                r += dr;
                c += dc;
                continue;
            }
            else //�ٸ� ����
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