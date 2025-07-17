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
    /// Clase de acceso a datos de la tabla PF_CONTRATOS
    /// </summary>
    public class PfContratosRepository : RepositoryBase, IPfContratosRepository
    {
        public PfContratosRepository(string strConn) : base(strConn)
        {
        }

        PfContratosHelper helper = new PfContratosHelper();

        public int Save(PfContratosDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Pfcontcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Pfcontcantidad, DbType.Decimal, entity.Pfcontcantidad);
            dbProvider.AddInParameter(command, helper.Pfcontvigenciaini, DbType.DateTime, entity.Pfcontvigenciaini);
            dbProvider.AddInParameter(command, helper.Pfcontvigenciafin, DbType.DateTime, entity.Pfcontvigenciafin);
            dbProvider.AddInParameter(command, helper.Pfcontobservacion, DbType.String, entity.Pfcontobservacion);
            dbProvider.AddInParameter(command, helper.Pfcontcedente, DbType.Int32, entity.Pfcontcedente);
            dbProvider.AddInParameter(command, helper.Pfcontcesionario, DbType.Int32, entity.Pfcontcesionario);
            dbProvider.AddInParameter(command, helper.Pfpericodi, DbType.Int32, entity.Pfpericodi);
            dbProvider.AddInParameter(command, helper.Pfverscodi, DbType.Int32, entity.Pfverscodi);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(PfContratosDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Pfcontcantidad, DbType.Decimal, entity.Pfcontcantidad);
            dbProvider.AddInParameter(command, helper.Pfcontvigenciaini, DbType.DateTime, entity.Pfcontvigenciaini);
            dbProvider.AddInParameter(command, helper.Pfcontvigenciafin, DbType.DateTime, entity.Pfcontvigenciafin);
            dbProvider.AddInParameter(command, helper.Pfcontobservacion, DbType.String, entity.Pfcontobservacion);
            dbProvider.AddInParameter(command, helper.Pfcontcedente, DbType.Int32, entity.Pfcontcedente);
            dbProvider.AddInParameter(command, helper.Pfcontcesionario, DbType.Int32, entity.Pfcontcesionario);
            dbProvider.AddInParameter(command, helper.Pfpericodi, DbType.Int32, entity.Pfpericodi);
            dbProvider.AddInParameter(command, helper.Pfverscodi, DbType.Int32, entity.Pfverscodi);
            dbProvider.AddInParameter(command, helper.Pfcontcodi, DbType.Int32, entity.Pfcontcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int pfcontcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Pfcontcodi, DbType.Int32, pfcontcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public PfContratosDTO GetById(int pfcontcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Pfcontcodi, DbType.Int32, pfcontcodi);
            PfContratosDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<PfContratosDTO> List()
        {
            List<PfContratosDTO> entitys = new List<PfContratosDTO>();
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

        public List<PfContratosDTO> GetByCriteria()
        {
            List<PfContratosDTO> entitys = new List<PfContratosDTO>();
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
        /// Listado de contratos de compra y Venta por Filtro
        /// </summary>
        /// <param name="pericodi"></param>
        /// <param name="recacodi"></param>
        /// <param name="versionId"></param>
        /// <returns></returns>
        public List<PfContratosDTO> ListarContratosCVFiltro(int pericodi, int recacodi, int versionId)
        {
            List<PfContratosDTO> entitys = new List<PfContratosDTO>();

            string query = string.Format(helper.SqlListarContratosCVFiltro, recacodi, pericodi, versionId);

            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    var entity = helper.Create(dr);
                    //entitys.Add(helper.Create(dr));
                    int iCedente = dr.GetOrdinal(helper.Cedente);
                    if (!dr.IsDBNull(iCedente)) entity.Pfcontnombcedente = dr.GetString(iCedente);

                    int iCesionario = dr.GetOrdinal(helper.Cesionario);
                    if (!dr.IsDBNull(iCesionario)) entity.Pfcontnombcesionario = dr.GetString(iCesionario);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }
    }
}
