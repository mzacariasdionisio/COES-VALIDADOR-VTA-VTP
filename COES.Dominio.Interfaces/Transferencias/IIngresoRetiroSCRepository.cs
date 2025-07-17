using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COES.Dominio.DTO.Transferencias;

namespace COES.Dominio.Interfaces.Transferencias
{
    /// <summary>
    /// Interface de acceso a datos de la tabla TRN_ING_RETIROSC
    /// </summary>
    public interface IIngresoRetiroSCRepository
    {
        int Save(IngresoRetiroSCDTO entity);
        void Delete(System.Int32 PeriCodi, System.Int32 Ingrscversion);
        List<IngresoRetiroSCDTO> GetByCodigo(int? pericodi, int? version);
        List<IngresoRetiroSCDTO> GetByCriteria(int pericodi, int version);
        List<IngresoRetiroSCDTO> ListByPeriodoVersion(int iPericodi, int iVersion);
        IngresoRetiroSCDTO GetByPeriodoVersionEmpresa(int iPericodi, int iVersion, int iEmprCodi);
    }
}

