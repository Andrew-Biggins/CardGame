namespace CardGame.Engine.UnitTests;

public class IntegrationTests
{
    [GwtTheory("Given I have started the Card Game application",
         "When I enter a '<listOfCards>'",
         "Then the correct '<score>' should be displayed on the user interface")]
    [InlineData("2C", true, 2, "")]
    [InlineData("2D", true, 4, "")]
    [InlineData("2H", true, 6, "")]
    [InlineData("2S", true, 8, "")]
    [InlineData("TC", true, 10, "")]
    [InlineData("JC", true, 11, "")]
    [InlineData("QC", true, 12, "")]
    [InlineData("KC", true, 13, "")]
    [InlineData("AC", true, 14, "")]
    [InlineData("3C,4C", true, 7, "")]
    [InlineData("TC,TD,TH,TS", true, 100, "")]
    public void T0(string input, bool expectedSuccess, int expectedScore, string expectedError)
    {
        var vm = new MainWindowViewModel(new GameEngine());

        vm.Input = input;
        vm.ComputeScoreCommand.Execute(null);

        Assert.Equal(expectedSuccess, string.IsNullOrEmpty(vm.Error));
        Assert.Equal(expectedScore, vm.Score);
        Assert.Equal(expectedError, vm.Error);
    }

    [GwtTheory("Given I have started the Card Game application",
        "When I enter a '<listOfCards>' containing one or two Jokers",
        "Then the '<score>' for the hand should be doubled and displayed on the user interface")]
    [InlineData("JR", true, 0, "")]
    [InlineData("JR,JR", true, 0, "")]
    [InlineData("2C,JR", true, 4, "")]
    [InlineData("JR,2C,JR", true, 8, "")]
    [InlineData("TC,TD,JR,TH,TS", true, 200, "")]
    [InlineData("TC,TD,JR,TH,TS,JR", true, 400, "")]
    // All cards
    [InlineData("2C,2D,2H,2S,3C,3D,3H,3S,4C,4D,4H,4S,5C,5D,5H,5S,6C,6D,6H,6S,7C,7D,7H,7S,8C,8D,8H,8S,9C,9D,9H,9S,TC,TD,TH,TS,JC,JD,JH,JS,QC,QD,QH,QS,KC,KD,KH,KS,AC,AD,AH,AS,JR,JR", true, 4160, "")]
    public void T1(string input, bool expectedSuccess, int expectedScore, string expectedError)
    {
        var vm = new MainWindowViewModel(new GameEngine());

        vm.Input = input;
        vm.ComputeScoreCommand.Execute(null);

        Assert.Equal(expectedSuccess, string.IsNullOrEmpty(vm.Error));
        Assert.Equal(expectedScore, vm.Score);
        Assert.Equal(expectedError, vm.Error);
    }

    [GwtTheory("Given I have started the Card Game application",
        "When I enter a '<listOfCards>' that is invalid",
        "Then an '<errorMessage>' should be displayed on the user interface")]
    [InlineData("1S", false, 0, ErrorMessages.CardNotRecognised)]
    [InlineData("2B", false, 0, ErrorMessages.CardNotRecognised)]
    [InlineData("2S,1S", false, 0, ErrorMessages.CardNotRecognised)]
    [InlineData("3H,3H", false, 0, ErrorMessages.DuplicateCards)]
    [InlineData("4D,5D,4D", false, 0, ErrorMessages.DuplicateCards)]
    [InlineData("JR,JR,JR", false, 0, ErrorMessages.TooManyJokers)]
    [InlineData("2S|3D", false, 0, ErrorMessages.InvalidInput)]
    [InlineData(null, false, 0, ErrorMessages.InvalidInput)]
    [InlineData("", false, 0, ErrorMessages.InvalidInput)]
    [InlineData("2SS", false, 0, ErrorMessages.InvalidInput)]
    [InlineData("T", false, 0, ErrorMessages.InvalidInput)]
    public void T2(string input, bool expectedSuccess, int expectedScore, string expectedError)
    {
        var vm = new MainWindowViewModel(new GameEngine());

        vm.Input = input;
        vm.ComputeScoreCommand.Execute(null);

        // expectedSuccess indicates "no error shown" in UI, keep assertion explicit
        if (expectedSuccess)
            Assert.True(string.IsNullOrEmpty(vm.Error));
        else
            Assert.Equal(expectedError, vm.Error);

        Assert.Equal(expectedScore, vm.Score);
    }
}
