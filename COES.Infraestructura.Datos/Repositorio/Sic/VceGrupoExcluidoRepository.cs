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
    /// Clase de acceso a datos de la tabla VCE_GRUPO_EXCLUIDO
    /// </summary>
    public class VceGrupoExcluidoRepository : RepositoryBase, IVceGrupoExcluidoRepository
    {
        public VceGrupoExcluidoRepository(string strConn) : base(strConn)
        {
        }

        VceGrupoExcluidoHelper helper = new VceGrupoExcluidoHelper();

        public int Save(VceGrupoExcluidoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Crgexccodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Pecacodi, DbType.Int32, entity.Pecacodi);
            dbProvider.AddInParameter(command, helper.Grupocodi, DbType.Int32, entity.Grupocodi);
            dbProvider.AddInParameter(command, helper.Crgexcusucreacion, DbType.String, entity.Crgexcusucreacion);
            dbProvider.AddInParameter(command, helper.Crgexcfeccreacion, DbType.DateTime, entity.Crgexcfeccreacion);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }
              

        public void Delete(int pecacodi, int grupocodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Pecacodi, DbType.Int32, pecacodi);
            dbProvider.AddInParameter(command, helper.Grupocodi, DbType.Int32, grupocodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void DeleteByVersion(int pecacodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDeleteByVersion);

            dbProvider.AddInParameter(command, helper.Pecacodi, DbType.Int32, pecacodi);

            dbProvider.ExecuteNonQuery(command);
        }
    }
}
