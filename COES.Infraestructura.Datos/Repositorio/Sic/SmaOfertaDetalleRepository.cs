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
    /// Clase de acceso a datos de la tabla SMA_OFERTA_DETALLE
    /// </summary>
    public class SmaOfertaDetalleRepository : RepositoryBase, ISmaOfertaDetalleRepository
    {
        public SmaOfertaDetalleRepository(string strConn)
            : base(strConn)
        {
        }

        SmaOfertaDetalleHelper helper = new SmaOfertaDetalleHelper();

        public int GetMaxId()
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            int id = 1;

            object result = dbProvider.ExecuteScalar(command);
            if (result != null) id = Convert.ToInt32(result);

            return id;
        }
        public int Save(int ofercodi, SmaOfertaDetalleDTO entity, IDbConnection conn, DbTransaction tran, int corrOferdet)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            int id = 1;

            if (corrOferdet != 0)
            {
                id = corrOferdet + 1;
            }
            else
            {
                object result = dbProvider.ExecuteScalar(command);
                if (result != null) id = Convert.ToInt32(result);
            }

            DbCommand command2 = (DbCommand)conn.CreateCommand();

            command2.CommandText = helper.SqlSave; // = db.GetSqlStringCommand(helper.SqlSave);
            command2.Transaction = tran;
            command2.Connection = (DbConnection)conn;

            IDbDataParameter param = command2.CreateParameter();
            param.ParameterName = helper.Urscodi;
            param.Value = entity.Urscodi;
            command2.Parameters.Add(param);

            param = command2.CreateParameter();
            param.ParameterName = helper.Ofdehorainicio;
            param.Value = entity.Ofdehorainicio;
            command2.Parameters.Add(param);

            param = command2.CreateParameter();
            param.ParameterName = helper.Ofdehorafin;
            param.Value = entity.Ofdehorafin;
            command2.Parameters.Add(param);

            param = command2.CreateParameter();
            param.ParameterName = helper.Ofdeprecio;
            param.Value = entity.Ofdeprecio;
            command2.Parameters.Add(param);

            param = command2.CreateParameter();
            param.ParameterName = helper.Ofdepotofer;
            param.Value = entity.Ofdepotofertada;
            command2.Parameters.Add(param);

            param = command2.CreateParameter();
            param.ParameterName = helper.Ofdedusucreacion;
            param.Value = entity.Ofdedusucreacion;
            command2.Parameters.Add(param);

            param = command2.CreateParameter();
            param.ParameterName = helper.Ofdemoneda;
            param.Value = entity.Ofdemoneda;
            command2.Parameters.Add(param);


            param = command2.CreateParameter();
            param.ParameterName = helper.Ofdecodi;
            param.Value = id;
            command2.Parameters.Add(param);

            param = command2.CreateParameter();
            param.ParameterName = helper.Ofercodi;
            param.Value = ofercodi;
            command2.Parameters.Add(param);

            param = command2.CreateParameter();
            param.ParameterName = helper.Ofdetipo;
            param.Value = entity.Ofdetipo;
            command2.Parameters.Add(param);

            try
            {
                command2.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                return -1;
            }

            return id;
        }

        public void Update(SmaOfertaDetalleDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Urscodi, DbType.String, entity.Urscodi);
            dbProvider.AddInParameter(command, helper.Ofdehorainicio, DbType.DateTime, entity.Ofdehorainicio);
            dbProvider.AddInParameter(command, helper.Ofdehorafin, DbType.DateTime, entity.Ofdehorafin);
            dbProvider.AddInParameter(command, helper.Ofdeprecio, DbType.String, entity.Ofdeprecio);
            dbProvider.AddInParameter(command, helper.Ofdedusucreacion, DbType.String, entity.Ofdedusucreacion);
            dbProvider.AddInParameter(command, helper.Ofdefeccreacion, DbType.DateTime, entity.Ofdefeccreacion);
            dbProvider.AddInParameter(command, helper.Ofdemoneda, DbType.String, entity.Ofdemoneda);
            dbProvider.AddInParameter(command, helper.Ofdeusumodificacion, DbType.String, entity.Ofdeusumodificacion);
            dbProvider.AddInParameter(command, helper.Ofdefecmodificacion, DbType.DateTime, entity.Ofdefecmodificacion);
            dbProvider.AddInParameter(command, helper.Ofdecodi, DbType.Int32, entity.Ofdecodi);
            dbProvider.AddInParameter(command, helper.Ofercodi, DbType.Int32, entity.Ofercodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void UpdatePrecio(int ofdecodi, string precio, DateTime fechaActualizacion, string usuarioActualizacion)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdatePrecio);

            dbProvider.AddInParameter(command, helper.Ofdeprecio, DbType.String, precio);
            dbProvider.AddInParameter(command, helper.Ofdefecmodificacion, DbType.DateTime, fechaActualizacion);
            dbProvider.AddInParameter(command, helper.Ofdeusumodificacion, DbType.String, usuarioActualizacion);
            dbProvider.AddInParameter(command, helper.Ofdecodi, DbType.Int32, ofdecodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int ofdecodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Ofdecodi, DbType.Int32, ofdecodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public SmaOfertaDetalleDTO GetById(int ofdecodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Ofdecodi, DbType.Int32, ofdecodi);
            SmaOfertaDetalleDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<SmaOfertaDetalleDTO> List()
        {
            List<SmaOfertaDetalleDTO> entitys = new List<SmaOfertaDetalleDTO>();
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

        public int GetByCriteria(int urscodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByCriteria);
            int id = 1;
            dbProvider.AddInParameter(command, helper.Urscodi, DbType.Int32, urscodi);
            object result = dbProvider.ExecuteScalar(command);
            if (result != null) id = Convert.ToInt32(result);

            return id;

        
        }

        #region FIT - VALORIZACION DIARIA

        public List<SmaOfertaDetalleDTO> ListByDate(DateTime fechaOfertaIni, DateTime fechaOfertaFin, int tipoOferta, string estado)
        {
            List<SmaOfertaDetalleDTO> entities = new List<SmaOfertaDetalleDTO>();
            string sql = string.Format(helper.SqlListByDate, fechaOfertaIni.ToString(ConstantesBase.FormatoFecha), fechaOfertaFin.ToString(ConstantesBase.FormatoFecha), tipoOferta, estado);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    SmaOfertaDetalleDTO entity = new SmaOfertaDetalleDTO();

                    int iUrscodi = dr.GetOrdinal(helper.urscodivtdvalorizacion);
                    if (!dr.IsDBNull(iUrscodi)) entity.Urscodi = Convert.ToInt32(dr.GetValue(iUrscodi));

                    int iOfdehorainicio = dr.GetOrdinal(helper.Ofdehorainicio);
                    if (!dr.IsDBNull(iOfdehorainicio)) entity.Ofdehorainicio = dr.GetString(iOfdehorainicio);

                    int iOfdehorafin = dr.GetOrdinal(helper.Ofdehorafin);
                    if (!dr.IsDBNull(iOfdehorafin)) entity.Ofdehorafin = dr.GetString(iOfdehorafin);

                    int iOfdeprecio = dr.GetOrdinal(helper.Ofdeprecio);
                    if (!dr.IsDBNull(iOfdeprecio)) entity.Ofdeprecio = dr.GetString(iOfdeprecio);

                    int iOfdedusucreacion = dr.GetOrdinal(helper.Ofdedusucreacion);
                    if (!dr.IsDBNull(iOfdedusucreacion)) entity.Ofdedusucreacion = dr.GetString(iOfdedusucreacion);

                    int iOfdefeccreacion = dr.GetOrdinal(helper.Ofdefeccreacion);
                    if (!dr.IsDBNull(iOfdefeccreacion)) entity.Ofdefeccreacion = dr.GetDateTime(iOfdefeccreacion);

                    int iOfdemoneda = dr.GetOrdinal(helper.Ofdemoneda);
                    if (!dr.IsDBNull(iOfdemoneda)) entity.Ofdemoneda = dr.GetString(iOfdemoneda);

                    int iOfdeusumodificacion = dr.GetOrdinal(helper.Ofdeusumodificacion);
                    if (!dr.IsDBNull(iOfdeusumodificacion)) entity.Ofdeusumodificacion = dr.GetString(iOfdeusumodificacion);

                    int iOfdefecmodificacion = dr.GetOrdinal(helper.Ofdefecmodificacion);
                    if (!dr.IsDBNull(iOfdefecmodificacion)) entity.Ofdefecmodificacion = dr.GetDateTime(iOfdefecmodificacion);

                    int iOfdecodi = dr.GetOrdinal(helper.Ofdecodi);
                    if (!dr.IsDBNull(iOfdecodi)) entity.Ofdecodi = Convert.ToInt32(dr.GetValue(iOfdecodi));

                    int iOfercodi = dr.GetOrdinal(helper.Ofercodi);
                    if (!dr.IsDBNull(iOfercodi)) entity.Ofercodi = Convert.ToInt32(dr.GetValue(iOfercodi));

                    int iGruponomb = dr.GetOrdinal(helper.Gruponomb);
                    if (!dr.IsDBNull(iGruponomb)) entity.Gruponomb = dr.GetString(iGruponomb);

                    entities.Add(entity);
                }
            }

            return entities;
        }

        public List<SmaOfertaDetalleDTO> ListByDateTipo(DateTime fechaOfertaIni, DateTime fechaOfertaFin, int tipoOferta, string estado)
        {
            List<SmaOfertaDetalleDTO> entities = new List<SmaOfertaDetalleDTO>();
            string sql = string.Format(helper.SqlListByDateTipo, fechaOfertaIni.ToString(ConstantesBase.FormatoFecha), fechaOfertaFin.ToString(ConstantesBase.FormatoFecha), tipoOferta, estado);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    SmaOfertaDetalleDTO entity = new SmaOfertaDetalleDTO();

                    int iUrscodi = dr.GetOrdinal(helper.urscodivtdvalorizacion);
                    if (!dr.IsDBNull(iUrscodi)) entity.Urscodi = Convert.ToInt32(dr.GetValue(iUrscodi));

                    int iOfdehorainicio = dr.GetOrdinal(helper.Ofdehorainicio);
                    if (!dr.IsDBNull(iOfdehorainicio)) entity.Ofdehorainicio = dr.GetString(iOfdehorainicio);

                    int iOfdehorafin = dr.GetOrdinal(helper.Ofdehorafin);
                    if (!dr.IsDBNull(iOfdehorafin)) entity.Ofdehorafin = dr.GetString(iOfdehorafin);

                    int iOfdeprecio = dr.GetOrdinal(helper.Ofdeprecio);
                    if (!dr.IsDBNull(iOfdeprecio)) entity.Ofdeprecio = dr.GetString(iOfdeprecio);

                    int iOfdedusucreacion = dr.GetOrdinal(helper.Ofdedusucreacion);
                    if (!dr.IsDBNull(iOfdedusucreacion)) entity.Ofdedusucreacion = dr.GetString(iOfdedusucreacion);

                    int iOfdefeccreacion = dr.GetOrdinal(helper.Ofdefeccreacion);
                    if (!dr.IsDBNull(iOfdefeccreacion)) entity.Ofdefeccreacion = dr.GetDateTime(iOfdefeccreacion);

                    int iOfdemoneda = dr.GetOrdinal(helper.Ofdemoneda);
                    if (!dr.IsDBNull(iOfdemoneda)) entity.Ofdemoneda = dr.GetString(iOfdemoneda);

                    int iOfdeusumodificacion = dr.GetOrdinal(helper.Ofdeusumodificacion);
                    if (!dr.IsDBNull(iOfdeusumodificacion)) entity.Ofdeusumodificacion = dr.GetString(iOfdeusumodificacion);

                    int iOfdefecmodificacion = dr.GetOrdinal(helper.Ofdefecmodificacion);
                    if (!dr.IsDBNull(iOfdefecmodificacion)) entity.Ofdefecmodificacion = dr.GetDateTime(iOfdefecmodificacion);

                    int iOfdecodi = dr.GetOrdinal(helper.Ofdecodi);
                    if (!dr.IsDBNull(iOfdecodi)) entity.Ofdecodi = Convert.ToInt32(dr.GetValue(iOfdecodi));

                    int iOfercodi = dr.GetOrdinal(helper.Ofercodi);
                    if (!dr.IsDBNull(iOfercodi)) entity.Ofercodi = Convert.ToInt32(dr.GetValue(iOfercodi));                   

                    int iOfdeTipo = dr.GetOrdinal(helper.Ofdetipo);
                    if (!dr.IsDBNull(iOfdeTipo)) entity.Ofdetipo = Convert.ToInt32(dr.GetValue(iOfdeTipo));

                    int iGruponomb = dr.GetOrdinal(helper.Gruponomb);
                    if (!dr.IsDBNull(iGruponomb)) entity.Gruponomb = dr.GetString(iGruponomb);

                    int iOferfuente = dr.GetOrdinal(helper.Oferfuente);
                    if (!dr.IsDBNull(iOferfuente)) entity.Oferfuente = dr.GetString(iOferfuente);

                    int iOfdepotofer = dr.GetOrdinal(helper.Ofdepotofer);
                    if (!dr.IsDBNull(iOfdepotofer)) entity.Ofdepotofertada = dr.GetDecimal(iOfdepotofer);

                    entities.Add(entity);
                }
            }

            return entities;
        }

        public List<SmaOfertaDetalleDTO> ListarPorOfertas(string ofercodis)
        {
            List<SmaOfertaDetalleDTO> entitys = new List<SmaOfertaDetalleDTO>();
            string sql = string.Format(helper.SqlListarPorOfertas, ofercodis);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    SmaOfertaDetalleDTO entity = new SmaOfertaDetalleDTO();

                    int iUrscodi = dr.GetOrdinal(helper.urscodivtdvalorizacion);
                    if (!dr.IsDBNull(iUrscodi)) entity.Urscodi = Convert.ToInt32(dr.GetValue(iUrscodi));

                    int iOfdehorainicio = dr.GetOrdinal(helper.Ofdehorainicio);
                    if (!dr.IsDBNull(iOfdehorainicio)) entity.Ofdehorainicio = dr.GetString(iOfdehorainicio);

                    int iOfdehorafin = dr.GetOrdinal(helper.Ofdehorafin);
                    if (!dr.IsDBNull(iOfdehorafin)) entity.Ofdehorafin = dr.GetString(iOfdehorafin);

                    int iOfdeprecio = dr.GetOrdinal(helper.Ofdeprecio);
                    if (!dr.IsDBNull(iOfdeprecio)) entity.Ofdeprecio = dr.GetString(iOfdeprecio);

                    int iOfdedusucreacion = dr.GetOrdinal(helper.Ofdedusucreacion);
                    if (!dr.IsDBNull(iOfdedusucreacion)) entity.Ofdedusucreacion = dr.GetString(iOfdedusucreacion);

                    int iOfdefeccreacion = dr.GetOrdinal(helper.Ofdefeccreacion);
                    if (!dr.IsDBNull(iOfdefeccreacion)) entity.Ofdefeccreacion = dr.GetDateTime(iOfdefeccreacion);

                    int iOfdemoneda = dr.GetOrdinal(helper.Ofdemoneda);
                    if (!dr.IsDBNull(iOfdemoneda)) entity.Ofdemoneda = dr.GetString(iOfdemoneda);

                    int iOfdeusumodificacion = dr.GetOrdinal(helper.Ofdeusumodificacion);
                    if (!dr.IsDBNull(iOfdeusumodificacion)) entity.Ofdeusumodificacion = dr.GetString(iOfdeusumodificacion);

                    int iOfdefecmodificacion = dr.GetOrdinal(helper.Ofdefecmodificacion);
                    if (!dr.IsDBNull(iOfdefecmodificacion)) entity.Ofdefecmodificacion = dr.GetDateTime(iOfdefecmodificacion);

                    int iOfdecodi = dr.GetOrdinal(helper.Ofdecodi);
                    if (!dr.IsDBNull(iOfdecodi)) entity.Ofdecodi = Convert.ToInt32(dr.GetValue(iOfdecodi));

                    int iOfercodi = dr.GetOrdinal(helper.Ofercodi);
                    if (!dr.IsDBNull(iOfercodi)) entity.Ofercodi = Convert.ToInt32(dr.GetValue(iOfercodi));

                    int iOfdeTipo = dr.GetOrdinal(helper.Ofdetipo);
                    if (!dr.IsDBNull(iOfdeTipo)) entity.Ofdetipo = Convert.ToInt32(dr.GetValue(iOfdeTipo));

                    entitys.Add(entity);
                }
            }

            return entitys;
        }
        

        #endregion
    }
}
