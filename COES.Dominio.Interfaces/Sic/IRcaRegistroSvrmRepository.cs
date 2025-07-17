using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla RCA_REGISTRO_SVRM
    /// </summary>
    public interface IRcaRegistroSvrmRepository
    {
        int Save(RcaRegistroSvrmDTO entity);
        void Update(RcaRegistroSvrmDTO entity);
        void Delete(int rcsvrmcodi);
        RcaRegistroSvrmDTO GetById(int rcsvrmcodi);
        List<RcaRegistroSvrmDTO> List();
        List<RcaRegistroSvrmDTO> GetByCriteria();

        List<RcaRegistroSvrmDTO> ListRcaRegistroSvrmsExcel(string empresa, string codigoSuministro, string fecIni, string fecFin, string maxDemComprometidaIni, string maxDemComprometidaFin, string estadoRegistro);

        RcaRegistroSvrmDTO ObtenerPorCodigo(int rcsvrmcodi);

        List<RcaRegistroSvrmDTO> ListRcaRegistroSvrmsFiltro(string empresa, string codigoSuministro, string fecIni, string fecFin, string maxDemComprometidaIni,
            string maxDemComprometidaFin, string estadoRegistro, int regIni, int regFin);

        int ListRcaRegistroSvrmsCount(string empresa, string codigoSuministro, string fecIni, string fecFin, string maxDemComprometidaIni, string maxDemComprometidaFin, string estadoRegistro);
    }
}
