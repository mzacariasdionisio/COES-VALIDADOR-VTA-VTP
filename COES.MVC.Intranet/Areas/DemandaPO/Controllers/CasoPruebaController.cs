using System;
using System.Collections.Generic;
using COES.MVC.Intranet.Areas.DemandaPO.Models;
using COES.Servicios.Aplicacion.Helper;
using COES.Servicios.Aplicacion.DPODemanda;
using COES.Servicios.Aplicacion.DPODemanda.Helper;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Globalization;
using COES.Dominio.DTO.Sic;
using log4net;
using COES.Servicios.Aplicacion.Scada;
using COES.MVC.Intranet.Helper;
using COES.Servicios.Aplicacion.PronosticoDemanda.Helper;

namespace COES.MVC.Intranet.Areas.DemandaPO.Controllers
{
    public class CasoPruebaController : Controller
    {
        /// <summary>
        /// Instancias de objeto para acceso a datos a los servicios de aplicación
        /// </summary>
        DemandaPOAppServicio service = new DemandaPOAppServicio();
        PerfilScadaServicio servicioSCADA = new PerfilScadaServicio();

        private static readonly ILog log = log4net.LogManager.GetLogger(typeof(CasoPruebaController));
        private static string NameController = "CasoPruebaController";


        public ActionResult Index()
        {
            CasoModel model = new CasoModel();

            model.Fecha = DateTime.Today.Year.ToString();
            model.FechaMes = DateTime.Now.ToString(ConstantesDpo.FormatoMesAnio);
            model.FechaDia = DateTime.Now.ToString(ConstantesDpo.FormatoFecha);

            model.ListaFeriados = service.GetByAnioDpoFeriados(Int32.Parse(model.Fecha));

            model.ListaAreaOperativa = this.service.ObtenerListaAreaOperativa();
            model.ListaFormulas = this.servicioSCADA.GetByCriteriaMePerfilRules(1, "-1");

            model.ListaNombreCasos = service.ListDpoNombreCasos();
            model.ListaUsuarios = service.ListDpoUsuarios();

            return View(model);
        }

        /// <summary>
        /// Lista Formulas
        /// </summary>
        /// <returns>JsonResult</returns>
        public JsonResult ListarFormulas(string areaOperativa)
        {
            CasoModel model = new CasoModel();

            if (areaOperativa == "")
            {
                areaOperativa = "-1";
            }

            try
            {
                model.ListaFormulas = this.servicioSCADA.GetByCriteriaMePerfilRules(1, areaOperativa);
                model.Mensaje = "Todo correcto";
                model.Resultado = "1";

                return Json(model);
            }
            catch (Exception ex)
            {
                model.Resultado = "-1";
                model.Mensaje = ex.Message;
                model.Detalle = ex.StackTrace;
                log.Error(NameController, ex);

                return Json(model);
            }
        }


        /// <summary>
        /// Lista los casos de prueba
        /// </summary>
        /// <returns>JsonResult</returns>
        public JsonResult ListarCasos()
        {
            CasoModel model = new CasoModel();

            try
            {
                model.ListaCasos = service.ListDpoCaso();
                model.Mensaje = "Todo correcto";
                model.Resultado = "1";

                return Json(model);
            }
            catch (Exception ex)
            {
                model.Resultado = "-1";
                model.Mensaje = ex.Message;
                model.Detalle = ex.StackTrace;
                log.Error(NameController, ex);

                return Json(model);

            }
        }

        /// <summary>
        /// Filtra los casos de prueba
        /// </summary>
        /// <param name="nombre"></param>
        /// <param name="areaOperativa"></param>
        /// <param name="usuario"></param>
        /// <returns>model.ListaCasos</returns>
        public JsonResult FiltrarCasos(string nombre, string areaOperativa, string usuario)
        {
            CasoModel model = new CasoModel();

            try
            {
                model.ListaCasos = service.FilterDpoCaso(nombre, areaOperativa, usuario);
                model.Mensaje = "Todo correcto";
                model.Resultado = "1";

                return Json(model);
            }
            catch (Exception ex)
            {
                model.Resultado = "-1";
                model.Mensaje = ex.Message;
                model.Detalle = ex.StackTrace;
                log.Error(NameController, ex);

                return Json(model);

            }
        }

        /// <summary>
        /// Obtiene las funciones del caso de configuracion
        /// </summary>
        /// <param name="idCaso">Identificador del caso de configuracion</param>
        /// <returns></returns>
        public JsonResult ListarFunciones(int idCaso)
        {
            object res = service.ListFunciones(idCaso);

            return Json(res);
        }

        /// <summary>
        /// Obtiene los parametros de las funciones r1 del caso de configuracion
        /// </summary>
        /// <param name="idCaso">Identificador del caso de configuracion</param>
        /// <param name="idDetalleCaso">Identificador del id detalle de caso de configuracion</param>
        /// <returns></returns>
        public JsonResult ListarParametrosR1(int idCaso, int idDetalleCaso)
        {
            object res = service.ListParametrosR1(idCaso, idDetalleCaso);

            return Json(res);
        }

        /// <summary>
        /// Obtiene los parametros de las funciones r2 del caso de configuracion
        /// </summary>
        /// <param name="idCaso">Identificador del caso de configuracion</param>
        /// <param name="idDetalleCaso">Identificador del id detalle de caso de configuracion</param>
        /// <returns></returns>
        public JsonResult ListarParametrosR2(int idCaso, int idDetalleCaso)
        {
            object res = service.ListParametrosR2(idCaso, idDetalleCaso);

            return Json(res);
        }

        /// <summary>
        /// Obtiene los parametros de las funciones f1 del caso de configuracion
        /// </summary>
        /// <param name="idCaso">Identificador del caso de configuracion</param>
        /// <param name="idDetalleCaso">Identificador del id detalle de caso de configuracion</param>
        /// <returns></returns>
        public JsonResult ListarParametrosF1(int idCaso, int idDetalleCaso)
        {
            object res = service.ListParametrosF1(idCaso, idDetalleCaso);

            return Json(res);
        }

        /// <summary>
        /// Obtiene los parametros de las funciones f2 del caso de configuracion
        /// </summary>
        /// <param name="idCaso">Identificador del caso de configuracion</param>
        /// <param name="idDetalleCaso">Identificador del id detalle de caso de configuracion</param>
        /// <returns></returns>
        public JsonResult ListarParametrosF2(int idCaso, int idDetalleCaso)
        {
            object res = service.ListParametrosF2(idCaso, idDetalleCaso);

            return Json(res);
        }

        /// <summary>
        /// Obtiene los parametros de las funciones a1 del caso de configuracion
        /// </summary>
        /// <param name="idCaso">Identificador del caso de configuracion</param>
        /// <param name="idDetalleCaso">Identificador del id detalle de caso de configuracion</param>
        /// <returns></returns>
        public JsonResult ListarParametrosA1(int idCaso, int idDetalleCaso)
        {
            object res = service.ListParametrosA1(idCaso, idDetalleCaso);

            return Json(res);
        }

        /// <summary>
        /// Obtiene los parametros de las funciones a2 del caso de configuracion
        /// </summary>
        /// <param name="idCaso">Identificador del caso de configuracion</param>
        /// <param name="idDetalleCaso">Identificador del id detalle de caso de configuracion</param>
        /// <returns></returns>
        public JsonResult ListarParametrosA2(int idCaso, int idDetalleCaso)
        {
            object res = service.ListParametrosA2(idCaso, idDetalleCaso);

            return Json(res);
        }

