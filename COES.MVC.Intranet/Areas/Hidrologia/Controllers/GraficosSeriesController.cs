using COES.Dominio.DTO.Sic;
using COES.Framework.Base.Tools;
using COES.MVC.Intranet.Controllers;
using COES.MVC.Intranet.Helper;
using COES.Servicios.Aplicacion.FormatoMedicion;
using COES.Servicios.Aplicacion.Helper;
using COES.Servicios.Aplicacion.Hidrologia;
using log4net;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using COES.Servicios.Aplicacion.General;
using COES.MVC.Intranet.Areas.TiempoReal.Models;
using COES.Dominio.DTO.Scada;
using COES.Servicios.Aplicacion.TiempoReal.Helper;
using COES.Servicios.Aplicacion.Equipamiento;
using COES.MVC.Intranet.Areas.Hidrologia.Helper;
using COES.Servicios.Aplicacion.IEOD;

namespace COES.MVC.Intranet.Areas.Hidrologia.Controllers
{
    public class GraficosSeriesController : BaseController
    {
        public List<FLecturaSp7DTO> ListaFLecturaSp7
        {
            get
            {
                return (Session[ConstantesTiempoReal.ListaFrecuencia] != null)
                    ? (List<FLecturaSp7DTO>)Session[ConstantesTiempoReal.ListaFrecuencia]
                    : new List<FLecturaSp7DTO>();
            }
            set { Session[ConstantesTiempoReal.ListaFrecuencia] = value; }
        }
        #region Propiedades

        /// <summary>
        /// Nombre del archivo
        /// </summary>
        public String NombreFile
        {
            get
            {
                return (Session[DatosSesionDemandaCP.SesionNombreArchivo] != null) ?
                    Session[DatosSesionDemandaCP.SesionNombreArchivo].ToString() : null;
            }
            set { Session[DatosSesionDemandaCP.SesionNombreArchivo] = value; }
        }

        /// <summary>
        /// Almacena solo en nombre del archivo
        /// </summary>
        public String FileName
        {
            get
            {
                return (Session[DatosSesionDemandaCP.SesionFileName] != null) ?
                    Session[DatosSesionDemandaCP.SesionFileName].ToString() : null;
            }
            set { Session[DatosSesionDemandaCP.SesionFileName] = value; }
        }

        /// <summary>
        /// Codigo del envio
        /// </summary>
        public int IdEnvio
        {
            get
            {
                return (Session[DatosSesionDemandaCP.SesionIdEnvio] != null) ?
                    (int)Session[DatosSesionDemandaCP.SesionIdEnvio] : 0;
            }
            set { Session[DatosSesionDemandaCP.SesionIdEnvio] = value; }
        }

        /// <summary>
        /// Nombre del formato
        /// </summary>
        public MeFormatoDTO Formato
        {
            get
            {
                return (Session[DatosSesionDemandaCP.SesionFormato] != null) ?
                    (MeFormatoDTO)Session[DatosSesionDemandaCP.SesionFormato] : new MeFormatoDTO();
            }
            set { Session[DatosSesionDemandaCP.SesionFormato] = value; }
        }

        /// <summary>
        /// Matriz de datos
        /// </summary>
        public string[][] MatrizExcel
        {
            get
            {
                return (Session[DatosSesionDemandaCP.SesionMatrizExcel] != null) ?
                    (string[][])Session[DatosSesionDemandaCP.SesionMatrizExcel] : new string[1][];
            }
            set { Session[DatosSesionDemandaCP.SesionMatrizExcel] = value; }
        }

        #endregion

        EquipamientoAppServicio appEquipamiento = new EquipamientoAppServicio();
        FormatoMedicionAppServicio logic = new FormatoMedicionAppServicio();
        HidrologiaAppServicio logicHidro = new HidrologiaAppServicio();
        SeguridadServicio.SeguridadServicioClient seguridad = new SeguridadServicio.SeguridadServicioClient();
        private GeneralAppServicio logicGeneral = new GeneralAppServicio();
        public FormatoMedicionAppServicio servicio = new FormatoMedicionAppServicio();


        #region Declaración de variables

