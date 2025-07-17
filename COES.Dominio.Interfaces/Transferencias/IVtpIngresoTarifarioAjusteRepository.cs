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
    /// Interface de acceso a datos de la tabla VTP_INGRESO_TARIFARIO_AJUSTE
    /// </summary>
    public interface IVtpIngresoTarifarioAjusteRepository
    {
        int Save(VtpIngresoTarifarioAjusteDTO entity);
        void Update(VtpIngresoTarifarioAjusteDTO entity);
        void Delete(int ingtajcodi);
        void DeleteByCriteria(int pericodi);
        VtpIngresoTarifarioAjusteDTO GetById(int ingtajcodi);
        List<VtpIngresoTarifarioAjusteDTO> List();
        List<VtpIngresoTarifarioAjusteDTO> GetByCriteria(int pericodi);
        decimal GetAjuste(int pericodi, int emprcodiping, int pingcodi, int emprcodingpot);
    }
}
