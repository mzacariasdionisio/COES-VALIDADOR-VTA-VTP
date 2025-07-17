using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla NR_CONCEPTO
    /// </summary>
    public interface INrConceptoRepository
    {
        int Save(NrConceptoDTO entity);
        void Update(NrConceptoDTO entity);
        void Delete(int nrcptcodi);
        NrConceptoDTO GetById(int nrcptcodi);
        List<NrConceptoDTO> List();
        List<NrConceptoDTO> ListSubModuloConcepto();
        List<NrConceptoDTO> GetByCriteria();
        int SaveNrConceptoId(NrConceptoDTO entity);
        List<NrConceptoDTO> BuscarOperaciones(int nrsmodCodi,int nroPage, int PageSize);
        int ObtenerNroFilas(int nrsmodCodi);
    }
}
