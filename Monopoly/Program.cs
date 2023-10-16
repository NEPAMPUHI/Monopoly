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
    private MainMenu inMenu;

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
        Game game = new Game();
        for (int i = 0; i < 10; i++) {
            Console.WriteLine(game.RollDice());
            Console.WriteLine(game.RollCoin());
        }
    }

    internal void PlayWithFriends() {
        Console.WriteLine("*гра з друзями*");
    }

    internal void ChangeSettings() {
        Console.WriteLine("*відкрилися налаштування*");
    }

    internal void QuitGame() {
        Console.WriteLine("*виходимо з гри*");
    }
}

struct PlayerPos {
    public int x;
    public int y;

    public PlayerPos(int x = 0, int y = 0) {
        this.x = x;
        this.y = y;
    }
}

class Player {
    public int moneyAmount;
    public string nameInGame;
    public ConsoleColor chipColor;
    public PlayerPos positionInField;
    public bool isInPrison;
    public int turnsToGoOutOfPrison;

    public Player(string nameInGame = "default", int moneyAmount = 0, PlayerPos positionInField = new PlayerPos(),
        ConsoleColor chipColor = ConsoleColor.White, bool isInPrison = false, int turnsToGoOutOfPrison = 0) {
        this.nameInGame = nameInGame;
        this.moneyAmount = moneyAmount;
        this.positionInField = positionInField;
        this.chipColor = chipColor;
        this.isInPrison = isInPrison;
        this.turnsToGoOutOfPrison = turnsToGoOutOfPrison;
    }
}

class Game {
    private Random _rand;

    public Game() {
        _rand = new Random();
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
    public int priceToBuy;
    public int priceToBuildHome;
    public int priceToPawnToBank;
    public int priceToSellToBank;
    public bool isPawnedInBank;
    public int turnsToDisappearIfPawned;
    public bool isCollectedThree;
    public bool isBuiltHome;
    public int priceToOthersPay;

    public Enterprise(int priceToBuy, Player? owner = null, bool isPawnedInBank = false, 
        int turnsToDisappearIfPawned = 0, bool isCollectedThree = false, bool isBuiltHome = false) {
        this.owner = owner;
        this.priceToBuy = priceToBuy;
        this.priceToBuildHome = priceToBuy * 3;
        this.priceToPawnToBank = priceToBuy / 2;
        this.priceToSellToBank = priceToBuy;
        this.isPawnedInBank = isPawnedInBank;
        this.turnsToDisappearIfPawned = turnsToDisappearIfPawned;
        this.isCollectedThree = isCollectedThree;
        this.isBuiltHome = isBuiltHome;
        this.priceToOthersPay = priceToBuy / 2;
    }
}