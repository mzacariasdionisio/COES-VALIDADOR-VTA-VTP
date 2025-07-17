using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using COES.Dominio.DTO.Transferencias;
using COES.Dominio.Interfaces.Transferencias;
using COES.Base.Core;
using COES.Infraestructura.Datos.Helper.Transferencias;

namespace COES.Infraestructura.Datos.Repositorio.Transferencias
{
    /// <summary>
    /// Clase de acceso a datos de la tabla CAI_EQUISDDPUNI
    /// </summary>
    public class CaiEquisddpuniRepository: RepositoryBase, ICaiEquisddpuniRepository
    {
        public CaiEquisddpuniRepository(string strConn): base(strConn)
        {
        }

        CaiEquisddpuniHelper helper = new CaiEquisddpuniHelper();

        public int Save(CaiEquisddpuniDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Casdducodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, entity.Equicodi);
            dbProvider.AddInParameter(command, helper.Casdduunidad, DbType.String, entity.Casdduunidad);
            dbProvider.AddInParameter(command, helper.Casddufecvigencia, DbType.DateTime, entity.Casddufecvigencia);
            dbProvider.AddInParameter(command, helper.Casdduusucreacion, DbType.String, entity.Casdduusucreacion);
            dbProvider.AddInParameter(command, helper.Casddufeccreacion, DbType.DateTime, entity.Casddufeccreacion);
            dbProvider.AddInParameter(command, helper.Casdduusumodificacion, DbType.String, entity.Casdduusumodificacion);
            dbProvider.AddInParameter(command, helper.Casddufecmodificacion, DbType.DateTime, entity.Casddufecmodificacion);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(CaiEquisddpuniDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, entity.Equicodi);
            dbProvider.AddInParameter(command, helper.Casdduunidad, DbType.String, entity.Casdduunidad);
            dbProvider.AddInParameter(command, helper.Casddufecvigencia, DbType.DateTime, entity.Casddufecvigencia);
            dbProvider.AddInParameter(command, helper.Casdduusumodificacion, DbType.String, entity.Casdduusumodificacion);
            dbProvider.AddInParameter(command, helper.Casddufecmodificacion, DbType.DateTime, entity.Casddufecmodificacion);
            dbProvider.AddInParameter(command, helper.Casdducodi, DbType.Int32, entity.Casdducodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int casdducodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Casdducodi, DbType.Int32, casdducodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public CaiEquisddpuniDTO GetById(int casdducodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Casdducodi, DbType.Int32, casdducodi);
            CaiEquisddpuniDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public CaiEquisddpuniDTO GetByIdCaiEquisddpuni(int casdducodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.GetByIdCaiEquisddpuni);

            dbProvider.AddInParameter(command, helper.Casdducodi, DbType.Int32, casdducodi);
            CaiEquisddpuniDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                    int iEquinomb = dr.GetOrdinal(this.helper.Equinomb);
                    if (!dr.IsDBNull(iEquinomb)) entity.Equinomb = dr.GetString(iEquinomb);
                }
            }

            return entity;
        }

        public CaiEquisddpuniDTO GetByNombreEquipoSddp(string sddpgmnombre)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.GetByNombreEquipoSddp);

            dbProvider.AddInParameter(command, helper.Casdduunidad, DbType.String, sddpgmnombre);
            CaiEquisddpuniDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<CaiEquisddpuniDTO> List()
        {
            List<CaiEquisddpuniDTO> entitys = new List<CaiEquisddpuniDTO>();
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

        public List<CaiEquisddpuniDTO> ListCaiEquisddpuni()
        {
            List<CaiEquisddpuniDTO> entitys = new List<CaiEquisddpuniDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListEquisddpuni);

            CaiEquisddpuniDTO entity;
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = helper.Create(dr);
                    int iEquinomb = dr.GetOrdinal(this.helper.Equinomb);
                    if (!dr.IsDBNull(iEquinomb)) entity.Equinomb = dr.GetString(iEquinomb);

                    entitys.Add(entity);

                }
            }

            return entitys;
        }

        public List<CentralGeneracionDTO> Unidad()
        {
            List<CentralGeneracionDTO> entitys = new List<CentralGeneracionDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUnidad);

            CentralGeneracionDTO entity;
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = new CentralGeneracionDTO();

                    int iEquicodi = dr.GetOrdinal(this.helper.Equicodi);
                    if (!dr.IsDBNull(iEquicodi)) entity.CentGeneCodi = Convert.ToInt32(dr.GetValue(iEquicodi));

                    int iEquinomb = dr.GetOrdinal(this.helper.Equinomb);
                    if (!dr.IsDBNull(iEquinomb)) entity.CentGeneNombre = dr.GetString(iEquinomb);

                    entitys.Add(entity);

                }
            }

            return entitys;
        }

        public List<CaiEquisddpuniDTO> GetByCriteria()
        {
            List<CaiEquisddpuniDTO> entitys = new List<CaiEquisddpuniDTO>();
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

        public List<CaiEquisddpuniDTO> GetByCriteriaCaiEquiunidbarrsNoIns()
        {
            List<CaiEquisddpuniDTO> entitys = new List<CaiEquisddpuniDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.GetByCriteriaCaiEquiunidbarrsNoIns);

            CaiEquisddpuniDTO entity;
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = new CaiEquisddpuniDTO();
                    entity = helper.Create(dr);

                    int iEquinomb = dr.GetOrdinal(this.helper.Equinomb);
                    if (!dr.IsDBNull(iEquinomb)) entity.Equinomb = dr.GetString(iEquinomb);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }
    }
}
