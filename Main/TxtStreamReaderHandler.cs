namespace Main
{
    public class TxtStreamReaderHandler : IDisposable
    {
        StreamReader streamReader;

        public TxtStreamReaderHandler(string path)
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
