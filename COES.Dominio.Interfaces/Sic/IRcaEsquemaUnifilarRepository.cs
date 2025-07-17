using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla RCA_ESQUEMA_UNIFILAR
    /// </summary>
    public interface IRcaEsquemaUnifilarRepository
    {
        int Save(RcaEsquemaUnifilarDTO entity);
        void Update(RcaEsquemaUnifilarDTO entity);
        void Delete(int rcesqucodi);
        RcaEsquemaUnifilarDTO GetById(int rcesqucodi);
        List<RcaEsquemaUnifilarDTO> List();
        List<RcaEsquemaUnifilarDTO> GetByCriteria();

        List<RcaEsquemaUnifilarDTO> ListarEsquemaUnifilarFiltro(string empresa, string codigoSuministro, string fecIni, string fecFin, int origen, int regIni, int regFin);
        List<RcaEsquemaUnifilarDTO> ListarEsquemaUnifilarHistorial(int emprcodi, int equicodi);

        RcaEsquemaUnifilarDTO ObtenerPorCodigo(int rccarecodi);

        List<RcaEsquemaUnifilarDTO> ListarEsquemaUnifilarExcel(string empresa, string codigoSuministro, string fecIni, string fecFin, int origen);

        int ListarEsquemaUnifilarCount(string empresa, string codigoSuministro, string fecIni, string fecFin, int origen);
    }
}
