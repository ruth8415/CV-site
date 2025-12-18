using Microsoft.AspNetCore.Mvc;
using GmachAPI.Controllers;
using GmachAPI.Entities;

namespace GmachAPI.Tests
{
    public class BorrowersControllerTests
    {
        private readonly BorrowersController _borrowersController;

        public BorrowersControllerTests()
        {
            _borrowersController = new BorrowersController();
        }

        // Test 1: Check that GetAll returns OkObjectResult
        [Fact]
        public void GetAll_ReturnsOk()
        {
            // Arrange - nothing needed

            // Act
            var result = _borrowersController.GetAll();

            // Assert
            Assert.IsType<OkObjectResult>(result.Result);
        }

        // Test 2: Check that GetById returns OkObjectResult for existing ID
        [Fact]
        public void GetById_ExistingId_ReturnsOk()
        {
            // Arrange
            var id = 1;

            // Act
            var result = _borrowersController.GetById(id);

            // Assert
            Assert.IsType<OkObjectResult>(result.Result);
        }

        // Test 3: Check that GetById returns NotFound for non-existing ID
        [Fact]
        public void GetById_NonExistingId_ReturnsNotFound()
        {
            // Arrange
            var id = 999;

            // Act
            var result = _borrowersController.GetById(id);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result.Result);
        }

        // Test 4: Check that Create returns CreatedAtActionResult
        [Fact]
        public void Create_ValidBorrower_ReturnsCreatedAtAction()
        {
            // Arrange
            var newBorrower = new Borrower
            {
                FirstName = "Test",
                LastName = "User",
                Phone = "050-0000000",
                Email = "test@test.com",
                Address = "Test Address"
            };

            // Act
            var result = _borrowersController.Create(newBorrower);

            // Assert
            Assert.IsType<CreatedAtActionResult>(result.Result);
        }

        // Test 5: Check that Update returns OkObjectResult for existing ID
        [Fact]
        public void Update_ExistingId_ReturnsOk()
        {
            // Arrange
            var id = 1;
            var updatedBorrower = new Borrower
            {
                FirstName = "Updated",
                LastName = "Name",
                Phone = "050-1111111",
                Email = "updated@test.com",
                Address = "Updated Address"
            };

            // Act
            var result = _borrowersController.Update(id, updatedBorrower);

            // Assert
            Assert.IsType<OkObjectResult>(result.Result);
        }

        // Test 6: Check that Update returns NotFound for non-existing ID
        [Fact]
        public void Update_NonExistingId_ReturnsNotFound()
        {
            // Arrange
            var id = 999;
            var updatedBorrower = new Borrower
            {
                FirstName = "Updated",
                LastName = "Name"
            };

            // Act
            var result = _borrowersController.Update(id, updatedBorrower);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result.Result);
        }

        // Test 7: Check that Delete returns NoContent for existing ID
        [Fact]
        public void Delete_ExistingId_ReturnsNoContent()
        {
            // Arrange - First create a borrower to delete
            var newBorrower = new Borrower
            {
                FirstName = "ToDelete",
                LastName = "User",
                Phone = "050-9999999",
                Email = "delete@test.com",
                Address = "Delete Address"
            };
            var createResult = _borrowersController.Create(newBorrower);
            var createdBorrower = (createResult.Result as CreatedAtActionResult)?.Value as Borrower;
            var id = createdBorrower!.Id;

            // Act
            var result = _borrowersController.Delete(id);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        // Test 8: Check that Delete returns NotFound for non-existing ID
        [Fact]
        public void Delete_NonExistingId_ReturnsNotFound()
        {
            // Arrange
            var id = 999;

            // Act
            var result = _borrowersController.Delete(id);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }
    }
}
