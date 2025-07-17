using COES.Base.Core;
using COES.Dominio.DTO.Sic;
using COES.Dominio.DTO.Transferencias;
using COES.Servicios.Aplicacion.Factory;
using COES.Servicios.Aplicacion.Helper;
using COES.Servicios.Aplicacion.Indisponibilidades;
using COES.Servicios.Aplicacion.SIOSEIN;
using COES.Servicios.Aplicacion.Transferencias;
using COES.Servicios.Aplicacion.TransfPotencia.Helper;
using log4net;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Servicios.Aplicacion.Transferencias
{
    public class VariacionCodigoAppServicio: AppServicioBase
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(VariacionEmpresaAppServicio));
        /// <summary>
        /// Permite obtener la variacion de codigo por codigo vtp
        /// </summary>
        /// <param name="varCodCodigoVtp">Codigo VTP</param>
        /// <returns>EmpresaDTO</returns>
        public VtpVariacionCodigoDTO GetVariacionCodigoByCodVtp(string varCodCodigoVtp)
        {
            try
            {
                return FactoryTransferencia.GetVtpVariacionCodigoRepository().GetVariacionCodigoByCodVtp(varCodCodigoVtp);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite registrar una variación por codigo
        /// </summary>
        /// <param name="varCodDTO">Objeto para grabar</param>
        /// <returns>EmpresaDTO</returns>
        public int Save(VtpVariacionCodigoDTO varCodDTO)
        {
            try
            {
                return FactoryTransferencia.GetVtpVariacionCodigoRepository().Save(varCodDTO);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite actualizar el status por codigo de variación
        /// </summary>
        /// <param name="entity">Objeto para actualizar</param>
        /// <returns>Cantidad de registros por tipo de comparación</returns>
        public void UpdateStatusVariationByCodigoVtp(VtpVariacionCodigoDTO entity)
        {
            try
            {
                FactoryTransferencia.GetVtpVariacionCodigoRepository().UpdateStatusVariationByCodigoVtp(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener la lista de variaciones de codigo por empresa
        /// </summary>
        /// <param name="emprCodi">Codigo VTP</param>
        /// <returns>EmpresaDTO</returns>
        public List<VtpVariacionCodigoDTO> ListVariacionCodigoByEmprCodi(int emprCodi, int nroPagina, int pageSize, string varCodiVtp)
        {
            try
            {
                return FactoryTransferencia.GetVtpVariacionCodigoRepository().ListVariacionCodigoByEmprCodi(emprCodi, nroPagina, pageSize, varCodiVtp);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener el total de registros de variación de codigo por empresa
        /// </summary>
        /// <param name="emprCodi">Codigo de empresa</param>
        /// <returns>Cantidad de registros por empresa</returns>
        public int GetNroRecordsVariacionCodigoByEmprCodi(int emprCodi, string varCodiVtp)
        {
            try
            {
                return FactoryTransferencia.GetVtpVariacionCodigoRepository().GetNroRecordsVariacionCodigoByEmprCodi(emprCodi, varCodiVtp);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }

        }

        /// <summary>
        /// Permite obtener la lista de variaciones de codigo por empresa
        /// </summary>
        /// <param name="emprCodi">Codigo VTP</param>
        /// <returns>EmpresaDTO</returns>
        public List<VtpVariacionCodigoDTO> ListVariacionCodigoVTEAByEmprCodi(int emprCodi, int nroPagina, int pageSize, string varCodiVtp)
        {
            try
            {
                return FactoryTransferencia.GetVtpVariacionCodigoRepository().ListVariacionCodigoVTEAByEmprCodi(emprCodi, nroPagina, pageSize, varCodiVtp);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener el total de registros de variación de codigo por empresa
        /// </summary>
        /// <param name="emprCodi">Codigo de empresa</param>
        /// <returns>Cantidad de registros por empresa</returns>
        public int GetNroRecordsVariacionCodigoVTEAByEmprCodi(int emprCodi, string varCodiVtp)
        {
            try
            {
                return FactoryTransferencia.GetVtpVariacionCodigoRepository().GetNroRecordsVariacionCodigoVTEAByEmprCodi(emprCodi, varCodiVtp);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }

        }

        /// <summary>
        /// Permite el historial de cambios de variación por codigo vtp
        /// </summary>
        /// <param name="codigovtp">CodigoVtp</param>
        /// <returns>List VtpVariacionEmpresaDTO</returns>
        public List<VtpVariacionCodigoDTO> ListHistoryVariacionCodigoByCodigoVtp(string codigovtp, string varemptipocomp)
        {
            try
            {
                return FactoryTransferencia.GetVtpVariacionCodigoRepository().ListHistoryVariacionCodigoByCodigoVtp(codigovtp,varemptipocomp);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }
    }
}
