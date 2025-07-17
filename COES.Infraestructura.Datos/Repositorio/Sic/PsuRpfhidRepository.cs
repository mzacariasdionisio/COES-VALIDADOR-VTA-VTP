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
    /// Clase de acceso a datos de la tabla PSU_RPFHID
    /// </summary>
    public class PsuRpfhidRepository: RepositoryBase, IPsuRpfhidRepository
    {
        public PsuRpfhidRepository(string strConn): base(strConn)
        {
        }

        PsuRpfhidHelper helper = new PsuRpfhidHelper();


        public void Save(PsuRpfhidDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Rpfhidfecha, DbType.DateTime, entity.Rpfhidfecha);
            dbProvider.AddInParameter(command, helper.Rpfenetotal, DbType.Decimal, entity.Rpfenetotal);
            dbProvider.AddInParameter(command, helper.Rpfpotmedia, DbType.Decimal, entity.Rpfpotmedia);
            dbProvider.AddInParameter(command, helper.Eneindhidra, DbType.Decimal, entity.Eneindhidra);
            dbProvider.AddInParameter(command, helper.Potindhidra, DbType.Decimal, entity.Potindhidra);
            dbProvider.AddInParameter(command, helper.Lastuser, DbType.String, entity.Lastuser);
            dbProvider.AddInParameter(command, helper.Lastdate, DbType.DateTime, entity.Lastdate);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Update(PsuRpfhidDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);
                        
            dbProvider.AddInParameter(command, helper.Rpfenetotal, DbType.Decimal, entity.Rpfenetotal);
            dbProvider.AddInParameter(command, helper.Rpfpotmedia, DbType.Decimal, entity.Rpfpotmedia);
            dbProvider.AddInParameter(command, helper.Eneindhidra, DbType.Decimal, entity.Eneindhidra);
            dbProvider.AddInParameter(command, helper.Potindhidra, DbType.Decimal, entity.Potindhidra);
            dbProvider.AddInParameter(command, helper.Lastuser, DbType.String, entity.Lastuser);
            dbProvider.AddInParameter(command, helper.Lastdate, DbType.DateTime, entity.Lastdate);
            dbProvider.AddInParameter(command, helper.Rpfhidfecha, DbType.DateTime, entity.Rpfhidfecha);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(DateTime rpfhidfecha)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Rpfhidfecha, DbType.DateTime, rpfhidfecha);

            dbProvider.ExecuteNonQuery(command);
        }

        public PsuRpfhidDTO GetById(DateTime rpfhidfecha)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Rpfhidfecha, DbType.DateTime, rpfhidfecha);
            PsuRpfhidDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<PsuRpfhidDTO> List()
        {
            List<PsuRpfhidDTO> entitys = new List<PsuRpfhidDTO>();
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

        public List<PsuRpfhidDTO> GetByCriteria(DateTime fechaInicio, DateTime fechaFin)
        {
            List<PsuRpfhidDTO> entitys = new List<PsuRpfhidDTO>();

            string query = String.Format(helper.SqlGetByCriteria, fechaInicio.ToString(ConstantesBase.FormatoFecha),
                fechaFin.ToString(ConstantesBase.FormatoFecha));

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
