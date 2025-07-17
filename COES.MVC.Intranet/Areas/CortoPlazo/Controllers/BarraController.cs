using COES.Dominio.DTO.Sic;
using COES.MVC.Intranet.Areas.CortoPlazo.Models;
using COES.MVC.Intranet.Controllers;
using COES.MVC.Intranet.Helper;
using COES.Servicios.Aplicacion.CortoPlazo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.CortoPlazo.Controllers
{
    public class BarraController :  BaseController
    {
        /// <summary>
        /// Instancia de la clase servicio correspondiente
        /// </summary>
        CortoPlazoAppServicio servicio = new CortoPlazoAppServicio();
               
        /// <summary>
        /// Muestra la pantalla de relaciones
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {                       
            return View();
        }

        /// <summary>
        /// Permite listar los registros de la consulta
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <param name="estado"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult Listar(string estado, string publicacion)
        {
            BarraModel model = new BarraModel();
            model.Listado = this.servicio.GetByCriteriaCmConfigbarras(estado, publicacion);
            return PartialView(model);
        }

        /// <summary>
        /// Permite editar los datos de la relacion
        /// </summary>
        /// <param name="idRelacion"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult Editar(int id)
        {
            BarraModel model = new BarraModel();
            model.Listado = this.servicio.ObtenerBarrasYupana();

            if (id != 0)
            {
                model.Entidad = this.servicio.GetByIdCmConfigbarra(id);                
            }
            else
            {
                model.Entidad = new CmConfigbarraDTO();
                model.Entidad.Cnfbarestado = Constantes.EstadoActivo;
                model.Entidad.Cnfbarindpublicacion = Constantes.SI;
                model.Entidad.Cnfbardefecto = Constantes.NO;
            }

            return PartialView(model);
        }

        /// <summary>
        /// Permite eliminar la relación de equivalencia
        /// </summary>
        /// <param name="idRelacion"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Eliminar(int id)
        {
            try
            {
                this.servicio.DeleteCmConfigbarra(id);
                return Json(1);
            }
            catch
            {
                return Json(-1);
            }
        }

        /// <summary>
        /// Permite grabar la relación de equivalencia
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Grabar(BarraModel model)
        {
            try
            {
                CmConfigbarraDTO entity = new CmConfigbarraDTO
                {
                    Cnfbarcodi = model.Codigo,
                    Cnfbarnodo = model.Nodobarra,
                    Cnfbarnombre = model.Nombrebarra,
                    Cnfbarestado = model.Estado,
                    Cnfbarindpublicacion = model.IndPublicar,
                    Cnfbardefecto = model.IndDefecto,
                    Cnfbarnombncp = model.NombreNCP,
                    Cnfbarnomtna = model.NombreTna,
                    Cnfbarfeccreacion = DateTime.Now,
                    Cnfbarusucreacion = base.UserName,
                    Cnfbarfecmodificacion = DateTime.Now,
                    Cnfbarusumodificacion = base.UserName,
                    Recurcodi = model.Recurcodi,
                    Canalcodi= model.Canalcodi
                };

                int result = this.servicio.SaveCmConfigbarra(entity);

                return Json(result);
            }
            catch
            {
                return Json(-1);
            }
        }


        /// <summary>
        /// Permite mostrar el mapa de coordenadas
        /// </summary>
        /// <param name="idEquipo"></param>
        /// <returns></returns>        
        public ViewResult Mapa(int id)
        {
            BarraModel model = new BarraModel();
            model.Entidad = this.servicio.GetByIdCmConfigbarra(id);
            return View(model);
        }

        /// <summary>
        /// Permite grabar los coordenadas de la barra de CMgN
        /// </summary>
        /// <param name="idEquipo"></param>
        /// <param name="coordenadaX"></param>
        /// <param name="coordenadaY"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GrabarCoordenada(int idBarra, string coordenadaX, string coordenadaY)
        {
            try
            {
                CmConfigbarraDTO entity = new CmConfigbarraDTO
                {
                    Cnfbarcodi = idBarra,
                    Cnfbarcoorx = coordenadaX,
                    Cnfbarcoory = coordenadaY,
                    Cnfbarfecmodificacion = DateTime.Now,
                    Cnfbarusumodificacion = base.UserName
                };

                this.servicio.UpdateCoordenada(entity);

                return Json(1);
            }
            catch
            {
                return Json(-1);
            }
        }

    }
}
