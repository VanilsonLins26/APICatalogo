using APICatalogo.Context;
using APICatalogo.Models;
using APICatalogo.Pagination;
using Microsoft.EntityFrameworkCore;
using X.PagedList;


namespace APICatalogo.Repositories
{
    public class CategoriaRepository : Repository<Categoria>, ICategoriaRepository 
    {
        
        public CategoriaRepository(AppDbContext context) : base(context)
        {
            
        }

        public async Task<IPagedList<Categoria>> GetCategoriaAsync(CategoriasParameters categoriasParams)
        {
            var categorias = await GetAllAsync();
            var categoriasOrdenadas = categorias.OrderBy(p => p.CategoriaId).AsQueryable();
            //var resultado = PagedList<Categoria>.ToPagedList(categoriasOrdenadas, categoriasParams.PageNumber, categoriasParams.PageSize);

            var resultado = await categorias.ToPagedListAsync(categoriasParams.PageNumber, categoriasParams.PageSize);

            return resultado;
        }

        public async Task<IPagedList<Categoria>> GetCategoriasFiltradosAsync(CategoriasFiltroNome categoriasFiltroParams)
        {
            var categorias = await GetAllAsync();

            if (!string.IsNullOrEmpty(categoriasFiltroParams.Nome))
            {
                categorias = categorias.Where(c => c.Nome.Contains(categoriasFiltroParams.Nome, StringComparison.OrdinalIgnoreCase));   

            }

            //var categoriasFiltradas = PagedList<Categoria>.ToPagedList(categorias.AsQueryable(), categoriasFiltroParams.PageNumber, categoriasFiltroParams.PageSize);

            var categoriasFiltradas = await categorias.ToPagedListAsync(categoriasFiltroParams.PageNumber, categoriasFiltroParams.PageSize);

            return categoriasFiltradas;
        }
    }
}
