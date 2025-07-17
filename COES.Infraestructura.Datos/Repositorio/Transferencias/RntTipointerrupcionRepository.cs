using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using COES.Dominio.DTO.Transferencias;
using COES.Dominio.Interfaces.Transferencias;
using COES.Base.Core;
using COES.Infraestructura.Datos.Helper.Transferencias;

namespace COES.Infraestructura.Datos.Repositorio.Transferencias
{
    /// <summary>
    /// Clase de acceso a datos de la tabla RNT_TIPO_INTERRUPCION
    /// </summary>
    public class RntTipointerrupcionRepository: RepositoryBase, IRntTipoInterrupcionRepository
    {
        public RntTipointerrupcionRepository(string strConn): base(strConn)
        {
        }

        RntTipoInterrupcionHelper helper = new RntTipoInterrupcionHelper();

        /// <summary>
        /// Registrar Tipo Interrupcion (ById)
        /// </summary>
        public int Save(RntTipoInterrupcionDTO entity)
        {
            int id = GetCodigoGenerado();

            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Tipointcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Tipointnombre, DbType.String, entity.TipoIntNombre);
            dbProvider.AddInParameter(command, helper.Tipointusuariocreacion, DbType.String, entity.TipoIntUsuarioCreacion);
            dbProvider.AddInParameter(command, helper.Tipointfechacreacion, DbType.DateTime, entity.TipoIntFechaCreacion);
            dbProvider.AddInParameter(command, helper.Tipointusuarioupdate, DbType.String, entity.TipoIntUsuarioUpdate);
            dbProvider.AddInParameter(command, helper.Tipointfechaupdate, DbType.DateTime, entity.TipoIntFechaUpdate);
           
            dbProvider.ExecuteNonQuery(command);
           
            return id;
        }

        /// <summary>
        /// Actualizar Tipo Interrupcion (ById)
        /// </summary>
        public void Update(RntTipoInterrupcionDTO entity)
        {           
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Tipointcodi, DbType.Int32, entity.TipoIntCodi);
            dbProvider.AddInParameter(command, helper.Tipointnombre, DbType.String, entity.TipoIntNombre);
            dbProvider.AddInParameter(command, helper.Tipointusuariocreacion, DbType.String, entity.TipoIntUsuarioCreacion);
            dbProvider.AddInParameter(command, helper.Tipointfechacreacion, DbType.DateTime, entity.TipoIntFechaCreacion);
            dbProvider.AddInParameter(command, helper.Tipointusuarioupdate, DbType.String, entity.TipoIntUsuarioUpdate);
            dbProvider.AddInParameter(command, helper.Tipointfechaupdate, DbType.DateTime, entity.TipoIntFechaUpdate);
           
            dbProvider.ExecuteNonQuery(command);   
        }

        /// <summary>
        /// Eliminar Tipo Interrupcion (ById)
        /// En realidad no se elimina, sino se cambia el Estado en la tabla para que no se pueda visualizar
        /// </summary>

        public void Delete(int tipointcodi)
        {           
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);
            dbProvider.AddInParameter(command, helper.Tipointcodi, DbType.Int32, tipointcodi);            
            dbProvider.ExecuteNonQuery(command);
        }

        /// <summary>
        /// Obtener Tipo Interrupcion (ById)
        /// </summary>

        public RntTipoInterrupcionDTO GetById(int tipointcodi)
        {            
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);
            dbProvider.AddInParameter(command, helper.Tipointcodi, DbType.Int32, tipointcodi);
            RntTipoInterrupcionDTO entity = null;
            
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        /// <summary>
        /// Obtener Tipo Interrupcion (List)
        /// </summary>
        public List<RntTipoInterrupcionDTO> List()
        {          
            List<RntTipoInterrupcionDTO> entitys = new List<RntTipoInterrupcionDTO>();
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

        /// <summary>
        /// Obtener Tipo Interrupcion por Criterios
        /// </summary>
        public List<RntTipoInterrupcionDTO> GetByCriteria()
        {
            List<RntTipoInterrupcionDTO> entitys = new List<RntTipoInterrupcionDTO>();
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

        /// <summary>
        /// Obtener Codigo Generado
        /// </summary>
        public int GetCodigoGenerado()
        {            
            int newId = 0;
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlCodigoGenerado);
            newId = Int32.Parse(dbProvider.ExecuteScalar(command).ToString());            
            return newId;
        }

    }
}
