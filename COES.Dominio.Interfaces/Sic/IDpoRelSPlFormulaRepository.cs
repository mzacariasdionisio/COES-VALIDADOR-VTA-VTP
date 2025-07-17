using COES.Dominio.DTO.Sic;
using System;
using System.Collections.Generic;

namespace COES.Dominio.Interfaces.Sic
{
    public interface IDpoRelSPlFormulaRepository
    {
        void Save(DpoRelSplFormulaDTO entity);
        void Update(DpoRelSplFormulaDTO entity);
        void Delete(int id);
        List<DpoRelSplFormulaDTO> List();
        List<DpoRelSplFormulaDTO> ListBarrasxVersion(int version);
        DpoRelSplFormulaDTO GetById(int id);

        //Adicionales
        List<DpoRelSplFormulaDTO> ListFormulasIndustrial();
        List<DpoRelSplFormulaDTO> ListFormulasVegetativa();
        void DeleteByVersion(int id);
        void DeleteByVersionxBarra(int version, int barra);
        void UpdateFormulas(DpoRelSplFormulaDTO entity);
    }
}
