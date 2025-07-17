using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla SI_PLANTILLACORREO
    /// </summary>
    public interface ISiPlantillacorreoRepository
    {
        int Save(SiPlantillacorreoDTO entity);
        void Update(SiPlantillacorreoDTO entity);
        void Delete(int plantcodi);
        SiPlantillacorreoDTO GetById(int plantcodi);
        List<SiPlantillacorreoDTO> List();
        List<SiPlantillacorreoDTO> GetByCriteria();
        SiPlantillacorreoDTO ObtenerPlantillaPorModulo(int idTipoPlantilla, int idModulo);
        void ActualizarPlantilla(SiPlantillacorreoDTO entity);
        List<SiPlantillacorreoDTO> ListarPlantillas(string plantillaCodis);
    }
}
