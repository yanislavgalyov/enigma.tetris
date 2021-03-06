﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tetris.Lib.Shapes
{
    public class SShape : AShape
    {
        public override void InitializePrints()
        {
            Prints = new List<int[,]>{
                new int[2, 3] 
                {
                    {0, 2, 2},
                    {2, 2, 0}
                },
                new int[3, 2] 
                {
                    {2, 0},
                    {2, 2},
                    {0, 2}
                }
            };

            InitializeRandomPosition();
        }
    }
}
