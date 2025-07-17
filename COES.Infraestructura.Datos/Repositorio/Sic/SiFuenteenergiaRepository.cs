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
    /// Clase de acceso a datos de la tabla SI_FUENTEENERGIA
    /// </summary>
    public class SiFuenteenergiaRepository : RepositoryBase, ISiFuenteenergiaRepository
    {
        public SiFuenteenergiaRepository(string strConn)
            : base(strConn)
        {
        }

        SiFuenteenergiaHelper helper = new SiFuenteenergiaHelper();

        public void Update(SiFuenteenergiaDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Fenergabrev, DbType.String, entity.Fenergabrev);
            dbProvider.AddInParameter(command, helper.Fenergnomb, DbType.String, entity.Fenergnomb);
            dbProvider.AddInParameter(command, helper.Tgenercodi, DbType.Int32, entity.Tgenercodi);
            dbProvider.AddInParameter(command, helper.Fenergcolor, DbType.String, entity.Fenergcolor);
            dbProvider.AddInParameter(command, helper.Osinergcodi, DbType.String, entity.Osinergcodi);
            dbProvider.AddInParameter(command, helper.Estcomcodi, DbType.Int32, entity.Estcomcodi);
            dbProvider.AddInParameter(command, helper.Tinfcoes, DbType.Int32, entity.Tinfcoes);
            dbProvider.AddInParameter(command, helper.Tinfosi, DbType.Int32, entity.Tinfosi);

            dbProvider.AddInParameter(command, helper.Fenergcodi, DbType.Int32, entity.Fenergcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public SiFuenteenergiaDTO GetById(int fenergcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Fenergcodi, DbType.Int32, fenergcodi);
            SiFuenteenergiaDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<SiFuenteenergiaDTO> List()
        {
            List<SiFuenteenergiaDTO> entitys = new List<SiFuenteenergiaDTO>();
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

        public List<SiFuenteenergiaDTO> GetByCriteria()
        {
            List<SiFuenteenergiaDTO> entitys = new List<SiFuenteenergiaDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByCriteria);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    SiFuenteenergiaDTO entity = helper.Create(dr);

                    int iTgenernomb = dr.GetOrdinal(helper.Tgenernomb);
                    if (!dr.IsDBNull(iTgenernomb)) entity.Tgenernomb = dr.GetString(iTgenernomb);

                    int iTinfosiabrev = dr.GetOrdinal(helper.Tinfosiabrev);
                    if (!dr.IsDBNull(iTinfosiabrev)) entity.Tinfosiabrev = dr.GetString(iTinfosiabrev);

                    int iTinfcoesabrev = dr.GetOrdinal(helper.Tinfcoesabrev);
                    if (!dr.IsDBNull(iTinfcoesabrev)) entity.Tinfcoesabrev = dr.GetString(iTinfcoesabrev);

                    int iTinfosidesc = dr.GetOrdinal(helper.Tinfosidesc);
                    if (!dr.IsDBNull(iTinfosidesc)) entity.Tinfosidesc = dr.GetString(iTinfosidesc);

                    int iTinfcoesdesc = dr.GetOrdinal(helper.Tinfcoesdesc);
                    if (!dr.IsDBNull(iTinfcoesdesc)) entity.Tinfcoesdesc = dr.GetString(iTinfcoesdesc);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        #region PR5
        public List<SiFuenteenergiaDTO> ListTipoCombustibleXTipoCentral(string famcodi, string idEmpresa)
        {
            List<SiFuenteenergiaDTO> entitys = new List<SiFuenteenergiaDTO>();
            string query = string.Format(helper.SqlTipoCombustibleXTipoCentral, famcodi, idEmpresa);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }

        public List<SiFuenteenergiaDTO> ListTipoCombustibleXEquipo(string equicodi)
        {
            List<SiFuenteenergiaDTO> entitys = new List<SiFuenteenergiaDTO>();
            string query = string.Format(helper.SqlTipoCombustibleXEquipo, equicodi);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    SiFuenteenergiaDTO entity = helper.Create(dr);

                    int iGrupocomb = dr.GetOrdinal(this.helper.Grupocomb);
                    if (!dr.IsDBNull(iGrupocomb)) entity.Grupocomb = dr.GetString(iGrupocomb);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        #endregion

        #region SIOSEIN        
        public List<SiFuenteenergiaDTO> PromedioEnergiaActivaPorTipodeRecursoYrangoFechas(string fechaI, string fechaF)
        {
            List<SiFuenteenergiaDTO> entitys = new List<SiFuenteenergiaDTO>();
            string query = string.Format(helper.SqlPromedioEnergiaActivaPorTipodeRecursoYrangoFechas, fechaI, fechaF);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    SiFuenteenergiaDTO entity = new SiFuenteenergiaDTO();

                    int iFenergcodi = dr.GetOrdinal(helper.Fenergcodi);
                    if (!dr.IsDBNull(iFenergcodi)) entity.Fenergcodi = Convert.ToInt32(dr.GetValue(iFenergcodi));

                    int iFenergnomb = dr.GetOrdinal(helper.Fenergnomb);
                    if (!dr.IsDBNull(iFenergnomb)) entity.Fenergnomb = dr.GetString(iFenergnomb);

                    int iPromedio = dr.GetOrdinal(this.helper.Promedio);
                    if (!dr.IsDBNull(iPromedio)) entity.Promedio = dr.GetDecimal(iPromedio);

                    entitys.Add(entity);
                }
            }
            return entitys;
        }
        #endregion
    }
}
