using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace disKlinigiOtomasyonu
{
    class ConnectionString
    {
        public SqlConnection GetCon()
        {
            SqlConnection baglanti = new SqlConnection();
            baglanti.ConnectionString = @"Data Source=ESAT;Initial Catalog=DisDB;Integrated Security=True;Pooling=False;Encrypt=True;TrustServerCertificate=True";
            return baglanti;
        }
    }
}
