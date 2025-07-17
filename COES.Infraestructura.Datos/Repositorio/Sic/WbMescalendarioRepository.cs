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
    /// Clase de acceso a datos de la tabla WB_MESCALENDARIO
    /// </summary>
    public class WbMescalendarioRepository: RepositoryBase, IWbMescalendarioRepository
    {
        public WbMescalendarioRepository(string strConn): base(strConn)
        {
        }

        WbMescalendarioHelper helper = new WbMescalendarioHelper();

        public int Save(WbMescalendarioDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Mescalcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Mescalcolorsat, DbType.String, entity.Mescalcolorsat);
            dbProvider.AddInParameter(command, helper.Mescalmes, DbType.Int32, entity.Mescalmes);
            dbProvider.AddInParameter(command, helper.Mescalanio, DbType.Int32, entity.Mescalanio);
            dbProvider.AddInParameter(command, helper.Mescalcolorsun, DbType.String, entity.Mescalcolorsun);
            dbProvider.AddInParameter(command, helper.Mescalcolor, DbType.String, entity.Mescalcolor);
            dbProvider.AddInParameter(command, helper.Mescalinfo, DbType.String, entity.Mescalinfo);
            dbProvider.AddInParameter(command, helper.Mescaltitulo, DbType.String, entity.Mescaltitulo);
            dbProvider.AddInParameter(command, helper.Mescaldescripcion, DbType.String, entity.Mescaldescripcion);
            dbProvider.AddInParameter(command, helper.Mescalestado, DbType.String, entity.Mescalestado);
            dbProvider.AddInParameter(command, helper.Mescalcolortit, DbType.String, entity.Mescalcolortit);
            dbProvider.AddInParameter(command, helper.Mescalcolorsubtit, DbType.String, entity.Mescalcolorsubtit);
            dbProvider.AddInParameter(command, helper.Mescalusumodificacion, DbType.String, entity.Mescalusumodificacion);
            dbProvider.AddInParameter(command, helper.Mescalfecmodificacion, DbType.DateTime, entity.Mescalfecmodificacion);
            dbProvider.AddInParameter(command, helper.Mesdiacolor, DbType.String, entity.Mesdiacolor);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(WbMescalendarioDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Mescalcolorsat, DbType.String, entity.Mescalcolorsat);
            dbProvider.AddInParameter(command, helper.Mescalmes, DbType.Int32, entity.Mescalmes);
            dbProvider.AddInParameter(command, helper.Mescalanio, DbType.Int32, entity.Mescalanio);
            dbProvider.AddInParameter(command, helper.Mescalcolorsun, DbType.String, entity.Mescalcolorsun);
            dbProvider.AddInParameter(command, helper.Mescalcolor, DbType.String, entity.Mescalcolor);            
            dbProvider.AddInParameter(command, helper.Mescaltitulo, DbType.String, entity.Mescaltitulo);
            dbProvider.AddInParameter(command, helper.Mescaldescripcion, DbType.String, entity.Mescaldescripcion);
            dbProvider.AddInParameter(command, helper.Mescalestado, DbType.String, entity.Mescalestado);
            dbProvider.AddInParameter(command, helper.Mescalcolortit, DbType.String, entity.Mescalcolortit);
            dbProvider.AddInParameter(command, helper.Mescalcolorsubtit, DbType.String, entity.Mescalcolorsubtit);
            dbProvider.AddInParameter(command, helper.Mescalusumodificacion, DbType.String, entity.Mescalusumodificacion);
            dbProvider.AddInParameter(command, helper.Mescalfecmodificacion, DbType.DateTime, entity.Mescalfecmodificacion);
            dbProvider.AddInParameter(command, helper.Mesdiacolor, DbType.String, entity.Mesdiacolor);
            dbProvider.AddInParameter(command, helper.Mescalcodi, DbType.Int32, entity.Mescalcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void QuitarImagen(int mescalcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlQuitarImagen);

            dbProvider.AddInParameter(command, helper.Mescalcodi, DbType.Int32, mescalcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void ActualizarInfografia(int id, string archivo)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlActualizarInfografia);

            dbProvider.AddInParameter(command, helper.Mescalinfo, DbType.String, archivo);
            dbProvider.AddInParameter(command, helper.Mescalcodi, DbType.Int32, id);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int mescalcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Mescalcodi, DbType.Int32, mescalcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public WbMescalendarioDTO GetById(int mescalcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Mescalcodi, DbType.Int32, mescalcodi);
            WbMescalendarioDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<WbMescalendarioDTO> List()
        {
            List<WbMescalendarioDTO> entitys = new List<WbMescalendarioDTO>();
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

        public List<WbMescalendarioDTO> GetByCriteria(int? anio, int? mes)
        {
            List<WbMescalendarioDTO> entitys = new List<WbMescalendarioDTO>();
            string query = string.Format(helper.SqlGetByCriteria, anio, mes);
            DbCommand command = dbProvider.GetSqlStringCommand(query);            

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
