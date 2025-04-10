﻿using APICatalogo.Models;
using APICatalogo.Pagination;
using X.PagedList;

namespace APICatalogo.Repositories
{
    public interface IProdutoRepository : IRepository<Produto>
    {
        Task<IPagedList<Produto>> GetProdutosAsync(ProdutosParameters produtosParams);
        Task<IEnumerable<Produto>> GetProdutosPorCategoriaAsync(int id);
        Task<IPagedList<Produto>> GetProdutosFiltradosAsync(ProdutosFiltroPreco produtosFiltroParams);


    }
}
