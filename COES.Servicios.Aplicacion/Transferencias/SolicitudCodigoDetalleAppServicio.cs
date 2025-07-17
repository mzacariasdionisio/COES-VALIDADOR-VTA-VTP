using COES.Base.Core;
using COES.Dominio.DTO.Transferencias;
using COES.Servicios.Aplicacion.Factory;
using System;
using System.Collections.Generic;

namespace COES.Servicios.Aplicacion.Transferencias
{
    public class SolicitudCodigoDetalleAppServicio : AppServicioBase
    {
        /// <summary>
        /// Permite listar los suministros relacionados
        /// </summary>
        /// <param name="codBarraTra">Codigo de barra de transferencia</param>       
        /// <returns>Lista de SolicitudCodigoDetalleDTO</returns>
        public List<SolicitudCodigoDetalleDTO> ListaRelacion(int codBarraTra)
        {
            return FactoryTransferencia.GetSolicitudCodigoDetalleRepository().ListaRelacion(codBarraTra);
        }
    }
}
