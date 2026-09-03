namespace Naadap.LlmStep.Tests;

public class TokenBudgetTests
{
    [Fact]
    public void TryReserve_WithinBudget_ReservesAndReturnsTrue()
    {
        var budget = new TokenBudget(100);

        Assert.True(budget.TryReserve(40));
        Assert.Equal(40, budget.Used);
        Assert.True(budget.TryReserve(60));
        Assert.Equal(100, budget.Used);
    }

    [Fact]
    public void TryReserve_ExceedsBudget_ReturnsFalseAndReservesNothing()
    {
        var budget = new TokenBudget(100);
        Assert.True(budget.TryReserve(90));

        Assert.False(budget.TryReserve(20));
        Assert.Equal(90, budget.Used); // unchanged - a failed reservation must not partially apply
    }

    [Fact]
    public void TryReserve_NegativeTokens_Throws()
    {
        var budget = new TokenBudget(100);
        Assert.Throws<ArgumentOutOfRangeException>(() => budget.TryReserve(-1));
    }
}
