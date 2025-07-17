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
    /// Clase de acceso a datos de la tabla CO_FACTOR_UTILIZACION
    /// </summary>
    public class CoFactorUtilizacionRepository: RepositoryBase, ICoFactorUtilizacionRepository
    {
        public CoFactorUtilizacionRepository(string strConn): base(strConn)
        {
        }

        CoFactorUtilizacionHelper helper = new CoFactorUtilizacionHelper();

        public int Save(CoFactorUtilizacionDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Facuticodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Prodiacodi, DbType.Int32, entity.Prodiacodi);
            dbProvider.AddInParameter(command, helper.Facutibeta, DbType.Decimal, entity.Facutibeta);
            dbProvider.AddInParameter(command, helper.Facutialfa, DbType.Decimal, entity.Facutialfa);
            dbProvider.AddInParameter(command, helper.Facutiperiodo, DbType.Int32, entity.Facutiperiodo);
            dbProvider.AddInParameter(command, helper.Facutiusucreacion, DbType.String, entity.Facutiusucreacion);
            dbProvider.AddInParameter(command, helper.Facutifeccreacion, DbType.DateTime, entity.Facutifeccreacion);
            dbProvider.AddInParameter(command, helper.Facutiusumodificacion, DbType.String, entity.Facutiusumodificacion);
            dbProvider.AddInParameter(command, helper.Facutifecmodificacion, DbType.DateTime, entity.Facutifecmodificacion);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(CoFactorUtilizacionDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);
                       
            dbProvider.AddInParameter(command, helper.Prodiacodi, DbType.Int32, entity.Prodiacodi);
            dbProvider.AddInParameter(command, helper.Facutibeta, DbType.Decimal, entity.Facutibeta);
            dbProvider.AddInParameter(command, helper.Facutialfa, DbType.Decimal, entity.Facutialfa);
            dbProvider.AddInParameter(command, helper.Facutiperiodo, DbType.Int32, entity.Facutiperiodo);
            dbProvider.AddInParameter(command, helper.Facutiusucreacion, DbType.String, entity.Facutiusucreacion);
            dbProvider.AddInParameter(command, helper.Facutifeccreacion, DbType.DateTime, entity.Facutifeccreacion);
            dbProvider.AddInParameter(command, helper.Facutiusumodificacion, DbType.String, entity.Facutiusumodificacion);
            dbProvider.AddInParameter(command, helper.Facutifecmodificacion, DbType.DateTime, entity.Facutifecmodificacion);
            dbProvider.AddInParameter(command, helper.Facuticodi, DbType.Int32, entity.Facuticodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int facuticodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Facuticodi, DbType.Int32, facuticodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public CoFactorUtilizacionDTO GetById(int facuticodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Facuticodi, DbType.Int32, facuticodi);
            CoFactorUtilizacionDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<CoFactorUtilizacionDTO> List()
        {
            List<CoFactorUtilizacionDTO> entitys = new List<CoFactorUtilizacionDTO>();
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

        public List<CoFactorUtilizacionDTO> GetByCriteria(int prodiacodi)
        {
            List<CoFactorUtilizacionDTO> entitys = new List<CoFactorUtilizacionDTO>();
            string sql = string.Format(helper.SqlGetByCriteria, prodiacodi);
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

        public List<CoFactorUtilizacionDTO> ObtenerReporte(int idPeriodo, int idVersion, DateTime fechaInicio, DateTime fechaFin)
        {
            List<CoFactorUtilizacionDTO> entitys = new List<CoFactorUtilizacionDTO>();
            
            string sql = string.Format(helper.SqlObtenerReporte, idPeriodo, idVersion, 
                fechaInicio.ToString(ConstantesBase.FormatoFecha), fechaFin.ToString(ConstantesBase.FormatoFecha));
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    CoFactorUtilizacionDTO entity = helper.Create(dr);

                    int iProdiafecha = dr.GetOrdinal(helper.Prodiafecha);
                    if (!dr.IsDBNull(iProdiafecha)) entity.Prodiafecha = dr.GetDateTime(iProdiafecha);

                    int iPerprgvalor = dr.GetOrdinal(helper.Perprgvalor);
                    if (!dr.IsDBNull(iPerprgvalor)) entity.Perprgvalor = dr.GetDecimal(iPerprgvalor);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<CoFactorUtilizacionDTO> ObtenerReporteDiario(DateTime fechaInicio, DateTime fechaFin)
        {
            List<CoFactorUtilizacionDTO> entitys = new List<CoFactorUtilizacionDTO>();

            string sql = string.Format(helper.SqlObtenerReporteDiario,
                fechaInicio.ToString(ConstantesBase.FormatoFecha), fechaFin.ToString(ConstantesBase.FormatoFecha));
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    CoFactorUtilizacionDTO entity = helper.Create(dr);

                    int iProdiafecha = dr.GetOrdinal(helper.Prodiafecha);
                    if (!dr.IsDBNull(iProdiafecha)) entity.Prodiafecha = dr.GetDateTime(iProdiafecha);

                    int iPerprgvalor = dr.GetOrdinal(helper.Perprgvalor);
                    if (!dr.IsDBNull(iPerprgvalor)) entity.Perprgvalor = dr.GetDecimal(iPerprgvalor);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public void EliminarFactoresUtilizacion(string listaProdiacodis)
        {
            string query = string.Format(helper.SqlEliminarFactoresUtilizacion, listaProdiacodis);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            dbProvider.ExecuteNonQuery(command);

        }

        public List<CoFactorUtilizacionDTO> ObtenerReporteResultados(int prodiacodi)
        {
            List<CoFactorUtilizacionDTO> entitys = new List<CoFactorUtilizacionDTO>();

            string sql = string.Format(helper.SqlObtenerReporteResultados, prodiacodi);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    CoFactorUtilizacionDTO entity = helper.Create(dr);

                    int iProdiafecha = dr.GetOrdinal(helper.Prodiafecha);
                    if (!dr.IsDBNull(iProdiafecha)) entity.Prodiafecha = dr.GetDateTime(iProdiafecha);

                    int iPerprgvalor = dr.GetOrdinal(helper.Perprgvalor);
                    if (!dr.IsDBNull(iPerprgvalor)) entity.Perprgvalor = dr.GetDecimal(iPerprgvalor);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public CoFactorUtilizacionDTO GetByProdiacodiYPeriodo(int prodiacodi, int periodo)
        {
            string sql = string.Format(helper.SqlGetByProdiacodiYPeriodo, prodiacodi, periodo);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);
            CoFactorUtilizacionDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }
    }
}
