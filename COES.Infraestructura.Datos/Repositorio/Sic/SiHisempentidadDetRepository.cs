using COES.Base.Core;
using COES.Dominio.DTO.Sic;
using COES.Dominio.Interfaces.Sic;
using COES.Infraestructura.Datos.Helper.Sic;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace COES.Infraestructura.Datos.Repositorio.Sic
{
    /// <summary>
    /// Clase de acceso a datos de la tabla SI_HISEMPENTIDAD_DET
    /// </summary>
    public class SiHisempentidadDetRepository : RepositoryBase, ISiHisempentidadDetRepository
    {
        public SiHisempentidadDetRepository(string strConn) : base(strConn)
        {
        }

        SiHisempentidadDetHelper helper = new SiHisempentidadDetHelper();

        public int Save(SiHisempentidadDetDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Hempedcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Hempencodi, DbType.Int32, entity.Hempencodi);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);
            dbProvider.AddInParameter(command, helper.Hempedfecha, DbType.DateTime, entity.Hempedfecha);
            dbProvider.AddInParameter(command, helper.Hempedvalorid, DbType.Int32, entity.Hempedvalorid);
            dbProvider.AddInParameter(command, helper.Hempedvalorestado, DbType.String, entity.Hempedvalorestado);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(SiHisempentidadDetDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Hempedcodi, DbType.Int32, entity.Hempedcodi);
            dbProvider.AddInParameter(command, helper.Hempencodi, DbType.Int32, entity.Hempencodi);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);
            dbProvider.AddInParameter(command, helper.Hempedfecha, DbType.DateTime, entity.Hempedfecha);
            dbProvider.AddInParameter(command, helper.Hempedvalorid, DbType.Int32, entity.Hempedvalorid);
            dbProvider.AddInParameter(command, helper.Hempedvalorestado, DbType.String, entity.Hempedvalorestado);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int hempedcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Hempedcodi, DbType.Int32, hempedcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public SiHisempentidadDetDTO GetById(int hempedcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Hempedcodi, DbType.Int32, hempedcodi);
            SiHisempentidadDetDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<SiHisempentidadDetDTO> List()
        {
            List<SiHisempentidadDetDTO> entitys = new List<SiHisempentidadDetDTO>();
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

        public List<SiHisempentidadDetDTO> GetByCriteria(int migracodi)
        {
            List<SiHisempentidadDetDTO> entitys = new List<SiHisempentidadDetDTO>();

            string sql = string.Format(helper.SqlGetByCriteria, migracodi);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }

        public List<SiHisempentidadDetDTO> GetByCriteriaXTabla(int migracodi, string tablename, string fieldid, string fielddesc, string fielddesc2, string fieldestado)
        {
            List<SiHisempentidadDetDTO> entitys = new List<SiHisempentidadDetDTO>();

            string sql = string.Format(helper.SqlGetByCriteriaXTabla, migracodi, tablename, fieldid, fielddesc, fielddesc2, fieldestado);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    var entity = helper.Create(dr);

                    int iEmprnombOrigen = dr.GetOrdinal(helper.EmprnombOrigen);
                    if (!dr.IsDBNull(iEmprnombOrigen)) entity.EmprnombOrigen = dr.GetString(iEmprnombOrigen);

                    int iNombre = dr.GetOrdinal(helper.Nombre);
                    if (!dr.IsDBNull(iNombre)) entity.Nombre = dr.GetString(iNombre);

                    int iNombre2 = dr.GetOrdinal(helper.Nombre2);
                    if (!dr.IsDBNull(iNombre2)) entity.Nombre2 = dr.GetString(iNombre2);

                    int iEstadoActual = dr.GetOrdinal(helper.EstadoActual);
                    if (!dr.IsDBNull(iEstadoActual)) entity.EstadoActual = dr.GetString(iEstadoActual);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }
    }
}
