using COES.Dominio.DTO.Transferencias;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.Interfaces.Transferencias
{
    /// <summary>
    /// Interface de acceso a datos de la tabla TRN_TRANS_ENTREGA y TRN_TRANS_RETIRO
    /// </summary>
    public interface IEnvioInformacionRepository 
    {
        List<ExportExcelDTO> GetByCriteria(int iPeriCodi, int iEmprCodi);
        List<ExportExcelDTO> ListVistaTodo(int iPeriCodi, int iEmprCodi, int iBarrCodi);
        List<ExportExcelDTO> GetByListCodigoInfoBase(int iPeriCodi, int iEmprCodi);
        /* ASSETEC 202001 */
        List<ExportExcelDTO> GetByListCodigoInfoBaseByEnvio(int trnenvcodi);
        List<ExportExcelDTO> GetByListCodigoModelo(int pericodi, int emprcodi, int trnmodcodi);
        List<ExportExcelDTO> GetByListCodigoModeloVTA(int pericodi, int emprcodi, int trnmodcodi);
    }
}

