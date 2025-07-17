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
    /// Clase de acceso a datos de la tabla WB_PROVEEDOR
    /// </summary>
    public class WbProveedorRepository: RepositoryBase, IWbProveedorRepository
    {
        public WbProveedorRepository(string strConn): base(strConn)
        {
        }

        WbProveedorHelper helper = new WbProveedorHelper();

        public int Save(WbProveedorDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Provcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Provnombre, DbType.String, entity.Provnombre);
            dbProvider.AddInParameter(command, helper.Provtipo, DbType.String, entity.Provtipo);
            dbProvider.AddInParameter(command, helper.Provfechainscripcion, DbType.DateTime, entity.Provfechainscripcion);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(WbProveedorDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Provcodi, DbType.Int32, entity.Provcodi);
            dbProvider.AddInParameter(command, helper.Provnombre, DbType.String, entity.Provnombre);
            dbProvider.AddInParameter(command, helper.Provtipo, DbType.String, entity.Provtipo);
            dbProvider.AddInParameter(command, helper.Provfechainscripcion, DbType.DateTime, entity.Provfechainscripcion);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int provcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Provcodi, DbType.Int32, provcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public WbProveedorDTO GetById(int provcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Provcodi, DbType.Int32, provcodi);
            WbProveedorDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<WbProveedorDTO> List()
        {
            List<WbProveedorDTO> entitys = new List<WbProveedorDTO>();
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

        public List<WbProveedorDTO> GetByCriteria(string nombre, string tipo, DateTime? fechaD, DateTime? fechaH)
        {
            List<WbProveedorDTO> entitys = new List<WbProveedorDTO>();            

            string desde = (fechaD != null) ? ((DateTime)fechaD).ToString(ConstantesBase.FormatoFecha) : 
                DateTime.MinValue.ToString(ConstantesBase.FormatoFecha);
            string hasta = (fechaH != null) ? ((DateTime)fechaH).ToString(ConstantesBase.FormatoFecha) : 
                DateTime.MaxValue.ToString(ConstantesBase.FormatoFecha);
                        string query = String.Format(helper.SqlGetByCriteria,nombre, tipo, desde, hasta);

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

        public List<string> GetByCriteriaTipo()
        {
            List<string> list = new List<string>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListarTipoProveedor);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    int iProvtipo = dr.GetOrdinal(helper.Provtipo);
                    if (!dr.IsDBNull(iProvtipo)) list.Add(dr.GetString(iProvtipo));
                }
            }

            return list;
        }
    }
}
