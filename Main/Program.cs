using Main.Utilities;

namespace Main
{
    public class Program
    {
        public static void Main()
        {
            string[] titles;
            using (var txtReader = new TxtStreamReaderHandler(Consts.txtPath))
            {
                titles = txtReader.GetTitles();
            }

            var shopReader = new ShopReader(Consts.webPath);
            var data = new Dictionary<string, List<ShopModel>>();
            foreach (var title in titles)
            {
                data[title] = shopReader.GetTitleModels(title);
            }


        }
    }
}
