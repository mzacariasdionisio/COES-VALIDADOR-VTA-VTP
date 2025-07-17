using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla RE_INTERRUPCION_ACCESO
    /// </summary>
    public interface IReInterrupcionAccesoRepository
    {
        int Save(ReInterrupcionAccesoDTO entity);
        void Update(ReInterrupcionAccesoDTO entity);
        void Delete(int reinaccodi);
        ReInterrupcionAccesoDTO GetById(int reinaccodi);
        List<ReInterrupcionAccesoDTO> List();
        List<ReInterrupcionAccesoDTO> GetByCriteria();
        void DeletePorPeriodo(int repercodi);
        List<ReInterrupcionAccesoDTO> ListByPeriodo(int repercodi);
        void GrabarEnGrupo(List<ReInterrupcionAccesoDTO> entitys);
        int GetIdMax();
    }
}
