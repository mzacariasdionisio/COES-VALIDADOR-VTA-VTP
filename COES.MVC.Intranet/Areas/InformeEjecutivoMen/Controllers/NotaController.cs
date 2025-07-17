using COES.Dominio.DTO.Sic;
using COES.MVC.Intranet.Areas.InformeEjecutivoMen.Models;
using COES.MVC.Intranet.Controllers;
using COES.MVC.Intranet.Helper;
using COES.Servicios.Aplicacion.IEOD;
using COES.Servicios.Aplicacion.Siosein2;
using log4net;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.InformeEjecutivoMen.Controllers
{
    public class NotaController : BaseController
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(NotaController));
        private static readonly string NameController = "NotaController";
        private readonly Siosein2AppServicio _servicioSiosein2;

        public NotaController()
        {
            _servicioSiosein2 = new Siosein2AppServicio();
        }

        /// <inheritdoc />
        /// <summary>
        /// Protected de log de errores page
        /// </summary>
        /// <param name="filterContext"></param>
        protected override void OnException(ExceptionContext filterContext)
        {
            try
            {
                log4net.Config.XmlConfigurator.Configure();
                Exception objErr = filterContext.Exception;
                Log.Error(NameController, objErr);
            }
            catch (Exception ex)
            {
                Log.Fatal(NameController, ex);
                throw;
            }
        }

        //
        // GET: /Boletin/Nota/

        public ActionResult Index()
        {
            return View();
        }


        [HttpPost]
        public JsonResult CargarListaNota(string periodo, int? mrepcodi, int verscodi)
        {
            DateTime fecha = DateTime.ParseExact(periodo, Constantes.FormatoMesAnio, CultureInfo.InvariantCulture);

            List<SiNotaDTO> result = new List<SiNotaDTO>();
            if (mrepcodi.HasValue)
            {
                result = _servicioSiosein2.GetByCriteriaSiNotas(fecha, mrepcodi.Value, verscodi);
            }


            return Json(result);
        }

        [HttpPost]
        public JsonResult GuardarNota(SiNotaDTO nota)
        {
            var model = new Siosein2Model();
            try
            {
                if (nota.Sinotacodi == 0)
                {
                    nota.Sinotafeccreacion = DateTime.Today;
                    nota.Sinotaestado = 1;//Activo
                    nota.Sinotausucreacion = User.Identity.Name;
                    _servicioSiosein2.SaveSiNota(nota);
                    model.Resultado = "Se registro correctamente";
                }
                else
                {
                    nota.Sinotafecmodificacion = DateTime.Today;
                    nota.Sinotaestado = 1;//Activo
                    nota.Sinotausumodificacion = User.Identity.Name;
                    _servicioSiosein2.UpdateSiNota(nota);
                    model.Resultado = "Se actualizo correctamente";
                }

                model.Estado = (int)Siosein2Model.TipoEstado.Ok;
            }
            catch (Exception e)
            {
                model.Estado = (int)Siosein2Model.TipoEstado.Error;
                model.Resultado = "Ha ocurrido un error";
            }
            return Json(model);
        }

        [HttpDelete]
        public JsonResult DeleteNota(int Sinotacodi)
        {
            var model = new Siosein2Model();
            try
            {
                _servicioSiosein2.DeleteSiNota(Sinotacodi);
                model.Estado = (int)Siosein2Model.TipoEstado.Ok;
                model.Resultado = "Se elimino correctamente";
            }
            catch (Exception)
            {
                model.Estado = (int)Siosein2Model.TipoEstado.Error;
                model.Resultado = "Ha ocurrido un error";
            }
            return Json(model);
        }


        [HttpPost]
        public JsonResult UpdateListaNota(List<SiNotaDTO> dataCambioJson)
        {
            var model = new Siosein2Model();
            try
            {

                foreach (var nota in dataCambioJson)
                {
                    nota.Sinotafecmodificacion = DateTime.Today;
                    nota.Sinotausumodificacion = User.Identity.Name;
                    _servicioSiosein2.UpdateSiNotaOrden(nota);
                }

                model.Estado = (int)Siosein2Model.TipoEstado.Ok;
                model.Resultado = "Se elimino correctamente";
            }
            catch (Exception)
            {
                model.Estado = (int)Siosein2Model.TipoEstado.Error;
                model.Resultado = "Ha ocurrido un error";
            }
            return Json(model);
        }

    }
}
