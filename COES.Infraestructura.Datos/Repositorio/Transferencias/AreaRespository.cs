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
    /// Clase que contiene las operaciones con la base de datos
    /// </summary>
    public class AreaRespository : RepositoryBase, IAreaRepository
    {
      
        public AreaRespository(string strConn) : base(strConn)
        {
        }

        AreaHelper helper = new AreaHelper();

        public AreaDTO GetById(System.Int32 id)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.AreaCodi, DbType.Int32, id);
            AreaDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<AreaDTO> List()
        {
            List<AreaDTO> entitys = new List<AreaDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlList);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }

        public List<AreaDTO> ListArea()
        {
            List<AreaDTO> entitys = new List<AreaDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListArea);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }

        public List<AreaDTO> ListAreaProceso()
        {
            List<AreaDTO> entitys = new List<AreaDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListAreaProceso);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }
        

        public List<AreaDTO> GetByCriteria(string nombre)
        {
            List<AreaDTO> entitys = new List<AreaDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByCriteria);
            dbProvider.AddInParameter(command, helper.AreaNombre, DbType.String, nombre);

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
