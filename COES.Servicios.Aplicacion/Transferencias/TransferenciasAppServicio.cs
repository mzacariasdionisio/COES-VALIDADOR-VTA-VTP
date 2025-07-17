using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Transferencias;
using COES.Base.Core;
using COES.Servicios.Aplicacion.Factory;
using COES.Servicios.Aplicacion.Helper;
using log4net;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COES.Servicios.Aplicacion.Mediciones;
using COES.Dominio.DTO.Sic;
using COES.Servicios.Aplicacion.Mediciones.Helper;
using OfficeOpenXml.Style;
using System.Net;
using OfficeOpenXml.Drawing;
using OfficeOpenXml;
using System.Globalization;
using System.Drawing;
using System.IO;
using System.Data;
using System.Data.Common;
using COES.Servicios.Aplicacion.TransfPotencia;
using System.Configuration;
using COES.Framework.Base.Tools;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;

namespace COES.Servicios.Aplicacion.Transferencias
{
    /// <summary>
    /// Clases con mÃ©todos del mÃ³dulo Transferencias
    /// </summary>
    public class TransferenciasAppServicio : AppServicioBase
    {
        /// <summary>
        /// Instancia para el manejo de logs
        /// </summary>
        private static readonly ILog Logger = LogManager.GetLogger(typeof(TransferenciasAppServicio));

        //Instancias
        ConsultaMedidoresAppServicio servicioConsultaMedidores = new ConsultaMedidoresAppServicio();
        CentralGeneracionAppServicio servicioCentral = new CentralGeneracionAppServicio();
        PeriodoAppServicio servicioPeriodo = new PeriodoAppServicio();
        RecalculoAppServicio servicioRecalculo = new RecalculoAppServicio();
        EmpresaAppServicio servicioEmpresa = new EmpresaAppServicio();
        CodigoRetiroAppServicio servicioCodigoRetiro = new CodigoRetiroAppServicio();
        TransfPotenciaAppServicio servicioTransfPotencia = new TransfPotenciaAppServicio();
        TransferenciaInformacionBaseAppServicio servicioBase = new TransferenciaInformacionBaseAppServicio();

