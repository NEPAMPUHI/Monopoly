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
        GamePlay game = new GamePlay();
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
    public readonly string nameInGame;
    public readonly ConsoleColor chipColor;
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

class GamePlay {
    private Random _rand;

    public GamePlay() {
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