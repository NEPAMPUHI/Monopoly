namespace Monopoly; 

public class Industry {
    private ConsoleColor color;
    private List<Enterprise> enterprises;
    private bool isFull;

    public Industry(ConsoleColor color, List<Enterprise> enterprises, bool isFull = false) {
        this.color = color;
        this.enterprises = enterprises;
        this.isFull = isFull;
    }
}