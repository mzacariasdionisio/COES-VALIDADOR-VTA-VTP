using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using COES.Dominio.DTO.Scada;
using COES.Dominio.Interfaces.Scada;
using COES.Base.Core;
using COES.Infraestructura.Datos.Helper.Scada;
using System.Data.Odbc;

namespace COES.Infraestructura.Datos.Repositorio.Scada
{
    /// <summary>
    /// Clase de acceso a datos de la tabla TR_CANAL_SP7
    /// </summary>
    public class TrCanalSp7Repository : RepositoryBase, ITrCanalSp7Repository
    {
        public TrCanalSp7Repository(string strConn)
            : base(strConn)
        {
        }

        TrCanalSp7Helper helper = new TrCanalSp7Helper();

        public int Save(TrCanalSp7DTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Canalmseg, DbType.Int32, entity.Canalmseg);
            dbProvider.AddInParameter(command, helper.Canalcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Canalvalor, DbType.Decimal, entity.Canalvalor);
            dbProvider.AddInParameter(command, helper.Alarmcodi, DbType.Int32, entity.Alarmcodi);
            dbProvider.AddInParameter(command, helper.Canalcalidad, DbType.Int32, entity.Canalcalidad);
            dbProvider.AddInParameter(command, helper.Canalfhora, DbType.DateTime, entity.Canalfhora);
            dbProvider.AddInParameter(command, helper.Canalnomb, DbType.String, entity.Canalnomb);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);
            dbProvider.AddInParameter(command, helper.Canaliccp, DbType.String, entity.Canaliccp);
            dbProvider.AddInParameter(command, helper.Canaltdato, DbType.Int32, entity.Canaltdato);
            dbProvider.AddInParameter(command, helper.Canalunidad, DbType.String, entity.Canalunidad);
            dbProvider.AddInParameter(command, helper.Zonacodi, DbType.Int32, entity.Zonacodi);
            dbProvider.AddInParameter(command, helper.Canaltipo, DbType.String, entity.Canaltipo);
            dbProvider.AddInParameter(command, helper.Canalabrev, DbType.String, entity.Canalabrev);
            dbProvider.AddInParameter(command, helper.Canalfhora2, DbType.DateTime, entity.Canalfhora2);
            dbProvider.AddInParameter(command, helper.Canalcodscada, DbType.String, entity.Canalcodscada);
            dbProvider.AddInParameter(command, helper.Canalflags, DbType.Int32, entity.Canalflags);
            dbProvider.AddInParameter(command, helper.Canalcalidadforzada, DbType.Int32, entity.Canalcalidadforzada);
            dbProvider.AddInParameter(command, helper.Canalvalor2, DbType.Decimal, entity.Canalvalor2);
            dbProvider.AddInParameter(command, helper.Canalestado, DbType.String, entity.Canalestado);
            dbProvider.AddInParameter(command, helper.Canalfhestado, DbType.DateTime, entity.Canalfhestado);
            dbProvider.AddInParameter(command, helper.Alarmmin1, DbType.Decimal, entity.Alarmmin1);
            dbProvider.AddInParameter(command, helper.Alarmmax1, DbType.Decimal, entity.Alarmmax1);
            dbProvider.AddInParameter(command, helper.Alarmmin2, DbType.Decimal, entity.Alarmmin2);
            dbProvider.AddInParameter(command, helper.Alarmmax2, DbType.Decimal, entity.Alarmmax2);
            dbProvider.AddInParameter(command, helper.Lastuser, DbType.String, entity.Lastuser);
            dbProvider.AddInParameter(command, helper.Lastdate, DbType.DateTime, entity.Lastdate);
            dbProvider.AddInParameter(command, helper.Canaldescripcionestado, DbType.String, entity.Canaldescripcionestado);
            dbProvider.AddInParameter(command, helper.Canalprior, DbType.Int32, entity.Canalprior);
            dbProvider.AddInParameter(command, helper.Canaldec, DbType.Int32, entity.Canaldec);
            dbProvider.AddInParameter(command, helper.Canalntension, DbType.Decimal, entity.Canalntension);
            dbProvider.AddInParameter(command, helper.Canalinvert, DbType.String, entity.Canalinvert);
            dbProvider.AddInParameter(command, helper.Canaldispo, DbType.Int32, entity.Canaldispo);
            dbProvider.AddInParameter(command, helper.Canalcritico, DbType.String, entity.Canalcritico);
            dbProvider.AddInParameter(command, helper.Canaliccpreenvio, DbType.String, entity.Canaliccpreenvio);
            dbProvider.AddInParameter(command, helper.Canalcelda, DbType.String, entity.Canalcelda);
            dbProvider.AddInParameter(command, helper.Canaldescrip2, DbType.String, entity.Canaldescrip2);
            dbProvider.AddInParameter(command, helper.Rdfid, DbType.String, entity.Rdfid);
            dbProvider.AddInParameter(command, helper.Gisid, DbType.Int32, entity.Gisid);
            dbProvider.AddInParameter(command, helper.Pathb, DbType.String, entity.Pathb);
            dbProvider.AddInParameter(command, helper.PointType, DbType.String, entity.PointType);
            //dbProvider.AddInParameter(command, helper.Lastdatesp7, DbType.DateTime, entity.Lastdatesp7);
            dbProvider.AddInParameter(command, helper.Gpscodi, DbType.Int32, entity.Gpscodi);
            dbProvider.AddInParameter(command, helper.Canalfeccreacion, DbType.DateTime, entity.Canalfeccreacion);
            dbProvider.AddInParameter(command, helper.Canalusucreacion, DbType.String, entity.Canalusucreacion);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(TrCanalSp7DTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Canalmseg, DbType.Int32, entity.Canalmseg);
            dbProvider.AddInParameter(command, helper.Canalvalor, DbType.Decimal, entity.Canalvalor);
            dbProvider.AddInParameter(command, helper.Alarmcodi, DbType.Int32, entity.Alarmcodi);
            dbProvider.AddInParameter(command, helper.Canalcalidad, DbType.Int32, entity.Canalcalidad);
            dbProvider.AddInParameter(command, helper.Canalfhora, DbType.DateTime, entity.Canalfhora);
            dbProvider.AddInParameter(command, helper.Canalnomb, DbType.String, entity.Canalnomb);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);
            dbProvider.AddInParameter(command, helper.Canaliccp, DbType.String, entity.Canaliccp);
            dbProvider.AddInParameter(command, helper.Canaltdato, DbType.Int32, entity.Canaltdato);
            dbProvider.AddInParameter(command, helper.Canalunidad, DbType.String, entity.Canalunidad);
            dbProvider.AddInParameter(command, helper.Zonacodi, DbType.Int32, entity.Zonacodi);
            dbProvider.AddInParameter(command, helper.Canaltipo, DbType.String, entity.Canaltipo);
            dbProvider.AddInParameter(command, helper.Canalabrev, DbType.String, entity.Canalabrev);
            dbProvider.AddInParameter(command, helper.Canalfhora2, DbType.DateTime, entity.Canalfhora2);
            dbProvider.AddInParameter(command, helper.Canalcodscada, DbType.String, entity.Canalcodscada);
            dbProvider.AddInParameter(command, helper.Canalflags, DbType.Int32, entity.Canalflags);
            dbProvider.AddInParameter(command, helper.Canalcalidadforzada, DbType.Int32, entity.Canalcalidadforzada);
            dbProvider.AddInParameter(command, helper.Canalvalor2, DbType.Decimal, entity.Canalvalor2);
            dbProvider.AddInParameter(command, helper.Canalestado, DbType.String, entity.Canalestado);
            dbProvider.AddInParameter(command, helper.Canalfhestado, DbType.DateTime, entity.Canalfhestado);
            dbProvider.AddInParameter(command, helper.Alarmmin1, DbType.Decimal, entity.Alarmmin1);
            dbProvider.AddInParameter(command, helper.Alarmmax1, DbType.Decimal, entity.Alarmmax1);
            dbProvider.AddInParameter(command, helper.Alarmmin2, DbType.Decimal, entity.Alarmmin2);
            dbProvider.AddInParameter(command, helper.Alarmmax2, DbType.Decimal, entity.Alarmmax2);
            dbProvider.AddInParameter(command, helper.Lastuser, DbType.String, entity.Lastuser);
            dbProvider.AddInParameter(command, helper.Lastdate, DbType.DateTime, entity.Lastdate);
            dbProvider.AddInParameter(command, helper.Canaldescripcionestado, DbType.String, entity.Canaldescripcionestado);
            dbProvider.AddInParameter(command, helper.Canalprior, DbType.Int32, entity.Canalprior);
            dbProvider.AddInParameter(command, helper.Canaldec, DbType.Int32, entity.Canaldec);
            dbProvider.AddInParameter(command, helper.Canalntension, DbType.Decimal, entity.Canalntension);
            dbProvider.AddInParameter(command, helper.Canalinvert, DbType.String, entity.Canalinvert);
            dbProvider.AddInParameter(command, helper.Canaldispo, DbType.Int32, entity.Canaldispo);
            dbProvider.AddInParameter(command, helper.Canalcritico, DbType.String, entity.Canalcritico);
            dbProvider.AddInParameter(command, helper.Canaliccpreenvio, DbType.String, entity.Canaliccpreenvio);
            dbProvider.AddInParameter(command, helper.Canalcelda, DbType.String, entity.Canalcelda);
            dbProvider.AddInParameter(command, helper.Canaldescrip2, DbType.String, entity.Canaldescrip2);
            dbProvider.AddInParameter(command, helper.Rdfid, DbType.String, entity.Rdfid);
            dbProvider.AddInParameter(command, helper.Gisid, DbType.Int32, entity.Gisid);
            dbProvider.AddInParameter(command, helper.Pathb, DbType.String, entity.Pathb);
            dbProvider.AddInParameter(command, helper.PointType, DbType.String, entity.PointType);
            //dbProvider.AddInParameter(command, helper.Lastdatesp7, DbType.DateTime, entity.Lastdatesp7);
            dbProvider.AddInParameter(command, helper.Gpscodi, DbType.Int32, entity.Gpscodi);
            dbProvider.AddInParameter(command, helper.Canalcodi, DbType.Int32, entity.Canalcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int canalcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Canalcodi, DbType.Int32, canalcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public TrCanalSp7DTO GetById(int canalcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Canalcodi, DbType.Int32, canalcodi);
            TrCanalSp7DTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public TrCanalSp7DTO GetByIdBdTreal(int canalcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Canalcodi, DbType.Int32, canalcodi);
            TrCanalSp7DTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.CreateBdTreal(dr);
                }
            }

            return entity;
        }

        public List<TrCanalSp7DTO> List()
        {
            List<TrCanalSp7DTO> entitys = new List<TrCanalSp7DTO>();
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

        public List<TrCanalSp7DTO> GetByCriteria()
        {
            List<TrCanalSp7DTO> entitys = new List<TrCanalSp7DTO>();
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

        public List<TrCanalSp7DTO> GetByIds(string canalcodi)
        {
            string query = string.Format(helper.SqlGetByIds, canalcodi);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            //dbProvider.AddInParameter(command, helper.Canalcodi, DbType.String, canalcodi);
            List<TrCanalSp7DTO> entitys = new List<TrCanalSp7DTO>();

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }

        public List<TrCanalSp7DTO> GetByCanalnomb(string canalnomb)
        {
            string query = string.Format(helper.SqlGetByCanalnomb, canalnomb);
            DbCommand command = dbProvider.GetSqlStringCommand(query);
            //DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByCanalnomb);
            //dbProvider.AddInParameter(command, helper.Canalnomb, DbType.String, canalnomb);

            List<TrCanalSp7DTO> entitys = new List<TrCanalSp7DTO>();

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }

        public List<TrCanalSp7DTO> GetByZona(int zonacodi)
        {
            string query = string.Format(helper.SqlGetByZona, zonacodi);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            List<TrCanalSp7DTO> entitys = new List<TrCanalSp7DTO>();

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    //entitys.Add(helper.Create(dr));

                    TrCanalSp7DTO entity = new TrCanalSp7DTO();

                    int iCanalcodi = dr.GetOrdinal(helper.Canalcodi);
                    if (!dr.IsDBNull(iCanalcodi)) entity.Canalcodi = Convert.ToInt32(dr.GetValue(iCanalcodi));

                    int iCanalnomb = dr.GetOrdinal(helper.Canalnomb);
                    if (!dr.IsDBNull(iCanalnomb)) entity.Canalnomb = dr.GetValue(iCanalnomb).ToString();

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<TrCanalSp7DTO> GetByZonaAnalogico(int zonacodi)
        {
            string query = string.Format(helper.SqlGetByZonaAnalogico, zonacodi);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            List<TrCanalSp7DTO> entitys = new List<TrCanalSp7DTO>();

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    TrCanalSp7DTO entity = new TrCanalSp7DTO();

                    int iCanalcodi = dr.GetOrdinal(helper.Canalcodi);
                    if (!dr.IsDBNull(iCanalcodi)) entity.Canalcodi = Convert.ToInt32(dr.GetValue(iCanalcodi));

                    int iCanalnomb = dr.GetOrdinal(helper.Canalnomb);
                    if (!dr.IsDBNull(iCanalnomb)) entity.Canalnomb = dr.GetValue(iCanalnomb).ToString();

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<TrCanalSp7DTO> GetByFiltro(int filtrocodi)
        {
            string query = string.Format(helper.SqlGetByFiltro, filtrocodi);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            List<TrCanalSp7DTO> entitys = new List<TrCanalSp7DTO>();

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    TrCanalSp7DTO entity = new TrCanalSp7DTO();

                    int iCanalcodi = dr.GetOrdinal(helper.Canalcodi);
                    if (!dr.IsDBNull(iCanalcodi)) entity.Canalcodi = Convert.ToInt32(dr.GetValue(iCanalcodi));

                    int iCanalnomb = dr.GetOrdinal(helper.Canalnomb);
                    if (!dr.IsDBNull(iCanalnomb)) entity.Canalnomb = dr.GetValue(iCanalnomb).ToString();

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<TrCanalSp7DTO> ListByZonaAndUnidad(string tipoPunto, int emprcodi, int zonacodi, string unidad)
        {
            List<TrCanalSp7DTO> entitys = new List<TrCanalSp7DTO>();

            if (unidad == null || unidad.Trim() == "")
                unidad = "EMPTY";

            string sql = string.Format(helper.SqlListByZonaAndUnidad, tipoPunto, zonacodi, unidad, emprcodi);

            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    TrCanalSp7DTO entity = new TrCanalSp7DTO();

                    int iCanalcodi = dr.GetOrdinal(this.helper.Canalcodi);
                    if (!dr.IsDBNull(iCanalcodi)) entity.Canalcodi = Convert.ToInt32(dr.GetValue(iCanalcodi));

                    int iCanalnomb = dr.GetOrdinal(this.helper.Canalnomb);
                    if (!dr.IsDBNull(iCanalnomb)) entity.Canalnomb = dr.GetString(iCanalnomb);

                    int iCanaliccp = dr.GetOrdinal(this.helper.Canaliccp);
                    if (!dr.IsDBNull(iCanaliccp)) entity.Canaliccp = dr.GetString(iCanaliccp);

                    int iCanalunidad = dr.GetOrdinal(this.helper.Canalunidad);
                    if (!dr.IsDBNull(iCanalunidad)) entity.Canalunidad = dr.GetString(iCanalunidad);

                    int iEmprcodi = dr.GetOrdinal(this.helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

                    int iZonacodi = dr.GetOrdinal(this.helper.Zonacodi);
                    if (!dr.IsDBNull(iZonacodi)) entity.Zonacodi = Convert.ToInt32(dr.GetValue(iZonacodi));

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        #region Mejoras IEOD

        public List<TrCanalSp7DTO> ListarUnidadPorZona(int zonacodi)
        {
            List<TrCanalSp7DTO> entitys = new List<TrCanalSp7DTO>();
            string sql = string.Format(helper.SqlListarUnidadPorZona,zonacodi);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);
            TrCanalSp7DTO entity;
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = new TrCanalSp7DTO();
                    int iCanalunidad = dr.GetOrdinal(helper.Canalunidad);
                    if (!dr.IsDBNull(iCanalunidad)) entity.Canalunidad = dr.GetString(iCanalunidad);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<TrCanalSp7DTO> GetByCriteriaBdTreal(string canalcodis)
        {
            List<TrCanalSp7DTO> entitys = new List<TrCanalSp7DTO>();
            string sql = string.Format(helper.SqlGetByCriteriaBdTreal, canalcodis);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    TrCanalSp7DTO entity = new TrCanalSp7DTO();

                    int iCanalcodi = dr.GetOrdinal(this.helper.Canalcodi);
                    if (!dr.IsDBNull(iCanalcodi)) entity.Canalcodi = Convert.ToInt32(dr.GetValue(iCanalcodi));

                    int iCanalnomb = dr.GetOrdinal(this.helper.Canalnomb);
                    if (!dr.IsDBNull(iCanalnomb)) entity.Canalnomb = dr.GetValue(iCanalnomb).ToString();

                    int iCanaliccp = dr.GetOrdinal(this.helper.Canaliccp);
                    if (!dr.IsDBNull(iCanaliccp)) entity.Canaliccp = dr.GetString(iCanaliccp);

                    int iCanalunidad = dr.GetOrdinal(this.helper.Canalunidad);
                    if (!dr.IsDBNull(iCanalunidad)) entity.Canalunidad = dr.GetString(iCanalunidad);

                    int iPointType = dr.GetOrdinal(this.helper.CanalPointType);
                    if (!dr.IsDBNull(iPointType)) entity.CanalPointType = dr.GetString(iPointType);

                    int iCanalabrev = dr.GetOrdinal(this.helper.Canalabrev);
                    if (!dr.IsDBNull(iCanalabrev)) entity.Canalabrev = dr.GetString(iCanalabrev);

                    int iZonacodi = dr.GetOrdinal(this.helper.Zonacodi);
                    if (!dr.IsDBNull(iZonacodi)) entity.Zonacodi = Convert.ToInt32(dr.GetValue(iZonacodi));

                    int iZonanomb = dr.GetOrdinal(this.helper.Zonanomb);
                    if (!dr.IsDBNull(iZonanomb)) entity.Zonanomb = dr.GetString(iZonanomb);

                    int iZonaabrev = dr.GetOrdinal(this.helper.Zonaabrev);
                    if (!dr.IsDBNull(iZonaabrev)) entity.Zonaabrev = dr.GetString(iZonaabrev);

                    int TrEmprcodi = dr.GetOrdinal(this.helper.TrEmprcodi);
                    if (!dr.IsDBNull(TrEmprcodi)) entity.TrEmprcodi = Convert.ToInt32(dr.GetValue(TrEmprcodi));

                    int iTrEmprnomb = dr.GetOrdinal(this.helper.TrEmprnomb);
                    if (!dr.IsDBNull(iTrEmprnomb)) entity.TrEmprnomb = dr.GetString(iTrEmprnomb);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        #endregion

        public List<TrCanalSp7DTO> GetDatosSP7(string query)
        {
            List<TrCanalSp7DTO> entitys = new List<TrCanalSp7DTO>();
            string sql = string.Format(helper.SqlListarDatosSP7, query);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {                   
                    TrCanalSp7DTO entity = new TrCanalSp7DTO();

                    int iCanalcodi = dr.GetOrdinal(helper.Canalcodi);
                    if (!dr.IsDBNull(iCanalcodi)) entity.Canalcodi = Convert.ToInt32(dr.GetValue(iCanalcodi));

                    int iCanalfhora2 = dr.GetOrdinal(helper.Canalfhora2);
                    if (!dr.IsDBNull(iCanalfhora2)) entity.Canalfhora2 = dr.GetDateTime(iCanalfhora2);

                    int iCanalvalor = dr.GetOrdinal(helper.Canalvalor);
                    if (!dr.IsDBNull(iCanalvalor)) entity.Canalvalor = dr.GetDecimal(iCanalvalor);

                    int iCanalcalidad = dr.GetOrdinal(helper.Canalcalidad);
                    if (!dr.IsDBNull(iCanalcalidad)) entity.Canalcalidad = Convert.ToInt32(dr.GetValue(iCanalcalidad));

                    int iCanalfhora = dr.GetOrdinal(helper.Canalfhora);
                    if (!dr.IsDBNull(iCanalfhora)) entity.Canalfhora = dr.GetDateTime(iCanalfhora);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }
    }
}
