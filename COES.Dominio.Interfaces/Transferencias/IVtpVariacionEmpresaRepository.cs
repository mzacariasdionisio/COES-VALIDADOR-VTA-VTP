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
    public interface IVtpVariacionEmpresaRepository
    {
        int Save(VtpVariacionEmpresaDTO entity);
        VtpVariacionEmpresaDTO GetDefaultPercentVariationByTipoComp(string varemptipocomp);
        void UpdateStatusVariationByTipoComp(VtpVariacionEmpresaDTO entity);
        void UpdateStatusVariationByTipoCompAndEmpresa(VtpVariacionEmpresaDTO entity);
        List<VtpVariacionEmpresaDTO> ListVariacionEmpresaByTipoComp(string varemptipocomp, int NroPagina, int PageSize, string varemprnomb);
        List<VtpVariacionEmpresaDTO> ListHistoryVariacionEmpresaByEmprCodiAndTipoComp(string varemptipocomp, int emprcodi);
        int GetNroRecordsVariacionEmpresaByTipoComp(string varemptipocomp, string varemprnomb);
        VtpVariacionEmpresaDTO GetPercentVariationByEmprCodiAndTipoComp(int emprcodi, string varemptipocomp);
    }
}
