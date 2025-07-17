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
    /// Clase de acceso a datos de la tabla IN_ARCHIVO
    /// </summary>
    public class InArchivoRepository : RepositoryBase, IInArchivoRepository
    {
        public InArchivoRepository(string strConn) : base(strConn)
        {
        }

        InArchivoHelper helper = new InArchivoHelper();

        public int GetMaxId()
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            int id = 1;

            object result = dbProvider.ExecuteScalar(command);
            if (result != null) id = Convert.ToInt32(result);

            return id;
        }

        public int Save(InArchivoDTO entity, IDbConnection conn, DbTransaction tran)
        {
            DbCommand command = (DbCommand)conn.CreateCommand();
            command.CommandText = helper.SqlSave;
            command.Transaction = tran;
            command.Connection = (DbConnection)conn;

            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Inarchcodi, DbType.Int32, entity.Inarchcodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Inarchnombreoriginal, DbType.String, entity.Inarchnombreoriginal));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Inarchnombrefisico, DbType.String, entity.Inarchnombrefisico));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Inarchorden, DbType.Int32, entity.Inarchorden));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Inarchestado, DbType.Int32, entity.Inarchestado));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Inarchtipo, DbType.Int32, entity.Inarchtipo));

            command.ExecuteNonQuery();
            return entity.Inarchcodi;
        }

        public void Update(InArchivoDTO entity, IDbConnection conn, DbTransaction tran)
        {
            DbCommand command = (DbCommand)conn.CreateCommand();
            command.CommandText = helper.SqlUpdate;
            command.Transaction = tran;
            command.Connection = (DbConnection)conn;

            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Inarchnombreoriginal, DbType.String, entity.Inarchnombreoriginal));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Inarchnombrefisico, DbType.String, entity.Inarchnombrefisico));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Inarchorden, DbType.Int32, entity.Inarchorden));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Inarchestado, DbType.Int32, entity.Inarchestado));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Inarchtipo, DbType.Int32, entity.Inarchtipo));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Inarchcodi, DbType.Int32, entity.Inarchcodi));

            command.ExecuteNonQuery();
        }

        public void Delete(int inarchcodi, IDbConnection conn, DbTransaction tran)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Inarchcodi, DbType.Int32, inarchcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public InArchivoDTO GetById(int inarchcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Inarchcodi, DbType.Int32, inarchcodi);
            InArchivoDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<InArchivoDTO> List()
        {
            List<InArchivoDTO> entitys = new List<InArchivoDTO>();
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

        public List<InArchivoDTO> GetByCriteria(int Infvercodi, string infmmhoja)
        {
            List<InArchivoDTO> entitys = new List<InArchivoDTO>();

            string sql = string.Format(helper.SqlGetByCriteria, Infvercodi, infmmhoja);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    InArchivoDTO entity = helper.Create(dr);

                    int iInfmmcodi = dr.GetOrdinal(helper.Infmmcodi);
                    if (!dr.IsDBNull(iInfmmcodi)) entity.Infmmcodi = Convert.ToInt32(dr.GetValue(iInfmmcodi));

                    int iInfvercodi = dr.GetOrdinal(helper.Infvercodi);
                    if (!dr.IsDBNull(iInfvercodi)) entity.Infvercodi = Convert.ToInt32(dr.GetValue(iInfvercodi));

                    int iInfmmhoja = dr.GetOrdinal(helper.Infmmhoja);
                    if (!dr.IsDBNull(iInfmmhoja)) entity.Infmmhoja = Convert.ToInt32(dr.GetValue(iInfmmhoja));

                    int iEmprnomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    int iAreanomb = dr.GetOrdinal(helper.Areanomb);
                    if (!dr.IsDBNull(iAreanomb)) entity.Areanomb = dr.GetString(iAreanomb);

                    int iEquiabrev = dr.GetOrdinal(helper.Equiabrev);
                    if (!dr.IsDBNull(iEquiabrev)) entity.Equiabrev = dr.GetString(iEquiabrev);

                    int iEquicodi = dr.GetOrdinal(helper.Equicodi);
                    if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

                    int iEmprcodi = dr.GetOrdinal(helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<InArchivoDTO> ListByIntervencion(string intercodis)
        {
            List<InArchivoDTO> entitys = new List<InArchivoDTO>();

            string sql = string.Format(helper.SqlListByIntervencion, intercodis);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    InArchivoDTO entity = helper.Create(dr);

                    int iIntercodi = dr.GetOrdinal(helper.Intercodi);
                    if (!dr.IsDBNull(iIntercodi)) entity.Intercodi = Convert.ToInt32(dr.GetValue(iIntercodi));

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<InArchivoDTO> ListByMensaje(string msgcodis)
        {
            List<InArchivoDTO> entitys = new List<InArchivoDTO>();

            string sql = string.Format(helper.SqlListByMensaje, msgcodis);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    InArchivoDTO entity = helper.Create(dr);

                    int iMsgcodi = dr.GetOrdinal(helper.Msgcodi);
                    if (!dr.IsDBNull(iMsgcodi)) entity.Msgcodi = Convert.ToInt32(dr.GetValue(iMsgcodi));

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<InArchivoDTO> ListBySustento(string instcodis)
        {
            List<InArchivoDTO> entitys = new List<InArchivoDTO>();

            string sql = string.Format(helper.SqlListBySustento, instcodis);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    InArchivoDTO entity = helper.Create(dr);

                    int iInstcodi = dr.GetOrdinal(helper.Instcodi);
                    if (!dr.IsDBNull(iInstcodi)) entity.Instcodi = Convert.ToInt32(dr.GetValue(iInstcodi));

                    int iInstdcodi = dr.GetOrdinal(helper.Instdcodi);
                    if (!dr.IsDBNull(iInstdcodi)) entity.Instdcodi = Convert.ToInt32(dr.GetValue(iInstdcodi));

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<InArchivoDTO> ListarArchivoSinFormato(int tipo, string prefijo)
        {
            List<InArchivoDTO> entitys = new List<InArchivoDTO>();

            string sql = string.Format(helper.SqlListarArchivoSinFormato, tipo, prefijo);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    InArchivoDTO entity = helper.Create(dr);

                    int iIntercodi = dr.GetOrdinal(helper.Intercodi);
                    if (!dr.IsDBNull(iIntercodi)) entity.Intercodi = Convert.ToInt32(dr.GetValue(iIntercodi));

                    int iProgrcodi = dr.GetOrdinal(helper.Progrcodi);
                    if (!dr.IsDBNull(iProgrcodi)) entity.Progrcodi = Convert.ToInt32(dr.GetValue(iProgrcodi));

                    int iIntercarpetafiles = dr.GetOrdinal(helper.Intercarpetafiles);
                    if (!dr.IsDBNull(iIntercarpetafiles)) entity.Intercarpetafiles = Convert.ToInt32(dr.GetValue(iIntercarpetafiles));

                    entitys.Add(entity);
                }
            }

            return entitys;
        }
    }
}
