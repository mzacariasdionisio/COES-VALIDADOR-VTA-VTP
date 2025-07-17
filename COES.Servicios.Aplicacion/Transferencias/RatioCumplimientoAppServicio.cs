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
    public  class RatioCumplimientoAppServicio: AppServicioBase
    {
        /// <summary>
        /// Obtiene  los ratio de cumplimiento en base al tipoempresa ,periodo y version
        /// </summary>
        /// <param name="iTipoEmprCodi">Tipo de empresa</param>
        /// <param name="iPeriCodi">Mes de valorización</param>
        /// <param name="iVersion">Versión del Mes de valorización</param>
        /// <returns>Lista de RatioCumplimientoDTO</returns>       
        public List<RatioCumplimientoDTO> ListRatioCumplimiento(int? iTipoEmprCodi, int? iPeriCodi, int iVersion)
        {
            return FactoryTransferencia.GetRatioCumplimientoRepository().GetByCodigo(iTipoEmprCodi, iPeriCodi, iVersion);
        }

        /// <summary>
        /// Obtiene  los ratio de cumplimiento en base nombre
        /// </summary>
        /// <param name="sNombre">Nombre del ratio</param>
        /// <returns>Lista RatioCumplimientoDTO</returns> 
        public List<RatioCumplimientoDTO> BuscarRatio(string nombre)
        {
            return FactoryTransferencia.GetRatioCumplimientoRepository().GetByCriteria(nombre);
        }
    }
}
