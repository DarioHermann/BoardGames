using System.Web.Mvc;

namespace BoardGames.Areas.Checkers.Controllers
{
    public class CheckersController : Controller
    {
        // GET: Checkers/Checkers
        public ActionResult Index()
        {
            return View();
        }
    }
}