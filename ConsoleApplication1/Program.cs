using ConsoleApplication1.DsProductsTableAdapters;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            SqlConnection conn = new SqlConnection(@"Data Source=(LocalDB)\SQLLocalDb;Initial Catalog=SportsStore;Integrated Security=True;Connect Timeout=15;Encrypt=False;TrustServerCertificate=False");
            
            //SP_Demo(conn);
            //await ReaderAccess(conn);
            DsProducts ds = new DsProducts();
            ProductsTableAdapter adapter = new ProductsTableAdapter();
            adapter.Connection = conn;
            conn.Open();
            adapter.Fill(ds.Products);
            ds.Products.AddProductsRow("Pen", "Pen with A Man", "Stationery", 12.50M);
            ds.WriteXml(@"C:\Lab\Products2.xml",System.Data.XmlWriteMode.DiffGram);
            adapter.Update(ds.Products);
            conn.Close();
            conn.Dispose();
        }

        private static void SP_Demo(SqlConnection conn)
        {
            var cmd = new SqlCommand("p_delete_product", conn);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@ProductID", 1));
            conn.Open();
            cmd.ExecuteNonQuery();
        }

        private static async Task ReaderAccess(SqlConnection conn)
        {
            SqlCommand cmd = new SqlCommand("Select * from dbo.products", conn);
            conn.Open();
            Task<SqlDataReader> waiter = cmd.ExecuteReaderAsync();
            Console.WriteLine("Fetching data");
            SqlDataReader reader = await waiter;
            while (reader.Read())
            {
                String output = String.Format("{0} - {1} - {2}", reader[0], reader[1], reader[2]);
                Console.WriteLine(output);
            }
        }
    }
}
