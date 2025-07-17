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
    public class GmmDetIncumplimientoRepository : RepositoryBase, IGmmDetIncumplimientoRepository
    {
        public GmmDetIncumplimientoRepository(string strConn)
            : base(strConn)
        {

        }

        GmmDetIncumplimientoHelper helper = new GmmDetIncumplimientoHelper();

        public int Save(GmmDetIncumplimientoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Incucodi, DbType.Int32, entity.INCUCODI);
            dbProvider.AddInParameter(command, helper.Tinfcodi, DbType.String, entity.TINFCODI);
            dbProvider.AddInParameter(command, helper.Dincfecrecepcion, DbType.String, Convert.ToDateTime(entity.DINCFECRECEPCION).ToString("yyyy-MM-dd"));
            dbProvider.AddInParameter(command, helper.Dincarchivo, DbType.String, entity.DINCARCHIVO);
            dbProvider.AddInParameter(command, helper.Dinccodi, DbType.Int32, id);
            
            var iRslt = dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Delete(int dinccodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Dinccodi, DbType.Int32, dinccodi); ;

            dbProvider.ExecuteNonQuery(command);
        }

        public List<GmmDetIncumplimientoDTO> ListarArchivos(int incucodi)
        {
            List<GmmDetIncumplimientoDTO> entities = new List<GmmDetIncumplimientoDTO>();
            string queryString = string.Format(helper.SqlListarArchivos, incucodi);

            DbCommand command = dbProvider.GetSqlStringCommand(queryString);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entities.Add(helper.CreateListarArchivos(dr));
                }
            }
            return entities;
        }

        public List<GmmTipInformeDTO> ListarTipoInforme()
        {
            List<GmmTipInformeDTO> entities = new List<GmmTipInformeDTO>();
            string queryString = helper.SqlListarTipoInforme;

            DbCommand command = dbProvider.GetSqlStringCommand(queryString);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entities.Add(helper.CreateListaTipoInforme(dr));
                }
            }
            return entities;
        }
    }
}
