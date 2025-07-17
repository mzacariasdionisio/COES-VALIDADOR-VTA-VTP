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
   public  class InfoDesviacionAppServicio: AppServicioBase
   {
       /// <summary>
       /// Permite estraer la lista de Codigos de entrega y Retiro con su respectiva energia mensual
       /// </summary>
       /// <param name="iPeriCodi">Periodo de valorización</param>
       /// <param name="iVersion">Versión de recalculo</param>
       /// <param name="iBarCodi">Codigo de la barra de transferencia</param>
       /// <returns>Lista de InfoDesviacionDTO</returns>
       public List<InfoDesviacionDTO> GetByListaCodigo(int iPeriCodi, int iVersion, int iBarrCodi)
       {
           return FactoryTransferencia.GetInfoDesviacionRepository().GetByListaCodigo(iPeriCodi, iVersion, iBarrCodi);
       }


       /// <summary>
       /// Permite extraer La energia acumulada en N dias por codigo y barra
       /// </summary>
       /// <param name="iPeriCodi">Periodo de valorización</param>
       /// <param name="iVersion">Versión de recalculo</param>
       /// <param name="iBarCodi">Codigo de la barra de transferencia</param>
       /// <param name="sCodigo">Codigo de Entrega o Retiro</param>
       /// <returns>Lista de InfoDesviacionDTO</returns>
       public InfoDesviacionDTO GetByEnergiaByBarraCodigo(int iPeriCodi, int iVersion, int iBarrCodi, string sCodigo)
       {
           return FactoryTransferencia.GetInfoDesviacionRepository().GetByEnergiaByBarraCodigo(iPeriCodi, iVersion, iBarrCodi, sCodigo);
       }
   }
}
