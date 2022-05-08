using System;
using System.Collections.Generic;

namespace ChessGame
{
    class Pawn : ChessPieces
    {
        bool _isFirstStep;
        bool _canBeatThroughGear;
        public Pawn(bool isWhite)
        {
            setIsWhite(isWhite);
            _isFirstStep = true;
            _canBeatThroughGear = false;
        }
        public Pawn(bool isWhite, bool isFirstStep, bool canBeatThroughGear)
        {
            setIsWhite(isWhite);
            _isFirstStep = isFirstStep;
            _canBeatThroughGear = canBeatThroughGear;
        }
        public void setIsFirstStep(bool isFirstStep)
        {
            _isFirstStep = isFirstStep;
        }
        public void setCanBeatThroughGear(bool canBeatThroughGear)
        {
            _canBeatThroughGear = canBeatThroughGear;
        }
        public override void setIsWhite(bool isWhite)
        {
            _isWhite = isWhite;
        }
        public override void copy(ChessPieces[,] chessBoardCopy, ChessPieces other, int row, int col)
        {
            Pawn pawn = (Pawn)other;
            chessBoardCopy[row, col] = new Pawn(other.getIsWhite(), pawn.getIsFirstStep(), pawn.getCanBeatThroughGear());
        }
        public bool getIsFirstStep()
        {
            return _isFirstStep;
        }
        public bool getCanBeatThroughGear()
        {
            return _canBeatThroughGear;
        }
        public override bool getIsWhite()
        {
            return _isWhite;
        }
        public override string ToString()
        {
            return (getIsWhite() ? "W" : "B") + "P";
        }
        public override bool Equals(object obj)
        {
            if (!(obj is Pawn))
                return false;
            Pawn other = (Pawn)obj;
            return getIsWhite() == other.getIsWhite() && getIsFirstStep() == other.getIsFirstStep() && getCanBeatThroughGear() == other.getCanBeatThroughGear();
        }
        public override List<PositionOnBoard> squaresCanMoveTo(ChessPieces[,] chessBoard, int row, int col, PositionOnBoard kingPosition)
        {
            List<PositionOnBoard> listSquare = new List<PositionOnBoard>();
            int i = chessBoard[row, col].getIsWhite() ? 1 : -1;
            stepsCanGoStraight(chessBoard, row, col, listSquare);
            stepsCanGoDiagonal(chessBoard, row, col, listSquare);
            return listSquare;
        }
        public void stepsCanGoDiagonal(ChessPieces[,] chessBoard, int row, int col, List<PositionOnBoard> listSquare)
        {
            int i = chessBoard[row, col].getIsWhite() ? 1 : -1;
            for (int j = -1; j < 2; j++)
            {
                if (j == 0)
                    continue;
                if (isLegalMove(row + i, col + j) && (chessBoard[row + i, col + j] != null &&
                chessBoard[row, col].getIsWhite() != chessBoard[row + i, col + j].getIsWhite()))
                    listSquare.Add(new PositionOnBoard(row + i, col + j));
            }
            for (int j = -1; j < 2; j++)
            {
                if (j == 0)
                    continue;
                if (isLegalMove(row, col + j) && chessBoard[row, col + j] != null && chessBoard[row, col + j] is Pawn)
                {
                    Pawn pawn = (Pawn)chessBoard[row, col + j];
                    if (pawn.getCanBeatThroughGear())
                        listSquare.Add(new PositionOnBoard(row + i, col + j));
                }
            }
        }
        public void stepsCanGoStraight(ChessPieces[,] chessBoard, int row, int col, List<PositionOnBoard> listSquare)
        {
            int i = chessBoard[row, col].getIsWhite() ? 1 : -1;
            if (isLegalMove(row + i, col) && chessBoard[row + i, col] == null)
            {
                listSquare.Add(new PositionOnBoard(row + i, col));
                if (getIsFirstStep())
                {
                    int k = chessBoard[row, col].getIsWhite() ? 2 : -2;
                    if (chessBoard[row + k, col] == null)
                        listSquare.Add(new PositionOnBoard(row + k, col));
                }
            }
        }
        public override void move(ChessPieces[,] chessBoard, int currentRow, int currentCol, int newRow, int newCol)
        {
            Pawn pawn = (Pawn)this;
            pawn.setIsFirstStep(false);
            // if Beat Through Gear
            if (newCol != currentCol && chessBoard[newRow, newCol] == null)
            {
                int i = chessBoard[currentRow, currentCol].getIsWhite() ? -1 : 1;
                chessBoard[newRow, newCol] = chessBoard[currentRow, currentCol];
                chessBoard[currentRow, currentCol] = null;
                chessBoard[newRow + i, newCol] = null;
            }
            else
            {
                chessBoard[newRow, newCol] = chessBoard[currentRow, currentCol];
                chessBoard[currentRow, currentCol] = null;
                if (Math.Abs(newRow - currentRow) == 2)
                    pawn.setCanBeatThroughGear(true);               
            }
        }
        public ChessPieces SelectTool(ChessPieces[,] chessBoard, int newRow, int newCol)
        {
            ChessPieces newTool;
            Console.WriteLine("GOOD LACK!!! yoe can changh yoer PAWN to another tool ");
            Console.WriteLine("PRESS: Q for Queen, R to Rook, N to Knight, B to Bishoph, and another Key to continue without change, end then presss ENTER ");
            Console.WriteLine("There will be reference only to the first letter ");
            string input = Console.ReadLine();
            input = input.ToUpper().Trim();
            switch (input[0])
            {
                case 'Q':
                    newTool = new Queen(chessBoard[newRow, newCol].getIsWhite());
                    break;
                case 'R':
                    newTool = new Rook(chessBoard[newRow, newCol].getIsWhite());
                    break;
                case 'N':
                    newTool = new Knight(chessBoard[newRow, newCol].getIsWhite());
                    break;
                case 'B':
                    newTool = new Bishoph(chessBoard[newRow, newCol].getIsWhite());
                    break;
                default:
                    newTool = chessBoard[newRow, newCol];
                    break;
            }
            return newTool;
        }
    }
}