        private static readonly ILog Log = log4net.LogManager.GetLogger(typeof(CargaDatosController));
        private static string NameController = MethodBase.GetCurrentMethod().DeclaringType.Name;

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

        #endregion

        /// <summary>
        /// Permite cargar los archivos
        /// </summary>
        /// <returns></returns>
        public ActionResult Upload()
        {
            MeArchivoDTO archivo = new MeArchivoDTO();
            MeEnvioDTO envio = new MeEnvioDTO();
            try
            {
                if (Request.Files.Count == 1)
                {
                    var file = Request.Files[0];
                    string fileRandom = System.IO.Path.GetRandomFileName();
                    this.FileName = fileRandom + "." + NombreArchivoDemandaCP.ExtensionFileUploadHidrologia;
                    string fileName = ConfigurationManager.AppSettings[RutaDirectorio.RutaCargaFile] + this.FileName;
                    this.NombreFile = fileName;
                    file.SaveAs(fileName);
                }

                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
        }


        /// <summary>
        /// Carga principal de la pantalla
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            FormatoModel model = new FormatoModel();
            model.ListaTipoSerie = servicio.ListarTipoSerie();
            model.ListaTipoPuntoMedicion = servicio.ListarTipoPuntoMedicion();
            //model.IdEnvio = 0;
            List<string> semanas = new List<string>();
            int nsemanas = EPDate.TotalSemanasEnAnho(DateTime.Now.Year, 6);
            for (int i = 1; i <= nsemanas; i++)
            {
                semanas.Add(i.ToString().PadLeft(2, '0'));
            }
            int nroSemana = EPDate.f_numerosemana(DateTime.Now);
            model.Anho = DateTime.Now.Year.ToString();
            model.NroSemana = nroSemana;
            model.Dia = DateTime.Now.ToString(Constantes.FormatoFecha);
            model.TipoSerie = 1;
            model.FechaIni = new DateTime(1965, 1, 1).ToString(Constantes.FormatoMesAnio3);
            model.FechaFin = DateTime.Now.AddMonths(-1).ToString(Constantes.FormatoMesAnio3);

            return View(model);
        }

        [HttpPost]
        public PartialViewResult graficoMensual()
        {
            FormatoModel model = new FormatoModel();
            model.ListaTipoSerie = servicio.ListarTipoSerie();
            model.ListaTipoPuntoMedicion = servicio.ListarTipoPuntoMedicion();

            model.IdEnvio = 0;
            
            var currentYear = DateTime.Now.Year;
            var years = new List<int>();

            for (int year = 1965; year <= currentYear; year++)
            {
                years.Add(year);
            }

            model.ListAnio = years;
            model.TipoSerie = 1;
            return PartialView(model);

        }
        [HttpPost]
        public PartialViewResult graficoComparativaVolumen()
        {
            FormatoModel model = new FormatoModel();
            model.ListaTipoSerie = servicio.ListarTipoSerie();
            model.ListaTipoPuntoMedicion = servicio.ListarTipoPuntoMedicion().Where(i=> i.TipoPtoMediCodi==7).ToList();
            model.IdEnvio = 0;
            var currentYear = DateTime.Now.Year;
            var years = new List<int>();

            for (int year = 1965; year <= currentYear; year++)
            {
                years.Add(year);
            }
            model.ListAnio = years;
            model.TipoSerie = 1;
            return PartialView(model);
        }
        [HttpPost]
        public PartialViewResult graficoComparativaNaturalEvaporada()
        {
            FormatoModel model = new FormatoModel();
            model.ListaTipoSerie = servicio.ListarTipoSerie();
            model.ListaTipoPuntoMedicion = servicio.ListarTipoPuntoMedicion().Where(i => i.TipoPtoMediCodi != 7).ToList();
            model.IdEnvio = 0;
            var currentYear = DateTime.Now.Year;
            var years = new List<int>();
            for (int year = 1965; year <= currentYear; year++)
            {
                years.Add(year);
            }
            model.ListAnio = years;
            model.TipoSerie = 1;
            return PartialView(model);
        }
        [HttpPost]
        public ActionResult graficoComparativaLineaTendencia()
        {
            FormatoModel model = new FormatoModel();
            model.ListaTipoSerie = servicio.ListarTipoSerie();
            model.ListaTipoPuntoMedicion = servicio.ListarTipoPuntoMedicion();
            model.IdEnvio = 0;
            List<string> semanas = new List<string>();
            int nsemanas = EPDate.TotalSemanasEnAnho(DateTime.Now.Year, 6);
            for (int i = 1; i <= nsemanas; i++)
            {
                semanas.Add(i.ToString().PadLeft(2, '0'));
            }
            int nroSemana = EPDate.f_numerosemana(DateTime.Now);
            model.Anho = DateTime.Now.Year.ToString();
            model.NroSemana = nroSemana;
            model.Dia = DateTime.Now.ToString(Constantes.FormatoFecha);
            model.TipoSerie = 1;
            model.FechaIni = new DateTime(1965, 1, 1).ToString(Constantes.FormatoMesAnio3);
            model.FechaFin = DateTime.Now.AddMonths(-1).ToString(Constantes.FormatoMesAnio3);
            return View(model);
        }

