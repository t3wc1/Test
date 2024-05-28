using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using Test.Models;

namespace Test.Controllers
{
    [Route ("api/[controller]")]
    [ApiController]
    public class ProductoController : Controller
    {
        private readonly string CadenaSQL;
        public ProductoController(IConfiguration configuration)
        {
            CadenaSQL = configuration.GetConnectionString("CadenaSQL");
        }


        [HttpPost]
        [Route("Save")]
        public IActionResult Save([FromBody] Producto producto)
        {
            try
            {
                using (var conexion = new SqlConnection(CadenaSQL))
                {
                    conexion.Open();
                    var cmd = new SqlCommand("sp_Insertar_Producto", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("nompro", producto.nompro);
                    cmd.Parameters.AddWithValue("precio", producto.precio);
                    cmd.Parameters.AddWithValue("categoria", producto.categoria);
                    cmd.Parameters.AddWithValue("codcat", producto.codcat);
                    cmd.ExecuteNonQuery();
                    return StatusCode(StatusCodes.Status200OK, new { mensaje = "OK", response = producto });
                }
            }catch (Exception error)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, new { error.Message, response = producto });
            }
        }
        [HttpGet]
        [Route("List")]
        public IActionResult List()
        {
            List<Producto> listar=new List<Producto>();
            try
            {
                using (var conexion = new SqlConnection(CadenaSQL))
                { 
                    conexion.Open() ;
                    var cmd = new SqlCommand("sp_Listar_Producto", conexion);
                    cmd.CommandType= CommandType.StoredProcedure;
                    using (var reader=cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            listar.Add(new Producto
                            {
                                codcat = reader["codcat"].ToString(),
                                precio = reader["precio"].ToString(),
                                categoria = reader["codcat"].ToString(),
                                nompro = reader["nompro"].ToString()
                            });
                        }
                        
                    }

                }
                return StatusCode(StatusCodes.Status200OK, new { mensaje = "OK", response = listar });

            }
            catch (Exception error)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje=error.Message, response = listar });

            }


        }


    }
}
