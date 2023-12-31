﻿using Moq;
using ProductManager.Domain.Entities.Product;
using ProductManager.Domain.Repositories.Common;

namespace ProductManager.Test.Mocks;

public static class MockProductRepository
{
    public static Mock<IGenericRepository<Product>> GetProductRepository()
    {
        var products = new List<Product>()
        {
            new()
            {
                Id = Guid.Parse("e4858aff-77e3-477e-2d94-08dbfb53603b"),
                Name = "Galaxy A7",
                ManufacturerEmail = "samsung@gmail.com",
                ManufacturerPhone = "09215483851",
                IsAvailable = true,
                CreateDate = DateOnly.FromDateTime(DateTime.Now).AddDays(-3),
                LastModifiedDate = DateOnly.FromDateTime(DateTime.Now.AddDays(-2)),
                CreatorUserName = "arminfrzm72"
            },
            new()
            {
                Id = Guid.Parse("71d7f859-d796-4a3b-2d97-08dbfb53603b"),
                Name = "Galaxy A5",
                ManufacturerEmail = "samsung2@gmail.com",
                ManufacturerPhone = "09215483852",
                IsAvailable = true,
                CreateDate = DateOnly.FromDateTime(DateTime.Now).AddDays(-5),
                LastModifiedDate = DateOnly.FromDateTime(DateTime.Now.AddDays(-3)),
                CreatorUserName = "arminfrzm73"
            }
        };
        var mockRepo = new Mock<IGenericRepository<Product>>();
        mockRepo.Setup(r => r.GetEntitiesQueryable()).Returns(() => products.AsQueryable());
        mockRepo.Setup(r => r.GetEntityById(It.IsAny<Guid>()))
            .Returns((Guid productId) => Task.FromResult(products.FirstOrDefault(p => p.Id == productId)));
        mockRepo.Setup(r => r.AddEntity(It.IsAny<Product>())).Returns((Product product) =>
        {
            products.Add(product);
            return Task.FromResult(products);
        });
        mockRepo.Setup(r => r.UpdateEntity(It.IsAny<Product>())).Callback((Product updatedProduct) =>
        {
            var existingProduct = products.FirstOrDefault(p => p.Id == updatedProduct.Id);
            if (existingProduct != null)
            {
                existingProduct.Name = updatedProduct.Name;
                existingProduct.ManufacturerEmail = updatedProduct.ManufacturerEmail;
                existingProduct.ManufacturerPhone = updatedProduct.ManufacturerPhone;
                existingProduct.IsAvailable = updatedProduct.IsAvailable;
            }
        });
        mockRepo.Setup(r => r.RemoveEntity(It.IsAny<Product>(), It.IsAny<bool>()))
            .Callback((Product productToRemove,bool forceDelete) => 
            {
                products.Remove(productToRemove);
            });
        return mockRepo;
    }
}