        [HttpPost]
        public PartialViewResult graficoBarrasEstadisticasAnuales()
        {
            FormatoModel model = new FormatoModel();
            model.ListaTipoSerie = servicio.ListarTipoSerie();
            model.ListaTipoPuntoMedicion = servicio.ListarTipoPuntoMedicion();
            model.ListadoCuenca = appEquipamiento.ObtenerEquipoPadresHidrologicosCuencaTodos();
            model.IdEnvio = 0;
            var currentYear = DateTime.Now.Year;
            var years = new List<int>();

            for (int year = 1965; year <= currentYear; year++)
            {
                years.Add(year);
            }
            model.ListAnio = years;
            model.TipoSerie = 1;
            return PartialView(model);
        }
        [HttpPost]
        public ActionResult graficoTablaVertical()
        {
            FormatoModel model = new FormatoModel();
            model.ListaTipoSerie = servicio.ListarTipoSerie();
            model.ListaTipoPuntoMedicion = servicio.ListarTipoPuntoMedicion();
            model.IdEnvio = 0;
            List<string> semanas = new List<string>();
            int nsemanas = EPDate.TotalSemanasEnAnho(DateTime.Now.Year, 6);
            for (int i = 1; i <= nsemanas; i++)
            {
                semanas.Add(i.ToString().PadLeft(2, '0'));
            }
            int nroSemana = EPDate.f_numerosemana(DateTime.Now);
            model.Anho = DateTime.Now.Year.ToString();
            model.NroSemana = nroSemana;
            model.Dia = DateTime.Now.ToString(Constantes.FormatoFecha);
            model.TipoSerie = 1;
            model.FechaIni = new DateTime(1965, 1, 1).ToString(Constantes.FormatoMesAnio3);
            model.FechaFin = DateTime.Now.AddMonths(-1).ToString(Constantes.FormatoMesAnio3);
            return View(model);
        }
        [HttpPost]
        public ActionResult graficoMatricesMensuales()
        {
            FormatoModel model = new FormatoModel();
            model.ListaTipoSerie = servicio.ListarTipoSerie();
            model.ListaTipoPuntoMedicion = servicio.ListarTipoPuntoMedicion();
            model.IdEnvio = 0;
            List<string> semanas = new List<string>();
            int nsemanas = EPDate.TotalSemanasEnAnho(DateTime.Now.Year, 6);
            for (int i = 1; i <= nsemanas; i++)
            {
                semanas.Add(i.ToString().PadLeft(2, '0'));
            }
            int nroSemana = EPDate.f_numerosemana(DateTime.Now);
            model.Anho = DateTime.Now.Year.ToString();
            model.NroSemana = nroSemana;
            model.Dia = DateTime.Now.ToString(Constantes.FormatoFecha);
            model.TipoSerie = 1;
            model.FechaIni = new DateTime(1965, 1, 1).ToString(Constantes.FormatoMesAnio3);
            model.FechaFin = DateTime.Now.AddMonths(-1).ToString(Constantes.FormatoMesAnio3);
            return View(model);
        }
        [HttpPost]
        public ActionResult graficoModeloPerseo()
        {
            FormatoModel model = new FormatoModel();
            model.ListaTipoSerie = servicio.ListarTipoSerie();
            model.ListaTipoPuntoMedicion = servicio.ListarTipoPuntoMedicion();
            model.IdEnvio = 0;
            List<string> semanas = new List<string>();
            int nsemanas = EPDate.TotalSemanasEnAnho(DateTime.Now.Year, 6);
            for (int i = 1; i <= nsemanas; i++)
            {
                semanas.Add(i.ToString().PadLeft(2, '0'));
            }
            int nroSemana = EPDate.f_numerosemana(DateTime.Now);
            model.Anho = DateTime.Now.Year.ToString();
            model.NroSemana = nroSemana;
            model.Dia = DateTime.Now.ToString(Constantes.FormatoFecha);
            model.TipoSerie = 1;
            model.FechaIni = new DateTime(1965, 1, 1).ToString(Constantes.FormatoMesAnio3);
            model.FechaFin = DateTime.Now.AddMonths(-1).ToString(Constantes.FormatoMesAnio3);
            return View(model);
        }
        [HttpPost]
        public PartialViewResult graficoPrueba()
        {
            BusquedaFLecturaSp7Model model = new BusquedaFLecturaSp7Model();
            model.ListaFLecturaSp7Grafica = ListaFLecturaSp7;
            return PartialView(model);
        }
        [HttpPost]
        public JsonResult CargarEquiposCuencaHidro()
        {
            List<EqEquipoDTO> lsPadres = new List<EqEquipoDTO>();
            lsPadres = appEquipamiento.ObtenerEquipoPadresHidrologicosCuencaTodos().ToList();
            var list = new SelectList(lsPadres, "EQUICODI", "EQUINOMB");
            return Json(list);
        }

