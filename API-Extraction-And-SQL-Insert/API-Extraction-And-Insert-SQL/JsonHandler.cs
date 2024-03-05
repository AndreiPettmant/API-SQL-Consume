using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace API_Extraction_And_Insert_SQL
{
    public class JsonHandler
    {
        public static async Task GetJson(string connectionString, string apiKey, string aplication, string url)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();

                string json = await GetJsonFromApi(url, apiKey, aplication);
                RootObject jsonDeserialized;

                try
                {
                    jsonDeserialized = JsonConvert.DeserializeObject<RootObject>(json);
                }
                catch (Exception exc)
                {

                    throw new Exception(exc.Message);
                }

                foreach (var value in jsonDeserialized.objects)
                {
                    if (value.active)
                        InsertActiveProduct(connection, value.nickname, value.id, value.sku);
                }
            }
        }

        static async Task<string> GetJsonFromApi(string url, string apiKey, string aplication)
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Authorization", $"api_key {apiKey} aplication {aplication}");

                HttpResponseMessage response = await client.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsStringAsync();
                }
                else
                {
                    throw new Exception($"Error fetching data from API. Status code: {response.StatusCode}");   
                }
            }
        }

        static void InsertActiveProduct(SqlConnection connection, string nickname, int id, string sku)
        {
            SqlCommand sqlCommand = new SqlCommand();

            sqlCommand.Connection = connection;
            sqlCommand.CommandText = "usp_InsertActiveProducts";
            sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;


            sqlCommand.Parameters.AddWithValue("@product_nickname_VC", nickname == null ? DBNull.Value : nickname);
            sqlCommand.Parameters.AddWithValue("@product_id_IN", id == null ? DBNull.Value : id);
            sqlCommand.Parameters.AddWithValue("@product_SKU", sku == null ? DBNull.Value : sku);

            sqlCommand.ExecuteNonQuery();
        }
    }

    public class RootObject
    {
        public Meta meta { get; set;}
        public Object[] objects { get; set;} 

    }

    public class  Meta
    { 

    }

    public class Object
    {
        public string nickname { get; set; }
        public bool active { get; set; }
        public bool blocked { get; set; }
        public string[] categories { get; set; }
        public string complete_description { get; set; }
        public string[] grids { get; set; }
        public string gtin { get; set; }
        public int id { get; set; }
        public object external_id { get; set; }
        public string mpn { get; set; }
        public string ncm { get; set; }
        public string name { get; set; }
        public bool removed { get; set; }
        public string resource_uri { get; set; }
        public string seo { get; set; }
        public string sku { get; set; }
        public string type { get; set; }
        public string url { get; set; }
        public string youtube_url { get; set; }
        public string[] variations { get; set; }
        public string[] childrens { get; set; }
    }
}
