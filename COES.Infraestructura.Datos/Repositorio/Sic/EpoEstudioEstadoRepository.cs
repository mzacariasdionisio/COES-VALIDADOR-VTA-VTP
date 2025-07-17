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
    /// Clase de acceso a datos de la tabla EPO_ESTUDIO_ESTADO
    /// </summary>
    public class EpoEstudioEstadoRepository: RepositoryBase, IEpoEstudioEstadoRepository
    {
        public EpoEstudioEstadoRepository(string strConn): base(strConn)
        {
        }

        EpoEstudioEstadoHelper helper = new EpoEstudioEstadoHelper();

        public int Save(EpoEstudioEstadoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Estacodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Estadescripcion, DbType.String, entity.Estadescripcion);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(EpoEstudioEstadoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Estacodi, DbType.Int32, entity.Estacodi);
            dbProvider.AddInParameter(command, helper.Estadescripcion, DbType.String, entity.Estadescripcion);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int estacodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Estacodi, DbType.Int32, estacodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public EpoEstudioEstadoDTO GetById(int estacodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Estacodi, DbType.Int32, estacodi);
            EpoEstudioEstadoDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<EpoEstudioEstadoDTO> List()
        {
            List<EpoEstudioEstadoDTO> entitys = new List<EpoEstudioEstadoDTO>();
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

        public List<EpoEstudioEstadoDTO> GetByCriteria()
        {
            List<EpoEstudioEstadoDTO> entitys = new List<EpoEstudioEstadoDTO>();
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

        #region Mejoras EO-EPO
        public List<EpoEstudioEstadoDTO> GetByCriteriaEstadosVigencia()
        {
            List<EpoEstudioEstadoDTO> entitys = new List<EpoEstudioEstadoDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByCriteriaEstadosVigencia);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }
        #endregion
    }
}
