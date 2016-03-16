using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace APITest.Extensiones
{
    public static class AccionesManuales
    {
        public static string AgregarEmpresa(string UserId, int CodigoEmpresa)
        {
            using (SqlConnection connection = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {
                try
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand("Update AspNetUsers set CodigoEmpresa=@CodigoEmpresa Where Id=@UserId", connection);
                    command.CommandType = CommandType.Text;
                    command.Parameters.Add(new SqlParameter("@UserId", UserId));
                    command.Parameters.Add(new SqlParameter("@CodigoEmpresa", CodigoEmpresa));
                    command.ExecuteNonQuery();
                    return "Usuario con empresa asociada creados correctamente";
                }
                catch (SqlException ex)
                {
                    return "Error: sucedio un error. " + ex.Message.ToString();
                }

            }
        }
    }
}