using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tetris.Lib.Shapes
{
    public class OShape : AShape
    {
        public override void InitializePrints()
        {
            Prints = new List<int[,]>{
                new int[2, 2] 
                {
                    {2, 2},
                    {2, 2}
                }
            };

            InitializeRandomPosition();
        }
    }
}
