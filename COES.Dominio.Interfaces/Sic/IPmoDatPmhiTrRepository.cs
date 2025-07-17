using COES.Dominio.DTO.Sic;
using System.Collections.Generic;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla PMO_DAT_PMHI_TR
    /// </summary>
    public interface IPmoDatPmhiTrRepository
    {
        int Save(PmoDatPmhiTrDTO entity);
        int Update(PmoDatPmhiTrDTO entity);
        List<PmoDatPmhiTrDTO> ListDatPmhiTr(int codigoPeriodo, string tipo);
        int CountDatPmhiTr(int periCodi, string tipo);
    }
}
