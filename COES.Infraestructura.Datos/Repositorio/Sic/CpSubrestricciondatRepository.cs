using COES.Base.Core;
using COES.Dominio.DTO.Sic;
using COES.Dominio.Interfaces.Sic;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace COES.Infraestructura.Datos.Repositorio.Sic
{
    /// <summary>
    /// Clase de acceso a datos de la tabla CP_SUBSRESTRICDAT
    /// </summary>
    public class CpSubrestricdatRepository : RepositoryBase, ICpSubsrestricdatRepository
    {
        public CpSubrestricdatRepository(string strConn)
            : base(strConn)
        {
        }

        CpSubsrestricdatHelper helper = new CpSubsrestricdatHelper();

        public List<CpSubrestricdatDTO> ListarDatosRestriccion(int topcodi, int restriccodi)
        {
            List<CpSubrestricdatDTO> entitys = new List<CpSubrestricdatDTO>();
            string query = string.Format(helper.SqlListarDatosRestriccion, topcodi, restriccodi);
            DbCommand command = dbProvider.GetSqlStringCommand(query);
            CpSubrestricdatDTO entity;
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = helper.Create(dr);
                    int iSrestnombregams = dr.GetOrdinal("Srestnombregams");
                    entity.Srestnombregams = dr.GetString(iSrestnombregams);
                    entitys.Add(entity);

                }
            }
            return entitys;
        }

        public List<CpSubrestricdatDTO> List(int topcodi)
        {
            List<CpSubrestricdatDTO> entitys = new List<CpSubrestricdatDTO>();
            string queryString = string.Format(helper.SqlList, topcodi);
            DbCommand command = dbProvider.GetSqlStringCommand(queryString);
            string CategoriaID = "catcodi";
            CpSubrestricdatDTO entity = new CpSubrestricdatDTO();
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = new CpSubrestricdatDTO();
                    entity = helper.Create(dr);
                    int iCategoriaID = dr.GetOrdinal(CategoriaID);
                    if (!dr.IsDBNull(iCategoriaID)) entity.Catcodi = Convert.ToInt16(dr.GetValue(iCategoriaID));
                    int iRecurconsideragams = dr.GetOrdinal(helper.Recurconsideragams);
                    if (!dr.IsDBNull(iRecurconsideragams)) entity.Recurconsideragams = Convert.ToInt32(dr.GetValue(iRecurconsideragams));
                    entity.Indiceorden = entity.Srestfecha.Value.Hour;
                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<CpSubrestricdatDTO> ListadeSubRestriccionCategoria(int topcodi, int catcodi)
        {
            string query = string.Format(helper.SqlListadeSubRestriccionCategoria, topcodi, catcodi);
            List<CpSubrestricdatDTO> entitys = new List<CpSubrestricdatDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(query);
            CpSubrestricdatDTO entity = new CpSubrestricdatDTO();
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = new CpSubrestricdatDTO();
                    entity = helper.Create(dr);
                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        //Yupana Continuo
        public List<CpSubrestricdatDTO> ListarRecursosEnSubRestriccion(int pTopologia, int sRestriccion)
        {
            List<CpSubrestricdatDTO> entitys = new List<CpSubrestricdatDTO>();
            string queryString = string.Format(helper.SqlListRecursoEnSubRestriccion, pTopologia, sRestriccion);

            DbCommand command = dbProvider.GetSqlStringCommand(queryString);
            string RecursoIDSicoes = "RecursoIDSicoes";
            string RecursoNombre = "RecursoNombre";
            CpSubrestricdatDTO entity = new CpSubrestricdatDTO();
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = new CpSubrestricdatDTO();
                    entity = helper.Create(dr);
                    int iRecursoNombre = dr.GetOrdinal(RecursoNombre);
                    if (!dr.IsDBNull(iRecursoNombre)) entity.Recurnombre = dr.GetString(iRecursoNombre);
                    int iRecursoIDSicoes = dr.GetOrdinal(RecursoIDSicoes);
                    if (!dr.IsDBNull(iRecursoIDSicoes)) entity.Recurcodisicoes = Convert.ToInt32(dr.GetValue(iRecursoIDSicoes));
                    int iRecurconsideragams = dr.GetOrdinal(helper.Recurconsideragams);
                    if (!dr.IsDBNull(iRecurconsideragams)) entity.Recurconsideragams = Convert.ToInt32(dr.GetValue(iRecurconsideragams));
                    entitys.Add(entity);
                }
            }

            return entitys;
        }
       
        public List<CpSubrestricdatDTO> ListarDatosSubRestriccion(int topcodi, int srestcodi)
        {
            List<CpSubrestricdatDTO> entitys = new List<CpSubrestricdatDTO>();
            string query = string.Format(helper.SqlListarDatosSubRestriccion, topcodi, srestcodi);
            DbCommand command = dbProvider.GetSqlStringCommand(query);
            CpSubrestricdatDTO entity;
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = helper.Create(dr);
                    int iRecurconsideragams = dr.GetOrdinal(helper.Recurconsideragams);
                    entity.Recurconsideragams = Convert.ToInt32(dr.GetValue(iRecurconsideragams));
                    if (entity.Srestfecha != null)
                        entity.Indiceorden = entity.Srestfecha.Value.Hour;
                    entitys.Add(entity);

                }
            }
            return entitys;
        }

        public void CrearCopia(int topcodi1, int topcodi2)
        {
            string query = string.Format(helper.SqlCrearCopia, topcodi1, topcodi2);
            DbCommand command = dbProvider.GetSqlStringCommand(query);
            dbProvider.ExecuteNonQuery(command);
        }
    }
}
