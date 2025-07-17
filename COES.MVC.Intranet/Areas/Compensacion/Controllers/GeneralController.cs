using COES.Dominio.DTO.Sic;
using COES.Dominio.DTO.Transferencias;
using COES.MVC.Intranet.Areas.Compensacion.Models;
using COES.MVC.Intranet.Areas.Compensacion.Models.General;
using COES.MVC.Intranet.Controllers;
using COES.MVC.Intranet.Helper;
using COES.Servicios.Aplicacion.Compensacion;
using COES.Servicios.Aplicacion.Compensacion.Helper;
using log4net;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace COES.MVC.Intranet.Areas.Compensacion.Controllers
{
    //- compensaciones.HDT - 16/03/2017: Cambio para atender el requerimiento. 
    /// <summary>
    /// Tipo de concepto. Está relacionad con la columna PR_CONCEPTO.CONCEPCODI.
    /// </summary>
    public enum TipoConcepto
    {
        Ninguno = 0,
        PotenciaEfectiva = 14,
        Rendimiento = 190
    }

    public class GeneralController : BaseController
    {

        //- compensaciones.HDT - 16/03/2017: Cambio para atender el requerimiento. 
        private readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        //- compensaciones.HDT - 06/04/2017: Cambio para atender el requerimiento. 
        private const int constRendimientoVigente = 190;

        //- compensaciones.HDT - 06/04/2017: Cambio para atender el requerimiento. 
        private const int constPrecioCombAlt = 240;

        //- compensaciones.HDT - 06/04/2017: Cambio para atender el requerimiento. 
        private const int constPrecioComb = 22;

        //- compensaciones.HDT - 16/03/2017: Cambio para atender el requerimiento. 
        public GeneralController()
        {
            log4net.Config.XmlConfigurator.Configure();
        }

        //- compensaciones.HDT - 16/03/2017: Cambio para atender el requerimiento. 
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

        //
        // GET: /Compensacion/Compensacion/

        /// <summary>
        /// Instancia de clase de aplicación
        /// </summary>
        CompensacionAppServicio servicio = new CompensacionAppServicio();

        public ActionResult Index()
        {

            return View();
        }

        /// <summary>
        /// Muestra la pantalla Mes de Valorización
        /// </summary>
        /// <returns></returns>
        public ActionResult MesValorizacion()
        {

            //base.ValidarSesionUsuario();
            MesValorizacionGeneralModel model = new MesValorizacionGeneralModel();

            return View(model);
        }

        /// <summary>
        /// Muestra la lista de meses de valoración
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult ListarMesValorizacion()
        {
            MesValorizacionGeneralModel model = new MesValorizacionGeneralModel();

            model.ListTrnPeriodo = servicio.ListarPeriodosCompensacion();

            return PartialView("ListarMesValorizacion", model);
        }

        /// <summary>
        /// Muestra la pantalla Horas de Operación
        /// </summary>
        /// <returns></returns>
        public ActionResult HorasOperacion()
        {
            base.ValidarSesionUsuario();
            HorasOperacionGeneralModel model = new HorasOperacionGeneralModel();
            model.ListTrnPeriodo = servicio.ListarPeriodosTC();
            model.ListSiEmpresa = servicio.ListaEmpresasCompensacion();

            int periodo = model.ListTrnPeriodo[0].PeriCodi;
            model.ListEveSubcausaevento = servicio.ListTipoOperacion(periodo);

            return View(model);
        }

        /// <summary>
        /// Muestra la lista de Horas de Operación
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult ListarHorasOperacion(int pecacodi, string empresa, string central, string grupo, string modo, string tipo, string fecIni, string fecFin, string arranque, string parada)
        {

            HorasOperacionGeneralModel model = new HorasOperacionGeneralModel();
            model.ListHoraOperacion = servicio.ListarHoraOperacionFiltro(pecacodi, empresa, central, grupo, modo, tipo, fecIni, fecFin, arranque, parada);
            return PartialView("ListarHorasOperacion", model);
        }

        [HttpPost]
        public JsonResult obtenerDataHorasOperacion(int id, int pecacodi)
        {
            HorasOperacionGeneralModel model = new HorasOperacionGeneralModel();

            VcePeriodoCalculoDTO vcePeriodoCalculoDTO = servicio.getVersionPeriodoById(pecacodi);
            if (vcePeriodoCalculoDTO != null)
            {
                PeriodoDTO trnPeriodo = servicio.getPeriodoById(vcePeriodoCalculoDTO.PeriCodi);

                DateTime fec_ini = new DateTime(trnPeriodo.AnioCodi, trnPeriodo.MesCodi, 1);
                DateTime fec_fin = new DateTime(trnPeriodo.AnioCodi, trnPeriodo.MesCodi, 1).AddMonths(1).AddDays(-1);

                //Eliminamos los registros
                servicio.DeleteHoraOperacion(pecacodi);

                //Migramos la data
                servicio.SaveHoraOperacion(pecacodi, fec_ini.ToString(Constantes.FormatoFecha), fec_fin.ToString(Constantes.FormatoFecha));

                //Actualizamos el LogCarga
                servicio.SaveVceLogCargaDet(id, User.Identity.Name, "VCE_HORA_OPERACION", pecacodi);

            }

            return Json(model);
        }

        public ActionResult VerificarHorasOperacion()
        {
            HorasOperacionGeneralModel model = new HorasOperacionGeneralModel();
            model.ListTrnPeriodo = servicio.ListarPeriodosTC();

            int periodo = model.ListTrnPeriodo[0].PeriCodi;

            return View(model);
        }

        /// <summary>
        /// Muestra la lista de horas de operacion a verificar
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult ListarVerificarHorasOperacion(int pecacodi)
        {
            HorasOperacionGeneralModel model = new HorasOperacionGeneralModel();
            model.ListHoraOperacion = servicio.ListarVerificarHoras(pecacodi);
            return PartialView("ListarVerificarHorasOperacion", model);
        }

        public JsonResult ObtenerHoraOperacion(int pecacodi, int hopcodi)
        {

            VceHoraOperacionDTO oVceHoraOperacionDTO = servicio.GetDataByIdHoraOperacion(pecacodi, hopcodi);

            var jsonSerialiser = new JavaScriptSerializer();
            return Json(jsonSerialiser.Serialize(oVceHoraOperacionDTO));
        }

        public ActionResult GuardarRangoHoraOperacion(int Pecacodi, int Hopcodi, string Crhophorini, string Crhophorfin, bool EsNuevo)
        {
            HorasOperacionGeneralModel model = new HorasOperacionGeneralModel();

            VceHoraOperacionDTO oVceHoraOperacionDTO = servicio.GetByIdHoraOperacion(Pecacodi, Hopcodi);

            if (EsNuevo)
            {
                if (oVceHoraOperacionDTO != null)
                {
                    //- Ya existe un registro igual, no se puede Crear.
                    return Json(new { success = false, message = "Ya existe un registro igual, no se puede crear" });
                }

                //this.servicio.CrearIncrementoReduccion(oVceArrparIncredGenDTO);
            }
            else
            {
                if (oVceHoraOperacionDTO == null)
                {
                    //- El registro ya no existe, no se puede actualizar.
                    return Json(new { success = false, message = "El registro ya no existe, no se puede actualizar" });
                }
                //FormatoFechaHora

                oVceHoraOperacionDTO.Crhophorini = DateTime.ParseExact(Crhophorini, "dd/MM/yyyy HH:mm", null);
                oVceHoraOperacionDTO.Crhophorfin = DateTime.ParseExact(Crhophorfin, "dd/MM/yyyy HH:mm", null);

                this.servicio.UpdateRangoHoraOperacion(oVceHoraOperacionDTO);
            }

            return Json(new { success = true, message = "Ok" });
        }

        public ActionResult EliminarRangoHoraOperacion(int pecacodi, int hopcodi)
        {
            HorasOperacionGeneralModel model = new HorasOperacionGeneralModel();

            VceHoraOperacionDTO oVceHoraOperacionDTO = servicio.GetByIdHoraOperacion(pecacodi, hopcodi);

            if (oVceHoraOperacionDTO == null)
            {
                //- El registro ya no existe, no se puede actualizar.
                return Json(new { success = false, message = "El registro ya no existe, no se puede eliminar" });
            }

            this.servicio.DeleteRangoHoraOperacion(pecacodi, hopcodi);

            return Json(new { success = true, message = "Ok" });
        }

        /// <summary>
        /// Muestra la pantalla Costos Marginales
        /// </summary>
        /// <returns></returns>
        public ActionResult CostosMarginales()
        {
            CostosMarginalesGeneralModel model = new CostosMarginalesGeneralModel();
            model.ListTrnPeriodo = servicio.ListarPeriodosTC();
            return View(model);
        }

        /// <summary>
        /// Muestra la lista de los Costos Marginales
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult ListarCostosMarginales(int pecacodi)
        {
            CostosMarginalesGeneralModel model = new CostosMarginalesGeneralModel();
            model.ListHeadCostoMarginal = servicio.lstHeadCostoMarginal(pecacodi);
            model.ListBodyCostoMarginal = servicio.lstBodyCostoMarginal(pecacodi, 1); // por regularizar
            return PartialView("ListarCostosMarginales", model);
        }

        /// <summary>
        /// Muestra la pantalla Energía
        /// </summary>
        /// <returns></returns>
        public ActionResult Energia()
        {
            EnergiaGeneralModel model = new EnergiaGeneralModel();
            model.ListTrnPeriodo = servicio.ListarPeriodosTC();
            return View(model);
        }

        /// <summary>
        /// Obtener la lista de Energias
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult ListarEnergia(int pecacodi)
        {
            EnergiaGeneralModel model = new EnergiaGeneralModel();
            model.ListEnergia = servicio.ListarEnergia(100, pecacodi);
            return PartialView("ListarEnergia", model);
        }


        [HttpPost]
        public JsonResult obtenerDataEnergia(int id, int pecacodi)
        {
            EnergiaGeneralModel model = new EnergiaGeneralModel();

            VcePeriodoCalculoDTO vcePeriodoCalculo = servicio.getVersionPeriodoById(pecacodi);
            if (vcePeriodoCalculo != null)
            {
                PeriodoDTO trnPeriodo = servicio.getPeriodoById(vcePeriodoCalculo.PeriCodi);

                DateTime fec_ini = new DateTime(trnPeriodo.AnioCodi, trnPeriodo.MesCodi, 1);
                DateTime fec_fin = new DateTime(trnPeriodo.AnioCodi, trnPeriodo.MesCodi, 1).AddMonths(1).AddDays(-1);

                //Eliminamos los registros
                servicio.DeleteEnergia(fec_ini.ToString(Constantes.FormatoFecha), fec_fin.ToString(Constantes.FormatoFecha));

                //Migramos la data
                servicio.SaveEnergia(pecacodi, fec_ini.ToString(Constantes.FormatoFecha), fec_fin.ToString(Constantes.FormatoFecha));

                //Actualizamos el LogCarga
                //servicio.ActualizarLogCarga("VCE_ENERGIA", pecacodi);
                servicio.SaveVceLogCargaDet(id, User.Identity.Name, "VCE_ENERGIA", pecacodi);
            }

            return Json(model);
        }

        /// <summary>
        /// Muestra la pantalla ILO I
        /// </summary>
        /// <returns></returns>
        public ActionResult IloI()
        {
            return View();
        }

        /// <summary>
        /// Muestra la pantalla Costos Variables
        /// </summary>
        /// <returns></returns>
        public ActionResult CostosVariables()
        {
            CostosVariablesGeneralModel model = new CostosVariablesGeneralModel();
            model.ListTrnPeriodo = servicio.ListarPeriodosTC();
            model.ListSiEmpresa = servicio.ListaEmpresasCompensacion();
            return View(model);
        }

        /// <summary>
        /// Listar los costos variables
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult ListarCostosVariables(int pecacodi, string empresa, string central, string grupo, string modo)
        {
            CostosVariablesGeneralModel model = new CostosVariablesGeneralModel();
            model.ListDatCalculo = servicio.ListarDatCalculo(pecacodi, empresa, central, grupo, modo);
            return PartialView("ListarCostosVariables", model);
        }

        [HttpGet]
        public ActionResult EditCalculo(int per, int grup)
        {
            CostosVariablesGeneralModel model = new CostosVariablesGeneralModel();

            VceDatcalculoDTO entity = this.servicio.VceDatCalculoGetById(per, grup);
            entity.PecaCodi = per;
            entity.Grupocodi = grup;

            model.VceDatCalculo = entity;
            model.ListTrnBarra = this.servicio.ListBarra();
            return View("EditCalculo", model);
        }

        [HttpPost]
        public JsonResult GrabarCalculo(int pecacodi, int grupocodi, int barrcodi, string considera, string energia, string tiempo)
        {
            CostosVariablesGeneralModel model = new CostosVariablesGeneralModel();

            VceDatcalculoDTO entity = new VceDatcalculoDTO();
            entity.PecaCodi = pecacodi;
            entity.Grupocodi = grupocodi;
            entity.Barrcodi = barrcodi;
            entity.Vcedcmconsiderapotnom = considera;
            entity.Vcedcmenergia = decimal.Parse(energia);
            entity.Vcedcmtiempo = decimal.Parse(tiempo);

            this.servicio.UpdateVceDatCalculo(entity);

            return Json(model);
        }

        //- compensaciones.HDT - 03/03/2017: Cambio para atender el requerimiento. 
        [HttpPost]
        public JsonResult obtenerDataCalculo(int id, int pecacodi)
        {
            Log.Debug("INICIO -> PROCESO DE GENERACION DE PARAMETROS");

            CostosVariablesGeneralModel model = new CostosVariablesGeneralModel();
            string periAnioMes = string.Empty;
            string pecaTipoCambio = string.Empty;

            //- 1. Se eliminan los registros previamente calculados.            
            Log.Debug("1. Se eliminan los registros previamente calculados");
            servicio.DeleteCalculo(pecacodi);

            //- 2. Se obtienen los campos del tipo dado.
            Log.Debug("2. Se obtienen los campos del tipo dado.");
            string cfgdctipoval = EnuVceCfgDatCalculoCfgDcTipoVal.V.ToString();
            List<VceCfgDatCalculoDTO> lVceCfgDatCalculoDTO = servicio.ObtenerCfgsCampo(cfgdctipoval);

            //- 3. Se preparan los datos para el cálculo.
            Log.Debug("3. Se preparan los datos para el cálculo.");
            servicio.ConfigurarParametroCalculo(pecacodi, ref periAnioMes, ref pecaTipoCambio);

            //- 4. Se pueblan los registros pero aún sin los cálculos.
            Log.Debug("4. Se pueblan los registros pero aún sin los cálculos.");
            servicio.PoblarRegistroSinCalculos(pecacodi, periAnioMes);

            //- 5. Se obtienen las distintas fechas de modificación.
            Log.Debug("5. Se obtienen las distintas fechas de modificación.");
            List<DateTime> lFechasModificacion = servicio.ObtenerDistintasFechasModificacion(pecacodi);

            string formatoFecha = "dd/MM/yyyy";

            //- 6. Actualizar Tipo de Combustible por las distintas fechas.
            Log.Debug("6. Actualizar Tipo de Combustible por las distintas fechas.");
            foreach (DateTime fechaModificacion in lFechasModificacion)
            {
                //- Realizar la actualización por cada fecha.
                servicio.ActualizarTipoCombustible(pecacodi, fechaModificacion.ToString(formatoFecha));
            }

            //- 7. Se obtienen los registros sin cálculos.
            Log.Debug("7. Se obtienen los registros sin cálculos.");

            List<VceDatcalculoDTO> lVceDtacalculoDTOObjetivo = null;
            VceDatcalculoDTO vceDatcalculoDTOActualizar = null;

            PropertyInfo infoPropiedad = null;

            List<VceDatcalculoDTO> lVceDtacalculoDTO = servicio.ObtenerRegistroSinCalculos(pecacodi);

            List<PrGrupodatDTO> lPrGrupodatDTO = null;
            List<int> lIdGrupo = null;

            //- 8. Actualizar los conceptos.
            Log.Debug("8. Actualizar los conceptos.");
            foreach (DateTime fechaModificacion in lFechasModificacion)
            {
                foreach (VceCfgDatCalculoDTO vceCfgDatCalculoDTO in lVceCfgDatCalculoDTO)
                {
                    //- 8.1. Se obtienen los distintos grupos involucrados en los registros aún sin cálculo.
                    Log.Debug("8.1. Se obtienen los distintos grupos involucrados en los registros aún sin cálculo: Concepcodi (" + vceCfgDatCalculoDTO.Concepcodi.ToString() + ")");

                    lIdGrupo = servicio.ObtenerDistintosIdGrupo(pecacodi, vceCfgDatCalculoDTO.Fenergcodi, vceCfgDatCalculoDTO.Cfgdccondsql);
                    if (lIdGrupo.Count == 0)
                    {
                        //- Nada que hacer porque no se encontró grupo alguno.
                        continue;
                    }

                    //- 8.2. Se obtienen los grupos con valores a través del resultado de la fórmula.
                    Log.Debug("8.2. Se obtienen los grupos con valores a través del resultado de la fórmula. Concepto:" + vceCfgDatCalculoDTO.Concepcodi);
                    //Console.WriteLine("8.2. Se obtienen los grupos con valores a través del resultado de la fórmula. Concepto:" + vceCfgDatCalculoDTO.Concepcodi);
                    //System.Diagnostics.Debug.WriteLine("8.2. Se obtienen los grupos con valores a través del resultado de la fórmula. Concepto: " + vceCfgDatCalculoDTO.Concepcodi);
                    
                   
                    lPrGrupodatDTO = servicio.ObtenerResultadoFormula(lIdGrupo, vceCfgDatCalculoDTO.Concepcodi, fechaModificacion, pecaTipoCambio);
                    string nombrePropiedad = string.Empty;

                    foreach (PrGrupodatDTO prGrupodatDTO in lPrGrupodatDTO)
                    {
                        Log.Debug("Codigo de GrupoCodi" + prGrupodatDTO.Grupocodi);
                        lVceDtacalculoDTOObjetivo = lVceDtacalculoDTO.Where(x => x.Grupocodi == prGrupodatDTO.Grupocodi
                                                                            && x.Crdcgfecmod.ToString(formatoFecha) == fechaModificacion.ToString(formatoFecha))
                                                                            .ToList();
                        if (lVceDtacalculoDTOObjetivo.Count == 1)
                        {
                            //- 8.2.1. Se procede a configurar las propiedades.
                            Log.Debug("8.2.1. Se procede a configurar las propiedades.");

                            vceDatcalculoDTOActualizar = lVceDtacalculoDTOObjetivo[0];

                            nombrePropiedad = vceCfgDatCalculoDTO.Cfgdccamponomb.Trim();

                            nombrePropiedad = nombrePropiedad.ToUpper().Substring(0, 1) + nombrePropiedad.ToLower().Substring(1);

                            infoPropiedad = vceDatcalculoDTOActualizar.GetType().GetProperty(nombrePropiedad);

                            if (infoPropiedad.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                            {
                                if (infoPropiedad.PropertyType == typeof(int?))
                                {
                                    infoPropiedad.SetValue(vceDatcalculoDTOActualizar, Decimal.ToInt32(prGrupodatDTO.Valor));
                                }
                                else
                                {
                                    infoPropiedad.SetValue(vceDatcalculoDTOActualizar, prGrupodatDTO.Valor);
                                }
                            }
                            else
                            {
                                infoPropiedad.SetValue(vceDatcalculoDTOActualizar, Convert.ChangeType(prGrupodatDTO.Valor, infoPropiedad.PropertyType), null);
                            }

                            vceDatcalculoDTOActualizar.ActualizarRegistro = true;

                        }

                    }
                }
            }

            //- 9. Realizando la actualización diferida resultante del punto 8.
            Log.Debug("9. Realizando la actualización diferida resultante del punto 8.");
            List<VceDatcalculoDTO> lVceDtacalculoDTOActualizar = lVceDtacalculoDTO.Where(x => x.ActualizarRegistro == true).ToList();
            foreach (VceDatcalculoDTO dtoActualizar in lVceDtacalculoDTOActualizar)
            {
                Log.Debug(""+dtoActualizar.Grupocodi + " " + dtoActualizar.Barrcodi);
                servicio.UpdateVceDatCalculo(dtoActualizar);
            }

            servicio.ActualizarDatosPosCalculo(pecacodi, pecaTipoCambio);

            //- 10. Actualizar los datos del Consumo de combustible en Potencia Efectiva.
            Log.Debug("10. Actualizar los datos del Consumo de combustible en Potencia Efectiva.");

            //- 10.0. Se obtienen las distintas fechas de modificación sobre la potencia efectiva.
            Log.Debug("10.0. Se obtienen las distintas fechas de modificación sobre la potencia efectiva.");
            lFechasModificacion = servicio.ObtenerDistintasFechasModificacionPotenciaEfectiva(pecacodi);

            //- 10.1. Se obtienen los registros sin cálculos en la potencia efectiva.
            Log.Debug("10.1. Se obtienen los registros sin cálculos en la potencia efectiva.");
            lVceDtacalculoDTO = servicio.ObtenerRegistroSinCalculosPotenciaEfectica(pecacodi);

            //- 10.2 Se procesa las fechas involucradas.
            Log.Debug("10.2 Se procesa las fechas involucradas.");
            foreach (DateTime fechaModificacion in lFechasModificacion)
            {
                lIdGrupo = servicio.ObtenerDistintosIdGrupo(pecacodi);
                if (lIdGrupo.Count == 0)
                {
                    //- Nada que hacer porque no se encontró grupo alguno.
                    continue;
                }

                //- 10.2.1 Procesando el Concepto Potencia Efectiva.
                lPrGrupodatDTO = servicio.ObtenerResultadoFormula(lIdGrupo, (int)TipoConcepto.PotenciaEfectiva, fechaModificacion, pecaTipoCambio);
                foreach (PrGrupodatDTO prGrupodatDTO in lPrGrupodatDTO)
                {
                    lVceDtacalculoDTOObjetivo = lVceDtacalculoDTO.Where(x => x.Grupocodi == prGrupodatDTO.Grupocodi
                                                                        && x.Crdcgfecmod.ToString(formatoFecha) == fechaModificacion.ToString(formatoFecha))
                                                                        .ToList();
                    if (lVceDtacalculoDTOObjetivo.Count == 1)
                    {
                        vceDatcalculoDTOActualizar = lVceDtacalculoDTOObjetivo[0];
                        vceDatcalculoDTOActualizar.CrdcgccpotefeNumerador = prGrupodatDTO.Valor;
                        vceDatcalculoDTOActualizar.ActualizarRegistro = true;
                    }

                }

                //- 10.2.2 Procesando el Concepto Rendimiento.
                lPrGrupodatDTO = servicio.ObtenerResultadoFormula(lIdGrupo, (int)TipoConcepto.Rendimiento, fechaModificacion, pecaTipoCambio);
                foreach (PrGrupodatDTO prGrupodatDTO in lPrGrupodatDTO)
                {
                    lVceDtacalculoDTOObjetivo = lVceDtacalculoDTO.Where(x => x.Grupocodi == prGrupodatDTO.Grupocodi
                                                                        && x.Crdcgfecmod.ToString(formatoFecha) == fechaModificacion.ToString(formatoFecha))
                                                                        .ToList();
                    if (lVceDtacalculoDTOObjetivo.Count == 1)
                    {
                        vceDatcalculoDTOActualizar = lVceDtacalculoDTOObjetivo[0];
                        vceDatcalculoDTOActualizar.CrdcgccpotefeDenominador = prGrupodatDTO.Valor;
                        vceDatcalculoDTOActualizar.ActualizarRegistro = true;
                        if (prGrupodatDTO.Valor == 0)
                        {
                            //Registramos en el Log
                            Log.Debug("***El valor del rendimiento es cero para el Grupo: " + prGrupodatDTO.Grupocodi + " Concepto: " + (int)TipoConcepto.Rendimiento + " Fecha: " + fechaModificacion);
                        }
                    }
                }

            }

            //- 10.3 Se realiza la actualización de los registros afectados.
            Log.Debug("10.3 Se realiza la actualización de los registros afectados.");
            lVceDtacalculoDTOActualizar = lVceDtacalculoDTO.Where(x => x.ActualizarRegistro == true).ToList();
            foreach (VceDatcalculoDTO dtoActualizar in lVceDtacalculoDTOActualizar)
            {
                if (dtoActualizar.CrdcgccpotefeNumerador.HasValue && dtoActualizar.CrdcgccpotefeDenominador.HasValue)
                {
                    if (dtoActualizar.CrdcgccpotefeDenominador.Value != 0)
                    {
                        dtoActualizar.Crdcgccpotefe = (dtoActualizar.CrdcgccpotefeNumerador.Value / dtoActualizar.CrdcgccpotefeDenominador.Value) * 1000;
                        servicio.UpdateVceDatCalculo(dtoActualizar);
                    }
                }

            }

            //- Llenar MedEnergiaGrupo
            servicio.LlenarMedeneriaGrupo(pecacodi);

            //- Llenar los Costos Variables
            servicio.LlenarCostosVariables(pecacodi);

            //- Actualizar el LogCarga
            servicio.SaveVceLogCargaDet(id, User.Identity.Name, "VCE_DATCALCULO", pecacodi);

            Log.Debug("FIN --> PROCESO DE GENERACION DE PARAMETROS");

            return Json(model);
        }

        //- compensaciones.HDT - Inicio 03/03/2017: : Cambio para atender el requerimiento.
        //- Remover en proxima revision: SI
        //public JsonResult obtenerDataCalculo(int id, int pecacodi)
        //{
        //    CostosVariablesGeneralModel model = new CostosVariablesGeneralModel();

        //    //Eliminamos los registros
        //    servicio.DeleteCalculo(pecacodi);

        //    //Migramos la data
        //    servicio.SaveCalculo(pecacodi);

        //    //Llenar MedEnergiaGrupo
        //    servicio.LlenarMedeneriaGrupo(pecacodi);

        //    //Llenar los Costos Variables
        //    servicio.LlenarCostosVariables(pecacodi);

        //    //Actualizar el LogCarga
        //    //servicio.ActualizarLogCarga("VCE_DATCALCULO", pecacodi);
        //    servicio.SaveVceLogCargaDet(id, User.Identity.Name, "VCE_DATCALCULO", pecacodi);

        //    return Json(model);
        //}
        //- HDT Fin

        /// <summary>
        /// Muestra la pantalla Arranques y Paradas
        /// </summary>
        /// <returns></returns>
        public ActionResult ArranquesParadas()
        {
            ArranquesParadasGeneralModel model = new ArranquesParadasGeneralModel();
            model.ListTrnPeriodo = servicio.ListarPeriodosTC();
            model.ListSiEmpresa = servicio.ListaEmpresasCompensacion();

            int periodo = model.ListTrnPeriodo[0].PeriCodi;
            model.ListEveSubcausaevento = servicio.ListTipoOperacion(periodo);
            return View(model);
        }

        /// <summary>
        /// Lista de Arranques y Paradas
        /// </summary>
        /// <param name="pecacodi"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult ListarArranquesParadas(int pecacodi, string empresa, string central, string grupo, string modo)
        {
            ArranquesParadasGeneralModel model = new ArranquesParadasGeneralModel();

            List<StructData> lstCabecera = new List<StructData>();
            List<string> lstCuerpo = new List<string>();

            IDataReader dr = servicio.ListCompensacionArrPar(pecacodi, empresa, central, grupo, modo);

            int cantColCab = 0;
            int cantColCabAgrupado = 0;
            int cantColIdCero = 0;

            using (dr)
            {
                //CABECERA
                for (int i = 0; i < dr.FieldCount; i++)
                {
                    StructData data = new StructData();
                    data.indice = i;
                    data.nombre = dr.GetName(i).Trim();
                    data.tipo = dr.GetDataTypeName(i).Trim();
                    lstCabecera.Add(data);

                }
                // cantidad de campos de cabecera
                cantColCab = lstCabecera.Count;

                //CUERPO
                while (dr.Read())
                {
                    string linea = "";
                    foreach (StructData item in lstCabecera)
                    {
                        var valor = "";
                        if (item.tipo.Contains("Double") || item.tipo.Contains("Float") || item.tipo.Contains("Decimal"))
                        {
                            valor = string.Format("{0:0.##}", dr[item.indice]);
                        }
                        else
                        {
                            valor = dr[item.indice].ToString();
                        }

                        if (valor != null)
                        {
                            if (linea.Equals(""))
                            {
                                linea = linea + valor;
                            }
                            else
                            {
                                linea = linea + "|" + valor;
                            }
                        }
                    }
                    lstCuerpo.Add(linea);
                }
            }
            List<ComboCompensaciones> ListCabAgrupado = new List<ComboCompensaciones>();
            IDataReader drCab = servicio.ListCabCompensacionArrPar(pecacodi);
            using (drCab)
            {
                while (drCab.Read())
                {
                    ComboCompensaciones combo = new ComboCompensaciones();
                    combo.name = drCab["TITULO"].ToString();
                    combo.id = drCab["COLSPAN"].ToString();
                    ListCabAgrupado.Add(combo);
                    cantColCabAgrupado = cantColCabAgrupado + int.Parse(combo.id);

                    if (String.IsNullOrEmpty(combo.id) || combo.id == "0")
                    {
                        cantColIdCero = cantColIdCero + 1;
                    }
                }
            }

            // validar calculo de la version
            if (cantColIdCero > 0)
            {
                model.mensaje = "No se realizado el calculo de Version";
                model.ListCabAgrupada = null;
            }
            else
            {
                model.mensaje = "";
                model.ListCabAgrupada = ListCabAgrupado;
            }


            model.ListBodyArranquesParadas = lstCuerpo;
            model.ListCabArranquesParadas = lstCabecera;

            return PartialView("ListarArranquesParadas", model);
        }


        /// <summary>
        /// Método para exportar en Excel Reporte de Cumplimiento
        /// </summary>
        /// <param name="pecacodi"></param>
        /// <param name="tipo"></param>
        /// <returns></returns>
        public JsonResult Exportar(int pecacodi, string tipo)
        {
            try
            {
                string file = "";
                if (tipo.Equals("EN"))
                {
                    file = this.servicio.GenerarFormatoEnergia(pecacodi, AppDomain.CurrentDomain.BaseDirectory + Constantes.RutaCarga);
                }
                else if (tipo.Equals("CV"))
                {
                    file = this.servicio.GenerarFormatoCostosVariables(pecacodi, AppDomain.CurrentDomain.BaseDirectory + Constantes.RutaCarga);
                }
                return Json(file);
            }
            catch(Exception ex)
            {
                Log.Error("Exportar", ex);
                return Json(-1);
            }
        }

        public JsonResult ExportarCostosVariables(int pecacodi, string empresa, string central, string grupo, string modo)
        {
            try
            {
                string file = this.servicio.GenerarFormatoCostosVariablesFiltro(pecacodi, empresa, central, grupo, modo, AppDomain.CurrentDomain.BaseDirectory + Constantes.RutaCarga);

                return Json(file);
            }
            catch(Exception ex)
            {
                Log.Error("ExportarCostosVariables", ex);
                return Json(-1);
            }
        }

        public JsonResult ExportarHorasOperacion(int pecacodi, string empresa, string central, string grupo, string modo, string tipo, string fecIni, string fecFin, string arranque, string parada)
        {
            try
            {
                string file = this.servicio.GenerarFormatoHorasOperacion(pecacodi, empresa, central, grupo, modo, tipo, fecIni, fecFin, arranque, parada, AppDomain.CurrentDomain.BaseDirectory + Constantes.RutaCarga);

                return Json(file);
            }
            catch(Exception ex)
            {
                Log.Error("ExportarHorasOperacion", ex);
                return Json(-1);
            }
        }

        // DSH 20-04-2017 : Agregar metodo 'ExportarArranquesParadas'
        public JsonResult ExportarArranquesParadas(int pecacodi, string empresa, string central, string grupo, string modo)
        {
            try
            {
                string file = this.servicio.GenerarFormatoArranquesParadas(pecacodi, empresa, central, grupo, modo, AppDomain.CurrentDomain.BaseDirectory + Constantes.RutaCarga);

                return Json(file);
            }
            catch(Exception ex)
            {
                Log.Error("ExportarArranquesParadas", ex);
                return Json(-1);
            }
        }

        // DSH 20-06-2017 : Agregar metodo 'ExportarGrillaPtoGrupo'
        public JsonResult ExportarGrillaPtoGrupo(int pecacodi)
        {
            try
            {
                string file = this.servicio.GenerarFormatoPtoGrupo(pecacodi, AppDomain.CurrentDomain.BaseDirectory + Constantes.RutaCarga);

                return Json(file);
            }
            catch(Exception ex)
            {
                Log.Error("ExportarGrillaPtoGrupo", ex);
                return Json(-1);
            }
        }
        /// <summary>
        /// Descarga el formato
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public virtual ActionResult Descargar(int formato, string file)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + Constantes.RutaCarga + file;
            string app = (formato == 1) ? Constantes.AppExcel : (formato == 2) ? Constantes.AppPdf : Constantes.AppWord;

            return File(path, app, file);
        }

        /// <summary>
        /// Muestra la pantalla Costos Marginales
        /// </summary>
        /// <returns></returns>
        public ActionResult GrillaPtoGrupo()
        {
            EditorPtoGrupoGeneralModel model = new EditorPtoGrupoGeneralModel();
            model.ListTrnPeriodo = servicio.ListarPeriodosTC();
            return View(model);
        }

        /// <summary>
        /// Muestra la lista de los Costos Marginales
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult ListarGrillaPtoGrupo(int pecacodi)
        {
            EditorPtoGrupoGeneralModel model = new EditorPtoGrupoGeneralModel();
            model.VcePeriodoCalculoDTO = this.servicio.getVersionPeriodoById(pecacodi);
            //VcePtomedModopeDTO entity = new VcePtomedModopeDTO();
            //model.VcePeriodoCalculoDTO.PecaCodi = pecacodi;

            model.ListGrillaHead = servicio.lstGrillaHead(pecacodi);
            model.ListGrillaBody = servicio.lstGrillaBody(pecacodi);
            return PartialView("ListarGrillaPtoGrupo", model);
        }

        [HttpGet]
        public ActionResult EditMesValorizacion(int periodo)
        {
            MesValorizacionGeneralModel model = new MesValorizacionGeneralModel();

            model.TrnPeriodoDTO = this.servicio.getPeriodoById(periodo);
            model.ListVcePeriodoCalculo = this.servicio.getVersionesPeridoByIdPeriodo(periodo);
            model.ListTrnCostoMarginal = this.servicio.ListCostoMarginalVersion(periodo);
           
            return View("EditMesValorizacion", model);
        }

        //- compensaciones.HDT - 21/03/2017: Cambio para atender el requerimiento. 
        [HttpGet]
        public ActionResult IncrementosReducciones(int periodo)
        {
            IncrementosReduccionesModel model = new IncrementosReduccionesModel();
            model.VcePeriodoCalculoDTO = this.servicio.getVersionPeriodoById(periodo);

            int pericodi = model.VcePeriodoCalculoDTO.PeriCodi;

            model.TrnPeriodoDTO = this.servicio.getPeriodoById(pericodi);
            model.ListPrGrupodatDTO = this.servicio.getModosOperacion("2");
            model.VceArrparIncredGenDTO = new VceArrparIncredGenDTO();
            model.VceArrparIncredGenDTO.Apinrefecha = DateTime.Now;

            return View(model);
        }

        //- compensaciones.HDT - 21/03/2017: Cambio para atender el requerimiento. 
        [HttpPost]
        public PartialViewResult ListarIncrementosReducciones(int periodo)
        {
            IncrementosReduccionesModel model = new IncrementosReduccionesModel();

            model.ListVceArrparIncredGenDTO = this.servicio.getListVceArrparIncred(periodo);

            return PartialView("listarIncrementosReducciones", model);
        }

        //- compensaciones.HDT - 23/03/2017: Cambio para atender el requerimiento. 
        public JsonResult ObtenerIncrementoReduccion(int pecacodi, int GrupoCodi, string ApinrefechaDesc)
        {

            VceArrparIncredGenDTO oVceArrparIncredGenDTO
                = servicio.ObtenerIncrementoReduccion(pecacodi, GrupoCodi, ApinrefechaDesc);

            var jsonSerialiser = new JavaScriptSerializer();
            return Json(jsonSerialiser.Serialize(oVceArrparIncredGenDTO));
        }

        //- compensaciones.HDT - 23/03/2017: Cambio para atender el requerimiento. 
        public ActionResult GuardarIncrementoReduccion(int pecacodi, int Grupocodi, string ApinrefechaDesc
                                                     , int Apinrenuminc, int Apinrenumdis, bool EsNuevo)
        {
            IncrementosReduccionesModel model = new IncrementosReduccionesModel();

            VceArrparIncredGenDTO oVceArrparIncredGenDTO = servicio.ObtenerIncrementoReduccion(pecacodi, Grupocodi, ApinrefechaDesc);

            if (EsNuevo)
            {
                if (oVceArrparIncredGenDTO != null)
                {
                    //- Ya existe un registro igual, no se puede Crear.
                    return Json(new { success = false, message = "Ya existe un registro igual, no se puede crear" });
                }

                oVceArrparIncredGenDTO = new VceArrparIncredGenDTO();
                oVceArrparIncredGenDTO.PecaCodi = pecacodi;
                oVceArrparIncredGenDTO.Grupocodi = Grupocodi;
                oVceArrparIncredGenDTO.Apinrefecha = DateTime.ParseExact(ApinrefechaDesc, "dd/MM/yyyy", null);
                oVceArrparIncredGenDTO.Apinrenuminc = Apinrenuminc;
                oVceArrparIncredGenDTO.Apinrenumdis = Apinrenumdis;
                oVceArrparIncredGenDTO.Apinreusucreacion = User.Identity.Name;
                oVceArrparIncredGenDTO.Apinrefeccreacion = DateTime.Now;

                this.servicio.CrearIncrementoReduccion(oVceArrparIncredGenDTO);
            }
            else
            {
                if (oVceArrparIncredGenDTO == null)
                {
                    //- El registro ya no existe, no se puede actualizar.
                    return Json(new { success = false, message = "El registro ya no existe, no se puede actualizar" });
                }

                oVceArrparIncredGenDTO.Apinrenuminc = Apinrenuminc;
                oVceArrparIncredGenDTO.Apinrenumdis = Apinrenumdis;
                oVceArrparIncredGenDTO.Apinreusumodificacion = User.Identity.Name;
                oVceArrparIncredGenDTO.Apinrefecmodificacion = DateTime.Now;

                this.servicio.GuardarIncrementoReduccion(oVceArrparIncredGenDTO);
            }

            return Json(new { success = true, message = "Ok" });
        }

        //- compensaciones.HDT - 23/03/2017: Cambio para atender el requerimiento. 
        public ActionResult EliminarIncrementoReduccion(int pecacodi, int Grupocodi, string ApinrefechaDesc)
        {
            IncrementosReduccionesModel model = new IncrementosReduccionesModel();

            VceArrparIncredGenDTO oVceArrparIncredGenDTO = servicio.ObtenerIncrementoReduccion(pecacodi, Grupocodi, ApinrefechaDesc);

            if (oVceArrparIncredGenDTO == null)
            {
                //- El registro ya no existe, no se puede actualizar.
                return Json(new { success = false, message = "El registro ya no existe, no se puede eliminar" });
            }

            this.servicio.EliminarIncrementoReduccion(oVceArrparIncredGenDTO);

            return Json(new { success = true, message = "Ok" });
        }

        //- compensaciones.HDT - 29/03/2017: Cambio para atender el requerimiento. 
        [HttpGet]
        public ActionResult RegistroCalculoManual(int periodo)
        {
            RegistroCalculoManualModel model = new RegistroCalculoManualModel();

            model.VcePeriodoCalculoDTO = this.servicio.getVersionPeriodoById(periodo);

            int pericodi = model.VcePeriodoCalculoDTO.PeriCodi;

            model.TrnPeriodoDTO = this.servicio.getPeriodoById(pericodi);
            model.ListPrGrupodatDTO = this.servicio.getModosOperacion("2/9");
            model.VceArrparGrupoCabDTO = new VceArrparGrupoCabDTO();

            return View(model);
        }

        //- compensaciones.HDT - 29/03/2017: Cambio para atender el requerimiento. 
        [HttpPost]
        public PartialViewResult ListarCalculosManuales(int periodo)
        {
            RegistroCalculoManualModel model = new RegistroCalculoManualModel();

            model.ListVceArrparGrupoCabDTO = this.servicio.getListVceArrparGrupoCabDTO(periodo);

            return PartialView("listarCalculosManuales", model);
        }

        //- compensaciones.HDT - 29/03/2017: Cambio para atender el requerimiento. 
        public JsonResult ObtenerCalculoManual(int pecacodi, int GrupoCodi, string Apgcfccodi)
        {

            VceArrparGrupoCabDTO oVceArrparGrupoCabDTO
                = servicio.ObtenerCalculoManual(pecacodi, GrupoCodi, Apgcfccodi);

            var jsonSerialiser = new JavaScriptSerializer();
            return Json(jsonSerialiser.Serialize(oVceArrparGrupoCabDTO));
        }

        //- compensaciones.HDT - 29/03/2017: Cambio para atender el requerimiento. 
        public ActionResult GuardarCalculoManual(int pecacodi, int Grupocodi, string Apgcfccodi
                                                     , decimal Apgcabccbef, bool EsNuevo)
        {
            RegistroCalculoManualModel model = new RegistroCalculoManualModel();

            VceArrparGrupoCabDTO oVceArrparGrupoCabDTO = servicio.ObtenerCalculoManual(pecacodi, Grupocodi, Apgcfccodi);

            if (EsNuevo)
            {
                if (oVceArrparGrupoCabDTO != null)
                {
                    //- Ya existe un registro igual, se procede a eliminar para crear otro registro nuevo.
                    this.EliminarCalculoManual(pecacodi, Grupocodi, Apgcfccodi);

                }

                oVceArrparGrupoCabDTO = new VceArrparGrupoCabDTO();
                oVceArrparGrupoCabDTO.PecaCodi = pecacodi;
                oVceArrparGrupoCabDTO.Grupocodi = Grupocodi;
                oVceArrparGrupoCabDTO.Apgcfccodi = "G1";
                oVceArrparGrupoCabDTO.Apgcabccbef = Apgcabccbef;
                oVceArrparGrupoCabDTO.Apgcabccmarr = 0;
                oVceArrparGrupoCabDTO.Apgcabccadic = 0;
                oVceArrparGrupoCabDTO.Apgcabflagcalcmanual = "1";

                this.servicio.CrearCalculoManual(oVceArrparGrupoCabDTO);
            }
            else
            {
                if (oVceArrparGrupoCabDTO == null)
                {
                    //- El registro ya no existe, no se puede actualizar.
                    return Json(new { success = false, message = "El registro ya no existe, no se puede actualizar" });
                }

                oVceArrparGrupoCabDTO.Apgcabccbef = Apgcabccbef;

                this.servicio.ActualizarCalculoManual(oVceArrparGrupoCabDTO);
            }

            return Json(new { success = true, message = "Ok" });
        }

        //- compensaciones.HDT - 29/03/2017: Cambio para atender el requerimiento. 
        public ActionResult EliminarCalculoManual(int pecacodi, int Grupocodi, string Apgcfccodi)
        {
            RegistroCalculoManualModel model = new RegistroCalculoManualModel();

            VceArrparGrupoCabDTO oVceArrparGrupoCabDTO = servicio.ObtenerCalculoManual(pecacodi, Grupocodi, Apgcfccodi);

            if (oVceArrparGrupoCabDTO == null)
            {
                //- El registro ya no existe, no se puede actualizar.
                return Json(new { success = false, message = "El registro ya no existe, no se puede eliminar" });
            }

            this.servicio.EliminarCalculoManual(oVceArrparGrupoCabDTO);

            return Json(new { success = true, message = "Ok" });
        }

        //- compensaciones.HDT - 29/03/2017: Cambio para atender el requerimiento. 
        // DSH 26-04-2017 : cambios por requerimiento
        [HttpGet]
        public ActionResult CompensacionEspecialIlo2(int periodo)
        {
            CompensacionEspecialIlo2GeneralModel model = new CompensacionEspecialIlo2GeneralModel();
            model.VcePeriodoCalculoDTO = this.servicio.getVersionPeriodoById(periodo);

            int pericodi = model.VcePeriodoCalculoDTO.PeriCodi;

            model.TrnPeriodoDTO = this.servicio.getPeriodoById(pericodi);
            model.ListPrGrupodatDTO = this.servicio.getModosOperacion("2/9");
            model.VceArrparCompEspDTO = new VceArrparCompEspDTO();
            model.ListVceArrparTipoOperaDTO = new List<VceArrparTipoOperaDTO>();

            return View(model);
        }

        //- compensaciones.HDT - 29/03/2017: Cambio para atender el requerimiento. 
        [HttpPost]
        public PartialViewResult ListarCompensacionesEspecialesIlo2(int periodo)
        {
            CompensacionEspecialIlo2GeneralModel model = new CompensacionEspecialIlo2GeneralModel();

            model.ListVceArrparCompEspDTO = this.servicio.getListVceArrparCompEspDTO(periodo);

            return PartialView("listarCompensacionesEspecialesIlo2", model);
        }

        //- compensaciones.HDT - 06/04/2017: Cambio para atender el requerimiento. 
        public ActionResult EliminarCompensacionEspecialIlo2(int pecacodi, int grupoCodi, string apespFechadesc, string apstoCodi)
        {
            CompensacionEspecialIlo2GeneralModel model = new CompensacionEspecialIlo2GeneralModel();

            VceArrparCompEspDTO oVceArrparCompEspDTO = servicio.getVceArrparCompEsp(pecacodi, grupoCodi, apespFechadesc, apstoCodi);

            if (oVceArrparCompEspDTO == null)
            {
                //- El registro ya no existe, no se puede actualizar.
                return Json(new { success = false, message = "El registro ya no existe, no se puede eliminar" });
            }

            this.servicio.eliminarVceArrparCompEsp(oVceArrparCompEspDTO);

            return Json(new { success = true, message = "Ok" });
        }

        //- compensaciones.HDT - 29/03/2017: Cambio para atender el requerimiento. 
        [HttpPost]
        public JsonResult ObtenerListaTipoOpera(string tipo)
        {
            List<VceArrparTipoOperaDTO> ListVceArrparTipoOperaDTO = servicio.getTiposOperacion(tipo);

            List<ComboCompensaciones> listaCombo = new List<ComboCompensaciones>();

            ComboCompensaciones entity = new ComboCompensaciones();
            entity.id = string.Empty;
            entity.name = " -- SELECCIONE -- ";
            listaCombo.Add(entity);
            foreach (var item in ListVceArrparTipoOperaDTO)
            {
                entity = new ComboCompensaciones();
                entity.name = item.Apstonombre.Trim();
                entity.id = item.Apstocodi.ToString();
                listaCombo.Add(entity);
            }

            var jsonSerialiser = new JavaScriptSerializer();
            return Json(jsonSerialiser.Serialize(listaCombo));
        }

        //- compensaciones.HDT - 29/03/2017: Cambio para atender el requerimiento. 
        // DSH 16-05-2017 : Se realizo cambios por requerimiento
        public ActionResult GuardarCompensacionEspecialIlo2(int pecacodi, int grupoCodi, string apespFechaDesc
                                                          , string apespTipo, string aptopSubtipo, decimal apespCargaFinal)
        {
            string mensaje = string.Empty;
            bool exito = false;

            //- 1. Obtener tipo de cambio.
            try
            {
                //PeriodoDTO oTrnPeriodoDTO = this.servicio.getPeriodoById(pecacodi);
                VcePeriodoCalculoDTO oTrnPeriodoDTO = this.servicio.getVersionPeriodoById(pecacodi);

                VceArrparRampaCfgDTO registroInferior = null;
                VceArrparRampaCfgDTO registroSuperior = null;

                if (apespTipo == "P")
                {
                    registroInferior = this.servicio.obtenerRangoInferiorPar(grupoCodi, aptopSubtipo, apespCargaFinal);
                    registroSuperior = this.servicio.obtenerRangoSuperiorPar(grupoCodi, aptopSubtipo, apespCargaFinal);
                }
                else
                {
                    registroInferior = this.servicio.obtenerRangoInferiorArr(grupoCodi, aptopSubtipo, apespCargaFinal);
                    registroSuperior = this.servicio.obtenerRangoSuperiorArr(grupoCodi, aptopSubtipo, apespCargaFinal);
                }

                decimal apramHorasAcum = 0;
                decimal apramPotenciaBruta = apespCargaFinal;
                decimal apramEnergiaAcum = 0;

                decimal apramConsumoAcumd2 = 0;
                decimal apramConsumoAcumCarb = 0;

                decimal rendVig = 0;
                decimal precioCombAlt = 0;
                decimal precioComb = 0;

                decimal consumoCombBaseAlt = 0;
                decimal consumoCombRecTransfAlt = 0;
                decimal consumoCombRecTransf = 0;
                decimal consumoCombBase = 0;

                decimal compensacion = 0;

                DateTime apespFecha = DateTime.ParseExact(apespFechaDesc, "dd/MM/yyyy", null);

                if (registroInferior != null && registroSuperior != null)
                {
                    apramHorasAcum = (registroSuperior.Apramhorasacum.Value - registroInferior.Apramhorasacum.Value)
                                     /
                                     (registroSuperior.Aprampotenciabruta.Value - registroInferior.Aprampotenciabruta.Value)
                                      *
                                     (apramPotenciaBruta - registroInferior.Aprampotenciabruta.Value)
                                     + registroInferior.Apramhorasacum.Value;

                    if (apespTipo == "P")
                    {
                        apramEnergiaAcum = ((apramPotenciaBruta + registroSuperior.Aprampotenciabruta.Value) / 2)
                                           *
                                           (registroSuperior.Apramhorasacum.Value - apramHorasAcum)
                                           + registroSuperior.Apramenergiaacum.Value;
                    }
                    else
                    {
                        apramEnergiaAcum = ((apramPotenciaBruta + registroInferior.Aprampotenciabruta.Value) / 2)
                                           *
                                           (apramHorasAcum - registroInferior.Apramhorasacum.Value)
                                           + registroInferior.Apramenergiaacum.Value;
                    }

                    apramConsumoAcumd2 = (registroSuperior.Apramconsumoacumd2.Value - registroInferior.Apramconsumoacumd2.Value)
                                         /
                                         (registroSuperior.Aprampotenciabruta.Value - registroInferior.Aprampotenciabruta.Value)
                                         *
                                         (apramPotenciaBruta - registroInferior.Aprampotenciabruta.Value)
                                         + registroInferior.Apramconsumoacumd2.Value;

                    apramConsumoAcumCarb = (registroSuperior.Apramconsumoacumcarb.Value - registroInferior.Apramconsumoacumcarb.Value)
                                           /
                                           (registroSuperior.Aprampotenciabruta.Value - registroInferior.Aprampotenciabruta.Value)
                                           * (apramPotenciaBruta - registroInferior.Aprampotenciabruta.Value)
                                           + registroInferior.Apramconsumoacumcarb.Value;

                    //- Energía Producida REG_CALC.APRAMENERGIAACUM
                    //- ===========================================
                    List<int> lIdGrupos = null;
                    List<PrGrupodatDTO> lGrupos = null;

                    lIdGrupos = new List<int>();
                    lIdGrupos.Add(grupoCodi);

                    //- Calcular rendVig
                    lGrupos = servicio.ObtenerResultadoFormula(lIdGrupos, constRendimientoVigente, apespFecha, oTrnPeriodoDTO.PecaTipoCambio.ToString());
                    rendVig = lGrupos[0].Valor;

                    //- Calcular precioCombAlt
                    lGrupos = servicio.ObtenerResultadoFormula(lIdGrupos, constPrecioCombAlt, apespFecha, oTrnPeriodoDTO.PecaTipoCambio.ToString());
                    precioCombAlt = lGrupos[0].Valor;

                    //- Calcular precioComb
                    lGrupos = servicio.ObtenerResultadoFormula(lIdGrupos, constPrecioComb, apespFecha, oTrnPeriodoDTO.PecaTipoCambio.ToString());
                    precioComb = lGrupos[0].Valor * 1000;

                    //- Calcular consumoCombBaseAlt

                    int conceptoConsumoComb = servicio.getConceptoVceArrparTipoOpera(apespTipo, aptopSubtipo);
                    lGrupos = servicio.ObtenerResultadoFormula(lIdGrupos, conceptoConsumoComb, apespFecha, oTrnPeriodoDTO.PecaTipoCambio.ToString());
                    consumoCombBaseAlt = lGrupos[0].Valor * 1000;

                    consumoCombBase = 0;


                    consumoCombRecTransfAlt = 0;
                    if (rendVig != 0)
                    {
                        consumoCombRecTransf = apramEnergiaAcum / rendVig;
                    }

                    compensacion = (
                                        (consumoCombBaseAlt + apramConsumoAcumd2 - consumoCombRecTransfAlt)
                                        *
                                        precioCombAlt
                                    )
                                   +
                                   (
                                       (consumoCombBase + apramConsumoAcumCarb - consumoCombRecTransf)
                                       *
                                       precioComb
                                    );

                }

                VceArrparCompEspDTO oVceArrparCompEspDTO = this.servicio.getVceArrparCompEsp(pecacodi, grupoCodi, apespFechaDesc, aptopSubtipo);
                if (oVceArrparCompEspDTO != null)
                {
                    //- Se procede a eliminar:
                    this.servicio.eliminarVceArrparCompEsp(oVceArrparCompEspDTO);
                }

                VceArrparCompEspDTO oVceArrparCompEspDTONuevo = new VceArrparCompEspDTO();
                oVceArrparCompEspDTONuevo.PecaCodi = pecacodi;
                oVceArrparCompEspDTONuevo.Grupocodi = grupoCodi;
                oVceArrparCompEspDTONuevo.Apespfecha = apespFecha;
                //oVceArrparCompEspDTONuevo.Apesptipo = apespTipo;
                oVceArrparCompEspDTONuevo.Apstocodi = aptopSubtipo;
                oVceArrparCompEspDTONuevo.Apespcargafinal = apespCargaFinal;
                oVceArrparCompEspDTONuevo.Apespenergprod = apramEnergiaAcum;
                oVceArrparCompEspDTONuevo.Apesprendvigente = rendVig;
                oVceArrparCompEspDTONuevo.Apesppreciocomb = precioComb;
                oVceArrparCompEspDTONuevo.Apespcombbase = consumoCombBase;
                oVceArrparCompEspDTONuevo.Apespcombrampa = apramConsumoAcumCarb;
                oVceArrparCompEspDTONuevo.Apespcombreconocxtransf = consumoCombRecTransf;
                oVceArrparCompEspDTONuevo.Apespcombreconocxtransf = consumoCombRecTransf;
                oVceArrparCompEspDTONuevo.Apesppreciocombalt = precioCombAlt;
                oVceArrparCompEspDTONuevo.Apespcombbasealt = consumoCombBaseAlt;
                oVceArrparCompEspDTONuevo.Apespcombrampaalt = apramConsumoAcumd2;
                oVceArrparCompEspDTONuevo.Apespcombreconocxtransfalt = consumoCombRecTransfAlt;
                oVceArrparCompEspDTONuevo.Apespcompensacion = compensacion;

                this.servicio.crearVceArrparCompEsp(oVceArrparCompEspDTONuevo);

                exito = true;
                mensaje = "Ok";
            }
            catch (Exception ex)
            {
                Log.Error("GuardarCompensacionEspecialIlo2", ex);
                exito = false;
                mensaje = "Ocurrió un error al guardar la compensación especial Ilo 2. Por favor consulte con el Administrador del Sistema";
                throw;
            }

            //- 2. 
            return Json(new { success = exito, message = mensaje });
        }

        [HttpGet]
        public ActionResult EditPtoGrupo(int pecacodi, int id)
        {
            EditorPtoGrupoGeneralModel model = new EditorPtoGrupoGeneralModel();

            model.VcePeriodoCalculoDTO = this.servicio.getVersionPeriodoById(pecacodi);
            int pericodi = model.VcePeriodoCalculoDTO.PeriCodi;

            model.TrnPeriodoDTO = this.servicio.getPeriodoById(pericodi);

            MePtomedicionDTO entity = new MePtomedicionDTO();
            entity.Ptomedicodi = id;

            model.mePtomedicion = entity;

            model.ListMePtomedicion = this.servicio.ListPtoMedicionCompensaciones(id, pecacodi);
            model.ListPrGrupodat = this.servicio.ListaModosOperacion(id, pecacodi);

            return View("EditPtoGrupo", model);
        }

        [HttpPost]
        public PartialViewResult ListarModosOperacion(int pecacodi, int ptoMediCodi)
        {
            EditorPtoGrupoGeneralModel model = new EditorPtoGrupoGeneralModel();
            model.ListVcePtomedModope = this.servicio.ListVcePtomedModope(pecacodi, ptoMediCodi);
            return PartialView("ListarModosOperacion", model);
        }

        [HttpPost]
        public JsonResult CrearMesValorizacion(int pericodi, string pecanombre, decimal tipocambio, int version, string motivo)
        {
            MesValorizacionGeneralModel model = new MesValorizacionGeneralModel();

            VcePeriodoCalculoDTO entity = new VcePeriodoCalculoDTO();

            List<VcePeriodoCalculoDTO> listCalculos = this.servicio.getVersionesPeridoByIdPeriodo(pericodi);

            entity.PecaNombre = pecanombre;
            entity.PecaTipoCambio = tipocambio;
            entity.PecaVersionVtea = version;

            if (listCalculos.Count() == 0)
            {
                //entity.PecaNombre = "Mensual";
                //entity.PecaVersionVtea = 1;
                entity.PecaVersionComp = 1;
            }
            else
            {
                //entity.PecaNombre = "Revisión " + listCalculos.Count().ToString().PadLeft(2, '0');
                //entity.PecaTipoCambio = listCalculos[0].PecaTipoCambio;
                //entity.PecaVersionVtea = listCalculos[0].PecaVersionVtea;
                entity.PecaVersionComp = listCalculos.Count() + 1;
            }
            entity.PeriCodi = pericodi;
            entity.PecaEstRegistro = "1";
            entity.PecaUsuCreacion = User.Identity.Name;
            entity.PecaMotivo = motivo;

            this.servicio.SaveVcePeriodoCalculo(entity);

            InicializaDatosByVersion(entity.PecaCodi);

            this.servicio.InitMesValorizacion(entity.PecaCodi);

            model.VcePeriodoCalculoDTO = entity;
            model.ListVcePeriodoCalculo = this.servicio.getVersionesPeridoByIdPeriodo(pericodi);

            return Json(model);
        }

        [HttpPost]
        public PartialViewResult InicializarMesValorizacion(int pecacodi)
        {
            MesValorizacionGeneralModel model = new MesValorizacionGeneralModel();

            InicializaDatosByVersion(pecacodi);

            //this.servicio.InitMesValorizacion(pecacodi);

            model.VcePeriodoCalculoDTO = this.servicio.getVersionPeriodoById(pecacodi);
            model.ListEntidades = this.servicio.ListEntidades(pecacodi);

            return PartialView("ListarEntidades", model);

        }

        public void InicializaDatosByVersion(int pecacodi)
        {
            VcePeriodoCalculoDTO entity = this.servicio.getVersionPeriodoById(pecacodi);

            int pecacodiOrigen;

            if (entity != null)
            {
                // obtener 
                // eliminar datos de calculos
                this.servicio.DeleteCompRegularDetByVersion(pecacodi);
                this.servicio.DeleteCompBajaeficByVersion(pecacodi);
                this.servicio.DeleteVceArrparGrupoCabByVersion(pecacodi);
                this.servicio.DeleteVceCostoVariableByVersion(pecacodi);
                this.servicio.DeleteCalculo(pecacodi);

                // eliminar datos de origen
                this.servicio.DeleteVceEnergiaByVersion(pecacodi);
                this.servicio.DeleteHoraOperacion(pecacodi);

                // eliminar datos de configuracion
                this.servicio.DeleteVcePtomedModopeByVersion(pecacodi);

                // eliminar datos de textos de reportes
                this.servicio.DeleteVceTextoReporteByVersion(pecacodi);

                // Verifica si la version de compensacion = 1
                if (entity.PecaVersionComp == 1)
                {
                    // obtener el ultimo pecacodi del periodo anterior
                    pecacodiOrigen = this.servicio.GetIdAnteriorConfig(entity.PeriCodi);

                    if (pecacodiOrigen > 0)
                    {
                        // Obtener datos de configuracion y Grabar
                        this.servicio.SaveVcePtomedModopeFromOtherversion(pecacodi, pecacodiOrigen, User.Identity.Name);

                        // Obtener datos de textos de reportes y Grabar
                        this.servicio.SaveVceTextoReporteFromOtherVersion(pecacodi, pecacodiOrigen, User.Identity.Name);
                    }

                }
                else
                {
                    // obtener el pecacodi anterior 
                    pecacodiOrigen = this.servicio.GetIdAnteriorCalculo(pecacodi);

                    if (pecacodiOrigen > 0)
                    {
                        // Obtener datos de configuracion y Grabar
                        this.servicio.SaveVcePtomedModopeFromOtherversion(pecacodi, pecacodiOrigen, User.Identity.Name);

                        // Obtener datos de textos de reportes y Grabar
                        this.servicio.SaveVceTextoReporteFromOtherVersion(pecacodi, pecacodiOrigen, User.Identity.Name);

                        // Obtener datos de origen y Grabar
                        this.servicio.SaveVceEnergiaFromOtherversion(pecacodi, pecacodiOrigen, User.Identity.Name);
                        this.servicio.SaveVceHoraOperacionFromOtherVersion(pecacodi, pecacodiOrigen, User.Identity.Name);

                        // Obtener datos de calculo y grabar 
                        this.servicio.SaveVceCompBajaeficFromOtherVersion(pecacodi, pecacodiOrigen, User.Identity.Name);
                        this.servicio.SaveVceCompRegularDetFromOtherVersion(pecacodi, pecacodiOrigen, User.Identity.Name);
                        this.servicio.SaveVceArrparGrupoCabFromOtherversion(pecacodi, pecacodiOrigen, User.Identity.Name);
                        this.servicio.SaveVceCostoVariableFromOtherversion(pecacodi, pecacodiOrigen, User.Identity.Name);
                        this.servicio.SaveVceDatCalculoFromOtherversion(pecacodi, pecacodiOrigen, User.Identity.Name);
                    }

                }

            }
        }

        public JsonResult GrabarMesValorizacion(int pecacodi, String pecanombre, decimal tc, int version, string motivo)
        {
            MesValorizacionGeneralModel model = new MesValorizacionGeneralModel();

            VcePeriodoCalculoDTO entity = this.servicio.getVersionPeriodoById(pecacodi);

            //Validamos los datos
            if (entity.PecaVersionComp > version)
            {
                Mensaje obj = new Mensaje();
                obj.msg = "La versión de compensación no puede ser mayor que la Revisión de VTEA";
                return Json(obj);
            }

            // validar cambios
            if (entity.PecaTipoCambio != tc || entity.PecaVersionVtea != version)
            {
                // borrar calculos
                this.servicio.DeleteCompRegularDetByVersionCalculoAutomatico(pecacodi);
                this.servicio.DeleteCompBajaeficByVersionCalculoAutomatico(pecacodi);
                this.servicio.DeleteVceArrparGrupoCabByVersion(pecacodi);
                this.servicio.DeleteVceCostoVariableByVersion(pecacodi);
                this.servicio.DeleteCalculo(pecacodi);

            }
            entity.PecaTipoCambio = tc;
            entity.PecaVersionVtea = version;
            entity.PecaNombre = pecanombre;
            entity.PecaMotivo = motivo;
            this.servicio.UpdateVcePeriodoCalculo(entity);

            return Json(model);
        }

        public JsonResult GrabarEstadoValorizacion(int pecacodi, String estado)
        {
            MesValorizacionGeneralModel model = new MesValorizacionGeneralModel();

            VcePeriodoCalculoDTO entity = this.servicio.getVersionPeriodoById(pecacodi);

            //Validamos los datos
            /*if (entity.PecaVersionComp > version)
            {
                Mensaje obj = new Mensaje();
                obj.msg = "La versión de compensación no puede ser mayor que la Revisión de VTEA";
                return Json(obj);
            }*/

            entity.PecaEstRegistro = estado;
            this.servicio.UpdateVcePeriodoCalculo(entity);

            return Json(model);
        }

        public Int32 ObtenerNroCalculosActivosPeriodo(int pericodi)
        {


            Int32 nroPeriodos = this.servicio.ObtenerNroCalculosActivosPeriodo(pericodi);

            return nroPeriodos;
        }


        /// <summary>
        /// Muestra la lista de entidades
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult ListarEntidades(int pecacodi)
        {
            MesValorizacionGeneralModel model = new MesValorizacionGeneralModel();

            model.VcePeriodoCalculoDTO = this.servicio.getVersionPeriodoById(pecacodi);
            model.ListEntidades = this.servicio.ListEntidades(pecacodi);

            return PartialView("ListarEntidades", model);
        }

        [HttpPost]
        public JsonResult DeleteVcePtomedModope(int pecacodi, int ptomedicodi, int grupocodi)
        {
            EditorPtoGrupoGeneralModel model = new EditorPtoGrupoGeneralModel();

            servicio.DeleteVcePtomedModope(pecacodi, ptomedicodi, grupocodi);

            return Json(model);
        }

        [HttpPost]
        public JsonResult SaveVcePtomedModope(int pecacodi, int ptomedicodi, int grupocodi)
        {
            EditorPtoGrupoGeneralModel model = new EditorPtoGrupoGeneralModel();

            VcePtomedModopeDTO entity = new VcePtomedModopeDTO();
            entity.Pecacodi = pecacodi;
            entity.Ptomedicodi = ptomedicodi;
            entity.Grupocodi = grupocodi;
            entity.Pmemopestregistro = "1";
            entity.Pmemopusucreacion = User.Identity.Name;

            if (servicio.ValidarVcePtomedModope(pecacodi, ptomedicodi, grupocodi) == 0)
            {
                //Guardamos el registro
                servicio.SaveVcePtomedModope(entity);
                model.mensaje = "Registro agregado correctamente";
            }
            else
            {
                model.mensaje = "";
            }

            return Json(model);
        }


        [HttpPost]
        public JsonResult ObtenerListaCentral(int emprcodi)
        {
            List<PrGrupodatDTO> list = servicio.ListaCentral(emprcodi);
            List<ComboCompensaciones> listaCombo = new List<ComboCompensaciones>();

            ComboCompensaciones entity = new ComboCompensaciones();
            entity.id = "";
            entity.name = " -- SELECCIONE -- ";
            listaCombo.Add(entity);
            foreach (var item in list)
            {
                entity = new ComboCompensaciones();
                entity.name = item.GrupoNomb.Trim();
                entity.id = item.Grupocodi.ToString();
                listaCombo.Add(entity);
            }
            var jsonSerialiser = new JavaScriptSerializer();
            return Json(jsonSerialiser.Serialize(listaCombo));
        }

        // DSH 19-06-2017 Incio de Actualizacion
        [HttpPost]
        public JsonResult ObtenerListaGrupo(int emprcodi, int grupopadre)
        {
            List<PrGrupodatDTO> list = servicio.ListaGrupo(emprcodi, grupopadre);
            List<ComboCompensaciones> listaCombo = new List<ComboCompensaciones>();

            ComboCompensaciones entity = new ComboCompensaciones();
            entity.id = "";
            entity.name = " -- SELECCIONE -- ";
            listaCombo.Add(entity);
            foreach (var item in list)
            {
                entity = new ComboCompensaciones();
                entity.name = item.GrupoNomb.Trim();
                entity.id = item.Grupocodi.ToString();
                listaCombo.Add(entity);
            }
            var jsonSerialiser = new JavaScriptSerializer();
            return Json(jsonSerialiser.Serialize(listaCombo));
        }

        [HttpPost]
        public JsonResult ObtenerListaModo(int emprcodi, int grupopadre)
        {
            List<PrGrupodatDTO> list = servicio.ListaModo(emprcodi, grupopadre);
            List<ComboCompensaciones> listaCombo = new List<ComboCompensaciones>();

            ComboCompensaciones entity = new ComboCompensaciones();
            entity.id = "";
            entity.name = " -- SELECCIONE -- ";
            listaCombo.Add(entity);
            foreach (var item in list)
            {
                entity = new ComboCompensaciones();
                entity.name = item.GrupoNomb.Trim();
                entity.id = item.Grupocodi.ToString();
                listaCombo.Add(entity);
            }
            var jsonSerialiser = new JavaScriptSerializer();
            return Json(jsonSerialiser.Serialize(listaCombo));
        }
        // Fin de Actualizacion
        

        [HttpPost]
        public JsonResult ObtenerRegistroPeriodoCalculo(int pecacodi)
        {
            /*MesValorizacionGeneralModel model = new MesValorizacionGeneralModel();
            model.VcePeriodoCalculoDTO = servicio.getVersionPeriodoById(pecacodi);            
            model.ListEntidades = this.servicio.ListEntidades(pecacodi);            */
            //var jsonSerialiser = new JavaScriptSerializer();
            //return Json(jsonSerialiser.Serialize(periodoCalculo));

            VcePeriodoCalculoDTO model = this.servicio.getVersionPeriodoById(pecacodi);
            return Json(model);
        }


        [HttpPost]
        public JsonResult ObtenerPeriodoCalculo(int pericodi)
        {
            List<VcePeriodoCalculoDTO> list = servicio.getVersionesPeridoByIdPeriodo(pericodi);
            List<ComboCompensaciones> listaCombo = new List<ComboCompensaciones>();

            ComboCompensaciones entity = new ComboCompensaciones();
            entity.id = "";
            entity.name = " -- SELECCIONE -- ";
            listaCombo.Add(entity);
            foreach (var item in list)
            {
                entity = new ComboCompensaciones();
                entity.name = item.PecaNombre;
                entity.id = item.PecaCodi.ToString();
                listaCombo.Add(entity);
            }
            var jsonSerialiser = new JavaScriptSerializer();
            return Json(jsonSerialiser.Serialize(listaCombo));
        }

        /// <summary>
        /// Pantalla Inicial - CompensacionesRegulares
        /// </summary>
        /// <returns></returns>
        public ActionResult CompensacionesRegulares()
        {
            base.ValidarSesionUsuario();
            CompensacionesRegularesGeneralModel model = new CompensacionesRegularesGeneralModel();
            model.ListTrnPeriodo = servicio.ListarPeriodosTC();
            model.ListSiEmpresa = servicio.ListaEmpresasCompensacion();

            int periodo = model.ListTrnPeriodo[0].PeriCodi;
            model.ListEveSubcausaevento = servicio.ListTipoOperacion(periodo);

            return View(model);
        }

        public PartialViewResult ListarCompensacionesRegulares(int pecacodi, string empresa, string central, string grupo, string modo, string tipo, string fecIni, string fecFin)
        {
            string tipocalculo = "";
            CompensacionesRegularesGeneralModel model = new CompensacionesRegularesGeneralModel();
            model.ListCompensacionesRegulares = servicio.ListCompensacionesRegulares(pecacodi, empresa, central, grupo, modo, tipo, fecIni, fecFin, tipocalculo);
            return PartialView("ListarCompensacionesRegulares", model);
        }

        public ActionResult CompensacionesEspeciales()
        {
            base.ValidarSesionUsuario();
            CompensacionesEspecialesGeneralModel model = new CompensacionesEspecialesGeneralModel();
            model.ListTrnPeriodo = servicio.ListarPeriodosTC();
            model.ListSiEmpresa = servicio.ListaEmpresasCompensacion();

            return View(model);
        }

        public PartialViewResult ListarCompensacionesEspeciales(int pecacodi, string empresa, string central, string grupo, string modo)
        {
            CompensacionesEspecialesGeneralModel model = new CompensacionesEspecialesGeneralModel();
            model.ListCompensacionesEspeciales = servicio.ListCompensacionesEspeciales(pecacodi, empresa, central, grupo, modo);
            return PartialView("ListarCompensacionesEspeciales", model);
        }

        // Cambios DSH 30-03-2017 - ExportarCompensacionesRegulares
        public JsonResult ExportarCompensacionesRegulares(int pecacodi, string empresa, string central, string grupo, string modo, string tipo, string fecIni, string fecFin)
        {
            try
            {
                string tipocalculo = "";
                string file = this.servicio.GenerarFormatoCompensacionesRegulares(pecacodi, empresa, central, grupo, modo, tipo, fecIni, fecFin, tipocalculo, AppDomain.CurrentDomain.BaseDirectory + Constantes.RutaCarga);

                return Json(file);
            }
            catch(Exception ex)
            {
                Log.Error("ExportarCompensacionesRegulares", ex);
                return Json(-1);
            }
        }

        // Cambios DSH 30-03-2017 - ExportarCompensacionesEspeciales
        public JsonResult ExportarCompensacionesEspeciales(int pecacodi, string empresa, string central, string grupo, string modo)
        {
            try
            {
                string file = this.servicio.GenerarFormatoCompensacionesEspeciales(pecacodi, empresa, central, grupo, modo, AppDomain.CurrentDomain.BaseDirectory + Constantes.RutaCarga);

                return Json(file);
            }
            catch(Exception ex)
            {
                Log.Error("ExportarCompensacionesEspeciales", ex);
                return Json(-1);
            }
        }

        public ActionResult ExportacionDatosCalculo()
        {
            base.ValidarSesionUsuario();
            ExportacionDatosCalculoGeneralModel model = new ExportacionDatosCalculoGeneralModel();
            model.ListTrnPeriodo = servicio.ListarPeriodosTC();

            return View(model);
        }

        //public JsonResult ExportarDatosCalculo(int pecacodi, string cr, string ce, string dcr, string dp, string cv, string pg, string me, string cm, string ho)
        public JsonResult ExportarDatosCalculo(int pecacodi, string grupo, string lista)
        {
            try
            {
                string file = this.servicio.GenerarFormatoDatosCalculo(pecacodi, AppDomain.CurrentDomain.BaseDirectory + Constantes.RutaCarga, grupo, lista);
                return Json(file);
            }
            catch(Exception ex)
            {
                Log.Error("ExportarDatosCalculo", ex);
                return Json(-1);
            }
        }

        // DSH 22-06-2017 - Texto de Reportes : Se agrego por Requerimiento 

        public ActionResult TextoReporte()
        {
            base.ValidarSesionUsuario();
            TextoReporteGeneralModel model = new TextoReporteGeneralModel();
            model.ListTrnPeriodo = servicio.ListarPeriodosTC();

            return View(model);
        }
                
        public PartialViewResult ListarTextoReporte(int pecacodi)
        {
            TextoReporteGeneralModel model = new TextoReporteGeneralModel();
            model.VcePeriodoCalculoDTO = servicio.getVersionPeriodoById(pecacodi);

            model.ListTextoReporte = servicio.ListTextoReporte(pecacodi);
            
            return PartialView("ListarTextoReporte", model);
        }

        // DSH 23-06-2017 
        public JsonResult ObtenerTextoReporte(int pecacodi, string codreporte, string codtexto)
        {

            VceTextoReporteDTO oVceTextoReporteDTO = servicio.getTextoReporteById(pecacodi, codreporte, codtexto);

            var jsonSerialiser = new JavaScriptSerializer();
            return Json(jsonSerialiser.Serialize(oVceTextoReporteDTO));
        }

        public ActionResult GuardarTextoReporte(int pecacodi, string codreporte, string codtexto, string texto, bool EsNuevo)
        {
            TextoReporteGeneralModel model = new TextoReporteGeneralModel();

            VceTextoReporteDTO oVceTextoReporteDTO = servicio.getTextoReporteById(pecacodi, codreporte, codtexto);

            if (EsNuevo)
            {
                if (oVceTextoReporteDTO != null)
                {
                    //- Ya existe un registro igual, no se puede Crear.
                    return Json(new { success = false, message = "Ya existe un registro igual, no se puede crear" });
                }

                oVceTextoReporteDTO = new VceTextoReporteDTO();
                oVceTextoReporteDTO.PecaCodi = pecacodi;
                oVceTextoReporteDTO.Txtrepcodreporte = codreporte;
                oVceTextoReporteDTO.Txtrepcodtexto = codtexto;
                oVceTextoReporteDTO.Txtreptexto = texto;
                oVceTextoReporteDTO.Txtrepusucreacion = User.Identity.Name;
                oVceTextoReporteDTO.Txtrepfeccreacion = DateTime.Now;

                this.servicio.SaveTextoReporte(oVceTextoReporteDTO);

            }
            else
            {
                if (oVceTextoReporteDTO == null)
                {
                    //- El registro ya no existe, no se puede actualizar.
                    return Json(new { success = false, message = "El registro ya no existe, no se puede actualizar" });
                }

                oVceTextoReporteDTO.Txtreptexto = texto;
                oVceTextoReporteDTO.Txtrepusumodificacion = User.Identity.Name;
                oVceTextoReporteDTO.Txtrepfecmodificacion = DateTime.Now;

                this.servicio.UpdateTextoReporte(oVceTextoReporteDTO);
            }

            return Json(new { success = true, message = "Ok" });
        }

        [HttpPost]
        public JsonResult ProcesarCompensacionEspecial(int pecacodi)
        {
            CompensacionesEspecialesGeneralModel model = new CompensacionesEspecialesGeneralModel();

            //Migramos la data
            servicio.ProcesarCompensacionEspecial(pecacodi);

            return Json(model);
        }

        //MZD START Compensaciones MME
        #region Compensaciones MME        
        public ActionResult CompensacionesMME()
        {
            base.ValidarSesionUsuario();
            CompensacionesMMEGeneralModel model = new CompensacionesMMEGeneralModel();
            model.ListTrnPeriodo = servicio.ListarPeriodosTC();
            model.ListSiEmpresa = servicio.ListaEmpresasCompensacion();

            return View(model);
        }

        public PartialViewResult ListarCompensacionesMME(int pecacodi, string empresa, string central, string grupo, string modo)
        {
            CompensacionesMMEGeneralModel model = new CompensacionesMMEGeneralModel();
            model.ListCompensacionesMME = servicio.ListCompensacionesMME(pecacodi, empresa, central, grupo, modo);
            return PartialView("ListarCompensacionesMME", model);
        }
        
        [HttpPost]
        public JsonResult ProcesarCompensacionMME(int pecacodi)
        {
            CompensacionesMMEGeneralModel model = new CompensacionesMMEGeneralModel();

            //Migramos la data
            servicio.ProcesarCompensacionMME(pecacodi);

            #region Compensacion Manual
            //Actualizamos los registros en la tabla Comepensacion Detalle
            servicio.UpdateCompensacionDet(pecacodi);

            //Insertamos los registros en la tabla Comepensacion Detalle
            servicio.SaveCompensacionDet(pecacodi);
            #endregion

            return Json(model);
        }
        
        public JsonResult ExportarCompensacionesMME(int pecacodi, string empresa, string central, string grupo, string modo)
        {
            try
            {
                string file = this.servicio.GenerarFormatoCompensacionesMME(pecacodi, empresa, central, grupo, modo, AppDomain.CurrentDomain.BaseDirectory + Constantes.RutaCarga);

                return Json(file);
            }
            catch(Exception ex)
            {
                Log.Error("ExportarCompensacionesMME", ex);
                return Json(-1);
            }
        }        
        
        #endregion
        //MZD END Compensaciones MME


        
        [HttpPost]
        public JsonResult ProcesarCompensacionRegular(int pecacodi)
        {
            CompensacionesRegularesGeneralModel model = new CompensacionesRegularesGeneralModel();

            //Migramos la data
            servicio.ProcesarCompensacionRegular(pecacodi);

            return Json(model);
        }


        public ActionResult CompensacionManual()
        {
            base.ValidarSesionUsuario();
            CompensacionManualGeneralModel model = new CompensacionManualGeneralModel();
            model.ListTrnPeriodo = servicio.ListarPeriodosTC();
            model.ListSiEmpresa = servicio.ListaEmpresasCompensacion();

            int periodo = model.ListTrnPeriodo[0].PeriCodi;
            model.ListEveSubcausaevento = servicio.ListTipoOperacion(periodo);

            return View(model);
        }

        // DSH 24-04-2017 : agrego por requerimiento
        public PartialViewResult ListarCompensacionesManuales(int pecacodi, string empresa, string central, string grupo, string modo, string tipo, string fecIni, string fecFin)
        {
            string tipocalculo = "M";
            CompensacionManualGeneralModel model = new CompensacionManualGeneralModel();
            model.ListCompensacionesManuales = servicio.ListCompensacionesManuales(pecacodi, empresa, central, grupo, modo, tipo, fecIni, fecFin, tipocalculo);
            return PartialView("ListarCompensacionesManuales", model);
        }

        // DSH 24-04-2017 : agrego por requerimiento
        public ActionResult EliminarCompensacionManual(int pecacodi, int grupocodi, int subcausacodi, DateTime crcbehorini)
        {
            //CompensacionManualGeneralModel model = new CompensacionManualGeneralModel();

            //VceCompBajaeficDTO oVceCompBajaeficpDTO = servicio.getVceCompBajaeficById(crcbehorfin, crcbehorini, subcausacodi, grupocodi, pecacodi);


            //if (oVceCompBajaeficpDTO == null)
            //{
            //    //- El registro ya no existe, no se puede actualizar.
            //    return Json(new { success = false, message = "El registro ya no existe, no se puede eliminar" });
            //}

            this.servicio.DeleteCompensacionManual(pecacodi, grupocodi, crcbehorini);
            //this.servicio.DeleteCompBajaefic(crcbehorfin, crcbehorini, subcausacodi, grupocodi, pecacodi);

            return Json(new { success = true, message = "Ok" });
            }

        // 23-04-2020 agregado por requerimiento
        public ActionResult EliminarCompensacionManualPorVersion(int pecacodi)
        {
            
            this.servicio.DeleteCompensacionManualByVersion(pecacodi);            

            return Json(new { success = true, message = "Ok" });
        }

        //DSH 26-04-2017 : se agrego por requerimiento
        public JsonResult DescargarFormatoCompensacionManual()
        {
            try
            {
                string file = this.servicio.DescargarFormatoCompensacionManual(AppDomain.CurrentDomain.BaseDirectory + Constantes.RutaCarga);
                return Json(file);
            }
            catch(Exception ex )
            {
                Log.Error("DescargarFormatoCompensacionManual", ex);
                return Json(-1);
            }
        }

        //DSH 12-05-2017 : se agrego por requerimiento
        public JsonResult DescargarFormatoIncrementoReduccion()
        {
            try
            {
                string file = this.servicio.DescargarFormatoIncrementoReduccion(AppDomain.CurrentDomain.BaseDirectory + Constantes.RutaCarga);
                return Json(file);
            }
            catch(Exception ex)
            {
                Log.Error("DescargarFormatoIncrementoReduccion", ex);
                return Json(-1);
            }
        }


        /// <summary>
        /// Nombre del archivo
        /// </summary>
        public String NombreFile
        {
            get
            {
                return (Session[ConstantesCompensacion.SesionNombreArchivo] != null) ?
                    Session[ConstantesCompensacion.SesionNombreArchivo].ToString() : null;
            }
            set { Session[ConstantesCompensacion.SesionNombreArchivo] = value; }
        }

        /// <summary>
        /// Almacena solo en nombre del archivo
        /// </summary>
        public String FileName
        {
            get
            {
                return (Session[ConstantesCompensacion.SesionFileName] != null) ?
                    Session[ConstantesCompensacion.SesionFileName].ToString() : null;
            }
            set { Session[ConstantesCompensacion.SesionFileName] = value; }
        }

        public ActionResult Upload()
        {
            try
            {
                string path = AppDomain.CurrentDomain.BaseDirectory + Constantes.RutaCarga;

                if (Request.Files.Count == 1)
                {
                    var file = Request.Files[0];
                    string fileName = path + Constantes.ArchivoPotencia;

                    if (System.IO.File.Exists(fileName))
                    {
                        System.IO.File.Delete(fileName);
                    }

                    file.SaveAs(fileName);
                }
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            catch(Exception ex)
            {
                Log.Error("Upload", ex);
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }

        }

        // DSH 14-01-2017 :Arraques y paradas - Carga de Incremento y Reducciones
        public ActionResult UploadIR()
        {
            try
            {
                string path = AppDomain.CurrentDomain.BaseDirectory + Constantes.RutaCarga;

                if (Request.Files.Count == 1)
                {
                    var file = Request.Files[0];
                    string fileName = path + Constantes.ArchivoIncrementoReduccion;

                    if (System.IO.File.Exists(fileName))
                    {
                        System.IO.File.Delete(fileName);
                    }

                    file.SaveAs(fileName);
                }
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            catch(Exception ex)
            {
                Log.Error("UploadIR", ex);
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }

        }

        /// <summary>
        /// Permite cargar la potencia desde excel
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult CargarILO(int pecacodi)
        {
            CompensacionManualGeneralModel model = new CompensacionManualGeneralModel();

            try
            {
                string path = AppDomain.CurrentDomain.BaseDirectory + Constantes.RutaCarga + Constantes.ArchivoPotencia;

                string mensaje = servicio.LeerVceCompBajaefic(path, pecacodi);
                model.mensaje = mensaje;

                if (mensaje.Contains("No se encontro"))
                {
                    model.resultado = 0;
                }
                else
                {
                    model.resultado = 1;
                }

                return Json(model);
            }
            catch(Exception ex)
            {
                Log.Error("CargarILO", ex);
                model.mensaje = "A ocurrido un error inprevisto comuniquese con el Administrador del sistema";
                model.resultado = 0;
                return Json(model);
            }
        }

        [HttpPost]
        public JsonResult GrabarArranquesParadas(int pecacodi)
        {
            ArranquesParadasGeneralModel model = new ArranquesParadasGeneralModel();

            this.servicio.SaveArranquesParadas(pecacodi);

            return Json(model);
        }

        //DSH 14-04-2017 : Agrego metodo por requerimiento
        [HttpPost]
        public JsonResult CargarILOIncred(int pericodi, int pecacodi)
        {
            IncrementosReduccionesModel model = new IncrementosReduccionesModel();

            try
            {
                string path = AppDomain.CurrentDomain.BaseDirectory + Constantes.RutaCarga + Constantes.ArchivoIncrementoReduccion;

                string mensaje = servicio.LeerVceArrparIncredGen(path, pericodi, pecacodi, User.Identity.Name);

                model.mensaje = mensaje;

                if (mensaje.Contains("No")) // mensaje.Contains("No se encontro"))
                {
                    model.resultado = 0;
                }
                else
                {
                    model.resultado = 1;
                }

                return Json(model);
            }
            catch(Exception ex)
            {
                Log.Error("CargarILOIncred", ex);
                model.mensaje = "A ocurrido un error inprevisto comuniquese con el Administrador del sistema";
                model.resultado = 0;
                return Json(model);
            }
        }

        //CAMBIOS 20181003

        public ActionResult ModosOperacion()
        {
            base.ValidarSesionUsuario();
            ModosOperacionModel model = new ModosOperacionModel();
            model.ListTrnPeriodo = servicio.ListarPeriodosTC();
            //model.ListSiEmpresa = servicio.ListaEmpresasCompensacion();
                      

            return View(model);
        }

        /// <summary>
        /// Muestra la lista de Modos de operacion
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult ListarModosOperacionCompensaciones(int pecacodi)
        {
            ModosOperacionModel model = new ModosOperacionModel();
            
            model.ListModosOperacion = servicio.ListModoOperacion(pecacodi);

            return PartialView("ListarModosOperacionGeneral", model);
        }

        public ActionResult AccionModo(string accion, int grupocodi, int pecacodi)
        {
            if (accion.Equals("Incluir"))
            {
                servicio.DeleteVceGrupoExcluido(pecacodi, grupocodi);
            }
            else
            {
                base.ValidarSesionUsuario();
                var vceGrupoExcluidoDTO = new VceGrupoExcluidoDTO();
                vceGrupoExcluidoDTO.Pecacodi = pecacodi;
                vceGrupoExcluidoDTO.Grupocodi = grupocodi;
                vceGrupoExcluidoDTO.Crgexcusucreacion = User.Identity.Name;
                vceGrupoExcluidoDTO.Crgexcfeccreacion = DateTime.Now;                

                servicio.SaveVceGrupoExcluido(vceGrupoExcluidoDTO);

            }

            return Json(new { success = true, message = "Ok" });
        }

        [HttpPost]
        public PartialViewResult ListErroresConceptos(int crcvalcodi, string fechadatos)
        {
            ModosOperacionModel model = new ModosOperacionModel();

            //Obtenemos los ids de los grupos
            var listIdsGrupos = servicio.GetListaModosIds(fechadatos).Select(p=>p.Grupocodi).ToList();

            var vceCfgValidaConcepto = servicio.GetVceCfgValidaConceptoById(crcvalcodi);
            DateTime fecha = DateTime.ParseExact(fechadatos, "dd/MM/yyyy", null);

            var lPrGrupodatDTO = servicio.ObtenerResultadoFormula(listIdsGrupos, vceCfgValidaConcepto.Concepcodi, fecha, string.Empty);
            //string nombrePropiedad = string.Empty;

            var valorReferencia = Convert.ToDecimal(vceCfgValidaConcepto.Crcvalvalorref);

            switch (vceCfgValidaConcepto.Crcvalcondicion)
            {
                case "=":
                    {
                        lPrGrupodatDTO = lPrGrupodatDTO.Where(p => p.Valor.Equals(valorReferencia)).ToList();
                        break;
                    }
                case ">":
                    {
                        lPrGrupodatDTO = lPrGrupodatDTO.Where(p => p.Valor > valorReferencia).ToList();
                        break;
                    }
                case "<":
                    {
                        lPrGrupodatDTO = lPrGrupodatDTO.Where(p => p.Valor < valorReferencia).ToList();
                        break;
                    }
                case ">=":
                    {
                        lPrGrupodatDTO = lPrGrupodatDTO.Where(p => p.Valor >= valorReferencia).ToList();
                        break;
                    }
                case "<=":
                    {
                        lPrGrupodatDTO = lPrGrupodatDTO.Where(p => p.Valor <= valorReferencia).ToList();
                        break;
                    }
                case "<>":
                    {
                        lPrGrupodatDTO = lPrGrupodatDTO.Where(p => p.Valor != valorReferencia).ToList();
                        break;
                    }
            }

            foreach (var grupo in lPrGrupodatDTO)
            {
                var grupoDTO = servicio.GetByIdPrGrupo(grupo.Grupocodi);

                grupo.GrupoNomb = grupoDTO.Gruponomb;
            }

            model.ListErroresConcepto = lPrGrupodatDTO;

            return PartialView("ListarErroresConcepto", model);
        }

       

        public ActionResult ValidarConceptos()
        {
            base.ValidarSesionUsuario();
            ModosOperacionModel model = new ModosOperacionModel();
            model.ListValidaConcepto = servicio.ListValidaConceptos();
            //model.ListSiEmpresa = servicio.ListaEmpresasCompensacion();


            return View(model);
        }

        public ActionResult AsignacionBarrasModosOperacion()
        {
            base.ValidarSesionUsuario();
            ModosOperacionModel model = new ModosOperacionModel();

            var grupocodi = string.IsNullOrEmpty(Request["grupocodi"]) ? 0 : Convert.ToInt32(Request["grupocodi"]);

            model.ModoOperacion = servicio.GetByIdPrGrupo(grupocodi);
            model.ListBarras = servicio.ListarBarras();

            return View(model);
        }

        [HttpPost]
        public PartialViewResult ListarAsignacionBarras(int grupocodi)
        {
            ModosOperacionModel model = new ModosOperacionModel();

            model.ListAsignacionBarras = servicio.ListarAsignacionBarraModoOperacion(grupocodi);

            return PartialView("ListarAsignacionBarrasModoOperacion", model);
        }
        public ActionResult GuardarAsignacionBarra(string fechaVigencia, int grupocodi, int barrcodi, bool esNuevo)
        {
            //HorasOperacionGeneralModel model = new HorasOperacionGeneralModel();

            var fecha = DateTime.ParseExact(fechaVigencia, "dd/MM/yyyy", null);

            var oPrGrupodatDTO = new PrGrupodatDTO();
            //PrGrupodatDTO oPrGrupodatDTO = servicio.GetByIdPrGrupodat(fecha, 262, grupocodi, 0);

            //if (oPrGrupodatDTO != null)
            //{
            //    //- Ya existe un registro igual, no se puede Crear.
            //    //return Json(new { success = false, message = "Ya existe un registro igual, no se puede ingresar la asignación" });
            //}
            //else
            //{

            //}

            base.ValidarSesionUsuario();

            oPrGrupodatDTO.Fechadat = fecha;
            oPrGrupodatDTO.Concepcodi = 262;
            oPrGrupodatDTO.Grupocodi = grupocodi;
            oPrGrupodatDTO.Formuladat = barrcodi.ToString();
            oPrGrupodatDTO.Deleted = 0;
            oPrGrupodatDTO.Lastuser = User.Identity.Name;
            oPrGrupodatDTO.Fechaact = DateTime.Now;

            if (esNuevo)
            {
                servicio.SavePrGrupodat(oPrGrupodatDTO);

            }
            else
            {
                servicio.UpdatePrGrupodat(oPrGrupodatDTO);
            }


            return Json(new { success = true, message = "Ok" });
        }

        //Modificacion Costo Marginal 04/11/2019
        /// <summary>
        /// Lista las versiones del periodo a editar
        /// </summary>
        /// <param name="pericodi"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult ListarVersionPeriodo(int pericodi)
        {
            MesValorizacionGeneralModel model = new MesValorizacionGeneralModel();

            //InicializaDatosByVersion(pecacodi);

            //this.servicio.InitMesValorizacion(pecacodi);
            model.ListVcePeriodoCalculo = this.servicio.getVersionesPeridoByIdPeriodo(pericodi).OrderBy(p => p.PecaNombre).ToList();
            //model.VcePeriodoCalculoDTO = this.servicio.getVersionPeriodoById(pecacodi);
            //model.ListEntidades = this.servicio.ListEntidades(pecacodi);

            return PartialView("ListarVersionesPeriodo", model);

        }

        //Se agrega metodo guardar nombre informe
        [HttpPost]
        public JsonResult GuardarInforme(int pericodi, string nombreInforme)
        {
            MesValorizacionGeneralModel model = new MesValorizacionGeneralModel();

            
            var res = this.servicio.UpdateCompensacionInforme(pericodi,nombreInforme);

            //InicializaDatosByVersion(entity.PecaCodi);

            //this.servicio.InitMesValorizacion(entity.PecaCodi);

            //model.VcePeriodoCalculoDTO = entity;
            //model.ListVcePeriodoCalculo = this.servicio.getVersionesPeridoByIdPeriodo(pericodi);

            //return Json(res);
            return Json(new { success = true, message = "Ok" });
        }

        public JsonResult DeletePeriodo(int pecacodi)
        {
            if(pecacodi > 0)
            {
                // eliminar datos de calculos
                this.servicio.DeleteCompRegularDetByVersion(pecacodi);
                this.servicio.DeleteCompBajaeficByVersion(pecacodi);
                this.servicio.DeleteVceArrparGrupoCabByVersion(pecacodi);
                this.servicio.DeleteVceCostoVariableByVersion(pecacodi);
                this.servicio.DeleteCalculo(pecacodi);

                // eliminar datos de origen
                this.servicio.DeleteVceEnergiaByVersion(pecacodi);
                this.servicio.DeleteHoraOperacion(pecacodi);

                // eliminar datos de configuracion
                this.servicio.DeleteVcePtomedModopeByVersion(pecacodi);

                // eliminar datos de textos de reportes
                this.servicio.DeleteVceTextoReporteByVersion(pecacodi);

                // eliminar log
                this.servicio.DeleteVceLogCargaDet(pecacodi);
                this.servicio.DeleteVceLogCargaCab(pecacodi);

                //GrupoExcluido
                this.servicio.DeleteVceGrupoExcluidoByVersion(pecacodi);

                //CompMME
                this.servicio.DeleteCompensacionManualByVersion(pecacodi);

                // eliminar registro de version
                this.servicio.DeleteVcePeriodoCalculo(pecacodi);
            }else
            {
                return Json(new { success = false, message = "-" });
            }
            return Json(new { success = true, message = "Ok" });
        }

    }    

    //- compensaciones.HDT - 03/03/2017: Cambio para atender el requerimiento. 
    /// <summary>
    /// Enumerado que sostiene los valores admisibles para la consulta de la información
    /// de la configuración de campos.
    /// </summary>
    public enum EnuVceCfgDatCalculoCfgDcTipoVal
    {
        Ningugo = 0,
        V = 1,
        F = 2,
        U = 3
    }

}
