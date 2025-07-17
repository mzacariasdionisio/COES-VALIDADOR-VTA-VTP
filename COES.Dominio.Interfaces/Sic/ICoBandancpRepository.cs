using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla CO_BANDANCP
    /// </summary>
    public interface ICoBandancpRepository
    {
        int Save(CoBandancpDTO entity);
        void Update(CoBandancpDTO entity);
        void Delete(int bandcodi);
        CoBandancpDTO GetById(int bandcodi);
        List<CoBandancpDTO> List();
        List<CoBandancpDTO> GetByCriteria(DateTime fecha, int grupocodi);
        List<CoBandancpDTO> ListBandaNCPxFecha(DateTime fecha);
    }
}
