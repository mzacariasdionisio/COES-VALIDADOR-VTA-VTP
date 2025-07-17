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
    /// Interface de acceso a datos de la tabla VTP_PEAJE_EMPRESA
    /// </summary>
    public interface IVtpPagoEgresoRepository
    {
        int Save(VtpPagoEgresoDTO entity);
        void Update(VtpPagoEgresoDTO entity);
        void Delete(int pagegrcodi);
        VtpPagoEgresoDTO GetById(int pagegrcodi);
        List<VtpPagoEgresoDTO> List(int pericodi, int recpotcodi);
        List<VtpPagoEgresoDTO> GetByCriteria(int pericodi, int recpotcodi);
        void DeleteByCriteria(int pericodi, int recpotcodi);
    }
}
