using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Common;
using COES.Dominio.DTO.Sic;
using COES.Dominio.Interfaces.Sic;
using COES.Base.Core;
using COES.Infraestructura.Datos.Helper.Sic;
using System.Data;

namespace COES.Infraestructura.Datos.Repositorio.Sic
{
    public class PrnPrdTransversalRepository : RepositoryBase, IPrnPrdTransversalRepository
    {
        public PrnPrdTransversalRepository(string strConn) : base(strConn)
        {

        }

        PrnPrdTransversalHelper helper = new PrnPrdTransversalHelper();
       

        public void Save(PrnPrdTransversalDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Prdtrncodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Prdtrnetqnomb, DbType.String, entity.Prdtrnetqnomb);
            dbProvider.AddInParameter(command, helper.Grupocodi, DbType.Int32, entity.Grupocodi);
            dbProvider.AddInParameter(command, helper.Prnvercodi, DbType.Int32, entity.Prnvercodi);
            dbProvider.AddInParameter(command, helper.Prrucodi, DbType.Int32, entity.Prrucodi);         
            dbProvider.AddInParameter(command, helper.Prdtrnflagesmanual, DbType.String, entity.Prdtrnflagesmanual ? "1" : "0");

            dbProvider.AddInParameter(command, helper.H1, DbType.Decimal, entity.H1);
            dbProvider.AddInParameter(command, helper.H2, DbType.Decimal, entity.H2);
            dbProvider.AddInParameter(command, helper.H3, DbType.Decimal, entity.H3);
            dbProvider.AddInParameter(command, helper.H4, DbType.Decimal, entity.H4);
            dbProvider.AddInParameter(command, helper.H5, DbType.Decimal, entity.H5);
            dbProvider.AddInParameter(command, helper.H6, DbType.Decimal, entity.H6);
            dbProvider.AddInParameter(command, helper.H7, DbType.Decimal, entity.H7);
            dbProvider.AddInParameter(command, helper.H8, DbType.Decimal, entity.H8);
            dbProvider.AddInParameter(command, helper.H9, DbType.Decimal, entity.H9);
            dbProvider.AddInParameter(command, helper.H10, DbType.Decimal, entity.H10);
            dbProvider.AddInParameter(command, helper.H11, DbType.Decimal, entity.H11);
            dbProvider.AddInParameter(command, helper.H12, DbType.Decimal, entity.H12);
            dbProvider.AddInParameter(command, helper.H13, DbType.Decimal, entity.H13);
            dbProvider.AddInParameter(command, helper.H14, DbType.Decimal, entity.H14);
            dbProvider.AddInParameter(command, helper.H15, DbType.Decimal, entity.H15);
            dbProvider.AddInParameter(command, helper.H16, DbType.Decimal, entity.H16);
            dbProvider.AddInParameter(command, helper.H17, DbType.Decimal, entity.H17);
            dbProvider.AddInParameter(command, helper.H18, DbType.Decimal, entity.H18);
            dbProvider.AddInParameter(command, helper.H19, DbType.Decimal, entity.H19);
            dbProvider.AddInParameter(command, helper.H20, DbType.Decimal, entity.H20);
            dbProvider.AddInParameter(command, helper.H21, DbType.Decimal, entity.H21);
            dbProvider.AddInParameter(command, helper.H22, DbType.Decimal, entity.H22);
            dbProvider.AddInParameter(command, helper.H23, DbType.Decimal, entity.H23);
            dbProvider.AddInParameter(command, helper.H24, DbType.Decimal, entity.H24);
            dbProvider.AddInParameter(command, helper.H25, DbType.Decimal, entity.H25);
            dbProvider.AddInParameter(command, helper.H26, DbType.Decimal, entity.H26);
            dbProvider.AddInParameter(command, helper.H27, DbType.Decimal, entity.H27);
            dbProvider.AddInParameter(command, helper.H28, DbType.Decimal, entity.H28);
            dbProvider.AddInParameter(command, helper.H29, DbType.Decimal, entity.H29);
            dbProvider.AddInParameter(command, helper.H30, DbType.Decimal, entity.H30);
            dbProvider.AddInParameter(command, helper.H31, DbType.Decimal, entity.H31);
            dbProvider.AddInParameter(command, helper.H32, DbType.Decimal, entity.H32);
            dbProvider.AddInParameter(command, helper.H33, DbType.Decimal, entity.H33);
            dbProvider.AddInParameter(command, helper.H34, DbType.Decimal, entity.H34);
            dbProvider.AddInParameter(command, helper.H35, DbType.Decimal, entity.H35);
            dbProvider.AddInParameter(command, helper.H36, DbType.Decimal, entity.H36);
            dbProvider.AddInParameter(command, helper.H37, DbType.Decimal, entity.H37);
            dbProvider.AddInParameter(command, helper.H38, DbType.Decimal, entity.H38);
            dbProvider.AddInParameter(command, helper.H39, DbType.Decimal, entity.H39);
            dbProvider.AddInParameter(command, helper.H40, DbType.Decimal, entity.H40);
            dbProvider.AddInParameter(command, helper.H41, DbType.Decimal, entity.H41);
            dbProvider.AddInParameter(command, helper.H42, DbType.Decimal, entity.H42);
            dbProvider.AddInParameter(command, helper.H43, DbType.Decimal, entity.H43);
            dbProvider.AddInParameter(command, helper.H44, DbType.Decimal, entity.H44);
            dbProvider.AddInParameter(command, helper.H45, DbType.Decimal, entity.H45);
            dbProvider.AddInParameter(command, helper.H46, DbType.Decimal, entity.H46);
            dbProvider.AddInParameter(command, helper.H47, DbType.Decimal, entity.H47);
            dbProvider.AddInParameter(command, helper.H48, DbType.Decimal, entity.H48);

