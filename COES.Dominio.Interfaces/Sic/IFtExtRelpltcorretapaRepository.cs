using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla FT_EXT_RELPLTCORRETAPA
    /// </summary>
    public interface IFtExtRelpltcorretapaRepository
    {
        int Save(FtExtRelpltcorretapaDTO entity);
        void Update(FtExtRelpltcorretapaDTO entity);
        void Delete(int fcoretcodi);
        FtExtRelpltcorretapaDTO GetById(int fcoretcodi);
        List<FtExtRelpltcorretapaDTO> List();
        List<FtExtRelpltcorretapaDTO> GetByCriteria(int tpcorrcodi, int ftetcodi);
        //FtExtRelpltcorretapaDTO ObtenerPorEtapaYCarpeta(int ftetcodi, int estenvcodi, int tpcorrcodi);
        FtExtRelpltcorretapaDTO ObtenerPorEtapaYCarpeta(int ftetcodi, int estenvcodi, int tpcorrcodi, int? esEspecial, int? esAmpliacion);
    }
}
