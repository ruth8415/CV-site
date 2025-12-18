using Microsoft.AspNetCore.Mvc;
using GmachAPI.Controllers;
using GmachAPI.Entities;

namespace GmachAPI.Tests
{
    public class LoansControllerTests
    {
        private readonly LoansController _loansController;

        public LoansControllerTests()
        {
            _loansController = new LoansController();
        }

        // Test 1: Check that GetAll returns OkObjectResult
        [Fact]
        public void GetAll_ReturnsOk()
        {
            // Arrange - nothing needed

            // Act
            var result = _loansController.GetAll(null, null);

            // Assert
            Assert.IsType<OkObjectResult>(result.Result);
        }

        // Test 2: Check that GetAll with status filter returns OkObjectResult
        [Fact]
        public void GetAll_WithStatusFilter_ReturnsOk()
        {
            // Arrange
            var status = "active";

            // Act
            var result = _loansController.GetAll(status, null);

            // Assert
            Assert.IsType<OkObjectResult>(result.Result);
        }

        // Test 3: Check that GetById returns OkObjectResult for existing ID
        [Fact]
        public void GetById_ExistingId_ReturnsOk()
        {
            // Arrange
            var id = 1;

            // Act
            var result = _loansController.GetById(id);

            // Assert
            Assert.IsType<OkObjectResult>(result.Result);
        }

        // Test 4: Check that GetById returns NotFound for non-existing ID
        [Fact]
        public void GetById_NonExistingId_ReturnsNotFound()
        {
            // Arrange
            var id = 999;

            // Act
            var result = _loansController.GetById(id);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result.Result);
        }

        // Test 5: Check that Create returns CreatedAtActionResult
        [Fact]
        public void Create_ValidLoan_ReturnsCreatedAtAction()
        {
            // Arrange
            var newLoan = new Loan
            {
                ItemId = 1,
                BorrowerId = 1,
                DueDate = DateTime.Now.AddDays(14),
                Notes = "Test loan"
            };

            // Act
            var result = _loansController.Create(newLoan);

            // Assert
            Assert.IsType<CreatedAtActionResult>(result.Result);
        }

        // Test 6: Check that Update returns OkObjectResult for existing ID
        [Fact]
        public void Update_ExistingId_ReturnsOk()
        {
            // Arrange
            var id = 1;
            var updatedLoan = new Loan
            {
                ItemId = 2,
                BorrowerId = 2,
                DueDate = DateTime.Now.AddDays(21),
                Notes = "Updated loan"
            };

            // Act
            var result = _loansController.Update(id, updatedLoan);

            // Assert
            Assert.IsType<OkObjectResult>(result.Result);
        }

        // Test 7: Check that Update returns NotFound for non-existing ID
        [Fact]
        public void Update_NonExistingId_ReturnsNotFound()
        {
            // Arrange
            var id = 999;
            var updatedLoan = new Loan
            {
                ItemId = 1,
                BorrowerId = 1
            };

            // Act
            var result = _loansController.Update(id, updatedLoan);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result.Result);
        }

        // Test 8: Check that ReturnLoan returns OkObjectResult for active loan
        [Fact]
        public void ReturnLoan_ActiveLoan_ReturnsOk()
        {
            // Arrange - Create a new loan to return
            var newLoan = new Loan
            {
                ItemId = 1,
                BorrowerId = 1,
                DueDate = DateTime.Now.AddDays(14)
            };
            var createResult = _loansController.Create(newLoan);
            var createdLoan = (createResult.Result as CreatedAtActionResult)?.Value as Loan;
            var id = createdLoan!.Id;

            // Act
            var result = _loansController.ReturnLoan(id);

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }

        // Test 9: Check that ReturnLoan returns NotFound for non-existing ID
        [Fact]
        public void ReturnLoan_NonExistingId_ReturnsNotFound()
        {
            // Arrange
            var id = 999;

            // Act
            var result = _loansController.ReturnLoan(id);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }

        // Test 10: Check that Delete returns NoContent for existing ID
        [Fact]
        public void Delete_ExistingId_ReturnsNoContent()
        {
            // Arrange - Create a loan to delete
            var newLoan = new Loan
            {
                ItemId = 1,
                BorrowerId = 1,
                DueDate = DateTime.Now.AddDays(14)
            };
            var createResult = _loansController.Create(newLoan);
            var createdLoan = (createResult.Result as CreatedAtActionResult)?.Value as Loan;
            var id = createdLoan!.Id;

            // Act
            var result = _loansController.Delete(id);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        // Test 11: Check that Delete returns NotFound for non-existing ID
        [Fact]
        public void Delete_NonExistingId_ReturnsNotFound()
        {
            // Arrange
            var id = 999;

            // Act
            var result = _loansController.Delete(id);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }
    }
}