        #region 202001 - Tabla TRN_ENVIO
        /// <summary>
        /// Inserta un registro de la tabla TRN_ENVIO
        /// </summary>
        /// <param name="entity">Objeto de registro Envio</param>
        /// <returns>Entero</returns>
        public int SaveTrnEnvio(TrnEnvioDTO entity)
        {
            int id = 0;
            try
            {
                id = FactoryTransferencia.GetTrnEnvioRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
            return id;
        }

        /// <summary>
        /// Actualiza un registro de la tabla TRN_ENVIO
        /// </summary>
        /// <param name="entity">Objeto de registro Envio</param>
        /// <returns>Entero</returns>
        public int UpdateTrnEnvio(TrnEnvioDTO entity)
        {
            try
            {
                return FactoryTransferencia.GetTrnEnvioRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla TRN_ENVIO
        /// </summary>
        /// <param name="trnenvcodi">Id Envio</param>        
        /// <returns>Entero</returns>
        public int DeleteTrnEnvio(int trnenvcodi)
        {
            try
            {
                return FactoryTransferencia.GetTrnEnvioRepository().Delete(trnenvcodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla TRN_ENVIO
        /// </summary>
        /// <param name="trnenvcodi">Id Envio</param>        
        public TrnEnvioDTO GetByIdTrnEnvio(int trnenvcodi)
        {
            return FactoryTransferencia.GetTrnEnvioRepository().GetById(trnenvcodi);
        }

        /// <summary>
        /// Permite listar todos los envios de la tabla TRN_ENVIO, por periodo, recalculo, empresa y tipoinformacion
        /// </summary>        
        /// <returns>Lista de TrnEnvioDTO</returns>
        public List<TrnEnvioDTO> ListarTrnEnvio(int pericodi, int recacodi, int emprcodi, string tipoinfocodi, int trnmodcodi)
        {
            return FactoryTransferencia.GetTrnEnvioRepository().List(pericodi, recacodi, emprcodi, tipoinfocodi, trnmodcodi);
        }

        /// <summary>
        /// Permite listar todos los envios de la tabla TRN_ENVIO, por periodo, recalculo, empresa, tipoinformacion, trnenvplazo, trnenvliqvt
        /// </summary>        
        /// <param name="pericodi">Id de Periodo</param> 
        /// <param name="recacodi">Id de Recalculo</param> 
        /// <param name="emprcodi">Id de Empresa</param>
        /// <param name="tipoinfocodi">Tipo de InformaciÃ³n reportada: ER: Entregas y Retiros, IB: InformciÃ³n base, DM: Datos de modelo.</param> 
        /// <param name="trnenvplazo">S:Esta en plazo, N:Caso contrario</param> 
        /// <param name="trnenvliqvt">S: Si ingresa a la liquidaciÃ³n / N: caso contrario (valor por defecto).</param> 
        /// <returns>Lista de TrnEnvioDTO</returns>
        public List<TrnEnvioDTO> ListarTrnEnvioIntranet(int pericodi, int recacodi, int emprcodi, string tipoinfocodi, string trnenvplazo, string trnenvliqvt)
        {
            return FactoryTransferencia.GetTrnEnvioRepository().ListIntranet(pericodi, recacodi, emprcodi, tipoinfocodi, trnenvplazo, trnenvliqvt);
        }

        /// <summary>
        /// Lista los envios realizados en la tabla Trn_potenciaContratada_envio
        /// </summary>
        /// <param name="periCodi"></param>
        /// <returns></returns>
        public List<TrnPotenciaContratadaDTO> ListarEnvios(int periCodi)
        {
            return FactoryTransferencia.GetTrnPotenciaContratadaRepository().GetEnvios(periCodi);
        }

        /// <summary>
        /// Permite listar todos los envios en base al Id de la empresa y Periodo
        /// </summary>
        /// <param name="emprcodi">Id de Empresa</param>
        /// <param name="pericodi">Id de Periodo</param> 
        /// <returns>Lista de TrnEnvioDTO</returns>
        public List<TrnEnvioDTO> ConsultarTrnEnvio(int emprcodi, int pericodi)
        {
            return FactoryTransferencia.GetTrnEnvioRepository().GetByCriteria(emprcodi, pericodi);
        }

        /// <summary>
        /// Permite obtener un registro - el mas reciente - de la tabla TRN_ENVIO, por periodo, recalculo y empresa
        /// </summary>
        /// <param name="pericodi">Id de Periodo</param> 
        /// <param name="recacodi">Id de recalculo</param> 
        /// <param name="emprcodi">Id de Empresa</param>
        /// <param name="trnenvtipinf">Tipo de iformaciÃ³n ER / IB / DM</param> 
        /// <param name="trnmodcodi">Tipo de iformaciÃ³n DM --> Identificador del modelo</param> 
        /// <returns>Lista de TrnEnvioDTO</returns>
        public TrnEnvioDTO GetByTrnEnvioIdPeriodoEmpresa(int pericodi, int recacodi, int emprcodi, string trnenvtipinf, int trnmodcodi)
        {
            return FactoryTransferencia.GetTrnEnvioRepository().GetByIdPeriodoEmpresa(pericodi, recacodi, emprcodi, trnenvtipinf, trnmodcodi);
        }

        /// <summary>
        /// Permite actualizar el estado de los envios anteriores en TRN_ENVIO
        /// </summary>
        public int UpdateByCriteriaTrnEnvio(int pericodi, int recacodi, int emprcodi, int trnmodcodi, string trnenvtipinf, string suser)
        {
            return FactoryTransferencia.GetTrnEnvioRepository().UpdateByCriteriaTrnEnvio(pericodi, recacodi, emprcodi, trnmodcodi, trnenvtipinf, suser);
        }

        /// <summary>
        /// Permite que las entregas y retiros relacionados al dtoTnrEnvio (trnenvcodi, pericodi, recacodi, emprcodi) ingresen a la liquidaciÃ³n y coloca en inactivo a los demas envios (pericodi, recacodi, emprcodi)
        /// </summary>
        /// <param name="dtoTrnEnvio">TrnEnvioDTO</param> 
        /// <returns></returns>
        public void UpdateTrnEnvioLiquidacion(TrnEnvioDTO dtoTrnEnvio)
        {
            try
            {
                FactoryTransferencia.GetTrnEnvioRepository().UpdateTrnEnvioLiquidacion(dtoTrnEnvio);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite que las entregas y retiros relacionados al dtoTnrEnvio (trnenvcodi, pericodi, recacodi, emprcodi) ingresen a la liquidaciÃ³n y coloca en inactivo a los demas envios (pericodi, recacodi, emprcodi)
        /// </summary>
        /// <param name="dtoEntrega">ExportExcelDTO</param> 
        /// <returns></returns>
        public void UpdateEntregaLiquidacion(ExportExcelDTO dtoEntrega)
        {
            try
            {
                FactoryTransferencia.GetTrnEnvioRepository().UpdateEntregaLiquidacion(dtoEntrega);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite que las entregas y retiros relacionados al dtoTnrEnvio (trnenvcodi, pericodi, recacodi, emprcodi) ingresen a la liquidaciÃ³n y coloca en inactivo a los demas envios (pericodi, recacodi, emprcodi)
        /// </summary>
        /// <param name="dtoRetiro">ExportExcelDTO</param> 
        /// /// <param name="trnenvcodi">Id Envios</param>
        /// <param name="trnmodcodi">Id Modelos</param>
        /// <param name="suser">Usuario que registra el cambio</param>
        /// <returns></returns>
        public void UpdateRetiroLiquidacion(ExportExcelDTO dtoRetiro, int trnenvcodi, int trnmodcodi, string suser)
        {
            try
            {
                FactoryTransferencia.GetTrnEnvioRepository().UpdateRetiroLiquidacion(dtoRetiro, trnenvcodi, trnmodcodi, suser);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }
        #endregion

        #region 202001 - Tabla TRN_MODELO
        /// <summary>
        /// Inserta un registro de la tabla TRN_MODELO
        /// </summary>
        /// <param name="entity">Objeto de registro Modelo</param>
        /// <returns>Nada</returns>
        public int SaveTrnModelo(TrnModeloDTO entity)
        {
            return FactoryTransferencia.GetTrnModeloRepository().SaveTrnModelo(entity);
        }

        /// <summary>
        /// Actualiza un registro de la tabla TRN_MODELO
        /// </summary>
        /// <param name="entity">Objeto de registro Modelo</param>
        /// <returns>Nada</returns>
        public int UpdateTrnModelo(TrnModeloDTO entity)
        {
            return FactoryTransferencia.GetTrnModeloRepository().UpdateTrnModelo(entity);
        }

        /// <summary>
        /// Elimina un registro de la tabla TRN_MODELO
        /// </summary>
        /// <param name="idModelo">Id Modelo</param>        
        /// <returns>Nada</returns>
        public int DeleteTrnModelo(int idModelo)
        {
            return FactoryTransferencia.GetTrnModeloRepository().DeleteTrnModelo(idModelo);
        }

        /// <summary>
        /// Permite listar todos los modelos de la tabla TRN_MODELO
        /// </summary>        
        /// <returns>Lista de TrnModeloRetiroDTO</returns>
        public List<TrnModeloDTO> ListarTrnModelo()
        {
            return FactoryTransferencia.GetTrnModeloRepository().ListTrnModelo();
        }

        /// <summary>
        /// Permite listar los modelos por empresa de la tabla TRN_MODELO
        /// </summary>        
        /// <returns>Lista de TrnModeloRetiroDTO</returns>
        public List<TrnModeloDTO> ListarTrnModeloByEmpresa(int emprcodi)
        {
            return FactoryTransferencia.GetTrnModeloRepository().ListTrnModeloByEmpresa(emprcodi);
        }
        #endregion

        #region Metodos Adicionales Trn_Modelo
        /// <summary>
        /// Permite listar el combo de los modelos
        /// </summary>        
        /// <returns>Combo de Modelos</returns>       
        public List<TrnModeloDTO> ListarComboTrnModelo()
        {
            return FactoryTransferencia.GetTrnModeloRepository().ListarComboTrnModelo();
        }
        #endregion

        #region Metodos Basicos Trn_Modelo_Retiro
        /// <summary>
        /// Inserta un registro de la tabla TRN_MODELO_RETIRO
        /// </summary>
        /// <param name="entity">Objeto de registro Modelo</param>
        /// <returns>Nada</returns>
        public int SaveTrnModeloRetiro(TrnModeloRetiroDTO entity)
        {
            return FactoryTransferencia.GetTrnModeloRepository().SaveTrnModeloRetiro(entity);
        }

        /// <summary>
        /// Actualiza un registro de la tabla TRN_MODELO_RETIRO
        /// </summary>
        /// <param name="entity">Objeto de registro Modelo</param>
        /// <returns>Nada</returns>
        public int UpdateTrnModeloRetiro(TrnModeloRetiroDTO entity)
        {
            return FactoryTransferencia.GetTrnModeloRepository().UpdateTrnModeloRetiro(entity);
        }

        /// <summary>
        /// Elimina un registro de la tabla TRN_MODELO_RETIRO
        /// </summary>
        /// <param name="idModeloRetiro">Id Modelo Retiro</param>        
        /// <returns>Nada</returns>
        public int DeleteTrnModeloRetiro(int idModeloRetiro)
        {
            return FactoryTransferencia.GetTrnModeloRepository().DeleteTrnModeloRetiro(idModeloRetiro);
        }

        /// <summary>
        /// Permite listar todos los modelos de la tabla TRN_MODELO_RETIRO
        /// </summary>        
        /// <returns>Lista de TrnModeloRetiroDTO</returns>
        public List<TrnModeloRetiroDTO> ListarTrnModeloRetiro(int idModelo)
        {
            return FactoryTransferencia.GetTrnModeloRepository().ListTrnModeloRetiro(idModelo);
        }
        #endregion

        #region Metodos Adicionales Trn_Modelo_Retiro
        /// <summary>
        /// Permite listar el combo de Barras
        /// </summary>        
        /// <returns>Combo de Barras</returns>       
        public List<BarraDTO> ListarComboBarras()
        {
            return FactoryTransferencia.GetTrnModeloRepository().ListarComboBarras();
        }

        /// <summary>
        /// Permite listar el combo de codigos de solicitud de retiro
        /// </summary>        
        /// <param name="idBarra">Id de Barra</param>
        /// <returns>Combo de Codigos de Retiro</returns>       
        public List<CodigoRetiroDTO> ListComboCodigoSolicitudRetiro(int idBarra)
        {
            return FactoryTransferencia.GetTrnModeloRepository().ListComboCodigoSolicitudRetiro(idBarra);
        }

        /// <summary>
        /// Permite verificar si un modelo seleccionado tiene codigos de retiro
        /// </summary>        
        /// <param name="idModelo">Id de Modelo</param>
        /// <returns>Valor boleano</returns> 
        public bool TieneCodigosRetiroModelo(int idModelo)
        {
            bool rspta = false;
            rspta = FactoryTransferencia.GetTrnModeloRepository().TieneCodigosRetiroModelo(idModelo);
            return rspta;
        }
        #endregion

        #region 202001 - Tabla TRN_POTENCIA_CONTRATADA

        /// <summary>
        /// Lee el archivo Excel modificado y subido previamente al directorio especificado y obtiene los datos para
        /// su proceso e inserciÃ³n en la tabla TRN_PONTENCIA_CONTRATADA
        /// </summary>
        /// <param name="idEmpresa">Id empresa</param>
        /// <param name="idPeriodo">Id Periodo</param>
        /// <param name="usuario">Usuario</param>
        /// <param name="path">Ruta del archivo Excel</param>
        /// <param name="fileName">nombre del archivo Excel</param>
        /// <returns>Listado de registros de Potencias Contratadas</returns>
        public List<TrnPotenciaContratadaDTO> LeerArchivoModificadoPotenciasContratadas(int idEmpresa, int idPeriodo, string usuario, string path, string fileName)
        {
            List<TrnPotenciaContratadaDTO> entitys = new List<TrnPotenciaContratadaDTO>();
            try
            {
                TrnPotenciaContratadaDTO entity;

                FileInfo fi = new FileInfo(path + fileName);
                var package = new ExcelPackage(fi);
                ExcelWorksheet workSheet = package.Workbook.Worksheets["PotenciaContratada"];
                if (workSheet != null)
                {
                    for (var row = 7; row <= workSheet.Dimension.End.Row; row++)
                    {
                        if (workSheet.Cells[row, 2].Value == null)
                        {
                            break;
                        }
                        entity = new TrnPotenciaContratadaDTO();
                        entity.TrnPctCodi = 0;
                        entity.PeriCodi = idPeriodo;
                        entity.EmprCodi = idEmpresa;
                        entity.CoresoCodi = Convert.ToInt32(workSheet.Cells[row, 2].Value.ToString());
                        //ASSETEC 20200805
                        if (entity.CoresoCodi == 0)
                        {
                            string sCodigo = workSheet.Cells[row, 3].Value.ToString();
                            CodigoRetiroDTO dtoCodigoRetiro = this.servicioCodigoRetiro.GetCodigoRetiroByCodigo(sCodigo);
                            if (dtoCodigoRetiro != null)
                            {
                                entity.CoresoCodi = dtoCodigoRetiro.SoliCodiRetiCodi;
                            }
                        }
                        //----------------------------------------/
                        if (workSheet.Cells[row, 11].Value != null) entity.TrnPctPtoSumins = workSheet.Cells[row, 11].Value.ToString();
                        else entity.TrnPctPtoSumins = null;

                        if (workSheet.Cells[row, 12].Value != null) entity.TrnPctTotalMwFija = ValidarNumero(workSheet.Cells[row, 12].Value.ToString());
                        else entity.TrnPctTotalMwFija = 0;
                        if (workSheet.Cells[row, 13].Value != null) entity.TrnPctHpMwFija = ValidarNumero(workSheet.Cells[row, 13].Value.ToString());
                        else entity.TrnPctHpMwFija = 0;
                        if (workSheet.Cells[row, 14].Value != null) entity.TrnPctHfpMwFija = ValidarNumero(workSheet.Cells[row, 14].Value.ToString());
                        else entity.TrnPctHfpMwFija = 0;
                        if (workSheet.Cells[row, 15].Value != null) entity.TrnPctTotalMwVariable = ValidarNumero(workSheet.Cells[row, 15].Value.ToString());
                        else entity.TrnPctTotalMwVariable = 0;
                        if (workSheet.Cells[row, 16].Value != null) entity.TrnPctHpMwFijaVariable = ValidarNumero(workSheet.Cells[row, 16].Value.ToString());
                        else entity.TrnPctHpMwFijaVariable = 0;
                        if (workSheet.Cells[row, 17].Value != null) entity.TrnPctHfpMwFijaVariable = ValidarNumero(workSheet.Cells[row, 17].Value.ToString());
                        else entity.TrnPctHfpMwFijaVariable = 0;
                        if (workSheet.Cells[row, 18].Value != null)
                            entity.TrnPctComeObs = workSheet.Cells[row, 18].Value.ToString();
                        else
                            entity.TrnPctComeObs = null;

                        entitys.Add(entity);
                    }
                }
                // Check if the file exists
                if (fi.Exists)
                {
                    fi.Delete();
                    fi = new FileInfo(path + fileName);
                }
            }
            catch (Exception e)
            {
                string smsg = e.Message;
            }

            return entitys;
        }


        /// <summary>
        /// Lee la hoja de calculo wev y obtiene los datos para su proceso e inserciÃ³n en la tabla TRN_PONTENCIA_CONTRATADA
        /// </summary>
        /// <param name="idEmpresa">Id empresa</param>
        /// <param name="idPeriodo">Id Periodo</param>
        /// <param name="usuario">Usuario</param>
        /// <param name="path">Ruta del archivo Excel</param>
        /// <param name="datos">Datos de la hoja de calculo web</param>
        /// <returns>Listado de registros de Potencias Contratadas</returns>
        public List<TrnPotenciaContratadaDTO> LeerHojaCalculoWebPotenciasContratadas(int idEmpresa, int idPeriodo, string usuario, List<string[]> datos)
        {
            List<TrnPotenciaContratadaDTO> entitys = new List<TrnPotenciaContratadaDTO>();
            try
            {
                int iAux = 0;
                foreach (string[] aFila in datos)
                {
                    if (iAux < 2)
                    {
                        iAux++;
                        continue;
                    }
                    TrnPotenciaContratadaDTO entity = new TrnPotenciaContratadaDTO();
                    entity.TrnPctCodi = 0;
                    entity.PeriCodi = idPeriodo;
                    entity.EmprCodi = idEmpresa;
                    string sCodigo = aFila[1].ToString();
                    CodigoRetiroDTO dtoCodigoRetiro = this.servicioCodigoRetiro.GetCodigoRetiroByCodigo(sCodigo);
                    if (dtoCodigoRetiro != null)
                    {
                        entity.CoresoCodi = dtoCodigoRetiro.SoliCodiRetiCodi;
                        if (aFila[9] != null)
                            entity.TrnPctPtoSumins = aFila[9].ToString();
                        entity.TrnPctTotalMwFija = ValidarNumero(aFila[10].ToString());
                        entity.TrnPctHpMwFija = ValidarNumero(aFila[11].ToString());
                        entity.TrnPctHfpMwFija = ValidarNumero(aFila[12].ToString());
                        entity.TrnPctTotalMwVariable = ValidarNumero(aFila[13].ToString());
                        entity.TrnPctHpMwFijaVariable = ValidarNumero(aFila[14].ToString());
                        entity.TrnPctHfpMwFijaVariable = ValidarNumero(aFila[15].ToString());
                        if (aFila[16] != null)
                            entity.TrnPctComeObs = aFila[16].ToString();
                        entitys.Add(entity);
                    }
                }
            }
            catch (Exception e)
            {
                string smsg = e.Message;
            }

            return entitys;
        }
        /// <summary>
        /// Permite registrar en la tabla TRN_PONTENCIA_CONTRATADA
        /// </summary>
        /// <param name="entitys">Lista de entidades de TrnPotenciaContratadaDTO</param>
        /// <param name="usuario">Usuario</param>
        /// <returns>Retorna el POCOTRCODI nuevo</returns>
        public int SavePotenciasContratadas(List<TrnPotenciaContratadaDTO> entitys, string usuario)
        {
            int id = -1;
            int iNumRegistros = 0;
            IDbConnection conn = null;
            DbTransaction tran = null;

            try
            {
                conn = FactorySic.GetInIntervencionRepository().BeginConnection();
                tran = FactorySic.GetInIntervencionRepository().StartTransaction(conn);

                foreach (TrnPotenciaContratadaDTO entity in entitys)
                {
                    id = FactoryTransferencia.GetTrnPotenciaContratadaRepository().Save(entity, usuario, conn, tran, id);
                    iNumRegistros++;
                }

                if (id != -1)
                {
                    tran.Commit();
                }
                else
                {
                    tran.Rollback();
                    throw new Exception("Error en inserciÃ³n de los registros!...");
                }

            }
            catch (Exception ex)
            {
                if (tran != null) tran.Rollback();
                throw new Exception(ex.Message, ex);
            }
            finally
            {
                if (conn != null)
                {
                    if (conn.State == ConnectionState.Open) conn.Close();
                }
            }

            return iNumRegistros;
        }

        /// <summary>
        /// Permite listar todos las potencias contratadas a partir de las tablas TRN_CODIGO_RETIRO_SOLICITUD
        /// y TRN_PONTENCIA_CONTRATADA
        /// en base al Id de la empresa
        /// </summary>
        /// <param name="idEmpresa">Id de Empresa</param>
        /// <param name="idPeriodo">Id de Periodo</param> 
        /// <param name="idCliente">Id de Empresa</param> 
        /// <param name="idBarra">Id de Barra</param> 
        /// <returns>Lista de TrnPotenciaContratadaDTO</returns>
        public List<TrnPotenciaContratadaDTO> ListarPotenciasContratadas(int idEmpresa, int idPeriodo, int idCliente = 0, int idBarra = 0)
        {
            return FactoryTransferencia.GetTrnPotenciaContratadaRepository().GetByCriteria(idEmpresa, idPeriodo, idCliente, idBarra);
        }

        /// <summary>
        /// Permite generar la plantilla en formato excel de las potencias contratadas
        /// </summary>
        /// <param name="ListaCodigoRetiro">List.CodigoRetiroDTO</param> 
        /// <param name="pericodi">Id de Periodo</param> 
        /// <param name="emprcodi">Id de Empresa</param>
        /// <param name="formato">formato</param> 
        /// <param name="pathFile">path File</param> 
        /// <param name="pathLogo">path Logo</param> 
        /// <returns>Lista de TrnPotenciaContratadaDTO</returns>
        public string GenerarFormatoPotenciaContratada(List<CodigoRetiroDTO> ListaCodigoRetiro, int pericodi, int emprcodi, int formato, string pathFile, string pathLogo)
        {
            try
            {
                PeriodoDTO dtoPeriodo = servicioPeriodo.GetByIdPeriodo(pericodi);
                Dominio.DTO.Transferencias.EmpresaDTO dtoEmpresa = servicioEmpresa.GetByIdEmpresa(emprcodi);

                string sPrefijoExcel = dtoEmpresa.EmprNombre + "_" + dtoPeriodo.PeriAnioMes + ".";

                string fileName = sPrefijoExcel + "PotenciaContratada.xlsx";

                FileInfo newFile = new FileInfo(pathFile + fileName);

                if (newFile.Exists)
                {
                    newFile.Delete();
                    newFile = new FileInfo(pathFile + fileName);
                }

                using (ExcelPackage xlPackage = new ExcelPackage(newFile))
                {
                    ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add("PotenciaContratada");
                    if (ws != null)
                    {
                        //TITULO
                        ws.Cells[2, 4].Value = "INFORMACION DE EMPRESAS GENERADORAS SOBRE LAS POTENCIAS CONTRATADAS CON SUS CLIENTES - " + dtoEmpresa.EmprNombre + " - " + dtoPeriodo.PeriNombre;
                        ExcelRange rg = ws.Cells[2, 4, 2, 4];
                        rg.Style.Font.Size = 16;
                        rg.Style.Font.Bold = true;
                        //CABECERA DE TABLA
                        //Primera Fila
                        ws.Cells[5, 2].Value = "Datos de los Contratos Vigentes";
                        //rg = ws.Cells[5, 2, 5, 8];
                        ws.Cells[5, 2, 5, 8].Merge = true;

                        ws.Cells[5, 9].Value = "Vigencia del Contrato";
                        //rg = ws.Cells[5, 9, 5, 10];
                        ws.Cells[5, 9, 5, 10].Merge = true;

                        ws.Cells[5, 11].Value = "Punto(s) de Suministro (3)";
                        //rg = ws.Cells[5, 9, 5, 10];
                        ws.Cells[5, 11, 6, 11].Merge = true;
                        ws.Cells[5, 11, 6, 11].Style.WrapText = true;

                        ws.Cells[5, 12].Value = "Datos de la Potencia Contratada Fija [MW]"; rg = ws.Cells[5, 9, 5, 10];
                        ws.Cells[5, 12, 5, 14].Merge = true;

                        ws.Cells[5, 15].Value = "Datos de la Potencia Contratada  Variable [MW]";
                        //rg = ws.Cells[5, 15, 5, 17];
                        ws.Cells[5, 15, 5, 17].Merge = true;

                        ws.Cells[5, 18].Value = "Comentario / ObservaciÃ³n";
                        //rg = ws.Cells[5, 18, 6, 18];
                        ws.Cells[5, 18, 6, 18].Merge = true;
                        ws.Cells[5, 18, 6, 18].Style.WrapText = true;

                        //Segunda Fila
                        ws.Cells[6, 2].Value = "ID";
                        ws.Column(2).Hidden = true; //Ocultamos la columnas
                        ws.Cells[6, 3].Value = "CÃ³digo de Retiro";
                        ws.Cells[6, 4].Value = "Suministrador";
                        ws.Cells[6, 5].Value = "Cliente (1)";
                        ws.Cells[6, 6].Value = "Tipo de Contrato";
                        ws.Cells[6, 7].Value = "Tipo de Usuario";
                        ws.Cells[6, 8].Value = "Barra de Transferencia (2)";

                        ws.Cells[6, 9].Value = "Fecha Inicio";
                        ws.Column(9).Style.Numberformat.Format = "dd/mm/yyyy";
                        ws.Cells[6, 10].Value = "Fecha Fin";
                        ws.Column(10).Style.Numberformat.Format = "dd/mm/yyyy";

                        ws.Cells[6, 12].Value = "Total [MW]";
                        ws.Column(12).Style.Numberformat.Format = "#,##0.00";
                        ws.Cells[6, 13].Value = "H.P. [MW]";
                        ws.Column(13).Style.Numberformat.Format = "#,##0.00";
                        ws.Cells[6, 14].Value = "H.F.P. [MW]";
                        ws.Column(14).Style.Numberformat.Format = "#,##0.00";
                        ws.Cells[6, 15].Value = "Total [MW]";
                        ws.Column(15).Style.Numberformat.Format = "#,##0.00";
                        ws.Cells[6, 16].Value = "H.P. [MW]";
                        ws.Column(16).Style.Numberformat.Format = "#,##0.00";
                        ws.Cells[6, 17].Value = "H.F.P. [MW]";
                        ws.Column(17).Style.Numberformat.Format = "#,##0.00";
                        rg = ws.Cells[5, 2, 6, 18];
                        rg.Style.WrapText = true;
                        rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                        rg.Style.Font.Color.SetColor(Color.White);
                        rg.Style.Font.Size = 11;
                        rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Left.Color.SetColor(Color.Black);
                        rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Right.Color.SetColor(Color.Black);
                        rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Top.Color.SetColor(Color.Black);
                        rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Bottom.Color.SetColor(Color.Black);
                        rg.Style.Font.Bold = true;

                        int row = 7;
                        foreach (var item in ListaCodigoRetiro)
                        {
                            ws.Cells[row, 2].Value = item.SoliCodiRetiCodi;
                            ws.Cells[row, 3].Value = item.SoliCodiRetiCodigo;
                            ws.Cells[row, 4].Value = item.EmprNombre;
                            ws.Cells[row, 5].Value = item.CliNombre;
                            ws.Cells[row, 6].Value = item.TipoContNombre;
                            ws.Cells[row, 7].Value = item.TipoUsuaNombre;
                            ws.Cells[row, 8].Value = item.BarrNombBarrTran;
                            ws.Cells[row, 9].Value = item.SoliCodiRetiFechaInicio;
                            ws.Cells[row, 10].Value = item.SoliCodiRetiFechaFin;
                            //Border por celda
                            rg = ws.Cells[row, 2, row, 18];
                            rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                            rg.Style.Border.Left.Color.SetColor(Color.Black);
                            rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                            rg.Style.Border.Right.Color.SetColor(Color.Black);
                            rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                            rg.Style.Border.Top.Color.SetColor(Color.Black);
                            rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                            rg.Style.Border.Bottom.Color.SetColor(Color.Black);
                            rg.Style.Font.Size = 10;
                            rg.Style.WrapText = true;
                            row++;
                        }
                        //Fijar panel
                        ws.View.FreezePanes(7, 11);
                        //Ajustar columnas
                        ws.Column(3).Width = 22;
                        ws.Column(4).Width = 15;
                        ws.Column(5).Width = 50;
                        ws.Column(6).Width = 15;
                        ws.Column(7).Width = 15;
                        ws.Column(8).Width = 25;
                        ws.Column(9).Width = 15;
                        ws.Column(10).Width = 15;
                        ws.Column(11).Width = 20;
                        ws.Column(12).Width = 15;
                        ws.Column(13).Width = 15;
                        ws.Column(14).Width = 15;
                        ws.Column(15).Width = 15;
                        ws.Column(16).Width = 15;
                        ws.Column(17).Width = 15;
                        ws.Column(18).Width = 50;
                        //Insertar el logo
                        HttpWebRequest request = (HttpWebRequest)System.Net.HttpWebRequest.Create(ConfigurationManager.AppSettings["LogoCoes"].ToString());
                        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                        System.Drawing.Image img = System.Drawing.Image.FromStream(response.GetResponseStream());
                        ExcelPicture picture = ws.Drawings.AddPicture("ff", img);
                        picture.From.Column = 1;
                        picture.From.Row = 1;
                        picture.To.Column = 2;
                        picture.To.Row = 2;
                        picture.SetSize(120, 60);
                    }
                    xlPackage.Save();
                }
                return fileName;
            }
            catch (Exception e)
            {
                return e.Message;//("-1");
            }
        }

        /// <summary>
        /// Permite generar el archivo excel de las potencias contratadas en la intranet
        /// </summary>
        /// <param name="ListaPotenciasContratadas">ListaPotenciasContratadas</param> 
        /// <param name="pericodi">Id de Periodo</param> 
        /// <param name="emprcodi">Id de Empresa</param>
        /// <param name="formato">formato</param> 
        /// <param name="pathFile">path File</param> 
        /// <param name="pathLogo">path Logo</param> 
        /// <returns>Lista de TrnPotenciaContratadaDTO</returns>
        public string ExportarPotenciaContratada(List<TrnPotenciaContratadaDTO> ListaPotenciasContratadas, int pericodi, int emprcodi, int formato, string pathFile, string pathLogo)
        {
            try
            {
                PeriodoDTO dtoPeriodo = servicioPeriodo.GetByIdPeriodo(pericodi);
                Dominio.DTO.Transferencias.EmpresaDTO dtoEmpresa = servicioEmpresa.GetByIdEmpresa(emprcodi);

                string sPrefijoExcel = dtoEmpresa.EmprNombre + "_" + dtoPeriodo.PeriAnioMes + ".";

                string fileName = sPrefijoExcel + "PotenciaContratada.xlsx";

                FileInfo newFile = new FileInfo(pathFile + fileName);

                if (newFile.Exists)
                {
                    newFile.Delete();
                    newFile = new FileInfo(pathFile + fileName);
                }

                using (ExcelPackage xlPackage = new ExcelPackage(newFile))
                {
                    ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add("PotenciaContratada");
                    if (ws != null)
                    {
                        //TITULO
                        ws.Cells[2, 4].Value = "INFORMACION DE EMPRESAS GENERADORAS SOBRE LAS POTENCIAS CONTRATADAS CON SUS CLIENTES - " + dtoEmpresa.EmprNombre + " - " + dtoPeriodo.PeriNombre;
                        ExcelRange rg = ws.Cells[2, 4, 2, 4];
                        rg.Style.Font.Size = 16;
                        rg.Style.Font.Bold = true;
                        //CABECERA DE TABLA
                        //Primera Fila
                        ws.Cells[5, 2].Value = "Datos de los Contratos Vigentes";
                        //rg = ws.Cells[5, 2, 5, 8];
                        ws.Cells[5, 2, 5, 8].Merge = true;

                        ws.Cells[5, 9].Value = "Vigencia del Contrato";
                        //rg = ws.Cells[5, 9, 5, 10];
                        ws.Cells[5, 9, 5, 10].Merge = true;

                        ws.Cells[5, 11].Value = "Punto(s) de Suministro (3)";
                        //rg = ws.Cells[5, 9, 5, 10];
                        ws.Cells[5, 11, 6, 11].Merge = true;
                        ws.Cells[5, 11, 6, 11].Style.WrapText = true;

                        ws.Cells[5, 12].Value = "Datos de la Potencia Contratada Fija [MW]"; rg = ws.Cells[5, 9, 5, 10];
                        ws.Cells[5, 12, 5, 14].Merge = true;

                        ws.Cells[5, 15].Value = "Datos de la Potencia Contratada  Variable [MW]";
                        //rg = ws.Cells[5, 15, 5, 17];
                        ws.Cells[5, 15, 5, 17].Merge = true;

                        ws.Cells[5, 18].Value = "Comentario / ObservaciÃ³n";
                        //rg = ws.Cells[5, 18, 6, 18];
                        ws.Cells[5, 18, 6, 18].Merge = true;
                        ws.Cells[5, 18, 6, 18].Style.WrapText = true;

                        //Segunda Fila
                        ws.Cells[6, 2].Value = "ID";
                        /* ws.Column(2).Hidden = true; //Ocultamos la columnas ASSETEC 20200805 */
                        ws.Cells[6, 3].Value = "CÃ³digo de Retiro";
                        ws.Cells[6, 4].Value = "Suministrador";
                        ws.Cells[6, 5].Value = "Cliente (1)";
                        ws.Cells[6, 6].Value = "Tipo de Contrato";
                        ws.Cells[6, 7].Value = "Tipo de Usuario";
                        ws.Cells[6, 8].Value = "Barra de Transferencia (2)";

                        ws.Cells[6, 9].Value = "Fecha Inicio";
                        ws.Column(9).Style.Numberformat.Format = "dd/mm/yyyy";
                        ws.Cells[6, 10].Value = "Fecha Fin";
                        ws.Column(10).Style.Numberformat.Format = "dd/mm/yyyy";

                        ws.Cells[6, 12].Value = "Total [MW]";
                        ws.Column(12).Style.Numberformat.Format = "#,##0.00";
                        ws.Cells[6, 13].Value = "H.P. [MW]";
                        ws.Column(13).Style.Numberformat.Format = "#,##0.00";
                        ws.Cells[6, 14].Value = "H.F.P. [MW]";
                        ws.Column(14).Style.Numberformat.Format = "#,##0.00";
                        ws.Cells[6, 15].Value = "Total [MW]";
                        ws.Column(15).Style.Numberformat.Format = "#,##0.00";
                        ws.Cells[6, 16].Value = "H.P. [MW]";
                        ws.Column(16).Style.Numberformat.Format = "#,##0.00";
                        ws.Cells[6, 17].Value = "H.F.P. [MW]";
                        ws.Column(17).Style.Numberformat.Format = "#,##0.00";
                        rg = ws.Cells[5, 2, 6, 18];
                        rg.Style.WrapText = true;
                        rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                        rg.Style.Font.Color.SetColor(Color.White);
                        rg.Style.Font.Size = 11;
                        rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Left.Color.SetColor(Color.Black);
                        rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Right.Color.SetColor(Color.Black);
                        rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Top.Color.SetColor(Color.Black);
                        rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Bottom.Color.SetColor(Color.Black);
                        rg.Style.Font.Bold = true;

                        int row = 7;
                        foreach (var item in ListaPotenciasContratadas)
                        {
                            ws.Cells[row, 2].Value = item.CoresoCodi;
                            ws.Cells[row, 3].Value = item.CoresoCodigo;
                            ws.Cells[row, 4].Value = item.EmprNomb;
                            ws.Cells[row, 5].Value = item.CliNombre;
                            ws.Cells[row, 6].Value = item.TipConNombre;
                            ws.Cells[row, 7].Value = item.TipUsuNombre;
                            ws.Cells[row, 8].Value = item.BarrBarraTransferencia;
                            ws.Cells[row, 9].Value = item.CoresoFechaInicio.ToString("dd/MM/yyyy");
                            ws.Cells[row, 10].Value = item.CoresoFechaFin.ToString("dd/MM/yyyy");

                            ws.Cells[row, 11].Value = item.TrnPctPtoSumins;
                            ws.Cells[row, 12].Value = item.TrnPctTotalMwFija;
                            ws.Cells[row, 13].Value = item.TrnPctHpMwFija;
                            ws.Cells[row, 14].Value = item.TrnPctHfpMwFija;
                            ws.Cells[row, 15].Value = item.TrnPctTotalMwVariable;
                            ws.Cells[row, 16].Value = item.TrnPctHpMwFijaVariable;
                            ws.Cells[row, 17].Value = item.TrnPctHfpMwFijaVariable;
                            ws.Cells[row, 18].Value = item.TrnPctComeObs;
                            //Border por celda
                            rg = ws.Cells[row, 2, row, 18];
                            rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                            rg.Style.Border.Left.Color.SetColor(Color.Black);
                            rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                            rg.Style.Border.Right.Color.SetColor(Color.Black);
                            rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                            rg.Style.Border.Top.Color.SetColor(Color.Black);
                            rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                            rg.Style.Border.Bottom.Color.SetColor(Color.Black);
                            rg.Style.Font.Size = 10;
                            rg.Style.WrapText = true;
                            row++;
                        }
                        //Fijar panel
                        ws.View.FreezePanes(7, 11);
                        //Ajustar columnas
                        ws.Column(3).Width = 22;
                        ws.Column(4).Width = 15;
                        ws.Column(5).Width = 50;
                        ws.Column(6).Width = 15;
                        ws.Column(7).Width = 15;
                        ws.Column(8).Width = 25;
                        ws.Column(9).Width = 15;
                        ws.Column(10).Width = 15;
                        ws.Column(11).Width = 20;
                        ws.Column(12).Width = 15;
                        ws.Column(13).Width = 15;
                        ws.Column(14).Width = 15;
                        ws.Column(15).Width = 15;
                        ws.Column(16).Width = 15;
                        ws.Column(17).Width = 15;
                        ws.Column(18).Width = 50;
                        //Insertar el logo
                        HttpWebRequest request = (HttpWebRequest)System.Net.HttpWebRequest.Create(ConfigurationManager.AppSettings["LogoCoes"].ToString());
                        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                        System.Drawing.Image img = System.Drawing.Image.FromStream(response.GetResponseStream());
                        ExcelPicture picture = ws.Drawings.AddPicture("ff", img);
                        picture.From.Column = 1;
                        picture.From.Row = 1;
                        picture.To.Column = 2;
                        picture.To.Row = 2;
                        picture.SetSize(120, 60);
                    }
                    xlPackage.Save();
                }
                return fileName;
            }
            catch (Exception e)
            {
                return e.Message;//("-1");
            }
        }

        /// <summary>
        /// Permite eliminar los registros de potencias contratada por empresa y periodo
        /// </summary>
        /// <param name="pericodi">Id de Periodo</param> 
        /// <param name="emprcodi">Id de Empresa</param>
        /// <returns>void</returns>
        public void DeleteTrnPotenciaContratada(int pericodi, int emprcodi)
        {
            {
                try
                {
                    FactoryTransferencia.GetTrnPotenciaContratadaRepository().Delete(pericodi, emprcodi);
                }
                catch (Exception ex)
                {
                    Logger.Error(ConstantesAppServicio.LogError, ex);
                    throw new Exception(ex.Message, ex);
                }
            }
        }

        /// <summary>
        /// Valida que la informaciÃ³n ingresada sea un numero valido, caso contrario devuelve cero
        /// /// </summary>
        /// <param name="sValor">Cadena de texto</param>
        public static decimal ValidarNumero(string sValor)
        {
            decimal dNumero;
            if (!sValor.Equals("") && (Decimal.TryParse(sValor, out dNumero)))
            {
                return dNumero;
            }
            else
            {
                return 0;
            }
        }

        //assetec 20200604
        /// <summary>
        /// Permite copiar todos las potencias contratadas de un periodoAnterior al periodoActual en TRN_PONTENCIA_CONTRATADA
        /// </summary>
        /// <param name="idPeriodoActual">Id de Periodo Actual</param> 
        /// <param name="idPeriodoAnterior">Id de Periodo Anterior</param> 
        public void CopiarPotenciasContratadasByPeriodo(int idPeriodoActual, int idPeriodoAnterior, string sUser)
        {
            try
            {
                FactoryTransferencia.GetTrnPotenciaContratadaRepository().CopiarPotenciasContratadasByPeriodo(idPeriodoActual, idPeriodoAnterior, sUser);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite borrar todos las potencias contratadas de un periodo en TRN_PONTENCIA_CONTRATADA
        /// </summary>
        /// <param name="pericodi">Id de Periodo Actual</param> 
        public void DeleteTrnPotenciaContratadaByPeriodo(int pericodi)
        {
            {
                try
                {
                    FactoryTransferencia.GetTrnPotenciaContratadaRepository().DeleteByPeriodo(pericodi);
                }
                catch (Exception ex)
                {
                    Logger.Error(ConstantesAppServicio.LogError, ex);
                    throw new Exception(ex.Message, ex);
                }
            }
        }
        #endregion

        #region - Reportes en Excel de VTEA

        /// <summary>
        /// Permite generar el archivo de exportaciÃ³n de los CÃ³digos de Entregas y Retiros
        /// </summary>
        /// <param name="pericodi">CÃ³digo del Mes de valorizaciÃ³n</param>
        /// <param name="recacodi">CÃ³digo del RecÃ¡lculo</param>
        /// <param name="emprcodi">CÃ³digo de la empresa</param>
        /// <param name="trnenvcodi">CÃ³digo de envio</param>
        /// <param name="formato">Excel=1 / PDF=2 / Word=3</param>
        /// <param name="pathFile">Rutal del archivo de descarga</param>
        /// <param name="pathLogo">Ruta del Logo del COES</param>
        /// <returns></returns>
        public string GenerarFormatoEntregaRetiro(int pericodi, int recacodi, int emprcodi, int trnenvcodi, int formato, string pathFile, string pathLogo, int option=0)
        {
            try
            {
                PeriodoDTO dtoPeriodo = servicioPeriodo.GetByIdPeriodo(pericodi);
                RecalculoDTO dtoRecalculo = servicioRecalculo.GetByIdRecalculo(pericodi, recacodi);
                Dominio.DTO.Transferencias.EmpresaDTO dtoEmpresa = servicioEmpresa.GetByIdEmpresa(emprcodi);

                string sPrefijoExcel = dtoEmpresa.EmprNombre + "_" + dtoPeriodo.PeriAnioMes + "_" + dtoRecalculo.RecaCodi + ".";

                List<ExportExcelDTO> ListaEntregReti;

                if (option == 0)
                {
                    ListaEntregReti = (new ExportarExcelGAppServicio()).BuscarCodigoRetiro(pericodi, emprcodi);
                }
                else
                {
                    ListaEntregReti = (new ExportarExcelGAppServicio()).BuscarCodigoRetiroVistaTodo(pericodi, emprcodi, 0);
                }
                //Buacamos todos los codigos de entrega y retiro que estan validos para el periodo seleccionado

                string fileName = sPrefijoExcel + "ReporteEntregaRetiro.xlsx";

                FileInfo newFile = new FileInfo(pathFile + fileName);

                if (newFile.Exists)
                {
                    newFile.Delete();
                    newFile = new FileInfo(pathFile + fileName);
                }

                using (ExcelPackage xlPackage = new ExcelPackage(newFile))
                {
                    ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add("REPORTE");
                    if (ws != null)
                    {
                        //TITULO
                        ws.Cells[2, 3].Value = "LISTA DE CÃ“DIGOS DE ENTREGA Y RETIRO DE " + dtoEmpresa.EmprNombre + " - " + dtoPeriodo.PeriNombre + "/" + dtoRecalculo.RecaNombre;
                        ExcelRange rg = ws.Cells[2, 3, 2, 3];
                        rg.Style.Font.Size = 16;
                        rg.Style.Font.Bold = true;
                        //CABECERA DE TABLA
                        ws.Cells[5, 2].Value = "EMPRESA";
                        ws.Cells[5, 3].Value = "BARRA TRANSFERENCIA";
                        ws.Cells[5, 4].Value = "CODIGO";
                        ws.Cells[5, 5].Value = "ENTREGA/RETIRO";
                        ws.Cells[5, 6].Value = "CENTRAL/CLIENTE";

                        rg = ws.Cells[5, 2, 5, 6];
                        rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                        rg.Style.Font.Color.SetColor(Color.White);
                        rg.Style.Font.Size = 10;
                        rg.Style.Font.Bold = true;

                        int row = 6;
                        foreach (var item in ListaEntregReti)
                        {
                            ws.Cells[row, 2].Value = item.EmprNomb;
                            ws.Cells[row, 3].Value = item.BarrNombBarrTran;
                            ws.Cells[row, 4].Value = item.CodiEntreRetiCodigo;
                            ws.Cells[row, 5].Value = item.Tipo;
                            ws.Cells[row, 6].Value = item.CentGeneCliNombre;
                            //Border por celda
                            rg = ws.Cells[row, 2, row, 6];
                            rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                            rg.Style.Border.Left.Color.SetColor(Color.Black);
                            rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                            rg.Style.Border.Right.Color.SetColor(Color.Black);
                            rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                            rg.Style.Border.Top.Color.SetColor(Color.Black);
                            rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                            rg.Style.Border.Bottom.Color.SetColor(Color.Black);
                            rg.Style.Font.Size = 10;
                            row++;
                        }
                        //Fijar panel
                        ws.View.FreezePanes(6, 7);
                        //Ajustar columnas
                        rg = ws.Cells[5, 2, row, 6];
                        rg.AutoFitColumns();

                        //Insertar el logo
                        HttpWebRequest request = (HttpWebRequest)System.Net.HttpWebRequest.Create(ConfigurationManager.AppSettings["LogoCoes"].ToString());
                        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                        System.Drawing.Image img = System.Drawing.Image.FromStream(response.GetResponseStream());
                        ExcelPicture picture = ws.Drawings.AddPicture("ff", img);
                        picture.From.Column = 1;
                        picture.From.Row = 1;
                        picture.To.Column = 2;
                        picture.To.Row = 2;
                        picture.SetSize(120, 60);
                        //*************************************************************************************************************************************
                        ExcelWorksheet ws2 = xlPackage.Workbook.Worksheets.Add("DATOS");
                        ws2.Cells[2, 1].Value = "VERSION DE DECLARACION";
                        int colum = 2;
                        foreach (ExportExcelDTO dto in ListaEntregReti)
                        {
                            string sCodigo = dto.CodiEntreRetiCodigo;
                            ws2.Cells[1, colum].Value = sCodigo;
                            ws2.Cells[2, colum].Value = option == 0 ? "MWh" : dto.BarrNombBarrTran;
                            ws2.Column(colum).Style.Numberformat.Format = "#,##0.0000";
                            if (dto.Tipo.Equals("Entrega"))
                            {
                                TransferenciaEntregaDTO EntidadEntrega = (new TransferenciaEntregaRetiroAppServicio()).GetTransferenciaEntregaByCodigoEnvio(emprcodi, pericodi, recacodi, trnenvcodi, dto.CodiEntreRetiCodigo);
                                if (EntidadEntrega != null)
                                {
                                    int iFilCodigo = 3;
                                    //traemos la lista de detalles del mes para Entregas y retiros
                                    List<TransferenciaEntregaDetalleDTO> ListaEntregaDetalle = (new TransferenciaEntregaRetiroAppServicio()).ListByTransferenciaEntrega(EntidadEntrega.TranEntrCodi, EntidadEntrega.TranEntrVersion);
                                    foreach (TransferenciaEntregaDetalleDTO dtoDetalle in ListaEntregaDetalle)
                                    {
                                        for (int k = 1; k <= 96; k++)
                                        {
                                            ws2.Cells[iFilCodigo++, colum].Value = dtoDetalle.GetType().GetProperty("TranEntrDetah" + k).GetValue(dtoDetalle, null);
                                        }
                                    }
                                }
                            }
                            else if (dto.Tipo.Equals("Retiro"))
                            {
                                TransferenciaRetiroDTO EntidadRetiro = (new TransferenciaEntregaRetiroAppServicio()).GetTransferenciaRetiroByCodigoEnvio(emprcodi, pericodi, recacodi, trnenvcodi, dto.CodiEntreRetiCodigo);
                                if (EntidadRetiro != null)
                                {
                                    int iFilCodigo = 3;
                                    //traemos la lista de detalles del mes para Entregas y retiros
                                    List<TransferenciaRetiroDetalleDTO> ListaRetiroDetalle = (new TransferenciaEntregaRetiroAppServicio()).ListByTransferenciaRetiro(EntidadRetiro.TranRetiCodi, EntidadRetiro.TranRetiVersion);
                                    foreach (TransferenciaRetiroDetalleDTO dtoDetalle in ListaRetiroDetalle)
                                    {
                                        for (int k = 1; k <= 96; k++)
                                        {
                                            ws2.Cells[iFilCodigo++, colum].Value = dtoDetalle.GetType().GetProperty("TranRetiDetah" + k).GetValue(dtoDetalle, null);
                                        }
                                    }
                                }
                            }
                            colum++;
                        }
                        //Color de fondo
                        rg = ws2.Cells[1, 1, 2, colum - 1];
                        rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        rg.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(54, 96, 146));
                        rg.Style.Font.Color.SetColor(System.Drawing.Color.FromArgb(255, 255, 255));
                        rg.Style.Font.Size = 10;
                        rg.Style.Font.Bold = true;

                        ////////////////////////////

                        string sMes = dtoPeriodo.MesCodi.ToString();
                        if (sMes.Length == 1) sMes = "0" + sMes;
                        var Fecha = "01/" + sMes + "/" + dtoPeriodo.AnioCodi;
                        var dates = new List<DateTime>();
                        var dateIni = DateTime.ParseExact(Fecha, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                        var dateFin = dateIni.AddMonths(1);
                        dateIni = dateIni.AddMinutes(15);
                        for (var dt = dateIni; dt <= dateFin; dt = dt.AddMinutes(15))
                        {
                            dates.Add(dt);
                        }

                        int row2 = 3;
                        foreach (var item in dates)
                        {
                            ws2.Cells[row2, 1].Value = item.ToString("dd/MM/yyyy HH:mm");
                            row2++;
                        }
                        rg = ws2.Cells[3, 1, row2 - 1, 1];
                        rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                        rg.Style.Font.Color.SetColor(Color.White);
                        rg.Style.Font.Size = 10;
                        rg.Style.Font.Bold = true;

                        //Border por celda
                        rg = ws2.Cells[1, 1, row2 - 1, colum - 1];
                        rg.AutoFitColumns();
                        rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Left.Color.SetColor(Color.Black);
                        rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Right.Color.SetColor(Color.Black);
                        rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Top.Color.SetColor(Color.Black);
                        rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Bottom.Color.SetColor(Color.Black);
                        rg.Style.Font.Size = 10;
                    }
                    xlPackage.Save();
                }
                return fileName;
            }
            catch (Exception e)
            {
                return e.Message;//("-1");
            }
        }

        /// <summary>
        /// Permite generar el archivo de exportaciÃ³n de los CÃ³digos de InformaciÃ³n Base
        /// </summary>
        /// <param name="pericodi">CÃ³digo del Mes de valorizaciÃ³n</param>
        /// <param name="recacodi">CÃ³digo del RecÃ¡lculo</param>
        /// <param name="emprcodi">CÃ³digo de la empresa</param>
        /// <param name="trnenvcodi">CÃ³digo de envio</param>
        /// <param name="formato">Excel=1 / PDF=2 / Word=3</param>
        /// <param name="pathFile">Rutal del archivo de descarga</param>
        /// <param name="pathLogo">Ruta del Logo del COES</param>
        /// <returns></returns>
        public string GenerarFormatoInfoBase(int pericodi, int recacodi, int emprcodi, int trnenvcodi, int formato, string pathFile, string pathLogo)
        {
            try
            {
                PeriodoDTO dtoPeriodo = servicioPeriodo.GetByIdPeriodo(pericodi);
                RecalculoDTO dtoRecalculo = servicioRecalculo.GetByIdRecalculo(pericodi, recacodi);
                Dominio.DTO.Transferencias.EmpresaDTO dtoEmpresa = servicioEmpresa.GetByIdEmpresa(emprcodi);

                string sPrefijoExcel = dtoEmpresa.EmprNombre + "_" + dtoPeriodo.PeriAnioMes + "_" + dtoRecalculo.RecaCodi + ".";

                //Buacamos todos los codigos de entrega y retiro que estan validos para el periodo seleccionado
                List<ExportExcelDTO> ListaEntregReti = (new ExportarExcelGAppServicio()).GetByListCodigoInfoBase(pericodi, emprcodi);

                string fileName = sPrefijoExcel + "ReporteInfoBase.xlsx";

                FileInfo newFile = new FileInfo(pathFile + fileName);

                if (newFile.Exists)
                {
                    newFile.Delete();
                    newFile = new FileInfo(pathFile + fileName);
                }

                using (ExcelPackage xlPackage = new ExcelPackage(newFile))
                {
                    ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add("REPORTE");
                    if (ws != null)
                    {
                        //TITULO
                        ws.Cells[2, 3].Value = "LISTA DE CÃ“DIGOS DE INFORMACIÃ“N BASE DE " + dtoEmpresa.EmprNombre + " - " + dtoPeriodo.PeriNombre + "/" + dtoRecalculo.RecaNombre;
                        ExcelRange rg = ws.Cells[2, 3, 2, 3];
                        rg.Style.Font.Size = 16;
                        rg.Style.Font.Bold = true;
                        //CABECERA DE TABLA
                        ws.Cells[5, 2].Value = "EMPRESA";
                        ws.Cells[5, 3].Value = "BARRA TRANSFERENCIA";
                        ws.Cells[5, 4].Value = "CODIGO";
                        ws.Cells[5, 5].Value = "TIPO";
                        ws.Cells[5, 6].Value = "CENTRAL/CLIENTE";

                        rg = ws.Cells[5, 2, 5, 6];
                        rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                        rg.Style.Font.Color.SetColor(Color.White);
                        rg.Style.Font.Size = 10;
                        rg.Style.Font.Bold = true;

                        int row = 6;
                        foreach (var item in ListaEntregReti)
                        {
                            ws.Cells[row, 2].Value = (item.EmprNomb != null) ? item.EmprNomb.ToString() : string.Empty;
                            ws.Cells[row, 3].Value = (item.BarrNombBarrTran != null) ? item.BarrNombBarrTran : string.Empty;
                            ws.Cells[row, 4].Value = (item.CoInfbCodigo != null) ? item.CoInfbCodigo : string.Empty;
                            ws.Cells[row, 5].Value = (item.Tipo != null) ? item.Tipo : string.Empty;
                            ws.Cells[row, 6].Value = (item.CentGeneCliNombre != null) ? item.CentGeneCliNombre : string.Empty;
                            //Border por celda
                            rg = ws.Cells[row, 2, row, 6];
                            rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                            rg.Style.Border.Left.Color.SetColor(Color.Black);
                            rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                            rg.Style.Border.Right.Color.SetColor(Color.Black);
                            rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                            rg.Style.Border.Top.Color.SetColor(Color.Black);
                            rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                            rg.Style.Border.Bottom.Color.SetColor(Color.Black);
                            rg.Style.Font.Size = 10;
                            row++;
                        }
                        //Fijar panel
                        ws.View.FreezePanes(6, 7);
                        //Ajustar columnas
                        rg = ws.Cells[5, 2, row, 6];
                        rg.AutoFitColumns();

                        //Insertar el logo
                        HttpWebRequest request = (HttpWebRequest)System.Net.HttpWebRequest.Create(ConfigurationManager.AppSettings["LogoCoes"].ToString());
                        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                        System.Drawing.Image img = System.Drawing.Image.FromStream(response.GetResponseStream());
                        ExcelPicture picture = ws.Drawings.AddPicture("ff", img);
                        picture.From.Column = 1;
                        picture.From.Row = 1;
                        picture.To.Column = 2;
                        picture.To.Row = 2;
                        picture.SetSize(120, 60);


                        ExcelWorksheet ws2 = xlPackage.Workbook.Worksheets.Add("DATOS");
                        ws2.Cells[2, 1].Value = "VERSION DE DECLARACION";
                        int colum = 2;
                        foreach (var item in ListaEntregReti)
                        {
                            string sCodigo = (item.CoInfbCodigo != null) ? item.CoInfbCodigo.ToString() : string.Empty;
                            ws2.Cells[1, colum].Value = sCodigo;
                            ws2.Cells[2, colum].Value = "MWh";
                            ws2.Column(colum).Style.Numberformat.Format = "#,##0.0000";
                            TransferenciaInformacionBaseDTO EntidadInformacionBase = (new TransferenciaInformacionBaseAppServicio()).GetTransferenciaInfoBaseByCodigoEnvio(emprcodi, pericodi, recacodi, trnenvcodi, sCodigo);
                            if (EntidadInformacionBase != null)
                            {
                                int iFilCodigo = 3;
                                //traemos la lista de detalles del mes para modelEI.EntidadInformacionBase.TinfbCodi
                                List<TransferenciaInformacionBaseDetalleDTO> ListaInformacionBaseDetalle = (new TransferenciaInformacionBaseAppServicio()).ListByTransferenciaInformacionBase(EntidadInformacionBase.TinfbCodi);
                                foreach (TransferenciaInformacionBaseDetalleDTO dtoDetalle in ListaInformacionBaseDetalle)
                                {
                                    for (int k = 1; k <= 96; k++)
                                    {
                                        ws2.Cells[iFilCodigo++, colum].Value = dtoDetalle.GetType().GetProperty("TinfbDe" + k).GetValue(dtoDetalle, null);
                                    }
                                }
                            }
                            colum++;
                        }
                        //Color de fondo
                        rg = ws2.Cells[1, 1, 2, colum - 1];
                        rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        rg.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(54, 96, 146));
                        rg.Style.Font.Color.SetColor(System.Drawing.Color.FromArgb(255, 255, 255));
                        rg.Style.Font.Size = 10;
                        rg.Style.Font.Bold = true;

                        ////////////////////////////

                        string sMes = dtoPeriodo.MesCodi.ToString();
                        if (sMes.Length == 1) sMes = "0" + sMes;
                        var Fecha = "01/" + sMes + "/" + dtoPeriodo.AnioCodi;
                        var dates = new List<DateTime>();
                        var dateIni = DateTime.ParseExact(Fecha, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                        var dateFin = dateIni.AddMonths(1);
                        dateIni = dateIni.AddMinutes(15);
                        for (var dt = dateIni; dt <= dateFin; dt = dt.AddMinutes(15))
                        {
                            dates.Add(dt);
                        }

                        int row2 = 3;
                        foreach (var item in dates)
                        {
                            ws2.Cells[row2, 1].Value = item.ToString("dd/MM/yyyy HH:mm");
                            row2++;
                        }
                        rg = ws2.Cells[3, 1, row2 - 1, 1];
                        rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                        rg.Style.Font.Color.SetColor(Color.White);
                        rg.Style.Font.Size = 10;
                        rg.Style.Font.Bold = true;

                        //Border por celda
                        rg = ws2.Cells[1, 1, row2 - 1, colum - 1];
                        rg.AutoFitColumns();
                        rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Left.Color.SetColor(Color.Black);
                        rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Right.Color.SetColor(Color.Black);
                        rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Top.Color.SetColor(Color.Black);
                        rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Bottom.Color.SetColor(Color.Black);
                        rg.Style.Font.Size = 10;
                    }
                    xlPackage.Save();
                }
                return fileName;
            }
            catch (Exception e)
            {
                return e.Message;//("-1");
            }
        }

        /// <summary>
        /// Permite generar el archivo de exportaciÃ³n de los CÃ³digos de un Modelo
        /// </summary>
        /// <param name="pericodi">CÃ³digo del Mes de valorizaciÃ³n</param>
        /// <param name="recacodi">CÃ³digo del RecÃ¡lculo</param>
        /// <param name="emprcodi">CÃ³digo de la empresa</param>
        /// <param name="trnenvcodi">CÃ³digo de envio</param>
        /// <param name="trnmodcodi">CÃ³digo de modelo</param>
        /// <param name="formato">Excel=1 / PDF=2 / Word=3</param>
        /// <param name="pathFile">Rutal del archivo de descarga</param>
        /// <param name="pathLogo">Ruta del Logo del COES</param>
        /// <returns></returns>
        public string GenerarFormatoModelo(int pericodi, int recacodi, int emprcodi, int trnenvcodi, int trnmodcodi, int formato, string pathFile, string pathLogo)
        {
            try
            {
                PeriodoDTO dtoPeriodo = servicioPeriodo.GetByIdPeriodo(pericodi);
                RecalculoDTO dtoRecalculo = servicioRecalculo.GetByIdRecalculo(pericodi, recacodi);
                //Empresa MODELO
                Dominio.DTO.Transferencias.EmpresaDTO dtoEmpresa = servicioEmpresa.GetByIdEmpresa(emprcodi);

                string sPrefijoExcel = dtoEmpresa.EmprNombre + "_" + dtoPeriodo.PeriAnioMes + "_" + dtoRecalculo.RecaCodi + ".";

                //Buacamos todos los codigos de entrega y retiro que estan validos para el periodo seleccionado
                List<ExportExcelDTO> ListaModelo = (new ExportarExcelGAppServicio()).GetByListCodigoModeloVTA(pericodi, emprcodi, trnmodcodi);

                string fileName = sPrefijoExcel + "ReporteDatosModelo.xlsx";

                FileInfo newFile = new FileInfo(pathFile + fileName);

                if (newFile.Exists)
                {
                    newFile.Delete();
                    newFile = new FileInfo(pathFile + fileName);
                }

                using (ExcelPackage xlPackage = new ExcelPackage(newFile))
                {
                    ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add("REPORTE");
                    if (ws != null)
                    {
                        //TITULO
                        ws.Cells[2, 3].Value = "LISTA DE CÃ“DIGOS DE DATOS DE MODELO DE " + dtoEmpresa.EmprNombre + " - " + dtoPeriodo.PeriNombre + "/" + dtoRecalculo.RecaNombre;
                        ExcelRange rg = ws.Cells[2, 3, 2, 3];
                        rg.Style.Font.Size = 16;
                        rg.Style.Font.Bold = true;
                        //CABECERA DE TABLA
                        ws.Cells[5, 2].Value = "EMPRESA";
                        ws.Cells[5, 3].Value = "BARRA TRANSFERENCIA";
                        ws.Cells[5, 4].Value = "CODIGO";
                        ws.Cells[5, 5].Value = "ENTREGA/RETIRO";
                        ws.Cells[5, 6].Value = "CENTRAL/CLIENTE";

                        rg = ws.Cells[5, 2, 5, 6];
                        rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                        rg.Style.Font.Color.SetColor(Color.White);
                        rg.Style.Font.Size = 10;
                        rg.Style.Font.Bold = true;

                        int row = 6;
                        foreach (var item in ListaModelo)
                        {
                            ws.Cells[row, 2].Value = item.EmprNomb;
                            ws.Cells[row, 3].Value = item.BarrNombBarrTran;
                            ws.Cells[row, 4].Value = item.CodiEntreRetiCodigo;
                            ws.Cells[row, 5].Value = item.Tipo;
                            ws.Cells[row, 6].Value = item.CentGeneCliNombre;
                            //Border por celda
                            rg = ws.Cells[row, 2, row, 6];
                            rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                            rg.Style.Border.Left.Color.SetColor(Color.Black);
                            rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                            rg.Style.Border.Right.Color.SetColor(Color.Black);
                            rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                            rg.Style.Border.Top.Color.SetColor(Color.Black);
                            rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                            rg.Style.Border.Bottom.Color.SetColor(Color.Black);
                            rg.Style.Font.Size = 10;
                            row++;
                        }
                        //Fijar panel
                        ws.View.FreezePanes(6, 7);
                        //Ajustar columnas
                        rg = ws.Cells[5, 2, row, 6];
                        rg.AutoFitColumns();

                        //Insertar el logo
                        HttpWebRequest request = (HttpWebRequest)System.Net.HttpWebRequest.Create(ConfigurationManager.AppSettings["LogoCoes"].ToString());
                        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                        System.Drawing.Image img = System.Drawing.Image.FromStream(response.GetResponseStream());
                        ExcelPicture picture = ws.Drawings.AddPicture("ff", img);
                        picture.From.Column = 1;
                        picture.From.Row = 1;
                        picture.To.Column = 2;
                        picture.To.Row = 2;
                        picture.SetSize(120, 60);
                        //*************************************************************************************************************************************
                        ExcelWorksheet ws2 = xlPackage.Workbook.Worksheets.Add("DATOS");
                        ws2.Cells[2, 1].Value = "VERSION DE DECLARACION";
                        int colum = 2;
                        foreach (ExportExcelDTO dto in ListaModelo)
                        {
                            string sCodigo = dto.CodiEntreRetiCodigo;
                            ws2.Cells[1, colum].Value = sCodigo;
                            ws2.Cells[2, colum].Value = "MWh";
                            ws2.Column(colum).Style.Numberformat.Format = "#,##0.0000";
                            TransferenciaRetiroDTO EntidadRetiro = (new TransferenciaEntregaRetiroAppServicio()).GetTransferenciaRetiroByCodigoEnvio(dto.EmprCodi, pericodi, recacodi, trnenvcodi, dto.CodiEntreRetiCodigo);
                            if (EntidadRetiro != null)
                            {
                                int iFilCodigo = 3;
                                //traemos la lista de detalles del mes para Entregas y retiros
                                List<TransferenciaRetiroDetalleDTO> ListaRetiroDetalle = (new TransferenciaEntregaRetiroAppServicio()).ListByTransferenciaRetiro(EntidadRetiro.TranRetiCodi, EntidadRetiro.TranRetiVersion);
                                foreach (TransferenciaRetiroDetalleDTO dtoDetalle in ListaRetiroDetalle)
                                {
                                    for (int k = 1; k <= 96; k++)
                                    {
                                        ws2.Cells[iFilCodigo++, colum].Value = dtoDetalle.GetType().GetProperty("TranRetiDetah" + k).GetValue(dtoDetalle, null);
                                    }
                                }
                            }
                            colum++;
                        }
                        //Color de fondo
                        rg = ws2.Cells[1, 1, 2, colum - 1];
                        rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        rg.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(54, 96, 146));
                        rg.Style.Font.Color.SetColor(System.Drawing.Color.FromArgb(255, 255, 255));
                        rg.Style.Font.Size = 10;
                        rg.Style.Font.Bold = true;

                        ////////////////////////////

                        string sMes = dtoPeriodo.MesCodi.ToString();
                        if (sMes.Length == 1) sMes = "0" + sMes;
                        var Fecha = "01/" + sMes + "/" + dtoPeriodo.AnioCodi;
                        var dates = new List<DateTime>();
                        var dateIni = DateTime.ParseExact(Fecha, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                        var dateFin = dateIni.AddMonths(1);
                        dateIni = dateIni.AddMinutes(15);
                        for (var dt = dateIni; dt <= dateFin; dt = dt.AddMinutes(15))
                        {
                            dates.Add(dt);
                        }

                        int row2 = 3;
                        foreach (var item in dates)
                        {
                            ws2.Cells[row2, 1].Value = item.ToString("dd/MM/yyyy HH:mm");
                            row2++;
                        }
                        rg = ws2.Cells[3, 1, row2 - 1, 1];
                        rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                        rg.Style.Font.Color.SetColor(Color.White);
                        rg.Style.Font.Size = 10;
                        rg.Style.Font.Bold = true;

                        //Border por celda
                        rg = ws2.Cells[1, 1, row2 - 1, colum - 1];
                        rg.AutoFitColumns();
                        rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Left.Color.SetColor(Color.Black);
                        rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Right.Color.SetColor(Color.Black);
                        rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Top.Color.SetColor(Color.Black);
                        rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Bottom.Color.SetColor(Color.Black);
                        rg.Style.Font.Size = 10;
                    }
                    xlPackage.Save();
                }
                return fileName;
            }
            catch (Exception e)
            {
                return e.Message;//("-1");
            }
        }


        #endregion

        #region MÃ©todos Tabla TRN_FACTOR_PERDIDA_MEDIA

        /// <summary>
        /// Inserta un registro de la tabla TRN_FACTOR_PERDIDA_MEDIA
        /// </summary>
        public void SaveTrnFactorPerdidaMedia(TrnFactorPerdidaMediaDTO entity)
        {
            try
            {
                FactoryTransferencia.GetTrnFactorPerdidaMediaRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla TRN_FACTOR_PERDIDA_MEDIA
        /// </summary>
        public void UpdateTrnFactorPerdidaMedia(TrnFactorPerdidaMediaDTO entity)
        {
            try
            {
                FactoryTransferencia.GetTrnFactorPerdidaMediaRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla TRN_FACTOR_PERDIDA_MEDIA
        /// </summary>
        public void DeleteTrnFactorPerdidaMedia(int pericodi, int version)
        {
            try
            {
                FactoryTransferencia.GetTrnFactorPerdidaMediaRepository().Delete(pericodi, version);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla TRN_FACTOR_PERDIDA_MEDIA
        /// </summary>
        public TrnFactorPerdidaMediaDTO GetByIdTrnFactorPerdidaMedia(int trnfpmcodi)
        {
            return FactoryTransferencia.GetTrnFactorPerdidaMediaRepository().GetById(trnfpmcodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla TRN_FACTOR_PERDIDA_MEDIA
        /// </summary>
        public List<TrnFactorPerdidaMediaDTO> ListTrnFactorPerdidaMedias(int pericodi, int version)
        {
            return FactoryTransferencia.GetTrnFactorPerdidaMediaRepository().List(pericodi, version);
        }

        /// <summary>
        /// Permite realizar bÃºsquedas en la tabla TrnFactorPerdidaMedia
        /// </summary>
        public List<TrnFactorPerdidaMediaDTO> GetByCriteriaTrnFactorPerdidaMedias()
        {
            return FactoryTransferencia.GetTrnFactorPerdidaMediaRepository().GetByCriteria();
        }

        //ASSETEC 20190104
        /// <summary>
        /// Permite Obtener el reporte de Desviaciones de Entregas
        /// </summary>
        public List<TrnFactorPerdidaMediaDTO> ListDesviacionesEntregas(int pericodi, int version)
        {
            return FactoryTransferencia.GetTrnFactorPerdidaMediaRepository().ListDesviacionesEntregas(pericodi, version);
        }
        #endregion

        //ASSETEC 20190104
        #region MÃ©todos Tabla TRN_INFOADICIONAL

        /// <summary>
        /// Inserta un registro de la tabla TRN_INFOADICIONAL
        /// </summary>
        public void SaveTrnInfoadicional(TrnInfoadicionalDTO entity)
        {
            try
            {
                FactoryTransferencia.GetTrnInfoadicionalRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla TRN_INFOADICIONAL
        /// </summary>
        public void UpdateTrnInfoadicional(TrnInfoadicionalDTO entity)
        {
            try
            {
                FactoryTransferencia.GetTrnInfoadicionalRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla TRN_INFOADICIONAL
        /// </summary>
        public void DeleteTrnInfoadicional(int infadicodi)
        {
            try
            {
                FactoryTransferencia.GetTrnInfoadicionalRepository().Delete(infadicodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla TRN_INFOADICIONAL
        /// </summary>
        public TrnInfoadicionalDTO GetByIdTrnInfoadicional(int infadicodi)
        {
            return FactoryTransferencia.GetTrnInfoadicionalRepository().GetById(infadicodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla TRN_INFOADICIONAL
        /// </summary>
        public List<TrnInfoadicionalDTO> ListTrnInfoadicionals()
        {
            return FactoryTransferencia.GetTrnInfoadicionalRepository().List();
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla TRN_INFOADICIONAL
        /// </summary>
        public List<TrnInfoadicionalDTO> ListVersionTrnInfoadicionals(int id)
        {
            return FactoryTransferencia.GetTrnInfoadicionalRepository().ListVersiones(id);
        }

        /// <summary>
        /// Permite realizar bÃºsquedas en la tabla TrnInfoadicional
        /// </summary>
        public List<TrnInfoadicionalDTO> GetByCriteriaTrnInfoadicionals()
        {
            return FactoryTransferencia.GetTrnInfoadicionalRepository().GetByCriteria();
        }

        #endregion

        #region MÃ©todos Tabla TRN_MEDBORNE

        /// <summary>
        /// Inserta un registro de la tabla TRN_MEDBORNE
        /// </summary>
        public void SaveTrnMedborne(TrnMedborneDTO entity)
        {
            try
            {
                FactoryTransferencia.GetTrnMedborneRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla TRN_MEDBORNE
        /// </summary>
        public void UpdateTrnMedborne(TrnMedborneDTO entity)
        {
            try
            {
                FactoryTransferencia.GetTrnMedborneRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla TRN_MEDBORNE
        /// </summary>
        public void DeleteTrnMedborne(int pericodi, int version)
        {
            try
            {
                FactoryTransferencia.GetTrnMedborneRepository().Delete(pericodi, version);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla TRN_MEDBORNE
        /// </summary>
        public TrnMedborneDTO GetByIdTrnMedborne(int trnmebcodi)
        {
            return FactoryTransferencia.GetTrnMedborneRepository().GetById(trnmebcodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla TRN_MEDBORNE
        /// </summary>
        public List<TrnMedborneDTO> ListTrnMedbornes()
        {
            return FactoryTransferencia.GetTrnMedborneRepository().List();
        }

        /// <summary>
        /// Permite realizar bÃºsquedas en la tabla TrnMedborne
        /// </summary>
        public List<TrnMedborneDTO> GetByCriteriaTrnMedbornes()
        {
            return FactoryTransferencia.GetTrnMedborneRepository().GetByCriteria();
        }

        //ASSETEC 20190116
        /// <summary>
        /// Graba la informaciÃ³n de la base de datos de los Medidores de Bornes de GeneraciÃ³n
        /// </summary>
        public int GrabarMedidorBorne(int pericodi, int version, string sUser, int tipoInfoCodi, string empresas, int central, string tiposGeneracion, DateTime fecInicio, DateTime fecFin)
        {
            int iNumReg = 0;
            try
            {
                string tiposEmpresa = "1,2,3,4,5";
                string fuentes = servicioConsultaMedidores.GetFuenteSSAA(tiposGeneracion);
                if (tiposEmpresa != ConstantesAppServicio.ParametroDefecto && empresas == ConstantesAppServicio.ParametroDefecto)
                {
                    List<int> idsEmpresas = servicioConsultaMedidores.ObteneEmpresasPorTipo(tiposEmpresa).Select(x => x.Emprcodi).ToList();
                    empresas = string.Join<int>(ConstantesAppServicio.CaracterComa.ToString(), idsEmpresas);
                }

                int lectcodi = 1;
                List<MeMedicion96DTO> listActiva = FactorySic.GetMeMedicion96Repository().ObtenerExportacionConsultaMedidores(fecInicio, fecFin, central, tiposGeneracion, empresas
                                                    , ConstantesMedicion.IdFamiliaSSAA, ConstantesMedicion.GrupoNoIntegrante, lectcodi, tipoInfoCodi
                                                    , ConstantesMedicion.TipoGenerrer, ConstantesMedidores.TptoMedicodiTodos);

                foreach (MeMedicion96DTO item in listActiva)
                {
                    TrnMedborneDTO dtoMedborne = new TrnMedborneDTO();
                    dtoMedborne.Pericodi = pericodi;
                    dtoMedborne.Trnmebversion = version;
                    string sNombre;
                    if (item.Emprcodi == 0)
                        sNombre = item.Emprnomb;
                    else
                        dtoMedborne.Emprcodi = item.Emprcodi; //item.EmprNomb
                    if (item.Equipadre == 0)
                    {
                        sNombre = item.Central;
                        CentralGeneracionDTO dtoCentral = servicioCentral.GetByCentGeneNomb(sNombre);
                        if (dtoCentral != null)
                            dtoMedborne.Equicodi = dtoCentral.CentGeneCodi;
                        else
                            sNombre = "No existe";
                    }
                    else
                        dtoMedborne.Equicodi = item.Equipadre; //item.Central
                    dtoMedborne.Trnmebfecha = (DateTime)item.Medifecha;
                    dtoMedborne.Trnmebptomed = item.Ptomedicodi.ToString(); //Identificador de la tabla PtoMedicion
                    decimal dTrnMebEnergia = 0;
                    for (int k = 1; k <= 96; k++)
                    {
                        object resultado = item.GetType().GetProperty("H" + k).GetValue(item, null);
                        if (resultado != null)
                        {
                            dTrnMebEnergia += Convert.ToDecimal(resultado); //cada intervalo es potencia
                        }
                    }
                    dtoMedborne.Trnmebenergia = dTrnMebEnergia / 4; //(TOTAL ENERGIA ACTIVA  (MWh)
                    dtoMedborne.Trnmebusucreacion = sUser;
                    this.SaveTrnMedborne(dtoMedborne);
                    iNumReg++;
                }
                List<MeMedicion96DTO> listServiciosAuxiliares = FactorySic.GetMeMedicion96Repository().ObtenerExportacionServiciosAuxiliares(fecInicio, fecFin, central, fuentes, empresas
                                                , ConstantesMedicion.IdFamiliaSSAA, ConstantesMedicion.GrupoNoIntegrante, lectcodi, ConstantesMedicion.IdTipoInfoPotenciaActiva
                                                , ConstantesMedicion.TipoGenerrer, ConstantesMedidores.TptoMedicodiTodos);
                foreach (MeMedicion96DTO item in listServiciosAuxiliares)
                {
                    decimal dTrnMebEnergia = 0;
                    for (int k = 1; k <= 96; k++)
                    {
                        object resultado = item.GetType().GetProperty("H" + k).GetValue(item, null);
                        if (resultado != null)
                        {
                            dTrnMebEnergia += Convert.ToDecimal(resultado);
                        }
                    }
                    if (Math.Abs(dTrnMebEnergia) > 0)
                    {
                        TrnMedborneDTO dtoMedborne = new TrnMedborneDTO();
                        dtoMedborne.Pericodi = pericodi;
                        dtoMedborne.Trnmebversion = version;
                        string sNombre;
                        if (item.Emprcodi == 0)
                            sNombre = item.Emprnomb;
                        else
                            dtoMedborne.Emprcodi = item.Emprcodi; //item.EmprNomb
                        if (item.Equipadre == 0)
                        {
                            sNombre = item.Central;
                            CentralGeneracionDTO dtoCentral = servicioCentral.GetByCentGeneNomb(sNombre);
                            if (dtoCentral != null)
                                dtoMedborne.Equicodi = dtoCentral.CentGeneCodi;
                            else
                                sNombre = "No existe";
                        }
                        else
                            dtoMedborne.Equicodi = item.Equipadre; //item.Central
                        dtoMedborne.Trnmebfecha = (DateTime)item.Medifecha;
                        dtoMedborne.Trnmebptomed = item.Ptomedicodi.ToString(); //Identificador de la tabla PtoMedicion
                        dtoMedborne.Trnmebenergia = dTrnMebEnergia / 4; //Va a restar Mw de servicios servicios auxiliares
                        this.UpdateTrnMedborne(dtoMedborne);
                    }
                    iNumReg++;
                }
            }
            catch (Exception e)
            {
                return -1;
            }
            return iNumReg;
        }



        #endregion

        //ASSETEC 202108
        #region TIEE ejecuta el procedimiento de Proceso para Mercados

        /// <summary>
        /// Procedimiento que se encarga de Migrar el Proceso Mercado desde el módulo de TIEE
        /// </summary>
        /// <param name="migracodi">Identificador de la tabla Migracodi</param>
        /// <param name="migradescripcion">Descripción del proceso de Migracodi</param>
        /// <param name="emprcodiorigen">Identificador de la empresa origen</param>
        /// <param name="emprcodidestino">Identificador de la empresa destino</param>
        /// <param name="suser">Usuario conectado</param>
        /// <param name="sMensaje">Mensaje de error</param>
        /// <param name="sDetalle">Detalle de error</param>
        public string MigrarProcesoMercado(int migracodi, string migradescripcion, int emprcodiorigen, int emprcodidestino, string suser, out string sMensaje, out string sDetalle)
        {
            string sResultado = "1";
            sMensaje = "";
            sDetalle = "";
            string sSqlVTEA;
            string sSqlVTP;
            try
            {
                TrnMigracionDTO dtoTrnMigracion = new TrnMigracionDTO();
                dtoTrnMigracion.Migracodi = migracodi;
                dtoTrnMigracion.Trnmigdescripcion = migradescripcion;
                dtoTrnMigracion.Emprcodiorigen = emprcodiorigen;
                dtoTrnMigracion.Emprcodidestino = emprcodidestino;
                dtoTrnMigracion.Trnmigusucreacion = suser;

                //VTEA
                //Lista de Periodos con la ultima versión de recalculo  de VTEA
                List<RecalculoDTO> ListaRecalculosVTEA = this.servicioRecalculo.ListMaxRecalculoByPeriodo();
                
                //Lista de Periodos con la ultima versión de recalculo  de VTP
                List<VtpRecalculoPotenciaDTO> ListaRecalculosVTP = this.servicioTransfPotencia.ListMaxRecalculoByPeriodo();

                foreach (RecalculoDTO dtoRecalculoVTEA in ListaRecalculosVTEA)
                {
                    sSqlVTEA = "";
                    sSqlVTEA = this.servicioRecalculo.MigrarSaldosVTEA(emprcodiorigen, emprcodidestino, dtoRecalculoVTEA.RecaPeriCodi, dtoRecalculoVTEA.RecaCodi, out sMensaje, out sDetalle);
                    if (!sMensaje.Equals(""))
                    {
                        dtoTrnMigracion.Trnmigsql = sSqlVTEA + " " + sMensaje + " " + sDetalle;
                        dtoTrnMigracion.Trnmigestado = "X"; //X:Error
                        this.SaveTrnMigracion(dtoTrnMigracion);
                        return sResultado;
                    }
                    else
                    {
                        //Sin errores
                        dtoTrnMigracion.Trnmigsql = sSqlVTEA;
                        dtoTrnMigracion.Trnmigestado = "T"; //Estado del procedimiento: T: TIEE
                        this.SaveTrnMigracion(dtoTrnMigracion);
                    }
                }
                
                
                foreach (VtpRecalculoPotenciaDTO dtoRecalculoVTP in ListaRecalculosVTP)
                {
                    sSqlVTP = "";
                    sSqlVTP = this.servicioTransfPotencia.MigrarSaldosVTP(emprcodiorigen, emprcodidestino, dtoRecalculoVTP.Pericodi, dtoRecalculoVTP.Recpotcodi, out sMensaje, out sDetalle);
                    if (!sMensaje.Equals(""))
                    {
                        dtoTrnMigracion.Trnmigsql = sSqlVTP + " " + sMensaje;
                        dtoTrnMigracion.Trnmigestado = "X"; //X:Error
                        this.SaveTrnMigracion(dtoTrnMigracion);
                        return sResultado;
                    }
                    else
                    {
                        //Sin errores
                        dtoTrnMigracion.Trnmigsql = sSqlVTP;
                        dtoTrnMigracion.Trnmigestado = "T"; //Estado del procedimiento: T: TIEE
                        this.SaveTrnMigracion(dtoTrnMigracion);
                    }
                }
                
            }
            catch (Exception ex)
            {
                sMensaje = ex.Message;
                sDetalle = ex.StackTrace;
            }

            return sResultado;
        }

        #region Métodos Tabla TRN_MIGRACION

        /// <summary>
        /// Inserta un registro de la tabla TRN_MIGRACION
        /// </summary>
        public void SaveTrnMigracion(TrnMigracionDTO entity)
        {
            try
            {
                FactoryTransferencia.GetTrnMigracionRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla TRN_MIGRACION
        /// </summary>
        public void UpdateTrnMigracion(TrnMigracionDTO entity)
        {
            try
            {
                FactoryTransferencia.GetTrnMigracionRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla TRN_MIGRACION
        /// </summary>
        /// <param name="trnmigcodi">Identificador de la tabla TRN_MIGRACION</param>
        public void DeleteTrnMigracion(int trnmigcodi)
        {
            try
            {
                FactoryTransferencia.GetTrnMigracionRepository().Delete(trnmigcodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla TRN_MIGRACION
        /// </summary>
        /// <param name="trnmigcodi">Identificador de la tabla TRN_MIGRACION</param>
        public TrnMigracionDTO GetByIdTrnMigracion(int trnmigcodi)
        {
            return FactoryTransferencia.GetTrnMigracionRepository().GetById(trnmigcodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla TRN_MIGRACION
        /// </summary>
        public List<TrnMigracionDTO> ListTrnMigracions()
        {
            return FactoryTransferencia.GetTrnMigracionRepository().List();
        }
        public List<TrnMigracionDTO> ListTrnMigracionDTI()
        {
            return FactoryTransferencia.GetTrnMigracionRepository().ListMigracion();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla TrnMigracion
        /// </summary>
        public List<TrnMigracionDTO> GetByCriteriaTrnMigracions()
        {
            return FactoryTransferencia.GetTrnMigracionRepository().GetByCriteria();
        }

        #endregion

        #endregion


        /// <summary>
        /// Inserta un registro de la tabla TRN_CONFIGURACION_PMME
        /// </summary>
        /// <param name="entity">Objeto de registro Configuracion Pto PMME</param>
        /// <returns>Entero</returns>
        public int SaveTrnConfiguracionPmme(TrnConfiguracionPmmeDTO entity)
        {
            int id = 0;
            try
            {
                id = FactoryTransferencia.GetTrnConfiguracionPmmeRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
            return id;
        }

        /// <summary>
        /// Inserta un registro de la tabla TRN_CONFIGURACION_PMME
        /// </summary>
        /// <param name="entity">Objeto de registro Configuracion Pto PMME</param>
        /// <returns>Entero</returns>
        public int UpdateTrnConfiguracionPmme(TrnConfiguracionPmmeDTO entity)
        {
            int id = 0;
            try
            {
                FactoryTransferencia.GetTrnConfiguracionPmmeRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
            return id;
        }

        /// <summary>
        /// Cambia el estado a inactivo de la tabla TRN_CONFIGURACION_PMME
        /// </summary>
        /// <param name="confconcodi"></param>
        /// <param name="estado"></param>
        /// <returns>Entero</returns>
        public void DeleteTrnConfiguracionPmme(int confconcodi, string estado)
        {
            try
            {
                FactoryTransferencia.GetTrnConfiguracionPmmeRepository().Delete(confconcodi, estado);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite buscar las configuraciones segun criterios especificados
        /// </summary>
        /// <param name="emprcodi"></param>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <returns></returns>
        public List<TrnConfiguracionPmmeDTO> ListaConfiguracionPtosMME(int emprcodi, DateTime fechaInicio, DateTime fechaFin)
        {
            return FactoryTransferencia.GetTrnConfiguracionPmmeRepository().List(emprcodi, fechaInicio, fechaFin);
        }

        /// <summary>
        /// Verifica si existen registros con los datos ingresados
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="existeConfig"></param>
        /// <returns></returns>
        public void VerificarExistenciaConfiguracion(TrnConfiguracionPmmeDTO entity, out bool existeConfig)
        {
            existeConfig = FactoryTransferencia.GetTrnConfiguracionPmmeRepository().ValidarExistencia(entity.Emprcodi,entity.Ptomedicodi,entity.Vigencia);
        }

        /// <summary>
        /// Permite buscar las configuraciones por empresa
        /// </summary>
        /// <param name="emprcodi"></param>
        /// <returns></returns>
        public List<TrnConfiguracionPmmeDTO> ListaConfPtosMMExEmpresa(int emprcodi)
        {
            return FactoryTransferencia.GetTrnConfiguracionPmmeRepository().ListaConfPtosMMExEmpresa(emprcodi);
        }

        /// <summary>
        /// Permite obtener información de una configuración de pto mme
        /// </summary>
        /// <param name="confconcodi"></param>
        /// <returns></returns>
        public TrnConfiguracionPmmeDTO ObtenerConfPtosMME(int confconcodi)
        {
            return FactoryTransferencia.GetTrnConfiguracionPmmeRepository().GetById(confconcodi);
        }

        /// <summary>
        /// Inserta un registro de la tabla TRN_CONFIGURACION_PMME
        /// </summary>
        /// <param name="entity">Objeto de registro Configuracion Pto PMME</param>
        /// <returns>Entero</returns>
        public int SaveTrnDemanda(TrnDemandaDTO entity)
        {
            int id = 0;
            try
            {
                this.DeleteTrnDemanda(entity);
                id = FactoryTransferencia.GetTrnDemandaRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
            return id;
        }

        /// <summary>
        /// Permite buscar las configuraciones por empresa
        /// </summary>
        /// <param name="periodo"></param>
        /// <param name="emprcodi"></param>
        /// <returns></returns>
        public TrnDemandaDTO TrnDemandaxEmpresa(string periodo, int emprcodi)
        {
            return FactoryTransferencia.GetTrnDemandaRepository().ObtenerTrnDemanda(periodo,emprcodi);
        }

        /// <summary>
        /// Inserta un registro de la tabla TRN_CONFIGURACION_PMME
        /// </summary>
        /// <param name="entity"></param>
        /// <returns>Entero</returns>
        public void DeleteTrnDemanda(TrnDemandaDTO entity)
        {
            try
            {
                FactoryTransferencia.GetTrnDemandaRepository().Delete(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite listar los dias configurados para cálculo de cna
        /// </summary>
        /// <param name="semanaperiodo">Id de Empresa</param>
        /// <returns>Lista de TrnEnvioDTO</returns>
        public TrnPeriodoCnaDTO ObtenerSemana(string semanaperiodo)
        {
            return FactoryTransferencia.GetTrnPeriodoCnaRepository().ObtenerSemana(semanaperiodo);
        }

        /// <summary>
        /// Inserta un registro de la tabla Trn_Periodo_Cna
        /// </summary>
        /// <param name="entity">Objeto de registro Configuracion Pto PMME</param>
        /// <returns>Entero</returns>
        public int SaveTrnPeriodoCna(TrnPeriodoCnaDTO entity)
        {
            int id = 0;
            try
            {
                this.DeleteTrnPeriodoCna(entity);
                id = FactoryTransferencia.GetTrnPeriodoCnaRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
            return id;
        }

        /// <summary>
        /// Inserta un registro de la tabla trn_periodo_cna
        /// </summary>
        /// <param name="entity"></param>
        /// <returns>Entero</returns>
        public void DeleteTrnPeriodoCna(TrnPeriodoCnaDTO entity)
        {
            try
            {
                FactoryTransferencia.GetTrnPeriodoCnaRepository().Delete(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Inserta un registro de la tabla Trn_Periodo_Cna
        /// </summary>
        /// <param name="entity">Objeto de registro Configuracion Pto PMME</param>
        /// <returns>Entero</returns>
        public int SaveTrnConsumoNoAutorizado(TrnConsumoNoAutorizadoDTO entity)
        {
            int id = 0;
            try
            {
                this.DeleteTrnConsumoNoAutorizado(entity.Emprcodi, entity.Fechacna.ToString(ConstantesBase.FormatoFecha));
                id = FactoryTransferencia.GetTrnConsumoNoAutorizadoRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
            return id;
        }

        /// <summary>
        /// Inserta un registro de la tabla trn_periodo_cna
        /// </summary>
        /// <param name="emprcodi"></param>
        /// <param name="fechacna"></param>
        /// <returns>Entero</returns>
        public void DeleteTrnConsumoNoAutorizado(int emprcodi, string fechacna)
        {
            try
            {
                FactoryTransferencia.GetTrnConsumoNoAutorizadoRepository().Delete(emprcodi, fechacna);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite buscar los CNA
        /// </summary>
        /// <param name="fechainicio"></param>
        /// <param name="fechafin"></param>
        /// <returns></returns>
        public List<TrnConsumoNoAutorizadoDTO> ListTrnConsumoNoAutorizado(string fechainicio, string fechafin)
        {
            return FactoryTransferencia.GetTrnConsumoNoAutorizadoRepository().ListTrnConsumoNoAutorizado(fechainicio, fechafin);
        }

        /// <summary>
        /// Elimina un registro de la tabla Trn_Contador_Correos_Cna
        /// </summary>
        /// <param name="emprcodi"></param>
        /// <returns>Entero</returns>
        public void DeleteTrnContadorCorreosCna(int emprcodi)
        {
            try
            {
                FactoryTransferencia.GetTrnContadorCorreosCnaRepository().Delete(emprcodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Inserta un registro de la tabla Trn_Contador_Correos_Cna
        /// </summary>
        /// <param name="entity"></param>
        /// <returns>Entero</returns>
        public int SaveTrnContadorCorreosCna(TrnContadorCorreosCnaDTO entity)
        {
            int id = 0;
            try
            {
                this.DeleteTrnContadorCorreosCna(entity.Emprcodi);
                id = FactoryTransferencia.GetTrnContadorCorreosCnaRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
            return id;
        }

        /// <summary>
        /// Permite obtener contador de correos
        /// </summary>
        /// <param name="emprcodi"></param>
        /// <returns></returns>
        public int ObtenerContadorParticipante(int emprcodi)
        {
            return FactoryTransferencia.GetTrnContadorCorreosCnaRepository().ObtenerContadorParticipante(emprcodi);
        }

        /// <summary>
        /// Permite buscar las configuraciones segun criterios especificados
        /// </summary>
        /// <param name="emprcodi"></param>
        /// <param name="ptomedicodi"></param>
        /// <param name="vigencia"></param>
        /// <returns></returns>
        public List<TrnConfiguracionPmmeDTO> ListaTrnConfiguracionPmmexVigencia(int emprcodi, int ptomedicodi, string vigencia)
        {
            return FactoryTransferencia.GetTrnConfiguracionPmmeRepository().ListaTrnConfiguracionPmme(emprcodi, ptomedicodi, vigencia);
        }

        /// <summary>
        /// Procesa el cálculo de consumo no autorizado (CNA)
        /// </summary>

        public void ProcesarCna(string fechaInicio, string fechaFin)
        {
            try
            {
                DateTime Finicio = DateTime.ParseExact(fechaInicio, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);
                DateTime Ffin = DateTime.ParseExact(fechaFin, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);
                DateTime Finiciotercer = new DateTime(Finicio.Year, Finicio.Month - 1, 1);
                DateTime Ffintercer = new DateTime(Finiciotercer.Year, Finiciotercer.Month + 1, 1).AddDays(-1);

                List<DemandaMercadoLibreDTO> ListaInformacionAgentes = ListaDemandaAgentes(Finicio, Ffin);
                List<DemandaMercadoLibreDTO> ListaInformacionAgentesEspecial = ListaDemandaAgentes(Finiciotercer, Ffintercer);

                //DateTime primerdia = new DateTime(Finicio.Year, Finicio.Month, 1);
                //DateTime tercerdia = primerdia.AddDays(2);
                //if(Finicio >= primerdia && Finicio <= tercerdia)
                //    primerdia = new DateTime(DateTime.Now.Year, DateTime.Now.Month - 1, 1);

                //int tipoEmpresa = 0;
                //string nombreEmpresa = string.Empty;
                //var fechaInicial = primerdia.AddMonths(-11);
                //int IdLectura = Int32.Parse(ConfigurationManager.AppSettings["IdLecturaTR"]);
                //string periodo = primerdia.Year.ToString() + primerdia.Month.ToString().PadLeft(2,'0');

                //DateTime[] PeriodosEvaluados = new DateTime[12];

                //for (int i = 0; i < 12; i++)
                //{
                //    PeriodosEvaluados[i] = fechaInicial.AddMonths(i);
                //}

                //List<DemandaMercadoLibreDTO> ListaInformacionAgentes = servicioBase.ListDemandaMercadoLibre(PeriodosEvaluados,
                //    primerdia, tipoEmpresa, nombreEmpresa, IdLectura, ConstantesAppServicio.CodigoOrigenLecturaML);

                //foreach (var item in ListaInformacionAgentes)
                //{
                //    item.PotenciaMaximaRetirar = item.DemandaMaxima * ConstantesAppServicio.dPorcentajePotenciaMaxima;

                //    Dominio.DTO.Transferencias.EmpresaDTO Entidad = (new EmpresaAppServicio()).GetByNombreSic(item.EmprRazSocial);
                //    TrnDemandaDTO demanda = this.TrnDemandaxEmpresa(periodo, Entidad.EmprCodi);
                //    if (demanda.Demcodi > 0)
                //        item.PotenciaMaximaRetirar = demanda.Valormaximo;
                //}

                var listaFechas = Enumerable.Range(0, 1 + Ffin.Subtract(Finicio).Days).Select(incremento => Finicio.AddDays(incremento)).ToList();

                int numSemana = EPDate.f_numerosemana(Finicio);
                string semanaperiodo = numSemana.ToString() + Finicio.Year.ToString();
                string aniosemana = Finicio.Year.ToString() + numSemana.ToString();
                DateTime fechaIniSemana = EPDate.GetFechaIniPeriodo(2, string.Empty, aniosemana, string.Empty, string.Empty).AddDays(2);
                DateTime fechaFinSemana = fechaIniSemana.AddDays(6);
                TrnPeriodoCnaDTO ConfSemana = this.ObtenerSemana(semanaperiodo);

                List<DateTime> lstEliminadoFechas = new List<DateTime>();
                if (ConfSemana.Percnacodi > 0)
                {
                    foreach (var itemF in listaFechas)
                    {
                        if (EPDate.f_NombreDiaSemanaCorto(itemF.DayOfWeek) == "LUN" && ConfSemana.Dl == "N")
                            lstEliminadoFechas.Add(itemF);

                        if (EPDate.f_NombreDiaSemanaCorto(itemF.DayOfWeek) == "MAR" && ConfSemana.Dm == "N")
                            lstEliminadoFechas.Add(itemF);

                        if (EPDate.f_NombreDiaSemanaCorto(itemF.DayOfWeek) == "MIE" && ConfSemana.Dmm == "N")
                            lstEliminadoFechas.Add(itemF);

                        if (EPDate.f_NombreDiaSemanaCorto(itemF.DayOfWeek) == "JUE" && ConfSemana.Dj == "N")
                            lstEliminadoFechas.Add(itemF);

                        if (EPDate.f_NombreDiaSemanaCorto(itemF.DayOfWeek) == "VIE" && ConfSemana.Dvr == "N")
                            lstEliminadoFechas.Add(itemF);

                        if (EPDate.f_NombreDiaSemanaCorto(itemF.DayOfWeek) == "SAB" && ConfSemana.Ds == "N")
                            lstEliminadoFechas.Add(itemF);

                        if (EPDate.f_NombreDiaSemanaCorto(itemF.DayOfWeek) == "DOM" && ConfSemana.Dd == "N")
                            lstEliminadoFechas.Add(itemF);
                    }
                    listaFechas.RemoveAll(lstEliminadoFechas.Contains);
                }

                List<TrnConfiguracionPmmeDTO> lstConfiguraciones = new List<TrnConfiguracionPmmeDTO>();
                List<TrnConsumoNoAutorizadoDTO> lstCNAs = new List<TrnConsumoNoAutorizadoDTO>();
                List<MePtomedicionDTO> lstPtosVigentes;
                List<Dominio.DTO.Transferencias.EmpresaDTO> ListaEmpresas = servicioEmpresa.ListaConfPtosMMExEmpresa();
                decimal sumMediTotalDia = 0;

                foreach (var fechaevaluada in listaFechas)
                {                                     
                    #region Suma Medidas conjunto Eval por FechaEvaluada
                    foreach (var itemEmp in ListaEmpresas)
                    {
                        lstPtosVigentes = new List<MePtomedicionDTO>();
                        decimal PMAX_MME = 0;
                        sumMediTotalDia = 0;
                        var listaPto = ListarPtosVigentes(itemEmp.EmprCodi, fechaevaluada, fechaevaluada);
                        lstPtosVigentes.AddRange(listaPto);

                        List<MeMedicion96DTO> listaPtos96 = FactorySic.GetMeMedicion96Repository().GetEnvioArchivo(ConstantesAppServicio.LectcodiTransferencia, itemEmp.EmprCodi, fechaevaluada, fechaevaluada);

                        int ptoAux = 0;
                        TrnConsumoNoAutorizadoDTO TrnCNA = new TrnConsumoNoAutorizadoDTO();
                        
                        foreach (var lpto in listaPto)
                        {
                            foreach (var ptosMME in listaPtos96.OrderBy(x => x.Medifecha))
                            {
                                if (lpto.Ptomedicodi == ptosMME.Ptomedicodi && ptosMME.Medifecha == fechaevaluada && ptosMME.Meditotal > 0 && ptoAux != ptosMME.Ptomedicodi)
                                {
                                    sumMediTotalDia += Convert.ToDecimal(ptosMME.Meditotal);
                                    ptoAux = ptosMME.Ptomedicodi;
                                    break;
                                }
                            }                                    
                        }

                        decimal EQ = 0;
                        decimal Exc_diario = 0;

                        DateTime fechacompara = new DateTime(fechaevaluada.Year, fechaevaluada.Month, 3);
                        if (fechaevaluada > fechacompara)
                        {
                            for (int x = 0; x < ListaInformacionAgentes.Count; x++)
                            {
                                if (ListaInformacionAgentes[x].EmprRazSocial.Trim() == itemEmp.EmprNombre.Trim())
                                {
                                    PMAX_MME = Convert.ToDecimal(ListaInformacionAgentes[x].PotenciaMaximaRetirar.ToString("N3"));
                                    break;
                                }
                            }
                        }
                        else
                        {
                            for (int x = 0; x < ListaInformacionAgentesEspecial.Count; x++)
                            {
                                if (ListaInformacionAgentesEspecial[x].EmprRazSocial.Trim() == itemEmp.EmprNombre.Trim())
                                {
                                    PMAX_MME = Convert.ToDecimal(ListaInformacionAgentesEspecial[x].PotenciaMaximaRetirar.ToString("N3"));
                                    break;
                                }
                            }
                        }

                        

                        EQ = Convert.ToDecimal((PMAX_MME * 25 / 100).ToString("N3"));
                        decimal ConsumoDiario = sumMediTotalDia * 25 / 100;
                        Exc_diario = Convert.ToDecimal(ConsumoDiario.ToString("N3")) - PMAX_MME * 24;
                        if (Exc_diario > 0 && Exc_diario >= EQ)
                        {
                            TrnCNA.Emprcodi = itemEmp.EmprCodi;
                            TrnCNA.Emprnomb = itemEmp.EmprNombre;
                            TrnCNA.Fechacna = fechaevaluada;
                            TrnCNA.Valorcna = Exc_diario;
                            TrnCNA.Lastuser = "Sistemas";
                            TrnCNA.Lastdate = DateTime.Now;
                            lstCNAs.Add(TrnCNA);
                        }
                    }
                    #endregion
                    
                }
                    
                foreach (var cna_ in lstCNAs)
                {
                    TrnConsumoNoAutorizadoDTO CnaDTO = new TrnConsumoNoAutorizadoDTO();
                    CnaDTO.Emprcodi = cna_.Emprcodi;
                    CnaDTO.Emprnomb = cna_.Emprnomb;
                    CnaDTO.Fechacna = cna_.Fechacna;
                    CnaDTO.Valorcna = cna_.Valorcna;
                    CnaDTO.Lastuser = cna_.Lastuser;
                    CnaDTO.Lastdate = cna_.Lastdate;

                    this.SaveTrnConsumoNoAutorizado(CnaDTO);
                }
                foreach (var log in ListaEmpresas) 
                {
                    TrnLogCnaDTO logDTO = new TrnLogCnaDTO();
                    logDTO.Emprcodi = log.EmprCodi;
                    logDTO.FechaProceso = DateTime.Now;
                    this.SaveTrnLogCna(logDTO);
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }

        }

        /// <summary>
        /// Permite listar los puntos de medición vigente por empresa
        /// </summary>
        /// <param name="Emprcodi"></param>
        /// <param name="Finicio"></param>
        /// <param name="Ffin"></param>
        /// <returns></returns>
        public List<MePtomedicionDTO>ListarPtosVigentes(int Emprcodi, DateTime Finicio, DateTime Ffin)
        {
            List<TrnConfiguracionPmmeDTO> lstConfiguraciones = new List<TrnConfiguracionPmmeDTO>();
            lstConfiguraciones = this.ListaConfPtosMMExEmpresa(Emprcodi).ToList();
            List<int> ids = lstConfiguraciones.Select(x => x.Ptomedicodi).Distinct().ToList();
            List<MePtomedicionDTO> lstPtosMME = new List<MePtomedicionDTO>();

            foreach (var pto in ids)
            {
                TrnConfiguracionPmmeDTO ptoNoVigente = lstConfiguraciones.Where(x => x.Ptomedicodi == pto && x.Vigencia == "N").OrderByDescending(x => x.Fechavigencia).FirstOrDefault();
                TrnConfiguracionPmmeDTO ptoVigente = lstConfiguraciones.Where(x => x.Ptomedicodi == pto && x.Vigencia == "S").OrderByDescending(x => x.Fechavigencia).FirstOrDefault();

                if (ptoVigente != null && ptoVigente.Fechavigencia <= Finicio && ptoVigente.Fechavigencia <= Ffin)
                {
                    if (ptoNoVigente != null)
                    {
                        if (ptoVigente.Fechavigencia < ptoNoVigente.Fechavigencia)
                        {
                            MePtomedicionDTO ptolista = new MePtomedicionDTO();
                            ptolista.Ptomedicodi = pto;
                            ptolista.Emprcodi = Emprcodi;
                            lstPtosMME.Add(ptolista);
                        }
                    }
                    else
                    {
                        MePtomedicionDTO ptolista = new MePtomedicionDTO();
                        ptolista.Ptomedicodi = pto;
                        ptolista.Emprcodi = Emprcodi;
                        lstPtosMME.Add(ptolista);
                    }
                }
            }

            return lstPtosMME;
        }

        /// <summary>
        /// Inserta un registro de la tabla TrnLogCna
        /// </summary>
        /// <param name="entity"></param>
        /// <returns>Entero</returns>
        public int SaveTrnLogCna(TrnLogCnaDTO entity)
        {
            int id = 0;
            try
            {
                id = FactoryTransferencia.GetTrnLogCnaRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
            return id;
        }

        /// <summary>
        /// Permite buscar las configuraciones segun criterios especificados
        /// </summary>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <returns></returns>
        public List<TrnLogCnaDTO> ListaTrnLogCna(DateTime fechaInicio, DateTime fechaFin)
        {
            return FactoryTransferencia.GetTrnLogCnaRepository().List(fechaInicio, fechaFin);
        }

        /// <summary>
        /// Lista la información de los agentes
        /// </summary>
        /// <param name="Finicio"></param>
        /// <param name="Ffin"></param>
        /// <returns></returns>
        public List<DemandaMercadoLibreDTO> ListaDemandaAgentes(DateTime Finicio, DateTime Ffin)
        {
            DateTime primerdia = new DateTime(Finicio.Year, Finicio.Month, 1);
            DateTime tercerdia = primerdia.AddDays(2);
            //if (Finicio >= primerdia && Finicio <= tercerdia)
            //    primerdia = new DateTime(DateTime.Now.Year, DateTime.Now.Month - 1, 1);

            int tipoEmpresa = 0;
            string nombreEmpresa = string.Empty;
            var fechaInicial = primerdia.AddMonths(-11);
            int IdLectura = Int32.Parse(ConfigurationManager.AppSettings["IdLecturaTR"]);
            string periodo = primerdia.Month.ToString().PadLeft(2, '0') + primerdia.Year.ToString();

            DateTime[] PeriodosEvaluados = new DateTime[12];

            for (int i = 0; i < 12; i++)
            {
                PeriodosEvaluados[i] = fechaInicial.AddMonths(i);
            }

            List<DemandaMercadoLibreDTO> ListaInformacionAgentes = servicioBase.ListDemandaMercadoLibre(PeriodosEvaluados,
                primerdia, tipoEmpresa, nombreEmpresa, IdLectura, ConstantesAppServicio.CodigoOrigenLecturaML);

            foreach (var item in ListaInformacionAgentes)
            {
                item.PotenciaMaximaRetirar = item.DemandaMaxima * ConstantesAppServicio.dPorcentajePotenciaMaxima;

                Dominio.DTO.Transferencias.EmpresaDTO Entidad = (new EmpresaAppServicio()).GetByNombreSic(item.EmprRazSocial);
                TrnDemandaDTO demanda = this.TrnDemandaxEmpresa(periodo, Entidad.EmprCodi);
                if (demanda.Demcodi > 0)
                    item.PotenciaMaximaRetirar = demanda.Valormaximo;
            }

            return ListaInformacionAgentes;
        }
        
        /// <summary>
        /// Notifica las empresas que han generado Cna
        /// </summary>
        /// <returns></returns>
        public void NotificacionCna(int Plantillacorreo, string Name)
        {
            var plantilla = new SiPlantillacorreoDTO();
            plantilla = null;
            string contenido = String.Empty;
            string sto = String.Empty;
            string asunto = String.Empty;
            string listaBCc = String.Empty;
            string listaCC = String.Empty;
            DateTime final = DateTime.Now;
            string listFrom = String.Empty;
            
            int numSemana = EPDate.f_numerosemana(DateTime.Now) - 1;
            string semanaperiodo = DateTime.Now.Year.ToString() + numSemana.ToString();
            DateTime fechaIniSemana = EPDate.GetFechaIniPeriodo(2, string.Empty, semanaperiodo, string.Empty, string.Empty).AddDays(2);
            DateTime fechaFinSemana = fechaIniSemana.AddDays(6);

            List<TrnConsumoNoAutorizadoDTO> lstCNA = new List<TrnConsumoNoAutorizadoDTO>();

            lstCNA = this.ListTrnConsumoNoAutorizado(fechaIniSemana.ToString(ConstantesAppServicio.FormatoFechaYMD), fechaFinSemana.ToString(ConstantesAppServicio.FormatoFechaYMD));

            plantilla = FactorySic.GetSiPlantillacorreoRepository().GetById(Plantillacorreo);

            sto = plantilla.Planticorreos;
            listaBCc = plantilla.PlanticorreosBcc;
            listaCC = plantilla.PlanticorreosCc;
            asunto = plantilla.Plantasunto;
            listFrom = plantilla.PlanticorreoFrom;

            List<string> listTo = new List<string>();
            listTo = sto.Split(';').ToList();

            List<string> listBCc = new List<string>();

            List<string> listCC = new List<string>();

            List<int> ids = lstCNA.Select(x => x.Emprcodi).Distinct().ToList();
            int contador;

            string nomEmpresa = string.Empty;
            foreach (var cna_mail in ids)
            {
                string dias = string.Empty;
                contenido = plantilla.Plantcontenido;
                contador = 0;
                int ContCorreos = 1;
                foreach (var item in lstCNA)
                {
                    if (cna_mail == item.Emprcodi)
                    {
                        contador++;
                        dias = dias + ", " + item.Fechacna.ToString(ConstantesAppServicio.FormatoFecha);
                        nomEmpresa = item.Emprnomb;
                    }
                }
                if (contador > 0)
                {
                    TrnContadorCorreosCnaDTO dtocorreo = new TrnContadorCorreosCnaDTO();
                    ContCorreos = this.ObtenerContadorParticipante(cna_mail) + ContCorreos;

                    dtocorreo.Emprcodi = cna_mail;
                    dtocorreo.Cantcorreos = ContCorreos;
                    dtocorreo.Lastdate = DateTime.Now;
                    dtocorreo.Lastuser = Name;

                    contenido = String.Format(contenido, contador.ToString(), nomEmpresa, dias, ContCorreos.ToString(), fechaIniSemana.ToString(ConstantesAppServicio.FormatoFecha), fechaFinSemana.ToString(ConstantesAppServicio.FormatoFecha));
                    asunto = String.Format(asunto, nomEmpresa, fechaIniSemana.ToString(ConstantesAppServicio.FormatoFecha), fechaFinSemana.ToString(ConstantesAppServicio.FormatoFecha));
                    Base.Tools.Util.SendEmail(listTo, listBCc, listCC, asunto, contenido, listFrom);
                    this.SaveTrnContadorCorreosCna(dtocorreo);
                }
            }
        }
    }
}