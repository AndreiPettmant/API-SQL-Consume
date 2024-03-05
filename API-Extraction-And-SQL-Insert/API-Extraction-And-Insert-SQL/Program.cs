using System.Data.SqlClient;

namespace API_Extraction_And_Insert_SQL
{
    internal class Program
    {
        public static SqlConnection Conexao = null;
        /// <summary>
        /// IP of the database server, can be localhost.
        /// </summary>
        public static string serverIP = "1.123.456.78";

        /// <summary>
        /// Database name.
        /// </summary>
        public static string dbName = "CoolDBName";

        /// <summary>
        /// Username for database access, e.g. JOHN.DOE.
        /// </summary>
        public static string dbUser = "JOHN.DOE";

        /// <summary>
        /// Password to access the database.
        /// </summary>
        public static string dbPassword = "123";

        /// <summary>
        /// Persist Security Info parameter of ConnectionString. By default True.
        /// </summary>
        public static bool persistCredentials = true;

        /// <summary>
        /// Persist Security Info parameter of ConnectionString. By default True.
        /// </summary
        public static string apiUrl = "https://api.awsli.com.br/api/v1/produto";

        static async Task Main(string[] args)
        {
            string connectionString = string.Format("Initial Catalog={0};Data Source={1};Uid={2};Pwd={3};persist security info={4};Application Name={5};TrustServerCertificate=true", dbName, serverIP, dbUser, dbPassword, persistCredentials);
            string apiKey = "API-KEY-PLACEHOLDER";
            string aplication = "Aplication";

            JsonHandler.GetJson(connectionString, apiKey, aplication, apiUrl);
        }
    }
}
