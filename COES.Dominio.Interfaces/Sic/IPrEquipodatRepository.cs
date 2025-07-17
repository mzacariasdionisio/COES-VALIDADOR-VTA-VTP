using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla PR_EQUIPODAT
    /// </summary>
    public interface IPrEquipodatRepository
    {
        void Save(PrEquipodatDTO entity);
        void Update(PrEquipodatDTO entity);
        void Delete(int equicodi, int grupocodi, int concepcodi, DateTime fechadat);
        PrEquipodatDTO GetById(int equicodi, int grupocodi, int concepcodi, DateTime fechadat);
        List<PrEquipodatDTO> List();
        List<PrEquipodatDTO> GetByCriteria();
        List<ConceptoDatoDTO> ListarDatosConceptoEquipoDat(int iEquiCodi, int iGrupoCodi);
    }
}

