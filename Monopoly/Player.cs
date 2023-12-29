using Monopoly.Cards;

namespace Monopoly;

public class Player {
    public readonly string nameInGame;
    public readonly ConsoleColor chipColor;
    public int moneyAmount;
    public Position? positionInField;
    public bool isInPrison;
    public int turnsToGoOutOfPrison;
    public int howManyTimesWasInPrison;
    public bool canGoOutOfCountry;
    public int turnsCanContinueWork;
    public int howManyTimesWorkedFullTerm;

    public Player(string nameInGame,
        ConsoleColor chipColor = ConsoleColor.White, int moneyAmount = 0, Position? positionInField = null,
        bool isInPrison = false, int turnsToGoOutOfPrison = 0, int howManyTimesWasInPrison = 0,
        bool canGoOutOfCountry = false, int turnsCanContinueWork = 0, int howManyTimesWorkedFullTerm = 0) {
        this.nameInGame = nameInGame;
        this.moneyAmount = moneyAmount;
        this.positionInField = positionInField;
        this.chipColor = chipColor;
        this.isInPrison = isInPrison;
        this.turnsToGoOutOfPrison = turnsToGoOutOfPrison;
        this.howManyTimesWasInPrison = howManyTimesWasInPrison;
        this.canGoOutOfCountry = canGoOutOfCountry;
        this.turnsCanContinueWork = turnsCanContinueWork;
        this.howManyTimesWorkedFullTerm = howManyTimesWorkedFullTerm;
    }

    public List<Enterprise> GetAllPlayerEnterprises(Field field) {
        List<Enterprise> ans = new List<Enterprise>();
        foreach (var i in field.fieldArrays) {
            foreach (var enterprise in i) {
                if (enterprise.owner == this) {
                    ans.Add(enterprise);
                }
            }
        }
        return ans;
    }
}