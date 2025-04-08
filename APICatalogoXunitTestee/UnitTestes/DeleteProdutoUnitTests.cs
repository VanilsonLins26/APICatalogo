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
    public class DeleteProdutoUnitTests : IClassFixture<ProdutosUnitTestController>
    {
        private readonly ProdutosController _controller;

        public DeleteProdutoUnitTests(ProdutosUnitTestController controller)
        {
            _controller = new ProdutosController(controller.repository, controller.mapper);
        }

        [Fact]
        public async Task DeleteProdutoById_Return_OkResult()
        {
            var prodId = 11;

            //Act
            var result = await _controller.Delete(prodId) as ActionResult<ProdutoDTO>;

            //Assert
            result.Should().NotBeNull();
            result.Result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async Task DeleteProdutoById_Return_notFoundResult()
        {
            var prodId = 2;

            //Act
            var result = await _controller.Delete(prodId);

            //Assert
            result.Result?.Should().NotBeNull();    
            result.Result.Should().BeOfType<NotFoundObjectResult>();
        }

    }
}
