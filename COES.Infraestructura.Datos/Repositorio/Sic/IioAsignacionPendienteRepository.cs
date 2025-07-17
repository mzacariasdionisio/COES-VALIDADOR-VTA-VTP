// =======================================================================================
//
// (c) COES Sinac. Todos los derechos reservados. 
//
// Autor: Henry Manuel Díaz Tuesta
// Acronimo: HDT
// Requerimiento: alpha
//
// Fecha creacion: 26/10/2016
// Descripcion: Archivo para la atencion del requerimiento.
//
// Historial de cambios:
// 
// Correlativo	Fecha		Requerimiento		Comentario
//
// =======================================================================================

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

    public class IioAsignacionPendienteRepository : RepositoryBase, IIioAsignacionPendienteRepository
    {

        IioAsignacionPendienteHelper helper = new IioAsignacionPendienteHelper();

        public IioAsignacionPendienteRepository(string strConn)
            : base(strConn)
        {
        }

        public int Save(IioAsignacionPendienteDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Mapencodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Mapenentidad, DbType.String, entity.Mapenentidad);
            dbProvider.AddInParameter(command, helper.Mapencodigo, DbType.String, entity.Mapencodigo);
            if (!string.IsNullOrEmpty(entity.Mapendescripcion))
            {
                dbProvider.AddInParameter(command, helper.Mapendescripcion, DbType.String, entity.Mapendescripcion);
            }
            else
            {
                dbProvider.AddInParameter(command, helper.Mapendescripcion, DbType.String, DBNull.Value);
            }
            dbProvider.AddInParameter(command, helper.Mapenestado, DbType.Int32, entity.Mapenestado);
            if (!string.IsNullOrEmpty(entity.Mapenindicacionestado))
            {
                dbProvider.AddInParameter(command, helper.Mapenindicacionestado, DbType.String, entity.Mapenindicacionestado);
            }
            else 
            {
                dbProvider.AddInParameter(command, helper.Mapenindicacionestado, DbType.String, DBNull.Value);
            }            
            dbProvider.AddInParameter(command, helper.Mapenestregistro, DbType.String, entity.Mapenestregistro);
            dbProvider.AddInParameter(command, helper.Mapenusucreacion, DbType.String, entity.Mapenusucreacion);
            dbProvider.AddInParameter(command, helper.Mapenfeccreacion, DbType.DateTime, entity.Mapenfeccreacion);
            dbProvider.AddInParameter(command, helper.Mapenusumodificacion, DbType.String, entity.Mapenusumodificacion);
            if (entity.Mapenfecmodificacion.HasValue)
            {
                dbProvider.AddInParameter(command, helper.Mapenfecmodificacion, DbType.DateTime, entity.Mapenfecmodificacion.Value);
            }
            else
            {
                dbProvider.AddInParameter(command, helper.Mapenfecmodificacion, DbType.DateTime, DBNull.Value);
            }            

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(IioAsignacionPendienteDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Mapenentidad, DbType.String, entity.Mapenentidad);
            dbProvider.AddInParameter(command, helper.Mapencodigo, DbType.String, entity.Mapencodigo);
            if (!string.IsNullOrEmpty(entity.Mapendescripcion))
            {
                dbProvider.AddInParameter(command, helper.Mapendescripcion, DbType.String, entity.Mapendescripcion);
            }
            else
            {
                dbProvider.AddInParameter(command, helper.Mapendescripcion, DbType.String, DBNull.Value);
            }
            dbProvider.AddInParameter(command, helper.Mapenestado, DbType.Int32, entity.Mapenestado);
            if (!string.IsNullOrEmpty(entity.Mapenindicacionestado))
            {
                dbProvider.AddInParameter(command, helper.Mapenindicacionestado, DbType.String, entity.Mapenindicacionestado);
            }
            else
            {
                dbProvider.AddInParameter(command, helper.Mapenindicacionestado, DbType.String, DBNull.Value);
            }    
            dbProvider.AddInParameter(command, helper.Mapenestregistro, DbType.String, entity.Mapenestregistro);
            dbProvider.AddInParameter(command, helper.Mapenusucreacion, DbType.String, entity.Mapenusucreacion);
            dbProvider.AddInParameter(command, helper.Mapenfeccreacion, DbType.DateTime, entity.Mapenusucreacion);
            dbProvider.AddInParameter(command, helper.Mapenusumodificacion, DbType.String, entity.Mapenusumodificacion);
            if (entity.Mapenfecmodificacion.HasValue)
            {
                dbProvider.AddInParameter(command, helper.Mapenfecmodificacion, DbType.DateTime, entity.Mapenfecmodificacion.Value);
            }
            else
            {
                dbProvider.AddInParameter(command, helper.Mapenfecmodificacion, DbType.DateTime, DBNull.Value);
            }
            dbProvider.AddInParameter(command, helper.Mapencodi, DbType.Int32, entity.Mapencodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int mapencodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Mapencodi, DbType.Int32, mapencodi);

            dbProvider.ExecuteNonQuery(command);
        }


        public List<IioAsignacionPendienteDTO> ListByCreationDate(string fechaHoraInicioSincronizacion)
        {
            var entities = new List<IioAsignacionPendienteDTO>();
            string mapenFecCreacion = "TO_DATE('" + fechaHoraInicioSincronizacion + "', 'dd/mm/yyyy hh24:mi:ss')";
            string strComando = string.Format(helper.SqlListByCreationDate, mapenFecCreacion);

            DbCommand command = dbProvider.GetSqlStringCommand(strComando);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    var oAsignacionPendienteDTO = new IioAsignacionPendienteDTO();
                    oAsignacionPendienteDTO = helper.Create(dr);
                    entities.Add(oAsignacionPendienteDTO);
                }
            }
            return entities;
        }

    }

}
