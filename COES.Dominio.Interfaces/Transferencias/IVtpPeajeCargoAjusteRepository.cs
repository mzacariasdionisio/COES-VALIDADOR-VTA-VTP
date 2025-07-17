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
    /// Interface de acceso a datos de la tabla VTP_PEAJE_CARGO_AJUSTE
    /// </summary>
    public interface IVtpPeajeCargoAjusteRepository
    {
        int Save(VtpPeajeCargoAjusteDTO entity);
        void Update(VtpPeajeCargoAjusteDTO entity);
        void Delete(int pecajcodi);
        void DeleteByCriteria(int pericodi);
        VtpPeajeCargoAjusteDTO GetById(int pecajcodi);
        List<VtpPeajeCargoAjusteDTO> List();
        List<VtpPeajeCargoAjusteDTO> GetByCriteria(int pericodi);
        decimal GetAjuste(int pericodi, int emprcodi, int pingcodi);
    }
}
