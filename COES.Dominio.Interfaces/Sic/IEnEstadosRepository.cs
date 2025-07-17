using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla EN_ESTADOS
    /// </summary>
    public interface IEnEstadosRepository
    {
        void Update(EnEstadoDTO entity);
        void Delete();
        EnEstadoDTO GetById();
        List<EnEstadoDTO> List();
        List<EnEstadoDTO> GetByCriteria();
        void Save(EnEstadoDTO entity);
    }
}
