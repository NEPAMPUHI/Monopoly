namespace Monopoly; 

public class MainMenu {
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
            Console.WriteLine(GamePlay.RollDice());
            Console.WriteLine(GamePlay.RollCoin());
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