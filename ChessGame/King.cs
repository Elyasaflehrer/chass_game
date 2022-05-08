using System;
using System.Collections.Generic;
using System.Text;

namespace ChessGame
{
    class King : ChessPieces
    {
        bool _canCastling;
        public King(bool isWhite)
        {
            setIsWhite(isWhite);
            setCanCastling(true);
        }
        public King(bool isWhite , bool canCastling)
        {
            setIsWhite(isWhite);
            setCanCastling(canCastling);
        }
        public override void setIsWhite(bool isWhite)
        {
            _isWhite = isWhite;
        }
        public override void copy(ChessPieces[,] chessBoardCopy, ChessPieces other, int row, int col)
        {
            King king = (King)other;
            chessBoardCopy[row, col] = new King(other.getIsWhite(), king.getCanCastling());
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
            return (getIsWhite() ? "W" : "B") + "K";
        }
        public override bool Equals(object obj)
        {
            if (!(obj is King))
                return false;
            King other = (King)obj;
            return getIsWhite() == other.getIsWhite() && getCanCastling() == other.getCanCastling();
        }
        public override List<PositionOnBoard> squaresCanMoveTo(ChessPieces[,] chessBoard, int row, int col, PositionOnBoard kingPosition)
        {

            List<PositionOnBoard> listSquare = new List<PositionOnBoard>();          
            for (int i = -1; i < 2; i++)
            {
                for (int j = -1; j < 2; j++)
                {
                    if (i == row && j == col)
                        continue;
                    if (isLegalMove(row + i, col + j))
                    {
                        if ((chessBoard[row + i, col + j] == null || chessBoard[row + i, col + j].getIsWhite() != chessBoard[row, col].getIsWhite()))                                                  
                        {
                            listSquare.Add(new PositionOnBoard(row + i, col + j));                         
                        }
                    }
                }
            }
            // Check if king can castling 
            King king = (King)chessBoard[row, col];
            if (king.getCanCastling())
            {
                if (isCanCasling(chessBoard, row, col - 2, kingPosition))                
                    listSquare.Add(new PositionOnBoard(row , col - 2));                                 
                if (isCanCasling(chessBoard, row, col + 2, kingPosition))               
                    listSquare.Add(new PositionOnBoard(row , col + 2));                              
            }
            // finish Check if king can castling 
            return listSquare;
        }
        public override void move(ChessPieces[,] chessBoard, int currentRow, int currentCol, int newRow, int newCol)
        {
            King king = (King)chessBoard[currentRow, currentCol];
            king.setCanCastling(false);
            if (Math.Abs(currentCol - newCol) > 1)
            {
                if (newCol < 4)
                {
                    chessBoard[newRow, newCol] = chessBoard[currentRow, currentCol];
                    chessBoard[currentRow, currentCol] = null;
                    chessBoard[currentRow, 3] = chessBoard[currentRow, 0];
                    chessBoard[currentRow, 0] = null;
                    Rook rook = (Rook)chessBoard[currentRow, 3];
                    rook.setCanCastling(false);

                }
                if (newCol > 4)
                {
                    chessBoard[newRow, newCol] = chessBoard[currentRow, currentCol];
                    chessBoard[currentRow, currentCol] = null;
                    chessBoard[currentRow, 5] = chessBoard[currentRow, 7];
                    chessBoard[currentRow, 7] = null;
                    Rook rook = (Rook)chessBoard[currentRow, 5];
                    rook.setCanCastling(false);

                }

            }
            else
            {
                chessBoard[newRow, newCol] = chessBoard[currentRow, currentCol];
                chessBoard[currentRow, currentCol] = null;
            }
        }
    }
}