        [HttpPost]
        public ActionResult CargarPtoMedicion(int cuenca, int tptomedicodi)
        {
            List<MePtomedicionDTO> listaPtoMedicion = new List<MePtomedicionDTO>();
            listaPtoMedicion = servicio.ListarPuntoMedicionPorCuenca(cuenca, tptomedicodi);
            var list = new SelectList(listaPtoMedicion, "Ptomedicodi", "Ptomedielenomb");
            return Json(list);
        }

        [HttpPost]
        public ActionResult CargarPtoMedicionNaturalEvaporado(int cuenca, int tipopuntomedicion)
        {
            List<MePtomedicionDTO> listaPtoMedicion = new List<MePtomedicionDTO>();
            listaPtoMedicion = servicio.ListarPtoMedicionCuencaPorTipoPtoMedicion(cuenca, tipopuntomedicion);
            var list = new SelectList(listaPtoMedicion, "Ptomedicodi", "Ptomedielenomb");
            return Json(list);
        }
        [HttpPost]
        public ActionResult ObtenerGraficoAnual(int tiposeriecodi , int tptomedicodi, int ptomedicodi, int anioinicio, int aniofin)
        {
            List<GraficoSeries> listaGrafico = new List<GraficoSeries>();
            MePtomedicionDTO ptoMedicion = new MePtomedicionDTO();
            ptoMedicion = servicio.GetPtoMedicionById(ptomedicodi);
            if (ptoMedicion.PtomediCalculado=="N")
            {
                listaGrafico = servicio.ObtenerGraficoAnual(tiposeriecodi, tptomedicodi, ptomedicodi, anioinicio, aniofin);
            } else
            {
                listaGrafico = servicio.ObtenerCaudalPuntosCalculados(tiposeriecodi, tptomedicodi, ptomedicodi, anioinicio, aniofin, "ANUAL", "");
            }
            return Json(listaGrafico);
        } 

