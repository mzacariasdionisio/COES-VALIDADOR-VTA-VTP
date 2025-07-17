using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using COES.Base.Core;
using COES.Dominio.DTO.Sic;
using COES.Dominio.Interfaces.Sic;
using COES.Infraestructura.Datos.Helper.Sic;

namespace COES.Infraestructura.Datos.Repositorio.Sic
{
    public class EveCondPreviaRepository : RepositoryBase, IEveCondPreviaRepository
    {
        public EveCondPreviaRepository(string strConn) : base(strConn)
        {
        }
        EveCondPreviaHelper helper = new EveCondPreviaHelper();
        public void Delete(int evecondprcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);
            dbProvider.AddInParameter(command, helper.Evecondprcodi, DbType.Int32, evecondprcodi);
            dbProvider.ExecuteNonQuery(command);
        }

        public List<EveCondPreviaDTO> List(int evencodi, string tipo)
        {
            List<EveCondPreviaDTO> entitys = new List<EveCondPreviaDTO>();
            String query = String.Format(helper.SqlList, evencodi, tipo);
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

        public int Save(EveCondPreviaDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Evecondprcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Evencodi, DbType.Int32, entity.EVENCODI);
            dbProvider.AddInParameter(command, helper.Evecondprtipo, DbType.String, entity.EVECONDPRTIPO);
            dbProvider.AddInParameter(command, helper.Evecondprcodigounidad, DbType.String, entity.EVECONDPRCODIGOUNIDAD);
            dbProvider.AddInParameter(command, helper.Evecondprsubestaciona, DbType.String, entity.EVECONDPRSUBESTACIONA);
            dbProvider.AddInParameter(command, helper.Evecondprsubestacioncent, DbType.Int32, entity.EVECONDPRSUBESTACIONCENT);
            dbProvider.AddInParameter(command, helper.Evecondprtension, DbType.Decimal, entity.EVECONDPRTENSION);
            dbProvider.AddInParameter(command, helper.Evecondprpotenciamw, DbType.String, entity.EVECONDPRPOTENCIAMW);
            dbProvider.AddInParameter(command, helper.Evecondprpotenciamvar, DbType.String, entity.EVECONDPRPOTENCIAMVAR);
            dbProvider.AddInParameter(command, helper.Evecondprcentralde, DbType.String, entity.EVECONDPRCENTRALDE);
            dbProvider.AddInParameter(command, helper.Lastdate, DbType.DateTime, entity.LASTDATE);
            dbProvider.AddInParameter(command, helper.Lastuser, DbType.String, entity.LASTUSER);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public EveCondPreviaDTO GetById(int evecondprcodi)
        {
            String query = String.Format(helper.SqlGetById, evecondprcodi);
            DbCommand command = dbProvider.GetSqlStringCommand(query);
            EveCondPreviaDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public void Update(EveCondPreviaDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);
            
            dbProvider.AddInParameter(command, helper.Evecondprcodigounidad, DbType.String, entity.EVECONDPRCODIGOUNIDAD);
            dbProvider.AddInParameter(command, helper.Evecondprsubestaciona, DbType.String, entity.EVECONDPRSUBESTACIONA);
            dbProvider.AddInParameter(command, helper.Evecondprtension, DbType.Decimal, entity.EVECONDPRTENSION);
            dbProvider.AddInParameter(command, helper.Evecondprcentralde, DbType.String, entity.EVECONDPRCENTRALDE);
            dbProvider.AddInParameter(command, helper.Evecondprpotenciamw, DbType.String, entity.EVECONDPRPOTENCIAMW);
            dbProvider.AddInParameter(command, helper.Evecondprpotenciamvar, DbType.String, entity.EVECONDPRPOTENCIAMVAR);
            dbProvider.AddInParameter(command, helper.Lastdate, DbType.DateTime, entity.LASTDATE);
            dbProvider.AddInParameter(command, helper.Lastuser, DbType.String, entity.LASTUSER);
            dbProvider.AddInParameter(command, helper.Evecondprcodi, DbType.Int32, entity.EVECONDPRCODI);

            dbProvider.ExecuteNonQuery(command);
        }

        public EveCondPreviaDTO GetByIdCanalZona(int evencodi, string evecondprtipo, int zona, string evecondprcodigounidad)
        {
            String query = String.Format(helper.GetByIdCanalZona, evencodi, evecondprtipo, zona, evecondprcodigounidad);
            DbCommand command = dbProvider.GetSqlStringCommand(query);
            EveCondPreviaDTO entity = null;

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
