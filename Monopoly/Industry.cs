using Monopoly.Cards;

namespace Monopoly; 

public class Industry {
    public List<Position> enterprisesIndexes;
    public string industryName;
    public ConsoleColor color;
    public bool isFull;

    public Industry(List<Position> enterprisesIndexes, string industryName, ConsoleColor color = ConsoleColor.Cyan,
        bool isFull = false) {
        this.color = color;
        this.enterprisesIndexes = enterprisesIndexes;
        this.industryName = industryName;
        this.isFull = isFull;
    }

    List<Enterprise> GetEnterprisesInIndustry(Field field) {
        List<Enterprise> ans = new List<Enterprise>();
        
        foreach (var pos in enterprisesIndexes) {
            ans.Add(field.fieldArrays[pos.arrayIndex][pos.cellIndex] as Enterprise);
        }

        return ans;
    }
}