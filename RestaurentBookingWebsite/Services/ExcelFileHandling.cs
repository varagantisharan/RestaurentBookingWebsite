using ClosedXML.Excel;
using RestaurentBookingWebsite.DbModels;

namespace RestaurentBookingWebsite.Services
{
    public class ExcelFileHandling
    {
        
        public MemoryStream CreateExcelFile(List<Booking> bookings)
        {
            //Create an Instance of Workbook, i.e., Creates a new Excel workbook
            var workbook = new XLWorkbook();
            //Add a Worksheets with the workbook
            //Worksheets name is Employees
            IXLWorksheet worksheet = workbook.Worksheets.Add("Bookings");
            //Create the Cell
            //First Row is going to be Header Row
            worksheet.Cell(1, 1).Value = "BookingId"; //First Row and First Column
            worksheet.Cell(1, 2).Value = "CustomerId"; //First Row and Second Column
            worksheet.Cell(1, 3).Value = "BookingDate"; //First Row and Third Column
            worksheet.Cell(1, 4).Value = "SlotTime"; //First Row and Fourth Column
            worksheet.Cell(1, 5).Value = "Status"; //First Row and Fifth Column
            worksheet.Cell(1, 6).Value = "CreationTime"; //First Row and Sixth Column
            //Data is going to stored from Row 2
            int row = 2;
            //Loop Through Each Employees and Populate the worksheet
            //For Each Employee increase row by 1
            foreach (var book in bookings)
            {
                worksheet.Cell(row, 1).Value = book.BookingId;
                worksheet.Cell(row, 2).Value = book.CustomerId;
                worksheet.Cell(row, 3).Value = book.BookingDate;
                worksheet.Cell(row, 4).Value = book.SlotTime;
                worksheet.Cell(row, 5).Value = book.Status;
                worksheet.Cell(row, 6).Value = book.CreationTime;
                row++; //Increasing the Data Row by 1
            }
            //Create an Memory Stream Object
            var stream = new MemoryStream();
            //Saves the current workbook to the Memory Stream Object.
            workbook.SaveAs(stream);
            //The Position property gets or sets the current position within the stream.
            //This is the next position a read, write, or seek operation will occur from.
            stream.Position = 0;
            return stream;
        }
    }
}
