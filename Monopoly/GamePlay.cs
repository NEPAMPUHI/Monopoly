namespace Monopoly; 

public class GamePlay {
    public Field field;
    private readonly int indexOfEndOfCountry;
    private readonly int indexOfEndOfArray;
    private readonly int enterOnArrayInAnother;
    private readonly int enterTnArrayAfterStart;

    public GamePlay() {
        RecreateField();
        indexOfEndOfCountry = field.specialIndexesByCellNames["ExitChance"];
        indexOfEndOfArray = field.fieldArrays.Length - 1;
        enterOnArrayInAnother = field.specialIndexesByCellNames["Work"];
        enterTnArrayAfterStart = field.specialIndexesByCellNames["Bonus"] + 1;
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

    public void StartGameWithFriends(Player[] players) {
        bool isGameEnd = false;
        int curIndexPlayerTurn = 0;
        Player curPlayer;
        bool isNextMoveNeed;
        string messageToPrint;

        while (!isGameEnd) {
            curPlayer = players[curIndexPlayerTurn];
            
            messageToPrint = (curPlayer.positionInField == null)
                ? StartTurn(curPlayer, out isNextMoveNeed)
                : field.TakeCardByPlayerPos(curPlayer).DoActionIfStayed(field, curPlayer, out isNextMoveNeed);
            
            Console.WriteLine(messageToPrint);

            if (isNextMoveNeed) {
                PlayerTurnWithDice(curPlayer);
                messageToPrint = field.TakeCardByPlayerPos(curPlayer).DoActionIfArrived(field, curPlayer);
            }
            
            Console.WriteLine(messageToPrint);
        }
    }

    private string StartTurn(Player player, out bool isNextMoveNeed) {
        isNextMoveNeed = true;
        player.positionInField.cellIndex = enterTnArrayAfterStart - 1;
        
        int countryIndex = Convert.ToInt32(RollCoin());
        player.positionInField.arrayIndex = countryIndex;
        return player.nameInGame + " випадає доля йти в країну " + field.countriesArray[countryIndex];
    }

    private void PlayerTurnWithDice(Player player) {
        int curPlayerArr = player.positionInField.arrayIndex;
        int curPlayerCell = player.positionInField.cellIndex;
        
        Console.Write(player.nameInGame + ", натисніть Enter щоб підкинути кубик");
        Console.ReadLine();
        int randTurnsAmount = RollDice();
        Console.Write("Випадає число " + randTurnsAmount + "!");

        int newPlayerCell = curPlayerCell + randTurnsAmount;
        if (curPlayerCell == indexOfEndOfCountry) {
            player.positionInField.cellIndex = newPlayerCell;
        }
        else if (curPlayerCell < indexOfEndOfCountry) {
            player.positionInField.cellIndex %= (indexOfEndOfCountry + 1);
        }
        else { // curPlayerCell > indexOfEndOfCountry
            if (newPlayerCell <= indexOfEndOfArray) {
                player.positionInField.cellIndex = newPlayerCell;
            }
            else {
                player.positionInField.arrayIndex = curPlayerArr == 0 ? 1 : 0;
                player.positionInField.cellIndex = newPlayerCell - indexOfEndOfArray - 1 + enterOnArrayInAnother;
            }
        }
    }
    
    
}