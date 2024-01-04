using System.Text;
namespace Monopoly; 

public class App {
    private static App instance;
    public static readonly Random rand = new ();
    private readonly MainMenu inMenu;

    private App() {
        inMenu = new MainMenu();
    }

    public static App GetInstance() {
        if (instance == null) {
            instance = new App();
        }
        return instance;
    }
    public void Run() {
        var shouldContinue = true;
        while (shouldContinue) {
            inMenu.DisplayMenu();
            inMenu.PerformAction(ref shouldContinue);
        }
    }
}