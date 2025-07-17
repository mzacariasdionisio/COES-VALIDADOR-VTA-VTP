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
    /// <summary>
    /// Clase de acceso a datos de la tabla SI_EMPRESAMENSAJE
    /// </summary>
    public class SiEmpresaMensajeRepository : RepositoryBase, ISiEmpresaMensajeRepository
    {
        public SiEmpresaMensajeRepository(string strConn)
            : base(strConn)
        {

        }

        SiEmpresamensajeHelper helper = new SiEmpresamensajeHelper();

        public int GetMaxId()
        {

            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            int id = 1;

            object result = dbProvider.ExecuteScalar(command);
            if (result != null) id = Convert.ToInt32(result);

            return id;
        }

        public void SaveEmpresaMensaje(SiEmpresaMensajeDTO entity, IDbConnection conn, DbTransaction tran)
        {
            DbCommand commandTbEmpresaMensaje = (DbCommand)conn.CreateCommand();
            commandTbEmpresaMensaje.CommandText = helper.SqlSave;
            commandTbEmpresaMensaje.Transaction = tran;
            commandTbEmpresaMensaje.Connection = (DbConnection)conn;

            commandTbEmpresaMensaje.Parameters.Add(dbProvider.CreateParameter(commandTbEmpresaMensaje, helper.Empmsjcodi, DbType.Int32, entity.Empmsjcodi));
            commandTbEmpresaMensaje.Parameters.Add(dbProvider.CreateParameter(commandTbEmpresaMensaje, helper.Msgcodi, DbType.Int32, entity.Msgcodi));
            commandTbEmpresaMensaje.Parameters.Add(dbProvider.CreateParameter(commandTbEmpresaMensaje, helper.Envdetcodi, DbType.Int32, entity.Envdetcodi)); // AQUI SE OBTIENE EL ID DE DEL DETALLE DE ENVIO
            commandTbEmpresaMensaje.Parameters.Add(dbProvider.CreateParameter(commandTbEmpresaMensaje, helper.Emprcodi, DbType.Int32, entity.Emprcodi));
            commandTbEmpresaMensaje.Parameters.Add(dbProvider.CreateParameter(commandTbEmpresaMensaje, helper.Empmsjusucreacion, DbType.String, entity.Empmsjusucreacion));
            commandTbEmpresaMensaje.Parameters.Add(dbProvider.CreateParameter(commandTbEmpresaMensaje, helper.Empmsjfeccreacion, DbType.DateTime, entity.Empmsjfeccreacion));
            commandTbEmpresaMensaje.Parameters.Add(dbProvider.CreateParameter(commandTbEmpresaMensaje, helper.Empmsjusumodificacion, DbType.String, entity.Empmsjusumodificacion));
            commandTbEmpresaMensaje.Parameters.Add(dbProvider.CreateParameter(commandTbEmpresaMensaje, helper.Empmsjfecmodificacion, DbType.DateTime, entity.Empmsjfecmodificacion));

            commandTbEmpresaMensaje.ExecuteNonQuery();
        }

        // ----------------------------------------------- 02-04-2019 ------------------------------------------------------
        // Funcion para eliminar fisicamente el registro de tabla SI_EMPRESAMENSAJE
        // -----------------------------------------------------------------------------------------------------------------
        public void EliminarEmpresaMensajeXEnvDetCodi(int envdetcodi, IDbConnection conn, DbTransaction tran)
        {
            DbCommand commandEliminarEmpresaMensajeXEnvDetCodi = (DbCommand)conn.CreateCommand();
            commandEliminarEmpresaMensajeXEnvDetCodi.CommandText = helper.SqlEliminarEmpresaMensajeXEnvDetCodi;
            commandEliminarEmpresaMensajeXEnvDetCodi.Transaction = tran;
            commandEliminarEmpresaMensajeXEnvDetCodi.Connection = (DbConnection)conn;
            commandEliminarEmpresaMensajeXEnvDetCodi.Parameters.Add(dbProvider.CreateParameter(commandEliminarEmpresaMensajeXEnvDetCodi,
                                                                                         helper.Envdetcodi,
                                                                                         DbType.Int32,
                                                                                         envdetcodi));
            commandEliminarEmpresaMensajeXEnvDetCodi.ExecuteNonQuery();
        }
        // -----------------------------------------------------------------------------------------------------------------

        public SiEmpresaMensajeDTO GetById(int msgcodi, string intercodis)
        {
            SiEmpresaMensajeDTO entity = null;

            string sql = string.Format(helper.SqlGetById, msgcodi, intercodis);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

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
