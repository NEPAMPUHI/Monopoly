using System.Text;

class Program {
    static void Main(string[] args) {
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        var enc1251 = Encoding.GetEncoding(1251);

        System.Console.OutputEncoding = System.Text.Encoding.UTF8;
        System.Console.InputEncoding = enc1251;

        App app = new App();
        app.Run();
    }
}

class App {
    private readonly MainMenu inMenu;

    public App() {
        inMenu = new MainMenu();
    }

    public void Run() {
        var shouldContinue = true;
        while (shouldContinue) {
            inMenu.DisplayMenu();
            inMenu.ChooseAction(ref shouldContinue);
        }
    }
}

class MainMenu {
    internal void DisplayMenu() {
        Console.WriteLine("Головне меню:\n" +
                          "1. Гра з комп'ютером\n" +
                          "2. Гра з друзями\n" +
                          "3. Налаштування\n" +
                          "4. Вихід з гри");
    }

    internal void ChooseAction(ref bool shouldContinue) {
        Console.Write("Введіть номер команди: ");
        var userChoice = Console.ReadLine();
        switch (userChoice) {
            case "1":
                PlayWithComputer();
                break;
            case "2":
                PlayWithFriends();
                break;
            case "3":
                ChangeSettings();
                break;
            case "4":
                QuitGame();
                shouldContinue = false;
                break;
            default:
                Console.WriteLine("Спробуйте ще раз ^_^");
                break;
        }
    }

    internal void PlayWithComputer() {
        Console.WriteLine("*гра з комп'ютером*");
        GamePlay game = new GamePlay();
        for (int i = 0; i < 10; i++) {
            Console.WriteLine(game.RollDice());
            Console.WriteLine(game.RollCoin());
        }
    }

    internal void PlayWithFriends() {
        Console.WriteLine("*гра з друзями*");

        GamePlay game = new GamePlay();
        bool isContinue = true;
        int playersAmount = 0;
        Player[] players = new Player[0];
        string choice;
        string playerName;

        Console.WriteLine("Ви можете додати від 2 до 4 гравців.");
        while (isContinue) {
            Console.Write("Введіть 1, якщо хочете додати друга для гри або 0, якщо більше не хочете: ");
            choice = Console.ReadLine();
            switch (choice) {
                case "1":
                    int arrayLength = players.Length;
                    Player[] newPlayers = new Player[arrayLength + 1];
                    for (int i = 0; i < arrayLength; i++) {
                        newPlayers[i] = players[i];
                    }

                    players = newPlayers;
                    Console.Write($"Введіть ім'я гравця під номером {playersAmount + 1}: ");
                    playerName = Console.ReadLine();
                    players[arrayLength] = new Player(playerName, chipColor: (ConsoleColor)(arrayLength + 1));
                    playersAmount++;
                    if (playersAmount == 4) {
                        isContinue = false;
                    }

                    break;
                case "0":
                    if (playersAmount < 2) {
                        Console.WriteLine("У грі мають брати участь не менше 2 людей, додайте гравців");
                    }
                    else {
                        isContinue = false;
                    }

                    break;
                default:
                    Console.WriteLine("Спробуйте ще раз ^_^");
                    break;
            }
        }

        ConsoleColor defaltColor = Console.ForegroundColor;
        Console.WriteLine("Чудово! Ось ваш список гравців:");
        for (int i = 0; i < players.Length; i++) {
            Console.Write($"Гравець {i + 1}: ");
            Console.ForegroundColor = players[i].chipColor;
            Console.WriteLine(players[i].nameInGame);
            Console.ForegroundColor = defaltColor;
        }
    }

    internal void ChangeSettings() {
        Console.WriteLine("*відкрилися налаштування*");
    }

    internal void QuitGame() {
        Console.WriteLine("*виходимо з гри*");
    }
}

struct Position {
    public int x;
    public int y;

    public Position(int x = 0, int y = 0) {
        this.x = x;
        this.y = y;
    }
}

class Player {
    public int moneyAmount;
    public readonly string nameInGame;
    public readonly ConsoleColor chipColor;
    public Position positionInField;
    public bool isInPrison;
    public int turnsToGoOutOfPrison;

    public Player(string nameInGame, int moneyAmount = 0, Position positionInField = new Position(),
        ConsoleColor chipColor = ConsoleColor.White, bool isInPrison = false, int turnsToGoOutOfPrison = 0) {
        this.nameInGame = nameInGame;
        this.moneyAmount = moneyAmount;
        this.positionInField = positionInField;
        this.chipColor = chipColor;
        this.isInPrison = isInPrison;
        this.turnsToGoOutOfPrison = turnsToGoOutOfPrison;
    }
}

class GamePlay {
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
                    allEnterprises[i][k] = new Enterprise(_rand.Next(1, 1001));
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

class Enterprise {
    public Player? owner;

    private readonly int priceToBuy;
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

    public Enterprise(int priceToBuy, Player? owner = null, bool isPawnedInBank = false,
        int turnsToDisappearIfPawned = 0, bool isBuiltHome = false) {
        this.owner = owner;

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

class Industry {
    private ConsoleColor color;
    private List<Enterprise> enterprises;
    private bool isFull;

    public Industry(ConsoleColor color, List<Enterprise> enterprises, bool isFull = false) {
        this.color = color;
        this.enterprises = enterprises;
        this.isFull = isFull;
    }
}

class Card {
    public void performAction() { //void??
        
    }
}
// hi