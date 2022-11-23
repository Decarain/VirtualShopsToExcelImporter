using Main.Utilities;

namespace Main
{
    public class Program
    {
        public static void Main()
        {
            string[] titles;
            using (var txtReader = new TxtReadHandler(Consts.txtPath))
            {
                titles = txtReader.GetTitles();
            }

            var data = new Dictionary<string, List<ShopModel>>();
            using (var shopReader = new ShopReader(Consts.webPath))
            {

                foreach (var title in titles)
                {
                    data[title] = shopReader.GetTitleModels(title);
                }
            }

            var excelWriterHandler = new ExcelWriteHandler();
            excelWriterHandler.SetShopModelValues(data);
            excelWriterHandler.TrySave(Consts.xlsxPath);
        }
    }
}
