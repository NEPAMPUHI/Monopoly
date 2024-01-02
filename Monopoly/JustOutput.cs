using Monopoly.Cards;
using System.Numerics;

namespace Monopoly; 

public static class JustOutput { // Максимум в ширину — 186 символів
    private const int maxCellsInOneLine = 8;
    private const int maxSymbolsInOneCell = 16;
    public static void PrintAllField(Field field) {
        for (int i = 0; i < field.fieldArrays.Length; i++) {
            Console.WriteLine("|" + field.countriesArray[i] + "|");
            for (int k = 0; k < field.fieldArrays[i].Length; k++) {
                Console.WriteLine("------------------------------");
                PrintACell(field.fieldArrays[i][k]);
            }
            Console.WriteLine("------------------------------");
            Console.WriteLine("______________________________________________________________________\n");
        }
    }

    public static void PrintAListOfEnterprisesInOneLine(List<Enterprise> enterprises) {
        string[][] enterprisesInLines = new string[enterprises.Count][];
        int curBoard;
        
        for (int i = 0; i < enterprisesInLines.Length; i++) {
            enterprisesInLines[i] = enterprises[i].TextToPrintInAField;
        }

        for (int i = 0; i < enterprisesInLines.Length; i += maxCellsInOneLine) {
            curBoard = Math.Min(enterprisesInLines.Length - i, maxCellsInOneLine);
            Console.WriteLine(CurWideLine(Math.Min(curBoard, maxCellsInOneLine), false));
            for (int h = 0; h < enterprisesInLines[0].Length; h++) {
                for (int k = 0; k < curBoard; k++) {
                    int freeSpace = maxSymbolsInOneCell - enterprisesInLines[k + i][h].Length;
                    Console.Write("| ");
                    Console.Write(new string (' ', freeSpace / 2));
                    Console.Write(enterprisesInLines[k + i][h]);
                    Console.Write(new string (' ', freeSpace - freeSpace / 2));
                    Console.Write(" | ");
                }
                Console.Write("\n");
            }
            Console.WriteLine(CurWideLine(Math.Min(curBoard, maxCellsInOneLine), true));
            Console.WriteLine();
        }
    }

    public static void PrintAllIndustries(Field field) {
        foreach (var industry in field.industriesArray) {
            Console.WriteLine("Назва індустрії: " + industry.industryName);
            foreach (var pos in industry.enterprisesIndexes) {
                Console.WriteLine("------------------------------");
                PrintACell(field.fieldArrays[pos.arrayIndex][pos.cellIndex]);
            }
            Console.WriteLine("------------------------------");
            Console.WriteLine("______________________________________________________________________\n");
        }
    }

    private static void PrintACell(Card cell) {
        for (int i = 0; i < cell.TextToPrintInAField.Length; i++) {
            Console.Write("    ");
            Console.WriteLine(cell.TextToPrintInAField[i]);
        }
    }

    public static string PrintCellTitleInAText(Card? card) {
        return ((card is null) ? 
            "<СТАРТ>" :
            (card is Enterprise enterprise)
            ? enterprise.title + " (" + enterprise.industry.industryName + ")"
            : card.TextToPrintInAField[0]);
    }

    public static string GetCountryNameByPlayer(Field field, Player player) {
        string ans;
        if (player.positionInField == null) {
            ans = "космосі";
        }
        else if (player.positionInField.cellIndex > field.specialIndexesByCellNames["ExitChance"]) {
            ans = "міжкраїнному просторі";
        }
        else {
            ans = field.countriesArray[player.positionInField.arrayIndex];
        }

        return ans;
    }

    private static string CurWideLine(int curEnterprisesAmount, bool withSideBoards) {
        string strToReturn = "";
        string strToDuplicate = "";

        strToDuplicate += withSideBoards ? '|' : ' ';
        for (int i = 0; i < maxSymbolsInOneCell + 2; i++) {
            strToDuplicate += '_';
        }
        strToDuplicate += withSideBoards ? '|' : ' ';
        strToDuplicate += ' ';

        for (int i = 0; i < curEnterprisesAmount; i++) {
            strToReturn += strToDuplicate;
        }

        return strToReturn;
    }

    public static void OutWorkChoice(Player player) {
        Console.WriteLine("До завершення роботи " + player.nameInGame + " має відробити ще " +
        player.turnsCanContinueWork + " " + DayEnding(player.turnsCanContinueWork) +
                          ". Гравець може відробити день на роботі або піти з неї.");
        Console.WriteLine("Потрібно зробити вибір:");
        Console.WriteLine("  1. Провести ще день на нудній роботі");
        Console.WriteLine("  2. Піти далі досліджувати простори країни");
    }

    public static void OutPawnInBank(Enterprise enterprise) {
        Console.WriteLine(enterprise.title + " закладено у банк. " + enterprise.owner.nameInGame + " отримує " +
            enterprise.currentPriceOthersPay + " на свій рахунок\n");
    }
    public static void OutUnPawnFromBank(Enterprise enterprise) {
        Console.WriteLine(enterprise.title + " викуплено з банку гравцем " + enterprise.owner.nameInGame + "!\n");
    }

    public static void OutBuildHomeInEnterprise(Enterprise enterprise) {
        Console.WriteLine(enterprise.title + " викуплено з банку гравцем " + enterprise.owner.nameInGame + "!\n");
    }

    private static string HryvnaEnding(int turnsAmount) {
        turnsAmount %= 10;
        return (turnsAmount == 1) ? "гривня" : ((turnsAmount is > 1 and < 5) ? "гривні" : "гривень");
    }

    private static string DayEnding(int turnsAmount) {
        turnsAmount %= 10;
        return (turnsAmount == 1) ? "день" : ((turnsAmount is > 1 and < 5) ? "дні" : "днів");
    }
}