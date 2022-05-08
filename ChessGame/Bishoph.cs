using System.Collections.Generic;

namespace ChessGame
{
    class Bishoph : ChessPieces
    {
        public Bishoph(bool isWhite)
        {
            setIsWhite(isWhite);
        }
        public override void setIsWhite(bool isWhite)
        {
            _isWhite = isWhite;
        }
        public override void copy(ChessPieces[,] chessBoardCopy, ChessPieces other, int row, int col)
        {
            chessBoardCopy[row, col] = new Bishoph(other.getIsWhite());
        }
        public override bool getIsWhite()
        {
            return _isWhite;
        }
        public override string ToString()
        {
            return (getIsWhite() ? "W" : "B") + "B";
        }
        public override bool Equals(object obj)
        {
            if (!(obj is Bishoph))
                return false;
            Bishoph other = (Bishoph)obj;
            return getIsWhite() == other.getIsWhite();
        }
        public override List<PositionOnBoard> squaresCanMoveTo(ChessPieces[,] chessBoard, int row, int col, PositionOnBoard kingPosition)
        {
            List<PositionOnBoard> listSquare = new List<PositionOnBoard>();
            LookForSquaresCanMoveTo(chessBoard, row, col, 1, 1, listSquare);
            LookForSquaresCanMoveTo(chessBoard, row, col, 1, -1, listSquare);
            LookForSquaresCanMoveTo(chessBoard, row, col, -1, 1, listSquare);
            LookForSquaresCanMoveTo(chessBoard, row, col, -1, -1, listSquare);
            return listSquare;
        }
        public override void move(ChessPieces[,] chessBoard, int currentRow, int currentCol, int newRow, int noeCol)
        {
            chessBoard[newRow, noeCol] = chessBoard[currentRow, currentCol];
            chessBoard[currentRow, currentCol] = null;
        }
    }
}
