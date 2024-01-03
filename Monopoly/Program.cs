using System.Text;
using Monopoly;
using Monopoly.Cards;
using Monopoly.OutputDesign;

class Program {
    static void Main() {
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        Console.OutputEncoding = Encoding.UTF8;
        Console.InputEncoding = Encoding.GetEncoding(1251);
        
        string startOfTextFiles = "../../../text_info";
        string[] nameFiles = Directory.GetFiles(startOfTextFiles);
        string[] allNames = File.ReadAllLines(nameFiles[0]);
        foreach (var str in allNames) {
            Console.WriteLine(str);
        }

        Field field = new Field();
        JustOutput.PrintAllField(field);
        
        App app = new App();
        app.Run();
    }
}