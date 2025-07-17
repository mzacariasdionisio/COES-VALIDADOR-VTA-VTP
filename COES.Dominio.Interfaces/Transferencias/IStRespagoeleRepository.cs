using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Transferencias;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Transferencias
{
    /// <summary>
    /// Interface de acceso a datos de la tabla ST_RESPAGOELE
    /// </summary>
    public interface IStRespagoeleRepository
    {
        int Save(StRespagoeleDTO entity);
        void Update(StRespagoeleDTO entity);
        void Delete(int strecacodi);
        void DeleteStRespagoEleVersion(int strecacodi);
        StRespagoeleDTO GetById(int respagcodi, int stcompcodi);
        List<StRespagoeleDTO> List();
        List<StRespagoeleDTO> GetByCriteria();
        List<StRespagoeleDTO> ListStRespagElePorID(int id);
    }
}
