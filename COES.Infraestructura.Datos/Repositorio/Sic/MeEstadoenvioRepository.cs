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
    /// Clase de acceso a datos de la tabla ME_ESTADOENVIO
    /// </summary>
    public class MeEstadoenvioRepository: RepositoryBase, IMeEstadoenvioRepository
    {
        public MeEstadoenvioRepository(string strConn): base(strConn)
        {
        }

        MeEstadoenvioHelper helper = new MeEstadoenvioHelper();

        public void Update(MeEstadoenvioDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Estenvcodi, DbType.Int32, entity.Estenvcodi);
            dbProvider.AddInParameter(command, helper.Estenvnombre, DbType.String, entity.Estenvnombre);
            dbProvider.AddInParameter(command, helper.Estenvabrev, DbType.String, entity.Estenvabrev);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete()
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);


            dbProvider.ExecuteNonQuery(command);
        }

        public MeEstadoenvioDTO GetById()
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            MeEstadoenvioDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<MeEstadoenvioDTO> List()
        {
            List<MeEstadoenvioDTO> entitys = new List<MeEstadoenvioDTO>();
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

        public List<MeEstadoenvioDTO> GetByCriteria()
        {
            List<MeEstadoenvioDTO> entitys = new List<MeEstadoenvioDTO>();
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
    }
}
