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
    /// Clase de acceso a datos de la tabla CM_CONGESTION_CALCULO
    /// </summary>
    public class CmCongestionCalculoRepository: RepositoryBase, ICmCongestionCalculoRepository
    {
        public CmCongestionCalculoRepository(string strConn): base(strConn)
        {
        }

        CmCongestionCalculoHelper helper = new CmCongestionCalculoHelper();

        public int Save(CmCongestionCalculoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Cmcongcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Configcodi, DbType.Int32, entity.Configcodi);
            dbProvider.AddInParameter(command, helper.Grulincodi, DbType.Int32, entity.Grulincodi);
            dbProvider.AddInParameter(command, helper.Regsegcodi, DbType.Int32, entity.Regsegcodi);
            dbProvider.AddInParameter(command, helper.Cmconfecha, DbType.DateTime, entity.Cmconfecha);
            dbProvider.AddInParameter(command, helper.Cmcongperiodo, DbType.Int32, entity.Cmcongperiodo);
            dbProvider.AddInParameter(command, helper.Cmgncorrelativo, DbType.Int32, entity.Cmgncorrelativo);
            dbProvider.AddInParameter(command, helper.Cmconglimite, DbType.Decimal, entity.Cmconglimite);
            dbProvider.AddInParameter(command, helper.Cmcongenvio, DbType.Decimal, entity.Cmcongenvio);
            dbProvider.AddInParameter(command, helper.Cmcongrecepcion, DbType.Decimal, entity.Cmcongrecepcion);
            dbProvider.AddInParameter(command, helper.Cmcongcongestion, DbType.Decimal, entity.Cmcongcongestion);
            dbProvider.AddInParameter(command, helper.Cmconggenlimite, DbType.Decimal, entity.Cmconggenlimite);
            dbProvider.AddInParameter(command, helper.Cmconggeneracion, DbType.Decimal, entity.Cmconggeneracion);
            dbProvider.AddInParameter(command, helper.Cmcongusucreacion, DbType.String, entity.Cmcongusucreacion);
            dbProvider.AddInParameter(command, helper.Cmcongfeccreacion, DbType.DateTime, entity.Cmcongfeccreacion);
            dbProvider.AddInParameter(command, helper.Cmcongusumodificacion, DbType.String, entity.Cmcongusumodificacion);
            dbProvider.AddInParameter(command, helper.Cmcongfecmodificacion, DbType.DateTime, entity.Cmcongfecmodificacion);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(CmCongestionCalculoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Configcodi, DbType.Int32, entity.Configcodi);
            dbProvider.AddInParameter(command, helper.Grulincodi, DbType.Int32, entity.Grulincodi);
            dbProvider.AddInParameter(command, helper.Regsegcodi, DbType.Int32, entity.Regsegcodi);
            dbProvider.AddInParameter(command, helper.Cmconfecha, DbType.DateTime, entity.Cmconfecha);
            dbProvider.AddInParameter(command, helper.Cmcongperiodo, DbType.Int32, entity.Cmcongperiodo);
            dbProvider.AddInParameter(command, helper.Cmgncorrelativo, DbType.Int32, entity.Cmgncorrelativo);
            dbProvider.AddInParameter(command, helper.Cmconglimite, DbType.Decimal, entity.Cmconglimite);
            dbProvider.AddInParameter(command, helper.Cmcongenvio, DbType.Decimal, entity.Cmcongenvio);
            dbProvider.AddInParameter(command, helper.Cmcongrecepcion, DbType.Decimal, entity.Cmcongrecepcion);
            dbProvider.AddInParameter(command, helper.Cmcongcongestion, DbType.Decimal, entity.Cmcongcongestion);
            dbProvider.AddInParameter(command, helper.Cmconggenlimite, DbType.Decimal, entity.Cmconggenlimite);
            dbProvider.AddInParameter(command, helper.Cmconggeneracion, DbType.Decimal, entity.Cmconggeneracion);
            dbProvider.AddInParameter(command, helper.Cmcongusucreacion, DbType.String, entity.Cmcongusucreacion);
            dbProvider.AddInParameter(command, helper.Cmcongfeccreacion, DbType.DateTime, entity.Cmcongfeccreacion);
            dbProvider.AddInParameter(command, helper.Cmcongusumodificacion, DbType.String, entity.Cmcongusumodificacion);
            dbProvider.AddInParameter(command, helper.Cmcongfecmodificacion, DbType.DateTime, entity.Cmcongfecmodificacion);
            dbProvider.AddInParameter(command, helper.Cmcongcodi, DbType.Int32, entity.Cmcongcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int periodo, DateTime fecha)
        {
            string query = string.Format(helper.SqlDelete, fecha.ToString(ConstantesBase.FormatoFecha), periodo);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            dbProvider.ExecuteNonQuery(command);
        }

        public CmCongestionCalculoDTO GetById(int cmcongcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Cmcongcodi, DbType.Int32, cmcongcodi);
            CmCongestionCalculoDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<CmCongestionCalculoDTO> List()
        {
            List<CmCongestionCalculoDTO> entitys = new List<CmCongestionCalculoDTO>();
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

        public List<CmCongestionCalculoDTO> GetByCriteria()
        {
            List<CmCongestionCalculoDTO> entitys = new List<CmCongestionCalculoDTO>();
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

        public List<CmCongestionCalculoDTO> ObtenerRegistroCongestion(DateTime fecha)
        {
            List<CmCongestionCalculoDTO> entitys = new List<CmCongestionCalculoDTO>();
            string sql = string.Format(helper.SqlObtenerRegistroCongestion, fecha.ToString(ConstantesBase.FormatoFecha));
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    CmCongestionCalculoDTO entity = new CmCongestionCalculoDTO();

                    int iConfigcodi = dr.GetOrdinal(helper.Configcodi);
                    if (!dr.IsDBNull(iConfigcodi)) entity.Configcodi = Convert.ToInt32(dr.GetValue(iConfigcodi));

                    int iGrulincodi = dr.GetOrdinal(helper.Grulincodi);
                    if (!dr.IsDBNull(iGrulincodi)) entity.Grulincodi = Convert.ToInt32(dr.GetValue(iGrulincodi));

                    int iRegsegcodi = dr.GetOrdinal(helper.Regsegcodi);
                    if (!dr.IsDBNull(iRegsegcodi)) entity.Regsegcodi = Convert.ToInt32(dr.GetValue(iRegsegcodi));

                    int iFamnomb = dr.GetOrdinal(helper.Famnomb);
                    if (!dr.IsDBNull(iFamnomb)) entity.Famnomb = dr.GetString(iFamnomb);

                    int iEquinomb = dr.GetOrdinal(helper.Equinomb);
                    if (!dr.IsDBNull(iEquinomb)) entity.Equinomb = dr.GetString(iEquinomb);

                    int iTipo = dr.GetOrdinal(helper.Tipo);
                    if (!dr.IsDBNull(iTipo)) entity.Tipo = Convert.ToInt32(dr.GetValue(iTipo));

                    int iCongesfecinicio = dr.GetOrdinal(helper.Congesfecinicio);
                    if (!dr.IsDBNull(iCongesfecinicio)) entity.Congesfecinicio = dr.GetDateTime(iCongesfecinicio);

                    int iCongesfecfin = dr.GetOrdinal(helper.Congesfecfin);
                    if (!dr.IsDBNull(iCongesfecfin)) entity.Congesfecfin = dr.GetDateTime(iCongesfecfin);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<CmCongestionCalculoDTO> ObtenerCongestionProceso(DateTime fecha)
        {
            List<CmCongestionCalculoDTO> entitys = new List<CmCongestionCalculoDTO>();
            string sql = string.Format(helper.SqlObtenerCongestionProceso, fecha.ToString(ConstantesBase.FormatoFecha));
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    CmCongestionCalculoDTO entity = helper.Create(dr);                   

                    int iFamnomb = dr.GetOrdinal(helper.Famnomb);
                    if (!dr.IsDBNull(iFamnomb)) entity.Famnomb = dr.GetString(iFamnomb);

                    int iEquinomb = dr.GetOrdinal(helper.Equinomb);
                    if (!dr.IsDBNull(iEquinomb)) entity.Equinomb = dr.GetString(iEquinomb);

                    int iTipo = dr.GetOrdinal(helper.Tipo);
                    if (!dr.IsDBNull(iTipo)) entity.Tipo = Convert.ToInt32(dr.GetValue(iTipo));

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<CmCongestionCalculoDTO> ObtenerCongestionPorLinea(DateTime fecha, string linea)
        {
            List<CmCongestionCalculoDTO> entitys = new List<CmCongestionCalculoDTO>();
            string sql = string.Format(helper.SqlObtenerCongestionPorLinea, fecha.ToString(ConstantesBase.FormatoFecha), linea);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    CmCongestionCalculoDTO entity = helper.Create(dr);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }
    }
}
