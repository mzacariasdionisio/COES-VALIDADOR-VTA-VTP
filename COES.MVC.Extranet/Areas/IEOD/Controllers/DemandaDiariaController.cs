using COES.Dominio.DTO.Sic;
using COES.Framework.Base.Tools;
using COES.MVC.Extranet.Areas.IEOD.Models;
using COES.MVC.Extranet.Controllers;
using COES.MVC.Extranet.Helper;
using COES.MVC.Extranet.Models;
using COES.Servicios.Aplicacion.FormatoMedicion;
using COES.Servicios.Aplicacion.IEOD;
using COES.Servicios.Aplicacion.PronosticoDemanda;
using COES.Servicios.Aplicacion.PronosticoDemanda.Helper;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Globalization;

namespace COES.MVC.Extranet.Areas.IEOD.Controllers
{
    public class DemandaDiariaController : FormatoController
    {
        #region Declaración de variables

        private static readonly ILog Log = log4net.LogManager.GetLogger(typeof(DemandaDiariaController));
        private static string NameController = MethodBase.GetCurrentMethod().DeclaringType.Name;
        protected bool EsUsuarioLibre = false;

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

        /// <summary>
        /// Instancia de la clase de servicio
        /// </summary>
        PronosticoDemandaAppServicio servPronostico = new PronosticoDemandaAppServicio();
        #endregion

        #region METODOS DEMANDA DIARIA

        /// <summary>
        /// Index de Demanda diaria
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            if (!base.IsValidSesion) return base.RedirectToLogin();
            if (this.IdModulo == null) return base.RedirectToHomeDefault();

            DemandaDiariaModel model = new DemandaDiariaModel();
            model.ListaFormato = this.servFormato.ListarFormatosDemandaDiaria();

            return View(model);
        }

        /// <summary>
        /// Hojas segun el formato
        /// </summary>
        /// <param name="formatcodi"></param>
        /// <returns></returns>
        public JsonResult CargarFormato(int formatcodi)
        {
            DemandaDiariaModel model = this.GenerarValoresDefecto(formatcodi, this.EsUsuarioLibre);
            return Json(model);
        }

        /// <summary>
        /// Devuelve Vista Parcial para cada hoja Padre
        /// </summary>
        /// <returns></returns>
        public PartialViewResult ViewHojaPadreCargaDatos(int idFormato, int? idHojaPadre = 0)
        {
            DemandaDiariaModel modelHoja = this.GenerarValoresDefecto(idFormato, this.EsUsuarioLibre);
            MeHojaDTO hoja = base.servFormato.GetByIdMeHoja(idHojaPadre.Value);
            /*
             * ASSETEC 201909: AGREGAMOS LA CONDICIÓNEN DONDE NO EXISTE HOJA PADRE
             */
            if (hoja != null)
            {
                modelHoja.IdHoja = hoja.Hojacodi;
                modelHoja.NombreHoja = hoja.Hojanombre;
            }
            return PartialView(modelHoja);
        }

        /// <summary>
        /// Devuelve Vista Parcial para cada hoja
        /// </summary>
        /// <returns></returns>
        public PartialViewResult ViewHojaCargaDatos(int idHoja, int idFormato)
        {
            DemandaDiariaModel modelHoja = new DemandaDiariaModel();
            MeHojaDTO hoja = base.servFormato.GetByIdMeHoja(idHoja);

            modelHoja.IdHoja = hoja.Hojacodi;
            modelHoja.NombreHoja = hoja.Hojanombre;

            return PartialView(modelHoja);
        }

        /// <summary>
        /// Metodo llamado desde cliente web para consultar el formato excel web de Demanda diaria
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <param name="idEnvio"></param>
        /// <param name="fecha"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult MostrarGrilla(int idEmpresa, int idEnvio, string fecha, string semana, string mes, int idFormato, int verUltimoEnvio, int? idHoja = 0)
        {
            List<MeHojaptomedDTO> entitys = this.servFormato.ObtenerPtosXFormato(idFormato, idEmpresa);

            if (entitys.Count > 0)
            {
                DemandaDiariaModel jsModel = BuildHojaExcelDemandaDiaria(idEmpresa, idEnvio, fecha, semana, mes, idFormato, false, verUltimoEnvio, idHoja.Value);
                var jsonResult = Json(jsModel, JsonRequestBehavior.DenyGet);
                jsonResult.MaxJsonLength = int.MaxValue;
                return jsonResult;
            }
            else
            {
                return Json(-1);
            }
        }

