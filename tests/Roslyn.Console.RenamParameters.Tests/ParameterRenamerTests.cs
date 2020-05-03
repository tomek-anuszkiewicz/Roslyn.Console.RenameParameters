using Roslyn.Console.RenameParameters;
using System;
using Xunit;

namespace Roslyn.Console.RenamParameters.Tests
{
    public class ParameterRenamerTests
    {
        [Theory]
        [InlineData("x", "x")]
        [InlineData("x_a", "xA")]
        [InlineData("a_x", "x")]
        [InlineData("a_x_y", "xY")]
        [InlineData("create_new", "createNew")]
        [InlineData("a_create_new", "createNew")]
        public void Rename(string name, string expectedName)
        {
            Assert.Equal(expectedName, ParameterRenamer.Rename(name));
        }

        [Fact]
        public void Rename_NameIsNull_Throws()
        {
            Assert.Throws<ArgumentNullException>("name", () => ParameterRenamer.Rename(null));
        }

        [Theory]
        [InlineData("_a")]
        public void Rename_NameStartsWithUnderscore_Throws(string name)
        {
            Assert.Throws<InvalidOperationException>(() => ParameterRenamer.Rename(name));
        }

        [Fact]
        public void Rename_NameEndsWithUnderscore_Throws()
        {
            Assert.Throws<InvalidOperationException>(() => ParameterRenamer.Rename("a_"));
        }

        [Fact]
        public void Rename_NameContainerConsecutibeUnderscores_Throws()
        {
            Assert.Throws<InvalidOperationException>(() => ParameterRenamer.Rename("a__b"));
        }

        [Fact]
        public void Rename_NameIsSingleUnderscore_DoNotRename()
        {
            Assert.Equal("_", ParameterRenamer.Rename("_"));
        }
    }
}
