using COES.Dominio.DTO.Sic;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla IN_SECCION
    public interface IInSeccionRepository
    {
        void Save(InSeccionDTO entity);
        void Update(InSeccionDTO entity);
        void Delete(int inseccodi);
        InSeccionDTO GetById(int inseccodi);
        List<InSeccionDTO> GetByCriteria(int repcodi);
        List<InSeccionDTO> List();
        void UpdateSeccion(InSeccionDTO entity);
    }
}
