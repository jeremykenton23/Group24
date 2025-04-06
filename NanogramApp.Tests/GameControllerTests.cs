using NanogramGit;
using Xunit;
using System.Collections.Generic;

namespace NanogramGit.Tests
{
    public class GameControllerTests
    {
        [Fact]
        public void GenerateAllPuzzles_ShouldGenerateUniquePuzzles()
        {
            // Arrange
            int puzzleCount = 10;  // Het aantal puzzels in dat gegenereerd moet worden
            var gameController = new GameController(5, 5, puzzleCount);  // Maak een nieuwe GameController met 5x5 puzzels

            // Act
            gameController.GenerateAllPuzzles();  // Genereer de puzzels

            // Assert
            Assert.Equal(puzzleCount, gameController.Puzzles.Count);  // Het aantal puzzels moet gelijk zijn aan puzzleCount

            // Controleer of alle puzzels uniek zijn
            var uniquePuzzles = new HashSet<string>();
            foreach (var puzzle in gameController.Puzzles)
            {
                uniquePuzzles.Add(puzzle.ToString());  // Voeg de stringrepresentatie van elke puzzel toe
            }
            Assert.Equal(puzzleCount, uniquePuzzles.Count);  // Aantal unieke puzzels moet gelijk zijn aan puzzleCount
        }
    }
}
