namespace Monopoly;

public class Position {
    public int arrayIndex;
    public int cellIndex;

    public Position(int arrayIndex = 0, int cellIndex = 0) {
        this.arrayIndex = arrayIndex;
        this.cellIndex = cellIndex;
    }
}