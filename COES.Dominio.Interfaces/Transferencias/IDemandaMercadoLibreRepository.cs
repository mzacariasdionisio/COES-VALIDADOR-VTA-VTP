using COES.Base.Core;
using COES.Dominio.DTO.Transferencias;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.Interfaces.Transferencias
{
    /// <summary>
    /// Interface de acceso a datos
    /// </summary>
    public interface IDemandaMercadoLibreRepository : IRepositoryBase
    {
        List<DemandaMercadoLibreDTO> ListDemandaMercadoLibre(DateTime[] periodos, DateTime periodoMes, int tipoEmpresa, string empresas,
            int codigoLectura, int codigoOrigenLectura);
    }
}
