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
    /// Clase de acceso a datos de la tabla DAI_CALENDARIOPAGO
    /// </summary>
    public class DaiCalendariopagoRepository: RepositoryBase, IDaiCalendariopagoRepository
    {
        public DaiCalendariopagoRepository(string strConn): base(strConn)
        {
        }

        DaiCalendariopagoHelper helper = new DaiCalendariopagoHelper();

        public int Save(DaiCalendariopagoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Calecodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Aporcodi, DbType.Int32, entity.Aporcodi);
            dbProvider.AddInParameter(command, helper.Caleanio, DbType.String, entity.Caleanio);
            dbProvider.AddInParameter(command, helper.Calenroamortizacion, DbType.Int32, entity.Calenroamortizacion);
            dbProvider.AddInParameter(command, helper.Calecapital, DbType.Decimal, entity.Calecapital);
            dbProvider.AddInParameter(command, helper.Caleinteres, DbType.Decimal, entity.Caleinteres);
            dbProvider.AddInParameter(command, helper.Caleamortizacion, DbType.Decimal, entity.Caleamortizacion);
            dbProvider.AddInParameter(command, helper.Caletotal, DbType.Decimal, entity.Caletotal);
            dbProvider.AddInParameter(command, helper.Calecartapago, DbType.String, entity.Calecartapago);
            dbProvider.AddInParameter(command, helper.Caleactivo, DbType.String, entity.Caleactivo);
            dbProvider.AddInParameter(command, helper.Caleusucreacion, DbType.String, entity.Caleusucreacion);
            dbProvider.AddInParameter(command, helper.Calefeccreacion, DbType.DateTime, entity.Calefeccreacion);
            dbProvider.AddInParameter(command, helper.Caleusumodificacion, DbType.String, entity.Caleusumodificacion);
            dbProvider.AddInParameter(command, helper.Calefecmodificacion, DbType.DateTime, entity.Calefecmodificacion);
            dbProvider.AddInParameter(command, helper.Tabcdcodiestado, DbType.Int32, entity.Tabcdcodiestado);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(DaiCalendariopagoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Aporcodi, DbType.Int32, entity.Aporcodi);
            dbProvider.AddInParameter(command, helper.Calecartapago, DbType.String, entity.Calecartapago);
            dbProvider.AddInParameter(command, helper.Caleactivo, DbType.String, entity.Caleactivo);
            dbProvider.AddInParameter(command, helper.Caleusumodificacion, DbType.String, entity.Caleusumodificacion);
            dbProvider.AddInParameter(command, helper.Calefecmodificacion, DbType.DateTime, entity.Calefecmodificacion);
            dbProvider.AddInParameter(command, helper.Tabcdcodiestado, DbType.Int32, entity.Tabcdcodiestado);
            dbProvider.AddInParameter(command, helper.Calecapital, DbType.Decimal, entity.Calecapital);
            dbProvider.AddInParameter(command, helper.Caleinteres, DbType.Decimal, entity.Caleinteres);
            dbProvider.AddInParameter(command, helper.Caleamortizacion, DbType.Decimal, entity.Caleamortizacion);
            dbProvider.AddInParameter(command, helper.Caletotal, DbType.Decimal, entity.Caletotal);
            dbProvider.AddInParameter(command, helper.Calechequeamortpago, DbType.String, entity.Calechequeamortpago);
            dbProvider.AddInParameter(command, helper.Calechequeintpago, DbType.String, entity.Calechequeintpago);

            dbProvider.AddInParameter(command, helper.Calecodi, DbType.Int32, entity.Calecodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int calecodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Calecodi, DbType.Int32, calecodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Reprocesar(DaiCalendariopagoDTO calendario)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlReprocesar);

            dbProvider.AddInParameter(command, helper.Caleusumodificacion, DbType.String, calendario.Caleusumodificacion);
            dbProvider.AddInParameter(command, helper.Calefecmodificacion, DbType.DateTime, calendario.Calefecmodificacion);
            dbProvider.AddInParameter(command, helper.Calecodi, DbType.Int32, calendario.Aporcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Liquidar(DaiCalendariopagoDTO calendario)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlLiquidar);

            dbProvider.AddInParameter(command, helper.Caleusumodificacion, DbType.String, calendario.Caleusumodificacion);
            dbProvider.AddInParameter(command, helper.Calefecmodificacion, DbType.DateTime, calendario.Calefecmodificacion);
            dbProvider.AddInParameter(command, helper.Calecodi, DbType.Int32, calendario.Calecodi);
            dbProvider.AddInParameter(command, helper.Aporcodi, DbType.Int32, calendario.Aporcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public DaiCalendariopagoDTO GetById(int calecodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Calecodi, DbType.Int32, calecodi);
            DaiCalendariopagoDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<DaiCalendariopagoDTO> List(int aporcodi, int estado)
        {
            string query = string.Format(helper.SqlList, estado, aporcodi);

            List<DaiCalendariopagoDTO> entitys = new List<DaiCalendariopagoDTO>();
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

        public List<DaiCalendariopagoDTO> GetByCriteria(int aporcodi)
        {
            List<DaiCalendariopagoDTO> entitys = new List<DaiCalendariopagoDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByCriteria);
            dbProvider.AddInParameter(command, helper.Aporcodi, DbType.Int32, aporcodi);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    DaiCalendariopagoDTO entity = helper.Create(dr);

                    int iTabddescripcion = dr.GetOrdinal(helper.Tabddescripcion);
                    if (!dr.IsDBNull(iTabddescripcion)) entity.Tabddescripcion = dr.GetString(iTabddescripcion);

                    int iCalechequeamortpago = dr.GetOrdinal(helper.Calechequeamortpago);
                    if (!dr.IsDBNull(iCalechequeamortpago)) entity.Calechequeamortpago = dr.GetString(iCalechequeamortpago);

                    int iCalechequeintpago = dr.GetOrdinal(helper.Calechequeintpago);
                    if (!dr.IsDBNull(iCalechequeintpago)) entity.Calechequeintpago = dr.GetString(iCalechequeintpago);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<DaiCalendariopagoDTO> GetByCriteriaAnio(int emprcodi)
        {
            List<DaiCalendariopagoDTO> entitys = new List<DaiCalendariopagoDTO>();

            string sql = string.Format(helper.SqlGetByCriteriaByAnio, emprcodi);

            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    DaiCalendariopagoDTO entity = helper.Create(dr);

                    int iTabddescripcion = dr.GetOrdinal(helper.Tabddescripcion);
                    if (!dr.IsDBNull(iTabddescripcion)) entity.Tabddescripcion = dr.GetString(iTabddescripcion);

                    int iPresanio = dr.GetOrdinal(helper.Presanio);
                    if (!dr.IsDBNull(iPresanio)) entity.Presanio = dr.GetString(iPresanio);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }
        
    }
}
