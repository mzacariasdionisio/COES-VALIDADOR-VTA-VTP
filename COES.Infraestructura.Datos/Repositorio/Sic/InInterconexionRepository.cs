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
    /// Clase de acceso a datos de la tabla ME_INTERCONEXION
    /// </summary>
    public class InInterconexionRepository: RepositoryBase, IInInterconexionRepository
    {
        public InInterconexionRepository(string strConn): base(strConn)
        {
        }

        InInterconexionHelper helper = new InInterconexionHelper();

        public List<InInterconexionDTO> List()
        {
            List<InInterconexionDTO> entitys = new List<InInterconexionDTO>();
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

        public InInterconexionDTO GetById(int id)
        {
            InInterconexionDTO entity = null;
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);
            dbProvider.AddInParameter(command, helper.Intercodi, DbType.Int32, id);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

    }
}
