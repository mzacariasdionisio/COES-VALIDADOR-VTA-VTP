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
    /// Interface de acceso a datos de la tabla VTP_PEAJE_EMPRESA_PAGO
    /// </summary>
    public interface IVtpPeajeEmpresaPagoRepository
    {
        int Save(VtpPeajeEmpresaPagoDTO entity);
        void Update(VtpPeajeEmpresaPagoDTO entity);
        void Delete(int pempagcodi);
        VtpPeajeEmpresaPagoDTO GetById(int pempagcodi);
        List<VtpPeajeEmpresaPagoDTO> List();
        List<VtpPeajeEmpresaPagoDTO> GetByCriteria();
        void DeleteByCriteria(int pericodi, int recpotcodi);
        List<VtpPeajeEmpresaPagoDTO> ListPeajePago(int pericodi, int recpotcodi);
        List<VtpPeajeEmpresaPagoDTO> ListPeajeCobro(int emprcodipago, int pericodi, int recpotcodi);
        List<VtpPeajeEmpresaPagoDTO> ListPeajeCobroNoTransm(int emprcodipago, int pericodi, int recpotcodi);
        List<VtpPeajeEmpresaPagoDTO> ListPeajeCobroReparto(int rrpecodi, int emprcodipago, int pericodi, int recpotcodi);
        decimal GetSaldoAnterior(int pempagpericodidest, int pingcodi, int emprcodipeaje, int emprcodicargo);
        List<VtpPeajeEmpresaPagoDTO> GetByIdSaldo(int pericodi, int recpotcodi, int pingcodi, int emprcodipeaje, int emprcodicargo);
        void UpdatePeriodoDestino(int pericodi, int recpotcodi, int pempagpericodidest);

        List<VtpPeajeEmpresaPagoDTO> ListPeajeCobroSelect(int pericodi, int recpotcodi);
        List<VtpPeajeEmpresaPagoDTO> ListPeajeCobroHistoricos(int emprcodipeaje, int pericodi, int recpotcodi);

        List<VtpPeajeEmpresaPagoDTO> GetPeajeEmpresaPagoByEmprCodiAndRecPotCodi(int emprcodicargo, int pericodi, int recpotcodi, int potcodi);


        #region SIOSEIN
        List<VtpPeajeEmpresaPagoDTO> ObtenerListVtpPeajeEmpresaPago(int pericodi, int recpotcodi, int? emprcodipeaje, string cargotransmision);

        List<VtpPeajeEmpresaPagoDTO> GetPeajeEmpresaPagoByComparative(int pericodiini, int pericodifin, int emprcodi, int emprcodicargo);
        List<VtpPeajeEmpresaPagoDTO> GetPeajeEmpresaPagoByComparativeUnique(int pericodiini, int pericodifin, int emprcodi, int emprcodicargo);
        List<VtpPeajeEmpresaPagoDTO> GetPeajeEmpresaPagoByCompHist(int pericodiini, int recpotcodiini, int pericodifin, int recpotcodifin, int emprcodi);
        List<VtpPeajeEmpresaPagoDTO> GetPeajeEmpresaPagoByCompHistUnique(int pericodiini, int recpotcodiini, int pericodifin, int recpotcodifin, int emprcodi);
        #endregion
        //CU21
        VtpPeajeEmpresaPagoDTO GetByCargoPrima(int pericodi, int recpotcodi, string pingnombre);
    }
}
