using GameLibraryManager.Controller;
using GameLibraryManager.View;

namespace GameLibraryManager
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var controller = new PlayerController();
            var menu = new ConsoleMenu(controller);
            menu.Run();
        }
    }
}