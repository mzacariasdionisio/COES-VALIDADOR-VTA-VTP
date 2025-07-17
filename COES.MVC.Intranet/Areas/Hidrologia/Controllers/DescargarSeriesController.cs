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
using static COES.Servicios.Aplicacion.Siosein2.ConstantesSiosein2;
using System.Text;

namespace COES.MVC.Intranet.Areas.Hidrologia.Controllers
{
    public class DescargarSeriesController : BaseController
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

 
        public ActionResult Index()
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
            return PartialView(model);
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
        public ActionResult CargarPtoMedicionNaturalEvaporado(int cuenca)
        {
            List<MePtomedicionDTO> listaPtoMedicion = new List<MePtomedicionDTO>();
            listaPtoMedicion = servicio.ListarPuntoMedicionPorCuencaNaturalEvaporado(cuenca);
            var list = new SelectList(listaPtoMedicion, "Ptomedicodi", "Ptomedielenomb");
            return Json(list);
        }
        [HttpPost]
        public JsonResult exportarTablaVertical(int cuenca, int tptomedicodi, int tiposeriecodi, int anioinicio, int aniofin, string rangofechainicial,string rangofechafinal)
        {
            GraficoSeries model = new GraficoSeries();
            
            List<MePtomedicionDTO> listaPuntoMedicion = servicio.ListarPtoMedicionCuencaPorTipoPtoMedicion(cuenca, tptomedicodi);
            var ptomedicodiString = string.Join(",", listaPuntoMedicion.Select(p => p.Ptomedicodi));

            List<TablaVertical> listaTablaVertical = new List<TablaVertical>();
            if (listaPuntoMedicion.Count>0)
            {
                listaTablaVertical = servicio.ListaTablaVertical(ptomedicodiString, tptomedicodi, tiposeriecodi, anioinicio, aniofin);
            }           

            List<GraficoSeries> listaGrafico = new List<GraficoSeries>();

            foreach (var itemPtoMedicion in listaPuntoMedicion)
            {
                MePtomedicionDTO ptoMedicion = new MePtomedicionDTO();
                ptoMedicion = servicio.GetPtoMedicionById(itemPtoMedicion.Ptomedicodi);
                if (ptoMedicion.PtomediCalculado == "S")
                {
                    listaGrafico = servicio.ObtenerCaudalPuntosCalculados(tiposeriecodi, tptomedicodi, itemPtoMedicion.Ptomedicodi, anioinicio, aniofin, "TABLA_VERTICAL", "");
                    if (listaGrafico!=null)
                    {
                        foreach (var itemGrafico in listaGrafico)
                        {
                            TablaVertical itemTabla = new TablaVertical();
                            itemTabla.Anio = itemGrafico.Anio;
                            itemTabla.Caudal = ptoMedicion.EquiPadrenomb;
                            itemTabla.Ptomedielenomb = ptoMedicion.Ptomedibarranomb;
                            itemTabla.Equinomb = ptoMedicion.Ptomedibarranomb;
                            itemTabla.Emprnomb = ptoMedicion.Emprnomb;
                            itemTabla.M1 = itemGrafico.M1;
                            itemTabla.M2 = itemGrafico.M2;
                            itemTabla.M3 = itemGrafico.M3;
                            itemTabla.M4 = itemGrafico.M4;
                            itemTabla.M5 = itemGrafico.M5;
                            itemTabla.M6 = itemGrafico.M6;
                            itemTabla.M7 = itemGrafico.M7;
                            itemTabla.M8 = itemGrafico.M8;
                            itemTabla.M9 = itemGrafico.M9;
                            itemTabla.M10 = itemGrafico.M10;
                            itemTabla.M11 = itemGrafico.M11;
                            itemTabla.M12 = itemGrafico.M12;

                            listaTablaVertical.Add(itemTabla);

                        }
                    }
                }
                
            }

            int result = 1;
            try
            {
                if (listaTablaVertical.Count>0)
                {
                    ExcelDocument.GenerarArchivoTablaVertical(listaTablaVertical, rangofechainicial, rangofechafinal, tptomedicodi);
                    result = 1;
                } else
                {
                    result = 0;
                }
                
            }
            catch
            {
                result = -1;
            }

            return Json(result);

        }
        [HttpPost]
        public JsonResult exportarMatricesMensuales(int cuenca, int tptomedicodi, int tiposeriecodi, int anioinicio, int aniofin, string rangofechainicial, string rangofechafinal)
        {
            GraficoSeries model = new GraficoSeries();
            List<MePtomedicionDTO> listaPuntoMedicionFinal = new List<MePtomedicionDTO>();

            List<MePtomedicionDTO> listaPuntoMedicion = servicio.ListarPtoMedicionCuencaPorTipoPtoMedicion(cuenca, tptomedicodi);
            var ptomedicodiString = string.Join(",", listaPuntoMedicion.Select(p => p.Ptomedicodi));

            List<TablaVertical> listaTablaVertical = new List<TablaVertical>();

            if (listaPuntoMedicion.Count>0)
            {
                listaTablaVertical = servicio.ListaTablaVertical(ptomedicodiString, tptomedicodi, tiposeriecodi, anioinicio, aniofin);
            }
            

            List<GraficoSeries> listaGrafico = new List<GraficoSeries>();

            foreach (var itemPtoMedicion in listaPuntoMedicion)
            {
                List<TablaVertical> listaTablaVerticalPtoMed = listaTablaVertical.Where(i => i.Ptomedicodi == itemPtoMedicion.Ptomedicodi).ToList();
                int numRegistros = listaTablaVerticalPtoMed.Count();
                MePtomedicionDTO ptoMedicion = new MePtomedicionDTO();
                ptoMedicion = servicio.GetPtoMedicionById(itemPtoMedicion.Ptomedicodi);
                if (ptoMedicion.PtomediCalculado=="S")
                {
                    listaPuntoMedicionFinal.Add(ptoMedicion);

                    listaGrafico = servicio.ObtenerCaudalPuntosCalculados(tiposeriecodi, tptomedicodi, itemPtoMedicion.Ptomedicodi, anioinicio, aniofin, "TABLA_VERTICAL", "");

                    if (listaGrafico != null)
                    {
                        foreach (var itemGrafico in listaGrafico)
                        {
                            TablaVertical itemTabla = new TablaVertical();
                            itemTabla.Anio = itemGrafico.Anio;
                            itemTabla.Caudal = ptoMedicion.EquiPadrenomb;
                            itemTabla.Ptomedielenomb = ptoMedicion.Ptomedibarranomb;
                            itemTabla.Equinomb = ptoMedicion.Ptomedibarranomb;
                            itemTabla.Emprnomb = ptoMedicion.Emprnomb;
                            itemTabla.Ptomedicodi = ptoMedicion.Ptomedicodi;
                            itemTabla.M1 = itemGrafico.M1;
                            itemTabla.M2 = itemGrafico.M2;
                            itemTabla.M3 = itemGrafico.M3;
                            itemTabla.M4 = itemGrafico.M4;
                            itemTabla.M5 = itemGrafico.M5;
                            itemTabla.M6 = itemGrafico.M6;
                            itemTabla.M7 = itemGrafico.M7;
                            itemTabla.M8 = itemGrafico.M8;
                            itemTabla.M9 = itemGrafico.M9;
                            itemTabla.M10 = itemGrafico.M10;
                            itemTabla.M11 = itemGrafico.M11;
                            itemTabla.M12 = itemGrafico.M12;

                            listaTablaVertical.Add(itemTabla);

                        }
                    }

                } else
                {
                    if (numRegistros > 0)
                    {
                        listaPuntoMedicionFinal.Add(ptoMedicion);
                    }
                }
                
            }
                

            int result = 1;
            try
            {
                if (listaTablaVertical.Count>0)
                {
                    ExcelDocument.GenerarArchivoMatricesMensuales(listaTablaVertical, listaPuntoMedicionFinal, tptomedicodi);
                    result = 1;
                } else
                {
                    result = 0;
                }
                
            }
            catch
            {
                result = -1;
            }

            return Json(result);

        }
        [HttpPost]
        public JsonResult exportarModeloPerseo(int cuenca, int tptomedicodi, int tiposeriecodi, int anioinicio, int aniofin, string rangofechainicial, string rangofechafinal, string tipoArchivo)
        {
            
            GraficoSeries model = new GraficoSeries();
            List<MePtomedicionDTO> listaPuntoMedicionFinal = new List<MePtomedicionDTO>();

            List<MePtomedicionDTO> listaPuntoMedicion = servicio.ListarPtoMedicionCuencaPorTipoPtoMedicion(cuenca, tptomedicodi);
            var ptomedicodiString = string.Join(",", listaPuntoMedicion.Select(p => p.Ptomedicodi));

            List<TablaVertical> listaTablaVertical = new List<TablaVertical>();
            if (listaPuntoMedicion.Count>0)
            {
                listaTablaVertical = servicio.ListaTablaVertical(ptomedicodiString, tptomedicodi, tiposeriecodi, anioinicio, aniofin);
            }
            

            List<GraficoSeries> listaGrafico = new List<GraficoSeries>();

            foreach (var itemPtoMedicion in listaPuntoMedicion)
            {
                List<TablaVertical> listaTablaVerticalPtoMed = listaTablaVertical.Where(i => i.Ptomedicodi == itemPtoMedicion.Ptomedicodi).ToList();
                int numRegistros = listaTablaVerticalPtoMed.Count();
                MePtomedicionDTO ptoMedicion = new MePtomedicionDTO();
                ptoMedicion = servicio.GetPtoMedicionById(itemPtoMedicion.Ptomedicodi);
                if (ptoMedicion.PtomediCalculado == "S")
                {
                    listaPuntoMedicionFinal.Add(ptoMedicion);

                    listaGrafico = servicio.ObtenerCaudalPuntosCalculados(tiposeriecodi, tptomedicodi, itemPtoMedicion.Ptomedicodi, anioinicio, aniofin, "TABLA_VERTICAL", "");

                    if (listaGrafico != null)
                    {
                        foreach (var itemGrafico in listaGrafico)
                        {
                            TablaVertical itemTabla = new TablaVertical();
                            itemTabla.Anio = itemGrafico.Anio;
                            itemTabla.Caudal = ptoMedicion.EquiPadrenomb;
                            itemTabla.Ptomedielenomb = ptoMedicion.Ptomedibarranomb;
                            itemTabla.Equinomb = ptoMedicion.Ptomedibarranomb;
                            itemTabla.Emprnomb = ptoMedicion.Emprnomb;
                            itemTabla.Ptomedicodi = ptoMedicion.Ptomedicodi;
                            itemTabla.M1 = itemGrafico.M1;
                            itemTabla.M2 = itemGrafico.M2;
                            itemTabla.M3 = itemGrafico.M3;
                            itemTabla.M4 = itemGrafico.M4;
                            itemTabla.M5 = itemGrafico.M5;
                            itemTabla.M6 = itemGrafico.M6;
                            itemTabla.M7 = itemGrafico.M7;
                            itemTabla.M8 = itemGrafico.M8;
                            itemTabla.M9 = itemGrafico.M9;
                            itemTabla.M10 = itemGrafico.M10;
                            itemTabla.M11 = itemGrafico.M11;
                            itemTabla.M12 = itemGrafico.M12;

                            listaTablaVertical.Add(itemTabla);

                        }
                    }

                }
                else
                {
                    if (numRegistros > 0)
                    {
                        listaPuntoMedicionFinal.Add(ptoMedicion);
                    }
                }

            }


            int result = 1;
            try
            {
                if (listaTablaVertical.Count > 0)
                {
                    ExcelDocument.GenerarArchivoModeloPerseo(listaTablaVertical, listaPuntoMedicionFinal, tptomedicodi);
                    result = 1;
                } else
                {
                    result = 0;
                }
                
            }
            catch (Exception ex)
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
        public virtual ActionResult ExportarReporteTablaVertical()
        {
            string nombreArchivo = string.Empty;
            nombreArchivo = ConstantesHidrologia.NombreTablaVertical;
            string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesHidrologia.FolderReporte;
            string fullPath = ruta + nombreArchivo;
            return File(fullPath, Constantes.AppExcel, nombreArchivo);
        }

        [HttpGet]
        public virtual ActionResult ExportarReporteMatricesMensuales()
        {
            string nombreArchivo = string.Empty;
            nombreArchivo = ConstantesHidrologia.NombreMatricesMensuales;
            string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesHidrologia.FolderReporte;
            string fullPath = ruta + nombreArchivo;
            return File(fullPath, Constantes.AppExcel, nombreArchivo);
        }
        [HttpGet]
        public virtual ActionResult ExportarReporteExcelModeloPerseo()
        {
            string nombreArchivo = string.Empty;
            nombreArchivo = ConstantesHidrologia.NombreModeloPerseo;
            string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesHidrologia.FolderReporte;
            string fullPath = ruta + nombreArchivo;
            return File(fullPath, Constantes.AppExcel, nombreArchivo);
        }

        [HttpGet]
        public ActionResult ExportarReporteTxtModeloPerseo(int cuenca, int tptomedicodi, int tiposeriecodi, int anioinicio, int aniofin, string rangofechainicial, string rangofechafinal, string tipoArchivo)
        {
            GraficoSeries model = new GraficoSeries();
            List<MePtomedicionDTO> listaPuntoMedicionFinal = new List<MePtomedicionDTO>();

            List<MePtomedicionDTO> listaPuntoMedicion = servicio.ListarPtoMedicionCuencaPorTipoPtoMedicion(cuenca, tptomedicodi);
            var ptomedicodiString = string.Join(",", listaPuntoMedicion.Select(p => p.Ptomedicodi));

            List<TablaVertical> listaTablaVertical = servicio.ListaTablaVertical(ptomedicodiString, tptomedicodi, tiposeriecodi, anioinicio, aniofin);

            List<GraficoSeries> listaGrafico = new List<GraficoSeries>();

            foreach (var itemPtoMedicion in listaPuntoMedicion)
            {
                List<TablaVertical> listaTablaVerticalPtoMed = listaTablaVertical.Where(i => i.Ptomedicodi == itemPtoMedicion.Ptomedicodi).ToList();
                int numRegistros = listaTablaVerticalPtoMed.Count();
                MePtomedicionDTO ptoMedicion = new MePtomedicionDTO();
                ptoMedicion = servicio.GetPtoMedicionById(itemPtoMedicion.Ptomedicodi);
                if (ptoMedicion.PtomediCalculado == "S")
                {
                    listaPuntoMedicionFinal.Add(ptoMedicion);

                    listaGrafico = servicio.ObtenerCaudalPuntosCalculados(tiposeriecodi, tptomedicodi, itemPtoMedicion.Ptomedicodi, anioinicio, aniofin, "TABLA_VERTICAL", "");

                    if (listaGrafico != null)
                    {
                        foreach (var itemGrafico in listaGrafico)
                        {
                            TablaVertical itemTabla = new TablaVertical();
                            itemTabla.Anio = itemGrafico.Anio;
                            itemTabla.Caudal = ptoMedicion.EquiPadrenomb;
                            itemTabla.Ptomedielenomb = ptoMedicion.Ptomedibarranomb;
                            itemTabla.Equinomb = ptoMedicion.Ptomedibarranomb;
                            itemTabla.Emprnomb = ptoMedicion.Emprnomb;
                            itemTabla.Ptomedicodi = ptoMedicion.Ptomedicodi;
                            itemTabla.M1 = itemGrafico.M1;
                            itemTabla.M2 = itemGrafico.M2;
                            itemTabla.M3 = itemGrafico.M3;
                            itemTabla.M4 = itemGrafico.M4;
                            itemTabla.M5 = itemGrafico.M5;
                            itemTabla.M6 = itemGrafico.M6;
                            itemTabla.M7 = itemGrafico.M7;
                            itemTabla.M8 = itemGrafico.M8;
                            itemTabla.M9 = itemGrafico.M9;
                            itemTabla.M10 = itemGrafico.M10;
                            itemTabla.M11 = itemGrafico.M11;
                            itemTabla.M12 = itemGrafico.M12;

                            listaTablaVertical.Add(itemTabla);

                        }
                    }

                }
                else
                {
                    if (numRegistros > 0)
                    {
                        listaPuntoMedicionFinal.Add(ptoMedicion);
                    }
                }

            }


            int result = 1;
            try
            {
                StringBuilder sb = new StringBuilder();
                if (listaPuntoMedicionFinal.Count>0)
                {
                    sb.AppendLine(listaPuntoMedicionFinal[0].EquiPadrenomb);
                    sb.AppendLine("CODIGO,ANIO,ENE,FEB,MAR,ABR,MAY,JUN,JUL,AGO,SET,OCT,NOV,DIC");
                    foreach (var itemPtoMedicion in listaPuntoMedicionFinal)
                    {
                        int CodPtoMed = itemPtoMedicion.Ptomedicodi;
                        List<TablaVertical> listaTablaVerticalPtoMed = listaTablaVertical.Where(i => i.Ptomedicodi == CodPtoMed).ToList();
                        int numRegistros = listaTablaVerticalPtoMed.Count();
                        if (numRegistros > 0)
                        {
                            // Insertar la descripción del punto de medición en la fila debajo de "AÑO"
                            sb.AppendLine(itemPtoMedicion.Ptomedidesc);

                            // Llenar datos dinámicos
                            foreach (var tablaPtoMed in listaTablaVerticalPtoMed)
                            {
                                sb.AppendLine(itemPtoMedicion.Ptomedielenomb + "," + tablaPtoMed.Anio + "," + tablaPtoMed.M1 + "," + tablaPtoMed.M2 + "," + tablaPtoMed.M3 + "," + tablaPtoMed.M4 + "," + tablaPtoMed.M5 + "," + tablaPtoMed.M6 + "," + tablaPtoMed.M7 + "," + tablaPtoMed.M8 + "," + tablaPtoMed.M9 + "," + tablaPtoMed.M10 + "," + tablaPtoMed.M11 + "," + tablaPtoMed.M12);
                            }


                        }
                    }
                }
                
                string strFileName = "";
                if (tipoArchivo=="TXT")
                {
                    strFileName = "ReporteModeloPerseo.txt";
                } else if (tipoArchivo == "CSV")
                {
                    strFileName = "ReporteModeloPerseo.csv";
                }


                Response.Clear();
                Response.ClearHeaders();

                Response.AppendHeader("Content-Length", sb.Length.ToString());
                Response.ContentType = "text/plain";
                Response.AppendHeader("Content-Disposition", "attachment;filename=\""+ strFileName  + "\"");

                Response.Write(sb);
                Response.End();

                return Json((object)new { result = "OK" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                result = -1;
                return Json((object)new { result = "-1" }, JsonRequestBehavior.AllowGet);
            }

        }


    }
}
