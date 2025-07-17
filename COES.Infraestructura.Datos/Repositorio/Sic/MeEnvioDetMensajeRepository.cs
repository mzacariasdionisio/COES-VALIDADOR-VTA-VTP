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
    /// Clase de acceso a datos de la tabla ME_ENVIODETMENSAJE
    /// </summary>
    public class MeEnvioDetMensajeRepository : RepositoryBase, IMeEnvioDetMensajeRepository
    {
        public MeEnvioDetMensajeRepository(string strConn)
            : base(strConn)
        {

        }

        MeEnviodetmensajeHelper helper = new MeEnviodetmensajeHelper();

        public int GetMaxId()
        {

            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            int id = 1;

            object result = dbProvider.ExecuteScalar(command);
            if (result != null) id = Convert.ToInt32(result);

            return id;
        }

        public void SaveEnvioDetMensaje(MeEnvioDetMensajeDTO entity, IDbConnection conn, DbTransaction tran)
        {
            DbCommand commandTbEnvioDetalleMensaje = (DbCommand)conn.CreateCommand();
            commandTbEnvioDetalleMensaje.CommandText = helper.SqlSave;
            commandTbEnvioDetalleMensaje.Transaction = tran;
            commandTbEnvioDetalleMensaje.Connection = (DbConnection)conn;

            commandTbEnvioDetalleMensaje.Parameters.Add(dbProvider.CreateParameter(commandTbEnvioDetalleMensaje, helper.Edtmsjcodi, DbType.Int32, entity.Edtmsjcodi));
            commandTbEnvioDetalleMensaje.Parameters.Add(dbProvider.CreateParameter(commandTbEnvioDetalleMensaje, helper.Msgcodi, DbType.Int32, entity.Msgcodi));
            commandTbEnvioDetalleMensaje.Parameters.Add(dbProvider.CreateParameter(commandTbEnvioDetalleMensaje, helper.Envdetcodi, DbType.Int32, entity.Envdetcodi));
            commandTbEnvioDetalleMensaje.Parameters.Add(dbProvider.CreateParameter(commandTbEnvioDetalleMensaje, helper.Edtmsjusucreacion, DbType.String, entity.Edtmsjusucreacion));
            commandTbEnvioDetalleMensaje.Parameters.Add(dbProvider.CreateParameter(commandTbEnvioDetalleMensaje, helper.Edtmsjfeccreacion, DbType.DateTime, entity.Edtmsjfeccreacion));
            commandTbEnvioDetalleMensaje.Parameters.Add(dbProvider.CreateParameter(commandTbEnvioDetalleMensaje, helper.Edtmsjusumodificacion, DbType.String, entity.Edtmsjusumodificacion));
            commandTbEnvioDetalleMensaje.Parameters.Add(dbProvider.CreateParameter(commandTbEnvioDetalleMensaje, helper.Edtmsjfecmodificacion, DbType.DateTime, entity.Edtmsjfecmodificacion));

            commandTbEnvioDetalleMensaje.ExecuteNonQuery();
        }

        // ----------------------------------------------- 02-04-2019 ------------------------------------------------------
        // Funcion para eliminar fisicamente el registro de tabla ME_ENVIODETMENSAJE
        // -----------------------------------------------------------------------------------------------------------------
        public void EliminarEnvDetMsgXEnvDetCodi(int envdetcodi, IDbConnection conn, DbTransaction tran)
        {
            DbCommand commandEliminarEnvioDetalleMensaje = (DbCommand)conn.CreateCommand();
            commandEliminarEnvioDetalleMensaje.CommandText = helper.SqlEliminarEnvDetMsgXEnvDetCodi;
            commandEliminarEnvioDetalleMensaje.Transaction = tran;
            commandEliminarEnvioDetalleMensaje.Connection = (DbConnection)conn;
            commandEliminarEnvioDetalleMensaje.Parameters.Add(dbProvider.CreateParameter(commandEliminarEnvioDetalleMensaje,
                                                                                         helper.Envdetcodi,
                                                                                         DbType.Int32,
                                                                                         envdetcodi));
            commandEliminarEnvioDetalleMensaje.ExecuteNonQuery();
        }
        // -----------------------------------------------------------------------------------------------------------------
    }
}
