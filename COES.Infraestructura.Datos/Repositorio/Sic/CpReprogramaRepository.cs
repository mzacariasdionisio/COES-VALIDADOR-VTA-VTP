//using COES.Aplicacion.CortoPlazo.DTO;
//using COES.Dominio.CortoPlazo.Interfaces.Ado;
//using COES.Infraestructura.Core.Ado;
using COES.Base.Core;
using COES.Dominio.DTO.Sic;
using COES.Dominio.Interfaces.Sic;
using COES.Infraestructura.Datos.Respositorio.Sic;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace COES.Infraestructura.Datos.Repositorio.Sic
{
    /// <summary>
    /// Clase de acceso a datos de la tabla CP_GRUPORECURSO
    /// </summary>
    public class CpReprogramaRepository : RepositoryBase, ICpReprogramaRepository
    {
        public CpReprogramaRepository(string strConn)
            : base(strConn)
        {
        }

        CpReprogramaHelper helper = new CpReprogramaHelper();

        public void Save(CpReprogramaDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Reprogcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Topcodi1, DbType.Int32, entity.Topcodi1);
            dbProvider.AddInParameter(command, helper.Topcodi2, DbType.Int32, entity.Topcodi2);
            dbProvider.AddInParameter(command, helper.Reprogorden, DbType.Int32, entity.Reprogorden);
            dbProvider.ExecuteNonQuery(command);
        }

        public void Update(CpReprogramaDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Reprogcodi, DbType.Int32, entity.Reprogcodi);
            dbProvider.AddInParameter(command, helper.Topcodi1, DbType.Int32, entity.Topcodi1);
            dbProvider.AddInParameter(command, helper.Topcodi2, DbType.Int32, entity.Topcodi2);
            dbProvider.AddInParameter(command, helper.Reprogorden, DbType.Int32, entity.Reprogorden);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int topcodi2)
        {
            string sqlquery = string.Format(helper.SqlDelete, topcodi2);
            DbCommand command = dbProvider.GetSqlStringCommand(sqlquery);
            dbProvider.ExecuteNonQuery(command);
        }


        public CpReprogramaDTO GetById(int topcodi1)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Topcodi1, DbType.Int32, topcodi1);

            CpReprogramaDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<CpReprogramaDTO> List()
        {
            string sqlQuery = string.Format(helper.SqlList);
            List<CpReprogramaDTO> entitys = new List<CpReprogramaDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(sqlQuery);

            CpReprogramaDTO entity = new CpReprogramaDTO();

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = helper.Create(dr);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<CpReprogramaDTO> GetByCriteria(int topcodi1)
        {
            List<CpReprogramaDTO> entitys = new List<CpReprogramaDTO>();
            string sqlQuery = string.Format(helper.SqlGetByCriteria, topcodi1);
            DbCommand command = dbProvider.GetSqlStringCommand(sqlQuery);
            CpReprogramaDTO entity;
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = helper.Create(dr);
                    int iTopfecha = dr.GetOrdinal(helper.Topfecha);
                    if (!dr.IsDBNull(iTopfecha)) entity.Topfecha = dr.GetDateTime(iTopfecha);
                    int iTopnombre = dr.GetOrdinal(helper.Topnombre);
                    if (!dr.IsDBNull(iTopnombre)) entity.Topnombre = dr.GetString(iTopnombre);
                    int iLastuser = dr.GetOrdinal(helper.Lastuser);
                    if (!dr.IsDBNull(iLastuser)) entity.Lastuser = dr.GetString(iLastuser);
                    int iLastdate = dr.GetOrdinal(helper.Lastdate);
                    if (!dr.IsDBNull(iLastdate)) entity.Lastdate = dr.GetDateTime(iLastdate).ToString(ConstantesBase.FormatoFecha);
                    int iTopiniciohora = dr.GetOrdinal(helper.Topiniciohora);
                    if (!dr.IsDBNull(iTopiniciohora)) entity.Topiniciohora = Convert.ToInt32(dr.GetValue(iTopiniciohora));

                    int iTophorareprog = dr.GetOrdinal(helper.Tophorareprog);
                    if (!dr.IsDBNull(iTophorareprog)) entity.Tophorareprog = Convert.ToInt32(dr.GetValue(iTophorareprog));

                    int iTopuserdespacho = dr.GetOrdinal(helper.Topuserdespacho);
                    if (!dr.IsDBNull(iTopuserdespacho)) entity.Topouserdespacho = dr.GetString(iTopuserdespacho);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<CpReprogramaDTO> ListTopPrincipal(int topcodi)
        {
            string sqlQuery = string.Format(helper.SqlListTopPrincipal, topcodi);
            List<CpReprogramaDTO> entitys = new List<CpReprogramaDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(sqlQuery);

            CpReprogramaDTO entity = new CpReprogramaDTO();

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = helper.Create(dr);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

    }
}
