using System;
using System.Collections.Generic;


namespace ChessGame
{
    class Chess
    {
        ChessPieces[,] _chessBoard ={{ new Rook(true),new Knight(true),new Bishoph(true) ,new Queen(true),new King(true),new Bishoph(true), new Knight(true),new Rook(true)},
                                     { new Pawn(true), new Pawn(true),new Pawn(true)     ,new Pawn(true) ,new Pawn(true),new Pawn(true)   ,new Pawn(true)   ,new Pawn(true)        },
                                     { null           ,null             , null              ,null            ,null           ,null              ,null              ,null           },
                                     { null           ,null             , null              ,null            ,null           ,null              ,null              ,null           },
                                     { null           ,null             , null              ,null            ,null           ,null              ,null              ,null           },
                                     { null           ,null             , null              ,null            ,null           ,null              ,null              ,null           },
                                     { new Pawn(false), new Pawn(false),new Pawn(false)     ,new Pawn(false) ,new Pawn(false),new Pawn(false)   ,new Pawn(false)  ,new Pawn(false) },
                                     { new Rook(false),new Knight(false),new Bishoph(false) ,new Queen(false),new King(false),new Bishoph(false) ,new Knight(false),new Rook(false)} };
        ChessPieces[,] _lastChessBoard;
        PositionOnBoard _positionKingWight;
        PositionOnBoard _positionKingBlack;
        PositionOnBoard _lastStepOfWhite;
        PositionOnBoard _lastStepOfBlack;
        List<PositionOnBoard> _listSquares;
        KeepSituetion[] _keepChessBoard = new KeepSituetion[50];
        bool _isTurnWhite;
        int _numberOfTrips = 0;
        public Chess()
        {
            _positionKingWight = new PositionOnBoard(0, 4);
            _positionKingBlack = new PositionOnBoard(7, 4);
            _lastStepOfWhite = new PositionOnBoard(0, 0);
            _lastStepOfBlack = new PositionOnBoard(0, 0);
            _numberOfTrips = 0;
        }
        public void play()
        {
            bool endWithMate = false;
            bool endWithDraw = false;
            PositionOnBoard kingPosition = getPositionKingWight();
            PositionOnBoard kingPositionEnemy = getPositionKingBlack();
            while (!endWithMate && !endWithDraw)
            {
                Console.Clear();
                print();
                _isTurnWhite = !_isTurnWhite;
                _listSquares = new List<PositionOnBoard>();
                ChessPieces[,] lastChessBoard = this._chessBoard;
                _lastChessBoard = copyChessBoard();
                Console.WriteLine("The turn of  {0} ", _isTurnWhite ? "White" : "Black");
                PositionOnBoard toolLocaton = chooseTool(_isTurnWhite);
                PositionOnBoard newLocation = chooseNewLocation(_chessBoard, _listSquares);
                bool isEat = isEatineg(newLocation);
                _chessBoard[toolLocaton.getRow(), toolLocaton.getCol()].move(_chessBoard, toolLocaton.getRow(), toolLocaton.getCol(), newLocation.getRow(), newLocation.getCol());
                updateNumberTrips(isEat, newLocation, _isTurnWhite);
                updateGame(newLocation);
                _lastChessBoard = copyChessBoard();
                endWithMate = isMate();
                if (endWithMate)
                    continue;
                endWithDraw = isDraw();
                if (endWithDraw)
                    continue;
            }
            Console.Clear();
            print();
            if (endWithMate)
                Console.WriteLine("is Mate, {0} won", _isTurnWhite ? "White" : "Black");
            if (endWithDraw && !endWithMate)
                Console.WriteLine("is Draw ");
        }
        private void updateGame(PositionOnBoard newLocation)
        {
            if (_chessBoard[newLocation.getRow(), newLocation.getCol()] is Pawn && (newLocation.getRow() == 0 || newLocation.getRow() == _chessBoard.GetLength(0) - 1))
            {
                Pawn pawn = (Pawn)_chessBoard[newLocation.getRow(), newLocation.getCol()];
                _chessBoard[newLocation.getRow(), newLocation.getCol()] = pawn.SelectTool(_chessBoard, newLocation.getRow(), newLocation.getCol());
            }
            if (_chessBoard[newLocation.getRow(), newLocation.getCol()] is King)
                updateKing(newLocation);
            updateLastStep(newLocation);
            updateCanBeatThroughGear(newLocation);
        }
        private void updateCanBeatThroughGear(PositionOnBoard newLocation)
        {
            PositionOnBoard laststep = _isTurnWhite ? _lastStepOfBlack : _lastStepOfWhite;
            if (_chessBoard[newLocation.getRow(), newLocation.getCol()] != null && _chessBoard[laststep.getRow(), laststep.getCol()] is Pawn)
            {
                Pawn pawn = (Pawn)_chessBoard[laststep.getRow(), laststep.getCol()];
                pawn.setCanBeatThroughGear(false);
            }
        }
        private void updateLastStep(PositionOnBoard newLocation)
        {
            PositionOnBoard laststep = _isTurnWhite ? _lastStepOfWhite : _lastStepOfBlack;
            if (_chessBoard[newLocation.getRow(), newLocation.getCol()] != null && _chessBoard[newLocation.getRow(), newLocation.getCol()] is Pawn)
            {
                laststep.setRow(newLocation.getRow());
                laststep.setCol(newLocation.getCol());
            }
        }
        private void updateKing(PositionOnBoard newLocation)
        {
            King king = (King)_chessBoard[newLocation.getRow(), newLocation.getCol()];
            PositionOnBoard kingPosition = _isTurnWhite ? _positionKingWight : _positionKingBlack;
            kingPosition.setRow(newLocation.getRow());
            kingPosition.setCol(newLocation.getCol());
            king.setCanCastling(false);
        }
        private bool isMate()
        {

            PositionOnBoard kingPostionEnemy = _isTurnWhite ? _positionKingBlack : _positionKingWight;
            PositionOnBoard kingPostion = _isTurnWhite ? _positionKingWight : _positionKingBlack;
            bool mate = true;
            if (!isSquerThreatened(kingPostionEnemy.getRow(), kingPostionEnemy.getCol(), !_isTurnWhite))
                return false;
            for (int i = 0; i < 8 && mate; i++)
            {
                for (int j = 0; j < 8 && mate; j++)
                {
                    if (_chessBoard[i, j] == null)
                        continue;
                    if (_chessBoard[i, j].getIsWhite() == !_isTurnWhite)
                    {
                        _listSquares = _chessBoard[i, j].squaresCanMoveTo(_chessBoard, i, j, kingPostionEnemy);
                        if (_listSquares.Count != 0)
                        {
                            removeIllgaleSquares(i, j, !_isTurnWhite);
                            if (_chessBoard[i, j] is King && _listSquares.Count != 0)// check if king can move
                                return false;
                            for (int index = 0; index < _listSquares.Count; index++)
                            {
                                _chessBoard[i, j].move(_chessBoard, i, j, _listSquares[index].getRow(), _listSquares[index].getCol());
                                if (!isSquerThreatened(kingPostionEnemy.getRow(), kingPostionEnemy.getCol(), !_isTurnWhite))
                                    mate = false;
                                undou();
                            }
                        }
                    }
                }
            }
            return mate;
        }
        private bool isDraw()
        {
            if (_numberOfTrips >= 50 || CheckIfSituationRepeatedMoreThanThreeTimes())
                return true;
            if (isDrawNotEnoughTools() || isDrawNoLegalMove())
                return true;
            return false;
        }
        private bool isDrawNotEnoughTools()
        {

            int numberOfTools = 0;
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (_chessBoard[i, j] == null || _chessBoard[i, j] is King)
                        continue;
                    else
                        numberOfTools++;
                    if ((_chessBoard[i, j] is Rook || _chessBoard[i, j] is Queen || _chessBoard[i, j] is Pawn) || numberOfTools >= 2)
                        return false;
                }
            }
            return true;
        }
        private bool isDrawNoLegalMove()
        {
            bool isWhiteCanMove = false;
            bool isBlackCamMove = false;
            bool isTurn = _isTurnWhite ? true : false;
            List<PositionOnBoard> LookForlistSquares = new List<PositionOnBoard>();
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (_chessBoard[i, j] == null)
                        continue;
                    _listSquares = new List<PositionOnBoard>();
                    if (_chessBoard[i, j].getIsWhite() && !isWhiteCanMove)
                    {
                        _listSquares = _chessBoard[i, j].squaresCanMoveTo(_chessBoard, i, j, _positionKingWight);
                        removeIllgaleSquares(i, j, true);
                        if (_listSquares.Count != 0)
                            isWhiteCanMove = true;
                    }
                    if (!(_chessBoard[i, j].getIsWhite()) && !isBlackCamMove)
                    {
                        _listSquares = _chessBoard[i, j].squaresCanMoveTo(_chessBoard, i, j, _positionKingBlack);
                        removeIllgaleSquares(i, j, false);

                        if (_listSquares.Count != 0)
                            isBlackCamMove = true;
                    }
                    if (isWhiteCanMove && isBlackCamMove)
                        break;
                }
            }
            return !isBlackCamMove || !isWhiteCanMove;
        }
        private bool CheckIfSituationRepeatedMoreThanThreeTimes()// passed  test
        {
            int count = 0;
            if (this._numberOfTrips < 2)
                return false;
            KeepSituetion last = new KeepSituetion(_keepChessBoard[_numberOfTrips - 1].getChessBoard(), _keepChessBoard[_numberOfTrips - 1].getIsTurnWhite());
            for (int i = 0; i < _numberOfTrips; i++)
            {
                if (isBoardEquals(_keepChessBoard[i].getChessBoard(), last.getChessBoard()) && _keepChessBoard[i].getIsTurnWhite() == last.getIsTurnWhite())
                    count++;
            }
            if (count > 2)
                return true;
            return false;
        }
        //In the method  isBoardEquals  should be send as a chessBoard parameter
        private bool isBoardEquals(ChessPieces[,] chessBoard, ChessPieces[,] last)//
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if ((chessBoard[i, j] == null && last[i, j] != null) || (chessBoard[i, j] != null && last[i, j] == null))
                        return false;
                    else if (chessBoard[i, j] == null && last[i, j] == null)
                        continue;
                    else if (!(last[i, j].Equals(chessBoard[i, j])))
                        return false;
                }
            }
            return true;
        }// psssed a secound time
        private bool isEatineg(PositionOnBoard newLocation)
        {
            return _chessBoard[newLocation.getRow(), newLocation.getCol()] != null;
        }
        private void updateNumberTrips(bool isEat, PositionOnBoard newLocation, bool isTurnWhite)
        {
            if (isEat || _chessBoard[newLocation.getRow(), newLocation.getCol()] is Pawn/*is move with pawn */)// 
            {
                _numberOfTrips = 0;
                _keepChessBoard = new KeepSituetion[50];
            }
            else
            {
                _keepChessBoard[_numberOfTrips] = new KeepSituetion(copyChessBoard(), isTurnWhite);
                _numberOfTrips++;
            }
        }
        private PositionOnBoard chooseTool(bool isTurnWhite)
        {
            PositionOnBoard kingPosition = _positionKingWight;
            kingPosition = isTurnWhite ? _positionKingWight : _positionKingBlack;
            int chooseCol = -1;
            int chooseRow = -1;
            bool legalTool = false;
            while (!legalTool)
            {
                chooseCol = isColLegalInput();
                chooseRow = isRowLegalInput();
                if (_chessBoard[chooseRow, chooseCol] != null && _chessBoard[chooseRow, chooseCol].getIsWhite() == isTurnWhite)
                {
                    _listSquares = _chessBoard[chooseRow, chooseCol].squaresCanMoveTo(_chessBoard, chooseRow, chooseCol, kingPosition);
                    removeIllgaleSquares(chooseRow, chooseCol, isTurnWhite);
                    if (_listSquares.Count != 0)
                        legalTool = true;
                    if (!legalTool)
                        Console.WriteLine("Can not move whith {0}, Please select another tool ", _chessBoard[chooseRow, chooseCol]);
                }
                if (!legalTool)
                    illigaleTool(isTurnWhite, kingPosition, chooseCol, chooseRow);
            }
            Console.WriteLine("Your choice is {0} ", _chessBoard[chooseRow, chooseCol]);
            return new PositionOnBoard(chooseRow, chooseCol);
        }
        private void removeIllgaleSquares(int currentRow, int currentCol, bool isTurnWhite)
        {
            if (_chessBoard[currentRow, currentCol] is King)
                removeIllgaleSquaresForKing(currentRow, currentCol, isTurnWhite);
            else
            {
                bool remove = false;
                int i = 0;
                PositionOnBoard kingPosition = isTurnWhite ? getPositionKingWight() : getPositionKingBlack();
                while (i < _listSquares.Count && _listSquares.Count != 0)
                {
                    remove = false;
                    _chessBoard[currentRow, currentCol].move(_chessBoard, currentRow, currentCol, _listSquares[i].getRow(), _listSquares[i].getCol());
                    if (isSquerThreatened(kingPosition.getRow(), kingPosition.getCol(), isTurnWhite))
                    {
                        _listSquares.RemoveAt(i);
                        remove = true;
                    }
                    undou();
                    if (!remove)
                        i++;
                }
            }
        }
        private void removeIllgaleSquaresForKing(int currentRow, int currentCol, bool isTurnWhite)
        {
            int i = 0;
            bool remove = false;
            while (i < _listSquares.Count && _listSquares.Count != 0)
            {
                remove = false;
                if (Math.Abs(currentCol - _listSquares[i].getCol()) == 1 || currentCol - _listSquares[i].getCol() == 0)
                {
                    _chessBoard[currentRow, currentCol].move(_chessBoard, currentRow, currentCol, _listSquares[i].getRow(), _listSquares[i].getCol());
                    if (isSquerThreatened(_listSquares[i].getRow(), _listSquares[i].getCol(), isTurnWhite))//|| isThreatenedByKing(_listSquares[i].getRow(), _listSquares[i].getCol(), isTurnWhite))
                    {
                        _listSquares.RemoveAt(i);
                        remove = true;
                    }
                    undou();
                }
                else
                {
                    if (_listSquares[i].getCol() > 4)
                    {
                        for (int col = 4; col < 7; col++)
                        {
                            if (isSquerThreatened(_listSquares[i].getRow(), col, _isTurnWhite))
                            {
                                _listSquares.RemoveAt(i);
                                remove = true;
                                break;
                            }
                        }
                    }
                    else if (_listSquares[i].getCol() < 4)
                    {
                        for (int col = 1; col < 5; col++)
                        {
                            if (isSquerThreatened(_listSquares[i].getRow(), col, _isTurnWhite))
                            {
                                _listSquares.RemoveAt(i);
                                remove = true;
                                break;
                            }
                        }
                    }
                }
                if (!remove)
                    i++;
            }
        }
        private void illigaleTool(bool isTurnWhite, PositionOnBoard kingPosition, int chooseCol, int chooseRow)
        {
            if (_chessBoard[chooseRow, chooseCol] == null)
                Console.WriteLine("Square is null");
            else if (_chessBoard[chooseRow, chooseCol].getIsWhite() != isTurnWhite)
                Console.WriteLine("It's not {0} tool ", _isTurnWhite ? "White" : "Black");
            //else if (!isToolSpoon(chooseRow, chooseCol, kingPosition))
            //  Console.WriteLine("The tool is spoon");
        }
        private PositionOnBoard chooseNewLocation(ChessPieces[,] chessBoard, List<PositionOnBoard> listSquare)
        {
            int newCol = 0;
            int newRow = 0;
            bool isCanMoveAway = false;
            while (!isCanMoveAway)
            {
                newCol = isColLegalInput();
                newRow = isRowLegalInput();
                isCanMoveAway = isLegalSqauer(listSquare, newRow, newCol);
                if (!isCanMoveAway)
                    Console.WriteLine("You can not move to this square");
            }
            return new PositionOnBoard(newRow, newCol);
        }
        private bool isSquerThreatened(int row, int col, bool isWhite)
        {
            if (isThreatenedByPawn(row, col, isWhite))
                return true;
            if (isThreatenedByKnight(row, col, isWhite))
                return true;
            if (isThreatenedByQueenOrRook(row, col, isWhite))
                return true;
            if (isThreatenedByQueenOrBishoph(row, col, isWhite))
                return true;
            if (isThreatenedByKing(row, col, isWhite))
                return true;
            return false;
        }
        private bool isThreatenedByQueenOrBishoph(int row, int col, bool isWhite)
        {

            return isThreatened(row, col, 1, 1, isWhite) ||
                   isThreatened(row, col, 1, -1, isWhite) ||
                   isThreatened(row, col, -1, 1, isWhite) ||
                   isThreatened(row, col, -1, -1, isWhite);
        }
        private bool isThreatenedByQueenOrRook(int row, int col, bool isWhite)
        {
            return isThreatened(row, col, 1, 0, isWhite) ||
                   isThreatened(row, col, -1, 0, isWhite) ||
                   isThreatened(row, col, 0, 1, isWhite) ||
                   isThreatened(row, col, 0, -1, isWhite);
        }
        private bool isThreatenedByKnight(int row, int col, bool isWhite)
        {
            int i, j;
            for (i = -2; i < 3; i++)
            {
                for (j = -2; j < 3; j++)
                {
                    if (isLegalMove(row + i, col + j))
                    {
                        if (i == j || i == -j || i == 0 || j == 0)
                            continue;
                        if (_chessBoard[row + i, col + j] is Knight && isWhite != _chessBoard[row + i, col + j].getIsWhite())
                            return true;
                    }
                }
            }
            return false;
        }
        private bool isThreatenedByPawn(int row, int col, bool isWhite)
        {
            int i = 0, j = 0;
            i = isWhite ? 1 : -1;
            for (j = -1; j < 2; j++)
            {
                if (j == 0)
                    continue;
                if (isLegalMove(row + i, col + j) && (_chessBoard[row + i, col + j] is Pawn && _chessBoard[row + i, col + j].getIsWhite() != isWhite))
                    return true;
            }
            return false;
        }
        private bool isThreatenedByKing(int row, int col, bool isWhite)
        {
            for (int i = -1; i < 2; i++)
            {
                for (int j = -1; j < 2; j++)
                {
                    if (isLegalMove(row + i, col + j))
                    {
                        if (_chessBoard[row + i, col + j] != null && _chessBoard[row + i, col + j] is King && _chessBoard[row + i, col + j].getIsWhite() != isWhite)
                            return true;
                    }
                }
            }
            return false;
        }
        private bool isThreatened(int row, int col, int oneSide, int anotherSide, bool isWhite)
        {
            PositionOnBoard kingPosition = isWhite == true ? getPositionKingBlack() : getPositionKingWight();

            int i = row + oneSide, j = col + anotherSide;
            while (isLegalMove(i, j) && _chessBoard[i, j] == null)
            {
                i = i + oneSide;
                j = j + anotherSide;
            }
            if (isLegalMove(i, j))
            {
                if (oneSide == 0 || anotherSide == 0)
                {
                    if ((_chessBoard[i, j] is Queen && _chessBoard[i, j].getIsWhite() != isWhite)
                        || (_chessBoard[i, j] is Rook && _chessBoard[i, j].getIsWhite() != isWhite))
                        return true;
                }
                if (oneSide != 0 && anotherSide != 0)
                {
                    if ((_chessBoard[i, j] is Queen && _chessBoard[i, j].getIsWhite() != isWhite)
                        || (_chessBoard[i, j] is Bishoph && _chessBoard[i, j].getIsWhite() != isWhite))
                        return true;
                }
            }
            return false;
        }
        private static int isColLegalInput()
        {
            int col = 0;
            Console.WriteLine("Please choose col, end press ENTER  ");
            string input;
            bool legal = false;
            while (!legal)
            {
                input = Console.ReadLine();
                input = input.Trim().ToUpper();
                switch (input)
                {
                    case "A":
                        col = 0;
                        legal = true;
                        break;
                    case "B":
                        col = 1;
                        legal = true;
                        break;
                    case "C":
                        col = 2;
                        legal = true;
                        break;
                    case "D":
                        col = 3;
                        legal = true;
                        break;
                    case "E":
                        col = 4;
                        legal = true;
                        break;
                    case "F":
                        col = 5;
                        legal = true;
                        break;
                    case "G":
                        col = 6;
                        legal = true;
                        break;
                    case "H":
                        col = 7;
                        legal = true;
                        break;
                    default:
                        legal = false;
                        Console.WriteLine("it's unvalid ");
                        Console.WriteLine("Please choose col, end press ENTER  ");
                        break;
                }
            }
            return col;
        }//end isColLegalInput 
        private static int isRowLegalInput()
        {
            int row = 0;
            Console.WriteLine("Please choose row, end then press ENTER  ");
            string input;
            bool legal = false;
            while (!legal)
            {
                input = Console.ReadLine();
                input = input.Trim();
                switch (input)
                {
                    case "1":
                    case "2":
                    case "3":
                    case "4":
                    case "5":
                    case "6":
                    case "7":
                    case "8":
                        row = int.Parse(input) - 1;
                        legal = true;
                        break;
                    default:
                        legal = false;
                        Console.WriteLine("it's unvalid ");
                        Console.WriteLine("Please choose row, end then press ENTER  ");
                        break;
                }
            }
            return row;
        }//end isColLegalInput
        private bool isLegalSqauer(List<PositionOnBoard> listSquare, int row, int col)//Checks whether the square the player wants to move to is in the legal squares array
        {
            bool isLegal = false;
            for (int index = 0; index < listSquare.Count && !isLegal; index++)
                if (row == listSquare[index].getRow() && col == listSquare[index].getCol())
                    isLegal = true;
            if (!isLegal)
                Console.WriteLine("it's illegal square, Please choose another square ");
            return isLegal;
        }// psssed a secound time
        private void print()// passed a second time
        {
            string abc = "ABCDEFGH";
            Console.WriteLine();
            for (int i = _chessBoard.GetLength(0) - 1; i >= 0; i--)
            {
                Console.Write(i + 1 + "  ");
                for (int j = 0; j < _chessBoard.GetLength(1); j++)
                    Console.Write(_chessBoard[i, j] == null ? "EE  " : (_chessBoard[i, j] + "  "));
                Console.WriteLine();
            }
            Console.Write("   ");
            for (int i = 0; i < 8; i++)
            {
                Console.Write(abc[i] + "   ");
            }
            Console.WriteLine();
        }
        private PositionOnBoard getPositionKingWight()
        {
            return _positionKingWight;
        }
        private PositionOnBoard getPositionKingBlack()
        {
            return _positionKingBlack;
        }
        private ChessPieces[,] getChessBoard()
        {
            return _chessBoard;
        }
        private ChessPieces[,] copyChessBoard()
        {
            ChessPieces[,] newChessBoard = new ChessPieces[8, 8];
            for (int i = 0; i < 8; i++)
                for (int j = 0; j < 8; j++)
                    if (_chessBoard[i, j] != null)
                        _chessBoard[i, j].copy(newChessBoard, _chessBoard[i, j], i, j);
            return newChessBoard;
        }
        private void undou()
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (_lastChessBoard[i, j] == null)
                    {
                        _chessBoard[i, j] = null;
                        continue;
                    }
                    _lastChessBoard[i, j].copy(_chessBoard, _lastChessBoard[i, j], i, j);
                }
            }
        }
        private static bool isLegalMove(int row, int col)
        {
            return row >= 0 && col < 8 && col >= 0 && row < 8;
        }
    }
}
