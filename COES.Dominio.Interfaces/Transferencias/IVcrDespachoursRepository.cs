using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Transferencias;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Transferencias
{
    /// <summary>
    /// Interface de acceso a datos de la tabla VCR_DESPACHOURS
    /// </summary>
    public interface IVcrDespachoursRepository
    {      
        int Save(VcrDespachoursDTO entity);
        void Update(VcrDespachoursDTO entity);
        void Delete(int vcrecacodi);
        VcrDespachoursDTO GetById(int vcdurscodi);
        List<VcrDespachoursDTO> List(int vcrecacodi);
        List<VcrDespachoursDTO> GetByCriteria();
        List<VcrDespachoursDTO> ListUnidadByUrsTipo(int vcrecacodi, int GrupoCodi, string Vcdurstipo);
        List<VcrDespachoursDTO> ListByUrsUnidadTipoDia(int vcrecacodi, int grupocodi, int equicodi, string vcdurstipo, DateTime vcdursfecha);
        List<VcrDespachoursDTO> ListByRangeDatetime(DateTime fechaInicio, DateTime fechaFin);

        #region Modificación Costos de Oportunidad
        int GetMaxId();
        void GrabarBulk(List<VcrDespachoursDTO> ListaDespacho);
        #endregion
    }
}
