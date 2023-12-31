namespace Monopoly; 

public class MainMenu {
    private const int minPlayersAmount = 2;
    private const int maxPlayersAmount = 4;
    private const int maxPlayerNameLength = 16;
    internal void DisplayMenu() {
        Console.WriteLine("Головне меню:\n" +
                          "1. Гра з комп'ютером\n" +
                          "2. Гра з друзями\n" +
                          "3. Налаштування\n" +
                          "4. Вихід з гри");
    }
    internal void ChooseAction(ref bool shouldContinue) {
        Console.Write("Введіть номер команди: ");
        switch (Console.ReadLine()) {
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
                QuitGame(ref shouldContinue);
                break;
            default:
                Console.WriteLine("Спробуйте ще раз ^_^");
                break;
        }
    }
    private void PlayWithComputer() {
        Console.WriteLine("\n*гра з комп'ютером*");
        GamePlay game = new GamePlay();
        for (int i = 0; i < 10; i++) {
            Console.WriteLine(GamePlay.RollDice());
            Console.WriteLine(GamePlay.RollCoin());
        }
    }
    private void PlayWithFriends() {
        Console.WriteLine("\n*гра з друзями*");
        string[] playerNames = InputPlayersToPlay();
        ConsoleColor[] playerColors = ChooseColorForEach(playerNames.Length);
        
        Player[] players = new Player[playerNames.Length];
        for (int i = 0; i < players.Length; i++) {
            players[i] = new Player(playerNames[i], playerColors[i]);
        }
        
        GamePlay game = new GamePlay();
        game.StartGameWithFriends(players);
        
    }

    private string[] InputPlayersToPlay() {
        bool isContinue = true;
        int playersAmount = 0;

        string[] previousRes = new string[maxPlayersAmount];

        Console.WriteLine("\nВи можете додати від 2 до 4 гравців.");
        while (isContinue) {
            Console.Write("\nВведіть 1, якщо хочете додати друга для гри або 0, якщо більше не хочете: ");
            
            switch (Console.ReadLine()) {
                case "1":
                    previousRes[playersAmount] = AddNamePlayer(previousRes, playersAmount);
                    playersAmount++;
                    if (playersAmount == maxPlayersAmount) {
                        isContinue = false;
                    }
                    break;
                case "0":
                    if (playersAmount < minPlayersAmount) {
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

        string[] res = new string[playersAmount];
        Array.Copy(previousRes, 0, res, 0, playersAmount);
        return res;
        // ConsoleColor defaltColor = Console.ForegroundColor;
        // Console.WriteLine("Чудово! Ось ваш список гравців:");
        // for (int i = 0; i < players.Length; i++) {
        //     Console.Write($"Гравець {i + 1}: ");
        //     Console.ForegroundColor = players[i].chipColor;
        //     Console.WriteLine(players[i].nameInGame);
        //     Console.ForegroundColor = defaltColor;
        // }
    }

    private ConsoleColor[] ChooseColorForEach(int playersAmount) {
        int[] colorIndexes = new int[playersAmount];
        ConsoleColor[] colors = new ConsoleColor[playersAmount];
        int curIndex = 0;
        int curColorIndex;

        while (curIndex != playersAmount) {
            curColorIndex = App.rand.Next(16);
            for (int i = 0; i < curIndex; i++) {
                if (colorIndexes[i] == curColorIndex) {
                    curColorIndex = -1;
                }
            }
            if (curColorIndex != -1) {
                colorIndexes[curIndex] = curColorIndex;
                curIndex++;
            }
        }
        
        for (int i = 0; i < playersAmount; i++) {
            colors[i] = (ConsoleColor)colorIndexes[i];
        }

        return colors;
    }

    private string AddNamePlayer(string[] playerNames, int playersAmount) {
        string name;
        bool isCorrect;

        do {
            isCorrect = true;
            Console.Write($"Введіть ім'я гравця під номером {playersAmount + 1}: ");
            name = Console.ReadLine();

            if (name.Length > 15) {
                Console.WriteLine("Ім'я надто велике, спробуйте ще");
                isCorrect = false;
                continue;
            }
            
            if (name.Length == 0) {
                Console.WriteLine("Ваше ім'я навіть під мікроскопом не видно, спробуйте ще");
                isCorrect = false;
                continue;
            }

            for (int i = 0; i < playersAmount; i++) {
                if (name == playerNames[i]) {
                    Console.WriteLine("Однакових імен у гравців бути не може, спробуйте ще");
                    isCorrect = false;
                }
            }
        } while (!isCorrect);

        return name;
    }
    
    private void ChangeSettings() {
        Console.WriteLine("\n*відкрилися налаштування*");
    }
    
    private void QuitGame(ref bool shouldContinue) {
        shouldContinue = false;
        Console.WriteLine("Сподіваємось, ви гарно провели час! Бувайте!");
    }
}