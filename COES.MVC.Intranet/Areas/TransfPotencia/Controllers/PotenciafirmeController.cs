using COES.Base.Core;
using COES.Dominio.DTO.Sic;
using COES.Framework.Base.Tools;
using COES.MVC.Intranet.Areas.TransfPotencia.Helper;
using COES.MVC.Intranet.Areas.TransfPotencia.Models;
using COES.MVC.Intranet.Controllers;
using COES.MVC.Intranet.Models;
using COES.Servicios.Aplicacion.FormatoMedicion;
using COES.Servicios.Aplicacion.Helper;
using COES.Servicios.Aplicacion.Indisponibilidades;
using COES.Servicios.Aplicacion.TransfPotencia;
using log4net;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.TransfPotencia.Controllers
{
    public class PotenciafirmeController : BaseController
    {
        //
        // GET: /TransfPotencia/Potenciafirme/
        TransfPotenciaAppServicio servicio = new TransfPotenciaAppServicio();
        FormatoMedicionAppServicio servFormato = new FormatoMedicionAppServicio();

        private static readonly ILog Log = log4net.LogManager.GetLogger(typeof(PotenciafirmeController));
        private static string NameController = "PotenciafirmeController";

        /// <summary>
        /// Protected de log de errores page
        /// </summary>
        /// <param name="filterContext"></param>
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
        /// listado ventos del controller
        /// </summary>
        public PotenciafirmeController()
        {
        }

        //#region Historico Indisponibilidades

        //public ActionResult GeneraHistoricoIndispo()
        //{
        //    PotenciafirmeModel model = new PotenciafirmeModel();

        //    model.Mes = DateTime.Now.ToString("MM yyyy");
        //    return View(model);
        //}

        //public JsonResult ConsultaIndispo(string fecha)
        //{
        //    base.ValidarSesionJsonResult();

        //    PotenciafirmeModel model = new PotenciafirmeModel();
        //    int opcion = 2;

        //    List<IndDetcuadro7DTO> list_IndDetcuadro7 = new List<IndDetcuadro7DTO>();
        //    DateTime MesIni = DateTime.MinValue, MesFin = DateTime.MinValue;
        //    if (fecha != null)
        //    {
        //        fecha = ConstantesAppServicio.IniDiaFecha + fecha.Replace(" ", "/");
        //        MesIni = DateTime.ParseExact(fecha, ConstantesBase.FormatoFechaBase, CultureInfo.InvariantCulture);
        //        MesFin = DateTime.ParseExact(fecha, ConstantesBase.FormatoFechaBase, CultureInfo.InvariantCulture).AddMonths(1);
        //    }

        //    var veri = srvIndis.ListIndPeriodocuadro7s().Where(x => x.Percu7mesini.Date == MesIni.Date && x.Percu7mesfin.Date == MesFin.Date && x.Percu7modofiltro == opcion && x.Percu7estado == "A").ToList();
        //    if (veri.Count == 0)
        //    {
        //        model.IdEnvio = this.srvIndis.GuardarReporteCuadro7(opcion, 0, 0, fecha, fecha, 0, 0, 0, User.Identity.Name);
        //    }
        //    else
        //    {
        //        model.IdEnvio = veri[0].Percu7codi;
        //    }
        //    var unidades = srvIndis.ListarUnidadesGenXTipogeneracion(ConstantesIndisponibilidades.TgenercodiTermoelectrica, MesIni, MesFin,  ConstantesAppServicio.ParametroDefecto, ConstantesAppServicio.ParametroDefecto, ConstantesAppServicio.ParametroDefecto);
        //    list_IndDetcuadro7 = srvIndis.GetCargarViewCuadro7(model.IdEnvio).ToList();

        //    model.Nregistros = list_IndDetcuadro7.Count;
        //    model.Resultado = servicio.ConsultaIndispoHtml(list_IndDetcuadro7, unidades, MesIni);

        //    return Json(model);
        //}

        //public JsonResult SaveIndispo(string fecha)
        //{
        //    PotenciafirmeModel model = new PotenciafirmeModel();
        //    List<IndDetalleMesDTO> Lista = new List<IndDetalleMesDTO>();

        //    int percu7codi = 0, opcion = 2;
        //    DateTime MesIni = DateTime.MinValue, MesFin = DateTime.MinValue;
        //    if (fecha != null)
        //    {
        //        fecha = ConstantesAppServicio.IniDiaFecha + fecha.Replace(" ", "/");
        //        MesIni = DateTime.ParseExact(fecha, ConstantesBase.FormatoFechaBase, CultureInfo.InvariantCulture);
        //        MesFin = DateTime.ParseExact(fecha, ConstantesBase.FormatoFechaBase, CultureInfo.InvariantCulture).AddMonths(1);
        //    }

        //    var veri = srvIndis.ListIndPeriodocuadro7s().Where(x => x.Percu7mesini.Date == MesIni.Date && x.Percu7mesfin.Date == MesFin.Date && x.Percu7modofiltro == opcion && x.Percu7estado == "A").ToList();
        //    if (veri.Count > 0)
        //    {
        //        percu7codi = veri[0].Percu7codi;
        //        var list_IndDetcuadro7 = srvIndis.GetCargarViewCuadro7(percu7codi).ToList();

        //        //Si ya existe data se elimina
        //        var getData = servicio.GetByCriteriaIndDetalleMes(MesIni.Year, MesIni.Month);
        //        if (getData.Count > 0) { servicio.DeleteIndDetalleMes(MesIni.Year, MesIni.Month); }

        //        int lastId = servicio.GetMaxIndDetalleMes();
        //        foreach (var d in list_IndDetcuadro7)
        //        {
        //            Lista.Add(new IndDetalleMesDTO()
        //            {
        //                Detmescodi = lastId++,
        //                Emprcodi = d.Emprcodi,
        //                Equicodi = d.Equicodi,
        //                Detmeship = d.Cuadr7hip,
        //                Detmeshif = d.Cuadr7hif,
        //                Detmesanno = MesIni.Year,
        //                Detmesmes = MesIni.Month,
        //                Detmesusucreacion = User.Identity.Name,
        //                Detmesfeccreacion = DateTime.Now,
        //                Detmesusumodificacion = User.Identity.Name,
        //                Detmesfecmodificacion = DateTime.Now
        //            });
        //        }
        //    }

        //    try
        //    {
        //        if (Lista.Count > 0)
        //        {
        //            servicio.SaveMasivoIndDetalleMes(Lista);
        //        }

        //        model.Nregistros = 1;
        //    }
        //    catch
        //    {
        //        model.Nregistros = -1;
        //    }

        //    return Json(model);
        //}

        //#endregion

        //#region Indisponibilidad Teorica

        //public ActionResult RegCatpropiedad()
        //{
        //    PotenciafirmeModel model = new PotenciafirmeModel();

        //    model.ListaEqCategoria = servicio.GetByCriteriaEqCategorias();

        //    return View(model);
        //}

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="id"></param>
        ///// <param name="descrip"></param>
        ///// <returns></returns>
        //public PartialViewResult PopupNuevoRegCatpropiedad(int id, string descrip)
        //{
        //    PotenciafirmeModel model = new PotenciafirmeModel();

        //    model.ObjEqCategoria = new EqCategoriaDTO();
        //    model.ObjEqCategoria.Ctgcodi = id;
        //    model.ObjEqCategoria.Ctgnomb = descrip;

        //    return PartialView(model);
        //}

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="ctgcodi"></param>
        ///// <param name="url"></param>
        ///// <returns></returns>
        //public JsonResult ConsultaRegCatpropiedad(int ctgcodi, string url)
        //{
        //    PotenciafirmeModel model = new PotenciafirmeModel();

        //    var lista = servicio.GetByCriteriaEqCatpropiedads(ctgcodi);
        //    model.Resultado = servicio.ListaIndispoTeoricaHtml(lista, url);

        //    return Json(model);
        //}

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="nam"></param>
        ///// <param name="ctgcodi"></param>
        ///// <returns></returns>
        //public JsonResult SaveRegCatpropiedad(string nam, int ctgcodi)
        //{
        //    PotenciafirmeModel model = new PotenciafirmeModel();

        //    try
        //    {
        //        servicio.SaveEqCatpropiedad(new EqCatpropiedadDTO()
        //        {
        //            Eqcatpnomb = nam.ToUpper(),
        //            Eqcatpusucreacion = User.Identity.Name,
        //            Eqcatpfeccreacion = DateTime.Now,
        //            Eqcatpusumodificacion = User.Identity.Name,
        //            Eqcatpfecmodificacion = DateTime.Now,
        //            Eqcatpestado = ConstantesAppServicio.Activo,
        //            Ctgcodi = ctgcodi
        //        });

        //        model.Nregistros = 1;
        //    }
        //    catch
        //    {
        //        model.Nregistros = -1;
        //    }

        //    return Json(model);
        //}

        ///// <summary>
        ///// Detalle Indisponibilidad Teorica detalle
        ///// </summary>
        ///// <param name="id"></param>
        ///// <returns></returns>
        //public ActionResult RegCatpropiedadValor()
        //{
        //    PotenciafirmeModel model = new PotenciafirmeModel();

        //    model.ListaEqCategoria = servicio.GetByCriteriaEqCategorias();

        //    return View(model);
        //}

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="ctgcodi"></param>
        ///// <returns></returns>
        //public JsonResult CargarCategoriaDet(int ctgcodi)
        //{
        //    PotenciafirmeModel model = new PotenciafirmeModel();

        //    model.ListaEqCategoriaDet = servicio.GetByCriteriaEqCategoriaDet(ctgcodi);

        //    return Json(model);
        //}

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="ctgcodi"></param>
        ///// <returns></returns>
        //public JsonResult CargarPropiedadValor(int ctgcodi)
        //{
        //    PotenciafirmeModel model = new PotenciafirmeModel();

        //    model.ListaEqCatpropiedad = servicio.GetByCriteriaEqCatpropiedads(ctgcodi);

        //    return Json(model);
        //}

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="eqcatpcodi"></param>
        ///// <param name="descrip"></param>
        ///// <returns></returns>
        //public PartialViewResult PopupNuevoRegCatpropiedadValor(int idCat, int id, string descrip)
        //{
        //    PotenciafirmeModel model = new PotenciafirmeModel();

        //    model.ObjEqCategoriaDet = new EqCategoriaDetDTO();
        //    model.ObjEqCategoriaDet.Ctgdetcodi = id;
        //    model.ObjEqCategoriaDet.Ctgdetnomb = descrip;

        //    model.ListaEqCatpropiedad = servicio.GetByCriteriaEqCatpropiedads(idCat);

        //    model.Fecha = DateTime.Now.ToString(ConstantesBase.FormatoFechaBase);

        //    return PartialView(model);
        //}

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="Eqcatpcodi"></param>
        ///// <returns></returns>
        //public JsonResult ConsultaRegCatpropiedadValor(int ctgdetcodi, string url)
        //{
        //    PotenciafirmeModel model = new PotenciafirmeModel();

        //    var lista = servicio.GetByCriteriaEqCatpropvalors(ctgdetcodi).OrderBy(x => x.Eqctpvfechadat).ToList();
        //    model.Resultado = servicio.ListaIndispoTeoricaDetHtml(lista, url);

        //    return Json(model);
        //}

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <returns></returns>
        //public JsonResult SaveRegCatpropiedadValor(string eqctpvvalor, int eqcatpcodi, int ctgdetcodi, string fecha)
        //{
        //    PotenciafirmeModel model = new PotenciafirmeModel();

        //    DateTime f_ = DateTime.ParseExact(fecha, ConstantesAppServicio.FormatoFecha, CultureInfo.CurrentCulture);

        //    try
        //    {
        //        servicio.SaveEqCatpropvalor(new EqCatpropvalorDTO()
        //        {
        //            Eqctpvvalor = decimal.Parse(eqctpvvalor),
        //            Eqctpvusucreacion = User.Identity.Name,
        //            Eqctpvfeccreacion = DateTime.Now,
        //            Eqctpvfechadat = f_,
        //            Eqcatpcodi = eqcatpcodi,
        //            Ctgdetcodi = ctgdetcodi
        //        });

        //        model.Nregistros = 1;
        //    }
        //    catch
        //    {
        //        model.Nregistros = -1;
        //    }

        //    return Json(model);
        //}

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="eqctpvcodi"></param>
        ///// <param name="eqctpvusucreacion"></param>
        ///// <returns></returns>
        //public JsonResult DeleteRegCatpropiedadValor(int eqctpvcodi, string eqctpvfechadat)
        //{
        //    PotenciafirmeModel model = new PotenciafirmeModel();
        //    DateTime f_ = DateTime.ParseExact(eqctpvfechadat, ConstantesAppServicio.FormatoFecha, CultureInfo.CurrentCulture);

        //    if (f_.Date == DateTime.Now.Date)
        //    {
        //        try
        //        {
        //            servicio.DeleteEqCatpropvalor(eqctpvcodi);

        //            model.Nregistros = 1;
        //        }
        //        catch
        //        {
        //            model.Nregistros = -1;
        //        }
        //    }
        //    else { model.Nregistros = -2; }

        //    return Json(model);
        //}

        //#endregion

        //#region calculo potencia firme

        //public ActionResult Index()
        //{
        //    return View();
        //}

        //public ActionResult CalculoPotenciafirme()
        //{
        //    if (!base.IsValidSesion) return base.RedirectToLogin();
        //    PotenciafirmeModel model = new PotenciafirmeModel();
        //    model.Mes = DateTime.Now.ToString("MM yyyy");
        //    return View(model);
        //}

        ///// <summary>
        ///// excel web
        ///// </summary>
        ///// <param name="idEnvio"></param>
        ///// <param name="fecha"></param>
        ///// <param name="tabla"></param>
        ///// <param name="tipo"></param>
        ///// <returns></returns>
        //public JsonResult MostrarGridExcelWeb(int idEnvio, string fecha, int tabla, int tipo)
        //{
        //    PotenciafirmeModel jsModel = new PotenciafirmeModel();
        //    try
        //    {
        //        switch (tabla)
        //        {
        //            case 1: jsModel = GetModelFormatoDeclaracionPrie01(idEnvio, fecha, ConstantesAppServicio.FormatcodiSiosein); break;
        //        }

        //        return Json(jsModel);
        //    }
        //    catch
        //    {
        //        return Json(-1);
        //    }
        //}

        ///// <summary>
        ///// Devuelve el model para mostrar en la pagina  web de envio de Disponibilidad de Gas
        ///// </summary>        
        ///// <param name="idEnvio"></param>
        ///// <param name="fecha"></param>
        ///// <returns></returns>
        //public PotenciafirmeModel GetModelFormatoDeclaracionPrie01(int idEnvio, string fecha, int idFormato)
        //{
        //    PotenciafirmeModel model = new PotenciafirmeModel();
        //    //int idEnvioUltimo = 0;
        //    //List<MeMedicion1DTO> Lista = new List<MeMedicion1DTO>();
        //    //List<EqEquipoDTO> unidades = new List<EqEquipoDTO>();
        //    //model.Handson = new HandsonModel();
        //    //int idEmpresa = -1, idCentral = -1, idEmpresa_ = 0;

        //    //int nBloques = 1;// listaData.Count;
        //    //List<MeCambioenvioDTO> listaCambios = new List<MeCambioenvioDTO>();
        //    //List<CabeceraRow> listaCabeceraRow = new List<CabeceraRow>();
        //    //model.Handson.ListaMerge = new List<CeldaMerge>();

        //    //DateTime dfecIniMes = DateTime.Now, dfecFinMes = DateTime.Now;
        //    //if (fecha != null)
        //    //{
        //    //    fecha = ConstantesAppServicio.IniDiaFecha + fecha.Replace(" ", "/");
        //    //    dfecIniMes = DateTime.ParseExact(fecha, ConstantesBase.FormatoFechaBase, CultureInfo.InvariantCulture);
        //    //    dfecFinMes = DateTime.ParseExact(fecha, ConstantesBase.FormatoFechaBase, CultureInfo.InvariantCulture).AddMonths(1).AddDays(-1);
        //    //}

        //    //////////// Obtiene el Fotmato ////////////////////////
        //    //model.Formato = servFormato.GetByIdMeFormato(idFormato);
        //    //var cabecera = servFormato.GetListMeCabecera().Where(x => x.Cabcodi == model.Formato.Cabcodi).FirstOrDefault();
        //    ///// DEFINICION DEL FORMATO //////
        //    //model.Formato.Formatcols = cabecera.Cabcolumnas;
        //    //model.Formato.Formatrows = cabecera.Cabfilas;
        //    //model.Formato.Formatheaderrow = cabecera.Cabcampodef;

        //    //int idCfgFormato = 0;
        //    //if (idEnvio <= 0)// Fecha proceso es obtenida del registro envio
        //    //{
        //    //    model.Formato.FechaProceso = EPDate.GetFechaIniPeriodo((int)model.Formato.Formatperiodo, "", "", fecha, ConstantesBase.FormatoFechaBase);
        //    //    this.GetSizeFormato(model.Formato);
        //    //    model.EnPlazo = this.ValidarPlazo(model.Formato);
        //    //    model.Handson.ReadOnly = true;// !ValidarFecha(model.Formato, idEmpresa);
        //    //}
        //    //else // Es nuevo envio(se consulta el ultimo envio) o solo se consulta envio seleccionado de la BD
        //    //{
        //    //    model.Handson.ReadOnly = true;

        //    //    var envioAnt = servFormato.GetByIdMeEnvio(idEnvio);
        //    //    if (envioAnt != null)
        //    //    {
        //    //        model.Formato.FechaProceso = (DateTime)envioAnt.Enviofechaperiodo;
        //    //        if (envioAnt.Cfgenvcodi != null)
        //    //        {
        //    //            idCfgFormato = (int)envioAnt.Cfgenvcodi;
        //    //        }
        //    //    }
        //    //    else { model.Formato.FechaProceso = DateTime.MinValue; }

        //    //    this.GetSizeFormato(model.Formato);
        //    //    model.EnPlazo = this.ValidarPlazo(model.Formato);
        //    //    if (idEnvio == 1) model.Formato.FechaProceso = dfecIniMes;
        //    //    listaCambios = servFormato.GetAllCambioEnvio(idFormato, model.Formato.FechaInicio, model.Formato.FechaFin, idEnvio, idEmpresa).Where(x => x.Enviocodi == idEnvio).ToList();
        //    //}
        //    //model.Mes = COES.Base.Tools.Util.ObtenerNombreMes(model.Formato.FechaInicio.Month);
        //    //if (idEnvio == 1) { model.Formato.FechaInicio = dfecIniMes; model.Formato.FechaFin = dfecFinMes; }
        //    //model.ListaEnvios = servFormato.GetByCriteriaMeEnvios(idEmpresa_, idFormato, model.Formato.FechaInicio);
        //    //if (model.ListaEnvios.Count > 0)
        //    //{
        //    //    model.IdEnvioLast = model.ListaEnvios[model.ListaEnvios.Count - 1].Enviocodi;
        //    //    idEnvioUltimo = model.ListaEnvios.Max(x => x.Enviocodi);
        //    //    var reg = model.ListaEnvios.Find(x => x.Enviocodi == idEnvio);
        //    //    if (reg != null)
        //    //        model.FechaEnvio = ((DateTime)reg.Enviofecha).ToString(ConstantesBase.FormatoFechaBase);
        //    //}

        //    //model.Handson.ListaColWidth = new List<int>();
        //    //model.Handson.ListaColWidth.Add(350);
        //    //model.Handson.ListaColWidth.Add(200);
        //    //model.Handson.ListaColWidth.Add(150);
        //    //model.Handson.ListaColWidth.Add(150);
        //    //model.Handson.ListaColWidth.Add(150);
        //    //model.Handson.ListaColWidth.Add(150);
        //    //model.Handson.ListaColWidth.Add(150);
        //    //model.Handson.ListaColWidth.Add(150);
        //    //model.Handson.ListaColWidth.Add(1);

        //    //if (idEnvio >= 0) // Es nuevo envio(se consulta el ultimo envio) o solo se consulta envio seleccionado de la BD
        //    //{
        //    //    model.Handson.ListaExcelData = this.InicializaMatriz(1, nBloques, ConstantesTransfPotencia.nColumnTabla1);
        //    //    var unidadesTermo = servicio.GetCargarUnidadesTermoByEmpresaCentral(idEmpresa, idCentral);
        //    //    foreach (var upd in unidadesTermo) { upd.Gruponomb = upd.Gruponomb.Replace("CCOMB", "").Replace("GAS", "TV").Replace("F.DIRECTO", "").Replace("CHILCA1", "").Replace("CHILCA2", "").Replace("FENIX", "").Replace("KALLPA", "").Replace("OLLEROS", "").Replace("VENTANILLA", "").Trim(); }
        //    //    var unidadesHidro = servicio.GetCargarUnidadesHidroByEmpresaCentral(dfecFinMes);
        //    //    if (idEnvio == 0 && model.IdEnvioLast == 0)
        //    //    {
        //    //        unidades = servicio.GetListaValidacionPoternciaFirme(unidadesTermo, unidadesHidro, idFormato, -1, dfecIniMes, dfecFinMes, null);
        //    //        if (unidades.Count > 0)
        //    //        {
        //    //            model.Handson.ListaExcelData = this.InicializaMatriz(1, unidades.Count, ConstantesTransfPotencia.nColumnTabla1);
        //    //        }
        //    //    }
        //    //    else
        //    //    {
        //    //        Lista = servFormato.GetDataFormato1(0, model.Formato, idEnvio, model.IdEnvioLast);
        //    //        if (Lista.Count > 0)
        //    //        {
        //    //            unidades = servicio.GetListaValidacionPoternciaFirme(unidadesTermo, unidadesHidro, idFormato, -1, dfecIniMes, dfecFinMes, Lista);
        //    //            if (unidades.Count > 0)
        //    //            {
        //    //                model.Handson.ListaExcelData = this.InicializaMatriz(1, unidades.Count, ConstantesTransfPotencia.nColumnTabla1);
        //    //            }
        //    //        }
        //    //    }
        //    //}

        //    //Creamos Cabecera de excel web
        //    this.GeneraCabeceraTabla01(model.Handson.ListaExcelData, ConstantesTransfPotencia.nColumnTabla1, dfecIniMes, dfecFinMes, unidades);

        //    model.IdEnvio = idEnvio;
        //    return model;
        //}

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="data"></param>
        ///// <param name="fecha"></param>
        ///// <returns></returns>
        //[HttpPost]
        //public JsonResult GrabarDatosTablaPrie01dec(string[][] data, string fecha)
        //{
        //    int result = 0, idEmpresa = 0;
        //    FormatoResultado model = new FormatoResultado();
        //    DateTime dfecha = DateTime.Now;

        //    if (fecha != null)
        //    {
        //        fecha = ConstantesAppServicio.IniDiaFecha + fecha.Replace(" ", "/");

        //        dfecha = DateTime.ParseExact(fecha, ConstantesBase.FormatoFechaBase, CultureInfo.InvariantCulture);
        //    }

        //    int idEmpresa_ = -1, idCentral = -1;
        //    var unidades = servicio.GetCargarUnidadesByEmpresaCentral(idEmpresa_, idCentral, dfecha);
        //    List<int> ListaEquicodi = new List<int>();
        //    for (int i = 1; i < data.Length; i++)
        //    {
        //        ListaEquicodi.Add(int.Parse(data[i][8]));
        //    }
        //    var Ptosmedicion = servicio.GetByCriteriaPtosMedicion(string.Join(",", ListaEquicodi), ConstantesTransfPotencia.Origlectcodi.ToString());

        //    if (Ptosmedicion.Count == unidades.Count)
        //    {
        //        List<MeMedicion1DTO> entitys = this.ObtenerDatos1(data, dfecha, ConstantesTransfPotencia.LectCodiPrie01, ConstantesAppServicio.TipoinfocodiMW, Ptosmedicion);
        //        string mensajePlazo = string.Empty;

        //        try
        //        {
        //            model = this.GrabarExcelWeb1(idEmpresa, dfecha, ConstantesAppServicio.FormatcodiSiosein, ConstantesTransfPotencia.LectCodiPrie01, entitys);
        //            result = 1;
        //        }
        //        catch
        //        {
        //            result = -1;
        //        }
        //    }
        //    else { result = -2; }
        //    model.Resultado = result;
        //    return Json(model);
        //}

        ///// <summary>
        ///// Graba los datos enviados a traves de la grilla excel para formatos configurados con informacion de cada media hora.
        ///// </summary>
        ///// <param name="data"></param>
        ///// <param name="idEmpresa"></param>
        ///// <param name="fecha"></param>
        ///// <param name="idFormato"></param>
        ///// <param name="lectcodi"></param>
        ///// <returns></returns>
        //public FormatoResultado GrabarExcelWeb1(int idEmpresa, DateTime fecha, int idFormato, int lectcodi, List<MeMedicion1DTO> entitys)
        //{
        //    int exito = 0;
        //    FormatoMedicionAppServicio servFormato = new FormatoMedicionAppServicio();
        //    FormatoResultado model = new FormatoResultado();
        //    model.Resultado = 0;
        //    try
        //    {
        //        //base.ValidarSesionUsuario();
        //        ///////// Definicion de Variables ////////////////    

        //        string empresa = string.Empty;
        //        var regEmp = servFormato.GetByIdSiEmpresa(idEmpresa); ;
        //        //////////////////////////////////////////////////
        //        if (regEmp != null)
        //            empresa = regEmp.Emprnomb;

        //        MeFormatoDTO formato = servFormato.GetByIdMeFormato(idFormato);
        //        var cabecera = servFormato.GetListMeCabecera().Where(x => x.Cabcodi == formato.Cabcodi).FirstOrDefault();
        //        formato.Formatcols = cabecera.Cabcolumnas;
        //        formato.Formatrows = cabecera.Cabfilas;
        //        formato.Formatheaderrow = cabecera.Cabcampodef;
        //        int filaHead = formato.Formatrows;
        //        int colHead = formato.Formatcols;

        //        /////////////// Obtiene Fecha Inicio y Fecha Fin del Proceso //////////////
        //        formato.FechaProceso = fecha; //GetFechaIniPeriodo((int)formato.Formatperiodo, string.Empty, string.Empty, fecha.ToShortDateString(), Constantes.FormatoFecha);
        //        GetSizeFormato(formato);

        //        var listaPto = servFormato.GetByCriteriaMeHojaptomeds(idEmpresa, idFormato, formato.FechaInicio, formato.FechaFin);
        //        int nPtos = listaPto.Count();

        //        /////////////// Grabar Config Formato Envio //////////////////
        //        MeConfigformatenvioDTO config = new MeConfigformatenvioDTO();
        //        config.Formatcodi = idFormato;
        //        config.Emprcodi = idEmpresa;
        //        config.FechaInicio = formato.FechaInicio;
        //        config.FechaFin = formato.FechaFin;
        //        int idConfig = servFormato.GrabarConfigFormatEnvio(config);
        //        ///////////////Grabar Envio//////////////////////////
        //        string mensajePlazo = string.Empty;
        //        Boolean enPlazo = ValidarPlazo(formato);
        //        MeEnvioDTO envio = new MeEnvioDTO();
        //        envio.Archcodi = 0;
        //        envio.Emprcodi = idEmpresa;
        //        envio.Enviofecha = DateTime.Now;
        //        envio.Enviofechaperiodo = formato.FechaProceso;
        //        envio.Enviofechaini = formato.FechaInicio;
        //        envio.Enviofechafin = formato.FechaFin;
        //        envio.Envioplazo = (enPlazo) ? "P" : "F";
        //        envio.Estenvcodi = ParametrosEnvio.EnvioEnviado;
        //        envio.Lastdate = DateTime.Now;
        //        envio.Lastuser = "UserPrie";// User.Identity.Name;
        //        envio.Userlogin = "UserPrie";// User.Identity.Name;
        //        envio.Formatcodi = idFormato;
        //        envio.Fdatcodi = 0;
        //        envio.Cfgenvcodi = idConfig;
        //        int idEnvio = servFormato.SaveMeEnvio(envio);
        //        model.IdEnvio = idEnvio;
        //        ///////////////////////////////////////////////////////
        //        int horizonte = formato.Formathorizonte;

        //        //var lista48 = ObtenerDatos48(data, listaPto, formato.Formatcheckblanco, (int)formato.Formatrows, lectcodi);
        //        //var lista1 = ObtenerDatos1(data, fecha, ConstantesSioSein.LectCodiPrie01, ConstantesSioSein.TipoInfoCodi, Ptosmedicion);
        //        servFormato.GrabarValoresCargados1(entitys, envio.Userlogin, idEnvio, idEmpresa, formato, lectcodi);
        //        envio.Estenvcodi = ParametrosEnvio.EnvioAprobado;
        //        envio.Enviocodi = idEnvio;
        //        servFormato.UpdateMeEnvio(envio);
        //        //            EnviarCorreo(enPlazo, idEnvio, idEmpresa, formato.Formatnombre, empresa, formato.Areaname, formato.FechaProceso,
        //        //(DateTime)envio.Enviofecha);
        //        exito = 1;
        //        model.Resultado = 1;
        //    }
        //    catch (Exception ex)
        //    {
        //        exito = -1;
        //        model.Resultado = -1;
        //        model.Mensaje = ex.Message;
        //    }

        //    model.Resultado = exito;
        //    return model;
        //}

        ///// <summary>
        ///// Lee los datos del  formato web Tabla Prie 01 en una lista de DTO Medicion1
        ///// </summary>
        ///// <param name="datos"></param>
        ///// <returns></returns>
        //public List<MeMedicion1DTO> ObtenerDatos1(string[][] datos, DateTime fecha, int lectcodi, int tipoinfocodi, List<MePtomedicionDTO> Ptosmedicion)
        //{
        //    List<MeMedicion1DTO> lista = new List<MeMedicion1DTO>();
        //    if (datos.Length > 1)
        //    {
        //        string stValor1 = string.Empty, stValor2 = string.Empty, stValor3 = string.Empty;
        //        decimal valor = decimal.MinValue;
        //        for (int i = 1; i < datos.Length; i++)
        //        {
        //            MeMedicion1DTO entity1 = new MeMedicion1DTO();
        //            MeMedicion1DTO entity2 = new MeMedicion1DTO();
        //            MeMedicion1DTO entity3 = new MeMedicion1DTO();
        //            if (datos[i][8].Trim() != "")
        //            {
        //                var dat = Ptosmedicion.Find(x => x.Origlectcodi == ConstantesTransfPotencia.Origlectcodi && x.Equicodi == int.Parse(datos[i][8]));
        //                entity1.Ptomedicodi = dat.Ptomedicodi;
        //                entity2.Ptomedicodi = dat.Ptomedicodi;
        //                entity3.Ptomedicodi = dat.Ptomedicodi;
        //            }

        //            entity1.Lectcodi = lectcodi;
        //            entity2.Lectcodi = lectcodi;
        //            entity3.Lectcodi = lectcodi;

        //            entity1.Tipoinfocodi = tipoinfocodi;
        //            entity2.Tipoinfocodi = tipoinfocodi;
        //            entity3.Tipoinfocodi = tipoinfocodi;

        //            entity1.Tipoptomedicodi = 1;
        //            entity2.Tipoptomedicodi = 2;
        //            entity3.Tipoptomedicodi = 3;

        //            entity1.Medifecha = new DateTime(fecha.Year, fecha.Month, fecha.Day);
        //            entity2.Medifecha = new DateTime(fecha.Year, fecha.Month, fecha.Day);
        //            entity3.Medifecha = new DateTime(fecha.Year, fecha.Month, fecha.Day);

        //            stValor1 = datos[i][7];
        //            if (Base.Tools.Util.EsNumero(stValor1))
        //            {
        //                valor = decimal.Parse(stValor1);
        //                entity1.H1 = valor;
        //            }
        //            else { entity1.H1 = 0; }

        //            stValor2 = datos[i][4];
        //            if (Base.Tools.Util.EsNumero(stValor2))
        //            {
        //                valor = decimal.Parse(stValor2);
        //                entity2.H1 = valor;
        //            }
        //            else { entity2.H1 = 0; entity2.Tipoptomedicodi = -2; }

        //            stValor3 = datos[i][6];
        //            if (Base.Tools.Util.EsNumero(stValor3))
        //            {
        //                valor = decimal.Parse(stValor3);
        //                entity3.H1 = valor;
        //            }
        //            else { entity3.H1 = 0; entity3.Tipoptomedicodi = -3; }

        //            lista.Add(entity1);
        //            lista.Add(entity2);
        //            lista.Add(entity3);
        //        }
        //    }

        //    return lista;
        //}

        ///// <summary>
        ///// Completa los parametros del DTO Formato
        ///// </summary>
        ///// <param name="formato"></param>
        //public void GetSizeFormato(COES.Dominio.DTO.Sic.MeFormatoDTO formato)
        //{

        //    formato.FechaInicio = formato.FechaProceso;
        //    formato.FechaFin = formato.FechaProceso.AddDays(formato.Formathorizonte - 1);
        //    formato.RowPorDia = ParametrosFormato.ResolucionDia / (int)formato.Formatresolucion;
        //    formato.FechaPlazo = formato.FechaProceso.AddDays(formato.Formatdiaplazo).AddMinutes(formato.Formatminplazo);
        //    formato.FechaPlazoIni = formato.FechaProceso.AddDays(formato.Formatdiaplazo);
        //    if (formato.Formatdiaplazo == 0)
        //    {
        //        formato.FechaPlazo = formato.FechaProceso.AddDays(1).AddMinutes(formato.Formatminplazo);
        //    }
        //    else
        //    {
        //        formato.FechaPlazo = formato.FechaProceso.AddDays(formato.Formatdiaplazo).AddMinutes(formato.Formatminplazo);
        //    }
        //}

        ///// <summary>
        ///// Verifica si un formato enviado esta en plazo o fuera de plazo
        ///// </summary>
        ///// <param name="formato"></param>
        ///// <returns></returns>
        //public bool ValidarPlazo(MeFormatoDTO formato)
        //{
        //    bool resultado = false;
        //    DateTime fechaActual = DateTime.Now;
        //    if ((fechaActual >= formato.FechaPlazoIni) && (fechaActual <= formato.FechaPlazo))
        //    {
        //        resultado = true;
        //    }
        //    return resultado;
        //}

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="data"></param>
        ///// <param name="col"></param>
        ///// <param name="dfecIniMes"></param>
        ///// <param name="dfecFinMes"></param>
        ///// <param name="unidades"></param>
        //public void GeneraCabeceraTabla01(string[][] data, int col, DateTime dfecIniMes, DateTime dfecFinMes, List<EqEquipoDTO> unidades)
        //{
        //    data[0] = new string[col];
        //    data[0][0] = "Empresa";
        //    data[0][1] = "Central";
        //    data[0][2] = "Unidad";
        //    data[0][3] = "Potencia Efectiva";
        //    data[0][4] = "Factor Indisponibilidad (%)";
        //    data[0][5] = "Potencia Garantizada";
        //    data[0][6] = "Factor Presencia";
        //    data[0][7] = "Potencia Firme";
        //    data[0][8] = "";

        //    int i = 1;
        //    if (unidades != null)
        //    {
        //        foreach (var reg in unidades)
        //        {
        //            data[i] = new string[col];
        //            data[i][0] = reg.Emprnomb;
        //            data[i][1] = reg.Equinomb;
        //            data[i][2] = reg.Gruponomb;
        //            data[i][3] = reg.Potenciaefectiva.ToString();
        //            data[i][4] = (reg.Factorindisponibilidad == null ? "" : reg.Factorindisponibilidad.ToString());
        //            data[i][5] = reg.Potenciagarantizada.ToString();
        //            data[i][6] = (reg.Factorpresencia == null ? "" : reg.Factorpresencia.ToString());
        //            data[i][7] = reg.Potenciafirme.ToString();
        //            data[i][8] = (reg.Equicodi != null ? reg.Equicodi.ToString() : reg.Equipadre.ToString());
        //            i++;
        //        }
        //    }
        //}

        ///// <summary>
        ///// Inicializamos matriz para el excel web
        ///// </summary>
        ///// <param name="rowsHead"></param>
        ///// <param name="nFil"></param>
        ///// <param name="nCol"></param>
        ///// <returns></returns>
        //public string[][] InicializaMatriz(int rowsHead, int nFil, int nCol)
        //{
        //    string[][] matriz = new string[nFil + rowsHead][];
        //    for (int i = 0; i < nFil + rowsHead; i++)
        //    {
        //        matriz[i] = new string[nCol];
        //        for (int j = 0; j < nCol;
        //            j++)
        //        {
        //            matriz[i][j] = string.Empty;

        //        }
        //    }
        //    return matriz;
        //}

        //#endregion
    }
}
