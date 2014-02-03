using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tetris.Lib.Shapes
{
    public class LShape : AShape
    {
        public override void InitializePrints()
        {
            Prints = new List<int[,]>{
                new int[2, 3] 
                {
                    {2, 2, 2},
                    {2, 0, 0}
                },
                new int[3, 2] 
                {
                    {2, 2},
                    {0, 2},
                    {0, 2}
                },
                new int[2, 3] 
                {
                    {0, 0, 2},
                    {2, 2, 2}
                },
                new int[3, 2] 
                {
                    {2, 0},
                    {2, 0},
                    {2, 2}
                }
            };

            InitializeRandomPosition();
        }
    }
}