        /// <summary>
        /// Devuelve el model con informacion de Demanda Diaria
        /// </summary>sic
        /// <param name="idEmpresa"></param>
        /// <param name="idEnvio"></param>
        /// <param name="fecha"></param>
        /// <returns></returns>
        public DemandaDiariaModel BuildHojaExcelDemandaDiaria(int idEmpresa, int idEnvio, string fecha, string semana, string mes, int idFormato, bool opGrabar, int verUltimoEnvio, int idHoja)
        {
            DemandaDiariaModel model = new DemandaDiariaModel();
            model.UtilizaHoja = true;
            model.IdEmpresa = idEmpresa;
            model.Semana = semana;
            model.Mes = mes;
            model.IdHojaPadre = idHoja > 0 ? idHoja : 0;

            this.BuildHojaExcel(model, idEmpresa, idEnvio, fecha, idFormato, verUltimoEnvio);

            model.ListaFamilia = model.ListaHojaPto.GroupBy(x => new { x.Famcodi, x.Famabrev }).Select(grp => new EqFamiliaDTO { Famcodi = grp.Key.Famcodi, Famabrev = grp.Key.Famabrev }).ToList();
            model.ListaAreaOperativa = model.ListaHojaPto.Select(y => y.AreaOperativa).Distinct().OrderBy(x => x).ToList();
            model.ListaSubestacion = model.ListaHojaPto.GroupBy(x => new { x.Areacodi, x.Areanomb }).Select(grp => new EqAreaDTO { Areacodi = grp.Key.Areacodi, Areanomb = grp.Key.Areanomb }).OrderBy(x => x.Areanomb).ToList();
            //ASSETEC 201909: apuntamos a la tabla prn_motivo 
            model.ListaCausaJustificacion = servPronostico.GetListaJustificacion(); //base.servFormato.GetListaJustificacion();

            this.GetListaCongeladoByIdEnvio(model, idEnvio);

            return model;
        }

