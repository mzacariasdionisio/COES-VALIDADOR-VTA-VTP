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
    /// Interface de acceso a datos de la tabla VTP_PEAJE_SALDO_TRANSMISION
    /// </summary>
    public interface IVtpPeajeSaldoTransmisionRepository
    {
        int Save(VtpPeajeSaldoTransmisionDTO entity);
        void Update(VtpPeajeSaldoTransmisionDTO entity);
        void Delete(int pstrnscodi);
        VtpPeajeSaldoTransmisionDTO GetById(int pstrnscodi);
        List<VtpPeajeSaldoTransmisionDTO> List();
        List<VtpPeajeSaldoTransmisionDTO> GetByCriteria(int pericodi, int recpotcodi);
        void DeleteByCriteria(int pericodi, int recpotcodi);
        List<VtpPeajeSaldoTransmisionDTO> ListEmpresaEgreso(int pericodi, int recpotcodi);
        VtpPeajeSaldoTransmisionDTO GetByIdEmpresa(int pericodi, int recpotcodi, int emprcodi);
    }
}
