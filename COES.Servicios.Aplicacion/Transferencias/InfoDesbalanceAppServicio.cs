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
    public class InfoDesbalanceAppServicio : AppServicioBase
    {
        /// <summary>
        /// Permite listar las Barras de Transferencias registradas en la InfoDesbalance
        /// </summary>
        /// <param name="iPeriCodi">Codigo del mes de valorización</param>
        /// <param name="iVersion">Versión de recalculo</param>
        /// <returns>Lista de InfoDesbalanceDTO</returns>
        public List<InfoDesbalanceDTO> GetByListaBarrasTrans(int iPeriCodi, int iVersion)
        {
            return FactoryTransferencia.GetInfoDesbalanceRepository().GetByListaBarrasTrans(iPeriCodi, iVersion);
        }

        /// <summary>
        /// Permite listar los calculos de Desbalance en un mes por Barra de Transferencia
        /// </summary>
        /// <param name="iPeriCodi">Codigo del mes de valorización</param>
        /// <param name="iVersion">Versión de recalculo</param>
        /// /// <param name="iBarrCodi">Código de la barra de transferencia</param>
        /// <returns>Lista de InfoDesbalanceDTO</returns>
        public List<InfoDesbalanceDTO> GetByListaInfoDesbalanceByBarra(int iPeriCodi, int iVersion, int iBarrCodi)
        {
            return FactoryTransferencia.GetInfoDesbalanceRepository().GetByListaInfoDesbalanceByBarra(iPeriCodi, iVersion, iBarrCodi);
        }




        /// <summary>
        /// Permite realizar búsquedas de InfoDesbalance en base al periodo
        /// </summary>
        /// <param name="iPeriCodi">Codigo del mes de valorización</param>
        /// <returns>Lista de InfoDesbalanceDTO</returns>
        //public List<InfoDesbalanceDTO> BuscarInfoDesbalanceMes(int iPeriCodi, int version, int desbalance)
        //{
        //    return FactoryTransferencia.GetInfoDesbalanceRepository().GetByCriteria(iPeriCodi, version, desbalance);
        //}

        /// <summary>
        /// Permite realizar búsquedas de InfoDesbalance en base al periodo
        /// </summary>
        /// <param name="iPeriCodi","version","barrcodi">Codigo del mes de valorización</param>
        /// <returns>Lista de InfoDesbalanceDTO</returns>
        //public List<InfoDesbalanceDTO> BuscarInfoDesbalanceDia(int iPeriCodi, int version, int barrcodi)
        //{
        //    return FactoryTransferencia.GetInfoDesbalanceRepository().GetDesbalanceDia(iPeriCodi, version, barrcodi);
        //}

        
    }
}
