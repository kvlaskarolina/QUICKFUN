using Xunit;
using QuickFun.Games.Memory;
using QuickFun.Games.Memory.Strategies;
using QuickFun.Domain.Enums;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace QuickFun.Tests
{
    public class MemoryEngineTests
    {

        private class TestMemoryStrategy : IMemoryDifficultyStrategy
        {
            public string Name => "teststrategy";
            public int Rows => 2;
            public int Columns => 2;
            public int DelayMs => 0;
            public int ScorePerRound => 5;
        }

        [Fact]
        public void InitializeGame_ShouldCreateCorrectNumberOfCards()
        {
            // Arrange
            var engine = new MemoryEngine();
            var strategy = new TestMemoryStrategy();

            // Act
            engine.SetDifficulty(strategy);

            // Assert
            Assert.Equal(4, engine.Cards.Count);
            Assert.Equal(2, engine.Cards.GroupBy(c => c.ImageUrl).Count()); // Powinny być dokładnie 2 pary
        }

        [Fact]
        public async Task HandleCardClick_ShouldMatchTwoIdenticalCards()
        {
            // Arrange
            var engine = new MemoryEngine();
            engine.SetDifficulty(new TestMemoryStrategy());

            var firstCard = engine.Cards[0];
            var secondCard = engine.Cards.First(c => c.Id != firstCard.Id && c.ImageUrl == firstCard.ImageUrl);

            // Act
            await engine.HandleCardClick(firstCard);
            await engine.HandleCardClick(secondCard);

            // Assert
            Assert.True(firstCard.IsMatched);
            Assert.True(secondCard.IsMatched);
            Assert.Equal(5, engine.Score);
            Assert.Contains("Znaleziono parę", engine.Message);
        }

        [Fact]
        public async Task HandleCardClick_ShouldNotMatchDifferentCards()
        {
            // Arrange
            var engine = new MemoryEngine();
            engine.SetDifficulty(new TestMemoryStrategy());

            var firstCard = engine.Cards[0];
            var secondCard = engine.Cards.First(c => c.ImageUrl != firstCard.ImageUrl);

            // Act
            await engine.HandleCardClick(firstCard);
            await engine.HandleCardClick(secondCard);

            // Assert
            Assert.False(firstCard.IsMatched);
            Assert.False(secondCard.IsMatched);
            Assert.Equal(-2, engine.Score);
            Assert.Contains("Nie pasują", engine.Message);
        }

        [Fact]
        public async Task CheckWin_ShouldSetIsGameOverTrue_WhenAllPairsFound()
        {
            // Arrange
            var engine = new MemoryEngine();
            engine.SetDifficulty(new TestMemoryStrategy());

            var pairs = engine.Cards.GroupBy(c => c.ImageUrl).ToList();

            // Act
            foreach (var pair in pairs)
            {
                var list = pair.ToList();
                await engine.HandleCardClick(list[0]);
                await engine.HandleCardClick(list[1]);
            }

            // Assert
            Assert.True(engine.IsGameOver);
            Assert.Contains("Gratulacje", engine.Message);
        }

        [Fact]
        public void Reset_ShouldClearState()
        {
            // Arrange
            var engine = new MemoryEngine();
            engine.SetDifficulty(new TestMemoryStrategy());
            engine.Cards[0].IsRevealed = true;

            // Act
            engine.Reset();

            // Assert
            Assert.False(engine.IsGameOver);
            Assert.Equal(0, engine.Score);
            Assert.All(engine.Cards, c => Assert.False(c.IsRevealed));
        }
    }
}