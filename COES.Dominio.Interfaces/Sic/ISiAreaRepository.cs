using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla SI_AREA
    /// </summary>
    public interface ISiAreaRepository
    {
        int Save(SiAreaDTO entity);
        void Update(SiAreaDTO entity);
        void Delete(int areacodi);
        SiAreaDTO GetById(int areacodi);
        List<SiAreaDTO> List();

        #region GESTPROTECT
        List<SiAreaDTO> ListSGOCOES();
        #endregion

        List<SiAreaDTO> GetByCriteria();
    
    }
}
