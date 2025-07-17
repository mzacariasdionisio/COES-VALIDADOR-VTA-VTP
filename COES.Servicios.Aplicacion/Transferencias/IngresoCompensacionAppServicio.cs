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
    public class IngresoCompensacionAppServicio : AppServicioBase
    {

        /// <summary>
        /// Permite grabar una entidad IngresoCompensacionDTO 
        /// </summary>
        /// <param name="entity">IngresoCompensacionDTO</param>
        /// <returns>El Código insertado de la tabla TRN_ING_COMPENSACION</returns>
        public int SaveIngresoCompensacion(IngresoCompensacionDTO entity)
        {
            try
            {
                int id = 0;
                if (entity.IngrCompCodi == 0)
                {
                    id = FactoryTransferencia.GetIngresoCompensacionRepository().Save(entity);
                }
                return id;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite eliminar un registro de la tabla TRN_ING_COMPENSACION 
        /// </summary>
        /// <param name="iPeriCodi">Mes de valorización</param>
        /// <param name="iIngrCompVersion">Versión del Mes de valorización</param>
        /// <returns>Retorna el Código del Mes de valorizasción</returns>
        public int DeleteListaIngresoCompensacion(int iPeriCodi, int iIngrCompVersion)
        {
            try
            {
                FactoryTransferencia.GetIngresoCompensacionRepository().Delete(iPeriCodi, iIngrCompVersion);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
            return iPeriCodi;
        }

        /// <summary>
        /// Permite buscar IngresoxCompensacion en base al periodo
        /// </summary>
        /// <param name="iPeriCodi">Mes de valorización</param>
        /// <returns>Lista de IngresoCompensacionDTO</returns>
        public List<IngresoCompensacionDTO> BuscarIngresoCompensacion(int? iPeriCodi, int? version)
        {
            return FactoryTransferencia.GetIngresoCompensacionRepository().GetByCodigo(iPeriCodi, version);
        }

        /// <summary>
        /// Permite buscar IngresoxCompensacion en base al periodo y version
        /// </summary>
        /// <param name="iPeriCodi">Mes de valorización</param>
        /// <param name="iVersion">Versión del Mes de valorización</param>
        /// <returns>Lista de IngresoCompensacionDTO</returns>            
        public List<IngresoCompensacionDTO> ListByPeriodoVersion(int iPeriCodi, int iVersion)
        {
            return FactoryTransferencia.GetIngresoCompensacionRepository().ListByPeriodoVersion(iPeriCodi, iVersion);
        }

        /// <summary>
        /// Retorna el Ingreso por Potencia en función a los parametros
        /// </summary>
        /// <param name="iPeriCodi">Mes de valorización</param>
        /// <param name="iCabComCodi">Identificador del código de la compensación</param>
        /// <param name="iVersion">Versión del Mes de valorización</param>
        /// <param name="iEmprCodi">Código de la Empresa</param>
        /// <returns>IngresoCompensacionDTO</returns>            
        public IngresoCompensacionDTO GetByPeriVersCabCompEmpr(int iPeriCodi, int iCabComCodi, int iVersion, int iEmprCodi)
        {
            return FactoryTransferencia.GetIngresoCompensacionRepository().GetByPeriVersCabCompEmpr(iPeriCodi, iCabComCodi, iVersion, iEmprCodi);
        }

        /// <summary>
        /// Retorna la lista de empresas presentes en el periodo y versión
        /// </summary>
        /// <param name="iPeriCodi">Mes de valorización</param>
        /// <param name="iVersion">Versión del Mes de valorización</param>
        /// <returns>Lista de IngresoCompensacionDTO</returns>            
        public List<IngresoCompensacionDTO> BuscarListaEmpresas(int iPeriCodi, int iVersion)
        {
            return FactoryTransferencia.GetIngresoCompensacionRepository().BuscarListaEmpresas(iPeriCodi, iVersion);
        }

        /// <summary>
        /// Retorna el Total de todas las empresa por la Compensacion Rentas por Congestion
        /// </summary>
        /// <param name="iPeriCodi">Mes de valorización</param>
        /// <param name="iVersion">Versión del Mes de valorización</param>
        /// <returns>IngresoCompensacionDTO</returns>            
        public IngresoCompensacionDTO GetByPeriVersRentasCongestion(int iPeriCodi, int iVersion)
        {
            return FactoryTransferencia.GetIngresoCompensacionRepository().GetByPeriVersRentasCongestion(iPeriCodi, iVersion);
        }


    }
}
