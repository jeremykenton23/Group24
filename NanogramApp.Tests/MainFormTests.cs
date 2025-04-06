using System.Windows.Forms;
using NanogramGit;
using Xunit;

namespace NanogramGit.Tests
{
    public class MainFormTests
    {
        [Fact]
        public void BtnGeneratePuzzle_Click_ShouldGenerateNewPuzzle()
        {
            // Arrange
            var mainForm = new MainForm();
            var initialPuzzleCount = mainForm.GameController.Puzzles.Count;  // Sla het aantal puzzels op

            // Act
            var btnGeneratePuzzle = mainForm.Controls["btnGeneratePuzzle"] as Button;  // Haal de knop op
            btnGeneratePuzzle?.PerformClick();  // Simuleer een klik op de knop

            // Assert
            Assert.Equal(initialPuzzleCount + 1, mainForm.GameController.Puzzles.Count);  // Het aantal puzzels moet zijn toegenomen
        }
    }
}
