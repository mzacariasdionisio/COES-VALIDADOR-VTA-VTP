using COES.Dominio.DTO.Sic;
using COES.Servicios.Aplicacion.General;
using COES.Servicios.Aplicacion.Hidrologia;
using COES.MVC.Publico.Helper;
using COES.MVC.Publico.Areas.Operacion.Helper;
using COES.MVC.Publico.Areas.Operacion.Models;
using System;
using COES.Servicios.Aplicacion.FormatoMedicion;
using COES.Servicios.Aplicacion.Helper;
using System.Collections.Generic;
using COES.Framework.Base.Core;
using COES.Framework.Base.Tools;
using System.Globalization;
using System.Linq;

using System.Web.Mvc;
using OfficeOpenXml.Drawing;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;
using OfficeOpenXml.Style;
using OfficeOpenXml;
using System.Drawing;
using System.IO;
using System.Configuration;
using System.Net;
using System.Text;
using System.Web;
using System.Reflection;
using log4net;
using COES.Servicios.Aplicacion.Factory;


namespace COES.MVC.Publico.Areas.Operacion.Controllers
{
    public class HistoricoHidrologiaController : Controller
    {
        //
        // GET: /Hidrologia/Reporte/
        HidrologiaAppServicio logic = new HidrologiaAppServicio();
        private GeneralAppServicio logicGeneral;
        //inicio modificado
        private static readonly ILog log = log4net.LogManager.GetLogger(typeof(HistoricoHidrologiaController));
        private static readonly string NameController = MethodBase.GetCurrentMethod().DeclaringType.Name;

        protected override void OnException(ExceptionContext filterContext)
        {
            try
            {
                log4net.Config.XmlConfigurator.Configure();
                Exception objErr = filterContext.Exception;
                log.Error("ReporteController", objErr);
            }
            catch (Exception ex)
            {
                log.Fatal("ReporteController", ex);
                throw;
            }
        }
        //fin modificado
        /// <summary>
        /// Almacena los fechas del reporte
        /// </summary>
        public List<DateTime> ListaFechas
        {
            get
            {
                return (Session[ConstantesHidrologia.ListaFechas] != null) ?
                    (List<DateTime>)Session[ConstantesHidrologia.ListaFechas] : new List<DateTime>();
            }
            set { Session[ConstantesHidrologia.ListaFechas] = value; }
        }

        /// <summary>
        /// Almacena los datos del reporte grafico Mensual QN
        /// </summary>
        public HidrologiaModel ModelGraficoMensual
        {
            get
            {
                return (Session[ConstantesHidrologia.ModelGraficoMensual] != null) ?
                    (HidrologiaModel)Session[ConstantesHidrologia.ModelGraficoMensual] : new HidrologiaModel();
            }
            set { Session[ConstantesHidrologia.ModelGraficoMensual] = value; }
        }

        /// <summary>
        /// Almacena los datos del reporte grafico Semanal QN
        /// </summary>
        public HidrologiaModel ModelGraficoSemanal
        {
            get
            {
                return (Session[ConstantesHidrologia.ModelGraficoSemanal] != null) ?
                    (HidrologiaModel)Session[ConstantesHidrologia.ModelGraficoSemanal] : new HidrologiaModel();
            }
            set { Session[ConstantesHidrologia.ModelGraficoSemanal] = value; }
        }

        /// <summary>
        /// Almacena los datos del reporte grafico diario
        /// </summary>
        public HidrologiaModel ModelGraficoDiario
        {
            get
            {
                return (Session[ConstantesHidrologia.ModelGraficoDiario] != null) ?
                    (HidrologiaModel)Session[ConstantesHidrologia.ModelGraficoDiario] : new HidrologiaModel();
            }
            set { Session[ConstantesHidrologia.ModelGraficoDiario] = value; }
        }

        /// <summary>
        /// Almacena los datos del reporte grafico diario
        /// </summary>
        public HidrologiaModel ModelGraficoTR
        {
            get
            {
                return (Session[ConstantesHidrologia.ModelGraficoTR] != null) ?
                    (HidrologiaModel)Session[ConstantesHidrologia.ModelGraficoTR] : new HidrologiaModel();
            }
            set { Session[ConstantesHidrologia.ModelGraficoTR] = value; }
        }


        public HistoricoHidrologiaController()
        {
            logicGeneral = new GeneralAppServicio();
            log4net.Config.XmlConfigurator.Configure();
        }


        #region REPORTES EVOLUCION HISTORICA DE HIDROLOGÍA

        /// <summary>
        /// Index para Reporte de General de Hidrología
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            HidrologiaModel model = new HidrologiaModel();
            model.ListaEmpresas = logicGeneral.ObtenerEmpresasHidro();
            model.ListaPtoMedida = HelperHidrologia.ObtenerListaMedida();
            model.ListaCuenca = this.logic.ListarEquiposXFamilia(ConstantesHidrologia.IdCuenca);
            model.FechaInicio = DateTime.Now.AddDays(-15).ToString(Constantes.FormatoFecha);
            model.FechaFin = DateTime.Now.ToString(Constantes.FormatoFecha);
            model.ListaLectura = this.logic.ListMeLecturas().Where(x => x.Origlectcodi == ConstantesHidrologia.IdOrigenLectura).ToList();
            model.ListaTipoPtoMedicion = this.logic.ListMeTipopuntomedicions(ConstantesHidrologia.IdOrigenHidro.ToString());
            int[] tipoHidro = { 4, 19, 23, 42, 20 };
            model.ListaTipoRecursos = this.logic.ListarFamilia().Where(x => tipoHidro.Contains(x.Famcodi)).ToList();
            if (model.ListaLectura.Count > 0)
                model.IdLectura = model.ListaLectura[0].Lectcodi;
            int[] tipoInfocodis = { 11, 14, 40, 1, 3 };
            var listaUnidades = this.logic.ListSiTipoinformacions().Where(x => tipoInfocodis.Contains(x.Tipoinfocodi)).ToList();
            model.ListaUnidades = new List<SiTipoinformacionDTO>();
            model.ListaUnidades.Add(listaUnidades.Find(x => x.Tipoinfocodi == 11));
            model.ListaUnidades.Add(listaUnidades.Find(x => x.Tipoinfocodi == 14));
            model.ListaUnidades.Add(listaUnidades.Find(x => x.Tipoinfocodi == 40));
            model.ListaUnidades.Add(listaUnidades.Find(x => x.Tipoinfocodi == 1));

            List<int> listaLectCodi = new List<int>();
            List<int> listaLectPeriodo = new List<int>();
            foreach (var reg in model.ListaLectura)
            {
                listaLectCodi.Add(reg.Lectcodi);
                listaLectPeriodo.Add((int)reg.Lectperiodo);
            }
            model.CadenaLectCodi = String.Join(",", listaLectCodi);
            model.CadenaLectPeriodo = String.Join(",", listaLectPeriodo);
            return View(model);
        }

