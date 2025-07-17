using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

using COES.Dominio.DTO.Sic;
using COES.Dominio.DTO.Transferencias;
using COES.Base.Core;
using COES.Servicios.Aplicacion.Factory;
using COES.Servicios.Aplicacion.Helper;
using log4net;
using System.Data.SqlClient;

using System.Data; //STS
using System.Data.Common; //STS

namespace COES.Servicios.Aplicacion.ResarcimientoNTCSE
{
    /// <summary>
    /// Clases con métodos del módulo ResarcimientoNTCSE
    /// </summary>
    public class ResarcimientoNTCSEAppServicio : AppServicioBase
    {
        /// <summary>
        /// Instancia para el manejo de logs
        /// </summary>
        private readonly ILog Logger = LogManager.GetLogger(typeof(ResarcimientoNTCSEAppServicio));

        /// <summary>
        /// Instancia para el manejo de Auditorias
        /// </summary>
        AuditoriaAppServicio Audit = new AuditoriaAppServicio();

        #region Métodos Tabla SI_ENVIO

        /// <summary>
        /// Método usado para grabar en la tabla SI_ENVIO el código de envío que se genera.
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="conn"></param>
        /// <param name="tran"></param>
        /// <returns></returns>
        public int SaveSiEnvio(MeEnvioDTO entity, IDbConnection conn, DbTransaction tran)
        {
            try
            {
                return FactorySic.GetMeEnvioRepository().Save(entity, conn, tran);

            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion


        #region Métodos Tabla EVE_EVENTO

        /// <summary>
        /// Permite listar los eventos por empresa generadora
        /// </summary>
        public List<EveEventoDTO> ListEventos(int emprgen)
        {
            return FactorySic.GetEveEventoRepository().ListEventos(emprgen);
        }

        #endregion

        #region Métodos Tabla EQ_AREA

        /// <summary>
        /// Permite listar las subestaciones 
        /// </summary>
        public List<EqAreaDTO> ListSubEstacion()
        {
            return FactorySic.GetEqAreaRepository().ListSubEstacion();
        }

        #endregion

        #region Métodos Tabla SI_EMPRESA

        /// <summary>
        /// Permite listar todas las empresas 
        /// </summary>
        public List<SiEmpresaDTO> ListSiEmpresasGeneral()
        {
            return FactorySic.GetSiEmpresaRepository().ListGeneral();
        }

        /// <summary>
        /// Permite listar las empresas generadoras
        /// </summary>
        public List<SiEmpresaDTO> ListEmpresasGeneradoras()
        {
            return FactorySic.GetSiEmpresaRepository().ListEmpresasGeneradoras();
        }

        /// <summary>
        /// Permite listar las empresas clientes
        /// </summary>
        public List<SiEmpresaDTO> ListEmpresasClientes()
        {
            return FactorySic.GetSiEmpresaRepository().ListEmpresasClientes();
        }

        #endregion

        #region Métodos Tabla RNT_CONFIGURACION

        /// <summary>
        /// Inserta un registro de la tabla RNT_CONFIGURACION
        /// </summary>
        public void SaveRntConfiguracion(RntConfiguracionDTO entity)
        {
            try
            {
                int id = FactoryTransferencia.GetRntConfiguracionRepository().Save(entity);
                /// <summary>
                /// Funcion de Auditoria
                /// </summary>
                Audit.GerarAuditoria(entity, entity.ConfUsuarioCreacion, id);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla RNT_CONFIGURACION
        /// </summary>
        public void UpdateRntConfiguracion(RntConfiguracionDTO entity)
        {
            try
            {
                FactoryTransferencia.GetRntConfiguracionRepository().Update(entity);
                /// <summary>
                /// Funcion de Auditoria
                /// </summary>
                Audit.GerarAuditoria(entity, entity.ConfUsuarioUpdate, entity.ConfigCodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla RNT_CONFIGURACION
        /// </summary>
        public void DeleteRntConfiguracion(RntConfiguracionDTO entity)
        {
            try
            {
                FactoryTransferencia.GetRntConfiguracionRepository().Delete(entity.ConfigCodi);
                /// <summary>
                /// Funcion de Auditoria
                /// </summary>
                Audit.GerarAuditoria(entity, entity.ConfUsuarioUpdate, entity.ConfigCodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla RNT_CONFIGURACION
        /// </summary>
        public RntConfiguracionDTO GetByIdRntConfiguracion(int configcodi)
        {
            return FactoryTransferencia.GetRntConfiguracionRepository().GetById(configcodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla RNT_CONFIGURACION
        /// </summary>
        public List<RntConfiguracionDTO> ListComboConfiguracion(string parametro)
        {
            return FactoryTransferencia.GetRntConfiguracionRepository().ListComboConfiguracion(parametro);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla RNT_CONFIGURACION
        /// </summary>
        public List<RntConfiguracionDTO> ListRntConfiguracions()
        {
            return FactoryTransferencia.GetRntConfiguracionRepository().List();
        }

        /// <summary>
        /// GetListParametroRep
        /// </summary>
        /// <param name="atributo"></param>
        /// <param name="parametro"></param>
        /// <returns></returns>
        public List<RntConfiguracionDTO> GetListParametroRep(string atributo, string parametro)
        {
            return FactoryTransferencia.GetRntConfiguracionRepository().GetListParametroRep(atributo, parametro);
        }

        /// <summary>
        /// GetComboParametro
        /// </summary>
        /// <param name="atributo"></param>
        /// <returns></returns>
        public List<RntConfiguracionDTO> GetComboParametro(string atributo)
        {
            return FactoryTransferencia.GetRntConfiguracionRepository().GetComboParametro(atributo);
        }

        #endregion

        #region Métodos Tabla RNT_EMPRESA_REGPTOENTREGA

        /// <summary>
        /// Permite listar todos los registros de la tabla RNT_EMPRESA_REGPTOENTREGA
        /// </summary>
        public List<RntEmpresaRegptoentregaDTO> ListRntEmpresaRegptoentregas(int key)
        {
            return FactoryTransferencia.GetRntEmpresaRegptoentregaRepository().List(key);
        }


        #endregion

        #region Métodos Tabla RNT_PERIODO

        /// <summary>
        /// Inserta un registro de la tabla RNT_PERIODO
        /// </summary>
        public void SaveRntPeriodo(RntPeriodoDTO entity)
        {
            try
            {
                int id = FactoryTransferencia.GetRntPeriodoRepository().Save(entity);
                /// <summary>
                /// Funcion de Auditoria
                /// </summary>
                Audit.GerarAuditoria(entity, entity.PerdUsuarioCreacion, id);
            }
            catch (Exception ex)
            {
                if (ex.Message.Substring(0, 9) == "ORA-00001")
                {
                    Logger.Error(ConstantesAppServicio.LogError, ex);
                    throw new Exception("INFORMACION YA ESTA REGISTRADA");
                }
                else
                {
                    Logger.Error(ConstantesAppServicio.LogError, ex);
                    throw new Exception(ex.Message, ex);
                }
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla RNT_PERIODO
        /// </summary>
        public void UpdateRntPeriodo(RntPeriodoDTO entity)
        {
            try
            {
                FactoryTransferencia.GetRntPeriodoRepository().Update(entity);
                /// <summary>
                /// Funcion de Auditoria
                /// </summary>
                Audit.GerarAuditoria(entity, entity.PerdUsuarioUpdate, entity.PeriodoCodi);
            }
            catch (Exception ex)
            {
                if (ex.Message.Substring(0, 9) == "ORA-00001")
                {
                    Logger.Error(ConstantesAppServicio.LogError, ex);
                    throw new Exception("INFORMACION YA ESTA REGISTRADA");
                }
                else
                {
                    Logger.Error(ConstantesAppServicio.LogError, ex);
                    throw new Exception(ex.Message, ex);
                }
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla RNT_PERIODO
        /// </summary>
        public void DeleteRntPeriodo(RntPeriodoDTO entity)
        {
            try
            {
                FactoryTransferencia.GetRntPeriodoRepository().Delete(entity);
                /// <summary>
                /// Funcion de Auditoria
                /// </summary>
                Audit.GerarAuditoria(entity, entity.PerdUsuarioUpdate, entity.PeriodoCodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla RNT_PERIODO
        /// </summary>
        public RntPeriodoDTO GetByIdRntPeriodo(int periodocodi)
        {
            return FactoryTransferencia.GetRntPeriodoRepository().GetById(periodocodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla RNT_PERIODO
        /// </summary>
        public List<RntPeriodoDTO> ListRntPeriodos()
        {
            return FactoryTransferencia.GetRntPeriodoRepository().List();
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla RNT_PERIODO
        /// </summary>
        public List<RntPeriodoDTO> ListComboRntPeriodos()
        {
            return FactoryTransferencia.GetRntPeriodoRepository().ListCombo();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla RntPeriodo
        /// </summary>
        public List<RntPeriodoDTO> GetByCriteriaRntPeriodos()
        {
            return FactoryTransferencia.GetRntPeriodoRepository().GetByCriteria();
        }

        #endregion

        #region Métodos Tabla RNT_REG_PUNTO_ENTREGA

        /// <summary>
        /// Inserta un registro de la tabla RNT_REG_PUNTO_ENTREGA
        /// </summary>
        public int SaveRntRegPuntoEntrega(RntRegPuntoEntregaDTO entity, int codEnvio, IDbConnection conn, DbTransaction tran)
        {
            try
            {
                int id = FactoryTransferencia.GetRntRegPuntoEntregaRepository().Save(entity, codEnvio, conn, tran, 0);
                /// <summary>
                /// Funcion de Auditoria
                /// </summary>
                return id;
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// SaveCargaRntRegPuntoEntrega
        /// </summary>
        /// <param name="username"></param>
        /// <param name="entities"></param>
        /// <param name="codEnvio"></param>
        /// <param name="conn"></param>
        /// <param name="tran"></param>
        public void SaveCargaRntRegPuntoEntrega(string username, List<RntRegPuntoEntregaDTO> entities, int codEnvio, IDbConnection conn, DbTransaction tran)
        {
            RntRegPuntoEntregaDTO entity;
            try
            {
                //Cambiar Estado a todos los PE antiguos a Estado = 2
                FactoryTransferencia.GetRntRegPuntoEntregaRepository().UpdatePE(entities[0].RpeEmpresaGeneradora, entities[0].PeriodoCodi, conn, tran);
                //Cambiar Estado a todos los RC antiguos a Estado = 2
                FactoryTransferencia.GetRntRegRechazoCargaRepository().UpdateRC(entities[0].RpeEmpresaGeneradora, entities[0].PeriodoCodi, conn, tran);

                int corrId = FactoryTransferencia.GetRntRegPuntoEntregaRepository().GetMaxId();
                int corrIdResp = FactoryTransferencia.GetRntEmpresaRegptoentregaRepository().GetMaxId();
                for (int i = 0; i < entities.Count; i++)
                {
                    entity = new RntRegPuntoEntregaDTO();
                    entity = entities[i];
                    entity.RpeUsuarioCreacion = username;
                    int ptoEntregaCodi = FactoryTransferencia.GetRntRegPuntoEntregaRepository().Save(entity, codEnvio, conn, tran, corrId);
                    corrId++;
                    //guardar Empresas Responsables
                    string[] empresasResponsables = entity.RpeEmpResponsables.Split(';');

                    for (int j = 0; j < empresasResponsables.Length; j++)
                    {
                        string[] datosEmpr = empresasResponsables[j].Split(',');
                        if (datosEmpr[0] != null && datosEmpr[0] != "0")
                            if (datosEmpr[0].Length > 0)
                            {
                                RntEmpresaRegptoentregaDTO dbto = new RntEmpresaRegptoentregaDTO();
                                dbto.EmpGenCodi = 0;
                                dbto.EmprCodi = Convert.ToInt32(datosEmpr[0]);
                                dbto.RegPorcentaje = Convert.ToDecimal(string.Format("{0:0.00}", Convert.ToDecimal(datosEmpr[1])));
                                dbto.RegPuntoEntCodi = ptoEntregaCodi;
                                dbto.PeeUsuarioCreacion = entity.RpeUsuarioCreacion;
                                dbto.PeeFechaCreacion = DateTime.Now;
                                FactoryTransferencia.GetRntEmpresaRegptoentregaRepository().Save(dbto, conn, tran, corrIdResp);
                                corrIdResp++;
                            }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// ListReporteCargaRntRegPuntoEntregas
        /// </summary>
        /// <param name="EmpresaGeneradora"></param>
        /// <param name="Periodo"></param>
        /// <returns></returns>
        public List<RntRegPuntoEntregaDTO> ListReporteCargaRntRegPuntoEntregas(int? EmpresaGeneradora, int Periodo, int PuntoEntrega, int codigoEnvio)
        {
            if (codigoEnvio != 0)
            {
                EmpresaGeneradora = 0;
                PuntoEntrega = 0;
                PuntoEntrega = 0;
            }

            return FactoryTransferencia.GetRntRegPuntoEntregaRepository().ListReporteCarga(EmpresaGeneradora, Periodo, PuntoEntrega, codigoEnvio);
        }

        /// <summary>
        /// ListReporteGrillaRntRegPuntoEntregas
        /// </summary>
        /// <param name="codEnvio"></param>
        /// <returns></returns>
        public string[][] ListReporteGrillaRntRegPuntoEntregas(int codEnvio)
        {
            int numColsPE = 27;
            int countGrupo = 0;
            List<RntRegPuntoEntregaDTO> dataGrilla = FactoryTransferencia.GetRntRegPuntoEntregaRepository().ListReporteGrilla(codEnvio);
            string[][] resultado = new string[dataGrilla.Count][];

            for (int i = 0; i < dataGrilla.Count; i++)
            {
                // Obtener empresas responsables x RpeCodi...
                List<RntEmpresaRegptoentregaDTO> empresasResponsables = this.ListRntEmpresaRegptoentregas(dataGrilla[i].RegPuntoEntCodi);

                resultado[i] = new string[numColsPE];
                for (int j = 0; j < numColsPE; j++)
                {
                    switch (j)
                    {
                        case 0://Grupo
                            countGrupo++;
                            resultado[i][j] = dataGrilla[i].RpeGrupoEnvio.ToString();
                            break;
                        case 1://Cliente
                            resultado[i][j] = dataGrilla[i].RpeClienteNombre;
                            break;
                        case 2:// Barra
                            resultado[i][j] = dataGrilla[i].BarrNombre;
                            break;
                        case 3: //Nivel Tension
                            resultado[i][j] = dataGrilla[i].RpeNivelTensionDesc;
                            break;
                        case 4: //Enegia Semestral
                            resultado[i][j] = Convert.ToString(dataGrilla[i].RpeEnergSem);
                            break;
                        case 5: //Incremento

                            if (dataGrilla[i].RpeIncremento == "NO") resultado[i][j] = "No";
                            else if (dataGrilla[i].RpeIncremento == "SI") resultado[i][j] = "Si";
                            else resultado[i][j] = dataGrilla[i].RpeIncremento;
                            break;
                        case 6: //Tipo
                            resultado[i][j] = dataGrilla[i].TipIntNombre;
                            break;
                        case 7: //Exonerado o Fuerza Mayor
                            if (dataGrilla[i].RpeTramFuerMayor == "NO") resultado[i][j] = "No";
                            else if (dataGrilla[i].RpeTramFuerMayor == "SI") resultado[i][j] = "Si";
                            else resultado[i][j] = dataGrilla[i].RpeTramFuerMayor;
                            break;
                        case 8: //Ni
                            resultado[i][j] = Convert.ToString(dataGrilla[i].RpeNi);
                            break;
                        case 9: //Ki
                            resultado[i][j] = Convert.ToString(dataGrilla[i].RpeKi);
                            break;
                        case 10: //Tiempo Ejecutado - Fecha Inicio
                            resultado[i][j] = dataGrilla[i].RpeFechaInicio.ToString("dd/MM/yyyy HH:mm:ss");
                            break;
                        case 11://Tiempo Ejecutado - Fecha Fin
                            resultado[i][j] = dataGrilla[i].RpeFechaFin.ToString("dd/MM/yyyy HH:mm:ss");
                            break;
                        case 12://Tiempo Programado - Fecha Inicio
                            if (dataGrilla[i].RpePrgFechaInicio != null)
                            {
                                DateTime RpePrgFechaInicio = (DateTime)dataGrilla[i].RpePrgFechaInicio;
                                resultado[i][j] = RpePrgFechaInicio.ToString("dd/MM/yyyy HH:mm:ss");
                            }
                            break;
                        case 13://Tiempo Programado - Fecha Fin
                            if (dataGrilla[i].RpePrgFechaFin != null)
                            {
                                DateTime RpePrgFechaFin = (DateTime)dataGrilla[i].RpePrgFechaFin;
                                resultado[i][j] = RpePrgFechaFin.ToString("dd/MM/yyyy HH:mm:ss");
                            }
                            break;
                        case 14://Empresa Responsable 1 - Nombre
                            if (empresasResponsables.Count > 0)
                            {

                                if (empresasResponsables[0] == null) resultado[i][j] = null;
                                else
                                    resultado[i][j] = empresasResponsables[0].EmpRpeNombre;
                            }
                            else
                                resultado[i][j] = null;
                            break;
                        case 15://Empresa Responsable 1 - Porcentaje
                            if (empresasResponsables.Count > 0)
                            {
                                if (empresasResponsables[0] == null) resultado[i][j] = null;
                                else
                                {
                                    if (empresasResponsables[0].RegPorcentaje > 0) resultado[i][j] = Convert.ToString(empresasResponsables[0].RegPorcentaje);
                                    else resultado[i][j] = null;
                                }
                            }
                            else
                                resultado[i][j] = null;
                            break;
                        case 16://Empresa Responsable 2 - Nombre
                            if (empresasResponsables.Count > 1)
                            {
                                if (empresasResponsables[1] == null)
                                {
                                    resultado[i][j] = null;
                                }
                                else
                                {
                                    if (empresasResponsables[1].EmpRpeNombre.Length > 0)
                                        resultado[i][j] = empresasResponsables[1].EmpRpeNombre;
                                    else
                                        resultado[i][j] = null;
                                }

                            }
                            else
                            {
                                resultado[i][j] = null;
                            }
                            break;
                        case 17://Empresa Responsable 2 - Porcentaje
                            if (empresasResponsables.Count > 1)
                            {
                                if (empresasResponsables[1] == null)
                                {
                                    resultado[i][j] = null;
                                }
                                else
                                {
                                    if (empresasResponsables[1].RegPorcentaje > 0) resultado[i][j] = Convert.ToString(empresasResponsables[1].RegPorcentaje);
                                    else
                                        resultado[i][j] = null;
                                }
                            }
                            else
                                resultado[i][j] = null;
                            break;
                        case 18://Empresa Responsable 3 - Nombre
                            if (empresasResponsables.Count > 2)
                            {
                                if (empresasResponsables[2] == null)
                                {
                                    resultado[i][j] = null;
                                }
                                else
                                {
                                    if (empresasResponsables[2].EmpRpeNombre.Length > 0)
                                        resultado[i][j] = empresasResponsables[2].EmpRpeNombre;
                                    else
                                        resultado[i][j] = null;
                                }

                            }
                            else
                            {
                                resultado[i][j] = null;
                            }

                            break;
                        case 19://Empresa Responsable 3 - Porcentaje
                            if (empresasResponsables.Count > 2)
                            {
                                if (empresasResponsables[2] == null)
                                {
                                    resultado[i][j] = null;
                                }
                                else
                                {
                                    if (empresasResponsables[2].RegPorcentaje > 0) resultado[i][j] = Convert.ToString(empresasResponsables[2].RegPorcentaje);
                                    else
                                        resultado[i][j] = null;
                                }
                            }
                            else
                                resultado[i][j] = null;
                            break;
                        case 20://Empresa Responsable 4 - Nombre
                            if (empresasResponsables.Count > 3)
                            {
                                if (empresasResponsables[3] == null)
                                {
                                    resultado[i][j] = null;
                                }
                                else
                                {
                                    if (empresasResponsables[3].EmpRpeNombre.Length > 0)
                                        resultado[i][j] = empresasResponsables[3].EmpRpeNombre;
                                    else
                                        resultado[i][j] = null;
                                }

                            }
                            else
                            {
                                resultado[i][j] = null;
                            }

                            break;
                        case 21://Empresa Responsable 4 - Porcentaje
                            if (empresasResponsables.Count > 3)
                            {
                                if (empresasResponsables[3] == null)
                                {
                                    resultado[i][j] = null;
                                }
                                else
                                {
                                    if (empresasResponsables[3].RegPorcentaje > 0) resultado[i][j] = Convert.ToString(empresasResponsables[3].RegPorcentaje);
                                    else
                                        resultado[i][j] = null;
                                }
                            }
                            else
                                resultado[i][j] = null;
                            break;
                        case 22://Empresa Responsable 5 - Nombre
                            if (empresasResponsables.Count > 4)
                            {
                                if (empresasResponsables[4] == null)
                                {
                                    resultado[i][j] = null;
                                }
                                else
                                {
                                    if (empresasResponsables[4].EmpRpeNombre.Length > 0)
                                        resultado[i][j] = empresasResponsables[4].EmpRpeNombre;
                                    else
                                        resultado[i][j] = null;
                                }

                            }
                            else
                            {
                                resultado[i][j] = null;
                            }

                            break;
                        case 23://Empresa Responsable 5 - Porcentaje
                            if (empresasResponsables.Count > 4)
                            {
                                if (empresasResponsables[4] == null)
                                {
                                    resultado[i][j] = null;
                                }
                                else
                                {
                                    if (empresasResponsables[4].RegPorcentaje > 0) resultado[i][j] = Convert.ToString(empresasResponsables[4].RegPorcentaje);
                                    else
                                        resultado[i][j] = null;
                                }
                            }
                            else
                                resultado[i][j] = null;
                            break;
                        case 24://Causa Resumida Interrupción
                            resultado[i][j] = dataGrilla[i].RpeCausaInterrupcion;
                            break;
                        case 25://Ei / E
                            resultado[i][j] = Convert.ToString((dataGrilla[i].RpeEiE / 100));
                            break;
                        case 26://Monto Resarcimiento
                            resultado[i][j] = Convert.ToString(dataGrilla[i].RpeCompensacion);
                            break;
                        default:
                            break;
                    }
                }
            }

            return resultado;
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla RntRegPuntoEntrega
        /// </summary>
        public List<RntRegPuntoEntregaDTO> GetByCriteriaRntRegPuntoEntregas(string user, int? EmpresaGeneradora, int Periodo, int Cliente, int PEntrega, int Ntension, DateTime desde)
        {
            return FactoryTransferencia.GetRntRegPuntoEntregaRepository().GetByCriteria(user, EmpresaGeneradora, Periodo, Cliente, PEntrega, Ntension, desde);
        }

        /// <summary>
        /// Obtener listado de Barras
        /// </summary>
        /// <returns></returns>
        public List<RntRegPuntoEntregaDTO> ListBarras()
        {
            return FactoryTransferencia.GetRntRegPuntoEntregaRepository().ListAllBarras();
        }

        #endregion

        #region Métodos Tabla RNT_REG_RECHAZO_CARGA

        /// <summary>
        /// SaveCargaRntRegRechazoCarga
        /// </summary>
        /// <param name="username"></param>
        /// <param name="entities"></param>
        /// <param name="codEnvio"></param>
        /// <param name="conn"></param>
        /// <param name="tran"></param>
        public void SaveCargaRntRegRechazoCarga(string username, List<RntRegRechazoCargaDTO> entities, int codEnvio, IDbConnection conn, DbTransaction tran)
        {
            RntRegRechazoCargaDTO entity;
            try
            {
                int corrId = FactoryTransferencia.GetRntRegRechazoCargaRepository().GetMaxId();
                for (int i = 0; i < entities.Count; i++)
                {
                    entity = new RntRegRechazoCargaDTO();
                    entity.RrcUsuarioCreacion = username;
                    entity = entities[i];
                    FactoryTransferencia.GetRntRegRechazoCargaRepository().Save(entity, codEnvio, conn, tran, corrId);
                    corrId++;
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla RNT_REG_RECHAZO_CARGA
        /// </summary>
        public int UpdateRntRegRechazoCarga(RntRegRechazoCargaDTO entity)
        {
            try
            {
                int id = FactoryTransferencia.GetRntRegRechazoCargaRepository().Update(entity);
                /// <summary>
                /// Funcion de Auditoria
                /// </summary>
                Audit.GerarAuditoria(entity, entity.RrcUsuarioUpdate, entity.RegRechazoCargaCodi);
                return id;
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza NRCF de la tabla RNT_REG_RECHAZO_CARGA
        /// </summary>
        public int UpdateNRCFRntRegRechazoCarga(RntRegRechazoCargaDTO entity)
        {
            try
            {
                int id = FactoryTransferencia.GetRntRegRechazoCargaRepository().UpdateNRCF(entity);
                return id;
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza EF de la tabla RNT_REG_RECHAZO_CARGA
        /// </summary>
        public int UpdateEFRntRegRechazoCarga(RntRegRechazoCargaDTO entity)
        {
            try
            {
                int id = FactoryTransferencia.GetRntRegRechazoCargaRepository().UpdateEF(entity);
                return id;
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla RNT_REG_RECHAZO_CARGA
        /// </summary>
        public void DeleteRntRegRechazoCarga(RntRegRechazoCargaDTO entity)
        {
            try
            {
                FactoryTransferencia.GetRntRegRechazoCargaRepository().Delete(entity.RegRechazoCargaCodi);
                /// <summary>
                /// Funcion de Auditoria
                /// </summary>
                Audit.GerarAuditoria(entity, entity.RrcUsuarioUpdate, entity.RegRechazoCargaCodi);
                Logger.Info(ConstantesAppServicio.LogError);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla RNT_REG_RECHAZO_CARGA
        /// </summary>
        public RntRegRechazoCargaDTO GetByIdRntRegRechazoCarga(int regrechazocargacodi)
        {
            return FactoryTransferencia.GetRntRegRechazoCargaRepository().GetById(regrechazocargacodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla RNT_REG_RECHAZO_CARGA
        /// </summary>
        public List<RntRegRechazoCargaDTO> ListRntRegRechazoCargas(int? EmpresaGeneradora, int Periodo, int Cliente, int PEntrega)
        {
            return FactoryTransferencia.GetRntRegRechazoCargaRepository().List(EmpresaGeneradora, Periodo, Cliente, PEntrega);
        }

        /// <summary>
        /// ListReporteGrillaRntRechazoCarga
        /// </summary>
        /// <param name="codEnvio"></param>
        /// <returns></returns>
        public string[][] ListReporteGrillaRntRechazoCarga(int codEnvio)
        {
            int numColsRC = 14;
            int countGrupo = 0;
            List<RntRegRechazoCargaDTO> dataGrilla = FactoryTransferencia.GetRntRegRechazoCargaRepository().ListReporteGrilla(codEnvio);
            string[][] resultado = new string[dataGrilla.Count][];

            for (int i = 0; i < dataGrilla.Count; i++)
            {
                resultado[i] = new string[numColsRC];
                for (int j = 0; j < numColsRC; j++)
                {
                    switch (j)
                    {
                        case 0://Grupo
                            countGrupo++;
                            resultado[i][j] = dataGrilla[i].RrcGrupoEnvio.ToString();
                            break;
                        case 1://Cliente
                            resultado[i][j] = dataGrilla[i].RrcClienteNombre;
                            break;
                        case 2:// Barra
                            resultado[i][j] = dataGrilla[i].BarrNombre;
                            break;
                        case 3: //Codigo Alimentador
                            resultado[i][j] = dataGrilla[i].RrcCodiAlimentador;
                            break;
                        case 4: //SED - Nombre
                            resultado[i][j] = dataGrilla[i].RrcSubestacionDstrb;
                            break;
                        case 5: //SED Kv
                            resultado[i][j] = Convert.ToString(dataGrilla[i].RrcNivelTensionSed);
                            break;
                        case 6: //ENS f
                            resultado[i][j] = Convert.ToString(dataGrilla[i].RrcEf);
                            break;
                        case 7: //Codigo Evento
                            resultado[i][j] = Convert.ToString(dataGrilla[i].RrcEvenCodiDesc);
                            break;
                        case 8://Interrupcion - Fecha Inicio
                            resultado[i][j] = dataGrilla[i].RrcFechaInicio.ToString("dd/MM/yyyy HH:mm:ss");
                            break;
                        case 9://Interrupcion - Fecha Fin
                            resultado[i][j] = dataGrilla[i].RrcFechaFin.ToString("dd/MM/yyyy HH:mm:ss");
                            break;
                        case 10://PK
                            resultado[i][j] = dataGrilla[i].RrcPk.ToString();
                            break;
                        case 11://Compensable
                            resultado[i][j] = dataGrilla[i].RrcCompensable; // 20 agosto 2016
                            break;
                        case 12://ENS fk
                            resultado[i][j] = dataGrilla[i].RrcEnsFk.ToString();
                            break;
                        case 13://Resarcimiento
                            resultado[i][j] = Convert.ToString(dataGrilla[i].RrcCompensacion);
                            break;
                        default:
                            break;
                    }
                }
            }

            return resultado;
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla RNT_REG_RECHAZO_CARGA
        /// </summary>
        public List<RntRegRechazoCargaDTO> ListPaginadoRntRegRechazoCargas(int? EmpresaGeneradora, int Periodo, int Cliente, int PEntrega, int NroPaginado, int PageSize)
        {
            return FactoryTransferencia.GetRntRegRechazoCargaRepository().ListPaginado(EmpresaGeneradora, Periodo, Cliente, PEntrega, NroPaginado, PageSize);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla RNT_REG_RECHAZO_CARGA
        /// </summary>
        public List<RntRegRechazoCargaDTO> ListReporteRntRegRechazoCargas(int? EmpresaGeneradora, int Periodo, int Cliente, int PEntrega)
        {
            return FactoryTransferencia.GetRntRegRechazoCargaRepository().ListReporte(EmpresaGeneradora, Periodo, Cliente, PEntrega);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla RNT_REG_RECHAZO_CARGA
        /// </summary>
        public List<RntRegRechazoCargaDTO> ListReportePaginadoRntRegRechazoCargas(int? EmpresaGeneradora, int Periodo, int Cliente, int PEntrega, int NroPaginado, int PageSize)
        {
            return FactoryTransferencia.GetRntRegRechazoCargaRepository().ListReportePaginado(EmpresaGeneradora, Periodo, Cliente, PEntrega, NroPaginado, PageSize);
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla RntRegRechazoCarga
        /// </summary>
        public List<RntRegRechazoCargaDTO> GetByCriteriaRntRegRechazoCargas(string user, int? EmpresaGeneradora, int Periodo, int Cliente, int PEntrega, int Ntension, DateTime desde)
        {
            return FactoryTransferencia.GetRntRegRechazoCargaRepository().GetByCriteria(user, EmpresaGeneradora, Periodo, Cliente, PEntrega, Ntension, desde);
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla RntRegRechazoCarga
        /// </summary>
        public List<RntRegRechazoCargaDTO> GetByCriteriaPaginadoRntRegRechazoCargas(string user, int? EmpresaGeneradora, int Periodo, int Cliente, int PEntrega, int Ntension, DateTime desde, int NroPaginado, int PageSize)
        {
            return FactoryTransferencia.GetRntRegRechazoCargaRepository().GetByCriteriaPaginado(user, EmpresaGeneradora, Periodo, Cliente, PEntrega, Ntension, desde, NroPaginado, PageSize);
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla RntRegRechazoCarga
        /// </summary>
        public List<RntRegRechazoCargaDTO> ListAuditoriaRechazoCarga(int Audittablacodi, int Tauditcodi)
        {
            return FactoryTransferencia.GetRntRegRechazoCargaRepository().ListAuditoriaRechazoCarga(Audittablacodi, Tauditcodi);
        }

        /// <summary>
        /// Permite listar todas las barras que fueron registrados en RntRegRechazoCarga
        /// </summary>
        public List<RntRegRechazoCargaDTO> ListAllRechazoCarga()
        {
            return FactoryTransferencia.GetRntRegRechazoCargaRepository().ListAllRechazoCarga();
        }


        /// <summary>
        /// Permite listar todas las barras que fueron registrados en RntRegRechazoCarga
        /// </summary>
        public List<RntRegRechazoCargaDTO> ListAllClienteRC()
        {
            return FactoryTransferencia.GetRntRegRechazoCargaRepository().ListAllClienteRC();
        }


        /// <summary>
        /// Permite listar todas las barras que fueron registrados en RntRegRechazoCarga
        /// </summary>
        public List<RntRegRechazoCargaDTO> ListChangeClienteRC(int idCliente)
        {
            return FactoryTransferencia.GetRntRegRechazoCargaRepository().ListChangeClienteRC(idCliente);
        }

        #endregion

        #region Métodos Tabla RNT_TIPO_INTERRUPCION

        /// <summary>
        /// Inserta un registro de la tabla RNT_TIPO_INTERRUPCION
        /// </summary>
        public void SaveRntTipoInterrupcion(RntTipoInterrupcionDTO entity)
        {
            try
            {
                FactoryTransferencia.GetRntTipoInterrupcionRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla RNT_TIPO_INTERRUPCION
        /// </summary>
        public void UpdateRntTipoInterrupcion(RntTipoInterrupcionDTO entity)
        {
            try
            {
                FactoryTransferencia.GetRntTipoInterrupcionRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla RNT_TIPO_INTERRUPCION
        /// </summary>
        public void DeleteRntTipoInterrupcion(int tipointcodi)
        {
            try
            {
                FactoryTransferencia.GetRntTipoInterrupcionRepository().Delete(tipointcodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla RNT_TIPO_INTERRUPCION
        /// </summary>
        public RntTipoInterrupcionDTO GetByIdRntTipoInterrupcion(int tipointcodi)
        {
            return FactoryTransferencia.GetRntTipoInterrupcionRepository().GetById(tipointcodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla RNT_TIPO_INTERRUPCION
        /// </summary>
        public List<RntTipoInterrupcionDTO> ListRntTipoInterrupcions()
        {
            return FactoryTransferencia.GetRntTipoInterrupcionRepository().List();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla RntTipoInterrupcion
        /// </summary>
        public List<RntTipoInterrupcionDTO> GetByCriteriaRntTipoInterrupcions()
        {
            return FactoryTransferencia.GetRntTipoInterrupcionRepository().GetByCriteria();
        }

        #endregion

        #region Métodos Reportes - Excel - Handsontable

        /// <summary>
        /// SaveReporte
        /// </summary>
        /// <param name="idModulo"></param>
        /// <param name="username"></param>
        /// <param name="periodo"></param>
        /// <param name="empGeneradora"></param>
        /// <param name="puntoEntrega"></param>
        /// <param name="arrFila"></param>
        /// <param name="arrFilaRC"></param>
        /// <param name="listErrores"></param>
        /// <param name="listErroresRC"></param>
        /// <returns></returns>
        public string SaveReporte(int idModulo, string username, int periodo, int empGeneradora, int puntoEntrega, string[][] arrFila, string[][] arrFilaRC, out  List<RntListErroresDTO> listErrores, out  List<RntListErroresDTO> listErroresRC)
        {
            IDbConnection conn = null;
            DbTransaction tran = null;

            string result = "Error";
            conn = FactorySic.GetMeEnvioRepository().BeginConnection();
            tran = FactorySic.GetMeEnvioRepository().StartTransaction(conn);

            MeEnvioDTO entity = new MeEnvioDTO();
            entity.Modcodi = idModulo;// Aqui obtenemos codigo de aplicacion - modulo
            entity.Userlogin = username;
            entity.Envioplazo = "S"; //Enviado
            entity.Estenvcodi = 1; //Enviado
            entity.Enviofecha = DateTime.Now; // Actualizacion Fecha Creacion de Envio

            try
            {
                int codigoEnvio = this.SaveSiEnvio(entity, conn, tran);
                if (codigoEnvio > 0)
                {
                    List<RntRegPuntoEntregaDTO> resultValidacionPE = this.ValidarGrillaPE(periodo, empGeneradora, puntoEntrega, arrFila, out  listErrores);
                    this.SaveCargaRntRegPuntoEntrega(username, resultValidacionPE, codigoEnvio, conn, tran);

                    List<RntRegRechazoCargaDTO> resultValidacionRC = this.ValidarGrillaRC(periodo, empGeneradora, puntoEntrega, arrFilaRC, out listErroresRC);
                    this.SaveCargaRntRegRechazoCarga(username, resultValidacionRC, codigoEnvio, conn, tran);

                    tran.Commit();
                    result = "Registro satisfactorio - Código Envío : " + codigoEnvio.ToString();
                }
                else
                {
                    listErrores = listErroresRC = null;
                    tran.Rollback();
                    result = "Error Generando Código Envío";
                }
            }
            catch (Exception e)
            {
                if (tran != null)
                    tran.Rollback();
                result = "Error grabando formatos Excel cargados";
                throw new ArgumentNullException("Error grabando formatos Excel cargados!");
            }
            finally
            {
                if (conn != null)
                    if (conn.State == ConnectionState.Open) conn.Close();
            }


            return result;
        }

        /// <summary>
        /// ValidarGrillaPE
        /// </summary>
        /// <param name="periodo"></param>
        /// <param name="emprcodi"></param>
        /// <param name="puntoEntrega"></param>
        /// <param name="arrFila"></param>
        /// <param name="listErrores"></param>
        /// <returns></returns>
        public List<RntRegPuntoEntregaDTO> ValidarGrillaPE(int periodo, int emprcodi, int puntoEntrega, string[][] arrFila, out List<RntListErroresDTO> listErrores)
        {
            int numColsPE = 27;
            DateTime? tefi = null;
            DateTime? teff = null;
            DateTime? tpfi = null;
            DateTime? tpff = null;
            decimal porcEmpr1 = 0;
            decimal porcEmpr2 = 0;
            decimal porcEmpr3 = 0;
            decimal porcEmpr4 = 0;
            decimal porcEmpr5 = 0;
            RntListErroresDTO errorCelda = new RntListErroresDTO();
            listErrores = new List<RntListErroresDTO>(arrFila.Length * numColsPE);

            List<RntRegPuntoEntregaDTO> result = new List<RntRegPuntoEntregaDTO>(arrFila.Length);
            RntRegPuntoEntregaDTO resultDetalle = null;

            int countGrupo = 0, posicion = -1;
            bool mismoGrupo = false;
            string[][] arrGrupo = new string[arrFila.Length][];
            string[][] arrCalculoGrupo = new string[arrFila.Length][];
            string valorCelda = null;

            //Inicializar arreglo de bases de grupo
            for (int k = 0; k < arrFila.Length; k++)
            {
                arrGrupo[k] = new string[numColsPE];
                arrCalculoGrupo[k] = new string[7];
                for (int l = 0; l < numColsPE; l++)
                {
                    arrGrupo[k][l] = "-1";
                }
            }


            double energiaSuministrada;
            double energia = 0;
            string incremento = "";
            double resarcimiento = 0;
            double factor = 1;
            string tension;
            int toln = 0, told = 0;


            //Calcular HoraSemestre
            RntPeriodoDTO periodoItem = this.GetByIdRntPeriodo(periodo);
            string fechaInicio = "";
            string fechaFin = "";

            if (periodoItem.PerdSemestre.ToString() == "SI")
            {
                fechaInicio = "01/01/" + periodoItem.PerdAnio.ToString();
                fechaFin = "01/07/" + periodoItem.PerdAnio.ToString();
            }
            else
            {
                fechaInicio = "01/07/" + periodoItem.PerdAnio.ToString();
                fechaFin = "01/01/" + (Convert.ToInt32(periodoItem.PerdAnio.ToString()) + 1).ToString();
            }

            tefi = DateTime.ParseExact(fechaInicio, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            teff = DateTime.ParseExact(fechaFin, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            int horasemestre = ((DateTime)teff - (DateTime)tefi).Days;

            double n = 0;
            double ni = 0;//20-08-2016
            double ki = 0;//20-08-2016
            double minutos = 0;//20-08-2016
            double d = 0;
            double dreal = 0;
            double valorEiE = 0;
            DateTime tini;
            DateTime tfin;
            double diffDays = 0;
            double diffFactor;

            if (arrFila.Length > 0)
            {
                //Recolectar Datos para Calculo de resarcimiento x Grupo
                try
                {


                    for (int i = 0; i < arrFila.Length; i++)
                    {
                        valorCelda = arrFila[i][0];
                        if (valorCelda != null)
                            if (analizarNumerico(valorCelda.ToString()) != false)
                                if ((mismoGrupo = EvalMismoGrupo(arrGrupo, countGrupo, valorCelda, out posicion)) == false)
                                {
                                    for (int k = 0; k < numColsPE; k++)
                                        arrGrupo[countGrupo][k] = arrFila[i][k];

                                    //Calcular valores factor y resarcimiento del Grupo anterior
                                    if (countGrupo > 0) //Segundo Registro en adelante empieza a cerrar calculos x grupos
                                    {

                                        if ((n < toln) && (d < told))
                                            factor = 0;
                                        else
                                        {
                                            factor = 1;
                                            if (n > toln)
                                                factor = factor + (n - toln) / toln;
                                            if (d > told)
                                                factor = factor + (d - told) / told;

                                        }
                                        energiaSuministrada = energia * d / ((horasemestre) * 24 - dreal);
                                        resarcimiento = 0.35 * energiaSuministrada * factor;

                                        arrCalculoGrupo[countGrupo - 1][0] = n.ToString();
                                        arrCalculoGrupo[countGrupo - 1][1] = d.ToString();
                                        arrCalculoGrupo[countGrupo - 1][2] = dreal.ToString();
                                        arrCalculoGrupo[countGrupo - 1][3] = factor.ToString();
                                        arrCalculoGrupo[countGrupo - 1][4] = resarcimiento.ToString();
                                        arrCalculoGrupo[countGrupo - 1][5] = toln.ToString();
                                        arrCalculoGrupo[countGrupo - 1][6] = told.ToString();

                                    }

                                    //Primer Registro del archivo o primer registro  del Grupo
                                    // Inicializar 
                                    //inicializar "n"
                                    arrCalculoGrupo[countGrupo][0] = "0";
                                    //inicializar "d"
                                    arrCalculoGrupo[countGrupo][1] = "0";
                                    //inicializar "dreal"
                                    arrCalculoGrupo[countGrupo][2] = "0";
                                    //inicializar "factor"
                                    arrCalculoGrupo[countGrupo][3] = "0";
                                    //inicializar "resarcimiento"
                                    arrCalculoGrupo[countGrupo][4] = "0";
                                    //inicializar "toln"
                                    arrCalculoGrupo[countGrupo][5] = "0";
                                    //inicializar "told"
                                    arrCalculoGrupo[countGrupo][6] = "0";

                                    //Calculos iniciales
                                    energia = Convert.ToDouble(arrFila[i][4]);
                                    tension = arrFila[i][3];
                                    incremento = arrFila[i][5];
                                    if (incremento.ToUpper() == "NO")
                                    {
                                        if (tension == "MAT" || tension == "AT")
                                        {
                                            toln = 2;
                                            told = 4;
                                        }
                                        else if (tension == "MT")
                                        {
                                            toln = 4;
                                            told = 7;
                                        }
                                        else if (tension == "BT")
                                        {
                                            toln = 6;
                                            told = 10;
                                        }
                                    }
                                    else
                                    {
                                        if (tension == "MAT" || tension == "AT")
                                        {
                                            toln = 3;
                                            told = 6;
                                        }
                                        else if (tension == "MT")
                                        {
                                            toln = 6;
                                            told = 10;
                                        }
                                        else if (tension == "BT")
                                        {
                                            toln = 8;
                                            told = 14;
                                        }
                                    }

                                    //n
                                    n = Convert.ToDouble(arrFila[i][8]); //comentado 20 agosto 2016

                                    ni = Convert.ToDouble(arrFila[i][8]);// 20-08-2016
                                    ki = Convert.ToDouble(arrFila[i][9]);// 20-08-2016

                                    //d
                                    tini = DateTime.ParseExact(arrFila[i][10].ToUpper().Replace(".", ""), "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
                                    tfin = DateTime.ParseExact(arrFila[i][11].ToUpper().Replace(".", ""), "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
                                    diffDays = ((DateTime)tfin - (DateTime)tini).TotalDays;
                                    d = Convert.ToDouble(arrFila[i][9]) * diffDays * 24;

                                    //d real
                                    dreal = Convert.ToDouble(arrCalculoGrupo[countGrupo][2]) + (diffDays * 24);

                                    countGrupo++;
                                }
                                else
                                { //mismo grupo
                                    //Calcular "n"
                                    //n = n + Convert.ToDouble(arrFila[i][8]); comentado 20agosto2016

                                    ni = Convert.ToDouble(arrFila[i][8]);// 20-08-2016
                                    ki = Convert.ToDouble(arrFila[i][9]);// 20-08-2016

                                    //Calcular "d"
                                    tini = DateTime.ParseExact(arrFila[i][10].ToUpper().Replace(".", ""), "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
                                    tfin = DateTime.ParseExact(arrFila[i][11].ToUpper().Replace(".", ""), "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
                                    diffDays = ((DateTime)tfin - (DateTime)tini).TotalDays;

                                    minutos = Math.Round(diffDays * 24 * 60, 3);
                                    if (minutos < 3)
                                    {
                                        diffDays = 0;
                                        ni = 0;
                                        ki = 0;
                                    }

                                    n = n + ni;

                                    diffFactor = Convert.ToDouble(arrFila[i][9]) * diffDays * 24;
                                    d = d + diffFactor;

                                    //Calcular "dreal"
                                    dreal = dreal + (diffDays * 24);

                                    // SI es Ultimo del Grupo y del Archivo
                                    if (arrFila.Length - 1 == i)
                                    {
                                        if ((n < toln) && (d < told))
                                            factor = 0;
                                        else
                                        {
                                            factor = 1;
                                            if (n > toln)
                                                factor = factor + (n - toln) / toln;
                                            if (d > told)
                                                factor = factor + (d - told) / told;

                                        }
                                        energiaSuministrada = energia * d / ((horasemestre) * 24 - dreal);
                                        resarcimiento = 0.35 * energiaSuministrada * factor;

                                        arrCalculoGrupo[countGrupo - 1][0] = n.ToString();
                                        arrCalculoGrupo[countGrupo - 1][1] = d.ToString();
                                        arrCalculoGrupo[countGrupo - 1][2] = dreal.ToString();
                                        arrCalculoGrupo[countGrupo - 1][3] = factor.ToString();
                                        arrCalculoGrupo[countGrupo - 1][4] = resarcimiento.ToString();
                                        arrCalculoGrupo[countGrupo - 1][5] = toln.ToString();
                                        arrCalculoGrupo[countGrupo - 1][6] = told.ToString();

                                    }

                                }

                    }

                }
                catch (Exception ex)
                {

                    Logger.Error("Error calculando datos de resarcimiento: " + ex);
                }




                //Termina Recolectar Datos para Calculo Resarcimiento

                //Nuevamente inicializa Contador de Grupo
                countGrupo = 0;
                //Inicializar arreglo de bases de grupo
                for (int k = 0; k < arrFila.Length; k++)
                {
                    arrGrupo[k] = new string[numColsPE];
                    for (int l = 0; l < numColsPE; l++)
                    {
                        arrGrupo[k][l] = "-1";
                    }
                }


                DateTime RpePrgFechaFin = DateTime.Now;
                DateTime RpePrgFechaInicio = DateTime.Now;

                for (int i = 0; i < arrFila.Length; i++)
                {
                    porcEmpr1 = porcEmpr2 = porcEmpr3 = 0;
                    string exo_fm = "";
                    bool porcentajeCompleto = false;

                    resultDetalle = new RntRegPuntoEntregaDTO();
                    resultDetalle.arrEsValido = new bool[numColsPE];

                    for (int j = 0; j < numColsPE; j++)
                    {
                        valorCelda = arrFila[i][j];
                        resultDetalle.arrEsValido[j] = true;
                        //Asignar Periodo y Empresa Generadora
                        resultDetalle.PeriodoCodi = periodo;
                        resultDetalle.RpeEmpresaGeneradora = emprcodi;
                        resultDetalle.AreaCodi = puntoEntrega;

                        switch (j)
                        {
                            case 0:
                                if (valorCelda == null)
                                {
                                    resultDetalle.arrEsValido[j] = false;
                                    listErrores.Add(PushError("PE", i, j, valorCelda, "Dato Nulo"));
                                }
                                else
                                {
                                    resultDetalle.arrEsValido[j] = analizarNumerico(valorCelda.ToString());
                                    if (resultDetalle.arrEsValido[j] == false)
                                        listErrores.Add(PushError("PE", i, j, valorCelda, "Dato No Numerico"));
                                    else
                                    {
                                        resultDetalle.RpeGrupoEnvio = Convert.ToInt32(valorCelda);
                                        if ((mismoGrupo = EvalMismoGrupo(arrGrupo, countGrupo, valorCelda, out posicion)) == false)
                                        {
                                            n = Convert.ToDouble(arrCalculoGrupo[countGrupo][0]);
                                            d = Convert.ToDouble(arrCalculoGrupo[countGrupo][1]);
                                            dreal = Convert.ToDouble(arrCalculoGrupo[countGrupo][2]);
                                            factor = Convert.ToDouble(arrCalculoGrupo[countGrupo][3]);
                                            resarcimiento = Convert.ToDouble(arrCalculoGrupo[countGrupo][4]);
                                            toln = Convert.ToInt32(arrCalculoGrupo[countGrupo][5]);
                                            told = Convert.ToInt32(arrCalculoGrupo[countGrupo][6]);


                                            for (int k = 0; k < numColsPE; k++)
                                                arrGrupo[countGrupo][k] = arrFila[i][k];

                                            countGrupo++;
                                        }
                                    }
                                }
                                break;
                            case 1: //Cliente
                                if (mismoGrupo == true && valorCelda == null) valorCelda = arrGrupo[posicion][j];
                                if (mismoGrupo == false && valorCelda == null) { resultDetalle.arrEsValido[j] = false; listErrores.Add(PushError("PE", i, j, valorCelda, "Dato Nulo")); }
                                else
                                {
                                    resultDetalle.arrEsValido[j] = analizarNumerico(valorCelda.ToString());
                                    if (resultDetalle.arrEsValido[j] == false) listErrores.Add(PushError("PE", i, j, valorCelda, "Id Cliente Invalido"));
                                    else resultDetalle.RpeCliente = Convert.ToInt32(valorCelda);
                                }
                                break;
                            case 2: // Barra
                                if (mismoGrupo == true && valorCelda == null) valorCelda = arrGrupo[posicion][j];
                                if (mismoGrupo == false && valorCelda == null) { resultDetalle.arrEsValido[j] = false; listErrores.Add(PushError("PE", i, j, valorCelda, "Dato Nulo")); }
                                else
                                {
                                    resultDetalle.arrEsValido[j] = analizarNumerico(valorCelda.ToString());
                                    if (resultDetalle.arrEsValido[j] == false) listErrores.Add(PushError("PE", i, j, valorCelda, "Id Barra Invalido"));
                                    else resultDetalle.Barrcodi = Convert.ToInt32(valorCelda);

                                }
                                break;
                            case 3: //Nivel Tension
                                if (mismoGrupo == true && valorCelda == null) valorCelda = arrGrupo[posicion][j];
                                if (mismoGrupo == false && valorCelda == null) { resultDetalle.arrEsValido[j] = false; listErrores.Add(PushError("PE", i, j, valorCelda, "Dato Nulo")); }
                                else
                                {
                                    if (FactoryTransferencia.GetRntConfiguracionRepository().ListParametroRep("NIVELTENSION", valorCelda.ToString()).ConfValor == null)
                                    {
                                        resultDetalle.arrEsValido[j] = false;
                                        listErrores.Add(PushError("PE", i, j, valorCelda, "Nivel de Tension Invalido"));
                                    }
                                    else
                                        resultDetalle.RpeNivelTensionDesc = valorCelda;

                                }

                                break;
                            case 4: //Enegia Semestral
                                if (mismoGrupo == true && valorCelda == null) valorCelda = arrGrupo[posicion][j];
                                if (mismoGrupo == false && valorCelda == null) { resultDetalle.arrEsValido[j] = false; listErrores.Add(PushError("PE", i, j, valorCelda, "Dato Nulo")); }
                                else
                                {
                                    resultDetalle.arrEsValido[j] = analizarNumerico(valorCelda.ToString());
                                    if (resultDetalle.arrEsValido[j] == false) listErrores.Add(PushError("PE", i, j, valorCelda, "Dato No Numerico"));
                                    else resultDetalle.RpeEnergSem = Convert.ToDouble(valorCelda);
                                }
                                break;
                            case 5: //Incremento
                                if (mismoGrupo == true && valorCelda == null) valorCelda = arrGrupo[posicion][j];
                                if (mismoGrupo == false && valorCelda == null) { resultDetalle.arrEsValido[j] = false; listErrores.Add(PushError("PE", i, j, valorCelda, "Dato Nulo")); }
                                else if ((exo_fm = FactoryTransferencia.GetRntConfiguracionRepository().ListParametroRep("INCREMENTO", valorCelda.ToString()).ConfValor) == null)
                                {
                                    resultDetalle.arrEsValido[j] = false;
                                    listErrores.Add(PushError("PE", i, j, valorCelda, "Dato No Valido"));
                                }
                                else
                                    resultDetalle.RpeIncremento = valorCelda.ToUpper();
                                break;
                            case 6: //Tipo
                                if (valorCelda == null) { resultDetalle.arrEsValido[j] = false; listErrores.Add(PushError("PE", i, j, valorCelda, "Dato Nulo")); }
                                else
                                {
                                    int encontro = 0;
                                    List<RntTipoInterrupcionDTO> lTipoInterrupciones = ListRntTipoInterrupcions();
                                    for (int h = 0; h < lTipoInterrupciones.Count; h++)
                                        if (lTipoInterrupciones[h].TipoIntNombre.Trim().ToUpper() == valorCelda.Trim().ToUpper())
                                        {
                                            encontro = 1;
                                            resultDetalle.RpeTipoIntDesc = valorCelda;
                                            resultDetalle.TipoIntCodi = lTipoInterrupciones[h].TipoIntCodi;
                                        }

                                    if (encontro == 0)
                                    {
                                        resultDetalle.arrEsValido[j] = false;
                                        listErrores.Add(PushError("PE", i, j, valorCelda, "Dato No Valido"));
                                    }
                                }


                                break;
                            case 7: //Exonerado o Fuerza Mayor
                                if (valorCelda == null) { resultDetalle.arrEsValido[j] = false; listErrores.Add(PushError("PE", i, j, valorCelda, "Dato Nulo")); }
                                else if ((exo_fm = FactoryTransferencia.GetRntConfiguracionRepository().ListParametroRep("EXONERADO_FM", valorCelda.ToString()).ConfValor) == null)
                                {
                                    resultDetalle.arrEsValido[j] = false;
                                    listErrores.Add(PushError("PE", i, j, valorCelda, "Dato No Valido"));
                                }
                                else
                                    resultDetalle.RpeTramFuerMayor = valorCelda.ToUpper();
                                break;
                            case 8: //Ni
                                if (valorCelda == null) { resultDetalle.arrEsValido[j] = false; listErrores.Add(PushError("PE", i, j, valorCelda, "Dato Nulo")); }
                                else
                                {
                                    if (exo_fm == "SI")
                                    {
                                        try
                                        {
                                            if (Convert.ToInt32(valorCelda) != 0) { resultDetalle.arrEsValido[j] = false; listErrores.Add(PushError("PE", i, j, valorCelda, "Dato No Es Cero")); }
                                            else resultDetalle.RpeNi = Convert.ToDouble(valorCelda);
                                        }
                                        catch (Exception e)
                                        {
                                            resultDetalle.arrEsValido[j] = false;
                                            listErrores.Add(PushError("PE", i, j, valorCelda, "Dato No Numerico"));
                                        }

                                    }
                                    else if (FactoryTransferencia.GetRntConfiguracionRepository().ListParametroRep("NIPE", valorCelda.ToString()).ConfValor == null)
                                    {
                                        resultDetalle.arrEsValido[j] = false;
                                        listErrores.Add(PushError("PE", i, j, valorCelda, "Dato Invalido"));
                                    }
                                    else
                                    {
                                        try
                                        {
                                            resultDetalle.RpeNi = Convert.ToDouble(valorCelda);
                                        }
                                        catch (Exception e)
                                        {
                                            resultDetalle.arrEsValido[j] = false;
                                            listErrores.Add(PushError("PE", i, j, valorCelda, "Dato No Numerico"));
                                        }
                                    }

                                }
                                break;
                            case 9: //Ki
                                if (valorCelda == null) { resultDetalle.arrEsValido[j] = false; listErrores.Add(PushError("PE", i, j, valorCelda, "Dato Nulo")); }
                                else
                                {
                                    if (exo_fm == "SI")
                                    {
                                        try
                                        {
                                            if (Convert.ToInt32(valorCelda) != 0) { resultDetalle.arrEsValido[j] = false; listErrores.Add(PushError("PE", i, j, valorCelda, "Dato No Es Cero")); }
                                            else
                                                resultDetalle.RpeKi = Convert.ToDouble(valorCelda);
                                        }
                                        catch (Exception e)
                                        {
                                            resultDetalle.arrEsValido[j] = false;
                                            listErrores.Add(PushError("PE", i, j, valorCelda, "Dato No Numerico"));
                                        }

                                    }
                                    else if (FactoryTransferencia.GetRntConfiguracionRepository().ListParametroRep("KIPE", valorCelda.ToString()).ConfValor == null)
                                    {
                                        resultDetalle.arrEsValido[j] = false;
                                        listErrores.Add(PushError("PE", i, j, valorCelda, "Dato Invalido"));
                                    }
                                    else
                                    {
                                        try
                                        {
                                            resultDetalle.RpeKi = Convert.ToDouble(valorCelda);
                                        }
                                        catch (Exception e)
                                        {
                                            resultDetalle.arrEsValido[j] = false;
                                            listErrores.Add(PushError("PE", i, j, valorCelda, "Dato No Numerico"));
                                        }
                                    }

                                }
                                break;
                            case 10: //Tiempo Ejecutado - Fecha Inicio
                                if (valorCelda == null) { resultDetalle.arrEsValido[j] = false; listErrores.Add(PushError("PE", i, j, valorCelda, "Dato Nulo")); }
                                else
                                {
                                    try
                                    {
                                        string fecha = valorCelda.ToUpper().Replace(".", "");
                                        tefi = DateTime.ParseExact(fecha, "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
                                        resultDetalle.RpeFechaInicio = (DateTime)tefi;
                                    }
                                    catch (Exception e)
                                    {
                                        resultDetalle.arrEsValido[j] = false;
                                        listErrores.Add(PushError("PE", i, j, valorCelda, "Formato Fecha Invalido"));
                                    }
                                }
                                break;
                            case 11://Tiempo Ejecutado - Fecha Fin
                                if (valorCelda == null) { resultDetalle.arrEsValido[j] = false; listErrores.Add(PushError("PE", i, j, valorCelda, "Dato Nulo")); }
                                else
                                {
                                    try
                                    {
                                        teff = DateTime.ParseExact(valorCelda.ToUpper().Replace(".", ""), "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
                                        diffDays = ((DateTime)teff - (DateTime)tefi).TotalDays;

                                        if (teff < tefi) { resultDetalle.arrEsValido[j] = false; resultDetalle.arrEsValido[j - 1] = false; listErrores.Add(PushError("PE", i, j, valorCelda, "Fecha Inicio mayor que Fecha Fin")); }
                                        else
                                            resultDetalle.RpeFechaFin = (DateTime)teff;
                                    }
                                    catch (Exception e)
                                    {
                                        listErrores.Add(PushError("PE", i, j, valorCelda, "Formato Fecha Invalido"));
                                        resultDetalle.arrEsValido[j] = false;
                                    }
                                }

                                break;
                            case 12://Tiempo Programado - Fecha Inicio
                                if (valorCelda == null && resultDetalle.TipoIntCodi != 3) { resultDetalle.arrEsValido[j] = false; listErrores.Add(PushError("PE", i, j, valorCelda, "Dato Nulo")); }
                                else if (valorCelda == null && resultDetalle.TipoIntCodi == 3) { resultDetalle.RpePrgFechaInicio = (DateTime?)null; }
                                else
                                {
                                    try
                                    {
                                        tpfi = DateTime.ParseExact(valorCelda.ToUpper().Replace(".", ""), "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
                                        resultDetalle.RpePrgFechaInicio = (DateTime)tpfi;
                                    }
                                    catch (Exception e)
                                    {
                                        resultDetalle.arrEsValido[j] = false;
                                        listErrores.Add(PushError("PE", i, j, valorCelda, "Formato Fecha Invalido"));
                                    }
                                }
                                break;
                            case 13://Tiempo Programado - Fecha Fin
                                if (valorCelda == null && resultDetalle.TipoIntCodi != 3) { resultDetalle.arrEsValido[j] = false; listErrores.Add(PushError("PE", i, j, valorCelda, "Dato Nulo")); }
                                else if (valorCelda == null && resultDetalle.TipoIntCodi == 3) { resultDetalle.RpePrgFechaFin = (DateTime?)null; }
                                else
                                {
                                    try
                                    {
                                        tpff = DateTime.ParseExact(valorCelda.ToUpper().Replace(".", ""), "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
                                        if (tpff < tpfi) { resultDetalle.arrEsValido[j] = false; resultDetalle.arrEsValido[j - 1] = false; listErrores.Add(PushError("PE", i, j, valorCelda, "Formato Inicio mayor que Fecha Fin")); }
                                        resultDetalle.RpePrgFechaFin = (DateTime)tpff;
                                    }
                                    catch (Exception e)
                                    {
                                        resultDetalle.arrEsValido[j] = false;
                                        listErrores.Add(PushError("PE", i, j, valorCelda, "Formato Fecha Invalido"));
                                    }
                                }
                                break;
                            case 14://Empresa Responsable 1 - Nombre
                                if (valorCelda == null) { resultDetalle.arrEsValido[j] = false; listErrores.Add(PushError("PE", i, j, valorCelda, "Dato Nulo")); }
                                else
                                {
                                    SiEmpresaDTO responsable = FactorySic.GetSiEmpresaRepository().ListResponsable(valorCelda);
                                    if (responsable.Emprnomb == null)
                                    {
                                        resultDetalle.arrEsValido[j] = false;
                                        listErrores.Add(PushError("PE", i, j, valorCelda, "Empresa Responsable Invalida"));
                                    }
                                    else
                                    {
                                        //obtener codigo de empresa responsable
                                        resultDetalle.RpeEmpResponsables = responsable.Emprcodi.ToString();
                                    }
                                }
                                break;
                            case 15://Empresa Responsable 1 - Porcentaje
                                if (valorCelda == null) { resultDetalle.arrEsValido[j] = false; listErrores.Add(PushError("PE", i, j, valorCelda, "Dato Nulo")); }
                                else
                                {
                                    try
                                    {
                                        porcEmpr1 = Convert.ToDecimal(valorCelda);
                                        resultDetalle.RpeEmpResponsables = resultDetalle.RpeEmpResponsables + "," + porcEmpr1.ToString() + ";";
                                        if (porcEmpr1 == 1)
                                        {
                                            porcentajeCompleto = true;

                                        }
                                        else if (porcEmpr1 > 1)
                                        {
                                            porcentajeCompleto = true;
                                            resultDetalle.arrEsValido[j] = false;
                                            listErrores.Add(PushError("PE", i, j, valorCelda, "Se excede del 100%"));
                                        }
                                    }
                                    catch (Exception e)
                                    {
                                        resultDetalle.arrEsValido[j] = false;
                                        listErrores.Add(PushError("PE", i, j, valorCelda, "Dato No Numerico"));
                                    }
                                }
                                break;
                            case 16://Empresa Responsable 2 - Nombre
                                if (porcentajeCompleto == true)
                                {
                                    if (valorCelda != null)
                                    {
                                        resultDetalle.arrEsValido[j] = false;
                                        listErrores.Add(PushError("PE", i, j, valorCelda, "Dato Invalido Si Llego al 100%"));
                                    }
                                }
                                else
                                {
                                    if (valorCelda == null)
                                    {
                                        resultDetalle.arrEsValido[j] = false;
                                        listErrores.Add(PushError("PE", i, j, valorCelda, "No se ha llegado a 100% con la empresa responsable anterior"));
                                        porcentajeCompleto = true;
                                    }
                                    else
                                    {
                                        SiEmpresaDTO responsable = FactorySic.GetSiEmpresaRepository().ListResponsable(valorCelda);
                                        if (responsable.Emprnomb == null)
                                        {
                                            resultDetalle.arrEsValido[j] = false;
                                            listErrores.Add(PushError("PE", i, j, valorCelda, "Empresa Responsable Invalida"));
                                        }
                                        else
                                        {
                                            resultDetalle.RpeEmpResponsables = resultDetalle.RpeEmpResponsables + responsable.Emprcodi.ToString();
                                        }
                                    }
                                }

                                break;
                            case 17://Empresa Responsable 2 - Porcentaje
                                if (porcentajeCompleto == true)
                                {
                                    if (valorCelda != null)
                                    {
                                        resultDetalle.arrEsValido[j] = false;
                                        listErrores.Add(PushError("PE", i, j, valorCelda, "Dato Invalido, porque ya se completo el 100%"));
                                    }
                                }
                                else
                                {
                                    try
                                    {
                                        porcEmpr2 = Convert.ToDecimal(valorCelda);
                                        resultDetalle.RpeEmpResponsables = resultDetalle.RpeEmpResponsables + "," + porcEmpr2.ToString() + ";";
                                        if (porcEmpr1 + porcEmpr2 == 1)
                                        {
                                            porcentajeCompleto = true;

                                        }
                                        else if (porcEmpr1 + porcEmpr2 > 1)
                                        {
                                            porcentajeCompleto = true;
                                            resultDetalle.arrEsValido[j] = false;
                                            listErrores.Add(PushError("PE", i, j, valorCelda, "Se excede del 100%"));
                                        }

                                    }
                                    catch (Exception e)
                                    {
                                        resultDetalle.arrEsValido[j] = false;
                                        listErrores.Add(PushError("PE", i, j, valorCelda, "Dato No Numerico"));
                                    }
                                }
                                break;
                            case 18://Empresa Responsable 3 - Nombre
                                if (porcentajeCompleto == true)
                                {
                                    if (valorCelda != null)
                                    {
                                        resultDetalle.arrEsValido[j] = false;
                                        listErrores.Add(PushError("PE", i, j, valorCelda, "Dato Invalido Si Llego al 100%"));
                                    }
                                }
                                else
                                {
                                    if (valorCelda == null)
                                    {
                                        resultDetalle.arrEsValido[j] = false;
                                        listErrores.Add(PushError("PE", i, j, valorCelda, "No se ha llegado a 100% con la empresa responsable anterior"));
                                        porcentajeCompleto = true;
                                    }
                                    else
                                    {
                                        SiEmpresaDTO responsable = FactorySic.GetSiEmpresaRepository().ListResponsable(valorCelda);
                                        if (responsable.Emprnomb == null)
                                        {
                                            resultDetalle.arrEsValido[j] = false;
                                            listErrores.Add(PushError("PE", i, j, valorCelda, "Empresa Responsable Invalida"));
                                        }
                                        else
                                        {
                                            resultDetalle.RpeEmpResponsables = resultDetalle.RpeEmpResponsables + responsable.Emprcodi.ToString();
                                        }
                                    }


                                }
                                break;
                            case 19://Empresa Responsable 3 - Porcentaje
                                if (porcentajeCompleto == true)
                                {
                                    if (valorCelda != null)
                                    {
                                        resultDetalle.arrEsValido[j] = false;
                                        listErrores.Add(PushError("PE", i, j, valorCelda, "Dato Invalido Si Llego al 100%"));
                                    }
                                }
                                else
                                {
                                    try
                                    {
                                        porcEmpr3 = Convert.ToDecimal(valorCelda);
                                        resultDetalle.RpeEmpResponsables = resultDetalle.RpeEmpResponsables + "," + porcEmpr3.ToString() + ";";
                                        if (porcEmpr3 + porcEmpr2 + porcEmpr1 == 1)
                                        {
                                            porcentajeCompleto = true;

                                        }
                                        else if (porcEmpr3 + porcEmpr2 + porcEmpr1 > 1)
                                        {
                                            porcentajeCompleto = true;
                                            resultDetalle.arrEsValido[j] = false;
                                            listErrores.Add(PushError("PE", i, j, valorCelda, "Se excede del 100%"));
                                        }

                                    }
                                    catch (Exception e)
                                    {
                                        resultDetalle.arrEsValido[j] = false;
                                        listErrores.Add(PushError("PE", i, j, valorCelda, "Dato No Numerico"));
                                    }
                                }
                                break;
                            case 20://Empresa Responsable 4 - Nombre
                                if (porcentajeCompleto == true)
                                {
                                    if (valorCelda != null)
                                    {
                                        resultDetalle.arrEsValido[j] = false;
                                        listErrores.Add(PushError("PE", i, j, valorCelda, "Dato Invalido Si Llego al 100%"));
                                    }
                                }
                                else
                                {
                                    if (valorCelda == null)
                                    {
                                        resultDetalle.arrEsValido[j] = false;
                                        listErrores.Add(PushError("PE", i, j, valorCelda, "No se ha llegado a 100% con la empresa responsable anterior"));
                                        porcentajeCompleto = true;
                                    }
                                    else
                                    {
                                        SiEmpresaDTO responsable = FactorySic.GetSiEmpresaRepository().ListResponsable(valorCelda);
                                        if (responsable.Emprnomb == null)
                                        {
                                            resultDetalle.arrEsValido[j] = false;
                                            listErrores.Add(PushError("PE", i, j, valorCelda, "Empresa Responsable Invalida"));
                                        }
                                        else
                                        {
                                            resultDetalle.RpeEmpResponsables = resultDetalle.RpeEmpResponsables + responsable.Emprcodi.ToString();
                                        }
                                    }

                                }
                                break;
                            case 21://Empresa Responsable 4 - Porcentaje

                                if (porcentajeCompleto == true)
                                {
                                    if (valorCelda != null)
                                    {
                                        resultDetalle.arrEsValido[j] = false;
                                        listErrores.Add(PushError("PE", i, j, valorCelda, "Dato Invalido Si Llego al 100%"));
                                    }
                                }
                                else
                                {
                                    try
                                    {
                                        porcEmpr4 = Convert.ToDecimal(valorCelda);
                                        resultDetalle.RpeEmpResponsables = resultDetalle.RpeEmpResponsables + "," + porcEmpr4.ToString() + ";";
                                        if (porcEmpr4 + porcEmpr3 + porcEmpr2 + porcEmpr1 == 1)
                                        {
                                            porcentajeCompleto = true;

                                        }
                                        else if (porcEmpr4 + porcEmpr3 + porcEmpr2 + porcEmpr1 > 1)
                                        {
                                            porcentajeCompleto = true;
                                            resultDetalle.arrEsValido[j] = false;
                                            listErrores.Add(PushError("PE", i, j, valorCelda, "Se excede del 100%"));
                                        }

                                    }
                                    catch (Exception e)
                                    {
                                        resultDetalle.arrEsValido[j] = false;
                                        listErrores.Add(PushError("PE", i, j, valorCelda, "Dato No Numerico"));
                                    }
                                }
                                break;
                            case 22://Empresa Responsable 5 - Nombre
                                if (porcentajeCompleto == true)
                                {
                                    if (valorCelda != null)
                                    {
                                        resultDetalle.arrEsValido[j] = false;
                                        listErrores.Add(PushError("PE", i, j, valorCelda, "Dato Invalido Si Llego al 100%"));
                                    }
                                }
                                else
                                {
                                    if (valorCelda == null)
                                    {
                                        resultDetalle.arrEsValido[j] = false;
                                        listErrores.Add(PushError("PE", i, j, valorCelda, "No se ha llegado a 100% con la empresa responsable anterior"));
                                        porcentajeCompleto = true;
                                    }
                                    else
                                    {
                                        SiEmpresaDTO responsable = FactorySic.GetSiEmpresaRepository().ListResponsable(valorCelda);
                                        if (responsable.Emprnomb == null)
                                        {
                                            resultDetalle.arrEsValido[j] = false;
                                            listErrores.Add(PushError("PE", i, j, valorCelda, "Empresa Responsable Invalida"));
                                        }
                                        else
                                        {
                                            resultDetalle.RpeEmpResponsables = resultDetalle.RpeEmpResponsables + responsable.Emprcodi.ToString();
                                        }
                                    }
                                }
                                break;
                            case 23://Empresa Responsable 5 - Porcentaje
                                if (porcentajeCompleto == true)
                                {
                                    if (valorCelda != null)
                                    {
                                        resultDetalle.arrEsValido[j] = false;
                                        listErrores.Add(PushError("PE", i, j, valorCelda, "Dato Invalido Si Llego al 100%"));
                                    }
                                }
                                else
                                {
                                    try
                                    {
                                        porcEmpr5 = Convert.ToDecimal(valorCelda);
                                        if (porcEmpr5 + porcEmpr4 + porcEmpr3 + porcEmpr2 + porcEmpr1 == 1)
                                        {
                                            porcentajeCompleto = true;
                                            resultDetalle.RpeEmpResponsables = resultDetalle.RpeEmpResponsables + "," + porcEmpr5.ToString();
                                        }
                                        else if (porcEmpr5 + porcEmpr4 + porcEmpr3 + porcEmpr2 + porcEmpr1 > 1)
                                        {
                                            porcentajeCompleto = true;
                                            resultDetalle.arrEsValido[j] = false;
                                            listErrores.Add(PushError("PE", i, j, valorCelda, "Se excede del 100%"));
                                        }
                                        else
                                        {
                                            resultDetalle.arrEsValido[j] = false;
                                            listErrores.Add(PushError("PE", i, j, valorCelda, "No suman 100%"));
                                        }

                                    }
                                    catch (Exception e)
                                    {
                                        resultDetalle.arrEsValido[j] = false;
                                        listErrores.Add(PushError("PE", i, j, valorCelda, "Dato No Numerico"));
                                    }
                                }
                                break;
                            case 24://Causa Resumida Interrupción
                                if (valorCelda == null) { resultDetalle.arrEsValido[j] = false; listErrores.Add(PushError("PE", i, j, valorCelda, "Dato Nulo")); }
                                else if (valorCelda.Length >= 255) { resultDetalle.arrEsValido[j] = false; listErrores.Add(PushError("PE", i, j, valorCelda, "Dato Longitud Mayor 255 caracteres")); }
                                else resultDetalle.RpeCausaInterrupcion = valorCelda;
                                break;
                            case 25://Ei / E
                                if (valorCelda == null) { resultDetalle.arrEsValido[j] = false; listErrores.Add(PushError("PE", i, j, valorCelda, "Dato Nulo")); }
                                else
                                {
                                    if ((resultDetalle.arrEsValido[j] = analizarNumerico(valorCelda.ToString())) == false) listErrores.Add(PushError("PE", i, j, valorCelda, "Dato No Numerico"));
                                    else
                                    {
                                        ni = resultDetalle.RpeNi;
                                        minutos = Math.Round(diffDays * 24 * 60, 3);
                                        if (minutos < 3)
                                        {
                                            diffDays = 0;
                                            ni = 0;
                                            ki = 0;
                                        }

                                        double di = resultDetalle.RpeKi * diffDays * 24;

                                        double factorParcial = 0;
                                        if (factor != 0)
                                        {
                                            factorParcial = 0.5 * (ni / n + di / d);
                                            if (n > toln) factorParcial = factorParcial + (ni / n) * (n - toln) / toln;
                                            if (d > told) factorParcial = factorParcial + (di / d) * (d - told) / told;
                                            factorParcial = factorParcial / factor;
                                        }

                                        valorEiE = resultDetalle.RpeEiE = factorParcial;

                                        if (resultDetalle.RpeEiE > Convert.ToDouble(valorCelda))
                                        {
                                            if (resultDetalle.RpeEiE - Convert.ToDouble(valorCelda) > Convert.ToDouble(System.Configuration.ConfigurationManager.AppSettings["MargenCalculo"]))
                                            {
                                                resultDetalle.arrEsValido[j] = false; listErrores.Add(PushError("PE", i, j, valorCelda, "Dato Invalido"));
                                            }
                                        }
                                        else
                                        {
                                            if (Convert.ToDouble(valorCelda) - resultDetalle.RpeEiE > Convert.ToDouble(System.Configuration.ConfigurationManager.AppSettings["MargenCalculo"]))
                                            {
                                                resultDetalle.arrEsValido[j] = false; listErrores.Add(PushError("PE", i, j, valorCelda, "Dato Invalido"));
                                            }
                                        }
                                        resultDetalle.RpeEiE = resultDetalle.RpeEiE * 100;
                                    }
                                }

                                break;
                            case 26://Monto Resarcimiento
                                if (valorCelda == null) { resultDetalle.arrEsValido[j] = false; listErrores.Add(PushError("PE", i, j, valorCelda, "Dato Nulo")); }
                                else
                                {
                                    if ((resultDetalle.arrEsValido[j] = analizarNumerico(valorCelda.ToString())) == false) listErrores.Add(PushError("PE", i, j, valorCelda, "Dato No Numerico"));
                                    else
                                    {
                                        //Validar Monto de Resarcimiento
                                        resultDetalle.RpeCompensacion = valorEiE * resarcimiento;

                                        if (resultDetalle.RpeCompensacion > Convert.ToDouble(valorCelda))
                                        {
                                            if (resultDetalle.RpeCompensacion - Convert.ToDouble(valorCelda) > Convert.ToDouble(System.Configuration.ConfigurationManager.AppSettings["MargenCalculo"]))
                                            {
                                                resultDetalle.arrEsValido[j] = false; listErrores.Add(PushError("PE", i, j, valorCelda, "Valor Invalido"));
                                            }
                                        }
                                        else
                                        {
                                            if (Convert.ToDouble(valorCelda) - resultDetalle.RpeCompensacion > Convert.ToDouble(System.Configuration.ConfigurationManager.AppSettings["MargenCalculo"]))
                                            {
                                                resultDetalle.arrEsValido[j] = false; listErrores.Add(PushError("PE", i, j, valorCelda, "Valor Invalido"));
                                            }
                                        }


                                    }
                                }
                                break;
                            default:
                                break;

                        }
                    }

                    result.Add(resultDetalle);


                }

            }

            return result;
        }

        /// <summary>
        /// Permite validar las celdas de la grilla con informaci►n de resarcimientos de Rechazo de Carga
        /// </summary>
        /// <param name="periodo"></param>
        /// <param name="emprcodi"></param>
        /// <param name="puntoEntrega"></param>
        /// <param name="arrFila"></param>
        /// <param name="listErrores"></param>
        /// <returns></returns>
        public List<RntRegRechazoCargaDTO> ValidarGrillaRC(int periodo, int emprcodi, int puntoEntrega, string[][] arrFila, out List<RntListErroresDTO> listErrores)
        {
            int numColsRC = 14; // 20 agosto 2016
            int LEN_MAX_SED_NOMBRE = 50, LEN_MAX_COD_ALIMENTADOR = 20;
            DateTime? tpfi = null;
            DateTime? tpff = null;
            RntListErroresDTO errorCelda = new RntListErroresDTO();
            listErrores = new List<RntListErroresDTO>(arrFila.Length * numColsRC);

            List<RntRegRechazoCargaDTO> result = new List<RntRegRechazoCargaDTO>(arrFila.Length);
            RntRegRechazoCargaDTO resultDetalle = null;

            int countGrupo = 0, posicion = -1;
            int countGrupoA = 0;
            int[] posicionA = new int[arrFila.Length];
            bool mismoGrupo = false;
            bool mismoGrupoA = false;
            string[][] arrGrupo = new string[arrFila.Length][];
            string[][] arrGrupoA = new string[arrFila.Length][];
            string[][] arrCalculoGrupo = new string[arrFila.Length][];
            string[][] arrCalculoGrupoA = new string[arrFila.Length][];
            string valorCelda = null;
            string valorCeldaA = null;
            string valorCeldaComp = null;

            //Inicializar arreglo de bases de grupo
            for (int k = 0; k < arrFila.Length; k++)
            {
                arrGrupo[k] = new string[numColsRC];
                arrGrupoA[k] = new string[numColsRC];
                arrCalculoGrupo[k] = new string[4];
                arrCalculoGrupoA[k] = new string[4];
                for (int m = 0; m < numColsRC; m++)
                {
                    arrGrupo[k][m] = "-1";
                    arrGrupoA[k][m] = "-1";
                }
            }

            int nregxgrupo = 0;
            double energia = 0;
            double ef = 0;
            double d = 0;
            double dk = 0;
            double pk = 0;
            double pkdk = 0;
            string[] alimentadores = new string[arrFila.Length + 1];//20 agosto 2016
            int nalimentadores = 0;
            DateTime tini;
            DateTime tfin;
            double diffDays = 0;
            double minutos = 0;//20-08-2016


            //Carga de alimentadores
            if (arrFila.Length > 0)
            {
                for (int i = 0; i < arrFila.Length; i++)
                {
                    valorCelda = arrFila[i][0];
                    if (valorCelda != null)
                        if (analizarNumerico(valorCelda.ToString()) != false)
                        {
                            bool existeAlimentador = false;
                            for (int k = 0; k < nalimentadores; k++)
                                if (alimentadores[k].ToString() == arrFila[i][3].ToString())
                                    existeAlimentador = true;

                            if (!existeAlimentador)
                            {
                                alimentadores[nalimentadores] = arrFila[i][3].ToString();
                                nalimentadores = nalimentadores + 1;
                            }
                        }
                }
            }
            //Termina Carga Alimentadores


            if (arrFila.Length > 0)
            {
                //Recolectar Datos para Calculo de resarcimiento x Grupo
                for (int i = 0; i < arrFila.Length; i++)
                {
                    valorCelda = arrFila[i][0];
                    valorCeldaA = arrFila[i][3];
                    valorCeldaComp = arrFila[i][11];

                    if (valorCelda != null)
                        if (analizarNumerico(valorCelda.ToString()) != false)
                        {
                            string compensable = "";
                            if ((compensable = FactoryTransferencia.GetRntConfiguracionRepository().ListParametroRep("COMPENSABLE", valorCeldaComp.ToString()).ConfValor) == null)
                            {
                                //error en dato de la celda. Defecto NO
                                compensable = "NO";
                            }

                            if ((mismoGrupoA = EvalMismoGrupoA(arrGrupoA, countGrupoA, valorCeldaA, out posicionA)) == false)
                            {
                                for (int k = 0; k < numColsRC; k++)
                                    arrGrupoA[countGrupoA][k] = arrFila[i][k];

                                //Calculos iniciales
                                tini = DateTime.ParseExact(arrFila[i][8].ToUpper().Replace(".", ""), "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
                                tfin = DateTime.ParseExact(arrFila[i][9].ToUpper().Replace(".", ""), "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
                                diffDays = ((DateTime)tfin - (DateTime)tini).TotalDays;

                                minutos = Math.Round(diffDays * 24 * 60, 3);  // 20 - agosto 2016
                                if ((minutos < 3) || (compensable == "NO"))
                                {
                                    diffDays = 0;
                                }

                                if (diffDays > 0)
                                {
                                    d = diffDays * 24;
                                    nregxgrupo = 1;
                                }

                                //inicializar "d"
                                arrCalculoGrupoA[countGrupoA][0] = d.ToString(); ;
                                //inicializar "nregxgrupo"
                                arrCalculoGrupoA[countGrupoA][1] = nregxgrupo.ToString();

                                countGrupoA++;

                            }
                            else
                            {
                                try
                                {
                                    //Calcular "d"
                                    tini = DateTime.ParseExact(arrFila[i][8].ToUpper().Replace(".", ""), "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
                                    tfin = DateTime.ParseExact(arrFila[i][9].ToUpper().Replace(".", ""), "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
                                    diffDays = ((DateTime)tfin - (DateTime)tini).TotalDays;

                                    minutos = Math.Round(diffDays * 24 * 60, 3);  // 20 - agosto 2016
                                    if ((minutos < 3) || (compensable == "NO"))
                                    {
                                        diffDays = 0;
                                    }

                                    if (diffDays > 0)
                                    {
                                        //recupera valores para sumar
                                        d = Convert.ToDouble(arrCalculoGrupoA[posicionA[0]][0]);
                                        nregxgrupo = Convert.ToInt32(arrCalculoGrupoA[posicionA[0]][1]);

                                        d = d + diffDays * 24;
                                        nregxgrupo++;

                                        //actualizar en todas las posiciones
                                        for (int k = 0; k < countGrupoA; k++)
                                        {
                                            if (posicionA[k] != -1)
                                            {
                                                arrCalculoGrupoA[posicionA[k]][0] = d.ToString();
                                                arrCalculoGrupoA[posicionA[k]][1] = nregxgrupo.ToString();
                                            }
                                        }

                                    }


                                }
                                catch (Exception e)
                                {

                                }

                            }

                            if ((mismoGrupo = EvalMismoGrupo(arrGrupo, countGrupo, valorCelda, out posicion)) == false)
                            {
                                for (int k = 0; k < numColsRC; k++)
                                    arrGrupo[countGrupo][k] = arrFila[i][k];

                                //Calcular valores factor y resarcimiento del Grupo anterior
                                if (countGrupo > 0) //Segundo Registro en adelante empieza a cerrar calculos x grupos
                                {
                                    arrCalculoGrupo[countGrupo - 1][0] = energia.ToString();
                                    arrCalculoGrupo[countGrupo - 1][3] = pkdk.ToString();
                                }

                                //Primer Registro del archivo o primer registro  del Grupo
                                // Inicializar 
                                //inicializar "energia"
                                arrCalculoGrupo[countGrupo][0] = "0";
                                //inicializar "pkdk"
                                arrCalculoGrupo[countGrupo][3] = "0";


                                //Calculos iniciales
                                //energia
                                energia = Convert.ToDouble(arrFila[i][6]);
                                //d
                                tini = DateTime.ParseExact(arrFila[i][8].ToUpper().Replace(".", ""), "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
                                tfin = DateTime.ParseExact(arrFila[i][9].ToUpper().Replace(".", ""), "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
                                diffDays = ((DateTime)tfin - (DateTime)tini).TotalDays;

                                dk = diffDays * 24;
                                pk = Convert.ToDouble(arrFila[i][10]);
                                pkdk = dk * pk;

                                countGrupo++;

                                // SI es Ultimo del Grupo y del Archivo
                                if (arrFila.Length - 1 == i)
                                {
                                    arrCalculoGrupo[countGrupo - 1][0] = energia.ToString();
                                    arrCalculoGrupo[countGrupo - 1][3] = pkdk.ToString();
                                }

                            }
                            else
                            { //mismo grupo
                                try
                                {
                                    //Calcular "d"
                                    tini = DateTime.ParseExact(arrFila[i][8].ToUpper().Replace(".", ""), "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
                                    tfin = DateTime.ParseExact(arrFila[i][9].ToUpper().Replace(".", ""), "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
                                    diffDays = ((DateTime)tfin - (DateTime)tini).TotalDays;

                                    dk = diffDays * 24;
                                    pk = Convert.ToDouble(arrFila[i][10]);
                                    pkdk = pkdk + dk * pk;

                                    // Si es Ultimo del Grupo y del Archivo
                                    if (arrFila.Length - 1 == i)
                                    {
                                        arrCalculoGrupo[countGrupo - 1][0] = energia.ToString();
                                        arrCalculoGrupo[countGrupo - 1][3] = pkdk.ToString();
                                    }

                                }
                                catch (Exception e)
                                {

                                }
                            }

                        }

                }
            }
            //Termina Recolectar Datos para Calculo Resarcimiento

            //Nuevamente inicializa Contador de Grupo
            countGrupo = 0;
            //Inicializar arreglo de bases de grupo
            for (int k = 0; k < arrFila.Length; k++)
            {
                arrGrupo[k] = new string[numColsRC];
                for (int l = 0; l < numColsRC; l++)
                {
                    arrGrupo[k][l] = "-1";
                }
            }


            bool error_nro_grupo = false;
            if (arrFila.Length > 0)
            {
                for (int i = 0; i < arrFila.Length; i++)
                {
                    string compensable = "";
                    resultDetalle = new RntRegRechazoCargaDTO();
                    resultDetalle.arrEsValido = new bool[numColsRC];

                    for (int j = 0; j < numColsRC; j++)
                    {
                        valorCelda = arrFila[i][j];
                        resultDetalle.arrEsValido[j] = true;

                        resultDetalle.PeriodoCodi = periodo;
                        resultDetalle.RrcEmpresaGeneradora = emprcodi;
                        resultDetalle.AreaCodi = puntoEntrega; // CONSULTAR DE DONDE OBTENER PUNTO DE ENTREGA/ AREA CODI

                        switch (j)
                        {
                            case 0:
                                if (valorCelda == null)
                                {
                                    resultDetalle.arrEsValido[j] = false;
                                    listErrores.Add(PushError("RC", i, j, valorCelda, "Dato nulo"));
                                }
                                else
                                {
                                    resultDetalle.arrEsValido[j] = analizarNumerico(valorCelda.ToString());
                                    if (resultDetalle.arrEsValido[j] == false) listErrores.Add(PushError("RC", i, j, valorCelda, "Dato No Numerico"));
                                    else
                                    {
                                        resultDetalle.RrcGrupoEnvio = Convert.ToInt32(valorCelda);
                                        if ((mismoGrupo = EvalMismoGrupo(arrGrupo, countGrupo, valorCelda, out posicion)) == false)
                                        {

                                            energia = Convert.ToDouble(arrCalculoGrupo[countGrupo][0]);
                                            pkdk = Convert.ToDouble(arrCalculoGrupo[countGrupo][3]);

                                            for (int k = 0; k < numColsRC; k++)
                                                arrGrupo[countGrupo][k] = arrFila[i][k];

                                            countGrupo++;
                                        }

                                        if (posicion == 999) { error_nro_grupo = true; resultDetalle.arrEsValido[j] = false; listErrores.Add(PushError("RC", i, j, valorCelda, "Nro. de Grupo de Punto Entrega (Primera columna) Invalido")); }
                                        else error_nro_grupo = false;
                                    }
                                }
                                break;
                            case 1: //Cliente
                                if (error_nro_grupo == true) break;
                                else
                                {
                                    if (mismoGrupo == true && valorCelda == null) valorCelda = arrGrupo[posicion][j];
                                    if (mismoGrupo == false && valorCelda == null) { resultDetalle.arrEsValido[j] = false; listErrores.Add(PushError("RC", i, j, valorCelda, "Dato nulo")); }
                                    else
                                    {

                                        if ((resultDetalle.arrEsValido[j] = analizarNumerico(valorCelda.ToString())) == true)
                                            resultDetalle.RrcCliente = Convert.ToInt32(valorCelda);
                                        else
                                        {
                                            listErrores.Add(PushError("RC", i, j, valorCelda, "ID Cliente Invalido"));
                                            resultDetalle.arrEsValido[j] = false;
                                        }
                                    }

                                }
                                break;
                            case 2: // Barra
                                if (error_nro_grupo == true) break;
                                if (mismoGrupo == true && valorCelda == null) valorCelda = arrGrupo[posicion][j];
                                if (mismoGrupo == false && valorCelda == null) { resultDetalle.arrEsValido[j] = false; listErrores.Add(PushError("RC", i, j, valorCelda, "Dato nulo")); }
                                else
                                {
                                    if ((resultDetalle.arrEsValido[j] = analizarNumerico(valorCelda.ToString())) == true)
                                        resultDetalle.Barrcodi = Convert.ToInt32(valorCelda);
                                    else
                                    {
                                        listErrores.Add(PushError("RC", i, j, valorCelda, "ID Barra Invalido"));
                                        resultDetalle.arrEsValido[j] = false;
                                    }

                                }
                                break;
                            case 3: //Codigo Alimentador
                                if (error_nro_grupo == true) break;
                                if (mismoGrupo == true && valorCelda == null) valorCelda = arrGrupo[posicion][j];
                                if (mismoGrupo == false && valorCelda == null) { resultDetalle.arrEsValido[j] = false; listErrores.Add(PushError("RC", i, j, valorCelda, "Dato nulo")); }
                                else if (valorCelda.Length > LEN_MAX_COD_ALIMENTADOR) { resultDetalle.arrEsValido[j] = false; listErrores.Add(PushError("RC", i, j, valorCelda, "Longitud Dato mayor que: " + LEN_MAX_COD_ALIMENTADOR)); }
                                else
                                {
                                    resultDetalle.RrcCodiAlimentador = valorCelda;

                                    if ((mismoGrupoA = EvalMismoGrupoA(arrGrupoA, countGrupoA, valorCelda, out posicionA)) == true)
                                    {

                                        d = Convert.ToDouble(arrCalculoGrupoA[posicionA[0]][0]);
                                        nregxgrupo = Convert.ToInt32(arrCalculoGrupoA[posicionA[0]][1]);
                                    }
                                    else// Error . Siempre debe encontrar los datos cargados
                                    {
                                        d = 0;
                                        nregxgrupo = 0;
                                    }

                                }
                                break;
                            case 4: //SED - Nombre
                                if (error_nro_grupo == true) break;
                                if (mismoGrupo == true && valorCelda == null) valorCelda = arrGrupo[posicion][j];
                                if (mismoGrupo == false && valorCelda == null) { resultDetalle.arrEsValido[j] = false; listErrores.Add(PushError("RC", i, j, valorCelda, "Dato nulo")); }
                                else if (valorCelda.Length > LEN_MAX_SED_NOMBRE) { resultDetalle.arrEsValido[j] = false; listErrores.Add(PushError("RC", i, j, valorCelda, "Longitud Dato mayor que: " + LEN_MAX_SED_NOMBRE)); }
                                else resultDetalle.RrcSubestacionDstrb = valorCelda;
                                break;
                            case 5: //SED Kv
                                if (error_nro_grupo == true) break;
                                if (mismoGrupo == true && valorCelda == null) valorCelda = arrGrupo[posicion][j];
                                if (mismoGrupo == false && valorCelda == null) { resultDetalle.arrEsValido[j] = false; listErrores.Add(PushError("RC", i, j, valorCelda, "Dato nulo")); }
                                else if ((resultDetalle.arrEsValido[j] = analizarNumerico(valorCelda.ToString())) == true)
                                    resultDetalle.RrcNivelTensionSed = Convert.ToDecimal(valorCelda);
                                break;
                            case 6: //ENS f
                                if (error_nro_grupo == true) break;
                                if (mismoGrupo == true && valorCelda == null) valorCelda = arrGrupo[posicion][j];
                                if (mismoGrupo == false && valorCelda == null) { resultDetalle.arrEsValido[j] = false; listErrores.Add(PushError("RC", i, j, valorCelda, "Dato nulo")); }
                                else if ((resultDetalle.arrEsValido[j] = analizarNumerico(valorCelda.ToString())) == true)
                                    resultDetalle.RrcEf = Convert.ToDecimal(valorCelda);
                                break;
                            case 7: //Codigo Evento
                                if (error_nro_grupo == true) break;
                                if (valorCelda == null) { resultDetalle.arrEsValido[j] = false; listErrores.Add(PushError("RC", i, j, valorCelda, "Dato nulo")); }
                                else
                                {
                                    List<EveEventoDTO> listEventos = this.ListEventos(emprcodi);

                                    var codEvento = listEventos.Find(item => item.CodEve == valorCelda);
                                    if (codEvento == null)
                                    {
                                        listErrores.Add(PushError("RC", i, j, valorCelda, "Codigo Evento Invalido"));
                                        resultDetalle.arrEsValido[j] = false;
                                    }
                                    else
                                    {
                                        resultDetalle.RrcEvenCodiDesc = valorCelda;
                                        resultDetalle.EvenCodi = codEvento.Evencodi; //grabando tambien codigo de evento
                                    }
                                }
                                break;
                            case 8://Interrupcion - Fecha Inicio
                                if (error_nro_grupo == true) break;
                                if (valorCelda == null) { resultDetalle.arrEsValido[j] = false; listErrores.Add(PushError("RC", i, j, valorCelda, "Dato nulo")); }
                                else
                                {
                                    try
                                    {
                                        tpfi = DateTime.ParseExact(valorCelda.ToUpper().Replace(".", ""), "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
                                        resultDetalle.RrcFechaInicio = (DateTime)tpfi;
                                    }
                                    catch (Exception e)
                                    {
                                        resultDetalle.arrEsValido[j] = false;
                                    }
                                }
                                break;
                            case 9://Interrupcion - Fecha Fin
                                if (error_nro_grupo == true) break;
                                if (valorCelda == null) { resultDetalle.arrEsValido[j] = false; listErrores.Add(PushError("RC", i, j, valorCelda, "Dato nulo")); }
                                else
                                {
                                    try
                                    {
                                        tpff = DateTime.ParseExact(valorCelda.ToUpper().Replace(".", ""), "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
                                        diffDays = ((DateTime)tpff - (DateTime)tpfi).TotalDays;
                                        if (tpff < tpfi) { resultDetalle.arrEsValido[j] = false; resultDetalle.arrEsValido[j - 1] = false; listErrores.Add(PushError("RC", i, j, valorCelda, "Fecha Inicio mayor que Fecha Fin")); }
                                        else resultDetalle.RrcFechaFin = (DateTime)tpff;
                                    }
                                    catch (Exception e)
                                    {
                                        resultDetalle.arrEsValido[j] = false;
                                        listErrores.Add(PushError("RC", i, j, valorCelda, "Formato Fecha Invalido"));
                                    }
                                }
                                break;
                            case 10://PK
                                if (error_nro_grupo == true) break;
                                if (valorCelda == null) { resultDetalle.arrEsValido[j] = false; listErrores.Add(PushError("RC", i, j, valorCelda, "Dato nulo")); }
                                else if ((resultDetalle.arrEsValido[j] = analizarNumerico(valorCelda.ToString())) == false) listErrores.Add(PushError("RC", i, j, valorCelda, "Dato No Numerico"));
                                else resultDetalle.RrcPk = Convert.ToDecimal(valorCelda);
                                break;
                            case 11: //Compensable
                                if (error_nro_grupo == true) break;
                                if (valorCelda == null) { resultDetalle.arrEsValido[j] = false; listErrores.Add(PushError("RC", i, j, valorCelda, "Dato Nulo")); }
                                else if ((compensable = FactoryTransferencia.GetRntConfiguracionRepository().ListParametroRep("COMPENSABLE", valorCelda.ToString()).ConfValor) == null)
                                {
                                    resultDetalle.arrEsValido[j] = false;
                                    listErrores.Add(PushError("RC", i, j, valorCelda, "Dato No Valido"));
                                }
                                else
                                    resultDetalle.RrcCompensable = valorCelda.ToUpper();
                                break;
                            case 12://ENS fk
                                if (error_nro_grupo == true) break;
                                if (valorCelda == null) { resultDetalle.arrEsValido[j] = false; listErrores.Add(PushError("RC", i, j, valorCelda, "Dato nulo")); }
                                else if ((resultDetalle.arrEsValido[j] = analizarNumerico(valorCelda.ToString())) == false) listErrores.Add(PushError("RC", i, j, valorCelda, "Dato No Numerico"));
                                else
                                {
                                    //calcular valor **
                                    double dk1 = (double)diffDays * 24;
                                    pk = (double)resultDetalle.RrcPk;
                                    double ens = (dk1 * pk) / pkdk * energia;
                                    resultDetalle.RrcEnsFk = (decimal)ens;

                                    if ((double)resultDetalle.RrcEnsFk > Convert.ToDouble(valorCelda))
                                    {
                                        if ((double)resultDetalle.RrcEnsFk - Convert.ToDouble(valorCelda) > Convert.ToDouble(System.Configuration.ConfigurationManager.AppSettings["MargenCalculo"]))
                                        {
                                            resultDetalle.arrEsValido[j] = false; listErrores.Add(PushError("RC", i, j, valorCelda, "Valor Invalido"));
                                        }
                                    }
                                    else
                                    {
                                        if (Convert.ToDouble(valorCelda) - (double)resultDetalle.RrcEnsFk > Convert.ToDouble(System.Configuration.ConfigurationManager.AppSettings["MargenCalculo"]))
                                        {
                                            resultDetalle.arrEsValido[j] = false; listErrores.Add(PushError("RC", i, j, valorCelda, "Valor Invalido"));
                                        }
                                    }


                                }
                                break;
                            case 13://Resarcimiento
                                if (error_nro_grupo == true) break;
                                if (valorCelda == null) { resultDetalle.arrEsValido[j] = false; listErrores.Add(PushError("RC", i, j, valorCelda, "Dato nulo")); }
                                else if ((resultDetalle.arrEsValido[j] = analizarNumerico(valorCelda.ToString())) == false) listErrores.Add(PushError("RC", i, j, valorCelda, "Dato No Numerico"));
                                else
                                {
                                    if (nregxgrupo <= 2)
                                        ef = 1;
                                    else
                                    {
                                        ef = 1 + 1.0 * ((nregxgrupo - 2) * 1.0 / 4);
                                        if (d > 0.15) ef = ef + (d - 0.15) / 0.15;
                                        ef = Math.Round(ef, 2);

                                    }

                                    minutos = Math.Round(diffDays * 24 * 60, 3);
                                    if ((minutos < 3) || (compensable == "NO"))
                                        resultDetalle.RrcCompensacion = 0;
                                    else
                                        resultDetalle.RrcCompensacion = Convert.ToDecimal(0.35 * (double)resultDetalle.RrcEnsFk * ef);

                                    if ((double)resultDetalle.RrcCompensacion > Convert.ToDouble(valorCelda))
                                    {
                                        if ((double)resultDetalle.RrcCompensacion - Convert.ToDouble(valorCelda) > Convert.ToDouble(System.Configuration.ConfigurationManager.AppSettings["MargenCalculo"]))
                                        {
                                            resultDetalle.arrEsValido[j] = false; listErrores.Add(PushError("RC", i, j, valorCelda, "Valor Invalido"));
                                        }
                                    }
                                    else
                                    {
                                        if (Convert.ToDouble(valorCelda) - (double)resultDetalle.RrcCompensacion > Convert.ToDouble(System.Configuration.ConfigurationManager.AppSettings["MargenCalculo"]))
                                        {
                                            resultDetalle.arrEsValido[j] = false; listErrores.Add(PushError("RC", i, j, valorCelda, "Valor Invalido"));
                                        }
                                    }

                                }
                                break;
                            default:
                                break;
                        }
                    }

                    result.Add(resultDetalle);


                }

            }

            return result;
        }

        /// <summary>
        /// permite evaluar si el registro pertenece al mismo grupo de registros de resarcimientos
        /// </summary>
        /// <param name="arrGrupo"></param>
        /// <param name="countGrupo"></param>
        /// <param name="valorCelda"></param>
        /// <param name="posicion"></param>
        /// <returns></returns>
        public bool EvalMismoGrupo(string[][] arrGrupo, int countGrupo, string valorCelda, out int posicion)
        {
            bool mismoGrupo = false;
            posicion = -1;
            for (int r = 0; r <= countGrupo; r++)
                if (arrGrupo[r][0] == valorCelda)
                {
                    mismoGrupo = true;
                    if (r < (countGrupo - 1)) posicion = 999;
                    else posicion = r;
                    break;
                }
            return mismoGrupo;
        }

        public bool EvalMismoGrupoA(string[][] arrGrupoA, int countGrupoA, string valorCeldaA, out int[] posicionA)
        {
            bool mismoGrupoA = false;

            posicionA = new int[countGrupoA];

            for (int i = 0; i < countGrupoA; i++)
            {
                posicionA[i] = -1;
            }

            int npos = 0;
            for (int r = 0; r <= countGrupoA; r++)
            {
                if (arrGrupoA[r][3] == valorCeldaA) { mismoGrupoA = true; posicionA[npos] = r; npos++; }
            }

            return mismoGrupoA;
        }

        /// <summary>
        /// permite colocar los errores en un arreglo para que luego sea enviado para respuesta detallada de inconsistencias          
        /// </summary>
        /// <param name="tipontcse"></param>
        /// <param name="i"></param>
        /// <param name="j"></param>
        /// <param name="valorCelda"></param>
        /// <param name="descError"></param>
        /// <returns></returns>
        RntListErroresDTO PushError(string tipontcse, int i, int j, string valorCelda, string descError)
        {
            RntListErroresDTO errorCelda = new RntListErroresDTO();
            int valinput = j;

            switch (j)
            {
                case 0: j = 65; break;
                case 1: j = 66; break;
                case 2: j = 67; break;
                case 3: j = 67; break;
                case 4: j = 69; break;
                case 5: j = 70; break;
                case 6: j = 71; break;
                case 7: j = 72; break;
                case 8: j = 73; break;
                case 9: j = 74; break;
                case 10: j = 75; break;
                case 11: j = 76; break;
                case 12: j = 77; break;
                case 13: j = 78; break;
                case 14: j = 79; break;
                case 15: j = 80; break;
                case 16: j = 81; break;
                case 17: j = 82; break;
                case 18: j = 83; break;
                case 19: j = 84; break;
                case 20: j = 85; break;
                case 21: j = 86; break;
                case 22: j = 87; break;
                case 23: j = 88; break;
                case 24: j = 89; break;
                case 25: j = 90; break;

                case 26: j = 65; break;
                case 27: j = 66; break;
                case 28: j = 67; break;
                case 29: j = 68; break;
            }

            errorCelda.tipontcse = tipontcse;
            if (valinput > 25)
                errorCelda.celda = "A" + Convert.ToChar(j) + "," + (i + 3);
            else
                errorCelda.celda = Convert.ToChar(j) + "," + (i + 3);
            errorCelda.valor = valorCelda;
            errorCelda.tipo = descError;

            return errorCelda;
        }

        /// <summary>
        /// Permite analizar si el valor es numérico.
        /// </summary>
        /// <param name="valor"></param>
        /// <returns></returns>
        private Boolean analizarNumerico(string valor)
        {
            Boolean bresult = false;
            decimal number3;
            bresult = decimal.TryParse(valor, out number3);

            return bresult;
        }

        #endregion


        // Punto de Entrega

        /// <summary>
        /// Permite listar todos los registros de la tabla RNT_REG_PUNTO_ENTREGA
        /// </summary>
        public List<RntRegPuntoEntregaDTO> ListRntRegPuntoEntregas(int? EmpresaGeneradora, int Periodo, int Cliente, int PEntrega)
        {
            return FactoryTransferencia.GetRntRegPuntoEntregaRepository().List(EmpresaGeneradora, Periodo, Cliente, PEntrega);
        }

        /// <summary>
        /// Permite obtener un registro de la tabla RNT_REG_PUNTO_ENTREGA
        /// </summary>
        public RntRegPuntoEntregaDTO GetByIdRntRegPuntoEntrega(int regpuntoentcodi)
        {
            return FactoryTransferencia.GetRntRegPuntoEntregaRepository().GetById(regpuntoentcodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla RNT_REG_PUNTO_ENTREGA
        /// </summary>
        public List<RntRegPuntoEntregaDTO> ListPaginadoRntRegPuntoEntregas(int? EmpresaGeneradora, int Periodo, int Cliente, int PEntrega, int NroPaginado, int PageSize)
        {
            return FactoryTransferencia.GetRntRegPuntoEntregaRepository().ListPaginado(EmpresaGeneradora, Periodo, Cliente, PEntrega, NroPaginado, PageSize);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla RNT_REG_PUNTO_ENTREGA
        /// </summary>
        public List<RntRegPuntoEntregaDTO> ListReporteRntRegPuntoEntregas(int? EmpresaGeneradora, int Periodo, int Cliente, int PEntrega)
        {
            return FactoryTransferencia.GetRntRegPuntoEntregaRepository().ListReporte(EmpresaGeneradora, Periodo, Cliente, PEntrega);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla RNT_REG_PUNTO_ENTREGA
        /// </summary>
        public List<RntRegPuntoEntregaDTO> ListReportePaginadoRntRegPuntoEntregas(int? EmpresaGeneradora, int Periodo, int Cliente, int PEntrega, int NroPaginado, int PageSize)
        {
            return FactoryTransferencia.GetRntRegPuntoEntregaRepository().ListReportePaginado(EmpresaGeneradora, Periodo, Cliente, PEntrega, NroPaginado, PageSize);
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla SI_EMPRESA
        /// </summary>
        public SiEmpresaDTO GetByIdSiEmpresa(int key)
        {
            return FactorySic.GetSiEmpresaRepository().GetById(key);
        }

        /// <summary>
        /// Permite listar todas las barras que fueron registrados en RntRegPuntoEntrega
        /// </summary>
        public List<RntRegPuntoEntregaDTO> ListAllPuntoEntrega()
        {
            return FactoryTransferencia.GetRntRegPuntoEntregaRepository().ListAllPuntoEntrega();
        }

        /// <summary>
        /// Permite listar todas las barras que fueron registrados en RntRegPuntoEntrega
        /// </summary>
        public List<RntRegPuntoEntregaDTO> ListAllClientePE()
        {
            return FactoryTransferencia.GetRntRegPuntoEntregaRepository().ListAllClientePE();
        }

        /// <summary>
        /// Permite listar todas las barras que fueron registrados en RntRegPuntoEntrega
        /// </summary>
        public List<RntRegPuntoEntregaDTO> ListChangeClientePE(int idCliente)
        {
            return FactoryTransferencia.GetRntRegPuntoEntregaRepository().ListChangeClientePE(idCliente);
        }

    }
}
