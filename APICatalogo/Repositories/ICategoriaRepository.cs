using APICatalogo.Models;
using APICatalogo.Pagination;
using System.Runtime.InteropServices;
using X.PagedList;

namespace APICatalogo.Repositories
{
    public interface ICategoriaRepository : IRepository<Categoria>
    {
        Task<IPagedList<Categoria>> GetCategoriaAsync(CategoriasParameters categoriasParams);
        Task<IPagedList<Categoria>> GetCategoriasFiltradosAsync(CategoriasFiltroNome categoriasFiltroParams );


    }
}
