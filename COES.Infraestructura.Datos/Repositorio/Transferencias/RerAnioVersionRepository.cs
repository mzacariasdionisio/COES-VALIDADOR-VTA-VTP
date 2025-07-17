using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using COES.Dominio.DTO.Transferencias;
using COES.Dominio.Interfaces.Transferencias;
using COES.Base.Core;
using COES.Infraestructura.Datos.Helper.Transferencias;

namespace COES.Infraestructura.Datos.Repositorio.Transferencias
{
    /// <summary>
    /// Clase de acceso a datos de la tabla RER_ANIOVERSION
    /// </summary>
    public class RerAnioVersionRepository : RepositoryBase, IRerAnioVersionRepository
    {
        public RerAnioVersionRepository(string strConn)
            : base(strConn)
        {
        }

        RerAnioVersionHelper helper = new RerAnioVersionHelper();

        public int Save(RerAnioVersionDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Reravcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Reravversion, DbType.String, entity.Reravversion);
            dbProvider.AddInParameter(command, helper.Reravaniotarif, DbType.Int32, entity.Reravaniotarif);
            dbProvider.AddInParameter(command, helper.Reravaniotarifdesc, DbType.String, entity.Reravaniotarifdesc);
            dbProvider.AddInParameter(command, helper.Reravinflacion, DbType.Decimal, entity.Reravinflacion);
            dbProvider.AddInParameter(command, helper.Reravestado, DbType.String, entity.Reravestado);
            dbProvider.AddInParameter(command, helper.Reravusucreacion, DbType.String, entity.Reravusucreacion);
            dbProvider.AddInParameter(command, helper.Reravfeccreacion, DbType.DateTime, entity.Reravfeccreacion);
            dbProvider.AddInParameter(command, helper.Reravusumodificacion, DbType.String, entity.Reravusumodificacion);
            dbProvider.AddInParameter(command, helper.Reravfecmodificacion, DbType.DateTime, entity.Reravfecmodificacion);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(RerAnioVersionDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Reravversion, DbType.String, entity.Reravversion);
            dbProvider.AddInParameter(command, helper.Reravaniotarif, DbType.Int32, entity.Reravaniotarif);
            dbProvider.AddInParameter(command, helper.Reravaniotarifdesc, DbType.String, entity.Reravaniotarifdesc);
            dbProvider.AddInParameter(command, helper.Reravinflacion, DbType.Decimal, entity.Reravinflacion);
            dbProvider.AddInParameter(command, helper.Reravestado, DbType.String, entity.Reravestado);
            dbProvider.AddInParameter(command, helper.Reravusumodificacion, DbType.String, entity.Reravusumodificacion);
            dbProvider.AddInParameter(command, helper.Reravfecmodificacion, DbType.DateTime, entity.Reravfecmodificacion);
            dbProvider.AddInParameter(command, helper.Reravcodi, DbType.Int32, entity.Reravcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int rerAvCodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Reravcodi, DbType.Int32, rerAvCodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public RerAnioVersionDTO GetById(int rerAvCodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Reravcodi, DbType.Int32, rerAvCodi);
            RerAnioVersionDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);

                    int iReravversiondesc = dr.GetOrdinal(helper.Reravversiondesc);
                    if (!dr.IsDBNull(iReravversiondesc)) entity.Reravversiondesc = dr.GetString(iReravversiondesc);
                }
            }

            return entity;
        }

        public List<RerAnioVersionDTO> List()
        {
            List<RerAnioVersionDTO> entities = new List<RerAnioVersionDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlList);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    RerAnioVersionDTO entity = helper.Create(dr);

                    int iReravversiondesc = dr.GetOrdinal(helper.Reravversiondesc);
                    if (!dr.IsDBNull(iReravversiondesc)) entity.Reravversiondesc = dr.GetString(iReravversiondesc);

                    entities.Add(entity);
                }
            }

            return entities;
        }

        public RerAnioVersionDTO GetByAnnioAndVersion(int reravaniotarif, string reravversion)
        {
            string query = string.Format(helper.SqlGetByAnioAndVersion, reravversion, reravaniotarif);
            DbCommand command = dbProvider.GetSqlStringCommand(query);
            RerAnioVersionDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                int i = 0;
                while (dr.Read())
                {
                    entity = helper.Create(dr);

                    int iReravversiondesc = dr.GetOrdinal(helper.Reravversiondesc);
                    if (!dr.IsDBNull(iReravversiondesc)) entity.Reravversiondesc = dr.GetString(iReravversiondesc);

                    i++;
                }

                if (i > 1)
                {
                    throw new Exception(string.Format("Existe más de un registro en la tabla rer_anioversion con los valores de reravversion = {0} y reravaniotarif = {1}", reravversion, reravaniotarif));
                }
            }
            return entity;
        }

        #region CU23
        public RerAnioVersionDTO GetByAnioVersion(int iRerAVAnioTarif, int iRerAVVersion)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByAnioVersion);

            dbProvider.AddInParameter(command, helper.Reravaniotarif, DbType.Int32, iRerAVAnioTarif);
            dbProvider.AddInParameter(command, helper.Reravversion, DbType.Int32, iRerAVVersion);
            RerAnioVersionDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }
        #endregion

        public List<RerAnioVersionDTO> ListRerAnioVersionesByAnio(int iRerAVAnioTarif)
        {
            List<RerAnioVersionDTO> entities = new List<RerAnioVersionDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListRerAnioVersionesByAnio);
            dbProvider.AddInParameter(command, helper.Reravaniotarif, DbType.Int32, iRerAVAnioTarif);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entities.Add(helper.Create(dr));
                }
            }

            return entities;
        }
    }
}

