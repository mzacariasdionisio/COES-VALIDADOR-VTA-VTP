using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COES.Base.Core;
using COES.Dominio.DTO.Transferencias;
using COES.Servicios.Aplicacion.Factory;

namespace COES.Servicios.Aplicacion.Transferencias
{
   public class IngresoRetiroSCAppServicio:AppServicioBase
    {
        /// <summary>
        /// Permite grabar una entidad IngresoRetiroSCDTO
        /// </summary>
        /// <param name="entity">IngresoRetiroSCDTO</param>
        /// <returns>El Código insertado de la tabla TRN_ING_RETIROSC</returns>
        public int SaveIngresoRetiroSC(IngresoRetiroSCDTO entity)
        {
            try
            {
                int id = 0;
                if (entity.IngrscCodi == 0)
                {
                    id = FactoryTransferencia.GetIngresoRetiroSCRepository().Save(entity);
                }
                return id;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite eliminar un registro de la tabla TRN_ING_RETIROSC en base al periodo y version
        /// </summary>
        /// <param name="iPeriCodi">Mes de valorización</param>
        /// <param name="iIngrscVersion">Versión del Mes de valorización</param>
        /// <returns>Retorna el Codigo del Mes de valorización</returns>   
        public int DeleteListaIngresoRetiroSC(int iPeriCodi, int iIngrscVersion)
        {
            try
            {
                FactoryTransferencia.GetIngresoRetiroSCRepository().Delete(iPeriCodi, iIngrscVersion);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
            return iPeriCodi;
        }

        /// <summary>
        /// Permite obtener importes de los retiros sin contrato en base al periodo y version
        /// </summary>
        /// <param name="iPeriCodi">Mes de valorización</param>
        /// <param name="iIngrscVersion">Versión del Mes de valorización</param>
        /// <returns>Lista de IngresoRetiroSCDTO</returns>
        public List<IngresoRetiroSCDTO> ListImportesByPeriVer(int iPeriCodi, int iIngrscVersion)
        {
            return FactoryTransferencia.GetIngresoRetiroSCRepository().GetByCriteria(iPeriCodi, iIngrscVersion);
        }

        /// <summary>
        /// Permite obtener los   retiro sin contrato en base al periodo
        /// </summary>
        /// <param name="iPeriCodi">Mes de valorización</param>
        /// <returns>Lista de IngresoRetiroSCDTO</returns>
        public List<IngresoRetiroSCDTO> BuscarIngresoRetiroSC(int? iPeriCodi,int? version)
        {
            return FactoryTransferencia.GetIngresoRetiroSCRepository().GetByCodigo(iPeriCodi, version);
        }

        /// <summary>
        /// Permite obtener  los retiro sin contrato en base al periodo y version
        /// </summary>
        /// <param name="iPeriCodi">Mes de valorización</param>
        /// <param name="iIngrscVersion">Versión del Mes de valorización</param>
        /// <returns>Lista de IngresoRetiroSCDTO</returns>
        public List<IngresoRetiroSCDTO> ListByPeriodoVersion(int iPericodi, int iIngrscVersion)
        {
            return FactoryTransferencia.GetIngresoRetiroSCRepository().ListByPeriodoVersion(iPericodi, iIngrscVersion);
        }

        /// <summary>
        /// Permite obtener una registro de IngresoRetiroSC en base a su periodo, version y empresa
        /// </summary>     
        /// <param name="iEmprCodi">Versión del Mes de valorización</param>
        /// <returns>Lista de IngresoPotenciaDTO</returns>    
        public IngresoRetiroSCDTO GetByPeriodoVersionEmpresa(int iPericodi, int iVersion, int iEmprCodi)
        {
            return FactoryTransferencia.GetIngresoRetiroSCRepository().GetByPeriodoVersionEmpresa(iPericodi, iVersion, iEmprCodi);
        }
    }
}
