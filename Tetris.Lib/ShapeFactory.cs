using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tetris.Lib.Shapes;

namespace Tetris.Lib
{
    public enum ShapeTypes
    {
        IShape = 1,
        OShape = 2,
        SShape = 3,
        ZShape = 4,
        LShape = 5,
        JShape = 6,
        TShape = 7
    }
    public class ShapeFactory
    {
        public Random rand = new Random();

        public AShape SpawnShape()
        {
            //todo add random position as well, not just shape
            int shapeType = rand.Next(1, 8);
            switch (shapeType)
            {
                case (int)ShapeTypes.IShape:
                    return new IShape();
                case (int)ShapeTypes.OShape:
                    return new OShape();
                case (int)ShapeTypes.SShape:
                    return new SShape();
                case (int)ShapeTypes.ZShape:
                    return new ZShape();
                case (int)ShapeTypes.LShape:
                    return new LShape();
                case (int)ShapeTypes.JShape:
                    return new JShape();
                case (int)ShapeTypes.TShape:
                    return new TShape();
                default:
                    throw new NotImplementedException();
            }
        }
    }
}
