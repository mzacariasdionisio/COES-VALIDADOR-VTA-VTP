using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Common;
using COES.Dominio.DTO.Sic;
using COES.Dominio.Interfaces.Sic;
using COES.Base.Core;
using COES.Infraestructura.Datos.Helper.Sic;
using System.Data;

namespace COES.Infraestructura.Datos.Repositorio.Sic
{
    public class PrnConfiguracionDiaRepository : RepositoryBase, IPrnConfiguracionDiaRepository
    {
        public PrnConfiguracionDiaRepository(string strConn)
         : base(strConn)
        {
        }

        PrnConfiguracionDiaHelper helper = new PrnConfiguracionDiaHelper();

        public void Delete(int id)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Cnfdiacodi, DbType.Int32, id);
            dbProvider.ExecuteNonQuery(command);
        }

        public void Save(PrnConfiguracionDiaDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);
            dbProvider.AddInParameter(command, helper.Cnfdiacodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Cnfdiafecha, DbType.DateTime, entity.Cnfdiafecha);
            dbProvider.AddInParameter(command, helper.Cnfdiaferiado, DbType.String, entity.Cnfdiaferiado);
            dbProvider.AddInParameter(command, helper.Cnfdiaatipico, DbType.String, entity.Cnfdiaatipico);
            dbProvider.AddInParameter(command, helper.Cnfdiaveda, DbType.String, entity.Cnfdiaveda);
            dbProvider.ExecuteNonQuery(command);
        }

        public List<PrnConfiguracionDiaDTO> List()
        {
            List<PrnConfiguracionDiaDTO> entitys = new List<PrnConfiguracionDiaDTO>();
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

        public List<PrnConfiguracionDiaDTO> ObtenerPorRango(string fechaIni, string fechaFin)
        {
            List<PrnConfiguracionDiaDTO> entitys = new List<PrnConfiguracionDiaDTO>();
            string query = string.Format(helper.SqlObtenerPorRango, fechaIni, fechaFin);
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

        public void Update(PrnConfiguracionDiaDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);            
            dbProvider.AddInParameter(command, helper.Cnfdiafecha, DbType.DateTime, entity.Cnfdiafecha);
            dbProvider.AddInParameter(command, helper.Cnfdiaferiado, DbType.String, entity.Cnfdiaferiado);
            dbProvider.AddInParameter(command, helper.Cnfdiaatipico, DbType.String, entity.Cnfdiaatipico);
            dbProvider.AddInParameter(command, helper.Cnfdiaveda, DbType.String, entity.Cnfdiaveda);
            dbProvider.AddInParameter(command, helper.Cnfdiacodi, DbType.Int32, entity.Cnfdiacodi);
            dbProvider.ExecuteNonQuery(command);
        }
    }
}
