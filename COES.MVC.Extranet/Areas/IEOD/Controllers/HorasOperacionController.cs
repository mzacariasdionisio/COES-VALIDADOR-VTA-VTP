using COES.Dominio.DTO.Sic;
using COES.Framework.Base.Tools;
using COES.MVC.Extranet.Areas.IEOD.Models;
using COES.MVC.Extranet.Controllers;
using COES.MVC.Extranet.Helper;
using COES.MVC.Extranet.SeguridadServicio;
using COES.Servicios.Aplicacion.FormatoMedicion;
using COES.Servicios.Aplicacion.General.Helper;
using COES.Servicios.Aplicacion.Helper;
using COES.Servicios.Aplicacion.IEOD;
using COES.Servicios.Aplicacion.Migraciones.Helper;
using log4net;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using COES.Base.Core;

namespace COES.MVC.Extranet.Areas.IEOD.Controllers
{
    public class HorasOperacionController : FormatoController
    {
        readonly IEODAppServicio servicio = new IEODAppServicio();
        private readonly SeguridadServicioClient servSeguridad = new SeguridadServicioClient();

        #region Declaración de variables

        private static readonly ILog Log = log4net.LogManager.GetLogger(typeof(HorasOperacionController));
        private static string NameController = MethodBase.GetCurrentMethod().DeclaringType.Name;

        /// <summary>
        /// Excepciones ocurridas en el controlador
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

        #endregion

        #region METODOS HORAS DE OPERACION

        /// <summary>
        /// Index Registro Horas de Operación
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            if (!base.IsValidSesion) return base.RedirectToLogin();
            if (this.IdModulo == null) return base.RedirectToHomeDefault();

            HorasOperacionModel model = new HorasOperacionModel();
            CargarFiltrosBusqueda(model);
            return View(model);
        }

