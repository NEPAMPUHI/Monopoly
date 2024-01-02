using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

using Monopoly.Cards;

namespace Monopoly;

public static class Interactive {

    public static string GetPersonChoice(List<string> inputVariants) {
        string? strToReturn = null;

        do {
            if (strToReturn != null) {
                Console.WriteLine("Спробуйте ще раз ^_^\n");
            }

            Console.Write("Ваш вибір: ");
            strToReturn = Console.ReadLine();
        } while (!IsExistInList(strToReturn, inputVariants));

        return strToReturn;
    }

    private static bool IsExistInList(string strToCheck, List<string> inputVariants) {
        bool isCorrect = false;
        foreach(var str in inputVariants) {
            if (strToCheck == str) {
                return true;
            }
        }
        return false;
    }
}

