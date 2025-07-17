using COES.Dominio.DTO.Sic;
using COES.Framework.Base.Tools;
using COES.MVC.Intranet.Areas.PronosticoDemanda.Models;
using COES.MVC.Intranet.Controllers;
using COES.MVC.Intranet.Helper;
using COES.Servicios.Aplicacion.Factory;
using COES.Servicios.Aplicacion.FormatoMedicion;
using COES.Servicios.Aplicacion.PronosticoDemanda;
using COES.Servicios.Aplicacion.PronosticoDemanda.Helper;
using System.Configuration;
using COES.Servicios.Aplicacion.ServicioRPF;
using log4net;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.PronosticoDemanda.Controllers
{
    public class BitacoraController : Controller
    {
        PronosticoDemandaAppServicio servicio = new PronosticoDemandaAppServicio();

        public ActionResult Index()
        {
            BitacoraModel model = new BitacoraModel();
            model.FechaIni = DateTime.Now
                .ToString(ConstantesProdem.FormatoFecha);
            model.FechaFin = DateTime.Now
                .AddDays(1)
                .ToString(ConstantesProdem.FormatoFecha);            
            model.ListTipoEmpresa = UtilProdem.ListTipoEmpresa();
            model.ListTipoDemanda = UtilProdem.ListTipoDemanda();
            return View(model);
        }

        /// <summary>
        /// Lista la bitacora
        /// </summary>
        /// <param name="fechaIni">Fecha inicio del rango de consulta</param>
        /// <param name="fechaFin">Fecha final del rango de consulta</param>
        /// <param name="lect">Identificador del tipo de lectura</param>
        /// <param name="empr">Identificador de la empresa</param>
        /// <returns></returns>
        public JsonResult BitacoraList(int start, 
            int length, string fechaIni,
            string fechaFin, List<int> lect, 
            List<int> empr, string tipreg)
        {
            object res = new object();            
            if (tipreg == "0")
                res = this.servicio.ListBitacora(fechaIni,
                    fechaFin, lect,
                    empr, start,
                    length);
            if (tipreg == "1")
                res = this.servicio.ListNewBitacora(fechaIni, 
                    fechaFin, lect,
                        empr, tipreg,
                        start, length);
            return Json(res);
        }

        /// <summary>
        /// Exportar la grilla a un documento excel.
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        public JsonResult Exportar(string[][] form)
        {
            PrnFormatoExcel data = new PrnFormatoExcel() {

                Contenido = form,
                Cabecera = new string[] {
                    "AREA", "EMPRESA", "FECHA INICIO", "HORA INICIO",
                    "FECHA FIN", "HORA FIN", "CONSUMO TIPICO", "CONSUMO PREVISTO",
                    "DIFERENCIAL", "BARRA", "PTO. MEDICION", "OCURRENCIA"
                },
                AnchoColumnas = new int[] { 
                    50,50,50,50,50,50,
                    50,50,50,50,50,50
                },
                Titulo = "PRONOSTICO DE LA DEMANDA - PRODEM",
                Subtitulo1 = "Bitacora",
                Subtitulo2 = "sub2"          
            };
            string pathFile = ConfigurationManager.AppSettings[ConstantesProdem.ReportePronostico].ToString();
            string filename = "Reporte Bitacora";
            string reporte = this.servicio.ExportarReporteSimple(data,pathFile,filename);

            return Json(reporte);
        }

        /// <summary>
        /// Permite descargar el archivo Excel al explorador
        /// </summary>
        public virtual ActionResult AbrirArchivo(int formato, string file)
        {
            string sFecha = DateTime.Now.ToString("yyyyMMddHHmmss");
            string path = ConfigurationManager.AppSettings[ConstantesProdem.ReportePronostico].ToString() + file;
            string app = Constantes.AppExcel;
            return File(path, app, sFecha + "_" + file);
        }
    }
}