        /// <summary>
        /// Agrega casos de prueba
        /// </summary>
        /// <param name="nombre"></param>
        /// <param name="areaOperativa"></param>
        /// <returns>JsonResult</returns>
        public JsonResult AgregarCaso(string nombre,
                                      string areaOperativa,
                                      string formScadaDatMae,
                                      string fecIniDatMae,
                                      string fecFinDatMae,
                                      List<DpoFuncionDataMaestraDTO> listFuncionesDataMaestra,
                                      string formScadaDatPro,
                                      string fecIniDatPro,
                                      string fecFinDatPro,
                                      List<DpoFuncionDataProcesarDTO> listFuncionesDataProcesar,

                                      List<DpoDiasTipicosR1DTO> listDiasTipicosR1,
                                      string fecIniR1,
                                      string fecFinR1,
                                      List<DpoDiasTipicosR2DTO> listDiasTipicosR2,
                                      string numToleranciaRampaF1,
                                      string numFactorKF2,
                                      string anioFeriadoA1,
                                      string idFeriadoA1,
                                      List<DpoDiasTipicosA1DTO> listDiasTipicosA1,
                                      string anioFeriadoA2,
                                      string idFeriadoA2,
                                      List<DpoDiasTipicosA2DTO> listDiasTipicosA2,

                                      List<DpoDiasTipicosR1DTO> listDiasTipicosDpR1,
                                      string fecIniDpR1,
                                      string fecFinDpR1,
                                      List<DpoDiasTipicosR2DTO> listDiasTipicosDpR2,
                                      string numToleranciaRampaDpF1,
                                      string numFactorKDpF2,
                                      string anioFeriadoDpA1,
                                      string idFeriadoDpA1,
                                      List<DpoDiasTipicosA1DTO> listDiasTipicosDpA1,
                                      string anioFeriadoDpA2,
                                      string idFeriadoDpA2,
                                      List<DpoDiasTipicosA2DTO> listDiasTipicosDpA2
                                      )
        {
            CasoModel model = new CasoModel();

            try
            {

                #region Armar el objeto DpoCasoDTO
                DpoCasoDTO dpoCasoDTO = new DpoCasoDTO
                {
                    Dpocsocnombre = nombre.TrimEnd(),
                    Areaabrev = areaOperativa.TrimEnd(),
                    Dpocsousucreacion = User.Identity.Name,
                    Dpocsofeccreacion = DateTime.Now
                };
                #endregion

                #region Armar el objeto DpoCasoDetalleDTO
                List<DpoCasoDetalleDTO> listCasoDetalleDTO = new List<DpoCasoDetalleDTO>();
                foreach (DpoFuncionDataMaestraDTO entityFuncionesDataMaestra in listFuncionesDataMaestra)
                {
                    DpoCasoDetalleDTO dpoCasoDetalleDTO = new DpoCasoDetalleDTO()
                    {
                        Dpocasdetcodi = 0,
                        Dpocsocodi = 0,

                        Dpodetmafscada = Int32.Parse(formScadaDatMae),
                        Dpodetmatinicio = DateTime.ParseExact(fecIniDatMae, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture),
                        Dpodetmatfin = DateTime.ParseExact(fecFinDatMae, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture),
                        Dpofnccodima = entityFuncionesDataMaestra.Dpofnccodima,

                        Dposecuencma = entityFuncionesDataMaestra.Dposecuencma != null ? entityFuncionesDataMaestra.Dposecuencma : "-",

                        Dpotipfuncion = "DM",

                        Pafunr1dtg1 = listDiasTipicosR1[0].Pafunr1dtg1 != null ? listDiasTipicosR1[0].Pafunr1dtg1 : "-",
                        Pafunr1dtg2 = listDiasTipicosR1[0].Pafunr1dtg2 != null ? listDiasTipicosR1[0].Pafunr1dtg2 : "-",
                        Pafunr1dtg3 = listDiasTipicosR1[0].Pafunr1dtg3 != null ? listDiasTipicosR1[0].Pafunr1dtg3 : "-",
                        Pafunr1dtg4 = listDiasTipicosR1[0].Pafunr1dtg4 != null ? listDiasTipicosR1[0].Pafunr1dtg4 : "-",
                        Pafunr1dtg5 = listDiasTipicosR1[0].Pafunr1dtg5 != null ? listDiasTipicosR1[0].Pafunr1dtg5 : "-",
                        Pafunr1dtg6 = listDiasTipicosR1[0].Pafunr1dtg6 != null ? listDiasTipicosR1[0].Pafunr1dtg6 : "-",
                        Pafunr1dtg7 = listDiasTipicosR1[0].Pafunr1dtg7 != null ? listDiasTipicosR1[0].Pafunr1dtg7 : "-",

                        Pafunr1deg7 = fecIniR1,
                        Pafunr1hag7 = fecFinR1,

                        Pafunr2dtg1 = "-",
                        Pafunr2dtg2 = "-",
                        Pafunr2dtg3 = "-",
                        Pafunr2dtg4 = "-",
                        Pafunr2dtg5 = "-",
                        Pafunr2dtg6 = "-",
                        Pafunr2dtg7 = "-",

                        Pafunf1toram = numToleranciaRampaF1,
                        Pafunf2factk = numFactorKF2,

                        Pafuna1aniof = anioFeriadoA1,
                        Pafuna1idfer = idFeriadoA1,
                        Pafuna1dtg1 = listDiasTipicosA1[0].Pafuna1dtg1 != null ? listDiasTipicosA1[0].Pafuna1dtg1 : "-",
                        Pafuna1dtg2 = listDiasTipicosA1[0].Pafuna1dtg2 != null ? listDiasTipicosA1[0].Pafuna1dtg2 : "-",
                        Pafuna1dtg3 = listDiasTipicosA1[0].Pafuna1dtg3 != null ? listDiasTipicosA1[0].Pafuna1dtg3 : "-",
                        Pafuna1dtg4 = listDiasTipicosA1[0].Pafuna1dtg4 != null ? listDiasTipicosA1[0].Pafuna1dtg4 : "-",
                        Pafuna1dtg5 = listDiasTipicosA1[0].Pafuna1dtg5 != null ? listDiasTipicosA1[0].Pafuna1dtg5 : "-",
                        Pafuna1dtg6 = listDiasTipicosA1[0].Pafuna1dtg6 != null ? listDiasTipicosA1[0].Pafuna1dtg6 : "-",
                        Pafuna1dtg7 = listDiasTipicosA1[0].Pafuna1dtg7 != null ? listDiasTipicosA1[0].Pafuna1dtg7 : "-",

                        Pafuna2aniof = anioFeriadoA2,
                        Pafuna2idfer = idFeriadoA2,
                        Pafuna2dtg1 = listDiasTipicosA2[0].Pafuna2dtg1 != null ? listDiasTipicosA2[0].Pafuna2dtg1 : "-",
                        Pafuna2dtg2 = listDiasTipicosA2[0].Pafuna2dtg2 != null ? listDiasTipicosA2[0].Pafuna2dtg2 : "-",
                        Pafuna2dtg3 = listDiasTipicosA2[0].Pafuna2dtg3 != null ? listDiasTipicosA2[0].Pafuna2dtg3 : "-",
                        Pafuna2dtg4 = listDiasTipicosA2[0].Pafuna2dtg4 != null ? listDiasTipicosA2[0].Pafuna2dtg4 : "-",
                        Pafuna2dtg5 = listDiasTipicosA2[0].Pafuna2dtg5 != null ? listDiasTipicosA2[0].Pafuna2dtg5 : "-",
                        Pafuna2dtg6 = listDiasTipicosA2[0].Pafuna2dtg6 != null ? listDiasTipicosA2[0].Pafuna2dtg6 : "-",
                        Pafuna2dtg7 = listDiasTipicosA2[0].Pafuna2dtg7 != null ? listDiasTipicosA2[0].Pafuna2dtg7 : "-"
                    };

                    listCasoDetalleDTO.Add(dpoCasoDetalleDTO);
                }


                foreach (DpoFuncionDataProcesarDTO entityFuncionesDataProcesar in listFuncionesDataProcesar)
                {
                    DpoCasoDetalleDTO dpoCasoDetalleDTO = new DpoCasoDetalleDTO()
                    {
                        Dpocasdetcodi = 0,
                        Dpocsocodi = 0,

                        Dpodetmafscada = Int32.Parse(formScadaDatPro),
                        Dpodetmatinicio = DateTime.ParseExact(fecIniDatPro, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture),
                        Dpodetmatfin = DateTime.ParseExact(fecFinDatPro, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture),
                        Dpofnccodima = entityFuncionesDataProcesar.Dpofnccodipr,

                        Dposecuencma = entityFuncionesDataProcesar.Dposecuencpr != null ? entityFuncionesDataProcesar.Dposecuencpr : "-",

                        Dpotipfuncion = "PR",

                        Pafunr1dtg1 = listDiasTipicosDpR1[0].Pafunr1dtg1 != null ? listDiasTipicosDpR1[0].Pafunr1dtg1 : "-",
                        Pafunr1dtg2 = listDiasTipicosDpR1[0].Pafunr1dtg2 != null ? listDiasTipicosDpR1[0].Pafunr1dtg2 : "-",
                        Pafunr1dtg3 = listDiasTipicosDpR1[0].Pafunr1dtg3 != null ? listDiasTipicosDpR1[0].Pafunr1dtg3 : "-",
                        Pafunr1dtg4 = listDiasTipicosDpR1[0].Pafunr1dtg4 != null ? listDiasTipicosDpR1[0].Pafunr1dtg4 : "-",
                        Pafunr1dtg5 = listDiasTipicosDpR1[0].Pafunr1dtg5 != null ? listDiasTipicosDpR1[0].Pafunr1dtg5 : "-",
                        Pafunr1dtg6 = listDiasTipicosDpR1[0].Pafunr1dtg6 != null ? listDiasTipicosDpR1[0].Pafunr1dtg6 : "-",
                        Pafunr1dtg7 = listDiasTipicosDpR1[0].Pafunr1dtg7 != null ? listDiasTipicosDpR1[0].Pafunr1dtg7 : "-",

                        Pafunr1deg7 = fecIniDpR1,
                        Pafunr1hag7 = fecFinDpR1,

                        Pafunr2dtg1 = listDiasTipicosDpR2[0].Pafunr2dtg1 != null ? listDiasTipicosDpR2[0].Pafunr2dtg1 : "-",
                        Pafunr2dtg2 = listDiasTipicosDpR2[0].Pafunr2dtg2 != null ? listDiasTipicosDpR2[0].Pafunr2dtg2 : "-",
                        Pafunr2dtg3 = listDiasTipicosDpR2[0].Pafunr2dtg3 != null ? listDiasTipicosDpR2[0].Pafunr2dtg3 : "-",
                        Pafunr2dtg4 = listDiasTipicosDpR2[0].Pafunr2dtg4 != null ? listDiasTipicosDpR2[0].Pafunr2dtg4 : "-",
                        Pafunr2dtg5 = listDiasTipicosDpR2[0].Pafunr2dtg5 != null ? listDiasTipicosDpR2[0].Pafunr2dtg5 : "-",
                        Pafunr2dtg6 = listDiasTipicosDpR2[0].Pafunr2dtg6 != null ? listDiasTipicosDpR2[0].Pafunr2dtg6 : "-",
                        Pafunr2dtg7 = listDiasTipicosDpR2[0].Pafunr2dtg7 != null ? listDiasTipicosDpR2[0].Pafunr2dtg7 : "-",

                        Pafunf1toram = numToleranciaRampaDpF1,
                        Pafunf2factk = numFactorKDpF2,

                        Pafuna1aniof = anioFeriadoDpA1,
                        Pafuna1idfer = idFeriadoDpA1,
                        Pafuna1dtg1 = listDiasTipicosDpA1[0].Pafuna1dtg1 != null ? listDiasTipicosDpA1[0].Pafuna1dtg1 : "-",
                        Pafuna1dtg2 = listDiasTipicosDpA1[0].Pafuna1dtg2 != null ? listDiasTipicosDpA1[0].Pafuna1dtg2 : "-",
                        Pafuna1dtg3 = listDiasTipicosDpA1[0].Pafuna1dtg3 != null ? listDiasTipicosDpA1[0].Pafuna1dtg3 : "-",
                        Pafuna1dtg4 = listDiasTipicosDpA1[0].Pafuna1dtg4 != null ? listDiasTipicosDpA1[0].Pafuna1dtg4 : "-",
                        Pafuna1dtg5 = listDiasTipicosDpA1[0].Pafuna1dtg5 != null ? listDiasTipicosDpA1[0].Pafuna1dtg5 : "-",
                        Pafuna1dtg6 = listDiasTipicosDpA1[0].Pafuna1dtg6 != null ? listDiasTipicosDpA1[0].Pafuna1dtg6 : "-",
                        Pafuna1dtg7 = listDiasTipicosDpA1[0].Pafuna1dtg7 != null ? listDiasTipicosDpA1[0].Pafuna1dtg7 : "-",

                        Pafuna2aniof = anioFeriadoDpA2,
                        Pafuna2idfer = idFeriadoDpA2,
                        Pafuna2dtg1 = listDiasTipicosDpA2[0].Pafuna2dtg1 != null ? listDiasTipicosDpA2[0].Pafuna2dtg1 : "-",
                        Pafuna2dtg2 = listDiasTipicosDpA2[0].Pafuna2dtg2 != null ? listDiasTipicosDpA2[0].Pafuna2dtg2 : "-",
                        Pafuna2dtg3 = listDiasTipicosDpA2[0].Pafuna2dtg3 != null ? listDiasTipicosDpA2[0].Pafuna2dtg3 : "-",
                        Pafuna2dtg4 = listDiasTipicosDpA2[0].Pafuna2dtg4 != null ? listDiasTipicosDpA2[0].Pafuna2dtg4 : "-",
                        Pafuna2dtg5 = listDiasTipicosDpA2[0].Pafuna2dtg5 != null ? listDiasTipicosDpA2[0].Pafuna2dtg5 : "-",
                        Pafuna2dtg6 = listDiasTipicosDpA2[0].Pafuna2dtg6 != null ? listDiasTipicosDpA2[0].Pafuna2dtg6 : "-",
                        Pafuna2dtg7 = listDiasTipicosDpA2[0].Pafuna2dtg7 != null ? listDiasTipicosDpA2[0].Pafuna2dtg7 : "-"
                    };

                    listCasoDetalleDTO.Add(dpoCasoDetalleDTO);
                }
                #endregion

                model.idCaso = service.SaveDpoCasoConfiguracion(dpoCasoDTO, listCasoDetalleDTO);

                model.Mensaje = "El caso fue agregado correctamente";
                model.ListaCasos = service.ListDpoCaso();
                model.Resultado = "1";
                

                return Json(model);
            }
            catch (Exception ex)
            {
                model.Resultado = "-1";
                model.Mensaje = ex.Message;
                model.Detalle = ex.StackTrace;
                log.Error(NameController, ex);

                return Json(model);
            }
        }

