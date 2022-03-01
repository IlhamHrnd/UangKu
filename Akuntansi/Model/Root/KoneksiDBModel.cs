namespace Akuntansi.Model.Root
{
    public class KoneksiDBModel
    {
        private string datadb = "Server = srv93.niagahoster.com;Database = u1031129_UangKu;Port = 3306;Username = u1031129_UangKu;Password = z27f&Dctd8iM;Port = 3306;SslMode = Preferred";

        public string DataDB
        {
            get { return datadb; }
            set { datadb = value; }
        }
    }
}
