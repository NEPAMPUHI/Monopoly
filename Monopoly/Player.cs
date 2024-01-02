using Monopoly.Cards;

namespace Monopoly;

public class Player {
    public readonly string nameInGame;
    public readonly ConsoleColor chipColor;
    public int moneyAmount;
    public Position? positionInField;
    public int turnsToGoOutOfPrison;
    public int howManyTimesPayedInPrison;
    public bool canGoOutOfCountry;
    public int turnsCanContinueWork;
    public int howManyTimesWorked;

    public Player(string nameInGame,
        ConsoleColor chipColor = ConsoleColor.White, int moneyAmount = 0, Position? positionInField = null,
        int turnsToGoOutOfPrison = 0, int howManyTimesPayedInPrison = 0,
        bool canGoOutOfCountry = false, int turnsCanContinueWork = 0, int howManyTimesWorked = 0) {
        this.nameInGame = nameInGame;
        this.moneyAmount = moneyAmount;
        this.positionInField = positionInField;
        this.chipColor = chipColor;
        this.turnsToGoOutOfPrison = turnsToGoOutOfPrison;
        this.howManyTimesPayedInPrison = howManyTimesPayedInPrison;
        this.canGoOutOfCountry = canGoOutOfCountry;
        this.turnsCanContinueWork = turnsCanContinueWork;
        this.howManyTimesWorked = howManyTimesWorked;
    }

    public List<Enterprise> GetAllPlayerEnterprises(Field field) {
        List<Enterprise> ans = new List<Enterprise>();
        foreach (var array in field.fieldArrays) {
            foreach (var card in array) {
                if (card is Enterprise enterprise && enterprise.owner == this) {
                    ans.Add(enterprise);
                }
            }
        }
        return ans;
    }

    public List<Enterprise> GetPawnedOrNotPlayerEnterprises(Field field, bool isPawnedNeed) {
        List<Enterprise> enterprises = GetAllPlayerEnterprises(field);
        
        for (int i = 0; i < enterprises.Count; i++) {
            Enterprise enterprise = enterprises[i];
            if (isPawnedNeed != enterprise.IsPawned()) {
                enterprises.RemoveAt(i);
                i--;
            }
        }
        return enterprises;
    }
    
    public List<Enterprise> GetFullIndustryWithoutNHotelsEnterprises(Field field) {
        List<Enterprise> enterprises = GetAllPlayerEnterprises(field);
        for (int i = 0; i < enterprises.Count; i++) {
            Enterprise enterprise = enterprises[i];
            if (!(enterprise.isFullIndustry && !enterprise.isBuiltHotel)) {
                enterprises.RemoveAt(i);
                i--;
            }
        }
        return enterprises;
    }

    public void MakeTurnForPawnedEnter(Field field) {
        List<Enterprise> enterprises = GetAllPlayerEnterprises(field);
        foreach (var enterprise in enterprises) {
            if (enterprise.IsPawned()) {
                enterprise.ReduceTurnsAmount();
                if (enterprise.turnsToDisappearIfPawned == 0) {
                    enterprise.ClearEnterprise();
                }
            }
        }
    }

    public void FreeAllEnterprises(Field field) { // Tut?
        List<Enterprise> enterprises = GetAllPlayerEnterprises(field);
        foreach (var enterprise in enterprises) {
            enterprise.ClearEnterprise();
        }
    }

    public bool IsInPrison() {
        return turnsToGoOutOfPrison != 0;
    }
}