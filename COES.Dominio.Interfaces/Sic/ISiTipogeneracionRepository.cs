using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla SI_TIPOGENERACION
    /// </summary>
    public interface ISiTipogeneracionRepository
    {
        int Save(SiTipogeneracionDTO entity);
        void Update(SiTipogeneracionDTO entity);
        void Delete(int tgenercodi);
        SiTipogeneracionDTO GetById(int tgenercodi);
        List<SiTipogeneracionDTO> List();
        List<SiTipogeneracionDTO> GetByCriteria();

        #region PR5
        List<SiTipogeneracionDTO> TipoGeneracionxCentral(string equicodi);
        #endregion
    }
}

