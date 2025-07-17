using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    public interface IPrnConfiguracionFormulaRepository
    {
        List<PrnConfiguracionFormulaDTO> List();
        PrnConfiguracionFormulaDTO GetById(int codigo);
        void Delete(int codigo);
        void Save(PrnConfiguracionFormulaDTO entity);
        void Update(PrnConfiguracionFormulaDTO entity);
        List<PrnConfiguracionFormulaDTO> ListConfiguracionFormulaByFiltros(DateTime fechaIni, DateTime fechaFin, string idFormula);
        PrnConfiguracionFormulaDTO GetIdByCodigoFecha(int ptomedicodi, DateTime cnffrmfecha);
        PrnConfiguracionFormulaDTO GetParametrosByIdFecha(int ptomedicodi, DateTime prncfgfecha);
        List<PrnConfiguracionFormulaDTO> ParametrosFormulasList(string fecdesde, string fechasta, string lstpuntos);
    }
}
