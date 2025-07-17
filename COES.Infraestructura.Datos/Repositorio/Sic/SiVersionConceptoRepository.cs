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
    /// Clase de acceso a datos de la tabla SI_VERSION_CONCEPTO
    /// </summary>
    public class SiVersionConceptoRepository : RepositoryBase, ISiVersionConceptoRepository
    {
        public SiVersionConceptoRepository(string strConn) : base(strConn)
        {
        }

        SiVersionConceptoHelper helper = new SiVersionConceptoHelper();

        public int Save(SiVersionConceptoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Vercnpcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Vercnpdesc, DbType.String, entity.Vercnpdesc);
            dbProvider.AddInParameter(command, helper.Vercnptipo, DbType.String, entity.Vercnptipo);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(SiVersionConceptoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Vercnpcodi, DbType.Int32, entity.Vercnpcodi);
            dbProvider.AddInParameter(command, helper.Vercnpdesc, DbType.String, entity.Vercnpdesc);
            dbProvider.AddInParameter(command, helper.Vercnptipo, DbType.String, entity.Vercnptipo);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int vercnpcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Vercnpcodi, DbType.Int32, vercnpcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public SiVersionConceptoDTO GetById(int vercnpcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Vercnpcodi, DbType.Int32, vercnpcodi);
            SiVersionConceptoDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<SiVersionConceptoDTO> List()
        {
            List<SiVersionConceptoDTO> entitys = new List<SiVersionConceptoDTO>();
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

        public List<SiVersionConceptoDTO> GetByCriteria()
        {
            List<SiVersionConceptoDTO> entitys = new List<SiVersionConceptoDTO>();
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
