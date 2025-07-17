using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    public interface IPrnRelacionTnaRepository
    {
        List<PrnRelacionTnaDTO> List();
        PrnRelacionTnaDTO GetById(int codigo);
        void Delete(int codigo);
        void Save(PrnRelacionTnaDTO entity);
        void Update(PrnRelacionTnaDTO entity);
        List<PrnRelacionTnaDTO> ListRelacionTnaDetalleById(int codigo);
        List<PrnRelacionTnaDTO> ListRelacionTnaDetalle();
        int SaveGetId(PrnRelacionTnaDTO entity);

        #region Métodos del detalle PRN_RELACIONTNADETALLE
        void SavePrnRelacionTnaDetalle(PrnRelacionTnaDTO entity);
        void DeletePrnRelacionTnaDetalle(int reltnacodi);        
        #endregion
    }
}
