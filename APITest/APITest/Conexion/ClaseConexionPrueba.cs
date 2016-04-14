using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using APITest.Models;
using Microsoft.Owin.Security.OAuth;

namespace APITest.Conexion
{
     class ClaseConexionPrueba
    {
         private int valor { get; set; } 
         public ClaseConexionPrueba(int _valor) {
             this.valor = _valor;
         }
        public string Prueba(){
            string strConnection="";
            strConnection = (this.valor == 1 ? "DefaultConnection" : "JadeCore1");
            using (SqlConnection connection = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings[strConnection].ConnectionString))
            {
                try
                {
                    DataTable DatosResultados = new DataTable();
                    connection.Open();
                    SqlCommand command = new SqlCommand("select getdate(),'"+ strConnection +"'", connection);
                    command.CommandType = CommandType.Text;
                    SqlDataAdapter da = new SqlDataAdapter(command);
                    da.Fill(DatosResultados);

                    if (DatosResultados.Rows.Count > 0) {
                        return "Todo chevere " + DatosResultados.Rows[0].Field<DateTime>(0).ToString()+ " " + DatosResultados.Rows[0].Field<string>(1).ToString();
                    }
                    return "No retorno datos la sentencia SQL";
                }
                catch (SqlException ex)
                {
                    return ex.Message;
                }

            }
         
        }

        
    }
}