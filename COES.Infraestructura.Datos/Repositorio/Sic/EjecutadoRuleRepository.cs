using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using COES.Dominio.DTO.Sic;
using COES.Dominio.Interfaces.Scada;
using COES.Base.Core;
using COES.Infraestructura.Datos.Helper.Sic;

namespace COES.Infraestructura.Datos.Repositorio.Sic
{
    public class EjecutadoRuleRepository : RepositoryBase
    {
        public EjecutadoRuleRepository(string strConn)
            : base(strConn)
        {

        }

        EjecutadoRuleHelper helper = new EjecutadoRuleHelper();


        public List<EjecutadoRuleDTO> ListarFormulaPorUsuario(string usuario)
        {
            List<EjecutadoRuleDTO> entitys = new List<EjecutadoRuleDTO>();

            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlList);
            dbProvider.AddInParameter(command, helper.EJRULASTUSER, DbType.String, usuario);
            
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    EjecutadoRuleDTO entity = helper.Create(dr);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }
    }
}
