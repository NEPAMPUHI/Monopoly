namespace Monopoly.Cards; 

public class Zrada : Card {
    private delegate string Action(Field field, Player player);
    
    private readonly int[] probability = { 50, 30, 10, 10 };
    
    public string[] TextToPrintInAField {
        get { return new[] { "<ЗРАДА>" }; } 
    }
    public string DoActionIfArrived(Field field, Player player, out bool isNextMoveNeed) {
        isNextMoveNeed = false;
        return GivePlayerAZrada(field, player);
    }

    public string DoActionIfStayed(Field field, Player player, out bool isNextMoveNeed) {
        return JustTurn(field, player, out isNextMoveNeed);
    }

    private string JustTurn(Field field, Player player, out bool isNextMoveNeed) {
        isNextMoveNeed = true;
        return player.nameInGame + " ходить.";
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
        Console.WriteLine(randFrom0To100);
        int curIndex = -1;
        do {
            curIndex++;
            randFrom0To100 -= probability[curIndex];
        } while (randFrom0To100 >= 0);
        return curIndex;
    }

    private string Take500FromAPlayer(Field field, Player player) {
        player.moneyAmount -= 500;
        return player.nameInGame + " втрачає 500 гривень!";
    }
    
    private string Take1000FromAPlayer(Field field, Player player) {
        player.moneyAmount -= 500;
        return player.nameInGame + " втрачає 1000 гривень!";
    }
    
    private string Take2000FromAPlayer(Field field, Player player) {
        player.moneyAmount -= 500;
        return player.nameInGame + " втрачає 2000 гривень!";
    }
    
    private string Take0FromAPlayer(Field field, Player player) {
        player.moneyAmount -= 500;
        return player.nameInGame + " оминає зраду і нічого не втрачає!";
    }
}