using System.Net.NetworkInformation;

namespace Monopoly.Cards; 

public class Work : Card {
    
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

        player.turnsCanContinueWork = 1;
        for (int i = 0; i < player.howManyTimesWorkedFullTerm; i++) {
            player.turnsCanContinueWork *= 2;
        }

        return player.nameInGame + " отримує стажування на " + player.turnsCanContinueWork + " днів. Початок — завтра.";
    }

    private string Working(Player player, out bool isNextMoveNeed) {
        if (player.turnsCanContinueWork == 0) {
            isNextMoveNeed = true;
            player.howManyTimesWorkedFullTerm++;
            return player.nameInGame +
                   " відробляє повне стажування та підвищує свій авторитет в очах роботодавця. Гравець може зробити хід далі";
        }

        int personChoice = GetPersonChoice(player);
        if (personChoice == 1) {
            isNextMoveNeed = false;
            player.turnsCanContinueWork--;

            int randSalary = App.rand.Next(50, 251);
            randSalary /= 10;
            randSalary *= 10;
            return "Гарна робота! Роботодавець заплатив " + randSalary + " гривень за день роботи";
        }
        
        isNextMoveNeed = true;
        player.turnsCanContinueWork = 0;
        return player.nameInGame + " покидає стажування та може зробити хід далі, назустріч мрії";
    }

    private int GetPersonChoice(Player player) {
        string? inputStr = null;

        Console.WriteLine("До завершення стажування" + player.nameInGame + " має відробити ще " +
                          player.turnsCanContinueWork +
                          " днів. Гравець може відробити день на роботі або завершити стажування та піти далі.");
        Console.WriteLine("Потрібно зробити вибір:");
        Console.WriteLine("  1. Провести ще день на нудній роботі");
        Console.WriteLine("  2. Піти далі досліджувати простори країни");

        do {
            if (inputStr != null) {
                Console.WriteLine("Спробуйте ще раз.");
            }

            Console.Write("Ваш вибір: ");
            inputStr = Console.ReadLine();
        } while (inputStr is "1" or "2");

        return Convert.ToInt32(inputStr);
    }

    private string HryvnaEnding(int turnsAmount) {
        turnsAmount %= 10;
        return (turnsAmount == 1) ? "гривня" : ((turnsAmount is > 1 or < 5) ? "гривні" : "гривень") ;
    }
}