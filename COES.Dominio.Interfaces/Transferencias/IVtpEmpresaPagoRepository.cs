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
    /// Interface de acceso a datos de la tabla VTP_EMPRESA_PAGO
    /// </summary>
    public interface IVtpEmpresaPagoRepository
    {
        int Save(VtpEmpresaPagoDTO entity);
        void Update(VtpEmpresaPagoDTO entity); 
        void Delete(int potepcodi);
        VtpEmpresaPagoDTO GetById(int potepcodi);
        List<VtpEmpresaPagoDTO> List(); 
        List<VtpEmpresaPagoDTO> GetByCriteria();
        List<VtpEmpresaPagoDTO> ListPago(int pericodi, int recpotcodi);
        List<VtpEmpresaPagoDTO> ListCobro(int emprcodipago, int pericodi, int recpotcodi);
        List<VtpEmpresaPagoDTO> ListCobroConsultaHistoricos(int emprcodipago, int pericodi, int recpotcodi);
        List<VtpEmpresaPagoDTO> GetEmpresaPagoByComparative(int pericodiini, int pericodifin, int emprcodi);
        List<VtpEmpresaPagoDTO> GetEmpresaPagoByComparativeUnique(int pericodiini, int pericodifin, int emprcodi);
        List<VtpEmpresaPagoDTO> GetEmpresaPagoByComparativeHistorico(int pericodiini, int pericodifin, int emprcodi);
        List<VtpEmpresaPagoDTO> GetEmpresaPagoByComparativeHistorico2(int pericodiini, int recpotcodiini, int pericodifin, int recpotcodifin, int emprcodi);
        List<VtpEmpresaPagoDTO> GetEmpresaPagoByHist(int pericodiini, int recpotcodiini, int pericodifin, int recpotcodifin, int emprcodi);
        List<VtpEmpresaPagoDTO> GetEmpresaPagoByHistUnique(int pericodiini, int recpotcodiini, int pericodifin, int recpotcodifin, int emprcodi);
        void DeleteByCriteria(int pericodi, int recpotcodi);

        #region SIOSEIN V2
        List<VtpEmpresaPagoDTO> ObtenerListaEmpresaPago(int pericodi, int recpotcodi, int? emprcodipago, int? emprcodicobro);
        #endregion
    }
}
