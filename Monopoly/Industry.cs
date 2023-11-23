namespace Monopoly; 

public class Industry {
    public List<Pair> enterprisesIndexes;
    public string industryName;
    public ConsoleColor color;
    public bool isFull;

    public Industry(List<Pair> enterprisesIndexes, string industryName, ConsoleColor color = ConsoleColor.Cyan,
        bool isFull = false) {
        this.color = color;
        this.enterprisesIndexes = enterprisesIndexes;
        this.industryName = industryName;
        this.isFull = isFull;
    }
}