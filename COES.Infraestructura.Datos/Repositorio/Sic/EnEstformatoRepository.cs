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
    /// Clase de acceso a datos de la tabla EN_ESTFORMATO
    /// </summary>
    public class EnEstformatoRepository : RepositoryBase, IEnEstformatoRepository
    {
        public EnEstformatoRepository(string strConn)
            : base(strConn)
        {
        }

        EnEstformatoHelper helper = new EnEstformatoHelper();


        public int Save(EnEstformatoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            string insertQuery = string.Format(helper.SqlSave,
                                                   id,
                                                   entity.Enunidadcodi,
                                                   entity.Formatocodi,
                                                   entity.Estadocodi,
                                                  ((DateTime)entity.Estfmtlastdate).ToString(ConstantesBase.FormatoFechaExtendido),
                                                  entity.Estfmtlastuser,
                                                   entity.Estfmtdescrip);

            command = dbProvider.GetSqlStringCommand(insertQuery);
            dbProvider.ExecuteNonQuery(command);
            return id;
        }


        public void Update(EnEstformatoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Estfmtcodi, DbType.Int32, entity.Estfmtcodi);
            dbProvider.AddInParameter(command, helper.Enunidadcodi, DbType.Int32, entity.Enunidadcodi);
            dbProvider.AddInParameter(command, helper.Formatocodi, DbType.Int32, entity.Formatocodi);
            dbProvider.AddInParameter(command, helper.Estadocodi, DbType.Int32, entity.Estadocodi);
            dbProvider.AddInParameter(command, helper.Estfmtlastdate, DbType.DateTime, entity.Estfmtlastdate);
            dbProvider.AddInParameter(command, helper.Estfmtlastuser, DbType.String, entity.Estfmtlastuser);
            dbProvider.AddInParameter(command, helper.Estfmtdescrip, DbType.String, entity.Estfmtdescrip);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int estfmtcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Estfmtcodi, DbType.Int32, estfmtcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public EnEstformatoDTO GetById(int estfmtcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Estfmtcodi, DbType.Int32, estfmtcodi);
            EnEstformatoDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<EnEstformatoDTO> List()
        {
            List<EnEstformatoDTO> entitys = new List<EnEstformatoDTO>();
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

        public List<EnEstformatoDTO> ListFormatoXEstado(int enunidadcodi, int iformatocodi)
        {
            List<EnEstformatoDTO> entitys = new List<EnEstformatoDTO>();
            string query = string.Format(helper.SqlListFormatoXEstado, enunidadcodi, iformatocodi);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            EnEstformatoDTO entity = new EnEstformatoDTO();
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = helper.Create(dr);
                    int iformatodesc = dr.GetOrdinal("enfmtdesc");
                    int iestadoNombre = dr.GetOrdinal("estadoNombre");
                    int iEstadocolor = dr.GetOrdinal("estadocolor");
                    if (!dr.IsDBNull(iformatodesc)) entity.Formatodesc = dr.GetString(iformatodesc);
                    if (!dr.IsDBNull(iestadoNombre)) entity.Estadonombre = dr.GetString(iestadoNombre);
                    if (!dr.IsDBNull(iEstadocolor)) entity.Estadocolor = dr.GetString(iEstadocolor);
                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<EnEstformatoDTO> GetByCriteria()
        {
            List<EnEstformatoDTO> entitys = new List<EnEstformatoDTO>();
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
    }
}