        /// <summary>
        /// Edita casos de prueba
        /// </summary>
        /// <param name="id"></param>
        /// <param name="nombre"></param>
        /// <param name="areaOperativa"></param>
        /// <returns>JsonResult</returns>
        public JsonResult EditarCaso(int id,
                                      string nombre,
                                      string areaOperativa,
                                      string formScadaDatMae,
                                      string fecIniDatMae,
                                      string fecFinDatMae,
                                      List<DpoFuncionDataMaestraDTO> listFuncionesDataMaestra,
                                      string formScadaDatPro,
                                      string fecIniDatPro,
                                      string fecFinDatPro,
                                      List<DpoFuncionDataProcesarDTO> listFuncionesDataProcesar,

                                      List<DpoDiasTipicosR1DTO> listDiasTipicosR1,
                                      string fecIniR1,
                                      string fecFinR1,
                                      List<DpoDiasTipicosR2DTO> listDiasTipicosR2,
                                      string numToleranciaRampaF1,
                                      string numFactorKF2,
                                      string anioFeriadoA1,
                                      string idFeriadoA1,
                                      List<DpoDiasTipicosA1DTO> listDiasTipicosA1,
                                      string anioFeriadoA2,
                                      string idFeriadoA2,
                                      List<DpoDiasTipicosA2DTO> listDiasTipicosA2,

                                      List<DpoDiasTipicosR1DTO> listDiasTipicosDpR1,
                                      string fecIniDpR1,
                                      string fecFinDpR1,
                                      List<DpoDiasTipicosR2DTO> listDiasTipicosDpR2,
                                      string numToleranciaRampaDpF1,
                                      string numFactorKDpF2,
                                      string anioFeriadoDpA1,
                                      string idFeriadoDpA1,
                                      List<DpoDiasTipicosA1DTO> listDiasTipicosDpA1,
                                      string anioFeriadoDpA2,
                                      string idFeriadoDpA2,
                                      List<DpoDiasTipicosA2DTO> listDiasTipicosDpA2
                                    )
        {
            CasoModel model = new CasoModel();

            try
            {

                #region Armar el objeto DpoCasoDTO
                DpoCasoDTO dpoCasoDTO = new DpoCasoDTO
                {
                    Dpocsocodi = id,
                    Dpocsocnombre = nombre.TrimEnd(),
                    Areaabrev = areaOperativa.TrimEnd(),
                    Dpocsousumodificacion = User.Identity.Name,
                    Dposcofecmodificacion = DateTime.Now
                };
                #endregion

                #region Armar el objeto DpoCasoDetalleDTO
                List<DpoCasoDetalleDTO> listCasoDetalleDTO = new List<DpoCasoDetalleDTO>();
                foreach (DpoFuncionDataMaestraDTO entityFuncionesDataMaestra in listFuncionesDataMaestra)
                {
                    DpoCasoDetalleDTO dpoCasoDetalleDTO = new DpoCasoDetalleDTO()
                    {
                        Dpocasdetcodi = 0,
                        Dpocsocodi = 0,

                        Dpodetmafscada = Int32.Parse(formScadaDatMae),
                        Dpodetmatinicio = DateTime.ParseExact(fecIniDatMae, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture),
                        Dpodetmatfin = DateTime.ParseExact(fecFinDatMae, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture),
                        Dpofnccodima = entityFuncionesDataMaestra.Dpofnccodima,
                        Dposecuencma = entityFuncionesDataMaestra.Dposecuencma != null ? entityFuncionesDataMaestra.Dposecuencma : "-",

                        Dpotipfuncion = "DM",

                        Pafunr1dtg1 = listDiasTipicosR1[0].Pafunr1dtg1 != null ? listDiasTipicosR1[0].Pafunr1dtg1 : "-",
                        Pafunr1dtg2 = listDiasTipicosR1[0].Pafunr1dtg2 != null ? listDiasTipicosR1[0].Pafunr1dtg2 : "-",
                        Pafunr1dtg3 = listDiasTipicosR1[0].Pafunr1dtg3 != null ? listDiasTipicosR1[0].Pafunr1dtg3 : "-",
                        Pafunr1dtg4 = listDiasTipicosR1[0].Pafunr1dtg4 != null ? listDiasTipicosR1[0].Pafunr1dtg4 : "-",
                        Pafunr1dtg5 = listDiasTipicosR1[0].Pafunr1dtg5 != null ? listDiasTipicosR1[0].Pafunr1dtg5 : "-",
                        Pafunr1dtg6 = listDiasTipicosR1[0].Pafunr1dtg6 != null ? listDiasTipicosR1[0].Pafunr1dtg6 : "-",
                        Pafunr1dtg7 = listDiasTipicosR1[0].Pafunr1dtg7 != null ? listDiasTipicosR1[0].Pafunr1dtg7 : "-",

                        Pafunr1deg7 = fecIniR1,
                        Pafunr1hag7 = fecFinR1,

                        Pafunr2dtg1 = "-",
                        Pafunr2dtg2 = "-",
                        Pafunr2dtg3 = "-",
                        Pafunr2dtg4 = "-",
                        Pafunr2dtg5 = "-",
                        Pafunr2dtg6 = "-",
                        Pafunr2dtg7 = "-",

                        Pafunf1toram = numToleranciaRampaF1,
                        Pafunf2factk = numFactorKF2,

                        Pafuna1aniof = anioFeriadoA1,
                        Pafuna1idfer = idFeriadoA1,
                        Pafuna1dtg1 = listDiasTipicosA1[0].Pafuna1dtg1 != null ? listDiasTipicosA1[0].Pafuna1dtg1 : "-",
                        Pafuna1dtg2 = listDiasTipicosA1[0].Pafuna1dtg2 != null ? listDiasTipicosA1[0].Pafuna1dtg2 : "-",
                        Pafuna1dtg3 = listDiasTipicosA1[0].Pafuna1dtg3 != null ? listDiasTipicosA1[0].Pafuna1dtg3 : "-",
                        Pafuna1dtg4 = listDiasTipicosA1[0].Pafuna1dtg4 != null ? listDiasTipicosA1[0].Pafuna1dtg4 : "-",
                        Pafuna1dtg5 = listDiasTipicosA1[0].Pafuna1dtg5 != null ? listDiasTipicosA1[0].Pafuna1dtg5 : "-",
                        Pafuna1dtg6 = listDiasTipicosA1[0].Pafuna1dtg6 != null ? listDiasTipicosA1[0].Pafuna1dtg6 : "-",
                        Pafuna1dtg7 = listDiasTipicosA1[0].Pafuna1dtg7 != null ? listDiasTipicosA1[0].Pafuna1dtg7 : "-",

                        Pafuna2aniof = anioFeriadoA2,
                        Pafuna2idfer = idFeriadoA2,
                        Pafuna2dtg1 = listDiasTipicosA2[0].Pafuna2dtg1 != null ? listDiasTipicosA2[0].Pafuna2dtg1 : "-",
                        Pafuna2dtg2 = listDiasTipicosA2[0].Pafuna2dtg2 != null ? listDiasTipicosA2[0].Pafuna2dtg2 : "-",
                        Pafuna2dtg3 = listDiasTipicosA2[0].Pafuna2dtg3 != null ? listDiasTipicosA2[0].Pafuna2dtg3 : "-",
                        Pafuna2dtg4 = listDiasTipicosA2[0].Pafuna2dtg4 != null ? listDiasTipicosA2[0].Pafuna2dtg4 : "-",
                        Pafuna2dtg5 = listDiasTipicosA2[0].Pafuna2dtg5 != null ? listDiasTipicosA2[0].Pafuna2dtg5 : "-",
                        Pafuna2dtg6 = listDiasTipicosA2[0].Pafuna2dtg6 != null ? listDiasTipicosA2[0].Pafuna2dtg6 : "-",
                        Pafuna2dtg7 = listDiasTipicosA2[0].Pafuna2dtg7 != null ? listDiasTipicosA2[0].Pafuna2dtg7 : "-"
                    };

                    listCasoDetalleDTO.Add(dpoCasoDetalleDTO);
                }


                foreach (DpoFuncionDataProcesarDTO entityFuncionesDataProcesar in listFuncionesDataProcesar)
                {
                    DpoCasoDetalleDTO dpoCasoDetalleDTO = new DpoCasoDetalleDTO()
                    {
                        Dpocasdetcodi = 0,
                        Dpocsocodi = 0,

                        Dpodetmafscada = Int32.Parse(formScadaDatPro),
                        Dpodetmatinicio = DateTime.ParseExact(fecIniDatPro, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture),
                        Dpodetmatfin = DateTime.ParseExact(fecFinDatPro, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture),
                        Dpofnccodima = entityFuncionesDataProcesar.Dpofnccodipr,
                        Dposecuencma = entityFuncionesDataProcesar.Dposecuencpr != null ? entityFuncionesDataProcesar.Dposecuencpr : "-",

                        Dpotipfuncion = "PR",

                        Pafunr1dtg1 = listDiasTipicosDpR1[0].Pafunr1dtg1 != null ? listDiasTipicosDpR1[0].Pafunr1dtg1 : "-",
                        Pafunr1dtg2 = listDiasTipicosDpR1[0].Pafunr1dtg2 != null ? listDiasTipicosDpR1[0].Pafunr1dtg2 : "-",
                        Pafunr1dtg3 = listDiasTipicosDpR1[0].Pafunr1dtg3 != null ? listDiasTipicosDpR1[0].Pafunr1dtg3 : "-",
                        Pafunr1dtg4 = listDiasTipicosDpR1[0].Pafunr1dtg4 != null ? listDiasTipicosDpR1[0].Pafunr1dtg4 : "-",
                        Pafunr1dtg5 = listDiasTipicosDpR1[0].Pafunr1dtg5 != null ? listDiasTipicosDpR1[0].Pafunr1dtg5 : "-",
                        Pafunr1dtg6 = listDiasTipicosDpR1[0].Pafunr1dtg6 != null ? listDiasTipicosDpR1[0].Pafunr1dtg6 : "-",
                        Pafunr1dtg7 = listDiasTipicosDpR1[0].Pafunr1dtg7 != null ? listDiasTipicosDpR1[0].Pafunr1dtg7 : "-",

                        Pafunr1deg7 = fecIniDpR1,
                        Pafunr1hag7 = fecFinDpR1,

                        Pafunr2dtg1 = listDiasTipicosDpR2[0].Pafunr2dtg1 != null ? listDiasTipicosDpR2[0].Pafunr2dtg1 : "-",
                        Pafunr2dtg2 = listDiasTipicosDpR2[0].Pafunr2dtg2 != null ? listDiasTipicosDpR2[0].Pafunr2dtg2 : "-",
                        Pafunr2dtg3 = listDiasTipicosDpR2[0].Pafunr2dtg3 != null ? listDiasTipicosDpR2[0].Pafunr2dtg3 : "-",
                        Pafunr2dtg4 = listDiasTipicosDpR2[0].Pafunr2dtg4 != null ? listDiasTipicosDpR2[0].Pafunr2dtg4 : "-",
                        Pafunr2dtg5 = listDiasTipicosDpR2[0].Pafunr2dtg5 != null ? listDiasTipicosDpR2[0].Pafunr2dtg5 : "-",
                        Pafunr2dtg6 = listDiasTipicosDpR2[0].Pafunr2dtg6 != null ? listDiasTipicosDpR2[0].Pafunr2dtg6 : "-",
                        Pafunr2dtg7 = listDiasTipicosDpR2[0].Pafunr2dtg7 != null ? listDiasTipicosDpR2[0].Pafunr2dtg7 : "-",

                        Pafunf1toram = numToleranciaRampaDpF1,
                        Pafunf2factk = numFactorKDpF2,

                        Pafuna1aniof = anioFeriadoDpA1,
                        Pafuna1idfer = idFeriadoDpA1,
                        Pafuna1dtg1 = listDiasTipicosDpA1[0].Pafuna1dtg1 != null ? listDiasTipicosDpA1[0].Pafuna1dtg1 : "-",
                        Pafuna1dtg2 = listDiasTipicosDpA1[0].Pafuna1dtg2 != null ? listDiasTipicosDpA1[0].Pafuna1dtg2 : "-",
                        Pafuna1dtg3 = listDiasTipicosDpA1[0].Pafuna1dtg3 != null ? listDiasTipicosDpA1[0].Pafuna1dtg3 : "-",
                        Pafuna1dtg4 = listDiasTipicosDpA1[0].Pafuna1dtg4 != null ? listDiasTipicosDpA1[0].Pafuna1dtg4 : "-",
                        Pafuna1dtg5 = listDiasTipicosDpA1[0].Pafuna1dtg5 != null ? listDiasTipicosDpA1[0].Pafuna1dtg5 : "-",
                        Pafuna1dtg6 = listDiasTipicosDpA1[0].Pafuna1dtg6 != null ? listDiasTipicosDpA1[0].Pafuna1dtg6 : "-",
                        Pafuna1dtg7 = listDiasTipicosDpA1[0].Pafuna1dtg7 != null ? listDiasTipicosDpA1[0].Pafuna1dtg7 : "-",

                        Pafuna2aniof = anioFeriadoDpA2,
                        Pafuna2idfer = idFeriadoDpA2,
                        Pafuna2dtg1 = listDiasTipicosDpA2[0].Pafuna2dtg1 != null ? listDiasTipicosDpA2[0].Pafuna2dtg1 : "-",
                        Pafuna2dtg2 = listDiasTipicosDpA2[0].Pafuna2dtg2 != null ? listDiasTipicosDpA2[0].Pafuna2dtg2 : "-",
                        Pafuna2dtg3 = listDiasTipicosDpA2[0].Pafuna2dtg3 != null ? listDiasTipicosDpA2[0].Pafuna2dtg3 : "-",
                        Pafuna2dtg4 = listDiasTipicosDpA2[0].Pafuna2dtg4 != null ? listDiasTipicosDpA2[0].Pafuna2dtg4 : "-",
                        Pafuna2dtg5 = listDiasTipicosDpA2[0].Pafuna2dtg5 != null ? listDiasTipicosDpA2[0].Pafuna2dtg5 : "-",
                        Pafuna2dtg6 = listDiasTipicosDpA2[0].Pafuna2dtg6 != null ? listDiasTipicosDpA2[0].Pafuna2dtg6 : "-",
                        Pafuna2dtg7 = listDiasTipicosDpA2[0].Pafuna2dtg7 != null ? listDiasTipicosDpA2[0].Pafuna2dtg7 : "-"
                    };

                    listCasoDetalleDTO.Add(dpoCasoDetalleDTO);
                }
                #endregion

                service.UpdateDpoCasoConfiguracion(id, dpoCasoDTO, listCasoDetalleDTO);

                model.Mensaje = "El caso fue modificado correctamente";
                model.Resultado = "1";


                model.ListaCasos = service.ListDpoCaso();

                return Json(model);
            }
            catch (Exception ex)
            {
                model.Resultado = "-1";
                model.Mensaje = ex.Message;
                model.Detalle = ex.StackTrace;
                log.Error(NameController, ex);

                return Json(model);
            }
        }

