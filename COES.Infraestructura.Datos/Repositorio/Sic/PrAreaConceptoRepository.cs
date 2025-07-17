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
    /// Clase de acceso a datos de la tabla PR_AREACONCEPTO
    /// </summary>
    public class PrAreaConceptoRepository : RepositoryBase, IPrAreaConceptoRepository
    {
        public PrAreaConceptoRepository(string strConn)
            : base(strConn)
        {
        }

        PrAreaConceptoHelper helper = new PrAreaConceptoHelper();

        public int Save(PrAreaConceptoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Arconcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Arconusucreacion, DbType.String, entity.Arconusucreacion);
            dbProvider.AddInParameter(command, helper.Arconfeccreacion, DbType.DateTime, entity.Arconfeccreacion);
            dbProvider.AddInParameter(command, helper.Arconusumodificacion, DbType.String, entity.Arconusumodificacion);
            dbProvider.AddInParameter(command, helper.Arconfecmodificacion, DbType.DateTime, entity.Arconfecmodificacion);
            dbProvider.AddInParameter(command, helper.Concepcodi, DbType.Int32, entity.Concepcodi);
            dbProvider.AddInParameter(command, helper.Arconactivo, DbType.Int32, entity.Arconactivo);
            dbProvider.AddInParameter(command, helper.Areacode, DbType.Int32, entity.Areacode);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(PrAreaConceptoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Arconusucreacion, DbType.String, entity.Arconusucreacion);
            dbProvider.AddInParameter(command, helper.Arconfeccreacion, DbType.DateTime, entity.Arconfeccreacion);
            dbProvider.AddInParameter(command, helper.Arconusumodificacion, DbType.String, entity.Arconusumodificacion);
            dbProvider.AddInParameter(command, helper.Arconfecmodificacion, DbType.DateTime, entity.Arconfecmodificacion);
            dbProvider.AddInParameter(command, helper.Concepcodi, DbType.Int32, entity.Concepcodi);
            dbProvider.AddInParameter(command, helper.Arconactivo, DbType.Int32, entity.Arconactivo);
            dbProvider.AddInParameter(command, helper.Areacode, DbType.Int32, entity.Areacode);

            dbProvider.AddInParameter(command, helper.Arconcodi, DbType.Int32, entity.Arconcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int arconcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Arconcodi, DbType.Int32, arconcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public PrAreaConceptoDTO GetById(int arconcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Arconcodi, DbType.Int32, arconcodi);
            PrAreaConceptoDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<PrAreaConceptoDTO> List()
        {
            List<PrAreaConceptoDTO> entitys = new List<PrAreaConceptoDTO>();
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

        public List<PrAreaConceptoDTO> GetByCriteria(int concepcodi, string arconactivo)
        {
            List<PrAreaConceptoDTO> entitys = new List<PrAreaConceptoDTO>();
            string query = string.Format(helper.SqlGetByCriteria, concepcodi, arconactivo);
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

        public List<int> ListarConcepcodiRegistrados()
        {
            List<int> entitys = new List<int>();
            string query = string.Format(helper.SqlListarConcepcodiRegistrados);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    int iConcepcodi = dr.GetOrdinal(this.helper.Concepcodi);
                    if (!dr.IsDBNull(iConcepcodi)) entitys.Add(Convert.ToInt32(dr.GetValue(iConcepcodi)));
                }
            }

            return entitys;
        }
    }
}
