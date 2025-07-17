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
    /// Clase de acceso a datos de la tabla PR_GENFORZADA
    /// </summary>
    public class PrGenforzadaRepository : RepositoryBase, IPrGenforzadaRepository
    {
        public PrGenforzadaRepository(string strConn)
            : base(strConn)
        {
        }

        PrGenforzadaHelper helper = new PrGenforzadaHelper();

        public int Save(PrGenforzadaDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Genforcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Relacioncodi, DbType.Int32, entity.Relacioncodi);
            dbProvider.AddInParameter(command, helper.Genforinicio, DbType.DateTime, entity.Genforinicio);
            dbProvider.AddInParameter(command, helper.Genforfin, DbType.DateTime, entity.Genforfin);
            dbProvider.AddInParameter(command, helper.Genforsimbolo, DbType.String, entity.Genforsimbolo);
            dbProvider.AddInParameter(command, helper.Subcausacodi, DbType.Int32, entity.Subcausacodi);
            dbProvider.AddInParameter(command, helper.Lastdate, DbType.DateTime, entity.Lastdate);
            dbProvider.AddInParameter(command, helper.Lastuser, DbType.String, entity.Lastuser);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(PrGenforzadaDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Relacioncodi, DbType.Int32, entity.Relacioncodi);            
            dbProvider.AddInParameter(command, helper.Genforinicio, DbType.DateTime, entity.Genforinicio);
            dbProvider.AddInParameter(command, helper.Genforfin, DbType.DateTime, entity.Genforfin);
            dbProvider.AddInParameter(command, helper.Genforsimbolo, DbType.String, entity.Genforsimbolo);
            dbProvider.AddInParameter(command, helper.Subcausacodi, DbType.Int32, entity.Subcausacodi);
            dbProvider.AddInParameter(command, helper.Lastdate, DbType.DateTime, entity.Lastdate);
            dbProvider.AddInParameter(command, helper.Lastuser, DbType.String, entity.Lastuser);
            dbProvider.AddInParameter(command, helper.Genforcodi, DbType.Int32, entity.Genforcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int genforcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Genforcodi, DbType.Int32, genforcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public PrGenforzadaDTO GetById(int genforcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Genforcodi, DbType.Int32, genforcodi);
            PrGenforzadaDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<PrGenforzadaDTO> List()
        {
            List<PrGenforzadaDTO> entitys = new List<PrGenforzadaDTO>();
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

        public List<PrGenforzadaDTO> GetByCriteria(DateTime fechaInicio, DateTime fechaFin)
        {
            string sql = string.Format(helper.SqlGetByCriteria, fechaInicio.ToString(ConstantesBase.FormatoFecha),
                fechaFin.ToString(ConstantesBase.FormatoFecha));
            List<PrGenforzadaDTO> entitys = new List<PrGenforzadaDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    PrGenforzadaDTO entity = helper.Create(dr);

                    int iEquinomb = dr.GetOrdinal(helper.Equinomb);
                    if (!dr.IsDBNull(iEquinomb)) entity.Equinomb = dr.GetString(iEquinomb);

                    int iNombarra = dr.GetOrdinal(helper.Nombarra);
                    if (!dr.IsDBNull(iNombarra)) entity.Nombarra = dr.GetString(iNombarra);

                    int iIdgener = dr.GetOrdinal(helper.Idgener);
                    if (!dr.IsDBNull(iIdgener)) entity.Idgener = dr.GetString(iIdgener);

                    int iSubcausadesc = dr.GetOrdinal(helper.Subcausadesc);
                    if (!dr.IsDBNull(iSubcausadesc)) entity.Subcausadesc = dr.GetString(iSubcausadesc);


                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<PrGenforzadaDTO> ObtenerGeneracionForzadaProceso(DateTime fechaProceso)
        {
            string sql = string.Format(helper.SqlObtenerGeneracionForzadaProceso, fechaProceso.ToString(ConstantesBase.FormatoFechaHora));
            List<PrGenforzadaDTO> entitys = new List<PrGenforzadaDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    PrGenforzadaDTO entity = new PrGenforzadaDTO();

                    int iGrupocodi = dr.GetOrdinal(helper.Grupocodi);
                    if (!dr.IsDBNull(iGrupocodi)) entity.Grupocodi = Convert.ToInt32(dr.GetValue(iGrupocodi));
                        
                    int iEquicodi = dr.GetOrdinal(helper.Equicodi);
                    if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

                    int iSubcausacmg = dr.GetOrdinal(helper.Subcausacmg);
                    if (!dr.IsDBNull(iSubcausacmg)) entity.Subcausacmg = dr.GetString(iSubcausacmg);                   

                    int iIdgener = dr.GetOrdinal(helper.Idgener);
                    if (!dr.IsDBNull(iIdgener)) entity.Idgener = dr.GetString(iIdgener);

                    int iNombarra = dr.GetOrdinal(helper.Nombarra);
                    if (!dr.IsDBNull(iNombarra)) entity.Nombarra = dr.GetString(iNombarra);

                    int iNombretna = dr.GetOrdinal(helper.Nombretna);
                    if (!dr.IsDBNull(iNombretna)) entity.Nombretna = dr.GetString(iNombretna);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<PrGenforzadaDTO> ObtenerGeneracionForzadaProcesoV2(DateTime fechaProceso)
        {
            string sql = string.Format(helper.SqlObtenerGeneracionForzadaProcesoV2, fechaProceso.ToString(ConstantesBase.FormatoFechaHora));
            List<PrGenforzadaDTO> entitys = new List<PrGenforzadaDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    PrGenforzadaDTO entity = new PrGenforzadaDTO();

                    int iGrupocodi = dr.GetOrdinal(helper.Grupocodi);
                    if (!dr.IsDBNull(iGrupocodi)) entity.Grupocodi = Convert.ToInt32(dr.GetValue(iGrupocodi));

                    int iEquicodi = dr.GetOrdinal(helper.Equicodi);
                    if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

                    int iSubcausacmg = dr.GetOrdinal(helper.Subcausacmg);
                    if (!dr.IsDBNull(iSubcausacmg)) entity.Subcausacmg = dr.GetString(iSubcausacmg);

                    int iIdgener = dr.GetOrdinal(helper.Idgener);
                    if (!dr.IsDBNull(iIdgener)) entity.Idgener = dr.GetString(iIdgener);

                    int iNombarra = dr.GetOrdinal(helper.Nombarra);
                    if (!dr.IsDBNull(iNombarra)) entity.Nombarra = dr.GetString(iNombarra);

                    int iNombretna = dr.GetOrdinal(helper.Nombretna);
                    if (!dr.IsDBNull(iNombretna)) entity.Nombretna = dr.GetString(iNombretna);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<PrGenforzadaDTO> ObtenerUnidadesPasada()
        {
            string sql = string.Format(helper.SqlObtenerUnidadesPasada);
            List<PrGenforzadaDTO> entitys = new List<PrGenforzadaDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    PrGenforzadaDTO entity = new PrGenforzadaDTO();

                    int iGrupocodi = dr.GetOrdinal(helper.Grupocodi);
                    if (!dr.IsDBNull(iGrupocodi)) entity.Grupocodi = Convert.ToInt32(dr.GetValue(iGrupocodi));

                    int iEquicodi = dr.GetOrdinal(helper.Equicodi);
                    if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

                    int iSubcausacmg = dr.GetOrdinal(helper.Subcausacmg);
                    if (!dr.IsDBNull(iSubcausacmg)) entity.Subcausacmg = dr.GetString(iSubcausacmg);

                    int iIdgener = dr.GetOrdinal(helper.Idgener);
                    if (!dr.IsDBNull(iIdgener)) entity.Idgener = dr.GetString(iIdgener);

                    int iNombarra = dr.GetOrdinal(helper.Nombarra);
                    if (!dr.IsDBNull(iNombarra)) entity.Nombarra = dr.GetString(iNombarra);

                    int iNombretna = dr.GetOrdinal(helper.Nombretna);
                    if (!dr.IsDBNull(iNombretna)) entity.Nombretna = dr.GetString(iNombretna);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }
    }
}
