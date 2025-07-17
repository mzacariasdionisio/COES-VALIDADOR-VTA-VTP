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
    public class EveRecomobservRepository : RepositoryBase, IEveRecomobservRepository
    {
        public EveRecomobservRepository(string strConn) : base(strConn)
        {
        }
        EveRecomobservHelper helper = new EveRecomobservHelper();
        public void Delete(int everecomobservcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);
            dbProvider.AddInParameter(command, helper.Everecomobservcodi, DbType.Int32, everecomobservcodi);
            dbProvider.ExecuteNonQuery(command);
        }

        public List<EveRecomobservDTO> List(int evencodi, int tipo)
        {
            List<EveRecomobservDTO> entitys = new List<EveRecomobservDTO>();
            String query = String.Format(helper.SqlList, evencodi, tipo);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    EveRecomobservDTO entity = helper.Create(dr);
                    int iEmprnomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.EMPRNOMB = dr.GetString(iEmprnomb);

                    entitys.Add(entity);
                }
            }
            return entitys;
        }

        public int Save(EveRecomobservDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Everecomobservcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Evencodi, DbType.Int32, entity.EVENCODI);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.EMPRCODI);
            dbProvider.AddInParameter(command, helper.Everecomobservtipo, DbType.Int32, entity.EVERECOMOBSERVTIPO);
            dbProvider.AddInParameter(command, helper.Everecomobservdesc, DbType.String, entity.EVERECOMOBSERVDESC);
            dbProvider.AddInParameter(command, helper.Everecomobservestado, DbType.String, entity.EVERECOMOBSERVESTADO);
            dbProvider.AddInParameter(command, helper.Lastdate, DbType.DateTime, entity.LASTDATE);
            dbProvider.AddInParameter(command, helper.Lastuser, DbType.String, entity.LASTUSER);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }
    }
}
