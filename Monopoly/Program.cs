using System.Text;
using Monopoly;

class Program {
    static void Main(string[] args) {
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        var enc1251 = Encoding.GetEncoding(1251);

        System.Console.OutputEncoding = System.Text.Encoding.UTF8;
        System.Console.InputEncoding = enc1251;

        // string absoluteFilePath = @"C:\Users\Intel\Documents\GitHub\Monopoly\Monopoly\text_info";
        //
        // // Получаем абсолютный путь к текущему рабочему каталогу
        // string currentDirectory = Directory.GetCurrentDirectory();
        //
        // // Получаем абсолютный путь к файлу
        // string absolutePath = Path.GetFullPath(absoluteFilePath);
        //
        // // Определяем относительный путь
        // string relativePath = Path.GetRelativePath(currentDirectory, absolutePath);
        //
        // // Выводим результат
        // Console.WriteLine("Абсолютный путь: " + absolutePath);
        // Console.WriteLine("вап: " + currentDirectory);
        // Console.WriteLine("Относительный путь от текущего каталога: " + relativePath);
        // string dirName = "../../../text_info" + "/" + "enterprises_for_countries";
        // string[] dirs1 = Directory.GetDirectories(dirName);
        // string one = dirs1[2];
        // string[] dirs = Directory.GetFiles(one);
        // for (int i = 0; i < dirs.Length; i++) {
        //     Console.WriteLine(dirs[i]);
        // }

        Field field = new Field();
        
        App app = new App();
        app.Run();
    }
}