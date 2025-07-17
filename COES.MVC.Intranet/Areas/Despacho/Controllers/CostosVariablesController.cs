using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using COES.Dominio.DTO.Sic;
using COES.MVC.Intranet.Areas.Despacho.Helper;
using COES.MVC.Intranet.Areas.Despacho.ViewModels;
using COES.MVC.Intranet.Areas.Equipamiento.Models;
using COES.MVC.Intranet.Helper;
using COES.Servicios.Aplicacion.Equipamiento;
using log4net;
using COES.Servicios.Aplicacion.Despacho.Helper;
using COES.Framework.Base.Tools;
using COES.Servicios.Aplicacion.FormatoMedicion;
using COES.Servicios.Aplicacion.Helper;
using System.IO;
using System.Reflection;

namespace COES.MVC.Intranet.Areas.Despacho.Controllers
{
    public class CostosVariablesController : Controller
    {
        //
        // GET: /Despacho/CostosVariables/
        DespachoAppServicio appDespacho = new DespachoAppServicio();
        List<DatoComboBox> lsTipoRepCV = new List<DatoComboBox>();
        private static readonly ILog log = log4net.LogManager.GetLogger(typeof(CostosVariablesController));
        private static string NameController = MethodBase.GetCurrentMethod().DeclaringType.Name;

        protected override void OnException(ExceptionContext filterContext)
        {
            try
            {
                log4net.Config.XmlConfigurator.Configure();
                Exception objErr = filterContext.Exception;
                log.Error(NameController, objErr);
            }
            catch (Exception ex)
            {
                log.Fatal(NameController, ex);
                throw;
            }
        }

        /// <summary>
        /// Nombre del archivo
        /// </summary>
        public String NombreFile
        {
            get
            {
                return (Session[ConstantesFormato.SesionNombreArchivo] != null) ?
                    Session[ConstantesFormato.SesionNombreArchivo].ToString() : null;
            }
            set { Session[ConstantesFormato.SesionNombreArchivo] = value; }
        }

        public CostosVariablesController()
        {
            lsTipoRepCV.Add(new DatoComboBox() { Descripcion = ConstantesDespacho.TipoProgramaDiarioDesc, Valor = ConstantesDespacho.TipoProgramaDiario });
            lsTipoRepCV.Add(new DatoComboBox() { Descripcion = ConstantesDespacho.TipoProgramaSemanalDesc, Valor = ConstantesDespacho.TipoProgramaSemanal });
        }

        public ActionResult Index()
        {
            var modelo = new IndexCostosVariablesViewModel();
            DateTime fechaTmp = DateTime.Today.AddDays(35);
            modelo.FechaFin = new DateTime(fechaTmp.Year, fechaTmp.Month, 1);
            modelo.FechaInicio = modelo.FechaFin.AddMonths(-3);
            modelo.ListaTipoPrograma = lsTipoRepCV;
            modelo.Anho = DateTime.Now.Year.ToString();
            modelo.SemanaActual = EPDate.f_numerosemana(DateTime.Now.AddDays(1)).ToString();
            modelo.FechaDia = DateTime.Now.AddDays(1).ToString(ConstantesAppServicio.FormatoFecha);

            return View(modelo);
        }

