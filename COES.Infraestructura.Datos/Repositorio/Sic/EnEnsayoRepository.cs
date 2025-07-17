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
    /// Clase de acceso a datos de la tabla EN_ENSAYO
    /// </summary>
    public class EnEnsayoRepository : RepositoryBase, IEnEnsayoRepository
    {
        public EnEnsayoRepository(string strConn)
            : base(strConn)
        {
        }

        EnEnsayoHelper helper = new EnEnsayoHelper();

        public void Update(EnEnsayoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);


            dbProvider.AddInParameter(command, helper.Ensayofecha, DbType.DateTime, entity.Ensayofecha);
            dbProvider.AddInParameter(command, helper.Usercodi, DbType.String, entity.Usercodi);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);
            dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, entity.Equicodi);
            dbProvider.AddInParameter(command, helper.Estadocodi, DbType.Int32, entity.Estadocodi);
            dbProvider.AddInParameter(command, helper.Ensayocodi, DbType.Int32, entity.Ensayocodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void UpdateEstadoEnsayo(int icodiensayo, DateTime dfechaEvento, int iCodEstado, DateTime lastdate, string lastuser)
        {
            string sqlQuery = string.Format(helper.SqlUpdateEstadoEnsayo, iCodEstado, ((DateTime)dfechaEvento).ToString(ConstantesBase.FormatoFechaExtendido),
                ((DateTime)lastdate).ToString(ConstantesBase.FormatoFechaExtendido), lastuser, icodiensayo);
            DbCommand command = dbProvider.GetSqlStringCommand(sqlQuery);
            dbProvider.ExecuteNonQuery(command);
        }

        public int Save(EnEnsayoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);
            string insertQuery = string.Format(helper.SqlSave, id, ((DateTime)entity.Ensayofecha).ToString(ConstantesBase.FormatoFechaExtendido),
                entity.Usercodi, entity.Emprcodi, entity.Equicodi, entity.Estadocodi,
                ((DateTime)entity.Ensayofechaevento).ToString(ConstantesBase.FormatoFechaExtendido), ((DateTime)entity.Lastdate).ToString(ConstantesBase.FormatoFechaExtendido),
                entity.Lastuser, entity.Ensayomodoper);

            command = dbProvider.GetSqlStringCommand(insertQuery);
            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Delete()
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);


            dbProvider.ExecuteNonQuery(command);
        }

        public EnEnsayoDTO GetById(int ensayocodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Ensayocodi, DbType.Int32, ensayocodi);
            EnEnsayoDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                    int iEmprnomb = dr.GetOrdinal("Emprnomb");
                    int iEquinomb = dr.GetOrdinal("Equinomb");
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);
                    if (!dr.IsDBNull(iEquinomb)) entity.Equinomb = dr.GetString(iEquinomb);

                }
            }

            return entity;
        }

        public List<EnEnsayoDTO> List()
        {
            List<EnEnsayoDTO> entitys = new List<EnEnsayoDTO>();
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

        public List<EnEnsayoDTO> GetByCriteria()
        {
            List<EnEnsayoDTO> entitys = new List<EnEnsayoDTO>();
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

        public List<EnEnsayoDTO> ListaDetalleFiltro(string emprcodi, string equicodi, string estados, DateTime fechaInicio, DateTime fechaFin, int nroPaginas, int pageSize)
        {
            List<EnEnsayoDTO> entitys = new List<EnEnsayoDTO>();
            string query = string.Format(helper.SqlListaDetalleFiltro, emprcodi, equicodi, estados, fechaInicio.ToString(ConstantesBase.FormatoFecha), fechaFin.ToString(ConstantesBase.FormatoFecha), nroPaginas, pageSize);
            DbCommand command = dbProvider.GetSqlStringCommand(query);
            EnEnsayoDTO entity = new EnEnsayoDTO();
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = helper.Create(dr);
                    int iEmprnomb = dr.GetOrdinal("Emprnomb");
                    int iEquinomb = dr.GetOrdinal("central");
                    int iEstadonombre = dr.GetOrdinal("Estadonombre");
                    int iEstadocolor = dr.GetOrdinal("Estadocolor");
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);
                    if (!dr.IsDBNull(iEquinomb)) entity.Equinomb = dr.GetString(iEquinomb);
                    if (!dr.IsDBNull(iEstadonombre)) entity.Estadonombre = dr.GetString(iEstadonombre);
                    if (!dr.IsDBNull(iEstadocolor)) entity.Estadocolor = dr.GetString(iEstadocolor);
                    entitys.Add(entity);

                }
            }
            return entitys;
        }

        public List<EnEnsayoDTO> ListaDetalleFiltroXls(string emprcodi, string equicodi, string estados, DateTime fechaInicio, DateTime fechaFin)
        {
            List<EnEnsayoDTO> entitys = new List<EnEnsayoDTO>();
            string query = string.Format(helper.SqlListaDetalleFiltroXls, emprcodi, equicodi, estados, fechaInicio.ToString(ConstantesBase.FormatoFecha), fechaFin.ToString(ConstantesBase.FormatoFecha));
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    EnEnsayoDTO entity = new EnEnsayoDTO();
                    int iEnsayocodi = dr.GetOrdinal("Ensayocodi");
                    int iEmprnomb = dr.GetOrdinal("Empresa");
                    int iEquinomb = dr.GetOrdinal("Central");
                    int iUnidadnomb = dr.GetOrdinal("Unidad");
                    int iGruponomb = dr.GetOrdinal("Modo");
                    int iEstadonombre = dr.GetOrdinal("Nombestado");
                    int iEstadocolor = dr.GetOrdinal("colestado");
                    int iEnsayofecha = dr.GetOrdinal("fecha");
                    if (!dr.IsDBNull(iEnsayocodi)) entity.Ensayocodi = Convert.ToInt32(dr.GetValue(iEnsayocodi));
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);
                    if (!dr.IsDBNull(iEquinomb)) entity.Equinomb = dr.GetString(iEquinomb);
                    if (!dr.IsDBNull(iUnidadnomb)) entity.Unidadnomb = dr.GetString(iUnidadnomb);
                    if (!dr.IsDBNull(iGruponomb)) entity.Ensayomodoper = dr.GetString(iGruponomb);
                    if (!dr.IsDBNull(iEstadonombre)) entity.Estadonombre = dr.GetString(iEstadonombre);
                    if (!dr.IsDBNull(iEstadocolor)) entity.Estadocolor = dr.GetString(iEstadocolor);
                    if (!dr.IsDBNull(iEnsayofecha)) entity.Ensayofecha = dr.GetDateTime(iEnsayofecha);
                    entitys.Add(entity);

                }
            }
            return entitys;
        }


        public int ObtenerTotalListaEnsayo(string emprcodi, string grupocodi, string estenvcodi, DateTime fechaInicio, DateTime fechaFin)
        {
            string sqlTotal = string.Format(helper.SqlTotalListaEnsayo, emprcodi, grupocodi, estenvcodi,
               fechaInicio.ToString(ConstantesBase.FormatoFecha), fechaFin.ToString(ConstantesBase.FormatoFecha));
            DbCommand command = dbProvider.GetSqlStringCommand(sqlTotal);
            object result = dbProvider.ExecuteScalar(command);
            int total = 0;
            if (result != null) total = Convert.ToInt32(result);
            return total;
        }

        public void SaveEnsMaster(int maxCodIngreso, int icodiensayo, DateTime lastdate, string lastuser)
        {
            string insertQuery = string.Format(helper.SqlSaveEnsayoMaster, maxCodIngreso, icodiensayo, ((DateTime)lastdate).ToString(ConstantesBase.FormatoFechaExtendido), lastuser);
            DbCommand command = dbProvider.GetSqlStringCommand(insertQuery);
            dbProvider.ExecuteNonQuery(command);
        }

        public int GetMaxIdEnsMaster()
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxIdEnMaster);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);
            return id;
        }


    }
}
