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
    /// Interface de acceso a datos de la tabla VTP_SALDO_EMPRESA_AJUSTE
    /// </summary>
    public interface IVtpSaldoEmpresaAjusteRepository
    {
        int Save(VtpSaldoEmpresaAjusteDTO entity);
        void Update(VtpSaldoEmpresaAjusteDTO entity);
        void Delete(int potseacodi);
        void DeleteByCriteria(int pericodi);
        VtpSaldoEmpresaAjusteDTO GetById(int potseacodi);
        List<VtpSaldoEmpresaAjusteDTO> List();
        List<VtpSaldoEmpresaAjusteDTO> GetByCriteria(int pericodi);
        decimal GetAjuste(int pericodi, int emprcodi);
    }
}
