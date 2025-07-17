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
    /// Clase de acceso a datos de la tabla CO_RAEJECUTADA
    /// </summary>
    public class CoRaejecutadaRepository: RepositoryBase, ICoRaejecutadaRepository
    {
        public CoRaejecutadaRepository(string strConn): base(strConn)
        {
        }

        CoRaejecutadaHelper helper = new CoRaejecutadaHelper();

        public int Save(CoRaejecutadaDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Coraejcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Copercodi, DbType.Int32, entity.Copercodi);
            dbProvider.AddInParameter(command, helper.Covercodi, DbType.Int32, entity.Covercodi);
            dbProvider.AddInParameter(command, helper.Coraejrasub, DbType.Decimal, entity.Coraejrasub);
            dbProvider.AddInParameter(command, helper.Coraejrabaj, DbType.Decimal, entity.Coraejrabaj);
            dbProvider.AddInParameter(command, helper.Coraejfecha, DbType.DateTime, entity.Coraejfecha);
            dbProvider.AddInParameter(command, helper.Coraejfecini, DbType.DateTime, entity.Coraejfecini);
            dbProvider.AddInParameter(command, helper.Coraejfecfin, DbType.DateTime, entity.Coraejfecfin);
            dbProvider.AddInParameter(command, helper.Coraejusucreacion, DbType.String, entity.Coraejusucreacion);
            dbProvider.AddInParameter(command, helper.Coraejfeccreacion, DbType.DateTime, entity.Coraejfeccreacion);
            dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, entity.Equicodi);
            dbProvider.AddInParameter(command, helper.Grupocodi, DbType.Int32, entity.Grupocodi);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(CoRaejecutadaDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Copercodi, DbType.Int32, entity.Copercodi);
            dbProvider.AddInParameter(command, helper.Covercodi, DbType.Int32, entity.Covercodi);
            dbProvider.AddInParameter(command, helper.Coraejrasub, DbType.Decimal, entity.Coraejrasub);
            dbProvider.AddInParameter(command, helper.Coraejrabaj, DbType.Decimal, entity.Coraejrabaj);
            dbProvider.AddInParameter(command, helper.Coraejfecha, DbType.DateTime, entity.Coraejfecha);
            dbProvider.AddInParameter(command, helper.Coraejfecini, DbType.DateTime, entity.Coraejfecini);
            dbProvider.AddInParameter(command, helper.Coraejfecfin, DbType.DateTime, entity.Coraejfecfin);
            dbProvider.AddInParameter(command, helper.Coraejusucreacion, DbType.String, entity.Coraejusucreacion);
            dbProvider.AddInParameter(command, helper.Coraejfeccreacion, DbType.DateTime, entity.Coraejfeccreacion);
            dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, entity.Equicodi);
            dbProvider.AddInParameter(command, helper.Grupocodi, DbType.Int32, entity.Grupocodi);
            dbProvider.AddInParameter(command, helper.Coraejcodi, DbType.Int32, entity.Coraejcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int coraejcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Coraejcodi, DbType.Int32, coraejcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public CoRaejecutadaDTO GetById(int coraejcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Coraejcodi, DbType.Int32, coraejcodi);
            CoRaejecutadaDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<CoRaejecutadaDTO> List()
        {
            List<CoRaejecutadaDTO> entitys = new List<CoRaejecutadaDTO>();
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

        public List<CoRaejecutadaDTO> GetByCriteria()
        {
            List<CoRaejecutadaDTO> entitys = new List<CoRaejecutadaDTO>();
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
