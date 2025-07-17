using COES.Dominio.DTO.Sic;
using COES.Framework.Base.Tools;
using COES.MVC.Extranet.Areas.Subastas.Helper;
using COES.MVC.Extranet.Areas.Subastas.Models;
using COES.MVC.Extranet.Controllers;
using COES.MVC.Extranet.SeguridadServicio;
using COES.Servicios.Aplicacion.Equipamiento;
using COES.Servicios.Aplicacion.Helper;
using COES.Servicios.Aplicacion.Subastas;
using log4net;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using static COES.MVC.Extranet.Areas.Subastas.Models.SubastaModel;
using Constantes2 = COES.MVC.Extranet.Helper.Constantes;
using HelperCommon = COES.MVC.Extranet.Helper;

namespace COES.MVC.Extranet.Areas.Subastas.Controllers
{
    public class SubastaController : BaseController
    {
        readonly SubastasAppServicio appServicioSubastas = new SubastasAppServicio();

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

        public SubastaController()
        {
            log4net.Config.XmlConfigurator.Configure();
        }

        private readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private static string NameController = MethodBase.GetCurrentMethod().DeclaringType.Name;

        #endregion

        /// <summary>
        /// Muestra la pantalla inicial
        /// </summary>
        /// <returns></returns>
        public ActionResult Defecto()
        {
            if (!base.IsValidSesionView()) return base.RedirectToLogin();
            string usuarioCorreo = base.UserEmail;
            bool isUsuarioCoes = usuarioCorreo.Contains("coes.");
            string[] fechaIntervaloTemporal = ConfigurationManager.AppSettings["rsfAsimetricoMostrarIntervaloFecha"].Split('_');
            //DateTime.Now.AddDays(1)
            DateTime fechaIntervaloTemporalInicio = DateTime.ParseExact(fechaIntervaloTemporal[0], "dd.MM.yyyy", CultureInfo.InvariantCulture);
            DateTime fechaIntervaloTemporalFin = DateTime.ParseExact(fechaIntervaloTemporal[1], "dd.MM.yyyy", CultureInfo.InvariantCulture);
            bool isEnIntervaloTemporal = appServicioSubastas.ExisteVigenteSmaOfertaSimetricaHorario();
            //bool isEnIntervaloTemporal = DateTime.Compare(fechaIntervaloTemporalInicio.Date, DateTime.Now.Date) <= 0 && DateTime.Compare(fechaIntervaloTemporalFin.Date, DateTime.Now.Date) >= 0 ? true : false;

            ViewData["maxdias"] = 30;

            SubastaModel objSubastaModel = new SubastaModel()
            {
                TipoOferta = Constantes.TipoOferta.Defecto,
                FechaOfertaDesc = DateTime.Now.AddMonths(1).AddDays(1).ToString(ConstantesAppServicio.FormatoMes)
            };


            ViewBag.usuarioCorreo = usuarioCorreo;
            ViewBag.isUsuarioCoes = isUsuarioCoes;
            ViewBag.isEnIntervaloTemporal = isEnIntervaloTemporal;

            return View(objSubastaModel);
        }

        /// <summary>
        /// Muestra la pantalla inicial
        /// </summary>
        /// <returns></returns>
        public ActionResult Diaria()
        {
            if (!base.IsValidSesionView()) return base.RedirectToLogin();
            string usuarioCorreo = base.UserEmail;
            bool isUsuarioCoes = usuarioCorreo.Contains("coes.");
            string[] fechaIntervaloTemporal = ConfigurationManager.AppSettings["rsfAsimetricoMostrarIntervaloFecha"].Split('_');
            //DateTime.Now.AddDays(1)
            DateTime fechaIntervaloTemporalInicio = DateTime.ParseExact(fechaIntervaloTemporal[0], "dd.MM.yyyy", CultureInfo.InvariantCulture);
            DateTime fechaIntervaloTemporalFin = DateTime.ParseExact(fechaIntervaloTemporal[1], "dd.MM.yyyy", CultureInfo.InvariantCulture);
            bool isEnIntervaloTemporal = appServicioSubastas.ExisteVigenteSmaOfertaSimetricaHorario();
            //bool isEnIntervaloTemporal = DateTime.Compare(fechaIntervaloTemporalInicio.Date, DateTime.Now.Date) <= 0 && DateTime.Compare(fechaIntervaloTemporalFin.Date, DateTime.Now.Date) >= 0 ? true : false; 

            //return ((UserDTO)Session[DatosSesion.SesionUsuario]).UserLogin;;

            //string usuarioCorreo = base.UserEmail;
            //((UserDTO)Session[DatosSesion.SesionUsuario]).UserEmail;

            SmaParamProcesoDTO reg = this.appServicioSubastas.GetParamValidoEnvioyProcesoAutomatico();

            ViewData["maxdias"] = reg.Papomaxdiasoferta;

            SubastaModel objSubastaModel = new SubastaModel()
            {
                TipoOferta = Constantes.TipoOferta.Diaria,
                FechaOfertaDesc = DateTime.Now.AddDays(1).ToString(ConstantesAppServicio.FormatoFecha),
            };

            ViewBag.usuarioCorreo = usuarioCorreo;
            ViewBag.isUsuarioCoes = isUsuarioCoes;
            ViewBag.isEnIntervaloTemporal = isEnIntervaloTemporal;

            return View(objSubastaModel);
        }

        /// <summary>
        /// Método usado para validar fechas al crear nuevas Ofertas
        /// </summary>
        /// <param name="collection"></param>
        /// <returns></returns>
        public string CrearNuevoDefecto(FormCollection collection)
        {
            base.ValidarSesionUsuario();
            string fechaEscogida = Convert.ToString(collection["fechaesc"]);
            string fechaActual = Convert.ToDateTime(DateTime.Now.AddDays(1)).ToString(ConstantesAppServicio.FormatoFecha); //Sumar +1 Dia por Fecha de Participacion

            if (fechaEscogida != fechaActual) { return "true"; } else { return "false"; }

        }

        /// <summary>
        /// Método usado para Listar las ofertas registradas a nivel de envíos
        /// </summary>
        /// <param name="tipo"></param>
        /// <param name="fechaenvio"></param>
        /// <param name="collection"></param>
        /// <returns></returns>
        public JsonResult EnvioListar(int tipo, string strfechaoferta)
        {
            SubastaModel modelResult = new SubastaModel();

            try
            {
                base.ValidarSesionUsuario();

                DateTime fechaOferta = appServicioSubastas.GetFechaInput(tipo, strfechaoferta);
                List<SmaOfertaDTO> arrSmaOfertas = this.appServicioSubastas.ListSmaOfertasxDia(tipo, fechaOferta, fechaOferta, this.GetUserCodeSession(), -1, "-1", ConstantesSubasta.FuenteExtranet);

                EnvioListItem[] arrEnvios = new EnvioListItem[arrSmaOfertas.Count];

                for (int i = 0, l = arrEnvios.Length; i < l; i++)
                {
                    SmaOfertaDTO objSmaOferta = arrSmaOfertas[i];

                    arrEnvios[i] = new SubastaModel.EnvioListItem()
                    {
                        Codigo = objSmaOferta.Ofercodi,
                        CodigoEnvio = objSmaOferta.Ofercodenvio,
                        FechaEnvio = objSmaOferta.Oferfechaenvio.Value.ToString(ConstantesAppServicio.FormatoFechaFull),
                        FechaOferta = appServicioSubastas.GetFechaOfertaDesc(tipo, objSmaOferta.Oferfechainicio.Value)
                    };
                }

                modelResult.ListaEnvio = arrEnvios;
            }

            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                modelResult.Resultado = -1;
                modelResult.Mensaje = ex.Message;
                modelResult.Detalle = ex.StackTrace;
            }

            return Json(modelResult);
        }

        /// <summary>
        /// Método usado para listar el detalle de las Ofertas
        /// </summary>
        /// <param name="tipo"></param>
        /// <param name="fechaenvio"></param>
        /// <param name="codigo"></param>
        /// <param name="estado"></param>
        /// <returns></returns>
        public JsonResult OfertaListar(int tipo, string strfechaoferta, int codigo, string estado, int accion)
        {
            SubastaModel modelResult = new SubastaModel();

            try
            {
                base.ValidarSesionUsuario();

                DateTime fechaOferta = appServicioSubastas.GetFechaInput(tipo, strfechaoferta);
                DateTime fechaOfertaEnvio = fechaOferta;
                if (codigo > 0)
                {
                    var objEnvio = this.appServicioSubastas.GetByIdSmaOferta(codigo);
                    if (objEnvio != null && objEnvio.Usercode == this.GetUserCodeSession())
                    {
                        fechaOfertaEnvio = objEnvio.Oferfechainicio.Value;
                        fechaOferta = ConstantesSubasta.AccionExcelWebCopiar == accion ? fechaOferta : fechaOfertaEnvio;
                        estado = objEnvio.Oferestado;

                        modelResult.OferCodi = codigo;
                        modelResult.OferCodenvio = objEnvio.Ofercodenvio;
                        modelResult.OferfechaenvioDesc = objEnvio.Oferfechaenvio.Value.ToString(ConstantesAppServicio.FormatoFechaFull);
                        modelResult.OferEstado = objEnvio.Oferestado;
                    }
                    else
                    {
                        throw new ArgumentException("Código de envío inválido.");
                    }
                }

                modelResult.FechaOferta = appServicioSubastas.GetFechaOfertaDesc(tipo, fechaOferta);
                modelResult.ObjParametros = GetParametros(tipo, fechaOferta, accion);

                modelResult.ListaTabSubir = new OfertaListItem[0];
                modelResult.ListaTabBajar = new OfertaListItem[0];
                if (accion != ConstantesSubasta.AccionExcelWebCrearNuevo)
                {
                    var modelTab = GetModelSubastaExtranet(tipo, fechaOfertaEnvio, this.GetUserCodeSession(), codigo, estado);
                    modelResult.ListaTabSubir = modelTab.ListaTabSubir;
                    modelResult.ListaTabBajar = modelTab.ListaTabBajar;
                }
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                modelResult.Resultado = -1;
                modelResult.Mensaje = ex.Message;
                modelResult.Detalle = ex.StackTrace;
            }

            return Json(modelResult);
        }

        #region Métodos - Oferta listar

