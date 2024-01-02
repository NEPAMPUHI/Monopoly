using System.Net.NetworkInformation;

namespace Monopoly.Cards; 

public class Work : Card {
    private const int startWorkingTerm = 3;
    public string[] TextToPrintInAField {
        get { return new[] { "<РОБОТА>" }; } 
    }
    public string DoActionIfArrived(Field field, Player player) {
        return StartWork(player);
    }
    
    public string DoActionIfStayed(Field field, Player player, out bool isNextMoveNeed) {
        return Working(player, out isNextMoveNeed);
    }
    
    private string StartWork(Player player) {
        player.turnsCanContinueWork = startWorkingTerm - player.howManyTimesWorked;

        if (player.turnsCanContinueWork == 0) {
            return player.nameInGame + " зробив уже всі завдання на роботі. Роботодавець не може дати роботу гравцю";
        }
        else {
            return player.nameInGame + " отримує роботу на " + player.turnsCanContinueWork + " " +
                   DayEnding(player.turnsCanContinueWork) + ". Початок — завтра.";
        }
    }

    private string Working(Player player, out bool isNextMoveNeed) {
        if (player.turnsCanContinueWork == 0) {
            isNextMoveNeed = true;
            if (player.howManyTimesWorked < startWorkingTerm) {
                player.howManyTimesWorked++;
            }
            return player.nameInGame +
                   " відробляє повний термін на даний момент. Гравець може зробити хід далі";
        }

        JustOutput.OutWorkChoice(player);
        string personChoice = Interactive.GetPersonChoice(new List<string>() { "1", "2"});
        if (personChoice == "1") {
            isNextMoveNeed = false;
            player.turnsCanContinueWork--;

            int randSalary = App.rand.Next(100, 251) * (player.howManyTimesWorked + 1);
            randSalary /= 10;
            randSalary *= 10;
            player.moneyAmount += randSalary;
            return "Гарна робота! Роботодавець заплатив " + randSalary + " гривень за день роботи";
        }
        
        isNextMoveNeed = true;
        player.turnsCanContinueWork = 0;
        if (player.howManyTimesWorked < startWorkingTerm) {
            player.howManyTimesWorked++;
        }
        return player.nameInGame + " покидає роботу та може зробити хід далі, назустріч мрії";
    }
}