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
    /// Clase de acceso a datos de la tabla CM_VOLUMEN_CALCULO
    /// </summary>
    public class CmVolumenCalculoRepository : RepositoryBase, ICmVolumenCalculoRepository
    {
        public CmVolumenCalculoRepository(string strConn) : base(strConn)
        {
        }

        CmVolumenCalculoHelper helper = new CmVolumenCalculoHelper();

        public int Save(CmVolumenCalculoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Volcalcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Volcalfecha, DbType.DateTime, entity.Volcalfecha);
            dbProvider.AddInParameter(command, helper.Volcalperiodo, DbType.Int32, entity.Volcalperiodo);
            dbProvider.AddInParameter(command, helper.Volcaltipo, DbType.String, entity.Volcaltipo);
            dbProvider.AddInParameter(command, helper.Volcalusucreacion, DbType.String, entity.Volcalusucreacion);
            dbProvider.AddInParameter(command, helper.Volcalfeccreacion, DbType.DateTime, entity.Volcalfeccreacion);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(CmVolumenCalculoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Volcalcodi, DbType.Int32, entity.Volcalcodi);
            dbProvider.AddInParameter(command, helper.Volcalfecha, DbType.DateTime, entity.Volcalfecha);
            dbProvider.AddInParameter(command, helper.Volcalperiodo, DbType.Int32, entity.Volcalperiodo);
            dbProvider.AddInParameter(command, helper.Volcaltipo, DbType.String, entity.Volcaltipo);
            dbProvider.AddInParameter(command, helper.Volcalusucreacion, DbType.String, entity.Volcalusucreacion);
            dbProvider.AddInParameter(command, helper.Volcalfeccreacion, DbType.DateTime, entity.Volcalfeccreacion);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int volcalcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Volcalcodi, DbType.Int32, volcalcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public CmVolumenCalculoDTO GetById(int volcalcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Volcalcodi, DbType.Int32, volcalcodi);
            CmVolumenCalculoDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<CmVolumenCalculoDTO> List()
        {
            List<CmVolumenCalculoDTO> entitys = new List<CmVolumenCalculoDTO>();
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

        public List<CmVolumenCalculoDTO> GetByCriteria(DateTime fechaPeriodo, int periodoH)
        {
            List<CmVolumenCalculoDTO> entitys = new List<CmVolumenCalculoDTO>();

            string sql = string.Format(helper.SqlGetByCriteria, fechaPeriodo.ToString(ConstantesBase.FormatoFecha), periodoH);
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
    }
}
