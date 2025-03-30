using MauiAppListaDeCompras.Models;
using MauiAppMinhasCompras.Models;
using SQLite;
using System.Data.Common;

namespace MauiAppListaDeCompras.Helpes
{
    public class SQLiteDatabaseHelper
    {
        readonly SQLiteAsyncConnection _asyncConnection;
        public SQLiteDatabaseHelper(string path)
        {
            _asyncConnection = new SQLiteAsyncConnection(path);

            _asyncConnection.CreateTableAsync<Produto>().Wait();
        }
        public async Task<int> Insert(Produto produto)
        {
            return await _asyncConnection.InsertAsync(produto);
        }
        public Task<int> Update(Produto produto)
        {
            string sql = "UPDATE Produto SET Descricao=?, Quantidade=?, Preco=? WHERE Id=?";
            return _asyncConnection.ExecuteAsync(sql, produto.Descricao, produto.Quantidade, produto.Preco, produto.Id);
        }

        public Task<int> Delete(int id)
        {
            return _asyncConnection.Table<Produto>().DeleteAsync(produto => produto.Id == id);
        }
        public async Task<List<Produto>> GetAll()
        {
            await _asyncConnection.Table<Produto>().DeleteAsync(p => p.Categoria == null);
            return await _asyncConnection.Table<Produto>().ToListAsync();
        }
        public async Task<List<Produto>> Search(string query)
        {
            string sql = "SELECT * FROM Produto WHERE descricao LIKE '%" + query + "%'";
            return await _asyncConnection.QueryAsync<Produto>(sql);
        }

        public async Task<List<RelatorioCategoria>> ObterProdutosPorCategoria()
        {
            // Consulta SQL para obter a soma do total gasto por categoria
            var query = @"
                        SELECT Categoria, SUM(Total) as TotalGasto
                        FROM Produto
                        GROUP BY Categoria";

            var resultado = await _asyncConnection.QueryAsync<RelatorioCategoria>(query);
            return resultado.ToList();
        }


        public async Task<List<RelatorioCategoria>> ObterRelatorioPorCategoria()
        {
            var produtos = await _asyncConnection.Table<Produto>().ToListAsync();
            var relatorio = produtos.GroupBy(p => p.Categoria)
                                    .Select(group => new RelatorioCategoria
                                    {
                                        Categoria = group.Key,
                                        TotalGasto = group.Sum(p => p.Total ?? 0)
                                    })
                                    .ToList();
            return relatorio;
        }

    }
}
