namespace Monopoly.Cards; 

public class Bonus : Card {
    private delegate string Action(Field field, Player player);
    
    private readonly int[] probability = { 20, 20, 10, 50 };
    
    public string[] TextToPrintInAField {
        get { return new[] { "<БОНУС>" }; }
    }
    
    public string DoActionIfArrived(Field field, Player player, out bool isNextMoveNeed) {
        isNextMoveNeed = false;
        return GivePlayerABonus(field, player);
    }
    
    public string DoActionIfStayed(Field field, Player player, out bool isNextMoveNeed) {
        return JustTurn(field, player, out isNextMoveNeed);
    }

    private string JustTurn(Field field, Player player, out bool isNextMoveNeed) {
        isNextMoveNeed = true;
        return player.nameInGame + " ходить.";
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
        Console.WriteLine(randFrom0To100);
        int curIndex = -1;
        do {
            curIndex++;
            randFrom0To100 -= probability[curIndex];
        } while (randFrom0To100 >= 0);
        return curIndex;
    }

    private string Give500ToPlayer(Field field, Player player) {
        player.moneyAmount += 500;
        return player.nameInGame + " отримує 500 гривень!";
    }
    
    private string Give1000ToPlayer(Field field, Player player) {
        player.moneyAmount += 1000;
        return player.nameInGame + " отримує 1000 гривень!";
    }

    private string Give5000ToPlayer(Field field, Player player) {
        player.moneyAmount += 5000;
        return player.nameInGame + " отримує 5000 гривень!";
    }

    private string GiveNothing(Field field, Player player) {
        return player.nameInGame + " нічого не отримує. Пощастить наступного разу!";
    }
}