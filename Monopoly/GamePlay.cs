using Monopoly.Cards;
using Monopoly.OutputDesign;
using System.Collections.Generic;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Monopoly;

public class GamePlay {
    private Field field;
    private readonly int indexOfEndOfCountry;
    private readonly int indexOfEndOfArray;
    private readonly int indexOfWorkCell;
    private readonly int enterOnArrayInAnother;
    private readonly int enterTnArrayAfterStart;

    private const int salary = 500;
    private const int startCapital = 1000;

    public GamePlay() {
        field = new Field();
        indexOfEndOfCountry = field.specialIndexesByCellNames["ExitChance"];
        indexOfEndOfArray = field.fieldArrays[0].Length - 1;
        indexOfWorkCell = field.specialIndexesByCellNames["Work"];
        enterOnArrayInAnother = indexOfWorkCell;
        enterTnArrayAfterStart = field.specialIndexesByCellNames["Bonus"] + 1;
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
            JustOutput.PrintText(messageToPrint);

            if (isNextMoveNeed) {
                PlayerTurnWithDice(curPlayer);
                JustOutput.PrintText(OutputPhrases.PlayerMovedTo(curPlayer, field));
                messageToPrint = field.TakeCardByPlayerPos(curPlayer).DoActionIfArrived(field, curPlayer);
                JustOutput.PrintText(messageToPrint);
            }

            if (IsPlayerGoOut(curPlayer)) {
                playersInGame.RemoveAt(curIndexPlayerTurn);
                curIndexPlayerTurn--;
                if (playersInGame.Count == 1) {
                    isGameEnd = true;
                    JustOutput.Congratulations(playersInGame[0], field);
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
        return OutputPhrases.TextStartTurn(player, field, startCapital);
    }

    private void PlayerTurnWithDice(Player player) {
        int curPlayerArr = player.positionInField.arrayIndex;
        int curPlayerCell = player.positionInField.cellIndex;
        
        JustOutput.PrintText(OutputPhrases.TextRollDice(player));
        Interactive.PressEnter();
        int randTurnsAmount = RollDice();
        JustOutput.PrintText(OutputPhrases.TextDiceNumber(randTurnsAmount));

        int newPlayerCell = curPlayerCell + randTurnsAmount;
        
        if (curPlayerCell < indexOfWorkCell && newPlayerCell >= indexOfWorkCell) {
            JustOutput.PrintText(OutputPhrases.TextGainSalary(player, salary));
            player.moneyAmount += salary;
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

    private bool IsPlayerGoOut(Player player) {
        if (player.moneyAmount >= 0) {
            return false;
        }
        JustOutput.PrintText(OutputPhrases.TextYouMustPawnEnterprises());
        
        List<Enterprise> enterprises = player.GetPawnedOrNotPlayerEnterprises(field, false);
        while (enterprises.Count > 0 && player.moneyAmount < 0) {
            JustOutput.PrintDebtAndUnPawnEnterprises(player, enterprises);

            JustOutput.PrintText(OutputPhrases.TextInputEnterpriseNum(enterprises.Count));
            int enterpriseToPawn = Convert.ToInt32(Interactive.GetPersonChoice(JustOutput.MakeAListFromDiapasone(1, enterprises.Count))) - 1;

            enterprises[enterpriseToPawn].PawnInBank(field);
            enterprises.RemoveAt(enterpriseToPawn);

            if (player.moneyAmount < 0) {
                JustOutput.PrintText(OutputPhrases.TextPawnToNotLost(player, "noEnough"));
            }
        }

        if (player.moneyAmount >= 0) {
            JustOutput.PrintText(OutputPhrases.TextPawnToNotLost(player, "backInGame"));
            return false;
        }

        JustOutput.PrintText(OutputPhrases.TextPawnToNotLost(player, "lost"));
        player.FreeAllEnterprises(field); // Tut.
        return true;
    }

    private void PreTurnThings(Player player, List<Player> playersInGame) {
        player.MakeTurnForPawnedEnter(field);
        JustOutput.PrintPlayersInfo(playersInGame, field);
        PawnEnterpriseOrBuildHotel(player);
    }
    private void PawnEnterpriseOrBuildHotel(Player player) {
        List<Enterprise> notPawnedEnterprises = player.GetPawnedOrNotPlayerEnterprises(field, false);
        List<Enterprise> pawnedEnterprises = player.GetPawnedOrNotPlayerEnterprises(field, true);
        List<Enterprise> enterprisesToBuildHotel = player.GetFullIndustryWithoutNHotelsEnterprises(field);
        bool isContinue = true;

        JustOutput.PrintText(OutputPhrases.TextPreTurnMainOutput(player, notPawnedEnterprises.Count, pawnedEnterprises.Count, enterprisesToBuildHotel.Count));

        do {
            JustOutput.PrintText(OutputPhrases.TextGetNumOfPreTurnAction());
            string actionNum = Interactive.GetPersonChoice(JustOutput.MakeAListFromDiapasone(0, 3));
            switch (actionNum) {
                case "1":
                    if (notPawnedEnterprises.Count == 0) {
                        JustOutput.PrintText(OutputPhrases.TextNoEnterprisesFor("pawn"));
                    }
                    else {
                        JustOutput.PrintEnterprises(notPawnedEnterprises, "notPawned");
                        JustOutput.PrintText(OutputPhrases.TextInputEnterpriseNum(notPawnedEnterprises.Count));
                        int enterpriseNum = Convert.ToInt32(Interactive.GetPersonChoice(JustOutput.MakeAListFromDiapasone(1, notPawnedEnterprises.Count))) - 1;

                        notPawnedEnterprises[enterpriseNum].PawnInBank(field);
                        notPawnedEnterprises.RemoveAt(enterpriseNum);
                    }
                    break;
                case "2":
                    if (pawnedEnterprises.Count == 0) {
                        JustOutput.PrintText(OutputPhrases.TextNoEnterprisesFor("unPawn"));
                    }
                    else {
                        JustOutput.PrintEnterprises(pawnedEnterprises, "pawned");
                        JustOutput.PrintText(OutputPhrases.TextInputEnterpriseNum(pawnedEnterprises.Count));
                        int enterpriseNum = Convert.ToInt32(Interactive.GetPersonChoice(JustOutput.MakeAListFromDiapasone(1, pawnedEnterprises.Count))) - 1;

                        if (player.moneyAmount > pawnedEnterprises[enterpriseNum].priceToBuy) {
                            pawnedEnterprises[enterpriseNum].UnPawnFromBank(field);
                            pawnedEnterprises.RemoveAt(enterpriseNum);
                        }
                        else {
                            JustOutput.PrintText(OutputPhrases.TextNoMoneyForUnpawnOrBuild(true));
                        }
                    }
                    break;
                case "3":
                    if (enterprisesToBuildHotel.Count == 0) {
                        JustOutput.PrintText(OutputPhrases.TextNoEnterprisesFor("hotel"));
                    }
                    else {
                        JustOutput.PrintEnterprises(enterprisesToBuildHotel, "hotel");
                        JustOutput.PrintText(OutputPhrases.TextInputEnterpriseNum(enterprisesToBuildHotel.Count));
                        int enterpriseNum = Convert.ToInt32(Interactive.GetPersonChoice(JustOutput.MakeAListFromDiapasone(1, enterprisesToBuildHotel.Count))) - 1;

                        if (player.moneyAmount > enterprisesToBuildHotel[enterpriseNum].priceToBuildHotel) {
                            enterprisesToBuildHotel[enterpriseNum].BuildHomeInEnterprise();
                            enterprisesToBuildHotel.RemoveAt(enterpriseNum);
                        }
                        else {
                            JustOutput.PrintText(OutputPhrases.TextNoMoneyForUnpawnOrBuild(false));
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
}