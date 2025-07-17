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
    /// Clase de acceso a datos de la tabla CM_UMBRAL_COMPARACION
    /// </summary>
    public class CmUmbralComparacionRepository: RepositoryBase, ICmUmbralComparacionRepository
    {
        public CmUmbralComparacionRepository(string strConn): base(strConn)
        {
        }

        CmUmbralComparacionHelper helper = new CmUmbralComparacionHelper();

        public int Save(CmUmbralComparacionDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Cmumcocodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Cmumcohopdesp, DbType.Decimal, entity.Cmumcohopdesp);
            dbProvider.AddInParameter(command, helper.Cmumcoemsdesp, DbType.Decimal, entity.Cmumcoemsdesp);
            dbProvider.AddInParameter(command, helper.Cmuncodemanda, DbType.Decimal, entity.Cmuncodemanda);
            dbProvider.AddInParameter(command, helper.Cmumcousucreacion, DbType.String, entity.Cmumcousucreacion);
            dbProvider.AddInParameter(command, helper.Cmumcofeccreacion, DbType.DateTime, entity.Cmumcofeccreacion);
            dbProvider.AddInParameter(command, helper.Cmuncousumodificacion, DbType.String, entity.Cmuncousumodificacion);
            dbProvider.AddInParameter(command, helper.Cmuncofecmodificacion, DbType.DateTime, entity.Cmuncofecmodificacion);
            dbProvider.AddInParameter(command, helper.Cmumcoci, DbType.Decimal, entity.Cmumcoci);
            dbProvider.AddInParameter(command, helper.Cmumconumiter, DbType.Decimal, entity.Cmumconumiter);
            dbProvider.AddInParameter(command, helper.Cmumcovarang, DbType.Decimal, entity.Cmumcovarang);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(CmUmbralComparacionDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Cmumcohopdesp, DbType.Decimal, entity.Cmumcohopdesp);
            dbProvider.AddInParameter(command, helper.Cmumcoemsdesp, DbType.Decimal, entity.Cmumcoemsdesp);
            dbProvider.AddInParameter(command, helper.Cmuncodemanda, DbType.Decimal, entity.Cmuncodemanda);
            dbProvider.AddInParameter(command, helper.Cmumcoci, DbType.Decimal, entity.Cmumcoci);
            dbProvider.AddInParameter(command, helper.Cmumconumiter, DbType.Decimal, entity.Cmumconumiter);
            dbProvider.AddInParameter(command, helper.Cmumcovarang, DbType.Decimal, entity.Cmumcovarang);
            dbProvider.AddInParameter(command, helper.Cmuncousumodificacion, DbType.String, entity.Cmuncousumodificacion);
            dbProvider.AddInParameter(command, helper.Cmuncofecmodificacion, DbType.DateTime, entity.Cmuncofecmodificacion);
            dbProvider.AddInParameter(command, helper.Cmumcocodi, DbType.Int32, entity.Cmumcocodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int cmumcocodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Cmumcocodi, DbType.Int32, cmumcocodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public CmUmbralComparacionDTO GetById(int cmumcocodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Cmumcocodi, DbType.Int32, cmumcocodi);
            CmUmbralComparacionDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<CmUmbralComparacionDTO> List()
        {
            List<CmUmbralComparacionDTO> entitys = new List<CmUmbralComparacionDTO>();
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

        public List<CmUmbralComparacionDTO> GetByCriteria()
        {
            List<CmUmbralComparacionDTO> entitys = new List<CmUmbralComparacionDTO>();
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
