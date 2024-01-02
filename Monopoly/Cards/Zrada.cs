namespace Monopoly.Cards;
using Monopoly.OutputDesign;

public class Zrada : Card {
    private delegate string Action(Field field, Player player);
    
    private readonly int[] probability = { 50, 30, 10, 10 };
    
    public override string[] TextToPrintInAField {
        get { return OutputPhrases.outputTextByTags["Zrada"]; } 
    }
    public override string DoActionIfArrived(Field field, Player player) {
        return GivePlayerAZrada(field, player);
    }

    public override string DoActionIfStayed(Field field, Player player, out bool isNextMoveNeed) {
        return JustTurn(field, player, out isNextMoveNeed);
    }

    private string GivePlayerAZrada(Field field, Player player) {
        Action action = Choose();
        return action(field, player);
    }

    private Action Choose() {
        return GetActionNumber() switch {
            0 => Take500FromAPlayer,
            1 => Take1000FromAPlayer,
            2 => Take2000FromAPlayer,
            _ => Take0FromAPlayer
        };
    }

    private int GetActionNumber() {
        int randFrom0To100 = App.rand.Next(100);
        int curIndex = -1;
        do {
            curIndex++;
            randFrom0To100 -= probability[curIndex];
        } while (randFrom0To100 >= 0);
        return curIndex;
    }

    private string Take500FromAPlayer(Field field, Player player) {
        int moneyToTake = 500;
        player.moneyAmount -= moneyToTake;
        return OutputPhrases.TextBonusOrZradaMoneyAmount(player, moneyToTake, false);
    }
    
    private string Take1000FromAPlayer(Field field, Player player) {
        int moneyToTake = 1000;
        player.moneyAmount -= moneyToTake;
        return OutputPhrases.TextBonusOrZradaMoneyAmount(player, moneyToTake, false);
    }
    
    private string Take2000FromAPlayer(Field field, Player player) {
        int moneyToTake = 2000;
        player.moneyAmount -= moneyToTake;
        return OutputPhrases.TextBonusOrZradaMoneyAmount(player, moneyToTake, false);
    }
    
    private string Take0FromAPlayer(Field field, Player player) {
        return OutputPhrases.TextNoGainOrTake(player);
    }
}