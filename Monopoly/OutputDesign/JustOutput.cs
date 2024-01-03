using Monopoly.Cards;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace Monopoly.OutputDesign;

public static class JustOutput { // Максимум в ширину — 186 символів
    public static readonly int maxCellsInOneLine = 8;
    public static readonly int maxSymbolsInOneCell = 16;
    public static readonly int screenWidth = 186;

    public static void PrintAListOfEnterprisesInOneLine(List<Enterprise> enterprises) {
        string[][] enterprisesInLines = new string[enterprises.Count][];
        int curBoard;

        for (int i = 0; i < enterprisesInLines.Length; i++) {
            enterprisesInLines[i] = enterprises[i].TextToPrintInAField;
        }

        for (int i = 0; i < enterprisesInLines.Length; i += maxCellsInOneLine) {
            curBoard = Math.Min(enterprisesInLines.Length - i, maxCellsInOneLine);
            Console.WriteLine(OutputPhrases.CurWideLine(Math.Min(curBoard, maxCellsInOneLine), false));
            for (int h = 0; h < enterprisesInLines[0].Length; h++) {
                for (int k = 0; k < curBoard; k++) {
                    int freeSpace = maxSymbolsInOneCell - enterprisesInLines[k + i][h].Length;
                    Console.Write("| ");
                    Console.Write(new string(' ', freeSpace / 2));
                    Console.Write(enterprisesInLines[k + i][h]);
                    Console.Write(new string(' ', freeSpace - freeSpace / 2));
                    Console.Write(" | ");
                }
                Console.Write("\n");
            }
            Console.WriteLine(OutputPhrases.CurWideLine(Math.Min(curBoard, maxCellsInOneLine), true));
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

    public static void PrintText(string textToPrint) {
        Thread.Sleep(500);
        Console.WriteLine(textToPrint);
    }

    public static void Congratulations(Player player, Field field) {
        Console.WriteLine("\n" + new string('-', screenWidth));
        Console.WriteLine(OutputPhrases.TextPlayerGreetings(player));
        PrintAListOfEnterprisesInOneLine(player.GetAllPlayerEnterprises(field));
        Console.WriteLine(new string('-', screenWidth) + "\n");
    }

    public static void PrintPlayersInfo(List<Player> playersInGame, Field field) {
        Console.WriteLine();
        foreach (var player in playersInGame) {
            Console.WriteLine(OutputPhrases.TextPlayerInfo(player, field));
            PrintAListOfEnterprisesInOneLine(player.GetAllPlayerEnterprises(field));
        }
        Console.WriteLine();
    }

    public static void PrintDebtAndUnPawnEnterprises(Player player, List<Enterprise> enterprises) {
        Console.WriteLine(OutputPhrases.TextDebtAndEnter(player));
        PrintAListOfEnterprisesInOneLine(enterprises);
    }

    public static List<string> MakeAListFromDiapasone(int start, int end) {
        List<string> list = new List<string>();
        for (int i = start; i < end + 1; i++) {
            list.Add(Convert.ToString(i));
        }
        return list;
    }

    public static void PrintEnterprises(List<Enterprise> enterprises, string tag) {
        Console.WriteLine(tag switch {
            "notPawned" => OutputPhrases.TextUnPawnedOrPawnedEnterprises(false),
            "pawned" => OutputPhrases.TextUnPawnedOrPawnedEnterprises(true),
            "hotel" => OutputPhrases.TextEnterprisesToBuildHotel(),
            _ => "unknown tag"
        });
        PrintAListOfEnterprisesInOneLine(enterprises);
    }

    public static void PrintMyChoice() {
        Console.Write("Мій вибір: ");
    }

    public static void PrintAllField(Field field) {
        List<int>[][] fieldIndexes = OutputPhrases.fieldIndexes;
        int cellWidth = OutputPhrases.maxCellWidth;
        int cellHeight = OutputPhrases.cellHeight;

        Console.Write(" ");
        foreach (var list in fieldIndexes[0]) {
            if (OutputPhrases.IsNotBoard(list)) {
                Console.Write(new string(' ', cellWidth));
            }
            else {
                Console.Write(new string('_', cellWidth));
            }
            Console.Write(" ");
        }
        Console.WriteLine();
        for (int l = 0; l < fieldIndexes.Length; l++) {
            string[][] curListCells = new string[fieldIndexes[l].Length][];
            for (int i = 0; i < fieldIndexes[l].Length; i++) {
                curListCells[i] = OutputPhrases.GetCellText(field, fieldIndexes[l][i]);
            }
            for (int i = 0; i < cellHeight; i++) {
                if (OutputPhrases.IsNotBoard(fieldIndexes[l][0])) {
                    Console.Write(" ");
                }
                else {
                    Console.Write("|");
                }
                for (int k = 0; k < fieldIndexes[l].Length; k++) {
                    Console.Write(curListCells[k][i]);
                    if (!OutputPhrases.IsNotBoard(fieldIndexes[l][k])) {
                        Console.Write("|");
                    }
                    else if (k < fieldIndexes[l].Length - 1 && !OutputPhrases.IsNotBoard(fieldIndexes[l][k + 1])) {
                        Console.Write("|");
                    }
                    else {
                        Console.Write(" ");
                    }
                }
                Console.WriteLine();
            }
            if (OutputPhrases.IsNotBoard(fieldIndexes[l][0])) {
                Console.Write(" ");
            }
            else {
                Console.Write("|");
            }
            for (int i = 0; i < fieldIndexes[l].Length; i++) {
                if (OutputPhrases.IsNotBoard(fieldIndexes[l][i]) &&
                    (l == fieldIndexes.Length - 1 || OutputPhrases.IsNotBoard(fieldIndexes[l + 1][i]))) {
                    Console.Write(new string(' ', cellWidth));
                }
                else {
                    Console.Write(new string('_', cellWidth));
                }
                
                if (!OutputPhrases.IsNotBoard(fieldIndexes[l][i])) {
                    Console.Write("|");
                }
                else if (i < fieldIndexes[l].Length - 1 && !OutputPhrases.IsNotBoard(fieldIndexes[l][i + 1])) {
                    Console.Write("|");
                }
                else {
                    Console.Write(" ");
                }
            }
            Console.WriteLine();
        }
    }
}