        /// <summary>
        /// Elimina casos de prueba
        /// </summary>
        /// <param name="id"></param>
        /// <returns>JsonResult</returns>
        public JsonResult EliminarCaso(int id)
        {
            CasoModel model = new CasoModel();

            try
            {
                service.DeleteCasoConfiguracion(id);

                model.Mensaje = "El caso fue eliminado con exito";
                model.Resultado = "1";

                model.ListaCasos = service.ListDpoCaso();

                return Json(model);
            }
            catch (Exception ex)
            {
                model.Resultado = "-1";
                model.Mensaje = ex.Message;
                model.Detalle = ex.StackTrace;
                log.Error(NameController, ex);

                return Json(model);
            }
        }

        /// <summary>
        /// Edita casos de prueba
        /// </summary>
        /// <param name="id"></param>
        /// <param name="nombre"></param>
        /// <returns>JsonResult</returns>
        public JsonResult CopiarCaso(int id, string nombre)
        {
            CasoModel model = new CasoModel();

            try
            {
                #region Armar el objeto DpoCasoDTO
                model.Caso = service.GetByIdDpoCaso(id);

                DpoCasoDTO dpoCasoDTO = new DpoCasoDTO
                {
                    Dpocsocnombre = nombre.TrimEnd(),
                    Areaabrev = model.Caso.Areaabrev.TrimEnd(),
                    Dpocsousucreacion = User.Identity.Name,
                    Dpocsofeccreacion = DateTime.Now
                };
                #endregion

                #region Armar el objeto DpoCasoDetalleDTO
                model.ListaDetalleCasos = service.ListByIdCaso(id);
                #endregion

                service.CopyCasoConfiguracion(dpoCasoDTO, model.ListaDetalleCasos);

                model.Mensaje = "El caso fue copiado con exito";
                model.Resultado = "1";

                model.ListaCasos = service.ListDpoCaso();

                return Json(model);
            }
            catch (Exception ex)
            {
                model.Resultado = "-1";
                model.Mensaje = ex.Message;
                model.Detalle = ex.StackTrace;
                log.Error(NameController, ex);

                return Json(model);
            }
        }