        /// <summary>
        /// Model utilizado para el handsontable
        /// </summary>
        /// <param name="tipo"></param>
        /// <param name="fechaOferta"></param>
        /// <param name="usercode"></param>
        /// <param name="codigo"></param>
        /// <param name="estado"></param>
        /// <returns></returns>
        private SubastaModel GetModelSubastaExtranet(int tipo, DateTime fechaOferta, int usercode, int codigo, string estado)
        {

            SubastaModel modelResult = new SubastaModel();

            List<SmaOfertaDTO> listaOferta = this.appServicioSubastas.ListSmaOfertas(tipo, fechaOferta, usercode, codigo, estado, true);

            List<int> listaTipo = new List<int>() { ConstantesSubasta.TipoCargaSubir, ConstantesSubasta.TipoCargaBajar };
            foreach (var tipoCarga in listaTipo)
            {
                List<SmaOfertaDTO> arrSmaOfertas = listaOferta.Where(x => x.Ofdetipo == tipoCarga).ToList();

                OfertaListItem[] arrOfertas = new OfertaListItem[arrSmaOfertas.Count];

                decimal lCantd = 0;
                decimal lIndice = 0;
                int urscodi = 0;
                string horaInicio = "";

                for (int i = 0, l = arrOfertas.Length; i < l; i++)
                {
                    SmaOfertaDTO objSmaOferta = arrSmaOfertas[i];

                    if (objSmaOferta.Urscodi != urscodi || (objSmaOferta.Ofdehorainicio != horaInicio)) // Cambio de URS
                    {
                        lIndice = 0;
                        var noticesGrouped = arrSmaOfertas.Where(n => n.Urscodi == objSmaOferta.Urscodi && n.Ofdehorainicio == objSmaOferta.Ofdehorainicio)
                                            .GroupBy(n => n.Urscodi)
                                            .Select(group =>
                                                 new
                                                 {
                                                     NoticeName = group.Key,
                                                     Notices = group.ToList(),
                                                     Count = group.Count()
                                                 });

                        foreach (var item in noticesGrouped)
                        {
                            lCantd = item.Count;
                        }
                    }
                    else
                        lIndice++;

                    arrOfertas[i] = new OfertaListItem()
                    {
                        URS = objSmaOferta.Urscodi,
                        HoraInicio = objSmaOferta.Ofdehorainicio,
                        HoraFin = objSmaOferta.Ofdehorafin,
                        Precio = objSmaOferta.Repoprecio,
                        Grupocodi = objSmaOferta.Grupocodi,  //.OferlistMO, // Aqui cambio
                        PotenciaOfertada = objSmaOferta.Repopotofer, //Potencia ofertada
                        Indice = lIndice,
                        Cantidad = lCantd,
                        BandaCalificada = objSmaOferta.BandaCalificada,
                        BandaDisponible = objSmaOferta.BandaDisponible,
                    };

                    urscodi = objSmaOferta.Urscodi;
                    horaInicio = objSmaOferta.Ofdehorainicio;
                }

                if (ConstantesSubasta.TipoCargaSubir == tipoCarga)
                    modelResult.ListaTabSubir = arrOfertas;
                if (ConstantesSubasta.TipoCargaBajar == tipoCarga)
                    modelResult.ListaTabBajar = arrOfertas;
            }

            return modelResult;

        }

        /// <summary>
        /// Método usado para listar los URS en cada Oferta que se registra
        /// 
        /// </summary>
        /// <returns></returns>
        private Parametros GetParametros(int tipo, DateTime fechaOferta, int accion)
        {
            Parametros objParametros = new Parametros();

            //Obtener lista de URS
            List<SmaUsuarioUrsDTO> arrSmaUsuarioUrsDTO = this.appServicioSubastas.GetByCriteriaSmaUsuarioUrss(this.GetUserCodeSession());
            objParametros.URSs = new SubastaModel.URSListItem[arrSmaUsuarioUrsDTO.Count];

            //Obtener mantenimientos de Modos de Operacion
            List<SmaUrsModoOperacionDTO> arrSmaUrsModoOperacionDTOMantt = this.appServicioSubastas.GetURSMOMantto(this.GetUserCodeSession(), fechaOferta);

            int[] arrUrs = new int[arrSmaUsuarioUrsDTO.Count];

            for (int i = 0, l = objParametros.URSs.Length; i < l; i++)
            {
                SmaUsuarioUrsDTO objSmaUsuarioUrsDTO = arrSmaUsuarioUrsDTO[i];
                int intURSId = objSmaUsuarioUrsDTO.Urscodi.Value;
                arrUrs[i] = intURSId;

                if (intURSId == 914)
                { }

                objParametros.URSs[i] = new SubastaModel.URSListItem()
                {
                    ID = intURSId,
                    Text = objSmaUsuarioUrsDTO.Ursnomb.Trim(),
                    OperationModes = ModoOperacionListar(intURSId, arrSmaUrsModoOperacionDTOMantt, out bool flag),
                    TieneBandaCalificada = flag
                };
            }


            if (accion == ConstantesSubasta.AccionExcelWebCrearNuevo)
            {
                //Solo mostrar las URS que tengan Banda Calificada
                objParametros.URSs = (objParametros.URSs).Where(x => x.TieneBandaCalificada).ToArray();
            }

            //Determinar valores Osinergmin
            SmaTraerParametrosDTO objSmaTraerParametrosDTO = appServicioSubastas.GetTraerParametros(fechaOferta);

            //Aqui actualizar los rangos horarios
            DateTime dteHoraActual = DateTime.Now;

            objParametros.TieneOfertaPorDefecto = objSmaTraerParametrosDTO.TParamOfertaDefecto;

            //ampliación de plazo (se utiliza para la cuenta regresiva en pantalla)
            objParametros.TieneExcelWebHabilitado = appServicioSubastas.EsFechaHabilitado(tipo, fechaOferta, out DateTime fechaHoraIniPlazo, out DateTime fechaHoraFinPlazo);
            objParametros.HoraInicioParaOfertar = fechaHoraIniPlazo.ToUniversalTime().Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds;
            objParametros.HoraFinParaOfertar = fechaHoraFinPlazo.ToUniversalTime().Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds;
            objParametros.HoraActual = dteHoraActual.ToUniversalTime().Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds;

            objParametros.PrecioMinimo = objSmaTraerParametrosDTO.TParamPrecioMinimo;
            objParametros.PrecioMaximo = objSmaTraerParametrosDTO.TParamPrecioMaximo;

            objParametros.PotenciaMinimaMan = objSmaTraerParametrosDTO.TParamPotenciaMinimaMan;

            //Ordenar alfabeticamente las URS
            objParametros.URSs = (objParametros.URSs).OrderBy(x => x.Text).ToArray();

            return objParametros;
        }

        /// <summary>
        /// Metodo Listar todos los Modos de Operacion , Potencias e Intervalos de Mantenimiento
        /// </summary>
        /// <param name="idUrs"></param>
        /// <param name="arrSmaUrsModoOperacionDTOMantt"></param>
        /// <returns></returns>
        private SubastaModel.ModoOperacionListItem[] ModoOperacionListar(int idUrs, List<SmaUrsModoOperacionDTO> arrSmaUrsModoOperacionDTOMantt, out bool flagBandaCalificada)
        {
            flagBandaCalificada = false;
            DespachoAppServicio despachoService = new DespachoAppServicio();
            List<SmaUrsModoOperacionDTO> arrSmaUrsModoOperacionDTO = this.appServicioSubastas.ListSmaUrsModoOperacions_MO(idUrs);
            SubastaModel.ModoOperacionListItem[] arrOperationModeList = new SubastaModel.ModoOperacionListItem[arrSmaUrsModoOperacionDTO.Count];

            for (int i = 0, l = arrSmaUrsModoOperacionDTO.Count; i < l; i++)
            {
                SmaUrsModoOperacionDTO objSmaUrsModoOperacionDTO = arrSmaUrsModoOperacionDTO[i];
                SmaUrsModoOperacionDTO objMant = arrSmaUrsModoOperacionDTOMantt.Find(s => (s.Urscodi == idUrs && s.Grupocodi == objSmaUrsModoOperacionDTO.Grupocodi));

                /**/
                //Averiguar si es Urs_Reserva_Firme

                decimal ProvisionPaseFirme = 0;
                bool urs_rsv_firme = false;
                var obtdatmo = despachoService.ObtenerDatosMO_URS((int)objSmaUrsModoOperacionDTO.Grupocodi, DateTime.Now);
                if (obtdatmo.Count > 0)
                {
                    for (int j = 0; j < obtdatmo.Count; j++)
                    {
                        if (objSmaUrsModoOperacionDTO.Grupotipo == "T")
                        {
                            switch (obtdatmo[j].Concepcodi)
                            {
                                case 253:
                                    ProvisionPaseFirme = this.appServicioSubastas.AnalizarNumerico(obtdatmo[j].Formuladat) ? Convert.ToDecimal(obtdatmo[j].Formuladat) / 100 : 0;
                                    break;
                            }
                        }
                        else
                        { //Es Hidraulica
                            switch (obtdatmo[j].Concepcodi)
                            {
                                case 254:
                                    ProvisionPaseFirme = this.appServicioSubastas.AnalizarNumerico(obtdatmo[j].Formuladat) ? Convert.ToDecimal(obtdatmo[j].Formuladat) / 100 : 0;
                                    break;

                            }
                        }
                    }
                }
                if (ProvisionPaseFirme > 0) urs_rsv_firme = true;
                /**/

                arrOperationModeList[i] = new SubastaModel.ModoOperacionListItem()
                {
                    ID = objSmaUrsModoOperacionDTO.Grupocodi.Value,
                    Text = objSmaUrsModoOperacionDTO.Gruponomb.Trim(),
                    EsReservaFirme = urs_rsv_firme,
                    Pot = (objMant == null ? null : objMant.CapacidadMax),
                    IntvMant = (objMant == null ? null : objMant.Intervalo),
                    Indice = i,
                    Cantidad = l,

                    BandaDisponible = (objMant == null ? null : objMant.BandaDisponible),
                    BandaCalificada = (objMant == null ? null : objMant.BandaURS),
                    BandaAdjudicada = (objMant == null ? null : objMant.BandaAdjudicada)

                };
            }
            if (arrOperationModeList.Length > 0)
            {
                var bnd = arrOperationModeList.First().BandaCalificada;
                if (bnd != null)
                {
                    flagBandaCalificada = true;
                }

            }


            return arrOperationModeList;
        }

        #endregion

