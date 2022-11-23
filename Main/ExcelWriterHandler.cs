using Main.Utilities;
using IronXL;


namespace Main
{
    public class ExcelWriteHandler
    {
        WorkBook workbook;
        public ExcelWriteHandler()
        {
            workbook = WorkBook.Create(ExcelFileFormat.XLSX);
        }

        public void SetShopModelValues(IDictionary<string, List<ShopModel>> modelsDictionary)
        {
            foreach (var keyValuePair in modelsDictionary)
            {
                var sheet = workbook.CreateWorkSheet(keyValuePair.Key);

                sheet["A1"].Value = "Title";
                sheet["B1"].Value = "Brand";
                sheet["C1"].Value = "Id";
                sheet["D1"].Value = "Feedbacks";
                sheet["E1"].Value = "Price";

                for (int i = 2; i < keyValuePair.Value.Count + 2; i++)
                {
                    var model = keyValuePair.Value[i - 2];
                    sheet["A" + i].Value = model.title;
                    sheet["B" + i].Value = model.brand;
                    sheet["C" + i].Value = model.id;
                    sheet["D" + i].Value = model.feedbacks;
                    sheet["E" + i].Value = model.price;
                }
            }
        }

        public bool TrySave(string path)
        {
            try
            {
                workbook.SaveAs(path);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}