        [HttpPost]
        public ActionResult ObtenerGraficoMensual(int tiposeriecodi, int tptomedicodi, int ptomedicodi, int anioinicio, int aniofin)
        {
            MePtomedicionDTO ptoMedicion = new MePtomedicionDTO();
            ptoMedicion = servicio.GetPtoMedicionById(ptomedicodi);
            List<GraficoSeries> listaGraficoMensual = new List<GraficoSeries>();
            List<GraficoSeries> listaGraficoTotal = new List<GraficoSeries>();
            if (ptoMedicion.PtomediCalculado == "N")
            {
                listaGraficoMensual = servicio.ObtenerGraficoMensual(tiposeriecodi, tptomedicodi, ptomedicodi, anioinicio);
                listaGraficoTotal = servicio.ObtenerGraficoTotal(tiposeriecodi, tptomedicodi, ptomedicodi, aniofin);
            }
            else
            {
                listaGraficoMensual = servicio.ObtenerCaudalPuntosCalculados(tiposeriecodi, tptomedicodi, ptomedicodi, anioinicio, 0, "MENSUAL", "");
                listaGraficoTotal = servicio.ObtenerCaudalPuntosCalculados(tiposeriecodi, tptomedicodi, ptomedicodi, 0, aniofin, "TOTALMENSUAL", "");
            }

            var data = new GraficoSeries
            {
                ListaGraficoMensual = listaGraficoMensual,
                ListaGraficoTotal = listaGraficoTotal
            };

            return Json(data);
        }
        [HttpPost]
        public ActionResult ObtenerGraficoComparativaVolumen(int tiposeriecodi, int tptomedicodi, int ptomedicodi, string anios, int aniofin)
        {
            MePtomedicionDTO ptoMedicion = new MePtomedicionDTO();
            ptoMedicion = servicio.GetPtoMedicionById(ptomedicodi);
            List<GraficoSeries> listaGraficoMensual = new List<GraficoSeries>();
            List<GraficoSeries> listaGraficoTotal = new List<GraficoSeries>();
            if (ptoMedicion.PtomediCalculado == "N")
            {
                listaGraficoMensual = servicio.ObtenerGraficoComparativaVolumen(tiposeriecodi, tptomedicodi, ptomedicodi, anios);
                listaGraficoTotal = servicio.ObtenerGraficoTotal(tiposeriecodi, tptomedicodi, ptomedicodi, aniofin);
            }
            else
            {
                listaGraficoMensual = servicio.ObtenerCaudalPuntosCalculados(tiposeriecodi, tptomedicodi, ptomedicodi, 0, 0, "COMPARATIVAVOLUMEN", anios);
                listaGraficoTotal = servicio.ObtenerCaudalPuntosCalculados(tiposeriecodi, tptomedicodi, ptomedicodi, 0, aniofin, "TOTALMENSUAL", "");
            }           

            var data = new GraficoSeries
            {
                ListaGraficoMensual = listaGraficoMensual,
                ListaGraficoTotal = listaGraficoTotal
            };
            return Json(data);
        }

        [HttpPost]
        public ActionResult ListarInformacionPuntoMedicionPorEmpresa(int CodPuntoMedicion)
        {
            MePtomedicionDTO ptoMedicion = new MePtomedicionDTO();
            ptoMedicion = servicio.GetPtoMedicionById(CodPuntoMedicion);
            return Json(ptoMedicion);
        }

        [HttpPost]
        public ActionResult ObtenerGraficoComparativaNaturalEvaporada(int tiposeriecodi, int ptomedicodi, string anioinicio, int aniofin)
        {
            List<GraficoSeries> listaGraficoMensual = new List<GraficoSeries>();
            List<GraficoSeries> listaGraficoTotal = new List<GraficoSeries>();

            MePtomedicionDTO ptoMedicion = new MePtomedicionDTO();
            ptoMedicion = servicio.GetPtoMedicionById(ptomedicodi);           
            if (ptoMedicion.PtomediCalculado == "N")
            {
                listaGraficoMensual = servicio.ObtenerGraficoComparativaNaturalEvaporada(tiposeriecodi, ptomedicodi, anioinicio);
                listaGraficoTotal = servicio.ObtenerGraficoTotalNaturalEvaporada(tiposeriecodi, ptomedicodi, aniofin);

            }
            else
            {
                listaGraficoMensual = servicio.ObtenerCaudalPuntosCalculados(tiposeriecodi, 0, ptomedicodi, 0, 0, "COMPARATIVANATURALEVAPORADA", anioinicio);
                listaGraficoTotal = servicio.ObtenerCaudalPuntosCalculados(tiposeriecodi, 0, ptomedicodi, 0, aniofin, "TOTALCOMPARATIVANATURALEVAPORADA", anioinicio);
            }

            
            var data = new GraficoSeries
            {
                ListaGraficoMensual = listaGraficoMensual,
                ListaGraficoTotal = listaGraficoTotal
            };
            return Json(data);
        }

