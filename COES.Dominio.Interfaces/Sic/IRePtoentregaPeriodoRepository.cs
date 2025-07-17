using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla RE_PTOENTREGA_PERIODO
    /// </summary>
    public interface IRePtoentregaPeriodoRepository
    {
        int Save(RePtoentregaPeriodoDTO entity);
        void Update(RePtoentregaPeriodoDTO entity);
        void Delete(int reptopcodi, int periodo);
        RePtoentregaPeriodoDTO GetById(int reptopcodi);
        List<RePtoentregaPeriodoDTO> List();
        List<RePtoentregaPeriodoDTO> GetByCriteria(int periodo);
        int ObtenerPorPtoEntrega(int ptoEntregaCodi, int pericodi);
        List<RePtoentregaPeriodoDTO> ObtenerPtoEntregaUtilizadosPorPeriodo(int idPeriodo, int idSuministrador, string tipo);
    }
}
