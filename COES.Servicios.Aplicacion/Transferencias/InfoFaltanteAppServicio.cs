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
   public  class InfoFaltanteAppServicio: AppServicioBase
    {
        /// <summary>
        /// Permite realizar búsquedas de informacionfaltante en base al periodo 
        /// </summary>
        /// <param name="iPeriCodi">Mes de valorización</param>
        /// <returns>Lista de InfoFaltanteDTO</returns>
        public List<InfoFaltanteDTO> BuscarInfoFaltante(Int32 iPeriCodi)
        {
            return FactoryTransferencia.GetInfoFaltanteRepository().GetByCriteria(iPeriCodi);
        }

           /// <summary>
        /// Permite realizar búsquedas de información entregada fuera del periodo-version en base al periodo 
        /// </summary>
        /// <param name="iPeriCodi">Mes de valorización</param>
        /// <returns>Lista de InfoFaltanteDTO</returns>
        public List<InfoFaltanteDTO> BuscarInfoEntregada(int iPeriCodi,int iversion)
        {
            return FactoryTransferencia.GetInfoFaltanteRepository().ListByPeriodoVersion(iPeriCodi,iversion);
        }



    
    }
}
