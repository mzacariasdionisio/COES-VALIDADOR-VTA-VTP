using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using COES.Dominio.DTO.Sic;
using COES.Dominio.Interfaces.Sic;
using COES.Base.Core;
using COES.Infraestructura.Datos.Helper.Sic;

namespace COES.Infraestructura.Datos.Repositorio.Sic
{
    /// <summary>
    /// Clase de acceso a datos de la tabla AUD_REQUERIMIENTO_INFORM
    /// </summary>
    public class AudRequerimientoInfoArchivoRepository: RepositoryBase, IAudRequerimientoInfoArchivoRepository
    {
        public AudRequerimientoInfoArchivoRepository(string strConn): base(strConn)
        {
        }

        AudRequerimientoInfoArchivoHelper helper = new AudRequerimientoInfoArchivoHelper();

        public int Save(AudRequerimientoInfoArchivoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Reqicodiarch, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Reqicodi, DbType.Int32, entity.Reqicodi);
            dbProvider.AddInParameter(command, helper.Archcodi, DbType.Int32, entity.Archcodi);
           
            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Delete(AudRequerimientoInfoArchivoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Reqicodi, DbType.Int32, entity.Reqicodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public List<AudRequerimientoInfoArchivoDTO> GetByCriteria(int reqicodi)
        {
            List<AudRequerimientoInfoArchivoDTO> entitys = new List<AudRequerimientoInfoArchivoDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByCriteria);
            dbProvider.AddInParameter(command, helper.Reqicodi, DbType.Int32, reqicodi);
            
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    AudRequerimientoInfoArchivoDTO requerimientoInform = helper.Create(dr);

                    int iReqicodiarch = dr.GetOrdinal(helper.Reqicodiarch);
                    if (!dr.IsDBNull(iReqicodiarch)) requerimientoInform.Reqicodiarch = Convert.ToInt32(dr.GetValue(iReqicodiarch));

                    int iReqicodi = dr.GetOrdinal(helper.Reqicodi);
                    if (!dr.IsDBNull(iReqicodi)) requerimientoInform.Reqicodi = Convert.ToInt32(dr.GetValue(iReqicodi));

                    int iArchcodi = dr.GetOrdinal(helper.Archcodi);
                    if (!dr.IsDBNull(iArchcodi)) requerimientoInform.Archcodi = Convert.ToInt32(dr.GetValue(iArchcodi));

                    entitys.Add(requerimientoInform);
                }
            }

            return entitys;
        }

    }
}
