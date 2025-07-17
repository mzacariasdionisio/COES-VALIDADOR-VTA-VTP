using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla WB_AYUDAAPP
    /// </summary>
    public interface IWbAyudaappRepository
    {
        int Save(WbAyudaappDTO entity);
        void Update(WbAyudaappDTO entity);
        void Delete(int ayuappcodi);
        WbAyudaappDTO GetById(int ayuappcodi);
        List<WbAyudaappDTO> List();
        List<WbAyudaappDTO> GetByCriteria();
    }
}
