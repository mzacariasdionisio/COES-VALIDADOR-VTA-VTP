using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Transferencias;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Transferencias
{
    /// <summary>
    /// Interface de acceso a datos de la tabla ST_COMPENSACION
    /// </summary>
    public interface IStCompensacionRepository
    {
        int Save(StCompensacionDTO entity);
        void Update(StCompensacionDTO entity);
        void Delete(int stcompcodi);
        void DeleteVersion(int strecacodi);
        StCompensacionDTO GetById(int stcompcodi);
        List<StCompensacionDTO> List();
        List<StCompensacionDTO> GetByCriteria(int strecacodi);
        List<StCompensacionDTO> GetBySisTrans(int sistrncodi);
        List<StCompensacionDTO> ListStCompensacionsPorID(int sistrncodi);

        
        
    }
}