        /// <summary>
        /// Validar magnitudes
        /// </summary>
        /// <param name="ofertasSubir"></param>
        /// <param name="ofertasBajar"></param>
        /// <param name="fechaini"></param>
        /// <returns></returns>
        public JsonResult ValidarCondicionMagnitudes(int tipo, string ofertasSubir, string ofertasBajar, string fechaini)
        {
            SubastaModel modelResult = new SubastaModel();
            try
            {
                base.ValidarSesionUsuario();

                System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();

                SubastaModel.OfertaListItem[] arrOfertasSubir = serializer.Deserialize<SubastaModel.OfertaListItem[]>(ofertasSubir);
                SubastaModel.OfertaListItem[] arrOfertasBajar = serializer.Deserialize<SubastaModel.OfertaListItem[]>(ofertasBajar);

                DateTime fechaInicialOferta;
                if (tipo == ConstantesSubasta.OfertipoDiaria)
                    fechaInicialOferta = !string.IsNullOrEmpty(fechaini) ? DateTime.ParseExact(fechaini, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture) : DateTime.Now.Date;
                else
                    fechaInicialOferta = !string.IsNullOrEmpty(fechaini) ? EPDate.GetFechaIniPeriodo(5, fechaini, string.Empty, string.Empty, string.Empty) : DateTime.Now.Date;

                int usercode = this.GetUserCodeSession();
                string validacionAB = ValidarAB2(arrOfertasSubir, arrOfertasBajar, fechaInicialOferta, usercode);

                if (validacionAB == "")
                    modelResult.Resultado = 1;
                else
                {
                    modelResult.Resultado = 2;
                    modelResult.Mensaje = validacionAB;
                }

            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                modelResult.Resultado = -1;
                modelResult.Mensaje = ex.Message;
                modelResult.Detalle = ex.StackTrace;
            }

            return Json(modelResult);
        }

        /// <summary>
        /// Validar magnitudes
        /// </summary>
        /// <param name="ofertasSubir"></param>
        /// <param name="ofertasBajar"></param>
        /// <param name="fechaini"></param>
        /// <returns></returns>
        public JsonResult ValidarCondicionMagnitudesTemporal(int tipo, string ofertasSubir, string ofertasBajar, string fechaini)
        {
            SubastaModel modelResult = new SubastaModel();
            try
            {
                base.ValidarSesionUsuario();

                System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();

                SubastaModel.OfertaListItem[] arrOfertasSubir = serializer.Deserialize<SubastaModel.OfertaListItem[]>(ofertasSubir);
                SubastaModel.OfertaListItem[] arrOfertasBajar = arrOfertasSubir;// serializer.Deserialize<SubastaModel.OfertaListItem[]>(ofertasBajar);

                DateTime fechaInicialOferta;
                if (tipo == ConstantesSubasta.OfertipoDiaria)
                    fechaInicialOferta = !string.IsNullOrEmpty(fechaini) ? DateTime.ParseExact(fechaini, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture) : DateTime.Now.Date;
                else
                    fechaInicialOferta = !string.IsNullOrEmpty(fechaini) ? EPDate.GetFechaIniPeriodo(5, fechaini, string.Empty, string.Empty, string.Empty) : DateTime.Now.Date;

                int usercode = this.GetUserCodeSession();
                string validacionAB = ValidarAB2(arrOfertasSubir, arrOfertasBajar, fechaInicialOferta, usercode);

                if (validacionAB == "")
                    modelResult.Resultado = 1;
                else
                {
                    modelResult.Resultado = 2;
                    modelResult.Mensaje = validacionAB;
                }

            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                modelResult.Resultado = -1;
                modelResult.Mensaje = ex.Message;
                modelResult.Detalle = ex.StackTrace;
            }

            return Json(modelResult);
        }

        /// <summary>
        /// Validación de traslape
        /// </summary>
        /// <param name="tipo"></param>
        /// <param name="fechaini"></param>
        /// <param name="codigo"></param>
        /// <param name="estado"></param>
        /// <param name="ofertasSubir"></param>
        /// <param name="ofertasBajar"></param>
        /// <param name="numofertas"></param>
        /// <returns></returns>
        public JsonResult ValidarTraslape(int tipo, string fechaini, int codigo, string estado, string ofertasSubir, string ofertasBajar, int numofertas)
        {
            base.ValidarSesionUsuario();
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();

            SubastaModel.OfertaListItem[] arrOfertasSubir = serializer.Deserialize<SubastaModel.OfertaListItem[]>(ofertasSubir);
            SubastaModel.OfertaListItem[] arrOfertasBajar = serializer.Deserialize<SubastaModel.OfertaListItem[]>(ofertasBajar);

            string mensaje = "\n";
            int indice = 1;  //1: sin traslapes ni ofertas realizadas, 2: sin traslapes pero con ofertas ya realizadas, -1: existe traslapes

            DateTime fechaInicialOferta;
            if (tipo == ConstantesSubasta.OfertipoDiaria)
                fechaInicialOferta = !string.IsNullOrEmpty(fechaini) ? DateTime.ParseExact(fechaini, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture) : DateTime.Now.Date;
            else
                fechaInicialOferta = !string.IsNullOrEmpty(fechaini) ? EPDate.GetFechaIniPeriodo(5, fechaini, string.Empty, string.Empty, string.Empty) : DateTime.Now.Date;

            int usercode = this.GetUserCodeSession();

            string traslapesGeneralSubir = "";
            string traslapesGeneralBajar = "";

            //Verifico existencia de traslapes en ambos handsontables 
            for (int n = 0; n < numofertas; n++)
            {
                DateTime fecha = fechaInicialOferta.AddDays(n);

                string traslapesDiarioSubir = GestionarTraslape(arrOfertasSubir, fecha, "Subir", ConstantesSubasta.ArrHoras);
                string traslapesDiarioBajar = GestionarTraslape(arrOfertasBajar, fecha, "Bajar", ConstantesSubasta.ArrHoras);

                traslapesGeneralSubir = traslapesGeneralSubir + traslapesDiarioSubir;
                traslapesGeneralBajar = traslapesGeneralBajar + traslapesDiarioBajar;

            }

            //Cuando no exista traslapes
            if (traslapesGeneralSubir == "" && traslapesGeneralBajar == "")
            {
                for (int k = 0; k < numofertas; k++)
                {
                    DateTime fech = fechaInicialOferta.AddDays(k);

                    SubastaModel.OfertaListItem[] arrOfertasOldSubir = GetModelSubastaExtranet(tipo, fech, usercode, codigo, estado).ListaTabSubir;
                    SubastaModel.OfertaListItem[] arrOfertasOldBajar = GetModelSubastaExtranet(tipo, fech, usercode, codigo, estado).ListaTabBajar;

                    if (arrOfertasOldSubir.Any())
                    {
                        if (arrOfertasOldBajar.Any())
                        {
                            mensaje = mensaje + "<br /> > Existe oferta en ambas hojas (Subir y Bajar) para la fecha : " + fech.ToString(ConstantesAppServicio.FormatoFecha) + "\n";
                            indice = 2;
                        }
                        else
                        {
                            mensaje = mensaje + "<br /> > Existe oferta en la hoja Subir para la fecha : " + fech.ToString(ConstantesAppServicio.FormatoFecha) + "\n";
                            indice = 2;
                        }
                    }
                    else
                    {
                        if (arrOfertasOldBajar.Any())
                        {
                            mensaje = mensaje + "<br /> > Existe oferta en la hoja Bajar para la fecha : " + fech.ToString(ConstantesAppServicio.FormatoFecha) + "\n";

                            indice = 2;
                        }
                        else
                        {

                        }
                    }
                }

            }
            else   //Existe traslapes en los handsontables a guardar
            {

                mensaje = "" + traslapesGeneralSubir + traslapesGeneralBajar;
                indice = -1;

            }

            return Json(new
            {
                mensaje = mensaje,
                indice = indice
            });
        }

        /// <summary>
        /// Validación de traslape
        /// </summary>
        /// <param name="tipo"></param>
        /// <param name="fechaini"></param>
        /// <param name="codigo"></param>
        /// <param name="estado"></param>
        /// <param name="ofertasSubir"></param>
        /// <param name="ofertasBajar"></param>
        /// <param name="numofertas"></param>
        /// <returns></returns>
        public JsonResult ValidarTraslapeTemporal(int tipo, string fechaini, int codigo, string estado, string ofertasSubir, string ofertasBajar, int numofertas)
        {
            base.ValidarSesionUsuario();
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();

            SubastaModel.OfertaListItem[] arrOfertasSubir = serializer.Deserialize<SubastaModel.OfertaListItem[]>(ofertasSubir);
            SubastaModel.OfertaListItem[] arrOfertasBajar = arrOfertasSubir;// serializer.Deserialize<SubastaModel.OfertaListItem[]>(ofertasBajar);

            string mensaje = "\n";
            int indice = 1;  //1: sin traslapes ni ofertas realizadas, 2: sin traslapes pero con ofertas ya realizadas, -1: existe traslapes

            DateTime fechaInicialOferta;
            if (tipo == ConstantesSubasta.OfertipoDiaria)
                fechaInicialOferta = !string.IsNullOrEmpty(fechaini) ? DateTime.ParseExact(fechaini, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture) : DateTime.Now.Date;
            else
                fechaInicialOferta = !string.IsNullOrEmpty(fechaini) ? EPDate.GetFechaIniPeriodo(5, fechaini, string.Empty, string.Empty, string.Empty) : DateTime.Now.Date;

            int usercode = this.GetUserCodeSession();

            string traslapesGeneralSubir = "";
            string traslapesGeneralBajar = "";

            //Verifico existencia de traslapes en ambos handsontables 
            for (int n = 0; n < numofertas; n++)
            {
                DateTime fecha = fechaInicialOferta.AddDays(n);

                string traslapesDiarioSubir = GestionarTraslape(arrOfertasSubir, fecha, "Subir", ConstantesSubasta.ArrHoras);
                string traslapesDiarioBajar = GestionarTraslape(arrOfertasBajar, fecha, "Bajar", ConstantesSubasta.ArrHoras);

                traslapesGeneralSubir = traslapesGeneralSubir + traslapesDiarioSubir;
                traslapesGeneralBajar = traslapesGeneralBajar + traslapesDiarioBajar;

            }

            //Cuando no exista traslapes
            if (traslapesGeneralSubir == "" && traslapesGeneralBajar == "")
            {
                for (int k = 0; k < numofertas; k++)
                {
                    DateTime fech = fechaInicialOferta.AddDays(k);

                    SubastaModel.OfertaListItem[] arrOfertasOldSubir = GetModelSubastaExtranet(tipo, fech, usercode, codigo, estado).ListaTabSubir;
                    SubastaModel.OfertaListItem[] arrOfertasOldBajar = GetModelSubastaExtranet(tipo, fech, usercode, codigo, estado).ListaTabBajar;

                    if (arrOfertasOldSubir.Any())
                    {
                        if (arrOfertasOldBajar.Any())
                        {
                            mensaje = mensaje + "<br /> > Existe oferta en ambas hojas (Subir y Bajar) para la fecha : " + fech.ToString(ConstantesAppServicio.FormatoFecha) + "\n";
                            indice = 2;
                        }
                        else
                        {
                            mensaje = mensaje + "<br /> > Existe oferta en la hoja Subir para la fecha : " + fech.ToString(ConstantesAppServicio.FormatoFecha) + "\n";
                            indice = 2;
                        }
                    }
                    else
                    {
                        if (arrOfertasOldBajar.Any())
                        {
                            mensaje = mensaje + "<br /> > Existe oferta en la hoja Bajar para la fecha : " + fech.ToString(ConstantesAppServicio.FormatoFecha) + "\n";

                            indice = 2;
                        }
                        else
                        {

                        }
                    }
                }

            }
            else   //Existe traslapes en los handsontables a guardar
            {

                mensaje = "" + traslapesGeneralSubir + traslapesGeneralBajar;
                indice = -1;

            }

            return Json(new
            {
                mensaje = mensaje,
                indice = indice
            });
        }

