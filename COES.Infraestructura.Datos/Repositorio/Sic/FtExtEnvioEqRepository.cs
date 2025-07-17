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
    /// Clase de acceso a datos de la tabla FT_EXT_ENVIO_EQ
    /// </summary>
    public class FtExtEnvioEqRepository : RepositoryBase, IFtExtEnvioEqRepository
    {
        public FtExtEnvioEqRepository(string strConn) : base(strConn)
        {
        }

        FtExtEnvioEqHelper helper = new FtExtEnvioEqHelper();

        public int GetMaxId()
        {

            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            int id = 1;

            object result = dbProvider.ExecuteScalar(command);
            if (result != null) id = Convert.ToInt32(result);

            return id;
        }

        public int Save(FtExtEnvioEqDTO entity, IDbConnection conn, DbTransaction tran)
        {
            DbCommand command = (DbCommand)conn.CreateCommand();
            command.CommandText = helper.SqlSave;
            command.Transaction = tran;
            command.Connection = (DbConnection)conn;

            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Fteeqcodi, DbType.Int32, entity.Fteeqcodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Fteeqestado, DbType.String, entity.Fteeqestado));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Ftevercodi, DbType.Int32, entity.Ftevercodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Equicodi, DbType.Int32, entity.Equicodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Grupocodi, DbType.Int32, entity.Grupocodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Ftfmtcodi, DbType.Int32, entity.Ftfmtcodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Fteeqflagespecial, DbType.Int32, entity.Fteeqflagespecial));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Fteeqcodiorigen, DbType.Int32, entity.Fteeqcodiorigen));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Fteeqflagaprob, DbType.String, entity.Fteeqflagaprob));

            command.ExecuteNonQuery();
            return entity.Fteeqcodi;
        }

        public void Update(FtExtEnvioEqDTO entity, IDbConnection conn, DbTransaction tran)
        {
            DbCommand command = (DbCommand)conn.CreateCommand();
            command.CommandText = helper.SqlUpdate;
            command.Transaction = tran;
            command.Connection = (DbConnection)conn;

            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Fteeqestado, DbType.String, entity.Fteeqestado));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Ftevercodi, DbType.Int32, entity.Ftevercodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Equicodi, DbType.Int32, entity.Equicodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Grupocodi, DbType.Int32, entity.Grupocodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Ftfmtcodi, DbType.Int32, entity.Ftfmtcodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Fteeqflagespecial, DbType.Int32, entity.Fteeqflagespecial));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Fteeqcodiorigen, DbType.Int32, entity.Fteeqcodiorigen));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Fteeqflagaprob, DbType.String, entity.Fteeqflagaprob));

            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Fteeqcodi, DbType.Int32, entity.Fteeqcodi));

            command.ExecuteNonQuery();
        }

        public void UpdateEstado(string fteeqcodis, string estado, IDbConnection conn, DbTransaction tran)
        {
            DbCommand command = (DbCommand)conn.CreateCommand();
            command.CommandText = string.Format(helper.SqlUpdateEstado, fteeqcodis, estado);
            command.Transaction = tran;
            command.Connection = (DbConnection)conn;

            command.ExecuteNonQuery();
        }

        public void Delete(int fteeqcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Fteeqcodi, DbType.Int32, fteeqcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public FtExtEnvioEqDTO GetById(int fteeqcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Fteeqcodi, DbType.Int32, fteeqcodi);
            FtExtEnvioEqDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);

                    int iFtenvcodi = dr.GetOrdinal(helper.Ftenvcodi);
                    if (!dr.IsDBNull(iFtenvcodi)) entity.Ftenvcodi = Convert.ToInt32(dr.GetValue(iFtenvcodi));

                    int iNombreelemento = dr.GetOrdinal(helper.Nombreelemento);
                    if (!dr.IsDBNull(iNombreelemento)) entity.Nombreelemento = dr.GetString(iNombreelemento);
                }
            }

            return entity;
        }

        public List<FtExtEnvioEqDTO> List()
        {
            List<FtExtEnvioEqDTO> entitys = new List<FtExtEnvioEqDTO>();
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

        public List<FtExtEnvioEqDTO> GetByCriteria(string ftevercodis, string estado)
        {
            List<FtExtEnvioEqDTO> entitys = new List<FtExtEnvioEqDTO>();

            string sql = string.Format(helper.SqlGetByCriteria, ftevercodis, estado);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    FtExtEnvioEqDTO entity = helper.Create(dr);

                    int iNombreelemento = dr.GetOrdinal(helper.Nombreelemento);
                    if (!dr.IsDBNull(iNombreelemento)) entity.Nombreelemento = dr.GetString(iNombreelemento);

                    int iIdelemento = dr.GetOrdinal(helper.Idelemento);
                    if (!dr.IsDBNull(iIdelemento)) entity.Idelemento = Convert.ToInt32(dr.GetValue(iIdelemento));

                    int iAreaelemento = dr.GetOrdinal(helper.Areaelemento);
                    if (!dr.IsDBNull(iAreaelemento)) entity.Areaelemento = dr.GetString(iAreaelemento);

                    int iFtenvcodi = dr.GetOrdinal(helper.Ftenvcodi);
                    if (!dr.IsDBNull(iFtenvcodi)) entity.Ftenvcodi = Convert.ToInt32(dr.GetValue(iFtenvcodi));

                    int iNombempresaelemento = dr.GetOrdinal(helper.Nombempresaelemento);
                    if (!dr.IsDBNull(iNombempresaelemento)) entity.Nombempresaelemento = dr.GetString(iNombempresaelemento);

                    int iIdempresaelemento = dr.GetOrdinal(helper.Idempresaelemento);
                    if (!dr.IsDBNull(iIdempresaelemento)) entity.Idempresaelemento = Convert.ToInt32(dr.GetValue(iIdempresaelemento));

                    int iTipoelemento = dr.GetOrdinal(helper.Tipoelemento);
                    if (!dr.IsDBNull(iTipoelemento)) entity.Tipoelemento = dr.GetString(iTipoelemento);

                    int iFamcodi = dr.GetOrdinal(helper.Famcodi);
                    if (!dr.IsDBNull(iFamcodi)) entity.Famcodi = Convert.ToInt32(dr.GetValue(iFamcodi));

                    int iCatecodi = dr.GetOrdinal(helper.Catecodi);
                    if (!dr.IsDBNull(iCatecodi)) entity.Catecodi = Convert.ToInt32(dr.GetValue(iCatecodi));

                    int iEquipadre = dr.GetOrdinal(helper.Equipadre);
                    if (!dr.IsDBNull(iEquipadre)) entity.Equipadre = Convert.ToInt32(dr.GetValue(iEquipadre));

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<FtExtEnvioEqDTO> ListarPorEnvios(string strIdsEnvios)
        {
            List<FtExtEnvioEqDTO> entitys = new List<FtExtEnvioEqDTO>();
            string sql = string.Format(helper.SqlListarPorEnvios, strIdsEnvios);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    FtExtEnvioEqDTO entity = helper.Create(dr);

                    int iNombreelemento = dr.GetOrdinal(helper.Nombreelemento);
                    if (!dr.IsDBNull(iNombreelemento)) entity.Nombreelemento = dr.GetString(iNombreelemento);

                    int iIdelemento = dr.GetOrdinal(helper.Idelemento);
                    if (!dr.IsDBNull(iIdelemento)) entity.Idelemento = Convert.ToInt32(dr.GetValue(iIdelemento));

                    int iAreaelemento = dr.GetOrdinal(helper.Areaelemento);
                    if (!dr.IsDBNull(iAreaelemento)) entity.Areaelemento = dr.GetString(iAreaelemento);

                    int iFtenvcodi = dr.GetOrdinal(helper.Ftenvcodi);
                    if (!dr.IsDBNull(iFtenvcodi)) entity.Ftenvcodi = Convert.ToInt32(dr.GetValue(iFtenvcodi));

                    int iNombempresaelemento = dr.GetOrdinal(helper.Nombempresaelemento);
                    if (!dr.IsDBNull(iNombempresaelemento)) entity.Nombempresaelemento = dr.GetString(iNombempresaelemento);

                    int iIdempresaelemento = dr.GetOrdinal(helper.Idempresaelemento);
                    if (!dr.IsDBNull(iIdempresaelemento)) entity.Idempresaelemento = Convert.ToInt32(dr.GetValue(iIdempresaelemento));

                    int iTipoelemento = dr.GetOrdinal(helper.Tipoelemento);
                    if (!dr.IsDBNull(iTipoelemento)) entity.Tipoelemento = dr.GetString(iTipoelemento);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<FtExtEnvioEqDTO> ListarPorIds(string fteeqcodis)
        {
            List<FtExtEnvioEqDTO> entitys = new List<FtExtEnvioEqDTO>();
            string sql = string.Format(helper.SqlListarPorIds, fteeqcodis);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    FtExtEnvioEqDTO entity = helper.Create(dr);

                    int iNombreelemento = dr.GetOrdinal(helper.Nombreelemento);
                    if (!dr.IsDBNull(iNombreelemento)) entity.Nombreelemento = dr.GetString(iNombreelemento);

                    int iIdelemento = dr.GetOrdinal(helper.Idelemento);
                    if (!dr.IsDBNull(iIdelemento)) entity.Idelemento = Convert.ToInt32(dr.GetValue(iIdelemento));

                    int iTipoelemento = dr.GetOrdinal(helper.Tipoelemento);
                    if (!dr.IsDBNull(iTipoelemento)) entity.Tipoelemento = dr.GetString(iTipoelemento);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<FtExtEnvioEqDTO> GetByVersionYModificacion(string ftevercodis, int flagModificacion)
        {
            List<FtExtEnvioEqDTO> entitys = new List<FtExtEnvioEqDTO>();

            string sql = string.Format(helper.SqlGetByVersionYModificacion, ftevercodis, flagModificacion);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    FtExtEnvioEqDTO entity = helper.Create(dr);

                    int iNombreelemento = dr.GetOrdinal(helper.Nombreelemento);
                    if (!dr.IsDBNull(iNombreelemento)) entity.Nombreelemento = dr.GetString(iNombreelemento);

                    int iIdelemento = dr.GetOrdinal(helper.Idelemento);
                    if (!dr.IsDBNull(iIdelemento)) entity.Idelemento = Convert.ToInt32(dr.GetValue(iIdelemento));

                    int iAreaelemento = dr.GetOrdinal(helper.Areaelemento);
                    if (!dr.IsDBNull(iAreaelemento)) entity.Areaelemento = dr.GetString(iAreaelemento);

                    int iFtenvcodi = dr.GetOrdinal(helper.Ftenvcodi);
                    if (!dr.IsDBNull(iFtenvcodi)) entity.Ftenvcodi = Convert.ToInt32(dr.GetValue(iFtenvcodi));

                    int iNombempresaelemento = dr.GetOrdinal(helper.Nombempresaelemento);
                    if (!dr.IsDBNull(iNombempresaelemento)) entity.Nombempresaelemento = dr.GetString(iNombempresaelemento);

                    int iIdempresaelemento = dr.GetOrdinal(helper.Idempresaelemento);
                    if (!dr.IsDBNull(iIdempresaelemento)) entity.Idempresaelemento = Convert.ToInt32(dr.GetValue(iIdempresaelemento));

                    int iTipoelemento = dr.GetOrdinal(helper.Tipoelemento);
                    if (!dr.IsDBNull(iTipoelemento)) entity.Tipoelemento = dr.GetString(iTipoelemento);

                    int iFamcodi = dr.GetOrdinal(helper.Famcodi);
                    if (!dr.IsDBNull(iFamcodi)) entity.Famcodi = Convert.ToInt32(dr.GetValue(iFamcodi));

                    int iCatecodi = dr.GetOrdinal(helper.Catecodi);
                    if (!dr.IsDBNull(iCatecodi)) entity.Catecodi = Convert.ToInt32(dr.GetValue(iCatecodi));

                    int iFtedatflagmodificado = dr.GetOrdinal(helper.Ftedatflagmodificado);
                    if (!dr.IsDBNull(iFtedatflagmodificado)) entity.Ftedatflagmodificado = Convert.ToInt32(dr.GetValue(iFtedatflagmodificado));

                    int IFtitcodi = dr.GetOrdinal(helper.Ftitcodi);
                    if (!dr.IsDBNull(IFtitcodi)) entity.Ftitcodi = Convert.ToInt32(dr.GetValue(IFtitcodi));

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public int GetTotalXFormatoExtranet(int ftfmtcodi)
        {
            string query = string.Format(helper.SqlGetTotalXFormatoExtranet, ftfmtcodi);
            DbCommand command = dbProvider.GetSqlStringCommand(query);
            int id = 1;

            object result = dbProvider.ExecuteScalar(command);
            if (result != null) id = Convert.ToInt32(result);

            return id;
        }

    }
}
