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
    public class EveTiposNumeralRepository : RepositoryBase, IEveTiposNumeralRepository
    {
        public EveTiposNumeralRepository(string strConn) : base(strConn)
        {
        }
        EveTiposNumeralHelper helper = new EveTiposNumeralHelper();

        public void Delete(int evetipnumcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);
            dbProvider.AddInParameter(command, helper.Evetipnumcodi, DbType.Int32, evetipnumcodi);
            dbProvider.ExecuteNonQuery(command);
        }

        public List<EveTiposNumeralDTO> List(string estado)
        {
            List<EveTiposNumeralDTO> entitys = new List<EveTiposNumeralDTO>();
            String query = String.Format(helper.SqlList, estado);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    EveTiposNumeralDTO entity = helper.Create(dr);
                    int iEstado = dr.GetOrdinal(helper.Estado);
                    if (!dr.IsDBNull(iEstado)) entity.ESTADO = dr.GetString(iEstado);

                    entitys.Add(entity);
                }
            }
            return entitys;
        }

        public int Save(EveTiposNumeralDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Evetipnumcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Evetipnumdescripcion, DbType.String, entity.EVETIPNUMDESCRIPCION);
            dbProvider.AddInParameter(command, helper.Evetipnumestado, DbType.String, entity.EVETIPNUMESTADO);
            dbProvider.AddInParameter(command, helper.Lastdate, DbType.DateTime, entity.LASTDATE);
            dbProvider.AddInParameter(command, helper.Lastuser, DbType.String, entity.LASTUSER);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(EveTiposNumeralDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);
                       
            dbProvider.AddInParameter(command, helper.Evetipnumdescripcion, DbType.String, entity.EVETIPNUMDESCRIPCION);
            dbProvider.AddInParameter(command, helper.Evetipnumestado, DbType.String, entity.EVETIPNUMESTADO);
            dbProvider.AddInParameter(command, helper.Lastdate, DbType.DateTime, entity.LASTDATE);
            dbProvider.AddInParameter(command, helper.Lastuser, DbType.String, entity.LASTUSER);
            dbProvider.AddInParameter(command, helper.Evetipnumcodi, DbType.Int32, entity.EVETIPNUMCODI);
            dbProvider.ExecuteNonQuery(command);
        }
    }
}