        /// <summary>
        /// Graba los datos enviados por el agente del formato Demanda Diaria
        /// </summary>
        /// <param name="dataExcel"></param>
        /// <param name="idFormato"></param>
        /// <param name="idEmpresa"></param>
        /// <param name="fecha"></param>
        /// <param name="semana"></param>
        /// <param name="mes"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GrabarExcelWeb(int idEmpresa, string fecha, string semana, string mes, List<Congelado> listaJustificacion, int idFormato, List<int> listaHoja, List<string[][]> listaData, int? idHoja = 0)
        {
            FormatoModel model = new FormatoModel();
            model.Handson = new HandsonModel();
            model.IdEmpresa = idEmpresa;
            model.Fecha = fecha;
            model.Semana = semana;
            model.Mes = mes;
            model.IdHojaPadre = idHoja.Value;
            model.IdFormato = idFormato;

            model.UtilizaHoja = true;
            model.OpGrabar = true;
            model.ListaHoja = listaHoja;
            model.ListaData = listaData;

            MeFormatoDTO formato = servFormato.GetByIdMeFormato(idFormato);

            //establecer datos de la justificacion de los datos congelados
            List<MeJustificacionDTO> listaMeJustificacion = new List<MeJustificacionDTO>();
            if (listaJustificacion != null && listaJustificacion.Count > 0)
            {
                for (int i = 0; i < listaJustificacion.Count; i++)
                {
                    Congelado c = listaJustificacion[i];
                    MeJustificacionDTO mj = new MeJustificacionDTO();
                    //ASSETEC 201909: mj.Justcodi = ConstantesProdem.IdCausaJustificacion;
                    if (idFormato == ConstantesProdem.FormatcodiDemandaDiaria) //95 o 72
                    {
                        //Diario
                        DateTime dFechaEnvio = DateTime.ParseExact(fecha, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                        if (dFechaEnvio > c.FechaInicio)
                            mj.Lectcodi = ConstantesProdem.LectcodiDemEjecDiario;
                        else
                            mj.Lectcodi = ConstantesProdem.LectcodiDemPrevDiario;
                    }
                    mj.Justcodi = ConstantesProdem.IdCausaJustificacion;
                    //-----------------------------------------------------------
                    mj.Ptomedicodi = c.Ptomedicodi;
                    mj.Subcausacodi = c.Justificacion;
                    mj.Justdescripcionotros = c.Texto;
                    mj.Justfechainicio = c.FechaInicio;
                    mj.Justfechafin = c.FechaFin;

                    listaMeJustificacion.Add(mj);
                }
            }

            model.ListaJustificacion = listaMeJustificacion;
            model.ValidaSimilitudDataPeriodoAnt = true;

            FormatoResultado modelResultado = GrabarExcelWeb(model);

            if (idFormato == ConstantesProdem.FormatcodiDemandaDiaria || idFormato == ConstantesProdem.FormatcodiDemandaSemanalDistrib
                || idFormato == ConstantesProdem.FormatcodiDemandaMensualDistrib)
            {
                modelResultado.sAplicacion = "DEMANDADIARIA";
                modelResultado.IdHojaPadre = model.IdHojaPadre;
            }
            return Json(modelResultado);
        }

        /// <summary>
        /// Permite generar el formato en formato excel de Demanda diaria
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public string GenerarFormato(int idEmpresa, string fecha, string semana, string mes, int idFormato, List<int> listaHoja, List<string[][]> listaData, int? idHoja = 0)
        {
            string ruta = string.Empty;
            try
            {
                int idEnvio = -1;
                this.ListaHoja = listaHoja;
                this.ListaMatrizExcel = listaData;
                DemandaDiariaModel model = BuildHojaExcelDemandaDiaria(idEmpresa, idEnvio, fecha, semana, mes, idFormato, true, ConstantesFormato.NoVerUltimoEnvio, idHoja.Value);
                ruta = ToolsFormato.GenerarFileExcelFormato(model);
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                ruta = "-1";
            }
            return ruta;
        }

        /// <summary>
        /// Congelados por día
        /// </summary>
        /// <param name="model"></param>
        /// <param name="idEnvio"></param>
        private void GetListaCongeladoByIdEnvio(DemandaDiariaModel model, int idEnvio)
        {
            int idEnvioConsulta = idEnvio != 0 ? idEnvio : model.IdEnvioLast;
            List<Congelado> lista = new List<Congelado>();
            var listaJustificacion = servFormato.ListMeJustificacionsByEnvio(idEnvioConsulta);
            foreach (var mj in listaJustificacion)
            {
                EveSubcausaeventoDTO m = model.ListaCausaJustificacion.Find(x => x.Subcausacodi == mj.Subcausacodi);

                Congelado c = new Congelado();
                c.Ptomedicodi = mj.Ptomedicodi.Value;
                c.Justificacion = mj.Subcausacodi.Value;
                c.SubcausacodiDesc = m != null ? m.Subcausadesc : string.Empty;
                c.Texto = mj.Justdescripcionotros != null ? mj.Justdescripcionotros : "";
                c.FechaInicio = mj.Justfechainicio.Value;
                c.FechaFin = mj.Justfechafin.Value;

                c.Periodo = c.FechaInicio.ToString(Constantes.FormatoFechaHora) + " - " + c.FechaFin.ToString(Constantes.FormatoFechaHora);

                var hojaPto = model.ListaHojaPto.Where(x => x.Ptomedicodi == c.Ptomedicodi).FirstOrDefault();
                if (hojaPto == null) continue;
                c.Empresa = hojaPto.Emprabrev;
                c.Subestacion = hojaPto.Areanomb;
                c.Equipo = hojaPto.Equinomb;

                lista.Add(c);
            }

            model.ListaCongeladoByEnvio = lista;
        }

        /// <summary>
        /// Lista de Semana por Año
        /// </summary>
        /// <param name="idAnho"></param>
        /// <returns></returns>
        public PartialViewResult CargarSemanas(string idAnho)
        {
            DemandaDiariaModel model = new DemandaDiariaModel();
            List<GenericoDTO> entitys = new List<GenericoDTO>();
            if (idAnho == "0")
            {
                idAnho = DateTime.Now.Year.ToString();
            }
            DateTime dfecha = new DateTime(Int32.Parse(idAnho), 12, 31);
            int nsemanas = EPDate.TotalSemanasEnAnho(Int32.Parse(idAnho), FirstDayOfWeek.Saturday);

            for (int i = 1; i <= nsemanas; i++)
            {
                GenericoDTO reg = new GenericoDTO();
                reg.Entero1 = i;
                reg.String1 = "Sem" + i + "-" + idAnho;
                entitys.Add(reg);

            }
            model.ListaGenSemanas = entitys;
            return PartialView(model);
        }

        /// <summary>
        /// Indica la fecha por defecto de cada formato
        /// </summary>
        /// <param name="idFormato"></param>
        /// <returns></returns>
        public JsonResult SetearFechasEnvio(int idFormato)
        {
            var formato = servFormato.GetByIdMeFormato(idFormato);
            string mes = string.Empty;
            string fecha = string.Empty;
            int semana = 0;
            int anho = 0;
            int tipo = formato.Lecttipo;
            if (formato.Formatdiaplazo == 0)
                tipo = 0;
            if (formato != null)
                ToolsFormato.GetFechaActualEnvio((int)formato.Formatperiodo, tipo, ref mes, ref fecha, ref semana, ref anho);
            var jason = new
            {
                mes = mes,
                fecha = fecha,
                semana = semana,
                anho = anho
            };
            return Json(jason);
        }

        /// <summary>
        /// Generar valor por defecto
        /// </summary>
        /// <param name="model"></param>
        private DemandaDiariaModel GenerarValoresDefecto(int formatcodi,bool usuarioLibres)
        {
            DemandaDiariaModel model = new DemandaDiariaModel();
            base.IndexFormato(model, formatcodi);

            model.ListaEmpresas = model.ListaEmpresas.Where(x => x.Tipoemprcodi == 4 || x.Tipoemprcodi == 2 || x.Tipoemprcodi == 3).ToList();

            //fechas
            DateTime fechaActual = DateTime.Now;
            int nroSemana = EPDate.f_numerosemana(DateTime.Now);
            
            if (DateTime.Now.Year.ToString() != model.Anho) model.NroSemana = int.Parse(model.Semana);
            else model.NroSemana = nroSemana;

            model.Dia = DateTime.Now.ToString(Constantes.FormatoFecha);
            model.Mes = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(-1).ToString("MM yyyy");

            //Hojas
            model.ListaMeHoja = base.servFormato.GetByCriteriaMeHoja(formatcodi);
            model.ListaMeHojaPadre = this.servFormato.ListHojaPadre(formatcodi);

            return model;
        }

        #endregion

    }
}
