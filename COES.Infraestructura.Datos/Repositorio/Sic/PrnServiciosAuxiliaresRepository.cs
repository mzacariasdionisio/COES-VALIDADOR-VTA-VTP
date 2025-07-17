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
    public class PrnServiciosAuxiliaresRepository : RepositoryBase, IPrnServiciosAuxiliaresRepository
    {
        public PrnServiciosAuxiliaresRepository(string strConn) : base(strConn)
        {

        }

        PrnServiciosAuxiliaresHelper helper = new PrnServiciosAuxiliaresHelper();


        public void Save(PrnServiciosAuxiliaresDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);

            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Prnserauxcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.PrGrupocodi, DbType.Int32, entity.Grupocodi);
            dbProvider.AddInParameter(command, helper.Prrucodi, DbType.Int32, entity.Prrucodi);
            dbProvider.AddInParameter(command, helper.FlagesManual, DbType.String, entity.Prnauxflagesmanual ? "1" : "0");
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

        public void Delete(int prdtrncodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Prnserauxcodi, DbType.Int32, prdtrncodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public List<PrnServiciosAuxiliaresDTO> List()
        {
            List<PrnServiciosAuxiliaresDTO> entitys = new List<PrnServiciosAuxiliaresDTO>();
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

        // -----------------------------------------------------------------------------------------------------------------
        public List<PrnServiciosAuxiliaresDTO> ListBarraFormulas()
        {
            List<PrnServiciosAuxiliaresDTO> entitys = new List<PrnServiciosAuxiliaresDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListBarraFormulas);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    PrnServiciosAuxiliaresDTO entity = new PrnServiciosAuxiliaresDTO();

                    int iGrupocodi = dr.GetOrdinal(helper.Grupocodi);
                    if (!dr.IsDBNull(iGrupocodi)) entity.Grupocodi = Convert.ToInt32(dr.GetValue(iGrupocodi));

                    int iGruponomb = dr.GetOrdinal(helper.Gruponomb);
                    if (!dr.IsDBNull(iGruponomb)) entity.Gruponomb = dr.GetString(iGruponomb);

                    int iPrruabrev = dr.GetOrdinal(helper.Prruabrev);
                    if (!dr.IsDBNull(iPrruabrev)) entity.Prruabrev = dr.GetString(iPrruabrev);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<PrnServiciosAuxiliaresDTO> GetServiciosAuxiliaresByGrupo(int PrGrupo)
        {
            List<PrnServiciosAuxiliaresDTO> entitys = new List<PrnServiciosAuxiliaresDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetServiciosAuxiliaresByGrupo);
            dbProvider.AddInParameter(command, helper.PrGrupocodi, DbType.Int32, PrGrupo);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }

        public List<MePerfilRuleDTO> ListFormulas()
        {
            List<MePerfilRuleDTO> entitys = new List<MePerfilRuleDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListFormulas);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    MePerfilRuleDTO entity = new MePerfilRuleDTO();

                    int iPrrucodi = dr.GetOrdinal(helper.Prrucodi);
                    if (!dr.IsDBNull(iPrrucodi)) entity.Prrucodi = Convert.ToInt32(dr.GetValue(iPrrucodi));

                    int iPrruabrev = dr.GetOrdinal(helper.Prruabrev);
                    if (!dr.IsDBNull(iPrruabrev)) entity.Prruabrev = dr.GetString(iPrruabrev);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<MePerfilRuleDTO> ListFormulasRelaciones(int Grupocodi)
        {
            List<MePerfilRuleDTO> entitys = new List<MePerfilRuleDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListFormulasRelaciones);
            dbProvider.AddInParameter(command, helper.Prrucodi, DbType.Int32, Grupocodi);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    MePerfilRuleDTO entity = new MePerfilRuleDTO();

                    int iPrrucodi = dr.GetOrdinal(helper.Prrucodi);
                    if (!dr.IsDBNull(iPrrucodi)) entity.Prrucodi = Convert.ToInt32(dr.GetValue(iPrrucodi));

                    int iPrruabrev = dr.GetOrdinal(helper.Prruabrev);
                    if (!dr.IsDBNull(iPrruabrev)) entity.Prruabrev = dr.GetString(iPrruabrev);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public void DeleteRelaciones(int grupoCodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDeleteRelaciones);

            dbProvider.AddInParameter(command, helper.Grupocodi, DbType.Int32, grupoCodi);

            dbProvider.ExecuteNonQuery(command);
        }
        // -----------------------------------------------------------------------------------------------------------------
    }
}
