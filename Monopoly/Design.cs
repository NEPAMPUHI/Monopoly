using Monopoly.Cards;

namespace Monopoly; 

public class Design { // Максимум в ширину — 186 символів
    
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
}