namespace Main
{
    public class TxtReadHandler : IDisposable
    {
        StreamReader streamReader;

        public TxtReadHandler(string path)
        {
            streamReader = new StreamReader(path);
        }

        public string[] GetTitles() => streamReader.ReadToEnd().Split("\r\n");

        public void Dispose()
        {
            streamReader.Dispose();
        }
    }
}
