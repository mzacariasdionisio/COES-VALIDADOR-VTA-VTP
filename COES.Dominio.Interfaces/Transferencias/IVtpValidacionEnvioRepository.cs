using COES.Dominio.DTO.Transferencias;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.Interfaces.Transferencias
{
    /// <summary>
    /// Interface de acceso a datos de la tabla VTP_VALIDACION_ENVIO
    /// </summary>
    public interface IVtpValidacionEnvioRepository
    {
        int Save(VtpValidacionEnvioDTO entity);
        List<VtpValidacionEnvioDTO> GetValidacionEnvioByPegrcodi(int pegrcodi);
    }
}
