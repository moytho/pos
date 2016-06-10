using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using APITest.Models;
using System.Web;
using APITest.Conexion;
using System.Configuration;
using AutoMapper;
using System.Threading.Tasks;
using APITest.Extensiones;
using System.IO;
namespace APITest.Controllers
{
    [Authorize]
    public class ProductosController : ApiController
    {
        private JadeCore1Entities db = new JadeCore1Entities();
        private string connectionString = "";
        private string UserId = "";

        [Route("api/productoimagenconvertirprincipal/{id?}")]
        public IHttpActionResult GetProductoImagenConvertirPrincipal(int CodigoProductoImagen,int CodigoProducto)
        {
            UserId = HttpContext.Current.User.Identity.GetUserId().ToString();
            ClaseConexion conexion = new ClaseConexion(UserId, this.GetType().FullName.ToString(), "ProductosController");

            if (conexion.PoseePermiso == 1)
            {
                try
                {
                    connectionString = ConfigurationManager.ConnectionStrings[conexion.NameConnectionString].ConnectionString;
                    using (JadeCore1Entities db = new JadeCore1Entities())
                    {

                        if (AccionesManuales.RemoverProductoImagenPrincipal(CodigoProductoImagen,CodigoProducto,conexion.NameConnectionString))
                        {
                            return Ok();
                        }
                        else {
                            return InternalServerError();
                        }

                        /*var query = (from productoImagenes in db.ProductoImagens
                                     where (productoImagenes.CodigoEmpresa == conexion.CodigoEmpresa && productoImagenes.CodigoProducto == id && productoImagenes.ImagenUrl != null)
                                     orderby productoImagenes.CodigoProductoImagen
                                     select new ProductoImagenDTO
                                     {
                                         CodigoEmpresa = productoImagenes.CodigoEmpresa,
                                         CodigoProductoImagen = productoImagenes.CodigoProductoImagen,
                                         CodigoProducto = productoImagenes.CodigoProducto,
                                         ImagenUrl = productoImagenes.ImagenUrl,
                                         Principal = productoImagenes.Principal
                                     }).ToList();


                        if (query == null)
                        {
                            return NotFound();
                        }*/

                        return Ok();
                    }
                }
                catch (Exception exception)
                {
                    return InternalServerError();
                }
            }
            else return Unauthorized();
        }

        [Route("api/productoimageneliminar/{id?}")]
        public IHttpActionResult GetDeleteProductoImagen(int id)
        {
            UserId = HttpContext.Current.User.Identity.GetUserId().ToString();
            ClaseConexion conexion = new ClaseConexion(UserId, this.GetType().FullName.ToString(), "ProductosController");

            if (conexion.PoseePermiso == 1)
            {
                try
                {
                    connectionString = ConfigurationManager.ConnectionStrings[conexion.NameConnectionString].ConnectionString;
                    using (JadeCore1Entities db = new JadeCore1Entities())
                    {
                        
                       ProductoImagen productoImagen = db.ProductoImagens.Find(id);
                       if (productoImagen == null)
                       {
                            return NotFound();
                       }
                        string PathUrl = System.Web.Hosting.HostingEnvironment.MapPath("~/" + productoImagen.ImagenUrl);
                        if (AccionesManuales.EliminarArchivo(PathUrl))
                        {
                            db.ProductoImagens.Remove(productoImagen);
                            db.SaveChanges();

                            return Ok();
                        }
                        else {
                            return InternalServerError();
                        }
                    }
                }
                catch (Exception exception)
                {
                    return InternalServerError();
                }
            }
            else return Unauthorized();
        }

