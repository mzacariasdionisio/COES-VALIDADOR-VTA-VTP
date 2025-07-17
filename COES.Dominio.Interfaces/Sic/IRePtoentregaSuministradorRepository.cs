using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla RE_PTOENTREGA_SUMINISTRADOR
    /// </summary>
    public interface IRePtoentregaSuministradorRepository
    {
        int Save(RePtoentregaSuministradorDTO entity);
        void Update(RePtoentregaSuministradorDTO entity);
        void Delete(int idPtoEntrega, int periodo);
        void EliminarPorPeriodo(int periodo);
        RePtoentregaSuministradorDTO GetById(int reptsmcodi);
        List<RePtoentregaSuministradorDTO> List();
        List<RePtoentregaSuministradorDTO> GetByCriteria(int idPeriodo);
        List<RePtoentregaSuministradorDTO> ObtenerPorPuntoEntregaPeriodo(int idPtoEntrega, int idPeriodo);        
    }
}
