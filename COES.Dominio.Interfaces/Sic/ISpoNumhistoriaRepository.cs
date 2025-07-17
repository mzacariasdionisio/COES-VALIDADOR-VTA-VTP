using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla SPO_NUMHISTORIA
    /// </summary>
    public interface ISpoNumhistoriaRepository
    {
        int Save(SpoNumhistoriaDTO entity);
        void Update(SpoNumhistoriaDTO entity);
        void Delete(int numhiscodi);
        SpoNumhistoriaDTO GetById(int numhiscodi);
        List<SpoNumhistoriaDTO> List();
        List<SpoNumhistoriaDTO> GetByCriteria();
    }
}
