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
    /// Clase de acceso a datos de la tabla SMA_USER_EMPRESA
    /// </summary>
    public class SmaUserEmpresaRepository: RepositoryBase, ISmaUserEmpresaRepository
    {
        public SmaUserEmpresaRepository(string strConn): base(strConn)
        {
        }

        SmaUserEmpresaHelper helper = new SmaUserEmpresaHelper();

        public List<SmaUserEmpresaDTO> List(int codigoEmpresa)
        {
            List<SmaUserEmpresaDTO> entitys = new List<SmaUserEmpresaDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlList);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, codigoEmpresa);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }

        public List<SmaUserEmpresaDTO> ListEmpresa(int codigoUser)
        {
            List<SmaUserEmpresaDTO> entitys = new List<SmaUserEmpresaDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListEmpresa);
            dbProvider.AddInParameter(command, helper.Usercode, DbType.Int32, codigoUser);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }


        public List<SmaUserEmpresaDTO> GetByCriteria(int codigoEmpresa)
        {
            List<SmaUserEmpresaDTO> entitys = new List<SmaUserEmpresaDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByCriteria);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, codigoEmpresa);

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
