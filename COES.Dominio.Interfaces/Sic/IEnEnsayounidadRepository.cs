using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla EN_ENSAYOUNIDAD
    /// </summary>
    public interface IEnEnsayounidadRepository
    {
        int Save(EnEnsayounidadDTO entity);
        void Update(EnEnsayounidadDTO entity);
        void Delete(int enunidadcodi);
        EnEnsayounidadDTO GetById(int enunidadcodi);
        List<EnEnsayounidadDTO> List();
        List<EnEnsayounidadDTO> GetByCriteria();
        EnEnsayounidadDTO GetEnsayoUnidad(int idensayo, int equicodi);
        List<EnEnsayounidadDTO> GetUnidadesXEnsayo(int idensayo);
    }
}
