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
   public class IngresoPotenciaAppServicio: AppServicioBase
    {

        /// <summary>
        /// Permite grabar una entidad IngresoPotenciaDTO
        /// </summary>
        /// <param name="entity">IngresoPotenciaDTO</param>
        /// <returns>El Código insertado de la tabla TRN_ING_POTENCIA</returns>
        public int SaveIngresoPotencia(IngresoPotenciaDTO entity)
        {
            try
            {
                int id = 0;
                if (entity.IngrPoteCodi == 0)
                {
                    id = FactoryTransferencia.GetIngresoPotenciaRepository().Save(entity);
                }
                return id;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite eliminar un registro en la tabla TRN_ING_POTENCIA en base a su periodo y version
        /// </summary>
        /// <param name="iPeriCodi">Mes de valorización</param>
        /// <param name="iIngrPoteVersion">Versión del Mes de valorización</param>
        /// <returns>Retorna el Codigo del Mes de valorización</returns>    
        public int DeleteListaIngresoPotencia(int iPeriCodi, int iIngrPoteVersion)
        {
            try
            {
                FactoryTransferencia.GetIngresoPotenciaRepository().Delete(iPeriCodi, iIngrPoteVersion);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
            return iPeriCodi;
        }

        /// <summary>
        /// Permite obtener importes IngresoxPotencia en base a su periodo y version
        /// </summary>
        /// <param name="iPeriCodi">Mes de valorización</param>
        /// <param name="iIngrPoteVersion">Versión del Mes de valorización</param>
        /// <returns>Lista de IngresoPotenciaDTO</returns>    
        public List<IngresoPotenciaDTO> ListImportesByPeriVer(int iPeriCodi, int iIngrPoteVersion)
        {
            return FactoryTransferencia.GetIngresoPotenciaRepository().GetByCriteria(iPeriCodi, iIngrPoteVersion);
        }

        /// <summary>
        /// Permite obtener IngresoxPotencia en base a su periodo 
        /// </summary>
        /// <param name="iPeriCodi">Mes de valorización</param>
        /// <returns>Lista de IngresoPotenciaDTO</returns>    
        public List<IngresoPotenciaDTO> BuscarIngresoPotencia(int? iPeriCodi,int? version)
        {
            return FactoryTransferencia.GetIngresoPotenciaRepository().GetByCodigo(iPeriCodi, version);
        }

        /// <summary>
        /// Permite obtener una lista de IngresoxPotencia en base a su periodo y version
        /// </summary>
        /// <param name="iPeriCodi">Mes de valorización</param>
        /// <param name="iIngrPoteVersion">Versión del Mes de valorización</param>
        /// <returns>Lista de IngresoPotenciaDTO</returns>    
        public List<IngresoPotenciaDTO> ListByPeriodoVersion(int iPericodi, int iIngrPoteVersion)
        {
            return FactoryTransferencia.GetIngresoPotenciaRepository().ListByPeriodoVersion(iPericodi, iIngrPoteVersion);
        }

        /// <summary>
        /// Permite obtener una registro de IngresoxPotencia en base a su periodo, version y empresa
        /// </summary>
        /// <param name="iPeriCodi">Mes de valorización</param>
        /// <param name="iIngrPoteVersion">Versión del Mes de valorización</param>
        /// <param name="iEmprCodi">Versión del Mes de valorización</param>
        /// <returns>Lista de IngresoPotenciaDTO</returns>    
        public IngresoPotenciaDTO GetByPeriodoVersionEmpresa(int iPericodi, int iIngrPoteVersion, int iEmprCodi)
        {
            return FactoryTransferencia.GetIngresoPotenciaRepository().GetByPeriodoVersionEmpresa(iPericodi, iIngrPoteVersion, iEmprCodi);
        }
    }
}
