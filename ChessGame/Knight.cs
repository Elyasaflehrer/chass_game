using System.Collections.Generic;

namespace ChessGame
{
    class Knight : ChessPieces
    {
        public Knight(bool isWhite)
        {
            setIsWhite(isWhite);
        }
        public override void setIsWhite(bool isWhite)
        {
            _isWhite = isWhite;
        }
        public override void copy(ChessPieces[,] chessBoardCopy, ChessPieces other, int row, int col)
        {
            chessBoardCopy[row, col] = new Knight(other.getIsWhite());
        }
        public override bool getIsWhite()
        {
            return _isWhite;
        }
        public override string ToString()
        {
            return (getIsWhite() ? "W" : "B") + "N";
        }
        public override bool Equals(object obj)
        {
            if (!(obj is Knight))
                return false;
            Knight other = (Knight)obj;
            return getIsWhite() == other.getIsWhite();
        }
        public override List<PositionOnBoard> squaresCanMoveTo(ChessPieces[,] chessBoard, int row, int col, PositionOnBoard kingPosition)
        {
            List<PositionOnBoard> listSquares = new List<PositionOnBoard>();
            for (int i = -2; i < 3; i++)
            {
                for (int j = -2; j < 3; j++)
                {
                    if (isLegalMove(row + i, col + j))
                    {
                        if (i == j || i == -j || i == 0 || j == 0)
                            continue;
                        if (chessBoard[row + i, col + j] == null || chessBoard[row, col].getIsWhite() != chessBoard[row + i, col + j].getIsWhite())
                            listSquares.Add(new PositionOnBoard(row + i, col + j));
                    }
                }
            }
            return listSquares;
        }
        public override void move(ChessPieces[,] chessBoard, int curreneRow, int currentCol, int newRow, int NewCol)
        {
            chessBoard[newRow, NewCol] = chessBoard[curreneRow, currentCol];
            chessBoard[curreneRow, currentCol] = null;
        }
    }
}
