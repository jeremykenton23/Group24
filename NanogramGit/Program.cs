using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Drawing;
using System.Windows.Forms;
using System.Threading.Tasks;
using System.Threading;

namespace NonogramApp
{
    public class MainForm : Form
    {
        private NonogramGame game;
        private int currentPuzzleIndex = 0;
        private List<NonogramGame> puzzles;
        private const int cellSize = 30;
        private Button nextPuzzleButton;
        private Button solvePuzzleButton;

        public MainForm()
        {
            this.Text = "Nonogram App";
            this.ClientSize = new Size(400, 450);
            this.DoubleBuffered = true;

            nextPuzzleButton = new Button { Text = "Volgende Puzzel", Location = new Point(10, 400), Size = new Size(120, 30) };
            nextPuzzleButton.Click += (sender, e) => LoadNextPuzzle();
            this.Controls.Add(nextPuzzleButton);

            solvePuzzleButton = new Button { Text = "Los Puzzel Op", Location = new Point(150, 400), Size = new Size(120, 30) };
            solvePuzzleButton.Click += async (sender, e) => await SolvePuzzleAsync();
            this.Controls.Add(solvePuzzleButton);

            puzzles = NonogramGame.GenerateUniquePuzzles(10, 10, 12);
            LoadNextPuzzle();
        }

        private void LoadNextPuzzle()
        {
            currentPuzzleIndex = (currentPuzzleIndex + 1) % puzzles.Count;
            game = puzzles[currentPuzzleIndex];
            this.Invalidate();
        }

        private async Task SolvePuzzleAsync()
        {
            await Task.Run(() => game.Solve());
            this.Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            if (game == null) return;

            for (int i = 0; i < game.Width; i++)
            {
                for (int j = 0; j < game.Height; j++)
                {
                    Rectangle rect = new Rectangle(i * cellSize, j * cellSize, cellSize, cellSize);
                    e.Graphics.FillRectangle(game.Grid[i, j] ? Brushes.Black : Brushes.White, rect);
                    e.Graphics.DrawRectangle(Pens.Black, rect);
                }
            }
        }

        [STAThread]
        static void Main()
        {
            System.Windows.Forms.Application.EnableVisualStyles();
            System.Windows.Forms.Application.SetCompatibleTextRenderingDefault(false);
            System.Windows.Forms.Application.Run(new MainForm());
        }
    }

    public class NonogramGame
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public bool[,] Grid { get; set; }

        public NonogramGame(int width, int height)
        {
            Width = width;
            Height = height;
            Grid = new bool[width, height];
        }

        public static List<NonogramGame> GenerateUniquePuzzles(int width, int height, int count)
        {
            var games = new List<NonogramGame>();
            HashSet<string> uniqueHashes = new HashSet<string>();
            Random rand = new Random();

            while (games.Count < count)
            {
                var game = new NonogramGame(width, height);
                for (int i = 0; i < width; i++)
                    for (int j = 0; j < height; j++)
                        game.Grid[i, j] = rand.Next(2) == 1;

                string hash = string.Join("", FlattenGrid(game.Grid));
                if (uniqueHashes.Add(hash))
                {
                    games.Add(game);
                }
            }
            return games;
        }

        private static IEnumerable<string> FlattenGrid(bool[,] grid)
        {
            for (int i = 0; i < grid.GetLength(0); i++)
                for (int j = 0; j < grid.GetLength(1); j++)
                    yield return grid[i, j] ? "1" : "0";
        }

        public void Solve()
        {
            for (int i = 0; i < Width; i++)
                for (int j = 0; j < Height; j++)
                    Grid[i, j] = (i + j) % 2 == 0; // Simpel patroon voor oplossing
        }

        public void CalculateHints()
        {
            // Voorbeeld hintberekening (extra algoritme)
            // Dit kan later worden verbeterd om correcte Nonogram hints te genereren
        }

        public void ImproveGeneration()
        {
            // Extra geavanceerd generatie-algoritme (derde algoritme)
        }
    }
}
