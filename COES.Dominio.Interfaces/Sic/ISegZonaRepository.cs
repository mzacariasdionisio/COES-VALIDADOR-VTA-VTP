using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla SEG_ZONA
    /// </summary>
    public interface ISegZonaRepository
    {
        int Save(SegZonaDTO entity);
        void Update(SegZonaDTO entity);
        void Delete();
        SegZonaDTO GetById();
        List<SegZonaDTO> List();
        List<SegZonaDTO> GetByCriteria();
    }
}
