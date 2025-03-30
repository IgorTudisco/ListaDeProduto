using MauiAppListaDeCompras.Models;

namespace MauiAppMinhasCompras.Views;

public partial class EditarProduto : ContentPage
{
    public EditarProduto()
    {
        InitializeComponent();
    }

    private async void ToolbarItem_Clicked(object sender, EventArgs e)
    {
        try
        {
            Produto? produto_recebido = BindingContext as Produto;

            if (produto_recebido == null)
            {
                await DisplayAlert("Erro", "Produto não encontrado", "OK");
            }

            Produto produtoAtualizado = new Produto
            {
                Id = produto_recebido!.Id,
                Descricao = txt_descricao.Text,
                Quantidade = Convert.ToDouble(txt_quantidade.Text),
                Preco = Convert.ToDouble(txt_preco.Text)
            };

            await App.DatabaseHelper.Insert(produtoAtualizado);

            await DisplayAlert("Sucesso", "Produto atualizado com sucesso", "OK");

            await Navigation.PopAsync();
        }
        catch (Exception ex)
        {
            await DisplayAlert("Erro", ex.Message, "OK");
        }
    }

}
