using MauiAppListaDeCompras.Helpes;

namespace MauiAppMinhasCompras.Views
{
    public partial class RelatorioDeCompras : ContentPage
    {
        private readonly SQLiteDatabaseHelper _databaseHelper;

        public RelatorioDeCompras(SQLiteDatabaseHelper databaseHelper)
        {
            InitializeComponent();
            _databaseHelper = databaseHelper; // Caminho do banco
            CarregarRelatorio(); // Chama a função para carregar os dados do relatório
        }

        // Método que carrega os dados de produtos por categoria
        private async void CarregarRelatorio()
        {
            try
            {
                // Obtém os dados do relatório agrupados por categoria
                var relatorio = await _databaseHelper.ObterRelatorioPorCategoria();
                // Vincula o relatório ao ListView
                relatorioListView.ItemsSource = relatorio;
            }
            catch (Exception ex)
            {
                await DisplayAlert("Erro", ex.Message, "OK"); // Exibe erro caso haja algum problema
            }
        }

        // Caso você tenha um evento para fazer refresh dos dados
        private async void RelatorioListView_Refreshing(object sender, EventArgs e)
        {
            CarregarRelatorio(); // Recarrega os dados
            relatorioListView.IsRefreshing = false; // Desativa o indicador de refresh
        }
    }
}
