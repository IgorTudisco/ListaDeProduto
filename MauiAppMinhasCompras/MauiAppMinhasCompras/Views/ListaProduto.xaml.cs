using MauiAppListaDeCompras.Models;
using System.Collections.ObjectModel;

namespace MauiAppMinhasCompras.Views;

public partial class ListaProduto : ContentPage
{
    ObservableCollection<Produto> ProdutosList = new ObservableCollection<Produto>();

    public ListaProduto()
    {
        InitializeComponent();

        list_produto.ItemsSource = ProdutosList;

    }

    protected async override void OnAppearing()
    {
        base.OnAppearing();        

        List<Produto> produtos = await App.DatabaseHelper.GetAll();

        produtos.ForEach(produto => ProdutosList.Add(produto));
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
            MenuItem selecionado = sender as MenuItem;

            Produto produto = selecionado.BindingContext as Produto;

            bool confirme = await DisplayAlert("Confirmação", $"Deseja excluir o produto {produto.Descricao}?", "Sim", "Não");

            if (confirme)
            {
                await App.DatabaseHelper.Delete(produto.Id);
                ProdutosList.Remove(produto);
            }
        }
        catch (Exception ex)
        {
            DisplayAlert("Erro", ex.Message, "OK");
        }
    }

    private async void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
    {
        string text = e.NewTextValue;

        ProdutosList.Clear();

        List<Produto> produtos = await App.DatabaseHelper.Search(text);

        produtos.ForEach(produto => ProdutosList.Add(produto));

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
}