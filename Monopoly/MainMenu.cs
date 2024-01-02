using Monopoly.Cards;
using Monopoly.OutputDesign;

namespace Monopoly;

// ConsoleColor defaltColor = Console.ForegroundColor;
// Console.WriteLine("Чудово! Ось ваш список гравців:");
// for (int i = 0; i < players.Length; i++) {
//     Console.Write($"Гравець {i + 1}: ");
//     Console.ForegroundColor = players[i].chipColor;
//     Console.WriteLine(players[i].nameInGame);
//     Console.ForegroundColor = defaltColor;
// }
public class MainMenu {
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
            default:
                break;
        }
    }
    private void PlayWithComputer() {
        GamePlay game = new GamePlay();
    }
    private void PlayWithFriends() {
        string[] playerNames = Interactive.InputPlayersNamesToPlay();
        ConsoleColor[] playerColors = ChooseColorForEach(playerNames.Length);

        Player[] players = new Player[playerNames.Length];
        for (int i = 0; i < players.Length; i++) {
            players[i] = new Player(playerNames[i], playerColors[i]);
        }

        GamePlay game = new GamePlay();
        game.StartGameWithFriends(players);
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

    private void ChangeSettings() {
    }

    private void QuitGame(ref bool shouldContinue) {
        shouldContinue = false;
        JustOutput.PrintText(OutputPhrases.TextGoodbye());
    }
}