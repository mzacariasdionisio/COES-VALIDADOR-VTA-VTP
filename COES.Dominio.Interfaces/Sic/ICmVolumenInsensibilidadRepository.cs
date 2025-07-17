using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla CM_VOLUMEN_INSENSIBILIDAD
    /// </summary>
    public interface ICmVolumenInsensibilidadRepository
    {
        int Save(CmVolumenInsensibilidadDTO entity);
        void Update(CmVolumenInsensibilidadDTO entity);
        void Delete(int volinscodi);
        CmVolumenInsensibilidadDTO GetById(int volinscodi);
        List<CmVolumenInsensibilidadDTO> List();
        List<CmVolumenInsensibilidadDTO> GetByCriteria(DateTime fecha);
        List<CmVolumenInsensibilidadDTO> ObtenerRegistros(DateTime fecha);
    }
}
