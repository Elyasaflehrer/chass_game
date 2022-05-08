using System;
using System.Collections.Generic;
using System.Text;

namespace ChessGame
{
    class Rook : ChessPieces
    {
        bool _canCastling;
        public Rook(bool isWhite)
        {
            setIsWhite(isWhite);
            setCanCastling(true);
        }
        public Rook(bool isWhite, bool canCastling)
        {
            setIsWhite(isWhite);
            _canCastling = canCastling;
        }
        public override void setIsWhite(bool isWhite)
        {
            _isWhite = isWhite;
        }
        public override void copy(ChessPieces[,] chessBoardCopy, ChessPieces other,int row ,int col)
        {
            Rook rook = (Rook)other;
            chessBoardCopy[row, col] = new Rook(other.getIsWhite(), rook.getCanCastling());        
        }
        public void setCanCastling(bool canCastling)
        {
            _canCastling = canCastling;
        }
        public bool getCanCastling()
        {
            return _canCastling;
        }
        public override bool getIsWhite()
        {
            return _isWhite;
        }
        public override string ToString()
        {
            return (getIsWhite() ? "W" : "B") + "R";
        }
        public override bool Equals(object obj)
        {
            if (!(obj is Rook))
                return false;
            Rook other = (Rook)obj;
            return getIsWhite() == other.getIsWhite()&&getCanCastling()==other.getCanCastling();
        }
        public override List<PositionOnBoard> squaresCanMoveTo(ChessPieces[,] chessBoard, int row, int col, PositionOnBoard kingPosition)
        {
            List<PositionOnBoard> listSquare = new List<PositionOnBoard>();        
             LookForSquaresCanMoveTo(chessBoard, row, col, 1, 0,  listSquare);
             LookForSquaresCanMoveTo(chessBoard, row, col, -1, 0,  listSquare);
             LookForSquaresCanMoveTo(chessBoard, row, col, 0, 1,  listSquare);
             LookForSquaresCanMoveTo(chessBoard, row, col, 0, -1,  listSquare);            
            return listSquare;
        }
        public override void move(ChessPieces[,] chessBoard, int currentRow, int currentCol, int newRow, int newCol)
        {
            Rook rook = (Rook)chessBoard[currentRow, currentCol];
            rook.setCanCastling(false);
            chessBoard[newRow, newCol] = chessBoard[currentRow, currentCol];
            chessBoard[currentRow, currentCol] = null;
        }
    }
}
