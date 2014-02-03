using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using System.Threading;
using Tetris.Lib.Shapes;

namespace Tetris.Lib
{
    public enum Direction
    {
        Left,
        Right,
        Down
    }

    public class Level
    {
        public int[,] Cells { get; set; }
        public int[,] NextCells { get; set; }

        public static int Rows = 20;
        public static int Cols = 10;

        public bool IsActive = true;

        private AShape Falling { get; set; }
        private static int[,] FallingArray;
        private ShapeFactory factory;

        public AShape NextFalling { get; set; }
        public static int[,] NextFallingArray;

        public float updateGameInterval { get; set; }
        public float updateGameIntervalConst { get; set; }

        public float maxUpdateGameInterval = 500.0f;
        public float minUpdateGameInterval = 50.0f;
        public float deltaUpdateGameInterval = 50.0f;

        private int _scores;

        public int Scores
        {
            get
            {
                return _scores;
            }
            set
            {
                _scores = value;
                if (_scores > 1000)
                {
                    _scores = 0;
                }
            }
        }

        public bool PlaySound { get; set; }

        public Level()
        {
            Cells = new int[Rows, Cols];

            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Cols; j++)
                {
                    Cells[i, j] = 0;
                }
            }

            NextCells = new int[4, 4];
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    Cells[i, j] = 0;
                }
            }

            updateGameInterval = 250.0f;
            updateGameIntervalConst = updateGameInterval;

            factory = new ShapeFactory();
            Scores = 0;
        }

        public void GetNext()
        {
            if (NextFalling != null)
            {
                Falling = NextFalling;
                FallingArray = NextFallingArray;
            }
            else
            {
                Falling = factory.SpawnShape();
                FallingArray = Falling.GetShapeArray;
            }

            NextFalling = factory.SpawnShape();
            NextFallingArray = NextFalling.GetShapeArray;

            for (int i = 0; i <= NextCells.GetUpperBound(0); i++)
            {
                for (int j = 0; j <= NextCells.GetUpperBound(1); j++)
                {
                    NextCells[i, j] = 0;
                }
            }

            int nextRowOffset = 0;
            int rowDif = NextCells.GetUpperBound(0) - NextFallingArray.GetUpperBound(0);
            if (rowDif >= 2)
            {
                nextRowOffset = 1;
            }

            int nextColOffset = 0;
            int colDif = NextCells.GetUpperBound(1) - NextFallingArray.GetUpperBound(1);
            if (colDif >= 2)
            {
                nextColOffset = 1;
            }

            for (int i = 0; i <= NextFallingArray.GetUpperBound(0) && i <= NextCells.GetUpperBound(0); i++)
            {
                for (int j = 0; j <= NextFallingArray.GetUpperBound(1) && j <= NextCells.GetUpperBound(1); j++)
                {
                    if (NextFallingArray[i, j] != 0)
                    {
                        NextCells[i + nextRowOffset, j + nextColOffset] = NextFallingArray[i, j];
                    }
                }
            }

            if (FallingArray.GetUpperBound(0) > Cols || FallingArray.GetUpperBound(0) > Rows
                || FallingArray.GetUpperBound(1) > Cols || FallingArray.GetUpperBound(1) > Rows)
            {
                throw new Exception();
            }

            Falling.UpperLeft = new Point(0, 2);
            ImprintAShapeOnLevel();
        }

        private float elapsedTime = 0.0f;

        public void ImprintAShapeOnLevel()
        {
            for (int i = Falling.UpperLeft.X; i <= Falling.UpperLeft.X + FallingArray.GetUpperBound(0); i++)
            {
                for (int j = Falling.UpperLeft.Y; j <= Falling.UpperLeft.Y + FallingArray.GetUpperBound(1); j++)
                {
                    int arrayRow = i - Falling.UpperLeft.X;
                    int arrayCol = j - Falling.UpperLeft.Y;

                    if (FallingArray[arrayRow, arrayCol] == 2 && Cells[i, j] == 1)
                    {
                        IsActive = false;
                    }
                    Cells[i, j] = FallingArray[arrayRow, arrayCol];
                }
            }
        }

        public void Petrify()
        {
            for (int i = Falling.UpperLeft.X; i <= Falling.UpperLeft.X + FallingArray.GetUpperBound(0); i++)
            {
                for (int j = Falling.UpperLeft.Y; j <= Falling.UpperLeft.Y + FallingArray.GetUpperBound(1); j++)
                {
                    int arrayRow = i - Falling.UpperLeft.X;
                    int arrayCol = j - Falling.UpperLeft.Y;

                    if (FallingArray[arrayRow, arrayCol] == 2)
                    {
                        Cells[i, j] = 1;
                    }
                }
            }
        }

        public bool CheckForObstacle(Direction dir)
        {
            for (int i = Falling.UpperLeft.X; i <= Falling.UpperLeft.X + FallingArray.GetUpperBound(0); i++)
            {
                for (int j = Falling.UpperLeft.Y; j <= Falling.UpperLeft.Y + FallingArray.GetUpperBound(1); j++)
                {
                    int arrayRow = i - Falling.UpperLeft.X;
                    int arrayCol = j - Falling.UpperLeft.Y;

                    if (FallingArray[arrayRow, arrayCol] == 2)
                    {
                        switch (dir)
                        {
                            case Direction.Down:
                                if (i + 1 == Rows || Cells[i + 1, j] == 1)
                                {
                                    return true;
                                }
                                break;
                            case Direction.Left:
                                if (j == 0 || Cells[i, j - 1] == 1)
                                {
                                    return true;
                                }
                                break;
                            case Direction.Right:
                                if (j + 1 == Cols || Cells[i, j + 1] == 1)
                                {
                                    return true;
                                }
                                break;
                        }
                    }
                }
            }

            return false;
        }

        public void Translate(Direction dir)
        {
            switch (dir)
            {
                case Direction.Down:
                    int tempValue = 0;
                    for (int j = Falling.UpperLeft.Y; j <= Falling.UpperLeft.Y + FallingArray.GetUpperBound(1); j++)
                    {
                        tempValue = 0;
                        for (int i = Falling.UpperLeft.X; i <= Falling.UpperLeft.X + FallingArray.GetUpperBound(0) + 1 && i < Rows; i++)
                        {
                            if (Cells[i, j] != 1)
                            {
                                int t = Cells[i, j];
                                Cells[i, j] = tempValue;
                                tempValue = t;
                            }
                        }
                    }
                    Falling.UpperLeftDelta(1, 0);
                    break;
                case Direction.Left:
                    tempValue = 0;
                    for (int i = Falling.UpperLeft.X; i <= Falling.UpperLeft.X + FallingArray.GetUpperBound(0); i++)
                    {
                        tempValue = 0;
                        for (int j = Falling.UpperLeft.Y + FallingArray.GetUpperBound(1); j >= Falling.UpperLeft.Y - 1; j--)
                        {
                            if (Cells[i, j] != 1)
                            {
                                int t = Cells[i, j];
                                Cells[i, j] = tempValue;
                                tempValue = t;
                            }
                        }
                    }
                    Falling.UpperLeftDelta(0, -1);
                    break;
                case Direction.Right:
                    tempValue = 0;
                    for (int i = Falling.UpperLeft.X; i <= Falling.UpperLeft.X + FallingArray.GetUpperBound(0); i++)
                    {
                        tempValue = 0;
                        for (int j = Falling.UpperLeft.Y; j <= Falling.UpperLeft.Y + FallingArray.GetUpperBound(1) + 1; j++)
                        {
                            if (Cells[i, j] != 1)
                            {
                                int t = Cells[i, j];
                                Cells[i, j] = tempValue;
                                tempValue = t;
                            }
                        }
                    }
                    Falling.UpperLeftDelta(0, 1);
                    break;
            }
        }

        public void Rotate()
        {
            var nextArray = Falling.GetNextShapeArray;
            Point TempUpperLeft = new Point(Falling.UpperLeft.X, Falling.UpperLeft.Y);

            if (nextArray.GetUpperBound(1) >= Cols)
            {
                Falling.PreviousPosition();
            }
            else
            {
                if (Falling.UpperLeft.Y + nextArray.GetUpperBound(1) >= Cols)
                {
                    var difference = Falling.UpperLeft.Y + nextArray.GetUpperBound(1) - (Cols - 1);
                    TempUpperLeft = new Point(Falling.UpperLeft.X, Falling.UpperLeft.Y - difference);
                }
            }

            bool rotationPossible = true;
            try // raises exception sometimes when rotating next to bottom of the level, must be investigated
            {
                for (int i = TempUpperLeft.X; i <= TempUpperLeft.X + nextArray.GetUpperBound(0); i++)
                {
                    for (int j = TempUpperLeft.Y; j <= TempUpperLeft.Y + nextArray.GetUpperBound(1); j++)
                    {
                        int arrayRow = i - TempUpperLeft.X;
                        int arrayCol = j - TempUpperLeft.Y;

                        if (nextArray[arrayRow, arrayCol] == 2 && Cells[i, j] == 1)
                        {
                            rotationPossible = false;
                        }
                    }
                }
            }
            catch
            {
                rotationPossible = false;
            }

            if (rotationPossible)
            {
                Clear();
                Falling.NextPosition();
                Falling.UpperLeft = new Point(TempUpperLeft.X, TempUpperLeft.Y);
                FallingArray = nextArray;
                ImprintAShapeOnLevel();
            }
        }

        public void Clear()
        {
            for (int i = Falling.UpperLeft.X; i <= Falling.UpperLeft.X + FallingArray.GetUpperBound(0); i++)
            {
                for (int j = Falling.UpperLeft.Y; j <= Falling.UpperLeft.Y + FallingArray.GetUpperBound(1); j++)
                {
                    int arrayRow = i - Falling.UpperLeft.X;
                    int arrayCol = j - Falling.UpperLeft.Y;

                    if (FallingArray[arrayRow, arrayCol] == 2)
                    {
                        Cells[i, j] = 0;
                    }
                }
            }
        }

        public void ShrinkLevel()
        {
            int shrinkenRows = 0;
            bool shrink = true;
            for (int i = Falling.UpperLeft.X; i <= Falling.UpperLeft.X + FallingArray.GetUpperBound(0); i++)
            {
                shrink = true;
                for (int j = 0; j < Cols; j++)
                {
                    if (Cells[i, j] == 0)
                    {
                        shrink = false;
                    }
                }

                if (shrink)
                {
                    shrinkenRows++;
                    for (int sj = 0; sj < Cols; sj++)
                    {
                        for (int si = i; si >= 0; si--)
                        {
                            if (si != 0)
                            {
                                Cells[si, sj] = Cells[si - 1, sj];
                            }
                            else
                            {
                                Cells[si, sj] = 0;
                            }
                        }
                    }
                }
            }

            if (shrinkenRows > 0)
            {
                switch (shrinkenRows)
                {
                    case 1:
                        Scores += 1;
                        break;
                    case 2:
                        Scores += 3;
                        break;
                    case 3:
                        Scores += 8;
                        break;
                    case 4:
                        Scores += 20;
                        break;
                    default:
                        break;
                }

                PlaySound = true;
            }
        }

        public void Update(GameTime gt)
        {
            elapsedTime += (float)gt.ElapsedGameTime.Milliseconds;
            if (elapsedTime > updateGameInterval)
            {
                elapsedTime = 0.0f;
                if (!CheckForObstacle(Direction.Down))
                {
                    Translate(Direction.Down);
                }
                else
                {
                    Petrify();
                    ShrinkLevel();
                    GetNext();
                }
            }
        }

        public string ScoreBoard()
        {
            return string.Format("Scores: {0}", Scores);
        }
    }
}
