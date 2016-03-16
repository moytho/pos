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
     class ClaseConexion
    {
        public string NameConnectionString { get; set; }
        public int PoseePermiso {get; set;}
        public int EsSuperAdmin { get; set; }
        public int CodigoEmpresa { get; set; }
        public string Usuario { get; set; }
        public string UserId { get; set; }
        public string Metodo { get; set; }
        public string Controlador { get; set; }
        public ClaseConexion(string _userId,string _metodo,string _controlador){
            UserId=_userId;
            Metodo = _metodo;
            Controlador = _controlador;
            PoseePermiso = 0;
            NameConnectionString = "";
            
            using (SqlConnection connection = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {
                try
                {
                    DataTable DatosResultados = new DataTable();
                    connection.Open();
                    SqlCommand command = new SqlCommand("dbo.ConexionYPermiso", connection);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter("@UserId", this.UserId));
                    command.Parameters.Add(new SqlParameter("@Metodo", this.Metodo));
                    command.Parameters.Add(new SqlParameter("@Controlador", this.Controlador));
                    SqlDataAdapter da = new SqlDataAdapter(command);
                    da.Fill(DatosResultados);

                    if (DatosResultados.Rows.Count > 0) {
                        this.NameConnectionString = DatosResultados.Rows[0].Field<string>(0);
                        this.UserId = DatosResultados.Rows[0].Field<string>(1);
                        this.CodigoEmpresa = DatosResultados.Rows[0].Field<int>(2);
                        this.PoseePermiso = DatosResultados.Rows[0].Field<int>(3);
                    }
                }
                catch (SqlException ex)
                {
                    /*Handle error*/
                }

            }
        }

        
    }
}