        /// <summary>
        /// Ejecuta casos de prueba
        /// </summary>
        /// <returns>JsonResult</returns>
        public JsonResult EjecutarCaso(int idCaso,
                                       int formScadaDatMae,
                                       string fecIniDatMae,
                                       string fecFinDatMae,
                                       List<DpoFuncionDataMaestraDTO> listFuncionesDataMaestra,
                                       int formScadaDatPro,
                                       string fecIniDatPro,
                                       string fecFinDatPro,
                                       List<DpoFuncionDataProcesarDTO> listFuncionesDataProcesar)

        {
            CasoModel model = new CasoModel();

            List<DpoHistorico48DTO> lstDatosHistoricosDm48 = new List<DpoHistorico48DTO>();
            List<DpoHistorico48DTO> lstDatosHistoricosDp48 = new List<DpoHistorico48DTO>();
            DpoHistorico48DTO obj48;

            List<DpoMedicion48DTO> lstDatosCorregidosDm48 = new List<DpoMedicion48DTO>();

            List<DpoMedicion48DTO> lstDatosCorregidosDm48A1 = new List<DpoMedicion48DTO>();
            List<List<DpoMedicion48DTO>> lstDatosCorregidosDm48A2 = new List<List<DpoMedicion48DTO>>();
            List<DpoMedicion48DTO> lstDatosCorregidosDm48F1 = new List<DpoMedicion48DTO>();
            List<DpoMedicion48DTO> lstDatosCorregidosDm48F2 = new List<DpoMedicion48DTO>();
            List<DpoMedicion48DTO> lstDatosCorregidosDm48R1 = new List<DpoMedicion48DTO>();
            List<DpoMedicion48DTO> lstDatosCorregidosDm48R2 = new List<DpoMedicion48DTO>();

            List<DpoMedicion48DTO> lstDatosCorregidosDp48 = new List<DpoMedicion48DTO>();

            List<DpoMedicion48DTO> lstDatosCorregidosDp48A1 = new List<DpoMedicion48DTO>();
            List<List<DpoMedicion48DTO>> lstDatosCorregidosDp48A2 = new List<List<DpoMedicion48DTO>>();
            List<DpoMedicion48DTO> lstDatosCorregidosDp48F1 = new List<DpoMedicion48DTO>();
            List<DpoMedicion48DTO> lstDatosCorregidosDp48F2 = new List<DpoMedicion48DTO>();
            List<DpoMedicion48DTO> lstDatosCorregidosDp48R1 = new List<DpoMedicion48DTO>();
            List<DpoMedicion48DTO> lstDatosCorregidosDp48R2 = new List<DpoMedicion48DTO>();


            try
            {
                #region Armar el origen de datos para la Data Maestra
                DateTime parseFechaIniDm = DateTime.ParseExact(fecIniDatMae, ConstantesDpo.FormatoFecha, CultureInfo.InvariantCulture);
                DateTime parseFecIniDm = DateTime.ParseExact(fecIniDatMae, ConstantesDpo.FormatoFecha, CultureInfo.InvariantCulture);
                DateTime parseFecFinDm = DateTime.ParseExact(fecFinDatMae, ConstantesDpo.FormatoFecha, CultureInfo.InvariantCulture);

                while (parseFecIniDm < parseFecFinDm)
                {
                    obj48 = new DpoHistorico48DTO();

                    decimal[] medicionesDm = service.ObtenerMedicionesCalculadas(formScadaDatMae, parseFecIniDm);

                    // lleno obj48 solo con los intervalos de la variable mdicio y falta fecha y pto medicion solo fecha se puede colocar
                    obj48.Medifecha = parseFecIniDm;
                    int i = 0;
                    foreach (decimal medicion in medicionesDm)
                    {
                        obj48.GetType().GetProperty($"H{(i + 1)}").SetValue(obj48, medicion);
                        i++;
                    }

                    // se arma una lista de decimales con add(decimal[] mdicio)
                    lstDatosHistoricosDm48.Add(obj48);

                    parseFecIniDm = parseFecIniDm.AddDays(1);
                }
                #endregion

                #region Ejecuta la Data Maestra
                listFuncionesDataMaestra = listFuncionesDataMaestra.Where(x => x.Dposecuencma != "-").OrderBy(x => x.Dposecuencma).ToList();

                // -------------------------------------------------------------------------------------------------
                // Valida secuencia de ejecucion de funciones
                // -------------------------------------------------------------------------------------------------
                if (listFuncionesDataMaestra.Count > 1)
                {
                    // -------------------------------------------------------------------------------------------------
                    // Primero se debe ejecutar la funcion A1 y de ahi R1
                    // -------------------------------------------------------------------------------------------------
                    int secuenciaDmR1 = 0;
                    int secuenciaDmA1 = 0;

                    foreach (DpoFuncionDataMaestraDTO entityFuncionesDataMaestra in listFuncionesDataMaestra)
                    {
                        if (entityFuncionesDataMaestra.Dpofnccodima == 1) // R1
                        {
                            secuenciaDmR1 = Convert.ToInt16(entityFuncionesDataMaestra.Dposecuencma);
                        }

                        if (entityFuncionesDataMaestra.Dpofnccodima == 5) // A1
                        {
                            secuenciaDmA1 = Convert.ToInt16(entityFuncionesDataMaestra.Dposecuencma);
                        }
                    }

                    if (secuenciaDmA1 > secuenciaDmR1)
                    {
                        model.Mensaje = "Primero se debe ejecutar la funcion A1 y de ahi R1";
                        model.Resultado = "-2";
                        return Json(model);
                    }

                    // -------------------------------------------------------------------------------------------------
                    // Primero se debe ejecutar la funcion A2, F2 y de ahi R2
                    // -------------------------------------------------------------------------------------------------
                    int secuenciaDmA2 = 0;
                    int secuenciaDmF2 = 0;
                    int secuenciaDmR2 = 0;

                    foreach (DpoFuncionDataMaestraDTO entityFuncionesDataMaestra in listFuncionesDataMaestra)
                    {
                        if (entityFuncionesDataMaestra.Dpofnccodima == 6) // A2
                        {
                            secuenciaDmA2 = Convert.ToInt16(entityFuncionesDataMaestra.Dposecuencma);
                        }

                        if (entityFuncionesDataMaestra.Dpofnccodima == 4) // F2
                        {
                            secuenciaDmF2 = Convert.ToInt16(entityFuncionesDataMaestra.Dposecuencma);
                        }

                        if (entityFuncionesDataMaestra.Dpofnccodima == 2) // R2
                        {
                            secuenciaDmR2 = Convert.ToInt16(entityFuncionesDataMaestra.Dposecuencma);
                        }
                    }

                    if (secuenciaDmA2 > secuenciaDmF2 && secuenciaDmF2 > secuenciaDmR2)
                    {
                        model.Mensaje = "Primero se debe ejecutar la funcion A2, F2 y de ahi R2";
                        model.Resultado = "-2";
                        return Json(model);
                    }

                    // -------------------------------------------------------------------------------------------------
                    // Primero se debe ejecutar la funcion A2 y de ahi F2
                    // -------------------------------------------------------------------------------------------------
                    secuenciaDmA2 = 0;
                    secuenciaDmF2 = 0;

                    foreach (DpoFuncionDataMaestraDTO entityFuncionesDataMaestra in listFuncionesDataMaestra)
                    {
                        if (entityFuncionesDataMaestra.Dpofnccodima == 6) // A2
                        {
                            secuenciaDmA2 = Convert.ToInt16(entityFuncionesDataMaestra.Dposecuencma);
                        }

                        if (entityFuncionesDataMaestra.Dpofnccodima == 4) // F2
                        {
                            secuenciaDmF2 = Convert.ToInt16(entityFuncionesDataMaestra.Dposecuencma);
                        }
                    }

                    if (secuenciaDmA2 > secuenciaDmF2)
                    {
                        model.Mensaje = "Primero se debe ejecutar la funcion A2, F2";
                        model.Resultado = "-2";
                        return Json(model);
                    }
                }


                foreach (DpoFuncionDataMaestraDTO entityFuncionesDataMaestra in listFuncionesDataMaestra)
                {
                    if (entityFuncionesDataMaestra.Dpofnccodima == 1)
                    {
                        // Primero se debe ejecutar la funcion A1 y de ahi R1
                        lstDatosCorregidosDm48R1 = service.AutoCompletarDelRangoFechasR1(idCaso,
                                                                                            entityFuncionesDataMaestra.Dpofnccodima,
                                                                                            ConstantesDpo.DataMaestra,
                                                                                            parseFechaIniDm,
                                                                                            parseFecFinDm,
                                                                                            lstDatosHistoricosDm48,
                                                                                            lstDatosCorregidosDm48A1);
                    }
                    else if (entityFuncionesDataMaestra.Dpofnccodima == 2)
                    {
                        // Primero se debe ejecutar la funcion A2, F2 y de ahi R2
                        lstDatosCorregidosDm48R2 = service.ReconstruirSeriesTiempoR2(idCaso,
                                                                                        entityFuncionesDataMaestra.Dpofnccodima,
                                                                                        ConstantesDpo.DataMaestra,
                                                                                        parseFechaIniDm,
                                                                                        parseFecFinDm,
                                                                                        lstDatosHistoricosDm48,
                                                                                        lstDatosCorregidosDm48A2,
                                                                                        lstDatosCorregidosDm48F2);
                    }
                    else if (entityFuncionesDataMaestra.Dpofnccodima == 3)
                    {
                        lstDatosCorregidosDm48F1 = service.FiltrarPorPMaximaVariacionRampaF1(idCaso,
                                                                                                entityFuncionesDataMaestra.Dpofnccodima,
                                                                                                ConstantesDpo.DataMaestra,
                                                                                                lstDatosHistoricosDm48);
                    }
                    else if (entityFuncionesDataMaestra.Dpofnccodima == 4)
                    {
                        // Primero se debe ejecutar la funcion A2 y de ahi F2
                        lstDatosCorregidosDm48F2 = service.ExcluirDatosFueraPromedioYDesviacionEstandarF2(idCaso,
                                                                                                            entityFuncionesDataMaestra.Dpofnccodima,
                                                                                                            ConstantesDpo.DataMaestra,
                                                                                                            parseFechaIniDm,
                                                                                                            parseFecFinDm,
                                                                                                            lstDatosHistoricosDm48,
                                                                                                            lstDatosCorregidosDm48A2);
                    }
                    else if (entityFuncionesDataMaestra.Dpofnccodima == 5)
                    {
                        lstDatosCorregidosDm48A1 = service.ObtenerUnaSemanaTipicaConPromedioYDesviacionEstandarA1(idCaso,
                                                                                                                    entityFuncionesDataMaestra.Dpofnccodima,
                                                                                                                    ConstantesDpo.DataMaestra,
                                                                                                                    parseFechaIniDm,
                                                                                                                    parseFecFinDm,
                                                                                                                    lstDatosHistoricosDm48);
                    }
                    else if (entityFuncionesDataMaestra.Dpofnccodima == 6)
                    {
                        lstDatosCorregidosDm48A2 = service.ObtenerDiasTipicosConPromedioYDesviacionEstandarA2(idCaso,
                                                                                                                entityFuncionesDataMaestra.Dpofnccodima,
                                                                                                                ConstantesDpo.DataMaestra,
                                                                                                                parseFechaIniDm,
                                                                                                                parseFecFinDm,
                                                                                                                lstDatosHistoricosDm48);
                    }
                }
                #endregion

                #region Armar el origen de datos para la Data a Procesar
                DateTime parseFechaIniDp = DateTime.ParseExact(fecIniDatPro, ConstantesDpo.FormatoFecha, CultureInfo.InvariantCulture);
                DateTime parseFecIniDp = DateTime.ParseExact(fecIniDatPro, ConstantesDpo.FormatoFecha, CultureInfo.InvariantCulture);
                DateTime parseFecFinDp = DateTime.ParseExact(fecFinDatPro, ConstantesDpo.FormatoFecha, CultureInfo.InvariantCulture);

                while (parseFecIniDp < parseFecFinDp)
                {
                    obj48 = new DpoHistorico48DTO();

                    decimal[] medicionesDp = service.ObtenerMedicionesCalculadas(formScadaDatPro, parseFecIniDp);

                    // lleno obj48 solo con los intervalos de la variable mdicio y falta fecha y pto medicion solo fecha se puede colocar
                    obj48.Medifecha = parseFecIniDp;
                    int i = 0;
                    foreach (decimal medicion in medicionesDp)
                    {
                        obj48.GetType().GetProperty($"H{(i + 1)}").SetValue(obj48, medicion);
                        i++;
                    }

                    // se arma una lista de decimales con add(decimal[] mdicio)
                    lstDatosHistoricosDp48.Add(obj48);

                    parseFecIniDp = parseFecIniDp.AddDays(1);
                }
                #endregion

                #region Ejecuta la Data a Procesar
                listFuncionesDataProcesar = listFuncionesDataProcesar.Where(x => x.Dposecuencpr != "-").OrderBy(x => x.Dposecuencpr).ToList();

                // -------------------------------------------------------------------------------------------------
                // Valida secuencia de ejecucion de funciones
                // -------------------------------------------------------------------------------------------------
                if (listFuncionesDataProcesar.Count > 1)
                {
                    // -------------------------------------------------------------------------------------------------
                    // Primero se debe ejecutar la funcion A1 y de ahi R1
                    // -------------------------------------------------------------------------------------------------
                    int secuenciaDpR1 = 0;
                    int secuenciaDpA1 = 0;

                    foreach (DpoFuncionDataProcesarDTO entityFuncionesDataProcesar in listFuncionesDataProcesar)
                    {
                        if (entityFuncionesDataProcesar.Dpofnccodipr == 1) // R1
                        {
                            secuenciaDpR1 = Convert.ToInt16(entityFuncionesDataProcesar.Dposecuencpr);
                        }

                        if (entityFuncionesDataProcesar.Dpofnccodipr == 5) // A1
                        {
                            secuenciaDpA1 = Convert.ToInt16(entityFuncionesDataProcesar.Dposecuencpr);
                        }
                    }

                    if (secuenciaDpA1 > secuenciaDpR1)
                    {
                        model.Mensaje = "Primero se debe ejecutar la funcion A1 y de ahi R1";
                        model.Resultado = "-2";
                        return Json(model);
                    }

                    // -------------------------------------------------------------------------------------------------
                    // Primero se debe ejecutar la funcion A2, F2 y de ahi R2
                    // -------------------------------------------------------------------------------------------------
                    int secuenciaDpA2 = 0;
                    int secuenciaDpF2 = 0;
                    int secuenciaDpR2 = 0;

                    foreach (DpoFuncionDataProcesarDTO entityFuncionesDataProcesar in listFuncionesDataProcesar)
                    {
                        if (entityFuncionesDataProcesar.Dpofnccodipr == 6) // A2
                        {
                            secuenciaDpA2 = Convert.ToInt16(entityFuncionesDataProcesar.Dposecuencpr);
                        }

                        if (entityFuncionesDataProcesar.Dpofnccodipr == 4) // F2
                        {
                            secuenciaDpF2 = Convert.ToInt16(entityFuncionesDataProcesar.Dposecuencpr);
                        }

                        if (entityFuncionesDataProcesar.Dpofnccodipr == 2) // R2
                        {
                            secuenciaDpR2 = Convert.ToInt16(entityFuncionesDataProcesar.Dposecuencpr);
                        }
                    }

                    if (secuenciaDpA2 > secuenciaDpF2 && secuenciaDpF2 > secuenciaDpR2)
                    {
                        model.Mensaje = "Primero se debe ejecutar la funcion A2, F2 y de ahi R2";
                        model.Resultado = "-2";
                        return Json(model);
                    }

                    // -------------------------------------------------------------------------------------------------
                    // Primero se debe ejecutar la funcion A2 y de ahi F2
                    // -------------------------------------------------------------------------------------------------
                    secuenciaDpA2 = 0;
                    secuenciaDpF2 = 0;

                    foreach (DpoFuncionDataProcesarDTO entityFuncionesDataProcesar in listFuncionesDataProcesar)
                    {
                        if (entityFuncionesDataProcesar.Dpofnccodipr == 6) // A2
                        {
                            secuenciaDpA2 = Convert.ToInt16(entityFuncionesDataProcesar.Dposecuencpr);
                        }

                        if (entityFuncionesDataProcesar.Dpofnccodipr == 4) // F2
                        {
                            secuenciaDpF2 = Convert.ToInt16(entityFuncionesDataProcesar.Dposecuencpr);
                        }
                    }

                    if (secuenciaDpA2 > secuenciaDpF2)
                    {
                        model.Mensaje = "Primero se debe ejecutar la funcion A2, F2";
                        model.Resultado = "-2";
                        return Json(model);
                    }
                }


                foreach (DpoFuncionDataProcesarDTO entityFuncionesDataProcesar in listFuncionesDataProcesar)
                {
                    if (entityFuncionesDataProcesar.Dpofnccodipr == 1)
                    {
                        // Primero se debe ejecutar la funcion A1 y de ahi R1
                        lstDatosCorregidosDp48R1 = service.AutoCompletarDelRangoFechasR1(idCaso,
                                                                                            entityFuncionesDataProcesar.Dpofnccodipr,
                                                                                            ConstantesDpo.DataProcesar,
                                                                                            parseFechaIniDp,
                                                                                            parseFecFinDp,
                                                                                            lstDatosHistoricosDp48,
                                                                                            lstDatosCorregidosDp48A1);
                    }
                    else if (entityFuncionesDataProcesar.Dpofnccodipr == 2)
                    {
                        // Primero se debe ejecutar la funcion A2, F2 y de ahi R2
                        lstDatosCorregidosDp48R2 = service.ReconstruirSeriesTiempoR2(idCaso,
                                                                                        entityFuncionesDataProcesar.Dpofnccodipr,
                                                                                        ConstantesDpo.DataProcesar,
                                                                                        parseFechaIniDp,
                                                                                        parseFecFinDp,
                                                                                        lstDatosHistoricosDp48,
                                                                                        lstDatosCorregidosDp48A2,
                                                                                        lstDatosCorregidosDp48F2);
                    }
                    else if (entityFuncionesDataProcesar.Dpofnccodipr == 3)
                    {
                        lstDatosCorregidosDp48F1 = service.FiltrarPorPMaximaVariacionRampaF1(idCaso,
                                                                                                entityFuncionesDataProcesar.Dpofnccodipr,
                                                                                                ConstantesDpo.DataProcesar,
                                                                                                lstDatosHistoricosDp48);
                    }
                    else if (entityFuncionesDataProcesar.Dpofnccodipr == 4)
                    {
                        // Primero se debe ejecutar la funcion A2 y de ahi F2
                        lstDatosCorregidosDp48F2 = service.ExcluirDatosFueraPromedioYDesviacionEstandarF2(idCaso,
                                                                                                            entityFuncionesDataProcesar.Dpofnccodipr,
                                                                                                            ConstantesDpo.DataProcesar,
                                                                                                            parseFechaIniDp,
                                                                                                            parseFecFinDp,
                                                                                                            lstDatosHistoricosDp48,
                                                                                                            lstDatosCorregidosDp48A2);
                    }
                    else if (entityFuncionesDataProcesar.Dpofnccodipr == 5)
                    {
                        lstDatosCorregidosDp48A1 = service.ObtenerUnaSemanaTipicaConPromedioYDesviacionEstandarA1(idCaso,
                                                                                                                    entityFuncionesDataProcesar.Dpofnccodipr,
                                                                                                                    ConstantesDpo.DataProcesar,
                                                                                                                    parseFechaIniDp,
                                                                                                                    parseFecFinDp,
                                                                                                                    lstDatosHistoricosDp48);
                    }
                    else if (entityFuncionesDataProcesar.Dpofnccodipr == 6)
                    {
                        lstDatosCorregidosDp48A2 = service.ObtenerDiasTipicosConPromedioYDesviacionEstandarA2(idCaso,
                                                                                                                entityFuncionesDataProcesar.Dpofnccodipr,
                                                                                                                ConstantesDpo.DataProcesar,
                                                                                                                parseFechaIniDp,
                                                                                                                parseFecFinDp,
                                                                                                                lstDatosHistoricosDp48);
                    }
                }
                #endregion

                #region Exporto la data maestra y a procesar corregida a un archivo Excel

                // Ordeno los datos
                lstDatosCorregidosDm48R1 = lstDatosCorregidosDm48R1.OrderBy(x => x.Medifecha).ToList();
                lstDatosCorregidosDm48F1 = lstDatosCorregidosDm48F1.OrderBy(x => x.Medifecha).ToList();
                lstDatosCorregidosDp48R1 = lstDatosCorregidosDp48R1.OrderBy(x => x.Medifecha).ToList();
                lstDatosCorregidosDp48F1 = lstDatosCorregidosDp48F1.OrderBy(x => x.Medifecha).ToList();

                // Establezco la ruta del archivo de exportación a generar
                //string pathFile = AppDomain.CurrentDomain.BaseDirectory + ConstantesDpo.RutaReportes;
                string pathFile = ConfigurationManager.AppSettings[ConstantesDpo.RutaReporte].ToString();
                string fileName = ConstantesDpo.NombreReporteConfiguracion;

                model.Resultado = service.ExportToExcelDataCorregida2(lstDatosHistoricosDm48,
                                                                      lstDatosCorregidosDm48R1,
                                                                      lstDatosCorregidosDm48R2,
                                                                      lstDatosCorregidosDm48F1,
                                                                      lstDatosCorregidosDm48F2,
                                                                      lstDatosCorregidosDm48A1,
                                                                      lstDatosCorregidosDm48A2,
                                                                      lstDatosHistoricosDp48,
                                                                      lstDatosCorregidosDp48R1,
                                                                      lstDatosCorregidosDp48R2,
                                                                      lstDatosCorregidosDp48F1,
                                                                      lstDatosCorregidosDp48F2,
                                                                      lstDatosCorregidosDp48A1,
                                                                      lstDatosCorregidosDp48A2,
                                                                      pathFile,
                                                                      fileName);
                #endregion

                model.Mensaje = "Se ejecuto correctamente el caso";


                return Json(model);
            }
            catch (Exception ex)
            {
                model.Resultado = "-1";
                model.Mensaje = ex.Message;
                model.Detalle = ex.StackTrace;
                log.Error(NameController, ex);

                return Json(model);
            }
        }

        /// <summary>
        /// Función que se encarga de descargar el archivo generado al vuelo en la raiz de la aplicación
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public virtual ActionResult AbrirArchivo(int formato, string file)
        {
            string sFecha = DateTime.Now.ToString("yyyyMMddHHmmss");
            string path = ConfigurationManager.AppSettings[ConstantesDpo.RutaReporte].ToString() + file;
            //string path = AppDomain.CurrentDomain.BaseDirectory + ConstantesDpo.RutaReportes + file;
            string app = Constantes.AppExcel;
            return File(path, app, sFecha + "_" + file);
        }

    }
}