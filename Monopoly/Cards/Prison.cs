namespace Monopoly.Cards; 

public class Prison : Card {

    public string[] TextToPrintInAField {
        get { return new[] { "<ТЮРМА>" }; } 
    }
    public string DoActionIfArrived(Field field, Player player) {
        return SendPlayerToPrison(player);
    }

    public string DoActionIfStayed(Field field, Player player, out bool isNextMoveNeed) {
        return CanPlayerGoOut(player, out isNextMoveNeed);
    }

    private string CanPlayerGoOut(Player player, out bool isNextMoveNeed) {
        string msgToReturn;
        isNextMoveNeed = false;
        
        player.turnsToGoOutOfPrison--;
        int turnsLeft = player.turnsToGoOutOfPrison;
        if (turnsLeft == 0) {
            player.isInPrison = false;
            msgToReturn = player.nameInGame + " нарешті виходить із тюрми!";
            isNextMoveNeed = true;
        }
        else {
            msgToReturn = player.nameInGame + " залишилося відсидіти ще " + turnsLeft + " " + TurnEnding(turnsLeft);
        }

        return msgToReturn;
    }

    private string SendPlayerToPrison(Player player) {
        int movesToGoOut = CountMovesToGoOut(player);
        
        player.isInPrison = true;
        player.turnsToGoOutOfPrison = movesToGoOut;
        player.howManyTimesWasInPrison++;

        return player.nameInGame + " потрапляє до в'язниці на " + movesToGoOut + " " + TurnEnding(movesToGoOut);
    }

    private int CountMovesToGoOut(Player player) {
        int movesToGoOut = 3;
        int playerTimesInPrison = player.howManyTimesWasInPrison;

        for (int i = 0; i < playerTimesInPrison; i++) {
            movesToGoOut += App.rand.Next(100) switch {
                < 34 => 0,
                < 77 => 1,
                _ => 2
            };
        }

        return movesToGoOut;
    }

    private string TurnEnding(int turnsAmount) {
        turnsAmount %= 10;
        return (turnsAmount == 1) ? "хід" : ((turnsAmount is > 1 or < 5) ? "ходи" : "ходів") ;
    }
}