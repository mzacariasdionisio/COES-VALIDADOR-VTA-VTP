using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla EQ_AREAREL
    /// </summary>
    public interface IEqArearelRepository
    {
        int Save(EqAreaRelDTO entity);
        void Update(EqAreaRelDTO entity);
        void Delete(int arearlcodi);
        void Delete_UpdateAuditoria(int arearlcodi, string user);
        EqAreaRelDTO GetById(int arearlcodi);
        List<EqAreaRelDTO> List();
        List<EqAreaRelDTO> GetByCriteria();
        #region Zonas
        List<EqAreaRelDTO> ListarAreasxAreapadre(int areacodi);
        EqAreaRelDTO GetxAreapadrexAreacodi(int areapadre, int areacodi);
        #endregion
    }
}
