using COES.Base.Core;
using COES.Dominio.DTO.Transferencias;
using COES.Servicios.Aplicacion.Factory;
using COES.Servicios.Aplicacion.Helper;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Servicios.Aplicacion.Transferencias
{
    /// <summary>
    /// Clases con métodos del módulo Barra_URS
    /// </summary>
    public class BarraUrsAppServicio : AppServicioBase
    {

        /// <summary>
        /// Instancia para el manejo de logs
        /// </summary>
        private static readonly ILog Logger = LogManager.GetLogger(typeof(BarraUrsAppServicio));

        #region Métodos Tabla TRN_BARRAURS

        /// <summary>
        /// Permite listar todas las barras URS por BARRCODI
        /// </summary>
        /// <param name="barrcodi">Identificador de la tabla TRN_BARRA</param>
        /// <returns>Lista de TrnBarraursDTO</returns>
        public List<TrnBarraursDTO> ListBarraURS(int barrcodi)
        {
            return FactoryTransferencia.GetBarraUrsRepository().List(barrcodi);
        }

        /// <summary>
        /// Permite listar todas las URS registrados en Barras
        /// </summary>
        /// <returns>Lista de TrnBarraursDTO</returns>
        public List<TrnBarraursDTO> ListURS()
        {
            return FactoryTransferencia.GetBarraUrsRepository().ListURS();
        }

        /// <summary>
        /// Permite listar todas las URS registrados en CostoMarginal
        /// </summary>
        /// <param name="pericodi">Periodo de calculo</param>
        /// <param name="vcrecacodi">Version de calculo</param>
        /// <returns>Lista de TrnBarraursDTO</returns>
        public List<TrnBarraursDTO> ListURSCostoMarginal(int pericodi, int vcrecacodi)
        {
            return FactoryTransferencia.GetBarraUrsRepository().ListURSCostoMarginal(pericodi, vcrecacodi);
        }

        /// <summary>
        /// Permite listar todas las barras URS por EQUICODI
        /// </summary>
        /// <param name="barrcodi">Identificador de la tabla TRN_BARRA</param>
        /// <returns>Lista de TrnBarraursDTO</returns>
        public List<TrnBarraursDTO> ListBarraURSbyEquicodi(int equicodi)
        {
            return FactoryTransferencia.GetBarraUrsRepository().ListbyEquicodi(equicodi);
        }

        /// <summary>
        /// Inserta un registro de la tabla TRN_BARRA_URS
        /// </summary>
        public void saveBarraUrs(TrnBarraursDTO entity)
        {
            try
            {
                FactoryTransferencia.GetBarraUrsRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla TRN_BARRA_URS
        /// </summary>
        /// <param name="barrcodi">Identificador de la tabla TRN_BARRA</param>
        public TrnBarraursDTO GetByIdUrs(int barrcodi, int grupocodi)
        {
            return FactoryTransferencia.GetBarraUrsRepository().GetById(barrcodi, grupocodi);
        }

        /// <summary>
        /// Permite obtener un registro de la tabla TRN_BARRA_URS
        /// </summary>
        /// <param name="grupocodi">Identificador de la tabla PR_GRUPO</param>
        public TrnBarraursDTO GetByIdUrsGrupoCodi(int grupocodi)
        {
            return FactoryTransferencia.GetBarraUrsRepository().GetByIdGrupoCodi(grupocodi);
        }

        /// <summary>
        /// Elimina un registro de la tabla ST_BARRA_URS
        /// </summary>
        /// <param name="barrcodi">Identificador de la tabla TRN_BARRA</param>
        public void DeleteUrs(int barrcodi, int grupocodi, string username)
        {
            try
            {
                FactoryTransferencia.GetBarraUrsRepository().Delete(barrcodi, grupocodi);
                FactoryTransferencia.GetBarraUrsRepository().Delete_UpdateAuditoria(barrcodi, grupocodi, username);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite listar todas las empresas por URS registradas
        /// </summary>
        /// <returns>Lista de TrnBarraursDTO</returns>
        public List<TrnBarraursDTO> GetEmpresas()
        {
            return FactoryTransferencia.GetBarraUrsRepository().GetEmpresas();
        }
        #endregion

        #region Métodos Tabla URS - PR_GRUPO

        /// <summary>
        /// Permite obtener un registro de la tabla PR_GRUPO por el Nombre del URS
        /// </summary>
        /// <param name="grupocnomb">Nombre de la URS</param>
        public TrnBarraursDTO GetByNombrePrGrupo(string grupocnomb)
        {
            return FactoryTransferencia.GetBarraUrsRepository().GetByNombrePrGrupo(grupocnomb);
        }

        /// <summary>
        /// Lista de URS obtenido de la tabla PR_GRUPO
        /// </summary>
        public List<TrnBarraursDTO> ListPrGrupo()
        {
            return FactoryTransferencia.GetBarraUrsRepository().ListPrGrupo();
        }

        /// <summary>
        /// Permite obtener un registro de la tabla TRN_BARRA_URS por el codigo del URS
        /// </summary>
        /// <param name="grupocodi">Codigo de la URS</param>
        public TrnBarraursDTO GetByIdGrupoCodiTRN(int grupocodi)
        {
            return FactoryTransferencia.GetBarraUrsRepository().GetByIdGrupoCodiTRN(grupocodi);
        }
        #endregion
    }
}
