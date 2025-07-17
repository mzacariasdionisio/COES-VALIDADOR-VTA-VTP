using COES.Dominio.DTO.Sic;
using System;
using System.Collections.Generic;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla IND_MANTTO
    /// </summary>
    public interface IIndManttoRepository
    {
        int Save(IndManttoDTO entity);
        void Update(IndManttoDTO entity);
        void Delete(int indmancodi);
        IndManttoDTO GetById(int indmancodi);
        List<IndManttoDTO> List();
        List<IndManttoDTO> GetByCriteria();

        IndManttoDTO GetById2(int indmancodi);

        List<IndManttoDTO> GetIndisponibilidadesIndmanto(DateTime fechaInicio, DateTime fechaFin, string famcodi, int famcodiCentral, int famcodiUnidad);

        List<IndManttoDTO> BuscarMantenimientos(string idsTipoMantenimiento, DateTime fechaInicio, DateTime fechaFin, string indispo,
            string idsTipoEmpresa, string idsEmpresa, string idsTipoEquipo, string indInterrupcion, string idstipoMantto, int nroPagina, int nroFilas);

        int ObtenerNroRegistros(string idsTipoMantenimiento, DateTime fechaInicio, DateTime fechaFin, string indispo,
           string idsTipoEmpresa, string idsEmpresa, string idsTipoEquipo, string indInterrupcion, string idstipoMantto);

        List<IndManttoDTO> ObtenerReporteMantenimientos(string idsTipoMantenimiento, DateTime fechaInicio, DateTime fechaFin, string indispo,
            string idsTipoEmpresa, string idsEmpresa, string idsTipoEquipo, string indInterrupcion, string idstipoMantto);

        List<IndManttoDTO> ListarIndManttoByEveMantto(string manttocodi);

        List<IndManttoDTO> ListHistoricoByIndmacodi(string indmacodi);

        List<IndManttoDTO> ListarIndManttoAppPR25(DateTime fechaInicio, DateTime fechaFin, string famcodi);

    }
}
