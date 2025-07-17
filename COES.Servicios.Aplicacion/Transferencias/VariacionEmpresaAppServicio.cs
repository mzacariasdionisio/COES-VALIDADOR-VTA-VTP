using COES.Base.Core;
using COES.Dominio.DTO.Transferencias;
using COES.Servicios.Aplicacion.Factory;
using COES.Servicios.Aplicacion.Helper;
using log4net;
using System;
using System.Collections.Generic;

namespace COES.Servicios.Aplicacion.Transferencias
{
    public class VariacionEmpresaAppServicio: AppServicioBase
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(VariacionEmpresaAppServicio));
        /// <summary>
        /// Permite obtener la variacion de empresa por tipo de comparación
        /// </summary>
        /// <param name="varEmpTipoComp">Tipo de Comparación</param>
        /// <returns>EmpresaDTO</returns>
        public VtpVariacionEmpresaDTO GetDefaultPercentVariationByTipoComp(string varEmpTipoComp)
        {
            try
            {
                return FactoryTransferencia.GetVtpVariacionEmpresaRepository().GetDefaultPercentVariationByTipoComp(varEmpTipoComp);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite registrar una variación de empresa
        /// </summary>
        /// <param name="varEmpDto">Objeto para grabar</param>
        /// <returns>EmpresaDTO</returns>
        public int Save(VtpVariacionEmpresaDTO varEmpDto)
        {
            try
            {
                return FactoryTransferencia.GetVtpVariacionEmpresaRepository().Save(varEmpDto);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite actualizar los estados de la tabla VTP_VARIACION_EMPRESA
        /// </summary>
        /// <param name="entity">Objeto para actualizar</param>
        /// <returns>EmpresaDTO</returns>

        public void UpdateStatusVariationByTipoComp(VtpVariacionEmpresaDTO entity)
        {
            try
            {
                FactoryTransferencia.GetVtpVariacionEmpresaRepository().UpdateStatusVariationByTipoComp(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener una lista de empresas con sus respectivos porcentajes de variacion
        /// </summary>
        /// <param name="varemptipocomp">Tipo de comp</param>
        /// <returns>Lista de ValorTransferenciaDTO</returns> 
        public List<VtpVariacionEmpresaDTO> ListValorTransferenciaEmpresaRE(string varemptipocomp, int NroPagina, int PageSize, string varempnomb)
        {
            try
            {
                return FactoryTransferencia.GetVtpVariacionEmpresaRepository().ListVariacionEmpresaByTipoComp(varemptipocomp, NroPagina, PageSize, varempnomb);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener el total de registros de variación de empresa por tipo de comparación
        /// </summary>
        /// <param name="varemptipocomp">Tipo de comparación A: Histórico , B: Energía</param>
        /// <returns>Cantidad de registros por tipo de comparación</returns>
        public int GetNroRecordsVariacionEmpresaByTipoComp(string varemptipocomp, string varemprnomb)
        {
            try
            {
                return FactoryTransferencia.GetVtpVariacionEmpresaRepository().GetNroRecordsVariacionEmpresaByTipoComp(varemptipocomp, varemprnomb);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
            
        }

        /// <summary>
        /// Permite obtener el total de registros de variación de empresa por tipo de comparación
        /// </summary>
        /// <param name="entity">Objeto para actualizar</param>
        /// <returns>Cantidad de registros por tipo de comparación</returns>
        public void UpdateStatusVariationByTipoCompAndEmpresa(VtpVariacionEmpresaDTO entity)
        {
            try
            {
                FactoryTransferencia.GetVtpVariacionEmpresaRepository().UpdateStatusVariationByTipoCompAndEmpresa(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener la variacion de empresa por tipo de comparación y codigo de empresa
        /// </summary>
        /// <param name="varemptipocomp">Tipo de Comparación</param>
        /// <param name="emprcodi">Codigo de empresa</param>
        /// <returns>List VtpVariacionEmpresaDTO</returns>
        public List<VtpVariacionEmpresaDTO> ListHistoryVariacionEmpresaByEmprCodiAndTipoComp(string varemptipocomp, int emprcodi)
        {
            try
            {
                return FactoryTransferencia.GetVtpVariacionEmpresaRepository().ListHistoryVariacionEmpresaByEmprCodiAndTipoComp(varemptipocomp, emprcodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener la variacion de empresa por tipo de comparación y codigo de empresa
        /// </summary>
        /// <param name="varEmpTipoComp">Tipo de Comparación</param>
        /// <param name="emprCodi">Codigo de empresa</param>
        /// <returns>EmpresaDTO</returns>
        public VtpVariacionEmpresaDTO GetPercentVariationByEmprCodiAndTipoComp(Int32 emprCodi, string varEmpTipoComp)
        {
            try
            {
                return FactoryTransferencia.GetVtpVariacionEmpresaRepository().GetPercentVariationByEmprCodiAndTipoComp(emprCodi, varEmpTipoComp);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }
    }
}
