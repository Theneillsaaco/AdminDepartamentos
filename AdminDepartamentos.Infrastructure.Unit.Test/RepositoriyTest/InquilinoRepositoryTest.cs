using AdminDepartament.Infrastructure.Context;
using AdminDepartament.Infrastructure.Exceptions;
using AdminDepartament.Infrastructure.Repositories;
using AdminDepartamentos.Domain.Entities;
using AdminDepartamentos.Domain.Interfaces;
using AdminDepartamentos.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace AdminDepartamentos.Infrastructure.Unit.Test.RepositoriyTest;

public class InquilinoRepositoryTest
{
    #region Arrange

    private DepartContext _context = null;
    private IInquilinoRepository inquilinoRepository;
    
    public InquilinoRepositoryTest()
    {
        var options = new DbContextOptionsBuilder<DepartContext>()
            .UseInMemoryDatabase("GestionDepartamentos")
            .Options;

        _context = new DepartContext(options);

        inquilinoRepository = new InquilinoRepository(_context);

        #region Entidades de prueba

        

        #endregion
    }

    #endregion

    [Fact]
    public async Task GetInquilino_WithValidInquilinoModel()
    {
        // Arrange

        var inquilinoTask = inquilinoRepository.GetInquilinos();
        
        // Act

        var inquilinos = await inquilinoTask;
        
        // Expect

        var inquilinoFistName = "Luz";
        var inquilinoLastName = "Peguero";
        
        // Assert

        Assert.NotNull(inquilinos);
        Assert.IsType<List<InquilinoModel>>(inquilinos);
        Assert.True(inquilinos.Any());
        Assert.Equal(inquilinoFistName, inquilinos[0].FirstName);
        Assert.Equal(inquilinoLastName, inquilinos[0].LastName);
    }

    [Fact]
    public async Task Get_ValidInquilinoId_ReturnInquilino()
    {
        // Arrange

        var idInquilino = 1;

        var inquilinoTask = inquilinoRepository.GetById(idInquilino);
        
        // Act

        var inquilino = await inquilinoTask;
        
        // Expect

        var inquilinoFistName = "Luz";
        var inquilinoLastName = "Peguero";
        var InquilinoCedula = "999999999";
        
        // Assert

        Assert.NotNull(inquilino);
        Assert.IsType<Inquilino>(inquilino);
        Assert.Equal(inquilinoFistName, inquilino.FirstName);
        Assert.Equal(inquilinoLastName, inquilino.LastName);
        Assert.Equal(InquilinoCedula, inquilino.Cedula);
    }

    [Fact]
    public async Task Get_InvalidInquilinoId_ThrowsException()
    {
        // Arrange

        var invalidId = 20;
        
        // Act

        async Task<Inquilino> GetInquilinoTask()
        {
            return await inquilinoRepository.GetById(invalidId);
        }

        Func<Task> act = async () => await GetInquilinoTask();
        
        // Assert

        Assert.NotNull(act);
        await Assert.ThrowsAsync<InquilinoException>(act);
    }

    [Fact]
    public async Task Exists_ShouldReturnTrue_WhereMatchingInquilinoExists()
    {
        // Arrange

        var idInquilino = 1;
        
        // Act

        var exists = await inquilinoRepository.Exists(cd => cd.IdInquilino == idInquilino);
        
        // Assert
        
        Assert.True(exists);
    }
    
    [Fact]
    public async Task Exists_ShouldReturnFalse_WhereMatchingInquilinoExists()
    {
        // Arrange

        var idInquilino = 10;
        
        // Act

        var exists = await inquilinoRepository.Exists(cd => cd.IdInquilino == idInquilino);
        
        // Assert
        
        Assert.False(exists);
    }

    [Fact]
    public async Task Save_NewInquilino_ShouldSaveInquilino()
    {
        // Arrange 

        
        
        // Act
        
        
        // Expect
        
        
        // Assert
        
    }

    [Fact]
    public async Task Update_ExistingInquilino_ShouldUpdateInquilino()
    {
        // Arrange

        var inquilinoToUpdate = await inquilinoRepository.GetById(2);
        inquilinoToUpdate.FirstName = "Pedro";
        
        // Act

        await inquilinoRepository.Update(inquilinoToUpdate);
        
        // Assert

        var inquilinoUpdated = await inquilinoRepository.GetById(2);

        Assert.NotNull(inquilinoUpdated);
        Assert.IsType<Inquilino>(inquilinoUpdated);
        Assert.Equal(inquilinoToUpdate.FirstName, inquilinoUpdated.FirstName);
        Assert.Equal(inquilinoToUpdate.LastName, inquilinoUpdated.LastName);
    }
}