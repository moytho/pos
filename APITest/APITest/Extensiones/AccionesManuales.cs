using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Web;

namespace APITest.Extensiones
{
    public static class AccionesManuales
    {
        public static bool RenombrarNombreImagen(string PathUrl,string NombreImagenViejo,string NombreImagen)
        {
            int ImagenesSubidas = 0;
                // Determinar si el archivo existe para evitar duplicados
                if (File.Exists(PathUrl + NombreImagenViejo))
                {
                    // renombra imagen
                    System.IO.File.Move(PathUrl+NombreImagenViejo , PathUrl+ NombreImagen);
                    ImagenesSubidas = ImagenesSubidas + 1;
                }
            

            if (ImagenesSubidas > 0)
                return true;
            else
                return false;
        }
        public static bool GuardarImagen(HttpPostedFile file, string PathUrl)
        {
            int ImagenesSubidas = 0;
            string extension = file.ContentType;
                if (file.ContentLength > 0)
                {
                    // Determinar si el archivo existe para evitar duplicados
                    if (!File.Exists(PathUrl + Path.GetFileName(file.FileName)))
                    {
                        // guardar imagen
                        file.SaveAs(PathUrl + Path.GetFileName(file.FileName));
                        ImagenesSubidas = ImagenesSubidas + 1;
                    }
                }

            if (ImagenesSubidas > 0)
                return true;
            else
                return false;
        }
        public static bool GuardarImagenes(HttpFileCollection Files,string PathUrl,List<String> NombreImagenes) {
            int ImagenesSubidas = 0;
            for (int iCnt = 0; iCnt <= Files.Count - 1; iCnt++)
            {
                System.Web.HttpPostedFile file = Files[iCnt];

                if (file.ContentLength > 0)
                {
                    // Determinar si el archivo existe para evitar duplicados
                    if (!File.Exists(PathUrl + Path.GetFileName(file.FileName)))
                    {
                        // guardar imagen
                        file.SaveAs(PathUrl + NombreImagenes[iCnt]);
                        ImagenesSubidas = ImagenesSubidas + 1;
                    }
                }
            }

            if (ImagenesSubidas > 0)
                return true;
            else
                return false;
        }
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
        // You could extract these two private methods to a separate utility class since
        // they do not really belong to a controller class but that is up to you
        public static MultipartFormDataStreamProvider GetMultipartProvider()
        {
            var uploadFolder = "~/App_Data/ProductoImagenes"; // you could put this to web.config
            var root = HttpContext.Current.Server.MapPath(uploadFolder);
            Directory.CreateDirectory(root);
            return new MultipartFormDataStreamProvider(root);
        }

        // Extracts Request FormatData as a strongly typed model
        public static object GetFormData<T>(MultipartFormDataStreamProvider result)
        {
            if (result.FormData.HasKeys())
            {
                var unescapedFormData = Uri.UnescapeDataString(result.FormData.GetValues(0).FirstOrDefault() ?? String.Empty);
                if (!String.IsNullOrEmpty(unescapedFormData))
                    return JsonConvert.DeserializeObject<T>(unescapedFormData);
            }

            return null;
        }

        public static string GetDeserializedFileName(MultipartFileData fileData)
        {
            var fileName = GetFileName(fileData);
            return JsonConvert.DeserializeObject(fileName).ToString();
        }

        public static string GetFileName(MultipartFileData fileData)
        {
            return fileData.Headers.ContentDisposition.FileName;
        }
    }
}