using System.Collections.Generic;

namespace ChessGame
{
    class ChessPieces
    {
        protected bool _isWhite;
        public virtual List<PositionOnBoard> squaresCanMoveTo(ChessPieces[,] chessBoard, int row, int col, PositionOnBoard kingPosition)
        {
            List<PositionOnBoard> listSquare = new List<PositionOnBoard>();
            return listSquare;
        }
        public virtual void setIsWhite(bool isWhite)
        {
            _isWhite = isWhite;
        }
        public virtual void copy(ChessPieces[,] chessBoardCopy, ChessPieces other, int row, int col)
        {
            return;
        }
        public virtual void move(ChessPieces[,] chessBoard, int currentRow, int currentCol, int newRow, int noeCol)
        {
            return;
        }
        public virtual bool getIsWhite()
        { return _isWhite; }
        public void LookForSquaresCanMoveTo(ChessPieces[,] chessBoard, int row, int col, int oneSide, int anotherSide, List<PositionOnBoard> LegalStep)
        {
            int i = row + oneSide, j = col + anotherSide;
            while (isLegalMove(i, j))
            {
                if ((chessBoard[i, j] != null && chessBoard[row, col].getIsWhite() == chessBoard[i, j].getIsWhite()))
                    break;
                LegalStep.Add(new PositionOnBoard(i, j));
                if (chessBoard[i, j] != null)
                    break;
                i = i + oneSide;
                j = j + anotherSide;
            }
        }
        public bool isCanCasling(ChessPieces[,] chessBoard, int newRow, int newCol, PositionOnBoard kingPosition)
        {
            bool isCanCastling = true;
            Rook rook;
            bool isWhite = chessBoard[kingPosition.getRow(), kingPosition.getCol()].getIsWhite();
            if (newCol > 4)
            {
                if (!(chessBoard[newRow, 7] is Rook))
                    return false;
                rook = (Rook)chessBoard[newRow, 7];
                if (!rook.getCanCastling())
                    return false;
                for (int index = 5; index < chessBoard.GetLength(1) - 1; index++)
                {
                    if (chessBoard[newRow, index] != null)
                        return false;
                }
            }
            else if (newCol < 4/*the player castling left */)
            {
                for (int index = 1; index < 4; index++)
                {
                    if (chessBoard[newRow, index] != null)
                        return false;
                }
                if (!(chessBoard[newRow, 0] is Rook))
                    return false;
                rook = (Rook)chessBoard[newRow, 0];
                if (!rook.getCanCastling())
                    return false;
            }
            return isCanCastling;
        }
        public bool isLegalMove(int row, int col)
        {
            return row >= 0 && col < 8 && col >= 0 && row < 8;
        }
    }
}
