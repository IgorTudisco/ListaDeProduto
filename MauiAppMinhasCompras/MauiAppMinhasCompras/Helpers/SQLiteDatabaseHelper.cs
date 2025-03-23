using MauiAppListaDeCompras.Models;
using SQLite;

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
        public Task<int> Insert(Produto produto)
        {
            return _asyncConnection.InsertAsync(produto);
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
        public Task<List<Produto>> GetAll()
        {
            return _asyncConnection.Table<Produto>().ToListAsync();
        }
        public Task<List<Produto>> Search(string query)
        {
            string sql = "SELECT * FROM Produto WHERE descricao LIKE '%" + query + "%'";
            return _asyncConnection.QueryAsync<Produto>(sql);
        }
    }
}
