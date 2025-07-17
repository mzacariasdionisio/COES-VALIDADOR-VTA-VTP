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
    /// Clase de acceso a datos de la tabla PMO_SDDP_CODIGO
    /// </summary>
    public class PmoSddpCodigoRepository : RepositoryBase, IPmoSddpCodigoRepository
    {
        public PmoSddpCodigoRepository(string strConn) : base(strConn)
        {
        }

        PmoSddpCodigoHelper helper = new PmoSddpCodigoHelper();

        public int Save(PmoSddpCodigoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Sddpcodi, DbType.Int32, id);

            dbProvider.AddInParameter(command, helper.Tsddpcodi, DbType.Int32, entity.Tsddpcodi);
            dbProvider.AddInParameter(command, helper.Sddpnum, DbType.Int32, entity.Sddpnum);
            dbProvider.AddInParameter(command, helper.Sddpnomb, DbType.String, entity.Sddpnomb);
            dbProvider.AddInParameter(command, helper.Sddpestado, DbType.String, entity.Sddpestado);
            dbProvider.AddInParameter(command, helper.Sddpdesc, DbType.String, entity.Sddpdesc);
            dbProvider.AddInParameter(command, helper.Sddpcomentario, DbType.String, entity.Sddpcomentario);
            dbProvider.AddInParameter(command, helper.Sddpusucreacion, DbType.String, entity.Sddpusucreacion);
            dbProvider.AddInParameter(command, helper.Sddpfeccreacion, DbType.DateTime, entity.Sddpfeccreacion);
            dbProvider.AddInParameter(command, helper.Sddpusumodificacion, DbType.String, entity.Sddpusumodificacion);
            dbProvider.AddInParameter(command, helper.Sddpfecmodificacion, DbType.DateTime, entity.Sddpfecmodificacion);
            dbProvider.AddInParameter(command, helper.Tptomedicodi, DbType.Int32, entity.Tptomedicodi);
            dbProvider.AddInParameter(command, helper.Ptomedicodi, DbType.Int32, entity.Ptomedicodi);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(PmoSddpCodigoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Tsddpcodi, DbType.Int32, entity.Tsddpcodi);
            dbProvider.AddInParameter(command, helper.Sddpnum, DbType.Int32, entity.Sddpnum);
            dbProvider.AddInParameter(command, helper.Sddpnomb, DbType.String, entity.Sddpnomb);
            dbProvider.AddInParameter(command, helper.Sddpestado, DbType.String, entity.Sddpestado);
            dbProvider.AddInParameter(command, helper.Sddpdesc, DbType.String, entity.Sddpdesc);
            dbProvider.AddInParameter(command, helper.Sddpcomentario, DbType.String, entity.Sddpcomentario);
            dbProvider.AddInParameter(command, helper.Sddpusucreacion, DbType.String, entity.Sddpusucreacion);
            dbProvider.AddInParameter(command, helper.Sddpfeccreacion, DbType.DateTime, entity.Sddpfeccreacion);
            dbProvider.AddInParameter(command, helper.Sddpusumodificacion, DbType.String, entity.Sddpusumodificacion);
            dbProvider.AddInParameter(command, helper.Sddpfecmodificacion, DbType.DateTime, entity.Sddpfecmodificacion);
            dbProvider.AddInParameter(command, helper.Tptomedicodi, DbType.Int32, entity.Tptomedicodi);
            dbProvider.AddInParameter(command, helper.Ptomedicodi, DbType.Int32, entity.Ptomedicodi);

            dbProvider.AddInParameter(command, helper.Sddpcodi, DbType.Int32, entity.Sddpcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int sddpcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Sddpcodi, DbType.Int32, sddpcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public PmoSddpCodigoDTO GetById(int sddpcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Sddpcodi, DbType.Int32, sddpcodi);
            PmoSddpCodigoDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public PmoSddpCodigoDTO GetByNumYTipo(int num, int tsddpcodi)
        {
            string sql = string.Format(helper.SqlGetByNumYTipo, num, tsddpcodi);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            PmoSddpCodigoDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<PmoSddpCodigoDTO> List()
        {
            List<PmoSddpCodigoDTO> entitys = new List<PmoSddpCodigoDTO>();
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

        public List<PmoSddpCodigoDTO> GetByCriteria(string tsddpcodi)
        {
            List<PmoSddpCodigoDTO> entitys = new List<PmoSddpCodigoDTO>();

            string sql = string.Format(helper.SqlGetByCriteria, tsddpcodi);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    var entity = helper.Create(dr);

                    int iTsddpnomb = dr.GetOrdinal(helper.Tsddpnomb);
                    if (!dr.IsDBNull(iTsddpnomb)) entity.Tsddpnomb = dr.GetString(iTsddpnomb);

                    int iPtomedielenomb = dr.GetOrdinal(helper.Ptomedielenomb);
                    if (!dr.IsDBNull(iPtomedielenomb)) entity.Ptomedielenomb = dr.GetString(iPtomedielenomb);

                    int iPtomedidesc = dr.GetOrdinal(helper.Ptomedidesc);
                    if (!dr.IsDBNull(iPtomedidesc)) entity.Ptomedidesc = dr.GetString(iPtomedidesc);

                    int iGrupocodi = dr.GetOrdinal(helper.Grupocodi);
                    if (!dr.IsDBNull(iGrupocodi)) entity.Grupocodi = Convert.ToInt32(dr.GetValue(iGrupocodi));

                    int iEquicodi = dr.GetOrdinal(helper.Equicodi);
                    if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

                    entitys.Add(entity);
                }
            }

            return entitys;
        }
    }
}
