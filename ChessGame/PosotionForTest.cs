using System;
using System.Collections.Generic;
using System.Text;

namespace ChessGame
{
    class PosotionForTest
    {
    }
}
/**
 
 * 
 * 
 * Position for SelectTool
 * = {{null            ,null             , null              ,null          ,new King(true)  ,null                ,null             ,null          },
                                     { null           ,null             , new Pawn(false,false,false)    ,null            ,null            ,null               ,null             ,null           },
                                     { null           ,null             , null              ,null            ,null            ,null               ,null             ,null           },
                                     { null           ,null             , null              ,null            ,null            ,null               ,null             ,null           },
                                     { null           ,null             , null              ,null            , null           , null              ,null             ,null           },
                                     { null           ,null             , null              ,null            ,null            ,null               ,null            ,new Knight(true) },
                                     { null           ,null             , new Pawn(true,false,false)            ,null            ,null            ,null                ,null             ,null           },
                                     { null           , new Rook(false), null              ,null            ,new King(false) ,null              ,new Queen(false)  ,null           }};
 * 
 * 
 * Position for isDrawNoLegalMove()
 * {{null            ,null             , null              ,new Queen(true) ,new King(true)  ,null               ,null             ,null          },
                                     { null           ,null             , null              ,null            ,null            ,null               ,null             ,null           },
                                     { null           ,null             , null              ,null            ,null            ,null               ,null             ,null           },
                                     { null           ,null             , null              ,null            ,null            ,null               ,null             ,null           },
                                     { null           ,null             , null              ,null            ,new King(true)  ,new Queen(true)   ,null             ,null           },
                                     { null           ,null             , null              ,null            ,null            ,null              ,null            ,new Knight(true) },
                                     { null           ,null             , null              ,null            ,null            ,null               ,null             ,null           },
                                     { null           ,new Bishoph(true), null              ,null            ,new King(false) ,null               ,null             ,null           }};
 * 
 * Position for isDrawNotEnoughTools
 * {{null            ,null             , null              ,null          ,new King(true)  ,null                ,null             ,null          },
                                     { null           ,null             , null              ,null            ,null            ,null               ,null             ,null           },
                                     { null           ,null             , null              ,null            ,null            ,null               ,null             ,null           },
                                     { null           ,null             , null              ,null            ,null            ,null               ,null             ,null           },
                                     { null           ,null             , null              ,null            , null           , null              ,null             ,null           },
                                     { null           ,null             , null              ,null            ,null            ,null               ,null            ,new Knight(true) },
                                     { null           ,null             , null              ,null            ,null            ,null                ,null             ,null           },
                                     { null           , new Rook(false), null              ,null            ,new King(false) ,null              ,new Queen(false)  ,null           }};
 * 
 * 
 * 
 * regular _chessBoard
 * {{ new Rook(true),new Knight(true),new Bishoph(true) ,new Queen(true),new King(true),new Bishoph(true), new Knight(true),new Rook(true)},
                                     { new Pawn(true), new Pawn(true),new Pawn(true)     ,new Pawn(true) ,new Pawn(true),new Pawn(true)   ,new Pawn(true)   ,new Pawn(true)        },
                                     { null           ,null             , null              ,null            ,null           ,null              ,null              ,null           },
                                     { null           ,null             , null              ,null            ,null           ,null              ,null              ,null           },
                                     { null           ,null             , null              ,null            ,null           ,null              ,null              ,null           },
                                     { null           ,null             , null              ,null            ,null           ,null              ,null              ,null           },
                                     { new Pawn(false), new Pawn(false),new Pawn(false)     ,new Pawn(false) ,new Pawn(false),new Pawn(false)   ,new Pawn(false)  ,new Pawn(false) },
                                     { new Rook(false),new Knight(false),new Bishoph(false) ,new Queen(false),new King(false),new Bishoph(false) ,new Knight(false),new Rook(false)} };
 * 
/**  public bool isToolSpoon(int row, int col, PositionOnBoard kingPosition)// passed a second time
          {
              bool isSpoon = false;
              if (col == kingPosition.getCol())
              {
                  if (kingPosition.getRow() > row)
                      isSpoon = isToolSpoon(row, col, -1, 0);
                  else if (kingPosition.getRow() < row)
                      isSpoon = isToolSpoon(row, col, 1, 0);
              }
              else if (row == kingPosition.getRow())
              {
                  if (kingPosition.getCol() > col)
                      isSpoon = isToolSpoon(row, col, 0, -1);
                  else if (kingPosition.getCol() < col)
                      isSpoon = isToolSpoon(row, col, 0, 1);
              }
              else if (Math.Abs(row - kingPosition.getRow()) == Math.Abs(col - kingPosition.getCol()))
              {
                  if (kingPosition.getRow() > row && kingPosition.getCol() > col)
                      isSpoon = isToolSpoon(row, col, -1, -1);
                  else if (kingPosition.getRow() < row && kingPosition.getCol() > col)
                      isSpoon = isToolSpoon(row, col, 1, -1);
                  else if (kingPosition.getRow() > row && kingPosition.getCol() < col)
                      isSpoon = isToolSpoon(row, col, -1, 1);
                  else if (kingPosition.getRow() < row && kingPosition.getCol() < col)
                      isSpoon = isToolSpoon(row, col, 1, 1);
              }
              return isSpoon;
          }// finish isToolSpoon
          public bool isToolSpoon(int row, int col, int highOrDown, int leftOrRight)// passed a second time
          {
              int i = row + highOrDown, j = col + leftOrRight;
              while (isLegalMove(i, j) && _chessBoard[i, j] == null)
              {
                  i = i + highOrDown;
                  j = j + leftOrRight;
              }
              if (isLegalMove(i, j))
              {
                  if (_chessBoard[i, j] is Queen && _chessBoard[row, col].getIsWhite() != _chessBoard[i, j].getIsWhite())
                      return true;
                  else if (leftOrRight == 0 || highOrDown == 0)
                      if (_chessBoard[i, j] is Rook && _chessBoard[row, col].getIsWhite() != _chessBoard[i, j].getIsWhite())
                          return true;
                      else if (leftOrRight != 0 && highOrDown != 0)
                          if (_chessBoard[i, j] is Bishoph && _chessBoard[row, col].getIsWhite() != _chessBoard[i, j].getIsWhite())
                              return true;
              }
              return false;
          }// finish isToolSpoon overloading  
// Method for texts
public void print(ChessPieces[,] chessBoard)// passed a second time
{
    string abc = "ABCDEFGH";
    Console.WriteLine();
    for (int i = chessBoard.GetLength(0) - 1; i >= 0; i--)
    {
        Console.Write(i + 1 + "  ");
        for (int j = 0; j < chessBoard.GetLength(1); j++)
            Console.Write(chessBoard[i, j] == null ? "EE  " : (chessBoard[i, j] + "  "));
        Console.WriteLine();
    }
    Console.Write("   ");
    for (int i = 0; i < 8; i++)
    {
        Console.Write(abc[i] + "   ");
    }
    Console.WriteLine();
}// psssed a secound time*/