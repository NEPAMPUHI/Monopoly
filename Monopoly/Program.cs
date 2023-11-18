﻿using System.Text;
using Monopoly;

class Program {
    static void Main(string[] args) {
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        var enc1251 = Encoding.GetEncoding(1251);

        System.Console.OutputEncoding = System.Text.Encoding.UTF8;
        System.Console.InputEncoding = enc1251;

        App app = new App();
        app.Run();
    }
}