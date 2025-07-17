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
    /// Clase de acceso a datos de la tabla PSU_DESVCMGSNC
    /// </summary>
    public class PsuDesvcmgsncRepository: RepositoryBase, IPsuDesvcmgsncRepository
    {
        public PsuDesvcmgsncRepository(string strConn): base(strConn)
        {
        }

        PsuDesvcmgsncHelper helper = new PsuDesvcmgsncHelper();

        public void Save(PsuDesvcmgsncDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Desvfecha, DbType.DateTime, entity.Desvfecha);
            dbProvider.AddInParameter(command, helper.Cmgsnc, DbType.Decimal, entity.Cmgsnc);
            dbProvider.AddInParameter(command, helper.Lastuser, DbType.String, entity.Lastuser);
            dbProvider.AddInParameter(command, helper.Lastdate, DbType.DateTime, entity.Lastdate);


            dbProvider.ExecuteNonQuery(command);
        }

        public void Update(PsuDesvcmgsncDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Cmgsnc, DbType.Decimal, entity.Cmgsnc);
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

        public PsuDesvcmgsncDTO GetById(DateTime desvfecha)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Desvfecha, DbType.DateTime, desvfecha);
            PsuDesvcmgsncDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<PsuDesvcmgsncDTO> List()
        {
            List<PsuDesvcmgsncDTO> entitys = new List<PsuDesvcmgsncDTO>();
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

        public List<PsuDesvcmgsncDTO> GetByCriteria(DateTime fechaMes)
        {
            List<PsuDesvcmgsncDTO> entitys = new List<PsuDesvcmgsncDTO>();
            string query = String.Format(helper.SqlGetByCriteria, fechaMes.ToString(ConstantesBase.FormatoFecha));
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
    }
}
