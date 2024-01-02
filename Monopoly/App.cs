using System.Text;
namespace Monopoly; 

public class App {
    public static readonly Random rand = new Random();
    private readonly MainMenu inMenu;

    public App() {
        inMenu = new MainMenu();
    }
    public void Run() {
        var shouldContinue = true;
        while (shouldContinue) {
            inMenu.DisplayMenu();
            inMenu.PerformAction(ref shouldContinue);
        }
    }
}