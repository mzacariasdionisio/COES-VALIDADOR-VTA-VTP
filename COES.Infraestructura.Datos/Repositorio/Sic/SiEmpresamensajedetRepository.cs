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
    /// Clase de acceso a datos de la tabla SI_EMPRESAMENSAJEDET
    /// </summary>
    public class SiEmpresamensajedetRepository : RepositoryBase, ISiEmpresamensajedetRepository
    {
        public SiEmpresamensajedetRepository(string strConn) : base(strConn)
        {
        }

        SiEmpresamensajedetHelper helper = new SiEmpresamensajedetHelper();

        public int Save(SiEmpresamensajedetDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Emsjdtcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);
            dbProvider.AddInParameter(command, helper.Emsjdtcorreo, DbType.String, entity.Emsjdtcorreo);
            dbProvider.AddInParameter(command, helper.Emsjdttipo, DbType.String, entity.Emsjdttipo);
            dbProvider.AddInParameter(command, helper.Emsjdtfeclectura, DbType.DateTime, entity.Emsjdtfeclectura);
            dbProvider.AddInParameter(command, helper.Emsjdtusulectura, DbType.String, entity.Emsjdtusulectura);
            dbProvider.AddInParameter(command, helper.Empmsjcodi, DbType.Int32, entity.Empmsjcodi);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(SiEmpresamensajedetDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);
            dbProvider.AddInParameter(command, helper.Emsjdtcorreo, DbType.String, entity.Emsjdtcorreo);
            dbProvider.AddInParameter(command, helper.Emsjdttipo, DbType.String, entity.Emsjdttipo);
            dbProvider.AddInParameter(command, helper.Emsjdtfeclectura, DbType.DateTime, entity.Emsjdtfeclectura);
            dbProvider.AddInParameter(command, helper.Emsjdtusulectura, DbType.String, entity.Emsjdtusulectura);
            dbProvider.AddInParameter(command, helper.Empmsjcodi, DbType.Int32, entity.Empmsjcodi);

            dbProvider.AddInParameter(command, helper.Emsjdtcodi, DbType.Int32, entity.Emsjdtcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int emsjdtcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Emsjdtcodi, DbType.Int32, emsjdtcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public SiEmpresamensajedetDTO GetById(int emsjdtcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Emsjdtcodi, DbType.Int32, emsjdtcodi);
            SiEmpresamensajedetDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<SiEmpresamensajedetDTO> List()
        {
            List<SiEmpresamensajedetDTO> entitys = new List<SiEmpresamensajedetDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlList);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }

        public List<SiEmpresamensajedetDTO> GetByCriteria(int empresaLectura, int msgcodi, string intercodis)
        {
            List<SiEmpresamensajedetDTO> entitys = new List<SiEmpresamensajedetDTO>();

            string sql = string.Format(helper.SqlGetByCriteria, empresaLectura, msgcodi, intercodis);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    var entity = helper.Create(dr);

                    int iMsgcodi = dr.GetOrdinal(helper.Msgcodi);
                    if (!dr.IsDBNull(iMsgcodi)) entity.Msgcodi = Convert.ToInt32(dr.GetValue(iMsgcodi));

                    entitys.Add(entity);
                }
            }

            return entitys;
        }
    }
}
