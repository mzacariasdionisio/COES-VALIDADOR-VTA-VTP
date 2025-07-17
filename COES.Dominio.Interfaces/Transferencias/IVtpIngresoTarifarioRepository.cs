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
    /// Interface de acceso a datos de la tabla VTP_INGRESO_TARIFARIO
    /// </summary>
    public interface IVtpIngresoTarifarioRepository
    {
        int Save(VtpIngresoTarifarioDTO entity);
        void Update(VtpIngresoTarifarioDTO entity);
        void Delete(int ingtarcodi);
        VtpIngresoTarifarioDTO GetById(int ingtarcodi);
        List<VtpIngresoTarifarioDTO> List();
        List<VtpIngresoTarifarioDTO> GetByCriteria();
        void DeleteByCriteria(int pericodi, int recpotcodi);
        List<VtpIngresoTarifarioDTO> ListEmpresaPago(int pericodi, int recpotcodi);
        List<VtpIngresoTarifarioDTO> ListEmpresaCobro(int emprcodingpot, int pericodi, int recpotcodi);
        List<VtpIngresoTarifarioDTO> ListEmpresaCobroParaMultEmprcodingpot(string emprcodingpot, int pericodi, int recpotcodi);
        VtpIngresoTarifarioDTO GetByIdSaldo(int pericodi, int recpotcodi, int pingcodi, int emprcodiping, int emprcodingpot);
        List<VtpIngresoTarifarioDTO> GetByCriteriaIngresoTarifarioSaldo(int pericodi, int recpotcodi, int pingcodi, int emprcodiping, int emprcodingpot);
        void UpdatePeriodoDestino(int pericodi, int recpotcodi, int ingtarpericodidest);
        decimal GetSaldoAnterior(int ingtarpericodidest, int pingcodi, int emprcodiping, int emprcodingpot);
    }
}