        [HttpPost]
        public JsonResult Lista(string sFecha, int idEmpresa, int idTipoCentral, int idCentral, int idEnvio)
        {
            HorasOperacionModel jsModel = new HorasOperacionModel();
            DateTime fechaPeriodo = DateTime.ParseExact(sFecha, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            DateTime fechaActual = DateTime.Now;
            jsModel.TipoModOp = 1; // modo de operacion normal

            try
            {
                jsModel.FechaListado = sFecha;

                List<EqEquipoDTO> listaCentralesByEmprYFamcodi = this.servHO.ListarCentralesXEmpresaGener(idEmpresa, fechaPeriodo.ToString(ConstantesBase.FormatoFecha)).Where(x => x.Famcodipadre == idTipoCentral).ToList();
                List<EqEquipoDTO> listaCentrales = listaCentralesByEmprYFamcodi.GroupBy(x => new { x.Codipadre, x.Nombrecentral }).Select(y => new EqEquipoDTO() { Equicodi = y.Key.Codipadre, Equinomb = y.Key.Nombrecentral }).ToList();

                jsModel.ListaCentrales = listaCentrales.OrderBy(x => x.Equinomb).ToList();

                jsModel.IdTipoCentral = idTipoCentral;
                List<EqEquipoDTO> ListaGrupoXCentral = new List<EqEquipoDTO>();
                if (jsModel.ListaCentrales.Count > 0)
                {
                    //agrupamos los grupos de todas las centrales en una sola lista
                    foreach (var entity in jsModel.ListaCentrales)
                    {
                        var listaAux = this.servHO.ListarGruposxCentralGEN(-2, entity.Equicodi, ConstantesHorasOperacion.IdGeneradorHidroelectrico);
                        if (listaAux.Count > 0)
                        {
                            foreach (var obj in listaAux)
                            {
                                ListaGrupoXCentral.Add(obj);
                            }
                        }
                    }
                }
                
                jsModel.ListaGrupo = ListaGrupoXCentral;

                int empr = this.servIEOD.getEmpresaMigracion(idEmpresa, sFecha);

                if(empr > 0)
                {
                    jsModel.ListaModosOperacion = this.servHO.ListarModoOperacionXCentralYEmpresa(idCentral, empr);
                    jsModel.ListaUnidades = this.servHO.ListarGruposxCentralGEN(empr, idCentral, ConstantesHorasOperacion.IdGeneradorTemoelectrico);
                    if (jsModel.ListaUnidades.Count > 0) jsModel.ListaUnidXModoOP = this.servHO.ListarUnidadesWithModoOperacionXCentralYEmpresa(idCentral, empr.ToString());
                }
                else
                {
                    jsModel.ListaModosOperacion = this.servHO.ListarModoOperacionXCentralYEmpresa(idCentral, idEmpresa);
                    jsModel.ListaUnidades = this.servHO.ListarGruposxCentralGEN(idEmpresa, idCentral, ConstantesHorasOperacion.IdGeneradorTemoelectrico);
                    if (jsModel.ListaUnidades.Count > 0) jsModel.ListaUnidXModoOP = this.servHO.ListarUnidadesWithModoOperacionXCentralYEmpresa(idCentral, idEmpresa.ToString());
                }                

                //Obtención del código de fuente de datos
                List<int> listaFenergcodi = new List<int>();
                listaFenergcodi.AddRange(jsModel.ListaModosOperacion.Where(x => x.Fenergcodi > 0).Select(x => x.Fenergcodi.Value).ToList());
                listaFenergcodi.AddRange(jsModel.ListaUnidades.Where(x => x.Fenergcodi > 0).Select(x => x.Fenergcodi).ToList());

                int fdatcodi = this.servHO.GetFdatcodiByFamcodi(idTipoCentral, listaFenergcodi);

                SiFuentedatosDTO objSifuentedatos = new SiFuentedatosDTO();
                objSifuentedatos = servicio.GetByIdSiFuentedatos(ConstantesIEOD.FdatcodiPadreHOP);

                int fdatcodiEnvio = ConstantesIEOD.FdatcodiHOPTermoelectricaBiogasBagazo == fdatcodi? ConstantesIEOD.FdatcodiHOPTermoelectrica : fdatcodi;
                jsModel.ListaEnvios = servicio.GetByCriteriaMeEnvios(idEmpresa, 0, fechaPeriodo).Where(x => x.Fdatcodi == fdatcodiEnvio).ToList();

                ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                jsModel.EsEmpresaVigente = servFormato.EsEmpresaVigente(idEmpresa, fechaPeriodo);

                /// Validación de Envio
                jsModel.PlazoEnvio = this.servicio.GetByIdSiPlazoenvioByFdatcodi(fdatcodi);
                jsModel.PlazoEnvio.Emprcodi = idEmpresa;
                jsModel.PlazoEnvio.IdEnvio = idEnvio;
                if (idEnvio <= 0)// no hay envios anteriores
                {
                    jsModel.PlazoEnvio.FechaProceso = EPDate.GetFechaIniPeriodo(ParametrosFormato.PeriodoDiario, string.Empty, string.Empty, sFecha, Constantes.FormatoFecha);
                    this.servicio.GetSizePlazoEnvio(jsModel.PlazoEnvio);
                    jsModel.PlazoEnvio.TipoPlazo = this.servicio.EnvioValidarPlazo(jsModel.PlazoEnvio);

                    jsModel.ListaHorasOperacion = this.servHO.GetEveHoraoperacionCriteriaxEmpresaxFecha(idEmpresa, fechaPeriodo, fechaPeriodo.AddDays(1), idCentral);
                }
                else// envio anterior
                {
                    MeEnvioDTO envioAnt = servFormato.GetByIdMeEnvio(idEnvio);
                    jsModel.PlazoEnvio.FechaEnvio = envioAnt.Enviofecha;
                    jsModel.PlazoEnvio.FechaProceso = (DateTime)envioAnt.Enviofechaperiodo;
                    jsModel.PlazoEnvio.TipoPlazo = envioAnt.Envioplazo;
                    jsModel.FechaEnvio = jsModel.PlazoEnvio.FechaEnvio.Value.ToString(ConstantesAppServicio.FormatoFechaFull);

                    /// mostrar los datos de un historico de envios
                    List<MeEnviodetDTO> listaDetalleEnvio = servicio.GetByCriteriaMeEnviodets(idEnvio);
                    List<int> listaDetalleEnvioAux = listaDetalleEnvio.Select(x=>x.Envdetfpkcodi).ToList();
                    string strCodHop = string.Join(",", listaDetalleEnvioAux.ToArray());
                    if (strCodHop != "")
                    {
                        jsModel.ListaHorasOperacion = this.servHO.GetEveHoraoperacionCriteriaxPKCodis(strCodHop);
                    }
                    else
                    {
                        jsModel.ListaHorasOperacion = this.servHO.GetEveHoraoperacionCriteriaxEmpresaxFecha(idEmpresa, fechaPeriodo, fechaPeriodo.AddDays(1), idCentral);
                    }

                }
                jsModel.ListaModosOperacion = jsModel.ListaModosOperacion.Where(x => jsModel.ListaHorasOperacion.Any(y => x.Grupocodi == y.Grupocodi) || x.Grupoactivo == ConstantesAppServicio.SI).ToList();
                jsModel.ListaHorasOperacionAnt = this.servHO.GetEveHoraoperacionCriteriaxEmpresaxFecha(idEmpresa, fechaActual.AddDays(-1), fechaActual, idCentral); // datos del dia anterior                                                     
                jsModel.ListaTipoOperacion = this.servHO.ListarTipoOperacionHO();

                jsModel.MensajeNotifuniesp = jsModel.TipoModOp == 0 && jsModel.ListaHorasOperacion != null && jsModel.ListaHorasOperacion.Where(x => x.Hopnotifuniesp == 1).Count() > 0 ?
                    ConstantesHorasOperacion.MensajeNotificacionUnidEspecial : string.Empty;
                ///*******************************************************************************

                return Json(jsModel);
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                return Json(new { error = -1, descripcion = ex.Message });
            }
        }

        /// <summary>
        /// Genera la vista para registrar datos de hora de operacion
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <param name="idTipoCentral"></param>
        /// <param name="idCentral"></param>
        /// <param name="opcion"></param> opcion 0: modificar, 1: nueva
        /// <param name="idEquipo"></param>
        /// <returns></returns>
        public PartialViewResult ViewIngresoHorasOperacion(string objJsonForm)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();

            HorasOperacionModel model = objJsonForm != null ? serializer.Deserialize<HorasOperacionModel>(objJsonForm) : new HorasOperacionModel();


            DateTime fecha = DateTime.ParseExact(model.Fecha, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            model.FechaAnterior = fecha.AddDays(-1).ToString(Constantes.FormatoFecha);
            model.FechaSiguiente = fecha.AddDays(1).ToString(Constantes.FormatoFecha);
            model.ListaFechaArranque = new List<string>();
            model.ListaFechaArranque.Add(model.Fecha);
            model.ListaFechaArranque.Add(fecha.AddDays(-1).ToString(Constantes.FormatoFecha));
            if (!model.ListaFechaArranque.Contains(model.Fechahorordarranq))
                model.ListaFechaArranque.Add(model.Fechahorordarranq);
            if (model.Hophorordarranq == "") model.Hophorordarranq = null;
            if (model.Hophorparada == "") model.Hophorparada = null;

            model.ActFiltroCtral = (model.IdTipoCentral == ConstantesHorasOperacion.IdTipoTermica) ? "disabled" : "";

            model.ListaCentrales = this.servHO.ListarCentralesXEmpresaGener(model.IdEmpresa)
                                  .Where(x => x.Famcodipadre == model.IdTipoCentral)
                                  .GroupBy(x => new { x.Codipadre, x.Nombrecentral })
                                  .Select(y => new EqEquipoDTO() { Equicodi = y.Key.Codipadre, Equinomb = y.Key.Nombrecentral })
                                  .ToList();

            model.ListaTipoOperacion = this.servHO.ListarTipoOperacionHO();

            if (model.IdPos > -1) // si es modificación de hora de operacion
            {
                model.IdEquipo = model.IdEquipoOrIdModo;
                model.IdGrupoModo = model.IdEquipoOrIdModo;
                model.ActFiltroCtral = "disabled";
                switch (model.IdTipoCentral)
                {
                    case ConstantesHorasOperacion.IdTipoHidraulica: //Hidraulicas                     
                        model.IdCentralSelect = (int)servicio.GetEquipo(model.IdEquipoOrIdModo).Equipadre;
                        break;
                    case ConstantesHorasOperacion.IdTipoSolar: //Solares                        
                    case ConstantesHorasOperacion.IdTipoEolica: //Eolicas
                        model.IdCentralSelect = model.IdEquipoOrIdModo;
                        break;
                }
            }
            else // si es nueva hora de operacion 
            {
                model.FechaFin = fecha.AddDays(1).ToString(Constantes.FormatoFecha);
                model.IdCentralSelect = model.IdCentralSelect == -1 ? model.ListaCentrales[0].Equicodi : model.IdCentralSelect;
            }

            //Determinar si la Central es un Reserva fria que necesita registrar las unidades en el campo descripción
            model.FlagCentralRsvFriaToRegistrarUnidad = ConstantesHorasOperacion.ListaEquicodiRsvFriaToRegistrarUnidad.Contains(model.IdCentralSelect) ? 1 : 0;

            if (model.IdTipoCentral == ConstantesHorasOperacion.IdTipoHidraulica) //Ctral Hidraulica
                model.EtiquetaFiltro = ConstantesHorasOperacion.EtiquetaGrupo;

            if (model.IdTipoCentral == ConstantesHorasOperacion.IdTipoHidraulica && model.IdPos == -1) //Ctral Hidraulica y no es modificación
            {
                model.EtiquetaFiltro = ConstantesHorasOperacion.EtiquetaGrupo;
                model.EtiquetaFiltroGrupoModo = ConstantesHorasOperacion.EtiquetaListaGrupo;
            }

            if (model.IdTipoCentral == ConstantesHorasOperacion.IdTipoTermica) //Térmicas
            {
                model.EtiquetaFiltro = ConstantesHorasOperacion.EtiquetaModo;
                model.EtiquetaFiltroGrupoModo = ConstantesHorasOperacion.EtiquetaListaModo;

                //*************************************************************************************************************
                //********* Para ciertas empresas y centrales con modos de operacion y unidades asociadas manualmente
                //*************************************************************************************************************

                int idModo = model.IdEquipoOrIdModo;
                PrGrupoDTO modoEspecial = idModo > 0 ? this.servHO.ListarModoOperacionXCentralYEmpresa(model.IdCentralSelect, model.IdEmpresa)
                                           .Find(x => x.FlagModoEspecial == ConstantesHorasOperacion.FlagModoEspecial && x.Grupocodi == idModo) : null;
                if (modoEspecial != null)
                {
                    model.ListaUnidades = this.servHO.ListarGruposxCentralGENEspecial(idModo, ConstantesHorasOperacion.IdGeneradorTemoelectrico);
                    if (model.ListaUnidades.Count > 1)
                    {
                        model.TipoModOp = 0; // modo de operacion tipo especial
                        foreach (var obj in model.ListaUnidades)
                        {
                            if (model.MatrizunidadesExtra != null)
                            {
                                var entity = model.MatrizunidadesExtra.Find(x => x.Equicodi == obj.Equicodi);
                                if (entity != null) // si existen unidades selecionadas capturamos su tipo de operacion
                                {
                                    obj.EquiCodiSelect = entity.Equicodi;
                                }
                            }
                            else
                            { // si viene de BD
                                if (model.IdPos != -1)
                                { // si hay datos y es para modificacion
                                    List<EveHoraoperacionDTO> ListaHop = this.servHO.GetEveHoraoperacionCriteriaxEmpresaxFecha(model.IdEmpresa, fecha, fecha.AddDays(1), model.IdCentralSelect);
                                    var entity2 = (EveHoraoperacionDTO)ListaHop[model.IdPos];
                                    foreach (var fil in ListaHop)
                                    {
                                        if (fil.Hopcodipadre == entity2.Hopcodi)
                                        {
                                            // si es su unidad correspondiente del modo de operación
                                            obj.EquiCodiSelect = (int)fil.Equicodi;
                                        }
                                    }
                                }
                                else // si es un nuevo hora de operación
                                {
                                    obj.EquiCodiSelect = obj.Equicodi; // habilitamos check a todas las unidades cuando es una nueva hora de operacion.
                                }

                            }
                        }
                    }
                }
            }

            //Rangos permitidos para centrales solares
            List<SiParametroValorDTO> listaParam = this.servParametro.ListSiParametroValorByIdParametro(ConstantesParametro.IdParametroRangoSolar);
            model.ParamSolar = this.servParametro.GetParametroRangoCentralSolar(listaParam, fecha, ParametrosFormato.ResolucionCuartoHora);

            return PartialView(model);
        }

        /// <summary>
        /// Genera la lista de tipos de centrales
        /// </summary>
        /// <param name="idsAgente"></param>
        /// <returns></returns>
        public PartialViewResult CargarTipoCentral(int idEmpresa, string fecha)
        {
            HorasOperacionModel model = new HorasOperacionModel();
            DateTime dfecha = DateTime.ParseExact(fecha, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            List<EqEquipoDTO> Lista = this.servHO.ListarCentralesXEmpresaGener(idEmpresa, dfecha.ToString(ConstantesBase.FormatoFecha));

            List<EqEquipoDTO> listaTCentral = Lista.GroupBy(x => new { x.Famcodipadre, x.Famnomb })
                                .Select(y => new EqEquipoDTO() { Famcodi = y.Key.Famcodipadre, Famnomb = y.Key.Famnomb })
                                .OrderBy(x => x.Famnomb).ToList();
            model.ListaTipoCentral = listaTCentral;
            return PartialView(model);
        }

        /// <summary>
        /// Genera la lista de centrales
        /// </summary>
        /// <param name="idsAgente"></param>
        /// <returns></returns>
        public PartialViewResult CargarCentral(int idEmpresa, int tipoCentral, string fecha)
        {
            HorasOperacionModel model = new HorasOperacionModel();

            DateTime dfecha = DateTime.ParseExact(fecha, Constantes.FormatoFecha, CultureInfo.InvariantCulture);

            List<EqEquipoDTO> ListaActivo = (this.servHO.ListarCentralesXEmpresaGener(idEmpresa, dfecha.ToString(ConstantesBase.FormatoFecha)).Where(x => x.Famcodipadre == tipoCentral && x.Equiestado == "A").ToList());
            List<EqEquipoDTO> ListaDesac = (this.servHO.ListarCentralesXEmpresaGener(idEmpresa, dfecha.ToString(ConstantesBase.FormatoFecha)).Where(x => x.Famcodipadre == tipoCentral && x.Equiestado == "F" && x.Equifechfinopcom > DateTime.ParseExact(fecha, Constantes.FormatoFecha, CultureInfo.InvariantCulture)).ToList());

            List<EqEquipoDTO> Lista = new List<EqEquipoDTO>();
            Lista.AddRange(ListaActivo);
            Lista.AddRange(ListaDesac);

            Lista = Lista.GroupBy(x => new { x.Codipadre, x.Nombrecentral })
                    .Select(y => new EqEquipoDTO()
                    {
                        Equicodi = y.Key.Codipadre,
                        Equinomb = y.Key.Nombrecentral
                    }
                    ).ToList();
            model.ListaCentrales = Lista.OrderBy(x => x.Equinomb).ToList();
            return PartialView(model);
        }

        /// <summary>
        /// Carga el listado de modos de operacion o Grupos de operacion según el tipo de central seleccionada
        /// </summary>
        /// <param name="idCentral"></param>
        /// <param name="IdEmpresa"></param>
        /// <returns></returns>
        public PartialViewResult CargarListaGrupoModo(int idCentral, int idEmpresa, int idTipoCentral, int modoGrupoSelect, int pos)
        {
            HorasOperacionModel model = new HorasOperacionModel();
            int idModGrup = 0;
            model.IdTipoCentral = idTipoCentral;
            model.ActFiltroCtral = "";
            if (pos > -1)
            {
                model.ActFiltroCtral = "disabled";
            }
            model.IdPos = pos; // posicion de la hora de operacion a modificar en el listado Horas de operacion 
            switch (idTipoCentral)
            {
                case ConstantesHorasOperacion.IdTipoHidraulica: //Hidraulicas                     
                    model.ListaGrupo = this.servHO.ListarGruposxCentralGEN(idEmpresa, idCentral, ConstantesHorasOperacion.IdGeneradorHidroelectrico);
                    if (model.ListaGrupo.Count > 0)
                        idModGrup = (int)model.ListaGrupo[0].Equicodi;
                    break;
                case ConstantesHorasOperacion.IdTipoTermica: //Térmicas
                    model.ListaModosOperacion = this.servHO.ListarModoOperacionXCentralYEmpresa(idCentral, idEmpresa).Where(x => x.Grupocodi == modoGrupoSelect || x.Grupoactivo == ConstantesAppServicio.SI).ToList(); ;
                    if (model.ListaModosOperacion.Count > 0)
                        idModGrup = (int)model.ListaModosOperacion[0].Grupocodi;
                    break;
                case ConstantesHorasOperacion.IdTipoSolar: //Solares
                case ConstantesHorasOperacion.IdTipoEolica: //Eolicas
                    break;
            }
            if (modoGrupoSelect != 0)
                model.IdGrupoModo = modoGrupoSelect;
            else
                model.IdGrupoModo = idModGrup;

            model.FlagCentralRsvFriaToRegistrarUnidad = ConstantesHorasOperacion.ListaEquicodiRsvFriaToRegistrarUnidad.Contains(idCentral) ? 1 : 0;

            return PartialView(model);
        }

        /// <summary>
        /// Cargar lista de Unidades Especiales
        /// </summary>
        /// <param name="idTipoCentral"></param>
        /// <param name="idEmpresa"></param>
        /// <param name="idCentral"></param>
        /// <returns></returns>
        public PartialViewResult CargarListaUnidadesModo(int idTipoCentral, int idEmpresa, int idCentral, int idModo)
        {
            HorasOperacionModel model = new HorasOperacionModel();
            model.TipoModOp = -1;
            model.IdTipoOperSelect = ConstantesSubcausaEvento.SubcausaNoIdentificado;
            model.IdTipoCentral = idTipoCentral;

            if (idTipoCentral == ConstantesHorasOperacion.IdTipoHidraulica) //Ctral Hidraulica
            {
                model.EtiquetaFiltro = ConstantesHorasOperacion.EtiquetaGrupo;
                model.EtiquetaFiltroGrupoModo = ConstantesHorasOperacion.EtiquetaListaGrupo;
                model.TipoModOp = 0; // modo de operacion tipo especial
                model.ListaUnidades = this.servHO.ListarGruposxCentralGEN(idEmpresa, idCentral, ConstantesHorasOperacion.IdGeneradorHidroelectrico);
                model.ListaTipoOperacion = this.servHO.ListarTipoOperacionHO();
            }

            if (idTipoCentral == ConstantesHorasOperacion.IdTipoTermica)
            {
                model.EtiquetaFiltro = ConstantesHorasOperacion.EtiquetaModo;
                model.EtiquetaFiltroGrupoModo = ConstantesHorasOperacion.EtiquetaListaModo;

                PrGrupoDTO modoEspecial = idModo > 0 ? this.servHO.ListarModoOperacionXCentralYEmpresa(idCentral, idEmpresa)
                                           .Find(x => x.FlagModoEspecial == ConstantesHorasOperacion.FlagModoEspecial && x.Grupocodi == idModo) : null;
                if (modoEspecial != null)
                {
                    var listaUnidades = this.servHO.ListarGruposxCentralGENEspecial(idModo, ConstantesHorasOperacion.IdGeneradorTemoelectrico);
                    if (listaUnidades.Count > 1)
                    {
                        model.TipoModOp = 0;
                        model.ListaUnidades = listaUnidades;
                        model.ListaTipoOperacion = this.servHO.ListarTipoOperacionHO();
                    }
                }
            }

            return PartialView(model);
        }

        /// <summary>
        /// Registro de Horas Operacion
        /// </summary>
        /// <param name="data"></param>
        /// <param name="idEmpresa"></param>
        /// <param name="tipoCentral"></param>
        /// <param name="fecha"></param>
        /// <returns></returns>
        public JsonResult RegistrarEnvioHorasOperacion(List<EveHoraoperacionDTO> data, string fecha, int idEmpresa, int tipoCentral, int idCentral, int flagConfirmarInterv)
        {
            //////// Definicion de Variables ////////////////
            HorasOperacionModel model = new HorasOperacionModel();

            try
            {
                base.ValidarSesionJsonResult();

                //////// Definicion de Variables ////////////////
                DateTime fechaHO = DateTime.ParseExact(fecha, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                DateTime fechaEnvio = DateTime.Now;
                string usuarioEnvio = User.Identity.Name;

                //Verificación de plazo
                var listaModosOperacion = this.servHO.ListarModoOperacionXCentralYEmpresa(idCentral, idEmpresa);
                var listaUnidades = this.servHO.ListarGruposxCentralGEN(idEmpresa, idCentral, ConstantesHorasOperacion.IdGeneradorTemoelectrico);
                List<int> listaFenergcodi = new List<int>();
                listaFenergcodi.AddRange(listaModosOperacion.Where(x => x.Fenergcodi > 0).Select(x => x.Fenergcodi.Value).ToList());
                listaFenergcodi.AddRange(listaUnidades.Where(x => x.Fenergcodi > 0).Select(x => x.Fenergcodi).ToList());

                int fdatcodi = this.servHO.GetFdatcodiByFamcodi(tipoCentral, listaFenergcodi);
                int fdatcodiEnvio = ConstantesIEOD.FdatcodiHOPTermoelectricaBiogasBagazo == fdatcodi ? ConstantesIEOD.FdatcodiHOPTermoelectrica : fdatcodi;

                SiPlazoenvioDTO plazoEnvio = this.servicio.GetByIdSiPlazoenvioByFdatcodi(fdatcodi);
                plazoEnvio.Emprcodi = idEmpresa;
                plazoEnvio.FechaProceso = EPDate.GetFechaIniPeriodo(ParametrosFormato.PeriodoDiario, string.Empty, string.Empty, fecha, Constantes.FormatoFecha);
                this.servicio.GetSizePlazoEnvio(plazoEnvio);
                plazoEnvio.TipoPlazo = this.servicio.EnvioValidarPlazo(plazoEnvio);

                if (ConstantesEnvio.ENVIO_PLAZO_DESHABILITADO == plazoEnvio.TipoPlazo)
                    throw new Exception("El envió no está en el Plazo Permitido. El plazo está definido entre " + plazoEnvio.FechaPlazoIni.ToString(ConstantesAppServicio.FormatoFechaFull) + " y " + plazoEnvio.FechaPlazoFuera.ToString(ConstantesAppServicio.FormatoFechaFull));

                //////////Validar con Intervenciones /////////////////       
                if (ConstantesHorasOperacion.FlagConfirmarValIntervenciones != flagConfirmarInterv)
                {
                    this.servHO.VerificarIntervencionFS(data, fechaHO, true, out List<EveHoraoperacionDTO> listaHoOut, out List<ResultadoValidacionAplicativo> listaValInterv);
                    if (listaValInterv.Count() > 0)
                    {
                        model.ListaHorasOperacion = listaHoOut;
                        model.ListaValidacionHorasOperacionIntervencion = listaValInterv;
                        model.Resultado = 2;
                        return Json(model);
                    }
                }

                ///////////////Guardar Envio//////////////////////////                
                MeEnvioDTO envio = new MeEnvioDTO();
                envio.Formatcodi = ConstantesHorasOperacion.IdFormato;
                envio.Fdatcodi = fdatcodiEnvio;
                envio.Archcodi = 0;
                envio.Emprcodi = idEmpresa;
                envio.Estenvcodi = ParametrosEnvio.EnvioEnviado;

                envio.Enviofecha = fechaEnvio;
                envio.Enviofechaperiodo = fechaHO;
                envio.Envioplazo = plazoEnvio.TipoPlazo;
                envio.Enviofechaini = plazoEnvio.FechaInicio;
                envio.Enviofechafin = plazoEnvio.FechaFin;
                envio.Enviofechaplazoini = plazoEnvio.FechaPlazoIni;
                envio.Enviofechaplazofin = plazoEnvio.FechaPlazoFin;

                envio.Lastuser = usuarioEnvio;
                envio.Lastdate = fechaEnvio;
                envio.Userlogin = usuarioEnvio;

                int idEnvio = servicio.SaveMeEnvio(envio);
                ///////////////////////////////////////////////////////
                /////////Guardar Horas de Operación////////////////////
                List<int> listCodHop = new List<int>(), listCodHopElim = new List<int>();
                var listaHorasOperacionBDAntes = this.servHO.GetListaHorasOperacionByCriteria(idEmpresa, ConstantesHorasOperacion.ParamTipoOperacionTodos, fechaHO, fechaHO.AddDays(1), tipoCentral, Int32.Parse(ConstantesHorasOperacion.ParamCentralTodos));
                this.servHO.GuardarHorasdeOperacionAgente(data, idEmpresa, listaHorasOperacionBDAntes, ref listCodHop, ref listCodHopElim, usuarioEnvio);
                var listaHorasOperacionBDDespues = this.servHO.GetListaHorasOperacionByCriteria(idEmpresa, ConstantesHorasOperacion.ParamTipoOperacionTodos, fechaHO, fechaHO.AddDays(1), tipoCentral, Int32.Parse(ConstantesHorasOperacion.ParamCentralTodos));

                //*********
                envio.Enviocodi = idEnvio;
                envio.Cfgenvcodi = -1;
                envio.Estenvcodi = ParametrosEnvio.EnvioAprobado;
                servicio.UpdateMeEnvio(envio);

                /////////Grabar Detalle de envios////////////////////
                listCodHop = listCodHop.Select(o => o).Distinct().ToList();
                foreach (var entity in listCodHop)
                {
                    var entityEnvioDet = new MeEnviodetDTO();
                    entityEnvioDet.Enviocodi = idEnvio;
                    entityEnvioDet.Envdetfpkcodi = entity;
                    servicio.SaveMeEnviodet(entityEnvioDet);
                }
                ///////////////////////////////////////////////////////

                /// Enviar notificacion
                this.servHO.EnviarCorreoNotificacionHayCambioHorasOperacion(listCodHopElim, listaHorasOperacionBDDespues, listaHorasOperacionBDAntes, DateTime.Now, plazoEnvio.FechaProceso, idEmpresa, idEnvio);

                /// Enviar notificación Intervenciones
                if (ConstantesHorasOperacion.FlagConfirmarValIntervenciones == flagConfirmarInterv)
                {
                    this.servHO.VerificarIntervencionFS(data, fechaHO, true, out List<EveHoraoperacionDTO> listaHoOut, out List<ResultadoValidacionAplicativo> listaValInterv);
                    if (listaValInterv.Count() > 0)
                    {
                        var arrayUsuario = this.servSeguridad.ListarUsuarios();
                        List<UsuarioParametro> listaUsuario = arrayUsuario.Select(x => this.ConvertirUsuarioServicio(x)).ToList();

                        foreach (var reg in listaValInterv)
                        {
                            UsuarioParametro usuario = listaUsuario.Find(x => x.UserLogin == reg.UltimaModificacionUsuarioDesc);
                            reg.UltimaModificacionUsuarioCorreo = usuario != null ? usuario.UserEmail : string.Empty;
                        }

                        this.servHO.EnviarCorreoNotificacionIntervencionesFS(listaHoOut, listaValInterv, usuarioEnvio, fechaEnvio, fechaHO, idEmpresa, idEnvio);
                    }
                }

                //////////////////////////////////////////////////////

                plazoEnvio.IdEnvio = idEnvio;
                plazoEnvio.FechaEnvio = envio.Enviofecha;

                model.PlazoEnvio = plazoEnvio;
                model.Resultado = 1;
                model.FechaEnvio = plazoEnvio.FechaEnvio.Value.ToString(ConstantesAppServicio.FormatoFechaFull);
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = -1;
                model.Mensaje = ex.Message;
                model.Detalle = ex.StackTrace;
            }

            return Json(model);
        }

        /// <summary>
        /// Convertir objetos de Usuario
        /// </summary>
        /// <param name="user"></param>
        /// <param name="listaArea"></param>
        /// <returns></returns>
        private UsuarioParametro ConvertirUsuarioServicio(UserDTO user)
        {
            UsuarioParametro u = new UsuarioParametro();
            u.UserCode = user.UserCode;
            u.UsernName = user.UsernName;
            u.UserLogin = user.UserLogin;
            u.AreaCode = user.AreaCode.GetValueOrDefault(0);
            u.UserEmail = user.UserEmail;

            return u;
        }

        #endregion

        #region UTIL

        /// <summary>
        /// Actualiza la informacion necesaria para cargar los filtros de busqueda en la vista principal
        /// </summary>
        /// <param name="model"></param>
        private void CargarFiltrosBusqueda(HorasOperacionModel model)
        {
            base.ValidarSesionUsuario();

            bool accesoEmpresa = base.VerificarAccesoAccion(Acciones.AccesoEmpresa, User.Identity.Name);
            List<SiEmpresaDTO> emprUsuario = base.ListaEmpresas.Select(x => new SiEmpresaDTO() { Emprcodi = x.EMPRCODI, Emprnomb = x.EMPRNOMB }).ToList();
            model.ListaEmpresas = this.servHO.ListarEmpresasHorasOperacionByTipoCentral(accesoEmpresa, emprUsuario, ConstantesHorasOperacion.CodFamilias);
            model.ListaEmpresas = model.ListaEmpresas.Count > 0 ? model.ListaEmpresas : new List<SiEmpresaDTO>() { new SiEmpresaDTO() { Emprcodi = 0, Emprnomb = "No Existe" } };

            if (model.Fecha == null)
            {
                model.Fecha = DateTime.Now.ToString(Constantes.FormatoFecha);
            }
        }

        private string GeneraStrCodigos(List<int> listCodHop)
        {
            string cadena = String.Empty;

            for (int i = 0; i < listCodHop.Count; i++)
            {
                if (i == listCodHop.Count - 1) // si es el ultimo
                    cadena += listCodHop[i].ToString();
                else
                    cadena += listCodHop[i].ToString() + ",";
            }
            return cadena;
        }

        #endregion

    }
}