        public PartialViewResult ListadoCostosVariables(string tipoPrograma, string fechaIni, string fechaFin, string anio, string semanaOp)
        {
            var modelo = new IndexCostosVariablesViewModel();
            DateTime fechaInicio = DateTime.Now, fechaFinal = DateTime.Now;
            if (tipoPrograma == ConstantesDespacho.TipoProgramaSemanal)
            {
                fechaInicio = EPDate.f_fechainiciosemana(Int32.Parse(anio), Int32.Parse(semanaOp));
                fechaFinal = fechaInicio.AddDays(6);
            }
            else
            {
                fechaInicio = DateTime.ParseExact(fechaIni, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                fechaFinal = DateTime.ParseExact(fechaFin, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            }

            modelo.ListadoCostos = appDespacho.GetByCriteriaPrRepcvs(tipoPrograma, fechaInicio, fechaFinal);

            return PartialView(modelo);
        }

        public PartialViewResult ListadoCostosVariablesDocumentos(string tipoPrograma, string fechaIni, string fechaFin, string anio, string semanaOp)
        {
            var modelo = new IndexCostosVariablesViewModel();
            DateTime fechaInicio = DateTime.Now, fechaFinal = DateTime.Now;
            if (tipoPrograma == ConstantesDespacho.TipoProgramaSemanal)
            {
                fechaInicio = EPDate.f_fechainiciosemana(Int32.Parse(anio), Int32.Parse(semanaOp));
                fechaFinal = fechaInicio.AddDays(6);
            }
            else
            {
                fechaInicio = DateTime.ParseExact(fechaIni, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                fechaFinal = DateTime.ParseExact(fechaFin, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            }

            modelo.ListaDocumentosCV = appDespacho.ListarDocumentosCostoVariables(fechaInicio, fechaFinal);

            return PartialView(modelo);
        }

        public ActionResult ViewCostoVariable(int repcodi)
        {
            var oRepCv = appDespacho.GetByIdPrRepcv(repcodi);
            var lEscenarios = appDespacho.GetEscenariosPorFechaRepCv(oRepCv.Repfecha);
            List<string> lsEscenarios = new List<string>();
            foreach (var pEscenarioDto in lEscenarios)
            {
                lsEscenarios.Add(pEscenarioDto.Escenomb);
            }
            var oModelo = new DetalleCostoVariableViewModel()
            {
                Enabled = "Disabled",
                dFechaRepCV = oRepCv.Repfecha,
                sDetalleRepCV = oRepCv.Repdetalle,
                sNombreRepCV = oRepCv.Repnomb,
                sObservaciones = oRepCv.Repobserva,
                iRepCodi = oRepCv.Repcodi,
                lsTipo = lsTipoRepCV,
                sTipoRepCV = oRepCv.Reptipo,
                lsEscenarios = lsEscenarios
            };
            return View(oModelo);
        }

        /// <summary>
        /// Formulario de Detalle
        /// </summary>
        /// <param name="repcodi"></param>
        /// <returns></returns>
        public PartialViewResult FormularioDetalle(int repcodi)
        {
            DetalleCostoVariableViewModel modelo = new DetalleCostoVariableViewModel();

            DateTime fecha = DateTime.Now.AddDays(1);
            modelo.ListaTipoPrograma = lsTipoRepCV;
            modelo.Anho = DateTime.Now.Year.ToString();
            modelo.SemanaActual = EPDate.f_numerosemana(fecha).ToString();
            modelo.FechaDia = fecha.ToString(ConstantesAppServicio.FormatoFecha);
            modelo.FechaDiaEmision = DateTime.Now.ToString(ConstantesAppServicio.FormatoFechaFull2);
            modelo.sDetalleRepCV = "...";
            modelo.sObservaciones = "...";
            modelo.sTipoRepCV = ConstantesDespacho.TipoProgramaDiario;
            modelo.sNombreRepCV = "CVAR" + fecha.Year + "s" + Convert.ToString(EPDate.f_numerosemana(fecha));
            modelo.sNombreRepCV = modelo.sTipoRepCV != ConstantesDespacho.TipoProgramaSemanal ? modelo.sNombreRepCV + "_" : modelo.sNombreRepCV;

            if (repcodi > 0)
            {
                PrRepcvDTO oRepCv = appDespacho.GetByIdPrRepcv(repcodi);

                modelo.iRepCodi = oRepCv.Repcodi;
                modelo.Anho = oRepCv.Repfecha.ToString();
                modelo.SemanaActual = EPDate.f_numerosemana(oRepCv.Repfecha).ToString();
                modelo.FechaDia = oRepCv.Repfecha.ToString(ConstantesAppServicio.FormatoFecha);
                modelo.FechaDiaEmision = oRepCv.Repfechaem != null ? oRepCv.Repfechaem.Value.ToString(ConstantesAppServicio.FormatoFechaFull2) : string.Empty;
                modelo.sDetalleRepCV = oRepCv.Repdetalle;
                modelo.sObservaciones = oRepCv.Repobserva;
                modelo.sNombreRepCV = oRepCv.Repnomb;
                modelo.sTipoRepCV = oRepCv.Reptipo;
            }

            return PartialView(modelo);
        }

        /// <summary>
        /// Guardar Detalle
        /// </summary>
        /// <param name="repcodi"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GuardarDetalle(int repcodi, string tipoPrograma, string strfecha, string anio, string semanaOp, string nombre, string detalle, string observacion, string strFechaEmision)
        {
            int result = 1;
            try
            {
                DateTime fechaEmision = DateTime.ParseExact(strFechaEmision, ConstantesAppServicio.FormatoFechaFull2, CultureInfo.InvariantCulture);
                if (repcodi > 0)
                {
                    PrRepcvDTO oRepCv = appDespacho.GetByIdPrRepcv(repcodi);
                    oRepCv.Repfechaem = fechaEmision;
                    oRepCv.Repnomb = nombre;
                    oRepCv.Repdetalle = detalle;
                    oRepCv.Repobserva = observacion;
                    oRepCv.Lastuser = User.Identity.Name;
                    oRepCv.Lastdate = DateTime.Now;

                    this.appDespacho.UpdatePrRepcv(oRepCv);
                }
                else
                {
                    DateTime fecha = DateTime.Now;
                    if (tipoPrograma == ConstantesDespacho.TipoProgramaSemanal)
                    {
                        fecha = EPDate.f_fechainiciosemana(Int32.Parse(anio), Int32.Parse(semanaOp));
                    }
                    else
                    {
                        fecha = DateTime.ParseExact(strfecha, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                    }
                    nombre = nombre != null ? nombre.ToString() : string.Empty;

                    PrRepcvDTO objVal = this.appDespacho.GetByFechaAndTipo(fecha, tipoPrograma);
                    if (objVal != null)
                    {
                        throw new Exception("Ya existe un registro para el tipo de programa y fecha seleccionados.");
                    }

                    PrRepcvDTO oRepCv = new PrRepcvDTO();
                    oRepCv.Repfecha = fecha;
                    oRepCv.Repfechaem = fechaEmision;
                    oRepCv.Reptipo = tipoPrograma;
                    oRepCv.Repnomb = nombre;
                    oRepCv.Repdetalle = detalle;
                    oRepCv.Repobserva = observacion;
                    oRepCv.Lastuser = User.Identity.Name;
                    oRepCv.Lastdate = DateTime.Now;
                    oRepCv.Deleted = ConstantesAppServicio.NO;

                    this.appDespacho.SavePrRepcv(oRepCv);
                }

                return Json(result);
            }
            catch (Exception ex)
            {
                log.Error("CostosVariablesController", ex);
                return Json(new { Error = -1, Descripcion = ex.Message });
            }
        }

        [HttpPost]
        public PartialViewResult ParametrosPorRepCv(int repcodi)
        {
            var oRepCv = appDespacho.GetByIdPrRepcv(repcodi);
            var lsParamtrosActualizados = appDespacho.ParametrosActualizadosPorFecha(oRepCv.Repfecha);
            var oModelo = new ParametrosRepCvViewModel()
            {
                iRepCodi = oRepCv.Repcodi,
                listaParametros = lsParamtrosActualizados
            };
            return PartialView(oModelo);
        }
        [HttpPost]
        public PartialViewResult CostosVariablesPorRepCv(int repcodi)
        {
            var oRepCv = appDespacho.GetByIdPrRepcv(repcodi);
            var lsCostos = appDespacho.GetCostosVariablesPorRepCv(oRepCv.Repcodi);
            var oModelo = new CostosVariableRepCvViewModel()
            {
                iRepCodi = oRepCv.Repcodi,
                listaCostosVariables = lsCostos
            };
            return PartialView(oModelo);
        }

        public ActionResult ReporteCostosVariables(int repcodi)
        {
            var oRepCv = appDespacho.GetByIdPrRepcv(repcodi);
            var oModelo = new DetalleCostoVariableViewModel()
            {
                Enabled = "Disabled",
                dFechaRepCV = oRepCv.Repfecha,
                sDetalleRepCV = oRepCv.Repdetalle,
                sNombreRepCV = oRepCv.Repnomb,
                sObservaciones = oRepCv.Repobserva,
                iRepCodi = oRepCv.Repcodi,
                lsTipo = lsTipoRepCV,
                sTipoRepCV = oRepCv.Reptipo,
                dFechaEmision = oRepCv.Repfechaem.Value
            };

            oModelo.NombreGenerarReporte = (oRepCv.Repfecha >= DateTime.Today) ? "oficial" : "Histórico";

            return View(oModelo);
        }
        [HttpPost]
        public PartialViewResult ListaReporteCostosVariables(int repcodi)
        {
            var lsResultado = new List<PrCvariablesDTO>();
            var oRepCv = appDespacho.GetByIdPrRepcv(repcodi);
            //appDespacho.GetReporteCostosVariablesPorRepCv(repcodi);
            appDespacho.GenerarCostosVariables(oRepCv, ref lsResultado, false);
            var modelo = new CostosVariableRepCvViewModel
            {
                listaCostosVariables = lsResultado
            };
            return PartialView(modelo);
        }
        [HttpPost]
        public PartialViewResult ListaParametrosGeneralesRepCv(int repcodi)
        {
            var oRepCv = appDespacho.GetByIdPrRepcv(repcodi);
            var lsParamtrosGen = appDespacho.ParametrosGeneralesPorFecha(oRepCv.Repfecha);
            var modelo = new ParametrosRepCvViewModel
            {
                listaParametros = lsParamtrosGen
            };
            return PartialView(modelo);
        }
        [HttpPost]
        public JsonResult ExportarReporteCostosVariablesPorRepCv(int repcodi)
        {
            int result = 1;
            try
            {
                var lsResultado = new List<PrCvariablesDTO>();
                var oRepCv = appDespacho.GetByIdPrRepcv(repcodi);
                bool bBaseDatos = (oRepCv.Repfecha >= DateTime.Today);
                appDespacho.GenerarCostosVariables(oRepCv, ref lsResultado, bBaseDatos);
                var lsParamtrosGen = appDespacho.ParametrosGeneralesPorFecha(oRepCv.Repfecha);
                ExcelDocument.GenerarReporteOficialCostosVariables(lsResultado, lsParamtrosGen, oRepCv);
            }
            catch (Exception ex)
            {
                log.Error(NameController, ex);
                result = -1;
            }
            return Json(result);
        }
        [HttpGet]
        public virtual ActionResult DescargarRerporteCostosVariablesPorRepCv()
        {
            string fullPath = ConfigurationManager.AppSettings[RutaDirectorio.ReporteDespacho] + NombreArchivo.ReporteOficialCostosVariablesRepCv;
            return File(fullPath, Constantes.AppExcel, NombreArchivo.ReporteOficialCostosVariablesRepCv);
        }

        /// <summary>
        /// Lista de Semana por Año
        /// </summary>
        /// <param name="idAnho"></param>
        /// <returns></returns>
        public JsonResult CargarSemanas(string idAnho)
        {
            string anioActual = DateTime.Now.Year.ToString();

            var model = new IndexCostosVariablesViewModel();
            List<DatoComboBox> entitys = new List<DatoComboBox>();
            if (idAnho == "0")
            {
                idAnho = anioActual;
            }
            DateTime dfecha = new DateTime(Int32.Parse(idAnho), 12, 31);
            int nsemanas = EPDate.TotalSemanasEnAnho(Int32.Parse(idAnho), FirstDayOfWeek.Saturday);

            for (int i = 1; i <= nsemanas; i++)
            {
                DatoComboBox reg = new DatoComboBox();
                reg.Valor = i.ToString();
                reg.Descripcion = "Sem" + i + "-" + idAnho;
                entitys.Add(reg);

            }
            model.ListaSemana = entitys;
            model.SemanaActual = "1";
            if (anioActual == idAnho)
            {
                model.SemanaActual = EPDate.f_numerosemana(DateTime.Now).ToString();
            }

            return Json(model);
        }

        /// <summary>
        /// Obtener nombre prefijo de la nueva actualizacion de CV
        /// </summary>
        /// <param name="repcodi"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetNombreRepCV(string tipoPrograma, string strFecha, string anio, string semanaOp)
        {
            string result = string.Empty;
            try
            {
                DateTime fecha = DateTime.Now;
                if (tipoPrograma == ConstantesDespacho.TipoProgramaSemanal)
                {
                    fecha = EPDate.f_fechainiciosemana(Int32.Parse(anio), Int32.Parse(semanaOp));
                }
                else
                {
                    fecha = DateTime.ParseExact(strFecha, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                }
                string sNombreRepCV = "CVAR" + fecha.Year + "s" + Convert.ToString(EPDate.f_numerosemana(fecha));
                sNombreRepCV = tipoPrograma != ConstantesDespacho.TipoProgramaSemanal ? sNombreRepCV + "_" : sNombreRepCV;

                result = sNombreRepCV;
            }
            catch (Exception ex)
            {
                log.Error("CostosVariablesController", ex);
            }

            return Json(result);
        }

    }
}
