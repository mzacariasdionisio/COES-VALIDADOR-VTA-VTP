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
    /// Clase de acceso a datos de la tabla PFR_CONCEPTO
    /// </summary>
    public class PfrConceptoRepository: RepositoryBase, IPfrConceptoRepository
    {
        public PfrConceptoRepository(string strConn): base(strConn)
        {
        }

        PfrConceptoHelper helper = new PfrConceptoHelper();

        public int Save(PfrConceptoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Pfrcnpcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Pfrcnpnomb, DbType.String, entity.Pfrcnpnomb);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(PfrConceptoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Pfrcnpcodi, DbType.Int32, entity.Pfrcnpcodi);
            dbProvider.AddInParameter(command, helper.Pfrcnpnomb, DbType.String, entity.Pfrcnpnomb);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int pfrcnpcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Pfrcnpcodi, DbType.Int32, pfrcnpcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public PfrConceptoDTO GetById(int pfrcnpcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Pfrcnpcodi, DbType.Int32, pfrcnpcodi);
            PfrConceptoDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<PfrConceptoDTO> List()
        {
            List<PfrConceptoDTO> entitys = new List<PfrConceptoDTO>();
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

        public List<PfrConceptoDTO> GetByCriteria()
        {
            List<PfrConceptoDTO> entitys = new List<PfrConceptoDTO>();
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
