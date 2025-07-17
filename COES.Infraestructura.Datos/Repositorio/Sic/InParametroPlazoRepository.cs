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
    /// Clase de acceso a datos de la tabla IN_PARAMETROPLAZO
    /// </summary>
    public class InParametroPlazoRepository : RepositoryBase, IInParametroPlazoRepository
    {
        public InParametroPlazoRepository(string strConn) : base(strConn)
        {
        }

        InParametroplazoHelper helper = new InParametroplazoHelper();


        public int Save(InParametroPlazoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Parplacodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Parpladesc, DbType.String, entity.Parpladesc);
            dbProvider.AddInParameter(command, helper.Parplafecdesde, DbType.DateTime, entity.Parplafecdesde);
            dbProvider.AddInParameter(command, helper.Parplafechasta, DbType.DateTime, entity.Parplafechasta);
            dbProvider.AddInParameter(command, helper.Parplahora, DbType.String, entity.Parplahora);
            dbProvider.AddInParameter(command, helper.Parplasucreacion, DbType.String, entity.Parplasucreacion);
            dbProvider.AddInParameter(command, helper.Parplafeccreacion, DbType.DateTime, entity.Parplafeccreacion);
            dbProvider.AddInParameter(command, helper.Parplausumodificacion, DbType.String, entity.Parplausumodificacion);
            dbProvider.AddInParameter(command, helper.Parplafecmodificacion, DbType.DateTime, entity.Parplafecmodificacion);

            dbProvider.AddInParameter(command, helper.Progrcodi, DbType.Int32, entity.Progrcodi);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(InParametroPlazoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Parpladesc, DbType.String, entity.Parpladesc);
            dbProvider.AddInParameter(command, helper.Parplafecdesde, DbType.DateTime, entity.Parplafecdesde);
            dbProvider.AddInParameter(command, helper.Parplafechasta, DbType.DateTime, entity.Parplafechasta);
            dbProvider.AddInParameter(command, helper.Parplahora, DbType.String, entity.Parplahora);
            dbProvider.AddInParameter(command, helper.Parplasucreacion, DbType.String, entity.Parplasucreacion);
            dbProvider.AddInParameter(command, helper.Parplafeccreacion, DbType.DateTime, entity.Parplafeccreacion);
            dbProvider.AddInParameter(command, helper.Parplausumodificacion, DbType.String, entity.Parplausumodificacion);
            dbProvider.AddInParameter(command, helper.Parplafecmodificacion, DbType.DateTime, entity.Parplafecmodificacion);
            dbProvider.AddInParameter(command, helper.Parplacodi, DbType.Int32, entity.Parplacodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int parplacodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Parplacodi, DbType.Int32, parplacodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public InParametroPlazoDTO GetById(int parplacodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Parplacodi, DbType.Int32, parplacodi);
            InParametroPlazoDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<InParametroPlazoDTO> List()
        {
            List<InParametroPlazoDTO> entitys = new List<InParametroPlazoDTO>();
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

        public List<InParametroPlazoDTO> GetByCriteria()
        {
            List<InParametroPlazoDTO> entitys = new List<InParametroPlazoDTO>();
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
