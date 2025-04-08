using APICatalogo.Controllers;
using APICatalogo.DTOs;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APICatalogoXunitTestee.UnitTestes
{
    public class PutProdutoUnitTests : IClassFixture<ProdutosUnitTestController>
    {
        private readonly ProdutosController _controller;

        public PutProdutoUnitTests(ProdutosUnitTestController controller)
        {
            _controller = new ProdutosController(controller.repository, controller.mapper);
        }

        [Fact]
        public async Task PutProduto_Return_OkResult()
        {
            //Arrange
            var prodId = 14;

            var updatedProdutoDto = new ProdutoDTO
            {
                ProdutoId = prodId,
                Nome = "Produto Ataualizado - Testes",
                Descricao = "Minha Descrição",
                ImagemUrl = "Imagem1.jpg",
                CategoriaId = 2
            };

            //Act
            var result = await _controller.Put(prodId, updatedProdutoDto);

            //Assert
            result.Result.Should().NotBeNull();
            result.Result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async Task PutProduto_Return_BadRequesResult()
        {
            //Arrange
            var prodId = 14;

            var updatedProdutoDto = new ProdutoDTO
            {
                ProdutoId = 99,
                Nome = "Produto Ataualizado - Testes",
                Descricao = "Minha Descrição",
                ImagemUrl = "Imagem1.jpg",
                CategoriaId = 2
            };

            //Act
            var result = await _controller.Put(prodId, updatedProdutoDto);

            //Assert

            result.Result.Should().BeOfType<BadRequestResult>().Which.StatusCode.Should().Be(400);
        }
    }
}
