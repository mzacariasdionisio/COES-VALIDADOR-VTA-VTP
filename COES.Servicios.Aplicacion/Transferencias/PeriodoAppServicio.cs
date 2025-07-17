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
    public class PeriodoAppServicio : AppServicioBase
    {

        /// <summary>
        /// Inserta (PeriCodi == 0) o Actualiza (PeriCodi != 0) un Periodo
        /// </summary>
        /// <param name="entity">PeriodoDTO</param>
        /// <returns>Retornar el Código insertado o actualizado de la tabla TRN_PERIODO</returns>       
        public int SaveOrUpdatePeriodo(PeriodoDTO entity)
        {
            try
            {
                int id = 0;

                if (entity.PeriCodi == 0)
                {
                    id = FactoryTransferencia.GetPeriodoRepository().Save(entity);
                }
                else
                {
                    int periCodiFirstFormatNew = FactoryTransferencia.GetPeriodoRepository().GetFirstPeriodoFormatNew();
                    entity.PeriFormNuevo = (entity.PeriCodi >= periCodiFirstFormatNew && periCodiFirstFormatNew != 0) ? 1 : 0;
                    FactoryTransferencia.GetPeriodoRepository().Update(entity);
                    id = entity.PeriCodi;
                }
                return id;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla TRN_PERIODO en base al iPeriCodi
        /// </summary>
        /// <param name="iPeriCodi">Mes de Valorización</param>
        /// <returns>Retornar el Código eliminado de la tabla TRN_PERIODO</returns>       
        public int DeletePeriodo(int iPeriCodi)
        {
            try
            {
                FactoryTransferencia.GetPeriodoRepository().Delete(iPeriCodi);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
            return iPeriCodi;
        }

        /// <summary>
        /// Permite obtener el Periodo en base al iPeriCodi
        /// </summary>
        /// <param name="iPeriCodi">Mes de Valorización</param>
        /// <returns>PeriodoDTO</returns>
        public PeriodoDTO GetByIdPeriodo(System.Int32? iPeriCodi)
        {
            return FactoryTransferencia.GetPeriodoRepository().GetById(iPeriCodi);
        }

        /// <summary>
        /// Permite listar todas los Periodos
        /// </summary>
        /// <returns>Lista de PeriodoDTO</returns>
        public List<PeriodoDTO> ListPeriodo()
        {
            return FactoryTransferencia.GetPeriodoRepository().List();
        }

        public List<PeriodoDTO> ListPeriodoPotencia()
        {
            return FactoryTransferencia.GetPeriodoRepository().ListPeriodoPotencia();
        }

        /// <summary>
        /// Permite realizar búsquedas de un Periodo en base al sPeriNombre
        /// </summary>
        /// <param name="sPeriNombre">Nombre del Mes de Valorización</param>
        /// <returns>Lista de PeriodoDTO</returns>
        public List<PeriodoDTO> BuscarPeriodo(string sPeriNombre)
        {
            return FactoryTransferencia.GetPeriodoRepository().GetByCriteria(sPeriNombre);
        }

        /// <summary>
        /// Permite realizar búsquedas de un Periodo en base al Anio y Mes del periodo
        /// </summary>
        /// <param name="iPeriAnioMes"></param>
        /// <returns>PeriodoDTO</returns>
        public PeriodoDTO GetByAnioMes(int iPeriAnioMes)
        {
            return FactoryTransferencia.GetPeriodoRepository().GetByAnioMes(iPeriAnioMes);
        }

        /// <summary>
        /// Retorna el numero de registros de otras tablas por este PeriCodi
        /// </summary>
        /// <param name="iPeriCodi">Mes de Valorización</param>
        /// <returns>Numero de registros asociados al iPeriCodi</returns>
        public int GetNumRegistros(int iPeriCodi)
        {
            int id = 0;
            try
            {
                id = FactoryTransferencia.GetPeriodoRepository().GetNumRegistros(iPeriCodi);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
            return id;
        }

        /// <summary>
        /// Retorna la lista de los periodos segun el estado sPeriEstado)
        /// </summary>
        /// <param name="sPeriEstado">Estado en que se encuentra el Periodo</param>
        /// <returns>Lista de PeriodoDTO</returns>
        public List<PeriodoDTO> ListarByEstado(string sPeriEstado)
        {
            return FactoryTransferencia.GetPeriodoRepository().ListarByEstado(sPeriEstado);
        }

        public List<PeriodoDTO> ListarByEstadoPublicarCerrado()
        {
            return FactoryTransferencia.GetPeriodoRepository().ListarByEstadoPublicarCerrado();
        }

        /// <summary>
        /// Retorna el periodo anterior al pasado como dato
        /// </summary>
        /// <param name="iPeriCodi">Código del periodo de valorización actual</param>
        /// <returns>PeriodoDTO</returns>
        public PeriodoDTO BuscarPeriodoAnterior(int iPeriCodi)
        {
            return FactoryTransferencia.GetPeriodoRepository().BuscarPeriodoAnterior(iPeriCodi);
        }

        /// <summary>
        /// Retorna la lista de los periodos futuros al pericodi actual
        /// </summary>
        /// <param name="iPeriCodi">Codigo del Periodo Actual</param>
        /// <returns>Lista de PeriodoDTO</returns>
        public List<PeriodoDTO> ListarPeriodosFuturos(int iPeriCodi)
        {
            return FactoryTransferencia.GetPeriodoRepository().ListarPeriodosFuturos(iPeriCodi);
        }

        //2018.Setiembre - Agregados por ASSETEC
        /// <summary>
        /// Retorna el numero del siguiente Primary key de la tabla TRN_CONTADOR
        /// </summary>
        /// <param name="trncnttabla">Nombre de la tabla</param>
        /// <param name="trncntcolumna">Nombre de la columna</param>
        /// <returns>Primary key de la tabla asociada</returns>
        public int GetPKTrnContador(string trncnttabla, string trncntcolumna)
        {
            int id = 0;
            try
            {
                id = FactoryTransferencia.GetPeriodoRepository().GetPKTrnContador(trncnttabla, trncntcolumna);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
            return id;
        }
        /// <summary>
        /// Actualiza el numero del siguiente Primary key disponible de la tabla TRN_CONTADOR
        /// </summary>
        /// <param name="trncnttabla">Nombre de la tabla</param>
        /// <param name="trncntcolumna">Nombre de la columna</param>
        public void UpdatePKTrnContador(string trncnttabla, string trncntcolumna, Int32 trncntcontador)
        {
            try
            {
                FactoryTransferencia.GetPeriodoRepository().UpdatePKTrnContador(trncnttabla, trncntcolumna, trncntcontador);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

    }
}
