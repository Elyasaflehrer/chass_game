using System;
using System.Collections.Generic;
using System.Text;

namespace ChessGame
{
    class Queen : ChessPieces
    {
        public Queen(bool isWhite)
        {
            setIsWhite(isWhite);
        }
        public override void setIsWhite(bool isWhite)
        {
            _isWhite = isWhite;
        }
        public override void copy(ChessPieces[,] chessBoardCopy, ChessPieces other, int row, int col)
        {
            chessBoardCopy[row, col] = new Queen(other.getIsWhite());
        }
        public override bool getIsWhite()
        {
            return _isWhite;
        }
        public override string ToString()
        {
            return (getIsWhite() ? "W" : "B") + "Q";
        }
        public override bool Equals(object obj)
        {
            if (!(obj is Queen))
                return false;
            Queen other = (Queen)obj;
            return getIsWhite() == other.getIsWhite();
        }
        public override List<PositionOnBoard> squaresCanMoveTo(ChessPieces[,] chessBoard, int row, int col, PositionOnBoard kingPosition)
        {
            List<PositionOnBoard> listSquares = new List<PositionOnBoard>();
            Rook rook = new Rook(chessBoard[row, col].getIsWhite());
            Bishoph bishoph = new Bishoph(chessBoard[row, col].getIsWhite());
            List<PositionOnBoard> listSquaresForRook = rook.squaresCanMoveTo(chessBoard, row, col, kingPosition);
            List<PositionOnBoard> listSquaresBishoph = bishoph.squaresCanMoveTo(chessBoard, row, col, kingPosition);
            listSquares.AddRange(listSquaresBishoph);
            listSquares.AddRange(listSquaresForRook);
            return listSquares;
        }
        public override void move(ChessPieces[,] chessBoard, int currentRow, int currentCol, int newRow, int noeCol)
        {
            chessBoard[newRow, noeCol] = chessBoard[currentRow, currentCol];
            chessBoard[currentRow, currentCol] = null;
        }
    }
}
