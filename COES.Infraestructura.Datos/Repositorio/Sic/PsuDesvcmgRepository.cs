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
    /// Clase de acceso a datos de la tabla PSU_DESVCMG
    /// </summary>
    public class PsuDesvcmgRepository: RepositoryBase, IPsuDesvcmgRepository
    {
        public PsuDesvcmgRepository(string strConn): base(strConn)
        {
        }

        PsuDesvcmgHelper helper = new PsuDesvcmgHelper();

        public void Save(PsuDesvcmgDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Desvfecha, DbType.DateTime, entity.Desvfecha);
            dbProvider.AddInParameter(command, helper.Cmgrpunta, DbType.Decimal, entity.Cmgrpunta);
            dbProvider.AddInParameter(command, helper.Cmgrmedia, DbType.Decimal, entity.Cmgrmedia);
            dbProvider.AddInParameter(command, helper.Cmgrbase, DbType.Decimal, entity.Cmgrbase);
            dbProvider.AddInParameter(command, helper.Lastuser, DbType.String, entity.Lastuser);
            dbProvider.AddInParameter(command, helper.Lastdate, DbType.DateTime, entity.Lastdate);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Update(PsuDesvcmgDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Cmgrpunta, DbType.Decimal, entity.Cmgrpunta);
            dbProvider.AddInParameter(command, helper.Cmgrmedia, DbType.Decimal, entity.Cmgrmedia);
            dbProvider.AddInParameter(command, helper.Cmgrbase, DbType.Decimal, entity.Cmgrbase);
            dbProvider.AddInParameter(command, helper.Lastuser, DbType.String, entity.Lastuser);
            dbProvider.AddInParameter(command, helper.Lastdate, DbType.DateTime, entity.Lastdate);
            dbProvider.AddInParameter(command, helper.Desvfecha, DbType.DateTime, entity.Desvfecha);
            

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(DateTime desvfecha)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Desvfecha, DbType.DateTime, desvfecha);

            dbProvider.ExecuteNonQuery(command);
        }

        public PsuDesvcmgDTO GetById(DateTime desvfecha)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Desvfecha, DbType.DateTime, desvfecha);
            PsuDesvcmgDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<PsuDesvcmgDTO> List()
        {
            List<PsuDesvcmgDTO> entitys = new List<PsuDesvcmgDTO>();
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

        public List<PsuDesvcmgDTO> GetByCriteria()
        {
            List<PsuDesvcmgDTO> entitys = new List<PsuDesvcmgDTO>();
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
