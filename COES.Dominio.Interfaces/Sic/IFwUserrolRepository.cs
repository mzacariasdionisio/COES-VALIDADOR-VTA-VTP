using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla FW_USERROL
    /// </summary>
    public interface IFwUserrolRepository
    {
        void Save(FwUserrolDTO entity);
        void Update(FwUserrolDTO entity);
        void Delete(int usercode, int rolcode);
        FwUserrolDTO GetById(int usercode, int rolcode);
        List<FwUserrolDTO> List();
        List<FwUserrolDTO> GetByCriteria();
        List<FwUserrolDTO> GetByRol(string rolcodes);
        
    }
}
