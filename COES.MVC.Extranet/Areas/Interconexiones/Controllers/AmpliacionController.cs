using COES.Dominio.DTO.Sic;
using COES.MVC.Extranet.Areas.Interconexiones.Helper;
using COES.MVC.Extranet.Areas.Interconexiones.Models;
using COES.MVC.Extranet.Controllers;
using COES.MVC.Extranet.Helper;
using COES.Servicios.Aplicacion.Interconexiones;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace COES.MVC.Extranet.Areas.Interconexiones.Controllers
{
    public class AmpliacionController : BaseController
    {
        InterconexionesAppServicio logic = new InterconexionesAppServicio();
        SeguridadServicio.SeguridadServicioClient seguridad = new SeguridadServicio.SeguridadServicioClient();
        //
        // GET: /Medidores/Ampliacion/

        //[COES.MVC.Extranet.Helper.CustomAuthorize]
        public ActionResult Index()
        {
            base.ValidarSesionUsuario();
            InterconexionesModel model = new InterconexionesModel();
            model.ListaEmpresas = this.seguridad.ObtenerEmpresasPorUsuario(User.Identity.Name).Where(
                x => x.EMPRCODI == ConstantesInterconexiones.IdEmpresaInterconexion).Select(x => new SiEmpresaDTO { Emprcodi = x.EMPRCODI, Emprnomb = x.EMPRNOMB }).ToList();
            if (model.ListaEmpresas.Count == 1)
            {
                model.IdEmpresa = model.ListaEmpresas[0].Emprcodi;
            }

            model.Empresa = ConstantesInterconexiones.IdEmpresaInterconexion.ToString();
            model.Fecha = DateTime.Now.ToString(Constantes.FormatoFecha);
            return View(model);
        }
        /// <summary>
        /// Obtiene la lista de todas las ampliaciones de plazo.
        /// </summary>
        /// <param name="empresa"></param>
        /// <param name="fecha"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult Lista(int empresa, string fecha)
        {
            InterconexionesModel model = new InterconexionesModel();
            DateTime fechaIni = DateTime.Now;
            DateTime fechaFin = DateTime.Now;
            if (fecha != null)
            {
                fechaIni = DateTime.ParseExact(fecha, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                fechaFin = fechaIni.AddDays(1);

            }

            var lista = this.logic.ObtenerListaMeAmpliacionfechas(fechaIni, fechaFin, empresa, ConstantesInterconexiones.IdFormato);
            model.ListaAmpliacion = lista;
            return PartialView(model);
        }
        /// <summary>
        /// Obtiene el model para pintar el popup de ingreso de la nueva ampliacion
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult AgregarAmpliacion()
        {
            base.ValidarSesionUsuario();
            InterconexionesModel model = new InterconexionesModel();
            model.ListaEmpresas = this.seguridad.ObtenerEmpresasPorUsuario(User.Identity.Name).Where(
                x => x.EMPRCODI == ConstantesInterconexiones.IdEmpresaInterconexion).Select(x => new SiEmpresaDTO { Emprcodi = x.EMPRCODI, Emprnomb = x.EMPRNOMB }).ToList();             
            if (model.ListaEmpresas.Count == 1)
            {
                model.IdEmpresa = model.ListaEmpresas[0].Emprcodi;
            }
            model.Empresa = ConstantesInterconexiones.IdEmpresaInterconexion.ToString();
            model.Fecha = DateTime.Now.AddDays(-1).ToString(Constantes.FormatoFecha);
            model.FechaPlazo = DateTime.Now.ToString(Constantes.FormatoFecha);

            model.HoraPlazo = DateTime.Now.Hour * 2 + 1;
            return PartialView(model);
        }
        /// <summary>
        /// Graba la Ampliacion ingresada.
        /// </summary>
        /// <param name="fecha"></param>
        /// <param name="hora"></param>
        /// <param name="empresa"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GrabarValidacion(string fecha, int hora, int empresa)
        {
            base.ValidarSesionUsuario();
            int resultado = 1;
            DateTime fechaEnvio = DateTime.Now;

            if (fecha != null)
            {
                fechaEnvio = DateTime.ParseExact(fecha, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            }

            MeAmpliacionfechaDTO ampliacion = new MeAmpliacionfechaDTO();
            ampliacion.Lastuser = User.Identity.Name;
            ampliacion.Lastdate = DateTime.Now;
            ampliacion.Amplifecha = fechaEnvio;
            ampliacion.Formatcodi = ConstantesInterconexiones.IdFormato;
            ampliacion.Emprcodi = empresa;
            ampliacion.Amplifechaplazo = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day).AddMinutes(hora * 30);
            try
            {
                var reg = logic.GetByIdMeAmpliacionfecha(fechaEnvio, empresa, ConstantesInterconexiones.IdFormato);
                if (reg == null)
                {
                    this.logic.SaveMeAmpliacionfecha(ampliacion);
                }
                else
                {
                    this.logic.UpdateMeAmpliacionfecha(ampliacion);
                }
            }
            catch
            {
                resultado = 0;
            }
            return Json(resultado);
        }

    }
}
