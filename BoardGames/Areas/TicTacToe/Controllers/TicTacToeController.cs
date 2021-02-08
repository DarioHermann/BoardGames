using System.Web.Mvc;

namespace BoardGames.Areas.TicTacToe.Controllers
{
    public class TicTacToeController : Controller
    {
        // GET: TicTacToe/TicTacToe
        public ActionResult Index()
        {
            return View();
        }
    }
}