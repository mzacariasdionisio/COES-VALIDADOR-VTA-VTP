using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla ME_REPORPTOMED
    /// </summary>
    public interface IMeReporptomedRepository
    {
        void Update(MeReporptomedDTO entity);
        void Delete(int repptocodi);
        MeReporptomedDTO GetById(int repptocodi);
        MeReporptomedDTO GetById2(int reporcodi);
        MeReporptomedDTO GetById3(int reporcodi, int ptomedicodi, int lectcodi, int tipoinfocodi, int tptomedicodi);
        List<MeReporptomedDTO> List();
        List<MeReporptomedDTO> GetByCriteria(int reporcodi, int ptomedicodi);
        int Save(MeReporptomedDTO entity);
        List<MeReporptomedDTO> ListarEncabezado(int reporcodi, string idsEmpresa, string idsTipoPtoMed);
        List<MeReporptomedDTO> GetByCriteria2(int reporcodi, string query);
        List<MeReporptomedDTO> ListarPuntoReporte(int reporcodi, DateTime fechaPeriodo);
        int GetOrdenPto(int reporcodi, int empresa);
        List<DateTime> PaginacionReporte(int idReporte, int lectnro, DateTime fechaInicio, DateTime fechaFin);
        List<MeReporptomedDTO> ListarEncabezadoPowel(int reporcodi);
    }
}
