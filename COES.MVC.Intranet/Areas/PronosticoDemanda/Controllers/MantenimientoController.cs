using COES.Dominio.DTO.Sic;
using COES.Framework.Base.Tools;
using COES.MVC.Intranet.Areas.PronosticoDemanda.Models;
using COES.MVC.Intranet.Controllers;
using COES.MVC.Intranet.Helper;
using COES.Servicios.Aplicacion.Factory;
using COES.Servicios.Aplicacion.FormatoMedicion;
using COES.Servicios.Aplicacion.PronosticoDemanda;
using COES.Servicios.Aplicacion.PronosticoDemanda.Helper;
using COES.Servicios.Aplicacion.ServicioRPF;
using COES.MVC.Intranet.Helper;
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
    public class MantenimientoController : BaseController
    {
        public MantenimientoController()
        {
            log4net.Config.XmlConfigurator.Configure();
        }
        
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
        
        /// <summary>
        /// Instanciamiento de Log4net
        /// </summary>
        private readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Instancia de la clase de servicio
        /// </summary>
        ComparativoAppServicio servicio = new ComparativoAppServicio();
        PronosticoDemandaAppServicio servicioPronostico = new PronosticoDemandaAppServicio();
        
        /// <summary>
        /// Pagina inicial del módulo
        /// </summary>
        /// <returns></returns>
        public ActionResult Index(string sAnio = "", string sNroSemana = "")
        {
            MantenimientoModel model = new MantenimientoModel();
            if (sAnio.Equals(""))
            {
                model.sAnio = DateTime.Now.Year.ToString();
                model.sNroSemana = EPDate.f_numerosemana(DateTime.Now).ToString();
            }
            else
            {
                model.sAnio = sAnio;
                model.sNroSemana = sNroSemana;
            }
            model.ListaGenSemanas = CargarSemanas(model.sAnio);
            
            return View(model);
        }

        /// <summary>
        /// Lista de Semana por Año
        /// </summary>
        /// <param name="sAnio"></param>
        /// <returns></returns>
        public List<GenericoDTO> CargarSemanas(string sAnio)
        {
            List<GenericoDTO> entitys = new List<GenericoDTO>();
            if (sAnio == "0")
            {
                sAnio = DateTime.Now.Year.ToString();
            }
            DateTime dfecha = new DateTime(Int32.Parse(sAnio), 12, 31);
            int nsemanas = EPDate.TotalSemanasEnAnho(Int32.Parse(sAnio), FirstDayOfWeek.Saturday);

            for (int i = 1; i <= nsemanas; i++)
            {
                GenericoDTO reg = new GenericoDTO();
                reg.Entero1 = i;
                reg.String1 = "Sem" + i + "-" + sAnio;
                entitys.Add(reg);

            }
            return entitys;
        }

        #region Grilla Excel

        /// <summary>
        /// Muestra la grilla excel con los registros de Distancias Electricas
        /// </summary>
        /// <param name="sAnio">Año</param>
        /// <param name="sNroSemana">Nro de Semana en el año</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GrillaExcel(int sTipo, string sAnio, string sNroSemana)
        {
            base.ValidarSesionUsuario();
            GridExcelModel model = new GridExcelModel();
            //Calculando fechas de intervalo
            int iAnio = Convert.ToInt32(sAnio);
            int iNroSemana = Convert.ToInt32(sNroSemana);
            DateTime dFechaInicio = EPDate.f_fechainiciosemana(iAnio, iNroSemana); //2018, 3
            DateTime dFechaFin = dFechaInicio.AddDays(6);
            //Asignamos la lista de fecha/hora que se imprime en la primera columna
            List<string> sDates = new List<string>();
            for (DateTime date = dFechaInicio.AddMinutes(30); date <= dFechaFin.AddDays(1); date = date.AddMinutes(30))
            {
                sDates.Add(date.ToString("dd/MM/yyyy HH:mm"));
            }

            //Listas de fallas y mantenimientos
            List<PrnMedicion48DTO> ListaMantNorte = new List<PrnMedicion48DTO>();
            List<PrnMedicion48DTO> ListaMantCentro = new List<PrnMedicion48DTO>();
            List<PrnMedicion48DTO> ListaMantSur = new List<PrnMedicion48DTO>();
            List<PrnMedicion48DTO> ListaMantSierraCentro = new List<PrnMedicion48DTO>();
            List<PrnMedicion48DTO> ListaFallaNorte = new List<PrnMedicion48DTO>();
            List<PrnMedicion48DTO> ListaFallaCentro = new List<PrnMedicion48DTO>();
            List<PrnMedicion48DTO> ListaFallaSur = new List<PrnMedicion48DTO>();
            List<PrnMedicion48DTO> ListaFallaSierraCentro = new List<PrnMedicion48DTO>();
            //Lista de Mantenimientos
            if (sTipo==1){ 
            Log.Info("Lista Mantenimientos del Área Norte - ListByIdTipoFecha");
            ListaMantNorte = servicioPronostico.ListByIdTipoFecha(ConstantesProdem.PtomedicodiANorte, ConstantesProdem.PrntMantNorte,dFechaInicio, dFechaFin);
            Log.Info("Lista Mantenimientos del Área Centro - ListByIdTipoFecha");
            ListaMantCentro = servicioPronostico.ListByIdTipoFecha(ConstantesProdem.PtomedicodiACentro, ConstantesProdem.PrntMantCentro, dFechaInicio, dFechaFin);
            Log.Info("Lista Mantenimientos del Área Sur - ListByIdTipoFecha");
            ListaMantSur = servicioPronostico.ListByIdTipoFecha(ConstantesProdem.PtomedicodiASur, ConstantesProdem.PrntMantSur, dFechaInicio, dFechaFin);
            Log.Info("Lista Mantenimientos del Área Sierra Centro - ListByIdTipoFecha");
            ListaMantSierraCentro = servicioPronostico.ListByIdTipoFecha(ConstantesProdem.PtomedicodiASierraCentro, ConstantesProdem.PrntMantSierraCentro, dFechaInicio, dFechaFin);
            //Lista de Fallas
            Log.Info("Lista Fallas del Área Norte - ListByIdTipoFecha");
            ListaFallaNorte = servicioPronostico.ListByIdTipoFecha(ConstantesProdem.PtomedicodiANorte, ConstantesProdem.PrntFallaNorte, dFechaInicio, dFechaFin);
            Log.Info("Lista Fallas del Área Centro - ListByIdTipoFecha");
            ListaFallaCentro = servicioPronostico.ListByIdTipoFecha(ConstantesProdem.PtomedicodiACentro, ConstantesProdem.PrntFallaCentro, dFechaInicio, dFechaFin);
            Log.Info("Lista Fallas del Área Sur - ListByIdTipoFecha");
            ListaFallaSur = servicioPronostico.ListByIdTipoFecha(ConstantesProdem.PtomedicodiASur, ConstantesProdem.PrntFallaSur, dFechaInicio, dFechaFin);
            Log.Info("Lista Fallas del Área Sierra Centro - ListByIdTipoFecha");
            ListaFallaSierraCentro = servicioPronostico.ListByIdTipoFecha(ConstantesProdem.PtomedicodiASierraCentro, ConstantesProdem.PrntFallaSierraCentro, dFechaInicio, dFechaFin);
            }

            #region PRODEM2 - PREVISTO
            else {
                Log.Info("Lista Mantenimientos del Área Norte - ListByIdTipoFecha");
                ListaMantNorte = servicioPronostico.ListByIdTipoFecha(ConstantesProdem.PtomedicodiANorte, ConstantesProdem.PrntMantNortePrevisto  , dFechaInicio, dFechaFin);
                Log.Info("Lista Mantenimientos del Área Centro - ListByIdTipoFecha");
                ListaMantCentro = servicioPronostico.ListByIdTipoFecha(ConstantesProdem.PtomedicodiACentro, ConstantesProdem.PrntMantCentroPrevisto, dFechaInicio, dFechaFin);
                Log.Info("Lista Mantenimientos del Área Sur - ListByIdTipoFecha");
                ListaMantSur = servicioPronostico.ListByIdTipoFecha(ConstantesProdem.PtomedicodiASur, ConstantesProdem.PrntMantSurPrevisto, dFechaInicio, dFechaFin);
                Log.Info("Lista Mantenimientos del Área Sierra Centro - ListByIdTipoFecha");
                ListaMantSierraCentro = servicioPronostico.ListByIdTipoFecha(ConstantesProdem.PtomedicodiASierraCentro, ConstantesProdem.PrntMantSierraCentroPrevisto, dFechaInicio, dFechaFin);
                //Lista de Fallas
                Log.Info("Lista Fallas del Área Norte - ListByIdTipoFecha");
                ListaFallaNorte = servicioPronostico.ListByIdTipoFecha(ConstantesProdem.PtomedicodiANorte, ConstantesProdem.PrntFallaNortePrevisto, dFechaInicio, dFechaFin);
                Log.Info("Lista Fallas del Área Norte - ListByIdTipoFecha");
                ListaFallaCentro = servicioPronostico.ListByIdTipoFecha(ConstantesProdem.PtomedicodiACentro, ConstantesProdem.PrntFallaCentroPrevisto, dFechaInicio, dFechaFin);
                Log.Info("Lista Fallas del Área Norte - ListByIdTipoFecha");
                ListaFallaSur = servicioPronostico.ListByIdTipoFecha(ConstantesProdem.PtomedicodiASur, ConstantesProdem.PrntFallaSurPrevisto, dFechaInicio, dFechaFin);
                Log.Info("Lista Fallas del Área Norte - ListByIdTipoFecha");
                ListaFallaSierraCentro = servicioPronostico.ListByIdTipoFecha(ConstantesProdem.PtomedicodiASierraCentro, ConstantesProdem.PrntFallaSierracentroPrevisto, dFechaInicio, dFechaFin);


            }
            #endregion

            #region Armando de contenido
            //Definimos la cabecera como una matriz
            string[] Cabecera1 = { "Area Operativa", "Norte", "", "Centro", "", "Sur", "", "Sierra/Centro", "" }; //Titulos de cada columna
            string[] Cabecera2 = { "Intervalos", "Mantenimiento", "Falla", "Mantenimiento", "Falla", "Mantenimiento", "Falla", "Mantenimiento", "Falla" };
            //Ancho de cada columna
            int[] widths = { 120, 100, 100, 100, 100, 100, 100, 100, 100 };
            object[] columnas = new object[9];


            //Se arma la matriz de datos
            string[][] data = new string[48*7+2][]; //48 Intervalos de 30 min * 7 días + 2 de la cabecera
            data[0] = Cabecera1;
            data[1] = Cabecera2;
            int index = 2;
            for (int iDia = 0; iDia < 7; iDia++)
            {
                string sMantNorte = "0";
                string sFallaNorte = "0";
                string sMantCentro = "0";
                string sFallaCentro = "0";
                string sMantSur = "0";
                string sFallaSur = "0";
                string sMantSierraCentro = "0";
                string sFallaSierraCentro = "0";
                for (int i = 1; i <= 48; i++)
                {
                    int iIndice = ((iDia * 48) + (i - 1));
                    if (ListaMantNorte.Count == 7)
                    {
                        sMantNorte = ListaMantNorte[iDia].GetType().GetProperty("H" + i).GetValue(ListaMantNorte[iDia], null).ToString(); 
                    }
                    if (ListaFallaNorte.Count == 7)
                    {  
                        sFallaNorte = ListaFallaNorte[iDia].GetType().GetProperty("H" + i).GetValue(ListaFallaNorte[iDia], null).ToString();
                    }
                    if (ListaMantCentro.Count == 7)
                    {  
                        sMantCentro = ListaMantCentro[iDia].GetType().GetProperty("H" + i).GetValue(ListaMantCentro[iDia], null).ToString();
                    }
                    if (ListaFallaCentro.Count == 7)
                    {  
                        sFallaCentro = ListaFallaCentro[iDia].GetType().GetProperty("H" + i).GetValue(ListaFallaCentro[iDia], null).ToString();
                    }
                    if (ListaMantSur.Count == 7)
                    {  
                        sMantSur = ListaMantSur[iDia].GetType().GetProperty("H" + i).GetValue(ListaMantSur[iDia], null).ToString();
                    }
                    if (ListaFallaSur.Count == 7)
                    {  
                        sFallaSur = ListaFallaSur[iDia].GetType().GetProperty("H" + i).GetValue(ListaFallaSur[iDia], null).ToString();
                    }
                    if (ListaMantSierraCentro.Count == 7)
                    {  
                        sMantSierraCentro = ListaMantSierraCentro[iDia].GetType().GetProperty("H" + i).GetValue(ListaMantSierraCentro[iDia], null).ToString();
                    }
                    if (ListaFallaSierraCentro.Count == 7)
                    {
                        sFallaSierraCentro = ListaFallaSierraCentro[iDia].GetType().GetProperty("H" + i).GetValue(ListaFallaSierraCentro[iDia], null).ToString();
                    }
                    string[] itemDato = { sDates[iIndice].ToString(), sMantNorte, sFallaNorte, sMantCentro, sFallaCentro, sMantSur, sFallaSur, sMantSierraCentro, sFallaSierraCentro };
                    data[index] = itemDato;
                    index++;
                }
            }
            //Formatos
            columnas[0] = new
            {   //Fecha/hora
                type = GridExcelModel.TipoTexto,
                source = (new List<String>()).ToArray(),
                strict = false,
                correctFormat = true,
                readOnly = true,
            };
            columnas[1] = new
            {   //sMantNorte
                type = GridExcelModel.TipoNumerico,
                source = (new List<String>()).ToArray(),
                strict = false,
                correctFormat = true,
                className = "htRight",
                format = "0,0.00",
                readOnly = false
            };
            columnas[2] = new
            {   //sFallaNorte
                type = GridExcelModel.TipoNumerico,
                source = (new List<String>()).ToArray(),
                strict = false,
                correctFormat = true,
                className = "htRight",
                format = "0,0.00",
                readOnly = false
            };
            columnas[3] = new
            {   //sMantCentro
                type = GridExcelModel.TipoNumerico,
                source = (new List<String>()).ToArray(),
                strict = false,
                correctFormat = true,
                className = "htRight",
                format = "0,0.00",
                readOnly = false
            };
            columnas[4] = new
            {   //sFallaCentro
                type = GridExcelModel.TipoNumerico,
                source = (new List<String>()).ToArray(),
                strict = false,
                correctFormat = true,
                className = "htRight",
                format = "0,0.00",
                readOnly = false
            };
            columnas[5] = new
            {   //sMantSur
                type = GridExcelModel.TipoNumerico,
                source = (new List<String>()).ToArray(),
                strict = false,
                correctFormat = true,
                className = "htRight",
                format = "0,0.00",
                readOnly = false
            };
            columnas[6] = new
            {   //sFallaSur
                type = GridExcelModel.TipoNumerico,
                source = (new List<String>()).ToArray(),
                strict = false,
                correctFormat = true,
                className = "htRight",
                format = "0,0.00",
                readOnly = false
            };
            columnas[7] = new
            {   //sMantSierraCentro
                type = GridExcelModel.TipoNumerico,
                source = (new List<String>()).ToArray(),
                strict = false,
                correctFormat = true,
                className = "htRight",
                format = "0,0.00",
                readOnly = false
            };
            columnas[8] = new
            {   //sFallaSierraCentro
                type = GridExcelModel.TipoNumerico,
                source = (new List<String>()).ToArray(),
                strict = false,
                correctFormat = true,
                className = "htRight",
                format = "0,0.00",
                readOnly = false
            };
            #endregion
            model.Grabar = false; //Permite grabar, en algun momento no deberia permitir grabar por alguna condicion, se cambia a true
            model.Data = data;
            model.Widths = widths;
            model.Columnas = columnas;
            model.NumeroColumnas = 2; //Es el numero de columnas dobles por area operativa
            model.FixedColumnsLeft = 1;
            model.FixedRowsTop = 2;
            return Json(model);
        }

        /// <summary>
        /// Permite grabar los datos del excel
        /// </summary>
        /// <param name="datos">Matriz que contiene los datos de la hoja de calculo</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GrabarGrillaExcel(int sTipo, string sAnio, string sNroSemana, string[][] datos)
        {
            base.ValidarSesionUsuario();
            GridExcelModel model = new GridExcelModel();
            model.sError = "";
            model.sMensaje = "";

            int mantNorte = (sTipo == 1) ? ConstantesProdem.PrntMantNorte : ConstantesProdem.PrntMantNortePrevisto;
            int mantSur = (sTipo == 1) ? ConstantesProdem.PrntMantSur : ConstantesProdem.PrntMantSurPrevisto;
            int mantCentro = (sTipo == 1) ? ConstantesProdem.PrntMantCentro : ConstantesProdem.PrntMantCentroPrevisto;
            int mantSierraCentro = (sTipo == 1) ? ConstantesProdem.PrntMantSierraCentro : ConstantesProdem.PrntMantSierraCentroPrevisto;
            int fallaNorte = (sTipo == 1) ? ConstantesProdem.PrntFallaNorte : ConstantesProdem.PrntFallaNortePrevisto;
            int fallaSur = (sTipo == 1) ? ConstantesProdem.PrntFallaSur : ConstantesProdem.PrntFallaSurPrevisto;
            int fallaCentro = (sTipo == 1) ? ConstantesProdem.PrntFallaCentro : ConstantesProdem.PrntFallaCentroPrevisto;
            int fallaSierraCentro = (sTipo == 1) ? ConstantesProdem.PrntFallaSierraCentro : ConstantesProdem.PrntFallaSierracentroPrevisto;


            if (sAnio.Equals("") && sNroSemana.Equals(""))
            {
                model.sError = "Lo sentimos, debe seleccionar un año y semana valido";
                return Json(model);
            }
            try
            {
                //Calculando fechas de intervalo
                int iAnio = Convert.ToInt32(sAnio);
                int iNroSemana = Convert.ToInt32(sNroSemana);
                DateTime dFechaInicio = EPDate.f_fechainiciosemana(iAnio, iNroSemana); //2018, 3
                DateTime dFechaFin = dFechaInicio.AddDays(7);
                //Lista de Mantenimientos
                List<PrnMedicion48DTO> ListaMantNorte = new List<PrnMedicion48DTO>();
                List<PrnMedicion48DTO> ListaMantCentro = new List<PrnMedicion48DTO>();
                List<PrnMedicion48DTO> ListaMantSur = new List<PrnMedicion48DTO>();
                List<PrnMedicion48DTO> ListaMantSierraCentro = new List<PrnMedicion48DTO>();
                //Lista de Fallas
                List<PrnMedicion48DTO> ListaFallaNorte = new List<PrnMedicion48DTO>();
                List<PrnMedicion48DTO> ListaFallaCentro = new List<PrnMedicion48DTO>();
                List<PrnMedicion48DTO> ListaFallaSur = new List<PrnMedicion48DTO>();
                List<PrnMedicion48DTO> ListaFallaSierraCentro = new List<PrnMedicion48DTO>();
                //Eliminamos la información procesada e inicializamos los objetos proximos a grabar
                for (DateTime date = dFechaInicio; date < dFechaFin; date = date.AddDays(1))
                {
                    //Eliminando Mantenimientos
                    Log.Info("Elimina una Medición de 48 Intervalos - DeletePrnMedicion48");
                    this.servicioPronostico.DeletePrnMedicion48(ConstantesProdem.PtomedicodiANorte, mantNorte, date);
                    Log.Info("Elimina una Medición de 48 Intervalos - DeletePrnMedicion48");
                    this.servicioPronostico.DeletePrnMedicion48(ConstantesProdem.PtomedicodiACentro, mantCentro, date);
                    Log.Info("Elimina una Medición de 48 Intervalos - DeletePrnMedicion48");
                    this.servicioPronostico.DeletePrnMedicion48(ConstantesProdem.PtomedicodiASur, mantSur, date);
                    Log.Info("Elimina una Medición de 48 Intervalos - DeletePrnMedicion48");
                    this.servicioPronostico.DeletePrnMedicion48(ConstantesProdem.PtomedicodiASierraCentro, mantSierraCentro, date);
                    //Eliminando Fallas
                    Log.Info("Elimina una Medición de 48 Intervalos - DeletePrnMedicion48");
                    this.servicioPronostico.DeletePrnMedicion48(ConstantesProdem.PtomedicodiANorte, fallaNorte, date);
                    Log.Info("Elimina una Medición de 48 Intervalos - DeletePrnMedicion48");
                    this.servicioPronostico.DeletePrnMedicion48(ConstantesProdem.PtomedicodiACentro, fallaCentro, date);
                    Log.Info("Elimina una Medición de 48 Intervalos - DeletePrnMedicion48");
                    this.servicioPronostico.DeletePrnMedicion48(ConstantesProdem.PtomedicodiASur, fallaSur, date);
                    Log.Info("Elimina una Medición de 48 Intervalos - DeletePrnMedicion48");
                    this.servicioPronostico.DeletePrnMedicion48(ConstantesProdem.PtomedicodiASierraCentro, fallaSierraCentro, date);
                    //Preparando a los objetos
                    PrnMedicion48DTO dtoMantNorte = new PrnMedicion48DTO();
                    PrnMedicion48DTO dtoMantCentro = new PrnMedicion48DTO();
                    PrnMedicion48DTO dtoMantSur = new PrnMedicion48DTO();
                    PrnMedicion48DTO dtoMantSierraCentro = new PrnMedicion48DTO();
                    PrnMedicion48DTO dtoFallaNorte = new PrnMedicion48DTO();
                    PrnMedicion48DTO dtoFallaCentro = new PrnMedicion48DTO();
                    PrnMedicion48DTO dtoFallaSur = new PrnMedicion48DTO();
                    PrnMedicion48DTO dtoMantFallaSierraCentro = new PrnMedicion48DTO();
                    dtoMantNorte.Medifecha = dtoMantCentro.Medifecha = dtoMantSur.Medifecha = dtoMantSierraCentro.Medifecha = dtoFallaNorte.Medifecha = dtoFallaCentro.Medifecha = dtoFallaSur.Medifecha = dtoMantFallaSierraCentro.Medifecha = date;
                    dtoMantNorte.Prnm48estado = dtoMantCentro.Prnm48estado = dtoMantSur.Prnm48estado = dtoMantSierraCentro.Prnm48estado = dtoFallaNorte.Prnm48estado = dtoFallaCentro.Prnm48estado = dtoFallaSur.Prnm48estado = dtoMantFallaSierraCentro.Prnm48estado = 0;
                    dtoMantNorte.Meditotal = dtoMantCentro.Meditotal = dtoMantSur.Meditotal = dtoMantSierraCentro.Meditotal = dtoFallaNorte.Meditotal = dtoFallaCentro.Meditotal = dtoFallaSur.Meditotal = dtoMantFallaSierraCentro.Meditotal = 0;
                    dtoMantNorte.Prnm48usucreacion = dtoMantNorte.Prnm48usumodificacion = dtoMantCentro.Prnm48usucreacion = dtoMantCentro.Prnm48usumodificacion = dtoMantSur.Prnm48usucreacion = dtoMantSur.Prnm48usumodificacion = dtoMantSierraCentro.Prnm48usucreacion = dtoMantSierraCentro.Prnm48usumodificacion = dtoFallaNorte.Prnm48usucreacion = dtoFallaNorte.Prnm48usumodificacion = dtoFallaCentro.Prnm48usucreacion = dtoFallaCentro.Prnm48usumodificacion = dtoFallaSur.Prnm48usucreacion = dtoFallaSur.Prnm48usumodificacion = dtoMantFallaSierraCentro.Prnm48usucreacion = dtoMantFallaSierraCentro.Prnm48usumodificacion = User.Identity.Name;
                    dtoMantNorte.Prnm48feccreacion = dtoMantNorte.Prnm48fecmodificacion = dtoMantCentro.Prnm48feccreacion = dtoMantCentro.Prnm48fecmodificacion = dtoMantSur.Prnm48feccreacion = dtoMantSur.Prnm48fecmodificacion = dtoMantSierraCentro.Prnm48feccreacion = dtoMantSierraCentro.Prnm48fecmodificacion = dtoFallaNorte.Prnm48feccreacion = dtoFallaNorte.Prnm48fecmodificacion = dtoFallaCentro.Prnm48feccreacion = dtoFallaCentro.Prnm48fecmodificacion = dtoFallaSur.Prnm48feccreacion = dtoFallaSur.Prnm48fecmodificacion = dtoMantFallaSierraCentro.Prnm48feccreacion = dtoMantFallaSierraCentro.Prnm48fecmodificacion = DateTime.Now;
                    //Norte
                    dtoMantNorte.Ptomedicodi = dtoFallaNorte.Ptomedicodi = ConstantesProdem.PtomedicodiANorte;
                    dtoMantNorte.Prnm48tipo = mantNorte;
                    ListaMantNorte.Add(dtoMantNorte);
                    dtoFallaNorte.Prnm48tipo = fallaNorte;
                    ListaFallaNorte.Add(dtoFallaNorte);
                    //Centro
                    dtoMantCentro.Ptomedicodi = dtoFallaCentro.Ptomedicodi = ConstantesProdem.PtomedicodiACentro;
                    dtoMantCentro.Prnm48tipo = mantCentro;
                    ListaMantCentro.Add(dtoMantCentro);
                    dtoFallaCentro.Prnm48tipo = fallaCentro;
                    ListaFallaCentro.Add(dtoFallaCentro);
                    //Sur
                    dtoMantSur.Ptomedicodi = dtoFallaSur.Ptomedicodi = ConstantesProdem.PtomedicodiASur;
                    dtoMantSur.Prnm48tipo = mantSur;
                    ListaMantSur.Add(dtoMantSur);
                    dtoFallaSur.Prnm48tipo = fallaSur;
                    ListaFallaSur.Add(dtoFallaSur);
                    //SierraCentro
                    dtoMantSierraCentro.Ptomedicodi = dtoMantFallaSierraCentro.Ptomedicodi = ConstantesProdem.PtomedicodiASierraCentro;
                    dtoMantSierraCentro.Prnm48tipo = mantSierraCentro;
                    ListaMantSierraCentro.Add(dtoMantSierraCentro);
                    dtoMantFallaSierraCentro.Prnm48tipo = fallaSierraCentro;
                    ListaFallaSierraCentro.Add(dtoMantFallaSierraCentro);
                }
                
                //Recorremos la matriz que se inicia en la fila 2
                for (int iDia = 0; iDia < 7; iDia++)
                {
                    
                    for (int i = 1; i <= 48; i++)
                    {
                        int iIndice = ((iDia * 48) + (i + 1)); //iIndice==2 -> datos[2][0] esta la columna de fecha hora
                        
                        //Norte.Mantenimiento - 1
                        decimal dMantenimientoNorte = UtilProdem.ValidarNumero(datos[iIndice][1].ToString());
                        ListaMantNorte[iDia].GetType().GetProperty("H" + i).SetValue(ListaMantNorte[iDia], dMantenimientoNorte);

                        //Norte.Falla - 2
                        decimal dFallaNorte = UtilProdem.ValidarNumero(datos[iIndice][2].ToString());
                        ListaFallaNorte[iDia].GetType().GetProperty("H" + i).SetValue(ListaFallaNorte[iDia], dFallaNorte);

                        //Centro.Mantenimiento - 3
                        decimal dMantenimientoCentro = UtilProdem.ValidarNumero(datos[iIndice][3].ToString());
                        ListaMantCentro[iDia].GetType().GetProperty("H" + i).SetValue(ListaMantCentro[iDia], dMantenimientoCentro);

                        //Centro.Falla - 4
                        decimal dFallaCentro = UtilProdem.ValidarNumero(datos[iIndice][4].ToString());
                        ListaFallaCentro[iDia].GetType().GetProperty("H" + i).SetValue(ListaFallaCentro[iDia], dFallaCentro);

                        //Sur.Mantenimiento - 5
                        decimal dMantenimientoSur = UtilProdem.ValidarNumero(datos[iIndice][5].ToString());
                        ListaMantSur[iDia].GetType().GetProperty("H" + i).SetValue(ListaMantSur[iDia], dMantenimientoSur);

                        //Sur.Falla - 6
                        decimal dFallaSur = UtilProdem.ValidarNumero(datos[iIndice][6].ToString());
                        ListaFallaSur[iDia].GetType().GetProperty("H" + i).SetValue(ListaFallaSur[iDia], dFallaSur);

                        //SierraCentro.Mantenimiento - 7
                        decimal dMantenimientoSierraCentro = UtilProdem.ValidarNumero(datos[iIndice][7].ToString());
                        ListaMantSierraCentro[iDia].GetType().GetProperty("H" + i).SetValue(ListaMantSierraCentro[iDia], dMantenimientoSierraCentro);

                        //SierraCentro.Falla - 8
                        decimal dFallaSierraCentro = UtilProdem.ValidarNumero(datos[iIndice][8].ToString());
                        ListaFallaSierraCentro[iDia].GetType().GetProperty("H" + i).SetValue(ListaFallaSierraCentro[iDia], dFallaSierraCentro);
                    }
                    //Insertamos el mantenimiento del día
                    Log.Info("Registra una Medición de 48 Intervalos - SavePrnMedicion48");
                    this.servicioPronostico.SavePrnMedicion48(ListaMantNorte[iDia]);
                    Log.Info("Registra una Medición de 48 Intervalos - SavePrnMedicion48");
                    this.servicioPronostico.SavePrnMedicion48(ListaMantCentro[iDia]);
                    Log.Info("Registra una Medición de 48 Intervalos - SavePrnMedicion48");
                    this.servicioPronostico.SavePrnMedicion48(ListaMantSur[iDia]);
                    Log.Info("Registra una Medición de 48 Intervalos - SavePrnMedicion48");
                    this.servicioPronostico.SavePrnMedicion48(ListaMantSierraCentro[iDia]);
                    //Insertamos la falla del día
                    Log.Info("Registra una Medición de 48 Intervalos - SavePrnMedicion48");
                    this.servicioPronostico.SavePrnMedicion48(ListaFallaNorte[iDia]);
                    Log.Info("Registra una Medición de 48 Intervalos - SavePrnMedicion48");
                    this.servicioPronostico.SavePrnMedicion48(ListaFallaCentro[iDia]);
                    Log.Info("Registra una Medición de 48 Intervalos - SavePrnMedicion48");
                    this.servicioPronostico.SavePrnMedicion48(ListaFallaSur[iDia]);
                    Log.Info("Registra una Medición de 48 Intervalos - SavePrnMedicion48");
                    this.servicioPronostico.SavePrnMedicion48(ListaFallaSierraCentro[iDia]);
                }
                model.sMensaje = " para la semana " + sNroSemana.ToString() + " [" + dFechaInicio.ToString("dd/MM/yyyy") + " al " + dFechaFin.ToString("dd/MM/yyyy") + "] - " + sAnio.ToString(); 
                return Json(model);
            }
            catch (Exception e)
            {
                model.sError = e.Message; //"-1"
                return Json(model);
            }
        }
        #endregion
    }
}