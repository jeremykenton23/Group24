using System.Collections.Generic;

namespace NonogramApp
{
    public abstract class PuzzleGame
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public bool[,] Grid { get; protected set; } = null!;
        public List<List<int>> RowHints { get; protected set; } = null!;
        public List<List<int>> ColumnHints { get; protected set; } = null!;

        public abstract void GeneratePuzzle();
        public abstract bool SolvePuzzle();
        public abstract void ShowSolution();
        public abstract bool CheckUserSolution();
    }
}
