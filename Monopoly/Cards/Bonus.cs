namespace Monopoly.Cards;
using Monopoly.OutputDesign;

public class Bonus : Card {
    private delegate string Action(Field field, Player player);
    
    private readonly int[] probability = { 50, 20, 10, 20 };
    
    public override string[] TextToPrintInAField {
        get { return OutputPhrases.outputTextByTags["Bonus"]; }
    }
    
    public override string DoActionIfArrived(Field field, Player player) {
        return GivePlayerABonus(field, player);
    }
    
    public override string DoActionIfStayed(Field field, Player player, out bool isNextMoveNeed) {
        return JustTurn(field, player, out isNextMoveNeed);
    }

    private string GivePlayerABonus(Field field, Player player) {
        Action action = Choose();
        return action(field, player);
    }

    private Action Choose() {
        return GetActionNumber() switch {
            0 => Give500ToPlayer,
            1 => Give1000ToPlayer,
            2 => Give5000ToPlayer,
            _ => GiveNothing
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

    private string Give500ToPlayer(Field field, Player player) {
        int bonusMoney = 500;
        player.moneyAmount += bonusMoney;
        return OutputPhrases.TextBonusOrZradaMoneyAmount(player, bonusMoney, true);
    }
    
    private string Give1000ToPlayer(Field field, Player player) {
        int bonusMoney = 1000;
        player.moneyAmount += bonusMoney;
        return OutputPhrases.TextBonusOrZradaMoneyAmount(player, bonusMoney, true);
    }

    private string Give5000ToPlayer(Field field, Player player) {
        int bonusMoney = 2000;
        player.moneyAmount += bonusMoney;
        return OutputPhrases.TextBonusOrZradaMoneyAmount(player, bonusMoney, true);
    }

    private string GiveNothing(Field field, Player player) {
        return OutputPhrases.TextNoGainOrTake(player);
    }
}