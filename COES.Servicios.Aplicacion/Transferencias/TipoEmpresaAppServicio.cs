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
   public class TipoEmpresaAppServicio: AppServicioBase
    {
        /// <summary>
        /// permite obtener los  tipoempresa
        /// </summary>
        /// <returns>Lista de TipoEmpresaDTO</returns>
        public List<TipoEmpresaDTO> ListTipoEmpresas()
        {
            return FactoryTransferencia.GetTipoEmpresaRepository().List();
        }
    }
}
