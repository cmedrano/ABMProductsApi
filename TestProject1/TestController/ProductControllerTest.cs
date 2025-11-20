using Microsoft.AspNetCore.Mvc;
using Moq;
using Products.Api.Controllers;
using Products.Application.Dtos;
using Products.Application.IServices;
using System;
using System.Collections.Generic;
using System.Text;

namespace TestProject1.TestController
{
    public class ProductControllerTest
    {
        private readonly Mock<IProductService> _mockProductService;
        private readonly ProductsController _controller;

        public ProductControllerTest()
        {
            _mockProductService = new Mock<IProductService>();
            _controller = new ProductsController(_mockProductService.Object);
        }

        [Fact]

        public async Task GetAllProducts_WhenProductsExist_ReturnsOk()
        {
            //Arrange
            var responseProduct = CreateListProductDtoSample();

            _mockProductService.Setup(service => service.getAllProducts())
                .ReturnsAsync(responseProduct);

            //Act
            var result = await _controller.GetAllProducts();

            //Assert
            Assert.NotNull(result);

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.NotNull(okResult);
            Assert.Equal(200, okResult.StatusCode);

            var returnProducts = Assert.IsType<List<ProductDto>>(okResult.Value);
            Assert.Equal(responseProduct.Count, returnProducts.Count);

            // Verifico que se llamo al servicio
            _mockProductService.Verify(service => service.getAllProducts(), Times.Once);
        }

        [Fact]
        public async Task GetAllProducts_WhenServiceThrowsError_ReturnsInternalServerError()
        {
            //Arrange
            var exceptionMessage = "Database connection failed";

            _mockProductService.Setup(service => service.getAllProducts())
                .ThrowsAsync(new Exception(exceptionMessage));

            //Act
            var result = await _controller.GetAllProducts();

            //Assert
            Assert.NotNull(result);

            var errorResult = Assert.IsType<ObjectResult>(result);
            Assert.NotNull(errorResult);
            Assert.Equal(500, errorResult.StatusCode);
            Assert.Contains(exceptionMessage, errorResult.Value!.ToString());
        }

        [Fact]
        public async Task GetProductById_WhenProductFound_ReturnsOk()
        {
            //Arrange
            int IdProduct = 1;
            var responseProduct = CreateProductDtoSample(IdProduct);

            _mockProductService.Setup(service => service.getProductById(IdProduct))
                .ReturnsAsync(responseProduct);

            //Act
            var result = await _controller.GetProductById(1);

            //Assert
            Assert.NotNull(result);

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.NotNull(okResult);
            Assert.Equal(200, okResult.StatusCode);

            var returnProduct = Assert.IsType<ProductDto>(okResult.Value);
            Assert.NotNull(returnProduct);
            Assert.Equal(responseProduct.Id, IdProduct);

            // Verifico que se llamo al servicio
            _mockProductService.Verify(service => service.getProductById(It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public async Task GetProductById_WhenProductNotFound_ReturnsNotFound()
        {
            //Arrange
            int IdProduct = 1;

            _mockProductService.Setup(service => service.getProductById(IdProduct))
                .ReturnsAsync((ProductDto?)null);

            //Act
            var result = await _controller.GetProductById(IdProduct);

            //Assert
            Assert.NotNull(result);

            var notFoundResult = Assert.IsType<NotFoundResult>(result);
            Assert.NotNull(notFoundResult);
            Assert.Equal(404, notFoundResult.StatusCode);

            // Verifico que se llamo al servicio
            _mockProductService.Verify(service => service.getProductById(It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public async Task GetProductById_WhenServiceThrowsError_ReturnsInternalServerError()
        {
            //Arrange
            int IdProduct = 1;

            var exceptionMessage = "Database connection failed";

            _mockProductService.Setup(service => service.getProductById(IdProduct))
                .ThrowsAsync(new Exception(exceptionMessage));

            //Act
            var result = await _controller.GetProductById(IdProduct);

            //Assert
            Assert.NotNull(result);

            var errorResult = Assert.IsType<ObjectResult>(result);
            Assert.NotNull(errorResult);
            Assert.Equal(500, errorResult.StatusCode);
            Assert.Contains(exceptionMessage, errorResult.Value!.ToString());

            // Verifico que se llamo al servicio
            _mockProductService.Verify(service => service.getProductById(It.IsAny<int>()), Times.Once);
        }

        // Metodos Auxiliares
        private List<ProductDto> CreateListProductDtoSample()
        {
            return new List<ProductDto>
                {
                    new ProductDto
                    {
                        Id = 1,
                        Name = "Product 1",
                        Description = "Description 1",
                        Categories = new List<CategoryDto>
                        {
                            new CategoryDto { Id = 1, Name = "Category 1" },
                            new CategoryDto { Id = 2, Name = "Category 2" }
                        }
                    },
                    new ProductDto
                    {
                        Id = 2,
                        Name = "Product 2",
                        Description = "Description 2",
                        Categories = new List<CategoryDto>
                        {
                            new CategoryDto { Id = 3, Name = "Category 3" }
                        }
                    }
                };
        }

        private ProductDto CreateProductDtoSample(int Id)
        {
            return new ProductDto
            {
                Id = Id,
                Name = "Product 1",
                Description = "Description 1",
                Categories = new List<CategoryDto>
                        {
                            new CategoryDto { Id = 1, Name = "Category 1" },
                            new CategoryDto { Id = 2, Name = "Category 2" }
                        }
            };
        }
    }
}
