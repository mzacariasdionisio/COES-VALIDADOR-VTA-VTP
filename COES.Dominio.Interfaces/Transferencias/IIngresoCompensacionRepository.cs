using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COES.Dominio.DTO.Transferencias;

namespace COES.Dominio.Interfaces.Transferencias
{
    /// <summary>
    /// Interface de acceso a datos de la tabla TRN_ING_COMPENSACION
    /// </summary>
    public interface IIngresoCompensacionRepository
    {
        int Save(IngresoCompensacionDTO entity);
        void Delete(System.Int32 PeriCodi, System.Int32 IngrCompVersion);
        List<IngresoCompensacionDTO> GetByCodigo(int? pericodi, int? version);
        List<IngresoCompensacionDTO> ListByPeriodoVersion(int iPericodi, int iVersion);
        IngresoCompensacionDTO GetByPeriVersCabCompEmpr(int iPeriCodi, int iCabComCodi, int iVersion, int iEmprCodi);
        List<IngresoCompensacionDTO> BuscarListaEmpresas(int iPeriCodi, int iVersion);
        IngresoCompensacionDTO GetByPeriVersRentasCongestion(int iPeriCodi, int iVersion);
    }
}
