using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla PFR_CUADRO
    /// </summary>
    public interface IPfrCuadroRepository
    {
        int Save(PfrCuadroDTO entity);
        void Update(PfrCuadroDTO entity);
        void Delete(int pfrcuacodi);
        PfrCuadroDTO GetById(int pfrcuacodi);
        List<PfrCuadroDTO> List();
        List<PfrCuadroDTO> GetByCriteria();
    }
}
