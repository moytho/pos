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
using APITest.Conexion;
using System.Web;
using System.Configuration;
using AutoMapper;

namespace APITest.Controllers
{
    public class PedidosController : ApiController
    {
        private JadeCore1Entities db = new JadeCore1Entities();
        private string connectionString = "";
        private string UserId = "";

        // GET api/Peidos
        public IQueryable<Pedido> GetPedidoes()
        {
            return db.Pedidoes;
        }

        // GET api/Peidos/5
        [ResponseType(typeof(Pedido))]
        public IHttpActionResult GetPedido(int id)
        {
            Pedido pedido = db.Pedidoes.Find(id);
            if (pedido == null)
            {
                return NotFound();
            }

            return Ok(pedido);
        }

        // PUT api/Peidos/5
        public IHttpActionResult PutPedido(int id, Pedido pedido)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != pedido.CodigoPedido)
            {
                return BadRequest();
            }

            db.Entry(pedido).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PedidoExists(id))
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

        // POST api/Peidos
        [ResponseType(typeof(Pedido))]
        public IHttpActionResult PostPedido(Pedido pedido)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            UserId = HttpContext.Current.User.Identity.GetUserId().ToString();
            ClaseConexion conexion = new ClaseConexion(UserId, this.GetType().FullName.ToString(), "PedidosController");

            if (conexion.PoseePermiso == 1)
            {
                connectionString = ConfigurationManager.ConnectionStrings[conexion.NameConnectionString].ConnectionString;
                using (JadeCore1Entities db = new JadeCore1Entities())
                {
                    pedido.UserId = UserId;
                    pedido.CodigoEmpresa = conexion.CodigoEmpresa;
                    pedido.CodigoSucursal =conexion.CodigoSucursal;
                    pedido.FechaCreacion = 1;// DateTime.Now;
                    db.Pedidoes.Add(pedido);
                    db.SaveChanges();
                    PedidoDTO pedidoDTO = new PedidoDTO();
                    pedidoDTO.CodigoEmpresa = conexion.CodigoEmpresa;
                    pedidoDTO.CodigoSucursal = conexion.CodigoSucursal;
                    pedidoDTO.CodigoPedido = pedido.CodigoPedido;
                    pedidoDTO.FechaCreacion = pedido.FechaCreacion;
                    //Mapper.CreateMap<Pedido, PedidoDTO>();
                    //PedidoDTO pedidoDTO = Mapper.Map<Pedido, PedidoDTO>(pedido);
                    return CreatedAtRoute("DefaultApi", new { id = pedido.CodigoPedido }, pedidoDTO);
                }
            }
            else return Unauthorized();
        }

        // DELETE api/Peidos/5
        [ResponseType(typeof(Pedido))]
        public IHttpActionResult DeletePedido(int id)
        {
            Pedido pedido = db.Pedidoes.Find(id);
            if (pedido == null)
            {
                return NotFound();
            }

            db.Pedidoes.Remove(pedido);
            db.SaveChanges();

            return Ok(pedido);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool PedidoExists(int id)
        {
            return db.Pedidoes.Count(e => e.CodigoPedido == id) > 0;
        }
    }
}