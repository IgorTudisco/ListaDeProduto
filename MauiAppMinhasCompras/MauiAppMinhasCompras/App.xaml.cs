using MauiAppListaDeCompras.Helpes;

namespace MauiAppMinhasCompras
{
    public partial class App : Application
    {
        private static SQLiteDatabaseHelper _databaseHelper;

        public static SQLiteDatabaseHelper DatabaseHelper
        {
            get
            {
                if (_databaseHelper == null)
                {
                    string path = Path.Combine
                        (
                            Environment.GetFolderPath
                            (

                                Environment.SpecialFolder.LocalApplicationData

                            ), "db_compras.db3"
                        );

                    _databaseHelper = new SQLiteDatabaseHelper(path);
                }
                return _databaseHelper;
            }
        }

        public App()
        {
            InitializeComponent();

            MainPage = new NavigationPage(new Views.ListaProduto());
        }
    }
}