        [Route("api/producto/uploadimagen/{id?}")]
        [HttpPost()]
        public HttpResponseMessage UploadImagen(int id)
        {
            string PathUrl = System.Web.Hosting.HostingEnvironment.MapPath("~/Content/ProductoImagenes/");
            string UrlCorto = "Content/ProductoImagenes/";
            HttpFileCollection Files = System.Web.HttpContext.Current.Request.Files;
            UserId = HttpContext.Current.User.Identity.GetUserId().ToString();
            ClaseConexion conexion = new ClaseConexion(UserId, this.GetType().FullName.ToString(), "ProductosController");
            if (conexion.PoseePermiso == 1)
            {
                try
                {
                    int ImagenesGuardadas = 0;
                    connectionString = ConfigurationManager.ConnectionStrings[conexion.NameConnectionString].ConnectionString;
                    using (JadeCore1Entities db = new JadeCore1Entities())
                    {
                        db.SetConnectionString(connectionString);
                        for (int iCnt = 0; iCnt <= Files.Count - 1; iCnt++)
                        {
                            System.Web.HttpPostedFile file = Files[iCnt];

                            if (file.ContentLength > 0)
                            {
                                //Determinamos la extension del archivo
                                string extension = Path.GetExtension(file.FileName);
                                //Guardar imagen con el nombre que se recibio
                                if (Extensiones.AccionesManuales.GuardarImagen(file, PathUrl))
                                {   //Si se guardo la imagen guardaremos un registro
                                    ProductoImagen productoImagen = new ProductoImagen();
                                    productoImagen.CodigoEmpresa = conexion.CodigoEmpresa;
                                    productoImagen.CodigoProducto = id;
                                    productoImagen.ImagenUrl = UrlCorto + file.FileName;
                                    db.ProductoImagens.Add(productoImagen);
                                    //Guardamos el registro
                                    db.SaveChanges();
                                    ImagenesGuardadas++;
                                    //Determinamos el nuevo nombre que le daremos a la imagen tomando en cuenta
                                    //el identity del record recien insertado con la extension del archivo
                                    string NuevoNombreImagen=productoImagen.CodigoProductoImagen.ToString()+extension;
                                    //Renombramos el nombre del archivo
                                    Extensiones.AccionesManuales.RenombrarNombreImagen(PathUrl, file.FileName, NuevoNombreImagen);
                                    //var productoInsertado=db.ProductoImagens.Find(productoImagen.CodigoProductoImagen,NuevoNombreImagen);
                                    productoImagen.ImagenUrl = UrlCorto+NuevoNombreImagen;
                                    db.Entry(productoImagen).State = EntityState.Modified;
                                    //actualizacion del registro
                                    db.SaveChanges();
                                    
                                }
                            }
                        }
                    }
                    //if (ImagenesGuardadas > 0)
                        //return Ok("Imagenes guardadas " + ImagenesGuardadas.ToString());
                        return Request.CreateResponse(HttpStatusCode.OK, "Imagen creada correctamente");
                    //else
                      //  return Ok();
                    
                } catch (Exception exception)
                {
                    //return InternalServerError(exception.Message.ToString());
                    HttpError myCustomError = new HttpError(exception.Message.ToString()) { { "Error", 42 } };
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, myCustomError);
                }
            }
            //si no posee permiso retornamos el estado Unauthorized 501
            else return Request.CreateErrorResponse(HttpStatusCode.Unauthorized,"Unauthorized");
        }
        // GET api/Productos
        //Este tendra que ser modificado, para consultar globalmente (todas las sucursales) o
        //solo la sucursal local

       [Route("api/productoimagenes/{id?}")]       
        public IHttpActionResult GetProductoImagenes(int id)
        {
            UserId = HttpContext.Current.User.Identity.GetUserId().ToString();
            ClaseConexion conexion = new ClaseConexion(UserId, this.GetType().FullName.ToString(), "ProductosController");

            if (conexion.PoseePermiso == 1)
            {
                try
                {
                    connectionString = ConfigurationManager.ConnectionStrings[conexion.NameConnectionString].ConnectionString;
                    using (JadeCore1Entities db = new JadeCore1Entities())
                    {
                        var query = (from productoImagenes in db.ProductoImagens
                                     where (productoImagenes.CodigoEmpresa == conexion.CodigoEmpresa && productoImagenes.CodigoProducto == id && productoImagenes.ImagenUrl != null)
                                     orderby productoImagenes.CodigoProductoImagen
                                     select new ProductoImagenDTO
                                     {
                                         CodigoEmpresa = productoImagenes.CodigoEmpresa,
                                         CodigoProductoImagen = productoImagenes.CodigoProductoImagen,
                                         CodigoProducto = productoImagenes.CodigoProducto,
                                         ImagenUrl = productoImagenes.ImagenUrl,
                                         Principal = productoImagenes.Principal
                                     }).ToList();


                        if (query == null)
                        {
                            return NotFound();
                        }

                        return Ok(query);
                    }
                }
                catch (Exception exception)
                {
                    return InternalServerError();
                }
            }
            else return Unauthorized();
        }
        
