namespace Monopoly.Cards; 

public class ExitChance : Card {
    
    public string[] TextToPrintInAField {
        get { return new[] { "<ШАНС ВИХОДУ>" }; } 
    }
    public string DoActionIfArrived(Field field, Player player) {
        return GuessIsGoOut(player);
    }
    
    public string DoActionIfStayed(Field field, Player player, out bool isNextMoveNeed) {
        return GoOutOrNot(player, out isNextMoveNeed);
    }

    private string GoOutOrNot(Player player, out bool isNextMoveNeed) {
        isNextMoveNeed = true;
        bool isGoOut = player.canGoOutOfCountry;

        if (!player.canGoOutOfCountry) {
            player.positionInField.cellIndex = -1;
        }
        player.canGoOutOfCountry = false;
        
        return player.nameInGame + " " + (isGoOut
            ? "нарешті виходить з країни та зараз ходить"
            : "не виходить з країни і йде далі по колу :(");
    }

    private string GuessIsGoOut(Player player) {
        bool isGoOut = GamePlay.RollCoin();
        player.canGoOutOfCountry = isGoOut;
        return "Вийде " + player.nameInGame + " з країни чи ні — пока що загадка.";
    }
}