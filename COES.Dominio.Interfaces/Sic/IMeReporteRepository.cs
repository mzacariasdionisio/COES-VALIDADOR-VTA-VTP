using COES.Dominio.DTO.Sic;
using System.Collections.Generic;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla ME_REPORTE
    /// </summary>
    public interface IMeReporteRepository
    {
        void Update(MeReporteDTO entity);
        void Delete();
        MeReporteDTO GetById(int id);
        List<MeReporteDTO> List();
        List<MeReporteDTO> ListarXModulo(int idmodulo);
        List<MeReporteDTO> ListarXArea(int idarea);
        int Save(MeReporteDTO entity);
        List<MeReporteDTO> ListarXAreaYModulo(int idarea, int idmodulo);
    }
}
