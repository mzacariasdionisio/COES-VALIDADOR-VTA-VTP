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
    public class SiSolicitudAmpliacionRepository : RepositoryBase, ISiSolicitudAmpliacionRepository
    {
        public SiSolicitudAmpliacionRepository(string strConn) : base(strConn)
        {
        }

        SiSolicitudAmpliacionHelper helper = new SiSolicitudAmpliacionHelper();

        public int Save(string fecha, int msgcodi, int emprcodi, string fechaplazo, string usuario, string fechaAct, int formatcodi, int fdatcodi, int flag)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);
            
            string query = string.Format(helper.SqlSave, id, msgcodi, fecha, emprcodi, fechaplazo, usuario, fechaAct, formatcodi, fdatcodi, flag);
            command = dbProvider.GetSqlStringCommand(query);
            
            dbProvider.ExecuteReader(command);
            
            return id;
        }

        public SiSolicitudAmpliacionDTO GetByMsgCodi(int MsgCodi)
        {
            SiSolicitudAmpliacionDTO entity = new SiSolicitudAmpliacionDTO();

            

            string query = string.Format(helper.SqlGetByMsgCodi, MsgCodi);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity =helper.Create(dr);
                }
            }

            return entity;
        }

    }
}
