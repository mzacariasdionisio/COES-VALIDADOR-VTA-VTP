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
    /// Interface de acceso a datos de la tabla VTP_PEAJE_CARGO
    /// </summary>
    public interface IVtpPeajeCargoRepository
    {
        int Save(VtpPeajeCargoDTO entity);
        void Update(VtpPeajeCargoDTO entity);
        void Delete(int pecarcodi);
        VtpPeajeCargoDTO GetById(int pecarcodi);
        List<VtpPeajeCargoDTO> List();
        List<VtpPeajeCargoDTO> GetByCriteria();
        void DeleteByCriteria(int pericodi, int recpotcodi);
        List<VtpPeajeCargoDTO> ListEmpresa(int pericodi, int recpotcodi);
        List<VtpPeajeCargoDTO> ListPagoNo(string emprcodi, int pericodi, int recpotcodi);
        List<VtpPeajeCargoDTO> ListPagoAdicional(int pericodi, int recpotcodi, int pingcodi);
        VtpPeajeCargoDTO GetByIdSaldo(int pericodi, int recpotcodi, int emprcodi, int pingcodi);
        void UpdatePeriodoDestino(int pericodi, int recpotcodi, int pecarpericodidest);
        decimal GetSaldoAnterior(int pecarpericodidest, int emprcodi, int pingcodi);
    }
}
