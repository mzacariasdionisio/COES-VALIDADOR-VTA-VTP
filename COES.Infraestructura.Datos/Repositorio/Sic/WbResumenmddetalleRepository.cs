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
    /// Clase de acceso a datos de la tabla WB_RESUMENMDDETALLE
    /// </summary>
    public class WbResumenmddetalleRepository : RepositoryBase, IWbResumenmddetalleRepository
    {
        public WbResumenmddetalleRepository(string strConn) : base(strConn)
        {
        }

        WbResumenmddetalleHelper helper = new WbResumenmddetalleHelper();

        public int Save(WbResumenmddetalleDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Resmddcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Resmdcodi, DbType.Int32, entity.Resmdcodi);
            dbProvider.AddInParameter(command, helper.Ptomedicodi, DbType.Int32, entity.Ptomedicodi);
            dbProvider.AddInParameter(command, helper.Fenergcodi, DbType.Int32, entity.Fenergcodi);
            dbProvider.AddInParameter(command, helper.Resmddfecha, DbType.DateTime, entity.Resmddfecha);
            dbProvider.AddInParameter(command, helper.Resmddvalor, DbType.Decimal, entity.Resmddvalor);
            dbProvider.AddInParameter(command, helper.Resmddmes, DbType.DateTime, entity.Resmddmes);
            dbProvider.AddInParameter(command, helper.Resmddtipogenerrer, DbType.String, entity.Resmddtipogenerrer);
            dbProvider.AddInParameter(command, helper.Resmddusumodificacion, DbType.String, entity.Resmddusumodificacion);
            dbProvider.AddInParameter(command, helper.Resmddfecmodificacion, DbType.DateTime, entity.Resmddfecmodificacion);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(WbResumenmddetalleDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Resmddcodi, DbType.Int32, entity.Resmddcodi);
            dbProvider.AddInParameter(command, helper.Resmdcodi, DbType.Int32, entity.Resmdcodi);
            dbProvider.AddInParameter(command, helper.Ptomedicodi, DbType.Int32, entity.Ptomedicodi);
            dbProvider.AddInParameter(command, helper.Fenergcodi, DbType.Int32, entity.Fenergcodi);
            dbProvider.AddInParameter(command, helper.Resmddfecha, DbType.DateTime, entity.Resmddfecha);
            dbProvider.AddInParameter(command, helper.Resmddvalor, DbType.Decimal, entity.Resmddvalor);
            dbProvider.AddInParameter(command, helper.Resmddmes, DbType.DateTime, entity.Resmddmes);
            dbProvider.AddInParameter(command, helper.Resmddtipogenerrer, DbType.String, entity.Resmddtipogenerrer);
            dbProvider.AddInParameter(command, helper.Resmddusumodificacion, DbType.String, entity.Resmddusumodificacion);
            dbProvider.AddInParameter(command, helper.Resmddfecmodificacion, DbType.DateTime, entity.Resmddfecmodificacion);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int resmddcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Resmddcodi, DbType.Int32, resmddcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void DeleteByMes(DateTime fechaPeriodo)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDeleteByMes);

            dbProvider.AddInParameter(command, helper.Resmddmes, DbType.DateTime, fechaPeriodo);

            dbProvider.ExecuteNonQuery(command);
        }

        public WbResumenmddetalleDTO GetById(int resmddcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Resmddcodi, DbType.Int32, resmddcodi);
            WbResumenmddetalleDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }
        public List<WbResumenmddetalleDTO> GetByIdMd(int resmdcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByIdMd);

            dbProvider.AddInParameter(command, helper.Resmdcodi, DbType.Int32, resmdcodi);
            List<WbResumenmddetalleDTO> entitys = new List<WbResumenmddetalleDTO>();

            if (dbProvider == null) throw new InvalidOperationException("dbProvider es null");
            if (command == null) throw new InvalidOperationException("command es null");
            if (helper == null) throw new InvalidOperationException("helper es null");

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    WbResumenmddetalleDTO entity = helper.Create(dr);

                    int iTgenercodi = dr.GetOrdinal(helper.Tgenercodi);
                    if (!dr.IsDBNull(iTgenercodi)) entity.Tgenerercodi = Convert.ToInt32(dr.GetValue(iTgenercodi));

                    int iTgenernomb = dr.GetOrdinal(helper.Tgenernomb);
                    if (!dr.IsDBNull(iTgenernomb)) entity.Tgenerernomb = dr.GetString(iTgenernomb);

                    int iTgenercolor = dr.GetOrdinal(helper.Tgenercolor);
                    if (!dr.IsDBNull(iTgenercolor)) entity.Tgenerercolor = dr.GetString(iTgenercolor);

                    int iEmprcodi = dr.GetOrdinal(helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

                    int iEmpnomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmpnomb)) entity.Emprnomb = dr.GetString(iEmpnomb);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<WbResumenmddetalleDTO> List()
        {
            List<WbResumenmddetalleDTO> entitys = new List<WbResumenmddetalleDTO>();
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

        public List<WbResumenmddetalleDTO> GetByCriteria()
        {
            List<WbResumenmddetalleDTO> entitys = new List<WbResumenmddetalleDTO>();
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
