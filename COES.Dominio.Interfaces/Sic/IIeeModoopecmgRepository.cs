using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla IEE_MODOOPECMG
    /// </summary>
    public interface IIeeModoopecmgRepository
    {
        int Save(IeeModoopecmgDTO entity);
        void Update(IeeModoopecmgDTO entity);
        void Delete(int mocmcodigo);
        IeeModoopecmgDTO GetById(int mocmcodigo);
        List<IeeModoopecmgDTO> List();
        List<IeeModoopecmgDTO> GetByCriteria();
    }
}
