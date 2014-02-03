using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tetris.Lib.Shapes
{
    public class IShape : AShape
    {
        public override void InitializePrints()
        {
            Prints = new List<int[,]>{
                new int[4, 1] 
                {
                    {2},
                    {2},
                    {2},
                    {2}
                }, 
                new int[1, 4] 
                {
                    {2, 2, 2 ,2}
                }
            };

            InitializeRandomPosition();
        }
    }
}
