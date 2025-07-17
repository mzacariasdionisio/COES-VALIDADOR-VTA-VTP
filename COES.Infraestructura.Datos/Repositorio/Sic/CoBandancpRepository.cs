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
    /// Clase de acceso a datos de la tabla CO_BANDANCP
    /// </summary>
    public class CoBandancpRepository: RepositoryBase, ICoBandancpRepository
    {
        public CoBandancpRepository(string strConn): base(strConn)
        {
        }

        CoBandancpHelper helper = new CoBandancpHelper();

        public int Save(CoBandancpDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Bandcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Bandmin, DbType.Decimal, entity.Bandmin);
            dbProvider.AddInParameter(command, helper.Bandmax, DbType.Decimal, entity.Bandmax);
            dbProvider.AddInParameter(command, helper.Grupocodi, DbType.Int32, entity.Grupocodi);
            dbProvider.AddInParameter(command, helper.Bandfecha, DbType.DateTime, entity.Bandfecha);
            dbProvider.AddInParameter(command, helper.Bandusumodificacion, DbType.String, entity.Bandusumodificacion);
            dbProvider.AddInParameter(command, helper.Bandfecmodificacion, DbType.DateTime, entity.Bandfecmodificacion);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(CoBandancpDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Bandcodi, DbType.Int32, entity.Bandcodi);
            dbProvider.AddInParameter(command, helper.Bandmin, DbType.Decimal, entity.Bandmin);
            dbProvider.AddInParameter(command, helper.Bandmax, DbType.Decimal, entity.Bandmax);
            dbProvider.AddInParameter(command, helper.Grupocodi, DbType.Int32, entity.Grupocodi);
            dbProvider.AddInParameter(command, helper.Bandfecha, DbType.DateTime, entity.Bandfecha);
            dbProvider.AddInParameter(command, helper.Bandusumodificacion, DbType.String, entity.Bandusumodificacion);
            dbProvider.AddInParameter(command, helper.Bandfecmodificacion, DbType.DateTime, entity.Bandfecmodificacion);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int bandcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Bandcodi, DbType.Int32, bandcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public CoBandancpDTO GetById(int bandcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Bandcodi, DbType.Int32, bandcodi);
            CoBandancpDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<CoBandancpDTO> List()
        {
            List<CoBandancpDTO> entitys = new List<CoBandancpDTO>();
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

        public List<CoBandancpDTO> GetByCriteria(DateTime fecha, int grupocodi)
        {
            string sqlQuery = string.Format(helper.SqlGetByCriteria, fecha.ToString(ConstantesBase.FormatoFecha), grupocodi);
            List<CoBandancpDTO> entitys = new List<CoBandancpDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(sqlQuery);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }

        public List<CoBandancpDTO> ListBandaNCPxFecha(DateTime fecha)
        {
            string sqlQuery = string.Format(helper.SqlGetListBandaNCPxFecha, fecha.ToString(ConstantesBase.FormatoFecha));
            List<CoBandancpDTO> entitys = new List<CoBandancpDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(sqlQuery);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    //entitys.Add(helper.Create(dr));
                    CoBandancpDTO entity = helper.Create(dr);
                    int iGruponomb = dr.GetOrdinal("GRUPONOMB");
                    if (!dr.IsDBNull(iGruponomb)) entity.Gruponomb = dr.GetString(iGruponomb);
                    entitys.Add(entity);
                }
            }

            return entitys;
        }
    }
}
