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
    /// Clase de acceso a datos de la tabla EPR_SUBESTACION
    /// </summary>
    public class EprSubestacionRepository : RepositoryBase, IEprSubestacionRepository
    {
        public EprSubestacionRepository(string strConn) : base(strConn)
        {
        }

        EprSubestacionHelper helper = new EprSubestacionHelper();

        public int Save(EprSubestacionDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Epsubecodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Areacodi, DbType.Int32, entity.Areacodi);
            dbProvider.AddInParameter(command, helper.Epproycodi, DbType.String, entity.Epproycodi);
            dbProvider.AddInParameter(command, helper.Epsubemotivo, DbType.String, entity.Epsubemotivo);
            dbProvider.AddInParameter(command, helper.Epsubefecha, DbType.String, entity.Epsubefecha);
            dbProvider.AddInParameter(command, helper.Epsubememoriacalculo, DbType.String, entity.Epsubememoriacalculo);
            dbProvider.AddInParameter(command, helper.Epsubeestregistro, DbType.String, entity.Epsubeestregistro);
            dbProvider.AddInParameter(command, helper.Epsubeusucreacion, DbType.String, entity.Epsubeusucreacion);
            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(EprSubestacionDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            
            dbProvider.AddInParameter(command, helper.Areacodi, DbType.Int32, entity.Areacodi);
            dbProvider.AddInParameter(command, helper.Epproycodi, DbType.Int32, entity.Epproycodi);
            dbProvider.AddInParameter(command, helper.Epsubemotivo, DbType.String, entity.Epsubemotivo);
            dbProvider.AddInParameter(command, helper.Epsubefecha, DbType.String, entity.Epsubefecha);
            dbProvider.AddInParameter(command, helper.Epsubememoriacalculo, DbType.String, entity.Epsubememoriacalculo);
            dbProvider.AddInParameter(command, helper.Epsubeestregistro, DbType.Int32, entity.Epsubeestregistro);
            dbProvider.AddInParameter(command, helper.Epsubeusumodificacion, DbType.String, entity.Epsubeusumodificacion);
            dbProvider.AddInParameter(command, helper.Epsubecodi, DbType.Int32, entity.Epsubecodi);
            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete_UpdateAuditoria(EprSubestacionDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete_UpdateAuditoria);
            dbProvider.AddInParameter(command, helper.Epsubeestregistro, DbType.Int32, entity.Epsubeestregistro);
            dbProvider.AddInParameter(command, helper.Epsubeusumodificacion, DbType.String, entity.Epsubeusumodificacion);
            dbProvider.AddInParameter(command, helper.Epsubecodi, DbType.Int32, entity.Epsubecodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public EprSubestacionDTO GetById(int epsubecodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Epsubecodi, DbType.Int32, epsubecodi);
            EprSubestacionDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<EprSubestacionDTO> List(int areacodi, int zonacodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlList);

            dbProvider.AddInParameter(command, helper.Areacodi, DbType.Int32, areacodi);
            dbProvider.AddInParameter(command, helper.Areacodi, DbType.Int32, areacodi);
            dbProvider.AddInParameter(command, helper.Zonacodi, DbType.Int32, zonacodi);
            dbProvider.AddInParameter(command, helper.Zonacodi, DbType.Int32, zonacodi);
            List<EprSubestacionDTO> entitys = new List<EprSubestacionDTO>();
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
