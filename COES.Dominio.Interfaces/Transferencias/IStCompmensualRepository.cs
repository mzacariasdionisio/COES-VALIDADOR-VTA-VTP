using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Transferencias;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Transferencias
{
    /// <summary>
    /// Interface de acceso a datos de la tabla ST_COMPMENSUAL
    /// </summary>
    public interface IStCompmensualRepository
    {
        int Save(StCompmensualDTO entity);
        void Update(StCompmensualDTO entity);
        void Delete(int recacodi);
        StCompmensualDTO GetById(int cmpmencodi);
        List<StCompmensualDTO> List();
        List<StCompmensualDTO> GetByCriteria(int recacodi);
        List<StCompmensualDTO> ListByStCompMensualVersion(int strecacodi);
    }
}
