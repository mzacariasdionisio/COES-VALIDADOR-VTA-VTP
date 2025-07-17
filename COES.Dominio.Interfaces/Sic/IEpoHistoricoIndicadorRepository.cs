using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla EPO_HISTORICO_INDICADOR
    /// </summary>
    public interface IEpoHistoricoIndicadorRepository
    {
        int Save(EpoHistoricoIndicadorDTO entity);
        void Update(EpoHistoricoIndicadorDTO entity);
        void Delete(int hincodi);
        EpoHistoricoIndicadorDTO GetById(int hincodi);
        List<EpoHistoricoIndicadorDTO> List();
        List<EpoHistoricoIndicadorDTO> GetByCriteria();
    }
}
