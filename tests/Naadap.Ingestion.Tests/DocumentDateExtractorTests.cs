namespace Naadap.Ingestion.Tests;

public class DocumentDateExtractorTests
{
    [Theory]
    [InlineData("Solicitation issued January 5, 2024 for review.", "2024-01-05")]
    [InlineData("Effective date: 2024-01-05.", "2024-01-05")]
    [InlineData("Due by 01/05/2024 close of business.", "2024-01-05")]
    public void Extract_FindsDateInVariousFormats(string text, string expectedIso)
    {
        var expected = DateOnly.Parse(expectedIso);

        var date = DocumentDateExtractor.Extract(text);

        Assert.Equal(expected, date);
    }

    [Fact]
    public void Extract_ReturnsNullWhenNoDatePresent()
    {
        var date = DocumentDateExtractor.Extract("No date anywhere in this text.");

        Assert.Null(date);
    }
}
