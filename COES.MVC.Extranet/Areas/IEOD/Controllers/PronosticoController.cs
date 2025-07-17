using COES.Dominio.DTO.Sic;
using COES.Framework.Base.Tools;
using COES.MVC.Extranet.Areas.IEOD.Models;
using COES.MVC.Extranet.Controllers;
using COES.MVC.Extranet.Helper;
using COES.Servicios.Aplicacion.Factory;
using COES.Servicios.Aplicacion.FormatoMedicion;
using COES.Servicios.Aplicacion.Helper;
using COES.Servicios.Aplicacion.PronosticoDemanda;
using COES.Servicios.Aplicacion.PronosticoDemanda.Helper;
using COES.Servicios.Aplicacion.ReservaFriaNodoEnergetico;
using log4net;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace COES.MVC.Extranet.Areas.IEOD.Controllers
{
    public class PronosticoController : BaseController
    {
        public PronosticoController()
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
        FormatoMedicionAppServicio servFormato = new FormatoMedicionAppServicio();
        PronosticoDemandaAppServicio servPronostico = new PronosticoDemandaAppServicio();
        ReservaFriaNodoEnergeticoAppServicio servReservaFria = new ReservaFriaNodoEnergeticoAppServicio();

        /// <summary>
        /// Muestra la pantalla inicial
        /// </summary>
        /// <param name="IdEnvio">Código de la tabla ME_ENVIO</param>
        /// <param name="NumPtosDepurar">Numero de puntos a depurar</param>
        /// <returns></returns>
        public ActionResult Index(int IdEmpresa, int IdEnvio, int IdHojaPadre, int NumPtosDepurar)
        {
            PronosticoModel model = new PronosticoModel();
            int iTipoEmpresa = 4; //USuario Libre por defecto
            //SI_EMPRESA
            Log.Info("Lista Empresas - GetByIdSiEmpresa");
            SiEmpresaDTO dtoEmpresa = servFormato.GetByIdSiEmpresa(IdEmpresa);
            if (dtoEmpresa != null)
            {
                iTipoEmpresa = dtoEmpresa.Tipoemprcodi;
            }
            model.IdEmpresa = IdEmpresa;
            model.IdEnvio = IdEnvio;
            model.IdHojaPadre = IdHojaPadre;
            model.NumPtosDepurar = NumPtosDepurar;
            model.IdTipoEmpresa = iTipoEmpresa;
            //Log.Info("ListaMotivo - ListPrnMotivo");
            //model.ListaMotivo = servPronostico.ListPrnMotivo().Where(x => x.Prnmtvestado == "A").ToList();
            return PartialView(model); 
        }

        /// <summary>
        /// Lista de puntos a depurar
        /// </summary>
        /// <param name="IdEnvio">Código de la tabla ME_ENVIO</param>
        /// <param name="NumPtosDepurar">Numero de puntos a depurar</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Lista(int IdEnvio, int NumPtosDepurar)
        {
            PronosticoModel model = new PronosticoModel();
            model.IdEnvio = IdEnvio;
            model.NumPtosDepurar = NumPtosDepurar;
            //ME_ENVIO
            Log.Info("EntidadEnvio - GetByIdMeEnvio");
            model.EntidadEnvio = servFormato.GetByIdMeEnvio(IdEnvio);
            //ME_FORMATO
            Log.Info("EntidadFormato - GetByIdMeFormato");
            model.EntidadFormato = servFormato.GetByIdMeFormato((int)model.EntidadEnvio.Formatcodi);
            model.Formatresolucion = (int)model.EntidadFormato.Formatresolucion;
            switch (model.Formatresolucion)
            {
                case ParametrosFormato.ResolucionMediaHora:
                    Log.Info("ListaPrn48 - ListPrnMedicion48ByIdEnvio");
                    //PrnMed48.Prnm48estado = IdEnvio; //Identificador que nos ayuda a rescatar a los puntos que estan pendientes de depurar
                    model.ListaPrnMed48 = this.servPronostico.ListPrnMedicion48ByIdEnvio(IdEnvio);
                    //Debe traer solo los ejecutados donde PrnMed48.Prnm48tipo = ConstantesProdem.LectcodiDemEjecDiario; //Lectcodi = 103... luego podria tambien ser el previsto diario
                    //PrnMed48.Meditotal = iDepuracion; //Almacea el numero (en decimal) de intervalos que presentan desviación 
                    break;
                case ParametrosFormato.ResolucionCuartoHora:
                    Log.Info("ListaPrn96 - ListPrnMedicion96ByIdEnvio");
                    //PrnMed96.Prnm96estado = IdEnvio; //Identificador que nos ayuda a rescatar a los puntos que estan pendientes de depurar
                    //model.ListaPrnMed96 = this.servPronostico.ListPrnMedicion96ByIdEnvio(IdEnvio);
                    //Debe traer solo los ejecutados donde PrnMed96.Prnm96tipo = ConstantesProdem.LectcodiDemEjecMensual; //Lectcodi = 51
                    //PrnMed96.Meditotal = iDepuracion; //Almacea el numero (en decimal) de intervalos que presentan desviación 
                    break;
            }
            return PartialView(model);
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //VISTA DETALLE
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Permite obtener la datade un dia
        /// </summary>
        /// <param name="IdEnvio">Identificador de la tabla ME_ENVIO</param>
        /// <param name="Ptomedicodi">Identificador de la tabla ME_PTOMEDICION</param>
        /// <param name="Lectcodi">Identificador de la tabla ME_LECTURA</param>
        /// <param name="Medifecha">Fecha en que se remitio la data</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Comparativo(int IdEnvio, int Ptomedicodi, int Lectcodi, string Medifecha)
        {
            return Json(this.CalcularComparativo(IdEnvio, Ptomedicodi, Lectcodi, Medifecha));
        }

        /// <summary>
        /// Obtener comparativo
        /// </summary>
        /// <param name="IdEnvio">Identificador de la tabla ME_ENVIO</param>
        /// <param name="Ptomedicodi">Identificador de la tabla ME_PTOMEDICION</param>
        /// <param name="Lectcodi">Identificador de la tabla ME_LECTURA</param>
        /// <param name="Medifecha">Fecha en que se remitio la data</param>
        /// <returns></returns>
        private List<ComparativoItemModel> CalcularComparativo(int IdEnvio, int Ptomedicodi, int Lectcodi, string Medifecha)
        {
            string sLectura = "";
            List<ComparativoItemModel> entitys = new List<ComparativoItemModel>();
            try
            {
                //Medifecha viene en formato YYYYMMDD
                DateTime dMedifecha = DateTime.ParseExact(Medifecha, "yyyyMMdd", CultureInfo.InvariantCulture);

                //PATRON - Base de comparación
                //Traemos el patron y el punto a corregir de PRN
                PrnConfiguracionDTO dataConfig = servPronostico.ParametrosGetConfiguracion(Ptomedicodi, ConstantesProdem.DefectoByPunto, dMedifecha);
                PrnPatronModel dataPatron = servPronostico.GetPatron(Ptomedicodi, ConstantesProdem.ProcPatronDemandaEjecutada, dMedifecha, dataConfig);

                //Valida si utiliza el patrón defecto
                decimal[] medPatron = (dataConfig.Prncfgflagdefecto == ConstantesProdem.RegSi) ? dataPatron.PatronDefecto : dataPatron.Patron;

                //PrnMed - Temporal, que almacena la información remitida inicialmente con valores de desvición
                Log.Info("Obtiene una Medición de 48 Intervalos - GetByIdPrnMedicion48");
                PrnMedicion48DTO PrnMed = servPronostico.GetByIdPrnMedicion48(Ptomedicodi, Lectcodi, dMedifecha); // Lectcodi:ConstantesProdem.LectcodiDemEjecDiario / ConstantesProdem.LectcodiDemPrevDiario / ConstantesProdem.LectcodiDemPrevSemanal
                                                                                                                  //Medidicon - Información final que el agente reporta cada vez que graba
                Log.Info("Obtiene una Medición de 48 Intervalos - GetByIdMeMedicion48");
                MeMedicion48DTO Medicion = servReservaFria.GetByIdMeMedicion48(Lectcodi, dMedifecha, ConstantesProdem.TipoinfocodiMWDemanda, Ptomedicodi);

                if (Lectcodi == ConstantesProdem.LectcodiDemEjecDiario)
                {
                    #region EJECUTADO DIARIO: Lectcodi = 103 y hojacodi = 28
                    sLectura = "Previsto Diario"; //SE COMPARA CON EL PREVISTO DIARIO

                    //Previsto - Temporal, que almacena información del Previsto Diario
                    Log.Info("Obtiene una Medición de 48 Intervalos - GetByIdMeMedicion48");
                    MeMedicion48DTO PrevistoDiario = servReservaFria.GetByIdMeMedicion48(ConstantesProdem.LectcodiDemPrevDiario, dMedifecha, ConstantesProdem.TipoinfocodiMWDemanda, Ptomedicodi);
                    if (PrevistoDiario == null)
                        PrevistoDiario = new MeMedicion48DTO();

                    for (int i = 1; i <= 48; i++)
                    {
                        ComparativoItemModel entity = new ComparativoItemModel();
                        entity.Hora = dMedifecha.AddMinutes((i) * 30).ToString(ConstantesProdem.FormatoHoraMinuto);
                        decimal dValorPatron = medPatron[i - 1];
                        decimal dValorPronostico = Convert.ToDecimal(PrnMed.GetType().GetProperty(Constantes.CaracterH + i).GetValue(PrnMed, null));
                        decimal dValorPrevisto = Convert.ToDecimal(PrevistoDiario.GetType().GetProperty(Constantes.CaracterH + i).GetValue(PrevistoDiario, null));
                        decimal dValorMedicion = Convert.ToDecimal(Medicion.GetType().GetProperty(Constantes.CaracterH + i).GetValue(Medicion, null));
                        //Asignando a la entidad
                        entity.ValorPatron = dValorPatron;
                        entity.Pronostico = dValorPronostico;
                        //Previsto Diario
                        entity.ValorPrevisto = dValorPrevisto;
                        entity.ValorMedicion = dValorMedicion;
                        entity.Desviacion = (dValorPatron != 0) ? (dValorMedicion - dValorPatron) / dValorPatron : 0;
                        entitys.Add(entity);
                    }
                    #endregion
                }
                else if (Lectcodi == ConstantesProdem.LectcodiDemPrevDiario)
                {
                    #region PREVISTO DIARIO
                    sLectura = "Previsto Semanal"; //SE COMPARA CON EL PREVISTO SEMANAL

                    //PrnMed - Temporal, que almacena información del Previsto Semanal
                    Log.Info("Obtiene una Medición de 48 Intervalos - GetByIdMeMedicion48");
                    MeMedicion48DTO PrevistoSemanal = servReservaFria.GetByIdMeMedicion48(ConstantesProdem.LectcodiDemPrevSemanal, dMedifecha, ConstantesProdem.TipoinfocodiMWDemanda, Ptomedicodi);
                    if (PrevistoSemanal == null)
                        PrevistoSemanal = new MeMedicion48DTO();

                    for (int i = 1; i <= 48; i++)
                    {
                        ComparativoItemModel entity = new ComparativoItemModel();
                        entity.Hora = dMedifecha.AddMinutes((i) * 30).ToString(ConstantesProdem.FormatoHoraMinuto);
                        decimal dValorPatron = medPatron[i - 1];
                        decimal dValorPronostico = Convert.ToDecimal(PrnMed.GetType().GetProperty(Constantes.CaracterH + i).GetValue(PrnMed, null));
                        decimal dValorPrevisto = Convert.ToDecimal(PrevistoSemanal.GetType().GetProperty(Constantes.CaracterH + i).GetValue(PrevistoSemanal, null));
                        decimal dValorMedicion = Convert.ToDecimal(Medicion.GetType().GetProperty(Constantes.CaracterH + i).GetValue(Medicion, null));
                        //Asignando a la entidad
                        entity.ValorPatron = dValorPatron;
                        entity.Pronostico = dValorPronostico;
                        //Previsto Diario
                        entity.ValorPrevisto = dValorPrevisto;
                        entity.ValorMedicion = dValorMedicion;
                        entity.Desviacion = (dValorPatron != 0) ? (dValorMedicion - dValorPatron) / dValorPatron : 0;
                        entitys.Add(entity);
                    }
                    #endregion
                }
                else if (Lectcodi == ConstantesProdem.LectcodiDemPrevSemanal)
                {
                    #region PREVISTO SEMANAL
                    sLectura = ""; //NO SE COMPARA CON NINGUNA INFORMACIÓN ADICIONAL

                    for (int i = 1; i <= 48; i++)
                    {
                        ComparativoItemModel entity = new ComparativoItemModel();
                        entity.Hora = dMedifecha.AddMinutes((i) * 30).ToString(ConstantesProdem.FormatoHoraMinuto);
                        decimal dValorPatron = medPatron[i - 1];
                        decimal dValorPronostico = Convert.ToDecimal(PrnMed.GetType().GetProperty(Constantes.CaracterH + i).GetValue(PrnMed, null));
                        decimal dValorMedicion = Convert.ToDecimal(Medicion.GetType().GetProperty(Constantes.CaracterH + i).GetValue(Medicion, null));
                        //Asignando a la entidad
                        entity.ValorPatron = dValorPatron;
                        entity.Pronostico = dValorPronostico;
                        //Previsto Diario
                        entity.ValorPrevisto = 0; // dValorPrevisto;
                        entity.ValorMedicion = dValorMedicion;
                        entity.Desviacion = (dValorPatron != 0) ? (dValorMedicion - dValorPatron) / dValorPatron : 0;
                        entitys.Add(entity);
                    }
                    #endregion
                }
                //Nombre del Punto y Fecha
                MePtomedicionDTO dtoPtoMedicion = this.servFormato.GetByIdMePtomedicion(Ptomedicodi);
                if (entitys != null)
                {
                    entitys[0].PtomediDesc = dtoPtoMedicion.Ptomedidesc + " - " + dtoPtoMedicion.Ptomedibarranomb;
                    entitys[0].Medifecha = dMedifecha.ToString("dd/MM/yyyy");
                    entitys[0].Lectura = sLectura;
                    //entitys[0].PorcDesviacion = 5; //Valor por defecto
                    entitys[0].PorcDesviacion = Convert.ToDecimal(dataConfig.Prncfgporcdsvptrn);

                    //Obtenemos la lista de justificación
                    entitys[0].ListaJustificacion = this.servPronostico.GetListaJustificacion();
                    //Log.Info("Lista Justificaciones - ListMeJustificacionsByEnvio");
                    entitys[0].ListaMeJustificacion = this.servPronostico.ListByIdEnvioPtoMedicodi(IdEnvio, Lectcodi, Ptomedicodi);
                    foreach (var mj in entitys[0].ListaMeJustificacion)
                    {
                        TimeSpan ts = (mj.Justfechafin.Value - mj.Justfechainicio.Value);
                        mj.nroFilas = Convert.ToInt32(ts.TotalMinutes) / 30 + 1;
                    }
                }
            }
            catch (Exception e)
            {
                sLectura = e.Message;
            }
            return entitys;
        }

        /// <summary>
        /// Permite grabar la data del punto de medición actualizado
        /// </summary>
        /// <param name="IdEmpresa"></param>
        /// <param name="IdEnvio"></param>
        /// <param name="Ptomedicodi"></param>
        /// <param name="Lectcodi"></param>
        /// <param name="Medifecha"></param>
        /// <param name="listaMeJust"></param>
        /// <param name="listaH"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GrabarMedicion(int IdEmpresa, int IdEnvio, int Ptomedicodi, int Lectcodi, string Medifecha, string listaMeJust, string listaH)
        {
            base.ValidarSesionUsuario();
            int iResultado = 1;
            string user = User.Identity.Name;
            int iTipoEmpresa = ConstantesProdem.TipoemprcodiUsuLibres; //USuario Libre por defecto
            //Medifecha viene en formato YYYYMMDD
            DateTime dMedifecha = DateTime.ParseExact(Medifecha, "yyyyMMdd", CultureInfo.InvariantCulture);
            try
            {
                //SI_EMPRESA
                Log.Info("Obtiene un Empresa - GetByIdSiEmpresa");
                SiEmpresaDTO dtoEmpresa = servFormato.GetByIdSiEmpresa(IdEmpresa);
                if (dtoEmpresa != null)
                {
                    iTipoEmpresa = dtoEmpresa.Tipoemprcodi;
                }
                decimal dPorcentajeDesviacion = 0.05M;
                decimal dDesviacion;
                
                PrnConfiguracionDTO dataConfig = this.servPronostico.ParametrosGetConfiguracion(Ptomedicodi, ConstantesProdem.DefectoByPunto, dMedifecha);
                dPorcentajeDesviacion = Convert.ToDecimal(dataConfig.Prncfgporcdsvptrn) * 0.01M;

                //LISTA DE LOS 48 INPUT QUE GUARDA LOS VALORES FINALES
                string[] ListaH = listaH.Split(',');
                //Medidicon - Información final que el agente reporta cada vez que graba
                Log.Info("Obtiene una Medición de 48 Intervalos - GetByIdMeMedicion48");
                MeMedicion48DTO Medicion = servReservaFria.GetByIdMeMedicion48(Lectcodi, dMedifecha, ConstantesProdem.TipoinfocodiMWDemanda, Ptomedicodi);
                for (int i = 1; i <= 48; i++)
                {
                    //Actualizamos los valores H
                    Medicion.GetType().GetProperty("H" + i).SetValue(Medicion, Convert.ToDecimal(ListaH[i - 1]));
                }
                
                //Eliminamos la medición 48
                Log.Info("Elimina una Medición de 48 Intervalos - DeleteMeMedicion48");
                this.servReservaFria.DeleteMeMedicion48(Lectcodi, dMedifecha, ConstantesProdem.TipoinfocodiMWDemanda, Ptomedicodi);
                //Insertamos la medición 48
                Log.Info("Registra una Medición de 48 Intervalos - SaveMeMedicion48Id");
                this.servReservaFria.SaveMeMedicion48Id(Medicion);
                
                //Obtiene el perfíl patrón
                PrnPatronModel dataPatron = servPronostico.GetPatron(Ptomedicodi, ConstantesProdem.ProcPatronDemandaEjecutada, dMedifecha, dataConfig);
                //Valida si debe utilizar el patrón defecto
                decimal[] medPatron = (dataConfig.Prncfgflagdefecto == ConstantesProdem.RegSi) ? dataPatron.PatronDefecto : dataPatron.Patron;

                if (dataPatron.NDias > 0)
                {
                    Log.Info("Obtiene una Medición de 48 Intervalos - GetByIdPrnMedicion48");
                    PrnMedicion48DTO PrnMed48 = servPronostico.GetByIdPrnMedicion48(Ptomedicodi, Lectcodi, dMedifecha);
                    int iDepuracion = 0; //NO hay nada que depurar
                    for (int i = 1; i <= 48; i++)
                    {
                        decimal dValorPatron = medPatron[i - 1];
                        decimal dValorEntidad = Convert.ToDecimal(Medicion.GetType().GetProperty(Constantes.CaracterH + i).GetValue(Medicion, null));
                         
                        dDesviacion = 0.0M;
                        if (dValorPatron != 0)
                        {
                            dDesviacion = Math.Abs((dValorEntidad - dValorPatron) / dValorPatron);
                        }

                        if (dDesviacion >= dPorcentajeDesviacion)
                        {
                            iDepuracion++; //Contabiliza lo que va a depurar
                        }
                        PrnMed48.GetType().GetProperty("H" + i).SetValue(PrnMed48, dValorEntidad);
                    }
                    //actualizamos el punto de medición en el pronostico para mostrar en la extranet en color amarillo
                    PrnMed48.Meditotal = iDepuracion * -1; //Almacea el numero de intervalos que presentan desviación con signo negativo para que pinte de otro color en la vista
                    Log.Info("Elimina una Medición de 48 Intervalos - DeletePrnMedicion48");
                    this.servPronostico.DeletePrnMedicion48(PrnMed48.Ptomedicodi, PrnMed48.Prnm48tipo, PrnMed48.Medifecha);
                    Log.Info("Registra una Medición de 48 Intervalos - SavePrnMedicion48");
                    this.servPronostico.SavePrnMedicion48(PrnMed48);
                }//Fin de Patron48
                
                //Actualizamos las justificaciones
                string[] ListaMeJustificacion = listaMeJust.Split(','); //Almacena en par: justcodi_subcausacodi (es la nueva causa)
                int iNumBloques = ListaMeJustificacion.Count();
                bool flagJustificacion = false;
                for (int i = 0; i < iNumBloques; i++)
                {
                    string[] just_subcausa = ListaMeJustificacion[i].Split('_');
                    int justcodi = Convert.ToInt32(just_subcausa[0]);
                    MeJustificacionDTO dtoMJ = servFormato.GetByIdMeJustificacion(justcodi);
                    if (dtoMJ != null)
                    {
                        dtoMJ.Subcausacodi = Convert.ToInt32(just_subcausa[1]);
                        servFormato.UpdateMeJustificacion(dtoMJ);
                    }
                    if (dtoMJ.Subcausacodi == ConstantesProdem.IdJustificacionMantenimiento || dtoMJ.Subcausacodi == ConstantesProdem.IdJustificacionFalla) {
                        flagJustificacion = true;
                    }
                }
                if (Lectcodi == ConstantesProdem.LectcodiDemEjecDiario)
                {
                    //EJECUTADO DIARIO - Procesos automáticos
                    servPronostico.ProcesarDemandaReportada(Ptomedicodi, dMedifecha, Medicion, dataConfig, dataPatron, flagJustificacion, user);
                }
            }
            catch (Exception e)
            {
                string sError = e.Message;
                iResultado = -1;
            }
            return Json(iResultado);
        }

        /// <summary>
        /// Permite realizar la verificación de la información reportada vs patron
        /// </summary>
        /// <param name="IdEnvio">Código de la tabla ME_ENVIO</param>
        /// <param name="semana">Semana a la que corresponde la data</param>
        /// <param name="mes">Mes a la que corresponde la data</param>
        [HttpPost]
        public JsonResult VerificarPronostico(int IdEnvio, int IdHojaPadre, string anio, string semana, string mes, int IdEmpresa)
        {
            base.ValidarSesionUsuario();
            string sNumPuntosDepurar = "0";
            try {
                int iNumPuntosDepurar = 0;
                decimal dPorcentajeDesviacionPorDefecto = 0.05M;
                decimal? dDesviacion;
                string user = User.Identity.Name;
                int iTipoEmpresa = ConstantesProdem.TipoemprcodiUsuLibres; //USuario Libre por defecto
                //ME_ENVIO
                Log.Info("EntidadFormato - GetByIdMeFormato");
                MeEnvioDTO dtoEnvio = servFormato.GetByIdMeEnvio(IdEnvio);
                //SI_EMPRESA
                Log.Info("Obtiene una Empresa - GetByIdSiEmpresa");
                SiEmpresaDTO dtoEmpresa = servFormato.GetByIdSiEmpresa(IdEmpresa);
                if (dtoEmpresa != null)
                {
                    iTipoEmpresa = dtoEmpresa.Tipoemprcodi;
                }

                //ME_FORMATO
                Log.Info("EnvioDTO - GetByIdMeEnvio");
                MeFormatoDTO formato = servFormato.GetByIdMeFormato((int)dtoEnvio.Formatcodi);
                Log.Info("Obtiene Hoja por Criterio - GetByCriteriaMeHoja");
                formato.ListaHoja = servFormato.GetByCriteriaMeHoja(formato.Formatcodi);
                if (IdHojaPadre > 0)
                {
                    formato.ListaHoja = formato.ListaHoja.Where(x => x.Hojapadre == IdHojaPadre).ToList();
                }
                formato.FlagUtilizaHoja = true;
                Log.Info("Lista Cabecera - GetListMeCabecera");
                var cabecera = servFormato.GetListMeCabecera().Where(x => x.Cabcodi == formato.Cabcodi).FirstOrDefault();
                formato.Formatcols = cabecera.Cabcolumnas;
                formato.Formatrows = cabecera.Cabfilas;
                formato.Formatheaderrow = cabecera.Cabcampodef;
                /////////////// Obtiene Fecha Inicio y Fecha Fin del Proceso //////////////
                string sDia = dtoEnvio.Enviofechaperiodo.GetValueOrDefault().ToString("dd/MM/yyyy");
                //string sSemana = sDia.Substring(6, 4) + semana; 
                string sSemana = anio + semana;
                formato.FechaProceso = EPDate.GetFechaIniPeriodo((int)formato.Formatperiodo, mes, sSemana, sDia, Constantes.FormatoFecha);
                //ToolsFormato.GetSizeFormato(formato);
                FormatoMedicionAppServicio.GetSizeFormato(formato);

                //OBTENIENDO LA LISTA DE PUNTOS DEL FORMATO
                switch (formato.Formatresolucion)
                {
                    case ParametrosFormato.ResolucionMediaHora:
                        //Log.Info("Obtiene Datos del Formato - GetDataFormato");
                        //List<Object> lista48 = servFormato.GetDataFormato((int)dtoEnvio.Emprcodi, formato, 0, 0);
                        Log.Info("Obtiene Datos del Formato - GetDataFormato48");
                        List<MeMedicion48DTO> lista48 = servFormato.GetDataFormato48((int)dtoEnvio.Emprcodi, formato, dtoEnvio.Enviocodi, 0);
                        
                        //Módificación Assetec - 20200707
                        foreach (MeMedicion48DTO entity in lista48)
                        {
                            int iPrnm48tipo = 0;

                            //Obtiene los datos de configuración de parámetros y el perfíl patrón
                            PrnConfiguracionDTO dataConfig = this.servPronostico.ParametrosGetConfiguracion(entity.Ptomedicodi, ConstantesProdem.DefectoByPunto, entity.Medifecha);
                            PrnPatronModel dataPatron = this.servPronostico.GetPatron(entity.Ptomedicodi, ConstantesProdem.ProcPatronDemandaEjecutada, entity.Medifecha, dataConfig);

                            if (entity.Lectcodi == ConstantesProdem.LectcodiDemEjecDiario && entity.Hojacodi == ConstantesProdem.HojacodiDemEjecDiario)
                            {
                                //EJECUTADO DIARIO: Lectcodi = 103 y hojacodi = 28
                                iPrnm48tipo = ConstantesProdem.LectcodiDemEjecDiario; //Lectcodi = 103

                                //Procesos automáticos
                                servPronostico.ProcesarDemandaReportada(entity.Ptomedicodi, entity.Medifecha, entity, dataConfig, dataPatron, false, user);
                            }
                            else if (entity.Lectcodi == ConstantesProdem.LectcodiDemPrevDiario && entity.Hojacodi == ConstantesProdem.HojacodiDemPrevDiario)
                            {
                                //PREVISTO DIARIO:  Lectcodi=110 y hojacodi=29
                                iPrnm48tipo = ConstantesProdem.LectcodiDemPrevDiario; //Lectcodi = 110

                                //Procesos automáticos
                                servPronostico.ProcesarDemandaReportada(entity.Ptomedicodi, entity.Medifecha, entity, dataConfig, dataPatron, false, user);
                            }
                            else if (entity.Lectcodi == ConstantesProdem.LectcodiDemPrevSemanal && entity.Hojacodi == ConstantesProdem.HojacodiDemPrevSemanal)
                            {
                                //PREVISTO SEMANAL:  Lectcodi=102 y hojacodi=26
                                iPrnm48tipo = ConstantesProdem.LectcodiDemPrevSemanal; //Lectcodi = 102

                                //Procesos automáticos
                                servPronostico.ProcesarDemandaReportada(entity.Ptomedicodi, entity.Medifecha, entity, dataConfig, dataPatron, false, user);
                            }
                            else
                                continue;

                            //------------------------------------------------------------------------------------------------------------------------------------------
                            DateTime dMedifechaInicio = DateTime.MinValue;
                            DateTime dMedifechaFin = DateTime.MaxValue;
                            decimal dPorcentajeDesviacion = dPorcentajeDesviacionPorDefecto;

                            dPorcentajeDesviacion = Convert.ToDecimal(dataConfig.Prncfgporcdsvptrn) * 0.01M;
                            decimal[] medPatron = (dataConfig.Prncfgflagdefecto == ConstantesProdem.RegSi) ? dataPatron.PatronDefecto : dataPatron.Patron;

                            if (dataPatron.NDias > 0)
                            {
                                //EXISTE PATRON
                                PrnMedicion48DTO PrnMed48 = new PrnMedicion48DTO();
                                int iDepuracion = 0; //NO hay nada que depurar
                                for (int i = 1; i <= 48; i++)
                                {
                                    decimal dValorPatron = medPatron[i - 1];
                                    decimal dValorEntidad = Convert.ToDecimal(entity.GetType().GetProperty(Constantes.CaracterH + i).GetValue(entity, null));
                                    dDesviacion = 0.0M;
                                    if (dValorPatron != 0)
                                    {
                                        dDesviacion = Math.Abs((dValorEntidad - dValorPatron) / dValorPatron);
                                    }
                                    //El intervalo necesita depuración?
                                    if (dDesviacion >= dPorcentajeDesviacion)
                                    {
                                        iDepuracion++; //Contabiliza lo que va a depurar
                                                       //Inicia intervalo de justificación
                                        if (dMedifechaInicio == DateTime.MinValue)
                                        {
                                            double dMinutos = 30 * i;
                                            dMedifechaInicio = entity.Medifecha.AddMinutes(dMinutos);
                                        }
                                    }
                                    else
                                    {
                                        //No necesita depuración
                                        if (dMedifechaInicio != DateTime.MinValue)
                                        {
                                            //Hay un grupo de datos que necesita justificación, hasta el intervalo anterior
                                            double dMinutos = 30 * (i - 1);
                                            dMedifechaFin = entity.Medifecha.AddMinutes(dMinutos);
                                            //Insertamos en me_justificación
                                            MeJustificacionDTO mj = new MeJustificacionDTO();
                                            mj.Enviocodi = IdEnvio;
                                            mj.Lectcodi = entity.Lectcodi;
                                            mj.Ptomedicodi = entity.Ptomedicodi;
                                            mj.Subcausacodi = ConstantesProdem.IdSubCausaJustificacion;
                                            mj.Justdescripcionotros = "";
                                            mj.Justfechainicio = dMedifechaInicio;
                                            mj.Justfechafin = dMedifechaFin;
                                            mj.Justfeccreacion = DateTime.Now;
                                            mj.Justusucreacion = User.Identity.Name;
                                            this.servFormato.SaveMeJustificacion(mj);
                                            //Limpiamos Fecha de inicio
                                            dMedifechaInicio = DateTime.MinValue;
                                        }
                                    }
                                    PrnMed48.GetType().GetProperty("H" + i).SetValue(PrnMed48, dValorEntidad);
                                }
                                //Por si queda el ultimointervalo suelto
                                if (dMedifechaInicio != DateTime.MinValue)
                                {
                                    //Hay un grupo de datos que necesita justificación, hasta el intervalo anterior
                                    dMedifechaFin = entity.Medifecha.AddDays(1);
                                    //Insertamos en me_justificación
                                    MeJustificacionDTO mj = new MeJustificacionDTO();
                                    mj.Enviocodi = IdEnvio;
                                    mj.Lectcodi = entity.Lectcodi;
                                    mj.Ptomedicodi = entity.Ptomedicodi;
                                    mj.Subcausacodi = ConstantesProdem.IdSubCausaJustificacion;
                                    mj.Justdescripcionotros = "";
                                    mj.Justfechainicio = dMedifechaInicio;
                                    mj.Justfechafin = dMedifechaFin;
                                    mj.Justfeccreacion = DateTime.Now;
                                    mj.Justusucreacion = User.Identity.Name;
                                    this.servFormato.SaveMeJustificacion(mj);
                                }
                                //Validamos si el punto de medición requerie depuración manual en la extranet
                                if (iDepuracion > 0) //Porcentaje de muestra -> elimine  > dPorcentajeDesviacion * 100
                                {
                                    //HAY QUE VALIDAR
                                    iNumPuntosDepurar++;
                                    PrnMed48.Ptomedicodi = (int)entity.GetType().GetProperty("Ptomedicodi").GetValue(entity, null);
                                    //ConstantesProdem.TipoinfocodiMWDemanda -> Tipoinfocodi = 1 
                                    PrnMed48.Prnm48tipo = iPrnm48tipo;
                                    PrnMed48.Medifecha = (DateTime)entity.GetType().GetProperty("Medifecha").GetValue(entity, null);
                                    PrnMed48.Prnm48estado = IdEnvio; //Identificador que nos ayuda a rescatar a los puntos que estan pendientes de depurar
                                    PrnMed48.Meditotal = iDepuracion; //Almacea el numero (en decimal) de intervalos que presentan desviación 
                                    PrnMed48.Prnm48usucreacion = user;
                                    PrnMed48.Prnm48feccreacion = DateTime.Now;
                                    PrnMed48.Prnm48usumodificacion = user;
                                    PrnMed48.Prnm48fecmodificacion = DateTime.Now;
                                    Log.Info("Elimina una Medición de 48 Intervalos - DeletePrnMedicion48");
                                    this.servPronostico.DeletePrnMedicion48(PrnMed48.Ptomedicodi, PrnMed48.Prnm48tipo, PrnMed48.Medifecha);
                                    Log.Info("Registra una Medición de 48 Intervalos - SavePrnMedicion48");
                                    //Es una copia de lo reportado por el agente mas el numero de intervalos de desviación
                                    this.servPronostico.SavePrnMedicion48(PrnMed48);
                                }
                            }
                        }
                        //Sincronización con el sistema COES(Actual)
                        this.SincronizacionCOES(lista48);
                        break;
                        
                }

                //Modificación Assetec - 20200707
                if (iTipoEmpresa.Equals(ConstantesProdem.TipoemprcodiDistribuidores))
                {
                    sNumPuntosDepurar = "0";
                }
                else
                {
                    if (formato.Lectcodi.Equals(ConstantesProdem.LectcodiDemEjecDiario))
                    {
                        sNumPuntosDepurar = "0";
                    }
                    else
                    {
                        sNumPuntosDepurar = iNumPuntosDepurar.ToString();
                    }
                }
                
                //sNumPuntosDepurar = iNumPuntosDepurar.ToString();
            }
            catch (Exception e)
            {
                sNumPuntosDepurar = e.Message;
            }
            return Json(sNumPuntosDepurar);
        }

        /// <summary>
        /// Permite registrar los datos reportados en nueva interfaz de extranet a el sistema COES(Actual)
        /// </summary>
        public void SincronizacionCOES(List<MeMedicion48DTO> dataMedicion)
        {
            foreach (MeMedicion48DTO med in dataMedicion)
            {
                switch (med.Hojacodi)
                {
                    case ConstantesProdem.HojacodiDemEjecDiario:
                        #region Demanda ejecutada diaria
                        {
                            //Validación
                            if (med.Lectcodi != ConstantesProdem.LectcodiDemEjecDiario) break;

                            //Actualiza la medición
                            med.Lectcodi = ConstantesProdem.AntiguoLectcodiEjecutado;
                            med.Medifecha = med.Medifecha;
                            med.Tipoinfocodi = ConstantesProdem.AntiguoTipoinfocodi;

                            this.servReservaFria.SaveMeMedicion48Id(med);
                        }
                        #endregion
                        break;
                    case ConstantesProdem.HojacodiDemPrevDiario:
                        #region Demanda prevista diaria
                        {
                            //Validación
                            if (med.Lectcodi != ConstantesProdem.LectcodiDemPrevDiario) break;

                            //Actualiza la medición
                            med.Lectcodi = ConstantesProdem.AntiguoLectcodiPrevisto;
                            med.Medifecha = med.Medifecha;
                            med.Tipoinfocodi = ConstantesProdem.AntiguoTipoinfocodi;

                            this.servReservaFria.SaveMeMedicion48Id(med);
                        }
                        #endregion
                        break;
                    case ConstantesProdem.HojacodiDemPrevSemanal:
                        #region Demanda prevista semanal
                        {
                            //Validación
                            if (med.Lectcodi != ConstantesProdem.LectcodiDemPrevSemanal) break;

                            //Actualiza la medición
                            med.Lectcodi = ConstantesProdem.AntiguoLectcodiSemanal;
                            med.Medifecha = med.Medifecha;
                            med.Tipoinfocodi = ConstantesProdem.AntiguoTipoinfocodi;

                            this.servReservaFria.SaveMeMedicion48Id(med);
                        }
                        #endregion
                        break;
                }
            }
        }
    }
}