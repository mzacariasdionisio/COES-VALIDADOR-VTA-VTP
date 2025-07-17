using COES.Dominio.DTO.Sic;
using COES.MVC.Publico.Areas.Mediciones.Models;
using COES.MVC.Publico.Areas.ReportePotencia.Models;
using COES.MVC.Publico.Helper;
using COES.Servicios.Aplicacion.Mediciones;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace COES.MVC.Publico.Areas.Mediciones.Controllers
{
    public class MedidoresGeneracionController : Controller
    {
        /// <summary>
        /// Clase para acceso a los datos y bl
        /// </summary>
        ConsultaMedidoresAppServicio servicio = new ConsultaMedidoresAppServicio();

        /// <summary>
        /// Carga inicial de la página
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            MedidoresGeneracionModel model = new MedidoresGeneracionModel();            
            model.ListaTipoGeneracion = this.servicio.ListaTipoGeneracion();
            model.ListaTipoEmpresas = this.servicio.ListaTipoEmpresas();

            DateTime fecha = DateTime.Now.AddMonths(-1);
            DateTime fechaInicio = new DateTime(fecha.Year, fecha.Month, 1);
            DateTime fechaFin = fechaInicio.AddMonths(1).AddDays(-1);
            model.FechaInicio = fechaInicio.ToString(Constantes.FormatoFechaISO);
            model.FechaFin = fechaFin.ToString(Constantes.FormatoFechaISO);
            model.FechaActualInicio = (new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1)).ToString(Constantes.FormatoFechaISO);
            model.FechaActualFin= (new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1)).AddMonths(1).AddDays(-1).ToString(Constantes.FormatoFechaISO);
            model.FechaAnteriorInicio = (new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1)).AddMonths(-1).ToString(Constantes.FormatoFechaISO);
            model.FechaAnteriorFin = (new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1)).AddDays(-1).ToString(Constantes.FormatoFechaISO);

            return View(model);
        }

        /// <summary>
        /// Listar parámetros disponibles
        /// </summary>
        /// <param name="formato"></param>
        /// <param name="fechaInicial"></param>
        /// <param name="fechaFinal"></param>
        /// <param name="parametros"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ValidarParametro(string fechaInicial, string fechaFinal, string tiposEmpresa, string empresas, string tiposGeneracion,
            int central)
        {
            try
            {
                List<EstadoModel> listaOpcion = new List<EstadoModel>();

                DateTime fecInicio = DateTime.Now;
                DateTime fecFin = DateTime.Now;

                if (!string.IsNullOrEmpty(fechaInicial))
                {
                    fecInicio = DateTime.ParseExact(fechaInicial, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                }
                if (!string.IsNullOrEmpty(fechaFinal))
                {
                    fecFin = DateTime.ParseExact(fechaFinal, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                }


                listaOpcion.Add(new EstadoModel() { EstadoCodigo = "1", EstadoDescripcion = "Potencia Activa (MW)" });

                int nroRegistrosReact = 0, nroRegistrosReactCap = 0, nroRegistrosReactInd = 0;

                if (!string.IsNullOrEmpty(empresas))
                {
                    nroRegistrosReact = this.servicio.ObtenerNroRegistroConsultaMedidores(fecInicio, fecFin, tiposEmpresa,
                        empresas, tiposGeneracion, central, 5);
                    nroRegistrosReactCap = this.servicio.ObtenerNroRegistroConsultaMedidores(fecInicio, fecFin, tiposEmpresa,
                        empresas, tiposGeneracion, central, 2);
                    nroRegistrosReactInd = this.servicio.ObtenerNroRegistroConsultaMedidores(fecInicio, fecFin, tiposEmpresa,
                        empresas, tiposGeneracion, central, 4);
                }

                if (nroRegistrosReact > 0 || (nroRegistrosReactCap == 0 || nroRegistrosReactInd == 0))
                {
                    listaOpcion.Add(new EstadoModel() { EstadoCodigo = "5", EstadoDescripcion = "Potencia Reactiva (MVAR)" });
                }

                if (nroRegistrosReactCap > 0 && nroRegistrosReactInd > 0)
                {
                    listaOpcion.Add(new EstadoModel() { EstadoCodigo = "2", EstadoDescripcion = "Potencia Reactiva Capacitiva (MVAR)" });
                    listaOpcion.Add(new EstadoModel() { EstadoCodigo = "4", EstadoDescripcion = "Potencia Reactiva Inductiva (MVAR)" });
                }

                listaOpcion.Add(new EstadoModel() { EstadoCodigo = "3", EstadoDescripcion = "Servicios Auxiliares" });

                return Json(listaOpcion);
            }
            catch (Exception ex)
            {
                return Json(-1);
            }
        }

        /// <summary>
        /// Permite cargar las empresas por los tipos seleccionados
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult Empresas(string tiposEmpresa)
        {
            MedidoresGeneracionModel model = new MedidoresGeneracionModel();
            List<SiEmpresaDTO> entitys = this.servicio.ObteneEmpresasPorTipo(tiposEmpresa);
            model.ListaEmpresas = entitys;
           
            return PartialView(model);                   
        }

        /// <summary>
        /// Permite listar los registros de medidores de generación
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult Lista(string fechaInicial, string fechaFinal, string tiposEmpresa, string empresas, string tiposGeneracion,
            int central, int parametro, int nroPagina)
        {
            MedidoresGeneracionModel model = new MedidoresGeneracionModel();

            DateTime fecInicio = DateTime.Now;
            DateTime fecFin = DateTime.Now;

            if (!string.IsNullOrEmpty(fechaInicial)) {
                fecInicio = DateTime.ParseExact(fechaInicial, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                model.FechaInicio = fecInicio.ToString(Constantes.FormatoFecha);
            }
            if (!string.IsNullOrEmpty(fechaFinal)) {
                fecFin = DateTime.ParseExact(fechaFinal, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                model.FechaFin = fecFin.ToString(Constantes.FormatoFecha);
            }
            this.ConsultaValidacion(fecInicio, ref tiposEmpresa, ref empresas);
            if (string.IsNullOrEmpty(empresas)) empresas = Constantes.ParametroDefecto;
            if (string.IsNullOrEmpty(tiposGeneracion)) tiposGeneracion = Constantes.ParametroDefecto;

            List<MeMedicion96DTO> sumatoria = new List<MeMedicion96DTO>();
            model.ListaDatos = this.servicio.ObtenerConsultaMedidores(fecInicio, fecFin, tiposEmpresa, empresas,
                tiposGeneracion, central, parametro,
                nroPagina, Constantes.PageSizeMedidores, out sumatoria);

            model.EntidadTotal = sumatoria;

            string header = string.Empty;
            if (parametro == 1) header = "Total Energía Activa  (MWh)";
            if (parametro == 2) header = "Total Energía Reactiva (MVarh)";
            if (parametro == 3) header = "Total Servicios Auxiliares (MWh)";

            model.TextoCabecera = header;

            return PartialView(model);
        }

        /// <summary>
        /// Permite mostrar el paginado de la consulta
        /// </summary>
        /// <param name="modelo"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult Paginado(string fechaInicial, string fechaFinal, string tiposEmpresa, string empresas, string tiposGeneracion,
            int central, int parametro)
        {
            MedidoresGeneracionModel model = new MedidoresGeneracionModel();

            DateTime fecInicio = DateTime.Now;
            DateTime fecFin = DateTime.Now;

            if (!string.IsNullOrEmpty(fechaInicial))
            {
                fecInicio = DateTime.ParseExact(fechaInicial, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            }
            if (!string.IsNullOrEmpty(fechaFinal))
            {
                fecFin = DateTime.ParseExact(fechaFinal, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            }

            this.ConsultaValidacion(fecInicio, ref tiposEmpresa, ref empresas);

            if (string.IsNullOrEmpty(empresas)) empresas = Constantes.ParametroDefecto;
            if (string.IsNullOrEmpty(tiposGeneracion)) tiposGeneracion = Constantes.ParametroDefecto;

            int nroRegistros = this.servicio.ObtenerNroRegistroConsultaMedidores(fecInicio, fecFin, tiposEmpresa,
                empresas, tiposGeneracion, central, parametro);

            if (nroRegistros > Constantes.NroPageShow)
            {
                int pageSize = Constantes.PageSizeMedidores;
                int nroPaginas = (nroRegistros % pageSize == 0) ? nroRegistros / pageSize : nroRegistros / pageSize + 1;
                model.NroPaginas = nroPaginas;
                model.NroMostrar = Constantes.NroPageShow;
                model.IndicadorPagina = true;
            }
            
            return PartialView(model);
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
        public JsonResult Exportar(string fechaInicial, string fechaFinal, string tiposEmpresa, string empresas, string tiposGeneracion,
            int central, string parametros, int tipo) 
        {
            try
            {
                DateTime fecInicio = DateTime.Now;
                DateTime fecFin = DateTime.Now;

                if (!string.IsNullOrEmpty(fechaInicial))
                {
                    fecInicio = DateTime.ParseExact(fechaInicial, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                }
                if (!string.IsNullOrEmpty(fechaFinal))
                {
                    fecFin = DateTime.ParseExact(fechaFinal, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                }

                this.ConsultaValidacion(fecInicio, ref tiposEmpresa, ref empresas);

                if (string.IsNullOrEmpty(empresas)) empresas = Constantes.ParametroDefecto;
                if (string.IsNullOrEmpty(tiposGeneracion)) tiposGeneracion = Constantes.ParametroDefecto;

                string path = ConfigurationManager.AppSettings[RutaDirectorio.ReporteMediciones];
                string file = string.Empty;

                if (tipo == 1) file = NombreArchivo.ReporteMedidoresHorizontal;
                if (tipo == 2) file = NombreArchivo.ReporteMedidoresVertical;
                if (tipo == 3) file = NombreArchivo.ReporteMedidoresCSV;

                this.servicio.GenerarArchivoExportacion(fecInicio, fecFin, tiposEmpresa, empresas, tiposGeneracion, central, 
                    parametros, path, file, tipo, false);
               

                return Json("1");
            }
            catch(Exception ex)
            {
                return Json(ex.Message);
            }
        }

        /// <summary>
        /// Permite abrir el archivo del reporte generado
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public virtual ActionResult Descargar(int tipo)
        {
            string file = string.Empty;
            string app = Constantes.AppExcel;

            if (tipo == 1) file = NombreArchivo.ReporteMedidoresHorizontal;
            if (tipo == 2) file = NombreArchivo.ReporteMedidoresVertical;
            if (tipo == 3)
            {
                file = NombreArchivo.ReporteMedidoresCSV;
                app = Constantes.AppCSV;
            }

            string fullPath = ConfigurationManager.AppSettings[RutaDirectorio.ReporteMediciones] + file;
            return File(fullPath, app, file);
        }

        /// <summary>
        /// Valida la selección de datos de exportación
        /// </summary>
        /// <param name="formato"></param>
        /// <param name="fechaInicial"></param>
        /// <param name="fechaFinal"></param>
        /// <param name="parametros"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ValidarExportacion(int formato, string fechaInicial, string fechaFinal, string parametros)
        {
            try
            {
                DateTime fecInicio = DateTime.Now;
                DateTime fecFin = DateTime.Now;

                if (!string.IsNullOrEmpty(fechaInicial))
                {
                    fecInicio = DateTime.ParseExact(fechaInicial, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                }
                if (!string.IsNullOrEmpty(fechaFinal))
                {
                    fecFin = DateTime.ParseExact(fechaFinal, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                }

                TimeSpan ts = fecFin.Subtract(fecInicio);

                if (ts.TotalDays > 31)
                {
                    return Json(2);
                }

                if (formato == 3)
                {
                    if (!string.IsNullOrWhiteSpace(parametros))
                    {
                        string[] ids = parametros.Split(Constantes.CaracterComa);

                        if (ids.Count() > 1)
                        {
                            return Json(3);
                        }
                    }
                    else 
                    {
                        return Json(4);
                    }
                }

                return Json(1);
            }
            catch
            {
                return Json(-1);
            }
        }

        /// <summary>
        /// Valida los empresas que ya han pasado la validacion de datos
        /// </summary>
        /// <param name="fecha"></param>
        /// <param name="tiposEmpresa"></param>
        /// <param name="empresas"></param>
        private void ConsultaValidacion(DateTime fecha, ref string tiposEmpresa, ref string empresas)
        {
            /// Codigo modificado
            #region Verificando plazos de validacion

            if (DateTime.Now.Year == fecha.Year && DateTime.Now.Month - 1 == fecha.Month)
            {
                List<int> idsTipoEmpresas = new List<int>();
                List<int> idsEmpresas = new List<int>();
                List<MdValidacionDTO> validacion = this.servicio.ObtenerValidacionMes(fecha);

                if (!string.IsNullOrEmpty(tiposEmpresa))
                {
                    idsTipoEmpresas = tiposEmpresa.Split(Constantes.CaracterComa).Select(int.Parse).ToList();
                }
                if (!string.IsNullOrEmpty(empresas))
                {
                    idsEmpresas = empresas.Split(Constantes.CaracterComa).Select(int.Parse).ToList();
                }

                if (idsTipoEmpresas.Count == 0)
                {
                    if (idsEmpresas.Count == 0)
                    {
                        empresas = string.Join<int>(Constantes.CaracterComa.ToString(), validacion.Select(x => x.Emprcodi));
                    }
                    else
                    {
                        List<int> subIdsEmpresas = idsEmpresas.Where(x => validacion.Any(y => y.Emprcodi == x)).Distinct().ToList();
                        if (subIdsEmpresas.Count == 0)
                        {
                            empresas = (-2).ToString();
                        }
                        else
                        {
                            empresas = string.Join<int>(Constantes.CaracterComa.ToString(), subIdsEmpresas);
                        }
                    }
                }
                else
                {
                    List<int> subIdsEmpresas = validacion.Where(x => idsTipoEmpresas.Any(y => y == x.Tipoemprcodi)).Select(x => x.Emprcodi).Distinct().ToList();

                    if (idsEmpresas.Count == 0)
                    {
                        if (subIdsEmpresas.Count == 0)
                        {
                            empresas = (-2).ToString();
                        }
                        else
                        {
                            empresas = string.Join<int>(Constantes.CaracterComa.ToString(), subIdsEmpresas);
                        }
                    }
                    else
                    {
                        List<int> subCodigosEmpresas = idsEmpresas.Where(x => subIdsEmpresas.Any(y => y == x)).Distinct().ToList();

                        if (subCodigosEmpresas.Count == 0)
                        {
                            empresas = (-2).ToString();
                        }
                        else
                        {
                            empresas = string.Join<int>(Constantes.CaracterComa.ToString(), subCodigosEmpresas);
                        }
                    }
                }
            }

            #endregion
        }
    }
}
