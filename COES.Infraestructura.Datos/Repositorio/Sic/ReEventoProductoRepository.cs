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
    /// Clase de acceso a datos de la tabla RE_EVENTO_PRODUCTO
    /// </summary>
    public class ReEventoProductoRepository : RepositoryBase, IReEventoProductoRepository
    {
        public ReEventoProductoRepository(string strConn) : base(strConn)
        {
        }

        ReEventoProductoHelper helper = new ReEventoProductoHelper();

        public int Save(ReEventoProductoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Reevprcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Reevpranio, DbType.Int32, entity.Reevpranio);
            dbProvider.AddInParameter(command, helper.Reevprmes, DbType.Int32, entity.Reevprmes);
            dbProvider.AddInParameter(command, helper.Reevprfecinicio, DbType.DateTime, entity.Reevprfecinicio);
            dbProvider.AddInParameter(command, helper.Reevprfecfin, DbType.DateTime, entity.Reevprfecfin);
            dbProvider.AddInParameter(command, helper.Reevprptoentrega, DbType.String, entity.Reevprptoentrega);
            dbProvider.AddInParameter(command, helper.Reevprtension, DbType.Decimal, entity.Reevprtension);
            dbProvider.AddInParameter(command, helper.Reevprempr1, DbType.Int32, entity.Reevprempr1);
            dbProvider.AddInParameter(command, helper.Reevprempr2, DbType.Int32, entity.Reevprempr2);
            dbProvider.AddInParameter(command, helper.Reevprempr3, DbType.Int32, entity.Reevprempr3);
            dbProvider.AddInParameter(command, helper.Reevprporc1, DbType.Decimal, entity.Reevprporc1);
            dbProvider.AddInParameter(command, helper.Reevprporc2, DbType.Decimal, entity.Reevprporc2);
            dbProvider.AddInParameter(command, helper.Reevprporc3, DbType.Decimal, entity.Reevprporc3);
            dbProvider.AddInParameter(command, helper.Reevprcomentario, DbType.String, entity.Reevprcomentario);
            dbProvider.AddInParameter(command, helper.Reevpracceso, DbType.String, entity.Reevpracceso);
            dbProvider.AddInParameter(command, helper.Reevprestado, DbType.String, entity.Reevprestado);
            dbProvider.AddInParameter(command, helper.Reevprusucreacion, DbType.String, entity.Reevprusucreacion);
            dbProvider.AddInParameter(command, helper.Reevprfeccreacion, DbType.DateTime, entity.Reevprfeccreacion);
            dbProvider.AddInParameter(command, helper.Reevprusumodificacion, DbType.String, entity.Reevprusumodificacion);
            dbProvider.AddInParameter(command, helper.Reevprfecmodificacion, DbType.DateTime, entity.Reevprfecmodificacion);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(ReEventoProductoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Reevpranio, DbType.Int32, entity.Reevpranio);
            dbProvider.AddInParameter(command, helper.Reevprmes, DbType.Int32, entity.Reevprmes);
            dbProvider.AddInParameter(command, helper.Reevprfecinicio, DbType.DateTime, entity.Reevprfecinicio);
            dbProvider.AddInParameter(command, helper.Reevprfecfin, DbType.DateTime, entity.Reevprfecfin);
            dbProvider.AddInParameter(command, helper.Reevprptoentrega, DbType.String, entity.Reevprptoentrega);
            dbProvider.AddInParameter(command, helper.Reevprtension, DbType.Decimal, entity.Reevprtension);
            dbProvider.AddInParameter(command, helper.Reevprempr1, DbType.Int32, entity.Reevprempr1);
            dbProvider.AddInParameter(command, helper.Reevprempr2, DbType.Int32, entity.Reevprempr2);
            dbProvider.AddInParameter(command, helper.Reevprempr3, DbType.Int32, entity.Reevprempr3);
            dbProvider.AddInParameter(command, helper.Reevprporc1, DbType.Decimal, entity.Reevprporc1);
            dbProvider.AddInParameter(command, helper.Reevprporc2, DbType.Decimal, entity.Reevprporc2);
            dbProvider.AddInParameter(command, helper.Reevprporc3, DbType.Decimal, entity.Reevprporc3);
            dbProvider.AddInParameter(command, helper.Reevprcomentario, DbType.String, entity.Reevprcomentario);
            dbProvider.AddInParameter(command, helper.Reevpracceso, DbType.String, entity.Reevpracceso);
            dbProvider.AddInParameter(command, helper.Reevprestado, DbType.String, entity.Reevprestado);
            dbProvider.AddInParameter(command, helper.Reevprusucreacion, DbType.String, entity.Reevprusucreacion);
            dbProvider.AddInParameter(command, helper.Reevprfeccreacion, DbType.DateTime, entity.Reevprfeccreacion);
            dbProvider.AddInParameter(command, helper.Reevprusumodificacion, DbType.String, entity.Reevprusumodificacion);
            dbProvider.AddInParameter(command, helper.Reevprfecmodificacion, DbType.DateTime, entity.Reevprfecmodificacion);
            dbProvider.AddInParameter(command, helper.Reevprcodi, DbType.Int32, entity.Reevprcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int reevprcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Reevprcodi, DbType.Int32, reevprcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public ReEventoProductoDTO GetById(int reevprcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Reevprcodi, DbType.Int32, reevprcodi);
            ReEventoProductoDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);

                    int iResponsablenomb1 = dr.GetOrdinal(helper.Responsablenomb1);
                    if (!dr.IsDBNull(iResponsablenomb1)) entity.Responsablenomb1 = dr.GetString(iResponsablenomb1);

                    int iResponsablenomb2 = dr.GetOrdinal(helper.Responsablenomb2);
                    if (!dr.IsDBNull(iResponsablenomb2)) entity.Responsablenomb2 = dr.GetString(iResponsablenomb2);

                    int iResponsablenomb3 = dr.GetOrdinal(helper.Responsablenomb3);
                    if (!dr.IsDBNull(iResponsablenomb3)) entity.Responsablenomb3 = dr.GetString(iResponsablenomb3);
                }
            }

            return entity;
        }

        public List<ReEventoProductoDTO> List()
        {
            List<ReEventoProductoDTO> entitys = new List<ReEventoProductoDTO>();
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

        public List<ReEventoProductoDTO> GetByCriteria(int anio, int mes)
        {
            List<ReEventoProductoDTO> entitys = new List<ReEventoProductoDTO>();
            string sql = string.Format(helper.SqlGetByCriteria, anio, mes);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    ReEventoProductoDTO entity = helper.Create(dr);

                    int iResponsablenomb1 = dr.GetOrdinal(helper.Responsablenomb1);
                    if (!dr.IsDBNull(iResponsablenomb1)) entity.Responsablenomb1 = dr.GetString(iResponsablenomb1);

                    int iResponsablenomb2 = dr.GetOrdinal(helper.Responsablenomb2);
                    if (!dr.IsDBNull(iResponsablenomb2)) entity.Responsablenomb2 = dr.GetString(iResponsablenomb2);

                    int iResponsablenomb3 = dr.GetOrdinal(helper.Responsablenomb3);
                    if (!dr.IsDBNull(iResponsablenomb3)) entity.Responsablenomb3 = dr.GetString(iResponsablenomb3);

                    int iSuministrador = dr.GetOrdinal(helper.Suministrador);
                    if (!dr.IsDBNull(iSuministrador)) entity.Suministrador = dr.GetString(iSuministrador);

                    int iEstadocarga = dr.GetOrdinal(helper.Estadocarga);
                    if (!dr.IsDBNull(iEstadocarga)) entity.Estadocarga = dr.GetString(iEstadocarga);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<ReEventoProductoDTO> ObtenerEventosPorSuministrador(int empresa, int anio, int mes, string buscar)
        {
            List<ReEventoProductoDTO> entitys = new List<ReEventoProductoDTO>();
            string sql = string.Format(helper.SqlObtenerEventosPorSuministrador, empresa, anio, mes, buscar);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    ReEventoProductoDTO entity = helper.Create(dr);

                    int iEstadocarga = dr.GetOrdinal(helper.Estadocarga);
                    if (!dr.IsDBNull(iEstadocarga)) entity.Estadocarga = dr.GetString(iEstadocarga);

                    entity.Nombremes = COES.Base.Tools.Util.ObtenerNombreMes((int)entity.Reevprmes);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }
    }
}

