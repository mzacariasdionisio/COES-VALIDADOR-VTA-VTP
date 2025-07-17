using System.Collections.Generic;
using COES.Dominio.DTO.Sic;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla IIO_LOG_REMISION
    /// </summary>
    public interface IIioLogRemisionRepository
    {
        List<IioLogRemisionDTO> List(IioControlCargaDTO scoControlCargaDto, int minRowToFetch, int maxRowToFetch);
        int Save(IioLogRemisionDTO scoLogRemisionDto);
        IioLogRemisionDTO GetById(IioLogRemisionDTO scoLogRemisionDto);
        void Delete(int rccacodi);
    }
}