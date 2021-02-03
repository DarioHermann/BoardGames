using System.Web.Mvc;

namespace BoardGames.Areas.TicTacToe
{
    public class TicTacToeAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "TicTacToe";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "TicTacToe_default",
                "TicTacToe/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}