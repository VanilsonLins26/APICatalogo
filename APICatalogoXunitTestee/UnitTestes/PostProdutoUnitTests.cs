﻿using APICatalogo.Controllers;
using APICatalogo.DTOs;
using System;
using FluentAssertions;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace APICatalogoXunitTestee.UnitTestes;

public class PostProdutoUnitTests : IClassFixture<ProdutosUnitTestController>
{
    private readonly ProdutosController _controller;

    public PostProdutoUnitTests(ProdutosUnitTestController controller)
    {
        _controller = new ProdutosController(controller.repository, controller.mapper);
    }

    //metodos de testes para POST
    [Fact]
    public async Task PostProduto_Return_CreatedStatusCode()
    {
        // Arrange  
        var novoProdutoDto = new ProdutoDTO
        {
            Nome = "Novo Produto",
            Descricao = "Descrição do Novo Produto",
            Preco = 10.99m,
            ImagemUrl = "imagemfake1.jpg",
            CategoriaId = 2
        };

        // Act  
        var data = await _controller.Post(novoProdutoDto);

        // Assert  
        var createdResult = data.Result.Should().BeOfType<CreatedAtRouteResult>();
        createdResult.Subject.StatusCode.Should().Be(201);
    }

    [Fact]
    public async Task PosProduto_Return_BadRequest()
    {
        ProdutoDTO prod = null;

        //Act
        var data = await _controller.Post(prod);

        //Assert
        var badRequestResult = data.Result.Should().BeOfType<BadRequestResult>();
        badRequestResult.Subject.StatusCode.Should().Be(400);
    }
}

