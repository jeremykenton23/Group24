using System.Collections.Generic;

namespace NonogramApp
{
    public class GameController
    {
        private readonly int width;
        private readonly int height;
        private readonly int puzzleCount;
        public List<PuzzleGame> Puzzles { get; private set; }

        public GameController(int width, int height, int puzzleCount)
        {
            this.width = width;
            this.height = height;
            this.puzzleCount = puzzleCount;
            Puzzles = new List<PuzzleGame>();
        }

        public void GenerateAllPuzzles()
        {
            Puzzles.Clear();
            for (int i = 0; i < puzzleCount; i++)
            {
                var game = new NonogramGame(width, height);
                game.GeneratePuzzle();
                Puzzles.Add(game);
            }
        }
    }
}
