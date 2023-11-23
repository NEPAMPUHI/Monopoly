namespace Monopoly; 

public class Industry {
    public List<Enterprise> enterprises;
    public string industryName;
    public ConsoleColor color;
    public bool isFull;

    public Industry(List<Enterprise> enterprises, string industryName, ConsoleColor color = ConsoleColor.Cyan, bool isFull = false) {
        this.color = color;
        this.enterprises = enterprises;
        this.industryName = industryName;
        this.isFull = isFull;
    }
}