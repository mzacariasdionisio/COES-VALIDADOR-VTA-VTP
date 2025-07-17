using COES.Base.Core;
using COES.Dominio.DTO.Sic;
using COES.Dominio.Interfaces.Sic;
using COES.Infraestructura.Datos.Helper.Sic;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Infraestructura.Datos.Repositorio.Sic
{
    public class DesviacionRepository : RepositoryBase, IDesviacionRepository
    {
        public DesviacionRepository(string strConn)
            : base(strConn)
        {
        }
        DesviacionHelper helper = new DesviacionHelper();

        public int Save(DesviacionDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Lectcodi, DbType.Int32, entity.Lectcodi);
            dbProvider.AddInParameter(command, helper.Desvfecha, DbType.DateTime, entity.Desvfecha);
            dbProvider.AddInParameter(command, helper.Ptomedicodi, DbType.Int32, entity.Ptomedicodi);
            dbProvider.AddInParameter(command, helper.Medorigdesv, DbType.Int32, entity.Medorigdesv);
            dbProvider.AddInParameter(command, helper.Lastuser, DbType.String, entity.Lastuser);

            return dbProvider.ExecuteNonQuery(command);
        }              

        public List<DesviacionDTO> ListarDesviacion(DateTime fecha)
        {
            List<DesviacionDTO> entitys = new List<DesviacionDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlList);
            dbProvider.AddInParameter(command, helper.Desvfecha, DbType.DateTime, fecha);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    if (helper.Create(dr).Ptomedicodi != 0)
                    {
                        DesviacionDTO entity = helper.Create(dr);

                        int iPtomedidesc = dr.GetOrdinal(helper.Ptomedidesc);
                        if (!dr.IsDBNull(iPtomedidesc)) entity.Ptomedidesc = dr.GetString(iPtomedidesc);

                        entitys.Add(entity);
                    }
                }
            }

            return entitys;
        }

        public void Delete(DateTime fecha)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);
            dbProvider.AddInParameter(command, helper.Desvfecha, DbType.Date, fecha);
            dbProvider.ExecuteNonQuery(command);
        }
    }
}