        [AllowAnonymous]
        [Route("api/productotipos")]
        //Esta es la misma misma informacion para todas las empresas, por eso no estoy haciendo consulta para determianr
        //A que empresa pertenece el usuario actual
        public IHttpActionResult GetProductoTipos()
        {
            var query = (from productoTipo in db.ProductoTipoes
                         orderby productoTipo.Descripcion
                         select new ProductoTipoDTO()
                         {
                             CodigoProductoTipo = productoTipo.CodigoProductoTipo,
                             Descripcion= productoTipo.Descripcion
                         }).ToList();
            if (query == null)
            {
                return NotFound();
            }

            return Ok(query);
        }
        public IHttpActionResult GetProductoes()
        {
            UserId = HttpContext.Current.User.Identity.GetUserId().ToString();
            //Esta clase nos retornara 
            //- empresa a la que pertenece el usuario y 
            //-su respectiva cadena de conexion
            //-tambien si posee permiso para realizar esta accion
            ClaseConexion conexion = new ClaseConexion(UserId, this.GetType().FullName.ToString(), "ProductosController");
            if (conexion.PoseePermiso == 1)
            {
                try
                {
                    connectionString = ConfigurationManager.ConnectionStrings[conexion.NameConnectionString].ConnectionString;
                    using (JadeCore1Entities db = new JadeCore1Entities())
                    {
                        db.SetConnectionString(connectionString);
                        //Consulta para mostrar todas las sucursales pertenecientes a esta empresa
                        var productos = (from producto in db.Productoes
                                                    join productomarca in db.ProductoMarcas
                                                    on producto.CodigoProductoMarca equals productomarca.CodigoProductoMarca
                                                    join productoclasificacion in db.ProductoClasificacions
                                                    on producto.CodigoProductoClasificacion equals productoclasificacion.CodigoProductoClasificacion
                                                    where (producto.CodigoEmpresa == conexion.CodigoEmpresa && producto.Estado == true)
                                                    orderby producto.Nombre
                                                    select new ProductoInformacion { 
                                                        CodigoProducto= producto.CodigoProducto, 
                                                        CodigoLocal=producto.CodigoLocal,
                                                        Marca=productomarca.Nombre, 
                                                        Clasificacion=productoclasificacion.Descripcion,
                                                        Nombre=producto.Nombre,
                                                        PrecioCosto=producto.PrecioCosto,
                                                        Estado=producto.Estado,
                                                        PrecioVenta=producto.PrecioVenta
  
                                                    }).ToList();
                        //Convirtiendo la Clase sucursal a sucursalDTO
                        //Esto con el afan de ocultar ciertos campos de nuestro objeto, si fuera el caso
                        //Mapper.CreateMap<ProductoYMas, ProductoYMasDTO>();
                        //List<ProductoYMasDTO> productosYMasDTO = Mapper.Map<List<ProductoYMas>, List<ProductoYMasDTO>>(productos);
                        return Ok(productos);
                    }
                }
                catch (Exception exception)
                {
                    return InternalServerError();
                }
            }
            //si no posee permiso retornamos el estado Unauthorized 501
            else return Unauthorized();

        }

