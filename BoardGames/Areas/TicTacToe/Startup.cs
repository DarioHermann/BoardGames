using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(BoardGames.Areas.TicTacToe.Startup))]
namespace BoardGames.Areas.TicTacToe
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.MapSignalR();
        }
    }
}