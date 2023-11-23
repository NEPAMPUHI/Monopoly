namespace Monopoly; 

public class Field {
    private Enterprise[][] fieldArrays;
    internal Industry[] industriesArray;
    internal string[] countriesArray;
    
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
    private readonly int[][] countryIndustriesIndexes = {
        new [] { 11, 12, 0 },
        new [] { 1, 3, 5 },
        new [] { 7, 8, 10 }
    };
    private readonly int[][] commonIndustriesForEach = {
        new [] { 16, 18 },
        new [] { 20 }
    };
    private readonly int[][] privateIndustriesForEach = {
        new [] { 14, 15, 19 }
    };

    public Field() {
        random = new Random();
        fieldArrays = new Enterprise[countriesAmount][];
        for (int i = 0; i < countriesAmount; i++) {
            fieldArrays[i] = new Enterprise[arrayLength];
        }
        countriesArray = new string[countriesAmount];
        int industriesArrLength = (countryIndustriesIndexes.Length + commonIndustriesForEach.Length) * 2 +
                                  privateIndustriesForEach.Length;
        industriesArray = new Industry[industriesArrLength];

        int curIndustryArrIndex = 0;
        string startOfTextFiles = "../../../text_info";
        string nameOfCountryDir = startOfTextFiles + "/" + "enterprises_for_countries";
        string nameOfInterDir = startOfTextFiles + "/" + "enterprises_for_international";

        CountryIndustriesFill(nameOfCountryDir, ref curIndustryArrIndex);
        PrintAllIndustries(industriesArray);
        Console.WriteLine("______________________________________________________________________________");
        PrintAllEnterprises(fieldArrays[0]);
        Console.WriteLine("______________________________________________________________________________");
        PrintAllEnterprises(fieldArrays[1]);
    }

    private void CountryIndustriesFill(string countryDirName, ref int curIndustryArrIndex) {
        
        string[] countries = Directory.GetDirectories(countryDirName);
        List<int> countryIndexesInFile =
            ChooseNonRepeatableNums(0, countries.Length, fieldArrays.Length);
        for (int i = 0; i < countryIndexesInFile.Count; i++) {
            FillCountriesForEach(countries[countryIndexesInFile[i]], i, out countriesArray[i],
                ref curIndustryArrIndex);
        }
    }

    private void FillCountriesForEach(string countryFileName, int curArrayIndex, out string curCounty,
        ref int curIndustryIndex) {

        curCounty = GetLastWordAfterSlash(countryFileName);
        string[] industries = Directory.GetFiles(countryFileName);
        List<int> industryIndexesInFile =
            ChooseNonRepeatableNums(0, industries.Length, countryIndustriesIndexes.Length);

        for (int i = 0; i < countryIndustriesIndexes.Length; i++) {
            string currentIndustryDir = industries[industryIndexesInFile[i]];
            string currentIndustryName = GetLastWordAfterSlash(currentIndustryDir);
            string[] curIndustryFile = File.ReadAllLines(currentIndustryDir);

            int enterprisesAmount = countryIndustriesIndexes[i].Length;
            List<Pair> curIndustry = new List<Pair>();
            industriesArray[curIndustryIndex] = new Industry(curIndustry, currentIndustryName);

            int startPrice = Convert.ToInt32(curIndustryFile[0].Substring(0, curIndustryFile[0].IndexOf('-')));
            int endPrice = Convert.ToInt32(curIndustryFile[0].Substring(curIndustryFile[0].IndexOf('-') + 1));
            int stepPrice = (endPrice - startPrice) / enterprisesAmount;

            List<int> enterpriseIndexesInFile =
                ChooseNonRepeatableNums(1, curIndustryFile.Length, enterprisesAmount);
            for (int k = 0; k < enterprisesAmount; k++) {
                int curPrice = random.Next(startPrice, startPrice + stepPrice);
                startPrice += stepPrice;
                curPrice = curPrice / 10 * 10;
                fieldArrays[curArrayIndex][countryIndustriesIndexes[i][k]] = new Enterprise(curPrice,
                    industriesArray[curIndustryIndex],
                    curIndustryFile[enterpriseIndexesInFile[k]]);
                curIndustry.Add(new Pair(curArrayIndex, countryIndustriesIndexes[i][k]));
            }
            curIndustryIndex++;
        }
    }

    private void PrintAllIndustries(Industry[] industriesArray) {
        foreach (var i in industriesArray) {
            if (i == null) {
                Console.WriteLine("NULL");
            }
            else {
                Console.WriteLine("Industry name: " + i.industryName);
                foreach (var i1 in i.enterprisesIndexes) {
                    Enterprise i2 = fieldArrays[i1.arrayIndex][i1.enterpriseIndex];
                    Console.WriteLine(i2.title + ", money to pay: " + i2.priceToBuy + ", industry name: " + i2.industry.industryName);
                }
                Console.WriteLine();
            }
        }
    }

    private void PrintAllEnterprises(Enterprise[] enterprisesArray) {
        foreach (var i in enterprisesArray) {
            if (i == null) {
                Console.WriteLine("NULL");
            }
            else {
                Console.WriteLine(i.title + ", money to pay: " + i.priceToBuy + ", industry: " + i.industry.industryName);
            }
        }
    }
    private string GetLastWordAfterSlash(string fileDirection) {
        return fileDirection.Substring(fileDirection.LastIndexOf("\\") + 1);
    }
    private List<int> ChooseNonRepeatableNums(int begin, int end, int amount) {
        if (end - begin < amount) {
            return null;
        }

        List<int> ans = new List<int>();
        bool[] isUsed = new bool[end - begin];
        int curNum;
        do {
            curNum = random.Next(begin, end);
            if (isUsed[curNum - begin] != true) {
                ans.Add(curNum);
                isUsed[curNum - begin] = true;
            }
        } while (ans.Count < amount);

        return ans;
    }
}