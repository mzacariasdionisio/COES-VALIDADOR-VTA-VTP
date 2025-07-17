using COES.Base.Core;
using COES.Dominio.DTO.Transferencias;
using COES.Dominio.Interfaces.Transferencias;
using COES.Infraestructura.Datos.Helper.Transferencias;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Infraestructura.Datos.Repositorio.Transferencias
{
    /// <summary>
    /// Clase de acceso a datos de la tabla CAI_SDDP_PARAMSEM
    /// </summary>
    public class CaiSddpParamsemRepository : RepositoryBase, ICaiSddpParamsemRepository
    {
        public CaiSddpParamsemRepository(string strConn): base(strConn)
        {
        }

        CaiSddpParamsemHelper helper = new CaiSddpParamsemHelper();

        public int Save(CaiSddpParamsemDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Sddppscodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Caiajcodi, DbType.Int32, entity.Caiajcodi);
            dbProvider.AddInParameter(command, helper.Sddppsnumsem, DbType.Int32, entity.Sddppsnumsem);
            dbProvider.AddInParameter(command, helper.Sddppsdiaini, DbType.DateTime, entity.Sddppsdiaini);
            dbProvider.AddInParameter(command, helper.Sddppsdiafin, DbType.DateTime, entity.Sddppsdiafin);
            dbProvider.AddInParameter(command, helper.Sddppsusucreacion, DbType.String, entity.Sddppsusucreacion);
            dbProvider.AddInParameter(command, helper.Sddppsfeccreacion, DbType.DateTime, entity.Sddppsfeccreacion);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Delete()
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.ExecuteNonQuery(command);
        }

        public List<CaiSddpParamsemDTO> GetByCriteria(int caiajcodi)
        {
            List<CaiSddpParamsemDTO> entitys = new List<CaiSddpParamsemDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByCriteria);

            dbProvider.AddInParameter(command, helper.Caiajcodi, DbType.Int32, caiajcodi);

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
