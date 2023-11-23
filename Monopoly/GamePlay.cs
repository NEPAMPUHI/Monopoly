namespace Monopoly; 

public class GamePlay {
    private readonly Random _rand;
    public Enterprise[][] allEnterprises;

    public GamePlay() {
        _rand = new Random();
        FillStartEnterprises(ref allEnterprises);
    }

    public void FillStartEnterprises(ref Enterprise[][] allEnterprises) {
        int fieldLength = _rand.Next(1, 11);
        allEnterprises = new Enterprise[fieldLength][];
        for (int i = 0; i < fieldLength; i++) {
            allEnterprises[i] = new Enterprise[fieldLength];
            for (int k = 0; k < fieldLength; k++) {
                if (RollCoin()) {
                    //allEnterprises[i][k] = new Enterprise(_rand.Next(1, 1001), "");
                }
                else {
                    allEnterprises[i][k] = null;
                }
            }
        }
    }

    public int RollDice() {
        return _rand.Next(1, 7);
    }

    public bool RollCoin() {
        return Convert.ToBoolean(_rand.Next(0, 2));
    }
}