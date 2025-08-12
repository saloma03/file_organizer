using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FileOrganizer.Core;
using FileOrganizer.Interfaces;
using FluentAssertions;
using Moq;

namespace FileOrganizer.TestApp.UnitTests
{
    public class UndoManagerTests
    {
        [Fact]
        public void Execute_ShouldCallCommandAndAddToHistory()
        {
            // Arrange
            var mockCommand = new Mock<ICommand>();
            var undoManager = new UndoManager();

            // Act
            undoManager.Execute(mockCommand.Object);

            // Assert
            mockCommand.Verify(c => c.Execute(), Times.Once);
            undoManager.history.Count.Should().Be(1);
        }

        [Fact]
        public void Undo_WithEmptyHistory_ShouldDoNothing()
        {
            // Arrange
            var undoManager = new UndoManager();

            // Act & Assert
            undoManager.Invoking(u => u.Undo())
                .Should().NotThrow();
        }
    }

}
