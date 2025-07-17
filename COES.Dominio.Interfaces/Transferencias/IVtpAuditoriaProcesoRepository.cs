using COES.Dominio.DTO.Transferencias;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.Interfaces.Transferencias
{
    /// <summary>
    /// Interface de acceso a datos de la tabla VTP_VARIACION_EMPRESA
    /// </summary>
    public interface IVtpAuditoriaProcesoRepository
    {

        int Save(VtpAuditoriaProcesoDTO entity);

        List<VtpAuditoriaProcesoDTO> ListAuditoriaProcesoByFiltro(string audprousucreacion, int tipprocodi, DateTime audprofeccreacioni, DateTime audprofeccreacionf, int NroPagina, int PageSize);
        int NroRegistroAuditoriaProcesoByFiltro(string audprousucreacion, int tipprocodi, DateTime audprofeccreacioni, DateTime audprofeccreacionf);
    }
}
