namespace Monopoly.Cards; 

public class Rewiew : Card {
    
    public string[] TextToPrintInAField {
        get { return new[] { "<ОГЛЯД>" }; } 
    }
    public string DoActionIfArrived(Field field, Player player, out bool isNextMoveNeed) {
        return GoToPrisonOrNot(field, player, out isNextMoveNeed);
    }
    
    public string DoActionIfStayed(Field field, Player player, out bool isNextMoveNeed) {
        return JustTurn(field, player, out isNextMoveNeed);
    }

    private string JustTurn(Field field, Player player, out bool isNextMoveNeed) {
        isNextMoveNeed = true;
        return player.nameInGame + " ходить.";
    }

    private string GoToPrisonOrNot(Field field, Player player, out bool isNextMoveNeed) {
        isNextMoveNeed = false;
        bool isGoToPrison = GamePlay.RollCoin();

        if (!isGoToPrison) {
            return player.nameInGame + " відкупляється від перевіряючих";
        }

        int prisonIndex = field.specialIndexesByCellNames["Prison"];
        player.positionInField.cellIndex = prisonIndex;
        field.fieldArrays[player.positionInField.arrayIndex][prisonIndex]
            .DoActionIfArrived(field, player, out isNextMoveNeed);
        return player.nameInGame + " не встигає сховати контрабанду та відправляється до тюрми!";
    }
}