            dbProvider.ExecuteNonQuery(command);
        }

        public List<PrnPrdTransversalDTO> GetRelacionesPorNombre(string Nombre)
        {
            List<PrnPrdTransversalDTO> lista = new List<PrnPrdTransversalDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetRelacionesPorNombre);
            dbProvider.AddInParameter(command, helper.Prdtrnetqnomb, DbType.String, Nombre);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    lista.Add(helper.Create(dr));
                }
            }

            return lista;
        }

        public List<PrnPrdTransversalDTO> GetPerdidaPorBarraCP(int idBarra)
        {
            List<PrnPrdTransversalDTO> lista = new List<PrnPrdTransversalDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetPerdidaPorBarraCP);
            dbProvider.AddInParameter(command, helper.Grupocodi, DbType.Int32, idBarra);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    lista.Add(helper.Create(dr));
                }
            }

            return lista;
        }

        
        // ---------------------------------------------------------------------------------------------------------------
        public List<PrnPrdTransversalDTO> List()
        {
            List<PrnPrdTransversalDTO> entitys = new List<PrnPrdTransversalDTO>();
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

        public List<PrGrupoDTO> ListaBarrasPerdidasTransversales()
        {
            List<PrGrupoDTO> entitys = new List<PrGrupoDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListaBarrasPerdidasTransversales);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    PrGrupoDTO entity = new PrGrupoDTO();

                    int iGrupocodi = dr.GetOrdinal(helper.Grupocodi);
                    if (!dr.IsDBNull(iGrupocodi)) entity.Grupocodi = Convert.ToInt32(dr.GetValue(iGrupocodi));

                    int iGruponomb = dr.GetOrdinal(helper.Gruponomb);
                    if (!dr.IsDBNull(iGruponomb)) entity.Gruponomb = dr.GetString(iGruponomb);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<PrnPrdTransversalDTO> ListPerdidasTransvBarraFormulas()
        {
            List<PrnPrdTransversalDTO> entitys = new List<PrnPrdTransversalDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListPerdidasTransvBarraFormulas);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    PrnPrdTransversalDTO entity = new PrnPrdTransversalDTO();

                    int iPrdtrncodi = dr.GetOrdinal(helper.Prdtrncodi);
                    if (!dr.IsDBNull(iPrdtrncodi)) entity.Prdtrncodi = Convert.ToInt32(dr.GetValue(iPrdtrncodi));

                    int iPrdtrnetqnomb = dr.GetOrdinal(helper.Prdtrnetqnomb);
                    if (!dr.IsDBNull(iPrdtrnetqnomb)) entity.Prdtrnetqnomb = dr.GetString(iPrdtrnetqnomb);

                    int iGrupocodi = dr.GetOrdinal(helper.Grupocodi);
                    if (!dr.IsDBNull(iGrupocodi)) entity.Grupocodi = Convert.ToInt32(dr.GetValue(iGrupocodi));

                    int iGruponomb = dr.GetOrdinal(helper.Gruponomb);
                    if (!dr.IsDBNull(iGruponomb)) entity.Gruponomb = dr.GetString(iGruponomb);

                    int iPrnvercodi = dr.GetOrdinal(helper.Prnvercodi);
                    if (!dr.IsDBNull(iPrnvercodi)) entity.Prnvercodi = Convert.ToInt32(dr.GetValue(iPrnvercodi));

                    int iPrrucodi = dr.GetOrdinal(helper.Prrucodi);
                    if (!dr.IsDBNull(iPrrucodi)) entity.Prrucodi = dr.GetInt32(iPrrucodi);

                    int iFlagesManual = dr.GetOrdinal(helper.Prdtrnflagesmanual);
                    if (!dr.IsDBNull(iFlagesManual)) entity.Prdtrnflagesmanual = Convert.ToInt32(dr.GetValue(iFlagesManual)) == 1 ? true : false;

                    int iPrruabrev = dr.GetOrdinal(helper.Prruabrev);
                    if (!dr.IsDBNull(iPrruabrev)) entity.Prruabrev = dr.GetString(iPrruabrev);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }


        // -------------------------------------------------------------------------------------------------------
        public List<PrnPrdTransversalDTO> ListBarrasPerdidasTransversales(int Prdtrncodi)
        {
            List<PrnPrdTransversalDTO> entitys = new List<PrnPrdTransversalDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListBarrasPerdidasTransversales);
            dbProvider.AddInParameter(command, helper.Prdtrncodi, DbType.Int32, Prdtrncodi);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    PrnPrdTransversalDTO entity = new PrnPrdTransversalDTO();

                    int iGrupocodi = dr.GetOrdinal(helper.Grupocodi);
                    if (!dr.IsDBNull(iGrupocodi)) entity.Grupocodi = Convert.ToInt32(dr.GetValue(iGrupocodi));

                    int iGruponomb = dr.GetOrdinal(helper.Gruponomb);
                    if (!dr.IsDBNull(iGruponomb)) entity.Gruponomb = dr.GetString(iGruponomb);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<PrnPrdTransversalDTO> GetPerdidasTransversalesByNombre(string Prdtrnetqnomb)
        {
            List<PrnPrdTransversalDTO> entitys = new List<PrnPrdTransversalDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetPerdidasTransversalesByNombre);
            dbProvider.AddInParameter(command, helper.Prdtrnetqnomb, DbType.String, Prdtrnetqnomb);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }

        public void DeleteRelacionesPerdidasTransv(string Prdtrnetqnomb)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDeleteRelaciones);

            dbProvider.AddInParameter(command, helper.Prdtrnetqnomb, DbType.String, Prdtrnetqnomb);

            dbProvider.ExecuteNonQuery(command);
        }
        // ---------------------------------------------------------------------------------------------------------------

        public List<PrnPrdTransversalDTO> ObtenerPerdidaPorBarraCP(int grupocodi)
        {
            PrnPrdTransversalDTO entity = new PrnPrdTransversalDTO();
            List<PrnPrdTransversalDTO> entities = new List<PrnPrdTransversalDTO>();
            string query = string.Format(helper.SqlObtenerPerdidaPorBarraCP, grupocodi);
            DbCommand command = dbProvider.GetSqlStringCommand(query);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = new PrnPrdTransversalDTO();

                    int iGrupocodi = dr.GetOrdinal(helper.Grupocodi);
                    if (!dr.IsDBNull(iGrupocodi)) entity.Grupocodi = Convert.ToInt32(dr.GetValue(iGrupocodi));

                    int iPrrucodi = dr.GetOrdinal(helper.Prrucodi);
                    if (!dr.IsDBNull(iPrrucodi)) entity.Prrucodi = Convert.ToInt32(dr.GetValue(iPrrucodi));

                    int iPrdtrnflagesmanual = dr.GetOrdinal(helper.Prdtrnflagesmanual);
                    if (!dr.IsDBNull(iPrdtrnflagesmanual))
                        entity.Prdtrnflagesmanual = Convert.ToInt32(dr.GetValue(iPrdtrnflagesmanual)) == 1 ? true : false;

                    for (int i = 1; i <= 48; i++)
                    {
                        int iOrdinal = dr.GetOrdinal("H" + i);
                        if (!dr.IsDBNull(iOrdinal))
                            entity.GetType().GetProperty("H" + i).SetValue(entity, dr.GetDecimal(iOrdinal));
                    }

                    entities.Add(entity);
                }
            }
            return entities;
        }
    }
}
