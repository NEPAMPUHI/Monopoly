namespace Monopoly.Cards; 

public interface Card {
    public string[] TextToPrintInAField { get; }
    public string DoActionIfArrived(Field field, Player player);
    public string DoActionIfStayed(Field field, Player player, out bool isNextMoveNeed);
    internal static string JustTurn(Field field, Player player, out bool isNextMoveNeed) {
        isNextMoveNeed = true;
        return player.nameInGame + " ходить.";
    }
}