        [HttpPost]
        public ActionResult ObtenerGraficoEstadisticasAnuales(int tiposeriecodi, int tptomedicodi, int ptomedicodi, int anioinicio, int mesinicio, int aniofin, int mesfin)
        {
            List<GraficoSeries> listaGraficoAnuales = new List<GraficoSeries>();

            MePtomedicionDTO ptoMedicion = new MePtomedicionDTO();
            ptoMedicion = servicio.GetPtoMedicionById(ptomedicodi);
            if (ptoMedicion.PtomediCalculado == "N")
            {
                listaGraficoAnuales = servicio.ObtenerGraficoEstadisticasAnuales(tiposeriecodi, tptomedicodi, ptomedicodi, anioinicio, mesinicio, aniofin, mesfin);
            }
            else
            {
                listaGraficoAnuales = servicio.ObtenerCaudalPuntosCalculados(tiposeriecodi, tptomedicodi, ptomedicodi, anioinicio, aniofin, "ESTADISTICAS_ANUALES", "");
            }

            var data = new GraficoSeries
            {
                ListaGraficoMensual = listaGraficoAnuales,
            };

            return Json(data);
        }

        [HttpPost]
        public ActionResult ObtenerGraficoLineaTendencia(int tiposeriecodi, string ptomedicodi, int tptomedicodi, int anioinicio, int aniofin)
        {
            List<GraficoSeries> listaGraficoMensual = servicio.ObtenerGraficoComparativaLineaTendencia(tiposeriecodi, tptomedicodi, ptomedicodi, anioinicio, aniofin);
            var data = new GraficoSeries
            {
                ListaGraficoMensual = listaGraficoMensual,
            };
            return Json(data);
        }
        [HttpPost]
        public JsonResult ExportarGraficoAnual(int tiposeriecodi, int tptomedicodi, int ptomedicodi, int aniofin,string cuencaNombre, string valorEmbalse, string ptomedicionNombre)
        {
            GraficoSeries model = new GraficoSeries();
            List<GraficoSeries> listaGraficoTotal = servicio.ObtenerGraficoTotal(tiposeriecodi, tptomedicodi, ptomedicodi, aniofin);


            int result = 1;

            try
            {
                MePtomedicionDTO ptoMedicion = new MePtomedicionDTO();
                ptoMedicion = servicio.GetPtoMedicionById(ptomedicodi);
                if (ptoMedicion!=null)
                {
                    cuencaNombre = ptoMedicion.EquiPadrenomb;
                    valorEmbalse = ptoMedicion.Ptomedidesc;
                }
                
                ExcelDocument.GenerarArchivoGraficoAnual(listaGraficoTotal, tptomedicodi, cuencaNombre, valorEmbalse, ptomedicionNombre);

                result = 1;

            }
            catch
            {
                result = -1;
            }

            return Json(result);

        }
        [HttpPost]
        public JsonResult ExportarGraficoAnualDesvStandar(int tiposeriecodi, int tptomedicodi, int ptomedicodi, int aniofin, string cuencaNombre, string valorEmbalse, string ptomedicionNombre)
        {
            GraficoSeries model = new GraficoSeries();
            List<GraficoSeries> listaGraficoTotal = servicio.ObtenerGraficoTotal(tiposeriecodi, tptomedicodi, ptomedicodi, aniofin);


            int result = 1;

            try
            {
                MePtomedicionDTO ptoMedicion = new MePtomedicionDTO();
                ptoMedicion = servicio.GetPtoMedicionById(ptomedicodi);
                if (ptoMedicion != null)
                {
                    cuencaNombre = ptoMedicion.EquiPadrenomb;
                    valorEmbalse = ptoMedicion.Ptomedidesc;
                }

                ExcelDocument.GenerarArchivoGraficoAnualDesvStandar(listaGraficoTotal, tptomedicodi, cuencaNombre, valorEmbalse, ptomedicionNombre);

                result = 1;

            }
            catch
            {
                result = -1;
            }

            return Json(result);

        }
        [HttpPost]
        public JsonResult exportarGraficoMensual(int tiposeriecodi, int tptomedicodi, int ptomedicodi, int aniofin, string cuencaNombre, string valorEmbalse, string ptomedicionNombre)
        {
            GraficoSeries model = new GraficoSeries();
            List<GraficoSeries> listaGraficoTotal = servicio.ObtenerGraficoTotal(tiposeriecodi, tptomedicodi, ptomedicodi, aniofin);
            int result = 1;
            try
            {
                MePtomedicionDTO ptoMedicion = new MePtomedicionDTO();
                ptoMedicion = servicio.GetPtoMedicionById(ptomedicodi);
                if (ptoMedicion != null)
                {
                    cuencaNombre = ptoMedicion.EquiPadrenomb;
                    valorEmbalse = ptoMedicion.Ptomedidesc;
                }

                ExcelDocument.GenerarArchivoGraficoMensual(listaGraficoTotal, tptomedicodi, cuencaNombre, valorEmbalse, ptomedicionNombre);
                result = 1;
            }
            catch
            {
                result = -1;
            }

            return Json(result);

        }
        [HttpPost]
        public JsonResult exportarGraficoComparativaVolumen(int tiposeriecodi, int tptomedicodi, int ptomedicodi, int aniofin, string cuencaNombre, string valorEmbalse, string ptomedicionNombre)
        {
            GraficoSeries model = new GraficoSeries();
            List<GraficoSeries> listaGraficoTotal = new List<GraficoSeries>();
            int result = 1;
            try
            {
                MePtomedicionDTO ptoMedicion = new MePtomedicionDTO();
                ptoMedicion = servicio.GetPtoMedicionById(ptomedicodi);
                if (ptoMedicion != null)
                {
                    cuencaNombre = ptoMedicion.EquiPadrenomb;
                    valorEmbalse = ptoMedicion.Ptomedidesc;
                }

                if (ptoMedicion.PtomediCalculado == "N")
                {
                    
                    listaGraficoTotal = servicio.ObtenerGraficoTotal(tiposeriecodi, tptomedicodi, ptomedicodi, aniofin);

                }
                else
                {
                    listaGraficoTotal = servicio.ObtenerCaudalPuntosCalculados(tiposeriecodi, tptomedicodi, ptomedicodi, 0, aniofin, "TOTALMENSUAL", "");
                }

                ExcelDocument.GenerarArchivoGraficoComparativaVolumen(listaGraficoTotal, cuencaNombre, valorEmbalse, ptomedicionNombre, tptomedicodi);
                result = 1;
            }
            catch (Exception ex)
            {
                result = -1;
            }

            return Json(result);

        }
        [HttpPost]
        public JsonResult exportarGraficoComparativaNaturalEvaporada(int tiposeriecodi, int ptomedicodi, int aniofin, string cuencaNombre, string valorEmbalse, string ptomedicionNombre, string anios)
        {
            GraficoSeries model = new GraficoSeries();

            List<GraficoSeries> listaGraficoMensual = servicio.ObtenerGraficoComparativaNaturalEvaporada(tiposeriecodi, ptomedicodi, anios);


            List<GraficoSeries> listaGraficoTotal = servicio.ObtenerGraficoTotalNaturalEvaporada(tiposeriecodi, ptomedicodi, aniofin);
            int result = 1;
            try
            {
                MePtomedicionDTO ptoMedicion = new MePtomedicionDTO();
                ptoMedicion = servicio.GetPtoMedicionById(ptomedicodi);
                if (ptoMedicion != null)
                {
                    cuencaNombre = ptoMedicion.EquiPadrenomb;
                    valorEmbalse = ptoMedicion.Ptomedidesc;
                }
                ExcelDocument.GenerarArchivoGraficoComparativaNaturalEvaporada(listaGraficoMensual, listaGraficoTotal, cuencaNombre, valorEmbalse, ptomedicionNombre);
                result = 1;
            }
            catch
            {
                result = -1;
            }

            return Json(result);

        }
        [HttpPost]
        public JsonResult exportarGraficoComparativaLineaTendencia(int tiposeriecodi, string ptomedicodi, int aniofin, string cuencaNombre, string valorEmbalse, int tptomedicodi)
        {
            GraficoSeries model = new GraficoSeries();
            List<GraficoSeries> listaGraficoTotal = servicio.ObtenerGraficoTotalLineaTendencia(tiposeriecodi, ptomedicodi, aniofin, tptomedicodi);
            int result = 1;
            try
            {
                string[] arrayPtoMed = ptomedicodi.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                string strPtoMedicion = string.Empty;
                string strPtoMedicionNom = string.Empty;
                foreach (string strPtoMed in arrayPtoMed)
                {
                    int intPtoMed = Convert.ToInt32(strPtoMed);
                    MePtomedicionDTO ptoMedicion = new MePtomedicionDTO();
                    ptoMedicion = servicio.GetPtoMedicionById(intPtoMed);
                    if (ptoMedicion != null)
                    {
                        cuencaNombre = ptoMedicion.EquiPadrenomb;
                        valorEmbalse = ptoMedicion.Ptomedidesc;
                        strPtoMedicion = strPtoMedicion + ptoMedicion.Ptomedielenomb + " - ";
                        strPtoMedicionNom = strPtoMedicionNom + ptoMedicion.Ptomedidesc + " - ";
                    }
                }
                

                ExcelDocument.GenerarArchivoGraficoComparativaLineaTendencia(listaGraficoTotal, cuencaNombre, strPtoMedicionNom, strPtoMedicion, tptomedicodi);
                result = 1;
            }
            catch
            {
                result = -1;
            }

            return Json(result);

        }
        [HttpGet]
        public virtual ActionResult ExportarReporte()
        {
            string nombreArchivo = string.Empty;
            nombreArchivo = ConstantesHidrologia.NombreGraficoAnual;
            string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesHidrologia.FolderReporte;
            string fullPath = ruta + nombreArchivo;
            return File(fullPath, Constantes.AppExcel, nombreArchivo);
        }
        [HttpGet]
        public virtual ActionResult ExportarReporteMensual()
        {
            string nombreArchivo = string.Empty;
            nombreArchivo = ConstantesHidrologia.NombreGraficoMensual;
            string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesHidrologia.FolderReporte;
            string fullPath = ruta + nombreArchivo;
            return File(fullPath, Constantes.AppExcel, nombreArchivo);
        }

