namespace Monopoly;

public class Field {
    public Enterprise[][] fieldArrays;
    public Industry[] industriesArray;
    public string[] countriesArray;

    private readonly Random random;

    // Struct of array #1: |ec01|ec02|bonus|ec02|zrada|ec02|money|ec03|ec03|prison|ec03|ec01|ec01|chance|ei1|ei1|ei2|review|ei2|ei1|ei3|
    // Indexes:            | 0  | 1  |  2  | 3  |  4  | 5  |  6  | 7  | 8  |  9   | 10 | 11 | 12 |  13  |14 |15 |16 |  17  |18 |19 |20 |
    // Country industries: | 00 | 01 |     | 01 |     | 01 |     | 02 | 02 |      | 02 | 00 | 00 |      |   |   |   |      |   |   |   |
    // Inter. industries:  |    |    |     |    |     |    |     |    |    |      |    |    |    |      | 1 | 1 | 2 |      | 2 | 1 | 3 |
    // Struct of array #2: |ec11|ec12|bonus|ec12|zrada|ec12|money|ec13|ec13|prison|ec13|ec11|ec11|chance|ei4|ei4|ei2|review|ei2|ei4|ei3|
    // Indexes:            | 0  | 1  |  2  | 3  |  4  | 5  |  6  | 7  | 8  |  9   | 10 | 11 | 12 |  13  |14 |15 |16 |  17  |18 |19 |20 |
    // Country industries: | 10 | 11 |     | 11 |     | 11 |     | 12 | 12 |      | 12 | 10 | 10 |      |   |   |   |      |   |   |   |
    // Inter. industries:  |    |    |     |    |     |    |     |    |    |      |    |    |    |      | 4 | 4 | 2 |      | 2 | 4 | 3 |
    // Start:              null
    private const int countriesAmount = 2;
    private const int arrayLength = 21;

    private readonly Dictionary<int, string> specialCellNamesByIndexes = new Dictionary<int, string>() {
        { 2, "Bonus" },
        { 4, "Zrada" },
        { 6, "Zarplata" },
        { 9, "Prison" },
        { 13, "Go out of country chance" },
        { 17, "Review" },
    };

    private readonly int[][] countryIndustriesIndexes = {
        new[] { 11, 12, 0 },
        new[] { 1, 3, 5 },
        new[] { 7, 8, 10 }
    };

    private readonly int[][] commonIndustriesForEach = {
        new[] { 16, 18 },
        new[] { 20 }
    };

    private readonly int[][] privateIndustriesForEach = {
        new[] { 14, 15, 19 }
    };

    public Field() {
        random = new Random();
        fieldArrays = new Enterprise[countriesAmount][];
        for (int i = 0; i < countriesAmount; i++) {
            fieldArrays[i] = new Enterprise[arrayLength];
        }

        countriesArray = new string[countriesAmount];
        int industriesArrLength = (countryIndustriesIndexes.Length + privateIndustriesForEach.Length) * 2 +
                                  commonIndustriesForEach.Length;
        industriesArray = new Industry[industriesArrLength];

        int curIndustryArrIndex = 0;
        string startOfTextFiles = "../../../text_info";
        string nameOfCountryDir = startOfTextFiles + "/" + "enterprises_for_countries";
        string nameOfInterDir = startOfTextFiles + "/" + "enterprises_for_international";

        CountryIndustriesFill(nameOfCountryDir, ref curIndustryArrIndex);
        InternationalIndustriesFill(nameOfInterDir, ref curIndustryArrIndex);
    }

    private void CountryIndustriesFill(string countryDirName, ref int curIndustryArrIndex) {
        string[] countries = Directory.GetDirectories(countryDirName);
        List<int> countryIndexesInFile =
            ChooseNonRepeatableNums(0, countries.Length, fieldArrays.Length);

        for (int i = 0; i < countriesArray.Length; i++) {
            string countryFileName = countries[countryIndexesInFile[i]];
            countriesArray[i] = GetLastWordAfterSlashForDirectories(countryFileName);
            string[] industries = Directory.GetFiles(countryFileName);

            List<int> industriesIndexes =
                ChooseNonRepeatableNums(0, industries.Length, countryIndustriesIndexes.Length);
            List<int> arraysIndexesToFill = new List<int>() { i };

            FillIndustriesInField(industriesIndexes, countryIndustriesIndexes, arraysIndexesToFill,
                ref curIndustryArrIndex,
                industries);
        }
    }

