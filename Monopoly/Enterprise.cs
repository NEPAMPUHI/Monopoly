namespace Monopoly; 

public class Enterprise {
    public Player? owner;
    public Industry industry;
    public string title;

    public readonly int priceToBuy;
    private readonly int priceToBuildHotel;
    private readonly int priceToPawnToBank;
    private readonly int priceToSellToBank;

    public bool isPawnedInBank;
    public int turnsToDisappearIfPawned;

    public bool isBuiltHotel;
    private readonly int priceOthersPayLevel1;
    private readonly int priceOthersPayLevel2;
    private readonly int priceOthersPayLevel3;
    public int currentPriceOthersPay;

    public Enterprise(int priceToBuy, Industry industry, string title, Player? owner = null, bool isPawnedInBank = false,
        int turnsToDisappearIfPawned = 0, bool isBuiltHome = false) {
        this.owner = owner;
        this.industry = industry;
        this.title = title;

        this.priceToBuy = priceToBuy;
        priceToBuildHotel = priceToBuy * 3;
        priceToPawnToBank = priceToBuy / 2;
        priceToSellToBank = priceToBuy;

        this.isPawnedInBank = isPawnedInBank;
        this.turnsToDisappearIfPawned = turnsToDisappearIfPawned;

        isBuiltHotel = isBuiltHome;
        priceOthersPayLevel1 = priceToBuy / 2;
        priceOthersPayLevel2 = priceToBuy;
        priceOthersPayLevel3 = priceToBuy * 2;
        currentPriceOthersPay = priceOthersPayLevel1;
    }
}