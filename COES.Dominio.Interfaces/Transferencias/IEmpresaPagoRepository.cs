using COES.Dominio.DTO.Transferencias;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.Interfaces.Transferencias
{
    /// <summary>
    /// Interface de acceso a datos de la tabla TRN_EMPRESA_PAGO
    /// </summary>
    public interface IEmpresaPagoRepository
    {
        int Save(EmpresaPagoDTO entity);
        void Delete(int PeriCodi, int Version);
        List<EmpresaPagoDTO> GetByCriteria(int pericodi, int version);
        List<EmpresaPagoDTO> GetEmpresaPositivaByCriteria(int pericodi, int version);
        List<EmpresaPagoDTO> GetEmpresaNegativaByCriteria(int pericodi, int version);
        List<EmpresaPagoDTO> ObtenerListaEmpresaPago(int pericodi, int emppagversion, int? emprcodi);
    }
}

