using COES.Dominio.DTO.Sic;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla SI_HISEMPPTO
    /// </summary>
    public interface ISiHisempptoRepository
    {
        int GetMaxId();
        int Save(SiHisempptoDTO entity, IDbConnection conn, DbTransaction tran);
        void Update(SiHisempptoDTO entity);
        void DeleteLogico(SiHisempptoDTO entity);
        void Delete(int hempptcodi);
        SiHisempptoDTO GetById(int hempptcodi);
        List<SiHisempptoDTO> List();
        List<SiHisempptoDTO> GetByCriteria();

        void UpdateAnularTransf(int migracodi, IDbConnection conn, DbTransaction tran);
        List<SiHisempptoDTO> ListPtsXMigracion(int migracodi);
        int ConsultarPtosMigracion(int migracodi, int ptomedicodi, DateTime fechacorte);
    }
}
