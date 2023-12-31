using Monopoly.Cards;

namespace Monopoly; 

public class GamePlay {
    private Field field;
    private Design design;
    private readonly int indexOfEndOfCountry;
    private readonly int indexOfEndOfArray;
    private readonly int indexOfWorkCell;
    private readonly int enterOnArrayInAnother;
    private readonly int enterTnArrayAfterStart;

    private const int zarplata = 500;
    private const int startCapital = 1000;

    public GamePlay() {
        RecreateField();
        design = new Design();
        indexOfEndOfCountry = field.specialIndexesByCellNames["ExitChance"];
        indexOfEndOfArray = field.fieldArrays[0].Length - 1;
        indexOfWorkCell = field.specialIndexesByCellNames["Work"];
        enterOnArrayInAnother = indexOfWorkCell;
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
        List<Player> playersInGame = players.ToList();
        int curIndexPlayerTurn = 0;
        Player curPlayer;
        bool isNextMoveNeed;
        string messageToPrint;

        while (!isGameEnd) {
            curPlayer = playersInGame[curIndexPlayerTurn];
            PreTurnThings(curPlayer, playersInGame);
            
            messageToPrint = (curPlayer.positionInField == null)
                ? StartTurn(curPlayer, out isNextMoveNeed)
                : field.TakeCardByPlayerPos(curPlayer).DoActionIfStayed(field, curPlayer, out isNextMoveNeed);
            Console.WriteLine(messageToPrint);

            if (isNextMoveNeed) {
                PlayerTurnWithDice(curPlayer);
                Card curCard = field.TakeCardByPlayerPos(curPlayer);
                Console.WriteLine(curPlayer.nameInGame + " переміщується на " + design.PrintCellTitleInAText(curCard) + "\n");
                messageToPrint = field.TakeCardByPlayerPos(curPlayer).DoActionIfArrived(field, curPlayer);
                Console.WriteLine(messageToPrint);
            }

            if (IsPlayerGoOut(curPlayer)) {
                playersInGame.RemoveAt(curIndexPlayerTurn);
                curIndexPlayerTurn--;
                if (playersInGame.Count == 1) {
                    isGameEnd = true;
                    Congratulations(playersInGame[0]);
                }
            }
            curIndexPlayerTurn = (curIndexPlayerTurn + 1) % playersInGame.Count;
        }
    }

    private string StartTurn(Player player, out bool isNextMoveNeed) {
        isNextMoveNeed = true;
        player.positionInField = new Position();
        player.positionInField.cellIndex = enterTnArrayAfterStart - 1;
        
        int countryIndex = Convert.ToInt32(RollCoin());
        player.positionInField.arrayIndex = countryIndex;
        player.moneyAmount += startCapital;
        return player.nameInGame + " випадає доля йти в країну " + field.countriesArray[countryIndex] +
               ". Видано стартові " + startCapital + " гривень на підняття економіки країни";
    }

