using System.Net.NetworkInformation;
using Monopoly.OutputDesign;

namespace Monopoly.Cards;

public class Work : Card {
    private const int startWorkingTerm = 3;
    public override string[] TextToPrintInAField {
        get { return OutputPhrases.outputTextByTags["Work"]; }
    }
    public override string DoActionIfArrived(Field field, Player player) {
        return StartWork(player);
    }

    public override string DoActionIfStayed(Field field, Player player, out bool isNextMoveNeed) {
        return Working(player, out isNextMoveNeed);
    }
    
    private string StartWork(Player player) {
        player.turnsCanContinueWork = startWorkingTerm - player.howManyTimesWorked;

        if (player.turnsCanContinueWork == 0) {
            return OutputPhrases.TextStartWork(player, false);
        }
        else {
            return OutputPhrases.TextStartWork(player, true);
        }
    }

    private string Working(Player player, out bool isNextMoveNeed) {
        if (player.turnsCanContinueWork == 0) {
            isNextMoveNeed = true;
            if (player.howManyTimesWorked < startWorkingTerm) {
                player.howManyTimesWorked++;
            }
            return OutputPhrases.TextFinishWorking(player, true);
        }
        
        JustOutput.PrintText(OutputPhrases.TextWorkChoice(player));
        string personChoice = Interactive.GetPersonChoice(new List<string>() { "1", "2"});
        if (personChoice == "1") {
            isNextMoveNeed = false;
            player.turnsCanContinueWork--;

            int randSalary = App.rand.Next(100, 251) * (player.howManyTimesWorked + 1);
            randSalary /= 10;
            randSalary *= 10;
            player.moneyAmount += randSalary;
            return OutputPhrases.TextBossPayed(randSalary);
        }
        
        isNextMoveNeed = true;
        player.turnsCanContinueWork = 0;
        if (player.howManyTimesWorked < startWorkingTerm) {
            player.howManyTimesWorked++;
        }
        return OutputPhrases.TextFinishWorking(player, false);
    }
}