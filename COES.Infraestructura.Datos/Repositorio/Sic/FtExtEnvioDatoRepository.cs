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
    /// Clase de acceso a datos de la tabla FT_EXT_ENVIO_DATO
    /// </summary>
    public class FtExtEnvioDatoRepository : RepositoryBase, IFtExtEnvioDatoRepository
    {
        public FtExtEnvioDatoRepository(string strConn) : base(strConn)
        {
        }

        FtExtEnvioDatoHelper helper = new FtExtEnvioDatoHelper();

        public int GetMaxId()
        {

            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            int id = 1;

            object result = dbProvider.ExecuteScalar(command);
            if (result != null) id = Convert.ToInt32(result);

            return id;
        }

        public int Save(FtExtEnvioDatoDTO entity, IDbConnection conn, DbTransaction tran)
        {
            DbCommand command = (DbCommand)conn.CreateCommand();
            command.CommandText = helper.SqlSave;
            command.Transaction = tran;
            command.Connection = (DbConnection)conn;

            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Ftedatcodi, DbType.Int32, entity.Ftedatcodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Fteeqcodi, DbType.Int32, entity.Fteeqcodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Fitcfgcodi, DbType.Int32, entity.Fitcfgcodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Ftedatvalor, DbType.String, entity.Ftedatvalor));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Ftedatflagvalorconf, DbType.String, entity.Ftedatflagvalorconf));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Ftedatcomentario, DbType.String, entity.Ftedatcomentario));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Ftedatflagsustentoconf, DbType.String, entity.Ftedatflagsustentoconf));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Ftedatflageditable, DbType.String, entity.Ftedatflageditable));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Ftedatflagrevisable, DbType.String, entity.Ftedatflagrevisable));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Ftedatflagmodificado, DbType.Int32, entity.Ftedatflagmodificado));
            

            command.ExecuteNonQuery();
            return entity.Ftedatcodi;
        }

        public void Update(FtExtEnvioDatoDTO entity, IDbConnection conn, DbTransaction tran)
        {
            DbCommand command = (DbCommand)conn.CreateCommand();
            command.CommandText = helper.SqlUpdate;
            command.Transaction = tran;
            command.Connection = (DbConnection)conn;

            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Fteeqcodi, DbType.Int32, entity.Fteeqcodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Fitcfgcodi, DbType.Int32, entity.Fitcfgcodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Ftedatvalor, DbType.String, entity.Ftedatvalor));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Ftedatflagvalorconf, DbType.String, entity.Ftedatflagvalorconf));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Ftedatcomentario, DbType.String, entity.Ftedatcomentario));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Ftedatflagsustentoconf, DbType.String, entity.Ftedatflagsustentoconf));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Ftedatflageditable, DbType.String, entity.Ftedatflageditable));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Ftedatflagrevisable, DbType.String, entity.Ftedatflagrevisable));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Ftedatflagmodificado, DbType.Int32, entity.Ftedatflagmodificado));

            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Ftedatcodi, DbType.Int32, entity.Ftedatcodi));

            command.ExecuteNonQuery();
        }

        public void UpdateXVersion(int ftevercodi, IDbConnection conn, DbTransaction tran)
        {
            DbCommand command = (DbCommand)conn.CreateCommand();
            command.CommandText = string.Format(helper.SqlUpdateXVersion, ftevercodi);
            command.Transaction = tran;
            command.Connection = (DbConnection)conn;

            command.ExecuteNonQuery();
        }

        public void UpdateXEquipo(int fteeqcodi, IDbConnection conn, DbTransaction tran)
        {
            DbCommand command = (DbCommand)conn.CreateCommand();
            command.CommandText = string.Format(helper.SqlUpdateXEquipo, fteeqcodi);
            command.Transaction = tran;
            command.Connection = (DbConnection)conn;

            command.ExecuteNonQuery();
        }

        public void UpdateXFtedatcodis(string ftedatcodis, IDbConnection conn, DbTransaction tran)
        {
            DbCommand command = (DbCommand)conn.CreateCommand();
            command.CommandText = string.Format(helper.SqlUpdateXFtedatcodis, ftedatcodis);
            command.Transaction = tran;
            command.Connection = (DbConnection)conn;

            command.ExecuteNonQuery();
        }

        public void Delete(int ftedatcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Ftedatcodi, DbType.Int32, ftedatcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public FtExtEnvioDatoDTO GetById(int ftedatcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Ftedatcodi, DbType.Int32, ftedatcodi);
            FtExtEnvioDatoDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<FtExtEnvioDatoDTO> List()
        {
            List<FtExtEnvioDatoDTO> entitys = new List<FtExtEnvioDatoDTO>();
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

        public List<FtExtEnvioDatoDTO> GetByCriteria(string fteeqcodis)
        {
            List<FtExtEnvioDatoDTO> entitys = new List<FtExtEnvioDatoDTO>();

            string sql = string.Format(helper.SqlGetByCriteria, fteeqcodis);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    var entity = helper.Create(dr);

                    int iFtitcodi = dr.GetOrdinal(helper.Ftitcodi);
                    if (!dr.IsDBNull(iFtitcodi)) entity.Ftitcodi = Convert.ToInt32(dr.GetValue(iFtitcodi));

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<FtExtEnvioDatoDTO> ListarParametros(string strFteeqcodis)
        {
            List<FtExtEnvioDatoDTO> entitys = new List<FtExtEnvioDatoDTO>();

            string sql = string.Format(helper.SqlListarParametros, strFteeqcodis);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    var entity = helper.Create(dr);
                    
                    int iTipoelemento = dr.GetOrdinal(helper.Tipoelemento);
                    if (!dr.IsDBNull(iTipoelemento)) entity.Tipoelemento = dr.GetString(iTipoelemento);

                    int iConcepcodi = dr.GetOrdinal(helper.Concepcodi);
                    if (!dr.IsDBNull(iConcepcodi)) entity.Concepcodi = Convert.ToInt32(dr.GetValue(iConcepcodi));

                    int iPropcodi = dr.GetOrdinal(helper.Propcodi);
                    if (!dr.IsDBNull(iPropcodi)) entity.Propcodi = Convert.ToInt32(dr.GetValue(iPropcodi));

                    int iEquicodi = dr.GetOrdinal(helper.Equicodi);
                    if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

                    int iGrupocodi = dr.GetOrdinal(helper.Grupocodi);
                    if (!dr.IsDBNull(iGrupocodi)) entity.Grupocodi = Convert.ToInt32(dr.GetValue(iGrupocodi));

                    int iFamcodi = dr.GetOrdinal(helper.Famcodi);
                    if (!dr.IsDBNull(iFamcodi)) entity.Famcodi = Convert.ToInt32(dr.GetValue(iFamcodi));

                    int iFtitcodi = dr.GetOrdinal(helper.Ftitcodi);
                    if (!dr.IsDBNull(iFtitcodi)) entity.Ftitcodi = Convert.ToInt32(dr.GetValue(iFtitcodi));

                    int iFtitactivo = dr.GetOrdinal(helper.Ftitactivo);
                    if (!dr.IsDBNull(iFtitactivo)) entity.Ftitactivo = Convert.ToInt32(dr.GetValue(iFtitactivo));

                    int iConcepabrev = dr.GetOrdinal(helper.Concepabrev);
                    if (!dr.IsDBNull(iConcepabrev)) entity.Concepabrev = dr.GetString(iConcepabrev);

                    int iPropabrev = dr.GetOrdinal(helper.Propabrev);
                    if (!dr.IsDBNull(iPropabrev)) entity.Propabrev = dr.GetString(iPropabrev);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }
    }
}
