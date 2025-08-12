﻿using System.IO.Abstractions.TestingHelpers;
using FileOrganizer.Core;
using FileOrganizer.Models;
using FluentAssertions;
using Moq;
using Xunit;

namespace FileOrganizer.TestApp.UnitTests
{
    public class FileManagerTests
    {
        private readonly MockFileSystem _fileSystem;
        private readonly FileManager _fileManager;

        public FileManagerTests()
        {
            _fileSystem = new MockFileSystem();
            _fileManager = new FileManager(_fileSystem);
        }

        [Fact]
        public void MoveFile_ShouldUpdateFileProperties()
        {
            // Arrange
            var sourceDir = Path.Combine("C:", "source");
            var sourceFile = Path.Combine(sourceDir, "file.txt");
            var destFolder = @"C:\destination";

            _fileSystem.AddDirectory(sourceDir);
            _fileSystem.AddFile(sourceFile, new MockFileData("content"));
            _fileSystem.AddDirectory(destFolder);

            var file = new Models.File
            {
                Name = "file.txt",
                Path = sourceFile,
                Extension = ".txt"
            };

            // Act
            _fileManager.MoveFile(file, destFolder);

            // Assert
            file.Path.Should().Be(@"C:\destination\file.txt");
            file.OriginalPath.Should().Be(sourceFile);
            _fileSystem.File.Exists(@"C:\destination\file.txt").Should().BeTrue();
            _fileSystem.File.Exists(sourceFile).Should().BeFalse();
        }

        [Fact]
        public void ScanFolder_ShouldReturnAllFiles()
        {
            // Arrange
            var testDir = Path.Combine("C:", "test");
            var file1 = Path.Combine(testDir, "file1.txt");
            var file2 = Path.Combine(testDir, "file2.jpg");

            _fileSystem.AddDirectory(testDir);
            _fileSystem.AddFile(file1, new MockFileData("test content"));
            _fileSystem.AddFile(file2, new MockFileData("test content"));

            // Act
            var result = _fileManager.ScanFolder(testDir);

            // Assert
            result.Should().HaveCount(2);
            result.Should().Contain(f => f.Name == "file1.txt");
            result.Should().Contain(f => f.Name == "file2.jpg");
        }
    }
}