        [HttpGet]
        public virtual ActionResult ExportarReporteComparativaVolumen()
        {
            string nombreArchivo = string.Empty;
            nombreArchivo = ConstantesHidrologia.NombreGraficoComparativaVolumen;
            string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesHidrologia.FolderReporte;
            string fullPath = ruta + nombreArchivo;
            return File(fullPath, Constantes.AppExcel, nombreArchivo);
        }
        [HttpGet]
        public virtual ActionResult ExportarReporteComparativaNaturalEvaporada()
        {
            string nombreArchivo = string.Empty;
            nombreArchivo = ConstantesHidrologia.NombreGraficoComparativaNaturalEvaporada;
            string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesHidrologia.FolderReporte;
            string fullPath = ruta + nombreArchivo;
            return File(fullPath, Constantes.AppExcel, nombreArchivo);
        }
        [HttpGet]
        public virtual ActionResult ExportarReporteComparativaLineaTendencia()
        {
            string nombreArchivo = string.Empty;
            nombreArchivo = ConstantesHidrologia.NombreGraficoComparativaLineaTendencia;
            string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesHidrologia.FolderReporte;
            string fullPath = ruta + nombreArchivo;
            return File(fullPath, Constantes.AppExcel, nombreArchivo);
        }

        
    }
}
