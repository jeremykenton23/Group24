using NanogramGit;
using Xunit;

namespace NanogramGit.Tests
{
    public class PuzzleGameTests
    {
        [Fact]
        public void Solve_ShouldSolvePuzzleCorrectly()
        {
            // Arrange
            var game = new NonogramGame(5, 5);  // Maak een nieuwe puzzel van 5x5
            game.GeneratePuzzle();  // Genereer de puzzel

            // Act
            var solution = game.Solve();  // Los de puzzel op

            // Assert
            Assert.True(game.VerifySolution(solution));  // Verifieer of de oplossing correct is
        }
    }
}
