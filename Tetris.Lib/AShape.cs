using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Tetris.Lib
{
    public abstract class AShape
    {
        public AShape()
        {
            InitializePrints();
        }

        public int Position { get; set; }

        public Point UpperLeft { get; set; }
        public void UpperLeftDelta(int x, int y)
        {
            UpperLeft = new Point(UpperLeft.X + x, UpperLeft.Y + y);
        }

        public int[,] GetShapeArray
        {
            get
            {
                return Prints[Position];
            }
        }

        public int[,] GetNextShapeArray
        {
            get
            {
                if (Position == Prints.Count - 1)
                    return Prints[0];
                else
                    return Prints[Position + 1];
            }
        }

        public void NextPosition()
        {
            if (Position == Prints.Count - 1)
            {
                Position = 0;
            }
            else
            {
                Position++;
            }
        }

        public void PreviousPosition()
        {
            if (Position == 0)
            {
                Position = Prints.Count - 1;
            }
            else
            {
                Position--;
            }
        }

        public List<int[,]> Prints;
        public abstract void InitializePrints();

        public void InitializeRandomPosition()
        {
            Random r = new Random();
            Position = r.Next(0, Prints.Count);
         }
    }
}
