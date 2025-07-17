using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Transferencias;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Transferencias
{
    /// <summary>
    /// Interface de acceso a datos de la tabla VCR_VERSIONINCPL
    /// </summary>
    public interface IVcrVersionincplRepository
    {
        int Save(VcrVersionincplDTO entity);
        void Update(VcrVersionincplDTO entity);
        void Delete(int vcrinccodi);
        VcrVersionincplDTO GetById(int vcrinccodi);
        List<VcrVersionincplDTO> List();
        List<VcrVersionincplDTO> GetByCriteria();
        List<VcrVersionincplDTO> ListIncpl(int id);
        List<VcrVersionincplDTO> ListIncplIndex();
        VcrVersionincplDTO GetByIdEdit(int vcrinccodi, int pericodi);
        VcrVersionincplDTO GetByIdView(int vcrinccodi, int pericodi);
    }
}
