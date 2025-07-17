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
    /// Clase de acceso a datos de la tabla CM_BARRA_RELACION
    /// </summary>
    public class CmBarraRelacionRepository: RepositoryBase, ICmBarraRelacionRepository
    {
        public CmBarraRelacionRepository(string strConn): base(strConn)
        {
        }

        CmBarraRelacionHelper helper = new CmBarraRelacionHelper();

        public int Save(CmBarraRelacionDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Cmbarecodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Barrcodi, DbType.Int32, entity.Barrcodi);
            dbProvider.AddInParameter(command, helper.Cnfbarcodi, DbType.Int32, entity.Cnfbarcodi);
            dbProvider.AddInParameter(command, helper.Cmbaretipreg, DbType.String, entity.Cmbaretipreg);
            dbProvider.AddInParameter(command, helper.Barrcodi2, DbType.Int32, entity.Barrcodi2);
            dbProvider.AddInParameter(command, helper.Cmbaretiprel, DbType.String, entity.Cmbaretiprel);
            dbProvider.AddInParameter(command, helper.Configcodi, DbType.Int32, entity.Configcodi);
            dbProvider.AddInParameter(command, helper.Cmbarevigencia, DbType.DateTime, entity.Cmbarevigencia);
            dbProvider.AddInParameter(command, helper.Cmbareexpira, DbType.DateTime, entity.Cmbareexpira);
            dbProvider.AddInParameter(command, helper.Cmbareestado, DbType.String, entity.Cmbareestado);
            dbProvider.AddInParameter(command, helper.Cmbareusucreacion, DbType.String, entity.Cmbareusucreacion);
            dbProvider.AddInParameter(command, helper.Cmbarefeccreacion, DbType.DateTime, entity.Cmbarefeccreacion);
            dbProvider.AddInParameter(command, helper.Cmbareusumodificacion, DbType.String, entity.Cmbareusumodificacion);
            dbProvider.AddInParameter(command, helper.Cmbarefecmodificacion, DbType.DateTime, entity.Cmbarefecmodificacion);
            dbProvider.AddInParameter(command, helper.Cnfbarcodi2, DbType.Int32, entity.Cnfbarcodi2);

            #region Ticket_6245
            dbProvider.AddInParameter(command, helper.Cmbarereporte, DbType.String, entity.Cmbarereporte);
            #endregion

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(CmBarraRelacionDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Barrcodi, DbType.Int32, entity.Barrcodi);
            dbProvider.AddInParameter(command, helper.Cnfbarcodi, DbType.Int32, entity.Cnfbarcodi);
            dbProvider.AddInParameter(command, helper.Cmbaretipreg, DbType.String, entity.Cmbaretipreg);
            dbProvider.AddInParameter(command, helper.Barrcodi2, DbType.Int32, entity.Barrcodi2);
            dbProvider.AddInParameter(command, helper.Cmbaretiprel, DbType.String, entity.Cmbaretiprel);
            dbProvider.AddInParameter(command, helper.Configcodi, DbType.Int32, entity.Configcodi);
            dbProvider.AddInParameter(command, helper.Cmbarevigencia, DbType.DateTime, entity.Cmbarevigencia);
            dbProvider.AddInParameter(command, helper.Cmbareexpira, DbType.DateTime, entity.Cmbareexpira);
            dbProvider.AddInParameter(command, helper.Cmbareestado, DbType.String, entity.Cmbareestado);
            //dbProvider.AddInParameter(command, helper.Cmbareusucreacion, DbType.String, entity.Cmbareusucreacion);
            //dbProvider.AddInParameter(command, helper.Cmbarefeccreacion, DbType.DateTime, entity.Cmbarefeccreacion);
            dbProvider.AddInParameter(command, helper.Cmbareusumodificacion, DbType.String, entity.Cmbareusumodificacion);
            dbProvider.AddInParameter(command, helper.Cmbarefecmodificacion, DbType.DateTime, entity.Cmbarefecmodificacion);
            dbProvider.AddInParameter(command, helper.Cnfbarcodi2, DbType.Int32, entity.Cnfbarcodi2);

            #region Ticket_6245
            dbProvider.AddInParameter(command, helper.Cmbarereporte, DbType.String, entity.Cmbarereporte);
            #endregion
            dbProvider.AddInParameter(command, helper.Cmbarecodi, DbType.Int32, entity.Cmbarecodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int cmbarecodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Cmbarecodi, DbType.Int32, cmbarecodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public CmBarraRelacionDTO GetById(int cmbarecodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Cmbarecodi, DbType.Int32, cmbarecodi);
            CmBarraRelacionDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<CmBarraRelacionDTO> List()
        {
            List<CmBarraRelacionDTO> entitys = new List<CmBarraRelacionDTO>();
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

        public List<CmBarraRelacionDTO> GetByCriteria(DateTime fecha)
        {
            List<CmBarraRelacionDTO> entitys = new List<CmBarraRelacionDTO>();
            string sql = string.Format(helper.SqlGetByCriteria, fecha.ToString(ConstantesBase.FormatoFecha));
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    CmBarraRelacionDTO entity = helper.Create(dr);

                    int iBarrnombre = dr.GetOrdinal(helper.Barrnomb);
                    if (!dr.IsDBNull(iBarrnombre)) entity.Barrnombre = dr.GetString(iBarrnombre);

                    int iTiporelacion = dr.GetOrdinal(helper.TipoRelacion);
                    if (!dr.IsDBNull(iTiporelacion)) entity.TipoRelacion = dr.GetString(iTiporelacion);

                    int iCnfbarnombre = dr.GetOrdinal(helper.Cnfbarnombre);
                    if (!dr.IsDBNull(iCnfbarnombre)) entity.Cnfbarnombre = dr.GetString(iCnfbarnombre);

                    int iBarrnombre2 = dr.GetOrdinal(helper.Barrnomb2);
                    if (!dr.IsDBNull(iBarrnombre2)) entity.Barrnombre2 = dr.GetString(iBarrnombre2);

                    int iEquinomb = dr.GetOrdinal(helper.Equinomb);
                    if (!dr.IsDBNull(iEquinomb)) entity.Equinomb = dr.GetString(iEquinomb);

                    entity.Vigencia = (entity.Cmbarevigencia != null) ?
                        ((DateTime)entity.Cmbarevigencia).ToString(ConstantesBase.FormatoFechaBase) : string.Empty;

                    entity.Modificacion = (entity.Cmbarefecmodificacion != null) ?
                        ((DateTime)entity.Cmbarefecmodificacion).ToString(ConstantesBase.FormatoFechaBase) : string.Empty;

                    #region Ticket_6245           
                    int iCmbarereporte = dr.GetOrdinal(helper.Cmbarereporte);
                    if (!dr.IsDBNull(iCmbarereporte)) entity.Cmbarereporte = dr.GetString(iCmbarereporte);
                    #endregion

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<CmBarraRelacionDTO> GetByCriteria(string tipoRegistro, int barra)
        {
            List<CmBarraRelacionDTO> entitys = new List<CmBarraRelacionDTO>();
            string sql = string.Format(helper.SqlObtenerPorBarra, tipoRegistro, barra);
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


        public List<CmBarraRelacionDTO> ObtenerHistorico(int barra, string tipoRegistro)
        {
            List<CmBarraRelacionDTO> entitys = new List<CmBarraRelacionDTO>();
            string sql = string.Format(helper.SqlObtenerHistorico, barra,  tipoRegistro);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    CmBarraRelacionDTO entity = helper.Create(dr);

                    int iBarrnombre = dr.GetOrdinal(helper.Barrnomb);
                    if (!dr.IsDBNull(iBarrnombre)) entity.Barrnombre = dr.GetString(iBarrnombre);

                    int iTiporelacion = dr.GetOrdinal(helper.TipoRelacion);
                    if (!dr.IsDBNull(iTiporelacion)) entity.TipoRelacion = dr.GetString(iTiporelacion);

                    int iCnfbarnombre = dr.GetOrdinal(helper.Cnfbarnombre);
                    if (!dr.IsDBNull(iCnfbarnombre)) entity.Cnfbarnombre = dr.GetString(iCnfbarnombre);

                    int iBarrnombre2 = dr.GetOrdinal(helper.Barrnomb2);
                    if (!dr.IsDBNull(iBarrnombre2)) entity.Barrnombre2 = dr.GetString(iBarrnombre2);

                    int iEquinomb = dr.GetOrdinal(helper.Equinomb);
                    if (!dr.IsDBNull(iEquinomb)) entity.Equinomb = dr.GetString(iEquinomb);

                    entity.Vigencia = (entity.Cmbarevigencia != null) ?
                        ((DateTime)entity.Cmbarevigencia).ToString(ConstantesBase.FormatoFechaBase) : string.Empty;

                    entity.Modificacion = (entity.Cmbarefecmodificacion != null) ?
                        ((DateTime)entity.Cmbarefecmodificacion).ToString(ConstantesBase.FormatoFechaBase) : string.Empty;

                    entity.Expirtacion = (entity.Cmbareexpira != null) ?
                       ((DateTime)entity.Cmbareexpira).ToString(ConstantesBase.FormatoFechaBase) : string.Empty;

                    entitys.Add(entity);
                }
            }

            return entitys;
        }
    }
}
