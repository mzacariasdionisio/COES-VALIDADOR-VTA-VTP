using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using System.Data;
using System.Data.Common;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla CB_REP_CABECERA
    /// </summary>
    public interface ICbRepCabeceraRepository
    {
        int getIdDisponible();
        void Save(CbRepCabeceraDTO entity, IDbConnection conn, IDbTransaction transaction);
        int Save(CbRepCabeceraDTO entity);
        void Update(CbRepCabeceraDTO entity);
        void Delete(int cbrcabcodi);
        CbRepCabeceraDTO GetById(int cbrcabcodi);
        List<CbRepCabeceraDTO> List();
        List<CbRepCabeceraDTO> GetByCriteria();
        List<CbRepCabeceraDTO> GetByTipoReporte(int tipoReporte);
        List<CbRepCabeceraDTO> GetByIdReporte(int cbrepcodi);
        List<CbRepCabeceraDTO> GetByTipoReporteYMesVigencia(int cbreptipo, string mesVigencia);
    }
}
