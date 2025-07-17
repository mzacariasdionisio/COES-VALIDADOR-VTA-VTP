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
    /// Clase de acceso a datos de la tabla IN_DESTINATARIOMENSAJE_DET
    /// </summary>
    public class InDestinatariomensajeDetRepository : RepositoryBase, IInDestinatariomensajeDetRepository
    {
        public InDestinatariomensajeDetRepository(string strConn) : base(strConn)
        {
        }

        InDestinatariomensajeDetHelper helper = new InDestinatariomensajeDetHelper();

        public int Save(InDestinatariomensajeDetDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Evenclasecodi, DbType.Int32, entity.Evenclasecodi);
            dbProvider.AddInParameter(command, helper.Indmdecodi, DbType.Int32, id);

            dbProvider.AddInParameter(command, helper.Indemecodi, DbType.Int32, entity.Indemecodi);
            dbProvider.AddInParameter(command, helper.Indmdeacceso, DbType.Int32, entity.Indmdeacceso);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(InDestinatariomensajeDetDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Evenclasecodi, DbType.Int32, entity.Evenclasecodi);
            dbProvider.AddInParameter(command, helper.Indmdecodi, DbType.Int32, entity.Indmdecodi);
            dbProvider.AddInParameter(command, helper.Indemecodi, DbType.Int32, entity.Indemecodi);
            dbProvider.AddInParameter(command, helper.Indmdeacceso, DbType.Int32, entity.Indmdeacceso);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int indmdecodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Indmdecodi, DbType.Int32, indmdecodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public InDestinatariomensajeDetDTO GetById(int indmdecodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Indmdecodi, DbType.Int32, indmdecodi);
            InDestinatariomensajeDetDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<InDestinatariomensajeDetDTO> List()
        {
            List<InDestinatariomensajeDetDTO> entitys = new List<InDestinatariomensajeDetDTO>();
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

        public List<InDestinatariomensajeDetDTO> GetByCriteria()
        {
            List<InDestinatariomensajeDetDTO> entitys = new List<InDestinatariomensajeDetDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByCriteria);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }
    }
}
