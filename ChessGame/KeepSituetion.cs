using System;
using System.Collections.Generic;
using System.Text;

namespace ChessGame
{
    class KeepSituetion
    {

        ChessPieces[,] _chessBoard;
        bool _isTurnWhite;
        public KeepSituetion(ChessPieces[,] chessBoard, bool isTurnWhite)
        {
            _chessBoard = chessBoard;
            _isTurnWhite = isTurnWhite;



        }
        public ChessPieces[,] getChessBoard()
        {
            return _chessBoard;
        }
        public bool getIsTurnWhite()
        {
            return _isTurnWhite;
        }

    }
}