    private void PlayerTurnWithDice(Player player) {
        int curPlayerArr = player.positionInField.arrayIndex;
        int curPlayerCell = player.positionInField.cellIndex;
        
        Console.Write(player.nameInGame + ", натисніть Enter щоб підкинути кубик");
        Console.ReadLine();
        int randTurnsAmount = RollDice();
        Console.WriteLine("Випадає число " + randTurnsAmount + "!\n");

        int newPlayerCell = curPlayerCell + randTurnsAmount;

        if (curPlayerCell < indexOfWorkCell && newPlayerCell >= indexOfWorkCell) {
            Console.WriteLine(player.nameInGame + " забігає швиденько на роботу, отримує премію в розмірі " + zarplata + " гривень");
            player.moneyAmount += zarplata;
        }
        
        if (curPlayerCell == indexOfEndOfCountry) {
            player.positionInField.cellIndex = newPlayerCell;
        }
        else if (curPlayerCell < indexOfEndOfCountry) {
            player.positionInField.cellIndex = newPlayerCell % (indexOfEndOfCountry + 1);
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

    private void Congratulations(Player player) {
        Console.WriteLine("Вітаємо гравця " + player.nameInGame + " з абсолютною монополією та банком у розмірі " +
                          player.moneyAmount + " гривень!");
    }
    private bool IsPlayerGoOut(Player player) {
        if (player.moneyAmount >= 0) {
            return false;
        }

        Console.WriteLine("У вас не вистачає грошей для продовження гри. Ви маєте закласти у банк якісь зі своїх" +
                          " підприємств і отримаєте стільки, скільки платять інші, коли стають на них" + 
                          " (зможете їх викупити згодом) або програєте");
        
        List<Enterprise> enterprises = player.GetPawnedOrNotPlayerEnterprises(field, false);
        while (enterprises.Count > 0 && player.moneyAmount < 0) {
            Console.WriteLine("Борг банку складає " + player.moneyAmount * (-1));
            Console.WriteLine("Ваші підприємства на даний момент:");
            design.PrintAListOfEnterprisesInOneLine(enterprises);
            int enterpriseToPawn = GainEnterpriseNum(enterprises.Count) - 1;

            enterprises[enterpriseToPawn].PawnInBank();
            enterprises.RemoveAt(enterpriseToPawn);

            if (player.moneyAmount < 0) {
                Console.WriteLine("Грошей для погашення боргу не вистачило\n");
            }
        }

        if (player.moneyAmount > 0) {
            Console.WriteLine("Борг вдалося погасити! Ви ще у грі!");
            return false;
        }
        
        Console.WriteLine("На жаль, підприємства скінчилися, а борг не погашено. " + player.nameInGame + " вибуває з гри");
        player.FreeAllEnterprises(field); // Tut.
        return true;
    }

    private int GainEnterpriseNum(int enterprisesAmount) {
        bool isCorrect = false;
        string numInStr;
        int num;
        
        do {
            Console.Write("Введіть номер підприємства (від 1 до " + enterprisesAmount + "): ");
            numInStr = Console.ReadLine();
            if (int.TryParse(numInStr, out num)) {
                if (num < 1 || num > enterprisesAmount) {
                    Console.WriteLine("Введіть номер підприємства з діапазону!");
                }
                else {
                    isCorrect = true;
                }
            }
            else {
                Console.WriteLine("Введіть число!");
            }
        } while (!isCorrect);

        return num;
    }

    private void PrintPlayersInfo(List<Player> playersInGame) {
        foreach (var player in playersInGame) {
            Console.WriteLine("Гравець " + player.nameInGame + " зараз має " + player.moneyAmount + " гривень. " +
                              "Знаходиться у " + design.GetCountryNameByPlayer(field, player) +
                              " на клітинці " + design.PrintCellTitleInAText(field.TakeCardByPlayerPos(player)) +
                              ". Його підприємства: ");
            design.PrintAListOfEnterprisesInOneLine(player.GetAllPlayerEnterprises(field));
        }
    }

    private void PreTurnThings(Player player, List<Player> playersInGame) {
        player.MakeTurnForPawnedEnter(field);
        Console.WriteLine();
        PrintPlayersInfo(playersInGame);
        Console.WriteLine();
        PawnEnterpriseOrBuildHotel(player);
    }
    private void PawnEnterpriseOrBuildHotel(Player player) {
        List<Enterprise> notPawnedEnterprises = player.GetPawnedOrNotPlayerEnterprises(field, false);
        List<Enterprise> pawnedEnterprises = player.GetPawnedOrNotPlayerEnterprises(field, true);
        List<Enterprise> enterprisesToBuildHotel = player.GetFullIndustryWithoutNHotelsEnterprises(field);
        bool isContinue = true;

        Console.WriteLine(player.nameInGame + " може перед ходом " +
                          "закласти підприємство (" + notPawnedEnterprises.Count + " шт.) у банк, " +
                          "викупити підприємство (" + pawnedEnterprises.Count + " шт.) з банку, " +
                          "побудувати готель (" + enterprisesToBuildHotel.Count + " шт.) на підприємстві");

        do {
            int actionNum = GetNumOfAction();
            switch (actionNum) {
                case 1:
                    if (notPawnedEnterprises.Count == 0) {
                        Console.WriteLine("На даний момент немає підприємств для закладання у банк");
                    }
                    else {
                        Console.WriteLine("Ваші не закладені у банк підприємства:");
                        design.PrintAListOfEnterprisesInOneLine(notPawnedEnterprises);
                        int enterpriseNum = GainEnterpriseNum(notPawnedEnterprises.Count) - 1;
                        notPawnedEnterprises[enterpriseNum].PawnInBank();
                        notPawnedEnterprises.RemoveAt(enterpriseNum);
                    }
                    break;
                case 2:
                    if (pawnedEnterprises.Count == 0) {
                        Console.WriteLine("На даний момент немає підприємств для викупу з банку");
                    }
                    else {
                        Console.WriteLine("Ваші закладені у банк підприємства:");
                        design.PrintAListOfEnterprisesInOneLine(pawnedEnterprises);
                        int enterpriseNum = GainEnterpriseNum(pawnedEnterprises.Count) - 1;
                        if (player.moneyAmount > pawnedEnterprises[enterpriseNum].priceToBuy) {
                            pawnedEnterprises[enterpriseNum].UnPawnFromBank(field);
                            pawnedEnterprises.RemoveAt(enterpriseNum);
                        }
                        else {
                            Console.WriteLine("На жаль, грошей для викупу не вистачає");
                        }
                    }
                    break;
                case 3:
                    if (enterprisesToBuildHotel.Count == 0) {
                        Console.WriteLine("На даний момент немає підприємств для будування готелю там");
                    }
                    else {
                        Console.WriteLine("Ваші підприємства, на яких можна збудувати готель (для побудови необхідно " +
                                          "в рази більше грошей від покупки підприємства):");
                        design.PrintAListOfEnterprisesInOneLine(enterprisesToBuildHotel);
                        int enterpriseNum = GainEnterpriseNum(enterprisesToBuildHotel.Count) - 1;
                        if (player.moneyAmount > enterprisesToBuildHotel[enterpriseNum].priceToBuildHotel) {
                            enterprisesToBuildHotel[enterpriseNum].BuildHomeInEnterprise();
                            enterprisesToBuildHotel.RemoveAt(enterpriseNum);
                        }
                        else {
                            Console.WriteLine("На жаль, грошей для побудови готелю тут не вистачає");
                        }
                    }
                    break;
                default:
                    Console.WriteLine();
                    isContinue = false;
                    break;
            }
        } while (isContinue);
    }

    private int GetNumOfAction() {
        Console.WriteLine("\nВведіть");
        Console.WriteLine("  1, якщо хочете закласти підприємство у банк");
        Console.WriteLine("  2, якщо хочете викупити підприємство з банку");
        Console.WriteLine("  3, якщо хочете побудувати готель на підприємстві");
        Console.WriteLine("  0, якщо поки що нічого не хочете");
        
        bool isCorrect = false;
        string numInStr;
        int num;
        
        do {
            Console.Write("Ваша відповідь: ");
            numInStr = Console.ReadLine();
            if (int.TryParse(numInStr, out num)) {
                if (num is < 0 or > 3) {
                    Console.WriteLine("Введіть коректний номер!");
                }
                else {
                    isCorrect = true;
                }
            }
            else {
                Console.WriteLine("Введіть число!");
            }
        } while (!isCorrect);

        return num;
    }
}