using COES.Dominio.DTO.Sic;
using COES.Framework.Base.Tools;
using COES.MVC.Intranet.App_Start;
using COES.MVC.Intranet.Areas.Hidrologia.Models;
using COES.MVC.Intranet.Controllers;
using COES.MVC.Intranet.Helper;
using COES.MVC.Intranet.Models;
using COES.Servicios.Aplicacion.Equipamiento;
using COES.Servicios.Aplicacion.FormatoMedicion;
using COES.Servicios.Aplicacion.Helper;
using COES.Servicios.Aplicacion.Hidrologia;
using COES.Servicios.Aplicacion.IEOD;
using COES.Servicios.Aplicacion.PMPO.Helper;
using COES.Servicios.Aplicacion.Transferencias;
using log4net;
using Microsoft.SqlServer.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.Hidrologia.Controllers
{
    public class FormatoMedicionController : BaseController
    {
        private static readonly ILog Log = log4net.LogManager.GetLogger(typeof(FormatoMedicionController));
        private static string NombreControlador = "FormatoMedicionController";
        private readonly List<EstadoModel> _lsEstadosFlag = new List<EstadoModel>();

        protected override void OnException(ExceptionContext filterContext)
        {
            try
            {
                log4net.Config.XmlConfigurator.Configure();
                Exception objErr = filterContext.Exception;
                Log.Error(NombreControlador, objErr);
            }
            catch (Exception ex)
            {
                Log.Fatal(NombreControlador, ex);
                throw;
            }
        }

        public FormatoMedicionAppServicio servicio;
        public HidrologiaAppServicio servHidrologia;
        public FormatoMedicionController()
        {
            servicio = new FormatoMedicionAppServicio();
            servHidrologia = new HidrologiaAppServicio();
            _lsEstadosFlag.Add(new EstadoModel { EstadoCodigo = "A", EstadoDescripcion = "Activo" });
            _lsEstadosFlag.Add(new EstadoModel { EstadoCodigo = "B", EstadoDescripcion = "Baja" });
        }

        #region Creación de Formatos

        /// <summary>
        /// Index de inicio de controller FormatoMedicion
        /// </summary>
        /// <returns></returns>
        public ActionResult Index(int? app)
        {
            if (!base.IsValidSesionView()) return base.RedirectToLogin();

            FormatoHidrologiaModel model = new FormatoHidrologiaModel();
            model.ListaOrigenLectura = servicio.ListMeOrigenlecturas();
            model.ListaLectura = servicio.ListMeLecturas();
            model.ListaAreasCoes = servicio.ListFwAreas();
            model.ListaFormato = servicio.ListMeFormatosOrigen();
            model.CodigoApp = app ?? 0;

            return View(model);
        }

        /// <summary>
        /// Devuelve vista parcial para mostrar listado de formatos
        /// </summary>
        /// <param name="area"></param>
        /// <param name="formatcodiOrigen"></param>
        /// <param name="app"></param>
        /// <returns></returns>
        public PartialViewResult ListaFormato(int area, int formatcodiOrigen, int? app)
        {
            FormatoHidrologiaModel model = new FormatoHidrologiaModel();
            model.ListaFormato = servicio.GetByCriteriaMeFormatos(area, formatcodiOrigen);
            model.ListaFormato = servicio.ListarFormatoByFiltroApp(model.ListaFormato, app.Value, 0);

            return PartialView(model);
        }

        /// <summary>
        /// Permite mostrar la vista para la creacion de puntos
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult Agregar()
        {
            string codigo = "0";
            if (Request["id"] != null)
                codigo = Request["id"];
            int id = int.Parse(codigo);
            FormatoHidrologiaModel model = new FormatoHidrologiaModel();
            model.IdFormato = id;
            model.IdModulo = 0;
            model.IdCabecera = 0;
            model.IdArea = 0;
            model.Periodo = 0;
            model.Horizonte = 0;
            model.Resolucion = 0;
            model.DiaPlazo = 0;
            model.DiaFinPlazo = 0;
            model.DiaFinFueraPlazo = 0;
            model.MinutoPlazo = 0;
            model.MinutoFinPlazo = 0;
            model.MinutoFinFueraPlazo = 0;
            model.Mesplazo = 0;
            model.Mesfinplazo = 0;
            model.Mesfinfueraplazo = 0;
            model.CheckPlazo = 0;
            model.CheckBlanco = 0;
            model.AllEmpresa = -1;
            model.IdFormato2 = 0;

            if (id > 0) //Edicion de Formato
            {
                var formato = servicio.GetByIdMeFormato(id);
                model.IdLectura = formato.Lectcodi;
                model.Descripcion = formato.Formatdescrip;
                model.Nombre = formato.Formatnombre;
                model.IdModulo = formato.Modcodi;
                model.IdCabecera = formato.Cabcodi;
                model.IdArea = (int)formato.Areacode;
                model.Periodo = (int)formato.Formatperiodo;
                model.Horizonte = (int)formato.Formathorizonte;
                model.Resolucion = (int)formato.Formatresolucion;
                model.DiaPlazo = formato.Formatdiaplazo;
                model.DiaFinPlazo = formato.Formatdiafinplazo;
                model.DiaFinFueraPlazo = formato.Formatdiafinfueraplazo;
                model.MinutoPlazo = formato.Formatminplazo;
                model.MinutoFinPlazo = formato.Formatminfinplazo;
                model.MinutoFinFueraPlazo = formato.Formatminfinfueraplazo;
                model.Mesplazo = formato.Formatmesplazo;
                model.Mesfinplazo = formato.Formatmesfinplazo;
                model.Mesfinfueraplazo = formato.Formatmesfinfueraplazo;
                model.CheckBlanco = formato.Formatcheckblanco;
                model.CheckPlazo = formato.Formatcheckplazo;
                model.AllEmpresa = formato.Formatallempresa;
                model.IdFormato2 = formato.Formatdependeconfigptos;
            }
            model.ListaLectura = servicio.ListMeLecturas();
            model.ListaAreasCoes = servicio.ListFwAreas();
            model.ListaCabecera = servicio.GetListMeCabecera();
            model.ListaModulo = servicio.ListMeModulos();
            model.ListaFormato = servicio.GetByCriteriaMeFormatos(0, 0);

            model.FechaPeriodo = DateTime.Now.ToString(ConstantesAppServicio.FormatoFecha);
            model.MesPeriodo = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).ToString(ConstantesAppServicio.FormatoMes);

            int idAnio = DateTime.Now.Year;
            int numSemana = EPDate.f_numerosemana(DateTime.Now);
            model.SemanaPeriodo = EPDate.GetFechaIniPeriodo(2, string.Empty, idAnio + "" + numSemana, string.Empty, string.Empty).ToString(ConstantesAppServicio.FormatoFecha);
            model.AnioPeriodo = idAnio + string.Empty;
            int nsemanas = EPDate.TotalSemanasEnAnho(idAnio, FirstDayOfWeek.Saturday);
            DateTime fechaIniSemana = EPDate.GetFechaIniPeriodo(2, string.Empty, idAnio + "1", string.Empty, string.Empty);
            List<GenericoDTO> entitys = new List<GenericoDTO>();
            for (int i = 1; i <= nsemanas; i++)
            {
                GenericoDTO reg = new GenericoDTO();
                reg.Entero1 = i;
                reg.String1 = "Sem" + i + "-" + idAnio;
                reg.String2 = fechaIniSemana.ToString(ConstantesAppServicio.FormatoFecha);
                entitys.Add(reg);
                fechaIniSemana = fechaIniSemana.AddDays(7);
            }
            model.ListaGenSemanas = entitys;

            return PartialView(model);
        }

        /// <summary>
        /// Agrega en BD formato y devuelve resultado al cliente
        /// </summary>
        /// <param name="nombre"></param>
        /// <param name="area"></param>
        /// <param name="resolucion"></param>
        /// <param name="horizonte"></param>
        /// <param name="periodo"></param>
        /// <param name="lectura"></param>
        /// <param name="tituloHoja"></param>
        /// <param name="diaPlazo"></param>
        /// <param name="minPlazo"></param>
        /// <param name="descripcion"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult AgregarFormato(string nombre, int area, int resolucion, int horizonte, int periodo, int lectura,
            string tituloHoja, int diaPlazo, int minPlazo, string descripcion)
        {
            MeFormatoDTO formato = new MeFormatoDTO();
            MeFormatohojaDTO hoja = new MeFormatohojaDTO();
            int resultado = 1;
            formato.Formatnombre = nombre;
            formato.Areacode = area;
            formato.Formatresolucion = resolucion;
            formato.Formathorizonte = horizonte;
            formato.Formatperiodo = periodo;
            formato.Modcodi = Constantes.IdModulo;
            formato.Formatextension = "xlsx";
            formato.Lastdate = DateTime.Now;
            formato.Lastuser = User.Identity.Name;
            formato.Formatdiaplazo = diaPlazo;
            formato.Formatminplazo = minPlazo;
            formato.Formatdescrip = descripcion;
            formato.Lectcodi = lectura;
            try
            {
                base.ValidarSesionJsonResult();
                int idFormato = servicio.SaveMeFormato(formato);
                return Json(resultado);
            }
            catch(Exception e)
            {
                Log.Error(NombreControlador, e);
                return Json(-1);
            }
        }
        /// <summary>
        /// Actualiza el formato y sus hojas
        /// </summary>
        /// <param name="idFormato"></param>
        /// <param name="idHoja"></param>
        /// <param name="nombre"></param>
        /// <param name="area"></param>
        /// <param name="resolucion"></param>
        /// <param name="horizonte"></param>
        /// <param name="periodo"></param>
        /// <param name="lectura"></param>
        /// <param name="tituloHoja"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ActualizarFormato(int idFormato, int idHoja, string nombre, int area, int resolucion,
            int horizonte, int periodo, int lectura, string tituloHoja, int diaPlazo, int minPlazo, string descripcion)
        {
            MeFormatoDTO formato = servicio.GetByIdMeFormato(idFormato);
            if (formato != null)
            {
                formato.Formatnombre = nombre;
                formato.Formatresolucion = resolucion;
                formato.Formathorizonte = horizonte;
                formato.Formatperiodo = periodo;
                formato.Lastdate = DateTime.Now;
                formato.Lastuser = User.Identity.Name;
                formato.Formatdiaplazo = diaPlazo;
                formato.Formatminplazo = minPlazo;
                formato.Formatdescrip = descripcion;
                formato.Lectcodi = lectura;
                try
                {
                    base.ValidarSesionJsonResult();

                    servicio.UpdateMeFormato(formato);
                    int resultado = 1;
                    return Json(resultado);
                }
                catch (Exception e)
                {
                    Log.Error(NombreControlador, e);
                    return Json(-1);
                }
            }
            else
            {
                return Json(-1);
            }


        }

        /// <summary>
        /// Graba formato en BD
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GrabarFormato(FormatoHidrologiaModel model)
        {

            int resultado = 0;
            MeFormatoDTO formato = new MeFormatoDTO();
            formato.Formatnombre = model.Nombre;
            formato.Areacode = model.IdArea;
            formato.Formatresolucion = model.Resolucion;
            formato.Formathorizonte = model.Horizonte;
            formato.Formatperiodo = model.Periodo;
            formato.Lectcodi = model.IdLectura;
            formato.Modcodi = model.IdModulo;
            formato.Formatdependeconfigptos = model.IdFormato2;
            formato.Lastdate = DateTime.Now;
            formato.Lastuser = User.Identity.Name;
            formato.Formatmesplazo = model.Mesplazo;
            formato.Formatdiaplazo = model.DiaPlazo;
            formato.Formatminplazo = model.MinutoPlazo;

            formato.Formatmesfinplazo = model.Mesfinplazo;
            formato.Formatdiafinplazo = model.DiaFinPlazo;
            formato.Formatminfinplazo = model.MinutoFinPlazo;

            formato.Formatmesfinfueraplazo = model.Mesfinfueraplazo;
            formato.Formatdiafinfueraplazo = model.DiaFinFueraPlazo;
            formato.Formatminfinfueraplazo = model.MinutoFinFueraPlazo;
            formato.Formatdescrip = model.Descripcion;
            formato.Cabcodi = model.IdCabecera;
            formato.Formatcheckplazo = model.CheckPlazo;
            formato.Formatcheckblanco = model.CheckBlanco;
            formato.Formatallempresa = model.AllEmpresa;
            if (model.IdFormato == 0) //Nuevo Formato
            {
                //Grabar Formato
                try
                {
                    base.ValidarSesionJsonResult();

                    int idFormato = servicio.SaveMeFormato(formato);
                    resultado = 1;
                }
                catch (Exception ex)
                {
                    Log.Error(NombreControlador, ex);
                    resultado = -1;
                }

            }
            else
            { //Edicion de Formato
                formato.Formatcodi = model.IdFormato;
                var find = servicio.GetByIdMeFormato(model.IdFormato);
                if (find != null)
                {
                    try
                    {
                        base.ValidarSesionJsonResult();

                        servicio.UpdateMeFormato(formato);
                        resultado = 1;
                    }
                    catch (Exception ex)
                    {
                        Log.Error(NombreControlador, ex);
                        resultado = -1;
                    }
                }

            }

            return Json(resultado);
        }

        #endregion

        #region RDO-Yupana

        /// <summary>
        /// Brinda informacion a la vista DetalleEquipos
        /// </summary>
        /// <param name="id"></param>
        /// <param name="app"></param>
        /// <returns></returns>
        public ActionResult DetalleEquipos(int? id, int? app)
        {
            if (!base.IsValidSesionView()) return base.RedirectToLogin();

            id = id ?? 0;

            FormatoHidrologiaModel model = new FormatoHidrologiaModel();
            model.Url = Url.Content("~/");

            var formato = servicio.GetByIdMeFormato(id.Value);
            if (formato == null)
                return base.RedirectToHomeDefault();

            model.CodigoApp = app ?? 0;
            model.Nombre = formato.Formatnombre;

            model.ListaHoja = servicio.GetByCriteriaMeHoja(id.Value);


            model.IdHoja = -1;
            if (model.ListaHoja.Count >= 1)
            {
                if (model.ListaHoja.Count >= 2)
                    model.IndicadorHoja = Constantes.SI;
                if (model.ListaHoja.Count == 1)
                    model.IdHoja = model.ListaHoja.First().Hojacodi;
            }

            model.IdFormato = id.Value;
            model.IdFormatoOrigen = formato.Formatdependeconfigptos ?? 0;


            return View(model);
        }

        /// <summary>
        /// Brinda informacion al listado de equipos para el compromiso hidráulico
        /// </summary>
        /// <param name="empresa"></param>
        /// <param name="formato"></param>
        /// <param name="hoja"></param>
        /// <param name="formatoOrigen"></param>
        /// <returns></returns>
        public PartialViewResult ListaEquipos(int empresa, int formato, int hoja, int? formatoOrigen = 0)
        {
            if (!base.IsValidSesionView()) throw new ArgumentException(Constantes.MensajeSesionExpirado);

            FormatoHidrologiaModel model = new FormatoHidrologiaModel();

            model.ListaHojaPto = servicio.GetListaHojaptomed(empresa, formato, hoja);

            //Actualizar orden de los puntos de medición
            this.servicio.UpdateListaMeHojaptomedByOrder(model.ListaHojaPto);


            model.EmpresaCodigo = empresa;
            model.HojaNumero = 1;
            model.FormatoCodigo = formato;
            model.IdFormatoOrigen = formatoOrigen ?? 0;
            return PartialView(model);
        }

        /// <summary>
        /// Brinda información a la ventana emergente que se abre al agregar nuevo equipo
        /// </summary>
        /// <param name="formato"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult MostrarAgregarEquipo(int formato)
        {
            FormatoHidrologiaModel model = new FormatoHidrologiaModel();

            model.HojaNumero = 1;
            model.EmpresaCodigo = -1;
            model.FormatoCodigo = formato;

            return PartialView(model);
        }

        /// <summary>
        /// Agrega un equipo (eleigiendo su primer ptomedicodi)
        /// </summary>
        /// <param name="empresa"></param>
        /// <param name="formato"></param>
        /// <param name="equipo"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult AgregarEquipo(int empresa, int formato, int equipo)
        {

            int medida = -1;
            int limsup = 0;
            int liminf = 0;
            int hoja = -1;
            int tipoPtoMedicion = -1;
            int punto = -1;
            int origenLectura = 16;

            FormatoHidrologiaModel model = new FormatoHidrologiaModel();
            try
            {
                var listaPtos = servicio.GetByIdEquipoMePtomedicion(equipo, 1);
                var listaPtosOrigLectura = listaPtos.Where(x => x.Origlectcodi == origenLectura).ToList();
                if (listaPtosOrigLectura.Count > 0)
                {
                    punto = listaPtosOrigLectura.First().Ptomedicodi;
                }
                else
                    throw new ArgumentException("El equipo no contiene punto de medición a referenciar.");



                MeHojaptomedDTO hojaptos = new MeHojaptomedDTO();

                //validar si punto pertenece a la empresa
                hojaptos.Ptomedicodi = punto;
                hojaptos.Formatcodi = formato;
                hojaptos.Hojacodi = hoja;
                hojaptos.Hojanumero = 1;
                hojaptos.Hojaptosigno = 1;
                hojaptos.Hojaptoactivo = 1;
                hojaptos.Tipoinfocodi = medida;
                hojaptos.Hojaptolimsup = limsup;
                hojaptos.Hojaptoliminf = liminf;
                hojaptos.Tptomedicodi = tipoPtoMedicion;
                hojaptos.Lastdate = DateTime.Now;
                hojaptos.Lastuser = User.Identity.Name;

                base.ValidarSesionJsonResult();

                var entity = servicio.GetByIdMeHojaptomed(1, formato, medida, punto, 1, hojaptos.Tptomedicodi, hoja);
                if (entity == null)
                {
                    model.HojaPto = servicio.GrabarHojaPtoMedicion(hojaptos, empresa);
                    model.Resultado = 1;
                }
                else
                    model.Resultado = 0;


            }
            catch (Exception ex)
            {
                Log.Error(NombreControlador, ex);
                model.Resultado = -1;
                model.Descripcion = ex.ToString();
            }
            return Json(model);
        }

        #endregion

        #region Configuración de puntos del formato

        /// <summary>
        /// Index para visualizar el detalle de puntos de medidcion del formato
        /// </summary>
        /// <returns></returns>
        public ActionResult IndexDetalle(int? id, int? app)
        {
            if (!base.IsValidSesionView()) return base.RedirectToLogin();

            id = id ?? 0;

            FormatoHidrologiaModel model = new FormatoHidrologiaModel();
            model.Url = Url.Content("~/");

            var formato = servicio.GetByIdMeFormato(id.Value);
            if (formato == null)
                return base.RedirectToHomeDefault();

            model.CodigoApp = app ?? 0;
            model.Nombre = formato.Formatnombre;
            model.ListaEmpresa = servicio.ListarEmpresas();
            model.ListaEmpresaFormato = servicio.GetListaEmpresaFormato(id.Value);
            if(id.Value == 54) model.ListaEmpresaFormato = model.ListaEmpresaFormato.Where(x => x.Emprestado == "A").ToList();
            model.ListaHoja = servicio.GetByCriteriaMeHoja(id.Value);
            model.AllEmpresa = formato.Formatallempresa;
            model.Formatcheckplazopunto = formato.Formatcheckplazopunto;
            model.DiaPlazo = formato.Formatdiaplazo;
            model.DiaFinPlazo = formato.Formatdiafinplazo;
            model.DiaFinFueraPlazo = formato.Formatdiafinfueraplazo;
            model.MinutoPlazo = formato.Formatminplazo;
            model.MinutoFinPlazo = formato.Formatminfinplazo;
            model.MinutoFinFueraPlazo = formato.Formatminfinfueraplazo;
            model.Mesplazo = formato.Formatmesplazo;
            // model.Periodo = 3;

            model.Periodo = (int)formato.Formatperiodo;

            int idAnio = DateTime.Now.Year;
            int numSemana = EPDate.f_numerosemana(DateTime.Now);
            model.SemanaPeriodo = EPDate.GetFechaIniPeriodo(2, string.Empty, idAnio + "" + numSemana, string.Empty, string.Empty).ToString(ConstantesAppServicio.FormatoFecha);
            model.AnioPeriodo = idAnio + string.Empty;
            int nsemanas = EPDate.TotalSemanasEnAnho(idAnio, FirstDayOfWeek.Saturday);
            DateTime fechaIniSemana = EPDate.GetFechaIniPeriodo(2, string.Empty, idAnio + "1", string.Empty, string.Empty);
            List<GenericoDTO> entitys = new List<GenericoDTO>();
            for (int i = 1; i <= nsemanas; i++)
            {
                GenericoDTO reg = new GenericoDTO();
                reg.Entero1 = i;
                reg.String1 = "Sem" + i + "-" + idAnio;
                reg.String2 = fechaIniSemana.ToString(ConstantesAppServicio.FormatoFecha);
                entitys.Add(reg);
                fechaIniSemana = fechaIniSemana.AddDays(7);
            }
            model.MesPeriodo = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).ToString(ConstantesAppServicio.FormatoMes);

            model.ListaGenSemanas = entitys;

            ///Modificación Tipo Punto Medición
            model.ListaTipoPuntoMedicion = servicio.ListarTiposPuntoMedicion(-1, -1);
            //////////////////////////////////

            model.IdHoja = -1;
            if (model.ListaHoja.Count >= 1)
            {
                if (model.ListaHoja.Count >= 2)
                    model.IndicadorHoja = Constantes.SI;
                if (model.ListaHoja.Count == 1)
                    model.IdHoja = model.ListaHoja.First().Hojacodi;
            }

            model.IdFormato = id.Value;
            model.IdFormatoOrigen = formato.Formatdependeconfigptos ?? 0;
            model.FormatoTieneCheckAdicional = formato.Formatenviocheckadicional == ConstantesAppServicio.SI;
            model.NombreOrigen = formato.FormatnombreOrigen;
            model.IdEmpresa = 0;
            model.IdArea = (formato.Areacode != null) ? (int)formato.Areacode : -1;
            if (model.ListaEmpresaFormato.Count > 0)
                model.IdEmpresa = model.ListaEmpresaFormato[0].Emprcodi;

            model.ListaLectura = servicio.ListMeLecturas();
            model.ListaCabecera = servicio.GetListMeCabecera();
            model.IdLectura = formato.Lectcodi;
            model.IdCabecera = formato.Cabcodi;

            return View(model);
        }

        /// <summary>
        /// Genera vista parcial para poppu de edicion de formato.
        /// </summary>
        /// <param name="formato"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult DetalleGeneralFormato(int formato)
        {
            FormatoHidrologiaModel model = new FormatoHidrologiaModel();
            model.Formato = servicio.GetByIdMeFormato(formato);
            model.ListaAreasCoes = servicio.ListFwAreas();
            return PartialView(model);
        }

        /// <summary>
        /// Devuelve vista parcial del listado de ptos de medicion en el detalle de formato
        /// </summary>
        /// <param name="empresa"></param>
        /// <param name="formato"></param>
        /// <returns></returns>
        public PartialViewResult ListaPtoMedicion(int empresa, int formato, int hoja, int? formatoOrigen = 0)
        {
            if (!base.IsValidSesionView()) throw new Exception(Constantes.MensajeSesionExpirado);

            FormatoHidrologiaModel model = new FormatoHidrologiaModel();
            model.IndicadorTransferencias = false;
            model.ListaHojaPto = servicio.GetListaHojaptomed(empresa, formato, hoja);

            //Actualizar orden de los puntos de medición
            this.servicio.UpdateListaMeHojaptomedByOrder(model.ListaHojaPto);

            //- Fit Aplicativo VTD

            if (formato == 101 || formato == 102)
            {
                model.IndicadorTransferencias = true;
            }

            //- Fin Fit

            model.EmpresaCodigo = empresa;
            model.HojaNumero = 1;
            model.FormatoCodigo = formato;
            model.IdFormatoOrigen = formatoOrigen ?? 0;
            return PartialView(model);
        }

        /// <summary>
        /// Abre el popup para ingresar el punto de medicion
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult MostrarAgregarPto(int formato)
        {
            FormatoHidrologiaModel model = new FormatoHidrologiaModel();
            model.ListaOrigenLectura = this.servicio.ListMeOrigenlecturas().Where(x => x.Origlectcodi != 0 && x.Origlectcodi != -1).OrderBy(x => x.Origlectnombre).ToList();
            model.ListaMedidas = servicio.ListSiTipoinformacions();
            model.ListaFamilia = servicio.ListarFamilia().Where(x => x.Famcodi != 0 && x.Famcodi != -1).ToList();
            model.ListaEquipo = servicio.GetByCriteriaEqequipo(-1, 3).Where(x => x.Equicodi != 0 && x.Equicodi != -1).ToList();
            model.ListaTipoGrupo = servicio.ListarTipoGrupo();
            model.HojaNumero = 1;
            model.EmpresaCodigo = -1;
            model.FormatoCodigo = formato;
            ///Modificación Tipo Punto Medición
            model.ListaTipoPuntoMedicion = servicio.ListarTiposPuntoMedicion(-1, -1);
            //////////////////////////////////            
            #region FIT - ValorizacionDiaria
            //Lista para Barra
            model.ListBarra = (new BarraAppServicio()).ListBarras();
            //Lista para Cliente
            model.ListaEmpresa = servicio.ListarEmpresas().Where(x => x.EMPRCODI > 0).ToList();
            #endregion

            return PartialView(model);
        }

        /// <summary>
        /// Actualiza en BD punto editado y devuelve resultado al cliente
        /// </summary>
        /// <param name="formato"></param>
        /// <param name="tipoinfo"></param>
        /// <param name="punto"></param>
        /// <param name="limsup"></param>
        /// <param name="liminf"></param>
        /// <param name="estado"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult EditarPto(int formato, int tipoinfo, int tptomedi, int punto, decimal limsup, decimal liminf, int estado
                                , int hoja, string observacion, int fila, int diaPlazo, int horaPlazo, string fechaVigencia, int estadoConf, int plazptocodi, int emprcodi
                                , string indcheckadicional)
        {
            FormatoHidrologiaModel model = new FormatoHidrologiaModel();

            try
            {
                base.ValidarSesionJsonResult();

                MePlazoptoDTO model2 = new MePlazoptoDTO();
                MeHojaptomedDTO entity = servicio.GetByIdMeHojaptomed(1, formato, tipoinfo, punto, 1, tptomedi, hoja);

                if (entity != null)
                {
                    entity.Hojaptolimsup = limsup;
                    entity.Hojaptoliminf = liminf;
                    entity.Hojaptoactivo = estado;
                    entity.Lastuser = base.UserName;
                    entity.Lastdate = DateTime.Now;
                    entity.Hptoobservacion = observacion;
                    entity.Hptoindcheck = indcheckadicional ?? "N";

                    model2.Plzptocodi = plazptocodi;
                    model2.Plzptominfila = fila;
                    model2.Plzptodiafinplazo = diaPlazo;
                    model2.Plzptominfinplazo = horaPlazo;
                    model2.Formatcodi = formato;
                    model2.Tipoinfocodi = tipoinfo;
                    model2.Ptomedicodi = punto;
                    model2.Plzptofecharegistro = DateTime.Now;

                    // model2.Emprcodi = emprcodi;

                    servicio.UpdateMeHojaptomed(entity);
                    if (estadoConf == 1)
                    {
                        List<MePlazoptoDTO> lisPlazo = new List<MePlazoptoDTO>();

                        if (fechaVigencia != null)
                        {
                            DateTime fechaDiaFormat = new DateTime(Int32.Parse(fechaVigencia.Substring(6, 4)), Int32.Parse(fechaVigencia.Substring(3, 2)), Int32.Parse(fechaVigencia.Substring(0, 2)));
                            model2.Plzptofechavigencia = fechaDiaFormat;
                            lisPlazo = servHidrologia.ListarMePlazoptoParametro(formato, punto, tipoinfo);
                            var lista = lisPlazo.Where(x => x.Plzptofechavigencia == fechaDiaFormat.Date).ToList();

                            if (lista.Count <= 0)
                            {
                                model2.Plzptousuarioregistro = User.Identity.Name;
                                servHidrologia.SaveMePlazopto(model2);
                                model.Resultado = 2;
                                model.Mensaje = "Se generó nueva extensión correctamente.";
                            }
                            else
                            {
                                model2.Plzptousuariomodificacion = User.Identity.Name;
                                model2.Plzptofechamodificacion = DateTime.Now;
                                servHidrologia.UpdateMePlazopto(model2);
                                model.Resultado = 1;
                                model.Mensaje = "El registro guardó correctamente";
                            }
                        }
                    }
                    else
                    {
                        model.Resultado = 1;
                        model.Mensaje = "El registro guardó correctamente";
                    }

                    #region Ejecutar copia de configuración

                    if (ConstantesPMPO.ListadoFormatcodiPMPO.Contains(formato))
                    {
                        try
                        {
                            //Copiará los puntos de medición de este formato origen a los formatos dependientes.
                            this.servicio.EjecutarCopiaConfiguracion(formato, 3, base.UserName);
                        }
                        catch (Exception ex)
                        {
                            Log.Error(NombreControlador, ex);
                        }
                    }

                    #endregion
                }
                else
                {
                    model.Resultado = -1;
                    model.Mensaje = "No existe la configuración del punto de medición";
                }
            }
            catch (Exception ex)
            {
                Log.Error(NombreControlador, ex);
                model.Mensaje = "Ocurrió un error al momento de guardar la configuración";
                model.Detalle = ex.ToString();
                model.Detalle2 = ex.StackTrace;
                model.Resultado = -2;
            }

            return Json(model);
        }

        /// <summary>
        /// aGREGA UN PTO DE MEDICION AL FORMATO
        /// </summary>
        /// <param name="empresa"></param>
        /// <param name="formato"></param>
        /// <param name="hoja"></param>
        /// <param name="punto"></param>
        /// <param name="medida"></param>
        /// <param name="limsup"></param>
        /// <param name="orden"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult AgregarPto(int empresa, int formato, int punto, int medida, decimal limsup, decimal liminf, int hoja, int tipoPtoMedicion)
        {
            FormatoHidrologiaModel model = new FormatoHidrologiaModel();
            MeHojaptomedDTO hojaptos = new MeHojaptomedDTO();

            //validar si punto pertenece a la empresa
            hojaptos.Ptomedicodi = punto;
            hojaptos.Formatcodi = formato;
            hojaptos.Hojacodi = hoja;
            hojaptos.Hojanumero = 1;
            hojaptos.Hojaptosigno = 1;
            hojaptos.Hojaptoactivo = 1;
            hojaptos.Tipoinfocodi = medida;
            hojaptos.Hojaptolimsup = limsup;
            hojaptos.Hojaptoliminf = liminf;
            hojaptos.Tptomedicodi = tipoPtoMedicion;// - 1;
            hojaptos.Lastdate = DateTime.Now;
            hojaptos.Lastuser = User.Identity.Name;

            try
            {
                base.ValidarSesionJsonResult();

                //if (hoja <= 0) throw new Exception("No existe hoja asociada al reporte."); //Faltó INSERT en la tabla ME_HOJA

                var entity = servicio.GetByIdMeHojaptomed(1, formato, medida, punto, 1, hojaptos.Tptomedicodi, hoja);
                if (entity == null)
                {
                    model.HojaPto = servicio.GrabarHojaPtoMedicion(hojaptos, empresa);
                    model.Resultado = 1;

                    #region Ejecutar copia de configuración

                    if (ConstantesPMPO.ListadoFormatcodiPMPO.Contains(formato))
                    {
                        try
                        {
                            //Copiará los puntos de medición de este formato origen a los formatos dependientes.
                            this.servicio.EjecutarCopiaConfiguracion(formato, 3, base.UserName);
                        }
                        catch (Exception ex)
                        {
                            Log.Error(NombreControlador, ex);
                        }
                    }

                    #endregion
                }
                else
                    model.Resultado = 0;
            }
            catch(Exception ex)
            {
                Log.Error(NombreControlador, ex);
                model.Resultado = -1;
                model.Descripcion = ex.ToString();
            }
            return Json(model);
        }

        /// <summary>
        /// Elimina punto de medicion del formato y lo reordena 
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <param name="idFormato"></param>
        /// <param name="idHoja"></param>
        /// <param name="idOrden"></param>
        /// <param name="idPunto"></param>
        /// <returns></returns>
        public JsonResult EliminarPto(int idEmpresa, int idFormato, int tipoInfo, int tptomedi, int idOrden, int idPunto, int hoja)
        {
            int resultado = 1;
            try
            {
                base.ValidarSesionJsonResult();
                servicio.DeleteMeHojaptomed(idEmpresa, idFormato, tipoInfo, idOrden, idPunto, tptomedi, hoja);

                #region Ejecutar copia de configuración

                if (ConstantesPMPO.ListadoFormatcodiPMPO.Contains(idFormato))
                {
                    try
                    {
                        //Copiará los puntos de medición de este formato origen a los formatos dependientes.
                        this.servicio.EjecutarCopiaConfiguracion(idFormato, 3, base.UserName);
                    }
                    catch (Exception ex)
                    {
                        Log.Error(NombreControlador, ex);
                    }
                }

                #endregion
            }
            catch (Exception ex)
            {
                Log.Error(NombreControlador, ex);
                resultado = -1;
            }
            return Json(resultado);
        }

        /// <summary>
        /// Listar empresas
        /// </summary>
        /// <param name="idTipoEmpresa"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ListarEmpresas(int origlectcodi)
        {
            FormatoHidrologiaModel model = new FormatoHidrologiaModel();

            if (origlectcodi == -1)
                model.ListaEmpresa2 = this.servicio.ListarEmpresasPorTipo(-1);
            else
                model.ListaEmpresa2 = this.servicio.ObtenerListaEmpresaByOriglectcodi(origlectcodi);

            var jsonResult = Json(model);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        /// <summary>
        /// Genera Vista de listado de equipo
        /// </summary>
        /// <param name="empresa"></param>
        /// <param name="familia"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ListarFamilia(int empresa, int origlectcodi)
        {
            FormatoHidrologiaModel model = new FormatoHidrologiaModel();
            if (origlectcodi == -1)
                model.ListaFamilia = this.servicio.ListarFamiliaXEmp(empresa).Where(x => x.Famcodi > 0).ToList();
            else
                model.ListaFamilia = this.servicio.ObtenerFamiliaPorOrigenLecturaEquipo(origlectcodi, empresa).Where(x => x.Famcodi > 0).ToList();

            var jsonResult = Json(model);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        /// <summary>
        /// Genera Vista de listado de categorias
        /// </summary>
        /// <param name="empresa"></param>
        /// <param name="familia"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ListarTipoGrupo(int empresa, int origlectcodi)
        {
            FormatoHidrologiaModel model = new FormatoHidrologiaModel();
            if (origlectcodi == -1)
                model.ListaTipoGrupo = this.servicio.ListarTipoGrupo();
            else
                model.ListaTipoGrupo = this.servicio.ListarTipoGrupoPorOrigenLecturaYEmpresa(origlectcodi, empresa).Where(x => x.Catecodi > 0).ToList();

            var jsonResult = Json(model);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        /// <summary>
        /// Permite devolver los grupos
        /// </summary>
        /// <param name="categoria"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ObtenerGrupos(int categoria, int? idEmpresa)
        {
            List<PrGrupoDTO> list = (new DespachoAppServicio()).ObtenerGruposPorCategoria(categoria, Constantes.EstadoActivo, -1, -1);
            if (idEmpresa > 0) list = list.Where(x => x.Emprcodi == idEmpresa.Value).ToList();

            var jsonResult = Json(list);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        /// <summary>
        /// Genera Vista de listado de equipo
        /// </summary>
        /// <param name="empresa"></param>
        /// <param name="familia"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ListarEquipo(int empresa, int familia)
        {
            FormatoHidrologiaModel model = new FormatoHidrologiaModel();
            model.ListaEquipo = servicio.ObtenerEquiposPorFamilia(empresa, familia).Where(x => x.Equicodi != 0 && x.Equicodi != -1).ToList();
            return Json(model);
        }

        /// <summary>
        /// Genera listado de puntos de medicion 
        /// </summary>
        /// <param name="empresa"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ListarPto(int codigo, int opcion, int? idEmpresa, int? cliente, int? barra, int? origlectcodi)
        {
            //- Debemos modificar aca
            FormatoHidrologiaModel model = new FormatoHidrologiaModel();
            if (opcion != 3)
            {
                model.ListaPtos = servicio.GetByIdEquipoMePtomedicion(codigo, opcion);
            }
            else
            {
                model.ListaPtos = servicio.GetByIdClienteBarraMePtomedicion(idEmpresa, cliente, barra);
            }
            if (origlectcodi > 0) model.ListaPtos = model.ListaPtos.Where(x => x.Origlectcodi == origlectcodi.Value).ToList();

            return Json(model);
        }

        /// <summary>
        /// Actualiza el orden de los ptos de medicion en el formato 
        /// </summary>
        /// <param name="formato"></param>
        /// <param name="hoja"></param>
        /// <param name="empresa"></param>
        /// <param name="id"></param>
        /// <param name="fromPosition"></param>
        /// <param name="toPosition"></param>
        /// <param name="direction"></param>
        public void UpdateOrder(int formato, int hoja, int empresa, int id, int fromPosition, int toPosition, string direction)
        {
            List<MeHojaptomedDTO> lista = servicio.GetByCriteriaMeHojaptomeds(empresa, formato, DateTime.Now, DateTime.Now).Where(x => x.Hojacodi == hoja || -1 == hoja).OrderBy(x => x.Hojaptoorden).ToList();

            if (direction == "back")
            {
                int orden = toPosition;
                List<MeHojaptomedDTO> ltmp = new List<MeHojaptomedDTO>();

                if (lista.Count == fromPosition - 1)
                {
                    ltmp.Add(lista[fromPosition - 2]);
                    ltmp.AddRange(lista.GetRange(toPosition - 1, fromPosition - toPosition - 1));
                }
                else
                {
                    ltmp.Add(lista[fromPosition - 1]);
                    ltmp.AddRange(lista.GetRange(toPosition - 1, fromPosition - toPosition));
                }


                foreach (var reg in ltmp)
                {
                    reg.Hojaptoorden = orden;
                    this.servicio.UpdateMeHojaptomed(reg); //Actualizar el orden
                    orden++;
                }
            }
            else
            {
                int orden = fromPosition;
                List<MeHojaptomedDTO> ltmp = new List<MeHojaptomedDTO>();
                if (lista.Count > toPosition - fromPosition)
                {
                    ltmp.AddRange(lista.GetRange(fromPosition, toPosition - fromPosition - 1));
                    ltmp.Add(lista[fromPosition - 1]);
                }
                else
                {
                    ltmp.AddRange(lista.GetRange(fromPosition, toPosition - fromPosition));
                    ltmp.Add(lista[fromPosition - 1]);
                }

                foreach (var reg in ltmp)
                {
                    reg.Hojaptoorden = orden;
                    this.servicio.UpdateMeHojaptomed(reg); //Actualizar el orden
                    orden++;
                }
            }
        }

        /// <summary>
        /// Genera listado de Tipo punto de medicion 
        /// </summary>
        /// <param name="empresa"></param>
        /// <returns></returns>
        public JsonResult ListarDataFiltroXPtomedicodi(int opcion, int origlectcodi, int ptomedicodi)
        {
            FormatoHidrologiaModel model = new FormatoHidrologiaModel();
            model.ListaEmpresa2 = new List<SiEmpresaDTO>();
            model.ListaFamilia = new List<EqFamiliaDTO>();
            model.ListaEquipo = new List<EqEquipoDTO>();
            model.ListaPtos = new List<MePtomedicionDTO>();

            try
            {
                MePtomedicionDTO regPto = this.servicio.GetByIdMePtomedicion(ptomedicodi);
                origlectcodi = regPto != null ? regPto.Origlectcodi ?? -1 : origlectcodi;

                model.ListaEmpresa2 = this.servicio.ObtenerListaEmpresaByOriglectcodi(origlectcodi);

                if (regPto != null)
                {
                    model.ListaOrigenLectura = this.servicio.ListMeOrigenlecturas().Where(x => x.Origlectcodi != 0 && x.Origlectcodi != -1).OrderBy(x => x.Origlectnombre).ToList();
                    model.Origlectcodi = regPto.Origlectcodi ?? -1;

                    model.IdEmpresa = regPto.Emprcodi ?? 0;

                    switch (opcion)
                    {
                        case 1:
                            model.ListaFamilia = this.servicio.ObtenerFamiliaPorOrigenLecturaEquipo(origlectcodi, model.IdEmpresa).Where(x => x.Famcodi > 0).ToList();
                            model.IdFamilia = regPto.Famcodi;

                            model.ListaEquipo = this.servicio.ObtenerEquiposPorFamilia(regPto.Emprcodi ?? 0, regPto.Famcodi).Where(x => x.Equicodi != 0 && x.Equicodi != -1).ToList();
                            model.IdEquipo = regPto.Equicodi ?? 0;
                            break;
                        case 2:
                            model.ListaTipoGrupo = this.servicio.ListarTipoGrupoPorOrigenLecturaYEmpresa(origlectcodi, model.IdEmpresa).Where(x => x.Catecodi > 0).ToList();
                            model.IdTipoGrupo = regPto.Catecodi;

                            model.ListaGrupos = (new DespachoAppServicio()).ObtenerGruposPorCategoria(model.IdTipoGrupo, Constantes.EstadoActivo, -1, -1).Where(x=>x.Emprcodi == model.IdEmpresa).ToList();
                            model.IdGrupo = regPto.Grupocodi ?? 0;

                            break;
                    }
                    model.ListaPtos = new List<MePtomedicionDTO>() { regPto };
                    model.Ptomedicodi = regPto.Ptomedicodi;
                }
            }
            catch (Exception ex)
            {
                Log.Error(NombreControlador, ex);
                model.Resultado = -1;
                model.Mensaje = ex.Message;
                model.Detalle = ex.Message + ConstantesAppServicio.CaracterEnter + ex.StackTrace;
            }

            var jsonResult = Json(model);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        #endregion

        #region Verificacion de Formato
        /// <summary>
        /// Index para visualizar el detalle de las verificaciones de formato
        /// </summary>
        /// <returns></returns>
        public ActionResult IndexVerificacionFormato(int? id, int? app)
        {
            //validación
            if (!base.IsValidSesionView()) return base.RedirectToLogin();

            VerificacionFormatoModel model = new VerificacionFormatoModel();
            var formato = servicio.GetByIdMeFormato(id.Value);
            if (formato != null)
            {
                model.Formatcodi = formato.Formatcodi;
            }
            model.CodigoApp = app ?? 0;

            return View(model);
        }

        /// <summary>
        /// Devuelve vista parcial para mostrar listado de verificacion
        /// </summary>
        /// <returns></returns>
        public PartialViewResult ListaVerificacionFormato(int idFormato)
        {
            VerificacionFormatoModel model = new VerificacionFormatoModel();

            MeFormatoDTO formato = this.servicio.GetByIdMeFormato(idFormato);
            model.Formatcodi = formato.Formatcodi;
            model.Formatnomb = formato.Formatnombre;

            model.ListaVerificacionFormato = this.servicio.ListMeVerificacionFormatosByFormato(idFormato);
            foreach (var vf in model.ListaVerificacionFormato)
            {
                vf.FmtverifestadoDescripcion = _lsEstadosFlag.Where(x => x.EstadoCodigo == vf.Fmtverifestado).First().EstadoDescripcion;
            }

            return PartialView(model);
        }

        /// <summary>
        /// Permite mostrar la vista para la creacion de verificacion formato
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult AgregarVerificacionFormato(int idFormato)
        {
            VerificacionFormatoModel model = new VerificacionFormatoModel();
            model.ListaVerificacion = this.servicio.ListMeVerificacions();
            model.ListaEstadoFlag = _lsEstadosFlag;

            MeFormatoDTO formato = this.servicio.GetByIdMeFormato(idFormato);
            model.Formatcodi = formato.Formatcodi;
            model.Formatnomb = formato.Formatnombre;

            return PartialView(model);
        }

        /// <summary>
        /// Registrar VerificacionFormato
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult RegistrarVerificacionFormato(VerificacionFormatoModel model)
        {
            try
            {
                base.ValidarSesionJsonResult();

                var fmtverif = new MeVerificacionFormatoDTO();
                fmtverif.Formatcodi = model.Formatcodi;
                fmtverif.Verifcodi = model.Verifcodi;
                fmtverif.Fmtverifestado = model.Fmtverifestado;
                fmtverif.Fmtverifusucreacion = User.Identity.Name;

                //Validación de existencia
                MeVerificacionFormatoDTO oactual = this.servicio.GetByIdMeVerificacionFormato(fmtverif.Formatcodi, fmtverif.Verifcodi);
                if (oactual != null)
                {
                    return Json("Verificación de formato ya registrado");
                }

                this.servicio.SaveMeVerificacionFormato(fmtverif);
                return Json(1);
            }
            catch (Exception ex)
            {
                Log.Error(NombreControlador, ex);
                return Json(-1);
            }
        }

        /// <summary>
        /// Ver informacion de verificación formato
        /// </summary>
        [HttpPost]
        public PartialViewResult MostrarVerificacionFormato(int idFormato, int idVerif)
        {
            MeVerificacionFormatoDTO oactual = this.servicio.GetByIdMeVerificacionFormato(idFormato, idVerif);
            var modelo = new VerificacionFormatoModel
            {
                Formatnomb = oactual.Formatnomb,
                Verifnomb = oactual.Verifnomb,

                FmtverifestadoDescripcion = _lsEstadosFlag.Where(x => x.EstadoCodigo == oactual.Fmtverifestado).First().EstadoDescripcion,
                Fmtverifusucreacion = oactual.Fmtverifusucreacion,
                Fmtveriffeccreacion = oactual.Fmtveriffeccreacion.HasValue ? oactual.Fmtveriffeccreacion.Value.ToString(Constantes.FormatoFechaFull) : "",
                Fmtverifusumodificacion = oactual.Fmtverifusumodificacion,
                Fmtveriffecmodificacion = oactual.Fmtveriffecmodificacion.HasValue ? oactual.Fmtveriffecmodificacion.Value.ToString(Constantes.FormatoFechaFull) : ""
            };

            return PartialView(modelo);
        }

        /// <summary>
        /// Editar categoria
        /// </summary>
        [HttpPost]
        public PartialViewResult EditarVerificacionFormato(int idFormato, int idVerif)
        {
            MeVerificacionFormatoDTO oactual = this.servicio.GetByIdMeVerificacionFormato(idFormato, idVerif);
            var modelo = new VerificacionFormatoModel
            {
                Formatcodi = oactual.Formatcodi,
                Formatnomb = oactual.Formatnomb,
                Verifcodi = oactual.Verifcodi,
                Verifnomb = oactual.Verifnomb,
                Fmtverifestado = oactual.Fmtverifestado
            };

            modelo.ListaEstadoFlag = _lsEstadosFlag;

            return PartialView(modelo);
        }

        /// <summary>
        /// Actualizar VerificacionFormato
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ActualizarVerificacionFormato(VerificacionFormatoModel model)
        {
            try
            {
                base.ValidarSesionJsonResult();

                var fmtverif = new MeVerificacionFormatoDTO();
                fmtverif.Formatcodi = model.Formatcodi;
                fmtverif.Verifcodi = model.Verifcodi;
                fmtverif.Fmtverifestado = model.Fmtverifestado;
                fmtverif.Fmtverifusumodificacion = User.Identity.Name;

                this.servicio.UpdateMeVerificacionFormato(fmtverif);
                return Json(1);
            }
            catch (Exception ex)
            {
                Log.Error(NombreControlador, ex);
                return Json(-1);
            }
        }

        #endregion

        /// <summary>
        /// Listar historico de  Extensión plazo
        /// </summary>
        /// <param name="idFT"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult ListarHistorico(int formatcodi, int tptomedi, int tipoinfo)
        {
            // if (!base.IsValidSesion) throw new Exception(Constantes.MensajeSesionExpirado);
            FormatoHidrologiaModel model = new FormatoHidrologiaModel();
            model.ListaPlazoPto = servHidrologia.ListarMePlazoptoParametro(formatcodi, tptomedi, tipoinfo);
            var formato = servicio.GetByIdMeFormato(formatcodi);
            model.Periodo = formato.Formatperiodo.Value;
            return PartialView(model);
        }

        ///// <summary>
        ///// Eliminar grupodat
        ///// </summary>
        ///// <param name="idFT"></param>
        ///// <returns></returns>
        //[HttpPost]
        public JsonResult PlazoExtenEliminar(int id)
        {
            FormatoHidrologiaModel model = new FormatoHidrologiaModel();
            MePlazoptoDTO lisPlazo = new MePlazoptoDTO();
            try
            {
                base.ValidarSesionJsonResult();

                this.servHidrologia.DeleteMePlazopto(id);
                model.Resultado = 1;
            }
            catch (Exception ex)
            {
                model.Resultado = -1;
            }
            return Json(model);
        }

        /// <summary>
        /// Ejecutar copia de configuración
        /// </summary>
        /// <param name="formatcodi"></param>
        /// <param name="tipoCopia"></param>
        /// <returns></returns>
        public JsonResult EjecutarCopiaConfiguracion(int formatcodi, int tipoCopia)
        {
            FormatoHidrologiaModel model = new FormatoHidrologiaModel();
            try
            {
                base.ValidarSesionJsonResult();

                model.Resultado = this.servicio.EjecutarCopiaConfiguracion(formatcodi, tipoCopia, base.User.Identity.Name);
            }
            catch (Exception ex)
            {
                Log.Error(NombreControlador, ex);
                model.Resultado = -1;
                model.Mensaje = ex.Message;
                model.Detalle = ex.StackTrace;
            }

            return Json(model);
        }

        /// <summary>
        /// Ejecutar copia de configuración
        /// </summary>
        /// <param name="formatcodi"></param>
        /// <param name="tipoCopia"></param>
        /// <returns></returns>
        public JsonResult AgregarHojaExcelWeb(int formatcodi, int lectcodi, int cabcodi, string nombre)
        {
            FormatoHidrologiaModel model = new FormatoHidrologiaModel();
            try
            {
                base.ValidarSesionJsonResult();

                MeHojaDTO hoja = new MeHojaDTO();
                hoja.Formatcodi = formatcodi;
                hoja.Hojaorden = 1;
                hoja.Lectcodi = lectcodi;
                hoja.Cabcodi = cabcodi;
                hoja.Hojanombre = nombre;

                this.servicio.SaveMeHoja(hoja);

                model.Resultado = 1;
            }
            catch (Exception ex)
            {
                Log.Error(NombreControlador, ex);
                model.Resultado = -1;
                model.Mensaje = ex.Message;
                model.Detalle = ex.StackTrace;
            }

            return Json(model);
        }

    }
}
