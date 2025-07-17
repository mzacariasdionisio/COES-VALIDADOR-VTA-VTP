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
   public  class TipoUsuarioRespository : RepositoryBase, ITipoUsuarioRepository
    {
       public TipoUsuarioRespository(string strConn)
            : base(strConn)
        {
        }

       TipoUsuarioHelper helper = new TipoUsuarioHelper();

       public int Save(TipoUsuarioDTO entity)
       {
           DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlSave);

           dbProvider.AddInParameter(command, helper.TIPOUSUACODI, DbType.Int32, GetCodigoGenerado());
           dbProvider.AddInParameter(command, helper.TIPOUSUANOMBRE, DbType.String, entity.TipoUsuaNombre);
           dbProvider.AddInParameter(command, helper.TIPOUSUAESTADO, DbType.String, entity.TipoUsuaEstado);
           dbProvider.AddInParameter(command, helper.TIPOUSUAUSERNAME, DbType.String, entity.TipoUsuaUserName);
           dbProvider.AddInParameter(command, helper.TIPOUSUAFECINS, DbType.DateTime, DateTime.Now);

           return dbProvider.ExecuteNonQuery(command);
       }

       public void Update(TipoUsuarioDTO entity)
       {
           DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

           dbProvider.AddInParameter(command, helper.TIPOUSUANOMBRE, DbType.String, entity.TipoUsuaNombre);
           dbProvider.AddInParameter(command, helper.TIPOUSUAESTADO, DbType.String, entity.TipoUsuaEstado);
           dbProvider.AddInParameter(command, helper.TIPOUSUAFECACT, DbType.DateTime, DateTime.Now);
           dbProvider.AddInParameter(command, helper.TIPOUSUACODI, DbType.Int32, entity.TipoUsuaCodi);

           dbProvider.ExecuteNonQuery(command);
       }

       public void Delete(System.Int32 id)
       {
           DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);
          
           dbProvider.AddInParameter(command, helper.TIPOUSUACODI, DbType.Int32, id);

           dbProvider.ExecuteNonQuery(command);
       }

       public TipoUsuarioDTO GetById(System.Int32 id)
       {
           DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

           dbProvider.AddInParameter(command, helper.TIPOUSUACODI, DbType.Int32, id);
           TipoUsuarioDTO entity = null;

           using (IDataReader dr = dbProvider.ExecuteReader(command))
           {
               if (dr.Read())
               {
                   entity = helper.Create(dr);
               }
           }

           return entity;
       }

       public List<TipoUsuarioDTO> List()
       {
           List<TipoUsuarioDTO> entitys = new List<TipoUsuarioDTO>();
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

       public List<TipoUsuarioDTO> GetByCriteria(string nombre)
       {
           List<TipoUsuarioDTO> entitys = new List<TipoUsuarioDTO>();
           DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByCriteria);
           dbProvider.AddInParameter(command, helper.TIPOUSUANOMBRE, DbType.String, nombre);

           using (IDataReader dr = dbProvider.ExecuteReader(command))
           {
               while (dr.Read())
               {
                   entitys.Add(helper.Create(dr));
               }
           }

           return entitys;
       }

       public int GetCodigoGenerado()
       {
           int newId = 0;
           DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlCodigoGenerado);
           newId = Int32.Parse(dbProvider.ExecuteScalar(command).ToString());

           return newId;
       }
    }
}
