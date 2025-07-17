using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla EN_ESTENSAYO
    /// </summary>
    public interface IEnEstensayoRepository
    {
        void Update(EnEstensayoDTO entity);
        void Delete();
        EnEstensayoDTO GetById();
        List<EnEstensayoDTO> List();
        List<EnEstensayoDTO> GetByCriteria();
        void Save(EnEstensayoDTO entity);
    }
}