        // GET api/Productos/5
        [ResponseType(typeof(Producto))]
        public IHttpActionResult GetProducto(int id)
        {
            UserId = HttpContext.Current.User.Identity.GetUserId().ToString();
            ClaseConexion conexion = new ClaseConexion(UserId, this.GetType().FullName.ToString(), "ProductosController");

            if (conexion.PoseePermiso == 1)
            {
                try
                {
                    connectionString = ConfigurationManager.ConnectionStrings[conexion.NameConnectionString].ConnectionString;
                    using (JadeCore1Entities db = new JadeCore1Entities())
                    {
                        
                        //una manera diferente de obtener la(s) sucursal(es) y convertir a la clase SucursalDTO
                        var query = (from producto in db.Productoes
                                     where (producto.CodigoProducto == id && producto.CodigoEmpresa == conexion.CodigoEmpresa)
                                     orderby producto.Nombre
                                     select new ProductoDTO()
                                     {
                                         CodigoEmpresa = producto.CodigoEmpresa,
                                         CodigoProducto = producto.CodigoProducto,
                                         Nombre = producto.Nombre,
                                         CodigoLocal = producto.CodigoLocal,
                                         CodigoProductoMarca = producto.CodigoProductoMarca,
                                         CodigoProductoClasificacion = producto.CodigoProductoClasificacion,
                                         CodigoProductoSubClasificacion = producto.CodigoProductoSubClasificacion,
                                         Descripcion = producto.Descripcion,
                                         PrecioCosto=producto.PrecioCosto,
                                         PrecioVenta=producto.PrecioVenta,
                                         CodigoProductoTipo=producto.CodigoProductoTipo,
                                         CodigoProductoAbastecimiento= producto.CodigoProductoAbastecimiento,
                                         Estado = producto.Estado,
                                         SKU=producto.SKU,
                                         Alto=producto.Alto,
                                         Ancho=producto.Ancho,
                                         Profundidad=producto.Profundidad,
                                         ImagenUrl=producto.ImagenUrl,
                                         StockMinimo=producto.StockMinimo
                                     }).ToList();
                        if (query == null)
                        {
                            return NotFound();
                        }

                        return Ok(query);
                    }
                }
                catch (Exception exception)
                {
                    return InternalServerError();
                }
            }
            else return Unauthorized();
        }

        // PUT api/Productos/5
        public IHttpActionResult PutProducto(int id, Producto producto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != producto.CodigoProducto)
            {
                return BadRequest();
            }
            UserId = HttpContext.Current.User.Identity.GetUserId().ToString();
            ClaseConexion conexion = new ClaseConexion(UserId, this.GetType().FullName.ToString(), "ProductosController");

