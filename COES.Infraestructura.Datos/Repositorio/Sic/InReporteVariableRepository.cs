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
    /// Clase de acceso a datos de la tabla IN_REPORTE_VARIABLE
    /// </summary>
    public class InReporteVariableRepository : RepositoryBase, IInReporteVariableRepository
    {
        public InReporteVariableRepository(string strConn) : base(strConn)
        {
        }

        InReporteVariableHelper helper = new InReporteVariableHelper();

        public int Save(InReporteVariableDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Inrevacodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Invarcodi, DbType.Int32, entity.Invarcodi);
            dbProvider.AddInParameter(command, helper.Inrevavalor, DbType.String, entity.Inrevavalor);
            dbProvider.AddInParameter(command, helper.Inrevausucreacion, DbType.String, entity.Inrevausucreacion);
            dbProvider.AddInParameter(command, helper.Inrevafeccreacion, DbType.DateTime, entity.Inrevafeccreacion);
            dbProvider.AddInParameter(command, helper.Inrevausumodificacion, DbType.String, entity.Inrevausumodificacion);
            dbProvider.AddInParameter(command, helper.Inrevafecmodificacion, DbType.DateTime, entity.Inrevafecmodificacion);
            dbProvider.AddInParameter(command, helper.Inrepcodi, DbType.Int32, entity.Inrepcodi);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(InReporteVariableDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Invarcodi, DbType.Int32, entity.Invarcodi);
            dbProvider.AddInParameter(command, helper.Inrevavalor, DbType.String, entity.Inrevavalor);
            dbProvider.AddInParameter(command, helper.Inrevausucreacion, DbType.String, entity.Inrevausucreacion);
            dbProvider.AddInParameter(command, helper.Inrevafeccreacion, DbType.DateTime, entity.Inrevafeccreacion);
            dbProvider.AddInParameter(command, helper.Inrevausumodificacion, DbType.String, entity.Inrevausumodificacion);
            dbProvider.AddInParameter(command, helper.Inrevafecmodificacion, DbType.DateTime, entity.Inrevafecmodificacion);
            dbProvider.AddInParameter(command, helper.Inrepcodi, DbType.Int32, entity.Inrepcodi);
            dbProvider.AddInParameter(command, helper.Inrevacodi, DbType.Int32, entity.Inrevacodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int inrevacodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Inrevacodi, DbType.Int32, inrevacodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public InReporteVariableDTO GetById(int inrevacodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Inrevacodi, DbType.Int32, inrevacodi);
            InReporteVariableDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<InReporteVariableDTO> List()
        {
            List<InReporteVariableDTO> entitys = new List<InReporteVariableDTO>();
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

        public List<InReporteVariableDTO> GetByCriteria(int progcodi, int tipo)
        {
            List<InReporteVariableDTO> entitys = new List<InReporteVariableDTO>();
            string query = string.Format(helper.SqlGetByCriteria, progcodi, tipo);

            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    InReporteVariableDTO entity = helper.Create(dr);

                    int iInvardescripcion = dr.GetOrdinal(helper.Invardescripcion);
                    if (!dr.IsDBNull(iInvardescripcion)) entity.Invardescripcion = dr.GetString(iInvardescripcion);

                    int iInvaridentificador = dr.GetOrdinal(helper.Invaridentificador);
                    if (!dr.IsDBNull(iInvaridentificador)) entity.Invaridentificador = dr.GetString(iInvaridentificador);

                    int iInvarnota = dr.GetOrdinal(helper.Invarnota);
                    if (!dr.IsDBNull(iInvarnota)) entity.Invarnota = dr.GetString(iInvarnota);

                    int iInvartipodato = dr.GetOrdinal(helper.Invartipodato);
                    if (!dr.IsDBNull(iInvartipodato)) entity.Invartipodato = dr.GetString(iInvartipodato);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }
    }
}
