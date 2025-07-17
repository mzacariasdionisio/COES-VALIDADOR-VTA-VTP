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
    /// Clase de acceso a datos de la tabla IND_RECALCULO
    /// </summary>
    public class IndRecalculoRepository : RepositoryBase, IIndRecalculoRepository
    {
        public IndRecalculoRepository(string strConn) : base(strConn)
        {
        }

        IndRecalculoHelper helper = new IndRecalculoHelper();

        public int Save(IndRecalculoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Irecacodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Irecadescripcion, DbType.String, entity.Irecadescripcion);
            dbProvider.AddInParameter(command, helper.Irecanombre, DbType.String, entity.Irecanombre);
            dbProvider.AddInParameter(command, helper.Ipericodi, DbType.Int32, entity.Ipericodi);
            dbProvider.AddInParameter(command, helper.Irecausucreacion, DbType.String, entity.Irecausucreacion);
            dbProvider.AddInParameter(command, helper.Irecafeccreacion, DbType.DateTime, entity.Irecafeccreacion);
            dbProvider.AddInParameter(command, helper.Irecausumodificacion, DbType.String, entity.Irecausumodificacion);
            dbProvider.AddInParameter(command, helper.Irecafecmodificacion, DbType.DateTime, entity.Irecafecmodificacion);
            dbProvider.AddInParameter(command, helper.Irecainforme, DbType.String, entity.Irecainforme);
            dbProvider.AddInParameter(command, helper.Irecafechalimite, DbType.DateTime, entity.Irecafechalimite);
            dbProvider.AddInParameter(command, helper.Irecafechaini, DbType.DateTime, entity.Irecafechaini);
            dbProvider.AddInParameter(command, helper.Irecafechafin, DbType.DateTime, entity.Irecafechafin);
            dbProvider.AddInParameter(command, helper.Irecafechaobs, DbType.DateTime, entity.Irecafechaobs);
            dbProvider.AddInParameter(command, helper.Irecatipo, DbType.String, entity.Irecatipo);
            dbProvider.AddInParameter(command, helper.Irecaesfinal, DbType.Int32, entity.Irecaesfinal);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(IndRecalculoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Irecadescripcion, DbType.String, entity.Irecadescripcion);
            dbProvider.AddInParameter(command, helper.Irecanombre, DbType.String, entity.Irecanombre);
            dbProvider.AddInParameter(command, helper.Ipericodi, DbType.Int32, entity.Ipericodi);
            dbProvider.AddInParameter(command, helper.Irecausucreacion, DbType.String, entity.Irecausucreacion);
            dbProvider.AddInParameter(command, helper.Irecafeccreacion, DbType.DateTime, entity.Irecafeccreacion);
            dbProvider.AddInParameter(command, helper.Irecausumodificacion, DbType.String, entity.Irecausumodificacion);
            dbProvider.AddInParameter(command, helper.Irecafecmodificacion, DbType.DateTime, entity.Irecafecmodificacion);
            dbProvider.AddInParameter(command, helper.Irecainforme, DbType.String, entity.Irecainforme);
            dbProvider.AddInParameter(command, helper.Irecafechalimite, DbType.DateTime, entity.Irecafechalimite);
            dbProvider.AddInParameter(command, helper.Irecafechaini, DbType.DateTime, entity.Irecafechaini);
            dbProvider.AddInParameter(command, helper.Irecafechafin, DbType.DateTime, entity.Irecafechafin);
            dbProvider.AddInParameter(command, helper.Irecafechaobs, DbType.DateTime, entity.Irecafechaobs);
            dbProvider.AddInParameter(command, helper.Irecatipo, DbType.String, entity.Irecatipo);
            dbProvider.AddInParameter(command, helper.Irecaesfinal, DbType.Int32, entity.Irecaesfinal);

            dbProvider.AddInParameter(command, helper.Irecacodi, DbType.Int32, entity.Irecacodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int irecacodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Irecacodi, DbType.Int32, irecacodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public IndRecalculoDTO GetById(int irecacodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Irecacodi, DbType.Int32, irecacodi);
            IndRecalculoDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<IndRecalculoDTO> List()
        {
            List<IndRecalculoDTO> entitys = new List<IndRecalculoDTO>();
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

        public List<IndRecalculoDTO> GetByCriteria(int ipericodi)
        {
            List<IndRecalculoDTO> entitys = new List<IndRecalculoDTO>();

            string query = string.Format(helper.SqlGetByCriteria, ipericodi);
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

        public List<IndRecalculoDTO> ListXMes(int anio, int mes)
        {
            List<IndRecalculoDTO> entitys = new List<IndRecalculoDTO>();

            string query = string.Format(helper.SqlListXMes, anio, mes);
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

    }
}
