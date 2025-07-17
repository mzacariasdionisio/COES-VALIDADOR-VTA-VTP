using COES.Base.Core;
using COES.Dominio.DTO.Sic;
using COES.Dominio.DTO.Transferencias;
using COES.Infraestructura.Datos.Repositorio.Transferencias;
using COES.Servicios.Aplicacion.Correo;
using COES.Servicios.Aplicacion.Eventos.Helper;
using COES.Servicios.Aplicacion.Factory;
using COES.Servicios.Aplicacion.Helper;
using COES.Servicios.Aplicacion.Transferencias.Helper;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;
using COES.Servicios.Aplicacion.TransfPotencia.Helper;
using System.Data;
using COES.Dominio.DTO.Enum;

namespace COES.Servicios.Aplicacion.Transferencias
{
    /// <summary>
    /// Instancia de clase de aplicación
    /// </summary>
    public class SolicitudCodigoAppServicio : AppServicioBase
    {
        /// <summary>
        /// Instancia para el manejo de logs
        /// </summary>
        private static readonly ILog Logger = LogManager.GetLogger(typeof(SolicitudCodigoAppServicio));

        CorreoAppServicio servCorreo = new CorreoAppServicio();

        /// <summary>
        /// Permite grabar o actualizar un  SolicitudCodigoDTO en base a la entidad
        /// </summary>
        /// <param name="entity">Entidad de SolicitudCodigoDTO</param>
        /// <param name="suministro">Entidad de SolicitudCodigoDTO</param>
        /// <returns>Retorna el iCoReSoCodi nuevo o actualizado</returns>
        public int SaveOrUpdateCodigoRetiro(SolicitudCodigoDTO entity, List<SolicitudCodigoDetalleDTO> suministro)
        {
            int id = 0;
            try
            {

                if (entity.SoliCodiRetiCodi == 0)
                {
                    id = FactoryTransferencia.GetSolicitudCodigoRepository().Save(entity);
                    if (id > 0)
                    {

                        entity.SoliCodiRetiCodi = id;
                        entity.TrnpcTipoPotencia = entity.TrnpcTipoCasoAgrupado == "AGRVTP" ? 2 : 1;
                        FactoryTransferencia.GetSolicitudCodigoRepository().SaveSolicitudPeriodo(entity);
                        foreach (var item in suministro)
                        {
                            item.Coresocodi = id;

                            int result = FactoryTransferencia.GetSolicitudCodigoDetalleRepository().Save(item);
                            if (result > 0)
                            {
                                List<TrnPotenciaContratadaDTO> potenciasAsignadas = new List<TrnPotenciaContratadaDTO>();
                                if (entity.TrnpcTipoCasoAgrupado == "AGRVTP")
                                    potenciasAsignadas = entity.ListaPotenciaContratadas.Where(x => x.BarrCodi == item.Barracodisum).ToList();

                                for (int i = 0; i < item.Coresdnumregistro; i++)
                                {
                                    CodigoGeneradoDTO gen = new CodigoGeneradoDTO();
                                    gen.Coresdcodi = result;
                                    gen.Coregeestado = "PAP";
                                    gen.Coregecodigovtp = null;
                                    gen.Coregeusuregistro = item.Coresdusuarioregistro;
                                    int idCodigoGenerado = FactoryTransferencia.GetSolicitudCodigoDetalleRepository().SaveSubDetalle(gen);
                                    #region Potencias Contratada No Existente
                                    //Aqui
                                    if (potenciasAsignadas.Count > 0)
                                    {

                                        var itemPot = potenciasAsignadas[i];
                                        itemPot.CoresoCodi = id;
                                        itemPot.CoregeCodi = idCodigoGenerado;
                                        itemPot.TrnPctUserNameIns = entity.UsuaCodi;
                                        itemPot.PeriCodi = entity.PeridcCodi;
                                        //itemPot.TrnPctComeObs = "Potencias  contradas, que fueron registradas a nivela a nivel VTP";
                                        itemPot.TrnPctComeObs = entity.SoliCodiRetiDescripcion;
                                        FactoryTransferencia.GetTrnPotenciaContratadaRepository().GenerarCargaDatos(itemPot);

                                    }



                                    //foreach (var itemPot in potenciasAsignadas)
                                    //{
                                    //    itemPot.CoresoCodi = id;
                                    //    itemPot.CoregeCodi = idCodigoGenerado;
                                    //    itemPot.TrnPctUserNameIns = entity.UsuaCodi;
                                    //    itemPot.PeriCodi = entity.PeridcCodi;
                                    //    itemPot.TrnPctComeObs = "Potencias  contradas, que fueron registradas a nivela a nivel VTP";
                                    //    FactoryTransferencia.GetTrnPotenciaContratadaRepository().GenerarCargaDatos(itemPot);

                                    //}
                                    #endregion Potencias Contratada Existente

                                    #region Consolidado Periodo VTP
                                    entity.Codretgencodi = idCodigoGenerado;
                                    FactoryTransferencia.GetSolicitudCodigoRepository().SaveSolicitudPeriodoVTP(entity);
                                    #endregion Consolidado Periodo VTP
                                }
                            }

                        }

                    }
                    entity.SoliCodiRetiCodi = id;
                    #region Potencias Contratadas


                    if (entity.TrnpcTipoCasoAgrupado == "AGRVTA")
                    {

                        FactoryTransferencia.GetTrnPotenciaContratadaRepository().DesactivarPotenciasContratadas(entity.SoliCodiRetiCodi, entity.PeridcCodi);


                        TrnPotenciaContratadaDTO paramPotencia = new TrnPotenciaContratadaDTO();
                        paramPotencia.PeriCodi = entity.PeridcCodi;
                        paramPotencia.CoresoCodi = entity.SoliCodiRetiCodi;
                        paramPotencia.CoregeCodi = null;
                        paramPotencia.TrnpcTipoCasoAgrupado = "AGRVTA";
                        paramPotencia.TrnPctTotalMwFija = entity.TrnPctTotalmwFija;
                        paramPotencia.TrnPctHpMwFija = entity.TrnPctHpmwFija;
                        paramPotencia.TrnPctHfpMwFija = entity.TrnPctHfpmwFija;
                        paramPotencia.TrnPctTotalMwVariable = entity.TrnPctTotalmwVariable;
                        paramPotencia.TrnPctHpMwFijaVariable = entity.TrnPctHpmwFijaVariable;
                        paramPotencia.TrnPctHfpMwFijaVariable = entity.TrnPctHfpmwFijaVariable;
                        paramPotencia.TrnPctComeObs = null;
                        paramPotencia.TrnPctUserNameIns = entity.UsuaCodi;
                        paramPotencia.TrnPcEnvCodi = null;
                        //  paramPotencia.TrnPctComeObs = "Potencias  contradas, que fueron registradas a nivela a nivel VTA";
                        paramPotencia.TrnPctComeObs = entity.SoliCodiRetiDescripcion;
                        FactoryTransferencia.GetTrnPotenciaContratadaRepository().GenerarCargaDatosExcel(paramPotencia);

                    }


                    #endregion Potencias Contratadas

                    #region Notificacion
                    if (id > 0)
                    {
                        bool envio = EnviarCorreoNotificacionRegistroSolicitud(entity);
                    }
                    #endregion Notificacion



                }
            }
            catch (Exception ex)
            {
                id = 0;
                Logger.Error(ConstantesAppServicio.LogError, ex);


            }
            return id;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        public void UpdateCodigoRetiroPotenciasContradasVTA(SolicitudCodigoDTO entity)
        {
            #region Potencias Contratadas



            TrnPotenciaContratadaDTO paramPotencia = new TrnPotenciaContratadaDTO();
            paramPotencia.PeriCodi = entity.PeridcCodi;
            paramPotencia.CoresoCodi = entity.SoliCodiRetiCodi;
            paramPotencia.CoregeCodi = null;
            paramPotencia.TrnpcTipoCasoAgrupado = "AGRVTA";
            paramPotencia.TrnPctTotalMwFija = entity.TrnPctTotalmwFija;
            paramPotencia.TrnPctHpMwFija = entity.TrnPctHpmwFija;
            paramPotencia.TrnPctHfpMwFija = entity.TrnPctHfpmwFija;
            paramPotencia.TrnPctTotalMwVariable = entity.TrnPctTotalmwVariable;
            paramPotencia.TrnPctHpMwFijaVariable = entity.TrnPctHpmwFijaVariable;
            paramPotencia.TrnPctHfpMwFijaVariable = entity.TrnPctHfpmwFijaVariable;
            paramPotencia.TrnPctComeObs = entity.TrnPctComeObs;
            paramPotencia.TrnPctUserNameIns = entity.UsuaCodi;
            paramPotencia.TrnPcEnvCodi = null;
            FactoryTransferencia.GetTrnPotenciaContratadaRepository().GenerarCargaDatos(paramPotencia);




            #endregion Potencias Contratadas
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        public void UpdateCodigoRetiroPotenciasContradasVTAAprobar(SolicitudCodigoDTO entity)
        {
            #region Potencias Contratadas



            TrnPotenciaContratadaDTO paramPotencia = new TrnPotenciaContratadaDTO();
            paramPotencia.PeriCodi = entity.PeridcCodi;
            paramPotencia.CoresoCodi = entity.SoliCodiRetiCodi;
            paramPotencia.CoregeCodi = null;
            paramPotencia.TrnpcTipoCasoAgrupado = "AGRVTA";
            paramPotencia.TrnPctTotalMwFija = entity.TrnPctTotalmwFija;
            paramPotencia.TrnPctHpMwFija = entity.TrnPctHpmwFija;
            paramPotencia.TrnPctHfpMwFija = entity.TrnPctHfpmwFija;
            paramPotencia.TrnPctTotalMwVariable = entity.TrnPctTotalmwVariable;
            paramPotencia.TrnPctHpMwFijaVariable = entity.TrnPctHpmwFijaVariable;
            paramPotencia.TrnPctHfpMwFijaVariable = entity.TrnPctHfpmwFijaVariable;
            paramPotencia.TrnPctComeObs = entity.TrnPctComeObs;
            paramPotencia.TrnPctUserNameIns = entity.UsuaCodi;
            paramPotencia.TrnPcEnvCodi = null;
            FactoryTransferencia.GetTrnPotenciaContratadaRepository().GenerarCargaDatosAprobar(paramPotencia);




            #endregion Potencias Contratadas
        }

        public void UpdateTipPotCodConsolidadoPeriodo(SolicitudCodigoDTO solcodDTO)
        {
            FactoryTransferencia.GetSolicitudCodigoRepository().UpdateTipPotCodConsolidadoPeriodo(solcodDTO);
        }

        public void UpdateTipPotCodCodigoRetiro(SolicitudCodigoDTO solcodDTO)
        {
            FactoryTransferencia.GetSolicitudCodigoRepository().UpdateTipPotCodCodigoRetiro(solcodDTO);
        }

        /// <summary>
        /// Permite grabar o actualizar un  SolicitudCodigoDTO en base a la entidad
        /// </summary>
        /// <param name="addDetalle">Entidad de SolicitudCodigoDTO</param>
        /// <param name="delDetalle">Entidad de SolicitudCodigoDTO</param>
        /// <returns>Retorna entero </returns>
        public int UpdateCodigoRetiro(SolicitudCodigoDTO entity, List<SolicitudCodigoDetalleDTO> addDetalle, List<SolicitudCodigoDetalleDTO> delDetalle, bool blnSolicitudCambio)
        {
            bool esPendiente = false;
            int result = 0;
            try
            {


                if (addDetalle.Count > 0)
                {
                    foreach (var item in addDetalle)
                    {
                        if (item.Coresdcodi == 0)
                        {
                            result = FactoryTransferencia.GetSolicitudCodigoDetalleRepository().Save(item);


                            if (result > 0)
                            {
                                //List<TrnPotenciaContratadaDTO> potenciasAsignadas = new List<TrnPotenciaContratadaDTO>();

                                //if (entity.TrnpcTipoCasoAgrupado == "AGRVTP")
                                //    potenciasAsignadas = entity.ListaPotenciaContratadas.Where(x => x.CoregeCodi == 0
                                // && x.BarrCodi == item.Barracodisum).ToList();

                                for (int i = 0; i < item.Coresdnumregistro; i++)
                                {
                                    esPendiente = true;

                                    CodigoGeneradoDTO gen = new CodigoGeneradoDTO();
                                    gen.Coresdcodi = result;
                                    gen.Coregeestado = "PAP";
                                    gen.Coregecodigovtp = null;
                                    gen.Coregeusuregistro = item.Coresdusuarioregistro;
                                    int idCodigoGenerado = FactoryTransferencia.GetSolicitudCodigoDetalleRepository().SaveSubDetalle(gen);


                                    if (entity.ListaPotenciaContratadas != null && entity.ListaPotenciaContratadas.Count > 0)
                                    {
                                        var rowPotenciaEncontrada = entity.ListaPotenciaContratadas[item.indexBarra];
                                        if (rowPotenciaEncontrada.BarrCodi == item.Barracodisum)
                                            rowPotenciaEncontrada.CoregeCodi = idCodigoGenerado;

                                        // si el numero registro es myor a 1 . le suma la posicion
                                        if (item.Coresdnumregistro > 1)
                                            item.indexBarra++;

                                    }


                                    //#region Potencias Contratada No Existente
                                    //foreach (var itemPot in potenciasAsignadas)
                                    //{
                                    //    itemPot.CoresoCodi = entity.SoliCodiRetiCodi;
                                    //    itemPot.CoregeCodi = idCodigoGenerado;
                                    //    itemPot.TrnPctUserNameIns = entity.UsuaCodi;
                                    //    itemPot.PeriCodi = entity.PeridcCodi;
                                    //    itemPot.TrnPctComeObs = "Potencias  contradas, que fueron registradas a nivela a nivel VTP";
                                    //    FactoryTransferencia.GetTrnPotenciaContratadaRepository().GenerarCargaDatos(itemPot);
                                    //}
                                    //#endregion Potencias Contratada Existente

                                }
                            }

                        }
                        else
                        {
                            CodigoGeneradoDTO oCodigo = new CodigoGeneradoDTO();
                            oCodigo.Coresdcodi = item.Coresdcodi;
                            oCodigo.Coregecodigovtp = null;
                            oCodigo.Coregeestado = item.coregeestado;
                            oCodigo.Coregeusuregistro = item.Coresdusuarioregistro;
                            //Obtiene el codigo generado al ser registrado
                            int idCodigoGenerado = FactoryTransferencia.GetSolicitudCodigoDetalleRepository().SaveSubDetalle(oCodigo);
                            if (idCodigoGenerado > 0)
                            {
                                int nroreg = FactoryTransferencia.GetSolicitudCodigoDetalleRepository().ObtenerNroCodigosGenerados(item.Coresdcodi);
                                item.Coresdnumregistro = nroreg;
                                result = FactoryTransferencia.GetSolicitudCodigoDetalleRepository().Update(item);
                                if (entity.ListaPotenciaContratadas != null)
                                {
                                    var rowPotenciaEncontrada = entity.ListaPotenciaContratadas[item.indexBarra];

                                    if (rowPotenciaEncontrada.BarrCodi == item.Barracodisum)
                                        rowPotenciaEncontrada.CoregeCodi = idCodigoGenerado;

                                }
                                //#region Potencias Contratada No Existente
                                //foreach (var itemPot in entity.ListaPotenciaContratadas.Where(x => x.CoregeCodi == 0))
                                //{
                                //    itemPot.CoresoCodi = entity.SoliCodiRetiCodi;
                                //    itemPot.CoregeCodi = idCodigoGenerado;
                                //    itemPot.TrnPctUserNameIns = entity.UsuaCodi;
                                //    itemPot.PeriCodi = entity.PeridcCodi;
                                //    itemPot.TrnPctComeObs = "Potencias  contradas, que fueron registradas a nivela a nivel VTP";
                                //    FactoryTransferencia.GetTrnPotenciaContratadaRepository().GenerarCargaDatos(itemPot);
                                //}
                                //#endregion Potencias Contratada Existente

                                result = 1;
                            }
                        }
                    }
                }

                if (entity.TrnpcTipoCasoAgrupado == "AGRVTP")
                {

                    if (entity.ListaPotenciaContratadas != null)
                    {
                        foreach (var itemPot in entity.ListaPotenciaContratadas)
                        {
                            itemPot.CoresoCodi = itemPot.CoresoCodi;
                            itemPot.CoregeCodi = itemPot.CoregeCodi;
                            itemPot.TrnPctUserNameIns = entity.UsuaCodi?? " ";
                            itemPot.TrnPctComeObs = "Potencias  contradas, que fueron registradas a nivela a nivel VTP";
                            itemPot.PeriCodi = entity.PeridcCodi;
                            if (blnSolicitudCambio)
                            {
                                FactoryTransferencia.GetTrnPotenciaContratadaRepository().GenerarCargaDatosAprobar(itemPot);
                            } else
                            {
                                FactoryTransferencia.GetTrnPotenciaContratadaRepository().GenerarCargaDatos(itemPot);
                            }
                            
                            result = 1;

                        }

                    }

                }

                //#region Potencias Contratada Existente
                //if (entity.ListaPotenciaContratadas != null)
                //{
                //    foreach (var item in entity.ListaPotenciaContratadas.Where(x => x.CoregeCodi > 0))
                //    {
                //        item.TrnPctUserNameIns = entity.UsuaCodi;
                //        item.PeriCodi = entity.PeridcCodi;
                //        item.TrnPctComeObs = "Potencias  contradas, que fueron registradas a nivela a nivel VTP";
                //        FactoryTransferencia.GetTrnPotenciaContratadaRepository().GenerarCargaDatos(item);
                //        result = 1;
                //    }

                //}
                //#endregion Potencias Contratada Existente


                #region Update TRNTIPOPOTENCIA

                SolicitudCodigoDTO solcodDTO = new SolicitudCodigoDTO
                {
                    PeridcCodi = entity.PeridcCodi,
                    CoresoCodi = entity.SoliCodiRetiCodi,
                    TrnpcTipoPotencia = entity.TrnpcTipoCasoAgrupado == "AGRVTA" ? 1 : 2
                };
                FactoryTransferencia.GetSolicitudCodigoRepository().UpdateTipPotCodConsolidadoPeriodo(solcodDTO);

                #endregion


                if (delDetalle != null)
                {
                    foreach (var item in delDetalle)
                    {
                        result = FactoryTransferencia.GetSolicitudCodigoDetalleRepository().DeleteGenerado(item.Coregecodigo, item.Coresdusuarioregistro, item.coregeestado);
                        if (result > 0)
                        {
                            int nroreg = FactoryTransferencia.GetSolicitudCodigoDetalleRepository().ObtenerNroCodigosGenerados(item.Coresdcodi);
                            item.Coresdnumregistro = nroreg;
                            result = FactoryTransferencia.GetSolicitudCodigoDetalleRepository().Update(item);
                        }
                    }

                }

                #region AuditoriaProceso
                VtpAuditoriaProcesoDTO objAuditoria = new VtpAuditoriaProcesoDTO();
                objAuditoria.Tipprocodi = (int)ETipoProcesoAuditoriaVTP.GestionCodigosEnvioInformacion;
                objAuditoria.Estdcodi = esPendiente == false ? (int)EVtpEstados.Activo : (int)EVtpEstados.Pendiente;
                objAuditoria.Audproproceso = "Auditoria.";
                objAuditoria.Audprodescripcion = string.Format("Extranet - se ha actualizado la información");
                objAuditoria.Audprousucreacion = entity.UsuaCodi;
                objAuditoria.Audprofeccreacion = DateTime.Now;
                int auditoria = new AuditoriaProcesoAppServicio().save(objAuditoria);
                if (auditoria == 0)
                    Logger.Error("Error Save Auditoria");
                #endregion AuditoriaProceso


            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                result = 0;
            }

            return result;
        }

        /// <summary>
        /// Desactivar potencias contratadas
        /// </summary>
        /// <param name="coresoCodi"></param>
        public void DesactivarPotenciasContratadas(int coresoCodi, int peridcCodi)
        {
            FactoryTransferencia.GetTrnPotenciaContratadaRepository().DesactivarPotenciasContratadas(coresoCodi, peridcCodi);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="coresoCodi"></param>
        /// <param name="coregeCodi"></param>
        /// <param name="peridcCodi"></param>
        public void DesactivarPotenciasPorBarrSum(int coresoCodi, int coregeCodi, int peridcCodi)
        {
            FactoryTransferencia.GetTrnPotenciaContratadaRepository().DesactivarPotenciasPorBarrSum(coresoCodi, coregeCodi, peridcCodi);
        }
        /// <summary>
        /// Permite obtener el CodigoRetiro en base al iCoReSoCodi
        /// </summary>
        /// <param name="iCoReSoCodi">Codigo de la tabla TRN_CODIGO_RETIRO_SOLICITUD</param>
        /// <returns>SolicitudCodigoDTO</returns>
        public SolicitudCodigoDTO GetByIdCodigoRetiro(int iCoReSoCodi, int periCodi)
        {
            try
            {
                SolicitudCodigoDTO resultado = new SolicitudCodigoDTO();
                resultado = FactoryTransferencia.GetSolicitudCodigoRepository().GetById(iCoReSoCodi);
                resultado.ListaPotenciaContratadas = FactoryTransferencia.GetTrnPotenciaContratadaRepository().GetPotenciasContratadas(iCoReSoCodi, periCodi);
                if (resultado.ListaPotenciaContratadas.Count > 0)
                    resultado.ListaPotenciaContratadas = resultado.ListaPotenciaContratadas.OrderBy(c => c.CoregeCodi).ToList();


                if (resultado.TrnpcTipoCasoAgrupado == "AGRVTA")
                {
                    TrnPotenciaContratadaDTO resultadoPotencia = resultado.ListaPotenciaContratadas.FirstOrDefault();
                    resultado.TrnPctHpmwFija = resultadoPotencia?.TrnPctHpMwFija;
                    resultado.TrnPctHfpmwFija = resultadoPotencia?.TrnPctHfpMwFija;
                    resultado.TrnPctTotalmwFija = resultadoPotencia?.TrnPctTotalMwFija;
                    resultado.TrnPctHpmwFijaVariable = resultadoPotencia?.TrnPctHpMwFijaVariable;
                    resultado.TrnPctHfpmwFijaVariable = resultadoPotencia?.TrnPctHfpMwFijaVariable;
                    resultado.TrnPctTotalmwVariable = resultadoPotencia?.TrnPctTotalMwVariable;
                    resultado.esPrimerRegistro = resultadoPotencia?.esPrimerRegistro;
                }
                return resultado;
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                return null;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="coresoCodi"></param>
        /// <param name="periCodi"></param>
        /// <returns></returns>
        public List<TrnPotenciaContratadaDTO> ListaPotenciaContratadas(int coresoCodi, int periCodi)
        {
            var resultado = FactoryTransferencia.GetTrnPotenciaContratadaRepository().GetPotenciasContratadas(coresoCodi, periCodi);

            return resultado.Where(x => x.CoregeCodi > 0).ToList();

        }

        /// <summary>
        /// Permite realizar búsquedas de CodigoRetiroSolicitud en base a cualquiera de los parametros
        /// </summary>
        /// <param name="sEmprNomb">Nombre de la Empresa</param>        
        /// <param name="sTipUsuNombre">Nombre del Tipo de Usuario</param>        
        /// <param name="sTipConNombre">Nombre del Tipo de Contrato</param>        
        /// <param name="sBarrBarraTransferencia">Nombre de la Barra de Transferencia</param>        
        /// <param name="sCliEmprNomb">Nombre del Cliente</param>        
        /// <param name="dCoReSoFechaInicio">Fecha en que inicia el código de retiro solicitado, puede ser nulo</param>        
        /// <param name="dCoReSoFechaFin">Fecha en que concluye el código de retiro solicitado, puede ser nulo</param>        
        /// <param name="SoliCodiRetiObservacion">Solicitud de baja de código de retiro</param>        
        /// <param name="sCoReSotEstado">Estado en que se encuentra el Código de Retiro solicitado</param>   
        /// <param name="NroPagina">Estado en que se encuentra el Código de Retiro solicitado</param>       
        /// <param name="PageSizeCodigoRetiro">Estado en que se encuentra el Código de Retiro solicitado</param>       
        /// <returns>Lista de SolicitudCodigoDTO</returns>
        public List<SolicitudCodigoDTO> ListarCodigoRetiro(string sEmprNomb, string sTipUsuNombre, string sTipConNombre, string sBarrBarraTransferencia, string sCliEmprNomb, DateTime? dCoReSoFechaInicio, DateTime? dCoReSoFechaFin, string SoliCodiRetiObservacion, string sCoReSotEstado, int? pericodi, int NroPagina, int PageSizeCodigoRetiro)
        {
            try
            {
                return FactoryTransferencia.GetSolicitudCodigoRepository().ListarCodigoRetiroPaginado(sEmprNomb, sTipUsuNombre, sTipConNombre, sBarrBarraTransferencia, sCliEmprNomb, dCoReSoFechaInicio, dCoReSoFechaFin, SoliCodiRetiObservacion, sCoReSotEstado, pericodi, NroPagina, PageSizeCodigoRetiro);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                return null;
            }
        }
        /// <summary>
        /// Permite realizar búsquedas de CodigoRetiroSolicitud en base a cualquiera de los parametros
        /// </summary>
        /// <param name="sEmprNomb">Nombre de la Empresa</param>        
        /// <param name="sTipUsuNombre">Nombre del Tipo de Usuario</param>        
        /// <param name="sTipConNombre">Nombre del Tipo de Contrato</param>        
        /// <param name="sBarrBarraTransferencia">Nombre de la Barra de Transferencia</param>        
        /// <param name="sCliEmprNomb">Nombre del Cliente</param>        
        /// <param name="dCoReSoFechaInicio">Fecha en que inicia el código de retiro solicitado, puede ser nulo</param>        
        /// <param name="dCoReSoFechaFin">Fecha en que concluye el código de retiro solicitado, puede ser nulo</param>        
        /// <param name="SoliCodiRetiObservacion">Solicitud de baja de código de retiro</param>        
        /// <param name="sCoReSotEstado">Estado en que se encuentra el Código de Retiro solicitado</param>   
        /// <param name="pericodi">Periodo</param>   
        /// <param name="NroPagina">Estado en que se encuentra el Código de Retiro solicitado</param>       
        /// <param name="PageSizeCodigoRetiro">Estado en que se encuentra el Código de Retiro solicitado</param>       
        /// <returns>Lista de SolicitudCodigoDTO</returns>
        public List<SolicitudCodigoDTO> ListarExportacionCodigoRetiro(string sEmprNomb, string sTipUsuNombre, string sTipConNombre, string sBarrBarraTransferencia, 
                                                                      string sCliEmprNomb, DateTime? dCoReSoFechaInicio, DateTime? dCoReSoFechaFin, 
                                                                      string SoliCodiRetiObservacion, string sCoReSotEstado, int? pericodi, int NroPagina, int PageSizeCodigoRetiro)
        {
            try
            {
                return FactoryTransferencia.GetSolicitudCodigoRepository().ListarExportacionCodigoRetiro(sEmprNomb, sTipUsuNombre, sTipConNombre, sBarrBarraTransferencia, 
                                                                                                         sCliEmprNomb, dCoReSoFechaInicio, dCoReSoFechaFin, SoliCodiRetiObservacion, 
                                                                                                         sCoReSotEstado, pericodi, NroPagina, PageSizeCodigoRetiro);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                return null;
            }
        }


        /// <summary>
        /// Permite realizar búsquedas de CodigoRetiroSolicitud en base a cualquiera de los parametros
        /// </summary>
        /// <param name="sEmprNomb">Nombre de la Empresa</param>        
        /// <param name="sTipUsuNombre">Nombre del Tipo de Usuario</param>        
        /// <param name="sTipConNombre">Nombre del Tipo de Contrato</param>        
        /// <param name="sBarrBarraTransferencia">Nombre de la Barra de Transferencia</param>        
        /// <param name="sCliEmprNomb">Nombre del Cliente</param>        
        /// <param name="dCoReSoFechaInicio">Fecha en que inicia el código de retiro solicitado, puede ser nulo</param>        
        /// <param name="dCoReSoFechaFin">Fecha en que concluye el código de retiro solicitado, puede ser nulo</param>        
        /// <param name="SoliCodiRetiObservacion">Solicitud de baja de código de retiro</param>        
        /// <param name="sCoReSotEstado">Estado en que se encuentra el Código de Retiro solicitado</param>        
        /// <returns>Numero de filas de la consulta</returns>
        public int ObtenerNroFilasCodigoRetiro(string sEmprNomb, string sTipUsuNombre, string sTipConNombre, string sBarrBarraTransferencia, string sCliEmprNomb, DateTime? dCoReSoFechaInicio, DateTime? dCoReSoFechaFin, string SoliCodiRetiObservacion, string sCoReSotEstado, int? pericodi)
        {
            return FactoryTransferencia.GetSolicitudCodigoRepository().ObtenerNroRegistros(sEmprNomb, sTipUsuNombre, sTipConNombre, sBarrBarraTransferencia, sCliEmprNomb, dCoReSoFechaInicio, dCoReSoFechaFin, SoliCodiRetiObservacion, sCoReSotEstado, pericodi);
        }

        /// <summary>
        /// Agrupa las potencias contratadas de la solicitud de codigos 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public ResultadoDTO<int> GenerarPotenciasAgrupadas(string userName, List<TrnPotenciaContratadaDTO> entity, bool esIntranet = false)
        {

            ResultadoDTO<int> resultado = new ResultadoDTO<int>();
            string error = "";
            int id = 0;

            StringBuilder detalleAuditoria = new StringBuilder();
            using (TransactionScope scope = new TransactionScope())
            {
                try
                {
                    List<TrnPotenciaContratadaDTO> entityAuxiliar = new List<TrnPotenciaContratadaDTO>();

                    int? coresoCodigAuxiliar = null;
                    int? codigoGrupoAgrupado = null;
                    int? codigoGrupoAgrupadoSgteMes = null;

                    int totalVTAS = entity.Where(x => x.TrnpcTipoCasoAgrupado == "AGRVTA").Count();

                    List<TrnPotenciaContratadaDTO> listaCodigosVTAContenido = new List<TrnPotenciaContratadaDTO>();
                    //foreach (var lista in entity.Where(x => x.TrnpcTipoCasoAgrupado == "AGRVTA").Select(r => r.CoresoCodi).Distinct().ToList())
                    //{
                    //    var listaCodigosAsociados = entity.Where(c => c.CoresoCodi == lista).ToList();
                    //    for (int i = listaCodigosAsociados.Count - 1; i >= 0; i--)
                    //    {
                    //        if (i > 0)
                    //        {
                    //            listaCodigosAsociados.RemoveAt(i);
                    //        }
                    //    }
                    //}

                    //Obtener los datos de periodo de valorizacion
                    PeriodoDTO entidadPeriodo = (new PeriodoAppServicio()).ListPeriodo().OrderByDescending(m => m.PeriCodi).ToList().FirstOrDefault();
                    int intPeriodoActual = 0;
                    int intPeriodoAnterior = 0;
                    string strPeriodoDesc = string.Empty;
                    string strErrorValidacion = string.Empty;
                    if (entidadPeriodo!=null)
                    {
                        intPeriodoActual = entidadPeriodo.PeriCodi;
                        intPeriodoAnterior = intPeriodoActual - 1;
                        strPeriodoDesc = entidadPeriodo.PeriNombre;
                    }
                    //Validacion de las potencias contratadas en el periodo m+1
                    foreach (var item in entity)
                    {
                        SolicitudCodigoDTO solicitud = FactoryTransferencia.GetSolicitudCodigoRepository().GetById(item.CoresoCodi);

                        if ((intPeriodoAnterior == item.PeriCodi)  && (solicitud.SoliCodiRetiEstado=="ACT")) //Solo las solicitudes con estado Activo.
                        {
                            string strBarraTransferencia = string.Empty;
                            List<TrnPotenciaContratadaDTO> listaPotenciasContratadasOrig = new List<TrnPotenciaContratadaDTO>();
                            listaPotenciasContratadasOrig = FactoryTransferencia.GetTrnPotenciaContratadaRepository().GetPotenciasContratadas(item.CoresoCodi, item.PeriCodi);
                            if (listaPotenciasContratadasOrig!=null)
                            {
                                strBarraTransferencia = listaPotenciasContratadasOrig.FirstOrDefault().TrnPctPtoSumins;
                            }

                            List<TrnPotenciaContratadaDTO> listaPotenciasContratadas = new List<TrnPotenciaContratadaDTO>();
                            listaPotenciasContratadas = FactoryTransferencia.GetTrnPotenciaContratadaRepository().GetPotenciasContratadas(item.CoresoCodi, intPeriodoActual);
                            if (listaPotenciasContratadas.Count==0)
                            {
                                strErrorValidacion = strErrorValidacion +  "La barra " + strBarraTransferencia + " tiene valores sin ingresar potencias para el periodo de declaracion siguiente: "+ strPeriodoDesc + ".\n";
                            }

                        }
                    }
                    if (!string.IsNullOrEmpty(strErrorValidacion))
                    {
                        resultado.EsCorrecto = -1;
                        throw new Exception(strErrorValidacion);
                    }



                    //Obtiene los codigos VTA asociados

                    if (totalVTAS > 0)
                    {
                        int PeriCodi = entity.FirstOrDefault().PeriCodi;
                        List<int> coresoCodi = entity.Where(x => x.TrnpcTipoCasoAgrupado == "AGRVTA").Select(r => r.CoresoCodi).Distinct().ToList();

                        listaCodigosVTAContenido = FactoryTransferencia.GetTrnPotenciaContratadaRepository().GetListaGrupoAsociadoVTA(coresoCodi, PeriCodi);

                        if (listaCodigosVTAContenido.Count > 0)
                        {
                            foreach (var itemCodigo in coresoCodi)
                            {
                                int index = -1;
                                var listarCotenidoVTAAuxiliar = listaCodigosVTAContenido.Where(c => c.CoresoCodiFirst == itemCodigo).ToList();

                                if (listarCotenidoVTAAuxiliar.Count > 0)
                                {
                                    foreach (var itemCodigoAuxiliar in entity.Where(x => x.TrnpcTipoCasoAgrupado == "AGRVTA" && x.CoresoCodi == itemCodigo))
                                    {
                                        index++;
                                        itemCodigoAuxiliar.CoresoCodi = listarCotenidoVTAAuxiliar[index].CoresoCodi;
                                    }
                                }

                            }

                            foreach (var item in entity)
                            {
                                FactoryTransferencia.GetTrnPotenciaContratadaRepository().DesagruparPotencias(item, 1);
                            }
                        }
                    }


                    int totalTipoAgrupados = entity.Select(x => x.TrnpcTipoCasoAgrupado).Distinct().Count();
                    if (totalTipoAgrupados >= 2)
                    {
                        resultado.EsCorrecto = -1;
                        error = "Solo debe agrupar barra transferencia o barra suministros.";
                    }
                    else
                    {

                        foreach (var item in entity)
                        {
                            TrnPotenciaContratadaDTO potenciaResultado = new TrnPotenciaContratadaDTO();

                            bool esAgrupado = false;
                            if (item.TrnpcTipoCasoAgrupado == "AGRVTP" && item.CoregeCodi != null)
                            {
                                esAgrupado = true;
                                if (coresoCodigAuxiliar != item.CoresoCodi)
                                    codigoGrupoAgrupado = null;
                                potenciaResultado = entity.Where(x => x.CoresoCodi == item.CoresoCodi && x.TrnpcNumOrd == 1).ToList().FirstOrDefault();
                            }
                            else if (item.TrnpcTipoCasoAgrupado == "AGRVTA" && item.CoregeCodi == null)
                            {
                                esAgrupado = true;
                                potenciaResultado = entity.Where(x => x.TrnpcTipoCasoAgrupado == "AGRVTA").ToList().FirstOrDefault();
                            }

                            if (esAgrupado)
                            {
                                coresoCodigAuxiliar = item.CoresoCodi;
                                // Obtiene del padre
                                var itemFirst = potenciaResultado;
                                item.CoresoCodiFirst = itemFirst.CoresoCodi;
                                item.CoregeCodiFirst = itemFirst.CoregeCodi;
                                item.TrnpCagrp = codigoGrupoAgrupado;



                                codigoGrupoAgrupado = FactoryTransferencia.GetTrnPotenciaContratadaRepository().GenerarPotenciasAgrupadas(item);

                                
                                detalleAuditoria.AppendLine(
                                    string.Format("CoresoCodi:{0};CoregeCodi:{1};TrnpcTipoCasoAgrupado:{2};PeriCodi:{3};Orden:{4}",
                               itemFirst.CoresoCodi,
                               itemFirst.CoregeCodi,
                                item.TrnpcTipoCasoAgrupado,
                                item.PeriCodi,
                                item.TrnpcNumOrd
                                ));


                                SolicitudCodigoDTO solicitudDTO = FactoryTransferencia.GetSolicitudCodigoRepository().GetById(item.CoresoCodi);

                                if ((intPeriodoAnterior == item.PeriCodi) && (solicitudDTO.SoliCodiRetiEstado == "ACT"))
                                {
                                    item.PeriCodi = intPeriodoActual;
                                    item.TrnpCagrp = codigoGrupoAgrupadoSgteMes;
                                    codigoGrupoAgrupadoSgteMes = FactoryTransferencia.GetTrnPotenciaContratadaRepository().GenerarPotenciasAgrupadas(item);

                                    detalleAuditoria.AppendLine(
                                    string.Format("CoresoCodi:{0};CoregeCodi:{1};TrnpcTipoCasoAgrupado:{2};PeriCodi:{3};Orden:{4}",
                               itemFirst.CoresoCodi,
                               itemFirst.CoregeCodi,
                                item.TrnpcTipoCasoAgrupado,
                                item.PeriCodi,
                                item.TrnpcNumOrd
                                ));

                                }

                                if (codigoGrupoAgrupado == null)
                                {
                                    error = "Ha ocurrido un erro al agrupar las potencias.";
                                    break;
                                }
                            }

                        }

                    }

                    if (string.IsNullOrEmpty(error))
                    {
                        string mensajeTipo = esIntranet == true ? "Intranet" : "Extranet";
                        #region AuditoriaProceso
                        VtpAuditoriaProcesoDTO objAuditoria = new VtpAuditoriaProcesoDTO();
                        objAuditoria.Tipprocodi = (int)ETipoProcesoAuditoriaVTP.GestionCodigosEnvioInformacion;
                        objAuditoria.Estdcodi = (int)EVtpEstados.AgruparFilas;
                        objAuditoria.Audproproceso = "Auditoria.";
                        objAuditoria.Audprodescripcion = string.Format(mensajeTipo + " - se ha agrupado la siguiente información:\n{0}", detalleAuditoria.ToString());
                        objAuditoria.Audprousucreacion = userName;
                        objAuditoria.Audprofeccreacion = DateTime.Now;
                        int auditoria = new AuditoriaProcesoAppServicio().save(objAuditoria);
                        if (auditoria == 0)
                            Logger.Error("Error Save Auditoria");
                        #endregion AuditoriaProceso

                        scope.Complete();

                    }
                    else
                    {
                        id = -1;
                        scope.Dispose();
                    }
                }
                catch (Exception ex)
                {
                    id = -1;
                    scope.Dispose();
                    error = ex.Message.ToString();
                }
            }

            resultado.Data = id;
            resultado.Mensaje = error;
            return resultado;
        }
        /// <summary>
        /// Generar los codigos agrupados reservados para evitar la duplicidad en las agrupaciones de mi tabla potencia contratada
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public ResultadoDTO<int> GenerarCodigosAgrupadosReservados(string userName)
        {
            ResultadoDTO<int> resultado = new ResultadoDTO<int>();
            try
            {
                resultado.Data = FactoryTransferencia.GetTrnPotenciaContratadaRepository().GenerarCodigosAgrupadosReservados(userName) ?? 0;
                if (resultado.Data == 0)
                {
                    resultado.EsCorrecto = -1;
                    resultado.Mensaje = "Ha ocurrido un error en la generación del codigo agrupado reservado";
                }
            }
            catch (Exception ex)
            {
                resultado.EsCorrecto = -1;
                resultado.Mensaje = ex.Message;
            }
            return resultado;

        }

        /// <summary>
        /// Desagrupa las potencias contratadas de la solicitud de codigos 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public ResultadoDTO<int> DesagruparPotencias(string userName, List<TrnPotenciaContratadaDTO> entity, bool esIntranet = false)
        {

            ResultadoDTO<int> resultado = new ResultadoDTO<int>();
            string error = "";
            int id = 0;

            StringBuilder detalleAuditoria = new StringBuilder();
            using (TransactionScope scope = new TransactionScope())
            {
                try
                {
                    //Obtener los datos de periodo de valorizacion
                    PeriodoDTO entidadPeriodo = (new PeriodoAppServicio()).ListPeriodo().OrderByDescending(m => m.PeriCodi).ToList().FirstOrDefault();
                    int intPeriodoActual = 0;
                    int intPeriodoAnterior = 0;
                    if (entidadPeriodo != null)
                    {
                        intPeriodoActual = entidadPeriodo.PeriCodi;
                        intPeriodoAnterior = intPeriodoActual - 1;
                    }

                    //List<TrnPotenciaContratadaDTO> entityAuxiliar = new List<TrnPotenciaContratadaDTO>();
                    foreach (var item in entity)
                    {
                        FactoryTransferencia.GetTrnPotenciaContratadaRepository().DesagruparPotencias(item);

                        detalleAuditoria.AppendLine(string.Format("CoresoCodi:{0};CoregeCodi:{1};TrnpcTipoCasoAgrupado:{2};PeriCodi:{3}",
                            item.CoresoCodi,
                            item.CoregeCodi,
                            item.TrnpcTipoCasoAgrupado,
                            item.PeriCodi
                            ));

                        if (intPeriodoAnterior == item.PeriCodi)
                        {
                            item.PeriCodi = intPeriodoActual;
                            FactoryTransferencia.GetTrnPotenciaContratadaRepository().DesagruparPotencias(item);

                            detalleAuditoria.AppendLine(string.Format("CoresoCodi:{0};CoregeCodi:{1};TrnpcTipoCasoAgrupado:{2};PeriCodi:{3}",
                           item.CoresoCodi,
                           item.CoregeCodi,
                           item.TrnpcTipoCasoAgrupado,
                           item.PeriCodi
                           ));

                        }
                    }
                    if (string.IsNullOrEmpty(error))
                    {

                        string mensajeTipo = esIntranet == true ? "Intranet" : "Extranet";

                        #region AuditoriaProceso
                        VtpAuditoriaProcesoDTO objAuditoria = new VtpAuditoriaProcesoDTO();
                        objAuditoria.Tipprocodi = (int)ETipoProcesoAuditoriaVTP.GestionCodigosEnvioInformacion;
                        objAuditoria.Estdcodi = (int)EVtpEstados.DesagruparFilas;
                        objAuditoria.Audproproceso = "Auditoria.";
                        objAuditoria.Audprodescripcion = string.Format(mensajeTipo + " - se ha desagrupado la siguiente información:\n{0}", detalleAuditoria.ToString());
                        objAuditoria.Audprousucreacion = userName;
                        objAuditoria.Audprofeccreacion = DateTime.Now;
                        int auditoria = new AuditoriaProcesoAppServicio().save(objAuditoria);
                        if (auditoria == 0)
                            Logger.Error("Error Save Auditoria");
                        #endregion AuditoriaProceso


                        scope.Complete();
                    }
                    else
                    {
                        id = -1;
                        scope.Dispose();
                    }
                }
                catch (Exception ex)
                {
                    id = -1;
                    scope.Dispose();
                    error = ex.Message.ToString();
                }
            }

            resultado.Data = id;
            resultado.Mensaje = error;
            return resultado;
        }


        /// <summary>
        /// Realiza la carga de datos
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public ResultadoDTO<int> GenerarCargaDatosExcel(string userName, List<TrnPotenciaContratadaDTO> entity, bool esIntranet = false)
        {
            ResultadoDTO<int> resultado = new ResultadoDTO<int>();
            string error = "";
            int id = 0;
            int numeroFilaError = 0;
            StringBuilder detalleAuditoria = new StringBuilder();
            using (TransactionScope scope = new TransactionScope())
            {
                try
                {
                    int? codigoEnvio = null;
                    int index = -1;
                    foreach (var item in entity)
                    {
                        if (item.CoresoCodi == 0 || (item.CoresoCodi == 0 && item.CoregeCodi == null))
                            continue;
                        numeroFilaError++;
                        index++;
                        item.TrnPctUserNameIns = userName;
                        if (index == 0)
                            codigoEnvio = FactoryTransferencia.GetTrnPotenciaContratadaRepository().GenerarCargaDatosExcelEnvio(item);

                        if (codigoEnvio == null)
                        {
                            error = "Error al generar la cabecera del envió.";
                            break;
                        }
                        else
                        {
                            SolicitudCodigoDTO objSolicitudCodigo = new SolicitudCodigoDTO();
                            objSolicitudCodigo.SoliCodiRetiCodi = item.CoresoCodi;
                            objSolicitudCodigo.SoliCodiRetiDescripcion = item.TrnPctComeObs;

                            item.TrnPcEnvCodi = Convert.ToInt32(codigoEnvio);
                            FactoryTransferencia.GetTrnPotenciaContratadaRepository().GenerarCargaDatosExcel(item);
                            FactoryTransferencia.GetSolicitudCodigoRepository().UpdateObservacion(objSolicitudCodigo);
                        }
                    }
                    if (string.IsNullOrEmpty(error))
                    {


                        #region AuditoriaProceso
                        string mensajeTipo = esIntranet == true ? "Intranet" : "Extranet";


                        VtpAuditoriaProcesoDTO objAuditoria = new VtpAuditoriaProcesoDTO();
                        objAuditoria.Tipprocodi = (int)ETipoProcesoAuditoriaVTP.GestionCodigosEnvioInformacion;
                        objAuditoria.Estdcodi = (int)EVtpEstados.EnviarDatos;
                        objAuditoria.Audproproceso = "Envio de informacion.";
                        objAuditoria.Audprodescripcion = string.Format(mensajeTipo + "-{0}", detalleAuditoria.AppendLine(string.Format("Se ha enviado la información.")).ToString());
                        objAuditoria.Audprousucreacion = userName;
                        objAuditoria.Audprofeccreacion = DateTime.Now;
                        int auditoria = new AuditoriaProcesoAppServicio().save(objAuditoria);
                        if (auditoria == 0)
                            Logger.Error("Error Save Auditoria");
                        #endregion AuditoriaProceso


                        scope.Complete();

                    }
                    else
                    {
                        id = -1;
                        scope.Dispose();
                    }
                }
                catch (Exception ex)
                {
                    id = -1;
                    resultado.EsCorrecto = -1;
                    error = ex.Message.ToString() + numeroFilaError.ToString();
                    scope.Dispose();
                }
            }
            resultado.Data = id;
            resultado.Mensaje = error;
            return resultado;
        }
        /// <summary>
        /// Exporta los datos de la grilla 
        /// </summary>
        /// <param name="empresa"></param>
        /// <param name="parametro"></param>
        /// <returns></returns>
        public string ExportarDatosCodigoRetiro(string empresa, List<SolicitudCodigoDTO> parametro)
        {
            return ExcelDocument.GenerarFormatoPotenciasContradas(empresa, parametro);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="empresa"></param>
        /// <param name="parametro"></param>
        /// <returns></returns>
        public string ExportarDatosCodigoRetiroIntranet(string empresa, List<CodigoRetiroDTO> parametro)
        {
            return ExcelDocument.GenerarFormatoPotenciasContradasIntranet(empresa, parametro);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Base64"></param>
        /// <param name="base64Compare"></param>
        /// <param name="hoja"></param>
        /// <returns></returns>
        public ResultadoDTO<List<SolicitudCodigoDetalleDTO>> ListarCodigoRetiroFromExcel(string Base64, string base64Compare, int hoja)
        {
            ResultadoDTO<List<SolicitudCodigoDetalleDTO>> resultado = new ResultadoDTO<List<SolicitudCodigoDetalleDTO>>();
            resultado.Data = new List<SolicitudCodigoDetalleDTO>();

            bool esFormato = false;
            int fila = -1;

            ResultadoDTO<DataSet> resultadoExcel = ExcelDocument.GeneraDatasetSolicitudCodigoRetiro(Base64, hoja);
            ResultadoDTO<DataSet> resultadoExcelCompare = ExcelDocument.GeneraDatasetSolicitudCodigoRetiro(base64Compare, hoja);

            List<string> posicioneComparar = new List<string>();
            List<string> posicioneOrigen = new List<string>();

            for (int i = 0; i < resultadoExcelCompare.Data.Tables[0].Rows.Count; i++)
            {
                if (i > 3)
                {
                    string comparar = resultadoExcelCompare.Data.Tables[0].Rows[i][19].ToString();
                    if (comparar != null && comparar != "null")
                        posicioneComparar.Add(comparar);
                }
            }

            for (int i = 0; i < resultadoExcel.Data.Tables[0].Rows.Count; i++)
            {
                if (i > 3)
                {
                    string comparar = resultadoExcel.Data.Tables[0].Rows[i][19].ToString();
                    if (comparar != null && comparar != "null")
                        posicioneOrigen.Add(comparar);
                }
            }

            string compararBase = string.Join(",", posicioneComparar.Where(x => !string.IsNullOrEmpty(x)).Distinct());
            string origenBase = string.Join(",", posicioneOrigen.Where(x => !string.IsNullOrEmpty(x)).Distinct());


            if (origenBase == compararBase)
                esFormato = true;


            if (!esFormato)
            {

                resultado.EsCorrecto = -2;
                resultado.Mensaje = "Error el formato subido no es compatible con la información de la grilla.";
                return resultado;
            }

            DataSet dts = null;
            if (resultadoExcel.EsCorrecto < 0)
            {
                resultado.EsCorrecto = -1;
                resultado.Mensaje = resultadoExcel.Mensaje;
            }

            dts = resultadoExcel.Data;

            foreach (DataRow dtRow in dts.Tables[0].Rows)
            {
                fila++;
                if (fila > 3)
                {


                    SolicitudCodigoDetalleDTO detalle = new SolicitudCodigoDetalleDTO();
                    detalle.Coresocodi = dtRow[7] == "null" ? 0 : Convert.ToInt32(dtRow[7].ToString());
                    detalle.Codigovta = dtRow[8].ToString();
                    detalle.Coregecodigo = dtRow[10] == "null" ? 0 : Convert.ToInt32(dtRow[10]?.ToString());
                    detalle.Codigovtp = dtRow[11].ToString();

                    detalle.PotenciaContratadaDTO = new SolicitudCodigoPotenciaContratadaDTO();
                    if ((dtRow[12]?.ToString() != "error"))
                        detalle.PotenciaContratadaDTO.PotenciaContrTotalFija = string.IsNullOrEmpty(dtRow[12]?.ToString()) ? null : (decimal?)UtilTransfPotencia.ValidarNumero(dtRow[12].ToString()); ;
                    if ((dtRow[13]?.ToString() != "error"))
                        detalle.PotenciaContratadaDTO.PotenciaContrHPFija = string.IsNullOrEmpty(dtRow[13]?.ToString()) ? null : (decimal?)UtilTransfPotencia.ValidarNumero(dtRow[13].ToString()); ;
                    if ((dtRow[14]?.ToString() != "error"))
                        detalle.PotenciaContratadaDTO.PotenciaContrHFPFija = string.IsNullOrEmpty(dtRow[14]?.ToString()) ? null : (decimal?)UtilTransfPotencia.ValidarNumero(dtRow[14].ToString()); ;
                    if ((dtRow[15]?.ToString() != "error"))
                        detalle.PotenciaContratadaDTO.PotenciaContrTotalVar = string.IsNullOrEmpty(dtRow[15]?.ToString()) ? null : (decimal?)UtilTransfPotencia.ValidarNumero(dtRow[15].ToString()); ;
                    if ((dtRow[16]?.ToString() != "error"))
                        detalle.PotenciaContratadaDTO.PotenciaContrHPVar = string.IsNullOrEmpty(dtRow[16]?.ToString()) ? null : (decimal?)UtilTransfPotencia.ValidarNumero(dtRow[16].ToString()); ;
                    if ((dtRow[17]?.ToString() != "error"))
                        detalle.PotenciaContratadaDTO.PotenciaContrHFPVar = string.IsNullOrEmpty(dtRow[17]?.ToString()) ? null : (decimal?)UtilTransfPotencia.ValidarNumero(dtRow[17].ToString()); ;
                    if ((dtRow[18]?.ToString() != "error"))
                        detalle.PotenciaContratadaDTO.PotenciaContrObservacion = dtRow[18]?.ToString();
                    resultado.Data.Add(detalle);
                }
            }



            return resultado;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Base64"></param>
        /// <param name="base64Compare"></param>
        /// <param name="hoja"></param>
        /// <returns></returns>
        public ResultadoDTO<List<CodigoRetiroGeneradoDTO>> ListarCodigoRetiroFromExcelIntranet(string Base64, string base64Compare, int hoja)
        {
            ResultadoDTO<List<CodigoRetiroGeneradoDTO>> resultado = new ResultadoDTO<List<CodigoRetiroGeneradoDTO>>();
            resultado.Data = new List<CodigoRetiroGeneradoDTO>();

            bool esFormato = false;
            int fila = -1;

            ResultadoDTO<DataSet> resultadoExcel = ExcelDocument.GeneraDatasetSolicitudCodigoRetiro(Base64, hoja, true);
            ResultadoDTO<DataSet> resultadoExcelCompare = ExcelDocument.GeneraDatasetSolicitudCodigoRetiro(base64Compare, hoja, true);

            List<string> posicioneComparar = new List<string>();
            List<string> posicioneOrigen = new List<string>();

            for (int i = 0; i < resultadoExcelCompare.Data.Tables[0].Rows.Count; i++)
            {
                if (i > 3)
                {
                    string comparar = resultadoExcelCompare.Data.Tables[0].Rows[i][22].ToString();
                    if (comparar != null && comparar != "null")
                        posicioneComparar.Add(comparar);
                }
            }

            for (int i = 0; i < resultadoExcel.Data.Tables[0].Rows.Count; i++)
            {
                if (i > 3)
                {
                    string comparar = resultadoExcel.Data.Tables[0].Rows[i][22].ToString();
                    if (comparar != null && comparar != "null")
                        posicioneOrigen.Add(comparar);
                }
            }



            string compararBase = string.Join(",", posicioneComparar.Where(x => !string.IsNullOrEmpty(x)).Distinct());
            string origenBase = string.Join(",", posicioneOrigen.Where(x => !string.IsNullOrEmpty(x)).Distinct());



            if (compararBase == origenBase)
                esFormato = true;


            if (!esFormato)
            {

                resultado.EsCorrecto = -2;
                resultado.Mensaje = "Error el formato subido no es compatible con la información de la grilla.";
                return resultado;
            }



            DataSet dts = null;
            if (resultadoExcel.EsCorrecto < 0)
            {
                resultado.EsCorrecto = -1;
                resultado.Mensaje = resultadoExcel.Mensaje;
            }

            dts = resultadoExcel.Data;

            foreach (DataRow dtRow in dts.Tables[0].Rows)
            {
                fila++;
                if (fila > 3)
                {


                    CodigoRetiroGeneradoDTO detalle = new CodigoRetiroGeneradoDTO();
                    detalle.CoresoCodi = dtRow[8] == "null" ? 0 : Convert.ToInt32(dtRow[8].ToString());
                    detalle.CoresoCodigoVTA = dtRow[10].ToString();
                    detalle.CoregeCodi = dtRow[11] == "null" ? 0 : Convert.ToInt32(dtRow[11]?.ToString());
                    detalle.CoregeCodVTP = dtRow[13].ToString();

                    detalle.PotenciaContratadaDTO = new SolicitudCodigoPotenciaContratadaDTO();
                    if ((dtRow[15]?.ToString() != "error"))
                        detalle.PotenciaContratadaDTO.PotenciaContrTotalFija = string.IsNullOrEmpty(dtRow[15]?.ToString()) ? null : (decimal?)UtilTransfPotencia.ValidarNumero(dtRow[15].ToString()); ;
                    if ((dtRow[16]?.ToString() != "error"))
                        detalle.PotenciaContratadaDTO.PotenciaContrHPFija = string.IsNullOrEmpty(dtRow[16]?.ToString()) ? null : (decimal?)UtilTransfPotencia.ValidarNumero(dtRow[16].ToString()); ;
                    if ((dtRow[17]?.ToString() != "error"))
                        detalle.PotenciaContratadaDTO.PotenciaContrHFPFija = string.IsNullOrEmpty(dtRow[17]?.ToString()) ? null : (decimal?)UtilTransfPotencia.ValidarNumero(dtRow[17].ToString()); ;
                    if ((dtRow[18]?.ToString() != "error"))
                        detalle.PotenciaContratadaDTO.PotenciaContrTotalVar = string.IsNullOrEmpty(dtRow[18]?.ToString()) ? null : (decimal?)UtilTransfPotencia.ValidarNumero(dtRow[18].ToString()); ;
                    if ((dtRow[19]?.ToString() != "error"))
                        detalle.PotenciaContratadaDTO.PotenciaContrHPVar = string.IsNullOrEmpty(dtRow[19]?.ToString()) ? null : (decimal?)UtilTransfPotencia.ValidarNumero(dtRow[19].ToString()); ;
                    if ((dtRow[20]?.ToString() != "error"))
                        detalle.PotenciaContratadaDTO.PotenciaContrHFPVar = string.IsNullOrEmpty(dtRow[20]?.ToString()) ? null : (decimal?)UtilTransfPotencia.ValidarNumero(dtRow[20].ToString()); ;
                    if ((dtRow[21]?.ToString() != "error"))
                        detalle.PotenciaContratadaDTO.PotenciaContrObservacion = dtRow[21]?.ToString();
                    resultado.Data.Add(detalle);
                }
            }



            return resultado;
        }

        /// <summary>
        /// Permite obtener el CodigoRetiro mediante su sCoReSoCodigo en la vista VW_TRN_CODIGO_RETIRO
        /// </summary>
        /// <param name="sRetCodigo"></param>
        /// <returns>SolicitudCodigoDTO</returns>
        public SolicitudCodigoDTO GetByCodigoRetiroCodigo(string sRetCodigo)
        {
            return FactoryTransferencia.GetSolicitudCodigoRepository().GetByCodigoRetiCodigo(sRetCodigo);
        }

        /// <summary>
        /// Permite obtener el CodigoRetiro mediante su sCoReSoCodigo de la tabla TRN_CODIGO_RETIRO_SOLICITUD
        /// </summary>
        /// <param name="sCoReSoCodigo"></param>
        /// <returns>SolicitudCodigoDTO</returns>
        public SolicitudCodigoDTO GetCodigoRetiroByCodigo(string sCoReSoCodigo)
        {
            return FactoryTransferencia.GetSolicitudCodigoRepository().GetCodigoRetiroByCodigo(sCoReSoCodigo);
        }

        /// <summary>
        /// Permite obtener un codigoretiro vigente en el periodo
        /// </summary>
        /// <param name="iPeriCodi">Periodo de valorización</param>
        /// <param name="sCodigo">Código de Retiro asignado</param>
        /// <returns>CodigoEntregaDTO</returns>
        public SolicitudCodigoDTO CodigoRetiroVigenteByPeriodo(int iPeriCodi, string sCodigo)
        {
            return FactoryTransferencia.GetSolicitudCodigoRepository().CodigoRetiroVigenteByPeriodo(iPeriCodi, sCodigo);
        }

        /// <summary>
        /// Permite obtener la lista de barras de suministro
        /// </summary>
        /// <returns>BarraDTO</returns>
        public List<BarraDTO> ListarBarraSuministro()
        {
            return FactoryTransferencia.GetSolicitudCodigoDetalleRepository().ListarBarraSuministro();
        }

        /// <summary>
        /// Permite registrar la relación de barras de trasferencia y barra de suministro
        /// </summary>
        /// <param name="entity">Entidad de relación de barras</param>
        /// <returns>Entero</returns>
        public int RegistrarRelacion(BarraRelacionDTO entity)
        {
            return FactoryTransferencia.GetSolicitudCodigoDetalleRepository().SaveBR(entity);
        }

        /// <summary>
        /// Permite quitar la relación de barras de trasferencia y barra de suministro
        /// </summary>
        /// <param name="id">Id de la relación</param>
        /// <returns>Entero</returns>
        public int QuitarRelacion(int id)
        {
            return FactoryTransferencia.GetSolicitudCodigoDetalleRepository().DeleteBR(id);
        }


        /// <summary>
        /// Permite obtener la lista de detalle
        /// </summary>
        /// <returns>SolicitudCodigoDetalleDTO</returns>
        public List<SolicitudCodigoDetalleDTO> ListarDetalle(int id)
        {
            try
            {
                return FactoryTransferencia.GetSolicitudCodigoDetalleRepository().ListarDetalle(id);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                return null;
            }
        }

        /// <summary>
        /// Permite obtener el codigo vtp generado
        /// </summary>
        /// <param name="id">id de detalle de codigos vtp</param>
        /// <returns>CodigoGeneradoDTO</returns>
        public CodigoGeneradoDTO GetByIdCodigoGenerado(int id)
        {
            try
            {
                return FactoryTransferencia.GetSolicitudCodigoDetalleRepository().GetByIdGenerado(id);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                return null;
            }
        }


        /// <summary>
        /// Permite registrar la solicitud de baja del codigo VTEA
        /// </summary>
        /// <param name="entity">Solicitud de codigo de retiro</param>
        /// <returns>Entero</returns>
        public int SolicitarBajarVTEA(SolicitudCodigoDTO entity)
        {
            try
            {
                int result = FactoryTransferencia.GetSolicitudCodigoRepository().SolicitarBajar(entity);
                if (result > 0)
                {
                    EnviarCorreoNotificacionSolicitarBajaVTEA(entity);
                }
                return result;
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                return 0;
            }
        }

        /// <summary>
        /// Permite registrar la solicitud de baja del codigo VTEA}}P
        /// </summary>
        /// <param name="entity">Codigo vtp generado</param>
        /// <returns>Entero</returns>
        public int SolicitarBajarVTP(CodigoGeneradoDTO entity)
        {
            try
            {
                int result = FactoryTransferencia.GetSolicitudCodigoDetalleRepository().SolicitarBajarGenerado(entity);
                if (result > 0)
                {
                    EnviarCorreoNotificacionSolicitarBajaVTP(entity);
                }
                return result;
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                return 0;
            }
        }

        /// <summary>
        /// Notifica via email cada vez que un agente realiza una solicitud 
        /// </summary>
        public bool EnviarCorreoNotificacionRegistroSolicitud(SolicitudCodigoDTO entity)
        {
            SiPlantillacorreoDTO plantilla = this.servCorreo.GetByIdSiPlantillacorreo(ConstantesExtranetCTAF.PlantcodiNotificacionCarga);
            using (TransactionScope scope = new TransactionScope())
            {
                try
                {
                    if (plantilla != null)
                    {
                        StringBuilder tbody = new StringBuilder();
                        String email = entity.UserEmail;
                        //Obtiene los codigos generados
                        entity = FactoryTransferencia.GetSolicitudCodigoRepository().GetById(entity.SoliCodiRetiCodi);

                        List<CodigoRetiroGeneradoDTO> codigoRetiroGenerados = FactoryTransferencia.GetCodigoRetiroGeneradoRepository().ListarCodigosGeneradoVTP(new List<int> { entity.SoliCodiRetiCodi }, null);

                        foreach (var item in codigoRetiroGenerados)
                        {
                            tbody.Append(string.Format(@"<tr>
                             <td style ='font-size: 9.0pt; font-family: Calibri,sans-serif;'>{0}</td>
                             <td style ='font-size: 9.0pt; font-family: Calibri,sans-serif;'>{1}</td>
                             </tr>", entity.BarrNombBarrTran, item.BarrNombre));
                        }

                        string contenido = string.Format(Resources.RegistroSolicitud, entity.EmprNombre, entity.CliNombre, tbody.ToString(), email);

                        List<string> listaTo = plantilla.Planticorreos.Split(';').Select(x => x).ToList();

                        List<string> listaCC = new List<string>();
                        string from = TipoPlantillaCorreo.MailFrom;

                        string asunto = string.Format("Registro de solicitud de código de retiro - {0}", entity.EmprNombre);

                        if (!string.IsNullOrEmpty(contenido))
                        {
                            //if (!ConstantesExtranetCTAF.FlagEnviarNotificacionCargaEvento)
                            //{
                            //    listaTo = ConstantesExtranetCTAF.ListaEmailAdminEventosTo.Split(';').ToList();
                            //    listaCC = ConstantesExtranetCTAF.ListaEmailAdminEventosCC.Split(';').ToList();
                            //}

                            COES.Base.Tools.Util.SendEmail(listaTo, listaCC, asunto, contenido);
                        }
                    }
                    scope.Complete();
                    return true;
                }
                catch (Exception ex)
                {
                    if (scope != null)
                    {
                        scope.Dispose();
                    }
                    return false;
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="CliNombre"></param>
        /// <param name="Ruc"></param>
        /// <param name="Comentario"></param>
        /// <returns></returns>
        public bool EnviarCorreoNotificacionSolicitudCliente(string CliNombre, string Ruc, string Comentario, string EmpresaGeneradora)
        {
            SiPlantillacorreoDTO plantilla = this.servCorreo.GetByIdSiPlantillacorreo(ConstantesExtranetCTAF.PlantcodiNotificacionCarga);
            try
            {
                if (plantilla != null)
                {
                    StringBuilder tbody = new StringBuilder();
                    string contenido = string.Format(Resources.SolicitudCodigoCliente, CliNombre, Ruc, Comentario, EmpresaGeneradora);
                    List<string> listaTo = plantilla.Planticorreos.Split(';').Select(x => x).ToList();
                    List<string> listaCC = new List<string>();
                    string from = TipoPlantillaCorreo.MailFrom;
                    string asunto = string.Format("Solicitud de Creación de Cliente - {0}", CliNombre);
                    if (!string.IsNullOrEmpty(contenido))
                        COES.Base.Tools.Util.SendEmail(listaTo, listaCC, asunto, contenido);
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// Notifica via email cada vez que un agente realiza una solicitud 
        /// </summary>
        public bool EnviarCorreoNotificacionSolicitarBajaVTEA(SolicitudCodigoDTO entity)
        {
            SiPlantillacorreoDTO plantilla = this.servCorreo.GetByIdSiPlantillacorreo(ConstantesExtranetCTAF.PlantcodiNotificacionCarga);
            using (TransactionScope scope = new TransactionScope())
            {
                try
                {
                    if (plantilla != null)
                    {
                        StringBuilder tbody = new StringBuilder();
                        //Obtiene los codigos generados
                        //Obtiene los codigos generados
                        entity = FactoryTransferencia.GetSolicitudCodigoRepository().GetById(entity.SoliCodiRetiCodi);

                        tbody.Append(string.Format(@"<tr>
                             <td style ='font-size: 9.0pt; font-family: Calibri,sans-serif;'>{0}</td>
                             <td style ='font-size: 9.0pt; font-family: Calibri,sans-serif;'>{1}</td>
                             </tr>", entity.BarrNombBarrTran, entity.SoliCodiRetiCodigo));

                        string contenido = string.Format(Resources.SolicitarBajaCodigo, entity.EmprNombre, entity.CliNombre,
                                                    "Barra Transferencia", "Código VTEA", tbody.ToString());

                        List<string> listaTo = plantilla.Planticorreos.Split(';').Select(x => x).ToList();

                        List<string> listaCC = new List<string>();
                        string from = TipoPlantillaCorreo.MailFrom;

                        string asunto = "Solicitar baja de código VTEA";

                        if (!string.IsNullOrEmpty(contenido))
                        {
                            //if (!ConstantesExtranetCTAF.FlagEnviarNotificacionCargaEvento)
                            //{
                            //    listaTo = ConstantesExtranetCTAF.ListaEmailAdminEventosTo.Split(';').ToList();
                            //    listaCC = ConstantesExtranetCTAF.ListaEmailAdminEventosCC.Split(';').ToList();
                            //}

                            COES.Base.Tools.Util.SendEmail(listaTo, listaCC, asunto, contenido);
                        }
                    }
                    scope.Complete();
                    return true;
                }
                catch (Exception ex)
                {
                    if (scope != null)
                    {
                        scope.Dispose();
                    }
                    return false;
                }
            }
        }

        /// <summary>
        /// Notifica via email cada vez que un agente realiza una solicitud 
        /// </summary>
        public bool EnviarCorreoNotificacionSolicitarBajaVTP(CodigoGeneradoDTO entity)
        {
            SiPlantillacorreoDTO plantilla = this.servCorreo.GetByIdSiPlantillacorreo(ConstantesExtranetCTAF.PlantcodiNotificacionCarga);
            using (TransactionScope scope = new TransactionScope())
            {
                try
                {
                    if (plantilla != null)
                    {
                        StringBuilder tbody = new StringBuilder();
                        SolicitudCodigoDTO solicitud = FactoryTransferencia.GetSolicitudCodigoRepository().GetById(entity.Coresocodi);

                        tbody.Append(string.Format(@"<tr>
                             <td style ='font-size: 9.0pt; font-family: Calibri,sans-serif;'>{0}</td>
                             <td style ='font-size: 9.0pt; font-family: Calibri,sans-serif;'>{1}</td>
                             </tr>", entity.BarrNombSum, entity.Coregecodigovtp));

                        string contenido = string.Format(Resources.SolicitarBajaCodigo, solicitud.EmprNombre, solicitud.CliNombre,
                                                    "Barra Suministro", "Código VTP", tbody.ToString());

                        List<string> listaTo = plantilla.Planticorreos.Split(';').Select(x => x).ToList();

                        List<string> listaCC = new List<string>();
                        string from = TipoPlantillaCorreo.MailFrom;

                        string asunto = "Solicitar baja de código VTP";
                        if (!string.IsNullOrEmpty(contenido))
                        {
                            //if (!ConstantesExtranetCTAF.FlagEnviarNotificacionCargaEvento)
                            //{
                            //    listaTo = ConstantesExtranetCTAF.ListaEmailAdminEventosTo.Split(';').ToList();
                            //    listaCC = ConstantesExtranetCTAF.ListaEmailAdminEventosCC.Split(';').ToList();
                            //}

                            COES.Base.Tools.Util.SendEmail(listaTo, listaCC, asunto, contenido);
                        }
                    }
                    scope.Complete();
                    return true;
                }
                catch (Exception ex)
                {
                    if (scope != null)
                    {
                        scope.Dispose();
                    }
                    return false;
                }
            }
        }

    }
}
