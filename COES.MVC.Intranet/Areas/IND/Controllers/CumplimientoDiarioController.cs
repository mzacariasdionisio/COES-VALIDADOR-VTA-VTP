using COES.Dominio.DTO.Sic;
using COES.MVC.Intranet.Areas.IND.Models;
using COES.MVC.Intranet.Controllers;
using COES.MVC.Intranet.Helper;
using COES.Servicios.Aplicacion.Helper;
using COES.Servicios.Aplicacion.IEOD;
using COES.Servicios.Aplicacion.Indisponibilidades;
using COES.Servicios.Aplicacion.Indisponibilidades.Helper;
using log4net;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.IND.Controllers
{
    public class CumplimientoDiarioController : BaseController
    {
        private readonly INDAppServicio _indAppServicio;

        #region Declaración de variables

        protected override void OnException(ExceptionContext filterContext)
        {
            try
            {
                log4net.Config.XmlConfigurator.Configure();
                Exception objErr = filterContext.Exception;
                Log.Error("Error", objErr);
            }
            catch (Exception ex)
            {
                Log.Fatal("Error", ex);
                throw;
            }
        }

        public CumplimientoDiarioController()
        {
            log4net.Config.XmlConfigurator.Configure();
            _indAppServicio = new INDAppServicio();
        }

        private readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private static string NameController = MethodBase.GetCurrentMethod().DeclaringType.Name;

        #endregion

        public ActionResult Index()
        {
            CumplimientoDiarioModel model = new CumplimientoDiarioModel();
            try
            {
                base.ValidarSesionJsonResult();

                //DateTime fechaPeriodo = _indAppServicio.GetPeriodoActual();

                model.ListaPeriodo = _indAppServicio.ListPeriodo()
                                        .Where(x => x.Iperihorizonte == ConstantesIndisponibilidades.HorizonteMensual)
                                        .ToList(); ;
                var empresas = _indAppServicio.ListIndRelacionEmpresa().Select(i => new { Emprcodi = i.Emprcodi, Emprnomb = i.Emprnomb })
                                                                             .Distinct().ToList();
                List<SiEmpresaDTO> lista = new List<SiEmpresaDTO>();
                foreach (var item in empresas)
                {
                    SiEmpresaDTO entidad = new SiEmpresaDTO
                    {
                        Emprcodi = item.Emprcodi,
                        Emprnomb = item.Emprnomb
                    };
                   lista.Add(entidad);
                }
                model.ListaEmpresa = lista;
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                //model.Mensaje = ex.Message;
            }

            return View(model);
        }

        /// <summary>
        /// Retorna Listado de cumplimiento de la informacion reportada por los agentes
        /// </summary>
        /// <param name="periodo">Identificador del periodo</param>
        /// <param name="empresa">Identificador de la empresa</param>
        /// <param name="estado">Estado Todos(0), Plazo(1) y Fuera Plazo(2)</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ListaCumplimiento(int periodo, int empresa, int estado)
        {
            CumplimientoDiarioModel model = new CumplimientoDiarioModel();

            try
            {
                base.ValidarSesionJsonResult();

                model.ListaCumplimiento = _indAppServicio.IndisponibilidadCumplimientoDiario(periodo, empresa, estado);
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                //model.Mensaje = ex.Message;
            }

            return Json(model);
        }

        /// <summary>
        /// Retorna Listado de Periodo por año en formato JSON
        /// </summary>
        /// <param name="periodo">Identificador del periodo</param>
        /// <param name="codigo">Codigo del registro en la tabla IND_CRDSUGAD</param>
        /// <param name="tipo">Indica si la data se visualiza o edita</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult EstadoInformacionPlazo(int periodo, int codigo, string tipo)
        {
            CumplimientoDiarioModel model = new CumplimientoDiarioModel();

            try
            {
                base.ValidarSesionJsonResult();
                model.CrdEstado = _indAppServicio.IndisponibilidadEstadoPlazo(periodo, codigo, tipo);
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                //model.Mensaje = ex.Message;
            }

            return Json(model);
        }

        /// <summary>
        /// Actualiza los estados del cumplimiento diario
        /// </summary>
        /// <param name="htEditar">Juego de datos, estados Fuera o en Plazo</param>
        /// <param name="codigo">Identificador de la tabla IND_CRDSUGAD</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GrabaCumplimiento(string[] htEditar, int codigo)
        {
            CumplimientoDiarioModel model = new CumplimientoDiarioModel();

            try
            {
                base.ValidarSesionJsonResult();
                model.Mensaje = _indAppServicio.GrabaCumplimiento(htEditar, codigo);
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                //model.Mensaje = ex.Message;
            }

            return Json(model);
        }

        /// <summary>
        /// Reporte de Cumplimiento
        /// </summary>
        /// <param name="datos">datos de la fila seleccionada</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ExportaCumplimiento(IndCrdSugadDTO datos)
        {
            string[] header2 = _indAppServicio.ObtenerDiasxPeriodo(datos.Ipericodi).ToArray();
            IndPeriodoDTO ePeriodo = _indAppServicio.GetByIdIndPeriodo(datos.Ipericodi);
            int numHeader2 = header2.Length;
            int[] ancho2 = new int[numHeader2];
            string[] data2 = new string[numHeader2];
            for (int i = 0; i < numHeader2; i++)
            {
                ancho2[i] = 20;
                data2[i] = datos.GetType().GetProperty("E" + (i + 1)).GetValue(datos, null).ToString();
            }
            string[] data1 = new string[8];
            data1[0] = datos.Emprnomb;
            data1[1] = datos.Equinombcentral;
            data1[2] = datos.Equinombunidad;
            data1[3] = ePeriodo.Iperinombre;
            data1[4] = datos.Indcbrfecha;
            data1[5] = datos.Cumplimiento;
            data1[6] = datos.Porcentaje.ToString();
            data1[7] = datos.Indcbrusucreacion;
            string reporte = "";
            try
            {
                base.ValidarSesionJsonResult();
                IndFormatoExcel data = new IndFormatoExcel()
                {
                    Cabecera1 = new string[] {
                    "EMPRESA","CENTRAL","UNIDAD","PERIODO", "FEC.ENVIO",
                    "CUMPLIMIENTO", "% CUMPLIMIENTO","USUARIO"
                    },
                    Cabecera2 = header2,
                    Contenido1 = data1,
                    Contenido2 = data2,
                    AnchoColumnas1 = new int[] {
                    20,20,20,20,20,20,20,20
                    },
                    AnchoColumnas2 = ancho2,
                    Titulo = "REPORTE DE CUMPLIMIENTO - INDISPONIBILIDADES",
                    Subtitulo1 = "(S = En Plazo y N = Fuera de Plazo)"
                    //Subtitulo2 = "sub2"
                };
                string pathFile = ConfigurationManager.AppSettings[ConstantesIndisponibilidades.ReporteCumplimiento].ToString();
                string filename = "Reporte de Cumplimiento";
                reporte = this._indAppServicio.ExportarReporteCumplimiento(data, pathFile, filename);
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                //model.Mensaje = ex.Message;
            }

            return Json(reporte);
        }

        /// <summary>
        /// Permite descargar el archivo Excel al explorador
        /// </summary>
        public virtual ActionResult AbrirArchivo(int formato, string file)
        {
            string sFecha = DateTime.Now.ToString("yyyyMMddHHmmss");
            string path = ConfigurationManager.AppSettings[ConstantesIndisponibilidades.ReporteCumplimiento].ToString() + file;
            string app = Constantes.AppExcel;
            return File(path, app, sFecha + "_" + file);
        }
    }
}
