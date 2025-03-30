using MauiAppListaDeCompras.Models;

namespace MauiAppMinhasCompras.Views
{
    public partial class NovoProduto : ContentPage
    {
        public List<string> Categorias { get; set; } = new()
        {
            "Alimentação", "Eletrônicos", "Roupas", "Higiene", "Outros"
        };

        public NovoProduto()
        {
            InitializeComponent();
            BindingContext = this;
        }

        private async void ToolbarItem_Clicked(object sender, EventArgs e)
        {
            try
            {
                // Validações antes de criar o produto
                if (string.IsNullOrWhiteSpace(txt_descricao.Text))
                {
                    await DisplayAlert("Erro", "A descrição não pode estar vazia.", "OK");
                    return;
                }

                if (!double.TryParse(txt_quantidade.Text, out double quantidade) || quantidade <= 0)
                {
                    await DisplayAlert("Erro", "Digite uma quantidade válida.", "OK");
                    return;
                }

                if (!double.TryParse(txt_preco.Text, out double preco) || preco <= 0)
                {
                    await DisplayAlert("Erro", "Digite um preço válido.", "OK");
                    return;
                }

                if (picker_categoria.SelectedItem == null)
                {
                    await DisplayAlert("Erro", "Selecione uma categoria.", "OK");
                    return;
                }

                Produto produto = new Produto
                {
                    Descricao = txt_descricao.Text.Trim(),
                    Quantidade = quantidade,
                    Preco = preco,
                    Categoria = picker_categoria.SelectedItem.ToString()
                };

                await App.DatabaseHelper.Insert(produto);

                await DisplayAlert("Sucesso", "Produto inserido com sucesso!", "OK");
                await Navigation.PopAsync();
            }
            catch (Exception ex)
            {
                await DisplayAlert("Erro", $"Ocorreu um erro: {ex.Message}", "OK");
            }
        }

        private void Picker_categoria_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (sender is Picker picker && picker.SelectedItem is string categoriaSelecionada)
            {
                Console.WriteLine($"Categoria selecionada: {categoriaSelecionada}");
            }
        }
    }
}
