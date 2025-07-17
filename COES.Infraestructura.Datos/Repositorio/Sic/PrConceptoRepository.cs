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
    /// Clase de acceso a datos de la tabla PR_CONCEPTO
    /// </summary>
    public class PrConceptoRepository : RepositoryBase, IPrConceptoRepository
    {
        public PrConceptoRepository(string strConn) : base(strConn)
        {
        }

        PrConceptoHelper helper = new PrConceptoHelper();

        public int Save(PrConceptoDTO entity, IDbConnection connection, IDbTransaction transaction)
        {
            using (var dbCommand = (DbCommand)connection.CreateCommand())
            {
                dbCommand.Transaction = (DbTransaction)transaction;
                dbCommand.Connection = (DbConnection)connection;
                dbCommand.CommandText = helper.SqlGetMaxId;

                object result = dbCommand.ExecuteScalar();
                int id = result == null ? 1 : Convert.ToInt32(result);

                dbCommand.CommandText = helper.SqlSave;

                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Concepcodi, DbType.Int32, id));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Concepnombficha, DbType.String, entity.Concepnombficha));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Concepdefinicion, DbType.String, entity.Concepdefinicion));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Conceptipolong1, DbType.Int32, entity.Conceptipolong1));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Conceptipolong2, DbType.Int32, entity.Conceptipolong2));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Concepusucreacion, DbType.String, entity.Concepusucreacion));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Concepfeccreacion, DbType.DateTime, entity.Concepfeccreacion));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Concepusumodificacion, DbType.String, entity.Concepusumodificacion));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Concepfecmodificacion, DbType.DateTime, entity.Concepfecmodificacion));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Concepocultocomentario, DbType.String, entity.Concepocultocomentario));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Catecodi, DbType.Int32, entity.Catecodi));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Concepabrev, DbType.String, entity.Concepabrev));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Concepdesc, DbType.String, entity.Concepdesc));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Concepunid, DbType.String, entity.Concepunid));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Conceptipo, DbType.String, entity.Conceptipo));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Conceporden, DbType.Int32, entity.Conceporden));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Concepfichatec, DbType.String, entity.Concepfichatec));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Concepactivo, DbType.String, entity.Concepactivo));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Conceppropeq, DbType.Int32, entity.Conceppropeq));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Concepliminf, DbType.Decimal, entity.Concepliminf));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Conceplimsup, DbType.Decimal, entity.Conceplimsup));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Concepflagcolor, DbType.Int32, entity.Concepflagcolor));

                dbCommand.ExecuteNonQuery();
                return id;
            }
        }

        public void Update(PrConceptoDTO entity, IDbConnection connection, IDbTransaction transaction)
        {

            using (var dbCommand = (DbCommand)connection.CreateCommand())
            {
                dbCommand.Transaction = (DbTransaction)transaction;
                dbCommand.Connection = (DbConnection)connection;
                dbCommand.CommandText = helper.SqlUpdate;

                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Concepnombficha, DbType.String, entity.Concepnombficha));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Concepdefinicion, DbType.String, entity.Concepdefinicion));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Conceptipolong1, DbType.Int32, entity.Conceptipolong1));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Conceptipolong2, DbType.Int32, entity.Conceptipolong2));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Concepusucreacion, DbType.String, entity.Concepusucreacion));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Concepfeccreacion, DbType.DateTime, entity.Concepfeccreacion));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Concepusumodificacion, DbType.String, entity.Concepusumodificacion));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Concepfecmodificacion, DbType.DateTime, entity.Concepfecmodificacion));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Concepocultocomentario, DbType.String, entity.Concepocultocomentario));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Catecodi, DbType.Int32, entity.Catecodi));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Concepabrev, DbType.String, entity.Concepabrev));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Concepdesc, DbType.String, entity.Concepdesc));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Concepunid, DbType.String, entity.Concepunid));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Conceptipo, DbType.String, entity.Conceptipo));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Conceporden, DbType.Int32, entity.Conceporden));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Concepfichatec, DbType.String, entity.Concepfichatec));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Concepactivo, DbType.String, entity.Concepactivo));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Conceppropeq, DbType.Int32, entity.Conceppropeq));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Concepliminf, DbType.Decimal, entity.Concepliminf));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Conceplimsup, DbType.Decimal, entity.Conceplimsup));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Concepflagcolor, DbType.Int32, entity.Concepflagcolor));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Concepcodi, DbType.Int32, entity.Concepcodi));

                dbCommand.ExecuteNonQuery();
            }
        }

        public void Delete(int concepcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Concepcodi, DbType.Int32, concepcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public PrConceptoDTO GetById(int concepcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Concepcodi, DbType.Int32, concepcodi);
            PrConceptoDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<PrConceptoDTO> List()
        {
            List<PrConceptoDTO> entitys = new List<PrConceptoDTO>();
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

        public List<PrConceptoDTO> GetByCriteria(string concepcodis)
        {
            List<PrConceptoDTO> entitys = new List<PrConceptoDTO>();

            string sql = string.Format(helper.SqlGetByCriteria, concepcodis);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }

        #region MigracionSGOCOES-GrupoB
        public List<PrConceptoDTO> ListByCatecodi(string catecodi)
        {
            List<PrConceptoDTO> entitys = new List<PrConceptoDTO>();

            string query = string.Format(helper.SqlListByCatecodi, catecodi);

            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    PrConceptoDTO entity = helper.Create(dr);

                    int iCatenomb = dr.GetOrdinal(this.helper.Catenomb);
                    if (!dr.IsDBNull(iCatenomb)) entity.Catenomb = dr.GetString(iCatenomb);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }
        #endregion

        #region Ficha Tecnica

        public List<PrConceptoDTO> ListarConceptosxFiltro(int catecodi, string nombre, int estado)
        {
            List<PrConceptoDTO> entitys = new List<PrConceptoDTO>();

            string strCommandText = string.Format(helper.SqlListarConceptosxFiltro, catecodi, nombre.ToUpperInvariant(), estado.ToString());
            DbCommand command = dbProvider.GetSqlStringCommand(strCommandText);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    var entity = helper.Create(dr);

                    int iCatenomb = dr.GetOrdinal(helper.Catenomb);
                    if (!dr.IsDBNull(iCatenomb)) entity.Catenomb = dr.GetString(iCatenomb);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        #endregion
    }
}
