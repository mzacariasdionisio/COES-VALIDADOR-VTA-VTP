using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla EVE_IEODCUADRO_DET
    /// </summary>
    public interface IEveIeodcuadroDetRepository
    {
        void Update(EveIeodcuadroDetDTO entity);
        void Delete(int iccodi);
        //EveIeodcuadroDetDTO GetById();
        List<EveIeodcuadroDetDTO> List();
        List<EveIeodcuadroDetDTO> GetByCriteria(int iccodi);
        void Save(EveIeodcuadroDetDTO entity);
    }
}
