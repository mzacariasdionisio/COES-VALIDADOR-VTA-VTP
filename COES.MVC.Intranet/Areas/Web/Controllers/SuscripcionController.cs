using COES.Dominio.DTO.Sic;
using COES.MVC.Intranet.Areas.Web.Models;
using COES.MVC.Intranet.Controllers;
using COES.MVC.Intranet.Helper;
using COES.Servicios.Aplicacion.General;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.Web.Controllers
{
    public class SuscripcionController : BaseController
    {
        /// <summary>
        /// Nombre del archivo de exportacion
        /// </summary>
        private string FileExportacion = "Suscripciones.xlsx";

        /// <summary>
        /// Instancia de la clase de servicio
        /// </summary>
        SubscripcionAppServicio servicio = new SubscripcionAppServicio();

        #region Relacion de Generadores

        /// <summary>
        /// Pantalla Inicial del módulo
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            SubscripcionModel model = new SubscripcionModel();
            model.FechaInicio = DateTime.Now.AddDays(-7).ToString(Constantes.FormatoFecha);
            model.FechaFin = DateTime.Now.AddDays(1).ToString(Constantes.FormatoFecha);
            model.ListaPublicacion = this.servicio.ListarPublicaciones();
            
            return View(model);
        }
        
        /// <summary>
        /// Permite listar los registros de la consulta
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <param name="estado"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult Listar(string fechaInicio, string fechaFin, int? idPublicacion)
        {
            SubscripcionModel model = new SubscripcionModel();

            DateTime fechaInicial = DateTime.ParseExact(fechaInicio, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            DateTime fechaFinal = DateTime.ParseExact(fechaFin, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            model.ListaSubscripciones = this.servicio.ListarSubscripciones(fechaInicial, fechaFinal, idPublicacion);

            return PartialView(model);
        }

        /// <summary>
        /// Permite editar los datos de la subscripcion
        /// </summary>
        /// <param name="idRelacion"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult Editar(int idSubscripcion)
        {
            SubscripcionModel model = new SubscripcionModel();
            model.ListaItems = this.servicio.ObtenerItemsSubscripcion(idSubscripcion);
            model.Entidad = this.servicio.ObtenerSubscripcion(idSubscripcion);
            List<int> ids = model.ListaItems.Where(x => x.Indicador > 0).Select(x => x.Publiccodi).ToList();
            model.Publicaciones = string.Join<int>(Constantes.CaracterComa.ToString(), ids);

            return PartialView(model);
        }

        /// <summary>
        /// Permite eliminar la relación de equivalencia
        /// </summary>
        /// <param name="idRelacion"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Eliminar(int idSubscripcion)
        {
            try
            {
                this.servicio.EliminarSubscripcion(idSubscripcion);
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
        public JsonResult Grabar(SubscripcionModel model)
        {
            try
            {
                WbSubscripcionDTO entity = new WbSubscripcionDTO
                {
                    Subscripapellidos = model.Apellidos,
                    Subscripcodi = model.Codigo,
                    Subscripemail = model.Email,
                    Subscripempresa = model.Empresa,
                    Subscripestado = Constantes.EstadoActivo,
                    Subscripfecha = DateTime.Now,
                    Subscripnombres = model.Nombres,
                    Subscriptelefono = model.Telefono,
                    Lastuser = base.UserName,
                    Lastdate = DateTime.Now
                };

                string items = string.Empty;

                if (model.Publicaciones.Length > 0)
                {
                    items = model.Publicaciones.Remove(model.Publicaciones.Length - 1, 1);
                }

                int result = this.servicio.GrabarSubscripcion(entity, items);
                return Json(result);
            }
            catch
            {
                return Json(-1);
            }
        }

        /// <summary>
        /// Permite generar el formato horizontal
        /// </summary>
        /// <param name="fechaInicial"></param>
        /// <param name="fechaFinal"></param>
        /// <param name="empresas"></param>
        /// <param name="tiposGeneracion"></param>
        /// <param name="central"></param>
        /// <param name="parametros"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Exportar(string fechaInicio, string fechaFin, int? idPublicacion)
        {
            try
            {
                string path = AppDomain.CurrentDomain.BaseDirectory + RutaDirectorio.RutaExportacionWeb;
                string file = this.FileExportacion;

                DateTime fechaInicial = DateTime.ParseExact(fechaInicio, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                DateTime fechaFinal = DateTime.ParseExact(fechaFin, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                List<WbSubscripcionDTO> list = this.servicio.ListarSubscripcionesExportar(fechaInicial, fechaFinal, idPublicacion);               
                this.servicio.GenerarReporteExcel(list, path, file);

                return Json(1);
            }
            catch
            {
                return Json(-1);
            }
        }

        /// <summary>
        /// Permite abrir el archivo del reporte generado
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public virtual ActionResult Descargar()
        {
            string fullPath = AppDomain.CurrentDomain.BaseDirectory + RutaDirectorio.RutaExportacionWeb + this.FileExportacion;
            return File(fullPath, Constantes.AppExcel, this.FileExportacion);
        }

        /// <summary>
        /// Muestra la vista de publicaciones
        /// </summary>
        /// <returns></returns>        
        public ViewResult Publicacion()
        {
            return View();
        }
        
        /// <summary>
        /// Muestra la lista de publicaciones
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult PublicacionList()
        {
            PublicacionModel model = new PublicacionModel();
            model.ListaPublicacion = this.servicio.ListarPublicaciones();
            return PartialView(model);
        }

        /// <summary>
        /// Permite eliminar la publicación
        /// </summary>
        /// <param name="idPublicacion"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult PublicacionDelete(int idPublicacion)
        {
            try
            {
                this.servicio.EliminarPublicacion(idPublicacion);
                return Json(1);
            }
            catch
            {
                return Json(-1);
            }
        }

        /// <summary>
        /// Permite editar la publicacion
        /// </summary>
        /// <param name="idPublicacion"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult PublicacionEdit(int idPublicacion)
        {
            PublicacionModel model = new PublicacionModel();
            model.ListaAreas = this.servicio.ListarAreas();

            if (idPublicacion == 0)
            {
                model.Entidad = new WbPublicacionDTO();
                model.Entidad.Publiccodi = 0;
            }
            else
            {
                model.Entidad = this.servicio.ObtenerPublicacion(idPublicacion);
            }

            return PartialView(model);
        }

        /// <summary>
        /// Permite grabar los datos de la publicacion
        /// </summary>
        /// <param name="idPublicacion"></param>
        /// <param name="nombre"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult PublicacionSave(PublicacionModel model)
        {
            try
            {
                WbPublicacionDTO entity = new WbPublicacionDTO
                {
                    Publiccodi = model.Codigo,
                    Publicnombre = model.Nombre,
                    Publicestado = model.Estado,
                    Publicemail = model.Correo,
                    Publicemail1 = model.Correo1,
                    Publicemail2 = model.Correo2,
                    Areacode = model.IdArea
                };

                this.servicio.GrabarPublicacion(entity);

                return Json(1);
            }
            catch
            {
                return Json(-1);
            }
        }


        #endregion

    }
}
