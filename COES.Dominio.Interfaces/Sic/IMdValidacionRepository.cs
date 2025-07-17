using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla MD_VALIDACION
    /// </summary>
    public interface IMdValidacionRepository
    {
        void Save(MdValidacionDTO entity);
        void Update(MdValidacionDTO entity);
        void Delete(int emprcodi, DateTime validames);
        MdValidacionDTO GetById(int emprcodi, DateTime validames);
        List<MdValidacionDTO> List();
        List<MdValidacionDTO> GetByCriteria(DateTime fecha);
    }
}
