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
    public class TipoTramiteAppServicio: AppServicioBase
    {
        /// <summary>
        /// Inserta o actualiza un registro de tipotramite
        /// </summary>
        /// <returns>Lista de TipoTramiteDTO</returns>
        public List<TipoTramiteDTO> ListTipoTramite()
        {
            return FactoryTransferencia.GetTipoTramiteRepository().List();
        }

        /// <summary>
        /// Permite obtener tipotramite en base a su id
        /// </summary>
        /// <param name="iTipTrmCodi"></param>        
        /// <returns>TipoTramiteDTO</returns>
        public TipoTramiteDTO GetByIdTipoTramite(System.Int32? iTipTrmCodi)
        {
            return FactoryTransferencia.GetTipoTramiteRepository().GetById(iTipTrmCodi);
        }
    }
}
