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
    /// Clase de acceso a datos de la tabla CM_REPORTEDETALLE
    /// </summary>
    public class CmReportedetalleRepository: RepositoryBase, ICmReportedetalleRepository
    {
        public CmReportedetalleRepository(string strConn): base(strConn)
        {
        }

        CmReportedetalleHelper helper = new CmReportedetalleHelper();

        public int ObtenerMaxId()
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            return id;
        }

        public int Save(CmReportedetalleDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);
                        
            dbProvider.AddInParameter(command, helper.Cmrepcodi, DbType.Int32, entity.Cmrepcodi);
            dbProvider.AddInParameter(command, helper.Barrcodi, DbType.Int32, entity.Barrcodi);
            dbProvider.AddInParameter(command, helper.Cmredefecha, DbType.DateTime, entity.Cmredefecha);
            dbProvider.AddInParameter(command, helper.Cmredeperiodo, DbType.Int32, entity.Cmredeperiodo);
            dbProvider.AddInParameter(command, helper.Cmredecmtotal, DbType.Decimal, entity.Cmredecmtotal);
            dbProvider.AddInParameter(command, helper.Cmredecmenergia, DbType.Decimal, entity.Cmredecmenergia);
            dbProvider.AddInParameter(command, helper.Cmredecongestion, DbType.Decimal, entity.Cmredecongestion);
            dbProvider.AddInParameter(command, helper.Cmredevaltotal, DbType.Int32, entity.Cmredevaltotal);
            dbProvider.AddInParameter(command, helper.Cmredevalenergia, DbType.Int32, entity.Cmredevalenergia);
            dbProvider.AddInParameter(command, helper.Cmredevalcongestion, DbType.Int32, entity.Cmredevalcongestion);
            dbProvider.AddInParameter(command, helper.Cmredetiporeporte, DbType.String, entity.Cmredetiporeporte);
            dbProvider.AddInParameter(command, helper.Cmredecodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Barrcodi2, DbType.Int32, entity.Barrcodi2);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(CmReportedetalleDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Cmrepcodi, DbType.Int32, entity.Cmrepcodi);
            dbProvider.AddInParameter(command, helper.Barrcodi, DbType.Int32, entity.Barrcodi);
            dbProvider.AddInParameter(command, helper.Cmredefecha, DbType.DateTime, entity.Cmredefecha);
            dbProvider.AddInParameter(command, helper.Cmredeperiodo, DbType.Int32, entity.Cmredeperiodo);
            dbProvider.AddInParameter(command, helper.Cmredecmtotal, DbType.Decimal, entity.Cmredecmtotal);
            dbProvider.AddInParameter(command, helper.Cmredecmenergia, DbType.Decimal, entity.Cmredecmenergia);
            dbProvider.AddInParameter(command, helper.Cmredecongestion, DbType.Decimal, entity.Cmredecongestion);
            dbProvider.AddInParameter(command, helper.Cmredevaltotal, DbType.Int32, entity.Cmredevaltotal);
            dbProvider.AddInParameter(command, helper.Cmredevalenergia, DbType.Int32, entity.Cmredevalenergia);
            dbProvider.AddInParameter(command, helper.Cmredevalcongestion, DbType.Int32, entity.Cmredevalcongestion);
            dbProvider.AddInParameter(command, helper.Cmredetiporeporte, DbType.String, entity.Cmredetiporeporte);
            dbProvider.AddInParameter(command, helper.Barrcodi2, DbType.Int32, entity.Barrcodi2);
            dbProvider.AddInParameter(command, helper.Cmredecodi, DbType.Int32, entity.Cmredecodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int cmredecodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Cmredecodi, DbType.Int32, cmredecodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public CmReportedetalleDTO GetById(int cmredecodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Cmredecodi, DbType.Int32, cmredecodi);
            CmReportedetalleDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<CmReportedetalleDTO> List()
        {
            List<CmReportedetalleDTO> entitys = new List<CmReportedetalleDTO>();
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

        public List<CmReportedetalleDTO> GetByCriteria(int idReporte, string tipo)
        {
            List<CmReportedetalleDTO> entitys = new List<CmReportedetalleDTO>();
            string sql = string.Format(helper.SqlGetByCriteria, idReporte, tipo);

            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    CmReportedetalleDTO entity = helper.Create(dr);

                    int iBarrnombre = dr.GetOrdinal(helper.Barrnombre);
                    if (!dr.IsDBNull(iBarrnombre)) entity.Barrnombre = dr.GetString(iBarrnombre);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public void GrabarDatosBulkResult(List<CmReportedetalleDTO> entitys)
        {
            #region AddColumnMapping

            dbProvider.AddColumnMapping(helper.Cmredecodi, DbType.Int32);
            dbProvider.AddColumnMapping(helper.Cmrepcodi, DbType.Int32);
            dbProvider.AddColumnMapping(helper.Barrcodi, DbType.Int32);
            dbProvider.AddColumnMapping(helper.Cmredefecha, DbType.DateTime);
            dbProvider.AddColumnMapping(helper.Cmredeperiodo, DbType.Int32);
            dbProvider.AddColumnMapping(helper.Cmredecmtotal, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Cmredecmenergia, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Cmredecongestion, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Cmredevaltotal, DbType.Int32);
            dbProvider.AddColumnMapping(helper.Cmredevalenergia, DbType.Int32);
            dbProvider.AddColumnMapping(helper.Cmredevalcongestion, DbType.Int32);
            dbProvider.AddColumnMapping(helper.Cmredetiporeporte, DbType.String);
            dbProvider.AddColumnMapping(helper.Barrcodi2, DbType.Int32);


            #endregion

            dbProvider.BulkInsert<CmReportedetalleDTO>(entitys, helper.NombreTabla);
        }

        public List<CmReportedetalleDTO> ObtenerReporte(DateTime fechaInicio, DateTime fechaFin)
        {
            List<CmReportedetalleDTO> entitys = new List<CmReportedetalleDTO>();
            string sql = string.Format(helper.SqlObtenerReporte, fechaInicio.ToString(ConstantesBase.FormatoFecha), 
                fechaFin.ToString(ConstantesBase.FormatoFecha));

            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    CmReportedetalleDTO entity = helper.Create(dr);

                    int iBarrnombre = dr.GetOrdinal(helper.Barrnombre);
                    if (!dr.IsDBNull(iBarrnombre)) entity.Barrnombre = dr.GetString(iBarrnombre);

                    int iBarrnombre2 = dr.GetOrdinal(helper.Barrnombre2);
                    if (!dr.IsDBNull(iBarrnombre2)) entity.Barrnombre2 = dr.GetString(iBarrnombre2);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }
    }
}
