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
    /// Clase de acceso a datos de la tabla EVE_RSFHORA
    /// </summary>
    public class EveRsfhoraRepository : RepositoryBase, IEveRsfhoraRepository
    {
        public EveRsfhoraRepository(string strConn) : base(strConn)
        {
        }

        EveRsfhoraHelper helper = new EveRsfhoraHelper();

        public int Save(EveRsfhoraDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Rsfhorcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Rsfhorindman, DbType.String, entity.Rsfhorindman);
            dbProvider.AddInParameter(command, helper.Rsfhorindaut, DbType.String, entity.Rsfhorindaut);
            dbProvider.AddInParameter(command, helper.Rsfhorcomentario, DbType.String, entity.Rsfhorcomentario);
            dbProvider.AddInParameter(command, helper.Lastuser, DbType.String, entity.Lastuser);
            dbProvider.AddInParameter(command, helper.Lastdate, DbType.DateTime, entity.Lastdate);
            dbProvider.AddInParameter(command, helper.Rsfhorfecha, DbType.DateTime, entity.Rsfhorfecha);
            dbProvider.AddInParameter(command, helper.Rsfhorinicio, DbType.DateTime, entity.Rsfhorinicio);
            dbProvider.AddInParameter(command, helper.Rsfhorfin, DbType.DateTime, entity.Rsfhorfin);
            dbProvider.AddInParameter(command, helper.Rsfhormaximo, DbType.Decimal, entity.Rsfhormaximo);
            dbProvider.AddInParameter(command, helper.Rsfhorindxml, DbType.String, entity.Rsfhorindxml);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(EveRsfhoraDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            //dbProvider.AddInParameter(command, helper.Rsfhorindman, DbType.String, entity.Rsfhorindman);
            //dbProvider.AddInParameter(command, helper.Rsfhorindaut, DbType.String, entity.Rsfhorindaut);
            //dbProvider.AddInParameter(command, helper.Rsfhorcomentario, DbType.String, entity.Rsfhorcomentario);
            dbProvider.AddInParameter(command, helper.Lastuser, DbType.String, entity.Lastuser);
            dbProvider.AddInParameter(command, helper.Lastdate, DbType.DateTime, entity.Lastdate);
            dbProvider.AddInParameter(command, helper.Rsfhorfecha, DbType.DateTime, entity.Rsfhorfecha);
            dbProvider.AddInParameter(command, helper.Rsfhorinicio, DbType.DateTime, entity.Rsfhorinicio);
            dbProvider.AddInParameter(command, helper.Rsfhorfin, DbType.DateTime, entity.Rsfhorfin);
            //dbProvider.AddInParameter(command, helper.Rsfhormaximo, DbType.Decimal, entity.Rsfhormaximo);
            dbProvider.AddInParameter(command, helper.Rsfhorcodi, DbType.Int32, entity.Rsfhorcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public int Update2(EveRsfhoraDTO entity)
        {

            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetCriteria);
            dbProvider.AddInParameter(command, helper.Rsfhorfecha, DbType.DateTime, entity.Rsfhorfecha);
            dbProvider.AddInParameter(command, helper.Rsfhorinicio, DbType.DateTime, entity.Rsfhorinicio);
            dbProvider.AddInParameter(command, helper.Rsfhorfin, DbType.DateTime, entity.Rsfhorfin);
            object result = dbProvider.ExecuteScalar(command);
            int id = 0;
            if (result != null) id = Convert.ToInt32(result);

            entity.Rsfhorcodi = id;
            
            command = dbProvider.GetSqlStringCommand(helper.SqlUpdate2);

            
            dbProvider.AddInParameter(command, helper.Lastuser, DbType.String, entity.Lastuser);
            dbProvider.AddInParameter(command, helper.Lastdate, DbType.DateTime, entity.Lastdate);
            dbProvider.AddInParameter(command, helper.Rsfhorcomentario, DbType.String, entity.Rsfhorcomentario);
            dbProvider.AddInParameter(command, helper.Rsfhorindman, DbType.String, entity.Rsfhorindman);
            dbProvider.AddInParameter(command, helper.Rsfhorindaut, DbType.String, entity.Rsfhorindaut);
            dbProvider.AddInParameter(command, helper.Rsfhorcodi, DbType.Int32, entity.Rsfhorcodi);

            dbProvider.ExecuteNonQuery(command);

            return id;
        }

        public void ActualizarXML(EveRsfhoraDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlActualizarXML);

            dbProvider.AddInParameter(command, helper.Rsfhorindxml, DbType.String, entity.Rsfhorindxml);
            dbProvider.AddInParameter(command, helper.Rsfhorcodi, DbType.Int32, entity.Rsfhorcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(DateTime fecha)
        {
            string sql = string.Format(helper.SqlDelete, fecha.ToString(ConstantesBase.FormatoFecha));
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            dbProvider.ExecuteNonQuery(command);
        }

        public EveRsfhoraDTO GetById(int rsfhorcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Rsfhorcodi, DbType.Int32, rsfhorcodi);
            EveRsfhoraDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<EveRsfhoraDTO> List()
        {
            List<EveRsfhoraDTO> entitys = new List<EveRsfhoraDTO>();
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

        public List<EveRsfhoraDTO> GetByCriteria(DateTime fecha)
        {
            List<EveRsfhoraDTO> entitys = new List<EveRsfhoraDTO>();
            string sql = string.Format(helper.SqlGetByCriteria, fecha.ToString(ConstantesBase.FormatoFecha));
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

        public int ValidarExistencia(DateTime fecha)
        {
            string sql = string.Format(helper.SqlValidarExistencia, fecha.ToString(ConstantesBase.FormatoFecha));
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            object result = dbProvider.ExecuteScalar(command);

            if (result != null)
            {
                return Convert.ToInt32(result);
            }

            return 0;
        }

        public List<EveRsfhoraDTO> ObtenerReporte(DateTime fechaInicio, DateTime fechaFin)
        {
            List<EveRsfhoraDTO> entitys = new List<EveRsfhoraDTO>();
            string sql = string.Format(helper.SqlObtenerReporte, fechaInicio.ToString(ConstantesBase.FormatoFecha),
                fechaFin.ToString(ConstantesBase.FormatoFecha));

            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    EveRsfhoraDTO entity = new EveRsfhoraDTO();

                    int iRsfhorfecha = dr.GetOrdinal(this.helper.Rsfhorfecha);
                    if (!dr.IsDBNull(iRsfhorfecha)) entity.Rsfhorfecha = dr.GetDateTime(iRsfhorfecha);

                    int iRsfhorinicio = dr.GetOrdinal(this.helper.Rsfhorinicio);
                    if (!dr.IsDBNull(iRsfhorinicio)) entity.Rsfhorinicio = dr.GetDateTime(iRsfhorinicio);

                    int iRsfhorfin = dr.GetOrdinal(this.helper.Rsfhorfin);
                    if (!dr.IsDBNull(iRsfhorfin)) entity.Rsfhorfin = dr.GetDateTime(iRsfhorfin);

                    int iUrsnomb = dr.GetOrdinal(this.helper.Ursnomb);
                    if (!dr.IsDBNull(iUrsnomb)) entity.Ursnomb = dr.GetString(iUrsnomb);

                    int iValorautomatico = dr.GetOrdinal(this.helper.ValorAutomatico);
                    if (!dr.IsDBNull(iValorautomatico)) entity.Valorautomatico = dr.GetDecimal(iValorautomatico);

                    entitys.Add(entity);
                }
            }

            return entitys;

        }

        public void DeletePorId(int id)
        {
            string sql = string.Format(helper.SqlDeletePorId, id);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            dbProvider.ExecuteNonQuery(command);

        }

        #region Modificación_RSF_05012021
        public List<EveRsfhoraDTO> ObtenerDatosXML(DateTime fecha)
        {
            List<EveRsfhoraDTO> entitys = new List<EveRsfhoraDTO>();
            string sql = string.Format(helper.SqlObtenerDatosXML, fecha.ToString(ConstantesBase.FormatoFecha));
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

        #endregion
    }
}
