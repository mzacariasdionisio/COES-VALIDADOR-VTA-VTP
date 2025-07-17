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
    /// Clase de acceso a datos de la tabla CM_BARRA_RELACION_DET
    /// </summary>
    public class CmBarraRelacionDetRepository: RepositoryBase, ICmBarraRelacionDetRepository
    {
        public CmBarraRelacionDetRepository(string strConn): base(strConn)
        {
        }

        CmBarraRelacionDetHelper helper = new CmBarraRelacionDetHelper();

        public int Save(CmBarraRelacionDetDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Cmbadecodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Cmbarecodi, DbType.Int32, entity.Cmbarecodi);
            dbProvider.AddInParameter(command, helper.Cnfbarcodi, DbType.Int32, entity.Cnfbarcodi);
            dbProvider.AddInParameter(command, helper.Cmbadeestado, DbType.String, entity.Cmbadeestado);
            dbProvider.AddInParameter(command, helper.Cmbadeusucreacion, DbType.String, entity.Cmbadeusucreacion);
            dbProvider.AddInParameter(command, helper.Cmbadefeccreacion, DbType.DateTime, entity.Cmbadefeccreacion);
            dbProvider.AddInParameter(command, helper.Cmbadeusumodificacion, DbType.String, entity.Cmbadeusumodificacion);
            dbProvider.AddInParameter(command, helper.Cmbadefecmodificacion, DbType.DateTime, entity.Cmbadefecmodificacion);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(CmBarraRelacionDetDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Cmbarecodi, DbType.Int32, entity.Cmbarecodi);
            dbProvider.AddInParameter(command, helper.Cnfbarcodi, DbType.Int32, entity.Cnfbarcodi);
            dbProvider.AddInParameter(command, helper.Cmbadeestado, DbType.String, entity.Cmbadeestado);
            dbProvider.AddInParameter(command, helper.Cmbadeusucreacion, DbType.String, entity.Cmbadeusucreacion);
            dbProvider.AddInParameter(command, helper.Cmbadefeccreacion, DbType.DateTime, entity.Cmbadefeccreacion);
            dbProvider.AddInParameter(command, helper.Cmbadeusumodificacion, DbType.String, entity.Cmbadeusumodificacion);
            dbProvider.AddInParameter(command, helper.Cmbadefecmodificacion, DbType.DateTime, entity.Cmbadefecmodificacion);
            dbProvider.AddInParameter(command, helper.Cmbadecodi, DbType.Int32, entity.Cmbadecodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int cmbadecodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Cmbadecodi, DbType.Int32, cmbadecodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public CmBarraRelacionDetDTO GetById(int cmbadecodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Cmbadecodi, DbType.Int32, cmbadecodi);
            CmBarraRelacionDetDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<CmBarraRelacionDetDTO> List()
        {
            List<CmBarraRelacionDetDTO> entitys = new List<CmBarraRelacionDetDTO>();
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

        public List<CmBarraRelacionDetDTO> GetByCriteria(int idRelacion)
        {
            List<CmBarraRelacionDetDTO> entitys = new List<CmBarraRelacionDetDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByCriteria);
            dbProvider.AddInParameter(command, helper.Cmbarecodi, DbType.Int32, idRelacion);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    CmBarraRelacionDetDTO entity = helper.Create(dr);

                    int iCnfbarnombre = dr.GetOrdinal(helper.Cnfbarnombre);
                    if (!dr.IsDBNull(iCnfbarnombre)) entity.Barranombre = dr.GetString(iCnfbarnombre);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }
    }
}
