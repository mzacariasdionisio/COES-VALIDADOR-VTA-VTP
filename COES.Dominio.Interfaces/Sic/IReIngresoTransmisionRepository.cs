using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla RE_INGRESO_TRANSMISION
    /// </summary>
    public interface IReIngresoTransmisionRepository
    {
        int Save(ReIngresoTransmisionDTO entity);
        void Update(ReIngresoTransmisionDTO entity);
        void Delete(int reingcodi);
        ReIngresoTransmisionDTO GetById(int reingcodi);
        List<ReIngresoTransmisionDTO> List();
        List<ReIngresoTransmisionDTO> GetByCriteria(int periodo);
        List<ReEmpresaDTO> ObtenerEmpresas();
        List<ReEmpresaDTO> ObtenerEmpresasSuministradoras();

        List<ReEmpresaDTO> ObtenerEmpresasSuministradorasTotal();
        ReIngresoTransmisionDTO ObtenerPorEmpresaPeriodo(int idEmpresa, int idPeriodo);
        void ActualizarArchivo(int id, string extension);
        List<ReEmpresaDTO> ObtenerSuministradoresPorPeriodo(int idPeriodo, string tipo);
        List<ReEmpresaDTO> ObtenerResponsablesPorPeriodo(int idPeriodo, string tipo);

    }
}
