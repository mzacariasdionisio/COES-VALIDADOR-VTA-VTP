using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla INT_DIRECTORIO
    /// </summary>
    public interface IIntDirectorioRepository
    {
        int Save(IntDirectorioDTO entity);
        void Update(IntDirectorioDTO entity);
        void Delete(int dircodi);
        IntDirectorioDTO GetById(int dircodi);
        List<IntDirectorioDTO> List();
        List<IntDirectorioDTO> GetByCriteria();
    }
}
