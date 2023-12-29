namespace Monopoly; 

public class GamePlay {
    public Field field;

    public GamePlay() {
    }

    private void RecreateField() {
        field = new Field();
    }

    public static int RollDice() {
        return App.rand.Next(1, 7);
    }

    public static bool RollCoin() {
        return Convert.ToBoolean(App.rand.Next(0, 2));
    }
}