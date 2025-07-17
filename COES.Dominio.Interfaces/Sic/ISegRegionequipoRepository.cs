using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla SEG_REGIONEQUIPO
    /// </summary>
    public interface ISegRegionequipoRepository
    {
        int Save(SegRegionequipoDTO entity);
        void Update(SegRegionequipoDTO entity);
        void Delete(int regcodi, int equicodi, int segcotipo);
        SegRegionequipoDTO GetById();
        List<SegRegionequipoDTO> List();
        List<SegRegionequipoDTO> GetByCriteria(int regsegcodi, int segcotipo);
    }
}
