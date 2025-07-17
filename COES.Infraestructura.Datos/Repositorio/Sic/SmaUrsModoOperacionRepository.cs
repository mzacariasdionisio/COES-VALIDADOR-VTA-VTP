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
    /// Clase de acceso a datos de la tabla SMA_URS_MODO_OPERACION
    /// </summary>
    public class SmaUrsModoOperacionRepository: RepositoryBase, ISmaUrsModoOperacionRepository
    {
        public SmaUrsModoOperacionRepository(string strConn): base(strConn)
        {
        }

        SmaUrsModoOperacionHelper helper = new SmaUrsModoOperacionHelper();       

        public List<SmaUrsModoOperacionDTO> List()
        {
            List<SmaUrsModoOperacionDTO> entitys = new List<SmaUrsModoOperacionDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlList);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.CreateList(dr));
                }
            }

            return entitys;
        }

        public List<SmaUrsModoOperacionDTO> ListUrs(int usercode)
        {
            List<SmaUrsModoOperacionDTO> entitys = new List<SmaUrsModoOperacionDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListUrs);
            dbProvider.AddInParameter(command, helper.Usercode, DbType.Int32, usercode);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.CreateList(dr));
                }
            }

            return entitys;
        }

        public List<SmaUrsModoOperacionDTO> ListInUrs(int usercode)
        {
            List<SmaUrsModoOperacionDTO> entitys = new List<SmaUrsModoOperacionDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListInUrs);
            dbProvider.AddInParameter(command, helper.Usercode, DbType.Int32, usercode);
            dbProvider.AddInParameter(command, helper.Usercode, DbType.Int32, usercode);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.CreateList(dr));
                }
            }

            return entitys;
        }

        public List<SmaUrsModoOperacionDTO> ListMO(int urscodi)
        {
            List<SmaUrsModoOperacionDTO> entitys = new List<SmaUrsModoOperacionDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListMO);
            dbProvider.AddInParameter(command, helper.Urscodi, DbType.Int32, urscodi);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }

        public List<SmaUrsModoOperacionDTO> GetByCriteria()
        {
            List<SmaUrsModoOperacionDTO> entitys = new List<SmaUrsModoOperacionDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByCriteria);

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
