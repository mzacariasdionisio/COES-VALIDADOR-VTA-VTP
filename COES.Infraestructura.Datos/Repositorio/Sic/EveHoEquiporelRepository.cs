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
    /// Clase de acceso a datos de la tabla EVE_HO_EQUIPOREL
    /// </summary>
    public class EveHoEquiporelRepository : RepositoryBase, IEveHoEquiporelRepository
    {
        public EveHoEquiporelRepository(string strConn)
            : base(strConn)
        {
        }

        EveHoEquiporelHelper helper = new EveHoEquiporelHelper();

        public int Save(EveHoEquiporelDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Hopcodi, DbType.Int32, entity.Hopcodi);
            dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, entity.Equicodi);
            dbProvider.AddInParameter(command, helper.Hoequicodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Hoequitipo, DbType.Int32, entity.Hoequitipo);
            dbProvider.AddInParameter(command, helper.Iccodi, DbType.Int32, entity.Iccodi);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(EveHoEquiporelDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Hopcodi, DbType.Int32, entity.Hopcodi);
            dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, entity.Equicodi);
            dbProvider.AddInParameter(command, helper.Hoequitipo, DbType.Int32, entity.Hoequitipo);
            dbProvider.AddInParameter(command, helper.Iccodi, DbType.Int32, entity.Iccodi);
            dbProvider.AddInParameter(command, helper.Hoequicodi, DbType.Int32, entity.Hoequicodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int hoequicodi)
        {
            string query = string.Format(helper.SqlDelete, hoequicodi);

            DbCommand command = dbProvider.GetSqlStringCommand(query);

            dbProvider.ExecuteNonQuery(command);
        }

        public void DeleteByHopcodi(int hopcodi)
        {
            string query = string.Format(helper.SqlDeleteByHopcodi, hopcodi);

            DbCommand command = dbProvider.GetSqlStringCommand(query);

            dbProvider.ExecuteNonQuery(command);
        }

        public EveHoEquiporelDTO GetById()
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            EveHoEquiporelDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<EveHoEquiporelDTO> List()
        {
            List<EveHoEquiporelDTO> entitys = new List<EveHoEquiporelDTO>();
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

        public List<EveHoEquiporelDTO> GetByCriteria(DateTime fechaIni, DateTime fechaFin)
        {
            List<EveHoEquiporelDTO> entitys = new List<EveHoEquiporelDTO>();

            string sql = string.Format(helper.SqlGetByCriteria
                , fechaIni.ToString(ConstantesBase.FormatoFecha), fechaFin.AddDays(1).ToString(ConstantesBase.FormatoFecha));

            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    var entity = helper.Create(dr);

                    int iSubcausacodi = dr.GetOrdinal(this.helper.Subcausacodi);
                    if (!dr.IsDBNull(iSubcausacodi)) entity.Subcausacodi = Convert.ToInt32(dr.GetValue(iSubcausacodi));

                    int iIchorini = dr.GetOrdinal(this.helper.Ichorini);
                    if (!dr.IsDBNull(iIchorini)) entity.Ichorini = dr.GetDateTime(iIchorini);

                    int iIchorfin = dr.GetOrdinal(this.helper.Ichorfin);
                    if (!dr.IsDBNull(iIchorfin)) entity.Ichorfin = dr.GetDateTime(iIchorfin);

                    int iIcvalor1 = dr.GetOrdinal(this.helper.Icvalor1);
                    if (!dr.IsDBNull(iIcvalor1)) entity.Icvalor1 = dr.GetDecimal(iIcvalor1);

                    int iSubcausadesc = dr.GetOrdinal(this.helper.Subcausadesc);
                    if (!dr.IsDBNull(iSubcausadesc)) entity.Subcausadesc = dr.GetString(iSubcausadesc);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<EveHoEquiporelDTO> ListaByHopcodi(int hopcodi)
        {
            List<EveHoEquiporelDTO> entitys = new List<EveHoEquiporelDTO>();

            string query = string.Format(helper.SqlListaByHopcodi, hopcodi);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    var entity = helper.Create(dr);

                    int iSubcausacodi = dr.GetOrdinal(this.helper.Subcausacodi);
                    if (!dr.IsDBNull(iSubcausacodi)) entity.Subcausacodi = Convert.ToInt32(dr.GetValue(iSubcausacodi));

                    int iIchorini = dr.GetOrdinal(this.helper.Ichorini);
                    if (!dr.IsDBNull(iIchorini)) entity.Ichorini = dr.GetDateTime(iIchorini);

                    int iIchorfin = dr.GetOrdinal(this.helper.Ichorfin);
                    if (!dr.IsDBNull(iIchorfin)) entity.Ichorfin = dr.GetDateTime(iIchorfin);

                    int iIcvalor1 = dr.GetOrdinal(this.helper.Icvalor1);
                    if (!dr.IsDBNull(iIcvalor1)) entity.Icvalor1 = dr.GetDecimal(iIcvalor1);

                    int iSubcausadesc = dr.GetOrdinal(this.helper.Subcausadesc);
                    if (!dr.IsDBNull(iSubcausadesc)) entity.Subcausadesc = dr.GetString(iSubcausadesc);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }
    }
}
