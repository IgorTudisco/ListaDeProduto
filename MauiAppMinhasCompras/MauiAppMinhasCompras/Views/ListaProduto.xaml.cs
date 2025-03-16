using MauiAppListaDeCompras.Models;
using System.Collections.ObjectModel;

namespace MauiAppMinhasCompras.Views;

public partial class ListaProduto : ContentPage
{
    ObservableCollection<Produto> produtosList = new ObservableCollection<Produto>();

    public ListaProduto()
    {
        InitializeComponent();

        list_produto.ItemsSource = produtosList;
    }

    protected async override void OnAppearing()
    {
        base.OnAppearing();

        List<Produto> produtos = await App.DatabaseHelper.GetAll();

        produtos.ForEach(produto => produtosList.Add(produto));
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

    private void MenuItem_Clicked(object sender, EventArgs e)
    {
    }

    private void MenuItem_Clicked_Excluir(object sender, EventArgs e)
    {
        //var produtoDeletar = protudo_excluir.value;

        //await App.DatabaseHelper.Delete(produtoDeletar.Id);

    }

    private async void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
    {
        string text = e.NewTextValue;

        produtosList.Clear();

        List<Produto> produtos = await App.DatabaseHelper.Search(text);

        produtos.ForEach(produto => produtosList.Add(produto));

    }

    private void ToolbarItem_Clicked_1(object sender, EventArgs e)
    {
        try
        {
            double? somaItem = produtosList.Sum(produto => produto.Total!);

            string msg = $"O total é : {somaItem:C}";

            DisplayAlert("Total dos produtos ", msg, "OK");

        }
        catch (Exception ex)
        {
            DisplayAlert("Erro", ex.Message, "OK");
        }

    }
}