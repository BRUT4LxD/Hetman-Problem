namespace HetmanProblem
{
    internal partial class Game
    {
        public class Quarter
        {
            public int StartX { get; }
            public int StartY { get; }
            public int EndX { get; }
            public int EndY { get; }

            public Quarter(int startX, int startY, int endX, int endY)
            {
                StartX = startX;
                StartY = startY;
                EndX = endX;
                EndY = endY;
            }
            public bool IsInQuarter(int x, int y)
            {
                return x >= StartX && x < EndX && y >= StartY && y < EndY;
            }
        }
    }
}