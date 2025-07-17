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
    /// Clase de acceso a datos de la tabla PR_GRUPO_EQUIPO_VAL
    /// </summary>
    public class PrGrupoEquipoValRepository: RepositoryBase, IPrGrupoEquipoValRepository
    {
        public PrGrupoEquipoValRepository(string strConn): base(strConn)
        {
        }

        PrGrupoEquipoValHelper helper = new PrGrupoEquipoValHelper();

        public void Save(PrGrupoEquipoValDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Grupocodi, DbType.Int32, entity.Grupocodi);
            dbProvider.AddInParameter(command, helper.Concepcodi, DbType.Int32, entity.Concepcodi);
            dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, entity.Equicodi);
            dbProvider.AddInParameter(command, helper.Greqvafechadat, DbType.DateTime, entity.Greqvafechadat);
            dbProvider.AddInParameter(command, helper.Greqvaformuladat, DbType.String, entity.Greqvaformuladat);
            dbProvider.AddInParameter(command, helper.Greqvadeleted, DbType.Int32, entity.Greqvadeleted);
            dbProvider.AddInParameter(command, helper.Greqvausucreacion, DbType.String, entity.Greqvausucreacion);
            dbProvider.AddInParameter(command, helper.Greqvafeccreacion, DbType.DateTime, entity.Greqvafeccreacion);
            dbProvider.AddInParameter(command, helper.Greqvausumodificacion, DbType.String, entity.Greqvausumodificacion);
            dbProvider.AddInParameter(command, helper.Greqvafecmodificacion, DbType.DateTime, entity.Greqvafecmodificacion);
            dbProvider.AddInParameter(command, helper.Greqvacomentario, DbType.String, entity.Greqvacomentario);
            dbProvider.AddInParameter(command, helper.Greqvasustento, DbType.String, entity.Greqvasustento);
            dbProvider.AddInParameter(command, helper.Greqvacheckcero, DbType.Int32, entity.Greqvacheckcero);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Update(PrGrupoEquipoValDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Greqvaformuladat, DbType.String, entity.Greqvaformuladat);
            dbProvider.AddInParameter(command, helper.Greqvadeleted2, DbType.Int32, entity.Greqvadeleted2);
            dbProvider.AddInParameter(command, helper.Greqvausumodificacion, DbType.String, entity.Greqvausumodificacion);
            dbProvider.AddInParameter(command, helper.Greqvafecmodificacion, DbType.DateTime, entity.Greqvafecmodificacion);
            dbProvider.AddInParameter(command, helper.Greqvacomentario, DbType.String, entity.Greqvacomentario);
            dbProvider.AddInParameter(command, helper.Greqvasustento, DbType.String, entity.Greqvasustento);
            dbProvider.AddInParameter(command, helper.Greqvacheckcero, DbType.Int32, entity.Greqvacheckcero);

            dbProvider.AddInParameter(command, helper.Grupocodi, DbType.Int32, entity.Grupocodi);
            dbProvider.AddInParameter(command, helper.Concepcodi, DbType.Int32, entity.Concepcodi);
            dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, entity.Equicodi);
            dbProvider.AddInParameter(command, helper.Greqvafechadat, DbType.DateTime, entity.Greqvafechadat);
            dbProvider.AddInParameter(command, helper.Greqvadeleted, DbType.Int32, entity.Greqvadeleted);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int grupocodi, int concepcodi, int equicodi, DateTime greqvafechadat, int greqvadeleted)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Grupocodi, DbType.Int32, grupocodi);
            dbProvider.AddInParameter(command, helper.Concepcodi, DbType.Int32, concepcodi);
            dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, equicodi);
            dbProvider.AddInParameter(command, helper.Greqvafechadat, DbType.DateTime, greqvafechadat);
            dbProvider.AddInParameter(command, helper.Greqvadeleted, DbType.Int32, greqvadeleted);

            dbProvider.ExecuteNonQuery(command);
        }

        public PrGrupoEquipoValDTO GetById(int grupocodi, int concepcodi, int equicodi, DateTime greqvafechadat, int greqvadeleted)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Grupocodi, DbType.Int32, grupocodi);
            dbProvider.AddInParameter(command, helper.Concepcodi, DbType.Int32, concepcodi);
            dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, equicodi);
            dbProvider.AddInParameter(command, helper.Greqvafechadat, DbType.DateTime, greqvafechadat);
            dbProvider.AddInParameter(command, helper.Greqvadeleted, DbType.Int32, greqvadeleted);
            PrGrupoEquipoValDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<PrGrupoEquipoValDTO> List()
        {
            List<PrGrupoEquipoValDTO> entitys = new List<PrGrupoEquipoValDTO>();
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

        public List<PrGrupoEquipoValDTO> GetByCriteria()
        {
            List<PrGrupoEquipoValDTO> entitys = new List<PrGrupoEquipoValDTO>();
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

        public List<PrGrupoEquipoValDTO> GetValorPropiedadDetalle(int idGrupo,int idConcepto)
        {
            List<PrGrupoEquipoValDTO> entitys = new List<PrGrupoEquipoValDTO>();
            string sqlQuery = string.Format(helper.SqlGetValorPropiedadDetalle, idGrupo, idConcepto);
            DbCommand command = dbProvider.GetSqlStringCommand(sqlQuery);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }

        //-- inicio pruebas aleatorias
        /// <summary>
        /// Permite obtener la propiedad de un equipo por concepto y fecha
        /// </summary>
        /// <param name="idEquipo"></param>
        /// <param name="idConcepto"></param>
        /// <param name="fecha"></param>
        /// <returns></returns>
        public decimal GetValorPropiedadDetalleEquipo(int idEquipo, int idConcepto, DateTime fecha)
        {
            try
            {
                List<PrGrupoEquipoValDTO> entitys = new List<PrGrupoEquipoValDTO>();
                string sqlQuery = string.Format(helper.SqlGetValorPropiedadDetalleEquipo, idEquipo, idConcepto, fecha.ToString(ConstantesBase.FormatoFecha));
                DbCommand command = dbProvider.GetSqlStringCommand(sqlQuery);
                using (IDataReader dr = dbProvider.ExecuteReader(command))
                {
                    while (dr.Read())
                    {
                        PrGrupoEquipoValDTO valor = helper.Create(dr);
                        decimal valorDat = Convert.ToDecimal(valor.Greqvaformuladat);
                        return valorDat;
                    }
                }
            }
            catch
            {

            }

            return -1;
        }
        //-- fin pruebas aleatorias
        
        public List<PrGrupoEquipoValDTO> ListarHistoricoValores(string concepcodi, string idEquipo, string idGrupo)
        {
            List<PrGrupoEquipoValDTO> entitys = new List<PrGrupoEquipoValDTO>();
            string sql = string.Format(helper.SqlHistoricoValores, concepcodi, idEquipo, idGrupo);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    var entity = helper.Create(dr);

                    int iConcepabrev = dr.GetOrdinal(helper.Concepabrev);
                    if (!dr.IsDBNull(iConcepabrev)) entity.Concepabrev = dr.GetString(iConcepabrev);

                    int iConceppadre = dr.GetOrdinal(helper.Conceppadre);
                    if (!dr.IsDBNull(iConceppadre)) entity.Conceppadre = Convert.ToInt32(dr.GetValue(iConceppadre));

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<PrGrupoEquipoValDTO> ListarPrGrupoEquipoValVigente(DateTime fechaVigencia, string concepcodi, string idEquipo, string idGrupo)
        {
            List<PrGrupoEquipoValDTO> entitys = new List<PrGrupoEquipoValDTO>();
            string sql = string.Format(helper.SqlListarPrGrupoEquipoValVigente, fechaVigencia.ToString(ConstantesBase.FormatoFecha), concepcodi, idEquipo, idGrupo);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    var entity = helper.Create(dr);

                    int iConcepabrev = dr.GetOrdinal(helper.Concepabrev);
                    if (!dr.IsDBNull(iConcepabrev)) entity.Concepabrev = dr.GetString(iConcepabrev);

                    int iConceppadre = dr.GetOrdinal(helper.Conceppadre);
                    if (!dr.IsDBNull(iConceppadre)) entity.Conceppadre = Convert.ToInt32(dr.GetValue(iConceppadre));

                    entitys.Add(entity);
                }
            }

            return entitys;
        }
    }
}
