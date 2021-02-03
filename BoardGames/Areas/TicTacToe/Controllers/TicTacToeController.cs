using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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