using COES.Base.Core;
using COES.Dominio.DTO.Sic;
using COES.Dominio.Interfaces.Sic;
using COES.Infraestructura.Datos.Helper.Sic;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Infraestructura.Datos.Repositorio.Sic
{
    

    public class RpfEnergiaPotenciaRepository : RepositoryBase, IRpfEnergiaPotenciaRepository
    {
        public RpfEnergiaPotenciaRepository(string strConn)
            : base(strConn)
        {
        }

        RpfEnergiaPotenciaHelper helper = new RpfEnergiaPotenciaHelper();

        public int Save(RpfEnergiaPotenciaDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Rpfhidfecha, DbType.Date, entity.Rpfhidfecha);
            dbProvider.AddInParameter(command, helper.Rpfenetotal, DbType.Decimal, entity.Rpfenetotal);
            dbProvider.AddInParameter(command, helper.Rpfpotmedia, DbType.Decimal, entity.Rpfpotmedia);
            dbProvider.AddInParameter(command, helper.Eneindhidra, DbType.Decimal, entity.Eneindhidra);
            dbProvider.AddInParameter(command, helper.Potindhidra, DbType.Decimal, entity.Potindhidra);
            dbProvider.AddInParameter(command, helper.Lastuser, DbType.String, entity.Lastuser);
            dbProvider.AddInParameter(command, helper.Lastdate, DbType.DateTime, entity.Lastdate);

            return dbProvider.ExecuteNonQuery(command);
        }

        public void Update(RpfEnergiaPotenciaDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);


            dbProvider.AddInParameter(command, helper.Rpfenetotal, DbType.Decimal, entity.Rpfenetotal);
            dbProvider.AddInParameter(command, helper.Rpfpotmedia, DbType.Decimal, entity.Rpfpotmedia);
            dbProvider.AddInParameter(command, helper.Eneindhidra, DbType.Decimal, entity.Eneindhidra);
            dbProvider.AddInParameter(command, helper.Potindhidra, DbType.Decimal, entity.Potindhidra);
            dbProvider.AddInParameter(command, helper.Lastuser, DbType.String, entity.Lastuser);
            dbProvider.AddInParameter(command, helper.Lastdate, DbType.DateTime, entity.Lastdate);
            dbProvider.AddInParameter(command, helper.Rpfhidfecha, DbType.Date, entity.Rpfhidfecha);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(DateTime fecha)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Rpfhidfecha, DbType.Date, fecha);

            dbProvider.ExecuteNonQuery(command);
        }

        public RpfEnergiaPotenciaDTO GetById(DateTime fecha)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Rpfhidfecha, DbType.Date, fecha);
            RpfEnergiaPotenciaDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<RpfEnergiaPotenciaDTO> List(DateTime fechaini, DateTime fechafin)
        {
            List<RpfEnergiaPotenciaDTO> entitys = new List<RpfEnergiaPotenciaDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlList);

            dbProvider.AddInParameter(command, helper.RpfhidfechaIni, DbType.Date, fechaini);
            dbProvider.AddInParameter(command, helper.RpfhidfechaFin, DbType.Date, fechafin);

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
