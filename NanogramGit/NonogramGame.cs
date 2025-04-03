using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace NonogramApp
{
    public class NonogramGame : PuzzleGame
    {
        private bool[,] solutionGrid = null!;

        public NonogramGame(int width, int height)
        {
            Width = width;
            Height = height;
            Grid = new bool[width, height];
            solutionGrid = new bool[width, height];
            RowHints = new List<List<int>>();
            ColumnHints = new List<List<int>>();
        }

        public override void GeneratePuzzle()
        {
            var rand = new Random();
            int fillRate = rand.Next(30, 60);

            for (int i = 0; i < Width; i++)
                for (int j = 0; j < Height; j++)
                    solutionGrid[i, j] = rand.Next(100) < fillRate;

            GenerateHintsFromSolution();
            HideSolution();
        }

        public override bool SolvePuzzle()
        {
            try
            {
                ShowSolution();
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error solving puzzle: " + ex.Message);
                return false;
            }
        }

        public override void ShowSolution()
        {
            Array.Copy(solutionGrid, Grid, solutionGrid.Length);
        }

        public override bool CheckUserSolution()
        {
            for (int i = 0; i < Width; i++)
                for (int j = 0; j < Height; j++)
                    if (Grid[i, j] != solutionGrid[i, j])
                        return false;
            return true;
        }

        private void GenerateHintsFromSolution()
        {
            RowHints.Clear();
            ColumnHints.Clear();

            for (int y = 0; y < Height; y++)
                RowHints.Add(GetHintsForLine(solutionGrid, y, true));

            for (int x = 0; x < Width; x++)
                ColumnHints.Add(GetHintsForLine(solutionGrid, x, false));
        }

        private List<int> GetHintsForLine(bool[,] grid, int index, bool isRow)
        {
            var hints = new List<int>();
            int count = 0;
            int length = isRow ? Width : Height;

            for (int i = 0; i < length; i++)
            {
                bool value = isRow ? grid[i, index] : grid[index, i];
                if (value) count++;
                else if (count > 0)
                {
                    hints.Add(count);
                    count = 0;
                }
            }

            if (count > 0) hints.Add(count);
            return hints.Count == 0 ? new List<int> { 0 } : hints;
        }

        private void HideSolution()
        {
            Grid = new bool[Width, Height];
        }
    }
}
