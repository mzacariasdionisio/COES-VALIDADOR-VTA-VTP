using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Scada;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Scada
{
    /// <summary>
    /// Interface de acceso a datos de la tabla TR_LOGDMP_SP7
    /// </summary>
    public interface ITrLogdmpSp7Repository
    {
        int Save(TrLogdmpSp7DTO entity);
        void Update(TrLogdmpSp7DTO entity);
        void Delete(int ldmcodi);
        TrLogdmpSp7DTO GetById(int ldmcodi);
        List<TrLogdmpSp7DTO> List();
        List<TrLogdmpSp7DTO> ListExportacion(string estado);
        List<TrLogdmpSp7DTO> ListImportacion(string estado);
        List<TrLogdmpSp7DTO> GetByCriteria();
        int SaveTrLogdmpSp7Id(TrLogdmpSp7DTO entity);
        List<TrLogdmpSp7DTO> BuscarOperaciones(DateTime fechaIni, DateTime fechaFin, string tipo, int nroPage, int pageSize);
        int ObtenerNroFilas(DateTime fechaIni, DateTime fechaFin);
    }
}
