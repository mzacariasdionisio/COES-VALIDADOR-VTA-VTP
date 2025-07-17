using COES.Base.Core;
using COES.Dominio.DTO.Transferencias;
using COES.Servicios.Aplicacion.Factory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Servicios.Aplicacion.Transferencias
{
    public class CompensacionAppServicio : AppServicioBase
    {
        /// <summary>
        /// Permite grabar o actualizar una CompensacionDTO en base a la entidad
        /// </summary>
        /// <param name="entity">Entidad de CompensacionDTO</param>
        /// <returns>Retorna el iCabComCodi nuevo o actualizado</returns>
        public int SaveOrUpdateCompensacion(CompensacionDTO entity)
        {
            try
            {
                int id = 0;
                if (entity.CabeCompCodi == 0)
                {
                    id = FactoryTransferencia.GetCompensacionRepository().Save(entity);
                }
                else
                {
                    FactoryTransferencia.GetCompensacionRepository().Update(entity);
                    id = entity.CabeCompCodi;
                }
                return id;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un Codigo Retiro sin contrato en base al iCabComCodi
        /// </summary>
        /// <param name="iCabComCodi">Codigo de la tabla TRN_CABE_COMPENSACION</param>
        /// <returns>Retorna el iCabComCodi eliminado</returns>
        public int DeleteCompensacion(int iCabComCodi)
        {
            try
            {
                FactoryTransferencia.GetCompensacionRepository().Delete(iCabComCodi);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
            return iCabComCodi;
        }

        /// <summary>
        /// Permite obtener una compensacion en base al iCabComCodi
        /// </summary>
        /// <param name="iCabComCodi">Codigo de la tabla TRN_CABE_COMPENSACION</param>
        /// <returns>CompensacionDTO</returns>
        public CompensacionDTO GetByIdCompensacion(int iCabComCodi)
        {
            return FactoryTransferencia.GetCompensacionRepository().GetById(iCabComCodi);
        }

        /// <summary>
        /// Permite obtener compensacion en base a su nombre y periodo
        /// </summary>
        /// <param name="sCabComNombre">Nombre de la compensación</param>
        /// <param name="iPeriCodi">Codigo de la Tabla TRN_PERIODO</param>
        /// <returns>CompensacionDTO</returns>        
        public CompensacionDTO GetByCodigo(string sCabComNombre, int iPeriCodi)
        {
            return FactoryTransferencia.GetCompensacionRepository().GetByCodigo(sCabComNombre, iPeriCodi);
        }

        /// <summary>
        /// Permite listar las compensaciones en base iPeriCodi
        /// </summary>
        /// <param name="iPeriCodi">Codigo de la Tabla TRN_PERIODO</param>
        /// <returns>Lista de CompensacionDTO</returns>        
        public List<CompensacionDTO> ListCompensaciones(int iPeriCodi)
        {
            return FactoryTransferencia.GetCompensacionRepository().List(iPeriCodi);
        }

        /// <summary>
        /// Permite realizar búsquedas de compensaciones en base al nombre
        /// </summary>
        /// <param name="sCabComNombre">Nombre de la compensación</param>
        /// <returns>Lista de CompensacionDTO</returns>
        public List<CompensacionDTO> BuscarCompensacion(string sCabComNombre)
        {
            return FactoryTransferencia.GetCompensacionRepository().GetByCriteria(sCabComNombre);
        }

        /// <summary>
        /// Permite listar las compensaciones en base iPeriCodi, menos Rentas por consignación
        /// </summary>
        /// <param name="iPeriCodi">Codigo de la Tabla TRN_PERIODO</param>
        /// <returns>Lista de CompensacionDTO</returns>        
        public List<CompensacionDTO> ListCompensacionesBase(int pericodi)
        {
            return FactoryTransferencia.GetCompensacionRepository().ListBase(pericodi);
        }

        /// <summary>
        /// Permite listar las compensaciones en base iPeriCodi en los reportes que si se pueden visualizar
        /// </summary>
        /// <param name="iPeriCodi">Codigo de la Tabla TRN_PERIODO</param>
        /// <returns>Lista de CompensacionDTO</returns>        
        public List<CompensacionDTO> ListCompensacionesReporte(int pericodi)
        {
            return FactoryTransferencia.GetCompensacionRepository().ListReporte(pericodi);
        }
    }
}
