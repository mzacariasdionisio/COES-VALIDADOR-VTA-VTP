using System.Collections.Generic;
using COES.Dominio.DTO.Sic;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla IIO_PERIODO_SEIN
    /// </summary>
    public interface IIioPeriodoSeinRepository
    {
        List<IioPeriodoSeinDTO> GetByCriteria(string anio);
        IioPeriodoSeinDTO GetById(string periodo);
        void Save(IioPeriodoSeinDTO iioPeriodoSeinDto);
        List<string> ListAnios();
        void Update(IioPeriodoSeinDTO iioPeriodoSeinDto);
    }
}