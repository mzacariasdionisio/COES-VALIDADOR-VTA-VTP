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
    /// Clase de acceso a datos de la tabla CM_PERIODO
    /// </summary>
    public class CmPeriodoRepository: RepositoryBase, ICmPeriodoRepository
    {
        public CmPeriodoRepository(string strConn): base(strConn)
        {
        }

        CmPeriodoHelper helper = new CmPeriodoHelper();

        public int Save(CmPeriodoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Cmpercodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Cmperbase, DbType.String, entity.Cmperbase);
            dbProvider.AddInParameter(command, helper.Cmpermedia, DbType.String, entity.Cmpermedia);
            dbProvider.AddInParameter(command, helper.Cmperpunta, DbType.String, entity.Cmperpunta);
            dbProvider.AddInParameter(command, helper.Cmperestado, DbType.String, entity.Cmperestado);
            dbProvider.AddInParameter(command, helper.Cmpervigencia, DbType.DateTime, entity.Cmpervigencia);
            dbProvider.AddInParameter(command, helper.Cmperexpira, DbType.DateTime, entity.Cmperexpira);
            dbProvider.AddInParameter(command, helper.Cmperusucreacion, DbType.String, entity.Cmperusucreacion);
            dbProvider.AddInParameter(command, helper.Cmperfeccreacion, DbType.DateTime, entity.Cmperfeccreacion);
            dbProvider.AddInParameter(command, helper.Cmperusumodificacion, DbType.String, entity.Cmperusumodificacion);
            dbProvider.AddInParameter(command, helper.Cmperfecmodificacion, DbType.DateTime, entity.Cmperfecmodificacion);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(CmPeriodoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Cmperbase, DbType.String, entity.Cmperbase);
            dbProvider.AddInParameter(command, helper.Cmpermedia, DbType.String, entity.Cmpermedia);
            dbProvider.AddInParameter(command, helper.Cmperpunta, DbType.String, entity.Cmperpunta);
            dbProvider.AddInParameter(command, helper.Cmperestado, DbType.String, entity.Cmperestado);
            dbProvider.AddInParameter(command, helper.Cmpervigencia, DbType.DateTime, entity.Cmpervigencia);
            dbProvider.AddInParameter(command, helper.Cmperexpira, DbType.DateTime, entity.Cmperexpira);
            //dbProvider.AddInParameter(command, helper.Cmperusucreacion, DbType.String, entity.Cmperusucreacion);
            //dbProvider.AddInParameter(command, helper.Cmperfeccreacion, DbType.DateTime, entity.Cmperfeccreacion);
            dbProvider.AddInParameter(command, helper.Cmperusumodificacion, DbType.String, entity.Cmperusumodificacion);
            dbProvider.AddInParameter(command, helper.Cmperfecmodificacion, DbType.DateTime, entity.Cmperfecmodificacion);
            dbProvider.AddInParameter(command, helper.Cmpercodi, DbType.Int32, entity.Cmpercodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int cmpercodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Cmpercodi, DbType.Int32, cmpercodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public CmPeriodoDTO GetById(int cmpercodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Cmpercodi, DbType.Int32, cmpercodi);
            CmPeriodoDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<CmPeriodoDTO> List()
        {
            List<CmPeriodoDTO> entitys = new List<CmPeriodoDTO>();
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

        public List<CmPeriodoDTO> GetByCriteria(DateTime fecha)
        {
            List<CmPeriodoDTO> entitys = new List<CmPeriodoDTO>();
            string sql = string.Format(helper.SqlGetByCriteria, fecha.ToString(ConstantesBase.FormatoFecha));
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    CmPeriodoDTO entity = helper.Create(dr);

                    entity.Vigencia = (entity.Cmpervigencia != null) ?
                       ((DateTime)entity.Cmpervigencia).ToString(ConstantesBase.FormatoFechaBase) : string.Empty;

                    entity.Modificacion = (entity.Cmperfecmodificacion != null) ?
                        ((DateTime)entity.Cmperfecmodificacion).ToString(ConstantesBase.FormatoFechaBase) : string.Empty;

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<CmPeriodoDTO> ObtenerHistoricoPeriodo()
        {
            List<CmPeriodoDTO> entitys = new List<CmPeriodoDTO>();
            string sql = string.Format(helper.SqlObtenerHistorico);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    CmPeriodoDTO entity = helper.Create(dr);

                    entity.Vigencia = (entity.Cmpervigencia != null) ?
                       ((DateTime)entity.Cmpervigencia).ToString(ConstantesBase.FormatoFechaBase) : string.Empty;

                    entity.Modificacion = (entity.Cmperfecmodificacion != null) ?
                        ((DateTime)entity.Cmperfecmodificacion).ToString(ConstantesBase.FormatoFechaBase) : string.Empty;

                    entity.Expiracion = (entity.Cmperexpira != null) ?
                      ((DateTime)entity.Cmperexpira).ToString(ConstantesBase.FormatoFechaBase) : string.Empty;

                    entitys.Add(entity);
                }
            }

            return entitys;
        }
    }
}
