using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla EN_ENSAYOFORMATO
    /// </summary>
    public interface IEnEnsayoformatoRepository
    {
        void UpdateEstado(int ensformtestado, int ensayocodi, int formatocodi);
        void Update(EnEnsayoformatoDTO entity);
        void Save(EnEnsayoformatoDTO entity);
        void Delete();
        void Delete(int formatocodi, int enunidadcodi);
        EnEnsayoformatoDTO GetById(int formatocodi, int enunidadcodi);
        List<EnEnsayoformatoDTO> List();
        List<EnEnsayoformatoDTO> GetByCriteria();

        List<EnEnsayoformatoDTO> ListaFormatoXEnsayo(int ensayocodi, int equicodi);
        List<EnEnsayoformatoDTO> ListaFormatoXEnsayo(int ensayocodi);
        List<EnEnsayoformatoDTO> ListarUnidadesConFormatos(int ensayocodi); 
        List<EnEnsayoformatoDTO> ListaFormatoXEnsayoEmpresaCtral(int emprcodi, int equicodi);

    }
}