        /// <summary>
        /// Validación al momento de seleccionar fechas para nueva Oferta diaria
        /// </summary>
        /// <param name="strfechaoferta"></param>
        /// <param name="numDia"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ValidarOfertaDiariaYSuOfDefecto(string strfechaoferta, int numDia)
        {
            SubastaModel model = new SubastaModel();
            try
            {
                this.ValidarSesionJsonResult();

                //insumos
                DateTime fechaOferta = appServicioSubastas.GetFechaInput(ConstantesSubasta.OfertipoDiaria, strfechaoferta);
                Parametros objParametros = GetParametros(ConstantesSubasta.OfertipoDiaria, fechaOferta, ConstantesSubasta.AccionExcelWebCrearNuevo);

                //validación
                var flagValido = appServicioSubastas.ExisteOfertaDefectoXRangoFecha(fechaOferta, numDia, objParametros.URSs.Select(x => x.ID).ToList(), out string mensaje);

                if (flagValido)
                {
                    model.Resultado = 1;
                    model.Mensaje = "";
                }
                else
                {
                    model.Resultado = 0;
                    model.Mensaje = mensaje;
                }
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
        /// Método usado para grabar la Oferta
        /// </summary>
        /// <param name="tipo"></param>
        /// <param name="ofertas"></param>
        /// <returns></returns>
        public JsonResult Grabar(int tipo, string ofertasSubir, string ofertasBajar, string fechaini, int numofertas)
        {
            SubastaModel modelResult = new SubastaModel();
            try
            {
                base.ValidarSesionUsuario();

                System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();

                SubastaModel.OfertaListItem[] arrOfertasSubir = serializer.Deserialize<SubastaModel.OfertaListItem[]>(ofertasSubir);
                SubastaModel.OfertaListItem[] arrOfertasBajar = serializer.Deserialize<SubastaModel.OfertaListItem[]>(ofertasBajar);

                /*Cambio Realizado Rango de Fechas */
                string resultado = "";
                SmaOfertaDTO enty = null;

                DateTime fechaInicialOferta;
                if (tipo == ConstantesSubasta.OfertipoDiaria)
                    fechaInicialOferta = !string.IsNullOrEmpty(fechaini) ? DateTime.ParseExact(fechaini, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture) : DateTime.Now.Date;
                else
                    fechaInicialOferta = !string.IsNullOrEmpty(fechaini) ? EPDate.GetFechaIniPeriodo(5, fechaini, string.Empty, string.Empty, string.Empty) : DateTime.Now.Date;

                #region Validar si está en fecha para enviar

                var tieneExcelWebHabilitado = appServicioSubastas.EsFechaHabilitado(tipo, fechaInicialOferta, out DateTime fechaHoraIniPlazo, out DateTime fechaHoraFinPlazo);
                if (!tieneExcelWebHabilitado)
                {
                    string tipoFecha = tipo == ConstantesSubasta.OfertipoDefecto ? "mes" : "día";
                    throw new ArgumentException(string.Format("Cerrado las ofertas para el {0} {1}", tipoFecha, fechaini));
                }

                #endregion

                if (tipo == ConstantesSubasta.OfertipoDefecto)
                    numofertas = 1; //solo es un mes. No se registra varios meses

                for (int k = 0; k < numofertas; k++)
                {
                    enty = new SmaOfertaDTO();
                    enty.Oferfechainicio = fechaInicialOferta.AddDays(k);// Grabar desde fecha inicial seleccionada para participacion -- + 1 dia
                    enty.Oferfechafin = fechaInicialOferta.AddDays(k);
                    enty.Oferfechaenvio = DateTime.Now;
                    enty.Ofertipo = tipo;
                    enty.Oferusucreacion = base.UserName;
                    enty.Usercode = this.GetUserCodeSession();
                    enty.Oferfuente = ConstantesSubasta.FuenteExtranet;

                    List<SmaOfertaDetalleDTO> entityDet = new List<SmaOfertaDetalleDTO>();

                    foreach (SubastaModel.OfertaListItem objOferta_s in arrOfertasSubir)
                    {
                        SmaOfertaDetalleDTO entyDSubida = new SmaOfertaDetalleDTO();

                        entyDSubida.Ofdedusucreacion = base.UserName;
                        entyDSubida.Urscodi = objOferta_s.URS;
                        entyDSubida.Ofdeprecio = objOferta_s.Precio;
                        //entyDSubida.Ofdepotmaxofer = objOferta_s.PotenciaMaxima;//dejar el mismo valor de cada potencia. maxima.
                        entyDSubida.BandaCalificada = objOferta_s.BandaCalificada;
                        entyDSubida.BandaDisponible = objOferta_s.BandaDisponible; //0. No se setea en el handson
                        entyDSubida.Ofdepotofertada = objOferta_s.PotenciaOfertada; //nuevo valor de potencia 
                        entyDSubida.Ofdemoneda = "604";
                        entyDSubida.Ofdehorainicio = objOferta_s.HoraInicio;
                        entyDSubida.Ofdehorafin = objOferta_s.HoraFin;
                        entyDSubida.Grupocodi = Convert.ToInt32(objOferta_s.ModoOperacion);
                        entyDSubida.Ofdetipo = ConstantesSubasta.TipoCargaSubir;

                        entityDet.Add(entyDSubida);
                    }

                    foreach (SubastaModel.OfertaListItem objOferta_b in arrOfertasBajar)
                    {
                        SmaOfertaDetalleDTO entyDBajada = new SmaOfertaDetalleDTO();

                        entyDBajada.Ofdedusucreacion = base.UserName;
                        entyDBajada.Urscodi = objOferta_b.URS;
                        entyDBajada.Ofdeprecio = objOferta_b.Precio;
                        //entyDBajada.Ofdepotmaxofer = objOferta_b.PotenciaMaxima;//dejar el mismo valor de cada potencia. maxima.
                        entyDBajada.BandaCalificada = objOferta_b.BandaCalificada;
                        entyDBajada.BandaDisponible = objOferta_b.BandaDisponible; //0. No se setea en el handson
                        entyDBajada.Ofdepotofertada = objOferta_b.PotenciaOfertada; //nuevo valor de potencia 
                        entyDBajada.Ofdemoneda = "604";
                        entyDBajada.Ofdehorainicio = objOferta_b.HoraInicio;
                        entyDBajada.Ofdehorafin = objOferta_b.HoraFin;
                        entyDBajada.Grupocodi = Convert.ToInt32(objOferta_b.ModoOperacion);
                        entyDBajada.Ofdetipo = ConstantesSubasta.TipoCargaBajar;

                        entityDet.Add(entyDBajada);
                    }

                    resultado += this.appServicioSubastas.SaveSmaOferta(enty, entityDet);//Cambio
                }

                if (resultado != "")
                    modelResult.Resultado = -1;
                modelResult.Mensaje = resultado;
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                modelResult.Resultado = -1;
                modelResult.Mensaje = ex.Message;
                modelResult.Detalle = ex.StackTrace;
            }

            return Json(modelResult);
        }

        /// <summary>
        /// Método usado para grabar la Oferta
        /// </summary>
        /// <param name="tipo"></param>
        /// <param name="ofertas"></param>
        /// <returns></returns>
        public JsonResult GrabarTemporal(int tipo, string ofertasSubir, string ofertasBajar, string fechaini, int numofertas)
        {
            SubastaModel modelResult = new SubastaModel();
            try
            {
                base.ValidarSesionUsuario();

                System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();

                SubastaModel.OfertaListItem[] arrOfertasSubir = serializer.Deserialize<SubastaModel.OfertaListItem[]>(ofertasSubir);
                SubastaModel.OfertaListItem[] arrOfertasBajar = arrOfertasSubir; // serializer.Deserialize<SubastaModel.OfertaListItem[]>(ofertasBajar);

                /*Cambio Realizado Rango de Fechas */
                string resultado = "";
                SmaOfertaDTO enty = null;

                DateTime fechaInicialOferta;
                if (tipo == ConstantesSubasta.OfertipoDiaria)
                    fechaInicialOferta = !string.IsNullOrEmpty(fechaini) ? DateTime.ParseExact(fechaini, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture) : DateTime.Now.Date;
                else
                    fechaInicialOferta = !string.IsNullOrEmpty(fechaini) ? EPDate.GetFechaIniPeriodo(5, fechaini, string.Empty, string.Empty, string.Empty) : DateTime.Now.Date;

                #region Validar si está en fecha para enviar y si tiene Oferta por defecto

                var tieneExcelWebHabilitado = appServicioSubastas.EsFechaHabilitado(tipo, fechaInicialOferta, out DateTime fechaHoraIniPlazo, out DateTime fechaHoraFinPlazo);
                if (!tieneExcelWebHabilitado)
                {
                    throw new ArgumentException("Cerrado las ofertas para el día " + fechaini);
                }

                #endregion

                for (int k = 0; k < numofertas; k++)
                {
                    enty = new SmaOfertaDTO();
                    enty.Oferfechainicio = fechaInicialOferta.AddDays(k);// Grabar desde fecha inicial seleccionada para participacion -- + 1 dia
                    enty.Oferfechafin = fechaInicialOferta.AddDays(k);
                    enty.Oferfechaenvio = DateTime.Now;
                    enty.Ofertipo = tipo;
                    enty.Oferusucreacion = base.UserName;
                    enty.Usercode = this.GetUserCodeSession();
                    enty.Oferfuente = ConstantesSubasta.FuenteExtranet;

                    List<SmaOfertaDetalleDTO> entityDet = new List<SmaOfertaDetalleDTO>();

                    foreach (SubastaModel.OfertaListItem objOferta_s in arrOfertasSubir)
                    {
                        SmaOfertaDetalleDTO entyDSubida = new SmaOfertaDetalleDTO();

                        entyDSubida.Ofdedusucreacion = base.UserName;
                        entyDSubida.Urscodi = objOferta_s.URS;
                        entyDSubida.Ofdeprecio = objOferta_s.Precio;
                        //entyDSubida.Ofdepotmaxofer = objOferta_s.PotenciaMaxima;//dejar el mismo valor de cada potencia. maxima.
                        entyDSubida.BandaCalificada = objOferta_s.BandaCalificada;
                        entyDSubida.BandaDisponible = objOferta_s.BandaDisponible;
                        entyDSubida.Ofdepotofertada = objOferta_s.PotenciaOfertada; //nuevo valor de potencia 
                        entyDSubida.Ofdemoneda = "604";
                        entyDSubida.Ofdehorainicio = objOferta_s.HoraInicio;
                        entyDSubida.Ofdehorafin = objOferta_s.HoraFin;
                        entyDSubida.Grupocodi = Convert.ToInt32(objOferta_s.ModoOperacion);
                        entyDSubida.Ofdetipo = ConstantesSubasta.TipoCargaSubir;

                        entityDet.Add(entyDSubida);
                    }

                    foreach (SubastaModel.OfertaListItem objOferta_b in arrOfertasBajar)
                    {
                        SmaOfertaDetalleDTO entyDBajada = new SmaOfertaDetalleDTO();

                        entyDBajada.Ofdedusucreacion = base.UserName;
                        entyDBajada.Urscodi = objOferta_b.URS;
                        entyDBajada.Ofdeprecio = objOferta_b.Precio;
                        //entyDBajada.Ofdepotmaxofer = objOferta_b.PotenciaMaxima;//dejar el mismo valor de cada potencia. maxima.
                        entyDBajada.BandaCalificada = objOferta_b.BandaCalificada;
                        entyDBajada.BandaDisponible = objOferta_b.BandaDisponible;
                        entyDBajada.Ofdepotofertada = objOferta_b.PotenciaOfertada; //nuevo valor de potencia 
                        entyDBajada.Ofdemoneda = "604";
                        entyDBajada.Ofdehorainicio = objOferta_b.HoraInicio;
                        entyDBajada.Ofdehorafin = objOferta_b.HoraFin;
                        entyDBajada.Grupocodi = Convert.ToInt32(objOferta_b.ModoOperacion);
                        entyDBajada.Ofdetipo = ConstantesSubasta.TipoCargaBajar;

                        entityDet.Add(entyDBajada);
                    }

                    resultado += this.appServicioSubastas.SaveSmaOferta(enty, entityDet);//Cambio
                }

                if (resultado != "")
                    modelResult.Resultado = -1;
                modelResult.Mensaje = resultado;
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                modelResult.Resultado = -1;
                modelResult.Mensaje = ex.Message;
                modelResult.Detalle = ex.StackTrace;
            }

            return Json(modelResult);
        }

        #region Métodos - Validar Intersección y Provision Base

        /// <summary>
        /// Valida si el rango existe las ofertas
        /// </summary>
        /// <param name="horarioMin"></param>
        /// <param name="horarioMax"></param>
        /// <param name="ofertas"></param>
        /// <returns></returns>
        private bool ExisteRangoEnListado(string horarioMin, string horarioMax, OfertaListItem[] ofertas)
        {
            bool existeEnLista = false;

            var lst = ofertas.Where(n => n != null).Where(x => x.HoraInicio == horarioMin && x.HoraFin == horarioMax).ToArray();
            if (lst.Length > 0) { existeEnLista = true; }

            return existeEnLista;
        }

        /// <summary>
        /// Valida que todas las URS cumplan las condiciones de Banda
        /// </summary>
        /// <param name="arrOfertasSubir"></param>
        /// <param name="arrOfertasBajar"></param>
        /// <param name="fecha"></param>
        /// <param name="usercode"></param>
        /// <returns></returns>
        private string ValidarAB2(OfertaListItem[] arrOfertasSubir, OfertaListItem[] arrOfertasBajar, DateTime fecha, int usercode)
        {
            string salida = "";

            List<SmaUsuarioUrsDTO> datoUrsMo = appServicioSubastas.GetByCriteriaSmaUsuarioUrssMO(usercode);

            List<int> listaUrsCodis = datoUrsMo.Select(x => x.Urscodi.Value).Distinct().ToList();

            List<SmaUrsModoOperacionDTO> listaTodasURS = appServicioSubastas.ReporteListadoURS(fecha, ConstantesSubasta.EstadoURSVigente);
            List<SmaUrsModoOperacionDTO> listaTodasURSBase = appServicioSubastas.ReporteListadoURSBase(fecha, ConstantesSubasta.EstadoURSVigente);

            List<SmaUrsModoOperacionDTO> listaUrsGeneral = listaTodasURS.Where(x => listaUrsCodis.Contains(x.Urscodi)).ToList();
            List<SmaUrsModoOperacionDTO> listaUrsGeneralBase = listaTodasURSBase.Where(x => listaUrsCodis.Contains(x.Urscodi)).ToList();

            /////////////////////////////////////////////////////////////////
            string[] arrHoras = ConstantesSubasta.ArrHoras;

            List<int> lstUrsCodisSubir = arrOfertasSubir.Select(x => x.URS).Distinct().ToList();
            List<int> lstUrsCodisBajar = arrOfertasBajar.Select(x => x.URS).Distinct().ToList();

            //Encuentro todas las URS involucradas en el envio de ofertas
            List<int> lstUrstotales = new List<int>();
            lstUrstotales.AddRange(lstUrsCodisSubir);
            lstUrstotales.AddRange(lstUrsCodisBajar);
            lstUrstotales = lstUrstotales.Distinct().ToList();

            //por cada urs involucrado, verificamos su validacion A+B=BC (sin provBase) ó A+B<=BC-Pmin-Pmax (con provBase)
            foreach (var urscodi in lstUrstotales)
            {
                string resultadoUrs = "";

                var regUrsCalif = listaUrsGeneral.First(x => x.Urscodi == urscodi);
                var regUrsBaseTemp = listaUrsGeneralBase.Where(x => x.Urscodi == urscodi).OrderBy(x => x.BandaAdjudicada).ToList();
                var regUrsBase = regUrsBaseTemp.Count > 0 ? regUrsBaseTemp.First() : new SmaUrsModoOperacionDTO();

                var lstSubirTemp = arrOfertasSubir.Where(x => x.URS == urscodi).ToList();
                var lstBajarTemp = arrOfertasBajar.Where(x => x.URS == urscodi).ToList();

                //Valido para cada caso particular
                if (lstSubirTemp.Count > 0)
                {
                    //Existe data, tanto en subi y bajar para la urs
                    if (lstBajarTemp.Count > 0)
                    {
                        //Agrupar por periodo (30min)cada 
                        OfertaListItem[] lstSubirNivelURS = ObtenerListadoANivelUrs(lstSubirTemp, arrHoras);
                        OfertaListItem[] lstBajarNivelURS = ObtenerListadoANivelUrs(lstBajarTemp, arrHoras);

                        resultadoUrs = ValidarABporUrs(1, urscodi, lstSubirNivelURS, lstBajarNivelURS, regUrsCalif, regUrsBase, arrHoras, fecha, listaTodasURSBase);
                    }
                    //Existe data solo en tab subir para la urs
                    else
                    {
                        //Agrupar por periodo (30min)cada 
                        OfertaListItem[] lstSubirNivelURS = ObtenerListadoANivelUrs(lstSubirTemp, arrHoras);

                        resultadoUrs = ValidarABporUrs(2, urscodi, lstSubirNivelURS, new OfertaListItem[0], regUrsCalif, regUrsBase, arrHoras, fecha, listaTodasURSBase);
                    }
                }
                else
                {
                    //Existe data solo en tab bajar para la urs
                    if (lstBajarTemp.Count > 0)
                    {
                        //Agrupar por periodo (30min)cada 
                        OfertaListItem[] lstBajarNivelURS = ObtenerListadoANivelUrs(lstBajarTemp, arrHoras);

                        resultadoUrs = ValidarABporUrs(3, urscodi, new OfertaListItem[0], lstBajarNivelURS, regUrsCalif, regUrsBase, arrHoras, fecha, listaTodasURSBase);
                    }
                    //NUNCA ENTRA AQUI
                    else
                    {

                    }
                }
                salida = salida + (resultadoUrs != "" ? "\n" + resultadoUrs : "");
            }

            return salida;
        }

        /// <summary>
        /// Valida que la URS cumplan las condiciones de Banda
        /// </summary>
        /// <param name="caso"></param>
        /// <param name="ursCodi"></param>
        /// <param name="lstSubir30min"></param>
        /// <param name="lstBajar30min"></param>
        /// <param name="ursCalificada"></param>
        /// <param name="ursBase"></param>
        /// <param name="arrHoras"></param>
        /// <param name="fecha"></param>
        /// <param name="listaTodasURSBase"></param>
        /// <returns></returns>
        private string ValidarABporUrs(int caso, int ursCodi, OfertaListItem[] lstSubir30min, OfertaListItem[] lstBajar30min, SmaUrsModoOperacionDTO ursCalificada, SmaUrsModoOperacionDTO ursBase, string[] arrHoras, DateTime fecha, List<SmaUrsModoOperacionDTO> listaTodasURSBase)
        {
            string salida = "";
            string msgTemp = "";
            string iniAnterior = "";
            string finAnterior = "";
            string[] sal_ = new string[100];
            int indice = -1;

            decimal? bandaCalificada = ursCalificada.BandaURS;
            string nombreURS = ursCalificada.Ursnomb;
            decimal? PotenciaOfertadaSubir = null;
            decimal? PotenciaOfertadaBajar = null;
            decimal? bandaProvBase = ursBase.BandaAdjudicada;
            decimal? potWs = ursBase.PMax;
            decimal? potWb = ursBase.PMin;

            //IniFor
            for (int i = 0; i <= 47; i++)
            {
                //Cada 30Min
                string horarioMin = arrHoras[i];
                string horarioMax = arrHoras[i + 1];

                switch (caso)
                {
                    //Existe ofertas para la urs tanto en tab SUBIR como en tab BAJAR
                    case 1:

                        //obtener POs y POb cada 30min
                        if (ExisteRangoEnListado(horarioMin, horarioMax, lstSubir30min))
                        {
                            if (ExisteRangoEnListado(horarioMin, horarioMax, lstBajar30min))
                            {
                                OfertaListItem regSubir = lstSubir30min.First(x => x.HoraInicio == horarioMin && x.HoraFin == horarioMax);
                                OfertaListItem regBajar = lstBajar30min.First(x => x.HoraInicio == horarioMin && x.HoraFin == horarioMax);

                                PotenciaOfertadaSubir = regSubir.PotenciaOfertada;
                                PotenciaOfertadaBajar = regBajar.PotenciaOfertada;
                            }
                            else
                            {
                                OfertaListItem regSubir = lstSubir30min.First(x => x.HoraInicio == horarioMin && x.HoraFin == horarioMax);


                                PotenciaOfertadaSubir = regSubir.PotenciaOfertada;
                                PotenciaOfertadaBajar = 0;
                            }
                        }
                        else
                        {
                            if (ExisteRangoEnListado(horarioMin, horarioMax, lstBajar30min))
                            {
                                OfertaListItem regBajar = lstBajar30min.First(x => x.HoraInicio == horarioMin && x.HoraFin == horarioMax);

                                PotenciaOfertadaSubir = 0;
                                PotenciaOfertadaBajar = regBajar.PotenciaOfertada;
                            }
                            else
                            {
                                PotenciaOfertadaSubir = null;
                                PotenciaOfertadaBajar = null;
                            }
                        }
                        break;

                    //Existe ofertas para la urs solo en tab SUBIR 
                    case 2:
                        //obtener POs y POb cada 30min
                        if (ExisteRangoEnListado(horarioMin, horarioMax, lstSubir30min))
                        {
                            OfertaListItem regSubir = lstSubir30min.First(x => x.HoraInicio == horarioMin && x.HoraFin == horarioMax);

                            PotenciaOfertadaSubir = regSubir.PotenciaOfertada;
                            PotenciaOfertadaBajar = 0;
                        }
                        else
                        {
                            PotenciaOfertadaSubir = null;
                            PotenciaOfertadaBajar = null;

                        }
                        break;

                    //Existe ofertas para la urs solo en tab BAJAR 
                    case 3:
                        //obtener POs y POb cada 30min
                        if (ExisteRangoEnListado(horarioMin, horarioMax, lstBajar30min))
                        {
                            OfertaListItem regBajar = lstBajar30min.First(x => x.HoraInicio == horarioMin && x.HoraFin == horarioMax);

                            PotenciaOfertadaSubir = 0;
                            PotenciaOfertadaBajar = regBajar.PotenciaOfertada;
                        }
                        else
                        {
                            PotenciaOfertadaSubir = null;
                            PotenciaOfertadaBajar = null;

                        }
                        break;
                    default:
                        break;
                }

                //Validación por cada 30 minutos donde exista data
                if ((PotenciaOfertadaSubir != null) && (PotenciaOfertadaBajar != null))
                {
                    bool condicionTabSubir = (PotenciaOfertadaSubir + (bandaProvBase.GetValueOrDefault(0) / 2)) <= bandaCalificada;
                    bool condicionTabBajar = (PotenciaOfertadaBajar + (bandaProvBase.GetValueOrDefault(0) / 2)) <= bandaCalificada;

                    if (condicionTabSubir && condicionTabBajar) { }
                    else
                    {
                        //Agrupamos el mensaje de salida para periodos contiguos
                        if (horarioMin.Trim() == finAnterior.Trim())
                        {
                            msgTemp = "";
                            if (!condicionTabSubir) msgTemp += string.Format("<br /> [{1}-{2}] {0}, la POfertada[subir] debe ser menor o igual a {3}.", nombreURS, iniAnterior, horarioMax, (bandaCalificada - (bandaProvBase.GetValueOrDefault(0) / 2))) + "\n";
                            if (!condicionTabBajar) msgTemp += string.Format("<br /> [{1}-{2}] {0}, la POfertada[bajar] debe ser menor o igual a {3}.", nombreURS, iniAnterior, horarioMax, (bandaCalificada - (bandaProvBase.GetValueOrDefault(0) / 2))) + "\n";
                            sal_[indice] = msgTemp;
                        }
                        else
                        {
                            msgTemp = "";
                            if (!condicionTabSubir) msgTemp += string.Format("<br /> [{1}-{2}] {0}, la POfertada[subir] debe ser menor o igual a {3}.", nombreURS, horarioMin, horarioMax, (bandaCalificada - (bandaProvBase.GetValueOrDefault(0) / 2))) + "\n";
                            if (!condicionTabBajar) msgTemp += string.Format("<br /> [{1}-{2}] {0}, la POfertada[bajar] debe ser menor o igual a {3}.", nombreURS, horarioMin, horarioMax, (bandaCalificada - (bandaProvBase.GetValueOrDefault(0) / 2))) + "\n";

                            iniAnterior = horarioMin;
                            indice++;
                            sal_[indice] = msgTemp;
                        }
                        finAnterior = horarioMax;
                    }
                }

            }

            //FinFor
            var arraySalidaAgrupada = sal_.Where(x => x != null).ToArray();
            for (int i = 0; i < arraySalidaAgrupada.Length; i++)
            {
                salida = salida + arraySalidaAgrupada[i];
            }

            return salida;
        }

        /// <summary>
        /// Devuelve las ofertas A Nivel URS. Para los que tienes varios MO, solo tomocomo referencia el 1ro para los calculos
        /// </summary>
        /// <param name="lstSubirTemp"></param>
        /// <returns></returns>
        private OfertaListItem[] ObtenerListadoANivelUrs(List<OfertaListItem> lstOfertasMismaUrs, string[] arrHoras)
        {
            OfertaListItem[] lista = new OfertaListItem[50];

            //Obtenemos solo los primeros de cada URS-Periodo y lo ordenamos ascendentemene segun su horaIni
            var lst = lstOfertasMismaUrs.GroupBy(x => new { x.URS, x.HoraInicio, x.HoraFin })
                                        .Select(x => new OfertaListItem() { HoraInicio = x.Key.HoraInicio, HoraFin = x.Key.HoraFin, URS = x.Key.URS, ModoOperacion = x.First().ModoOperacion, PotenciaOfertada = x.First().PotenciaOfertada })
                                        .OrderBy(n => n.HoraInicio).ToList();

            int indice = 0;
            //se crea un listado por cada 30min de todo el periodo de la URS
            foreach (var registro in lst)
            {
                string horaInicio = registro.HoraInicio;
                string horaFin = registro.HoraFin;

                int indexIni = Array.IndexOf(arrHoras, horaInicio);
                int indexFin = Array.IndexOf(arrHoras, horaFin);

                for (int i = indexIni; i < indexFin; i++)
                {
                    OfertaListItem obj = new OfertaListItem();
                    obj.HoraInicio = arrHoras[i];
                    obj.HoraFin = arrHoras[i + 1];
                    obj.ModoOperacion = registro.ModoOperacion;
                    obj.URS = registro.URS;
                    obj.PotenciaOfertada = registro.PotenciaOfertada;

                    lista[indice] = obj;

                    indice++;
                }
            }

            lista = lista.Where(x => x != null).ToArray();

            return lista;
        }

        /// <summary>
        /// Analiza los cruces entre los periodos de las ofertas realizadas
        /// </summary>
        /// <param name="arrOfertas"></param>
        /// <param name="fecha"></param>
        /// <param name="nomHoja"></param>
        /// <returns></returns>
        private string GestionarTraslape(OfertaListItem[] arrOfertas, DateTime fecha, string nomHoja, string[] arrHoras)
        {
            string salida = "";

            List<int> lstUrsCodis = arrOfertas.Select(x => x.URS).Distinct().ToList();

            //para cada urs, obtenemos sus periodos y verificamos traslape
            foreach (int urscodi in lstUrsCodis)
            {
                List<RangosFechas> listaPeriodos = new List<RangosFechas>();
                int numRango = 0;

                //obtenemos data de las urs, obtenemos su nombre
                PrGrupoDTO listaUrs = appServicioSubastas.GetByIdPrGrupo(urscodi);

                var ofertasMismoUrs = ObtenerListadoANivelUrs(arrOfertas.Where(x => x.URS == urscodi).ToList(), arrHoras);
                string nombreUrs = "";

                //Obtenemos todos los periodos de una URS
                foreach (var ofertaX in ofertasMismoUrs)
                {
                    RangosFechas rangoTiempo = new RangosFechas();
                    List<DateTime> rango = new List<DateTime>();
                    int codRango = numRango;

                    //Agregamos solo los que representan un bloque, los que tienen varios MO solo es necesario guardar UNA SOLA VEZ su rango (NO por cada MO)
                    string horaIni = ofertaX.HoraInicio.Trim();
                    string horaFin = ofertaX.HoraFin.Trim();

                    var lsiHoraIni = horaIni.Split(':');
                    var lsiHoraFin = horaFin.Split(':');

                    var hoIni = Convert.ToInt32(lsiHoraIni[0]);
                    var miIni = Convert.ToInt32(lsiHoraIni[1]);

                    var hoFin = Convert.ToInt32(lsiHoraFin[0]);
                    var miFin = Convert.ToInt32(lsiHoraFin[1]);

                    DateTime fecInicial = new DateTime(fecha.Year, fecha.Month, fecha.Day, hoIni, miIni, 0);
                    DateTime fecFinal = new DateTime(fecha.Year, fecha.Month, fecha.Day, hoFin, miFin, 0);

                    rango.Add(fecInicial);
                    rango.Add(fecFinal);

                    rangoTiempo.Rango = rango;
                    rangoTiempo.CodigoRango = codRango;
                    rangoTiempo.NombreUrs = string.Format("{0}", ofertaX.URS);
                    rangoTiempo.Hoja = nomHoja;

                    nombreUrs = listaUrs.Gruponomb;

                    listaPeriodos.Add(rangoTiempo);
                    numRango++;
                }

                //Verificamos traslapes para un grupo de rangos
                string strSalida = VerificarExistenciaTraslapes(listaPeriodos);

                //Si existe traslape, mostrar un mensaje
                if (strSalida != "")
                {
                    salida = salida + string.Format("<br/> Existe traslape de horas en la 'hoja {0}' en la URS '{1}'  para el dia  {2} entre los rangos:  <ul>", nomHoja, nombreUrs, fecha.ToString(ConstantesAppServicio.FormatoFecha)) + strSalida + "</ul>";
                }


            }

            return salida;
        }

        /// <summary>
        /// Verifica si existe traslapes entre los rangos pasados como parametro
        /// </summary>
        /// <param name="listaPeriodos"></param>
        /// <returns></returns>
        private string VerificarExistenciaTraslapes(List<RangosFechas> listaPeriodos)
        {
            string resultado = "";

            //Guardara los codigoRango de los ya comparados, para evitar compararlos nuevamente
            List<string> listaYaComparados = new List<string>();

            foreach (var periodo1 in listaPeriodos)
            {
                int cod1 = periodo1.CodigoRango;

                //cada periodo lo comparamos con los otros
                foreach (var periodo2 in listaPeriodos)
                {
                    int cod2 = periodo2.CodigoRango;

                    if (cod1 != cod2)
                    {
                        string codigosComparados1 = string.Format("{0}-{1}", cod1, cod2);
                        string codigosComparados2 = string.Format("{0}-{1}", cod2, cod1); //comparar r1_r2 es igual a comparar r2_r1

                        //verifico si ya hice la comparacion de traslapes entre dos rangos (comparar: r1_r2 = r2_r1)
                        if (listaYaComparados.Contains(codigosComparados1) || listaYaComparados.Contains(codigosComparados2))
                        {
                        }
                        else
                        {
                            listaYaComparados.Add(codigosComparados1);
                            listaYaComparados.Add(codigosComparados2);

                            string msgInterseccion = VerificarInterseccion(periodo1, periodo2);
                            resultado = resultado + msgInterseccion;
                        }
                    }
                }
            }

            return resultado;
        }

        /// <summary>
        /// verifico traslape para dos periodos
        /// </summary>
        /// <param name="periodo1"></param>
        /// <param name="periodo2"></param>
        /// <returns></returns>
        private string VerificarInterseccion(RangosFechas periodo1, RangosFechas periodo2)
        {
            string msgesultado = "";

            List<DateTime> rango1 = periodo1.Rango;
            List<DateTime> rango2 = periodo2.Rango;

            DateTime fechaIni_rango1 = rango1[0];
            DateTime fechaFin_rango1 = rango1[1];

            DateTime fechaIni_rango2 = rango2[0];
            DateTime fechaFin_rango2 = rango2[1];

            bool hayInterseccion = false;

            var valor = fechaIni_rango1.CompareTo(fechaIni_rango2);

            //rango1 inicia primero
            if (valor < 0)
            {
                var valor2 = fechaFin_rango1.CompareTo(fechaIni_rango2);

                //rango1 termina antes que inice rango2 
                if (valor2 < 0)
                {
                }
                else
                {
                    //rango1 termina justo cuando inicia rango2
                    if (valor2 == 0)
                    {
                    }
                    //rango2 inicia cuando rango1 aun NO TERMINA
                    else
                    {
                        hayInterseccion = true;
                    }
                }
            }
            else
            {
                //rango1 inicia igual a rango2
                if (valor == 0)
                {
                    hayInterseccion = true;
                }
                //rango2 inicia primero
                else
                {
                    var valor3 = fechaFin_rango2.CompareTo(fechaIni_rango1);

                    //rango2 termina antes que inice rango1 
                    if (valor3 < 0)
                    {
                    }
                    else
                    {
                        //rango2 termina justo cuando inicia rango1
                        if (valor3 == 0)
                        {
                        }
                        //rango1 inicia cuando rango2 aun NO TERMINA
                        else
                        {
                            hayInterseccion = true;
                        }
                    }
                }
            }

            if (hayInterseccion)
            {
                msgesultado = string.Format("<li>    *  [{0} - {1}] y [{2} - {3}]. </li>", periodo1.Rango[0].ToString("HH:mm"), periodo1.Rango[1].ToString("HH:mm"), periodo2.Rango[0].ToString("HH:mm"), periodo2.Rango[1].ToString("HH:mm")); // periodo1.nombreUrs = periodo2.nombreUrs
            }

            return msgesultado;
        }

        #endregion

        /// <summary>
        /// Obtener código de usuario login
        /// </summary>
        /// <returns></returns>
        private int GetUserCodeSession()
        {
            UserDTO objUserDTO = Session[HelperCommon.DatosSesion.SesionUsuario] as UserDTO;

            if (ConfigurationManager.AppSettings[ConstantesSubasta.KeyAmbientePrueba].ToString() == "S")
            {
                string usercodePrueba = ConfigurationManager.AppSettings[ConstantesSubasta.KeyUsercodePrueba].ToString();
                objUserDTO.UserCode = Convert.ToInt16(usercodePrueba);
            }

            return objUserDTO.UserCode;
        }

        /// <summary>
        /// //Mostrar del manual usuario
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public virtual ActionResult MostrarManualSubastas()
        {
            if (Session[HelperCommon.DatosSesion.SesionUsuario] != null)
            {
                string fullPath = AppDomain.CurrentDomain.BaseDirectory + "Documentos/Manual_de_Usuario_Extranet_Subastas.pdf";
                return File(fullPath, Constantes2.AppPdf, "Manual_de_Usuario_Extranet_Subastas.pdf");
            }
            return null;
        }

        #region Métodos No utilizados

        ///// <summary>
        ///// Método usado para validar los Modos de Operación
        ///// </summary>
        ///// <param name="modosOperacion"></param>
        ///// <returns></returns>
        //public bool ValidarModosOperacion(string modosOperacion)
        //{
        //    bool blnResultado = false;

        //    int[] arrModosOperacion = Array.ConvertAll(modosOperacion.Split(','), s => int.Parse(s));

        //    List<SmaModoOperValDTO> arrSmaModoOperValDTO = this.appServicioSubastas.ListSmaModoOperVals(modosOperacion);

        //    List<int> arrGrupos = new List<int>();

        //    foreach (SmaModoOperValDTO objSmaOperValDTO in arrSmaModoOperValDTO)
        //    {
        //        if (!arrGrupos.Exists(p => p == objSmaOperValDTO.Mopvgrupoval.Value))
        //        {
        //            arrGrupos.Add(objSmaOperValDTO.Mopvgrupoval.Value);
        //        }
        //    }

        //    foreach (int intGrupo in arrGrupos)
        //    {
        //        List<SmaModoOperValDTO> arrSmaModoOperValDTOGrupo = arrSmaModoOperValDTO.FindAll(p => p.Mopvgrupoval.Value == intGrupo);

        //        if (arrSmaModoOperValDTOGrupo != null && arrModosOperacion.Length == arrSmaModoOperValDTOGrupo.Count)
        //        {
        //            bool blnEsValido = true;

        //            foreach (int intMO in arrModosOperacion)
        //            {
        //                if (!arrSmaModoOperValDTO.Exists(p => p.Grupocodi == intMO))
        //                {
        //                    blnEsValido = false;
        //                    break;
        //                }
        //            }

        //            blnResultado = blnEsValido;
        //            break;
        //        }
        //    }

        //    return blnResultado;
        //}

        #region Opción Cargar Excel Deshabilitado

        ///// <summary>
        ///// Método usado para cargar por Excel los datos
        ///// </summary>
        ///// <param name="collection"></param>
        ///// <returns></returns>
        //[HttpPost]
        //public JsonResult CargarExcelOferta(FormCollection collection)
        //{
        //    base.ValidarSesionUsuario();
        //    HttpPostedFileBase fileData = Request.Files[0];

        //    ExcelPackage pck = new ExcelPackage(fileData.InputStream);

        //    ExcelWorkbook workBook = pck.Workbook;

        //    string[][] arrGrilla = null;

        //    if (workBook != null)
        //    {
        //        ExcelWorksheet objHoja;

        //        if (workBook.Worksheets.Count > 0)
        //        {
        //            objHoja = workBook.Worksheets[1];

        //            if (objHoja != null)
        //            {
        //                UserDTO dtoUser = ((UserDTO)Session[COES.MVC.Extranet.Helper.DatosSesion.SesionUsuario]);
        //                DateTime dteActual = DateTime.Now;

        //                for (int i = 10, j = 14; i <= j; i++)
        //                {
        //                    object objTitle = objHoja.GetValue(i, 1);

        //                    if (objTitle == null || objTitle.ToString().Trim().Length == 0)
        //                    {
        //                        throw new FormatException("El archivo no existe el formato correcto.");
        //                    }

        //                    object objValue = objHoja.GetValue(i, 2);

        //                    if (objValue == null || objValue.ToString().Trim().Length == 0)
        //                    {
        //                        throw new ArgumentNullException(string.Format("El {0} no ha sido especifigado", objTitle.ToString().ToLower()));
        //                    }
        //                    else
        //                    {
        //                        string strValue = objValue.ToString().Trim().ToUpper();

        //                        switch (i)
        //                        {
        //                            case 10:
        //                                if (strValue != dtoUser.UserLogin.Trim().ToUpper())
        //                                {
        //                                    throw new ArgumentException("Usuario no válido");
        //                                }
        //                                break;
        //                            case 11:
        //                            ////habilitar cuando la variable de sesión  del usuario tenga configurado la empresa.
        //                            //if (strvalue != dtouser.empresanombre)
        //                            //{
        //                            //    throw new argumentexception("empresa no válido");
        //                            //}
        //                            //break;
        //                            case 12:
        //                                if (int.Parse(strValue) != dteActual.Year)
        //                                {
        //                                    throw new ArgumentException("Año no valido");
        //                                }
        //                                break;
        //                            case 13:
        //                                DateTimeFormatInfo dtinfo = new CultureInfo("es-PE", false).DateTimeFormat;
        //                                string g = dtinfo.GetMonthName(dteActual.Month);
        //                                if (strValue != dtinfo.GetMonthName(dteActual.Month).ToUpper())
        //                                {
        //                                    throw new ArgumentException("Mes no valido");
        //                                }
        //                                break;
        //                            case 14:
        //                                if (int.Parse(strValue) != dteActual.Day)
        //                                {
        //                                    throw new ArgumentException("Día no valido");
        //                                }
        //                                break;
        //                        }
        //                    }
        //                }

        //                int intFilas = objHoja.Dimension.End.Row - 16;

        //                if (intFilas > 0)
        //                {
        //                    arrGrilla = new string[intFilas][];

        //                    for (int i = 0, j = intFilas; i < j; i++)
        //                    {
        //                        arrGrilla[i] = new string[6];

        //                        for (int m = 0, n = arrGrilla[i].Length; m < n; m++)
        //                        {
        //                            object objValue = objHoja.GetValue(i + 17, m + 1);

        //                            if (objValue != null)
        //                            {
        //                                string strValue = objValue.ToString();

        //                                if ((m == 0 || m == 1))
        //                                {
        //                                    DateTime dteValue;

        //                                    if (DateTime.TryParse(strValue, out dteValue))
        //                                    {
        //                                        strValue = dteValue.ToString("HH:mm");
        //                                    }
        //                                }
        //                                else
        //                                {
        //                                }

        //                                arrGrilla[i][m] = strValue;
        //                            }
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //    }

        //    return Json(arrGrilla);
        //}

        #endregion

        ////MODIFICADO POR STS - 2018, NUEVO
        //private int ConvertStringHoraToIntMin(string hora)
        //{
        //    string[] sHora = hora.Split(':');
        //    int minHora = Convert.ToInt32(sHora[0]) * 60 + Convert.ToInt32(sHora[1]);

        //    return minHora;
        //}

        ///// <summary>
        ///// Se usa para habilitar fechas posteriores a la fecha de oferta actual
        ///// </summary>
        ///// <param name="collection"></param>
        ///// <returns></returns>
        //public JsonResult ValidarFechaPosterior(FormCollection collection)
        //{
        //    List<SmaParamProcesoDTO> list = this.appServicioSubastas.ListSmaParamProcesos();

        //    //MODIFICADO POR STS - 2018
        //    int maxDias = list[0].Papomaxdiasoferta;

        //    string fechaEscogida = Convert.ToString(collection["fechaesc"]);

        //    DateTime fecha;

        //    DateTime.TryParse(fechaEscogida, new CultureInfo("es-ES"), DateTimeStyles.None, out fecha);

        //    string fechaActual = DateTime.Now.AddDays(1).ToString(ConstantesAppServicio.FormatoFecha);
        //    DateTime sfechaActual;
        //    DateTime.TryParse(fechaActual, new CultureInfo("es-ES"), DateTimeStyles.None, out sfechaActual);

        //    string fechaFinal = DateTime.Now.AddDays(maxDias).ToString(ConstantesAppServicio.FormatoFecha);
        //    DateTime sfechaFinal;
        //    DateTime.TryParse(fechaFinal, new CultureInfo("es-ES"), DateTimeStyles.None, out sfechaFinal);

        //    //if (fechaEscogida != fechaActual) { return "true"; } else { return "false"; }

        //    string indFecha = (fecha == sfechaActual) ? "true" : "false";
        //    string validaFecha = (fecha >= sfechaActual) && (fecha <= sfechaFinal) ? "true" : "false";

        //    return Json(new
        //    {
        //        indFecha = indFecha,
        //        validaFecha = validaFecha
        //    });
        //}

        #endregion

        #region Mantenimientos

        /// <summary>
        /// Lista todos los mantenimientos por cada URS y Modo de Operacion
        /// </summary>
        /// <param name="fecha"></param>
        /// <returns></returns>
        public JsonResult OfertaMantenimimentoListar(string fecha)
        {
            base.ValidarSesionUsuario();

            string fechaFormato = fecha.Substring(0, 10);
            DateTime dteFecha = DateTime.ParseExact(fechaFormato, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);


            List<SmaUrsModoOperacionDTO> arrSmaUrsModoOperacionDTO = this.appServicioSubastas.GetURSMOMantto(this.GetUserCodeSession(), dteFecha);

            List<Models.SubastaModel.OfertaMantenimientoListItem> arrOfertaMantenimientos = new List<SubastaModel.OfertaMantenimientoListItem>();

            foreach (SmaUrsModoOperacionDTO objSmaUrsModoOperacionDTO in arrSmaUrsModoOperacionDTO)
            {
                arrOfertaMantenimientos.Add(new SubastaModel.OfertaMantenimientoListItem()
                {
                    Fecha = dteFecha.ToString(ConstantesAppServicio.FormatoFecha),
                    URS = objSmaUrsModoOperacionDTO.Ursnomb,
                    ModoOperacion = objSmaUrsModoOperacionDTO.Gruponomb,
                    CapacidadMaximaRSF = Convert.ToDecimal(String.Format("{0:0.#0}", objSmaUrsModoOperacionDTO.CapacidadMax.GetValueOrDefault(0))),
                    MantenimientoProgramado = (objSmaUrsModoOperacionDTO.ManttoProgramado.ToUpper() == "SI"),
                    IntervaloMantenimieto = objSmaUrsModoOperacionDTO.Intervalo
                });
            }

            return Json(arrOfertaMantenimientos);
        }

        /// <summary>
        /// Devuelve los datos para las coordenadas del Grafico, el cual se calculo en base al mantenimiento de cada Modo de Operacion
        /// </summary>
        /// <param name="fecha"></param>
        /// <returns></returns>
        public JsonResult OfertaMantenimimentoGrafico(string fecha)
        {
            base.ValidarSesionUsuario();

            string fechaFormato = fecha.Substring(0, 10);
            DateTime dteFecha = DateTime.ParseExact(fechaFormato, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);

            decimal capacidad = 0;
            decimal capacidadMaxTot = 0;
            int urscodi = 0;
            string[] listIntvMantGrupo = new string[48];
            for (int n = 0; n < 48; n++) listIntvMantGrupo[n] = "";

            List<SmaModoOperValDTO> listMOValxUrs;
            List<SmaModoOperValDTO> totMOValxUrs = new List<SmaModoOperValDTO>();
            List<SmaModoOperValDTO>[] totMOValxUrsxIntv = new List<SmaModoOperValDTO>[48];
            List<SmaUrsModoOperacionDTO> arrSmaUrsModoOperacionDTO = this.appServicioSubastas.GetURSMOMantto(this.GetUserCodeSession(), dteFecha);

            Decimal[] arrListCapacidad = new Decimal[48];

            //Obtener potencias maximas de modos de operacion validos / Logica x URS
            foreach (SmaUrsModoOperacionDTO objSmaUrsModoOperacionDTO in arrSmaUrsModoOperacionDTO)
            {
                if (objSmaUrsModoOperacionDTO.Urscodi != urscodi)
                {
                    listMOValxUrs = this.appServicioSubastas.GetListMOValxUrs(objSmaUrsModoOperacionDTO.Urscodi);
                    for (int j = 0; j < listMOValxUrs.Count; j++)
                    {
                        string[] listMO = listMOValxUrs[j].MopvListMOVal.Split(',');
                        for (int s = 0; s < listMO.Length; s++)
                            for (int m = 0; m < arrSmaUrsModoOperacionDTO.Count; m++)
                                if (arrSmaUrsModoOperacionDTO[m].Grupocodi == Convert.ToInt32(listMO[s])) { capacidad = capacidad + (decimal)arrSmaUrsModoOperacionDTO[m].CapacidadMax; break; }

                        listMOValxUrs[j].CapacidadMaxima = capacidad;
                        capacidad = 0;
                    }
                    listMOValxUrs = listMOValxUrs.OrderByDescending(CapacidadMaxima => CapacidadMaxima).ToList();

                    if (objSmaUrsModoOperacionDTO.ListIntervalos != null)
                    {
                        string[] listIntervalos = objSmaUrsModoOperacionDTO.ListIntervalos.Split(',');
                        for (int k = 0; k < listIntervalos.Count(); k++)
                        {
                            int intvFromList = Convert.ToInt32(listIntervalos[k]);
                            if (listIntvMantGrupo[intvFromList] == "")
                                listIntvMantGrupo[intvFromList] = objSmaUrsModoOperacionDTO.Grupocodi.ToString() + ",";
                            else
                                listIntvMantGrupo[intvFromList] = listIntvMantGrupo[intvFromList] + objSmaUrsModoOperacionDTO.Grupocodi + ",";
                        }
                    }

                    if (listMOValxUrs.Count > 0)
                        totMOValxUrs.AddRange(listMOValxUrs);
                }
                urscodi = objSmaUrsModoOperacionDTO.Urscodi;
            }
            //termino de obtener capacidades maximas x MO para URS Orden Descendente
            List<SmaModoOperValDTO> mallaTemp;
            //Definir un arreglo de 0 a 48 con el valor de maxima potencia
            for (int intv = 0; intv < 48; intv++)
            {
                totMOValxUrsxIntv[intv] = new List<SmaModoOperValDTO>();
                mallaTemp = new List<SmaModoOperValDTO>();
                mallaTemp.AddRange(totMOValxUrs.Select(x => x.Copy()));

                if (listIntvMantGrupo[intv].Length > 0)
                {
                    string[] listado = listIntvMantGrupo[intv].Split(',');
                    for (int i = 0; i < listado.Length; i++)
                    {
                        for (int j = 0; j < mallaTemp.Count; j++)
                            if (mallaTemp[j].MopvListMOVal.Split(',').Contains(listado[i]))
                            {
                                Log.Info("eliminando fila  [" + j + "] mallaTemp.Count = " + mallaTemp.Count);
                                mallaTemp.RemoveAt(j);
                                Log.Info("elimino fila  [" + j + "] mallaTemp.Count = " + mallaTemp.Count);
                                j--;
                            }

                    }
                }
                Log.Info("asignando intervalo [" + intv + "] mallaTemp = " + mallaTemp);

                totMOValxUrsxIntv[intv].AddRange(mallaTemp);
            }


            for (int intv = 0; intv < 48; intv++)
            {
                capacidadMaxTot = 0;
                urscodi = 0;
                Log.Info("totMOValxUrsxIntv[intv].Count = " + totMOValxUrsxIntv[intv].Count);
                for (int k = 0; k < totMOValxUrsxIntv[intv].Count; k++)
                {
                    Log.Info("totMOValxUrsxIntv[intv][k].Urscodi = " + totMOValxUrsxIntv[intv][k].Urscodi);
                    Log.Info("totMOValxUrsxIntv[intv][k].MopvListMOVal = " + totMOValxUrsxIntv[intv][k].MopvListMOVal);
                    Log.Info("totMOValxUrsxIntv[intv][k].CapacidadMaxima = " + totMOValxUrsxIntv[intv][k].CapacidadMaxima);

                    if (totMOValxUrsxIntv[intv][k].Urscodi != urscodi)
                    {
                        capacidadMaxTot = capacidadMaxTot + totMOValxUrsxIntv[intv][k].CapacidadMaxima;
                        Log.Info("Incluyo para capacidad total del URS capacidadMaxTot = " + capacidadMaxTot);
                    }


                    urscodi = totMOValxUrsxIntv[intv][k].Urscodi;
                }


                arrListCapacidad[intv] = Math.Round(capacidadMaxTot, 2);

            }

            return Json(arrListCapacidad);
        }

        #endregion
    }
}
