using FileOrganizer.Core;
using FluentAssertions;
using Xunit;

namespace FileOrganizer.TestApp.UnitTests
{
    public class SettingManagerTests
    {
        [Fact]
        public void LoadRules_WhenFileNotExists_ShouldReturnDefaultRules()
        {
            // Arrange
            var tempFile = Path.GetTempFileName();
            File.Delete(tempFile); // تأكد أن الملف غير موجود
            var manager = new SettingManager(tempFile);

            // Act
            var rules = manager.LoadRules();

            // Assert
            rules.Should().NotBeEmpty();
            rules.Should().Contain(r => r.Extension == ".jpg" && r.FolderName == "Images");
            File.Exists(tempFile).Should().BeTrue(); // تأكد أن الملف أنشئ
        }

        [Fact]
        public void SaveRules_ShouldCreateValidJsonFile()
        {
            // Arrange
            var tempFile = Path.GetTempFileName();
            File.Delete(tempFile);
            var manager = new SettingManager(tempFile);
            var rules = new List<Rule>
            {
                new Rule { Extension = ".test", FolderName = "TestFolder" }
            };

            // Act
            manager.SaveRules(rules);
            var loadedRules = manager.LoadRules();

            // Assert
            loadedRules.Should().BeEquivalentTo(rules);
        }
    }
}