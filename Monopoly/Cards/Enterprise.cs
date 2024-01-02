using System.Numerics;

namespace Monopoly.Cards; 

public class Enterprise : Card {
    public Player? owner;
    public Industry industry;
    public string title;

    public readonly int priceToBuy;
    public readonly int priceToBuildHotel;

    public int turnsToDisappearIfPawned;
    private const int turnsInGeneralToDisappIfPawned = 5;

    public bool isBuiltHotel;
    public bool isFullIndustry;
    private readonly int priceOthersPayLevel1;
    private readonly int priceOthersPayLevel2;
    private readonly int priceOthersPayLevel3;
    public int currentPriceOthersPay;

    private string[] textToShow;

    public Enterprise(int priceToBuy, Industry industry, string title, Player? owner = null,
        int turnsToDisappearIfPawned = 0, bool isBuiltHotel = false, bool isFullIndustry = false) {
        this.owner = owner;
        this.industry = industry;
        this.title = title;

        this.priceToBuy = priceToBuy;
        priceToBuildHotel = priceToBuy * 3;

        this.turnsToDisappearIfPawned = turnsToDisappearIfPawned;

        this.isBuiltHotel = isBuiltHotel;
        this.isFullIndustry = isFullIndustry;
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
    
    public void PawnInBank(Field field) {
        owner.moneyAmount += currentPriceOthersPay;
        JustOutput.OutPawnInBank(this);
        CollectOrDestroyIndustry(field, owner, false);
        currentPriceOthersPay = priceOthersPayLevel1;
        turnsToDisappearIfPawned = turnsInGeneralToDisappIfPawned;
        UpdateTextToShow();
    }

    public void UnPawnFromBank(Field field) {
        owner.moneyAmount -= priceToBuy;
        JustOutput.OutUnPawnFromBank(this);
        turnsToDisappearIfPawned = 0;
        CollectOrDestroyIndustry(field, owner, true);
        UpdateTextToShow();
    }

    public bool IsPawned() {
        return turnsToDisappearIfPawned > 0;
    }

    public void ClearEnterprise() { // Tut?
        owner = null;
        turnsToDisappearIfPawned = 0;
        isBuiltHotel = false;
        isFullIndustry = false;
        currentPriceOthersPay = priceOthersPayLevel1;
        UpdateTextToShow();
    }

    public void ReduceTurnsAmount() {
        turnsToDisappearIfPawned--;
        UpdateTextToShow();
    }

    public void BuildHomeInEnterprise() {
        isBuiltHotel = true;
        currentPriceOthersPay = priceOthersPayLevel3;
        owner.moneyAmount -= priceToBuildHotel;
        Console.WriteLine("Тепер " + owner.nameInGame + " може відпочивати у своєму будинку біля " + title + ", якщо буде тут проїздом");
        UpdateTextToShow();
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
            if (IsPawned()) {
                return player.nameInGame + " щастить, так як картка на даний момент закладена у банк";
            }
            else if (owner.IsInPrison())
            {
                return player.nameInGame + " щастить, власник картки зараз у тюрмі";
            }
            else {
                player.moneyAmount -= currentPriceOthersPay;
                this.owner.moneyAmount += currentPriceOthersPay;
                return player.nameInGame + " заходить не на свою територію, тому передає " + currentPriceOthersPay +
                       " гривень гравцю " + this.owner.nameInGame;
            }
        }

        if (player.moneyAmount < priceToBuy) {
            return "Грошей на придбання " + title + " (" + priceToBuy + " гривень) у " + player.nameInGame + " немає, тому йдемо далі.";
        }
        int playerChoice = GetPersonChoice(player);
        if (playerChoice == 1) {
            BuyingCard(field, player);
            return title + " придбано гравцем " + player.nameInGame + "! Вітаємо!";
        }

        return player.nameInGame + " стримує емоції та зберігає гроші на майбутні інвестиції";
    }
    
    private int GetPersonChoice(Player player) {
        string? inputStr = null;

        Console.WriteLine(player.nameInGame + " зараз може купити " + title + " з індустрії " + industry.industryName +
                          " за " + priceToBuy + " гривень");
        Console.WriteLine("Потрібно зробити вибір:");
        Console.WriteLine("  1. Придбати підприємство та отримувати з інших багато грошей");
        Console.WriteLine("  2. Залишити підприємство на покупку іншим і потім платити їм");

        do {
            if (inputStr != null) {
                Console.WriteLine("Спробуйте ще раз.");
            }

            Console.Write("Ваш вибір: ");
            inputStr = Console.ReadLine();
        } while (!(inputStr is "1" or "2"));

        return Convert.ToInt32(inputStr);
    }

    private void UpdateTextToShow() {
        textToShow = new[] {
            title,
            industry.industryName,
            priceOthersPayLevel1 + "/" + priceOthersPayLevel2 + "/" + priceOthersPayLevel3,
            "До сплати: " + ((owner == null) ? 0 : currentPriceOthersPay),
            (owner == null) ? "Власника немає" : owner.nameInGame,
            (this.IsPawned()) ? "Заклали (ще " + turnsToDisappearIfPawned + ")" : "Не закладено"
        };
    }

    private void BuyingCard(Field field, Player player) {
        player.moneyAmount -= priceToBuy;
        this.owner = player;

        CollectOrDestroyIndustry(field, player, true);
        UpdateTextToShow();
    }

    private void UpdateIfFullIndustry() {
        currentPriceOthersPay = priceOthersPayLevel2;
        isFullIndustry = true;
        UpdateTextToShow();
    }
    private void UpdateIfDestroyedIndustry() {
        currentPriceOthersPay = priceOthersPayLevel1;
        isBuiltHotel = false;
        isFullIndustry = false;
        UpdateTextToShow();
    }

    private void CollectOrDestroyIndustry(Field field, Player player, bool isToCollect) {
        bool isFullIndustryCur = true;
        foreach (var enterprise in industry.GetEnterprisesInIndustry(field)) {
            if (enterprise.owner != player || enterprise.IsPawned()) {
                isFullIndustryCur = false;
            }
        }

        if (isFullIndustryCur) {
            if (isToCollect) {
                Console.WriteLine(player.nameInGame + " стає локальним монополістом в індустрії " + industry.industryName + "!\n");
            }
            else {
                Console.WriteLine(player.nameInGame + " втрачає локальну монополію в індустрії " + industry.industryName + "\n");
            }
            foreach (var enterprise in industry.GetEnterprisesInIndustry(field)) {
                if (isToCollect) {
                    enterprise.UpdateIfFullIndustry();
                }
                else {
                    enterprise.UpdateIfDestroyedIndustry();
                }
            }
        }
    }
}