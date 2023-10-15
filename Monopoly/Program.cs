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
                Console.WriteLine("Ало дебил");
                break;
        }
    }

    internal void PlayWithComputer() {
        Console.WriteLine("*гра з комп'ютером*");
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