using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla EVE_RSFDETALLE
    /// </summary>
    public interface IEveRsfdetalleRepository
    {
        int Save(EveRsfdetalleDTO entity);
        void Update(EveRsfdetalleDTO entity);
        void Update2(EveRsfdetalleDTO entity);
        void Delete(DateTime fecha);
        EveRsfdetalleDTO GetById(int rsfdetcodi);
        List<EveRsfdetalleDTO> List();
        List<EveRsfdetalleDTO> GetByCriteria();
        List<EveRsfdetalleDTO> ObtenerConfiguracion(DateTime fechaPeriodo);
        List<EveRsfdetalleDTO> ObtenerDetalleReserva(DateTime fecha);
        List<EveRsfdetalleDTO> ObtenerUnidadesRSF(DateTime fecha);
        List<EveRsfdetalleDTO> ObtenerConfiguracionCO();
        void DeletePorId(int id);

        #region Modificación_RSF_05012021
        List<EveRsfdetalleDTO> ObtenerDetalleXML(DateTime fecha);
        #endregion
    }
}
