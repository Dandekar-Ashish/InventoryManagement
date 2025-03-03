using CsvHelper;
using InventoryManagement.DataTransferModel;
using InventoryManagement.Mapper;
using System.Globalization;

namespace InventoryManagement.WebApi.Tests.Mapper
{
    public class MemberMapTests
    {
        [Fact]
        public void MembersMap_Should_Map_Csv_Columns_To_MemberDto_Properties_Correctly()
        {
            // Arrange
            var csv = "name,surname,booking_count,date_joined\n" +
                      "John,Doe,5,01/01/2020";
            var reader = new StringReader(csv);
            var csvReader = new CsvReader(reader, CultureInfo.InvariantCulture);

            csvReader.Context.RegisterClassMap<MembersMap>();

            // Act
            var records = csvReader.GetRecords<MemberDto>().ToList();

            // Assert
            var member = records.First();

            Assert.Equal("John", member.Name);
            Assert.Equal("Doe", member.Surname);
            Assert.Equal(5, member.BookingCount);
            Assert.Equal(DateTime.ParseExact("01/01/2020", "dd/MM/yyyy", CultureInfo.InvariantCulture), member.DateJoined);
        }
    }
}
