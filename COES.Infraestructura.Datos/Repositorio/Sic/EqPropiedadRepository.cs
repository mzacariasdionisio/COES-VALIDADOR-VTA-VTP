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
    /// Clase de acceso a datos de la tabla EQ_PROPIEDAD
    /// </summary>
    public class EqPropiedadRepository : RepositoryBase, IEqPropiedadRepository
    {
        public EqPropiedadRepository(string strConn) : base(strConn)
        {
        }

        EqPropiedadHelper helper = new EqPropiedadHelper();

        public int Save(EqPropiedadDTO entity, IDbConnection connection, IDbTransaction transaction)
        {

            using (var dbCommand = (DbCommand)connection.CreateCommand())
            {
                dbCommand.Transaction = (DbTransaction)transaction;
                dbCommand.Connection = (DbConnection)connection;
                dbCommand.CommandText = helper.SqlGetMaxId;

                object result = dbCommand.ExecuteScalar();
                int id = result == null ? 1 : Convert.ToInt32(result);

                dbCommand.CommandText = helper.SqlSave;

                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Propcodi, DbType.Int32, id));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Propnombficha, DbType.String, entity.Propnombficha));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Proptipolong1, DbType.Int32, entity.Proptipolong1));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Proptipolong2, DbType.Int32, entity.Proptipolong2));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Propactivo, DbType.Int32, entity.Propactivo));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Propusucreacion, DbType.String, entity.Propusucreacion));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Propfeccreacion, DbType.DateTime, entity.Propfeccreacion));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Propocultocomentario, DbType.String, entity.Propocultocomentario));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Famcodi, DbType.Int32, entity.Famcodi));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Propabrev, DbType.String, entity.Propabrev));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Propnomb, DbType.String, entity.Propnomb));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Propunidad, DbType.String, entity.Propunidad));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Orden, DbType.Int32, entity.Orden));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Proptipo, DbType.String, entity.Proptipo));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Propdefinicion, DbType.String, entity.Propdefinicion));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Propcodipadre, DbType.Int32, entity.Propcodipadre));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Propusumodificacion, DbType.String, entity.Propusumodificacion));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Propfecmodificacion, DbType.DateTime, entity.Propfecmodificacion));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Propfichaoficial, DbType.String, entity.Propfichaoficial));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Propliminf, DbType.Decimal, entity.Propliminf));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Proplimsup, DbType.Decimal, entity.Proplimsup));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Propflagcolor, DbType.Int32, entity.Propflagcolor));

                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Propformula, DbType.String, entity.Propformula ?? ""));

                dbCommand.ExecuteNonQuery();
                return id;
            }
        }

        public void Update(EqPropiedadDTO entity, IDbConnection connection, IDbTransaction transaction)
        {

            using (var dbCommand = (DbCommand)connection.CreateCommand())
            {
                dbCommand.Transaction = (DbTransaction)transaction;
                dbCommand.Connection = (DbConnection)connection;
                dbCommand.CommandText = helper.SqlUpdate;

                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Propnombficha, DbType.String, entity.Propnombficha));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Proptipolong1, DbType.Int32, entity.Proptipolong1));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Proptipolong2, DbType.Int32, entity.Proptipolong2));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Propactivo, DbType.Int32, entity.Propactivo));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Propusucreacion, DbType.String, entity.Propusucreacion));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Propfeccreacion, DbType.DateTime, entity.Propfeccreacion));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Propocultocomentario, DbType.String, entity.Propocultocomentario));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Famcodi, DbType.Int32, entity.Famcodi));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Propabrev, DbType.String, entity.Propabrev));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Propnomb, DbType.String, entity.Propnomb));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Propunidad, DbType.String, entity.Propunidad));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Orden, DbType.Int32, entity.Orden));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Proptipo, DbType.String, entity.Proptipo));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Propdefinicion, DbType.String, entity.Propdefinicion));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Propcodipadre, DbType.Int32, entity.Propcodipadre));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Propusumodificacion, DbType.String, entity.Propusumodificacion));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Propfecmodificacion, DbType.DateTime, entity.Propfecmodificacion));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Propfichaoficial, DbType.String, entity.Propfichaoficial));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Propliminf, DbType.Decimal, entity.Propliminf));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Proplimsup, DbType.Decimal, entity.Proplimsup));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Propflagcolor, DbType.Int32, entity.Propflagcolor));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Propformula, DbType.String, entity.Propformula ?? ""));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Propcodi, DbType.Int32, entity.Propcodi));

                dbCommand.ExecuteNonQuery();
            }
        }

        public void Delete(int propcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Propcodi, DbType.Int32, propcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete_UpdateAuditoria(int propcodi, string username)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Propusumodificacion, DbType.String, username);
            dbProvider.AddInParameter(command, helper.Propcodi, DbType.Int32, propcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public EqPropiedadDTO GetById(int propcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Propcodi, DbType.Int32, propcodi);
            EqPropiedadDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                    int iFamnomb = dr.GetOrdinal("FAMNOMB");
                    if (!dr.IsDBNull(iFamnomb)) entity.NombreFamilia = dr.GetString(iFamnomb);
                    //int ipadrenomb = dr.GetOrdinal("PADRENOMB");
                    //if (!dr.IsDBNull(ipadrenomb)) entity.NombrePadre = dr.GetString(ipadrenomb);

                    int iPropFormula = dr.GetOrdinal("PROPFORMULA");
                    if (!dr.IsDBNull(iPropFormula)) entity.Propformula = dr.GetString(iPropFormula);
                }
            }

            return entity;
        }

        public List<EqPropiedadDTO> List()
        {
            List<EqPropiedadDTO> entitys = new List<EqPropiedadDTO>();
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

        public List<EqPropiedadDTO> GetByCriteria(int famcodi, string nombre, int estado)
        {
            List<EqPropiedadDTO> entitys = new List<EqPropiedadDTO>();

            string strCommandText = string.Format(helper.SqlGetByCriteria, famcodi, nombre.ToUpperInvariant(), estado);
            DbCommand command = dbProvider.GetSqlStringCommand(strCommandText);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    var entity = helper.Create(dr);

                    int iFamnomb = dr.GetOrdinal("FAMNOMB");
                    if (!dr.IsDBNull(iFamnomb)) entity.NombreFamilia = dr.GetString(iFamnomb);

                    int iPropFormula = dr.GetOrdinal("PROPFORMULA");
                    if (!dr.IsDBNull(iPropFormula)) entity.Propformula = dr.GetString(iPropFormula);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        #region MigracionSGOCOES-GrupoB
        public List<EqPropiedadDTO> ListByFamcodi(int famcodi)
        {
            List<EqPropiedadDTO> entitys = new List<EqPropiedadDTO>();

            string query = string.Format(helper.SqlListByFamcodi, famcodi);

            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    EqPropiedadDTO entity = helper.Create(dr);

                    int iFamnomb = dr.GetOrdinal(this.helper.Famnomb);
                    if (!dr.IsDBNull(iFamnomb)) entity.NombreFamilia = dr.GetString(iFamnomb);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }        
        #endregion

        #region Ficha Tecnica 2023
        public List<EqPropiedadDTO> ListByIds(string propcodis)
        {
            List<EqPropiedadDTO> entitys = new List<EqPropiedadDTO>();
            string query = string.Format(helper.SqlListByIds, propcodis);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }

        #endregion
    }
}
