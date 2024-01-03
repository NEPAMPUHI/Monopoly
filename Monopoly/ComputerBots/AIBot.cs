using Monopoly.Cards;
using Monopoly.OutputDesign;

namespace Monopoly.ComputerBots; 

public interface AIBot {
    public string BotBuyEnterpriseOrNot(Player player, Enterprise enterprise);
    public string BotPayToGoOutOfPrisonOrNot(Player player);
    public string BotStayOnWorkOrNot(Player player);
    public int BotWhichEnterprisePawnToNotLose(Player player, List<Enterprise> enterprises);
    public string BotPawnEnterpriseOrBuildHotelPreTurn(Player player, List<Enterprise> notPawnedEnterprises,
        List<Enterprise> pawnedEnterprises, List<Enterprise> enterprisesToBuildHotel);
    public int BotWhichEnterprisePawnPreTurn(Player player, List<Enterprise> notPawnedEnterprises);
    public int BotWhichEnterpriseUnPawnPreTurn(Player player, List<Enterprise> pawnedEnterprises);
    public int BotWhichEnterpriseBuildHotelPreTurn(Player player, List<Enterprise> enterprisesToBuildHotel);
}