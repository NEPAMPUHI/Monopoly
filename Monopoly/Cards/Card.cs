namespace Monopoly.Cards;
using Monopoly.OutputDesign;

public abstract class Card {
    public abstract string[] TextToPrintInAField { get; }
    public abstract string DoActionIfArrived(Field field, Player player);
    public abstract string DoActionIfStayed(Field field, Player player, out bool isNextMoveNeed);
    private protected string JustTurn(Field field, Player player, out bool isNextMoveNeed) {
        isNextMoveNeed = true;
        return player.nameInGame + " ходить.";
    }
}