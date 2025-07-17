using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla SEG_COORDENADA
    /// </summary>
    public interface ISegCoordenadaRepository
    {
        int Save(SegCoordenadaDTO entity);
        void Update(SegCoordenadaDTO entity);
        void Delete(int segcocodi);
        SegCoordenadaDTO GetById(int segcocodi);
        List<SegCoordenadaDTO> List();
        List<SegCoordenadaDTO> GetByCriteria(int regcodi, int idtipo);
        List<SegCoordenadaDTO> Totalrestriccion();
    }
}
