﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace APICatalogo.Migrations
{
    /// <inheritdoc />
    public partial class PopulaCategorias : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder mb)
        {
            mb.Sql("insert into Produtos(Nome,Descricao,Preco,ImagemUrl,Estoque,DataCadastro,CategoriaId) " + "Values('Coca-Cola Diet','Refrigerante de Cola 350ml',5.45,'cocacola.jpg',50,now(),1)");

            mb.Sql("insert into Produtos(Nome,Descricao,Preco,ImagemUrl,Estoque,DataCadastro,CategoriaId) " + "Values('Lanche de Atum','Lanche de atum com maionese',8.50,'atum.jpg',10,now(),2)");

            mb.Sql("insert into Produtos(Nome,Descricao,Preco,ImagemUrl,Estoque,DataCadastro,CategoriaId) " + "Values('Pudim 100 g','Pudim de leite condensado 100g',6.75,'pudim.jpg',50,now(),3)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder mb)
        {

        }
    }
}
