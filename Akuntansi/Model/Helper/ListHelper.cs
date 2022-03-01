using System.Collections.Generic;

namespace Akuntansi.Model.Helper
{
    public class ListHelper
    {
        public ListHelper()
        {
            Pemasukan = new List<string>()
            {
                "Bonus",
                "Bulanan",
                "Gaji",
                "Lain - Lain"
            };

            Pengeluaran = new List<string>()
            {
                "Belanja",
                "Lain - Lain",
                "Makanan",
                "Minuman",
                "Transportasi"
            };
        }

        public List<string> Pemasukan { get; set; }
        public List<string> Pengeluaran { get; set; }
    }
}
