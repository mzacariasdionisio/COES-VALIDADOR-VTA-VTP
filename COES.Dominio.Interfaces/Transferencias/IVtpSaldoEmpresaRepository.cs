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
    /// Interface de acceso a datos de la tabla VTP_SALDO_EMPRESA
    /// </summary>
    public interface IVtpSaldoEmpresaRepository
    {
        int Save(VtpSaldoEmpresaDTO entity);
        void Update(VtpSaldoEmpresaDTO entity);
        void Delete(int potsecodi);
        VtpSaldoEmpresaDTO GetById(int potsecodi);
        List<VtpSaldoEmpresaDTO> List(int pericodi, int recpotcodi);
        List<VtpSaldoEmpresaDTO> GetByCriteria();
        List<VtpSaldoEmpresaDTO> ListCalculaSaldo(int pericodi, int recpotcodi);
        void DeleteByCriteria(int pericodi, int recpotcodi);
        List<VtpSaldoEmpresaDTO> ListPositiva(int pericodi, int recpotcodi);
        List<VtpSaldoEmpresaDTO> ListNegativa(int pericodi, int recpotcodi);
        VtpSaldoEmpresaDTO GetByIdSaldo(int pericodi, int recpotcodi, int emprcodi);
        List<VtpSaldoEmpresaDTO> GetByIdSaldoGeneral(int pericodi, int recpotcodi, int emprcodi);
        void UpdatePeriodoDestino(int pericodi, int recpotcodi, int potsepericodidest);
        decimal GetSaldoAnterior(int potsepericodidest, int emprcodi);
        List<VtpSaldoEmpresaDTO> ListPeriodosDestino(int potsepericodidest);
        VtpSaldoEmpresaDTO GetSaldoEmpresaPeriodo(int emprcodi, int pericodi, int potsepericodidest);
    }
}
