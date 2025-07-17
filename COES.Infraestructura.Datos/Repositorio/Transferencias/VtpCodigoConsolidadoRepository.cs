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
    public class VtpCodigoConsolidadoRepository : RepositoryBase, IVtpCodigoConsolidadoRepository
    {
        public VtpCodigoConsolidadoRepository(string strConn) : base(strConn)
        {
        }

        VtpCodigoConsolidadoHelper helper = new VtpCodigoConsolidadoHelper();

        public VtpCodigoConsolidadoDTO GetByCodigoVTP(string codcncodivtp)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByCodigoVTP);
            dbProvider.AddInParameter(command, helper.CodCnCodiVtp, DbType.String, codcncodivtp);
            VtpCodigoConsolidadoDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }
    }
}
