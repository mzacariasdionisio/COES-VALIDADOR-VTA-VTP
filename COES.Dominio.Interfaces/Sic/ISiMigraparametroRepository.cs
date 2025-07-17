using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla SI_MIGRAPARAMETRO
    /// </summary>
    public interface ISiMigraparametroRepository
    {
        int Save(SiMigraParametroDTO entity);
        void Update(SiMigraParametroDTO entity);
        void Delete(int migparcodi);
        SiMigraParametroDTO GetById(int migparcodi);
        List<SiMigraParametroDTO> List();
        List<SiMigraParametroDTO> GetByCriteria();

        List<SiMigraParametroDTO> ObtenerByTipoOperacion(int idTipoOperacionMigracion);
    }
}