    private void InternationalIndustriesFill(string internDirName, ref int curIndustryArrIndex) {
        string[] industries = Directory.GetFiles(internDirName);
        int industriesNeeded = commonIndustriesForEach.Length + 2 * privateIndustriesForEach.Length;
        List<int> internationalIndustriesIndexes = ChooseNonRepeatableNums(0, industries.Length, industriesNeeded);
        int curInternationalIndustriesIndexesIndex = 0;
        List<int> industriesList = new List<int>();
        List<int> arraysIndexesToFill;

        for (int i = 0; i < fieldArrays.Length; i++) {
            for (int k = 0; k < privateIndustriesForEach.Length; k++) {
                industriesList.Add(internationalIndustriesIndexes[curInternationalIndustriesIndexesIndex]);
                curInternationalIndustriesIndexesIndex++;
            }

            arraysIndexesToFill = new List<int>() { i };
            FillIndustriesInField(industriesList, privateIndustriesForEach, arraysIndexesToFill,
                ref curIndustryArrIndex, industries);
            industriesList.Clear();
        }

        for (int i = 0; i < commonIndustriesForEach.Length; i++) {
            industriesList.Add(internationalIndustriesIndexes[curInternationalIndustriesIndexesIndex]);
            curInternationalIndustriesIndexesIndex++;
        }

        arraysIndexesToFill = new List<int>();

        for (int i = 0; i < fieldArrays.Length; i++) {
            arraysIndexesToFill.Add(i);
        }

        FillIndustriesInField(industriesList, commonIndustriesForEach, arraysIndexesToFill, ref curIndustryArrIndex,
            industries);
    }

    private void FillIndustriesInField(List<int> industriesIndexes, int[][] enterprisesIndexesInField,
        List<int> arraysIndexesToFill, ref int curIndustryIndexInGeneralArray, string[] industries) {

        for (int i = 0; i < industriesIndexes.Count; i++) {
            string currentIndustryDir = industries[industriesIndexes[i]];
            string currentIndustryName = GetLastWordAfterSlashForTxtFiles(currentIndustryDir);
            string[] curIndustryFile = File.ReadAllLines(currentIndustryDir);
            int arraysAmount = arraysIndexesToFill.Count;

            int enterprisesAmount = enterprisesIndexesInField[i].Length;
            List<Position> curIndustry = new List<Position>();
            industriesArray[curIndustryIndexInGeneralArray] = new Industry(curIndustry, currentIndustryName);

            int startPrice = Convert.ToInt32(curIndustryFile[0].Substring(0, curIndustryFile[0].IndexOf('-')));
            int endPrice = Convert.ToInt32(curIndustryFile[0].Substring(curIndustryFile[0].IndexOf('-') + 1));
            int stepPrice = (endPrice - startPrice) / enterprisesAmount / arraysAmount;

            List<int> enterpriseIndexesInFile =
                ChooseNonRepeatableNums(1, curIndustryFile.Length, enterprisesAmount * arraysAmount);

            for (int j = 0; j < arraysAmount; j++) {
                for (int k = 0; k < enterprisesAmount; k++) {

                    int curPrice = random.Next(startPrice, startPrice + stepPrice);
                    startPrice += stepPrice;
                    curPrice = curPrice / 10 * 10;

                    fieldArrays[arraysIndexesToFill[j]][enterprisesIndexesInField[i][k]] = new Enterprise(curPrice,
                        industriesArray[curIndustryIndexInGeneralArray],
                        curIndustryFile[enterpriseIndexesInFile[j * enterprisesAmount + k]]);

                    curIndustry.Add(new Position(arraysIndexesToFill[j], enterprisesIndexesInField[i][k]));
                }
            }

            curIndustryIndexInGeneralArray++;
        }
    }

    public void PrintAllIndustries() {
        foreach (var industry in industriesArray) {
            Console.WriteLine("Industry title: " + industry.industryName);
            foreach (var pos in industry.enterprisesIndexes) {
                Enterprise enterprise = fieldArrays[pos.arrayIndex][pos.cellIndex];
                Console.WriteLine("------------------------------");
                Console.WriteLine("    Enterprise: " + enterprise.title + "\n    Price to buy: " + enterprise.priceToBuy +
                                  "\n    Industry: " + enterprise.industry.industryName);
            }
            Console.WriteLine("______________________________________________________________________\n");
        }
    }
    
    public void PrintAllField() {
        for (int i = 0; i < fieldArrays.Length; i++) {
            Console.WriteLine("|" + countriesArray[i] + "|");
            for (int k = 0; k < fieldArrays[i].Length; k++) {
                Enterprise cur = fieldArrays[i][k];
                Console.WriteLine("------------------------------");
                Console.Write("    ");
                Console.WriteLine((cur == null) ? 
                    "<" + specialCellNamesByIndexes[k] + ">" : 
                    "Enterprise: " + cur.title + "\n    Price to buy: " + cur.priceToBuy + "\n    Industry: " + cur.industry.industryName);
            }
            Console.WriteLine("______________________________________________________________________\n");
        }
    }

    private string GetLastWordAfterSlashForDirectories(string fileDirection) {
        return fileDirection.Substring(fileDirection.LastIndexOf("\\") + 1);
    }

    private string GetLastWordAfterSlashForTxtFiles(string fileDirection) {
        int lastIndexOfSlash = fileDirection.LastIndexOf("\\");
        return fileDirection.Substring(lastIndexOfSlash + 1, fileDirection.LastIndexOf(".") - (lastIndexOfSlash + 1));
    }

    private List<int> ChooseNonRepeatableNums(int begin, int end, int amount) {
        if (end - begin < amount) {
            return null;
        }

        List<int> ans = new List<int>();
        bool[] isUsed = new bool[end - begin];
        do {
            int curNum = random.Next(begin, end);
            if (isUsed[curNum - begin]) {
                continue;
            }
            ans.Add(curNum);
            isUsed[curNum - begin] = true;
        } while (ans.Count < amount);

        return ans;
    }
}