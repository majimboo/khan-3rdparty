using KhanEngine.ViewModel;
using Lang;
using Net.Model;

namespace KhanEngine.View
{
    public class MainView
    {
        public Show_hidden Show { get; set; }
        public LoginModel LoginM { get; set; }
        public LoginVM LoginVM { get; set; }

        public GameVM GameVM { get; set; }
        public Language Language { get; set; }

        public InfoView InfoV { get; set; }
        public GameModel GameM { get; set; }

        public MainView()
        {
            GameM = new GameModel();
            Language = new Language();
            Show = new Show_hidden();
            InfoV = new InfoView();
            LoginM = new LoginModel();
            GameVM = new GameVM(GameM, LoginM, InfoV);
            LoginVM = new LoginVM(LoginM, GameM);
        }
    }
}