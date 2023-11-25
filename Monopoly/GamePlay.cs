namespace Monopoly; 

public class GamePlay {
    private readonly Random rand;
    public Field field;

    public GamePlay() {
        rand = new Random();
        field = new Field();
    }

    public int RollDice() {
        return rand.Next(1, 7);
    }

    public bool RollCoin() {
        return Convert.ToBoolean(rand.Next(0, 2));
    }
}