using System.Text;
using Monopoly;
using Monopoly.Cards;
using Monopoly.OutputDesign;

class Program {
    static void Main() {
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        Console.OutputEncoding = Encoding.UTF8;
        Console.InputEncoding = Encoding.GetEncoding(1251);

        App app = App.GetInstance();
        app.Run();
    }
}