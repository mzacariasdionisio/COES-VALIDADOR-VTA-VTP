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
    /// Clase de acceso a datos de la tabla CM_GENERACION_EMS
    /// </summary>
    public class CmGeneracionEmsRepository: RepositoryBase, ICmGeneracionEmsRepository
    {
        public CmGeneracionEmsRepository(string strConn): base(strConn)
        {
        }

        CmGeneracionEmsHelper helper = new CmGeneracionEmsHelper();

        public int Save(CmGeneracionEmsDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);
            entity.Emprcodi = entity.Emprcodi > 0 ? entity.Emprcodi : -1;

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Genemscodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Cmgncorrelativo, DbType.Int32, entity.Cmgncorrelativo);
            dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, entity.Equicodi);
            dbProvider.AddInParameter(command, helper.Genemsgeneracion, DbType.Decimal, entity.Genemsgeneracion);
            dbProvider.AddInParameter(command, helper.Genemsoperativo, DbType.Int32, entity.Genemsoperativo);
            dbProvider.AddInParameter(command, helper.Genemsfecha, DbType.DateTime, entity.Genemsfecha);
            dbProvider.AddInParameter(command, helper.Genemsusucreacion, DbType.String, entity.Genemsusucreacion);
            dbProvider.AddInParameter(command, helper.Genemsfechacreacion, DbType.DateTime, entity.Genemsfechacreacion);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);
            dbProvider.AddInParameter(command, helper.Genemstipoestimado, DbType.String, entity.Genemstipoestimador);
            dbProvider.AddInParameter(command, helper.Genemspotmax, DbType.Decimal, entity.Genemspotmax);
            dbProvider.AddInParameter(command, helper.Genemspotmin, DbType.Decimal, entity.Genemspotmin);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(CmGeneracionEmsDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);
            entity.Emprcodi = entity.Emprcodi > 0 ? entity.Emprcodi : -1;

            dbProvider.AddInParameter(command, helper.Cmgncorrelativo, DbType.Int32, entity.Cmgncorrelativo);
            dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, entity.Equicodi);
            dbProvider.AddInParameter(command, helper.Genemsgeneracion, DbType.Decimal, entity.Genemsgeneracion);
            dbProvider.AddInParameter(command, helper.Genemsoperativo, DbType.Int32, entity.Genemsoperativo);
            dbProvider.AddInParameter(command, helper.Genemsfecha, DbType.DateTime, entity.Genemsfecha);
            dbProvider.AddInParameter(command, helper.Genemsusucreacion, DbType.String, entity.Genemsusucreacion);
            dbProvider.AddInParameter(command, helper.Genemsfechacreacion, DbType.DateTime, entity.Genemsfechacreacion);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);

            dbProvider.AddInParameter(command, helper.Genemscodi, DbType.Int32, entity.Genemscodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int genemscodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Genemscodi, DbType.Int32, genemscodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public CmGeneracionEmsDTO GetById(int genemscodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Genemscodi, DbType.Int32, genemscodi);
            CmGeneracionEmsDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<CmGeneracionEmsDTO> List()
        {
            List<CmGeneracionEmsDTO> entitys = new List<CmGeneracionEmsDTO>();
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

        public List<CmGeneracionEmsDTO> GetByCriteria(DateTime fechaHora)
        {
            List<CmGeneracionEmsDTO> entitys = new List<CmGeneracionEmsDTO>();

            string sql = string.Format(helper.SqlGetByCriteria, fechaHora.ToString(ConstantesBase.FormatoFechaHora));
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

        /// <summary>
        /// Borra todos los registros de Generacion EMS de una fechaHora
        /// </summary>
        /// <param name="genemsfecha">Fecha/Hora</param>
        public void DeleteByFecha(DateTime genemsfecha, string tipoestimador)
        {
            string sql = string.Format(helper.SqlDeleteByFecha, genemsfecha.ToString(ConstantesBase.FormatoFechaHora), tipoestimador);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            dbProvider.AddInParameter(command, helper.Genemsfecha, DbType.DateTime, genemsfecha);

            dbProvider.ExecuteNonQuery(command);
        }

        /// <summary>
        /// Método que obtiene la generación EMS por correlativo de Costo marginal nodal
        /// </summary>
        /// <param name="correlativo">Correlativo de cálculo de C.Marginal Nodal</param>
        /// <returns>Listado de generación por Generador</returns>
        public List<CmGeneracionEmsDTO> ObtenerGeneracionPorCorrelativo(int correlativo)
        {
            List<CmGeneracionEmsDTO> entitys = new List<CmGeneracionEmsDTO>();
            string query = String.Format(helper.SqlGeneracionPorCorrelativo, correlativo);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    var oGeneracion = helper.Create(dr);

                    int iEmprnomb = dr.GetOrdinal("EMPRNOMB");
                    if (!dr.IsDBNull(iEmprnomb)) oGeneracion.Emprnomb = dr.GetString(iEmprnomb);

                    int iEquinomb = dr.GetOrdinal("EQUINOMB");
                    if (!dr.IsDBNull(iEquinomb)) oGeneracion.Equinomb = dr.GetString(iEquinomb);

                    int iEquiabrev = dr.GetOrdinal("EQUIABREV");
                    if (!dr.IsDBNull(iEquiabrev)) oGeneracion.Equiabrev = dr.GetString(iEquiabrev);

                    entitys.Add(oGeneracion);
                }
            }

            return entitys;
        }

        /// <summary>
        /// Método para obtener la Generación EMS en un rango de fechas.
        /// </summary>
        /// <param name="fechaInicio">Fecha de inicio</param>
        /// <param name="fechaFin">Fecha de fin</param>
        /// <returns>Listado de generación EMS</returns>
        public List<CmGeneracionEmsDTO> ObtenerGeneracionPorFechas(DateTime fechaInicio, DateTime fechaFin, string estimador)
        {
            List<CmGeneracionEmsDTO> entitys = new List<CmGeneracionEmsDTO>();
            string query = String.Format(helper.SqlGeneracionPorFechas, fechaInicio.ToString(ConstantesBase.FormatoFecha),
                fechaFin.ToString(ConstantesBase.FormatoFecha), estimador);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    var oGeneracion = helper.Create(dr);

                    int iEmprnomb = dr.GetOrdinal("EMPRNOMB");
                    if (!dr.IsDBNull(iEmprnomb)) oGeneracion.Emprnomb = dr.GetString(iEmprnomb);

                    int iEquinomb = dr.GetOrdinal("EQUINOMB");
                    if (!dr.IsDBNull(iEquinomb)) oGeneracion.Equinomb = dr.GetString(iEquinomb);

                    int iEquiabrev = dr.GetOrdinal("EQUIABREV");
                    if (!dr.IsDBNull(iEquiabrev)) oGeneracion.Equiabrev = dr.GetString(iEquiabrev);

                    entitys.Add(oGeneracion);
                }
            }

            return entitys;
        }

        public void ActualizarModoOperacion(CmGeneracionEmsDTO entity)
        {
            string sql = string.Format(helper.SqlActualizarModoOperacion, entity.Equicodi, entity.Grupocodi,
                entity.Genemsfecha.ToString(ConstantesBase.FormatoFechaHora), entity.Genemsusucreacion);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            dbProvider.ExecuteNonQuery(command);
        }

        #region EMS
        public List<CmGeneracionEmsDTO> GetListaGeneracionByEquipoFecha(DateTime fechaInicio, DateTime fechaFin, string empresa, string famcodi)
        {
            List<CmGeneracionEmsDTO> entitys = new List<CmGeneracionEmsDTO>();
            string query = String.Format(helper.SqlGetListaGeneracionByEquipoFecha, fechaInicio.ToString(ConstantesBase.FormatoFecha), fechaFin.ToString(ConstantesBase.FormatoFecha)
                , empresa, famcodi);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    var oGeneracion = helper.Create(dr);

                    int iEmprnomb = dr.GetOrdinal("EMPRNOMB");
                    if (!dr.IsDBNull(iEmprnomb)) oGeneracion.Emprnomb = dr.GetString(iEmprnomb);

                    int iEquinomb = dr.GetOrdinal("EQUINOMB");
                    if (!dr.IsDBNull(iEquinomb)) oGeneracion.Equinomb = dr.GetString(iEquinomb);

                    int iEquiabrev = dr.GetOrdinal("EQUIABREV");
                    if (!dr.IsDBNull(iEquiabrev)) oGeneracion.Equiabrev = dr.GetString(iEquiabrev);

                    int iCentral = dr.GetOrdinal(this.helper.Central);
                    if (!dr.IsDBNull(iCentral)) oGeneracion.Central = dr.GetString(iCentral);

                    int iEquipadre = dr.GetOrdinal(helper.Equipadre);
                    if (!dr.IsDBNull(iEquipadre)) oGeneracion.Equipadre = dr.GetInt32(iEquipadre);

                    int iFamcodipadre = dr.GetOrdinal(helper.Famcodipadre);
                    if (!dr.IsDBNull(iFamcodipadre)) oGeneracion.Famcodipadre = dr.GetInt32(iFamcodipadre);

                    entitys.Add(oGeneracion);
                }
            }

            return entitys;
        }
        #endregion

        #region Mejoras CMgN

        public List<CmGeneracionEmsDTO> ObtenerGeneracionCostoIncremental(string equipos, DateTime fecha)
        {
            List<CmGeneracionEmsDTO> entitys = new List<CmGeneracionEmsDTO>();
            string query = String.Format(helper.SqlObtenerGeneracionCostoIncremental, equipos, fecha.ToString(ConstantesBase.FormatoFecha));
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    CmGeneracionEmsDTO entity = new CmGeneracionEmsDTO();

                    int iEquicodi = dr.GetOrdinal(helper.Equicodi);
                    if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

                    int iGenemsoperativo = dr.GetOrdinal(helper.Genemsoperativo);
                    if (!dr.IsDBNull(iGenemsoperativo)) entity.Genemsoperativo = Convert.ToInt32(dr.GetValue(iGenemsoperativo));

                    int iGenemsgeneracion = dr.GetOrdinal(helper.Genemsgeneracion);
                    if (!dr.IsDBNull(iGenemsgeneracion)) entity.Genemsgeneracion = dr.GetDecimal(iGenemsgeneracion);

                    int iGenemsfecha = dr.GetOrdinal(helper.Genemsfecha);
                    if (!dr.IsDBNull(iGenemsfecha)) entity.Genemsfecha = dr.GetDateTime(iGenemsfecha);

                    if (entity.Genemsfecha.Hour == 23 && entity.Genemsfecha.Minute == 59)
                        entity.Cmgncorrelativo = 48;
                    else
                        entity.Cmgncorrelativo = entity.Genemsfecha.Hour * 2 + (int)(entity.Genemsfecha.Minute / 30);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        #endregion

    }
}