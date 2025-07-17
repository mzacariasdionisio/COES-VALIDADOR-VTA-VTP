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
    /// Clase de acceso a datos de la tabla EN_ESTADOS
    /// </summary>
    public class EnEstadoRepository : RepositoryBase, IEnEstadosRepository
    {
        public EnEstadoRepository(string strConn)
            : base(strConn)
        {
        }

        EnEstadoHelper helper = new EnEstadoHelper();

        public void Save(EnEstadoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Estadocodi, DbType.Int32, entity.Estadocodi);
            dbProvider.AddInParameter(command, helper.Estadonombre, DbType.String, entity.Estadonombre);
            dbProvider.AddInParameter(command, helper.Estadocolor, DbType.String, entity.Estadocolor);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Update(EnEstadoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Estadocodi, DbType.Int32, entity.Estadocodi);
            dbProvider.AddInParameter(command, helper.Estadonombre, DbType.String, entity.Estadonombre);
            dbProvider.AddInParameter(command, helper.Estadocolor, DbType.String, entity.Estadocolor);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete()
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);


            dbProvider.ExecuteNonQuery(command);
        }

        public EnEstadoDTO GetById()
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            EnEstadoDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<EnEstadoDTO> List()
        {
            List<EnEstadoDTO> entitys = new List<EnEstadoDTO>();
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

        public List<EnEstadoDTO> GetByCriteria()
        {
            List<EnEstadoDTO> entitys = new List<EnEstadoDTO>();
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