        /// <summary>
        /// Genera Vista Parcial para listado de reporte general
        /// </summary>
        /// <param name="idsEmpresa"></param>
        /// <param name="idsCuenca"></param>
        /// <param name="idsFamilia"></param>
        /// <param name="idLectura"></param>
        /// <param name="idsPtoMedicion"></param>
        /// <param name="fechaInicial"></param>
        /// <param name="fechaFinal"></param>
        /// <param name="nroPagina"></param>
        /// <param name="anho"></param>
        /// <param name="anhoInicial"></param>
        /// <param name="anhoFinal"></param>
        /// <param name="semanaIni"></param>
        /// <param name="semanaFin"></param>
        /// <param name="opcion"></param>
        /// <param name="rbDetalleRpte"></param>
        /// <param name="unidad"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult Lista(string idsEmpresa, string idsCuenca, string idsFamilia, int idLectura, string idsPtoMedicion,
            string fechaInicial, string fechaFinal, int nroPagina,
            string anho, string anhoInicial, string anhoFinal, int semanaIni, int semanaFin,
            int opcion, int rbDetalleRpte, int unidad)
        {
            HidrologiaModel model = new HidrologiaModel();
            DateTime fechaIni = DateTime.MinValue;
            DateTime fechaFin = DateTime.MinValue;
            if (idsPtoMedicion == null)
                idsPtoMedicion = "-1";
            var entity = logic.GetByIdMeLectura(idLectura);

            int numeroMedicion = 0;
            switch (rbDetalleRpte)
            {
                case ConstantesRbDetalleRpte.Diario:
                    numeroMedicion = ConstantesHidrologia.LectNroME1;
                    break;
                case ConstantesRbDetalleRpte.Horas:
                    numeroMedicion = ConstantesHidrologia.LectNroME24;
                    break;
                case ConstantesRbDetalleRpte.SemanalProgramado: //Rpte detallado semanal programado
                case ConstantesRbDetalleRpte.SemanalCronologico: //Rpte detallado semanal cronológico
                case ConstantesRbDetalleRpte.Mensual: // Rpte detallado mensual
                case ConstantesRbDetalleRpte.Anual: //Rpte detallado anual
                    numeroMedicion = entity.Lectnro.Value;
                    break;
            }

            switch (numeroMedicion)
            {
                case ConstantesHidrologia.LectNroME24:
                    switch (entity.Lectperiodo)
                    {
                        case ConstHidrologia.Horas:
                            switch (rbDetalleRpte)
                            {
                                case ConstantesRbDetalleRpte.Horas:
                                case ConstantesRbDetalleRpte.Diario://Rpte detallado diario
                                    fechaIni = DateTime.ParseExact(fechaInicial, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                                    fechaFin = DateTime.ParseExact(fechaFinal, Constantes.FormatoFecha, CultureInfo.InvariantCulture);

                                    List<MeMedicion24DTO> lista24 = this.logic.ListaMed24Hidrologia(idLectura, ConstantesHidrologia.IdOrigenLectura, idsEmpresa,
                                        idsCuenca, idsFamilia, fechaIni, fechaFin, idsPtoMedicion);
                                    this.ListaFechas = lista24.Select(x => x.Medifecha).Distinct().ToList();
                                    if (ListaFechas.Count > 0)
                                    {
                                        fechaIni = ListaFechas[nroPagina - 1];
                                    }
                                    break;
                                case ConstantesRbDetalleRpte.SemanalProgramado: //Rpte detallado semanal programado
                                    int ianho = Int32.Parse(anho);
                                    fechaIni = COES.Base.Tools.Util.GenerarFecha(ianho, semanaIni, ConstantesHidrologia.SemanalProgramado);
                                    fechaFin = COES.Base.Tools.Util.GenerarFecha(ianho, semanaFin + 1, ConstantesHidrologia.SemanalProgramado);
                                    fechaFin = fechaFin.AddDays(-1);
                                    List<MeMedicion24DTO> lista242 = this.logic.ListaMed24Hidrologia(idLectura, ConstantesHidrologia.IdOrigenLectura,
                                        idsEmpresa, idsCuenca, idsFamilia, fechaIni, fechaFin, idsPtoMedicion);
                                    ListaFechas = lista242.Select(x => x.Medifecha).Distinct().ToList();
                                    //fechaIni = ListaFechas.Min();
                                    //fechaFin = ListaFechas.Max();
                                    break;
                                case ConstantesRbDetalleRpte.SemanalCronologico: //Rpte detallado semanal cronológico
                                    int ianho2 = Int32.Parse(anho);

                                    fechaIni = COES.Base.Tools.Util.GenerarFecha(ianho2, semanaIni, ConstantesHidrologia.SemanalCronologico);
                                    fechaFin = COES.Base.Tools.Util.GenerarFecha(ianho2, semanaFin + 1, ConstantesHidrologia.SemanalCronologico);
                                    fechaFin = fechaFin.AddDays(-1);
                                    List<MeMedicion24DTO> lista243 = this.logic.ListaMed24Hidrologia(idLectura, ConstantesHidrologia.IdOrigenLectura,
                                        idsEmpresa, idsCuenca, idsFamilia, fechaIni, fechaFin, idsPtoMedicion);
                                    ListaFechas = lista243.Select(x => x.Medifecha).Distinct().ToList();
                                    break;
                                case ConstantesRbDetalleRpte.Mensual: // Rpte detallado mensual
                                    fechaIni = COES.Base.Tools.Util.FormatFecha(fechaInicial);
                                    fechaFin = COES.Base.Tools.Util.FormatFecha(fechaFinal);
                                    fechaFin = fechaFin.AddMonths(1);
                                    fechaFin = fechaFin.AddDays(-1);
                                    List<MeMedicion24DTO> lista244 = this.logic.ListaMed24Hidrologia(idLectura, ConstantesHidrologia.IdOrigenLectura,
                                        idsEmpresa, idsCuenca, idsFamilia, fechaIni, fechaFin, idsPtoMedicion);
                                    ListaFechas = lista244.Select(x => x.Medifecha).Distinct().ToList();
                                    break;
                                case ConstantesRbDetalleRpte.Anual: //Rpte detallado anual
                                    int ianhoIni = Int32.Parse(anhoInicial);
                                    int ianhoFin = Int32.Parse(anhoFinal);
                                    fechaIni = new DateTime(ianhoIni, 1, 1);
                                    fechaFin = new DateTime(ianhoFin, 12, 31);
                                    break;
                            }
                            break;
                        case 2:
                            break;
                    }
                    break;
                case ConstantesHidrologia.LectNroME1:
                    switch (entity.Lectperiodo)
                    {
                        case ConstHidrologia.Diario: // diario                            
                            switch (rbDetalleRpte)
                            {
                                case ConstantesRbDetalleRpte.Diario: //Rpte detallado diario
                                    fechaIni = DateTime.ParseExact(fechaInicial, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                                    fechaFin = DateTime.ParseExact(fechaFinal, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                                    break;
                                case ConstantesRbDetalleRpte.SemanalProgramado: //Rpte detallado semanal programado
                                    int ianho = Int32.Parse(anho);
                                    fechaIni = COES.Base.Tools.Util.GenerarFecha(ianho, semanaIni, ConstantesHidrologia.SemanalProgramado);
                                    fechaFin = COES.Base.Tools.Util.GenerarFecha(ianho, semanaFin + 1, ConstantesHidrologia.SemanalProgramado);
                                    fechaFin = fechaFin.AddDays(-1);
                                    break;
                                case ConstantesRbDetalleRpte.SemanalCronologico: //Rpte detallado semanal cronológico
                                    int ianho2 = Int32.Parse(anho);
                                    fechaIni = COES.Base.Tools.Util.GenerarFecha(ianho2, semanaIni, ConstantesHidrologia.SemanalCronologico);
                                    fechaFin = COES.Base.Tools.Util.GenerarFecha(ianho2, semanaFin + 1, ConstantesHidrologia.SemanalCronologico);
                                    fechaFin = fechaFin.AddDays(-1);
                                    break;
                                case ConstantesRbDetalleRpte.Mensual: // Rpte detallado mensual
                                    fechaIni = COES.Base.Tools.Util.FormatFecha(fechaInicial);
                                    fechaFin = COES.Base.Tools.Util.FormatFecha(fechaFinal);
                                    fechaFin = fechaFin.AddMonths(1);
                                    fechaFin = fechaFin.AddDays(-1);
                                    break;
                                case ConstantesRbDetalleRpte.Anual: //Rpte detallado anual
                                    int ianhoIni = Int32.Parse(anhoInicial);
                                    int ianhoFin = Int32.Parse(anhoFinal);
                                    fechaIni = new DateTime(ianhoIni, 1, 1);
                                    fechaFin = new DateTime(ianhoFin, 12, 31);
                                    break;
                            }
                            break;
                        case ConstHidrologia.SemanalProg:
                        case ConstHidrologia.Semanal: // Semanal
                            switch (rbDetalleRpte)
                            {
                                case ConstantesRbDetalleRpte.SemanalProgramado: //Rpte detallado semanal programado
                                    int ianho = Int32.Parse(anho);
                                    fechaIni = COES.Base.Tools.Util.GenerarFecha(ianho, semanaIni, ConstantesHidrologia.SemanalProgramado);
                                    fechaFin = COES.Base.Tools.Util.GenerarFecha(ianho, semanaFin + 1, ConstantesHidrologia.SemanalProgramado);
                                    fechaFin = fechaFin.AddDays(-1);
                                    break;
                                case ConstantesRbDetalleRpte.SemanalCronologico://Rpte detallado semanal cronológico
                                    int ianho2 = Int32.Parse(anho);
                                    fechaIni = COES.Base.Tools.Util.GenerarFecha(ianho2, semanaIni, ConstantesHidrologia.SemanalCronologico);
                                    fechaFin = COES.Base.Tools.Util.GenerarFecha(ianho2, semanaFin + 1, ConstantesHidrologia.SemanalCronologico);
                                    fechaFin = fechaFin.AddDays(-1);
                                    break;
                                case ConstantesRbDetalleRpte.Mensual: // Rpte detallado mensual
                                    fechaIni = COES.Base.Tools.Util.FormatFecha(fechaInicial);
                                    fechaFin = COES.Base.Tools.Util.FormatFecha(fechaFinal);
                                    fechaFin = fechaFin.AddMonths(1);
                                    fechaFin = fechaFin.AddDays(-1);
                                    break;
                                case ConstantesRbDetalleRpte.Anual: //Rpte detallado anual
                                    int ianhoIni = Int32.Parse(anhoInicial);
                                    int ianhoFin = Int32.Parse(anhoFinal);
                                    fechaIni = new DateTime(ianhoIni, 1, 1);
                                    fechaFin = new DateTime(ianhoFin, 12, 31);
                                    break;
                            }
                            break;
                        case ConstHidrologia.Mensual: // Mensual
                            switch (rbDetalleRpte)
                            {
                                case ConstantesRbDetalleRpte.Mensual: // Rpte detallado mensual
                                    fechaIni = COES.Base.Tools.Util.FormatFecha(fechaInicial);
                                    fechaFin = COES.Base.Tools.Util.FormatFecha(fechaFinal);
                                    fechaFin = fechaFin.AddMonths(1);
                                    fechaFin = fechaFin.AddDays(-1);
                                    break;
                                case ConstantesRbDetalleRpte.Anual: //Rpte detallado anual
                                    int ianhoIni = Int32.Parse(anhoInicial);
                                    int ianhoFin = Int32.Parse(anhoFinal);
                                    fechaIni = new DateTime(ianhoIni, 1, 1);
                                    fechaFin = new DateTime(ianhoFin, 12, 31);
                                    break;
                            }
                            break;
                    }
                    break;

            }
            string resultado = ObtenerReporteHidrologia(idsEmpresa, idsCuenca, idsFamilia, idsPtoMedicion, fechaIni, fechaFin, idLectura, opcion, rbDetalleRpte, unidad, numeroMedicion);
            model.Resultado = resultado;
            return PartialView(model);
        }

        /// <summary>
        /// Lista los tipos de informacion de acuerdo a la unidad seleccionada
        /// </summary>
        /// </summary>
        /// <param name="sUnidad"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult ListarPuntosMedicion(int iUnidad)
        {
            HidrologiaModel model = new HidrologiaModel();
            var lista = this.logic.ListMeTipopuntomedicions(ConstantesHidrologia.IdOrigenHidro.ToString());
            model.ListaTipoPtoMedicion = lista.Where(x => x.Tipoinfocodi == iUnidad).ToList();
            return PartialView(model);
        }

        /// <summary>
        /// Genera grafico para reporte general 
        /// </summary>
        /// <param name="idsEmpresas"></param>
        /// <param name="idsCuencas"></param>
        /// <param name="idsFamilia"></param>
        /// <param name="fechaInicial"></param>
        /// <param name="fechaFinal"></param>
        /// <param name="idLectura"></param>
        /// <param name="nroPagina"></param>
        /// <param name="anho"></param>
        /// <param name="semanaIni"></param>
        /// <param name="semanaFin"></param>
        /// <param name="anhoInicial"></param>
        /// <param name="anhoFinal"></param>
        /// <param name="idsPtoMedicion"></param>
        /// <param name="rbDetalleRpte"></param>
        /// <param name="unidad"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GraficoReporte(string idsEmpresas, string idsCuencas, string idsFamilia, string fechaInicial, string fechaFinal, int idLectura, int nroPagina,
                                        string anho, int semanaIni, int semanaFin, string anhoInicial, string anhoFinal, string idsPtoMedicion, int rbDetalleRpte, int unidad)
        {
            HidrologiaModel model = GeneraModelM24(idsEmpresas, idsCuencas, idsFamilia, fechaInicial, fechaFinal, idLectura, nroPagina,
                                         anho, semanaIni, semanaFin, anhoInicial, anhoFinal, idsPtoMedicion, rbDetalleRpte, unidad);
            var jsonResult = Json(model);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        /// <summary>
        ///  Genera el model para generar grafico y reportes en excel
        /// </summary>
        /// <param name="idsEmpresas"></param>
        /// <param name="idsCuencas"></param>
        /// <param name="idsFamilia"></param>
        /// <param name="fechaInicial"></param>
        /// <param name="fechaFinal"></param>
        /// <param name="idLectura"></param>
        /// <param name="nroPagina"></param>
        /// <param name="anho"></param>
        /// <param name="semanaIni"></param>
        /// <param name="semanaFin"></param>
        /// <param name="anhoInicial"></param>
        /// <param name="anhoFinal"></param>
        /// <param name="idsPtoMedicion"></param>
        /// <param name="rbDetalleRpte"></param>
        /// <param name="unidad"></param>
        /// <returns></returns>
        private HidrologiaModel GeneraModelM24(string idsEmpresas, string idsCuencas, string idsFamilia, string fechaInicial, string fechaFinal, int idLectura, int nroPagina,
                                        string anho, int semanaIni, int semanaFin, string anhoInicial, string anhoFinal, string idsPtoMedicion, int rbDetalleRpte, int unidad)
        {
            HidrologiaModel model = new HidrologiaModel();
            DateTime fechaIni = DateTime.MinValue;
            DateTime fechaFin = DateTime.MinValue;
            var entity = logic.GetByIdMeLectura(idLectura);
            //model.Grafico.titleText = "Titulo Reporte";

            switch (entity.Lectnro)
            {
                case ConstantesHidrologia.LectNroME24:
                    switch (entity.Lectperiodo)
                    {
                        case ConstHidrologia.Horas: //Diario
                            switch (rbDetalleRpte)
                            {
                                case ConstantesRbDetalleRpte.Horas: //Detallado Horas
                                    fechaIni = DateTime.ParseExact(fechaInicial, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                                    fechaFin = DateTime.ParseExact(fechaFinal, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                                    model = GraficoMed24(idLectura, idsEmpresas, idsCuencas, idsFamilia, idsPtoMedicion, fechaIni, fechaFin, nroPagina, rbDetalleRpte, unidad);
                                    model.TipoReporte = 1;
                                    break;
                                case ConstantesRbDetalleRpte.Diario: //Datallado Dia
                                    fechaIni = DateTime.ParseExact(fechaInicial, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                                    fechaFin = DateTime.ParseExact(fechaFinal, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                                    model = GraficoMed24(idLectura, idsEmpresas, idsCuencas, idsFamilia, idsPtoMedicion, fechaIni, fechaFin, nroPagina, rbDetalleRpte, unidad);
                                    model.TipoReporte = 1;
                                    break;
                                case ConstantesRbDetalleRpte.SemanalProgramado: // Detallado Semanal programado
                                    int ianho = Int32.Parse(anho);
                                    fechaIni = COES.Base.Tools.Util.GenerarFecha(ianho, semanaIni, ConstantesHidrologia.SemanalProgramado);
                                    fechaFin = COES.Base.Tools.Util.GenerarFecha(ianho, semanaFin + 1, ConstantesHidrologia.SemanalProgramado);
                                    fechaFin = fechaFin.AddDays(-1);
                                    model = GraficoMed24(idLectura, idsEmpresas, idsCuencas, idsFamilia, idsPtoMedicion, fechaIni, fechaFin, nroPagina, rbDetalleRpte, unidad);
                                    model.TipoReporte = 1;
                                    break;
                                case ConstantesRbDetalleRpte.SemanalCronologico: //Detalle semanal cronologico
                                    int ianho2 = Int32.Parse(anho);
                                    fechaIni = COES.Base.Tools.Util.GenerarFecha(ianho2, semanaIni, ConstantesHidrologia.SemanalCronologico);
                                    fechaFin = COES.Base.Tools.Util.GenerarFecha(ianho2, semanaFin + 1, ConstantesHidrologia.SemanalCronologico);
                                    fechaFin = fechaFin.AddDays(-1);
                                    model = GraficoMed24(idLectura, idsEmpresas, idsCuencas, idsFamilia, idsPtoMedicion, fechaIni, fechaFin, nroPagina, rbDetalleRpte, unidad);
                                    model.TipoReporte = 1;
                                    break;
                                case ConstantesRbDetalleRpte.Mensual: //Detallado Mes
                                    fechaIni = COES.Base.Tools.Util.FormatFecha(fechaInicial);
                                    fechaFin = COES.Base.Tools.Util.FormatFecha(fechaFinal);
                                    fechaFin = fechaFin.AddMonths(1);
                                    fechaFin = fechaFin.AddDays(-1);
                                    model = GraficoMed24(idLectura, idsEmpresas, idsCuencas, idsFamilia, idsPtoMedicion, fechaIni, fechaFin, nroPagina, rbDetalleRpte, unidad);
                                    model.TipoReporte = 1;
                                    break;
                                case ConstantesRbDetalleRpte.Anual: //Detallado Año
                                    int ianhoIni = Int32.Parse(anhoInicial);
                                    int ianhoFin = Int32.Parse(anhoFinal);
                                    fechaIni = new DateTime(ianhoIni, 1, 1);
                                    fechaFin = new DateTime(ianhoFin, 12, 31);
                                    model = GraficoMed24(idLectura, idsEmpresas, idsCuencas, idsFamilia, idsPtoMedicion, fechaIni, fechaFin, nroPagina, rbDetalleRpte, unidad);
                                    model.TipoReporte = 1;
                                    break;
                            }
                            break;
                    }
                    break;
                case ConstantesHidrologia.LectNroME1:
                    switch (entity.Lectperiodo)
                    {
                        case ConstHidrologia.Diario: //Diario
                            switch (rbDetalleRpte)
                            {
                                case ConstantesRbDetalleRpte.Diario: // Diario
                                    fechaIni = DateTime.ParseExact(fechaInicial, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                                    fechaFin = DateTime.ParseExact(fechaFinal, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                                    model = GraficoMed1(idLectura, idsEmpresas, idsCuencas, idsFamilia, fechaIni, fechaFin, idsPtoMedicion, rbDetalleRpte, unidad);
                                    model.TipoReporte = 2;
                                    break;
                                case ConstantesRbDetalleRpte.SemanalProgramado: // Semanal programado
                                    int ianho = Int32.Parse(anho);
                                    fechaIni = COES.Base.Tools.Util.GenerarFecha(ianho, semanaIni, ConstantesHidrologia.SemanalProgramado);
                                    fechaFin = COES.Base.Tools.Util.GenerarFecha(ianho, semanaFin + 1, ConstantesHidrologia.SemanalProgramado);
                                    fechaFin = fechaFin.AddDays(-1);
                                    model = GraficoMed1(idLectura, idsEmpresas, idsCuencas, idsFamilia, fechaIni, fechaFin, idsPtoMedicion, rbDetalleRpte, unidad);
                                    model.TipoReporte = 2;
                                    break;
                                case ConstantesRbDetalleRpte.SemanalCronologico: //Semanal Cronológico
                                    int ianho2 = Int32.Parse(anho);
                                    fechaIni = COES.Base.Tools.Util.GenerarFecha(ianho2, semanaIni, ConstantesHidrologia.SemanalCronologico);
                                    fechaFin = COES.Base.Tools.Util.GenerarFecha(ianho2, semanaFin + 1, ConstantesHidrologia.SemanalCronologico);
                                    fechaFin = fechaFin.AddDays(-1);
                                    model = GraficoMed1(idLectura, idsEmpresas, idsCuencas, idsFamilia, fechaIni, fechaFin, idsPtoMedicion, rbDetalleRpte, unidad);
                                    model.TipoReporte = 2;
                                    break;
                                case ConstantesRbDetalleRpte.Mensual: //Mensual
                                    fechaIni = COES.Base.Tools.Util.FormatFecha(fechaInicial);
                                    fechaFin = COES.Base.Tools.Util.FormatFecha(fechaFinal);
                                    fechaFin = fechaFin.AddMonths(1);
                                    fechaFin = fechaFin.AddDays(-1);
                                    //model = GraficoMensual(idLectura, idsEmpresas, fechaIni, fechaFin, idsPtoMedicion);
                                    model = GraficoMed1(idLectura, idsEmpresas, idsCuencas, idsFamilia, fechaIni, fechaFin, idsPtoMedicion, rbDetalleRpte, unidad);
                                    model.TipoReporte = 2;
                                    break;
                                case ConstantesRbDetalleRpte.Anual: //Anual
                                    int ianhoIni = Int32.Parse(anhoInicial);
                                    int ianhoFin = Int32.Parse(anhoFinal);
                                    fechaIni = new DateTime(ianhoIni, 1, 1);
                                    fechaFin = new DateTime(ianhoFin, 12, 31);
                                    model = GraficoMed1(idLectura, idsEmpresas, idsCuencas, idsFamilia, fechaIni, fechaFin, idsPtoMedicion, rbDetalleRpte, unidad);
                                    model.TipoReporte = 2;
                                    break;
                            }
                            break;
                        case ConstHidrologia.SemanalProg://Semanal
                        case ConstHidrologia.Semanal:
                            switch (rbDetalleRpte)
                            {
                                case ConstantesRbDetalleRpte.SemanalProgramado: // Semanal programado
                                    int ianho = Int32.Parse(anho);
                                    fechaIni = COES.Base.Tools.Util.GenerarFecha(ianho, semanaIni, ConstantesHidrologia.SemanalProgramado);
                                    fechaFin = COES.Base.Tools.Util.GenerarFecha(ianho, semanaFin + 1, ConstantesHidrologia.SemanalProgramado);
                                    fechaFin = fechaFin.AddDays(-1);
                                    model = GraficoMed1(idLectura, idsEmpresas, idsCuencas, idsFamilia, fechaIni, fechaFin, idsPtoMedicion, rbDetalleRpte, unidad);
                                    //model = GraficoSemanal(idLectura, idsEmpresas, fechaIni, fechaFin, idsPtoMedicion, unidad);
                                    model.TipoReporte = 2;
                                    break;
                                case ConstantesRbDetalleRpte.SemanalCronologico: //Semanal cronológico
                                    int ianho2 = Int32.Parse(anho);
                                    fechaIni = COES.Base.Tools.Util.GenerarFecha(ianho2, semanaIni, ConstantesHidrologia.SemanalCronologico);
                                    fechaFin = COES.Base.Tools.Util.GenerarFecha(ianho2, semanaFin + 1, ConstantesHidrologia.SemanalCronologico);
                                    fechaFin = fechaFin.AddDays(-1);
                                    model = GraficoMed1(idLectura, idsEmpresas, idsCuencas, idsFamilia, fechaIni, fechaFin, idsPtoMedicion, rbDetalleRpte, unidad);
                                    //model = GraficoSemanal(idLectura, idsEmpresas, fechaIni, fechaFin, idsPtoMedicion, unidad);
                                    model.TipoReporte = 2;
                                    break;
                                case ConstantesRbDetalleRpte.Mensual: //mensual
                                    fechaIni = COES.Base.Tools.Util.FormatFecha(fechaInicial);
                                    fechaFin = COES.Base.Tools.Util.FormatFecha(fechaFinal);
                                    fechaFin = fechaFin.AddMonths(1);
                                    fechaFin = fechaFin.AddDays(-1);
                                    //model = GraficoMensual(idLectura, idsEmpresas, fechaIni, fechaFin, idsPtoMedicion);
                                    model = GraficoMed1(idLectura, idsEmpresas, idsCuencas, idsFamilia, fechaIni, fechaFin, idsPtoMedicion, rbDetalleRpte, unidad);
                                    model.TipoReporte = 2;
                                    break;
                                case ConstantesRbDetalleRpte.Anual: // anual
                                    int ianhoIni = Int32.Parse(anhoInicial);
                                    int ianhoFin = Int32.Parse(anhoFinal);
                                    fechaIni = new DateTime(ianhoIni, 1, 1);
                                    fechaFin = new DateTime(ianhoFin, 12, 31);
                                    model = GraficoMed1(idLectura, idsEmpresas, idsCuencas, idsFamilia, fechaIni, fechaFin, idsPtoMedicion, rbDetalleRpte, unidad);
                                    model.TipoReporte = 2;
                                    break;
                            }
                            break;
                        case ConstHidrologia.Mensual://Mes
                            switch (rbDetalleRpte)
                            {
                                case ConstantesRbDetalleRpte.Mensual: //mensual
                                    fechaIni = COES.Base.Tools.Util.FormatFecha(fechaInicial);
                                    fechaFin = COES.Base.Tools.Util.FormatFecha(fechaFinal);
                                    fechaFin = fechaFin.AddMonths(1);
                                    fechaFin = fechaFin.AddDays(-1);
                                    //model = GraficoMensual(idLectura, idsEmpresas, fechaIni, fechaFin, idsPtoMedicion);
                                    model = GraficoMed1(idLectura, idsEmpresas, idsCuencas, idsFamilia, fechaIni, fechaFin, idsPtoMedicion, rbDetalleRpte, unidad);
                                    model.TipoReporte = 2;
                                    break;
                                case ConstantesRbDetalleRpte.Anual://Anual
                                    int ianhoIni = Int32.Parse(anhoInicial);
                                    int ianhoFin = Int32.Parse(anhoFinal);
                                    fechaIni = new DateTime(ianhoIni, 1, 1);
                                    fechaFin = new DateTime(ianhoFin, 12, 31);
                                    model = GraficoMed1(idLectura, idsEmpresas, idsCuencas, idsFamilia, fechaIni, fechaFin, idsPtoMedicion, rbDetalleRpte, unidad);
                                    model.TipoReporte = 2;
                                    break;
                            }
                            break;
                    }
                    break;
            }


            return model;
        }

        /// <summary>
        /// Permite mostrar el paginado de la consulta
        /// </summary>
        /// <param name="modelo"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult Paginado(string idsEmpresa, string idsCuenca, string idsFamilia, int idLectura, string idsPtoMedicion, string fechaInicial, string fechaFinal)
        {
            HidrologiaModel model = new HidrologiaModel();
            model.IndicadorPagina = false;
            if (idsPtoMedicion == null)
                idsPtoMedicion = "-1";
            //var formato = logic.GetByIdMeFormato(idTipoInformacion);
            //formato.ListaHoja = logic.GetByCriteriaMeFormatohojas(idTipoInformacion);
            DateTime fechaInicio = DateTime.Now;
            DateTime fechaFin = DateTime.Now;

            if (fechaInicial != null)
            {
                fechaInicio = DateTime.ParseExact(fechaInicial, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            }
            if (fechaFinal != null)
            {
                fechaFin = DateTime.ParseExact(fechaFinal, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            }
            //fechaFin = fechaFin.AddDays(1);

            int nroRegistros = this.logic.ObtenerNroFilasMed1Hidrologia(idLectura, ConstantesHidrologia.IdOrigenLectura, idsEmpresa, idsCuenca, idsFamilia, fechaInicio, fechaFin, idsPtoMedicion);

            if (nroRegistros > 0)
            {
                model.NroPaginas = nroRegistros;
                model.NroMostrar = Constantes.NroPageShow;
                model.IndicadorPagina = true;
            }

            return PartialView(model);
        }

        /// <summary>
        /// Funcion que permite graficar el reporte semana-día en pantalla 
        /// </summary> 
        public HidrologiaModel GraficoMed1(int idLectura, string idsEmpresas, string idsCuenca, string idsFamilia, DateTime fechaIni, DateTime fechaFin, string idsPtoMedicion, int rbDetalleRpte, int unidad)
        {
            HidrologiaModel model = new HidrologiaModel();
            GraficoWeb grafico = new GraficoWeb();
            model.Grafico = grafico;
            List<MeMedicion1DTO> listaM1 = new List<MeMedicion1DTO>();
            if (rbDetalleRpte == ConstantesRbDetalleRpte.Diario) //Diario
            {
                listaM1 = this.logic.ListaMed1Hidrologia(idLectura, ConstantesHidrologia.IdOrigenLectura, idsEmpresas, idsCuenca, idsFamilia, fechaIni, fechaFin, idsPtoMedicion);
                ListaFechas = listaM1.Select(x => x.Medifecha).Distinct().ToList();
            }
            else
            {
                if (rbDetalleRpte != ConstantesRbDetalleRpte.Anual)
                {
                    if (unidad == ConstHidrologia.Caudal) // Caudal: lista de valores promedio 
                        listaM1 = this.logic.GenerarListaDetalladaMed1Promedio(idLectura, ConstantesHidrologia.IdOrigenLectura, idsEmpresas, idsCuenca, idsFamilia, fechaIni,
                                                                          fechaFin, idsPtoMedicion, rbDetalleRpte);
                    else // lista de valores del ultimo dia de cada periodo
                        listaM1 = this.logic.GenerarListaDetalladaMed1Hmax(idLectura, ConstantesHidrologia.IdOrigenLectura, idsEmpresas, idsCuenca, idsFamilia, fechaIni,
                                                                          fechaFin, idsPtoMedicion, rbDetalleRpte);
                }
                else // anual
                {
                    if (unidad == ConstHidrologia.Caudal) // Caudal: lista de valores promedio 
                        listaM1 = this.logic.GenerarListaMed1PromAnual(idLectura, ConstantesHidrologia.IdOrigenLectura, idsEmpresas, idsCuenca, idsFamilia, fechaIni, fechaFin, idsPtoMedicion);
                    else // lista de valores del ultimo dia de cada periodo
                        listaM1 = this.logic.GenerarListaMed1AnualHmaximo(idLectura, ConstantesHidrologia.IdOrigenLectura, idsEmpresas, idsCuenca, idsFamilia, fechaIni, fechaFin, idsPtoMedicion);
                }
                ListaFechas = listaM1.Select(x => x.Medifecha).Distinct().ToList();

            }
            if (listaM1.Count > 0)
            {
                model.ListaMedicion1 = listaM1;
                model.Grafico.XAxisCategories = new List<string>(); //
                model.Grafico.SeriesName = new List<string>();
                if (ListaFechas.Count > 0)
                {
                    fechaIni = listaM1.Min(x => x.Medifecha);
                    fechaFin = listaM1.Max(x => x.Medifecha);
                }
                model.FechaInicio = fechaIni.ToString(Constantes.FormatoFecha);
                model.FechaFin = fechaFin.ToString(Constantes.FormatoFecha);
                model.RbTipoReporte = rbDetalleRpte;

                model.Grafico.XAxisTitle = "AÑO :" + fechaIni.Year.ToString();
                model.Grafico.YaxixTitle = " (" + listaM1[0].Tipoinfoabrev + ")";

                // obtenemos el nombre del tipo de informacion para el titulo el reporte
                var regTitulo = this.logic.GetByIdMeLectura(idLectura);
                model.Grafico.TitleText = "REPORTE GRÁFICO " + regTitulo.Lectnomb;
                model.SheetName = "GRAFICO";

                // Obtener Lista de dias/semanas/meses ordenados para la categoria del grafico

                foreach (var reg in ListaFechas)
                {
                    switch (rbDetalleRpte)
                    {
                        case ConstantesRbDetalleRpte.Diario: //diario
                            string dia = reg.ToString(Constantes.FormatoFecha); //reg.Day + "-" + COES.Base.Tools.Util.ObtenerNombreMesAbrev(reg.Month) + "-" + reg.Year;
                            model.Grafico.XAxisCategories.Add(dia);
                            break;
                        case ConstantesRbDetalleRpte.SemanalProgramado: //semanal programado
                            string semana = "Sem " + COES.Base.Tools.Util.GenerarNroSemana(reg, FirstDayOfWeek.Saturday) + "-" + reg.Year;
                            model.Grafico.XAxisCategories.Add(semana);
                            break;
                        case ConstantesRbDetalleRpte.SemanalCronologico: //semanal cronológico
                            string semanaC = "Sem " + COES.Base.Tools.Util.GenerarNroSemana(reg, FirstDayOfWeek.Sunday) + "-" + reg.Year;
                            model.Grafico.XAxisCategories.Add(semanaC);
                            break;
                        case ConstantesRbDetalleRpte.Mensual://mensual
                            string mes = COES.Base.Tools.Util.ObtenerNombreMesAbrev(reg.Month) + "-" + reg.Year;
                            model.Grafico.XAxisCategories.Add(mes);
                            break;
                        case ConstantesRbDetalleRpte.Anual://anual
                            string yyyy = reg.Year.ToString();
                            model.Grafico.XAxisCategories.Add(yyyy);
                            break;
                    }
                }

                // Obtener Lista de nombres de las series del grafico.
                var listaGrupoMedicion = listaM1.GroupBy(x => x.Ptomedicodi).Select(group => group.First()).ToList();
                foreach (var reg in listaGrupoMedicion)
                {
                    //string nombreSerie = reg.Ptomedibarranomb + " " + reg.Tipoptomedinomb + " " + reg.Tipoinfoabrev;
                    string nombreSerie = string.Empty;
                    if (reg.Famcodi == ConstantesHidrologia.EstacionHidrologica)//si es Estacion Hidrologica
                        nombreSerie = reg.Ptomedibarranomb;
                    else
                        nombreSerie = (reg.Equinomb.Length > 20) ? reg.Equinomb.Substring(0, 19) : reg.Equinomb;
                    if (reg.Tipoptomedinomb != null)
                    {
                        var descrip = (reg.Tipoptomedinomb.Length > 15) ? reg.Tipoptomedinomb.Substring(0, 14) : reg.Tipoptomedinomb;
                        nombreSerie += " - " + descrip;
                        model.Grafico.SeriesName.Add(nombreSerie);
                    }
                }
                // Obtener lista de valores para las series del grafico
                model.Grafico.SeriesData = new decimal?[listaGrupoMedicion.Count()][];
                for (var i = 0; i < listaGrupoMedicion.Count(); i++)
                {
                    model.Grafico.SeriesData[i] = new decimal?[ListaFechas.Count];
                    for (var j = 0; j < ListaFechas.Count; j++)
                    {
                        DateTime fecha = ListaFechas[j];
                        decimal? valor = null;
                        MeMedicion1DTO entity = new MeMedicion1DTO();

                        //if (rbDetalleRpte == 1) // diario
                        //{
                        entity = listaM1.Find(x => x.Ptomedicodi == listaGrupoMedicion[i].Ptomedicodi && x.Medifecha == fecha);
                        //}                    
                        //if (rbDetalleRpte == 2 || rbDetalleRpte == 3) // semanal programado, cronológico
                        //    entity = listaM1.Find(x => x.Ptomedicodi == listaGrupoMedicion[i].Ptomedicodi && x.Medifecha == fecha);
                        //if (rbDetalleRpte == 4) // mensual
                        //    entity = listaM1.Find(x => x.Ptomedicodi == listaGrupoMedicion[i].Ptomedicodi && x.Medifecha == fecha);
                        if (entity != null)
                        {
                            valor = entity.H1;
                            model.Grafico.SeriesData[i][j] = valor;
                        }
                    }
                }
                this.ModelGraficoDiario = model;
            }// ende del if
            return model;
        }

        /// <summary>
        /// Funcion que permite graficar el reporte con datos diarios por horas, dias
        /// </summary>      
        public HidrologiaModel GraficoMed24(int idLectura, string idsEmpresas, string idsCuencas, string idsFamilia, string idsPtoMedicion, DateTime fechaIni, DateTime fechaFin, int nroPagina, int rbDetalleRpte, int unidad)
        {
            HidrologiaModel model = new HidrologiaModel();
            GraficoWeb grafico = new GraficoWeb();
            DateTime fechaIniAux = fechaIni;
            DateTime fechaFinAux = fechaFin;
            model.Grafico = grafico;
            model.Grafico.Series = new List<RegistroSerie>();
            List<MeMedicion24DTO> listaM24 = new List<MeMedicion24DTO>();
            listaM24 = this.logic.ListaMed24Hidrologia(idLectura, ConstantesHidrologia.IdOrigenLectura, idsEmpresas, idsCuencas, idsFamilia, fechaIni, fechaFin, idsPtoMedicion);

            if (listaM24.Count > 0)
            {
                var ListaFechas = listaM24.Select(x => x.Medifecha).Distinct().ToList();
                if (ListaFechas.Count > 0)
                {
                    if (rbDetalleRpte == ConstantesRbDetalleRpte.Horas)
                        fechaIni = ListaFechas[nroPagina - 1];
                    else
                        fechaIni = ListaFechas.Min();
                    fechaFin = ListaFechas.Max();
                }


                if (rbDetalleRpte == ConstantesRbDetalleRpte.Horas) // Detallado Horas
                {
                    //listaM24 = this.logic.ListaMed24Hidrologia(idLectura, ConstantesHidrologia.IdOrigenLectura, idsEmpresas, idsCuencas, idsFamilia, fechaIni, fechaIni, idsPtoMedicion);
                }
                else  // 
                {
                    if (rbDetalleRpte != ConstantesRbDetalleRpte.Anual)
                    {
                        if (unidad == ConstHidrologia.Caudal)//Caudal: lista los valores promedio de cada periodo
                            listaM24 = this.logic.GenerarListaDetalladaMed24Promedio(idLectura, ConstantesHidrologia.IdOrigenLectura, idsEmpresas, idsCuencas, idsFamilia, fechaIniAux,
                                                                              fechaFinAux, idsPtoMedicion, rbDetalleRpte);
                        else // lista los valores del ultimo dia de cada periodo
                            listaM24 = this.logic.GenerarListaMed24Hmaximo(idLectura, ConstantesHidrologia.IdOrigenLectura, idsEmpresas, idsCuencas, idsFamilia, fechaIniAux,
                                fechaFinAux, idsPtoMedicion, rbDetalleRpte);
                    }
                    else // anual
                    {
                        if (unidad == ConstHidrologia.Caudal)//Caudal lista los valores promedio de cada periodo
                            listaM24 = this.logic.GenerarListaMed24PromAnual(idLectura, ConstantesHidrologia.IdOrigenLectura, idsEmpresas, idsCuencas, idsFamilia, fechaIniAux,
                                                                            fechaFinAux, idsPtoMedicion);
                        else // lista los valores del ultimo dia de cada periodo
                            listaM24 = this.logic.GenerarListaMed24AnualHmaximo(idLectura, ConstantesHidrologia.IdOrigenLectura, idsEmpresas, idsCuencas, idsFamilia, fechaIniAux,
                                                                            fechaFinAux, idsPtoMedicion);
                    }
                }

                var lstFechasAux = listaM24.Select(x => x.Medifecha).Distinct().ToList();// para los rangos de fechas de reporte semanal, mensual , anual           
                model.FechaInicio = fechaIni.ToString(Constantes.FormatoFecha);
                model.FechaFin = fechaFin.ToString(Constantes.FormatoFecha);
                model.RbTipoReporte = rbDetalleRpte;

                if (rbDetalleRpte == ConstantesRbDetalleRpte.Horas) // Detallado Horas
                {
                    model.Grafico.XAxisTitle = "Dia:" + fechaIni.ToString(Constantes.FormatoFecha);
                }
                else  // Detallado Diario, semanales, mes, año
                {
                    model.Grafico.XAxisTitle = "Año:" + fechaIni.Year;
                }

                // obtenemos el nombre del tipo de informacion para el titulo el reporte
                var regTitulo = this.logic.GetByIdMeLectura(idLectura);
                model.Grafico.TitleText = "REPORTE GRÁFICO " + regTitulo.Lectnomb;
                model.SheetName = "GRAFICO";


                model.Grafico.YaxixTitle = "(" + listaM24[0].Tipoinfoabrev + ")";
                model.Grafico.XAxisCategories = new List<string>();

                int totalIntervalos = 0;

                // Obtener Lista de intervalos categoria del grafico
                if (rbDetalleRpte == ConstantesRbDetalleRpte.Horas) // hora
                {
                    totalIntervalos = 24;
                    for (var j = 0; j <= (totalIntervalos - 1); j++)
                    {
                        string hora = ("0" + j.ToString()).Substring(("0" + j.ToString()).Length - 2, 2) + ":00";
                        model.Grafico.XAxisCategories.Add(hora);

                    }
                }
                if (rbDetalleRpte == ConstantesRbDetalleRpte.Diario) // dia
                {
                    totalIntervalos = ListaFechas.Count;
                    for (var i = 0; i < totalIntervalos; i++)
                    {
                        string dia = ListaFechas[i].Day + "-" + COES.Base.Tools.Util.ObtenerNombreMesAbrev(ListaFechas[i].Month);
                        model.Grafico.XAxisCategories.Add(dia);
                    }

                }
                if (rbDetalleRpte == ConstantesRbDetalleRpte.SemanalProgramado) // semanal programado
                // Obtener Lista de nombres de las series del grafico.
                {
                    totalIntervalos =
                    COES.Base.Tools.Util.ObtenerNroSemanasxAnho(lstFechasAux.Max(), FirstDayOfWeek.Saturday) - COES.Base.Tools.Util.ObtenerNroSemanasxAnho(lstFechasAux.Min(), FirstDayOfWeek.Saturday) + 1;
                    for (var i = 0; i < totalIntervalos; i++)
                    {
                        string semana = "Sem" + "-" + COES.Base.Tools.Util.ObtenerNroSemanasxAnho(lstFechasAux[i], FirstDayOfWeek.Saturday);
                        model.Grafico.XAxisCategories.Add(semana);
                    }
                }
                if (rbDetalleRpte == ConstantesRbDetalleRpte.SemanalCronologico) // semanal cronologico
                {
                    totalIntervalos =
                    COES.Base.Tools.Util.ObtenerNroSemanasxAnho(lstFechasAux.Max(), FirstDayOfWeek.Sunday) - COES.Base.Tools.Util.ObtenerNroSemanasxAnho(lstFechasAux.Min(), FirstDayOfWeek.Sunday) + 1;
                    for (var i = 0; i < totalIntervalos; i++)
                    {
                        string semana = "Sem" + "-" + COES.Base.Tools.Util.ObtenerNroSemanasxAnho(lstFechasAux[i], FirstDayOfWeek.Sunday);
                        model.Grafico.XAxisCategories.Add(semana);
                    }
                }
                if (rbDetalleRpte == ConstantesRbDetalleRpte.Mensual) // Mensual
                {
                    //totalIntervalos = (ListaFechas[ListaFechas.Count].Month) - (ListaFechas[0].Month) + 1;
                    totalIntervalos = lstFechasAux.Max().Month - lstFechasAux.Min().Month + 1;
                    for (var i = 0; i < totalIntervalos; i++)
                    {
                        string mes = "Mes" + "-" + COES.Base.Tools.Util.ObtenerNombreMesAbrev(lstFechasAux[i].Month);
                        model.Grafico.XAxisCategories.Add(mes);
                    }
                }
                if (rbDetalleRpte == ConstantesRbDetalleRpte.Anual)// Anual
                {
                    totalIntervalos = lstFechasAux.Max().Year - lstFechasAux.Min().Year + 1;
                    for (var i = 0; i < totalIntervalos; i++)
                    {
                        string yyyy = "Año" + "-" + lstFechasAux[i].Year;
                        model.Grafico.XAxisCategories.Add(yyyy);
                    }
                }

                var listaGrupoMedicion = listaM24.Where(x => x.Meditotal != null).GroupBy(x => new { x.Ptomedicodi, x.Tipoinfocodi }).Select(group => group.First()).ToList();



                int nSeries = 0;
                foreach (var reg in listaGrupoMedicion)
                {
                    string nombreSerie = string.Empty;
                    if (reg.Famcodi == ConstantesHidrologia.EstacionHidrologica)//si es Estacion Hidrologica
                        nombreSerie = reg.Ptomedibarranomb;
                    else
                        nombreSerie = (reg.Equinomb.Length > 20) ? reg.Equinomb.Substring(0, 19) : reg.Equinomb;
                    var descrip = (reg.Tipoptomedinomb.Length > 15) ? reg.Tipoptomedinomb.Substring(0, 14) : reg.Tipoptomedinomb;
                    nombreSerie += " - " + descrip;

                    model.Grafico.Series.Add(new RegistroSerie());
                    model.Grafico.Series[nSeries].Name = nombreSerie;
                    model.Grafico.Series[nSeries].Data = new List<DatosSerie>();
                    model.Grafico.Series[nSeries].Type = "line";
                    nSeries++;
                }
                // Obtener lista de valores para las series del grafico
                for (var i = 0; i < listaGrupoMedicion.Count(); i++)
                {
                    //// Recorrer por cada pto medicion
                    var pto = listaGrupoMedicion[i].Ptomedicodi;
                    var lista = listaM24.Where(x => x.Ptomedicodi == pto).OrderBy(x => x.Medifecha);
                    foreach (var reg in lista)
                    {
                        for (var j = 1; j <= totalIntervalos; j++)
                        {
                            decimal? valor = null;
                            DateTime fechaSerie = DateTime.MinValue;
                            if (rbDetalleRpte == ConstantesRbDetalleRpte.Horas)
                            {
                                valor = (decimal?)reg.GetType().GetProperty("H" + j).GetValue(reg, null);
                                fechaSerie = reg.Medifecha.AddMinutes(j * 60);
                            }
                            else
                            {
                                valor = reg.Meditotal;
                                fechaSerie = reg.Medifecha;
                            }
                            model.Grafico.Series[i].Data.Add(new DatosSerie()
                            {
                                Y = valor,
                                X = fechaSerie
                            });
                        }
                    }
                }

                this.ModelGraficoDiario = model;

            }// end del if 
            return model;
        }

        /// <summary>
        /// exporta el reporte general consultado a archivo excel
        /// </summary>
        /// <param name="idsEmpresa"></param>
        /// <param name="idsCuenca"></param>
        /// <param name="idsFamilia"></param>
        /// <param name="idLectura"></param>
        /// <param name="idsPtoMedicion"></param>
        /// <param name="fechaInicial"></param>
        /// <param name="fechaFinal"></param>
        /// <param name="annho"></param>
        /// <param name="semanaIni"></param>
        /// <param name="semanaFin"></param>
        /// <param name="anhoInicial"></param>
        /// <param name="anhoFinal"></param>
        /// <param name="rbDetalleRpte"></param>
        /// <param name="unidad"></param>
        /// <returns></returns>

        [HttpPost]
        public JsonResult GenerarArchivoReporteXLS(string idsEmpresa, string idsCuenca, string idsFamilia, int idLectura, string idsPtoMedicion, string fechaInicial, string fechaFinal,
            string annho, int semanaIni, int semanaFin, string anhoInicial, string anhoFinal, int rbDetalleRpte, int unidad)
        {
            int indicador = 1;
            DateTime fechaIni = DateTime.MinValue;
            DateTime fechaFin = DateTime.MinValue;
            HidrologiaModel model = new HidrologiaModel();
            string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesHidrologia.FolderReporte;
            var entity = logic.GetByIdMeLectura(idLectura);
            var codigosPermitidos = new List<int> {40376, 40805, 40265, 40384, 40268, 40381, 40178, 40330, 40329, 40328, 40331, 40175, 40554, 40783, 40366, 40544, 40541, 40551, 40399, 40396, 40394, 40391, 40429, 40426, 41169, 40273, 40416, 41097, 41100, 40469, 40466, 40474, 40484, 40481, 40264, 40339, 40401, 40787, 40274, 40739, 40736, 40594, 40591, 41171, 40726, 41173, 41223, 40302, 40270, 40564, 40569, 40574, 40579, 40584, 40589, 40561, 40566, 40571, 40576, 40581, 40586, 41191, 41192, 41952, 40654, 40679, 40684, 40689, 40694, 40704, 40714, 40719, 40724, 41227, 41230, 41233, 40641, 40646, 40651, 40656, 40666, 40671, 40676, 40681, 40686, 40691, 40696, 40701, 40706, 40711, 40716, 40721, 41226, 41229, 41232, 41235, 40514, 40524, 40529, 41985, 40506, 40511, 40516, 40521, 40526, 41986, 40225, 40185, 40190, 40210, 40215, 40220, 40230, 40180, 40235, 40240, 40245, 40250, 40160, 40165, 40170, 40150, 40155, 40193, 40218, 40243, 40253, 40238, 40163, 40168, 40173, 40195, 40200, 40205, 40198, 40203, 40208 };
            //idTipoInformacion = (int)formato.Formatperiodo; // 1: diario, 2:semanal, 3: mensual, 4: anual
            try
            {
                int numeroMedicion = 0;
                switch (rbDetalleRpte)
                {
                    case ConstantesRbDetalleRpte.Diario:
                        numeroMedicion = ConstantesHidrologia.LectNroME1;
                        break;
                    case ConstantesRbDetalleRpte.Horas:
                        numeroMedicion = ConstantesHidrologia.LectNroME24;
                        break;
                    case ConstantesRbDetalleRpte.SemanalProgramado: //Rpte detallado semanal programado
                    case ConstantesRbDetalleRpte.SemanalCronologico: //Rpte detallado semanal cronológico
                    case ConstantesRbDetalleRpte.Mensual: // Rpte detallado mensual
                    case ConstantesRbDetalleRpte.Anual: //Rpte detallado anual
                        numeroMedicion = entity.Lectnro.Value;
                        break;
                }

                switch (numeroMedicion)
                {
                    case ConstantesHidrologia.LectNroME24://Horas
                        switch (entity.Lectperiodo)
                        {
                            case ConstHidrologia.Horas: //Diario
                                List<MeMedicion24DTO> lista24 = new List<MeMedicion24DTO>();
                                switch (rbDetalleRpte)
                                {
                                    case ConstantesRbDetalleRpte.Horas://Horas
                                        fechaIni = DateTime.ParseExact(fechaInicial, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                                        fechaFin = DateTime.ParseExact(fechaFinal, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                                        lista24 = this.logic.ListaMed24Hidrologia(idLectura, ConstantesHidrologia.IdOrigenLectura, idsEmpresa, idsCuenca,
                                            idsFamilia, fechaIni, fechaFin, idsPtoMedicion);
                                        //Filtrar lista por código permitidos en Portal Web
                                        lista24 = lista24.Where(m => codigosPermitidos.Contains(m.Ptomedicodi)).ToList();
                                        List<MeMedicion24DTO> listaCabeceraM24 = lista24.GroupBy(x => new { x.Ptomedicodi, x.Cuenca, x.Emprnomb, x.Emprcodi, x.Ptomedibarranomb, x.Tipoinfoabrev, x.Tipoptomedinomb, x.Equinomb, x.Famcodi, x.Famabrev })
                                            .Select(y => new MeMedicion24DTO()
                                            {
                                                Cuenca = y.Key.Cuenca,
                                                Emprcodi = y.Key.Emprcodi,
                                                Emprnomb = y.Key.Emprnomb,
                                                Famabrev = y.Key.Famabrev,
                                                Ptomedicodi = y.Key.Ptomedicodi,
                                                Ptomedibarranomb = y.Key.Ptomedibarranomb,
                                                Tipoinfoabrev = y.Key.Tipoinfoabrev,
                                                Tipoptomedinomb = y.Key.Tipoptomedinomb,
                                                Equinomb = y.Key.Equinomb,
                                                Famcodi = y.Key.Famcodi
                                            }
                                            ).ToList();
                                        model.FechaInicio = fechaIni.ToString(Constantes.FormatoFecha);
                                        model.FechaFin = fechaFin.ToString(Constantes.FormatoFecha);
                                        model.ListaMedicion24horas = lista24;
                                        model.ListaMedicion24Cabecera = listaCabeceraM24;
                                        model.TituloReporteXLS = "REPORTE " + entity.Lectnomb;
                                        model.SheetName = "HORAS";
                                        ExcelDocument.GenerarArchivoHidrologiaM24(model, rbDetalleRpte, ruta);
                                        indicador = 1;
                                        break;
                                    case ConstantesRbDetalleRpte.Diario://Diario
                                        fechaIni = DateTime.ParseExact(fechaInicial, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                                        fechaFin = DateTime.ParseExact(fechaFinal, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                                        if (unidad == ConstHidrologia.Caudal)
                                            lista24 = this.logic.GenerarListaDetalladaMed24Promedio(idLectura, ConstantesHidrologia.IdOrigenLectura,
                                                idsEmpresa, idsCuenca, idsFamilia, fechaIni, fechaFin, idsPtoMedicion, rbDetalleRpte);
                                        else
                                            lista24 = this.logic.GenerarListaMed24Hmaximo(idLectura, ConstantesHidrologia.IdOrigenLectura, idsEmpresa,
                                                idsCuenca, idsFamilia, fechaIni, fechaFin, idsPtoMedicion, rbDetalleRpte);

                                        //Filtrar lista por código permitidos en Portal Web
                                        lista24 = lista24.Where(m => codigosPermitidos.Contains(m.Ptomedicodi)).ToList();
                                        model.FechaInicio = fechaIni.ToString(Constantes.FormatoFecha);
                                        model.FechaFin = fechaFin.ToString(Constantes.FormatoFecha);
                                        model.ListaMedicion24horas = lista24;
                                        model.TituloReporteXLS = "REPORTE " + entity.Lectnomb;
                                        model.SheetName = "DIARIO";
                                        ExcelDocument.GenerarArchivoHidrologiaM24(model, rbDetalleRpte, ruta);
                                        indicador = 1;
                                        break;
                                    case ConstantesRbDetalleRpte.SemanalProgramado://Semanal Programado
                                        int ianho = Int32.Parse(annho);
                                        fechaIni = COES.Base.Tools.Util.GenerarFecha(ianho, semanaIni, ConstantesHidrologia.SemanalProgramado);
                                        fechaFin = COES.Base.Tools.Util.GenerarFecha(ianho, semanaFin, ConstantesHidrologia.SemanalProgramado);
                                        if (unidad == ConstHidrologia.Caudal)
                                            lista24 = this.logic.GenerarListaDetalladaMed24Promedio(idLectura, ConstantesHidrologia.IdOrigenLectura,
                                                idsEmpresa, idsCuenca, idsFamilia, fechaIni, fechaFin, idsPtoMedicion, rbDetalleRpte);
                                        else
                                            lista24 = this.logic.GenerarListaMed24Hmaximo(idLectura, ConstantesHidrologia.IdOrigenLectura, idsEmpresa,
                                                idsCuenca, idsFamilia, fechaIni, fechaFin, idsPtoMedicion, rbDetalleRpte);

                                        //Filtrar lista por código permitidos en Portal Web
                                        lista24 = lista24.Where(m => codigosPermitidos.Contains(m.Ptomedicodi)).ToList();
                                        model.FechaInicio = "Sem - " + COES.Base.Tools.Util.GenerarNroSemana(fechaIni, FirstDayOfWeek.Saturday) + " - " + fechaIni.Year;
                                        model.FechaFin = "Sem - " + COES.Base.Tools.Util.GenerarNroSemana(fechaFin, FirstDayOfWeek.Sunday) + " - " + fechaIni.Year;
                                        model.ListaMedicion24horas = lista24;
                                        model.TituloReporteXLS = "REPORTE " + entity.Lectnomb;
                                        model.SheetName = "SEMANAL PROGRAMADO";
                                        ExcelDocument.GenerarArchivoHidrologiaM24(model, rbDetalleRpte, ruta);
                                        indicador = 1;
                                        break;
                                    case ConstantesRbDetalleRpte.SemanalCronologico://Semanal Cronológico
                                        int ianho2 = Int32.Parse(annho);
                                        fechaIni = COES.Base.Tools.Util.GenerarFecha(ianho2, semanaIni, ConstantesHidrologia.SemanalCronologico);
                                        fechaFin = COES.Base.Tools.Util.GenerarFecha(ianho2, semanaFin, ConstantesHidrologia.SemanalCronologico);
                                        if (unidad == ConstHidrologia.Caudal)
                                            lista24 = this.logic.GenerarListaDetalladaMed24Promedio(idLectura, ConstantesHidrologia.IdOrigenLectura,
                                                idsEmpresa, idsCuenca, idsFamilia, fechaIni, fechaFin, idsPtoMedicion, rbDetalleRpte);
                                        else
                                            lista24 = this.logic.GenerarListaMed24Hmaximo(idLectura, ConstantesHidrologia.IdOrigenLectura, idsEmpresa,
                                                idsCuenca, idsFamilia, fechaIni, fechaFin, idsPtoMedicion, rbDetalleRpte);
                                        //Filtrar lista por código permitidos en Portal Web
                                        lista24 = lista24.Where(m => codigosPermitidos.Contains(m.Ptomedicodi)).ToList();
                                        model.FechaInicio = "Sem - " + COES.Base.Tools.Util.GenerarNroSemana(fechaIni, FirstDayOfWeek.Saturday) + " - " + fechaIni.Year;
                                        model.FechaFin = "Sem - " + COES.Base.Tools.Util.GenerarNroSemana(fechaFin, FirstDayOfWeek.Sunday) + " - " + fechaIni.Year;
                                        model.ListaMedicion24horas = lista24;
                                        model.TituloReporteXLS = "REPORTE " + entity.Lectnomb;
                                        model.SheetName = "SEMANAL CRONOLOGICO";
                                        ExcelDocument.GenerarArchivoHidrologiaM24(model, rbDetalleRpte, ruta);
                                        indicador = 1;
                                        break;
                                    case ConstantesRbDetalleRpte.Mensual://Mensual
                                        fechaIni = COES.Base.Tools.Util.FormatFecha(fechaInicial);
                                        fechaFin = COES.Base.Tools.Util.FormatFecha(fechaFinal);
                                        fechaFin = fechaFin.AddMonths(1);
                                        fechaFin = fechaFin.AddDays(-1);
                                        if (unidad == ConstHidrologia.Caudal)
                                            lista24 = this.logic.GenerarListaDetalladaMed24Promedio(idLectura, ConstantesHidrologia.IdOrigenLectura,
                                                idsEmpresa, idsCuenca, idsFamilia, fechaIni, fechaFin, idsPtoMedicion, rbDetalleRpte);
                                        else
                                            lista24 = this.logic.GenerarListaMed24Hmaximo(idLectura, ConstantesHidrologia.IdOrigenLectura, idsEmpresa,
                                                idsCuenca, idsFamilia, fechaIni, fechaFin, idsPtoMedicion, rbDetalleRpte);
                                        //Filtrar lista por código permitidos en Portal Web
                                        lista24 = lista24.Where(m => codigosPermitidos.Contains(m.Ptomedicodi)).ToList();
                                        var anho = fechaIni.Year.ToString();
                                        var mes = fechaIni.Month;
                                        model.FechaInicio = COES.Base.Tools.Util.ObtenerNombreMes(mes) + " - " + anho;
                                        anho = fechaFin.Year.ToString();
                                        mes = fechaFin.Month;
                                        model.FechaFin = COES.Base.Tools.Util.ObtenerNombreMes(mes) + " - " + anho;
                                        model.ListaMedicion24horas = lista24;
                                        model.TituloReporteXLS = "REPORTE " + entity.Lectnomb;
                                        model.SheetName = "MENSUAL";
                                        ExcelDocument.GenerarArchivoHidrologiaM24(model, rbDetalleRpte, ruta);
                                        indicador = 1;
                                        break;
                                    case ConstantesRbDetalleRpte.Anual://Anual
                                        int ianhoIni = Int32.Parse(anhoInicial);
                                        int ianhoFin = Int32.Parse(anhoFinal);
                                        fechaIni = new DateTime(ianhoIni, 1, 1);
                                        fechaFin = new DateTime(ianhoFin, 12, 31);
                                        if (unidad == ConstHidrologia.Caudal)
                                            lista24 = this.logic.GenerarListaMed24PromAnual(idLectura, ConstantesHidrologia.IdOrigenLectura, idsEmpresa,
                                                idsCuenca, idsFamilia, fechaIni, fechaFin, idsPtoMedicion);
                                        else
                                            lista24 = this.logic.GenerarListaMed24AnualHmaximo(idLectura, ConstantesHidrologia.IdOrigenLectura, idsEmpresa,
                                                idsCuenca, idsFamilia, fechaIni, fechaFin, idsPtoMedicion);
                                        //Filtrar lista por código permitidos en Portal Web
                                        lista24 = lista24.Where(m => codigosPermitidos.Contains(m.Ptomedicodi)).ToList();

                                        model.FechaInicio = anhoInicial;
                                        model.FechaFin = anhoFinal;
                                        model.ListaMedicion24horas = lista24;
                                        model.TituloReporteXLS = "REPORTE " + entity.Lectnomb;
                                        model.SheetName = "ANUAL";
                                        ExcelDocument.GenerarArchivoHidrologiaM24(model, rbDetalleRpte, ruta);
                                        indicador = 1;
                                        break;
                                }

                                break;
                        }
                        break;
                    case ConstantesHidrologia.LectNroME1:
                        switch (entity.Lectperiodo)
                        {
                            case ConstHidrologia.Diario: //Diario
                                List<MeMedicion1DTO> listaM1 = new List<MeMedicion1DTO>();
                                switch (rbDetalleRpte)
                                {
                                    case ConstantesRbDetalleRpte.Diario: //Dias
                                        fechaIni = DateTime.ParseExact(fechaInicial, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                                        fechaFin = DateTime.ParseExact(fechaFinal, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                                        listaM1 = this.logic.ListaMed1Hidrologia(idLectura, ConstantesHidrologia.IdOrigenLectura, idsEmpresa, idsCuenca, idsFamilia, fechaIni, fechaFin, idsPtoMedicion);
                                        //Filtrar lista por codigos permitidos en Portal Web
                                        listaM1 = listaM1.Where(m => codigosPermitidos.Contains(m.Ptomedicodi)).ToList();
                                        model.ListaMedicion1 = listaM1;
                                        model.FechaInicio = fechaIni.ToString(Constantes.FormatoFecha);
                                        model.FechaFin = fechaFin.ToString(Constantes.FormatoFecha);
                                        model.TituloReporteXLS = "REPORTE " + entity.Lectnomb + " - DIARIO";
                                        model.SheetName = "DIARIO";
                                        ExcelDocument.GenerarArchivoHidrologiaM1(model, rbDetalleRpte, ruta);
                                        indicador = 4;
                                        break;
                                    case ConstantesRbDetalleRpte.SemanalProgramado: //Semanal programado
                                        int ianho = Int32.Parse(annho);
                                        fechaIni = COES.Base.Tools.Util.GenerarFecha(ianho, semanaIni, ConstantesHidrologia.SemanalProgramado);
                                        fechaFin = COES.Base.Tools.Util.GenerarFecha(ianho, semanaFin, ConstantesHidrologia.SemanalProgramado);
                                        if (unidad == ConstHidrologia.Caudal)
                                            listaM1 = this.logic.GenerarListaDetalladaMed1Promedio(idLectura, ConstantesHidrologia.IdOrigenLectura, idsEmpresa, idsCuenca, idsFamilia, fechaIni, fechaFin, idsPtoMedicion, rbDetalleRpte);
                                        else
                                            listaM1 = this.logic.GenerarListaDetalladaMed1Hmax(idLectura, ConstantesHidrologia.IdOrigenLectura, idsEmpresa, idsCuenca, idsFamilia, fechaIni, fechaFin, idsPtoMedicion, rbDetalleRpte);
                                        //Filtrar lista por codigos permitidos en Portal Web
                                        listaM1 = listaM1.Where(m => codigosPermitidos.Contains(m.Ptomedicodi)).ToList();
                                        model.ListaMedicion1 = listaM1;
                                        model.FechaInicio = "Sem - " + COES.Base.Tools.Util.GenerarNroSemana(fechaIni, FirstDayOfWeek.Saturday) + " - " + fechaIni.Year;
                                        model.FechaFin = "Sem - " + COES.Base.Tools.Util.GenerarNroSemana(fechaFin, FirstDayOfWeek.Sunday) + " - " + fechaIni.Year;
                                        model.TituloReporteXLS = "REPORTE " + entity.Lectnomb + " - SEMANAL PROGRAMADO";
                                        model.SheetName = "SEMANAL PROGRAMADO";
                                        ExcelDocument.GenerarArchivoHidrologiaM1(model, rbDetalleRpte, ruta);
                                        indicador = 4;
                                        break;
                                    case ConstantesRbDetalleRpte.SemanalCronologico: // Semanal Cronológico
                                        int ianho2 = Int32.Parse(annho);
                                        fechaIni = COES.Base.Tools.Util.GenerarFecha(ianho2, semanaIni, ConstantesHidrologia.SemanalCronologico);
                                        fechaFin = COES.Base.Tools.Util.GenerarFecha(ianho2, semanaFin, ConstantesHidrologia.SemanalCronologico);
                                        if (unidad == ConstHidrologia.Caudal)
                                            listaM1 = this.logic.GenerarListaDetalladaMed1Promedio(idLectura, ConstantesHidrologia.IdOrigenLectura, idsEmpresa, idsCuenca, idsFamilia, fechaIni, fechaFin, idsPtoMedicion, rbDetalleRpte);
                                        else
                                            listaM1 = this.logic.GenerarListaDetalladaMed1Hmax(idLectura, ConstantesHidrologia.IdOrigenLectura, idsEmpresa, idsCuenca, idsFamilia, fechaIni, fechaFin, idsPtoMedicion, rbDetalleRpte);
                                        //Filtrar lista por codigos permitidos en Portal Web
                                        listaM1 = listaM1.Where(m => codigosPermitidos.Contains(m.Ptomedicodi)).ToList();
                                        model.ListaMedicion1 = listaM1;
                                        model.FechaInicio = "Sem - " + COES.Base.Tools.Util.GenerarNroSemana(fechaIni, FirstDayOfWeek.Saturday) + " - " + fechaIni.Year;
                                        model.FechaFin = "Sem - " + COES.Base.Tools.Util.GenerarNroSemana(fechaFin, FirstDayOfWeek.Sunday) + " - " + fechaIni.Year;
                                        model.TituloReporteXLS = "REPORTE " + entity.Lectnomb + " - SEMANAL CRONOLÓGICO";
                                        model.SheetName = "SEMANAL PROGRAMADO";
                                        ExcelDocument.GenerarArchivoHidrologiaM1(model, rbDetalleRpte, ruta);
                                        indicador = 4;
                                        break;
                                    case ConstantesRbDetalleRpte.Mensual: // Mes
                                        fechaIni = COES.Base.Tools.Util.FormatFecha(fechaInicial);
                                        fechaFin = COES.Base.Tools.Util.FormatFecha(fechaFinal);
                                        fechaFin = fechaFin.AddMonths(1);
                                        fechaFin = fechaFin.AddDays(-1);
                                        if (unidad == ConstHidrologia.Caudal)
                                            listaM1 = this.logic.GenerarListaDetalladaMed1Promedio(idLectura, ConstantesHidrologia.IdOrigenLectura, idsEmpresa, idsCuenca, idsFamilia, fechaIni, fechaFin, idsPtoMedicion, rbDetalleRpte);
                                        else
                                            listaM1 = this.logic.GenerarListaDetalladaMed1Hmax(idLectura, ConstantesHidrologia.IdOrigenLectura, idsEmpresa, idsCuenca, idsFamilia, fechaIni, fechaFin, idsPtoMedicion, rbDetalleRpte);
                                        //Filtrar lista por codigos permitidos en Portal Web
                                        listaM1 = listaM1.Where(m => codigosPermitidos.Contains(m.Ptomedicodi)).ToList();
                                        model.ListaMedicion1 = listaM1;
                                        model.FechaInicio = COES.Base.Tools.Util.ObtenerNombreMes(fechaIni.Month) + " - " + fechaIni.Year;
                                        model.FechaFin = COES.Base.Tools.Util.ObtenerNombreMes(fechaFin.Month) + " - " + fechaFin.Year;
                                        model.TituloReporteXLS = "REPORTE " + entity.Lectnomb + " - MENSUAL";
                                        model.SheetName = "MENSUAL";
                                        ExcelDocument.GenerarArchivoHidrologiaM1(model, rbDetalleRpte, ruta);
                                        indicador = 4;
                                        break;
                                    case ConstantesRbDetalleRpte.Anual: //Anual
                                        int ianhoIni = Int32.Parse(anhoInicial);
                                        int ianhoFin = Int32.Parse(anhoFinal);
                                        fechaIni = new DateTime(ianhoIni, 1, 1);
                                        fechaFin = new DateTime(ianhoFin, 12, 31);
                                        if (unidad == ConstHidrologia.Caudal)
                                            listaM1 = this.logic.GenerarListaMed1PromAnual(idLectura, ConstantesHidrologia.IdOrigenLectura, idsEmpresa, idsCuenca, idsFamilia, fechaIni, fechaFin, idsPtoMedicion);
                                        else
                                            listaM1 = this.logic.GenerarListaMed1AnualHmaximo(idLectura, ConstantesHidrologia.IdOrigenLectura, idsEmpresa, idsCuenca, idsFamilia, fechaIni, fechaFin, idsPtoMedicion);
                                        //Filtrar lista por codigos permitidos en Portal Web
                                        listaM1 = listaM1.Where(m => codigosPermitidos.Contains(m.Ptomedicodi)).ToList();
                                        model.ListaMedicion1 = listaM1;
                                        model.FechaInicio = fechaIni.Year.ToString();
                                        model.FechaFin = fechaFin.Year.ToString();
                                        model.TituloReporteXLS = "REPORTE " + entity.Lectnomb + " - ANUAL";
                                        model.SheetName = "ANUAL";
                                        ExcelDocument.GenerarArchivoHidrologiaM1(model, rbDetalleRpte, ruta);
                                        indicador = 4;
                                        break;
                                }

                                break;
                            case ConstHidrologia.SemanalProg:
                            case ConstHidrologia.Semanal://Semanal
                                switch (rbDetalleRpte)
                                {
                                    case ConstantesRbDetalleRpte.SemanalProgramado: //Semanal programado
                                        int ianho = Int32.Parse(annho);
                                        fechaIni = COES.Base.Tools.Util.GenerarFecha(ianho, semanaIni, ConstantesHidrologia.SemanalProgramado);
                                        fechaFin = COES.Base.Tools.Util.GenerarFecha(ianho, semanaFin, ConstantesHidrologia.SemanalProgramado);
                                        List<MeMedicion1DTO> listaS = this.logic.ListaMed1Hidrologia(idLectura, ConstantesHidrologia.IdOrigenLectura, idsEmpresa, idsCuenca, idsFamilia, fechaIni, fechaFin, idsPtoMedicion);
                                        //Filtrar lista por codigos permitidos en Portal Web
                                        listaS = listaS.Where(m => codigosPermitidos.Contains(m.Ptomedicodi)).ToList();
                                        model.ListaMedicion1 = listaS;
                                        model.FechaInicio = "Sem - " + COES.Base.Tools.Util.GenerarNroSemana(fechaIni, FirstDayOfWeek.Saturday) + " - " + fechaIni.Year;
                                        model.FechaFin = "Sem - " + COES.Base.Tools.Util.GenerarNroSemana(fechaFin, FirstDayOfWeek.Sunday) + " - " + fechaIni.Year;
                                        model.TituloReporteXLS = "REPORTE " + entity.Lectnomb;
                                        model.SheetName = "SEMANAL";
                                        //ExcelDocument.GenerarArchivoHidrologiaSemanal(model);
                                        //indicador = 2;
                                        ExcelDocument.GenerarArchivoHidrologiaM1(model, rbDetalleRpte, ruta);
                                        indicador = 4;
                                        break;
                                    case ConstantesRbDetalleRpte.Mensual: // Mes
                                        fechaIni = COES.Base.Tools.Util.FormatFecha(fechaInicial);
                                        fechaFin = COES.Base.Tools.Util.FormatFecha(fechaFinal);
                                        fechaFin = fechaFin.AddMonths(1);
                                        fechaFin = fechaFin.AddDays(-1);
                                        if (unidad == ConstHidrologia.Caudal)
                                            listaM1 = this.logic.GenerarListaDetalladaMed1Promedio(idLectura, ConstantesHidrologia.IdOrigenLectura, idsEmpresa, idsCuenca, idsFamilia, fechaIni, fechaFin, idsPtoMedicion, rbDetalleRpte);
                                        else
                                            listaM1 = this.logic.GenerarListaDetalladaMed1Hmax(idLectura, ConstantesHidrologia.IdOrigenLectura, idsEmpresa, idsCuenca, idsFamilia, fechaIni, fechaFin, idsPtoMedicion, rbDetalleRpte);
                                        //Filtrar lista por codigos permitidos en Portal Web
                                        listaM1 = listaM1.Where(m => codigosPermitidos.Contains(m.Ptomedicodi)).ToList();
                                        model.ListaMedicion1 = listaM1;
                                        model.FechaInicio = COES.Base.Tools.Util.ObtenerNombreMes(fechaIni.Month) + " - " + fechaIni.Year;
                                        model.FechaFin = COES.Base.Tools.Util.ObtenerNombreMes(fechaFin.Month) + " - " + fechaFin.Year;
                                        model.TituloReporteXLS = "REPORTE " + entity.Lectnomb + " - MENSUAL";
                                        model.SheetName = "MENSUAL";
                                        ExcelDocument.GenerarArchivoHidrologiaM1(model, rbDetalleRpte, ruta);
                                        indicador = 4;
                                        break;
                                    case ConstantesRbDetalleRpte.Anual: // anual
                                        int ianhoIni = Int32.Parse(anhoInicial);
                                        int ianhoFin = Int32.Parse(anhoFinal);
                                        fechaIni = new DateTime(ianhoIni, 1, 1);
                                        fechaFin = new DateTime(ianhoFin, 12, 31);
                                        if (unidad == ConstHidrologia.Caudal)
                                            listaM1 = this.logic.GenerarListaMed1PromAnual(idLectura, ConstantesHidrologia.IdOrigenLectura, idsEmpresa, idsCuenca, idsFamilia, fechaIni, fechaFin, idsPtoMedicion);
                                        else
                                            listaM1 = this.logic.GenerarListaMed1AnualHmaximo(idLectura, ConstantesHidrologia.IdOrigenLectura, idsEmpresa, idsCuenca, idsFamilia, fechaIni, fechaFin, idsPtoMedicion);
                                        //Filtrar lista por codigos permitidos en Portal Web
                                        listaM1 = listaM1.Where(m => codigosPermitidos.Contains(m.Ptomedicodi)).ToList();
                                        model.ListaMedicion1 = listaM1;
                                        model.FechaInicio = fechaIni.Year.ToString();
                                        model.FechaFin = fechaFin.Year.ToString();
                                        model.TituloReporteXLS = "REPORTE " + entity.Lectnomb + " - ANUAL";
                                        model.SheetName = "ANUAL";
                                        ExcelDocument.GenerarArchivoHidrologiaM1(model, rbDetalleRpte, ruta);
                                        indicador = 4;
                                        break;
                                }
                                break;
                            case ConstHidrologia.Mensual://Mes
                                switch (rbDetalleRpte)
                                {
                                    case ConstantesRbDetalleRpte.Mensual: // Mes
                                        fechaIni = COES.Base.Tools.Util.FormatFecha(fechaInicial);
                                        fechaFin = COES.Base.Tools.Util.FormatFecha(fechaFinal);
                                        fechaFin = fechaFin.AddMonths(1);
                                        fechaFin = fechaFin.AddDays(-1);
                                        List<MeMedicion1DTO> lista = this.logic.ListaMed1Hidrologia(idLectura, ConstantesHidrologia.IdOrigenLectura, idsEmpresa, idsCuenca, idsFamilia, fechaIni, fechaFin, idsPtoMedicion);
                                        //Filtrar lista por codigos permitidos en Portal Web
                                        lista = lista.Where(m => codigosPermitidos.Contains(m.Ptomedicodi)).ToList();
                                        model.ListaMedicion1 = lista;
                                        var anho = fechaIni.Year.ToString();
                                        var mes = fechaIni.Month;
                                        model.FechaInicio = COES.Base.Tools.Util.ObtenerNombreMes(mes) + " - " + anho;
                                        anho = fechaFin.Year.ToString();
                                        mes = fechaFin.Month;
                                        model.FechaFin = COES.Base.Tools.Util.ObtenerNombreMes(mes) + " - " + anho;
                                        model.TituloReporteXLS = "REPORTE " + entity.Lectnomb;
                                        model.SheetName = "MENSUAL";
                                        ExcelDocument.GenerarArchivoHidrologiaM1(model, rbDetalleRpte, ruta);
                                        indicador = 4;
                                        //ExcelDocument.GenerarArchivoHidrologiaMesQN(model);
                                        //indicador = 3;
                                        break;
                                    case ConstantesRbDetalleRpte.Anual:
                                        int ianhoIni = Int32.Parse(anhoInicial);
                                        int ianhoFin = Int32.Parse(anhoFinal);
                                        fechaIni = new DateTime(ianhoIni, 1, 1);
                                        fechaFin = new DateTime(ianhoFin, 12, 31);
                                        if (unidad == ConstHidrologia.Caudal)
                                            listaM1 = this.logic.GenerarListaMed1PromAnual(idLectura, ConstantesHidrologia.IdOrigenLectura, idsEmpresa, idsCuenca, idsFamilia, fechaIni, fechaFin, idsPtoMedicion);
                                        else
                                            listaM1 = this.logic.GenerarListaMed1AnualHmaximo(idLectura, ConstantesHidrologia.IdOrigenLectura, idsEmpresa, idsCuenca, idsFamilia, fechaIni, fechaFin, idsPtoMedicion);
                                        //Filtrar lista por codigos permitidos en Portal Web
                                        listaM1 = listaM1.Where(m => codigosPermitidos.Contains(m.Ptomedicodi)).ToList();
                                        model.ListaMedicion1 = listaM1;
                                        model.FechaInicio = fechaIni.Year.ToString();
                                        model.FechaFin = fechaFin.Year.ToString();
                                        model.TituloReporteXLS = "REPORTE " + entity.Lectnomb + " - ANUAL";
                                        model.SheetName = "ANUAL";
                                        ExcelDocument.GenerarArchivoHidrologiaM1(model, rbDetalleRpte, ruta);
                                        indicador = 4;
                                        break;
                                }
                                break;
                        }
                        break;
                }
            }
            catch
            {
                indicador = -1;
            }

            return Json(indicador);
        }
        //Obtener Reporte de Hidrologia para Portal Web
        public string ObtenerReporteHidrologia(string idsEmpresa, string idsCuenca, string idsFamilia, string idsPtoMedicion, DateTime fechaInicio, DateTime fechaFin, int idLectura, int opcion, int rbDetalleRpte, int unidad, int numeroMedicion)
        {// rbDetalleRpte(0: Horas, 1: Diario, 2,3: Semanal, 4: Mensual, 5: Anual)
            NumberFormatInfo nfi = logic.GenerarNumberFormatInfo();
            string strHtml = string.Empty;
            if (string.IsNullOrEmpty(idsEmpresa)) idsEmpresa = ConstantesAppServicio.ParametroNoExiste;
            List<Object> listaGenerica = new List<Object>();
            List<MeMedicion24DTO> lista24 = new List<MeMedicion24DTO>();
            var nroLectura = logic.GetByIdMeLectura(idLectura);
            //Lista de códigos solicitados para Portal Web
            var codigosPermitidos = new List<int> { 40376, 40805, 40265, 40384, 40268, 40381, 40178, 40330, 40329, 40328, 40331, 40175, 40554, 40783, 40366, 40544, 40541, 40551, 40399, 40396, 40394, 40391, 40429, 40426, 41169, 40273, 40416, 41097, 41100, 40469, 40466, 40474, 40484, 40481, 40264, 40339, 40401, 40787, 40274, 40739, 40736, 40594, 40591, 41171, 40726, 41173, 41223, 40302, 40270, 40564, 40569, 40574, 40579, 40584, 40589, 40561, 40566, 40571, 40576, 40581, 40586, 41191, 41192, 41952, 40654, 40679, 40684, 40689, 40694, 40704, 40714, 40719, 40724, 41227, 41230, 41233, 40641, 40646, 40651, 40656, 40666, 40671, 40676, 40681, 40686, 40691, 40696, 40701, 40706, 40711, 40716, 40721, 41226, 41229, 41232, 41235, 40514, 40524, 40529, 41985, 40506, 40511, 40516, 40521, 40526, 41986, 40225, 40185, 40190, 40210, 40215, 40220, 40230, 40180, 40235, 40240, 40245, 40250, 40160, 40165, 40170, 40150, 40155, 40193, 40218, 40243, 40253, 40238, 40163, 40168, 40173, 40195, 40200, 40205, 40198, 40203, 40208 };
            switch (numeroMedicion)
            {
                case ConstantesAppServicio.LectNroME24:
                    switch (nroLectura.Lectperiodo)
                    {
                        case ConstHidrologia.Horas:// 
                            if (rbDetalleRpte == ConstantesRbDetalleRpte.Horas)
                                lista24 = this.logic.ListaMed24Hidrologia(idLectura, ConstantesAppServicio.IdOrigenLectura,
                                    idsEmpresa, idsCuenca, idsFamilia, fechaInicio, fechaInicio, idsPtoMedicion);
                            else
                                lista24 = this.logic.ListaMed24Hidrologia(idLectura, ConstantesAppServicio.IdOrigenLectura,
                                    idsEmpresa, idsCuenca, idsFamilia, fechaInicio, fechaFin, idsPtoMedicion);
                            //Filtrar lista por codigos permitidos para Portal Web
                            lista24 = lista24.Where(m => codigosPermitidos.Contains(m.Ptomedicodi)).ToList();

                            List<MeMedicion24DTO> listaCabeceraM24 = lista24.GroupBy(x => new { x.Ptomedicodi, x.Cuenca, x.Emprnomb, x.Ptomedibarranomb, x.Tipoinfoabrev, x.Tipoptomedinomb, x.Equinomb, x.Famcodi, x.Famabrev })
                                .Select(y => new MeMedicion24DTO()
                                {
                                    Cuenca = y.Key.Cuenca,
                                    Emprnomb = y.Key.Emprnomb,
                                    Famabrev = y.Key.Famabrev,
                                    Ptomedicodi = y.Key.Ptomedicodi,
                                    Ptomedibarranomb = y.Key.Ptomedibarranomb,
                                    Tipoinfoabrev = y.Key.Tipoinfoabrev,
                                    Tipoptomedinomb = y.Key.Tipoptomedinomb,
                                    Equinomb = y.Key.Equinomb,
                                    Famcodi = y.Key.Famcodi
                                }
                                ).ToList();

                            switch (rbDetalleRpte)
                            {
                                case ConstantesRbDetalleRpte.Horas: //Rpte detallado horas   
                                    strHtml = logic.GeneraViewHidrologiaMed24_2(lista24, (int)nroLectura.Lectnro, fechaInicio);
                                    break;
                                case ConstantesRbDetalleRpte.Diario: //Rpte detallado diario                                      
                                    if (unidad == ConstHidrologia.Caudal)
                                    { //Caudal
                                        var listaTemp = logic.GenerarListaDetalladaMed24Promedio(idLectura, ConstantesAppServicio.IdOrigenLectura, idsEmpresa, idsCuenca,
                                            idsFamilia, fechaInicio, fechaFin, idsPtoMedicion, rbDetalleRpte);
                                        //Filtrar lista por codigos permitidos para Portal Web
                                        listaTemp = listaTemp.Where(m => codigosPermitidos.Contains(m.Ptomedicodi)).ToList();
                                        strHtml = logic.GeneraViewHidrologiaMed24(listaTemp, listaCabeceraM24, (int)nroLectura.Lectnro, fechaInicio, rbDetalleRpte);
                                    }
                                    else
                                    {
                                        var listaTemp = logic.GenerarListaMed24Hmaximo(idLectura, ConstantesAppServicio.IdOrigenLectura, idsEmpresa,
                                            idsCuenca, idsFamilia, fechaInicio, fechaFin, idsPtoMedicion, rbDetalleRpte);
                                        //Filtrar lista por codigos permitidos para Portal Web
                                        listaTemp = listaTemp.Where(m => codigosPermitidos.Contains(m.Ptomedicodi)).ToList();
                                        strHtml = logic.GeneraViewHidrologiaMed24(listaTemp, listaCabeceraM24, (int)nroLectura.Lectnro, fechaInicio, rbDetalleRpte);

                                    }

                                    break;
                                case ConstantesRbDetalleRpte.SemanalProgramado:
                                case ConstantesRbDetalleRpte.SemanalCronologico://Rpte detallado semanal programado, cronologico
                                    if (unidad == ConstHidrologia.Caudal) //Cuadal
                                    {
                                        var listaTemp = logic.GenerarListaDetalladaMed24Promedio(idLectura, ConstantesAppServicio.IdOrigenLectura, idsEmpresa, idsCuenca,
                                            idsFamilia, fechaInicio, fechaFin, idsPtoMedicion, rbDetalleRpte);
                                        //Filtrar lista por codigos permitidos para Portal Web
                                        listaTemp = listaTemp.Where(m => codigosPermitidos.Contains(m.Ptomedicodi)).ToList();
                                        strHtml = logic.GeneraViewHidrologiaMed24(listaTemp, listaCabeceraM24, (int)nroLectura.Lectnro, fechaInicio, rbDetalleRpte);
                                    }

                                        
                                    else
                                    {
                                        var listaTemp = logic.GenerarListaMed24Hmaximo(idLectura, ConstantesAppServicio.IdOrigenLectura, idsEmpresa, idsCuenca,
                                                idsFamilia, fechaInicio, fechaFin, idsPtoMedicion, rbDetalleRpte);
                                        //Filtrar lista por codigos permitidos para Portal Web
                                        listaTemp = listaTemp.Where(m => codigosPermitidos.Contains(m.Ptomedicodi)).ToList();
                                        strHtml = logic.GeneraViewHidrologiaMed24(listaTemp, listaCabeceraM24, (int)nroLectura.Lectnro, fechaInicio, rbDetalleRpte);

                                    }

                                        
                                    break;
                                case ConstantesRbDetalleRpte.Mensual: // Rpte detallado mensual
                                    if (unidad == ConstHidrologia.Caudal) //Caudal
                                    {
                                        var listaTemp = logic.GenerarListaDetalladaMed24Promedio(idLectura, ConstantesAppServicio.IdOrigenLectura, idsEmpresa, idsCuenca, idsFamilia,
                                                fechaInicio, fechaFin, idsPtoMedicion, rbDetalleRpte);
                                        //Filtrar lista por codigos permitidos para Portal Web
                                        listaTemp = listaTemp.Where(m => codigosPermitidos.Contains(m.Ptomedicodi)).ToList();
                                        strHtml = logic.GeneraViewHidrologiaMed24(listaTemp,
                                            listaCabeceraM24, (int)nroLectura.Lectnro, fechaInicio, rbDetalleRpte);
                                    }
                                    else
                                    {
                                        var listaTemp = logic.GenerarListaMed24Hmaximo(idLectura, ConstantesAppServicio.IdOrigenLectura, idsEmpresa, idsCuenca, idsFamilia,
                                                fechaInicio, fechaFin, idsPtoMedicion, rbDetalleRpte);
                                        //Filtrar lista por codigos permitidos para Portal Web
                                        listaTemp = listaTemp.Where(m => codigosPermitidos.Contains(m.Ptomedicodi)).ToList();
                                        strHtml = logic.GeneraViewHidrologiaMed24(listaTemp,
                                            listaCabeceraM24, (int)nroLectura.Lectnro, fechaInicio, rbDetalleRpte);
                                    }
                                    break;
                                case ConstantesRbDetalleRpte.Anual: //Rpte detallado anual
                                    if (unidad == ConstHidrologia.Caudal) //Caudal
                                    {
                                        var listaTemp = logic.GenerarListaMed24PromAnual(idLectura, ConstantesAppServicio.IdOrigenLectura, idsEmpresa, idsCuenca, idsFamilia, fechaInicio, fechaFin, idsPtoMedicion);
                                        //Filtrar lista por codigos permitidos para Portal Web
                                        listaTemp = listaTemp.Where(m => codigosPermitidos.Contains(m.Ptomedicodi)).ToList();
                                        strHtml = logic.GeneraViewHidrologiaMed24(listaTemp,
                                            listaCabeceraM24, (int)nroLectura.Lectnro, fechaInicio, rbDetalleRpte);
                                    }
                                        
                                    else
                                    {
                                        var listaTemp = logic.GenerarListaMed24AnualHmaximo(idLectura, ConstantesAppServicio.IdOrigenLectura, idsEmpresa, idsCuenca, idsFamilia, fechaInicio, fechaFin, idsPtoMedicion);
                                        //Filtrar lista por codigos permitidos para Portal Web
                                        listaTemp = listaTemp.Where(m => codigosPermitidos.Contains(m.Ptomedicodi)).ToList();
                                        strHtml = logic.GeneraViewHidrologiaMed24(listaTemp,
                                            listaCabeceraM24, (int)nroLectura.Lectnro, fechaInicio, rbDetalleRpte);
                                    }
                                    break;
                            }
                            break;
                    }
                    break;
                case ConstantesAppServicio.LectNroME1:
                    List<MeMedicion1DTO> listaMed1 = this.logic.ListaMed1Hidrologia(idLectura, ConstantesAppServicio.IdOrigenLectura,
                        idsEmpresa, idsCuenca, idsFamilia, fechaInicio, fechaFin, idsPtoMedicion);
                    //Filtrar lista por códigos permitidos en Portal Web
                    listaMed1 = listaMed1.Where(m => codigosPermitidos.Contains(m.Ptomedicodi)).ToList();
                    List<MeMedicion1DTO> listaCabeceraPM1 = listaMed1.GroupBy(x => new { x.Ptomedicodi, x.Cuenca, x.Emprnomb, x.Ptomedibarranomb, x.Tipoinfoabrev, x.Tipoptomedinomb, x.Equinomb, x.Famcodi, x.Famabrev })
                        .Select(y => new MeMedicion1DTO()
                        {
                            Cuenca = y.Key.Cuenca,
                            Emprnomb = y.Key.Emprnomb,
                            Famabrev = y.Key.Famabrev,
                            Ptomedicodi = y.Key.Ptomedicodi,
                            Ptomedibarranomb = y.Key.Ptomedibarranomb,
                            Tipoinfoabrev = y.Key.Tipoinfoabrev,
                            Tipoptomedinomb = y.Key.Tipoptomedinomb,
                            Equinomb = y.Key.Equinomb,
                            Famcodi = y.Key.Famcodi
                        }
                        ).ToList();


                    switch (nroLectura.Lectperiodo)
                    {
                        case ConstHidrologia.Diario:
                            switch (rbDetalleRpte)
                            {
                                case ConstantesRbDetalleRpte.Diario: //Rpte detallado diario
                                    strHtml = logic.GeneraViewHidrologiaMed1(listaMed1, listaCabeceraPM1, fechaInicio, fechaFin, rbDetalleRpte);
                                    break;
                                case ConstantesRbDetalleRpte.SemanalProgramado:
                                case ConstantesRbDetalleRpte.SemanalCronologico://Rpte detallado semanal programado                                
                                    if (unidad == ConstHidrologia.Caudal)
                                        strHtml = logic.GeneraViewHidrologiaMed1(logic.GenerarListaDetalladaMed1Promedio(idLectura, ConstantesAppServicio.IdOrigenLectura, idsEmpresa, idsCuenca, idsFamilia, fechaInicio, fechaFin, idsPtoMedicion, rbDetalleRpte),
                                            listaCabeceraPM1, fechaInicio, fechaFin, rbDetalleRpte);
                                    else
                                        strHtml = logic.GeneraViewHidrologiaMed1(logic.GenerarListaDetalladaMed1Hmax(idLectura, ConstantesAppServicio.IdOrigenLectura, idsEmpresa, idsCuenca, idsFamilia, fechaInicio, fechaFin, idsPtoMedicion, rbDetalleRpte),
                                            listaCabeceraPM1, fechaInicio, fechaFin, rbDetalleRpte);
                                    break;
                                case ConstantesRbDetalleRpte.Mensual: // Rpte detallado mensual
                                    if (unidad == ConstHidrologia.Caudal)
                                        strHtml = logic.GeneraViewHidrologiaMed1(logic.GenerarListaDetalladaMed1Promedio(idLectura, ConstantesAppServicio.IdOrigenLectura, idsEmpresa, idsCuenca, idsFamilia, fechaInicio, fechaFin, idsPtoMedicion, rbDetalleRpte),
                                            listaCabeceraPM1, fechaInicio, fechaFin, rbDetalleRpte);
                                    else
                                        strHtml = logic.GeneraViewHidrologiaMed1(logic.GenerarListaDetalladaMed1Hmax(idLectura, ConstantesAppServicio.IdOrigenLectura, idsEmpresa, idsCuenca, idsFamilia, fechaInicio, fechaFin, idsPtoMedicion, rbDetalleRpte),
                                            listaCabeceraPM1, fechaInicio, fechaFin, rbDetalleRpte);
                                    break;
                                case ConstantesRbDetalleRpte.Anual: //Rpte detallado anual
                                    if (unidad == ConstHidrologia.Caudal)
                                        strHtml = logic.GeneraViewHidrologiaMed1(logic.GenerarListaMed1PromAnual(idLectura, ConstantesAppServicio.IdOrigenLectura, idsEmpresa, idsCuenca, idsFamilia, fechaInicio, fechaFin, idsPtoMedicion),
                                            listaCabeceraPM1, fechaInicio, fechaFin, rbDetalleRpte);
                                    else
                                        strHtml = logic.GeneraViewHidrologiaMed1(logic.GenerarListaMed1AnualHmaximo(idLectura, ConstantesAppServicio.IdOrigenLectura, idsEmpresa, idsCuenca, idsFamilia, fechaInicio, fechaFin, idsPtoMedicion),
                                            listaCabeceraPM1, fechaInicio, fechaFin, rbDetalleRpte);
                                    break;
                            }
                            break;
                        case ConstHidrologia.SemanalProg:
                        case ConstHidrologia.Semanal:
                            switch (rbDetalleRpte)
                            {

                                case ConstantesRbDetalleRpte.SemanalProgramado: //Rpte detallado semanal programado                                                                    
                                    strHtml = logic.GeneraViewHidrologiaMed1(listaMed1, listaCabeceraPM1, fechaInicio, fechaFin, rbDetalleRpte);
                                    break;
                                case ConstantesRbDetalleRpte.Mensual: // Rpte detallado mensual
                                    if (unidad == ConstHidrologia.Caudal)
                                        strHtml = logic.GeneraViewHidrologiaMed1(logic.GenerarListaDetalladaMed1Promedio(idLectura, ConstantesAppServicio.IdOrigenLectura, idsEmpresa
                                                , idsCuenca, idsFamilia, fechaInicio, fechaFin, idsPtoMedicion, rbDetalleRpte),
                                            listaCabeceraPM1, fechaInicio, fechaFin, rbDetalleRpte);
                                    else
                                        strHtml = logic.GeneraViewHidrologiaMed1(logic.GenerarListaDetalladaMed1Hmax(idLectura, ConstantesAppServicio.IdOrigenLectura, idsEmpresa
                                                , idsCuenca, idsFamilia, fechaInicio, fechaFin, idsPtoMedicion, rbDetalleRpte),
                                            listaCabeceraPM1, fechaInicio, fechaFin, rbDetalleRpte);
                                    break;
                                case ConstantesRbDetalleRpte.Anual: //Rpte detallado anual
                                    if (unidad == ConstHidrologia.Caudal)
                                        strHtml = logic.GeneraViewHidrologiaMed1(logic.GenerarListaMed1PromAnual(idLectura, ConstantesAppServicio.IdOrigenLectura, idsEmpresa, idsCuenca, idsFamilia, fechaInicio, fechaFin, idsPtoMedicion),
                                            listaCabeceraPM1, fechaInicio, fechaFin, rbDetalleRpte);
                                    else
                                        strHtml = logic.GeneraViewHidrologiaMed1(logic.GenerarListaMed1AnualHmaximo(idLectura, ConstantesAppServicio.IdOrigenLectura, idsEmpresa, idsCuenca, idsFamilia, fechaInicio, fechaFin, idsPtoMedicion),
                                            listaCabeceraPM1, fechaInicio, fechaFin, rbDetalleRpte);
                                    break;
                            }
                            break;
                        case ConstHidrologia.Mensual:
                            switch (rbDetalleRpte)
                            {

                                case ConstantesRbDetalleRpte.Mensual: // Rpte detallado mensual
                                    strHtml = logic.GeneraViewHidrologiaMed1(listaMed1, listaCabeceraPM1, fechaInicio, fechaFin, rbDetalleRpte);
                                    break;
                                case ConstantesRbDetalleRpte.Anual: //Rpte detallado anual
                                    if (unidad == ConstHidrologia.Caudal)
                                        strHtml = logic.GeneraViewHidrologiaMed1(logic.GenerarListaMed1PromAnual(idLectura, ConstantesAppServicio.IdOrigenLectura, idsEmpresa, idsCuenca, idsFamilia, fechaInicio, fechaFin, idsPtoMedicion),
                                            listaCabeceraPM1, fechaInicio, fechaFin, rbDetalleRpte);
                                    else
                                        strHtml = logic.GeneraViewHidrologiaMed1(logic.GenerarListaMed1AnualHmaximo(idLectura, ConstantesAppServicio.IdOrigenLectura, idsEmpresa, idsCuenca, idsFamilia, fechaInicio, fechaFin, idsPtoMedicion),
                                            listaCabeceraPM1, fechaInicio, fechaFin, rbDetalleRpte);
                                    break;
                            }
                            break;
                    }
                    break;
            }
            return strHtml;
        }



        /// <summary>
        /// Genera Archivo para reporte general de resolucion horas
        /// </summary>
        /// <param name="fechaInicial"></param>
        /// <param name="fechaFinal"></param>
        /// <returns></returns>
        public JsonResult GenerarArchivoGrafM24(string fechaInicial, string fechaFinal)
        {
            int indicador = 1;
            try
            {
                HidrologiaModel model = new HidrologiaModel();
                model = this.ModelGraficoDiario;
                string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesHidrologia.FolderReporte;
                ExcelDocument.GenerarArchivoGrafM24(model, ruta);
                indicador = 1;
            }
            catch
            {
                indicador = -1;
            }

            return Json(indicador);
        }

        /// <summary>
        /// Genera Archivo para reporte general de resolucion diaria
        /// </summary>
        /// <param name="fechaInicial"></param>
        /// <param name="fechaFinal"></param>
        /// <returns></returns>
        public JsonResult GenerarArchivoGrafM1(string fechaInicial, string fechaFinal)
        {
            int indicador = 1;
            try
            {
                HidrologiaModel model = new HidrologiaModel();
                model = this.ModelGraficoDiario;
                string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesHidrologia.FolderReporte;
                ExcelDocument.GenerarArchivoGrafM1(model, ruta);
                indicador = 1;
            }
            catch
            {
                indicador = -1;
            }

            return Json(indicador);
        }

        #endregion

        #region REPORTE TIEMPO REAL
        /// <summary>
        /// Index para reporte de tiempo real 
        /// </summary>
        /// <returns></returns>
        public ActionResult ReporteTiempoReal()
        {
            HidrologiaModel model = new HidrologiaModel();
            model.LstTipoReporte = HelperHidrologia.ObtenerListaTipoReporte();
            model.ListaEmpresas = logicGeneral.ObtenerEmpresasHidro();
            //model.FechaInicio = DateTime.Now.AddDays(-15).ToString(Constantes.FormatoFecha);
            int[] tipoInfocodis = { 11, 14, 40, 1, 3 };
            model.ListaUnidades = this.logic.ListSiTipoinformacions().Where(x => tipoInfocodis.Contains(x.Tipoinfocodi)).ToList();
            model.FechaInicio = DateTime.Now.ToString(Constantes.FormatoFecha);
            model.FechaFin = DateTime.Now.ToString(Constantes.FormatoFecha);
            return View(model);
        }

        /// <summary>
        /// Genera vista parcial para reporte de lista  de reporte en tiempo real
        /// </summary>
        /// <param name="idsEmpresa"></param>
        /// <param name="tipoReporte"></param>
        /// <param name="fecha"></param>
        /// <param name="fechaFinal"></param>
        /// <param name="idsTipoPtoMed"></param>
        /// <param name="nroPagina"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult ListaTiempoReal(string idsEmpresa, string fecha, string fechaFinal, string idsTipoPtoMed, int nroPagina)
        {
            HidrologiaModel model = new HidrologiaModel();
            DateTime fechaIni = DateTime.MinValue;
            DateTime fechaFin = DateTime.MinValue;
            int tipoReporte = 1;
            fechaIni = DateTime.ParseExact(fecha, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            fechaFin = DateTime.ParseExact(fechaFinal, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            List<MeMedicion24DTO> lista24 = this.logic.ListaMed24HidrologiaTiempoReal(tipoReporte, ConstantesHidrologia.IdOrigenLectura, idsEmpresa,
                fechaIni, fechaFin, idsTipoPtoMed);
            this.ListaFechas = lista24.Select(x => x.Medifecha).Distinct().ToList();
            if (ListaFechas.Count > 0)
            {
                fechaIni = ListaFechas[nroPagina - 1];
            }
            string resultado = this.logic.ObtenerReporteHidrologiaTiempoReal(idsEmpresa, fechaIni, fechaIni, idsTipoPtoMed, tipoReporte);

            model.Resultado = resultado;
            return PartialView(model);
        }

        /// <summary>
        /// /Genera grafico para reporte de tiempo real
        /// </summary>
        /// <param name="idsEmpresa"></param>
        /// <param name="tipoReporte"></param>
        /// <param name="fecha"></param>
        /// <param name="fechaFinal"></param>
        /// <param name="idsTipoPtoMed"></param>
        /// <param name="nroPagina"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GraficoReporteTiempoReal(string idsEmpresa, string fecha, string fechaFinal, string idsTipoPtoMed, int nroPagina)
        {
            HidrologiaModel model = new HidrologiaModel();
            int tipoReporte = 1;

            model = GraficoMed24TiempoReal(idsEmpresa, tipoReporte, fecha, fechaFinal, idsTipoPtoMed, nroPagina);
            var jsonResult = Json(model);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        /// <summary>
        /// Genera model para grafico de tiempo real
        /// </summary>
        /// <param name="idsEmpresa"></param>
        /// <param name="tipoReporte"></param>
        /// <param name="fecha"></param>
        /// <param name="fechaFinal"></param>
        /// <param name="idsTipoPtoMed"></param>
        /// <param name="nroPagina"></param>
        /// <returns></returns>
        public HidrologiaModel GraficoMed24TiempoReal(string idsEmpresa, int tipoReporte, string fecha, string fechaFinal, string idsTipoPtoMed, int nroPagina)
        {
            HidrologiaModel model = new HidrologiaModel();
            GraficoWeb grafico = new GraficoWeb();
            DateTime fechaIni = DateTime.ParseExact(fecha, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            DateTime fechaFin = DateTime.ParseExact(fechaFinal, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            model.Grafico = grafico;
            model.Grafico.Series = new List<RegistroSerie>();
            List<MeReporptomedDTO> listaCabecera = new List<MeReporptomedDTO>();
            List<MeMedicion24DTO> listaM24 = new List<MeMedicion24DTO>();
            listaCabecera = this.logic.ListarEncabezadoMeReporptomeds(tipoReporte, idsEmpresa, idsTipoPtoMed);
            listaM24 = this.logic.ListaMed24HidrologiaTiempoReal(tipoReporte, ConstantesHidrologia.IdOrigenLectura, idsEmpresa, fechaIni, fechaFin, idsTipoPtoMed);

            this.ListaFechas = listaM24.Select(x => x.Medifecha).Distinct().ToList();
            if (ListaFechas.Count > 0)
            {
                fechaIni = ListaFechas[nroPagina - 1];
            }

            if (listaM24.Count > 0)
            {
                model.FechaInicio = fechaIni.ToString(Constantes.FormatoFecha);
                model.Grafico.XAxisTitle = "Dia:" + model.FechaInicio;

                if (tipoReporte == 1)
                    model.Grafico.TitleText = "REPORTE GRÁFICO - TIEMPO REAL";
                else
                    model.Grafico.TitleText = "REPORTE GRÁFICO - HIDROLOGÍA";
                model.SheetName = "GRAFICO";

                model.Grafico.YaxixTitle = "(" + listaM24[0].Tipoinfoabrev + ")";
                // model.Grafico.subtitleText = "";// listaM24[0].Tipoptomedinomb + " - ";
                model.Grafico.XAxisCategories = new List<string>();
                model.Grafico.SeriesName = new List<string>();
                int totalIntervalos = 24;

                //Obtener lista de nombre para la serie, eje x.

                foreach (var reg in listaCabecera)
                {
                    string nombreSerie = string.Empty;
                    if (reg.Famcodi == ConstantesHidrologia.EstacionHidrologica)//si es Estacion Hidrologica
                        nombreSerie = reg.Ptomedibarranomb;
                    else
                        nombreSerie = reg.Equinomb;

                    var registro = new RegistroSerie();
                    registro.Name = nombreSerie;
                    registro.Data = new List<DatosSerie>();
                    registro.Type = "line";
                    model.Grafico.Series.Add(registro);

                }
                // Obtener lista de valores para las series del grafico

                model.Grafico.SeriesData = new decimal?[listaCabecera.Count()][];

                for (var i = 0; i < listaCabecera.Count(); i++)
                {
                    var pto = listaCabecera[i].Ptomedicodi;
                    var lista = listaM24.Where(x => x.Ptomedicodi == pto).OrderBy(x => x.Medifecha);
                    foreach (var reg in lista)
                    {
                        for (var j = 1; j <= totalIntervalos; j++)
                        {
                            decimal? valor = null;
                            DateTime fechaSerie = DateTime.MinValue;
                            valor = (decimal?)reg.GetType().GetProperty("H" + j).GetValue(reg, null);
                            fechaSerie = reg.Medifecha.AddMinutes(j * 60);
                            model.Grafico.Series[i].Data.Add(new DatosSerie()
                            {
                                Y = valor,
                                X = fechaSerie
                            });

                        }
                    }
                }
                ModelGraficoTR = model;
            }// end del if
            return model;
        }

        /// <summary>
        /// Permite mostrar el paginado de la consulta para los reportes de tiempo real
        /// </summary>
        /// <param name="modelo"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult PaginadoTiempoReal(string idsEmpresa, string fecha, string fechaFinal, string idsTipoPtoMed)
        {
            HidrologiaModel model = new HidrologiaModel();
            int tipoReporte = 1;
            model.IndicadorPagina = false;
            if (idsTipoPtoMed == null)
                idsTipoPtoMed = "-1";
            //var formato = logic.GetByIdMeFormato(idTipoInformacion);
            //formato.ListaHoja = logic.GetByCriteriaMeFormatohojas(idTipoInformacion);
            DateTime fechaInicio = DateTime.Now;
            DateTime fechaFin = DateTime.Now;

            if (fecha != null)
            {
                fechaInicio = DateTime.ParseExact(fecha, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            }
            if (fechaFinal != null)
            {
                fechaFin = DateTime.ParseExact(fechaFinal, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            }
            //fechaFin = fechaFin.AddDays(1);

            int nroRegistros = this.logic.ObtenerNroFilasHidrologiaTiempoReal(tipoReporte, ConstantesHidrologia.IdOrigenLectura, idsEmpresa, fechaInicio, fechaFin, idsTipoPtoMed);

            if (nroRegistros > 0)
            {
                model.NroPaginas = nroRegistros;
                model.NroMostrar = Constantes.NroPageShow;
                model.IndicadorPagina = true;
            }

            return PartialView(model);
        }

        /// <summary>
        /// Genera archivo para reporte de teimpo real
        /// </summary>
        /// <param name="idsEmpresa"></param>
        /// <param name="tipoReporte"></param>
        /// <param name="fecha"></param>
        /// <param name="fechaFinal"></param>
        /// <param name="idsTipoPtoMed"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GenerarArchivoReporteTR(string idsEmpresa, string fecha, string fechaFinal, string idsTipoPtoMed)
        {
            int indicador = 1;
            int tipoReporte = 1;
            DateTime fechaIni = DateTime.MinValue;
            DateTime fechaFin = DateTime.MinValue;
            fechaIni = DateTime.ParseExact(fecha, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            fechaFin = DateTime.ParseExact(fechaFinal, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            HidrologiaModel model = new HidrologiaModel();
            List<MeMedicion24DTO> lista24 = new List<MeMedicion24DTO>();
            List<MeReporptomedDTO> listaCabecera = new List<MeReporptomedDTO>();
            lista24 = this.logic.ListaMed24HidrologiaTiempoReal(tipoReporte, ConstantesHidrologia.IdOrigenLectura, idsEmpresa, fechaIni, fechaFin, idsTipoPtoMed);
            listaCabecera = this.logic.ListarEncabezadoMeReporptomeds(tipoReporte, idsEmpresa, idsTipoPtoMed);
            model.FechaInicio = fechaIni.ToString(Constantes.FormatoFecha);
            model.FechaFin = fechaFin.ToString(Constantes.FormatoFecha);
            model.FechaRepInicio = fechaIni.ToString(Constantes.FormatoF);
            model.FechaRepFinal = fechaFin.ToString(Constantes.FormatoF);

            model.ListaMedicion24horas = lista24;
            string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesHidrologia.FolderReporte;

            try
            {
                switch (tipoReporte)
                {
                    case 1://(Formato SCO
                        model.TituloReporteXLS = "REPORTE TIEMPO REAL";
                        model.SheetName = "CAUDALES Y VOL";
                        indicador = 1;
                        break;
                    case 2://Volúmenes
                        model.TituloReporteXLS = "REPORTE DE HIDROLOGÍA";
                        model.SheetName = "VOLUMENES";
                        indicador = 1;
                        break;

                }
                ExcelDocument.GenerarArchivoHidrologiaM24TR(model, listaCabecera, tipoReporte, ruta);
            }
            catch
            {
                indicador = -1;
            }

            return Json(indicador);
        }

        /// <summary>
        /// Genera Archivo para reporte de tiempo real
        /// </summary>
        /// <returns></returns>
        public JsonResult GenerarArchivoGrafTR()
        {
            int indicador = 1;
            try
            {
                HidrologiaModel model = new HidrologiaModel();
                model = ModelGraficoTR;
                string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesHidrologia.FolderReporte;
                ExcelDocument.GenerarArchivoGrafTR(model, ruta);
                indicador = 1;
            }
            catch
            {
                indicador = -1;
            }

            return Json(indicador);
        }


        #endregion

        #region REPORTE CAUDAL VOLUMEN
        /// <summary>
        /// Index para Reporte de Caudales y Volumen del tipo2
        /// </summary>
        /// <returns></returns>
        public ActionResult ReporteCaudalVol()
        {
            HidrologiaModel model = new HidrologiaModel();
            model.ListaEmpresas = logicGeneral.ObtenerEmpresasHidro();
            model.FechaInicio = DateTime.Now.ToString(Constantes.FormatoFecha);
            int?[] tipoInfocodis = { 11, 14 };
            model.ListaUnidades = this.logic.ListSiTipoinformacions().Where(x => tipoInfocodis.Contains(x.Tipoinfocodi)).ToList();
            model.ListaLectura = this.logic.ListMeLecturas().Where(x => x.Origlectcodi == ConstantesHidrologia.IdOrigenLectura).ToList();
            model.ListaCuenca = this.logic.ListarEquiposXFamilia(ConstantesHidrologia.IdCuenca);
            model.ListaTipoPtoMedicion = this.logic.ListMeTipopuntomedicions(ConstantesHidrologia.IdOrigenLectura.ToString());
            if (model.ListaLectura.Count > 0)
                model.IdLectura = model.ListaLectura[0].Lectcodi;

            return View(model);

        }

        /// <summary>
        /// Genera vista parcial para reporte de lista  de reporte Caudales y Volumen del tipo2
        /// </summary>
        /// <param name="idsEmpresa"></param>
        /// <param name="idsCuenca"></param>
        /// <param name="idsFamilia"></param>
        /// <param name="idsPtoMedicion"></param>
        /// <param name="unidad"></param>
        /// <param name="idLectura"></param>
        /// <param name="fecha"></param>
        /// <param name="anho"></param>
        /// <param name="anhoInicial"></param>
        /// <param name="anhoFinal"></param>
        /// <param name="semana"></param>
        /// <param name="rbDetalleRpte"></param>
        /// <returns></returns>
        public PartialViewResult ListaQnVolTipo2(string idsEmpresa, string idsCuenca, string idsFamilia, string idsPtoMedicion, int unidad, int idLectura, string fecha, string anho, string anhoInicial, string anhoFinal,
                                                int semana, int rbDetalleRpte)
        {//int rbDetalleRpte =  1: Mensual (Qn/Vol), 2:Semanal (Qn/Vol), 3:Anual (Volumen)
            HidrologiaModel model = new HidrologiaModel();
            DateTime fechaIni = DateTime.MinValue;
            DateTime fechaFin = DateTime.MinValue;
            //List<MeMedicion24DTO> listaM24 = new List<MeMedicion24DTO>();
            //List<MeMedicion1DTO> listaM1 = new List<MeMedicion1DTO>();
            idsFamilia = "-1";
            switch (rbDetalleRpte)
            {
                case ConstantesrbTipoRpteQnVol.Semanal: // Semanal
                    int ianho = Int32.Parse(anho);
                    fechaIni = COES.Base.Tools.Util.GenerarFecha(ianho, semana, ConstantesHidrologia.SemanalProgramado);
                    fechaFin = fechaIni.AddDays(6);
                    break;
                case ConstantesrbTipoRpteQnVol.Mensual: //Mensual
                    fechaIni = COES.Base.Tools.Util.FormatFecha(fecha);
                    fechaFin = fechaIni.AddMonths(1);
                    fechaFin = fechaFin.AddDays(-1);
                    break;
                case ConstantesrbTipoRpteQnVol.Anual: //Anual
                    int ianhoIni = Int32.Parse(anhoInicial);
                    int ianhoFin = Int32.Parse(anhoFinal);
                    fechaIni = new DateTime(ianhoIni, 1, 1);
                    fechaFin = new DateTime(ianhoFin, 12, 31);
                    break;
            }

            string resultado = this.logic.ObtenerReporteHidrologiaQnVolTipo2(idsEmpresa, fechaIni, fechaFin, rbDetalleRpte, unidad,
                idLectura, idsCuenca, idsFamilia, idsPtoMedicion);
            model.Resultado = resultado;
            return PartialView(model);
        }


        #endregion

        #region DESCARGA DE LAGUNAS Y VERTIMIENTOS DE EMBALSES
        /// <summary>
        /// Index para Reporte Descarga de Lagunas y Vertimientos 
        /// </summary>
        /// <returns></returns>
        public ActionResult DescargaLagVert()
        {
            HidrologiaModel model = new HidrologiaModel();
            model.ListaEmpresas = logicGeneral.ObtenerEmpresasHidro();
            model.FechaInicio = DateTime.Now.AddDays(-15).ToString(Constantes.FormatoFecha);
            int[] tipoInfocodis = { 11, 14, 40 };
            //model.FechaInicio = DateTime.Now.ToString(Constantes.FormatoFecha);
            model.FechaFin = DateTime.Now.ToString(Constantes.FormatoFecha);
            return View(model);
        }

        /// <summary>
        /// Permite mostrar el paginado de la consulta para los reportes  de Descarga de Lagunas y Vertimientos de Embalses
        /// </summary>
        /// <param name="idsEmpresa"></param>
        /// <param name="tipoReporte"></param>
        /// <param name="fecha"></param>
        /// <param name="fechaFinal"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult PaginadoDescargaVert(string idsEmpresa, int tipoReporte, string fecha, string fechaFinal)
        {
            HidrologiaModel model = new HidrologiaModel();
            model.IndicadorPagina = false;
            int formato = 0;
            DateTime fechaInicio = DateTime.Now;
            DateTime fechaFin = DateTime.Now;

            if (fecha != null)
            {
                fechaInicio = DateTime.ParseExact(fecha, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            }
            if (fechaFinal != null)
            {
                fechaFin = DateTime.ParseExact(fechaFinal, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            }
            switch (tipoReporte)
            {
                case 1: //Descarga de Lagunas
                    formato = ConstantesHidrologia.IdFormatoDescargaLaguna;
                    break;
                case 2: //Vertimiento de Embalses
                    formato = ConstantesHidrologia.IdFormatoVertimiento;
                    break;
            }
            int nroRegistros = 0;
            nroRegistros = this.logic.ObtenerNroFilasDescargVert(formato, idsEmpresa, fechaInicio, fechaFin);
            if (nroRegistros > 0)
            {
                int pageSize = Constantes.PageSize;
                int nroPaginas = (nroRegistros % pageSize == 0) ? nroRegistros / pageSize : nroRegistros / pageSize + 1;
                model.NroPaginas = nroPaginas;
                model.NroMostrar = Constantes.PageSize;
                model.IndicadorPagina = true;
            }

            return PartialView(model);
        }


        /// <summary>
        /// Genera vista del reporte de de Descarga de Lagunas ó Vertimientos de Embalses
        /// </summary>
        /// <param name="idsEmpresa"></param>
        /// <param name="tipoReporte"></param>
        /// <param name="fecha"></param>
        /// <param name="fechaFinal"></param>
        /// <param name="nroPagina"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult ListaDescargaVertimiento(string idsEmpresa, int tipoReporte, string fecha, string fechaFinal, int nroPagina)
        {
            HidrologiaModel model = new HidrologiaModel();
            DateTime fechaIni = DateTime.MinValue;
            DateTime fechaFin = DateTime.MinValue;
            int formato = 0;
            fechaIni = DateTime.ParseExact(fecha, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            fechaFin = DateTime.ParseExact(fechaFinal, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            List<MeMedicionxintervaloDTO> lista = new List<MeMedicionxintervaloDTO>();
            switch (tipoReporte)
            {
                case 1:
                    formato = ConstantesHidrologia.IdFormatoDescargaLaguna;
                    break;
                case 2:
                    formato = ConstantesHidrologia.IdFormatoVertimiento;
                    break;
            }

            string resultado = this.logic.ObtenerReporteDescargaVertimiento(formato, idsEmpresa, fechaIni, fechaFin, nroPagina, Constantes.PageSize);

            model.Resultado = resultado;
            return PartialView(model);
        }

        /// <summary>
        /// Genera archivo excel para reporte de Descarga de Lagunas y Vertimiento de Embalses
        /// </summary>
        /// <param name="idsEmpresa"></param>
        /// <param name="tipoReporte"></param>
        /// <param name="fecha"></param>
        /// <param name="fechaFinal"></param>        
        /// <returns></returns>
        [HttpPost]
        public JsonResult GenerarArchivoReporteDescargaVert(string idsEmpresa, int tipoReporte, string fecha, string fechaFinal)
        {
            int indicador = 1;
            DateTime fechaIni = DateTime.MinValue;
            DateTime fechaFin = DateTime.MinValue;
            fechaIni = DateTime.ParseExact(fecha, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            fechaFin = DateTime.ParseExact(fechaFinal, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            HidrologiaModel model = new HidrologiaModel();
            List<MeMedicionxintervaloDTO> lista = new List<MeMedicionxintervaloDTO>();
            model.FechaInicio = fechaIni.ToString(Constantes.FormatoFecha);
            model.FechaFin = fechaFin.ToString(Constantes.FormatoFecha);
            string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesHidrologia.FolderReporte;
            try
            {
                switch (tipoReporte)
                {
                    case 1://Descarga de Lagunas
                        lista = this.logic.ListaMedIntervaloDescargaVert(ConstantesHidrologia.IdFormatoDescargaLaguna, idsEmpresa, fechaIni, fechaFin);
                        model.TituloReporteXLS = "REPORTE DESCARGA DE LAGUNAS";
                        model.SheetName = "DESCARGA";
                        indicador = 1;
                        break;
                    case 2://Vertimiento de Embalses
                        lista = this.logic.ListaMedIntervaloDescargaVert(ConstantesHidrologia.IdFormatoVertimiento, idsEmpresa, fechaIni, fechaFin);
                        model.TituloReporteXLS = "REPORTE VERTIMIENTO DE EMBALSES";
                        model.SheetName = "VERTIMIENTO";
                        indicador = 1;
                        break;
                }
                model.ListaDescargaVert = lista;
                ExcelDocument.GenerarArchivoHidrologiaDescargaVert(model, tipoReporte, ruta);
            }
            catch
            {
                indicador = -1;
            }

            return Json(indicador);
        }


        #endregion

        #region Recursos Hidricos Ejecutados

        public ActionResult RecHidEjecutados()
        {
            HidrologiaModel model = new HidrologiaModel();
            model.LstTipoReporte = HelperHidrologia.ObtenerListaTipoReporte();
            model.ListaEmpresas = logicGeneral.ObtenerEmpresasHidro();
            //model.FechaInicio = DateTime.Now.AddDays(-15).ToString(Constantes.FormatoFecha);
            int[] tipoInfocodis = { 11, 14 };
            model.ListaUnidades = this.logic.ListSiTipoinformacions().Where(x => tipoInfocodis.Contains(x.Tipoinfocodi)).ToList();

            model.FechaInicio = DateTime.Now.ToString(Constantes.FormatoFecha);
            model.FechaFin = DateTime.Now.ToString(Constantes.FormatoFecha);
            return View(model);
        }

        /// <summary>
        /// Genera vista parcial para reporte de lista  de reporte en tiempo real
        /// </summary>
        /// <param name="idsEmpresa"></param>
        /// <param name="tipoReporte"></param>
        /// <param name="fecha"></param>
        /// <param name="fechaFinal"></param>
        /// <param name="idsTipoPtoMed"></param>
        /// <param name="nroPagina"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult ListaHidEjecutados(string idsEmpresa, int tipoReporte, string fecha, string fechaFinal, string idsTipoPtoMed, int nroPagina)
        {
            HidrologiaModel model = new HidrologiaModel();
            DateTime fechaIni = DateTime.MinValue;
            DateTime fechaFin = DateTime.MinValue;
            fechaIni = DateTime.ParseExact(fecha, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            fechaFin = DateTime.ParseExact(fechaFinal, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            List<MeMedicion24DTO> lista24 = this.logic.ListaMed24HidrologiaTiempoReal(tipoReporte, ConstantesHidrologia.IdOrigenLectura, idsEmpresa,
                fechaIni, fechaFin, idsTipoPtoMed);
            this.ListaFechas = lista24.Select(x => x.Medifecha).Distinct().ToList();
            if (ListaFechas.Count > 0)
            {
                fechaIni = ListaFechas[nroPagina - 1];
            }
            string resultado = this.logic.ObtenerReporteHidrologiaTiempoReal(idsEmpresa, fechaIni, fechaIni, idsTipoPtoMed, tipoReporte);

            model.Resultado = resultado;
            return PartialView(model);
        }

        #endregion

        #region REPORTE DE PRONÓSTICOS HIDRÓLOGICOS SEMANAL Y DIARIO
        public ActionResult ReportePronostico()
        {
            HidrologiaModel model = new HidrologiaModel();

            model.Anho = DateTime.Now.Year.ToString();
            model.NroSemana = EPDate.f_numerosemana(DateTime.Now);
            model.Dia = DateTime.Now.ToString(Constantes.FormatoFecha);

            return View(model);
        }

        //inicio agregado
        [HttpPost]
        public PartialViewResult CargarListaPronostico(int tipoReporte, string fecha, string semana)
        {
            HidrologiaModel model = new HidrologiaModel();

            GenerarFormatoPronostico(model, tipoReporte, fecha, semana);

            List<MeMedicion1DTO> listaDataPronostico = logic.ListaPronosticoHidrologia(model.idReportePronostico,
                model.FechainiPronostico, model.FechafinPronostico);
            List<MeMedicion24DTO> listaDataHistorico = logic.ListaHistoricoHidrologia(ConstantesRptePronostico.IdReporteCodiHistorico,
                model.FechainiHistorico, model.FechafinHistorico);

            model.ListaMedicion1 = listaDataPronostico;
            model.ListaMedicion24 = listaDataHistorico;

            model.HtmlReportePronostico = ListaReportePronosticoHidrologia(listaDataPronostico, model.ListaFechaPronostico, model.TituloReportePronostico);
            model.HtmlReporteHistorico = ListaReporteHistoricoHidrologia(listaDataHistorico, model.ListaFechaHistorico, model.TituloReporteHistorico);

            return PartialView(model);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ptoCalculadocodi"></param>
        /// <param name="fecha"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult ViewDetalleFaltante(int tipoReporte, int ptoCalculadocodi, string fecha)
        {
            HidrologiaModel model = new HidrologiaModel();
            DateTime dfecha = DateTime.ParseExact(fecha, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            int idReportePronostico = ConstantesRptePronostico.IdReporteCodiPronosticoSemanal;
            if (ConstantesRptePronostico.TipoReporteDiario == tipoReporte)
            {
                idReportePronostico = ConstantesRptePronostico.IdReporteCodiPronosticoDiario;
            }
            //obtener data de pronóstico
            List<MeMedicion1DTO> data = logic.ListaPronosticoHidrologiaByPtoCalculadoAndFecha(idReportePronostico,
                   ptoCalculadocodi, dfecha);

            var empresaDesc = data.Select(x => x.Emprnomb).FirstOrDefault();
            var ubicacionDesc = data.Select(x => x.Ubicaciondesc).FirstOrDefault();
            var ptomedicionDesc = data.Select(x => x.CalculadoPtomedidesc).FirstOrDefault(); ;

            model.ListaMedicion1 = data;
            model.EmpresaDesc = empresaDesc;
            model.UbicacionDesc = ubicacionDesc;
            model.PuntoCalculadoDesc = ptomedicionDesc;

            return PartialView(model);
        }

        [HttpPost]
        public JsonResult ExportarExcel(int tipoReporte, string fecha, string semana)
        {
            string ruta = string.Empty;
            string[] datos = new string[2];
            try
            {
                HidrologiaModel model = new HidrologiaModel();
                GenerarFormatoPronostico(model, tipoReporte, fecha, semana);

                List<MeMedicion1DTO> listaDataPronostico = logic.ListaPronosticoHidrologia(model.idReportePronostico,
                    model.FechainiPronostico, model.FechafinPronostico);
                List<MeMedicion24DTO> listaDataHistorico = logic.ListaHistoricoHidrologia(ConstantesRptePronostico.IdReporteCodiHistorico,
                    model.FechainiHistorico, model.FechafinHistorico);
                model.ListaMedicion1 = listaDataPronostico;
                model.ListaMedicion24 = listaDataHistorico;

                ruta = GenerarFileExcelReportePronosticoHistorico(model);
                datos[0] = ruta;
                datos[1] = model.NombreArchivo;
            }
            catch
            {
                datos[0] = "-1";
                datos[1] = "";
            }
            var jsonResult = Json(datos);
            return jsonResult;
        }
        //fin agregado

        /// <summary>
        /// Permite descargar el formato de carga
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public virtual ActionResult DescargarExcelPronostico()
        {
            string strArchivoTemporal = Request["archivo"];
            string strArchivoNombre = Request["nombre"];
            byte[] buffer = null;

            if (System.IO.File.Exists(strArchivoTemporal))
            {
                buffer = System.IO.File.ReadAllBytes(strArchivoTemporal);
                System.IO.File.Delete(strArchivoTemporal);
            }

            string strNombreArchivo = string.Format("{0}.xlsx", strArchivoNombre);

            return File(buffer, System.Net.Mime.MediaTypeNames.Application.Octet, strNombreArchivo);
        }

        //inicio modificado 
        /// <summary>
        /// Genera Archivo excel y devuelve la ruta mas el nombre del archivo
        /// </summary>
        private string GenerarFileExcelReportePronosticoHistorico(HidrologiaModel model)
        {
            string fileExcel = string.Empty;

            using (ExcelPackage xlPackage = new ExcelPackage())
            {
                ExcelWorksheet wsPron = xlPackage.Workbook.Worksheets.Add(model.HojaPronostico);
                var data = model.ListaMedicion1;

                #region Hoja pronóstico
                int row = 6;
                int column = 1;

                //Imprimir filtro
                wsPron.Cells[row, column].Value = model.FiltroPronosticoDesc + ":";
                wsPron.Cells[row, column].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                wsPron.Cells[row, column + 1].Value = model.FiltroPronosticoValor;
                wsPron.Cells[row, column + 1].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                ///Imprimimos cabecera de puntos de medicion
                //titulo
                row += 2;
                int cantDia = model.ListaFechaPronostico.Count;
                var filIniTitulo = row;
                var colIniTitulo = 1;
                var colFinTitulo = colIniTitulo + cantDia;
                int totalFilas = colIniTitulo + model.ListaFechaPronostico.Count + 2;
                wsPron.Cells[filIniTitulo, colIniTitulo].Value = model.TituloReportePronostico;
                wsPron.Cells[row, colIniTitulo, row, totalFilas].Merge = true;
                wsPron.Cells[row, colIniTitulo, row, totalFilas].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                wsPron.Cells[row, colIniTitulo, row, totalFilas].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                wsPron.Cells[row, colIniTitulo, row, totalFilas].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                //empresa
                row++;
                wsPron.Cells[row, colIniTitulo].Value = "EMPRESA";
                wsPron.Cells[row, colIniTitulo, row + 1, colIniTitulo].Merge = true;
                wsPron.Cells[row, colIniTitulo, row + 1, colIniTitulo].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                wsPron.Cells[row, colIniTitulo, row + 1, colIniTitulo].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                wsPron.Cells[row, colIniTitulo, row + 1, colIniTitulo].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                wsPron.Cells[row, colIniTitulo + 1].Value = "UBICACIÓN";
                wsPron.Cells[row, colIniTitulo + 1, row + 1, colIniTitulo + 1].Merge = true;
                wsPron.Cells[row, colIniTitulo + 1, row + 1, colIniTitulo + 1].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                wsPron.Cells[row, colIniTitulo + 1, row + 1, colIniTitulo + 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                wsPron.Cells[row, colIniTitulo + 1, row + 1, colIniTitulo + 1].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                wsPron.Cells[row, colIniTitulo + 2].Value = "DESCRIPCIÓN";
                wsPron.Cells[row, colIniTitulo + 2, row + 1, colIniTitulo + 2].Merge = true;
                wsPron.Cells[row, colIniTitulo + 2, row + 1, colIniTitulo + 2].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                wsPron.Cells[row, colIniTitulo + 2, row + 1, colIniTitulo + 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                wsPron.Cells[row, colIniTitulo + 2, row + 1, colIniTitulo + 2].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                var iniColFecha = 0;
                foreach (var f in model.ListaFechaPronostico)
                {
                    wsPron.Cells[row, colIniTitulo + 3 + iniColFecha].Value = f.ToString(ConstantesAppServicio.FormatoFecha);
                    wsPron.Cells[row, colIniTitulo + 3 + iniColFecha].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    wsPron.Cells[row, colIniTitulo + 3 + iniColFecha].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    wsPron.Cells[row, colIniTitulo + 3 + iniColFecha].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    wsPron.Cells[row + 1, colIniTitulo + 3 + iniColFecha].Value = "m3/s";
                    wsPron.Cells[row + 1, colIniTitulo + 3 + iniColFecha].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    wsPron.Cells[row + 1, colIniTitulo + 3 + iniColFecha].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    wsPron.Cells[row + 1, colIniTitulo + 3 + iniColFecha].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    iniColFecha++;
                }

                using (var range = wsPron.Cells[filIniTitulo, colIniTitulo, filIniTitulo + 2, colIniTitulo + iniColFecha + 2])
                {
                    range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                    range.Style.Font.Color.SetColor(Color.White);
                }


                /// **************  CUERPO DE LA TABLA **************//       
                row += 2;
                var listaEmpresa = data.Select(y => new { y.Emprcodi, y.Emprnomb }).Distinct().ToList().OrderBy(c => c.Emprnomb);
                var listaEmprcodi = listaEmpresa.Select(x => x.Emprcodi).ToList();
                foreach (var empcodi in listaEmprcodi)
                {
                    //empresa
                    string nomEmpresa = data.Where(x => x.Emprcodi == empcodi).FirstOrDefault().Emprnomb;
                    int cantPtoPorEmpresa = data.Where(x => x.Emprcodi == empcodi).Select(y => y.CalculadoPtomedicodi).Distinct().Count();
                    wsPron.Cells[row, colIniTitulo].Value = nomEmpresa;
                    wsPron.Cells[row, colIniTitulo, row + cantPtoPorEmpresa - 1, colIniTitulo].Merge = true;
                    wsPron.Cells[row, colIniTitulo, row + cantPtoPorEmpresa - 1, colIniTitulo].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    //wsPron.Cells[row, colIniTitulo, row + cantPtoPorEmpresa - 1, colIniTitulo].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    wsPron.Cells[row, colIniTitulo, row + cantPtoPorEmpresa - 1, colIniTitulo].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    //ubicacion
                    var listaUbicacion = data.Where(x => x.Emprcodi == empcodi).Select(y => new { y.Ubicacioncodi, y.Ubicaciondesc }).Distinct().ToList().OrderBy(c => c.Ubicaciondesc);
                    var listaUbicacioncodi = listaUbicacion.Select(x => x.Ubicacioncodi).ToList();
                    for (int u = 0; u < listaUbicacioncodi.Count; u++)
                    {
                        if (u != 0)
                        {
                            row++;
                        }

                        var ubicacioncodi = listaUbicacioncodi[u];
                        var nomUbicacion = data.Where(x => x.Emprcodi == empcodi && x.Ubicacioncodi == ubicacioncodi).FirstOrDefault().Ubicaciondesc;
                        int cantPtoPorUbicacion = data.Where(x => x.Emprcodi == empcodi && x.Ubicacioncodi == ubicacioncodi).Select(y => y.CalculadoPtomedicodi).Distinct().Count();

                        wsPron.Cells[row, colIniTitulo + 1].Value = nomUbicacion;
                        wsPron.Cells[row, colIniTitulo + 1, row + cantPtoPorUbicacion - 1, colIniTitulo + 1].Merge = true;
                        wsPron.Cells[row, colIniTitulo + 1, row + cantPtoPorUbicacion - 1, colIniTitulo + 1].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        //wsPron.Cells[row, colIniTitulo + 1, row + cantPtoPorUbicacion - 1, colIniTitulo + 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        wsPron.Cells[row, colIniTitulo + 1, row + cantPtoPorUbicacion - 1, colIniTitulo + 1].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        //Puntos calculados
                        var listaPtoCalculado = data.Where(x => x.Emprcodi == empcodi && x.Ubicacioncodi == ubicacioncodi).Select(y => new { y.CalculadoPtomedicodi, y.CalculadoPtomedidesc }).Distinct().ToList().OrderBy(c => c.CalculadoPtomedidesc);
                        var listaPtoCalculadocodi = listaPtoCalculado.Select(y => y.CalculadoPtomedicodi).ToList();
                        for (int pc = 0; pc < listaPtoCalculadocodi.Count; pc++)
                        {
                            if (pc != 0)
                            {
                                row++;
                            }
                            var calculadoptocodi = listaPtoCalculadocodi[pc];
                            var ptoCalculado = data.Where(x => x.CalculadoPtomedicodi == calculadoptocodi).FirstOrDefault();
                            var nomPtoCalculado = ptoCalculado.CalculadoPtomedidesc;
                            var cantPtoPorCalculado = data.Where(x => x.CalculadoPtomedicodi == calculadoptocodi).Select(y => y.OrigenPtomedicodi).Distinct().Count();
                            var tipoRelacion = ptoCalculado.TipoRelacioncodi;

                            wsPron.Cells[row, colIniTitulo + 2].Value = nomPtoCalculado;
                            wsPron.Cells[row, colIniTitulo + 2, row, colIniTitulo + 2].Merge = true;
                            wsPron.Cells[row, colIniTitulo + 2, row, colIniTitulo + 2].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                            var iniColfecha = 0;
                            foreach (var f in model.ListaFechaPronostico)
                            {
                                //obtener puntos de medicion
                                var listaPto = data.Where(x => x.CalculadoPtomedicodi == calculadoptocodi && x.Medifecha.Date == f.Date).ToList();
                                decimal? resultado = 0;
                                bool dataCompleta = true;

                                if (listaPto.Count > 0)
                                {
                                    foreach (var pto in listaPto)
                                    {
                                        if (pto.H1 == null)
                                        {
                                            dataCompleta = false;
                                        }
                                        var factorCalculado = pto.CalculadoFactor;
                                        resultado += pto.H1 * factorCalculado;
                                    }

                                    wsPron.Cells[row, colIniTitulo + 3 + iniColfecha].Value = resultado;
                                }

                                if (listaPto.Count == 0 || cantPtoPorCalculado != listaPto.Count)
                                {
                                    dataCompleta = false;
                                }

                                if (!dataCompleta)
                                {
                                    wsPron.Cells[row, colIniTitulo + 3 + iniColfecha].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                    wsPron.Cells[row, colIniTitulo + 3 + iniColfecha].Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#ffa500"));
                                }

                                wsPron.Cells[row, colIniTitulo + 3 + iniColfecha].Style.Numberformat.Format = @"0.000";
                                wsPron.Cells[row, colIniTitulo + 3 + iniColfecha].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                                iniColfecha++;
                            }
                        }
                    }
                    row++;
                }

                row++;
                //leyenda
                wsPron.Cells[row, 1].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                wsPron.Cells[row, 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
                wsPron.Cells[row, 1].Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#ffa500"));

                wsPron.Cells[row, 2].Value = ConstantesRptePronostico.LeyendaDataFaltante;

                //imagen
                HttpWebRequest request = (HttpWebRequest)System.Net.HttpWebRequest.Create(ConfigurationManager.AppSettings["LogoCoes"].ToString());
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                System.Drawing.Image img = System.Drawing.Image.FromStream(response.GetResponseStream());
                ExcelPicture picture = wsPron.Drawings.AddPicture("Imagen", img);
                picture.From.Column = 0;
                picture.From.Row = 1;
                picture.To.Column = 1;
                picture.To.Row = 2;
                picture.SetSize(120, 60);

                #endregion
                wsPron.Cells.AutoFitColumns();
                wsPron.Column(1).Width = 35;
                wsPron.Column(2).Width = 20;
                wsPron.Column(3).Width = 60;

                ExcelWorksheet wsHist = xlPackage.Workbook.Worksheets.Add(model.HojaHistorico);
                var data24 = model.ListaMedicion24;
                #region Hoja histórico
                row = 6;
                column = 1;

                //Imprimir filtro
                wsHist.Cells[row, column].Value = model.FiltroHistoricoDesc + ":";
                wsHist.Cells[row, column].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                wsHist.Cells[row, column + 1].Value = model.FiltroHistoricoValor;
                wsHist.Cells[row, column + 1].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                //********* CABECERA DE LA TABLA *******//
                //datos
                var listaEmpcodi = data24.Select(x => x.Emprcodi).Distinct().ToList();
                int cantPto = data24.Select(x => x.Ptomedicodi).Distinct().Count();
                cantPto = cantPto == 0 ? 1 : cantPto;
                int numHoras = 24 * model.ListaFechaHistorico.Count;

                string[][] listaData = new string[numHoras][];
                for (int i = 0; i < listaData.Count(); i++)
                {
                    listaData[i] = new string[cantPto];
                }
                int posHora = 0;
                int posPtomedi = 0;

                string[] listaHoraFormato = new string[numHoras];

                for (int i = 0; i < model.ListaFechaHistorico.Count(); i++)
                {
                    DateTime f = model.ListaFechaHistorico[i].Date;
                    for (int j = 0; j < 24; j++)
                    {
                        DateTime horaFila = f.AddHours(j);
                        listaHoraFormato[posHora] = horaFila.ToString(ConstantesAppServicio.FormatoFechaFull);
                        posHora++;
                    }
                }

                //fecha
                row += 2;
                colIniTitulo = 1;
                wsHist.Cells[row, colIniTitulo].Value = "Fecha";
                wsHist.Cells[row, colIniTitulo, row + 5, colIniTitulo].Merge = true;
                wsHist.Cells[row, colIniTitulo, row + 5, colIniTitulo].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                wsHist.Cells[row, colIniTitulo, row + 5, colIniTitulo].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                wsHist.Cells[row, colIniTitulo, row + 5, colIniTitulo].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                wsHist.Cells[row, colIniTitulo, row + 5, colIniTitulo].Style.Fill.PatternType = ExcelFillStyle.Solid;
                wsHist.Cells[row, colIniTitulo, row + 5, colIniTitulo].Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));

                wsHist.Cells[row, colIniTitulo + 1].Value = model.TituloReporteHistorico;
                wsHist.Cells[row, colIniTitulo + 1, row, colIniTitulo + 1 + cantPto - 1].Merge = true;
                wsHist.Cells[row, colIniTitulo + 1, row, colIniTitulo + 1 + cantPto - 1].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                wsHist.Cells[row, colIniTitulo + 1, row, colIniTitulo + 1 + cantPto - 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                wsHist.Cells[row, colIniTitulo + 1, row, colIniTitulo + 1 + cantPto - 1].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                wsHist.Cells[row, colIniTitulo + 1, row, colIniTitulo + 1 + cantPto - 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
                wsHist.Cells[row, colIniTitulo + 1, row, colIniTitulo + 1 + cantPto - 1].Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));

                row++;
                //empresas
                var colEmpresa = colIniTitulo + 1;
                var colGrupo = colIniTitulo + 1;
                var colEquipo = colIniTitulo + 1;
                foreach (var empcodi in listaEmpcodi)
                {
                    var empnomb = data24.Where(x => x.Emprcodi == empcodi).FirstOrDefault().Emprnomb;
                    int cantPtoByEmp = data24.Where(x => x.Emprcodi == empcodi).Select(y => y.Ptomedicodi).Distinct().Count();
                    wsHist.Cells[row, colEmpresa].Value = empnomb;
                    wsHist.Cells[row, colEmpresa, row, colEmpresa + cantPtoByEmp - 1].Merge = true;
                    wsHist.Cells[row, colEmpresa, row, colEmpresa + cantPtoByEmp - 1].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    wsHist.Cells[row, colEmpresa, row, colEmpresa + cantPtoByEmp - 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    wsHist.Cells[row, colEmpresa, row, colEmpresa + cantPtoByEmp - 1].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    wsHist.Cells[row, colEmpresa, row, colEmpresa + cantPtoByEmp - 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    wsHist.Cells[row, colEmpresa, row, colEmpresa + cantPtoByEmp - 1].Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));

                    colEmpresa += cantPtoByEmp;

                    //grupos x empresa
                    var listaGrupocodi = data24.Where(x => x.Emprcodi == empcodi).Select(y => y.Grupocodi).Distinct().ToList();
                    foreach (var grupocodi in listaGrupocodi)
                    {
                        var gruponomb = data24.Where(x => x.Grupocodi == grupocodi).FirstOrDefault().Gruponomb;
                        int cantPtoByGrupo = data24.Where(x => x.Emprcodi == empcodi && x.Grupocodi == grupocodi).Select(y => y.Ptomedicodi).Distinct().Count();
                        wsHist.Cells[row + 1, colGrupo].Value = gruponomb;
                        wsHist.Cells[row + 1, colGrupo, row + 1, colGrupo + cantPtoByGrupo - 1].Merge = true;
                        wsHist.Cells[row + 1, colGrupo, row + 1, colGrupo + cantPtoByGrupo - 1].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        wsHist.Cells[row + 1, colGrupo, row + 1, colGrupo + cantPtoByGrupo - 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        wsHist.Cells[row + 1, colGrupo, row + 1, colGrupo + cantPtoByGrupo - 1].Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));

                        colGrupo += cantPtoByGrupo;

                        //pto medicion x grupo
                        var listaPtocodi = data24.Where(x => x.Emprcodi == empcodi && x.Grupocodi == grupocodi).Select(y => y.Ptomedicodi).Distinct().ToList();
                        foreach (var ptocodi in listaPtocodi)
                        {
                            var pto = data24.Where(x => x.Ptomedicodi == ptocodi).FirstOrDefault();
                            var equinomb = pto.Equinomb;
                            var tipoPtonomb = pto.Tipoptomedinomb;
                            var tipoInfonomb = pto.Tipoinfoabrev;

                            wsHist.Cells[row + 2, colEquipo].Value = equinomb;
                            wsHist.Cells[row + 2, colEquipo].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                            wsHist.Cells[row + 3, colEquipo].Value = tipoPtonomb;
                            wsHist.Cells[row + 3, colEquipo].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                            wsHist.Cells[row + 4, colEquipo].Value = "m3/s";
                            wsHist.Cells[row + 4, colEquipo].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                            using (var range = wsHist.Cells[row + 2, colEquipo, row + 4, colEquipo])
                            {
                                range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                                range.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#5B91DD"));
                            }

                            colEquipo++;

                            posHora = 0;
                            foreach (var fecha in model.ListaFechaHistorico)
                            {
                                MeMedicion24DTO ptoFecha = data24.Where(x => x.Ptomedicodi == ptocodi && x.Medifecha.Date == fecha.Date).FirstOrDefault();

                                if (ptoFecha != null)
                                {
                                    for (int h = 0; h < 24; h++)
                                    {
                                        var valorHora = (decimal?)ptoFecha.GetType().GetProperty("H" + (h + 1).ToString()).GetValue(ptoFecha, null);
                                        listaData[posHora][posPtomedi] = valorHora + "";
                                        posHora++;
                                    }
                                }
                                else
                                {
                                    for (int h = 0; h < 24; h++)
                                    {
                                        listaData[posHora][posPtomedi] = "";
                                        posHora++;
                                    }
                                }
                            }

                            posPtomedi++;
                        }
                    }
                }

                using (var range = wsHist.Cells[8, 1, 13, colGrupo - 1])
                {
                    range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    range.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    range.Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    range.Style.Font.Color.SetColor(Color.White);
                }

                //cuerpo
                row += 5;
                for (int i = 0; i < listaData.Count(); i++)
                {
                    var fila = listaData[i];
                    wsHist.Cells[row, 1].Value = listaHoraFormato[i];
                    wsHist.Cells[row, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    for (int j = 0; j < fila.Count(); j++)
                    {
                        decimal? valorData = null;
                        if (fila[j] != null && fila[j] != "")
                        {
                            valorData = decimal.Parse(fila[j]);
                        }
                        wsHist.Cells[row, j + 2].Value = valorData;
                        wsHist.Cells[row, j + 2].Style.Numberformat.Format = @"0.000";
                    }
                    row++;
                }

                using (var range = wsHist.Cells[14, 1, 14 + listaData.Count() - 1, colGrupo - 1])
                {
                    range.Style.Border.BorderAround(ExcelBorderStyle.Thin);
                }

                request = (HttpWebRequest)System.Net.HttpWebRequest.Create(ConfigurationManager.AppSettings["LogoCoes"].ToString());
                response = (HttpWebResponse)request.GetResponse();
                img = System.Drawing.Image.FromStream(response.GetResponseStream());
                picture = wsHist.Drawings.AddPicture("Imagen", img);
                picture.From.Column = 0;
                picture.From.Row = 1;
                picture.To.Column = 1;
                picture.To.Row = 2;
                picture.SetSize(120, 60);

                #endregion
                wsHist.Column(1).Width = 35;
                for (int i = 2; i <= colGrupo; i++)
                {
                    wsHist.Column(i).Width = 20;
                }

                fileExcel = System.IO.Path.GetTempFileName();
                xlPackage.SaveAs(new FileInfo(fileExcel));
            }

            return fileExcel;
        }
        //fin modificado

        //inicio agregado
        private void GenerarFormatoPronostico(HidrologiaModel model, int tipoReporte, string fecha, string semana)
        {
            model.Dia = fecha;
            model.Semana = semana;

            DateTime fechaIniPronostico, fechaIniHistorico;

            int idReportePronostico;
            int cantDiaHistorico, cantDiaPronostico;
            string tituloReportePronostico, tituloReporteHistorico;
            string tipoPeriodo, filtroPronosticValor, filtroPronosticoDesc, filtroHistoricoValor, filtroHistoricoDesc;
            string hojaPronostico, hojaHistorico;
            if (ConstantesRptePronostico.TipoReporteDiario == tipoReporte)
            {
                idReportePronostico = ConstantesRptePronostico.IdReporteCodiPronosticoDiario;

                fechaIniPronostico = EPDate.GetFechaIniPeriodo(1, model.Mes, model.Semana, model.Dia, Constantes.FormatoFecha);
                fechaIniHistorico = EPDate.GetFechaIniPeriodo(1, model.Mes, model.Semana, model.Dia, Constantes.FormatoFecha);

                cantDiaPronostico = ConstantesRptePronostico.TipoReporteDiarioPronosticoCantDia;
                cantDiaHistorico = ConstantesRptePronostico.TipoReporteDiarioHistoricoCantDia;
                tituloReportePronostico = ConstantesRptePronostico.TituloReporteDiarioPronostico;
                tituloReporteHistorico = ConstantesRptePronostico.TituloReporteDiarioHistorico;
                tipoPeriodo = ConstantesRptePronostico.TipoPeriodoReporteDiario;

                filtroPronosticoDesc = ConstantesRptePronostico.FiltroPronosticoDiarioDesc;
                filtroPronosticValor = fechaIniPronostico.ToString(ConstantesAppServicio.FormatoFecha);

                filtroHistoricoDesc = ConstantesRptePronostico.FiltroHistoricoDiarioDesc;
                filtroHistoricoValor = fechaIniHistorico.ToString(ConstantesAppServicio.FormatoFecha);

                hojaPronostico = ConstantesRptePronostico.HojaReporteDiarioPronostico;
                hojaHistorico = ConstantesRptePronostico.HojaReporteDiarioHistorico;
            }
            else
            {
                idReportePronostico = ConstantesRptePronostico.IdReporteCodiPronosticoSemanal;

                fechaIniPronostico = EPDate.GetFechaIniPeriodo(2, model.Mes, model.Semana, model.Dia, Constantes.FormatoFecha);
                fechaIniHistorico = EPDate.GetFechaIniPeriodo(2, model.Mes, model.Semana, model.Dia, Constantes.FormatoFecha);
                fechaIniHistorico = fechaIniHistorico.AddDays(ConstantesRptePronostico.TipoReporteSemanalHistoricoCantSemana * 7);

                cantDiaPronostico = ConstantesRptePronostico.TipoReporteSemanalPronosticoCantDia;
                cantDiaHistorico = ConstantesRptePronostico.TipoReporteSemanalHistoricoCantDia;
                tituloReportePronostico = ConstantesRptePronostico.TituloReporteSemanalPronostico;
                tituloReporteHistorico = ConstantesRptePronostico.TituloReporteSemanalHistorico;
                tipoPeriodo = ConstantesRptePronostico.TipoPeriodoReporteSemanal;

                filtroPronosticoDesc = ConstantesRptePronostico.FiltroPronosticoSemanalDesc;
                filtroPronosticValor = EPDate.f_numerosemana(fechaIniPronostico) + "-" + fechaIniPronostico.Year;
                filtroHistoricoDesc = ConstantesRptePronostico.FiltroHistoricoSemanalDesc;
                filtroHistoricoValor = EPDate.f_numerosemana(fechaIniHistorico.AddDays(-1)) + "-" + fechaIniHistorico.Year;

                hojaPronostico = ConstantesRptePronostico.HojaReporteSemanalPronostico;
                hojaHistorico = ConstantesRptePronostico.HojaReporteSemanalHistorico;

                fechaIniPronostico = fechaIniPronostico.AddDays(-1);
            }

            //generación del período
            List<DateTime> listaFechaPronostico = new List<DateTime>();
            List<DateTime> listaFechaHistorico = new List<DateTime>();
            DateTime dayPron = fechaIniPronostico.Date;
            for (var i = 1; i <= cantDiaPronostico; i++)
            {
                listaFechaPronostico.Add(dayPron.AddDays(i));
            }
            DateTime dayHist = fechaIniHistorico.Date;
            for (var i = 1; i <= cantDiaHistorico; i++)
            {
                listaFechaHistorico.Add(dayHist.AddDays(-i));
            }

            listaFechaPronostico = listaFechaPronostico.OrderBy(o => o.Date).ToList();
            listaFechaHistorico = listaFechaHistorico.OrderBy(o => o.Date).ToList();

            DateTime finiPronostico = listaFechaPronostico[0];
            DateTime ffinPronostico = listaFechaPronostico[listaFechaPronostico.Count - 1];
            DateTime finiHistorico = listaFechaHistorico[0];
            DateTime ffinHistorico = listaFechaHistorico[listaFechaHistorico.Count - 1];

            model.idReportePronostico = idReportePronostico;
            model.TipoReportePronostico = tipoReporte;
            model.ListaFechaPronostico = listaFechaPronostico;
            model.ListaFechaHistorico = listaFechaHistorico;
            model.FechainiPronostico = finiPronostico;
            model.FechafinPronostico = ffinPronostico;
            model.FechainiHistorico = finiHistorico;
            model.FechafinHistorico = ffinHistorico;
            model.NomTipoPeriodo = tipoPeriodo;
            model.TituloReportePronostico = tituloReportePronostico;
            model.TituloReporteHistorico = tituloReporteHistorico;

            model.FiltroPronosticoDesc = filtroPronosticoDesc;
            model.FiltroPronosticoValor = filtroPronosticValor;
            model.FiltroHistoricoDesc = filtroHistoricoDesc;
            model.FiltroHistoricoValor = filtroHistoricoValor;

            model.HojaPronostico = hojaPronostico;
            model.HojaHistorico = hojaHistorico;

            string nombreArchivo = "Hidrología-";
            nombreArchivo += hojaPronostico.Replace(" ", "") + "_";
            if (ConstantesRptePronostico.TipoReporteDiario == tipoReporte)
            {
                nombreArchivo += finiPronostico.ToString("ddMMyyyy") + "_al_" + ffinPronostico.ToString("ddMMyyyy");
            }
            else
            {
                nombreArchivo += "SEM" + semana.Substring(4, semana.Length - 4) + semana.Substring(0, 4);
            }

            model.NombreArchivo = string.Format("{0}_{1:_HHmmss}.xlsx", nombreArchivo, DateTime.Now);
        }
        //fin agregado

        //inicio modificado
        /// <summary>
        /// Generacion del Html de los reportes de tipo Prónostico
        /// </summary>
        /// <param name="data"></param>
        /// <param name="listaFecha"></param>
        /// <param name="tituloReporte"></param>
        /// <returns></returns>        
        public string ListaReportePronosticoHidrologia(List<MeMedicion1DTO> data, List<DateTime> listaFecha, string tituloReporte)
        {
            int cantDia = listaFecha.Count;
            StringBuilder strHtml = new StringBuilder();

            #region cabecera_reporte
            //********* CABECERA DE LA TABLA *******//
            strHtml.Append("<table class='pretty tabla-icono'>");
            strHtml.Append("<thead>");

            strHtml.Append("<tr>");
            strHtml.Append(string.Format("<th style='width:70px;' colspan='{0}'>{1}</th>", 3 + cantDia, tituloReporte));
            strHtml.Append("</tr>");

            strHtml.Append("<tr>");
            strHtml.Append("<th style='width:120px;' rowspan='2'>EMPRESA</th>");
            strHtml.Append("<th style='width:120px;' rowspan='2'>UBICACIÓN</th>");
            strHtml.Append("<th style='width:170px;' rowspan='2'>DESCRIPCIÓN</th>");

            foreach (var f in listaFecha)
            {
                strHtml.Append(string.Format("<th style='width:70px;' colspan='1'>{0}</th>", f.ToString(ConstantesAppServicio.FormatoFecha)));
            }
            strHtml.Append("</tr>");

            strHtml.Append("<tr>");
            for (int i = 0; i < cantDia; i++)
            {
                strHtml.Append("<th style='width:70px;' colspan='1'>m3/s</th>");
            }
            strHtml.Append("</tr>");

            strHtml.Append("</thead>");
            #endregion

            /// **************  CUERPO DE LA TABLA **************//
            var listaEmpresa = data.Select(y => new { y.Emprcodi, y.Emprnomb }).Distinct().ToList().OrderBy(c => c.Emprnomb);
            var listaEmprcodi = listaEmpresa.Select(x => x.Emprcodi).ToList();

            strHtml.Append("<tbody>");
            foreach (var empcodi in listaEmprcodi)
            {
                //empresa
                string nomEmpresa = data.Where(x => x.Emprcodi == empcodi).FirstOrDefault().Emprnomb;
                int cantPtoPorEmpresa = data.Where(x => x.Emprcodi == empcodi).Select(y => y.CalculadoPtomedicodi).Distinct().Count();
                strHtml.Append("<tr>");
                strHtml.Append(string.Format("<td rowspan='" + cantPtoPorEmpresa + "'>{0}</td>", nomEmpresa));

                //ubicacion
                var listaUbicacion = data.Where(x => x.Emprcodi == empcodi).Select(y => new { y.Ubicacioncodi, y.Ubicaciondesc }).Distinct().ToList().OrderBy(c => c.Ubicaciondesc);
                var listaUbicacioncodi = listaUbicacion.Select(x => x.Ubicacioncodi).ToList();
                for (int u = 0; u < listaUbicacioncodi.Count; u++)
                {
                    if (u != 0)
                    {
                        strHtml.Append("</tr>");
                        strHtml.Append("<tr>");
                    }

                    var ubicacioncodi = listaUbicacioncodi[u];
                    var nomUbicacion = data.Where(x => x.Emprcodi == empcodi && x.Ubicacioncodi == ubicacioncodi).FirstOrDefault().Ubicaciondesc;
                    int cantPtoPorUbicacion = data.Where(x => x.Emprcodi == empcodi && x.Ubicacioncodi == ubicacioncodi).Select(y => y.CalculadoPtomedicodi).Distinct().Count();
                    strHtml.Append(string.Format("<td rowspan='" + cantPtoPorUbicacion + "'>{0}</td>", nomUbicacion));

                    //Puntos calculados
                    var listaPtoCalculado = data.Where(x => x.Emprcodi == empcodi && x.Ubicacioncodi == ubicacioncodi).Select(y => new { y.CalculadoPtomedicodi, y.CalculadoPtomedidesc }).Distinct().ToList().OrderBy(c => c.CalculadoPtomedidesc);
                    var listaPtoCalculadocodi = listaPtoCalculado.Select(y => y.CalculadoPtomedicodi).ToList();
                    for (int pc = 0; pc < listaPtoCalculadocodi.Count; pc++)
                    {
                        if (pc != 0)
                        {
                            strHtml.Append("</tr>");
                            strHtml.Append("<tr>");
                        }
                        var calculadoptocodi = listaPtoCalculadocodi[pc];
                        var ptoCalculado = data.Where(x => x.CalculadoPtomedicodi == calculadoptocodi).FirstOrDefault();
                        var nomPtoCalculado = ptoCalculado.CalculadoPtomedidesc;
                        var tipoRelacion = ptoCalculado.TipoRelacioncodi;
                        var cantPtoPorCalculado = data.Where(x => x.CalculadoPtomedicodi == calculadoptocodi).Select(y => y.OrigenPtomedicodi).Distinct().Count();
                        strHtml.Append(string.Format("<td rowspan='1' style='padding-top: 5px;padding-bottom: 5px;'>{0}</td>", nomPtoCalculado));

                        foreach (var f in listaFecha)
                        {
                            //obtener puntos de medicion
                            var listaPto = data.Where(x => x.CalculadoPtomedicodi == calculadoptocodi && x.Medifecha.Date == f.Date).ToList();
                            decimal? resultado = null;
                            string valorHtml = "";
                            string classFaltante = "";
                            bool dataCompleta = true;

                            if (listaPto.Count > 0)
                            {
                                foreach (var pto in listaPto)
                                {
                                    if (pto.H1 == null)
                                    {
                                        dataCompleta = false;
                                    }
                                    var factorCalculado = pto.CalculadoFactor;

                                    if (pto.H1 != null)
                                    {
                                        resultado = resultado == null ? 0 : resultado;
                                        resultado += pto.H1 * factorCalculado;
                                    }

                                }
                                valorHtml = String.Format("{0:0.0000}", resultado);
                            }


                            if (listaPto.Count == 0 || cantPtoPorCalculado != listaPto.Count)
                            {
                                dataCompleta = false;
                            }

                            if (!dataCompleta)
                            {
                                valorHtml += valorHtml == null || valorHtml == "" ? "" : " - ";
                                valorHtml += "<a href='#' onclick=verDetalleFaltante('" + calculadoptocodi + "','" + f.ToString(Constantes.FormatoFecha) + "')>Ver detalle de faltantes</a>";
                                classFaltante = "class='detalle-faltante' ";
                            }

                            strHtml.Append(string.Format("<td rowspan='1' {1}>{0}</td>", valorHtml, classFaltante));
                        }
                    }
                }

                strHtml.Append("</tr>");
            }

            strHtml.Append("</tbody>");
            strHtml.Append("</table>");

            return strHtml.ToString();
        }
        //fin modificado

        /// <summary>
        /// Generacion del Html de los reportes de tipo Histórico
        /// </summary>
        /// <param name="data"></param>
        /// <param name="listaFecha"></param>
        /// <param name="tituloReporte"></param>
        /// <returns></returns>
        public string ListaReporteHistoricoHidrologia(List<MeMedicion24DTO> data, List<DateTime> listaFecha, string tituloReporte)
        {
            int cantDia = listaFecha.Count;
            StringBuilder strHtml = new StringBuilder();
            StringBuilder strHtmlGrupo = new StringBuilder();
            StringBuilder strHtmlEquipo = new StringBuilder();
            StringBuilder strHtmlTipoPto = new StringBuilder();
            StringBuilder strHtmlTipoInfo = new StringBuilder();
            StringBuilder strHtmlData = new StringBuilder();

            #region cabecera_reporte
            //********* CABECERA DE LA TABLA *******//
            //datos
            var listaEmpcodi = data.Select(x => x.Emprcodi).Distinct().ToList();
            int cantPto = data.Select(x => x.Ptomedicodi).Distinct().Count();
            int numHoras = 24 * listaFecha.Count;

            string[][] listaData = new string[numHoras][];
            for (int i = 0; i < listaData.Count(); i++)
            {
                listaData[i] = new string[cantPto];
            }
            int posHora = 0;
            int posPtomedi = 0;

            string[] listaHoraFormato = new string[numHoras];

            for (int i = 0; i < listaFecha.Count(); i++)
            {
                DateTime f = listaFecha[i].Date;
                for (int j = 0; j < 24; j++)
                {
                    DateTime horaFila = f.AddHours(j);
                    listaHoraFormato[posHora] = horaFila.ToString(ConstantesAppServicio.FormatoFechaFull);
                    posHora++;
                }
            }

            strHtml.Append("<table class='pretty tabla-icono'>");
            strHtml.Append("<thead>");

            strHtml.Append("<tr>");
            strHtml.Append("<th class='th-fecha' rowspan='6'>FECHA</th>");
            strHtml.Append(string.Format("<th class='th-titulo' colspan='{0}'>{1}</th>", cantPto, tituloReporte));
            strHtml.Append("</tr>");

            //empresas
            strHtml.Append("<tr>");
            foreach (var empcodi in listaEmpcodi)
            {
                var empnomb = data.Where(x => x.Emprcodi == empcodi).FirstOrDefault().Emprnomb;
                int cantPtoByEmp = data.Where(x => x.Emprcodi == empcodi).Select(y => y.Ptomedicodi).Distinct().Count();
                strHtml.Append(string.Format("<th class='th-empresa' colspan='{0}'>{1}</th>", cantPtoByEmp, empnomb));

                //grupos x empresa
                var listaGrupocodi = data.Where(x => x.Emprcodi == empcodi).Select(y => y.Grupocodi).Distinct().ToList();
                foreach (var grupocodi in listaGrupocodi)
                {
                    var gruponomb = data.Where(x => x.Grupocodi == grupocodi).FirstOrDefault().Gruponomb;
                    int cantPtoByGrupo = data.Where(x => x.Emprcodi == empcodi && x.Grupocodi == grupocodi).Select(y => y.Ptomedicodi).Distinct().Count();
                    strHtmlGrupo.Append(string.Format("<th class='th-grupo' colspan='{0}'>{1}</th>", cantPtoByGrupo, gruponomb));

                    //pto medicion x grupo
                    var listaPtocodi = data.Where(x => x.Emprcodi == empcodi && x.Grupocodi == grupocodi).Select(y => y.Ptomedicodi).Distinct().ToList();
                    foreach (var ptocodi in listaPtocodi)
                    {
                        var pto = data.Where(x => x.Ptomedicodi == ptocodi).FirstOrDefault();
                        var equinomb = pto.Equinomb;
                        var tipoPtonomb = pto.Tipoptomedinomb;
                        var tipoInfonomb = "m3/s";//pto.Tipoinfoabrev;

                        strHtmlEquipo.Append(string.Format("<th class='th-equipo'>{0}</th>", equinomb));
                        strHtmlTipoPto.Append(string.Format("<th class='th-tipo-pto'>{0}</th>", tipoPtonomb));
                        strHtmlTipoInfo.Append(string.Format("<th class='th-tipo-info'>{0}</th>", tipoInfonomb));

                        posHora = 0;
                        foreach (var fecha in listaFecha)
                        {
                            MeMedicion24DTO ptoFecha = data.Where(x => x.Ptomedicodi == ptocodi && x.Medifecha.Date == fecha.Date).FirstOrDefault();

                            if (ptoFecha != null)
                            {
                                for (int h = 0; h < 24; h++)
                                {
                                    var valorHora = (decimal?)ptoFecha.GetType().GetProperty("H" + (h + 1).ToString()).GetValue(ptoFecha, null);
                                    listaData[posHora][posPtomedi] = valorHora + "";
                                    posHora++;
                                }
                            }
                            else
                            {
                                for (int h = 0; h < 24; h++)
                                {
                                    listaData[posHora][posPtomedi] = "";
                                    posHora++;
                                }
                            }
                        }

                        posPtomedi++;
                    }
                }
            }
            strHtml.Append("</tr>");

            strHtml.Append("<tr>");
            strHtml.Append(strHtmlGrupo);
            strHtml.Append("</tr>");

            strHtml.Append("<tr>");
            strHtml.Append(strHtmlEquipo);
            strHtml.Append("</tr>");

            strHtml.Append("<tr>");
            strHtml.Append(strHtmlTipoPto);
            strHtml.Append("</tr>");

            strHtml.Append("<tr>");
            strHtml.Append(strHtmlTipoInfo);
            strHtml.Append("</tr>");

            strHtml.Append("</thead>");
            #endregion

            /// **************  CUERPO DE LA TABLA **************//       
            strHtml.Append("<tbody>");


            for (int i = 0; i < listaData.Count(); i++)
            {
                var fila = listaData[i];

                strHtml.Append("<tr>");
                strHtml.Append("<td class='td-hora'>" + listaHoraFormato[i] + "</td>");
                for (int j = 0; j < fila.Count(); j++)
                {
                    strHtml.Append(string.Format("<td>{0}</td>", fila[j]));
                }
                strHtml.Append("</tr>");
            }

            strHtml.Append("</tbody>");
            strHtml.Append("</table>");

            return strHtml.ToString();
        }

        #endregion

        #region Reporte Información Aplicativo POWEL

        /// <summary>
        /// Index App Powel
        /// </summary>
        /// <returns></returns>
        public ActionResult ReporteAppPowel()
        {
            HidrologiaModel model = new HidrologiaModel();

            model.Anho = DateTime.Now.Year.ToString();
            model.NroSemana = EPDate.f_numerosemana(DateTime.Now);
            model.FechaFin = DateTime.Now.AddDays(-1).ToString(Constantes.FormatoFecha);
            model.FechaInicio = DateTime.Now.AddDays(-1).ToString(Constantes.FormatoFecha);
            return View(model);
        }

        /// <summary>
        /// View Reporte Powel
        /// </summary>
        /// <param name="tipoReporte"></param>
        /// <param name="fechaInicial"></param>
        /// <param name="fechaFinal"></param>
        /// <param name="semanaIni"></param>
        /// <param name="SemanaFin"></param>
        /// <param name="nroPagina"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult CargarListaAppPowel(int tipoReporte, string fechaInicial, string fechaFinal, string semanaIni, string SemanaFin, int nroPagina)
        {
            HidrologiaModel model = new HidrologiaModel();
            //model.Dia = fecha;
            //GenerarFormatoPronostico(model, tipoReporte, fecha, semana);
            DateTime fechaIni = new DateTime();
            DateTime fechaFin = new DateTime();

            if (tipoReporte == 1)
            { //Diario
                fechaIni = EPDate.GetFechaIniPeriodo(1, model.Mes, model.Semana, fechaInicial, Constantes.FormatoFecha);
                fechaFin = EPDate.GetFechaIniPeriodo(1, model.Mes, model.Semana, fechaFinal, Constantes.FormatoFecha);
                List<MeMedicion24DTO> listaDataDiaria = logic.ListaHistoricoHidrologia(ConstHidrologia.IdReporteAppPowelDiario,
                fechaIni, fechaFin).OrderBy(x => x.Medifecha).ToList();
                DateTime dataNull = new DateTime();
                listaDataDiaria = listaDataDiaria.Where(x => x.Medifecha != dataNull).ToList();
                //this.ListaFechas = listaDataDiaria.Select(x => x.Medifecha).Distinct().ToList();
                List<DateTime> listFechas = new List<DateTime>();
                for (var f = fechaIni.Date; f <= fechaFin; f = f.AddDays(1))
                {
                    listFechas.Add(f);
                }
                if (listFechas.Count > 0)
                {
                    fechaIni = listFechas[nroPagina - 1];
                }

                List<MeReporptomedDTO> listaCabecera = logic.ListarEncabezadoPowel(ConstHidrologia.IdReporteAppPowelDiario);

                model.HtmlReporteHistorico = ListaReporteAppPowelDiario(listaDataDiaria, listaCabecera, fechaIni);
            }
            else //Semanal
            {
                fechaIni = EPDate.GetFechaIniPeriodo(2, model.Mes, semanaIni, fechaInicial, Constantes.FormatoFecha);
                fechaFin = EPDate.GetFechaIniPeriodo(2, model.Mes, SemanaFin, fechaFinal, Constantes.FormatoFecha).AddDays(6);
                List<MeMedicion1DTO> listaDataSemanal = logic.ListaSemanalPOwel(fechaIni, fechaFin, ConstHidrologia.IdReporteAppPowelSem);
                List<MeReporptomedDTO> listaCabecera = logic.ListarEncabezadoPowel(ConstHidrologia.IdReporteAppPowelSem);
                model.HtmlReporteHistorico = ListaReporteAppPowelSemanal(listaDataSemanal, listaCabecera, fechaIni, fechaFin);
            }
            return PartialView(model);
        }

        /// <summary>
        /// Generacion del Html del reporte diario de Información Hidrológica Aplicativo Powel
        /// </summary>
        /// <param name="data"></param>
        /// <param name="listaFecha"></param>
        /// <param name="tituloReporte"></param>
        /// <returns></returns>
        public string ListaReporteAppPowelDiario(List<MeMedicion24DTO> data, List<MeReporptomedDTO> listaCabecera, DateTime listaFecha)
        {

            StringBuilder strHtml = new StringBuilder();
            StringBuilder strHtmlCodigos = new StringBuilder();
            StringBuilder strHtmlEmpresas = new StringBuilder();
            StringBuilder strHtmlEquipo = new StringBuilder();
            StringBuilder strHtmlTipoPto = new StringBuilder();
            StringBuilder strHtmlTipoInfo = new StringBuilder();
            StringBuilder strHtmlData = new StringBuilder();

            NumberFormatInfo nfi = logic.GenerarNumberFormatInfo();
            //nfi.NumberDecimalDigits = 2;         

            #region cabecera_reporte
            //********* CABECERA DE LA TABLA *******//
            //datos
            var listaEmpcodi = listaCabecera.Select(x => x.Emprcodi).Distinct().ToList();
            int cantPto = listaCabecera.Select(x => x.Ptomedicodi).Distinct().Count();
            int numHoras = 24;

            string[][] listaData = new string[numHoras][];
            for (int i = 0; i < listaData.Count(); i++)
            {
                listaData[i] = new string[cantPto];
            }
            int posHora = 0;
            int posPtomedi = 0;

            string[] listaHoraFormato = new string[numHoras];

            for (int i = 0; i < 1; i++)
            {
                DateTime f = listaFecha.Date;
                for (int j = 0; j < 24; j++)
                {
                    DateTime horaFila = f.AddHours(j);
                    listaHoraFormato[posHora] = horaFila.ToString(ConstantesAppServicio.FormatoFechaFull);
                    posHora++;
                }
            }

            strHtml.Append("<table class='pretty tabla-icono'>");
            strHtml.Append("<thead>");

            foreach (var empcodi in listaEmpcodi)
            {
                var empnomb = listaCabecera.Where(x => x.Emprcodi == empcodi).FirstOrDefault().Emprnomb;
                int cantPtoByEmp = listaCabecera.Where(x => x.Emprcodi == empcodi).Select(y => y.Ptomedicodi).Distinct().Count();
                strHtmlEmpresas.Append(string.Format("<th class='th-empresa' colspan='{0}'>{1}</th>", cantPtoByEmp, empnomb));

                //centrales x empresa
                var listaCentralcodi = listaCabecera.Where(x => x.Emprcodi == empcodi).Select(y => y.Equicodi).Distinct().ToList();
                foreach (var centralcodi in listaCentralcodi)
                {
                    //pto medicion x central
                    var listaPtocodi = listaCabecera.Where(x => x.Emprcodi == empcodi && x.Equicodi == centralcodi).Select(y => new { y.Ptomedicodi, y.Repptoorden }).Distinct().OrderBy(y => y.Repptoorden).ToList();
                    foreach (var ptocodi in listaPtocodi)
                    {
                        var pto = listaCabecera.Where(x => x.Ptomedicodi == ptocodi.Ptomedicodi).FirstOrDefault();
                        var equinomb = pto.Equinomb;
                        var tipoPtonomb = pto.Tipoptomedinomb;
                        var tipoInfonomb = pto.Tipoinfoabrev;
                        var codigo = pto.Ptomedicodi;

                        strHtmlEquipo.Append(string.Format("<th class='th-equipo'>{0}</th>", equinomb));
                        strHtmlTipoPto.Append(string.Format("<th class='th-tipo-pto'>{0}</th>", tipoPtonomb));
                        strHtmlTipoInfo.Append(string.Format("<th class='th-tipo-info'>{0}</th>", tipoInfonomb));
                        strHtmlCodigos.Append(string.Format("<th>{0}</th>", codigo));

                        posHora = 0;
                        //foreach (var fecha in listaFecha)
                        //{
                        MeMedicion24DTO ptoFecha = data.Where(x => x.Ptomedicodi == ptocodi.Ptomedicodi && x.Medifecha.Date == listaFecha.Date).FirstOrDefault();

                        if (ptoFecha != null)
                        {
                            for (int h = 0; h < 24; h++)
                            {
                                var valorHora = (decimal?)ptoFecha.GetType().GetProperty("H" + (h + 1).ToString()).GetValue(ptoFecha, null);
                                listaData[posHora][posPtomedi] = valorHora + "";
                                posHora++;
                            }
                        }
                        else
                        {
                            for (int h = 0; h < 24; h++)
                            {
                                listaData[posHora][posPtomedi] = "";
                                posHora++;
                            }
                        }
                        // }

                        posPtomedi++;
                    }
                }
            }


            strHtml.Append("<tr>");
            strHtml.Append("<th class='th-colIni'>CÓDIGOS</th>");
            strHtml.Append(strHtmlCodigos);
            strHtml.Append("</tr>");

            strHtml.Append("<tr>");
            strHtml.Append("<th width = '100px' class='th-colIni'>EMPRESA</th>");
            strHtml.Append(strHtmlEmpresas);
            strHtml.Append("</tr>");

            strHtml.Append("<tr>");
            strHtml.Append("<th class='th-colIni'>CENTRAL/EMBALSE/ESTACION</th>");
            strHtml.Append(strHtmlEquipo);
            strHtml.Append("</tr>");

            strHtml.Append("<tr>");
            strHtml.Append("<th class='th-colIni'>MEDIDA</th>");
            strHtml.Append(strHtmlTipoPto);
            strHtml.Append("</tr>");

            strHtml.Append("<tr>");
            strHtml.Append("<th class='th-colIni'>FECHA/UNIDAD</th>");
            strHtml.Append(strHtmlTipoInfo);
            strHtml.Append("</tr>");

            strHtml.Append("</thead>");
            #endregion

            /// **************  CUERPO DE LA TABLA **************//       
            strHtml.Append("<tbody>");

            if (listaData.Count() > 0)
            {

                for (int i = 0; i < listaData.Count(); i++)
                {
                    var fila = listaData[i];

                    strHtml.Append("<tr>");
                    strHtml.Append("<td class='td-hora'>" + listaHoraFormato[i] + "</td>");
                    for (int j = 0; j < fila.Count(); j++)
                    {
                        //decimal valor = (fila[j] == "") ? 0.00m : Convert.ToDecimal(fila[j]);   

                        if (fila[j] != "")
                        {
                            decimal valor = Convert.ToDecimal(fila[j]);
                            strHtml.Append(string.Format("<td>{0}</td>", valor.ToString("N", nfi)));
                        }
                        else
                        {
                            strHtml.Append("<td></td>");
                        }
                    }
                    strHtml.Append("</tr>");
                }

            }
            else // no existen registros
            {
                strHtml.Append("<tr>");
                strHtml.Append("<td colspan = '9'>No Existen registros..!</td>");
                strHtml.Append("</tr>");
            }
            strHtml.Append("</tbody>");
            strHtml.Append("</table>");
            return strHtml.ToString();
        }

        /// <summary>
        /// Generacion del Html del reporte Semanal de Información Hidrológica Aplicativo Powel
        /// </summary>
        /// <param name="data"></param>
        /// <param name="listaFecha"></param>
        /// <param name="tituloReporte"></param>
        /// <returns></returns>        
        public string ListaReporteAppPowelSemanal(List<MeMedicion1DTO> data, List<MeReporptomedDTO> listaCabecera, DateTime fechaIni, DateTime fechaFin)
        {

            StringBuilder strHtml = new StringBuilder();
            StringBuilder strHtmlCodigos = new StringBuilder();
            StringBuilder strHtmlEmpresas = new StringBuilder();
            StringBuilder strHtmlEquipo = new StringBuilder();
            StringBuilder strHtmlTipoPto = new StringBuilder();
            StringBuilder strHtmlTipoInfo = new StringBuilder();
            StringBuilder strHtmlData = new StringBuilder();
            NumberFormatInfo nfi = logic.GenerarNumberFormatInfo();
            // nfi.NumberDecimalDigits = 2;


            #region cabecera_reporte
            //********* CABECERA DE LA TABLA *******//
            //datos
            var listaEmpcodi = listaCabecera.Select(x => x.Emprcodi).Distinct().ToList();
            int cantPto = listaCabecera.Select(x => x.Ptomedicodi).Distinct().Count();

            //var listFechas = data.Select(x => x.Medifecha).Distinct().OrderBy(x => x).ToList();
            List<DateTime> listFechas = new List<DateTime>();
            for (var f = fechaIni.Date; f <= fechaFin; f = f.AddDays(1))
            {
                listFechas.Add(f);
            }
            // int numHoras = listFechas.Count;

            string[][] listaData = new string[listFechas.Count][];
            for (int i = 0; i < listaData.Count(); i++)
            {
                listaData[i] = new string[cantPto];
            }
            int posHora = 0;
            int posPtomedi = 0;

            //strHtml.Append("<table class='pretty tabla-icono'>");

            strHtml.Append("<table border='1' class='pretty tabla-icono' cellspacing='0' id='tabla'>");

            strHtml.Append("<thead>");

            foreach (var empcodi in listaEmpcodi)
            {
                var empnomb = listaCabecera.Where(x => x.Emprcodi == empcodi).FirstOrDefault().Emprnomb;
                int cantPtoByEmp = listaCabecera.Where(x => x.Emprcodi == empcodi).Select(y => y.Ptomedicodi).Distinct().Count();
                strHtmlEmpresas.Append(string.Format("<th class='th-empresa' colspan='{0}'>{1}</th>", cantPtoByEmp, empnomb));

                //Embalses x empresa
                var listaEquicodi = listaCabecera.Where(x => x.Emprcodi == empcodi).Select(y => y.Equicodi).Distinct().ToList();
                foreach (var equicodi in listaEquicodi)
                {
                    //pto medicion x equipo
                    var listaPtocodi = listaCabecera.Where(x => x.Emprcodi == empcodi && x.Equicodi == equicodi).Select(y => new { y.Ptomedicodi, y.Repptoorden }).Distinct().OrderBy(y => y.Repptoorden).ToList();
                    foreach (var ptocodi in listaPtocodi)
                    {
                        var pto = listaCabecera.Where(x => x.Ptomedicodi == ptocodi.Ptomedicodi).FirstOrDefault();
                        var equinomb = pto.Equinomb;
                        var tipoPtonomb = pto.Tipoptomedinomb;
                        var tipoInfonomb = pto.Tipoinfoabrev;
                        var codigo = pto.Ptomedicodi;

                        strHtmlEquipo.Append(string.Format("<th class='th-equipo'>{0}</th>", equinomb));
                        strHtmlTipoPto.Append(string.Format("<th class='th-tipo-pto'>{0}</th>", tipoPtonomb));
                        strHtmlTipoInfo.Append(string.Format("<th class='th-tipo-info'>{0}</th>", tipoInfonomb));
                        strHtmlCodigos.Append(string.Format("<th>{0}</th>", codigo));

                        posHora = 0;
                        foreach (var fecha in listFechas)
                        {
                            MeMedicion1DTO ptoFecha = data.Where(x => x.Ptomedicodi == ptocodi.Ptomedicodi && x.Medifecha.Date == fecha.Date).FirstOrDefault();
                            if (ptoFecha != null)
                            {
                                var valorHora = (decimal?)ptoFecha.GetType().GetProperty("H" + (1).ToString()).GetValue(ptoFecha, null);
                                listaData[posHora][posPtomedi] = valorHora + "";
                                posHora++;
                            }
                            else
                            {
                                listaData[posHora][posPtomedi] = "";
                                posHora++;
                            }
                        }
                        posPtomedi++;
                    }
                }
            }
            strHtml.Append("<tr>");
            strHtml.Append("<th class='th-colIni'>CÓDIGO</th>");
            strHtml.Append(strHtmlCodigos);
            strHtml.Append("</tr>");

            strHtml.Append("<tr>");
            strHtml.Append("<th class='th-colIni'>EMPRESA</th>");
            strHtml.Append(strHtmlEmpresas);
            strHtml.Append("</tr>");

            strHtml.Append("<tr>");
            strHtml.Append("<th class='th-colIni'>CENTRAL/EMBALSE/ESTACION</th>");
            strHtml.Append(strHtmlEquipo);
            strHtml.Append("</tr>");

            strHtml.Append("<tr>");
            strHtml.Append("<th class='th-colIni'>MEDIDA</th>");
            strHtml.Append(strHtmlTipoPto);
            strHtml.Append("</tr>");

            strHtml.Append("<tr>");
            strHtml.Append("<th class='th-colIni'>FECHA/UNIDAD</th>");
            strHtml.Append(strHtmlTipoInfo);
            strHtml.Append("</tr>");

            strHtml.Append("</thead>");
            #endregion

            /// **************  CUERPO DE LA TABLA **************//       
            strHtml.Append("<tbody>");
            if (data.Count > 0)
            {
                for (int i = 0; i < listaData.Count(); i++)
                {
                    var fila = listaData[i];

                    strHtml.Append("<tr>");
                    strHtml.Append("<td>" + listFechas[i].ToString(Constantes.FormatoFecha) + "</td>");
                    for (int j = 0; j < fila.Count(); j++)
                    {
                        if (fila[j] != "")
                        {
                            decimal valor = Convert.ToDecimal(fila[j]);
                            strHtml.Append(string.Format("<td>{0}</td>", valor.ToString("N", nfi)));
                        }
                        else
                        {
                            strHtml.Append("<td></td>");
                        }
                    }
                    strHtml.Append("</tr>");
                }

            }
            else // no existen registros
            {
                //strHtml.Append("<tr>");
                //strHtml.Append("<td colspan = '18'>No Existen registros..!</td>");
                //strHtml.Append("</tr>");
            }
            strHtml.Append("</tbody>");
            strHtml.Append("</table>");

            return strHtml.ToString();
        }

        /// <summary>
        /// Permite mostrar el paginado de la consulta
        /// </summary>
        /// <param name="modelo"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult PaginadoAppPowel(string fechaInicial, string fechaFinal)
        {
            HidrologiaModel model = new HidrologiaModel();
            model.IndicadorPagina = false;
            //var formato = logic.GetByIdMeFormato(idTipoInformacion);
            //formato.ListaHoja = logic.GetByCriteriaMeFormatohojas(idTipoInformacion);
            DateTime fechaIni = DateTime.Now;
            DateTime fechaFin = DateTime.Now;

            if (fechaInicial != null)
            {
                fechaIni = DateTime.ParseExact(fechaInicial, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            }
            if (fechaFinal != null)
            {
                fechaFin = DateTime.ParseExact(fechaFinal, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            }
            List<MeMedicion24DTO> listaDataDiaria = logic.ListaHistoricoHidrologia(ConstHidrologia.IdReporteAppPowelDiario,
                fechaIni, fechaFin);
            DateTime dataNull = new DateTime();
            listaDataDiaria = listaDataDiaria.Where(x => x.Medifecha != dataNull).ToList();
            //this.ListaFechas = listaDataDiaria.Select(x => x.Medifecha).Distinct().ToList();

            List<DateTime> listFechas = new List<DateTime>();
            for (var f = fechaIni.Date; f <= fechaFin; f = f.AddDays(1))
            {
                listFechas.Add(f);
            }
            int nroRegistros = listFechas.Count;

            if (nroRegistros > 0)
            {
                int pageSize = Constantes.PageSize;
                int nroPaginas = nroRegistros; // (nroRegistros % pageSize == 0) ? nroRegistros / pageSize : nroRegistros / pageSize + 1;
                model.NroPaginas = nroPaginas;
                model.NroMostrar = Constantes.NroPageShow;
                model.IndicadorPagina = true;
            }

            return PartialView(model);
        }

        /// <summary>
        /// Exportación de reporte App Powel
        /// </summary>
        /// <param name="tipoReporte"></param>
        /// <param name="fechaInicial"></param>
        /// <param name="fechaFinal"></param>
        /// <param name="semanaIni"></param>
        /// <param name="semanaFin"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ExportarExcelListaAppPowel(int tipoReporte, string fechaInicial, string fechaFinal, string semanaIni, string semanaFin)
        {
            string ruta = string.Empty;
            string[] datos = new string[2];
            try
            {
                HidrologiaModel model = new HidrologiaModel();
                //GenerarFormatoPronostico(model, tipoReporte, fecha, semana);
                //*****
                DateTime fechaIni = new DateTime();
                DateTime fechaFin = new DateTime();

                if (tipoReporte == 1)
                { //Diario
                    fechaIni = EPDate.GetFechaIniPeriodo(1, model.Mes, model.Semana, fechaInicial, Constantes.FormatoFecha);
                    fechaFin = EPDate.GetFechaIniPeriodo(1, model.Mes, model.Semana, fechaFinal, Constantes.FormatoFecha);
                    List<MeMedicion24DTO> listaDataDiaria = logic.ListaHistoricoHidrologia(ConstHidrologia.IdReporteAppPowelDiario,
                    fechaIni, fechaFin).OrderBy(x => x.Medifecha).ToList();
                    DateTime dataNull = new DateTime();
                    listaDataDiaria = listaDataDiaria.Where(x => x.Medifecha != dataNull).ToList();
                    model.FechaInicio = fechaInicial;
                    model.FechaFin = fechaFinal;
                    model.FechaIniPowel = fechaIni;
                    model.FechaFinPowel = fechaFin;
                    model.ListaMedicion24 = listaDataDiaria;
                    List<MeReporptomedDTO> listaCabecera = logic.ListarEncabezadoPowel(ConstHidrologia.IdReporteAppPowelDiario);
                    model.Listacabecera = listaCabecera;
                    //nombre del archivo
                    string nombreArchivo = "Información_diaria_Powel_";

                    nombreArchivo += fechaIni.ToString("ddMMyyyy") + "_al_" + fechaFin.ToString("ddMMyyyy");
                    model.NombreArchivo = string.Format("{0}.xlsx", nombreArchivo);



                }
                else //Semanal
                {
                    fechaIni = EPDate.GetFechaIniPeriodo(2, model.Mes, semanaIni, fechaInicial, Constantes.FormatoFecha);
                    fechaFin = EPDate.GetFechaIniPeriodo(2, model.Mes, semanaFin, fechaFinal, Constantes.FormatoFecha).AddDays(6);
                    model.FechaIniPowel = fechaIni;
                    model.FechaFinPowel = fechaFin;

                    List<MeMedicion1DTO> listaDataSemanal = logic.ListaSemanalPOwel(fechaIni, fechaFin, ConstHidrologia.IdReporteAppPowelSem).OrderBy(x => x.Medifecha).ToList(); ;
                    //model.FechaInicio = fechaInicial;
                    //model.FechaFin = fechaFinal;
                    model.ListaMedicion1 = listaDataSemanal;
                    List<MeReporptomedDTO> listaCabecera = logic.ListarEncabezadoPowel(ConstHidrologia.IdReporteAppPowelSem);
                    model.Listacabecera = listaCabecera;

                    var strSemIni = "SEM" + Int32.Parse(semanaIni.Substring(4, semanaIni.Length - 4)) + "-" + Int32.Parse(semanaIni.Substring(0, 4));
                    var strSemFin = "SEM" + Int32.Parse(semanaFin.Substring(4, semanaFin.Length - 4)) + "-" + Int32.Parse(semanaFin.Substring(0, 4));

                    model.SemanaIni = strSemIni;
                    model.SemanaFin = strSemFin;


                    //nombre del archivo
                    string nombreArchivo = "Información_semanal_Powel_";

                    //nombreArchivo += "Sem_" + fechaIni.ToString("ddMMyyyy") + "_al_" + fechaFin.ToString("ddMMyyyy");
                    nombreArchivo += "Sem" + semanaIni.Substring(4, semanaIni.Length - 4) + "_" + semanaIni.Substring(0, 4) + "_a_" + "Sem" + semanaFin.Substring(4, semanaFin.Length - 4) + "_" + semanaFin.Substring(0, 4);
                    model.NombreArchivo = string.Format("{0}.xlsx", nombreArchivo);

                }
                ruta = GenerarFileExcelReporteInfoAppPowel(model, tipoReporte);
                datos[0] = ruta;
                datos[1] = model.NombreArchivo;
            }
            catch
            {
                datos[0] = "-1";
                datos[1] = "";
            }
            var jsonResult = Json(datos);
            return jsonResult;
        }

        /// <summary>
        /// Genera Archivo excel y devuelve la ruta mas el nombre del archivo
        /// </summary>
        private string GenerarFileExcelReporteInfoAppPowel(HidrologiaModel model, int tipoReporte)
        {
            string fileExcel = string.Empty;

            using (ExcelPackage xlPackage = new ExcelPackage())
            {
                if (tipoReporte == 1) //Diario
                {
                    ExcelWorksheet wsHist = xlPackage.Workbook.Worksheets.Add(ConstHidrologia.HojaReporteDiarioInfoAppPowel);
                    var data24 = model.ListaMedicion24;
                    var listaCabecera = model.Listacabecera;
                    #region Hoja Reporte Diario
                    int row = 4;
                    int column = 1;

                    //imagen
                    HttpWebRequest request = (HttpWebRequest)System.Net.HttpWebRequest.Create(ConfigurationManager.AppSettings["LogoCoes"].ToString());
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                    System.Drawing.Image img = System.Drawing.Image.FromStream(response.GetResponseStream());
                    ExcelPicture picture = wsHist.Drawings.AddPicture("Imagen", img);
                    picture.From.Column = 0;
                    picture.From.Row = 1;
                    picture.To.Column = 1;
                    picture.To.Row = 2;
                    picture.SetSize(120, 60);


                    //Imprimir filtro
                    wsHist.Cells[row, column].Value = "Fecha Inicio:";
                    wsHist.Cells[row, column].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                    wsHist.Cells[row, column + 1].Value = model.FechaInicio;
                    wsHist.Cells[row, column + 1].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                    wsHist.Cells[row + 1, column].Value = "Fecha Fin:";
                    wsHist.Cells[row + 1, column].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                    wsHist.Cells[row + 1, column + 1].Value = model.FechaFin;
                    wsHist.Cells[row + 1, column + 1].Style.Border.BorderAround(ExcelBorderStyle.Thin);


                    //********* CABECERA DE LA TABLA *******//
                    //datos
                    var listaEmpcodi = listaCabecera.Select(x => x.Emprcodi).Distinct().ToList();
                    int cantPto = listaCabecera.Select(x => x.Ptomedicodi).Distinct().Count();
                    //var listFechas = data24.Select(x => x.Medifecha).Distinct().ToList();



                    List<DateTime> listFechas = new List<DateTime>();
                    for (var f = model.FechaIniPowel.Date; f <= model.FechaFinPowel; f = f.AddDays(1))
                    {
                        listFechas.Add(f);
                    }
                    cantPto = cantPto == 0 ? 1 : cantPto;
                    int numHoras = 24 * listFechas.Count;

                    string[][] listaData = new string[numHoras][];
                    for (int i = 0; i < listaData.Count(); i++)
                    {
                        listaData[i] = new string[cantPto];
                    }
                    int posHora = 0;
                    int posPtomedi = 0;

                    string[] listaHoraFormato = new string[numHoras];

                    for (int i = 0; i < listFechas.Count(); i++)
                    {
                        DateTime f = listFechas[i].Date;
                        for (int j = 0; j < 24; j++)
                        {
                            DateTime horaFila = f.AddHours(j);
                            listaHoraFormato[posHora] = horaFila.ToString(ConstantesAppServicio.FormatoFechaFull);
                            posHora++;
                        }
                    }

                    //fecha
                    row += 3;
                    var colIniTitulo = 1;
                    int sizeFont = 10;

                    using (ExcelRange r1 = wsHist.Cells[row, colIniTitulo, row + 4, cantPto + 1])
                    {
                        r1.Style.Font.Color.SetColor(Color.White);
                        r1.Style.Font.Size = sizeFont;
                        r1.Style.Font.Bold = true;
                        r1.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        r1.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        r1.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        //r1.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(23, 55, 93));
                        r1.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                        r1.Style.WrapText = true;
                    }


                    wsHist.Cells[row, colIniTitulo].Value = "CÓDIGO";
                    wsHist.Cells[row + 1, colIniTitulo].Value = "EMPRESA";
                    wsHist.Cells[row + 2, colIniTitulo].Value = "CENTRAL/EMBALSE/ESTACION";
                    wsHist.Cells[row + 3, colIniTitulo].Value = "MEDIDA";
                    wsHist.Cells[row + 4, colIniTitulo].Value = "FECHA/UNIDAD";

                    //empresas
                    var colEmpresa = colIniTitulo + 1;
                    var colGrupo = colIniTitulo + 1;
                    var colEquipo = colIniTitulo + 1;
                    foreach (var empcodi in listaEmpcodi)
                    {
                        var empnomb = listaCabecera.Where(x => x.Emprcodi == empcodi).FirstOrDefault().Emprnomb;
                        int cantPtoByEmp = listaCabecera.Where(x => x.Emprcodi == empcodi).Select(y => y.Ptomedicodi).Distinct().Count();
                        wsHist.Cells[row + 1, colEmpresa].Value = empnomb;
                        wsHist.Cells[row + 1, colEmpresa, row + 1, colEmpresa + cantPtoByEmp - 1].Merge = true;
                        colEmpresa += cantPtoByEmp;

                        //centrales x empresa
                        var listaCentralcodi = listaCabecera.Where(x => x.Emprcodi == empcodi).Select(y => y.Equicodi).Distinct().ToList();
                        foreach (var centralcodi in listaCentralcodi)
                        {
                            //pto medicion x grupo
                            var listaPtocodi = listaCabecera.Where(x => x.Emprcodi == empcodi && x.Equicodi == centralcodi).Select(y => new { y.Ptomedicodi, y.Repptoorden }).Distinct().OrderBy(y => y.Repptoorden).ToList();
                            foreach (var ptocodi in listaPtocodi)
                            {
                                var pto = listaCabecera.Where(x => x.Ptomedicodi == ptocodi.Ptomedicodi).FirstOrDefault();
                                var equinomb = pto.Equinomb;
                                var tipoPtonomb = pto.Tipoptomedinomb;
                                var tipoInfonomb = pto.Tipoinfoabrev;
                                var codigo = pto.Ptomedicodi;

                                wsHist.Cells[row + 2, colEquipo].Value = equinomb;
                                wsHist.Cells[row + 3, colEquipo].Value = tipoPtonomb;
                                wsHist.Cells[row + 4, colEquipo].Value = tipoInfonomb;
                                wsHist.Cells[row, colEquipo].Value = codigo; ;
                                colEquipo++;

                                posHora = 0;
                                foreach (var fecha in listFechas)
                                {
                                    MeMedicion24DTO ptoFecha = data24.Where(x => x.Ptomedicodi == ptocodi.Ptomedicodi && x.Medifecha.Date == fecha.Date).FirstOrDefault();

                                    if (ptoFecha != null)
                                    {
                                        for (int h = 0; h < 24; h++)
                                        {
                                            var valorHora = (decimal?)ptoFecha.GetType().GetProperty("H" + (h + 1).ToString()).GetValue(ptoFecha, null);
                                            listaData[posHora][posPtomedi] = valorHora + "";
                                            posHora++;
                                        }
                                    }
                                    else
                                    {
                                        for (int h = 0; h < 24; h++)
                                        {
                                            listaData[posHora][posPtomedi] = "";
                                            posHora++;
                                        }
                                    }
                                }

                                posPtomedi++;
                            }
                        }
                    }

                    //cuerpo
                    row += 5;
                    for (int i = 0; i < listaData.Count(); i++)
                    {
                        var fila = listaData[i];
                        wsHist.Cells[row, 1].Value = listaHoraFormato[i];
                        wsHist.Cells[row, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        for (int j = 0; j < fila.Count(); j++)
                        {
                            decimal? valorData = null;
                            if (fila[j] != null && fila[j] != "")
                            {
                                valorData = decimal.Parse(fila[j]);
                            }
                            wsHist.Cells[row, j + 2].Value = valorData;
                            wsHist.Cells[row, j + 2].Style.Numberformat.Format = @"0.000";
                        }
                        row++;
                    }

                    //bordes de celda
                    var borderTabla = wsHist.Cells[7, column, row - 1, column + cantPto].Style.Border;
                    borderTabla.Bottom.Style = borderTabla.Top.Style = borderTabla.Left.Style = borderTabla.Right.Style = ExcelBorderStyle.Hair;


                    #endregion

                    wsHist.Column(1).Width = 25;
                    for (int i = 2; i <= cantPto + 1; i++)
                    {
                        wsHist.Column(i).Width = 20;
                    }

                }
                else // Semanal
                {
                    ExcelWorksheet wsSem = xlPackage.Workbook.Worksheets.Add(ConstHidrologia.HojaReporteSemanalInfoAppPowel);

                    var data = model.ListaMedicion1;
                    var listaCabecera = model.Listacabecera;
                    #region Hoja Reporte Semanal
                    int row = 4;
                    int column = 1;
                    //imagen
                    HttpWebRequest request = (HttpWebRequest)System.Net.HttpWebRequest.Create(ConfigurationManager.AppSettings["LogoCoes"].ToString());
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                    System.Drawing.Image img = System.Drawing.Image.FromStream(response.GetResponseStream());
                    ExcelPicture picture = wsSem.Drawings.AddPicture("Imagen", img);
                    picture.From.Column = 0;
                    picture.From.Row = 1;
                    picture.To.Column = 1;
                    picture.To.Row = 2;
                    picture.SetSize(120, 60);


                    //Imprimir filtro
                    wsSem.Cells[row, column].Value = "Semana Inicio";
                    wsSem.Cells[row, column].Style.Border.BorderAround(ExcelBorderStyle.Thin);



                    wsSem.Cells[row, column + 1].Value = model.SemanaIni;
                    wsSem.Cells[row, column + 1].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                    wsSem.Cells[row + 1, column].Value = "Semana Fin";
                    wsSem.Cells[row + 1, column].Style.Border.BorderAround(ExcelBorderStyle.Thin);


                    wsSem.Cells[row + 1, column + 1].Value = model.SemanaFin;
                    wsSem.Cells[row + 1, column + 1].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                    ///Imprimimos cabecera de puntos de medicion

                    var listaEmpcodi = listaCabecera.Select(x => x.Emprcodi).Distinct().ToList();
                    int cantPto = listaCabecera.Select(x => x.Ptomedicodi).Distinct().Count();
                    List<DateTime> listFechas = new List<DateTime>();
                    for (var f = model.FechaIniPowel.Date; f <= model.FechaFinPowel; f = f.AddDays(1))
                    {
                        listFechas.Add(f);
                    }
                    cantPto = cantPto == 0 ? 1 : cantPto;
                    int numHoras = listFechas.Count;

                    string[][] listaData = new string[numHoras][];
                    for (int i = 0; i < listaData.Count(); i++)
                    {
                        listaData[i] = new string[cantPto];
                    }
                    int posHora = 0;
                    int posPtomedi = 0;

                    string[] listaHoraFormato = new string[numHoras];

                    for (int i = 0; i < listFechas.Count(); i++)
                    {
                        DateTime f = listFechas[i].Date;
                        listaHoraFormato[posHora] = f.ToString(Constantes.FormatoFecha);
                        posHora++;
                    }

                    //fecha
                    row += 3;
                    var colIniTitulo = 1;
                    int sizeFont = 10;

                    using (ExcelRange r1 = wsSem.Cells[row, colIniTitulo, row + 4, cantPto + 1])
                    {
                        r1.Style.Font.Color.SetColor(Color.White);
                        r1.Style.Font.Size = sizeFont;
                        r1.Style.Font.Bold = true;
                        r1.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        r1.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        r1.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        //r1.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(23, 55, 93));
                        r1.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                        r1.Style.WrapText = true;
                    }


                    wsSem.Cells[row, colIniTitulo].Value = "CÓDIGO";
                    wsSem.Cells[row + 1, colIniTitulo].Value = "EMPRESA";
                    wsSem.Cells[row + 2, colIniTitulo].Value = "CENTRAL/EMBALSE/ESTACION";
                    wsSem.Cells[row + 3, colIniTitulo].Value = "MEDIDA";
                    wsSem.Cells[row + 4, colIniTitulo].Value = "FECHA/UNIDAD";

                    //empresas
                    var colEmpresa = colIniTitulo + 1;
                    var colGrupo = colIniTitulo + 1;
                    var colEquipo = colIniTitulo + 1;
                    foreach (var empcodi in listaEmpcodi)
                    {
                        var empnomb = listaCabecera.Where(x => x.Emprcodi == empcodi).FirstOrDefault().Emprnomb;
                        int cantPtoByEmp = listaCabecera.Where(x => x.Emprcodi == empcodi).Select(y => y.Ptomedicodi).Distinct().Count();
                        wsSem.Cells[row + 1, colEmpresa].Value = empnomb;
                        wsSem.Cells[row + 1, colEmpresa, row + 1, colEmpresa + cantPtoByEmp - 1].Merge = true;
                        colEmpresa += cantPtoByEmp;

                        //equipos x empresa
                        var listaEquipocodi = listaCabecera.Where(x => x.Emprcodi == empcodi).Select(y => y.Equicodi).Distinct().ToList();
                        foreach (var equipocodi in listaEquipocodi)
                        {
                            //pto medicion x equipo
                            var listaPtocodi = listaCabecera.Where(x => x.Emprcodi == empcodi && x.Equicodi == equipocodi).Select(y => new { y.Ptomedicodi, y.Repptoorden }).Distinct().OrderBy(y => y.Repptoorden).ToList();
                            foreach (var ptocodi in listaPtocodi)
                            {
                                var pto = listaCabecera.Where(x => x.Ptomedicodi == ptocodi.Ptomedicodi).FirstOrDefault();
                                var equinomb = pto.Equinomb;
                                var tipoPtonomb = pto.Tipoptomedinomb;
                                var tipoInfonomb = pto.Tipoinfoabrev;
                                var codigo = pto.Ptomedicodi;

                                wsSem.Cells[row + 2, colEquipo].Value = equinomb;
                                wsSem.Cells[row + 3, colEquipo].Value = tipoPtonomb;
                                wsSem.Cells[row + 4, colEquipo].Value = tipoInfonomb;
                                wsSem.Cells[row, colEquipo].Value = codigo;
                                colEquipo++;

                                posHora = 0;
                                foreach (var fecha in listFechas)
                                {
                                    MeMedicion1DTO ptoFecha = data.Where(x => x.Ptomedicodi == ptocodi.Ptomedicodi && x.Medifecha.Date == fecha.Date).FirstOrDefault();

                                    if (ptoFecha != null)
                                    {

                                        var valorHora = (decimal?)ptoFecha.GetType().GetProperty("H" + (1).ToString()).GetValue(ptoFecha, null);
                                        listaData[posHora][posPtomedi] = valorHora + "";
                                        posHora++;

                                    }
                                    else
                                    {
                                        listaData[posHora][posPtomedi] = "";
                                        posHora++;
                                    }
                                }

                                posPtomedi++;
                            }
                        }
                    }

                    //cuerpo
                    row += 5;
                    for (int i = 0; i < listaData.Count(); i++)
                    {
                        var fila = listaData[i];
                        wsSem.Cells[row, 1].Value = listaHoraFormato[i];
                        wsSem.Cells[row, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        for (int j = 0; j < fila.Count(); j++)
                        {
                            decimal? valorData = null;
                            if (fila[j] != null && fila[j] != "")
                            {
                                valorData = decimal.Parse(fila[j]);
                            }
                            wsSem.Cells[row, j + 2].Value = valorData;
                            wsSem.Cells[row, j + 2].Style.Numberformat.Format = @"0.000";
                        }
                        row++;
                    }

                    //bordes de celda
                    var borderTabla = wsSem.Cells[7, column, row - 1, column + cantPto].Style.Border;
                    borderTabla.Bottom.Style = borderTabla.Top.Style = borderTabla.Left.Style = borderTabla.Right.Style = ExcelBorderStyle.Hair;


                    #endregion

                    wsSem.Column(1).Width = 25;
                    for (int i = 2; i <= cantPto + 1; i++)
                    {
                        wsSem.Column(i).Width = 20;
                    }

                }

                fileExcel = System.IO.Path.GetTempFileName();

                xlPackage.SaveAs(new FileInfo(fileExcel));
            }

            return fileExcel;
        }

        #endregion

        #region Reporte Proyección Hidrológica - REQ 2023-000403

        /// <summary>
        /// Index Reporte Proyección Hidrológica
        /// </summary>
        /// <returns></returns>
        public ActionResult IndexProyeccionHidrologica()
        {
            HidrologiaModel model = new HidrologiaModel();
            model.ListaFormato = logic.ListarLecturaRptProyHidro();
            model.FechaInicio = DateTime.Today.ToString(Constantes.FormatoFecha);
            model.FechaFin = DateTime.Today.ToString(Constantes.FormatoFecha);
            model.Anho = DateTime.Today.Year.ToString();
            return View(model);
        }

        /// <summary>
        /// Generar reporte excel
        /// </summary>
        /// <param name="formatcodi"></param>
        /// <param name="fechaInicial"></param>
        /// <param name="fechaFinal"></param>
        /// <param name="anhoIni"></param>
        /// <param name="semanaIni"></param>
        /// <param name="anhoFin"></param>
        /// <param name="semanaFin"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GenerarArchivoReporteProyeccionHidrologica(int formatcodi, string fechaInicial, string fechaFinal
                            , int anhoIni, int semanaIni, int anhoFin, int semanaFin)
        {
            HidrologiaModel model = new HidrologiaModel();
            string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesHidrologia.FolderReporte;

            try
            {
                DateTime fechaIni = DateTime.Today;
                DateTime fechaFin = DateTime.Today;
                if (formatcodi == 35 || formatcodi == 36) //mensual
                {
                    fechaIni = COES.Base.Tools.Util.FormatFecha(fechaInicial);
                    fechaFin = COES.Base.Tools.Util.FormatFecha(fechaFinal);
                }
                if (formatcodi == 30)  //semanal
                {
                    if (anhoIni <= 0 || semanaIni <= 0) throw new ArgumentException("Debe seleccionar 'fecha desde'");
                    if (anhoFin <= 0 || semanaFin <= 0) throw new ArgumentException("Debe seleccionar 'fecha hasta'");
                    fechaIni = EPDate.f_fechainiciosemana(anhoIni, semanaIni);
                    fechaFin = EPDate.f_fechainiciosemana(anhoFin, semanaFin);
                }

                if (formatcodi == 31) //diario
                {
                    fechaIni = DateTime.ParseExact(fechaInicial, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                    fechaFin = DateTime.ParseExact(fechaFinal, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                }

                //validación
                if (fechaFin < fechaIni) throw new ArgumentException("La 'fecha hasta' debe ser posterior a 'fecha desde'.");
                if (fechaFin > fechaIni.AddYears(2).AddDays(-1)) throw new ArgumentException("El rango de tiempo no puede ser mayor a 2 años.");

                //reporte
                logic.GenerarReporteProyHidro(ruta, formatcodi, fechaIni, fechaFin, out string nombreArchivo);
                model.Resultado = nombreArchivo;
            }
            catch (Exception ex)
            {
                log.Error(NameController, ex);
                model.Resultado = "-1";
                model.Mensaje = ex.Message;
                model.Detalle = ex.StackTrace;
            }

            return Json(model);
        }

        /// <summary>
        /// Descarga el archivo excel
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public virtual FileResult ExportarProyeccionHidrologica()
        {
            string nombreArchivo = Request["file_name"];
            string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesHidrologia.FolderReporte;
            string strArchivoTemporal = ruta + nombreArchivo;

            byte[] buffer = null;

            if (System.IO.File.Exists(strArchivoTemporal))
            {
                buffer = System.IO.File.ReadAllBytes(strArchivoTemporal);
                System.IO.File.Delete(strArchivoTemporal);
            }

            return File(buffer, System.Net.Mime.MediaTypeNames.Application.Octet, nombreArchivo);
        }

        #endregion

        #region UTIL

        /// <summary>
        /// Permite exportar el reporte general
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public virtual ActionResult ExportarReporte(int tipoReporte, string fechaInicio, string fechaFinal)
        {
            DateTime fechaIni = DateTime.MinValue;
            DateTime fechaFin = DateTime.MinValue;

            if (!string.IsNullOrEmpty(fechaInicio)) fechaIni = DateTime.ParseExact(fechaInicio, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            if (!string.IsNullOrEmpty(fechaFinal)) fechaFin = DateTime.ParseExact(fechaFinal, Constantes.FormatoFecha, CultureInfo.InvariantCulture);

            var FechaRepInicio = fechaIni.ToString(Constantes.FormatoF);
            var FechaRepFinal = fechaFin.ToString(Constantes.FormatoF);

            string nombreArchivoFinal = "";
            string nombreArchivo = string.Empty;

            switch (tipoReporte)
            {
                case 0:
                    nombreArchivo = ConstantesHidrologia.NombreReporteHidrologia00;//Reporte Mensual, Semanal-diario
                    nombreArchivoFinal = nombreArchivo;
                    break;
                case 1:
                    nombreArchivo = ConstantesHidrologia.NombreRptGraficoHidrologia00; // Reporte Grafico Mensual
                    nombreArchivoFinal = nombreArchivo;
                    break;
                case 2:
                    if (FechaRepInicio == FechaRepFinal)
                    {
                        nombreArchivo = ConstantesHidrologia.NombreReporteHidrologia01;
                        nombreArchivoFinal = String.Format(ConstantesHidrologia.NombreReporteHidrologia01, FechaRepInicio); // Reporte Ejecutado Historico/Tr Horas
                    }
                    else
                    {
                        nombreArchivo = ConstantesHidrologia.NombreReporteHidrologia001;
                        nombreArchivoFinal = String.Format(ConstantesHidrologia.NombreReporteHidrologia001, FechaRepInicio, FechaRepFinal);
                    }
                    break;
                case 3:
                    nombreArchivo = ConstantesHidrologia.NombreRptGraficoHidrologia01; // Reporte Grafico Diario - horas
                    nombreArchivoFinal = nombreArchivo;
                    break;
                case 4:
                    nombreArchivo = ConstantesHidrologia.NombreReporteHidrologia02; // Reporte Año - Semana
                    nombreArchivoFinal = nombreArchivo;
                    break;
                case 5:
                    nombreArchivo = ConstantesHidrologia.NombreRptGraficoHidrologia02; // Reporte grafico Año - semana
                    nombreArchivoFinal = nombreArchivo;
                    break;
                case 6:
                    nombreArchivo = ConstantesHidrologia.NombreReporteHidrologia03; // Reporte Semanal-Dia
                    nombreArchivoFinal = nombreArchivo;
                    break;
                case 7:
                    nombreArchivo = ConstantesHidrologia.NombreRptGraficoHidrologia03; // Reporte grafico Semanal-Dia
                    nombreArchivoFinal = nombreArchivo;
                    break;
                case 8:
                    nombreArchivo = ConstantesHidrologia.NombreRptGraficoHidrologia04; // Reporte grafico Diario - dias
                    nombreArchivoFinal = nombreArchivo;
                    break;
                case 9:
                    nombreArchivo = ConstantesHidrologia.NombreReporteHidrologia05; // Reporte grafico Diario - dias
                    nombreArchivoFinal = nombreArchivo;
                    break;
                case 10:
                    nombreArchivo = ConstantesHidrologia.NombreReporteHidrologia01;
                    nombreArchivoFinal = ConstantesHidrologia.NombreReporteHidrologia06; // Reporte grafico Diario - dias                    
                    break;
            }
            string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesHidrologia.FolderReporte;
            string fullPath = ruta + nombreArchivo;
            return File(fullPath, Constantes.AppExcel, nombreArchivoFinal);
        }

        /// <summary>
        /// Carga lista de Semanas
        /// </summary>
        /// <param name="idAnho"></param>
        /// <returns></returns>
        public PartialViewResult CargarSemanas(string idAnho)
        {
            HidrologiaModel model = new HidrologiaModel();
            List<TipoInformacion> entitys = new List<TipoInformacion>();
            DateTime dfecha = new DateTime(Int32.Parse(idAnho), 12, 31);
            int nsemanas = COES.Base.Tools.Util.ObtenerNroSemanasxAnho(dfecha, FirstDayOfWeek.Saturday);

            for (int i = 1; i <= nsemanas; i++)
            {
                TipoInformacion reg = new TipoInformacion();
                reg.IdTipoInfo = i;
                reg.NombreTipoInfo = "Sem" + i + "-" + idAnho;
                entitys.Add(reg);

            }
            model.ListaSemanas = entitys;
            return PartialView(model);
        }

        /// <summary>
        /// Carga lista de Semanas
        /// </summary>
        /// <param name="idAnho"></param>
        /// <returns></returns>
        public PartialViewResult CargarSemanas2(string idAnho)
        {
            HidrologiaModel model = new HidrologiaModel();
            List<TipoInformacion> entitys = new List<TipoInformacion>();
            DateTime dfecha = new DateTime(Int32.Parse(idAnho), 12, 31);
            int nsemanas = COES.Base.Tools.Util.ObtenerNroSemanasxAnho(dfecha, FirstDayOfWeek.Saturday);

            for (int i = 1; i <= nsemanas; i++)
            {
                TipoInformacion reg = new TipoInformacion();
                reg.IdTipoInfo = i;
                reg.NombreTipoInfo = "Sem" + i + "-" + idAnho;
                entitys.Add(reg);

            }
            model.ListaSemanas = entitys;
            return PartialView(model);
        }

        #endregion

    }
}
