using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla EVE_RSFHORA
    /// </summary>
    public interface IEveRsfhoraRepository
    {
        int Save(EveRsfhoraDTO entity);
        void Update(EveRsfhoraDTO entity);
        int Update2(EveRsfhoraDTO entity);
        void Delete(DateTime fecha);
        EveRsfhoraDTO GetById(int rsfhorcodi);
        List<EveRsfhoraDTO> List();
        List<EveRsfhoraDTO> GetByCriteria(DateTime fecha);
        int ValidarExistencia(DateTime fecha);
        List<EveRsfhoraDTO> ObtenerReporte(DateTime fechaInicio, DateTime fechaFin);
        void DeletePorId(int id);
        void ActualizarXML(EveRsfhoraDTO entity);

        #region Modificación_RSF_05012021
        List<EveRsfhoraDTO> ObtenerDatosXML(DateTime fecha);
        #endregion
    }
}
