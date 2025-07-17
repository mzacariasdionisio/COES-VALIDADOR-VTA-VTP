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
    /// Clase de acceso a datos de la tabla ME_MODULO
    /// </summary>
    public class MeModuloRepository: RepositoryBase, IMeModuloRepository
    {
        public MeModuloRepository(string strConn): base(strConn)
        {
        }

        MeModuloHelper helper = new MeModuloHelper();

        

        public void Update(MeModuloDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Modcodi, DbType.Int32, entity.Modcodi);
            dbProvider.AddInParameter(command, helper.Modnomb, DbType.String, entity.Modnomb);
            dbProvider.AddInParameter(command, helper.Origlectcodi, DbType.Int32, entity.Origlectcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete()
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);


            dbProvider.ExecuteNonQuery(command);
        }

        public MeModuloDTO GetById()
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            MeModuloDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<MeModuloDTO> List()
        {
            List<MeModuloDTO> entitys = new List<MeModuloDTO>();
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

        public List<MeModuloDTO> GetByCriteria()
        {
            List<MeModuloDTO> entitys = new List<MeModuloDTO>();
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
