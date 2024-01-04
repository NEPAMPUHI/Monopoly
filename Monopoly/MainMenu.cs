using Monopoly.Cards;
using Monopoly.ComputerBots;
using Monopoly.OutputDesign;

namespace Monopoly;

public class MainMenu {
    private readonly GamePlay game;

    public MainMenu() {
        game = new GamePlay();
    }
    
    internal void DisplayMenu() {
        JustOutput.PrintText(OutputPhrases.TextMainMenu());
    }
    internal void PerformAction(ref bool shouldContinue) {
        switch (Interactive.GetPersonChoice(JustOutput.MakeAListFromDiapasone(1, 4))) {
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
        }
    }
    private void PlayWithComputer() {
        string playerName = Interactive.InputYourName();

        int botsAmount = 2;
        string[] botsNames = GenerateNamesForBots(botsAmount);
        ConsoleColor[] playerColors = ChooseColorForEach(botsAmount + 1);
        
        Player[] players = new Player[botsAmount + 1];
        players[0] = new Player(playerName, null, playerColors[0]);
        for (int i = 1; i < botsAmount + 1; i++) {
            players[i] = new Player(botsNames[i - 1], new AlwaysAgreeIfCanBot(), playerColors[i]);
        }

        PlayGame(players);
    }
    private void PlayWithFriends() {
        string[] playerNames = Interactive.InputPlayersNamesToPlay();
        ConsoleColor[] playerColors = ChooseColorForEach(playerNames.Length);

        Player[] players = new Player[playerNames.Length];
        for (int i = 0; i < players.Length; i++) {
            players[i] = new Player(playerNames[i], null, playerColors[i]);
        }

        PlayGame(players);
    }

    private void PlayGame(Player[] players) {
        game.RecreateField();
        game.StartGame(players);
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
            
            if (curColorIndex == -1) {
                continue;
            }
            
            foreach (var badColor in JustOutput.notGoodColors) {
                if ((ConsoleColor)curColorIndex == badColor) {
                    curColorIndex = -1;
                    break;
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

    private string[] GenerateNamesForBots(int botsAmount) {
        string[] names = new string[botsAmount];
        
        string startOfTextFiles = "../../../text_info";
        string nameOfNamesDir = startOfTextFiles + "/" + "names_for_bots";
        string[] nameFiles = Directory.GetFiles(nameOfNamesDir);
        string[] allNames = File.ReadAllLines(nameFiles[0]);

        int allNamesAmount = allNames.Length;
        for (int i = 0; i < botsAmount; i++) {
            names[i] = allNames[App.rand.Next(0, allNamesAmount)];
            for (int k = 0; k < i; k++) {
                if (names[k] == names[i]) {
                    i--;
                    break;
                }
            }
        }

        return names;
    }

    private void ChangeSettings() {
    }

    private void QuitGame(ref bool shouldContinue) {
        shouldContinue = false;
        JustOutput.PrintText(OutputPhrases.TextGoodbye());
    }
}