using COES.Dominio.DTO.Sic;
using COES.MVC.Intranet.Areas.RolTurnos.Models;
using COES.MVC.Intranet.Controllers;
using COES.MVC.Intranet.Helper;
using COES.Servicios.Aplicacion.General;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.RolTurnos.Controllers
{
    public class ConfiguracionController : BaseController
    {
        readonly RolTurnosAppServicio servicio = new RolTurnosAppServicio();

        public ActionResult Index()
        {
            ConfiguracionModel model = new ConfiguracionModel
            {
                Anio = DateTime.Now.Year,
                Mes = DateTime.Now.Month
            };
            return View(model);
        }

        [HttpPost]
        public PartialViewResult ConfiguracionPersonal(int anio, int mes)
        {
            ConfiguracionModel model = new ConfiguracionModel
            {
                Estructura = this.servicio.ObtenerConfiguracionRolTurno(anio, mes)
            };
            return PartialView(model);
        }

        [HttpPost]
        public JsonResult GrabarConfiguracion(List<DataConfiguracionItem> data, int anio, int mes)
        {
            return Json(this.servicio.GrabarConfiguracion(data, anio, mes, base.UserName));
        }

        [HttpPost]
        public PartialViewResult ListarActividad()
        {
            ConfiguracionModel model = new ConfiguracionModel
            {
                ListaActividad = this.servicio.GetByCriteriaRtuActividads()
            };
            return PartialView(model);
        }

        [HttpPost]
        public PartialViewResult EditarActividad(int id)
        {
            ConfiguracionModel model = new ConfiguracionModel
            {
                ListaTipoResponsabilidad = this.servicio.ObtenerTiposResponsabilidad()
            };

            if (id == 0)
            {
                model.EntidadActividad = new RtuActividadDTO
                {
                    Rtuactestado = Constantes.EstadoActivo
                };
            }

            else
                model.EntidadActividad = this.servicio.GetByIdRtuActividad(id);

            return PartialView(model);
        }

        [HttpPost]
        public JsonResult EliminarActividad(int id)
        {
            return Json(this.servicio.DeleteRtuActividad(id, base.UserName));
        }

        [HttpPost]
        public JsonResult GrabarActividad(ConfiguracionModel model)
        {
            RtuActividadDTO entity = new RtuActividadDTO
            {
                Rtuactabreviatura = model.AbreviaturaActividad,
                Rtuactdescripcion = model.DescripcionActividad,
                Rtuactestado = model.EstadoActividad,
                Rtuactcodi = model.CodigoActividad,
                Rtuactusumodificacion = base.UserName,
                Rtuactreporte = model.Reporte,
                Rturescodi = model.TipoResponsble
            };

            return Json(this.servicio.SaveRtuActividad(entity));
        }
    }
}