using COES.Dominio.DTO.Sic;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla PFR_RELACION_POTENCIA_FIRME
    /// </summary>
    public interface IPfrRelacionPotenciaFirmeRepository
    {
        //int Save(PfrRelacionPotenciaFirmeDTO entity);
        int Save(PfrRelacionPotenciaFirmeDTO entity, IDbConnection conn, DbTransaction tran);
        void Update(PfrRelacionPotenciaFirmeDTO entity);
        void Delete(int pfrrpfcodi);
        PfrRelacionPotenciaFirmeDTO GetById(int pfrrpfcodi);
        List<PfrRelacionPotenciaFirmeDTO> List();
        List<PfrRelacionPotenciaFirmeDTO> GetByCriteria(int pfrrptcodi);
    }
}
