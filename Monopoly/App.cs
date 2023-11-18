using System.Text;
namespace Monopoly; 

public class App {
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