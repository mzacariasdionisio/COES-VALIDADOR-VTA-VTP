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
    /// Clase de acceso a datos de la tabla PR_AREACONCEPUSER
    /// </summary>
    public class PrAreaConcepUserRepository : RepositoryBase, IPrAreaConcepUserRepository
    {
        public PrAreaConcepUserRepository(string strConn)
            : base(strConn)
        {
        }

        PrAreaConcepUserHelper helper = new PrAreaConcepUserHelper();

        public int Save(PrAreaConcepUserDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Aconuscodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Usercode, DbType.Int32, entity.Usercode);
            dbProvider.AddInParameter(command, helper.Arconcodi, DbType.Int32, entity.Arconcodi);
            dbProvider.AddInParameter(command, helper.Aconusactivo, DbType.Int32, entity.Aconusactivo);
            dbProvider.AddInParameter(command, helper.Aconususucreacion, DbType.String, entity.Aconususucreacion);
            dbProvider.AddInParameter(command, helper.Aconusfeccreacion, DbType.DateTime, entity.Aconusfeccreacion);
            dbProvider.AddInParameter(command, helper.Aconususumodificacion, DbType.String, entity.Aconususumodificacion);
            dbProvider.AddInParameter(command, helper.Aconusfecmodificacion, DbType.DateTime, entity.Aconusfecmodificacion);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(PrAreaConcepUserDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Usercode, DbType.Int32, entity.Usercode);
            dbProvider.AddInParameter(command, helper.Arconcodi, DbType.Int32, entity.Arconcodi);
            dbProvider.AddInParameter(command, helper.Aconusactivo, DbType.Int32, entity.Aconusactivo);
            dbProvider.AddInParameter(command, helper.Aconususucreacion, DbType.String, entity.Aconususucreacion);
            dbProvider.AddInParameter(command, helper.Aconusfeccreacion, DbType.DateTime, entity.Aconusfeccreacion);
            dbProvider.AddInParameter(command, helper.Aconususumodificacion, DbType.String, entity.Aconususumodificacion);
            dbProvider.AddInParameter(command, helper.Aconusfecmodificacion, DbType.DateTime, entity.Aconusfecmodificacion);

            dbProvider.AddInParameter(command, helper.Aconuscodi, DbType.Int32, entity.Aconuscodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int aconuscodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Aconuscodi, DbType.Int32, aconuscodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public PrAreaConcepUserDTO GetById(int aconuscodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Aconuscodi, DbType.Int32, aconuscodi);
            PrAreaConcepUserDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<PrAreaConcepUserDTO> List()
        {
            List<PrAreaConcepUserDTO> entitys = new List<PrAreaConcepUserDTO>();
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

        public List<PrAreaConcepUserDTO> GetByCriteria(int concepcodi, string arconactivo, string aconusactivo)
        {
            List<PrAreaConcepUserDTO> entitys = new List<PrAreaConcepUserDTO>();
            string query = string.Format(helper.SqlGetByCriteria, concepcodi, arconactivo, aconusactivo);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }

        public List<PrAreaConcepUserDTO> ListarConcepcodiByUsercode(string usercode)
        {
            List<PrAreaConcepUserDTO> entitys = new List<PrAreaConcepUserDTO>();
            string query = string.Format(helper.SqlListarConcepcodiByUsercode, usercode);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    PrAreaConcepUserDTO entity = new PrAreaConcepUserDTO();

                    int iConcepcodi = dr.GetOrdinal(this.helper.Concepcodi);
                    if (!dr.IsDBNull(iConcepcodi)) entity.Concepcodi = Convert.ToInt32(dr.GetValue(iConcepcodi));

                    int iUsercode = dr.GetOrdinal(this.helper.Usercode);
                    if (!dr.IsDBNull(iUsercode)) entity.Usercode = Convert.ToInt32(dr.GetValue(iUsercode));

                    int iAconusactivo = dr.GetOrdinal(this.helper.Aconusactivo);
                    if (!dr.IsDBNull(iAconusactivo)) entity.Aconusactivo = Convert.ToInt32(dr.GetValue(iAconusactivo));

                    entitys.Add(entity);
                }
            }

            return entitys;
        }
    }
}
