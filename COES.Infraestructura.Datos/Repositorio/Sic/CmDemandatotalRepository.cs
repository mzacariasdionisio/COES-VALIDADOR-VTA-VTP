using COES.Base.Core;
using COES.Dominio.DTO.Sic;
using COES.Dominio.Interfaces.Sic;
using COES.Infraestructura.Datos.Helper.Sic;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace COES.Infraestructura.Datos.Repositorio.Sic
{
    /// <summary>
    /// Clase de acceso a datos de la tabla CM_DEMANDATOTAL
    /// </summary>
    public class CmDemandatotalRepository : RepositoryBase, ICmDemandatotalRepository
    {
        public CmDemandatotalRepository(string strConn) : base(strConn)
        {
        }

        CmDemandatotalHelper helper = new CmDemandatotalHelper();

        public int Save(CmDemandatotalDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Demacodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Demafecha, DbType.DateTime, entity.Demafecha);
            dbProvider.AddInParameter(command, helper.Demaintervalo, DbType.Int32, entity.Demaintervalo);
            dbProvider.AddInParameter(command, helper.Dematermica, DbType.Decimal, entity.Dematermica);
            dbProvider.AddInParameter(command, helper.Demahidraulica, DbType.Decimal, entity.Demahidraulica);
            dbProvider.AddInParameter(command, helper.Dematotal, DbType.Decimal, entity.Dematotal);
            dbProvider.AddInParameter(command, helper.Demasucreacion, DbType.String, entity.Demasucreacion);
            dbProvider.AddInParameter(command, helper.Demafeccreacion, DbType.DateTime, entity.Demafeccreacion);
            dbProvider.AddInParameter(command, helper.Demausumodificacion, DbType.String, entity.Demausumodificacion);
            dbProvider.AddInParameter(command, helper.Demafecmodificacion, DbType.DateTime, entity.Demafecmodificacion);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(CmDemandatotalDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Demacodi, DbType.Int32, entity.Demacodi);
            dbProvider.AddInParameter(command, helper.Demafecha, DbType.DateTime, entity.Demafecha);
            dbProvider.AddInParameter(command, helper.Demaintervalo, DbType.Int32, entity.Demaintervalo);
            dbProvider.AddInParameter(command, helper.Dematermica, DbType.Decimal, entity.Dematermica);
            dbProvider.AddInParameter(command, helper.Demahidraulica, DbType.Decimal, entity.Demahidraulica);
            dbProvider.AddInParameter(command, helper.Dematotal, DbType.Decimal, entity.Dematotal);
            dbProvider.AddInParameter(command, helper.Demasucreacion, DbType.String, entity.Demasucreacion);
            dbProvider.AddInParameter(command, helper.Demafeccreacion, DbType.DateTime, entity.Demafeccreacion);
            dbProvider.AddInParameter(command, helper.Demausumodificacion, DbType.String, entity.Demausumodificacion);
            dbProvider.AddInParameter(command, helper.Demafecmodificacion, DbType.String, entity.Demafecmodificacion);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int demacodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Demacodi, DbType.Int32, demacodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public CmDemandatotalDTO GetById(int demacodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Demacodi, DbType.Int32, demacodi);
            CmDemandatotalDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<CmDemandatotalDTO> List()
        {
            List<CmDemandatotalDTO> entitys = new List<CmDemandatotalDTO>();
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

        public List<CmDemandatotalDTO> GetByCriteria()
        {
            List<CmDemandatotalDTO> entitys = new List<CmDemandatotalDTO>();
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

        public void DeleteByCriteria(int intervalo, DateTime fecha)
        {
            string sql = string.Format(helper.SqlDeleteByCriteria, fecha.ToString(ConstantesBase.FormatoFecha), intervalo);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);            

            dbProvider.ExecuteNonQuery(command);
        }

        public CmDemandatotalDTO GetByDate(DateTime demafecha)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByDate);

            dbProvider.AddInParameter(command, helper.Demafecha, DbType.DateTime, demafecha);
            CmDemandatotalDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public CmDemandatotalDTO GetDemandaTotal(DateTime demafecha)
        {
            string query = string.Format(helper.SqlGetDemandaTotal, 
                                        demafecha.ToString(ConstantesBase.FormatoFecha), 
                                        demafecha.AddDays(1).ToString(ConstantesBase.FormatoFecha));

            DbCommand command = dbProvider.GetSqlStringCommand(query);
            CmDemandatotalDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = new CmDemandatotalDTO();

                    int iDematermica = dr.GetOrdinal(helper.Dematermica);
                    if (!dr.IsDBNull(iDematermica)) entity.Dematermica = dr.GetDecimal(iDematermica);

                    int iDemahidraulica = dr.GetOrdinal(helper.Demahidraulica);
                    if (!dr.IsDBNull(iDemahidraulica)) entity.Demahidraulica = dr.GetDecimal(iDemahidraulica);

                    int iDematotal = dr.GetOrdinal(helper.Dematotal);
                    if (!dr.IsDBNull(iDematotal)) entity.Dematotal = dr.GetDecimal(iDematotal);
                }
            }

            return entity;
        }
    }
}
