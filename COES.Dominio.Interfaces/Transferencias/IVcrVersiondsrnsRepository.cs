using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Transferencias;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Transferencias
{
    /// <summary>
    /// Interface de acceso a datos de la tabla VCR_VERSIONDSRNS
    /// </summary>
    public interface IVcrVersiondsrnsRepository
    {
        int Save(VcrVersiondsrnsDTO entity);
        void Update(VcrVersiondsrnsDTO entity);
        void Delete(int vcrdsrcodi);
        VcrVersiondsrnsDTO GetById(int vcrdsrcodi);
        List<VcrVersiondsrnsDTO> List();
        List<VcrVersiondsrnsDTO> GetByCriteria();
        List<VcrVersiondsrnsDTO> ListVersion(int id);
        List<VcrVersiondsrnsDTO> ListIndex();
        VcrVersiondsrnsDTO GetByIdEdit(int vcrdsrcodi, int pericodi);
        VcrVersiondsrnsDTO GetByIdView(int vcrdsrcodi, int pericodi);
        VcrVersiondsrnsDTO GetByIdPeriodo(int pericodi);
    }
}
