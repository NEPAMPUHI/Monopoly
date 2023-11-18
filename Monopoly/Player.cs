namespace Monopoly;

public class Player {
    public int moneyAmount;
    public readonly string nameInGame;
    public readonly ConsoleColor chipColor;
    public Position? positionInField;
    public bool isInPrison;
    public int turnsToGoOutOfPrison;

    public Player(string nameInGame, int moneyAmount = 0, Position? positionInField = null,
        ConsoleColor chipColor = ConsoleColor.White, bool isInPrison = false, int turnsToGoOutOfPrison = 0) {
        this.nameInGame = nameInGame;
        this.moneyAmount = moneyAmount;
        this.positionInField = positionInField;
        this.chipColor = chipColor;
        this.isInPrison = isInPrison;
        this.turnsToGoOutOfPrison = turnsToGoOutOfPrison;
    }
}