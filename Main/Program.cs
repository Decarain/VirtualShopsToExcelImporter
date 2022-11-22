using Main.Utilities;

namespace Main
{
    public class Program
    {
        public static void Main()
        {
            var shopReader = new ShopReader(Consts.mainPath);
            shopReader.GetTitleModels("Amogus");
        }
    }
}
