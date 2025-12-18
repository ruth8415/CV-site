using Microsoft.AspNetCore.Mvc;
using GmachAPI.Controllers;
using GmachAPI.Entities;

namespace GmachAPI.Tests
{
    public class ItemsControllerTests
    {
        private readonly ItemsController _itemsController;

        public ItemsControllerTests()
        {
            _itemsController = new ItemsController();
        }

        // Test 1: Check that GetAll returns OkObjectResult
        [Fact]
        public void GetAll_ReturnsOk()
        {
            // Arrange - nothing needed

            // Act
            var result = _itemsController.GetAll(null);

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
            var result = _itemsController.GetById(id);

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
            var result = _itemsController.GetById(id);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result.Result);
        }

        // Test 4: Check that GetAll with status filter returns OkObjectResult
        [Fact]
        public void GetAll_WithStatusFilter_ReturnsOk()
        {
            // Arrange
            var status = "available";

            // Act
            var result = _itemsController.GetAll(status);

            // Assert
            Assert.IsType<OkObjectResult>(result.Result);
        }

        // Test 5: Check that Create returns CreatedAtActionResult
        [Fact]
        public void Create_ValidItem_ReturnsCreatedAtAction()
        {
            // Arrange
            var newItem = new Item
            {
                Name = "Test Item",
                Description = "Test Description",
                Category = "Test Category",
                Quantity = 1
            };

            // Act
            var result = _itemsController.Create(newItem);

            // Assert
            Assert.IsType<CreatedAtActionResult>(result.Result);
        }

        // Test 6: Check that Update returns OkObjectResult for existing ID
        [Fact]
        public void Update_ExistingId_ReturnsOk()
        {
            // Arrange
            var id = 1;
            var updatedItem = new Item
            {
                Name = "Updated Item",
                Description = "Updated Description",
                Category = "Updated Category",
                Quantity = 5
            };

            // Act
            var result = _itemsController.Update(id, updatedItem);

            // Assert
            Assert.IsType<OkObjectResult>(result.Result);
        }

        // Test 7: Check that Update returns NotFound for non-existing ID
        [Fact]
        public void Update_NonExistingId_ReturnsNotFound()
        {
            // Arrange
            var id = 999;
            var updatedItem = new Item
            {
                Name = "Updated Item"
            };

            // Act
            var result = _itemsController.Update(id, updatedItem);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result.Result);
        }

        // Test 8: Check that Delete returns NoContent for existing ID
        [Fact]
        public void Delete_ExistingId_ReturnsNoContent()
        {
            // Arrange - First create an item to delete
            var newItem = new Item
            {
                Name = "To Delete",
                Description = "Will be deleted",
                Category = "Test"
            };
            var createResult = _itemsController.Create(newItem);
            var createdItem = (createResult.Result as CreatedAtActionResult)?.Value as Item;
            var id = createdItem!.Id;

            // Act
            var result = _itemsController.Delete(id);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        // Test 9: Check that Delete returns NotFound for non-existing ID
        [Fact]
        public void Delete_NonExistingId_ReturnsNotFound()
        {
            // Arrange
            var id = 999;

            // Act
            var result = _itemsController.Delete(id);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }

        // Test 10: Check GetAvailability returns OkObjectResult for existing ID
        [Fact]
        public void GetAvailability_ExistingId_ReturnsOk()
        {
            // Arrange
            var id = 1;

            // Act
            var result = _itemsController.GetAvailability(id);

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }

        // Test 11: Check GetAvailability returns NotFound for non-existing ID
        [Fact]
        public void GetAvailability_NonExistingId_ReturnsNotFound()
        {
            // Arrange
            var id = 999;

            // Act
            var result = _itemsController.GetAvailability(id);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }
    }
}
