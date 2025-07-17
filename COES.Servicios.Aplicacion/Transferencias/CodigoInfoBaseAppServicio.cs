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
    public class CodigoInfoBaseAppServicio : AppServicioBase
    {
        /// <summary>
        /// Permite grabar o actualizar un  CodigoInfoBaseDTO en base a la entidad
        /// </summary>
        /// <param name="entity">Entidad de CodigoInfoBaseDTO</param>
        /// <returns>Retorna el iCoInfBCodi nuevo o actualizado</returns>
        public int SaveOrUpdateCodigoInfoBase(CodigoInfoBaseDTO entity)
        {
            try
            {
                int id = 0;

                if (entity.CoInfBCodi == 0)
                {
                    id = FactoryTransferencia.GetCodigoInfoBaseRepository().Save(entity);
                }
                else
                {
                    FactoryTransferencia.GetCodigoInfoBaseRepository().Update(entity);
                    id = entity.CoInfBCodi;
                }

                return id;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un CodigoInfoBase en base al iCoInfBCodi
        /// </summary>
        /// <param name="iCoInfBCodi">Código de la tabla TRN_CODIGO_INFOBASE</param>
        /// <returns>Retorna el iCoInfBCodi eliminado</returns>
        public int DeleteCodigoInfoBase(int iCoInfBCodi)
        {
            try
            {
                FactoryTransferencia.GetCodigoInfoBaseRepository().Delete(iCoInfBCodi);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
            return iCoInfBCodi;
        }

        /// <summary>
        /// Permite obtener el CodigoInfoBase en base al iCoInfBCodi
        /// </summary>
        /// <param name="iCoInfBCodi">Código de la tabla TRN_CODIGO_INFOBASE</param>
        /// <returns>CodigoInfoBaseDTO</returns>
        public CodigoInfoBaseDTO GetByIdCodigoInfoBase(int iCoInfBCodi)
        {
            return FactoryTransferencia.GetCodigoInfoBaseRepository().GetById(iCoInfBCodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla TRN_CODIGO_INFOBASE
        /// </summary>
        /// <returns>Lista de CodigoInfoBaseDTO</returns>
        public List<CodigoInfoBaseDTO> ListCodigoInfoBase()
        {
            return FactoryTransferencia.GetCodigoInfoBaseRepository().List();
        }

        /// <summary>
        /// Permite realizar búsquedas de CodigoInfoBase en base a cualquiera de los parametros
        /// </summary>
        /// <param name="sEmprNombre">Nombre de la Empresa</param>        
        /// <param name="sBarrBarraTransferencia">Nombre de la Barra de Transferencia</param>        
        /// <param name="sEquiNomb">Nombre de la Central de generación</param>        
        /// <param name="dCoInfBFechaInicio">Fecha en que inicia el código de información base, puede ser nulo</param>        
        /// <param name="dCoInfBFechaFin">Fecha en que concluye el código de información base, puede ser nulo</param>        
        /// <param name="sCoInfBEstado">Estado en que se encuentra el código de información base</param>        
        /// <returns>Lista de CodigoInfoBaseDTO</returns>
        public List<CodigoInfoBaseDTO> BuscarCodigoInfoBase(string sEmprNombre, string sBarrBarraTransferencia, string sEquiNomb, DateTime? dCoInfBFechaInicio, DateTime? dCoInfBFechaFin, string sCoInfBEstado,string codinfobase, int NroPagina, int PageSizeCodigoInfoBase)
        {
            return FactoryTransferencia.GetCodigoInfoBaseRepository().GetByCriteria(sEmprNombre, sBarrBarraTransferencia, sEquiNomb, dCoInfBFechaInicio, dCoInfBFechaFin, sCoInfBEstado, codinfobase, NroPagina, PageSizeCodigoInfoBase);
        }

        /// <summary>
        /// Permite realizar búsquedas de CodigoInfoBase en base a cualquiera de los parametros
        /// </summary>
        /// <param name="sEmprNombre">Nombre de la Empresa</param>        
        /// <param name="sBarrBarraTransferencia">Nombre de la Barra de Transferencia</param>        
        /// <param name="sEquiNomb">Nombre de la Central de generación</param>        
        /// <param name="dCoInfBFechaInicio">Fecha en que inicia el código de información base, puede ser nulo</param>        
        /// <param name="dCoInfBFechaFin">Fecha en que concluye el código de información base, puede ser nulo</param>        
        /// <param name="sCoInfBEstado">Estado en que se encuentra el código de información base</param>        
        /// <returns>Numero de filas de la consulta</returns>
        public int ObtenerNroFilasCodigoInfoBase(string sEmprNombre, string sBarrBarraTransferencia, string sEquiNomb, DateTime? dCoInfBFechaInicio, DateTime? dCoInfBFechaFin, string sCoInfBEstado,string codinfobase)
        {
            return FactoryTransferencia.GetCodigoInfoBaseRepository().ObtenerNroRegistros(sEmprNombre, sBarrBarraTransferencia, sEquiNomb, dCoInfBFechaInicio, dCoInfBFechaFin, sCoInfBEstado,codinfobase);
        }

        /// <summary>
        /// Permite obtener un codigoentrega  mediante su codigo de información base
        /// </summary>
        /// <param name="sCoInfBCodigo">Código asignado de la tabla</param>
        /// <returns>CodigoInfoBaseDTO</returns>
        public CodigoInfoBaseDTO GetByCodigoInfoBaseCodigo(string sCoInfBCodigo)
        {
            return FactoryTransferencia.GetCodigoInfoBaseRepository().GetByCoInfBCodigo(sCoInfBCodigo);
        }

        /// <summary>
        /// Permite obtener un codigoinformacionbase vigente en el periodo
        /// </summary>
        /// <param name="iPericodi">Periodo de valorización</param>
        /// <param name="sCodigo">Códigoa asignado</param>
        /// <returns>CodigoEntregaDTO</returns>
        public CodigoInfoBaseDTO CodigoInfoBaseVigenteByPeriodo(int iPericodi, string sCodigo)
        {
            return FactoryTransferencia.GetCodigoInfoBaseRepository().CodigoInfoBaseVigenteByPeriodo(iPericodi, sCodigo);
        }
    }
}
