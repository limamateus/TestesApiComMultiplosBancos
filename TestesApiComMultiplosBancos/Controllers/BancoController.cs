using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using System.Data;
using System.Net;
using TestesApiComMultiplosBancos.Database;

namespace TestesApiComMultiplosBancos.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BancoController : ControllerBase
    {
        private string connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=NomeDoBanco;Integrated Security=True;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False";

        [HttpGet]

        public async Task<IActionResult> Get([FromHeader] string nomeDoBanco, [FromHeader] string tabela)
        {
            try
            {
                string query = $"SELECT * FROM {tabela}";
                DataTable dataTable = new DataTable();

                using (SqlConnection connection = new SqlConnection(connectionString.Replace("NomeDoBanco", nomeDoBanco)))
                {
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        connection.Open();
                        SqlDataAdapter adapter = new SqlDataAdapter(command);
                        adapter.Fill(dataTable);
                    }
                }

                List<Dictionary<string, object>> result = new List<Dictionary<string, object>>();

                foreach (DataRow row in dataTable.Rows)
                {
                    Dictionary<string, object> item = new Dictionary<string, object>();

                    foreach (DataColumn column in dataTable.Columns)
                    {
                        item.Add(column.ColumnName, row[column]);
                    }

                    result.Add(item);
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }

}


