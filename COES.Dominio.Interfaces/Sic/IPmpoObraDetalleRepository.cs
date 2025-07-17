using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla PMPO_OBRA_DETALLE
    /// </summary>
    public interface IPmpoObraDetalleRepository
    {
        void Save(PmpoObraDetalleDTO entity);
        void Update(PmpoObraDetalleDTO entity);
        void Delete(int obradetcodi);
        List<PmpoObraDetalleDTO> GetBarras(int codEmpresa);
        List<PmpoObraDetalleDTO> GetEquipos(int codEmpresa);
        List<PmpoObraDetalleDTO> GetGrupos(int codEmpresa);

        List<PmpoObraDetalleDTO> ListDetalleObras(int obracodi);
        List<PmpoObraDetalleDTO> GetByCriteria(int obracodi, int tipoObra,int emprcodi);



    }
}
