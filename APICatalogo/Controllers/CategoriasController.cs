using APICatalogo.Context;
using APICatalogo.DTOs;
using APICatalogo.Models;
using APICatalogo.Pagination;
using APICatalogo.Repositories;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using X.PagedList;

namespace APICatalogo.Controllers
{
    [EnableCors("OrigensComAcessoPermitido")]
    [Route("[controller]")]
    [ApiController]
     [EnableRateLimiting("fixedwindow")]
    public class CategoriasController : Controller
    {
        private readonly IUnitOfWork _uof;
        private readonly IMapper _mapper;
        private readonly IMemoryCache _cache;
        private const string CacheCategoriasKey = "CacheCategorias";

        public CategoriasController(IUnitOfWork uof, IMapper mapper, IMemoryCache cache)
        {
            _uof = uof;
            _mapper = mapper;
            _cache = cache;
        }

        /// <summary>
        /// Obtem uma lista de objetos Categoria
        /// </summary>
        /// <returns>Uma lista de objetos Categoria</returns>
        [HttpGet("pagination")]
        [Authorize]
        [DisableRateLimiting]
        public async Task<ActionResult<IEnumerable<CategoriaDTO>>> GetAsync([FromQuery] CategoriasParameters categoriasParameters)
        {
            var categorias = await _uof.CategoriaRepository.GetCategoriaAsync(categoriasParameters);
            return ObterCategorias(categorias);

        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Categoria>>> Get()
        {
            if (!_cache.TryGetValue(CacheCategoriasKey, out IEnumerable<Categoria>? categorias))
            {
                categorias = await _uof.CategoriaRepository.GetAllAsync();

                if (categorias is not null && categorias.Any())
                {
                    var cacheOptions = new MemoryCacheEntryOptions
                    {
                        AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(30),
                        SlidingExpiration = TimeSpan.FromSeconds(15),
                        Priority = CacheItemPriority.High
                    };
                    _cache.Set(CacheCategoriasKey, categorias, cacheOptions);
                }
                else
                {

                    return NotFound("Não existem categorias...");
                }
                -
            }
            return Ok(categorias);
        }
        

        [HttpGet("filter/nome/pagination")]
        public async Task<ActionResult<IEnumerable<CategoriaDTO>>> GetCategoriasFiltradasAsync([FromQuery] CategoriasFiltroNome categoriaFiltroParams)
        {
            var categorias = await _uof.CategoriaRepository.GetCategoriasFiltradosAsync(categoriaFiltroParams);

            return ObterCategorias(categorias);
        }


        [HttpGet("produtos")]
        public async Task<ActionResult<IEnumerable<CategoriaDTO>>> GetCategoriasProdutosAsync()
        {
            var categorias = await _uof.CategoriaRepository.GetAllAsync();

            var categoriasDto = _mapper.Map<IEnumerable<CategoriaDTO>>(categorias);
            return Ok(categoriasDto);  
        }


        /// <summary>
        /// Obtem uma catagoria por id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Objetos Categoria</returns>
        [HttpGet("{id:int}", Name = "ObterCategoria")]
        public async Task<ActionResult<CategoriaDTO>> GetAsync(int id)
        {
            
                var categoria = await _uof.CategoriaRepository.GetAsync(c => c.CategoriaId == id);
                if (categoria is null)
                {
                    return NotFound();
                }

                var categoriaDTO = _mapper.Map<CategoriaDTO>(categoria);    
                return Ok(categoriaDTO);
         
            

        }

        /// <summary>
        /// Inclui uma nova categoria
        /// </summary>
        /// <remarks>
        /// Exemplo de request:
        /// </remarks>
        /// <param name="categoriaDto"></param>
        /// <returns>O objeto Categoria incluida</returns>
        [HttpPost]
        public async Task<ActionResult> PostAsync(CategoriaDTO categoriaDto)
        {
            if(categoriaDto is null)
                return BadRequest();

            var categoria = _mapper.Map<Categoria>(categoriaDto);
            var novaCategoria = _uof.CategoriaRepository.Create(categoria);
            await _uof.CommitAsync();

            var NovaCategoriaDto = _mapper.Map<CategoriaDTO>(novaCategoria);
            return new CreatedAtRouteResult("ObterCategoria", new { id = categoriaDto.CategoriaId }, categoriaDto);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(int id, Categoria categoriaDto)
        {
            if (id != categoriaDto.CategoriaId)
            {
                return BadRequest();
            }

            var categoria = _mapper.Map<Categoria>(categoriaDto);
            var NovaCategoria = _uof.CategoriaRepository.Update(categoria);
            await _uof.CommitAsync();

            var novaCategoriaDto = _mapper.Map<CategoriaDTO>(NovaCategoria);

            return Ok(novaCategoriaDto);


        }
        [HttpDelete("{id:int}")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<ActionResult> Delete(int id)
        {
            var categoria = await _uof.CategoriaRepository.GetAsync(c => c.CategoriaId == id);
            if (categoria is null)
            {
                return NotFound("Categoria não localizado");
            }

            var categoriaExcluida = _uof.CategoriaRepository.Delete(categoria);
            await _uof.CommitAsync();

            var categoriaExcluidaDto = _mapper.Map<CategoriaDTO>(categoriaExcluida);

            return Ok(categoriaExcluidaDto);

        }

        private ActionResult<IEnumerable<CategoriaDTO>> ObterCategorias(IPagedList<Categoria> categorias)
        {
            var metadata = new
            {
                categorias.Count,
                categorias.PageSize,
                categorias.PageCount,
                categorias.TotalItemCount,
                categorias.HasNextPage,
                categorias.HasPreviousPage
            };

            Response.Headers.Append("X-Pagination", JsonConvert.SerializeObject(metadata));

            var categoriasDto = _mapper.Map<IEnumerable<CategoriaDTO>>(categorias);
            return Ok(categoriasDto);
        }
    }
}
