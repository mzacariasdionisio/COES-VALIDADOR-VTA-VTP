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
    public class GmmEstadoEmpresaRepository : RepositoryBase, IGmmEstadoEmpresaRepository
    {
        public GmmEstadoEmpresaRepository(string strConn)
            : base(strConn)
        {

        }

        GmmEstadoEmpresaHelper helper = new GmmEstadoEmpresaHelper();

        public int Save(GmmEstadoEmpresaDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Estcodi, DbType.Int32, id);
            //dbProvider.AddInParameter(command, helper.Estfecregistro, DbType.DateTime, entity.ESTFECREGISTRO);
            dbProvider.AddInParameter(command, helper.Estestado, DbType.String, entity.ESTESTADO);
            dbProvider.AddInParameter(command, helper.Estusuedicion, DbType.String, entity.ESTUSUEDICION);
            dbProvider.AddInParameter(command, helper.Empgcodi, DbType.Int32, entity.EMPGCODI);

            var iRslt = dbProvider.ExecuteNonQuery(command);
            return id;
        }
    }
}
