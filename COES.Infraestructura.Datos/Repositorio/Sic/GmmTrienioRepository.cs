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
    public class GmmTrienioRepository : RepositoryBase, IGmmTrienioRepository
    {
        public GmmTrienioRepository(string strConn)
            : base(strConn)
        {

        }

        GmmTrienioHelper helper = new GmmTrienioHelper();

         public int Save(GmmTrienioDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);
            dbProvider.AddInParameter(command, helper.Tricodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Empgcodi, DbType.Int32, entity.EMPGCODI);
            dbProvider.AddInParameter(command, helper.Trinuminc, DbType.Int32, entity.TRINUMINC);
            dbProvider.AddInParameter(command, helper.Trisecuencia, DbType.Int32, entity.TRISECUENCIA);
            dbProvider.AddInParameter(command, helper.Trifecinicio, DbType.String, Convert.ToDateTime(entity.TRIFECINICIO).ToString("yyyy-MM-dd"));
            dbProvider.AddInParameter(command, helper.Trifeclimite, DbType.String, Convert.ToDateTime(entity.TRIFECLIMITE).ToString("yyyy-MM-dd"));

            var iRslt = dbProvider.ExecuteNonQuery(command);
            return id;
        }
    }
}
