using Monopoly.Cards;

namespace Monopoly; 

public class Design { // Максимум в ширину — 186 символів
    private const int maxCellsInOneLine = 8;
    private const int maxSymbolsInOneCell = 16;
    public void PrintAllField(Field field) {
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

    public void PrintAListOfEnterprisesInOneLine(List<Enterprise> enterprises) {
        string[][] enterprisesInLines = new string[enterprises.Count][];
        int curBoard;
        
        for (int i = 0; i < enterprisesInLines.Length; i++) {
            enterprisesInLines[i] = enterprises[i].TextToPrintInAField;
        }

        for (int i = 0; i < enterprisesInLines.Length; i += maxCellsInOneLine) {
            curBoard = Math.Min(enterprisesInLines.Length - i, maxCellsInOneLine);
            Console.WriteLine(CurWideLine(Math.Min(curBoard, maxCellsInOneLine), false));
            for (int h = 0; h < enterprisesInLines[0].Length; h++) {
                for (int k = i; k < curBoard; k++) {
                    int freeSpace = maxSymbolsInOneCell - enterprisesInLines[k][h].Length;
                    Console.Write("| ");
                    Console.Write(new string (' ', freeSpace / 2));
                    Console.Write(enterprisesInLines[k][h]);
                    Console.Write(new string (' ', freeSpace - freeSpace / 2));
                    Console.Write(" | ");
                }
                Console.Write("\n");
            }
            Console.WriteLine(CurWideLine(Math.Min(curBoard, maxCellsInOneLine), true));
            Console.WriteLine();
        }
    }
    
    public void PrintAllIndustries(Field field) {
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

    private void PrintACell(Card cell) {
        for (int i = 0; i < cell.TextToPrintInAField.Length; i++) {
            Console.Write("    ");
            Console.WriteLine(cell.TextToPrintInAField[i]);
        }
    }

    public string PrintCellTitleInAText(Card? card) {
        return ((card is null) ? 
            "<СТАРТ>" :
            (card is Enterprise enterprise)
            ? enterprise.title + " (" + enterprise.industry.industryName + ")"
            : card.TextToPrintInAField[0]);
    }

    public string GetCountryNameByPlayer(Field field, Player player) {
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

    private string CurWideLine(int curEnterprisesAmount, bool withSideBoards) {
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
}