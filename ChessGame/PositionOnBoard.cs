using System;
using System.Collections.Generic;
using System.Text;

namespace ChessGame
{
    class PositionOnBoard
    {
        int _row;
        int _col;
        string _color;
        public PositionOnBoard(int row, int col)
        {
            setRow(row);
            setCol(col);
        }
        public PositionOnBoard(int row, int col, string color)
        {
            setRow(row);
            setCol(col);
            setColor(color);
        }
        public void setRow(int row)
        {
            _row = row;
        }
        public void setCol(int col)
        {
            _col = col;
        }
        public void setColor(string color)
        {
            _color = color;
        }
        public int getRow()
        {
            return _row;
        }
        public int getCol()
        {
            return _col;
        }
        public string getColor()
        {
            return _color;
        }

    }
}
