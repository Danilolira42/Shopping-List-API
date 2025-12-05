using ClosedXML.Excel;
using Management.API.ModelDTO;

namespace ShoppingList.API.Services
{
    public class ExportExcel
    {
        public byte[] ExportToExcel(List<Product> products)
        {
            using (var wBook = new XLWorkbook())
            {
                var wSheet = wBook.Worksheets.Add("Products");

                wSheet.Cell(1, 1).Value = "ProductID";
                wSheet.Cell(1, 1).Style.Font.Bold = true;
                wSheet.Cell(1, 1).Style.Fill.BackgroundColor = XLColor.FromHtml("#CA3884");
                wSheet.Cell(1, 1).Style.Font.FontColor = XLColor.White;

                wSheet.Cell(1, 2).Value = "ProductName";
                wSheet.Cell(1, 2).Style.Font.Bold = true;
                wSheet.Cell(1, 2).Style.Fill.BackgroundColor = XLColor.FromHtml("#CA3884");
                wSheet.Cell(1, 2).Style.Font.FontColor = XLColor.White;

                wSheet.Cell(1, 3).Value = "RegistrationDate";
                wSheet.Cell(1, 3).Style.Font.Bold = true;
                wSheet.Cell(1, 3).Style.Fill.BackgroundColor = XLColor.FromHtml("#CA3884");
                wSheet.Cell(1, 3).Style.Font.FontColor = XLColor.White;

                int row = 2;
                foreach(var p in products)
                {
                    wSheet.Cell(row, 1).Value = p.ProductId.ToString();
                    wSheet.Cell(row, 2).Value = p.ProductName.ToString();
                    wSheet.Cell(row, 3).Value = p.RegistrationDate.ToShortDateString();
                    wSheet.Cell(row, 3).Style.DateFormat.Format = "dd/MM/yyyy";

                    row++;
                }

                wSheet.Columns().AdjustToContents();

                using (var stream = new MemoryStream()){
                    
                    wBook.SaveAs(stream);

                    var content = stream.ToArray();

                    return content;
                }
            }
        }
    }
}

