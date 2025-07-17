using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla EPO_HISTORICO_INDICADOR_DET
    /// </summary>
    public interface IEpoHistoricoIndicadorDetRepository
    {
        void Save(EpoHistoricoIndicadorDetDTO entity);
        void Update(EpoHistoricoIndicadorDetDTO entity);
        void Delete(int hincodi, int percodi);
        EpoHistoricoIndicadorDetDTO GetById(int hincodi, int percodi);
        List<EpoHistoricoIndicadorDetDTO> List();
        List<EpoHistoricoIndicadorDetDTO> GetByCriteria();
    }
}
