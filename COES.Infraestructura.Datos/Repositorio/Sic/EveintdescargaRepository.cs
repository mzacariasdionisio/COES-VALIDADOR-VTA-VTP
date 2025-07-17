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
    public class EveintdescargaRepository : RepositoryBase, IEveintdescargaRepository
    {
        public EveintdescargaRepository(string strConn) : base(strConn)
        {
        }
        EveintdescargaHelper helper = new EveintdescargaHelper();
        public void Delete(int evencodi, int tipo)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);
            dbProvider.AddInParameter(command, helper.Evencodi, DbType.Int32, evencodi);
            dbProvider.AddInParameter(command, helper.Eveintdestipo, DbType.Int32, tipo);
            dbProvider.ExecuteNonQuery(command);
        }

        public List<EveintdescargaDTO> List(int evencodi, int tipo)
        {
            List<EveintdescargaDTO> entitys = new List<EveintdescargaDTO>();
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

        public int Save(EveintdescargaDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Eveintdescodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Evencodi, DbType.Int32, entity.EVENCODI);
            dbProvider.AddInParameter(command, helper.Eveintdestipo, DbType.Int32, entity.EVEINTDESTIPO);
            dbProvider.AddInParameter(command, helper.Eveintdescelda, DbType.String, entity.EVEINTDESCELDA);
            dbProvider.AddInParameter(command, helper.Eveintdescodigo, DbType.String, entity.EVEINTDESCODIGO);
            dbProvider.AddInParameter(command, helper.Eveintdessubestacion, DbType.String, entity.EVEINTDESSUBESTACION);
            dbProvider.AddInParameter(command, helper.Eveintdesr_Antes, DbType.Int32, entity.EVEINTDESR_ANTES);
            dbProvider.AddInParameter(command, helper.Eveintdess_Antes, DbType.Int32, entity.EVEINTDESS_ANTES);
            dbProvider.AddInParameter(command, helper.Eveintdest_Antes, DbType.Int32, entity.EVEINTDEST_ANTES);
            dbProvider.AddInParameter(command, helper.Eveintdesr_Despues, DbType.Int32, entity.EVEINTDESR_DESPUES);
            dbProvider.AddInParameter(command, helper.Eveintdess_Despues, DbType.Int32, entity.EVEINTDESS_DESPUES);
            dbProvider.AddInParameter(command, helper.Eveintdest_Despues, DbType.Int32, entity.EVEINTDEST_DESPUES);
            dbProvider.AddInParameter(command, helper.Lastdate, DbType.DateTime, entity.LASTDATE);
            dbProvider.AddInParameter(command, helper.Lastuser, DbType.String, entity.LASTUSER);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }
    }
}
