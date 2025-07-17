using COES.Base.Core;
using COES.Base.Tools;
using COES.Dominio.DTO.Transferencias;
using COES.Servicios.Aplicacion.Factory;
using COES.Servicios.Aplicacion.Transferencias.Helper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
using COES.Servicios.Aplicacion.IEOD;
using System.Transactions;
using COES.Dominio.DTO.Sic;
using COES.Servicios.Aplicacion.Correo;
using COES.Servicios.Aplicacion.Eventos.Helper;
using COES.Servicios.Aplicacion.Helper;
using COES.Dominio.DTO.Enum;

namespace COES.Servicios.Aplicacion.Transferencias
{
    /// <summary>
    /// Clases con métodos del módulo Codigo Retiro
    /// </summary>
    public class CodigoRetiroAppServicio : AppServicioBase
    {

        private static readonly ILog Logger = LogManager.GetLogger(typeof(CodigoRetiroAppServicio));

        const string AgrupacionVTP = "AGRVTP";
        const string AgrupacionVTA = "AGRVTA";

        /// <summary>
        /// Permite grabar o actualizar un  CodigoRetiroDTO en base a la entidad
        /// </summary>
        /// <param name="entity">Entidad de CodigoRetiroDTO</param>
        /// <returns>Retorna el iCoReSoCodi nuevo o actualizado</returns>
        public int SaveOrUpdateCodigoRetiro(CodigoRetiroDTO entity)
        {
            try
            {
                int id = 0;

                if (entity.SoliCodiRetiCodi == 0)
                {
                    id = FactoryTransferencia.GetCodigoRetiroRepository().Save(entity);
                }
                else
                {
                    FactoryTransferencia.GetCodigoRetiroRepository().Update(entity);
                    id = entity.SoliCodiRetiCodi;
                }

                return id;
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }
        /// <summary>
        /// Permite grabar las modificaciones que realizar el usuario para agregar o dar de baja barras susministros
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public int SaveSuministrosAsignados(CodigoRetiroDTO entity)
        {
            int resultado = 0;

            int estadoAuditoria = 0;
            int idSolicitud = 0;

            List<CodigoRetiroRelacionDetalleDTO> objCodigoRetiroRelacionDet = new List<CodigoRetiroRelacionDetalleDTO>();

            StringBuilder mensajeAuditoria = new StringBuilder();
            using (TransactionScope scope = new TransactionScope())
            {
                try
                {


                    int cantBaja = 0;
                    idSolicitud = entity.SoliCodiRetiCodi;
                    entity.SoliCodiRetiEstado = "ACT";

                    IDbConnection conn = null;
                    DbTransaction tran = null;
                    FactoryTransferencia.GetCodigoRetiroRepository().UpdateVariacion(entity, conn, tran);
                    FactoryTransferencia.GetCodigoRetiroRepository().UpdateEstadoAprobacion(entity);


                    #region Periodos Abiertos
                    CodigoRetiroGeneradoDTO paramCodigoRetiroGenerado = new CodigoRetiroGeneradoDTO();
                    paramCodigoRetiroGenerado.CoresoCodi = entity.SoliCodiRetiCodi;
                    paramCodigoRetiroGenerado.CoresoCodigoVTA = entity.SoliCodiRetiCodigo;
                    paramCodigoRetiroGenerado.GenemprCodi = entity.EmprCodi;
                    paramCodigoRetiroGenerado.PeridcCodi = entity.PeridcCodi;
                    paramCodigoRetiroGenerado.SoliCodiRetiFechaInicio = entity.SoliCodiRetiFechaInicio;
                    paramCodigoRetiroGenerado.SoliCodiRetiFechaFin = entity.SoliCodiRetiFechaFin;
                    paramCodigoRetiroGenerado.TrnPcTipoPotencia = Convert.ToInt32(entity.TrnpcTipoPotencia);
                    FactoryTransferencia.GetCodigoRetiroGeneradoRepository().GenerarVTAPeriodosAbiertos(paramCodigoRetiroGenerado);
                    // agregar update a trn_codigo_consolidado_periodo actual
                    SolicitudCodigoDTO solcodDTO = new SolicitudCodigoDTO
                    {
                        PeridcCodi = entity.PeridcCodi,
                        CoresoCodi = entity.SoliCodiRetiCodi,
                        TrnpcTipoPotencia = entity.tipoPotencia == "AGRVTA" ? 1 : 2
                    };
                    FactoryTransferencia.GetSolicitudCodigoRepository().UpdateTipPotCodConsolidadoPeriodo(solcodDTO);
                    FactoryTransferencia.GetSolicitudCodigoRepository().UpdateTipPotCodCodigoRetiro(solcodDTO);
                    FactoryTransferencia.GetCodigoRetiroGeneradoRepository().GenerarPotenciasPeriodosAbiertos(paramCodigoRetiroGenerado);


                    FactoryTransferencia.GetCodigoRetiroGeneradoRepository().GenerarVTPPeriodosAbiertos(paramCodigoRetiroGenerado);


                    #endregion  Periodos Abiertos


                    #region Barra suministro

                    if (entity.ListarBarraSuministro != null)
                    {
                        foreach (var item in entity.ListarBarraSuministro)
                        {
                            if (item.EstdAbrev == "PAP")
                            {
                                int idAuxiliar = item.CoregeCodi;
                                item.EstdAbrev = "ACT";
                                item.PeridcCodi = entity.PeridcCodi;
                                item.TrnPcTipoPotencia = entity.tipoPotencia == "AGRVTA" ? 1 : 2;

                                // mandar parámetro para actualizar el trctipopotencia 1 o 2
                                CodigoRetiroGeneradoDTO objCodigoRetiro = FactoryTransferencia.GetCodigoRetiroGeneradoRepository().GenerarAprobacion(item, conn, tran);

                                CodigoRetiroRelacionDetalleDTO obj = new CodigoRetiroRelacionDetalleDTO();
                                obj.Genemprcodivtp = Convert.ToInt32(item.GenemprCodi);
                                obj.Cliemprcodivtp = Convert.ToInt32(item.CliemprCodi);
                                obj.Barrcodivtp = Convert.ToInt32(item.BarrCodiSum);
                                obj.Coresocodvtp = Convert.ToInt32(objCodigoRetiro.CoregeCodi);

                                objCodigoRetiroRelacionDet.Add(obj);


                                mensajeAuditoria.AppendLine(string.Format("Se genero el codigo VTP. PeridcCodi:{0};CoregeCodi:{1}", item.PeridcCodi, item.CoregeCodi));

                                if (entity.tipoPotencia == "AGRVTP")
                                {

                                    if (idAuxiliar == 0)
                                    {
                                        var rowPotenciaEncontrada = entity.ListaPotenciaSum[item.indexBarra];

                                        if (rowPotenciaEncontrada.BarrCodi == item.BarrCodiSum)
                                            rowPotenciaEncontrada.CoregeCodi = objCodigoRetiro.CoregeCodi;

                                    }
                                    

                                }
                            }
                            else if (item.EstdAbrev == "BAJ")
                            {
                                //throw new Exception("dd");
                                FactoryTransferencia.GetCodigoRetiroGeneradoRepository().UpdateEstado(item, conn, tran);
                                cantBaja++;

                                mensajeAuditoria.AppendLine(string.Format("Se ha realizado la baja para el codigo VTP. CoregeCodi:{0}", item.CoregeCodi));

                            }
                        }
                    }





                    #endregion Barra suministro

                    #region Registro Potencias 

                    SolicitudCodigoDTO objSolicitud = (new SolicitudCodigoAppServicio()).GetByIdCodigoRetiro(paramCodigoRetiroGenerado.CoresoCodi, entity.PeridcCodi);
                    entity.esPrimerRegistro = entity.esPrimerRegistro ?? 1;
                    if (entity.tipoPotencia == "AGRVTP")
                    {
                        if (objSolicitud.TrnpcTipoCasoAgrupado == "AGRVTA")
                            FactoryTransferencia.GetTrnPotenciaContratadaRepository().DesactivarPotenciasContratadas(paramCodigoRetiroGenerado.CoresoCodi, entity.PeridcCodi);

                        if (entity.ListaPotenciaSum != null)
                        {
                            foreach (var itemPot in entity.ListaPotenciaSum)
                            {
                                itemPot.CoresoCodi = itemPot.CoresoCodi;
                                itemPot.CoregeCodi = itemPot.CoregeCodi;
                                itemPot.TrnPctUserNameIns = entity.CoesUserName;
                                itemPot.TrnPctComeObs = "Potencias  contradas, que fueron registradas a nivela a nivel VTP";
                                itemPot.PeriCodi = entity.PeridcCodi;
                                FactoryTransferencia.GetTrnPotenciaContratadaRepository().GenerarCargaDatos(itemPot);

                                mensajeAuditoria.AppendLine(string.Format("Se ha generado las potencias para el codigo VTP. CoregeCodi:{0}", itemPot.CoregeCodi));

                            }
                        }

                    }
                    else if (entity.tipoPotencia == "AGRVTA" && entity.esPrimerRegistro == 1)
                    {
                        if (objSolicitud.TrnpcTipoCasoAgrupado == "AGRVTP")
                            FactoryTransferencia.GetTrnPotenciaContratadaRepository().DesactivarPotenciasContratadas(paramCodigoRetiroGenerado.CoresoCodi, entity.PeridcCodi);


                        TrnPotenciaContratadaDTO paramPotencia = new TrnPotenciaContratadaDTO();
                        paramPotencia.PeriCodi = entity.ListaPotenciaTran.PeriCodi;
                        paramPotencia.CoresoCodi = entity.ListaPotenciaTran.CoresoCodi;
                        paramPotencia.CoregeCodi = null;
                        paramPotencia.TrnpcTipoCasoAgrupado = "AGRVTA";
                        paramPotencia.TrnPctTotalMwFija = entity.ListaPotenciaTran.TrnPctTotalMwFija;
                        paramPotencia.TrnPctHpMwFija = entity.ListaPotenciaTran.TrnPctHpMwFija;
                        paramPotencia.TrnPctHfpMwFija = entity.ListaPotenciaTran.TrnPctHfpMwFija;
                        paramPotencia.TrnPctTotalMwVariable = entity.ListaPotenciaTran.TrnPctTotalMwVariable;
                        paramPotencia.TrnPctHpMwFijaVariable = entity.ListaPotenciaTran.TrnPctHpMwFijaVariable;
                        paramPotencia.TrnPctHfpMwFijaVariable = entity.ListaPotenciaTran.TrnPctHfpMwFijaVariable;
                        paramPotencia.TrnPctComeObs = null;
                        paramPotencia.TrnPctUserNameIns = entity.CoesUserName;
                        paramPotencia.TrnPcEnvCodi = null;
                        paramPotencia.TrnPctComeObs = "Potencias  contradas, que fueron registradas a nivela a nivel VTA";

                        FactoryTransferencia.GetTrnPotenciaContratadaRepository().GenerarCargaDatos(paramPotencia);
                        mensajeAuditoria.AppendLine(string.Format("Se ha registrado potencias a nivel de barra de transferencias."));
                    }
                    #endregion Registro Potencias

                    #region Desactiva VTA si no hay VTP
                    if (cantBaja > 0)
                    {
                        entity.SoliCodiRetiEstado = "BAJ";
                        estadoAuditoria = (int)EVtpEstados.Baja;
                        FactoryTransferencia.GetCodigoRetiroRepository().UpdateBajaCodigoVTEA(entity, conn, tran);
                        mensajeAuditoria.AppendLine(string.Format("Se ha realizado la baja para el codigo VTA/VTP."));
                    }
                    else
                        estadoAuditoria = (int)EVtpEstados.Activo;


                    #endregion Desactiva VTA si no hay VTP

                    #region Registra Equivalencias

                    CodigoRetiroRelacionDetalleDTO objRelacionEquivalencia = FactoryTransferencia.GetCodigoRetiroEquivalenciaDetalleRepository().GetRelacionDetallePorVTEA(idSolicitud);

                    if (objRelacionEquivalencia != null)
                    {
                        foreach (var item in objCodigoRetiroRelacionDet)
                        {
                            item.Retrelcodi = objRelacionEquivalencia.Retrelcodi;
                            FactoryTransferencia.GetCodigoRetiroEquivalenciaDetalleRepository().Save(item);
                        }
                    }
                    else
                    {
                        CodigoRetiroRelacionDTO oCodRetrel = new CodigoRetiroRelacionDTO();
                        oCodRetrel.Retrelvari = Convert.ToDecimal(ConfigurationManager.AppSettings["Variacion"].ToString());
                        oCodRetrel.Retelestado = "ACT";
                        oCodRetrel.Retrelusucreacion = entity.CoesUserName;
                        oCodRetrel.RetrelCodi = 0;
                        int idRelacion = FactoryTransferencia.GetCodigoRetiroRelacionEquivalenciasRepository().Save(oCodRetrel);
                        if (idRelacion > 0)
                        {
                            int indexEnvio = -1;
                            List<CodigoRetiroGeneradoDTO> codigoRetirosParaEnvio = new List<CodigoRetiroGeneradoDTO>();
                            codigoRetirosParaEnvio = FactoryTransferencia.GetCodigoRetiroGeneradoRepository().ListarCodigosGeneradoVTP(new List<int> { idSolicitud }, null);
                            foreach (var item in codigoRetirosParaEnvio)
                            {
                                indexEnvio++;

                                CodigoRetiroRelacionDetalleDTO obj = new CodigoRetiroRelacionDetalleDTO();
                                obj.Genemprcodivtp = Convert.ToInt32(entity.EmprCodi);
                                obj.Cliemprcodivtp = Convert.ToInt32(entity.CliCodi);
                                obj.Barrcodivtp = Convert.ToInt32(item.BarrCodiSum);
                                obj.Coresocodvtp = Convert.ToInt32(item.CoregeCodi);

                                if (indexEnvio == 0)
                                {
                                    obj.Coresocodvtea = Convert.ToInt32(idSolicitud);
                                    obj.Genemprcodivtea = Convert.ToInt32(entity.EmprCodi);
                                    obj.Cliemprcodivtea = Convert.ToInt32(entity.CliCodi);
                                    obj.Barrcodivtea = Convert.ToInt32(entity.BarrCodi);
                                }
                                obj.Retrelcodi = idRelacion;
                                FactoryTransferencia.GetCodigoRetiroEquivalenciaDetalleRepository().Save(obj);
                            }

                        }

                    }
                    #endregion Registra Equivalencias

                    //#region Desactivar periodo actual 
                    //if (entity.SoliCodiRetiEstado == "ACT")
                    //    FactoryTransferencia.GetCodigoRetiroGeneradoRepository().DesactivarSolicitudPeriodoActual(entity);
                    //#endregion Desactivar periodo actual


                    #region AuditoriaProceso

                    StringBuilder mensajeAuxiliar = new StringBuilder();
                    mensajeAuxiliar.AppendLine(string.Format("Solicitud Nro.{0}", idSolicitud));
                    mensajeAuxiliar.AppendLine("Se ha generado los codigos vtp");

                    VtpAuditoriaProcesoDTO objAuditoria = new VtpAuditoriaProcesoDTO();
                    objAuditoria.Tipprocodi = (int)ETipoProcesoAuditoriaVTP.GestionCodigosEnvioInformacion;
                    objAuditoria.Estdcodi = estadoAuditoria;
                    objAuditoria.Audproproceso = "Auditoria.";
                    objAuditoria.Audprodescripcion = string.Format("Intranet - {0}", mensajeAuxiliar);
                    objAuditoria.Audprousucreacion = entity.CoesUserName;
                    objAuditoria.Audprofeccreacion = DateTime.Now;
                    int auditoria = new AuditoriaProcesoAppServicio().save(objAuditoria);
                    if (auditoria == 0)
                    {
                        Logger.Error(" - Error Save Auditoria");
                    }

                    #endregion

                    resultado = 1;
                    scope.Complete();
                }
                catch (Exception ex)
                {
                    if (scope != null)
                    {
                        resultado = -1;
                        scope.Dispose();
                    }
                    Logger.Error(ConstantesAppServicio.LogError, ex);
                    throw new Exception(ex.Message, ex);
                }
            }

            return resultado;
        }

        /// <summary>
        /// Permite grabar la aprobacion de la solicitud de cambio
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public int AprobarSuministrosAsignados(CodigoRetiroDTO entity)
        {
            int resultado = 0;

            int estadoAuditoria = 0;
            int idSolicitud = 0;

            List<CodigoRetiroRelacionDetalleDTO> objCodigoRetiroRelacionDet = new List<CodigoRetiroRelacionDetalleDTO>();

            StringBuilder mensajeAuditoria = new StringBuilder();
            using (TransactionScope scope = new TransactionScope())
            {
                try
                {


                    int cantBaja = 0;
                    idSolicitud = entity.SoliCodiRetiCodi;
                    entity.SoliCodiRetiEstado = "ACT";

                    IDbConnection conn = null;
                    DbTransaction tran = null;
                    FactoryTransferencia.GetCodigoRetiroRepository().UpdateVariacion(entity, conn, tran);
                    FactoryTransferencia.GetCodigoRetiroRepository().UpdateEstadoAprobacion(entity);


                    #region Periodos Abiertos
                    CodigoRetiroGeneradoDTO paramCodigoRetiroGenerado = new CodigoRetiroGeneradoDTO();
                    paramCodigoRetiroGenerado.CoresoCodi = entity.SoliCodiRetiCodi;
                    paramCodigoRetiroGenerado.CoresoCodigoVTA = entity.SoliCodiRetiCodigo;
                    paramCodigoRetiroGenerado.GenemprCodi = entity.EmprCodi;
                    paramCodigoRetiroGenerado.PeridcCodi = entity.PeridcCodi;
                    paramCodigoRetiroGenerado.SoliCodiRetiFechaInicio = entity.SoliCodiRetiFechaInicio;
                    paramCodigoRetiroGenerado.SoliCodiRetiFechaFin = entity.SoliCodiRetiFechaFin;
                    paramCodigoRetiroGenerado.TrnPcTipoPotencia = Convert.ToInt32(entity.TrnpcTipoPotencia);
                    FactoryTransferencia.GetCodigoRetiroGeneradoRepository().GenerarVTAPeriodosAbiertos(paramCodigoRetiroGenerado);
                    // agregar update a trn_codigo_consolidado_periodo actual
                    SolicitudCodigoDTO solcodDTO = new SolicitudCodigoDTO
                    {
                        PeridcCodi = entity.PeridcCodi,
                        CoresoCodi = entity.SoliCodiRetiCodi,
                        TrnpcTipoPotencia = entity.tipoPotencia == "AGRVTA" ? 1 : 2
                    };
                    FactoryTransferencia.GetSolicitudCodigoRepository().UpdateTipPotCodConsolidadoPeriodo(solcodDTO);
                    FactoryTransferencia.GetSolicitudCodigoRepository().UpdateTipPotCodCodigoRetiro(solcodDTO);
                    FactoryTransferencia.GetCodigoRetiroGeneradoRepository().GenerarPotenciasPeriodosAbiertos(paramCodigoRetiroGenerado);


                    FactoryTransferencia.GetCodigoRetiroGeneradoRepository().GenerarVTPPeriodosAbiertos(paramCodigoRetiroGenerado);


                    #endregion  Periodos Abiertos


                    #region Barra suministro

                    if (entity.ListarBarraSuministro != null)
                    {
                        foreach (var item in entity.ListarBarraSuministro)
                        {
                            if (item.EstdAbrev == "PAP")
                            {
                                int idAuxiliar = item.CoregeCodi;
                                item.EstdAbrev = "ACT";
                                item.PeridcCodi = entity.PeridcCodi;
                                item.TrnPcTipoPotencia = entity.tipoPotencia == "AGRVTA" ? 1 : 2;

                                // mandar parámetro para actualizar el trctipopotencia 1 o 2
                                CodigoRetiroGeneradoDTO objCodigoRetiro = FactoryTransferencia.GetCodigoRetiroGeneradoRepository().GenerarAprobacion(item, conn, tran);

                                CodigoRetiroRelacionDetalleDTO obj = new CodigoRetiroRelacionDetalleDTO();
                                obj.Genemprcodivtp = Convert.ToInt32(item.GenemprCodi);
                                obj.Cliemprcodivtp = Convert.ToInt32(item.CliemprCodi);
                                obj.Barrcodivtp = Convert.ToInt32(item.BarrCodiSum);
                                obj.Coresocodvtp = Convert.ToInt32(objCodigoRetiro.CoregeCodi);

                                objCodigoRetiroRelacionDet.Add(obj);


                                mensajeAuditoria.AppendLine(string.Format("Se genero el codigo VTP. PeridcCodi:{0};CoregeCodi:{1}", item.PeridcCodi, item.CoregeCodi));

                                if (entity.tipoPotencia == "AGRVTP")
                                {

                                    if (idAuxiliar == 0)
                                    {
                                        var rowPotenciaEncontrada = entity.ListaPotenciaSum[item.indexBarra];

                                        if (rowPotenciaEncontrada.BarrCodi == item.BarrCodiSum)
                                            rowPotenciaEncontrada.CoregeCodi = objCodigoRetiro.CoregeCodi;

                                    }


                                }
                            }
                            else if (item.EstdAbrev == "BAJ")
                            {
                                //throw new Exception("dd");
                                FactoryTransferencia.GetCodigoRetiroGeneradoRepository().UpdateEstado(item, conn, tran);
                                cantBaja++;

                                mensajeAuditoria.AppendLine(string.Format("Se ha realizado la baja para el codigo VTP. CoregeCodi:{0}", item.CoregeCodi));

                            }
                        }
                    }





                    #endregion Barra suministro

                    #region Registro Potencias 

                    SolicitudCodigoDTO objSolicitud = (new SolicitudCodigoAppServicio()).GetByIdCodigoRetiro(paramCodigoRetiroGenerado.CoresoCodi, entity.PeridcCodi);
                    entity.esPrimerRegistro = entity.esPrimerRegistro ?? 1;
                    if (entity.tipoPotencia == "AGRVTP")
                    {
                        if (objSolicitud.TrnpcTipoCasoAgrupado == "AGRVTA")
                            FactoryTransferencia.GetTrnPotenciaContratadaRepository().DesactivarPotenciasContratadas(paramCodigoRetiroGenerado.CoresoCodi, entity.PeridcCodi);

                        if (entity.ListaPotenciaSum != null)
                        {
                            foreach (var itemPot in entity.ListaPotenciaSum)
                            {
                                itemPot.CoresoCodi = itemPot.CoresoCodi;
                                itemPot.CoregeCodi = itemPot.CoregeCodi;
                                itemPot.TrnPctUserNameIns = entity.CoesUserName;
                                itemPot.TrnPctComeObs = "Potencias  contradas, que fueron registradas a nivela a nivel VTP";
                                itemPot.PeriCodi = entity.PeridcCodi;
                                FactoryTransferencia.GetTrnPotenciaContratadaRepository().GenerarCargaDatos(itemPot);
                                FactoryTransferencia.GetTrnPotenciaContratadaRepository().AprobarSolicitudCambios(itemPot);

                                mensajeAuditoria.AppendLine(string.Format("Se ha generado las potencias para el codigo VTP. CoregeCodi:{0}", itemPot.CoregeCodi));

                            }
                        }

                    }
                    else if (entity.tipoPotencia == "AGRVTA" && entity.esPrimerRegistro == 1)
                    {
                        if (objSolicitud.TrnpcTipoCasoAgrupado == "AGRVTP")
                            FactoryTransferencia.GetTrnPotenciaContratadaRepository().DesactivarPotenciasContratadas(paramCodigoRetiroGenerado.CoresoCodi, entity.PeridcCodi);


                        TrnPotenciaContratadaDTO paramPotencia = new TrnPotenciaContratadaDTO();
                        paramPotencia.PeriCodi = entity.ListaPotenciaTran.PeriCodi;
                        paramPotencia.CoresoCodi = entity.ListaPotenciaTran.CoresoCodi;
                        paramPotencia.CoregeCodi = null;
                        paramPotencia.TrnpcTipoCasoAgrupado = "AGRVTA";
                        paramPotencia.TrnPctTotalMwFija = entity.ListaPotenciaTran.TrnPctTotalMwFija;
                        paramPotencia.TrnPctHpMwFija = entity.ListaPotenciaTran.TrnPctHpMwFija;
                        paramPotencia.TrnPctHfpMwFija = entity.ListaPotenciaTran.TrnPctHfpMwFija;
                        paramPotencia.TrnPctTotalMwVariable = entity.ListaPotenciaTran.TrnPctTotalMwVariable;
                        paramPotencia.TrnPctHpMwFijaVariable = entity.ListaPotenciaTran.TrnPctHpMwFijaVariable;
                        paramPotencia.TrnPctHfpMwFijaVariable = entity.ListaPotenciaTran.TrnPctHfpMwFijaVariable;
                        paramPotencia.TrnPctComeObs = null;
                        paramPotencia.TrnPctUserNameIns = entity.CoesUserName;
                        paramPotencia.TrnPcEnvCodi = null;
                        paramPotencia.TrnPctComeObs = "Potencias  contradas, que fueron registradas a nivela a nivel VTA";

                        FactoryTransferencia.GetTrnPotenciaContratadaRepository().GenerarCargaDatos(paramPotencia);
                        FactoryTransferencia.GetTrnPotenciaContratadaRepository().AprobarSolicitudCambios(paramPotencia);
                        mensajeAuditoria.AppendLine(string.Format("Se ha registrado potencias a nivel de barra de transferencias."));
                    }
                    #endregion Registro Potencias

                    #region Desactiva VTA si no hay VTP
                    if (cantBaja > 0)
                    {
                        entity.SoliCodiRetiEstado = "BAJ";
                        estadoAuditoria = (int)EVtpEstados.Baja;
                        FactoryTransferencia.GetCodigoRetiroRepository().UpdateBajaCodigoVTEA(entity, conn, tran);
                        mensajeAuditoria.AppendLine(string.Format("Se ha realizado la baja para el codigo VTA/VTP."));
                    }
                    else
                        estadoAuditoria = (int)EVtpEstados.Activo;


                    #endregion Desactiva VTA si no hay VTP

                    #region Registra Equivalencias

                    CodigoRetiroRelacionDetalleDTO objRelacionEquivalencia = FactoryTransferencia.GetCodigoRetiroEquivalenciaDetalleRepository().GetRelacionDetallePorVTEA(idSolicitud);

                    if (objRelacionEquivalencia != null)
                    {
                        foreach (var item in objCodigoRetiroRelacionDet)
                        {
                            item.Retrelcodi = objRelacionEquivalencia.Retrelcodi;
                            FactoryTransferencia.GetCodigoRetiroEquivalenciaDetalleRepository().Save(item);
                        }
                    }
                    else
                    {
                        CodigoRetiroRelacionDTO oCodRetrel = new CodigoRetiroRelacionDTO();
                        oCodRetrel.Retrelvari = Convert.ToDecimal(ConfigurationManager.AppSettings["Variacion"].ToString());
                        oCodRetrel.Retelestado = "ACT";
                        oCodRetrel.Retrelusucreacion = entity.CoesUserName;
                        oCodRetrel.RetrelCodi = 0;
                        int idRelacion = FactoryTransferencia.GetCodigoRetiroRelacionEquivalenciasRepository().Save(oCodRetrel);
                        if (idRelacion > 0)
                        {
                            int indexEnvio = -1;
                            List<CodigoRetiroGeneradoDTO> codigoRetirosParaEnvio = new List<CodigoRetiroGeneradoDTO>();
                            codigoRetirosParaEnvio = FactoryTransferencia.GetCodigoRetiroGeneradoRepository().ListarCodigosGeneradoVTP(new List<int> { idSolicitud }, null);
                            foreach (var item in codigoRetirosParaEnvio)
                            {
                                indexEnvio++;

                                CodigoRetiroRelacionDetalleDTO obj = new CodigoRetiroRelacionDetalleDTO();
                                obj.Genemprcodivtp = Convert.ToInt32(entity.EmprCodi);
                                obj.Cliemprcodivtp = Convert.ToInt32(entity.CliCodi);
                                obj.Barrcodivtp = Convert.ToInt32(item.BarrCodiSum);
                                obj.Coresocodvtp = Convert.ToInt32(item.CoregeCodi);

                                if (indexEnvio == 0)
                                {
                                    obj.Coresocodvtea = Convert.ToInt32(idSolicitud);
                                    obj.Genemprcodivtea = Convert.ToInt32(entity.EmprCodi);
                                    obj.Cliemprcodivtea = Convert.ToInt32(entity.CliCodi);
                                    obj.Barrcodivtea = Convert.ToInt32(entity.BarrCodi);
                                }
                                obj.Retrelcodi = idRelacion;
                                FactoryTransferencia.GetCodigoRetiroEquivalenciaDetalleRepository().Save(obj);
                            }

                        }

                    }
                    #endregion Registra Equivalencias

                    //#region Desactivar periodo actual 
                    //if (entity.SoliCodiRetiEstado == "ACT")
                    //    FactoryTransferencia.GetCodigoRetiroGeneradoRepository().DesactivarSolicitudPeriodoActual(entity);
                    //#endregion Desactivar periodo actual


                    #region AuditoriaProceso

                    StringBuilder mensajeAuxiliar = new StringBuilder();
                    mensajeAuxiliar.AppendLine(string.Format("Solicitud Nro.{0}", idSolicitud));
                    mensajeAuxiliar.AppendLine("Se ha generado los codigos vtp");

                    VtpAuditoriaProcesoDTO objAuditoria = new VtpAuditoriaProcesoDTO();
                    objAuditoria.Tipprocodi = (int)ETipoProcesoAuditoriaVTP.GestionCodigosEnvioInformacion;
                    objAuditoria.Estdcodi = estadoAuditoria;
                    objAuditoria.Audproproceso = "Auditoria.";
                    objAuditoria.Audprodescripcion = string.Format("Intranet - {0}", mensajeAuxiliar);
                    objAuditoria.Audprousucreacion = entity.CoesUserName;
                    objAuditoria.Audprofeccreacion = DateTime.Now;
                    int auditoria = new AuditoriaProcesoAppServicio().save(objAuditoria);
                    if (auditoria == 0)
                    {
                        Logger.Error(" - Error Save Auditoria");
                    }

                    #endregion

                    resultado = 1;
                    scope.Complete();
                }
                catch (Exception ex)
                {
                    if (scope != null)
                    {
                        resultado = -1;
                        scope.Dispose();
                    }
                    Logger.Error(ConstantesAppServicio.LogError, ex);
                    throw new Exception(ex.Message, ex);
                }
            }

            return resultado;
        }


        /// <summary>
        /// Permite rechazar la solicitud de cambio
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public int RechazarSuministrosAsignados(CodigoRetiroDTO entity)
        {
            int resultado = 0;

            //int estadoAuditoria = 0;
            //int idSolicitud = 0;

            List<CodigoRetiroRelacionDetalleDTO> objCodigoRetiroRelacionDet = new List<CodigoRetiroRelacionDetalleDTO>();

            StringBuilder mensajeAuditoria = new StringBuilder();
            using (TransactionScope scope = new TransactionScope())
            {
                try
                {


                    TrnPotenciaContratadaDTO paramPotencia = new TrnPotenciaContratadaDTO();
                    paramPotencia.PeriCodi = entity.ListaPotenciaTran.PeriCodi;
                    paramPotencia.CoresoCodi = entity.ListaPotenciaTran.CoresoCodi;
                    paramPotencia.TrnPctUserNameIns = entity.CoesUserName;

                    FactoryTransferencia.GetTrnPotenciaContratadaRepository().RechazarSolicitudCambios(paramPotencia);


                   


                    #region AuditoriaProceso

                    /*StringBuilder mensajeAuxiliar = new StringBuilder();
                    mensajeAuxiliar.AppendLine(string.Format("Solicitud Nro.{0}", idSolicitud));
                    mensajeAuxiliar.AppendLine("Se ha generado los codigos vtp");

                    VtpAuditoriaProcesoDTO objAuditoria = new VtpAuditoriaProcesoDTO();
                    objAuditoria.Tipprocodi = (int)ETipoProcesoAuditoriaVTP.GestionCodigosEnvioInformacion;
                    objAuditoria.Estdcodi = estadoAuditoria;
                    objAuditoria.Audproproceso = "Auditoria.";
                    objAuditoria.Audprodescripcion = string.Format("Intranet - {0}", mensajeAuxiliar);
                    objAuditoria.Audprousucreacion = entity.CoesUserName;
                    objAuditoria.Audprofeccreacion = DateTime.Now;
                    int auditoria = new AuditoriaProcesoAppServicio().save(objAuditoria);
                    if (auditoria == 0)
                    {
                        Logger.Error(" - Error Save Auditoria");
                    }*/

                    #endregion

                    resultado = 1;
                    scope.Complete();
                }
                catch (Exception ex)
                {
                    if (scope != null)
                    {
                        resultado = -1;
                        scope.Dispose();
                    }
                    Logger.Error(ConstantesAppServicio.LogError, ex);
                    throw new Exception(ex.Message, ex);
                }
            }

            return resultado;
        }

        /// <summary>
        /// Bar de baja a un suministro VTEA
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>

        public int SaveDarBajaSuministro(int iCoReSoCodi, string sCoesUserName)
        {
            int resultado = 0;
            try
            {
                FactoryTransferencia.GetCodigoRetiroRepository().UpdateBajaCodigoVTEAVTP(iCoReSoCodi, sCoesUserName);
                resultado = 1;

                #region AuditoriaProceso
                int estadoId = 0;
                string mensajeError = "";
                estadoId = (int)EVtpEstados.Baja;
                mensajeError = string.Format("Solicitud Nro.{0}, Se ha realizado la baja de todos los codigos VTA y VTP", iCoReSoCodi);

                VtpAuditoriaProcesoDTO objAuditoria = new VtpAuditoriaProcesoDTO();
                objAuditoria.Tipprocodi = (int)ETipoProcesoAuditoriaVTP.GestionCodigosEnvioInformacion;
                objAuditoria.Estdcodi = estadoId;
                objAuditoria.Audproproceso = "Auditoria.";
                objAuditoria.Audprodescripcion = string.Format("Intranet - {0}", mensajeError);
                objAuditoria.Audprousucreacion = sCoesUserName;
                objAuditoria.Audprofeccreacion = DateTime.Now;
                int auditoria = new AuditoriaProcesoAppServicio().save(objAuditoria);
                if (auditoria == 0)
                {
                    Logger.Error(" - Error Save Auditoria");
                }

                #endregion

            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }

            return resultado;
        }
        /// <summary>
        ///  Permite dar de baja un codigo VT
        /// </summary>
        /// <param name="iCoReSoCodi"></param>
        /// <param name="iCoregeCodi"></param>
        /// <param name="CoesUserName"></param>
        /// <returns></returns>
        public int SaveDarBajaSuministroVTP(int iCoReSoCodi, int iCoregeCodi, string CoesUserName)
        {
            int resultado = 0;
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    try
                    {
                        CodigoRetiroGeneradoDTO entity = new CodigoRetiroGeneradoDTO();
                        entity.EstdAbrev = "BAJ";
                        entity.CoregeCodi = iCoregeCodi;
                        FactoryTransferencia.GetCodigoRetiroGeneradoRepository().UpdateEstado(entity, null, null);
                        List<CodigoRetiroGeneradoDTO> codigoRetiroGenerados = FactoryTransferencia.GetCodigoRetiroGeneradoRepository().ListarCodigosGeneradoVTP(new List<int> { iCoReSoCodi }, null);
                        int totalActivos = codigoRetiroGenerados.Where(x => x.EstdAbrev == "PAP" || x.EstdAbrev == "ACT"
                        || x.EstdAbrev == "SBJ").Count();
                        if (totalActivos == 0)
                        {
                            CodigoRetiroDTO paramCodigoRetiro = new CodigoRetiroDTO();
                            paramCodigoRetiro.SoliCodiRetiCodi = iCoReSoCodi;
                            paramCodigoRetiro.CoesUserName = CoesUserName;
                            paramCodigoRetiro.SoliCodiRetiEstado = "BAJ";

                            FactoryTransferencia.GetCodigoRetiroRepository().UpdateBajaCodigoVTEA(paramCodigoRetiro, null, null);
                        }


                        #region AuditoriaProceso
                        int estadoId = 0;
                        string mensajeError = "";
                        estadoId = (int)EVtpEstados.Baja;
                        mensajeError = string.Format("Solicitud Nro.{0}, Se ha realizado la baja del codigo VTP. CoregeCodi:{0}", iCoregeCodi);

                        VtpAuditoriaProcesoDTO objAuditoria = new VtpAuditoriaProcesoDTO();
                        objAuditoria.Tipprocodi = (int)ETipoProcesoAuditoriaVTP.GestionCodigosEnvioInformacion;
                        objAuditoria.Estdcodi = estadoId;
                        objAuditoria.Audproproceso = "Auditoria.";
                        objAuditoria.Audprodescripcion = string.Format("Intranet - {0}", mensajeError);
                        objAuditoria.Audprousucreacion = CoesUserName;
                        objAuditoria.Audprofeccreacion = DateTime.Now;
                        int auditoria = new AuditoriaProcesoAppServicio().save(objAuditoria);
                        if (auditoria == 0)
                        {
                            Logger.Error(" - Error Save Auditoria");
                        }

                        resultado = 1;
                        scope.Complete();

                        #endregion
                    }
                    catch (Exception ex)
                    {
                        if (scope != null)
                        {
                            resultado = -1;
                            scope.Dispose();
                        }
                        Logger.Error(ConstantesAppServicio.LogError, ex);
                        throw new Exception(ex.Message, ex);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }

            return resultado;
        }

        /// <summary>
        /// Actualiza el estado de la solicitud
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public ResultadoDTO<int> AprobarRechazarSolicitud(CodigoRetiroDTO entity)
        {
            ResultadoDTO<int> resultado = new ResultadoDTO<int>();
            string error = "";
            int id = 0;
            int idCodigoSolicitud = 0;
            List<CodigoRetiroGeneradoDTO> codigoRetirosParaEnvio = new List<CodigoRetiroGeneradoDTO>();
            StringBuilder tbody = new StringBuilder();
            using (TransactionScope scope = new TransactionScope())
            {
                try
                {
                    CodigoRetiroDTO codigoRetiroEntidad = new CodigoRetiroDTO();


                    //Validar Abreviatura
                    codigoRetiroEntidad = FactoryTransferencia.GetCodigoRetiroRepository().GetByIdGestionCodigosVTEAVTP(entity.SoliCodiRetiCodi, entity.PeridcCodi);
                    if (string.IsNullOrEmpty(codigoRetiroEntidad.EmprAbrevia)
                        && entity.SoliCodiRetiEstado != "REC")
                    {
                        error = "La empresa no tiene asignada un abreviatura";
                    }
                    if (string.IsNullOrEmpty(error))
                    {

                        //Obtiene los codigos generados
                        List<CodigoRetiroGeneradoDTO> codigoRetiroGenerados = FactoryTransferencia.GetCodigoRetiroGeneradoRepository().ListarCodigosGeneradoVTP(new List<int> { entity.SoliCodiRetiCodi }, null);

                        if (entity.SoliCodiRetiEstado == "REC")
                            FactoryTransferencia.GetCodigoRetiroRepository().UpdateEstadoRechazar(entity);

                        else
                            FactoryTransferencia.GetCodigoRetiroRepository().UpdateEstadoAprobacion(entity);


                        if (entity.SoliCodiRetiEstado == "ACT")
                        {
                            CodigoRetiroGeneradoDTO paramCodigoRetiroGenerado = new CodigoRetiroGeneradoDTO();
                            paramCodigoRetiroGenerado.CoresoCodi = entity.SoliCodiRetiCodi;
                            paramCodigoRetiroGenerado.CoresoCodigoVTA = entity.SoliCodiRetiCodigo;
                            paramCodigoRetiroGenerado.GenemprCodi = entity.EmprCodi;
                            paramCodigoRetiroGenerado.PeridcCodi = entity.PeridcCodi;
                            paramCodigoRetiroGenerado.SoliCodiRetiFechaInicio = entity.SoliCodiRetiFechaInicio;
                            paramCodigoRetiroGenerado.SoliCodiRetiFechaFin = entity.SoliCodiRetiFechaFin;
                            paramCodigoRetiroGenerado.TrnPcTipoPotencia = Convert.ToInt32(entity.TrnpcTipoPotencia);
                            FactoryTransferencia.GetCodigoRetiroGeneradoRepository().GenerarVTAPeriodosAbiertos(paramCodigoRetiroGenerado);
                            FactoryTransferencia.GetCodigoRetiroGeneradoRepository().GenerarPotenciasPeriodosAbiertos(paramCodigoRetiroGenerado);

                        }

                        foreach (var item in codigoRetiroGenerados)
                        {
                            CodigoRetiroGeneradoDTO entityGenerado = new CodigoRetiroGeneradoDTO();
                            entityGenerado.EstdAbrev = entity.SoliCodiRetiEstado;
                            entityGenerado.GenemprCodi = entity.EmprCodi;
                            entityGenerado.CoregeCodi = item.CoregeCodi;
                            entityGenerado.BarrCodiTra = entity.BarrCodi;
                            entityGenerado.BarrCodiSum = item.BarrCodiSum;
                            entityGenerado.CoresoCodi = entity.SoliCodiRetiCodi;
                            entityGenerado.PeridcCodi = entity.PeridcCodi;
                            entityGenerado.Username = entity.Seinusername?? "defaultvalue";
                            // mandar parámetro para actualizar el trctipopotencia 1 o 2
                            FactoryTransferencia.GetCodigoRetiroGeneradoRepository().GenerarAprobacion(entityGenerado, null, null);
                        }
                        id = entity.SoliCodiRetiCodi;
                        idCodigoSolicitud = entity.SoliCodiRetiCodi;
                        //--> old correo
                        #region Desactivar periodo actual 

                        if (entity.SoliCodiRetiEstado == "ACT")
                            FactoryTransferencia.GetCodigoRetiroGeneradoRepository().DesactivarSolicitudPeriodoActual(entity);
                        #endregion Desactivar periodo actual


                        #region Lista de codigos generados despues de generar
                        if (entity.SoliCodiRetiEstado == "ACT")
                            codigoRetirosParaEnvio = FactoryTransferencia.GetCodigoRetiroGeneradoRepository().ListarCodigosGeneradoVTP(new List<int> { entity.SoliCodiRetiCodi }, null);

                        #endregion Lista de codigos generados despues de generar

                        #region Registra Equivalencias

                        if (entity.SoliCodiRetiEstado == "ACT")
                        {
                            CodigoRetiroRelacionDTO oCodRetrel = new CodigoRetiroRelacionDTO();
                            oCodRetrel.Retrelvari = Convert.ToDecimal(ConfigurationManager.AppSettings["Variacion"].ToString());
                            oCodRetrel.Retelestado = "ACT";
                            oCodRetrel.Retrelusucreacion = entity.CoesUserName;
                            oCodRetrel.RetrelCodi = 0;
                            int idRelacion = FactoryTransferencia.GetCodigoRetiroRelacionEquivalenciasRepository().Save(oCodRetrel);
                            if (idRelacion > 0)
                            {
                                int indexEnvio = -1;
                                foreach (var item in codigoRetirosParaEnvio)
                                {
                                    indexEnvio++;

                                    CodigoRetiroRelacionDetalleDTO obj = new CodigoRetiroRelacionDetalleDTO();
                                    obj.Genemprcodivtp = Convert.ToInt32(entity.EmprCodi);
                                    obj.Cliemprcodivtp = Convert.ToInt32(entity.CliCodi);
                                    obj.Barrcodivtp = Convert.ToInt32(item.BarrCodiSum);
                                    obj.Coresocodvtp = Convert.ToInt32(item.CoregeCodi);

                                    if (indexEnvio == 0)
                                    {
                                        obj.Coresocodvtea = Convert.ToInt32(entity.SoliCodiRetiCodi);
                                        obj.Genemprcodivtea = Convert.ToInt32(entity.EmprCodi);
                                        obj.Cliemprcodivtea = Convert.ToInt32(entity.CliCodi);
                                        obj.Barrcodivtea = Convert.ToInt32(entity.BarrCodi);
                                    }
                                    obj.Retrelcodi = idRelacion;
                                    FactoryTransferencia.GetCodigoRetiroEquivalenciaDetalleRepository().Save(obj);
                                }

                            }
                        }
                        #endregion Registra Equivalencias






                        #region AuditoriaProceso
                        int estadoId = 0;
                        string mensajeError = "";
                        if (entity.SoliCodiRetiEstado == "REC")
                        {
                            estadoId = (int)EVtpEstados.Rechazado;
                            mensajeError = string.Format("Se ha rechazado la solicitud Nro.{0}", idCodigoSolicitud);
                        }
                        else if (entity.SoliCodiRetiEstado == "ACT")
                        {
                            estadoId = (int)EVtpEstados.Activo;
                            mensajeError = string.Format("Se ha aprobado la solicitud Nro.{0}", idCodigoSolicitud);

                        }
                        else if (entity.SoliCodiRetiEstado == "PVT")
                        {
                            estadoId = (int)EVtpEstados.Activo;
                            mensajeError = string.Format("La solicitud Nro.{0} ha sido aprobada por el usuario LVTA.", idCodigoSolicitud);

                        }

                        VtpAuditoriaProcesoDTO objAuditoria = new VtpAuditoriaProcesoDTO();
                        objAuditoria.Tipprocodi = (int)ETipoProcesoAuditoriaVTP.GestionCodigosEnvioInformacion;
                        objAuditoria.Estdcodi = estadoId;
                        objAuditoria.Audproproceso = "Auditoria.";
                        objAuditoria.Audprodescripcion = string.Format("Intranet - {0}", mensajeError);
                        objAuditoria.Audprousucreacion = entity.Seinusername;
                        objAuditoria.Audprofeccreacion = DateTime.Now;
                        int auditoria = new AuditoriaProcesoAppServicio().save(objAuditoria);
                        if (auditoria == 0)
                        {
                            Logger.Error(" - Error Save Auditoria");
                        }

                        #endregion

                        id = 1;
                        scope.Complete();



                    }
                    else
                    {
                        id = -2;
                        scope.Dispose();
                    }


                }
                catch (Exception ex)
                {
                    if (scope != null)
                    {
                        id = -1;
                        scope.Dispose();
                    }
                    Logger.Error(string.Format("{0}:{1}", "Aprobación", ConstantesAppServicio.LogError), ex);
                    throw new Exception(ex.Message, ex);
                }
            }


            #region Notificar Correo

            try
            {
                if (id > 0 && entity.SoliCodiRetiEstado == "ACT")
                {
                    CorreoAppServicio servCorreo = new CorreoAppServicio();
                    SiPlantillacorreoDTO plantilla = servCorreo.GetByIdSiPlantillacorreo(ConstantesExtranetCTAF.PlantcodiNotificacionCarga);

                    List<string> listaTo = plantilla.Planticorreos.Split(';').Select(x => x).ToList();

                    #region NotificacionCorreo
                    foreach (var item in codigoRetirosParaEnvio)
                    {
                        tbody.Append(string.Format(@"<tr>
                     <th style ='font-size: 9.0pt; font-family: Calibri,sans-serif;'>{0}</th>
                     <td style ='font-size: 9.0pt; font-family: Calibri,sans-serif;'>{1}</td>
                     <td style ='font-size: 9.0pt; font-family: Calibri,sans-serif;'>{2}</td>
                     <th style = 'font-size: 9.0pt; font-family: Calibri,sans-serif;' >{3}</th>
                      </tr>", entity.SoliCodiRetiCodigo, entity.BarrNombBarrTran, item.CoregeCodVTP, item.BarrNombre));
                    }
                    string asunto = "Creación de Códigos Solicitados -" + entity.EmprNombre;
                    string mensaje = string.Format(Resources.SolicitudCodigoRetiro, entity.EmprNombre, entity.CliNombre, tbody.ToString(), entity.UsuarioAgenteRegistro);
                    List<string> listTo = listaTo;
                    List<string> listBCc = new List<string>();
                    List<string> listCC = new List<string>();
                    COES.Base.Tools.Util.SendEmail(listaTo, listCC, asunto, mensaje);
                    #endregion NotificacionCorreo
                }

            }
            catch (Exception ex)
            {
                Logger.Error(string.Format("{0}:{1}", "Notificación Correo", ConstantesAppServicio.LogError), ex);
                throw new Exception(ex.Message, ex);
            }


            #endregion Notificar Correo


            resultado.Data = id;
            resultado.Mensaje = error;
            return resultado;

        }



        /// <summary>
        /// Elimina un Codigoretiro en base al iCoReSoCodi
        /// </summary>
        /// <param name="iCoReSoCodi">Codigo de la tabla TRN_CODIGO_RETIRO_SOLICITUD</param>
        /// <returns>Retorna el iCodRetCodi eliminado</returns>
        public int DeleteCodigoRetiro(int iCoReSoCodi)
        {
            try
            {
                FactoryTransferencia.GetCodigoRetiroRepository().Delete(iCoReSoCodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
            return iCoReSoCodi;
        }

        /// <summary>
        /// Elimina un Codigoretiro en base al iCoReSoCodi
        /// </summary>
        /// <param name="iCoReSoCodi">Codigo de la tabla TRN_CODIGO_RETIRO_SOLICITUD</param>
        /// <returns>Retorna el iCodRetCodi eliminado</returns>
        public int DeleteCodigoRetiroTotal(int iCoReSoCodi)
        {
            try
            {
                FactoryTransferencia.GetCodigoRetiroRepository().Delete(iCoReSoCodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
            return iCoReSoCodi;
        }

        /// <summary>
        /// Permite obtener el CodigoRetiro en base al iCoReSoCodi
        /// </summary>
        /// <param name="iCoReSoCodi">Codigo de la tabla TRN_CODIGO_RETIRO_SOLICITUD</param>
        /// <returns>CodigoRetiroDTO</returns>
        public CodigoRetiroDTO GetByIdCodigoRetiro(int iCoReSoCodi)
        {
            try
            {
                return FactoryTransferencia.GetCodigoRetiroRepository().GetById(iCoReSoCodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                return null;
            }

        }
        /// <summary>
        /// Metodo que obtiene la solicitud registrada por el agente
        /// </summary>
        /// <param name="iCoReSoCodi">codigo generado en la tabla trn_codigo_retiro_solicitud</param>
        /// /// <param name="periCodi">periodo de declaración en tabla trn_periodo_declaracion</param>
        /// <returns></returns>
        public CodigoRetiroDTO GetByIdGestionCodigosVTEAVTP(int iCoReSoCodi, int periCodi)
        {
            try
            {
                CodigoRetiroDTO codigoRetiros = FactoryTransferencia.GetCodigoRetiroRepository().GetByIdGestionCodigosVTEAVTP(iCoReSoCodi, periCodi);
                List<CodigoRetiroDetalleDTO> codigoRetiroDetalle = FactoryTransferencia.GetCodigoRetiroDetalleRepository().ListarCodigoRetiroDetalle(iCoReSoCodi);
                List<CodigoRetiroGeneradoDTO> codigoRetiroGenerados = FactoryTransferencia.GetCodigoRetiroGeneradoRepository().ListarCodigosGeneradoVTP(new List<int> { iCoReSoCodi }, null);
                if (codigoRetiros.EstApr=="PEN")
                {
                    codigoRetiros.ListaPotenciaContratadas = FactoryTransferencia.GetTrnPotenciaContratadaRepository().GetPotenciasContratadasAprobar(iCoReSoCodi, periCodi);
                }
                else
                {
                    codigoRetiros.ListaPotenciaContratadas = FactoryTransferencia.GetTrnPotenciaContratadaRepository().GetPotenciasContratadas(iCoReSoCodi, periCodi);
                }

                if (codigoRetiroDetalle.Count > 0)
                    codigoRetiros.ListarBarraDetalle = codigoRetiroDetalle;
                if (codigoRetiroGenerados.Count > 0)
                    codigoRetiros.ListarBarraSuministro = codigoRetiroGenerados.Where(x => x.CoresoCodi == iCoReSoCodi).OrderBy(c => c.CoregeCodi).ToList();


                if (codigoRetiros.TrnpcTipoCasoAgrupado == "AGRVTA")
                {
                    TrnPotenciaContratadaDTO resultadoPotencia = codigoRetiros.ListaPotenciaContratadas.FirstOrDefault();
                    codigoRetiros.TrnPctHpmwFija = resultadoPotencia?.TrnPctHpMwFija;
                    codigoRetiros.TrnPctHfpmwFija = resultadoPotencia?.TrnPctHfpMwFija;
                    codigoRetiros.TrnPctTotalmwFija = resultadoPotencia?.TrnPctTotalMwFija;
                    codigoRetiros.TrnPctHpmwFijaVariable = resultadoPotencia?.TrnPctHpMwFijaVariable;
                    codigoRetiros.TrnPctHfpmwFijaVariable = resultadoPotencia?.TrnPctHfpMwFijaVariable;
                    codigoRetiros.TrnPctTotalmwVariable = resultadoPotencia?.TrnPctTotalMwVariable;
                    codigoRetiros.esPrimerRegistro = resultadoPotencia?.esPrimerRegistro;
                }
                return codigoRetiros;
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                return null;
            }

        }


        /// <summary>
        /// Permite listar todas codigoRetiro en base a su estado
        /// </summary>
        /// <param name="sCoReSoEstado">Estado en que se encuentra el Código de Retiro Solicitado</param>       
        /// <returns>Lista de CodigoRetiroDTO</returns>
        public List<CodigoRetiroDTO> ListCodigoRetiro(string sCoReSoEstado)
        {
            try
            {
                return FactoryTransferencia.GetCodigoRetiroRepository().List(sCoReSoEstado);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                return null;
            }
        }

        /// <summary>
        /// Permite listar las solicitudes realizadas para generar el codigo VTEA y VTP
        /// </summary>
        /// <param name="genEmprCodi"></param>
        /// <param name="cliCodi"></param>
        /// <param name="tipoCont"></param>
        /// <param name="tipoUsu"></param>
        /// <param name="barrCodi"></param>
        /// <param name="barrCodiSum"></param>
        /// <param name="coresoEstado"></param>
        /// <param name="coregeCodVteaVtp"></param>
        /// <param name="fechaIni"></param>
        /// <param name="fechaFin"></param>
        /// <param name="nroPagina"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public ResultadoDTO<List<CodigoRetiroDTO>> ListarGestionCodigosVTEAVTP(string userName, int? genEmprCodi, int? cliCodi, int? tipoCont, int? tipoUsu, int? barrCodi, int? barrCodiSum, string coresoEstado, string coresoestapr, string coregeCodVteaVtp, DateTime? fechaIni, DateTime? fechaFin, int periCodi, int nroPagina, int pageSize, string base64 = null, string base64Comparar = null)
        {
            ResultadoDTO<List<CodigoRetiroDTO>> resultado = new ResultadoDTO<List<CodigoRetiroDTO>>();

            List<CodigoRetiroDTO> codigoRetirosAuxiliar = new List<CodigoRetiroDTO>();
            List<CodigoRetiroDTO> codigoRetiros = new List<CodigoRetiroDTO>();
            List<CodigoRetiroGeneradoDTO> codigoRetiroDetalle = null;

            ResultadoDTO<List<CodigoRetiroGeneradoDTO>> resultadoExcel = null;

            if (base64 != null)
            {
                resultadoExcel = new SolicitudCodigoAppServicio().ListarCodigoRetiroFromExcelIntranet(base64, base64Comparar, 1);

                if (resultadoExcel.EsCorrecto < 0)
                {
                    resultado.EsCorrecto = resultadoExcel.EsCorrecto;
                    resultado.Mensaje = resultadoExcel.Mensaje;
                }

            }
            if (resultadoExcel != null && resultadoExcel.EsCorrecto != -2)
            {
                codigoRetiros = FactoryTransferencia.GetCodigoRetiroRepository().ListarGestionCodigosVTEAVTP(genEmprCodi, cliCodi, tipoCont, tipoUsu, barrCodi, barrCodiSum, coresoEstado, coresoestapr, coregeCodVteaVtp, fechaIni, fechaFin, periCodi, nroPagina, pageSize);
            }       
            else if (resultadoExcel == null)
            {
                if (string.IsNullOrEmpty(coresoestapr))
                {
                    codigoRetiros = FactoryTransferencia.GetCodigoRetiroRepository().ListarGestionCodigosVTEAVTP(genEmprCodi, cliCodi, tipoCont, tipoUsu, barrCodi, barrCodiSum, coresoEstado, coresoestapr, coregeCodVteaVtp, fechaIni, fechaFin, periCodi, nroPagina, pageSize);
                }
                else
                {
                    codigoRetiros = FactoryTransferencia.GetCodigoRetiroRepository().ListarGestionCodigosVTEAVTPAprobar(genEmprCodi, cliCodi, tipoCont, tipoUsu, barrCodi, barrCodiSum, coresoEstado, coresoestapr, coregeCodVteaVtp, fechaIni, fechaFin, periCodi, nroPagina, pageSize);
                }
                
            }
                

            ///codigoRetiros = codigoRetiros.Where(x => x.SoliCodiRetiCodi == 5614).ToList();
            if (codigoRetiros.Count > 0)
            {
                IEnumerable<int> codigoRetiroLista = codigoRetiros.Select(x => x.SoliCodiRetiCodi).Distinct();
                foreach (var item in codigoRetiroLista)
                {
                    List<CodigoRetiroDTO> codigoRetiroFiltro = codigoRetiros.Where(f => f.SoliCodiRetiCodi == item).AsParallel().ToList();
                    if (codigoRetiroFiltro != null && codigoRetiroFiltro.Count > 0)
                    {
                        CodigoRetiroDTO entidadCodigoRetiro = codigoRetiros.First(x => x.SoliCodiRetiCodi == item);
                        //CodigoRetiroDTO entidadCodigoRetiro = codigoRetiros.FirstOrDefault();
                        codigoRetirosAuxiliar.Add(entidadCodigoRetiro);
                        codigoRetiroDetalle = new List<CodigoRetiroGeneradoDTO>();
                        codigoRetiroFiltro.ForEach(child =>
                        {
                            codigoRetiroDetalle.Add(new CodigoRetiroGeneradoDTO
                            {
                                CoresoCodi = child.SoliCodiRetiCodi,
                                CoregeCodi = child.CoregeCodi,
                                BarrCodiSum = child.BarrCodiSum,
                                BarrNombre = child.BarrNombBarrSum,
                                CoregeCodVTP = child.CoregeCodVTP,
                                EstdDescripcion = child.EstDescripcionVTP,
                                EstdAbrev = child.EstAbrevVTP,
                                PotenciaContratadaDTO = new SolicitudCodigoPotenciaContratadaDTO
                                {
                                    CodigoAgrupacion = child.TrnpCagrp,
                                    CoresoCodigo = child.CoresoCodiPotcn,
                                    CoregeCodigo = child.CoregeCodiPotcn,
                                    TipoAgrupacion = child.TipCasaAbrev,
                                    PotenciaContrTotalFija = child.TrnPctTotalmwFija,
                                    PotenciaContrHPFija = child.TrnPctHpmwFija,
                                    PotenciaContrHFPFija = child.TrnPctHfpmwFija,
                                    PotenciaContrTotalVar = child.TrnPctTotalmwVariable,
                                    PotenciaContrHPVar = child.TrnPctHpmwFijaVariable,
                                    PotenciaContrHFPVar = child.TrnPctHfpmwFijaVariable,
                                    PotenciaContrObservacion = child.TrnPctComeObs,
                                }
                            });
                        });

                        entidadCodigoRetiro.ListarBarraSuministro = codigoRetiroDetalle;
                    }
                }

                // Potencias contratadas
                foreach (var item in codigoRetiros)
                {
                    bool finalizar = false;

                    if (item.OmitirFilaVTA == 1 || item.ListarBarraSuministro == null)
                        continue;

                    foreach (var detalle in item.ListarBarraSuministro)
                    {
                        if (detalle.OmitirFila == 1)
                            continue;

                        //---------------------------------------------------------------------------------------
                        //Si 5 Existen registros
                        //---------------------------------------------------------------------------------------
                        List<CodigoRetiroDTO> obtenerPotencias = new List<CodigoRetiroDTO>();
                        if (detalle.PotenciaContratadaDTO.TipoAgrupacion == AgrupacionVTP
                            || detalle.PotenciaContratadaDTO.TipoAgrupacion == AgrupacionVTA)
                        {
                            int? rowSpan = 0;
                            List<int> omitirFilas = new List<int>();
                            int encontroPrimerRegistro = 0;
                            string Observacion = "";
                            decimal? PotenciaContrTotalFija = 0;
                            decimal? PotenciaContrHPFija = 0;
                            decimal? PotenciaContrHFPFija = 0;
                            decimal? PotenciaContrTotalVar = 0;
                            decimal? PotenciaContrHPVar = 0;
                            decimal? PotenciaContrHFPVar = 0;

                            int PotenciaEsExcel = 0;


                            if (detalle.PotenciaContratadaDTO.TipoAgrupacion == AgrupacionVTP)
                            {
                                obtenerPotencias = codigoRetiros.Where(x => x.CoresoCodiPotcn == item.CoresoCodiPotcn
                            && x.TrnpCagrp == detalle.PotenciaContratadaDTO.CodigoAgrupacion
                            ).ToList();
                            }
                            else
                            {
                                //Porque corregir
                                if (detalle.PotenciaContratadaDTO.CodigoAgrupacion is null)
                                    break;
                                obtenerPotencias = codigoRetiros.Where(x => x.TrnpCagrp == detalle.PotenciaContratadaDTO.CodigoAgrupacion).OrderBy(c => c.TrnpcNumordm).ToList();


                            }

                            // Obtiene sus potencias asociadas
                            rowSpan = obtenerPotencias.Count;
                            foreach (var itemPotencia in obtenerPotencias)
                            {
                                if (encontroPrimerRegistro > 0)
                                {

                                    if (detalle.PotenciaContratadaDTO.TipoAgrupacion == AgrupacionVTP)
                                        omitirFilas.Add(itemPotencia.CoregeCodi);
                                    else if (detalle.PotenciaContratadaDTO.TipoAgrupacion == AgrupacionVTA)
                                        omitirFilas.Add(Convert.ToInt32(itemPotencia.CoresoCodiPotcn));
                                }

                                //---------------------------------------------------------------------------------------
                                //Si es VTP
                                //---------------------------------------------------------------------------------------
                                if (detalle.PotenciaContratadaDTO.TipoAgrupacion == AgrupacionVTP)
                                {

                                    if (resultadoExcel == null)
                                    {
                                        if (itemPotencia.TrnPctExcel == 1)
                                        {
                                            PotenciaContrTotalFija = itemPotencia.TrnPctTotalmwFija;
                                            PotenciaContrHPFija = itemPotencia.TrnPctHpmwFija;
                                            PotenciaContrHFPFija = itemPotencia.TrnPctHfpmwFija;

                                            PotenciaContrTotalVar = itemPotencia.TrnPctTotalmwVariable;
                                            PotenciaContrHPVar = itemPotencia.TrnPctHpmwFijaVariable;
                                            PotenciaContrHFPVar = itemPotencia.TrnPctHfpmwFijaVariable;

                                        }
                                        else
                                        {
                                            PotenciaContrTotalFija += itemPotencia.TrnPctTotalmwFija;
                                            PotenciaContrHPFija += itemPotencia.TrnPctHpmwFija;
                                            PotenciaContrHFPFija += itemPotencia.TrnPctHfpmwFija;

                                            PotenciaContrTotalVar += itemPotencia.TrnPctTotalmwVariable;
                                            PotenciaContrHPVar += itemPotencia.TrnPctHpmwFijaVariable;
                                            PotenciaContrHFPVar += itemPotencia.TrnPctHfpmwFijaVariable;

                                        }

                                    }
                                    else
                                    {

                                        #region Obtiene del archivo Excel
                                        CodigoRetiroGeneradoDTO potenciaVTAExcel = resultadoExcel.Data.Where(x => x.CoresoCodi == item.SoliCodiRetiCodi).FirstOrDefault();
                                        #endregion Obtiene del archivo Excel

                                        if (potenciaVTAExcel != null)
                                        {

                                            if (potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrTotalFija != null)
                                                PotenciaContrTotalFija = potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrTotalFija;

                                            if (potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrHPFija != null)
                                                PotenciaContrHPFija = potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrHPFija;

                                            if (potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrHFPFija != null)
                                                PotenciaContrHFPFija = potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrHFPFija;

                                            if (potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrTotalVar != null)
                                                PotenciaContrTotalVar = potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrTotalVar;

                                            if (potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrHPVar != null)
                                                PotenciaContrHPVar = potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrHPVar;

                                            if (potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrHFPVar != null)
                                                PotenciaContrHFPVar = potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrHFPVar;
                                            //obs.
                                            if (potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrObservacion != null)
                                                Observacion = potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrObservacion;

                                            if (potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrTotalFija != null
                                    || potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrHPFija != null
                                    || potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrHFPFija != null
                                    || potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrTotalVar != null
                                    || potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrHPVar != null
                                    || potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrHFPVar != null
                                    || potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrObservacion != null
                                    )
                                                PotenciaEsExcel = 1;
                                        }
                                    }



                                }

                                //---------------------------------------------------------------------------------------
                                //Si es VTA
                                //---------------------------------------------------------------------------------------
                                else if (
                                    detalle.PotenciaContratadaDTO.TipoAgrupacion == AgrupacionVTA
                                    && encontroPrimerRegistro == 0)
                                {
                                    if (resultadoExcel != null)
                                    {
                                        #region Obtiene del archivo Excel
                                        CodigoRetiroGeneradoDTO potenciaVTAExcel = resultadoExcel.Data.Where(x => x.CoresoCodi == item.SoliCodiRetiCodi).FirstOrDefault();
                                        #endregion Obtiene del archivo Excel

                                        if (potenciaVTAExcel != null)
                                        {

                                            if (potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrTotalFija != null)
                                                itemPotencia.TrnPctTotalmwFija = potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrTotalFija;

                                            if (potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrHPFija != null)
                                                itemPotencia.TrnPctHpmwFija = potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrHPFija;

                                            if (potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrHFPFija != null)
                                                itemPotencia.TrnPctHfpmwFija = potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrHFPFija;

                                            if (potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrTotalVar != null)
                                                itemPotencia.TrnPctTotalmwVariable = potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrTotalVar;

                                            if (potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrHPVar != null)
                                                itemPotencia.TrnPctHpmwFijaVariable = potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrHPVar;

                                            if (potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrHFPVar != null)
                                                itemPotencia.TrnPctHfpmwFijaVariable = potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrHFPVar;

                                            //obs.
                                            if (potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrObservacion != null)
                                                itemPotencia.TrnPctComeObs = potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrObservacion;


                                            if (potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrTotalFija != null
                                    || potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrHPFija != null
                                    || potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrHFPFija != null
                                    || potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrTotalVar != null
                                    || potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrHPVar != null
                                    || potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrHFPVar != null
                                      || potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrObservacion != null
                                    )
                                                itemPotencia.TrnPctExcel = 1;



                                        }
                                    }


                                    PotenciaContrTotalFija = itemPotencia.TrnPctTotalmwFija;
                                    PotenciaContrHPFija = itemPotencia.TrnPctHpmwFija;
                                    PotenciaContrHFPFija = itemPotencia.TrnPctHfpmwFija;
                                    PotenciaContrTotalVar = itemPotencia.TrnPctTotalmwVariable;
                                    PotenciaContrHPVar = itemPotencia.TrnPctHpmwFijaVariable;
                                    PotenciaContrHFPVar = itemPotencia.TrnPctHfpmwFijaVariable;
                                    Observacion = itemPotencia.TrnPctComeObs;
                                    //EsExcel
                                    PotenciaEsExcel = itemPotencia.TrnPctExcel;



                                }

                                encontroPrimerRegistro++;

                            }

                            detalle.PotenciaContratadaDTO.PotenciaContrTotalFija = PotenciaContrTotalFija;
                            detalle.PotenciaContratadaDTO.PotenciaContrHPFija = PotenciaContrHPFija;
                            detalle.PotenciaContratadaDTO.PotenciaContrHFPFija = PotenciaContrHFPFija;
                            detalle.PotenciaContratadaDTO.PotenciaContrTotalVar = PotenciaContrTotalVar;
                            detalle.PotenciaContratadaDTO.PotenciaContrHPVar = PotenciaContrHPVar;
                            detalle.PotenciaContratadaDTO.PotenciaContrHFPVar = PotenciaContrHFPVar;
                            detalle.PotenciaContratadaDTO.PotenciaContrObservacion = Observacion;
                            //EsExcel
                            detalle.PotenciaContratadaDTO.PotenciaEsExcel = PotenciaEsExcel;



                            detalle.PotenciaContratadaDTO.RowSpan = rowSpan == 0 ? null : rowSpan;
                            foreach (var itemFormato in codigoRetiros)
                            {
                                IEnumerable<CodigoRetiroGeneradoDTO> detalleEncontrado = null;

                                if (itemFormato.TipCasaAbrev == AgrupacionVTP
                                   && itemFormato.ListarBarraSuministro != null)
                                {
                                    detalleEncontrado = itemFormato.ListarBarraSuministro.Where(x =>
                                      omitirFilas.Contains(x.CoregeCodi)
                                    );
                                }
                                else if (itemFormato.TipCasaAbrev == AgrupacionVTA
                                     && itemFormato.ListarBarraSuministro != null)
                                {
                                    detalleEncontrado = itemFormato.ListarBarraSuministro.Where(x =>
                                     omitirFilas.Contains((int)(x.PotenciaContratadaDTO.CoresoCodigo ?? 0))
                                   );
                                }

                                if (detalleEncontrado != null)
                                {
                                    foreach (var itemDetalle in detalleEncontrado)
                                        itemDetalle.OmitirFila = 1;
                                    if (detalleEncontrado.Count() > 0
                                    && detalle.PotenciaContratadaDTO.TipoAgrupacion == AgrupacionVTA
                                    && itemFormato.CoresoCodiPotcn != item.CoresoCodiPotcn)
                                    {
                                        itemFormato.OmitirFilaVTA = 1;
                                        itemFormato.SoliCodiRetiCodiVTAParent = (int)item.CoresoCodiPotcn;
                                    }
                                }
                            }

                        }
                        else
                            obtenerPotencias = codigoRetiros.Where(x => x.SoliCodiRetiCodi == item.SoliCodiRetiCodi).ToList();

                        //---------------------------------------------------------------------------------------
                        //Si no Existen registros
                        //---------------------------------------------------------------------------------------
                        if (obtenerPotencias.Count > 0
                             && detalle.TipCasaAbrev != AgrupacionVTP)
                        {
                            List<int> omitirFilas = new List<int>();
                            int? rowSpan = obtenerPotencias.Count;
                            int encontroPrimerRegistro = 0;
                            CodigoRetiroDTO potenciaEncontrada = null;
                            foreach (var itemPtcn in obtenerPotencias)
                            {
                                //Potencias registradas a nivel de VTEA
                                //if (itemPtcn.SoliCodiRetiCodi != null
                                // && itemPtcn.CoregeCodiPotcn == null
                                if (itemPtcn.SoliCodiRetiCodi != 0
                                    && itemPtcn.CoregeCodiPotcn != 0
                                    && (itemPtcn.TipCasaAbrev == AgrupacionVTA)
                                    && encontroPrimerRegistro == 0)
                                {
                                    encontroPrimerRegistro++;

                                    if (resultadoExcel == null)
                                        potenciaEncontrada = itemPtcn;
                                    else
                                    {
                                        #region Obtiene del archivo Excel
                                        CodigoRetiroGeneradoDTO potenciaVTAExcel = resultadoExcel.Data.Where(x => x.CoresoCodi == itemPtcn.SoliCodiRetiCodi).FirstOrDefault();
                                        #endregion Obtiene del archivo Excel

                                        potenciaEncontrada = itemPtcn;

                                        //esExcel
                                        if (potenciaVTAExcel != null)
                                        {

                                            potenciaEncontrada.CoresoCodiPotcn = itemPtcn.CoregeCodiPotcn;
                                            potenciaEncontrada.CoregeCodiPotcn = itemPtcn.CoregeCodiPotcn;
                                            potenciaEncontrada.TipCasaAbrev = itemPtcn.TipCasaAbrev;
                                            if (potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrTotalFija != null)
                                                potenciaEncontrada.TrnPctTotalmwFija = potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrTotalFija;

                                            if (potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrHPFija != null)
                                                potenciaEncontrada.TrnPctHpmwFija = potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrHPFija;

                                            if (potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrHFPFija != null)
                                                potenciaEncontrada.TrnPctHfpmwFija = potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrHFPFija;

                                            if (potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrTotalVar != null)
                                                potenciaEncontrada.TrnPctTotalmwVariable = potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrTotalVar;
                                            if (potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrHPVar != null)
                                                potenciaEncontrada.TrnPctHpmwFijaVariable = potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrHPVar;

                                            if (potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrHFPVar != null)
                                                potenciaEncontrada.TrnPctHfpmwFijaVariable = potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrHFPVar;
                                            //obs.
                                            if (potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrObservacion != null)
                                                potenciaEncontrada.TrnPctComeObs = potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrObservacion;

                                            if (potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrTotalFija != null
                                         || potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrHPFija != null
                                         || potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrHFPFija != null
                                         || potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrTotalVar != null
                                         || potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrHPVar != null
                                         || potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrHFPVar != null
                                     || potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrObservacion != null
                                         )
                                                potenciaEncontrada.TrnPctExcel = 1;

                                        }
                                    }
                                    continue;
                                }
                                //Potencias registradas a nivel de VTP
                                //else if (itemPtcn.CoresoCodiPotcn != null
                                //  && itemPtcn.CoregeCodiPotcn != null
                                else if (itemPtcn.SoliCodiRetiCodi != 0

                                && string.IsNullOrEmpty(itemPtcn.TipCasaAbrev))
                                {
                                    if (item.TrnpcTipoPotencia == 2 && itemPtcn.CoregeCodiPotcn != 0)
                                    {
                                        rowSpan = null;
                                        potenciaEncontrada = null;
                                        break;
                                    }
                                    else
                                    {
                                        if (encontroPrimerRegistro == 0)
                                        {
                                            encontroPrimerRegistro++;
                                            itemPtcn.TipCasaAbrev = AgrupacionVTA;
                                            if (resultadoExcel == null)
                                                potenciaEncontrada = itemPtcn;
                                            else
                                            {
                                                #region Obtiene del archivo Excel
                                                CodigoRetiroGeneradoDTO potenciaVTAExcel = resultadoExcel.Data.Where(x => x.CoresoCodi == itemPtcn.SoliCodiRetiCodi).FirstOrDefault();
                                                #endregion Obtiene del archivo Excel

                                                potenciaEncontrada = itemPtcn;

                                                //esExcel
                                                if (potenciaVTAExcel != null)
                                                {
                                                    potenciaEncontrada.CoresoCodiPotcn = itemPtcn.CoregeCodiPotcn;
                                                    potenciaEncontrada.CoregeCodiPotcn = itemPtcn.CoregeCodiPotcn;
                                                    potenciaEncontrada.TipCasaAbrev = itemPtcn.TipCasaAbrev;
                                                    if (potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrTotalFija != null)
                                                        potenciaEncontrada.TrnPctTotalmwFija = potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrTotalFija;

                                                    if (potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrHPFija != null)
                                                        potenciaEncontrada.TrnPctHpmwFija = potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrHPFija;

                                                    if (potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrHFPFija != null)
                                                        potenciaEncontrada.TrnPctHfpmwFija = potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrHFPFija;

                                                    if (potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrTotalVar != null)
                                                        potenciaEncontrada.TrnPctTotalmwVariable = potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrTotalVar;
                                                    if (potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrHPVar != null)
                                                        potenciaEncontrada.TrnPctHpmwFijaVariable = potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrHPVar;

                                                    if (potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrHFPVar != null)
                                                        potenciaEncontrada.TrnPctHfpmwFijaVariable = potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrHFPVar;

                                                    //obs.
                                                    if (potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrObservacion != null)
                                                        potenciaEncontrada.TrnPctComeObs = potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrObservacion;


                                                    if (potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrTotalFija != null
                                                 || potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrHPFija != null
                                                 || potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrHFPFija != null
                                                 || potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrTotalVar != null
                                                 || potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrHPVar != null
                                                 || potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrHFPVar != null
                                                  || potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrObservacion != null
                                                 )
                                                    {
                                                        potenciaEncontrada.TrnpcTipoPotencia = 1;
                                                        potenciaEncontrada.TrnPctExcel = 1;
                                                    }

                                                }
                                            }

                                        }
                                        else
                                        {
                                            omitirFilas.Add(itemPtcn.CoregeCodiPotcn ?? itemPtcn.CoregeCodi);
                                        }
                                    }
                                }
                                else
                                {
                                    omitirFilas.Add(itemPtcn.CoregeCodiPotcn ?? itemPtcn.CoregeCodi);
                                }
                            }

                            if (potenciaEncontrada != null && rowSpan != null)
                            {
                                SolicitudCodigoPotenciaContratadaDTO itemPotencia = new SolicitudCodigoPotenciaContratadaDTO();
                                itemPotencia.CoresoCodigo = potenciaEncontrada.CoresoCodiPotcn;
                                itemPotencia.CoregeCodigo = potenciaEncontrada.CoregeCodiPotcn;
                                itemPotencia.TipoAgrupacion = potenciaEncontrada.TipCasaAbrev;
                                itemPotencia.PotenciaContrTotalFija = potenciaEncontrada.TrnPctTotalmwFija;
                                itemPotencia.PotenciaContrHPFija = potenciaEncontrada.TrnPctHpmwFija;
                                itemPotencia.PotenciaContrHFPFija = potenciaEncontrada.TrnPctHfpmwFija;
                                itemPotencia.PotenciaContrTotalVar = potenciaEncontrada.TrnPctTotalmwVariable;
                                itemPotencia.PotenciaContrHPVar = potenciaEncontrada.TrnPctHpmwFijaVariable;
                                itemPotencia.PotenciaContrHFPVar = potenciaEncontrada.TrnPctHfpmwFijaVariable;
                                itemPotencia.PotenciaContrObservacion = potenciaEncontrada.TrnPctComeObs;

                                //esExcel
                                itemPotencia.PotenciaEsExcel = potenciaEncontrada.TrnPctExcel;

                                //obs
                                itemPotencia.PotenciaContrObservacion = potenciaEncontrada.TrnPctComeObs;
                                itemPotencia.RowSpan = rowSpan == 0 ? null : rowSpan;
                                item.OmitirFilaVTA = 0;
                                detalle.PotenciaContratadaDTO = itemPotencia;

                                foreach (var itemFormato in codigoRetiros)
                                {
                                    if (itemFormato.ListarBarraSuministro != null)
                                    {
                                        var detalleEncontrado = itemFormato.ListarBarraSuministro.Where(x => omitirFilas.Contains(x.CoregeCodi));
                                        foreach (var itemDetalle in detalleEncontrado)
                                        {
                                            itemDetalle.OmitirFila = 1;
                                        }
                                    }

                                }
                            }
                            else if (rowSpan == null)
                            {
                                var t = detalle.PotenciaContratadaDTO;
                                if (resultadoExcel != null)
                                {
                                    #region Obtiene del archivo Excel
                                    CodigoRetiroGeneradoDTO potenciaVTAExcel = resultadoExcel.Data.Where(x => x.CoregeCodi == detalle.CoregeCodi).FirstOrDefault();
                                    #endregion Obtiene del archivo Excel
                                    //EsExcel
                                    if (potenciaVTAExcel != null)
                                    {

                                        SolicitudCodigoPotenciaContratadaDTO itemPotencia = new SolicitudCodigoPotenciaContratadaDTO();
                                        itemPotencia.CoresoCodigo = item.SoliCodiRetiCodi;
                                        itemPotencia.CoregeCodigo = detalle.PotenciaContratadaDTO.CoregeCodigo;
                                        itemPotencia.TipoAgrupacion = null;

                                        if (potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrTotalFija != null)
                                            itemPotencia.PotenciaContrTotalFija = potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrTotalFija;

                                        if (potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrHPFija != null)
                                            itemPotencia.PotenciaContrHPFija = potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrHPFija;

                                        if (potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrHFPFija != null)
                                            itemPotencia.PotenciaContrHFPFija = potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrHFPFija;

                                        if (potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrTotalVar != null)
                                            itemPotencia.PotenciaContrTotalVar = potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrTotalVar;

                                        if (potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrHPVar != null)

                                            itemPotencia.PotenciaContrHPVar = potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrHPVar;

                                        if (potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrHFPVar != null)
                                            itemPotencia.PotenciaContrHFPVar = potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrHFPVar;

                                        //obs.
                                        if (potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrObservacion != null)
                                            itemPotencia.PotenciaContrObservacion = potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrObservacion;

                                        if (potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrTotalFija != null
                                            || potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrHPFija != null
                                            || potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrHFPFija != null
                                            || potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrTotalVar != null
                                            || potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrHPVar != null
                                            || potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrHFPVar != null

                                        || potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrObservacion != null
                                            )
                                            itemPotencia.PotenciaEsExcel = 1;


                                        itemPotencia.PotenciaContrObservacion = potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrObservacion;
                                        detalle.PotenciaContratadaDTO = itemPotencia;
                                    }
                                }
                            }

                        }


                    }


                }


            }

            resultado.Data = codigoRetirosAuxiliar;


            return resultado;
        }

        /// <summary>
        /// Permite listar las solicitudes realizadas para generar el codigo VTEA y VTP
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="genEmprCodi"></param>
        /// <param name="cliCodi"></param>
        /// <param name="tipoCont"></param>
        /// <param name="tipoUsu"></param>
        /// <param name="barrCodi"></param>
        /// <param name="barrCodiSum"></param>
        /// <param name="coresoEstado"></param>
        /// <param name="coregeCodVteaVtp"></param>
        /// <param name="fechaIni"></param>
        /// <param name="fechaFin"></param>
        /// <param name="periCodi"></param>
        /// <param name="nroPagina"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public ResultadoDTO<List<CodigoRetiroDTO>> ListarGestionCodigosExportarVTEAVTP(string userName, int? genEmprCodi, int? cliCodi, int? tipoCont, int? tipoUsu, int? barrCodi, int? barrCodiSum, string coresoEstado,
                                                                                       string coregeCodVteaVtp, DateTime? fechaIni, DateTime? fechaFin, int periCodi, int nroPagina, int pageSize)
        {
            ResultadoDTO<List<CodigoRetiroDTO>> resultado = new ResultadoDTO<List<CodigoRetiroDTO>>();
            List<CodigoRetiroDTO> codigoRetirosAuxiliar = new List<CodigoRetiroDTO>();

            List<CodigoRetiroDTO> codigoRetiros = FactoryTransferencia.GetCodigoRetiroRepository()
                                                  .ListarGestionCodigosExportarVTEAVTP(genEmprCodi, cliCodi, tipoCont, tipoUsu, barrCodi, barrCodiSum, coresoEstado, coregeCodVteaVtp, fechaIni, fechaFin, periCodi, nroPagina, pageSize)
                                                  .OrderBy(o => o.EmprNombre)
                                                  .OrderBy(o => o.CliNombre)
                                                  .OrderBy(o => o.FechaInicio)                                                 
                                                  .ToList();
            
            List<CodigoRetiroGeneradoDTO> codigoRetiroDetalle = null;
            ResultadoDTO<List<CodigoRetiroGeneradoDTO>> resultadoExcel = null;

            if (codigoRetiros.Count > 0)
            {
                IEnumerable<int> codigoRetiroLista = codigoRetiros.Select(x => x.SoliCodiRetiCodi).Distinct();
                foreach (var item in codigoRetiroLista)
                {
                    List<CodigoRetiroDTO> codigoRetiroFiltro = codigoRetiros.Where(f => f.SoliCodiRetiCodi == item).AsParallel().ToList();
                    if (codigoRetiroFiltro != null && codigoRetiroFiltro.Count > 0)
                    {
                        CodigoRetiroDTO entidadCodigoRetiro = codigoRetiros.First(x => x.SoliCodiRetiCodi == item);
                        //CodigoRetiroDTO entidadCodigoRetiro = codigoRetiros.FirstOrDefault();
                        codigoRetirosAuxiliar.Add(entidadCodigoRetiro);
                        codigoRetiroDetalle = new List<CodigoRetiroGeneradoDTO>();
                        codigoRetiroFiltro.ForEach(child =>
                        {
                            codigoRetiroDetalle.Add(new CodigoRetiroGeneradoDTO
                            {
                                CoresoCodi = child.SoliCodiRetiCodi,
                                CoregeCodi = child.CoregeCodi,
                                BarrCodiSum = child.BarrCodiSum,
                                BarrNombre = child.BarrNombBarrSum,
                                CoregeCodVTP = child.CoregeCodVTP,
                                EstdDescripcion = child.EstDescripcionVTP,
                                EstdAbrev = child.EstAbrevVTP,
                                PotenciaContratadaDTO = new SolicitudCodigoPotenciaContratadaDTO
                                {
                                    CodigoAgrupacion = child.TrnpCagrp,
                                    CoresoCodigo = child.CoresoCodiPotcn,
                                    CoregeCodigo = child.CoregeCodiPotcn,
                                    TipoAgrupacion = child.TipCasaAbrev,
                                    PotenciaContrTotalFija = child.TrnPctTotalmwFija,
                                    PotenciaContrHPFija = child.TrnPctHpmwFija,
                                    PotenciaContrHFPFija = child.TrnPctHfpmwFija,
                                    PotenciaContrTotalVar = child.TrnPctTotalmwVariable,
                                    PotenciaContrHPVar = child.TrnPctHpmwFijaVariable,
                                    PotenciaContrHFPVar = child.TrnPctHfpmwFijaVariable,
                                    PotenciaContrObservacion = child.TrnPctComeObs,
                                }
                            });
                        });

                        entidadCodigoRetiro.ListarBarraSuministro = codigoRetiroDetalle;
                    }
                }

                // Potencias contratadas
                foreach (var item in codigoRetiros)
                {

                    if (item.OmitirFilaVTA == 1 || item.ListarBarraSuministro == null)
                        continue;

                    foreach (var detalle in item.ListarBarraSuministro)
                    {
                        if (detalle.OmitirFila == 1)
                            continue;

                        //---------------------------------------------------------------------------------------
                        //Si 5 Existen registros
                        //---------------------------------------------------------------------------------------
                        List<CodigoRetiroDTO> obtenerPotencias = new List<CodigoRetiroDTO>();
                        if (detalle.PotenciaContratadaDTO.TipoAgrupacion == AgrupacionVTP
                            || detalle.PotenciaContratadaDTO.TipoAgrupacion == AgrupacionVTA)
                        {
                            int? rowSpan = 0;
                            List<int> omitirFilas = new List<int>();
                            int encontroPrimerRegistro = 0;
                            string Observacion = "";
                            decimal? PotenciaContrTotalFija = 0;
                            decimal? PotenciaContrHPFija = 0;
                            decimal? PotenciaContrHFPFija = 0;
                            decimal? PotenciaContrTotalVar = 0;
                            decimal? PotenciaContrHPVar = 0;
                            decimal? PotenciaContrHFPVar = 0;

                            int PotenciaEsExcel = 0;

                            if (detalle.PotenciaContratadaDTO.TipoAgrupacion == AgrupacionVTP)
                            {
                                obtenerPotencias = codigoRetiros.Where(x => x.CoresoCodiPotcn == item.CoresoCodiPotcn
                            && x.TrnpCagrp == detalle.PotenciaContratadaDTO.CodigoAgrupacion
                            ).ToList();
                            }
                            else
                            {
                                //Porque corregir
                                if (detalle.PotenciaContratadaDTO.CodigoAgrupacion is null)
                                    break;
                                obtenerPotencias = codigoRetiros.Where(x => x.TrnpCagrp == detalle.PotenciaContratadaDTO.CodigoAgrupacion
                            ).OrderBy(c => c.TrnpcNumordm).ToList();

                            }

                            // Obtiene sus potencias asociadas
                            rowSpan = obtenerPotencias.Count;
                            foreach (var itemPotencia in obtenerPotencias)
                            {
                                if (encontroPrimerRegistro > 0)
                                {

                                    if (detalle.PotenciaContratadaDTO.TipoAgrupacion == AgrupacionVTP)
                                        omitirFilas.Add(itemPotencia.CoregeCodi);
                                    else if (detalle.PotenciaContratadaDTO.TipoAgrupacion == AgrupacionVTA)
                                        omitirFilas.Add(Convert.ToInt32(itemPotencia.CoresoCodiPotcn));
                                }

                                //---------------------------------------------------------------------------------------
                                //Si es VTP
                                //---------------------------------------------------------------------------------------
                                if (detalle.PotenciaContratadaDTO.TipoAgrupacion == AgrupacionVTP)
                                {

                                    if (resultadoExcel == null)
                                    {
                                        if (itemPotencia.TrnPctExcel == 1)
                                        {
                                            PotenciaContrTotalFija = itemPotencia.TrnPctTotalmwFija;
                                            PotenciaContrHPFija = itemPotencia.TrnPctHpmwFija;
                                            PotenciaContrHFPFija = itemPotencia.TrnPctHfpmwFija;

                                            PotenciaContrTotalVar = itemPotencia.TrnPctTotalmwVariable;
                                            PotenciaContrHPVar = itemPotencia.TrnPctHpmwFijaVariable;
                                            PotenciaContrHFPVar = itemPotencia.TrnPctHfpmwFijaVariable;

                                        }
                                        else
                                        {
                                            PotenciaContrTotalFija += itemPotencia.TrnPctTotalmwFija;
                                            PotenciaContrHPFija += itemPotencia.TrnPctHpmwFija;
                                            PotenciaContrHFPFija += itemPotencia.TrnPctHfpmwFija;

                                            PotenciaContrTotalVar += itemPotencia.TrnPctTotalmwVariable;
                                            PotenciaContrHPVar += itemPotencia.TrnPctHpmwFijaVariable;
                                            PotenciaContrHFPVar += itemPotencia.TrnPctHfpmwFijaVariable;

                                        }

                                    }
                                    else
                                    {

                                        #region Obtiene del archivo Excel
                                        CodigoRetiroGeneradoDTO potenciaVTAExcel = resultadoExcel.Data.Where(x => x.CoresoCodi == item.SoliCodiRetiCodi).FirstOrDefault();
                                        #endregion Obtiene del archivo Excel

                                        if (potenciaVTAExcel != null)
                                        {

                                            if (potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrTotalFija != null)
                                                PotenciaContrTotalFija = potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrTotalFija;

                                            if (potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrHPFija != null)
                                                PotenciaContrHPFija = potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrHPFija;

                                            if (potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrHFPFija != null)
                                                PotenciaContrHFPFija = potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrHFPFija;

                                            if (potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrTotalVar != null)
                                                PotenciaContrTotalVar = potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrTotalVar;

                                            if (potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrHPVar != null)
                                                PotenciaContrHPVar = potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrHPVar;

                                            if (potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrHFPVar != null)
                                                PotenciaContrHFPVar = potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrHFPVar;

                                            if (potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrObservacion != null)
                                                Observacion = potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrObservacion;

                                            if (potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrTotalFija != null
                                    || potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrHPFija != null
                                    || potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrHFPFija != null
                                    || potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrTotalVar != null
                                    || potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrHPVar != null
                                    || potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrHFPVar != null
                                    || potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrObservacion != null
                                    )
                                                PotenciaEsExcel = 1;
                                        }
                                    }
                                }

                                //---------------------------------------------------------------------------------------
                                //Si es VTA
                                //---------------------------------------------------------------------------------------
                                else if (
                                    detalle.PotenciaContratadaDTO.TipoAgrupacion == AgrupacionVTA
                                    && encontroPrimerRegistro == 0)
                                {
                                    if (resultadoExcel != null)
                                    {
                                        #region Obtiene del archivo Excel
                                        CodigoRetiroGeneradoDTO potenciaVTAExcel = resultadoExcel.Data.Where(x => x.CoresoCodi == item.SoliCodiRetiCodi).FirstOrDefault();
                                        #endregion Obtiene del archivo Excel

                                        if (potenciaVTAExcel != null)
                                        {

                                            if (potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrTotalFija != null)
                                                itemPotencia.TrnPctTotalmwFija = potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrTotalFija;

                                            if (potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrHPFija != null)
                                                itemPotencia.TrnPctHpmwFija = potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrHPFija;

                                            if (potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrHFPFija != null)
                                                itemPotencia.TrnPctHfpmwFija = potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrHFPFija;

                                            if (potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrTotalVar != null)
                                                itemPotencia.TrnPctTotalmwVariable = potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrTotalVar;

                                            if (potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrHPVar != null)
                                                itemPotencia.TrnPctHpmwFijaVariable = potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrHPVar;

                                            if (potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrHFPVar != null)
                                                itemPotencia.TrnPctHfpmwFijaVariable = potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrHFPVar;

                                            if (potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrHFPVar != null)
                                                itemPotencia.TrnPctComeObs = potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrObservacion;


                                            if (potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrTotalFija != null
                                    || potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrHPFija != null
                                    || potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrHFPFija != null
                                    || potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrTotalVar != null
                                    || potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrHPVar != null
                                    || potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrHFPVar != null
                                    || potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrObservacion != null

                                    )
                                                itemPotencia.TrnPctExcel = 1;
                                        }
                                    }


                                    PotenciaContrTotalFija = itemPotencia.TrnPctTotalmwFija;
                                    PotenciaContrHPFija = itemPotencia.TrnPctHpmwFija;
                                    PotenciaContrHFPFija = itemPotencia.TrnPctHfpmwFija;
                                    PotenciaContrTotalVar = itemPotencia.TrnPctTotalmwVariable;
                                    PotenciaContrHPVar = itemPotencia.TrnPctHpmwFijaVariable;
                                    PotenciaContrHFPVar = itemPotencia.TrnPctHfpmwFijaVariable;
                                    Observacion = itemPotencia.TrnPctComeObs;
                                    //EsExcel
                                    PotenciaEsExcel = itemPotencia.TrnPctExcel;
                                }

                                encontroPrimerRegistro++;

                            }

                            detalle.PotenciaContratadaDTO.PotenciaContrTotalFija = PotenciaContrTotalFija;
                            detalle.PotenciaContratadaDTO.PotenciaContrHPFija = PotenciaContrHPFija;
                            detalle.PotenciaContratadaDTO.PotenciaContrHFPFija = PotenciaContrHFPFija;
                            detalle.PotenciaContratadaDTO.PotenciaContrTotalVar = PotenciaContrTotalVar;
                            detalle.PotenciaContratadaDTO.PotenciaContrHPVar = PotenciaContrHPVar;
                            detalle.PotenciaContratadaDTO.PotenciaContrHFPVar = PotenciaContrHFPVar;
                            //EsExcel
                            detalle.PotenciaContratadaDTO.PotenciaEsExcel = PotenciaEsExcel;

                            detalle.PotenciaContratadaDTO.RowSpan = rowSpan == 0 ? null : rowSpan;
                            foreach (var itemFormato in codigoRetiros)
                            {
                                IEnumerable<CodigoRetiroGeneradoDTO> detalleEncontrado = null;

                                if (itemFormato.TipCasaAbrev == AgrupacionVTP
                                   && itemFormato.ListarBarraSuministro != null)
                                {
                                    detalleEncontrado = itemFormato.ListarBarraSuministro.Where(x =>
                                      omitirFilas.Contains(x.CoregeCodi)
                                    );
                                }
                                else if (itemFormato.TipCasaAbrev == AgrupacionVTA
                                     && itemFormato.ListarBarraSuministro != null)
                                {
                                    detalleEncontrado = itemFormato.ListarBarraSuministro.Where(x =>
                                     omitirFilas.Contains((int)(x.PotenciaContratadaDTO.CoresoCodigo ?? 0))
                                   );
                                }

                                if (detalleEncontrado != null)
                                {
                                    foreach (var itemDetalle in detalleEncontrado)
                                        itemDetalle.OmitirFila = 1;
                                    if (detalleEncontrado.Count() > 0
                                    && detalle.PotenciaContratadaDTO.TipoAgrupacion == AgrupacionVTA
                                    && itemFormato.CoresoCodiPotcn != item.CoresoCodiPotcn)
                                    {
                                        itemFormato.OmitirFilaVTA = 1;
                                        itemFormato.SoliCodiRetiCodiVTAParent = (int)item.CoresoCodiPotcn;
                                    }
                                }
                            }

                        }
                        else
                            obtenerPotencias = codigoRetiros.Where(x => x.SoliCodiRetiCodi == item.SoliCodiRetiCodi).ToList();

                        //---------------------------------------------------------------------------------------
                        //Si no Existen registros
                        //---------------------------------------------------------------------------------------
                        if (obtenerPotencias.Count > 0
                             && detalle.TipCasaAbrev != AgrupacionVTP)
                        {
                            List<int> omitirFilas = new List<int>();
                            int? rowSpan = obtenerPotencias.Count;
                            int encontroPrimerRegistro = 0;
                            CodigoRetiroDTO potenciaEncontrada = null;
                            foreach (var itemPtcn in obtenerPotencias)
                            {
                                //Potencias registradas a nivel de VTEA
                                //if (itemPtcn.SoliCodiRetiCodi != null
                                // && itemPtcn.CoregeCodiPotcn == null
                                if (itemPtcn.SoliCodiRetiCodi != 0
                                    && itemPtcn.CoregeCodiPotcn != 0
                                    && (itemPtcn.TipCasaAbrev == AgrupacionVTA)
                                    && encontroPrimerRegistro == 0)
                                {
                                    encontroPrimerRegistro++;

                                    if (resultadoExcel == null)
                                        potenciaEncontrada = itemPtcn;
                                    else
                                    {
                                        #region Obtiene del archivo Excel
                                        CodigoRetiroGeneradoDTO potenciaVTAExcel = resultadoExcel.Data.Where(x => x.CoresoCodi == itemPtcn.SoliCodiRetiCodi).FirstOrDefault();
                                        #endregion Obtiene del archivo Excel

                                        potenciaEncontrada = itemPtcn;

                                        //esExcel
                                        if (potenciaVTAExcel != null)
                                        {

                                            potenciaEncontrada.CoresoCodiPotcn = itemPtcn.CoregeCodiPotcn;
                                            potenciaEncontrada.CoregeCodiPotcn = itemPtcn.CoregeCodiPotcn;
                                            potenciaEncontrada.TipCasaAbrev = itemPtcn.TipCasaAbrev;
                                            if (potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrTotalFija != null)
                                                potenciaEncontrada.TrnPctTotalmwFija = potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrTotalFija;

                                            if (potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrHPFija != null)
                                                potenciaEncontrada.TrnPctHpmwFija = potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrHPFija;

                                            if (potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrHFPFija != null)
                                                potenciaEncontrada.TrnPctHfpmwFija = potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrHFPFija;

                                            if (potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrTotalVar != null)
                                                potenciaEncontrada.TrnPctTotalmwVariable = potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrTotalVar;
                                            if (potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrHPVar != null)
                                                potenciaEncontrada.TrnPctHpmwFijaVariable = potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrHPVar;

                                            if (potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrHFPVar != null)
                                                potenciaEncontrada.TrnPctHfpmwFijaVariable = potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrHFPVar;

                                            if (potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrObservacion != null)
                                                potenciaEncontrada.TrnPctComeObs = potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrObservacion;

                                            if (potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrTotalFija != null
                                         || potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrHPFija != null
                                         || potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrHFPFija != null
                                         || potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrTotalVar != null
                                         || potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrHPVar != null
                                         || potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrHFPVar != null
                                         || potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrObservacion != null
                                         )
                                                potenciaEncontrada.TrnPctExcel = 1;

                                        }
                                    }
                                    continue;
                                }
                                //Potencias registradas a nivel de VTP
                                //else if (itemPtcn.CoresoCodiPotcn != null
                                //  && itemPtcn.CoregeCodiPotcn != null
                                else if (itemPtcn.SoliCodiRetiCodi != 0

                                && string.IsNullOrEmpty(itemPtcn.TipCasaAbrev))
                                {
                                    if (item.TrnpcTipoPotencia == 2 && itemPtcn.CoregeCodiPotcn != 0)
                                    {
                                        rowSpan = null;
                                        potenciaEncontrada = null;
                                        break;
                                    }
                                    else
                                    {
                                        if (encontroPrimerRegistro == 0)
                                        {
                                            encontroPrimerRegistro++;
                                            itemPtcn.TipCasaAbrev = AgrupacionVTA;
                                            if (resultadoExcel == null)
                                                potenciaEncontrada = itemPtcn;
                                            else
                                            {
                                                #region Obtiene del archivo Excel
                                                CodigoRetiroGeneradoDTO potenciaVTAExcel = resultadoExcel.Data.Where(x => x.CoresoCodi == itemPtcn.SoliCodiRetiCodi).FirstOrDefault();
                                                #endregion Obtiene del archivo Excel

                                                potenciaEncontrada = itemPtcn;

                                                //esExcel
                                                if (potenciaVTAExcel != null)
                                                {
                                                    potenciaEncontrada.CoresoCodiPotcn = itemPtcn.CoregeCodiPotcn;
                                                    potenciaEncontrada.CoregeCodiPotcn = itemPtcn.CoregeCodiPotcn;
                                                    potenciaEncontrada.TipCasaAbrev = itemPtcn.TipCasaAbrev;
                                                    if (potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrTotalFija != null)
                                                        potenciaEncontrada.TrnPctTotalmwFija = potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrTotalFija;

                                                    if (potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrHPFija != null)
                                                        potenciaEncontrada.TrnPctHpmwFija = potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrHPFija;

                                                    if (potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrHFPFija != null)
                                                        potenciaEncontrada.TrnPctHfpmwFija = potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrHFPFija;

                                                    if (potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrTotalVar != null)
                                                        potenciaEncontrada.TrnPctTotalmwVariable = potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrTotalVar;
                                                    if (potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrHPVar != null)
                                                        potenciaEncontrada.TrnPctHpmwFijaVariable = potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrHPVar;

                                                    if (potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrHFPVar != null)
                                                        potenciaEncontrada.TrnPctHfpmwFijaVariable = potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrHFPVar;

                                                    if (potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrObservacion != null)
                                                        potenciaEncontrada.TrnPctComeObs = potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrObservacion;


                                                    if (potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrTotalFija != null
                                                 || potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrHPFija != null
                                                 || potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrHFPFija != null
                                                 || potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrTotalVar != null
                                                 || potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrHPVar != null
                                                 || potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrHFPVar != null
                                                 || potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrObservacion != null

                                                 )
                                                    {
                                                        potenciaEncontrada.TrnpcTipoPotencia = 1;
                                                        potenciaEncontrada.TrnPctExcel = 1;
                                                    }

                                                }
                                            }

                                        }
                                        else
                                        {
                                            omitirFilas.Add(itemPtcn.CoregeCodiPotcn ?? itemPtcn.CoregeCodi);
                                        }
                                    }
                                }
                                else
                                {
                                    omitirFilas.Add(itemPtcn.CoregeCodiPotcn ?? itemPtcn.CoregeCodi);
                                }
                            }

                            if (potenciaEncontrada != null && rowSpan != null)
                            {
                                SolicitudCodigoPotenciaContratadaDTO itemPotencia = new SolicitudCodigoPotenciaContratadaDTO();
                                itemPotencia.CoresoCodigo = potenciaEncontrada.CoresoCodiPotcn;
                                itemPotencia.CoregeCodigo = potenciaEncontrada.CoregeCodiPotcn;
                                itemPotencia.TipoAgrupacion = potenciaEncontrada.TipCasaAbrev;
                                itemPotencia.PotenciaContrTotalFija = potenciaEncontrada.TrnPctTotalmwFija;
                                itemPotencia.PotenciaContrHPFija = potenciaEncontrada.TrnPctHpmwFija;
                                itemPotencia.PotenciaContrHFPFija = potenciaEncontrada.TrnPctHfpmwFija;
                                itemPotencia.PotenciaContrTotalVar = potenciaEncontrada.TrnPctTotalmwVariable;
                                itemPotencia.PotenciaContrHPVar = potenciaEncontrada.TrnPctHpmwFijaVariable;
                                itemPotencia.PotenciaContrHFPVar = potenciaEncontrada.TrnPctHfpmwFijaVariable;
                                //esExcel
                                itemPotencia.PotenciaEsExcel = potenciaEncontrada.TrnPctExcel;
                                //obs

                                itemPotencia.PotenciaContrObservacion = potenciaEncontrada.TrnPctComeObs;
                                itemPotencia.RowSpan = rowSpan == 0 ? null : rowSpan;
                                item.OmitirFilaVTA = 0;
                                detalle.PotenciaContratadaDTO = itemPotencia;

                                foreach (var itemFormato in codigoRetiros)
                                {
                                    if (itemFormato.ListarBarraSuministro != null)
                                    {
                                        var detalleEncontrado = itemFormato.ListarBarraSuministro.Where(x => omitirFilas.Contains(x.CoregeCodi));
                                        foreach (var itemDetalle in detalleEncontrado)
                                        {
                                            itemDetalle.OmitirFila = 1;
                                        }
                                    }

                                }
                            }
                            else if (rowSpan == null)
                            {
                                var t = detalle.PotenciaContratadaDTO;
                                if (resultadoExcel != null)
                                {
                                    #region Obtiene del archivo Excel
                                    CodigoRetiroGeneradoDTO potenciaVTAExcel = resultadoExcel.Data.Where(x => x.CoregeCodi == detalle.CoregeCodi).FirstOrDefault();
                                    #endregion Obtiene del archivo Excel
                                    //EsExcel
                                    if (potenciaVTAExcel != null)
                                    {

                                        SolicitudCodigoPotenciaContratadaDTO itemPotencia = new SolicitudCodigoPotenciaContratadaDTO();
                                        itemPotencia.CoresoCodigo = item.SoliCodiRetiCodi;
                                        itemPotencia.CoregeCodigo = detalle.PotenciaContratadaDTO.CoregeCodigo;
                                        itemPotencia.TipoAgrupacion = null;

                                        if (potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrTotalFija != null)
                                            itemPotencia.PotenciaContrTotalFija = potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrTotalFija;

                                        if (potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrHPFija != null)
                                            itemPotencia.PotenciaContrHPFija = potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrHPFija;

                                        if (potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrHFPFija != null)
                                            itemPotencia.PotenciaContrHFPFija = potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrHFPFija;

                                        if (potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrTotalVar != null)
                                            itemPotencia.PotenciaContrTotalVar = potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrTotalVar;

                                        if (potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrHPVar != null)

                                            itemPotencia.PotenciaContrHPVar = potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrHPVar;

                                        if (potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrHFPVar != null)
                                            itemPotencia.PotenciaContrHFPVar = potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrHFPVar;

                                        if (potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrObservacion != null)
                                            itemPotencia.PotenciaContrObservacion = potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrObservacion;

                                        if (potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrTotalFija != null
                                            || potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrHPFija != null
                                            || potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrHFPFija != null
                                            || potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrTotalVar != null
                                            || potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrHPVar != null
                                            || potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrHFPVar != null
                                            || potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrObservacion != null
                                            )
                                            itemPotencia.PotenciaEsExcel = 1;

                                        itemPotencia.PotenciaContrObservacion = potenciaVTAExcel.PotenciaContratadaDTO.PotenciaContrObservacion;

                                        detalle.PotenciaContratadaDTO = itemPotencia;
                                    }
                                }
                            }
                        }
                    }
                }
            }


            #region "Agrupaciones"
            int IndiceGrupo = 1;
            foreach (var item in codigoRetirosAuxiliar)
            {
                int IndexDetalle = -1;
                foreach (var detalle in item.ListarBarraSuministro)
                {
                    IndexDetalle++;
                    if (item.OmitirFilaVTA.Equals(0))
                    {                        
                        item.EsAgrupado = 0;                        
                        item.IndiceGrupo = IndiceGrupo;
                        if (detalle.PotenciaContratadaDTO.TipoAgrupacion != null
                               && detalle.PotenciaContratadaDTO.TipoAgrupacion.Equals("AGRVTP"))
                        {
                            if (item.ListarBarraSuministro.Count > 1)
                            {
                                var existeVTP = item.ListarBarraSuministro.FirstOrDefault();
                                if (existeVTP != null)
                                {
                                    if (item.EsAgrupado.Equals(0))
                                        item.EsAgrupado = 1;
                                }
                            }
                        }
                    }
                    else
                    {
                        //No omite fila, pero buscamos si ya tiene idParent
                        var existeParent = codigoRetirosAuxiliar.Where(x => x.SoliCodiRetiCodi == item.SoliCodiRetiCodiVTAParent)
                                                                .FirstOrDefault();
                        if (existeParent != null)
                        {
                            if (existeParent.EsAgrupado.Equals(0)) //Si el existe y no esta considerado como agrupado lo cambiamos a agrupado
                                existeParent.EsAgrupado = 1;

                            item.EsAgrupado = existeParent.EsAgrupado;
                            item.IndiceGrupo = existeParent.IndiceGrupo;
                        }
                    }
                }

                IndiceGrupo++;
            }
            

            /*Agrupado por Estado : Pendiente | Activos - (Otros) */
            codigoRetirosAuxiliar = codigoRetirosAuxiliar.OrderByDescending(p => p.EstAbrev.Equals("PAP") || p.EstAbrev.Equals("PVT"))
                                                         .ThenByDescending(p => p.EstAbrev.Equals("PAP") || p.EstAbrev.Equals("PVT"))
                                                         .ThenByDescending(p => p.EstAbrev.Equals("ACT"))
                                                         .ThenByDescending(p => p.EstAbrev.Equals("BAJ"))
                                                         .ThenByDescending(p => p.EstAbrev.Equals("REC"))
                                                         .ThenByDescending(p => p.EstAbrev.Equals("SBJ"))
                                                         /*Desagrupados(0) y Agrupados(1)*/
                                                         .ThenBy(p => p.EmprNombre)
                                                         .ThenBy(p => p.CliNombre)
                                                         .ThenBy(p => p.FechaInicio)
                                                         .ThenByDescending(p => p.EsAgrupado.Equals(0))
                                                         .ThenByDescending(p => p.EsAgrupado.Equals(1))                                                                                                                  
                                                         .ToList();            

            #endregion "Agrupaciones"

            resultado.Data = codigoRetirosAuxiliar;


            return resultado;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="genemprcodi"></param>
        /// <param name="cliemprcodi"></param>
        /// <param name="barrcodi"></param>
        /// <returns></returns>
        public List<CodigoRetiroDTO> ListarCodigoVTEAByEmprBarr(int? genemprcodi, int? cliemprcodi, int? barrcodi)
        {
            try
            {
                return FactoryTransferencia.GetCodigoRetiroRepository().ListarCodigoVTEAByEmprBarr(genemprcodi, cliemprcodi, barrcodi);
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
        /// <param name="Solicodiretiobservacion">Solicitud de baja de código de retiro</param>        
        /// <param name="sCoReSotEstado">Estado en que se encuentra el Código de Retiro solicitado</param>        
        /// <returns>Lista de CodigoRetiroDTO</returns>
        public List<CodigoRetiroDTO> BuscarCodigoRetiroExtranet(string sEmprNomb, string sTipUsuNombre, string sTipConNombre, string sBarrBarraTransferencia, string sCliEmprNomb, DateTime? dCoReSoFechaInicio, DateTime? dCoReSoFechaFin, string Solicodiretiobservacion, string sCoReSotEstado)
        {
            try
            {
                return FactoryTransferencia.GetCodigoRetiroRepository().GetByCriteriaExtranet(sEmprNomb, sTipUsuNombre, sTipConNombre, sBarrBarraTransferencia, sCliEmprNomb, dCoReSoFechaInicio, dCoReSoFechaFin, Solicodiretiobservacion, sCoReSotEstado);
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
        /// <param name="Solicodiretiobservacion">Solicitud de baja de código de retiro</param>        
        /// <param name="sCoReSotEstado">Estado en que se encuentra el Código de Retiro solicitado</param>        
        /// <returns>Lista de CodigoRetiroDTO</returns>
        public List<CodigoRetiroDTO> BuscarCodigoRetiro(string sEmprNomb, string sTipUsuNombre, string sTipConNombre, string sBarrBarraTransferencia, string sCliEmprNomb, DateTime? dCoReSoFechaInicio, DateTime? dCoReSoFechaFin, string Solicodiretiobservacion, string sCoReSotEstado, string codretiro, int NroPagina, int PageSizeCodigoRetiro)
        {
            try
            {
                return FactoryTransferencia.GetCodigoRetiroRepository().GetByCriteria(sEmprNomb, sTipUsuNombre, sTipConNombre, sBarrBarraTransferencia, sCliEmprNomb, dCoReSoFechaInicio, dCoReSoFechaFin, Solicodiretiobservacion, sCoReSotEstado, codretiro, NroPagina, PageSizeCodigoRetiro);
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
        /// <param name="Solicodiretiobservacion">Solicitud de baja de código de retiro</param>        
        /// <param name="sCoReSotEstado">Estado en que se encuentra el Código de Retiro solicitado</param>        
        /// <returns>Numero de filas de la consulta</returns>
        public int ObtenerNroFilasCodigoRetiro(string sEmprNomb, string sTipUsuNombre, string sTipConNombre, string sBarrBarraTransferencia, string sCliEmprNomb, DateTime? dCoReSoFechaInicio, DateTime? dCoReSoFechaFin, string Solicodiretiobservacion, string sCoReSotEstado, string codretiro)
        {
            try
            {
                return FactoryTransferencia.GetCodigoRetiroRepository().ObtenerNroRegistros(sEmprNomb, sTipUsuNombre, sTipConNombre, sBarrBarraTransferencia, sCliEmprNomb, dCoReSoFechaInicio, dCoReSoFechaFin, Solicodiretiobservacion, sCoReSotEstado, codretiro);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                return 0;
            }
        }


        /// <summary>
        /// Permite obtener el numero de registros de la gestion de codigos VTEA/VTP para la paginacion
        /// </summary>
        /// <param name="genemprcodi"></param>
        /// <param name="clicodi"></param>
        /// <param name="tipocont"></param>
        /// <param name="tipousu"></param>
        /// <param name="barrcodi"></param>
        /// <param name="barrcodisum"></param>
        /// <param name="coresoestado"></param>
        /// <param name="coregecodvteavtp"></param>
        /// <param name="fechaIni"></param>
        /// <param name="fechaFin"></param>
        /// <returns></returns>
        public int ObtenerNroRegistrosGestionCodigosVTEAVTP(int periCodi, int? genemprcodi, int? clicodi, int? tipocont, int? tipousu, int? barrcodi, int? barrcodisum, string coresoestado, string coregecodvteavtp, DateTime? fechaIni, DateTime? fechaFin)
        {
            try
            {
                return FactoryTransferencia.GetCodigoRetiroRepository().ObtenerNroRegistrosGestionCodigosVTEAVTP(periCodi, genemprcodi, clicodi, tipocont, tipousu, barrcodi, barrcodisum, coresoestado, coregecodvteavtp, fechaIni, fechaFin);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                return 0;
            }
        }


        /// <summary>
        /// Permite obtener el CodigoRetiro mediante su sCoReSoCodigo en la vista VW_TRN_CODIGO_RETIRO
        /// </summary>
        /// <param name="sTRetCodigo"></param>
        /// <returns>CodigoRetiroDTO</returns>
        public CodigoRetiroDTO GetByCodigoRetiroCodigo(string sTRetCodigo)
        {
            try
            {
                return FactoryTransferencia.GetCodigoRetiroRepository().GetByCodigoRetiCodigo(sTRetCodigo);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                return null;
            }
        }

        /// <summary>
        /// Permite obtener el CodigoRetiro mediante su sCoReSoCodigo de la tabla TRN_CODIGO_RETIRO_SOLICITUD
        /// </summary>
        /// <param name="sCoReSoCodigo"></param>
        /// <returns>CodigoRetiroDTO</returns>
        public CodigoRetiroDTO GetCodigoRetiroByCodigo(string sCoReSoCodigo)
        {
            try
            {
                return FactoryTransferencia.GetCodigoRetiroRepository().GetCodigoRetiroByCodigo(sCoReSoCodigo);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                return null;
            }
        }

        /// <summary>
        /// Permite obtener un codigoretiro vigente en el periodo
        /// </summary>
        /// <param name="iPericodi">Periodo de valorización</param>
        /// <param name="sCodigo">Código de Retiro asignado</param>
        /// <returns>CodigoEntregaDTO</returns>
        public CodigoRetiroDTO CodigoRetiroVigenteByPeriodo(int iPericodi, string sCodigo)
        {
            try
            {
                return FactoryTransferencia.GetCodigoRetiroRepository().CodigoRetiroVigenteByPeriodo(iPericodi, sCodigo);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                return null;
            }
        }

        // ASSETEC 2019-11
        /// <summary>
        /// Permite listar todos las potencias contratadas de la tabla TRN_CODIGO_RETIRO_SOLICITUD 
        /// en base al Id de la empresa
        /// </summary>
        /// <param name="pericodi">Id del mes de valorizacion</param>       
        /// <param name="idEmpresa">Id de Empresa</param>       
        /// <returns>Lista de CodigoRetiroDTO</returns>
        public List<CodigoRetiroDTO> ImportarPotenciasContratadas(int pericodi, int idEmpresa)
        {
            try
            {
                return FactoryTransferencia.GetCodigoRetiroRepository().ImportarPotenciasContratadas(pericodi, idEmpresa);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                return null;
            }
        }

        /// <summary>
        /// Permite validar si un código VTEA esta dentro de algún envío
        /// </summary>
        /// <param name="sTRetCodigo">Código VTA</param>       
        /// <returns>Booleano True si el código existe en algún envío</returns>
        public bool ValidarExisteCodigoEnEnvios(string sTRetCodigo)
        {
            try
            {
                return FactoryTransferencia.GetCodigoRetiroRepository().ValidarExisteCodigoEnEnvios(sTRetCodigo);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                return false;
            }
        }

        #region PrimasRER.2023
        /// <summary>
        /// Permite listar todos las potencias contratadas de la tabla TRN_CODIGO_RETIRO_SOLICITUD 
        /// en base al Id de la empresa y fechas de inicio y fin
        /// </summary>
        /// <param name="genemprcodi">Id de la empresa</param>       
        /// <param name="coresofechainicio">Fecha de inicio</param>  
        /// <param name="coresofechafin">Fecha final</param> 
        /// <returns>Lista de CodigoRetiroDTO</returns>
        public List<CodigoRetiroDTO> ListCodRetirosByEmpresaYFecha(int genemprcodi, string coresofechainicio, string coresofechafin)
        {
            return FactoryTransferencia.GetCodigoRetiroRepository().ListCodRetirosByEmpresaYFecha(genemprcodi, coresofechainicio, coresofechafin);
        }

        #endregion
    }
}
