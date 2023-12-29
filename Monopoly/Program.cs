using System.Text;
using Monopoly;
using Monopoly.Cards;

class Program {
    static void Main(string[] args) {
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        var enc1251 = Encoding.GetEncoding(1251);

        System.Console.OutputEncoding = System.Text.Encoding.UTF8;
        System.Console.InputEncoding = enc1251;
        
        Field field = new Field();
        
        field.PrintAllField();
        field.PrintAllIndustries();
        // Uncomment above if you wanna check out field fill (Ctrl+/)
        
        Console.ReadLine();
        App app = new App();
        app.Run();
    }
}