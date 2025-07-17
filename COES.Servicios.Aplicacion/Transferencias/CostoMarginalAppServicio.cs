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
    public class CostoMarginalAppServicio : AppServicioBase
    {

        /// <summary>
        /// Inserta un nuevo Costo Marinal
        /// </summary>
        /// <param name="CostoMarginalDTO">Entidad del Costo Marginal</param>
        /// <returns>Codigo del Nuevo Costo Marginal</returns>
        public int SaveCostoMarginal(CostoMarginalDTO entity)
        {
            try
            {
                int id = 0;
                if (entity.CosMarCodi == 0)
                {
                    id = FactoryTransferencia.GetCostoMarginalRepository().Save(entity);
                }

                return id;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina Todos los Costos Marginales del Periodo : PeriCodi y Versión
        /// </summary>
        /// <param name="iPeriCodi">Código del Periodo</param>
        /// <param name="iCostMargVersion">Versión dentro del Periodo</param>
        /// <returns>El mes de valorización</returns>
        public int DeleteListaCostoMarginal(int iPeriCodi, int iCostMargVersion)
        {
            int iResultado = iPeriCodi;
            try
            {
                FactoryTransferencia.GetCostoMarginalRepository().Delete(iPeriCodi, iCostMargVersion);
            }
            catch (Exception ex)
            {
                iResultado = 0;
                throw new Exception(ex.Message, ex);
            }
            return iResultado;
        }

        /// <summary>
        /// Permite listar los Costos Marginales
        /// </summary>
        /// <param name="iPeriCodi">Código del Periodo</param>
        /// <param name="iCostMargVersion">Versión dentro del Periodo</param>
        /// <returns>Lista de CostoMarginalDTO</returns>
        public List<CostoMarginalDTO> ListCostoMarginal(int iPeriCodi, int iCostMargVersion)
        {
            return FactoryTransferencia.GetCostoMarginalRepository().List(iPeriCodi, iCostMargVersion);
        }

        /// <summary>
        /// Permite realizar búsquedas los Costos Marginales
        /// </summary>
        /// <param name="iPeriCodi">Código del Periodo</param>
        /// <param name="iBarrCodi">Código de la Barra de Transferencia</param>
        /// <returns>Lista de CostoMarginalDTO</returns>
        public List<CostoMarginalDTO> BuscarCostoMarginal(int iPeriCodi, string iBarrCodi)
        {
            return FactoryTransferencia.GetCostoMarginalRepository().GetByCriteria(iPeriCodi, iBarrCodi);
        }



        /// <summary>
        /// Lista costso marginales en base a valor de transferencia por  codigo de barra, periodo ,version
        /// </summary>
        /// <param name="iBarrCodi"></param>
        /// <param name="iPeriCodi"></param>
        /// <param name="iCostMargVersion"></param>
        ///<returns>Lista de CostoMarginalDTO</returns>
        public List<CostoMarginalDTO> ListValorTransferenciaByBarraPeridoVersion(int iBarrCodi, int iPeriCodi, int iCostMargVersion)
        {
            return FactoryTransferencia.GetCostoMarginalRepository().ListByBarrPeriodoVersion(iBarrCodi, iPeriCodi, iCostMargVersion);
        }

        /// <summary>
        /// Permite listar los Costos Marginales en base al periodo
        /// </summary>
        /// <param name="iPeriCodi">Código del Periodo</param>
        /// <returns>Lista de CostoMarginalDTO</returns>
        public List<CostoMarginalDTO> ListCostoMarginal(int? iPeriCodi)
        {
            return FactoryTransferencia.GetCostoMarginalRepository().GetByCodigo(iPeriCodi);
        }

        /// <summary>
        /// Permite listar los Costos Marginales en base a su transferencia por  periodo y version
        /// </summary>
        /// <param name="iPeriCodi">Código del Periodo</param>
        /// <param name="iCostMargVersion">Versión</param>
        /// <returns>Lista de CostoMarginalDTO</returns>
        public List<CostoMarginalDTO> ListNombreBarraTransferencia(int iPeriCodi, int iCostMargVersion)
        {
            return FactoryTransferencia.GetCostoMarginalRepository().GetByBarraTransferencia(iPeriCodi, iCostMargVersion);
        }

        /// <summary>
        /// Lista  los costos marginales por barra ,pericodi,version
        /// </summary>
        /// <param name="iBarrCodi"></param>
        /// <param name="iPeriCodi"></param>
        /// <param name="iCostMargVersion"></param>
        /// <returns>Lista de CostoMarginalDTO</returns>
        public List<CostoMarginalDTO> ListCostoMarginalByBarraPeridoVersion(int iBarrCodi, int iPeriCodi, int iCostMargVersion)
        {
            return FactoryTransferencia.GetCostoMarginalRepository().ListByBarrPeriodoVersion(iBarrCodi, iPeriCodi, iCostMargVersion);
        }

        /// <summary>
        /// Permite listar los Costos Marginales
        /// </summary>
        /// <param name="iFacPerCodi">Código del Factor de Perdida</param>
        /// <returns>Lista de CostoMarginalDTO</returns>
        public List<CostoMarginalDTO> ListByFactorPerdida(int iFacPerCodi)
        {
            return FactoryTransferencia.GetCostoMarginalRepository().ListByFactorPerdida(iFacPerCodi);
        }


        /// <summary>
        /// Permite listar los Costos Marginales Pero de las Barras que si estan habilitadas para la Extranet - BarrBarraBgr = SI
        /// </summary>
        /// <param name="iPeriCodi">Código del Periodo</param>
        /// <param name="iCostMargVersion">Versión dentro del Periodo</param>
        /// <returns>Lista de CostoMarginalDTO</returns>
        public List<CostoMarginalDTO> ListCostoMarginalByReporte(int iPeriCodi, int iCostMargVersion)
        {
            return FactoryTransferencia.GetCostoMarginalRepository().ListByReporte(iPeriCodi, iCostMargVersion);
        }

        /// <summary>
        /// Permite obtener un registro de la tabla TRN_COSTO_MARGINAL filtrado por URS
        /// </summary>
        /// <param name="PeriCodi">Código del Periodo</param>
        /// <param name="iCostMargVersion">Código de la Versión de Recálculo de VEA</param>
        /// <param name="BarrCodi">Identificador de la tabla TRN_BARRA</param>
        /// <param name="CosMarDia">Dia registrado para el costo marginal</param>
        public CostoMarginalDTO GetByIdCostoMarginalPorBarraDia(int PeriCodi, int iCostMargVersion, int BarrCodi, int CosMarDia)
        {
            return FactoryTransferencia.GetCostoMarginalRepository().GetByIdPorBarraDia(PeriCodi, iCostMargVersion, BarrCodi, CosMarDia);
        }

        /// <summary>
        /// Inserta de forma masiva una lista de ValorCostoMarginalDTO
        /// </summary>
        /// <param name="entity">ValorCostoMarginalDTO</param>    
        public void BulkInsertValorCostoMarginal(List<TrnCostoMarginalBullk> entitys)
        {
            try
            {
                FactoryTransferencia.GetCostoMarginalRepository().BulkInsert(entitys);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener el siguiente Primary Key de la tabla
        /// </summary>
        public int GetCodigoGenerado()
        {
            return FactoryTransferencia.GetCostoMarginalRepository().GetCodigoGenerado();
        }

        /// <summary>
        /// Permite obtener el siguiente en menor valor del Primary Key de la tabla
        /// </summary>
        public int GetCodigoGeneradoDec()
        {
            return FactoryTransferencia.GetCostoMarginalRepository().GetCodigoGeneradoDec();
        }

        //CU21
        /// <summary>
        /// Obtener lista de Costos Marginales de cada 15 min por el codigo de Entrega
        /// </summary>
        /// <param name="iPeriCodi">Código de Periodo</param>
        /// <param name="iRecaCodi">Versión de Recalculo</param>
        /// <param name="iCodEntCodi">Código de Entrega</param>
        /// <returns>Lista de ValorTransferenciaDTO</returns> 
        public List<CostoMarginalDTO> ListCostoMarginalByCodigoEntrega(int iPeriCodi, int iRecaCodi, int iCodEntCodi)
        {
            return FactoryTransferencia.GetCostoMarginalRepository().ListarByCodigoEntrega(iPeriCodi, iRecaCodi, iCodEntCodi);
        }
    }
}
