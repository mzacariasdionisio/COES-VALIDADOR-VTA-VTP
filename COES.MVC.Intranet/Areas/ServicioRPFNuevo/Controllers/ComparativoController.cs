using COES.Dominio.DTO.Sic;
using COES.MVC.Intranet.Areas.ServicioRPFNuevo.Helper;
using COES.MVC.Intranet.Areas.ServicioRPFNuevo.Models;
using COES.MVC.Intranet.Controllers;
using COES.MVC.Intranet.Helper;
using COES.Servicios.Aplicacion.FormatoMedicion;
using COES.Servicios.Aplicacion.ServicioRPF;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.ServicioRPFNuevo.Controllers
{
    public class ComparativoController : BaseController
    {
        /// <summary>
        /// Instancia de la clase de servicio
        /// </summary>
        ComparativoAppServicio servicio = new ComparativoAppServicio();

        /// <summary>
        /// Pagina inicial del módulo
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            ComparativoModel model = new ComparativoModel();
            model.ListaEmpresas = this.servicio.ObtenerEmpresas();
            model.ListaCentrales = this.servicio.ObtenerCentrales(-1, DateTime.Now.AddDays(-1));
            model.Fecha = DateTime.Now.AddDays(-1).ToString(Constantes.FormatoFecha);

            return View(model);
        }

        /// <summary>
        /// Pagina inicial de la opcion de equivalencias de puntos de medicion
        /// </summary>
        /// <returns></returns>
        public ActionResult Equivalencia()
        {
            EquivalenciaModel model = new EquivalenciaModel();
            model.ListaEmpresas = this.servicio.ObtenerEmpresas();
            model.ListCentrales = this.servicio.ObtenerCentrales(-1, DateTime.Now);
            return View(model);
        }

        /// <summary>
        /// Permite obtener la tabla de equivalencias
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <param name="idCentral"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ObtenerEquivalencia(int? idEmpresa, int? idCentral)
        {
            string ruta = Url.Content("~/");
            return Json(this.servicio.ObtenerRelacion(idEmpresa, idCentral, ruta));
        }

        /// <summary>
        /// Permite obtener las centrales de una empresa
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ObtenerCentrales(int idEmpresa, string fecha)
        {
            DateTime fechaConsulta = string.IsNullOrEmpty(fecha) ? DateTime.Now : DateTime.ParseExact(fecha, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            return Json(this.servicio.ObtenerCentrales(idEmpresa, fechaConsulta));
        }

        /// <summary>
        /// Permite obtener la edición de equivalencias
        /// </summary>
        /// <param name="idCentral"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult EquivalenciaEdit(int idCentral)
        {
            EquivalenciaModel model = new EquivalenciaModel();
            EqEquipoDTO equipo = new EqEquipoDTO();
            SiEmpresaDTO empresa = new SiEmpresaDTO();
            List<MePtorelacionDTO> list = this.servicio.ObtenerConfiguracion(idCentral, out equipo, out empresa);
            model.IdCentral = idCentral;
            model.Central = equipo.Equinomb;
            model.Empresa = empresa.Emprnomb;
            model.ListaServicioRpf = list.Where(x => x.Origlectcodi == 1).ToList();
            model.ListaDespacho = list.Where(x => x.Origlectcodi == 2).ToList();

            return PartialView(model);
        }

        /// <summary>
        /// Permite grabar las relaciones entre los puntos de medicion
        /// </summary>
        /// <param name="idCentral"></param>
        /// <param name="idsRpf"></param>
        /// <param name="idsDespacho"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult EquivalenciaSave(int idCentral, string idsRpf, string idsDespacho)
        {
            try
            {
                this.servicio.GrabarEquivalencia(idCentral, idsRpf, idsDespacho, base.UserName);
                return Json(1);
            }
            catch
            {
                return Json(-1);
            }
        }

        /// <summary>
        /// Permite obtener la data
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <param name="idCentral"></param>
        /// <param name="fecha"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Comparativo(int? idEmpresa, int? idCentral, string fecha)
        {
            return Json(this.CalcularComparativo(idEmpresa, idCentral, fecha));
        }

        /// <summary>
        /// Obtener comparativo
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <param name="idCentral"></param>
        /// <param name="fecha"></param>
        /// <returns></returns>
        private List<ComparativoItemModelrpf> CalcularComparativo(int? idEmpresa, int? idCentral, string fecha)
        {
            //DateTime f1 = DateTime.ParseExact("01/10/2018", Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            //DateTime f2 = DateTime.ParseExact("31/10/2018", Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            //(new COES.Servicios.Aplicacion.Migraciones.MigracionesAppServicio()).ObtenerGenerarFileZip(f1, f2);
            DateTime fechaConsulta = DateTime.ParseExact(fecha, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            List<int> puntosRpf = new List<int>();
            List<int> puntosDespacho = new List<int>();
            this.servicio.ObtenerPuntosMedicion(idEmpresa, idCentral, out puntosRpf, out puntosDespacho, fechaConsulta);

            string rpf = string.Join<int>(Constantes.CaracterComa.ToString(), puntosRpf);
            string despacho = string.Join<int>(Constantes.CaracterComa.ToString(), puntosDespacho);

            List<MeMedicion60DTO> datosRpf = (new RpfAppServicio()).ObtenerDatosComparacionRangoResolucion(fechaConsulta, rpf, ParametrosFormato.ResolucionMediaHora).ToList();
            MeMedicion48DTO datosDespacho = this.servicio.ObtenerDatosDespacho(fechaConsulta, despacho);

            List<ComparativoItemModelrpf> list = ComparativoHelper.ObtenerComparativo(fechaConsulta, datosRpf, datosDespacho);

            return list;
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
        public JsonResult Exportar(int? idEmpresa, int? idCentral, string fecha)
        {
            try
            {
                string path = AppDomain.CurrentDomain.BaseDirectory + RutaDirectorio.RutaServicioRpf;
                string file = NombreArchivo.ReporteComparacionRPF;
                List<ComparativoItemModelrpf> comparativo = this.CalcularComparativo(idEmpresa, idCentral, fecha);
                ComparativoHelper.ExportarComparativo(comparativo, path, file, fecha);

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
            string fullPath = AppDomain.CurrentDomain.BaseDirectory + RutaDirectorio.RutaServicioRpf + NombreArchivo.ReporteComparacionRPF;
            return File(fullPath, Constantes.AppExcel, NombreArchivo.ReporteComparacionRPF);
        }

        /// <summary>
        /// Permite exportar los datos de RPF
        /// </summary>
        /// <param name="fecha"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ExportarRPF(string fecha)
        {
            try
            {
                string path = AppDomain.CurrentDomain.BaseDirectory + RutaDirectorio.RutaServicioRpf;
                string file = NombreArchivo.ReporteRpfMedioHorario;
                DateTime fechaConsulta = DateTime.ParseExact(fecha, Constantes.FormatoFecha, CultureInfo.InvariantCulture);

                List<MePtorelacionDTO> puntos = this.servicio.ObtenerPuntosRPF(fechaConsulta);
                string ptomedicodi = string.Join<int>(Constantes.CaracterComa.ToString(), puntos.Select(x => (int)x.Ptomedicodi));
                List<MeMedicion60DTO> list = (new RpfAppServicio()).ObtenerDatosComparacionRangoResolucion(fechaConsulta, ptomedicodi, ParametrosFormato.ResolucionMediaHora).ToList();

                ComparativoHelper.ExportarDatosRPF(list, puntos, fecha, path, file);

                return Json(1);
            }
            catch
            {
                return Json(-1);
            }
        }

        /// <summary>
        /// Permite descargar los datos RPF
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public virtual ActionResult DescargarRPF()
        {
            string fullPath = AppDomain.CurrentDomain.BaseDirectory + RutaDirectorio.RutaServicioRpf + NombreArchivo.ReporteRpfMedioHorario;
            return File(fullPath, Constantes.AppExcel, NombreArchivo.ReporteRpfMedioHorario);
        }
    }
}
