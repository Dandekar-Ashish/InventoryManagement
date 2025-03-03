using CsvHelper;
using CsvHelper.Configuration;
using InventoryManagement.DataTransferModel;
using InventoryManagement.Mapper;
using Moq;
using System;
using System.Globalization;
using System.IO;
using Xunit;

namespace InventoryManagement.WebApi.Tests.Mapper
{
    public class InventoryMapTests
    {
        [Fact]
        public void InventoryMap_Should_Map_Csv_Columns_To_InventoryDto_Properties_Correctly()
        {
            // Arrange
            var csv = "title,description,remaining_count,expiration_date\n" +
                      "Test Title,Test Description,100,01/01/2023";  // Example CSV content
            var reader = new StringReader(csv);
            var csvReader = new CsvReader(reader, CultureInfo.InvariantCulture);

            csvReader.Context.RegisterClassMap<InventoryMap>();

            // Act
            var records = csvReader.GetRecords<InventoryDto>();

            // Assert
            var inventory = records.First();

            // Check if the mapping works correctly
            Assert.Equal("Test Title", inventory.Title);
            Assert.Equal("Test Description", inventory.Description);
            Assert.Equal(100, inventory.RemainingCount);
            Assert.Equal(DateTime.ParseExact("01/01/2023", "dd/MM/yyyy", CultureInfo.InvariantCulture), inventory.ExpirationDate);
        }
    }
}