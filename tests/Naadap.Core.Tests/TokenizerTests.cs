namespace Naadap.Core.Tests;

public class TokenizerTests
{
    [Fact]
    public void Tokenize_LowercasesAndKeepsContentWords()
    {
        var tokens = Tokenizer.Tokenize("Flight-Line Engineering Support");

        Assert.Contains("flight-line", tokens);
        Assert.Contains("engineering", tokens);
    }

    [Fact]
    public void Tokenize_DropsStopWordsAndShortTokens()
    {
        var tokens = Tokenizer.Tokenize("The Contractor shall provide a system for it.");

        Assert.DoesNotContain("the", tokens);
        Assert.DoesNotContain("shall", tokens);
        Assert.DoesNotContain("contractor", tokens);
        Assert.DoesNotContain("for", tokens);
        Assert.DoesNotContain("it", tokens);
        Assert.DoesNotContain("a", tokens);
    }

    [Fact]
    public void Tokenize_IgnoresNumbersAndPunctuationOnlyText()
    {
        var tokens = Tokenizer.Tokenize("1.1 2024-01-01 -- $500.00");

        Assert.Empty(tokens);
    }

    [Fact]
    public void Tokenize_EmptyText_ReturnsEmptyList()
    {
        Assert.Empty(Tokenizer.Tokenize(string.Empty));
    }
}