            if (conexion.PoseePermiso == 1)
            {
                connectionString = ConfigurationManager.ConnectionStrings[conexion.NameConnectionString].ConnectionString;
                using (JadeCore1Entities db = new JadeCore1Entities())
                {
                    producto.CodigoEmpresa = conexion.CodigoEmpresa;
                    db.Entry(producto).State = EntityState.Modified;

                    try
                    {
                        db.SaveChanges();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!ProductoExists(id,conexion.NameConnectionString))
                        {
                            return NotFound();
                        }
                        else
                        {
                            throw;
                        }
                    }

                    return StatusCode(HttpStatusCode.NoContent);
                }
            }
            else return Unauthorized();
        }

        // POST api/Productos
        //[ResponseType(typeof(Producto))]
        public IHttpActionResult PostProducto(Producto producto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            UserId = HttpContext.Current.User.Identity.GetUserId().ToString();
            ClaseConexion conexion = new ClaseConexion(UserId, this.GetType().FullName.ToString(), "ProductosController");

            if (conexion.PoseePermiso == 1)
            {
                connectionString = ConfigurationManager.ConnectionStrings[conexion.NameConnectionString].ConnectionString;
                using (JadeCore1Entities db = new JadeCore1Entities())
                {

                    producto.CodigoEmpresa = conexion.CodigoEmpresa;
                    producto.Estado = true;
                    //Como estamos mandando solo el value del select no necesitamos convertira null las entidades siguientes
                    //convirtiendo el objecto clasificacion a null para que no haga un insert de este objeto
                    //producto.ProductoClasificacion = null;
                    //convirtiendo el objecto marca a null para que no haga un insert de este objeto
                    //producto.ProductoMarca = null;
                    //producto.ProductoAbastecimiento = null;
                    db.Productoes.Add(producto);
                    db.SaveChanges();
                    Mapper.CreateMap<Producto, ProductoDTO>();
                    ProductoDTO productoDTO = Mapper.Map<Producto, ProductoDTO>(producto);
                    return CreatedAtRoute("DefaultApi", new { id = producto.CodigoProducto }, productoDTO);
                }
            }
            else return Unauthorized();
        }

        // DELETE api/Productos/5
        [ResponseType(typeof(Producto))]
        public IHttpActionResult DeleteProducto(int id)
        {
            UserId = HttpContext.Current.User.Identity.GetUserId().ToString();
            ClaseConexion conexion = new ClaseConexion(UserId, this.GetType().FullName.ToString(), "ProductosController");

            if (conexion.PoseePermiso == 1)
            {
                connectionString = ConfigurationManager.ConnectionStrings[conexion.NameConnectionString].ConnectionString;
                using (JadeCore1Entities db = new JadeCore1Entities())
                {
                    Producto producto = db.Productoes.Find(id,conexion.CodigoEmpresa);
                    if (producto == null)
                    {
                        return NotFound();
                    }

                    producto.Estado=false;
                    //db.Productoes.Remove(producto);
                    db.SaveChanges();
                    Mapper.CreateMap<Producto, ProductoDTO>();
                    ProductoDTO productoDTO = Mapper.Map<Producto, ProductoDTO>(producto);
                    
                    return Ok(productoDTO);
                    //}
                }
            }
            else return Unauthorized();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ProductoExists(int id, string nameConnectionString)
        {
            connectionString = ConfigurationManager.ConnectionStrings[nameConnectionString].ConnectionString;
            using (JadeCore1Entities db = new JadeCore1Entities())
            {
                return db.Productoes.Count(e => e.CodigoProducto == id) > 0;
            }
        }

        private bool ProductoBelongsToYourCompany(int id, string nameConnectionString, int CodigoEmpresa)
        {
            connectionString = ConfigurationManager.ConnectionStrings[nameConnectionString].ConnectionString;
            using (JadeCore1Entities db = new JadeCore1Entities())
            {
                return db.Productoes.Count(e => e.CodigoProducto == id && e.CodigoEmpresa == CodigoEmpresa) > 0;
            }
        }
    }
}

#region otra forma de guardar imagenes
/*   [Route("api/producto/uploadimagen")]
        [HttpPost] // This is from System.Web.Http, and not from System.Web.Mvc
        public async Task<HttpResponseMessage> Upload()
        {
            if (!Request.Content.IsMimeMultipartContent())
            {
                this.Request.CreateResponse(HttpStatusCode.UnsupportedMediaType);
            }

            var provider = Extensiones.AccionesManuales.GetMultipartProvider();
            var result = await Request.Content.ReadAsMultipartAsync(provider);

            // On upload, files are given a generic name like "BodyPart_26d6abe1-3ae1-416a-9429-b35f15e6e5d5"
            // so this is how you can get the original file name
            var originalFileName =Extensiones.AccionesManuales.GetDeserializedFileName(result.FileData.First());

            // uploadedFileInfo object will give you some additional stuff like file length,
            // creation time, directory name, a few filesystem methods etc..
            var uploadedFileInfo = new FileInfo(result.FileData.First().LocalFileName);

            // Remove this line as well as GetFormData method if you're not 
            // sending any form data with your upload request
            var fileUploadObj = Extensiones.AccionesManuales.GetFormData<ProductoDTO>(result);
            //int CodigoProdcto = 
            // Through the request response you can return an object to the Angular controller
            // You will be able to access this in the .success callback through its data attribute
            // If you want to send something to the .error callback, use the HttpStatusCode.BadRequest instead
            var returnData = "ReturnTest";
            return this.Request.CreateResponse(HttpStatusCode.OK, new { returnData });
        }*/
#endregion