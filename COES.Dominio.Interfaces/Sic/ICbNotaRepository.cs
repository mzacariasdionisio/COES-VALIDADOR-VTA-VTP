using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;
using System.Data;
using System.Data.Common;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla CB_NOTA
    /// </summary>
    public interface ICbNotaRepository
    {
        int getIdDisponible();
        void Save(CbNotaDTO entity, IDbConnection connection, IDbTransaction transaction);        
        int Save(CbNotaDTO entity);
        void Update(CbNotaDTO entity);
        void Delete(int cbnotacodi);
        CbNotaDTO GetById(int cbnotacodi);
        List<CbNotaDTO> List();
        List<CbNotaDTO> GetByCriteria();
        List<CbNotaDTO> GetByReporte(int cbrepcodi);
        List<CbNotaDTO> GetByTipoReporte(int cbreptipo);
    }
}
