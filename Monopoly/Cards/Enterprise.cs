namespace Monopoly.Cards; 

public class Enterprise : Card {
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

    private string[] textToShow;

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

        UpdateTextToShow();
    }
    
    public string[] TextToPrintInAField {
        get { return textToShow; } 
    }
    
    public string DoActionIfArrived(Field field, Player player) {
        return PayBuyOrStay(field, player);
    }

    public string DoActionIfStayed(Field field, Player player, out bool isNextMoveNeed) {
        return JustTurn(field, player, out isNextMoveNeed);
    }

    private string JustTurn(Field field, Player player, out bool isNextMoveNeed) {
        isNextMoveNeed = true;
        return player.nameInGame + " ходить.";
    }

    private string PayBuyOrStay(Field field, Player player) {
        if (this.owner == player) {
            return "Яке щастя! У себе вдома " + player.nameInGame + " не зобов'язується платити комусь!";
        }

        if (this.owner != null) {
            player.moneyAmount -= currentPriceOthersPay;
            this.owner.moneyAmount += currentPriceOthersPay;
            return player.nameInGame + " заходить не на свою територію, тому передає " + currentPriceOthersPay +
                   " гривень гравцю " + this.owner.nameInGame;
        }

        int playerChoice = GetPersonChoice(player);
        if (playerChoice == 1) {
            player.moneyAmount -= priceToBuy;
            this.owner = player;
            UpdateTextToShow();
            return title + " придбано гравцем " + player.nameInGame + "! Вітаємо!";
        }

        return player.nameInGame + " стримує емоції та зберігає гроші на майбутні інвестиції";
    }
    
    private int GetPersonChoice(Player player) {
        string? inputStr = null;

        Console.WriteLine(player.nameInGame + " зараз може купити " + title + " з індустрії " + industry.industryName);
        Console.WriteLine("Потрібно зробити вибір:");
        Console.WriteLine("  1. Придбати підприємство та отримувати з інших багато грошей");
        Console.WriteLine("  2. Залишити підприємство на покупку іншим і потім платити їм");

        do {
            if (inputStr != null) {
                Console.WriteLine("Спробуйте ще раз.");
            }

            Console.Write("Ваш вибір: ");
            inputStr = Console.ReadLine();
        } while (inputStr is "1" or "2");

        return Convert.ToInt32(inputStr);
    }

    public void UpdateTextToShow() {
        textToShow = new[] {
            title,
            industry.industryName,
            priceOthersPayLevel1 + "/" + priceOthersPayLevel2 + "/" + priceOthersPayLevel3,
            (owner == null) ? "Власника немає" : owner.nameInGame
        };
    }
}