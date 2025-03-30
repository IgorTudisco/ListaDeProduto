using MauiAppListaDeCompras.Helpes;
using MauiAppListaDeCompras.Models;
using System.Collections.ObjectModel;

namespace MauiAppMinhasCompras.Views
{
    public partial class ListaProduto : ContentPage
    {
        ObservableCollection<Produto> ProdutosList = new ObservableCollection<Produto>();
        private readonly SQLiteDatabaseHelper _databaseHelper;

        public ListaProduto(SQLiteDatabaseHelper databaseHelper)
        {
            InitializeComponent();
            ProdutosList.Clear();
            list_produto.ItemsSource = ProdutosList;
            _databaseHelper = databaseHelper;
            list_produto.ItemsSource = ProdutosList;
        }

        protected async override void OnAppearing()
        {
            try
            {
                base.OnAppearing();
                ProdutosList.Clear();
                List<Produto> produtos = await App.DatabaseHelper.GetAll();
                produtos.ForEach(produto => ProdutosList.Add(produto));
            }
            catch (Exception ex)
            {
                await DisplayAlert("Erro", ex.Message, "OK");
            }
        }

        private void ToolbarItem_Clicked(object sender, EventArgs e)
        {
            try
            {
                Navigation.PushAsync(new Views.NovoProduto());
            }
            catch (Exception ex)
            {
                DisplayAlert("Erro", ex.Message, "OK");
            }
        }

        private void List_produto_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            try
            {
                Produto selecionado = e.SelectedItem as Produto;
                Navigation.PushAsync(new Views.EditarProduto { BindingContext = selecionado });
            }
            catch (Exception ex)
            {
                DisplayAlert("Erro", ex.Message, "OK");
            }
        }

        private async void MenuItem_Clicked_Excluir(object sender, EventArgs e)
        {
            try
            {
                MenuItem? selecionado = sender as MenuItem;
                Produto? produto = selecionado!.BindingContext as Produto;
                bool confirme = await DisplayAlert("Confirmação", $"Deseja excluir o produto {produto!.Descricao}?", "Sim", "Não");

                if (confirme)
                {
                    ProdutosList.Clear();
                    await App.DatabaseHelper.Delete(produto.Id);
                    ProdutosList.Remove(produto);
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Erro", ex.Message, "OK");
            }
        }

        private async void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(e.NewTextValue))
                {
                    List<Produto> produtos = await App.DatabaseHelper.GetAll();
                    ProdutosList.Clear();
                    produtos.ForEach(produto => ProdutosList.Add(produto));
                    return;
                }
                string text = e.NewTextValue;
                list_produto.IsRefreshing = true;
                ProdutosList.Clear();
                List<Produto> produtosFiltrados = await App.DatabaseHelper.Search(text);
                produtosFiltrados.ForEach(produto => ProdutosList.Add(produto));
            }
            catch (Exception ex)
            {
                await DisplayAlert("Erro", ex.Message, "OK");
            }
            finally
            {
                list_produto.IsRefreshing = false;
            }
        }

        private void ToolbarItem_Clicked_1(object sender, EventArgs e)
        {
            try
            {
                double? somaItem = ProdutosList.Sum(produto => produto.Total!);
                string msg = $"O total é : {somaItem:C}";
                DisplayAlert("Total dos produtos ", msg, "OK");
            }
            catch (Exception ex)
            {
                DisplayAlert("Erro", ex.Message, "OK");
            }
        }

        private async void List_produto_Refreshing(object sender, EventArgs e)
        {
            try
            {
                base.OnAppearing();
                ProdutosList.Clear();
                List<Produto> produtos = await App.DatabaseHelper.GetAll();
                produtos.ForEach(produto => ProdutosList.Add(produto));
            }
            catch (Exception ex)
            {
                await DisplayAlert("Erro", ex.Message, "OK");
            }
            finally
            {
                list_produto.IsRefreshing = false;
            }
        }

 
        // Update the OnCarregarRelatorioClicked method to pass the databaseHelper
        private async void OnCarregarRelatorioClicked(object sender, EventArgs e)
        {
            try
            {
                // Create an instance of the RelatorioDeCompras page with the databaseHelper
                var relatorioDeComprasPage = new RelatorioDeCompras(_databaseHelper);

                // Navigate to the RelatorioDeCompras page
                await Navigation.PushAsync(relatorioDeComprasPage);
            }
            catch (Exception ex)
            {
                await DisplayAlert("Erro", ex.Message, "OK");
            }
        }
    }
}
