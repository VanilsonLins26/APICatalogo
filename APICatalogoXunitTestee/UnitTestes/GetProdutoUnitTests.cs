using APICatalogo.Controllers;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using APICatalogo.DTOs;

namespace APICatalogoXunitTestee.UnitTestes;

public class GetProdutoUnitTests : IClassFixture<ProdutosUnitTestController>
{
    private readonly ProdutosController _controller;

    public GetProdutoUnitTests(ProdutosUnitTestController controller)
    {
        _controller = new ProdutosController(controller.repository, controller.mapper);
    }

    [Fact]
    public async Task GetProdutoById_OkResult()
    {
        //Arrange
        var proId = 16;
        //Act
        var data = await _controller.Get(proId);
        //Assert
        //var okResult = Assert.IsType<OkObjectResult>(data.Result);
        //Assert.Equal(200, okResult.StatusCode);

        //Assert(fluent)
        data.Result.Should().BeOfType<OkObjectResult>().Which.StatusCode.Should().Be(200);
    }

    [Fact]
    public async Task GetProdutoById_Return_NotFound()
    {
        //Arrange
        var proId = 99;
        //Act
        var data = await _controller.Get(proId);


        //Assert(fluent)
        data.Result.Should().BeOfType<NotFoundObjectResult>().Which.StatusCode.Should().Be(404);

    }

    [Fact]
    public async Task GetProdutoBYId_Return_BadRequest()
    {
        //Arrange
        var proId = -1;
        //Act
        var data = await _controller.Get(proId);


        //Assert(fluent)
        data.Result.Should().BeOfType<BadRequestObjectResult>().Which.StatusCode.Should().Be(400);
    }

    [Fact]
    public async Task GetProdutos_Return_ListOfProdutoDTO()
    {
        
        //Act
        var data = await _controller.Get();


        //Assert(fluent)
        data.Result.Should().BeOfType<OkObjectResult>()
            .Which.Value.Should().BeAssignableTo<IEnumerable<ProdutoDTO>>()
            .And.NotBeNull();
    }

    [Fact]
    public async Task GetProdutos_Return_BadRequestResult()
    {

        //Act
        var data = await _controller.Get();


        //Assert(fluent)
        data.Result.Should().BeOfType<BadRequestResult>();
            
    }
}

