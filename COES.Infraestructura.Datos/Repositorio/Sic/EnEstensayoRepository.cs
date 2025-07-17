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
    /// Clase de acceso a datos de la tabla EN_ESTENSAYO
    /// </summary>
    public class EnEstensayoRepository: RepositoryBase, IEnEstensayoRepository
    {
        public EnEstensayoRepository(string strConn): base(strConn)
        {
        }

        EnEstensayoHelper helper = new EnEstensayoHelper();

        public void Save(EnEstensayoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Ensayocodi, DbType.Int32, entity.Ensayocodi);
            dbProvider.AddInParameter(command, helper.Estadocodi, DbType.Int32, entity.Estadocodi);
            dbProvider.AddInParameter(command, helper.Estensayofecha, DbType.DateTime, entity.Estensayofecha);
            dbProvider.AddInParameter(command, helper.Estensayouser, DbType.String, entity.Estensayouser);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Update(EnEstensayoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Ensayocodi, DbType.Int32, entity.Ensayocodi);
            dbProvider.AddInParameter(command, helper.Estadocodi, DbType.Int32, entity.Estadocodi);
            dbProvider.AddInParameter(command, helper.Estensayofecha, DbType.DateTime, entity.Estensayofecha);
            dbProvider.AddInParameter(command, helper.Estensayouser, DbType.String, entity.Estensayouser);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete()
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);


            dbProvider.ExecuteNonQuery(command);
        }

        public EnEstensayoDTO GetById()
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            EnEstensayoDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<EnEstensayoDTO> List()
        {
            List<EnEstensayoDTO> entitys = new List<EnEstensayoDTO>();
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

        public List<EnEstensayoDTO> GetByCriteria()
        {
            List<EnEstensayoDTO> entitys = new List<EnEstensayoDTO>();
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
