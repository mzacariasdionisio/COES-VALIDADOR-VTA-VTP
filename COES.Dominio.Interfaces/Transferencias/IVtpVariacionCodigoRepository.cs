using COES.Dominio.DTO.Transferencias;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.Interfaces.Transferencias
{
    /// <summary>
    /// Interface de acceso a datos de la tabla VTP_VARIACION_CODIGO
    /// </summary>
    public interface IVtpVariacionCodigoRepository
    {
        VtpVariacionCodigoDTO GetVariacionCodigoByCodVtp(string varcodcodigovtp);
        List<VtpVariacionCodigoDTO> ListVariacionCodigoByEmprCodi(int emprcodi, int NroPagina, int PageSize, string varCodiVtp);
        int GetNroRecordsVariacionCodigoByEmprCodi(int emprcodi,string varCodiVtp);
        List<VtpVariacionCodigoDTO> ListVariacionCodigoVTEAByEmprCodi(int emprcodi, int NroPagina, int PageSize, string varCodiVtp);
        int GetNroRecordsVariacionCodigoVTEAByEmprCodi(int emprcodi, string varCodiVtp);
        void UpdateStatusVariationByCodigoVtp(VtpVariacionCodigoDTO entity);
        int Save(VtpVariacionCodigoDTO entity);
        List<VtpVariacionCodigoDTO> ListHistoryVariacionCodigoByCodigoVtp(string varCodCodigoVtp, string varemptipocomp);
    }
}
