namespace Monopoly.Cards; 

public class Prison : Card {

    private const int startTurnsToGoOut = 3;
    private const int startPriceToPay = 300;
    private const int additionPriceForEachTurn = 100;


    public string[] TextToPrintInAField {
        get { return new[] { "<ТЮРМА>" }; } 
    }
    public string DoActionIfArrived(Field field, Player player) {
        return SendPlayerToPrison(player);
    }

    public string DoActionIfStayed(Field field, Player player, out bool isNextMoveNeed) {
        return CanPlayerGoOut(player, out isNextMoveNeed);
    }

    public bool IsPayedForFreedom(Player player, int turnsToGoOut) {
        int priceToPay = (startPriceToPay + turnsToGoOut * additionPriceForEachTurn) * (player.howManyTimesPayedInPrison + 1);

        Console.WriteLine("До вас підійшов охоронець та запропонував витягнути з тюрми за " + priceToPay + " гривень");
        Console.WriteLine("Зробіть вибір:");
        Console.WriteLine("  1. Дати хабар та вийти з тюрми");
        Console.WriteLine("  2. Зберегти гроші ті сидіти в тюрмі далі");

        int choice = GetChoice();
        if (choice == 1) {
            if (player.moneyAmount >= priceToPay) {
                player.howManyTimesPayedInPrison++;
                player.moneyAmount -= priceToPay;
                return true;
            }
            else {
                Console.WriteLine("На жаль, грошей виявилося недостатньо");
                return false;
            }
        }
        else {
            Console.WriteLine(player.nameInGame + " вирішив продовжувати відбувати покарання");
            return false;
        }
    }

    private string CanPlayerGoOut(Player player, out bool isNextMoveNeed) {
        string msgToReturn;
        isNextMoveNeed = false;
        
        player.turnsToGoOutOfPrison--;
        int turnsLeft = player.turnsToGoOutOfPrison;
        if (turnsLeft == 0) {
            msgToReturn = player.nameInGame + " нарешті виходить із тюрми!";
            isNextMoveNeed = true;
        }
        else {
            if (IsPayedForFreedom(player, turnsLeft)) {
                player.turnsToGoOutOfPrison = 0;
                msgToReturn = player.nameInGame + " нарешті виходить із тюрми!";
                isNextMoveNeed = true;
            }
            else {
                msgToReturn = player.nameInGame + " залишилося відсидіти ще " + turnsLeft + " " + TurnEnding(turnsLeft);
            }
        }

        return msgToReturn;
    }

    private string SendPlayerToPrison(Player player) {
        player.turnsToGoOutOfPrison = startTurnsToGoOut;

        return player.nameInGame + " потрапляє до в'язниці на " + startTurnsToGoOut + " " + TurnEnding(startTurnsToGoOut);
    }

    private string TurnEnding(int turnsAmount) {
        turnsAmount %= 10;
        return (turnsAmount == 1) ? "хід" : ((turnsAmount is > 1 and < 5) ? "ходи" : "ходів") ;
    }

    private int GetChoice() {
        string? inputStr = null;
        do {
            if (inputStr != null) {
                Console.WriteLine("Спробуйте ще раз.");
            }

            Console.Write("Ваш вибір: ");
            inputStr = Console.ReadLine();
        } while (!(inputStr is "1" or "2"));

        return Convert.ToInt32(inputStr);
    }
}