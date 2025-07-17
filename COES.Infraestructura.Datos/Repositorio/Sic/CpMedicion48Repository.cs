using COES.Base.Core;
using COES.Dominio.DTO.Sic;
using COES.Dominio.Interfaces.Sic;
using COES.Infraestructura.Datos.Helper.Sic;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;

namespace COES.Infraestructura.Datos.Repositorio.Sic
{
    /// <summary>
    /// Clase de acceso a datos de la tabla CP_MEDICION48
    /// </summary>
    public class CpMedicion48Repository : RepositoryBase, ICpMedicion48Repository
    {
        public CpMedicion48Repository(string strConn) : base(strConn)
        {
        }

        CpMedicion48Helper helper = new CpMedicion48Helper();

        public void Save(CpMedicion48DTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Recurcodi, DbType.Int32, entity.Recurcodi);
            dbProvider.AddInParameter(command, helper.Srestcodi, DbType.Int32, entity.Srestcodi);
            dbProvider.AddInParameter(command, helper.Topcodi, DbType.Int32, entity.Topcodi);
            dbProvider.AddInParameter(command, helper.H48, DbType.Decimal, entity.H48);
            dbProvider.AddInParameter(command, helper.H47, DbType.Decimal, entity.H47);
            dbProvider.AddInParameter(command, helper.H46, DbType.Decimal, entity.H46);
            dbProvider.AddInParameter(command, helper.H45, DbType.Decimal, entity.H45);
            dbProvider.AddInParameter(command, helper.H44, DbType.Decimal, entity.H44);
            dbProvider.AddInParameter(command, helper.H43, DbType.Decimal, entity.H43);
            dbProvider.AddInParameter(command, helper.H42, DbType.Decimal, entity.H42);
            dbProvider.AddInParameter(command, helper.H41, DbType.Decimal, entity.H41);
            dbProvider.AddInParameter(command, helper.H40, DbType.Decimal, entity.H40);
            dbProvider.AddInParameter(command, helper.H39, DbType.Decimal, entity.H39);
            dbProvider.AddInParameter(command, helper.H38, DbType.Decimal, entity.H38);
            dbProvider.AddInParameter(command, helper.H37, DbType.Decimal, entity.H37);
            dbProvider.AddInParameter(command, helper.H36, DbType.Decimal, entity.H36);
            dbProvider.AddInParameter(command, helper.H35, DbType.Decimal, entity.H35);
            dbProvider.AddInParameter(command, helper.H34, DbType.Decimal, entity.H34);
            dbProvider.AddInParameter(command, helper.H33, DbType.Decimal, entity.H33);
            dbProvider.AddInParameter(command, helper.H32, DbType.Decimal, entity.H32);
            dbProvider.AddInParameter(command, helper.H31, DbType.Decimal, entity.H31);
            dbProvider.AddInParameter(command, helper.H30, DbType.Decimal, entity.H30);
            dbProvider.AddInParameter(command, helper.H29, DbType.Decimal, entity.H29);
            dbProvider.AddInParameter(command, helper.H28, DbType.Decimal, entity.H28);
            dbProvider.AddInParameter(command, helper.H27, DbType.Decimal, entity.H27);
            dbProvider.AddInParameter(command, helper.H26, DbType.Decimal, entity.H26);
            dbProvider.AddInParameter(command, helper.H25, DbType.Decimal, entity.H25);
            dbProvider.AddInParameter(command, helper.H24, DbType.Decimal, entity.H24);
            dbProvider.AddInParameter(command, helper.H23, DbType.Decimal, entity.H23);
            dbProvider.AddInParameter(command, helper.H22, DbType.Decimal, entity.H22);
            dbProvider.AddInParameter(command, helper.H21, DbType.Decimal, entity.H21);
            dbProvider.AddInParameter(command, helper.H20, DbType.Decimal, entity.H20);
            dbProvider.AddInParameter(command, helper.H19, DbType.Decimal, entity.H19);
            dbProvider.AddInParameter(command, helper.H18, DbType.Decimal, entity.H18);
            dbProvider.AddInParameter(command, helper.H17, DbType.Decimal, entity.H17);
            dbProvider.AddInParameter(command, helper.H16, DbType.Decimal, entity.H16);
            dbProvider.AddInParameter(command, helper.H15, DbType.Decimal, entity.H15);
            dbProvider.AddInParameter(command, helper.H14, DbType.Decimal, entity.H14);
            dbProvider.AddInParameter(command, helper.H13, DbType.Decimal, entity.H13);
            dbProvider.AddInParameter(command, helper.H12, DbType.Decimal, entity.H12);
            dbProvider.AddInParameter(command, helper.H11, DbType.Decimal, entity.H11);
            dbProvider.AddInParameter(command, helper.H10, DbType.Decimal, entity.H10);
            dbProvider.AddInParameter(command, helper.H9, DbType.Decimal, entity.H9);
            dbProvider.AddInParameter(command, helper.H8, DbType.Decimal, entity.H8);
            dbProvider.AddInParameter(command, helper.H7, DbType.Decimal, entity.H7);
            dbProvider.AddInParameter(command, helper.H6, DbType.Decimal, entity.H6);
            dbProvider.AddInParameter(command, helper.H5, DbType.Decimal, entity.H5);
            dbProvider.AddInParameter(command, helper.H4, DbType.Decimal, entity.H4);
            dbProvider.AddInParameter(command, helper.H3, DbType.Decimal, entity.H3);
            dbProvider.AddInParameter(command, helper.H2, DbType.Decimal, entity.H2);
            dbProvider.AddInParameter(command, helper.Medifecha, DbType.DateTime, entity.Medifecha);
            dbProvider.AddInParameter(command, helper.H1, DbType.Decimal, entity.H1);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Update(CpMedicion48DTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Recurcodi, DbType.Int32, entity.Recurcodi);
            dbProvider.AddInParameter(command, helper.Srestcodi, DbType.Int32, entity.Srestcodi);
            dbProvider.AddInParameter(command, helper.Topcodi, DbType.Int32, entity.Topcodi);
            dbProvider.AddInParameter(command, helper.H48, DbType.Decimal, entity.H48);
            dbProvider.AddInParameter(command, helper.H47, DbType.Decimal, entity.H47);
            dbProvider.AddInParameter(command, helper.H46, DbType.Decimal, entity.H46);
            dbProvider.AddInParameter(command, helper.H45, DbType.Decimal, entity.H45);
            dbProvider.AddInParameter(command, helper.H44, DbType.Decimal, entity.H44);
            dbProvider.AddInParameter(command, helper.H43, DbType.Decimal, entity.H43);
            dbProvider.AddInParameter(command, helper.H42, DbType.Decimal, entity.H42);
            dbProvider.AddInParameter(command, helper.H41, DbType.Decimal, entity.H41);
            dbProvider.AddInParameter(command, helper.H40, DbType.Decimal, entity.H40);
            dbProvider.AddInParameter(command, helper.H39, DbType.Decimal, entity.H39);
            dbProvider.AddInParameter(command, helper.H38, DbType.Decimal, entity.H38);
            dbProvider.AddInParameter(command, helper.H37, DbType.Decimal, entity.H37);
            dbProvider.AddInParameter(command, helper.H36, DbType.Decimal, entity.H36);
            dbProvider.AddInParameter(command, helper.H35, DbType.Decimal, entity.H35);
            dbProvider.AddInParameter(command, helper.H34, DbType.Decimal, entity.H34);
            dbProvider.AddInParameter(command, helper.H33, DbType.Decimal, entity.H33);
            dbProvider.AddInParameter(command, helper.H32, DbType.Decimal, entity.H32);
            dbProvider.AddInParameter(command, helper.H31, DbType.Decimal, entity.H31);
            dbProvider.AddInParameter(command, helper.H30, DbType.Decimal, entity.H30);
            dbProvider.AddInParameter(command, helper.H29, DbType.Decimal, entity.H29);
            dbProvider.AddInParameter(command, helper.H28, DbType.Decimal, entity.H28);
            dbProvider.AddInParameter(command, helper.H27, DbType.Decimal, entity.H27);
            dbProvider.AddInParameter(command, helper.H26, DbType.Decimal, entity.H26);
            dbProvider.AddInParameter(command, helper.H25, DbType.Decimal, entity.H25);
            dbProvider.AddInParameter(command, helper.H24, DbType.Decimal, entity.H24);
            dbProvider.AddInParameter(command, helper.H23, DbType.Decimal, entity.H23);
            dbProvider.AddInParameter(command, helper.H22, DbType.Decimal, entity.H22);
            dbProvider.AddInParameter(command, helper.H21, DbType.Decimal, entity.H21);
            dbProvider.AddInParameter(command, helper.H20, DbType.Decimal, entity.H20);
            dbProvider.AddInParameter(command, helper.H19, DbType.Decimal, entity.H19);
            dbProvider.AddInParameter(command, helper.H18, DbType.Decimal, entity.H18);
            dbProvider.AddInParameter(command, helper.H17, DbType.Decimal, entity.H17);
            dbProvider.AddInParameter(command, helper.H16, DbType.Decimal, entity.H16);
            dbProvider.AddInParameter(command, helper.H15, DbType.Decimal, entity.H15);
            dbProvider.AddInParameter(command, helper.H14, DbType.Decimal, entity.H14);
            dbProvider.AddInParameter(command, helper.H13, DbType.Decimal, entity.H13);
            dbProvider.AddInParameter(command, helper.H12, DbType.Decimal, entity.H12);
            dbProvider.AddInParameter(command, helper.H11, DbType.Decimal, entity.H11);
            dbProvider.AddInParameter(command, helper.H10, DbType.Decimal, entity.H10);
            dbProvider.AddInParameter(command, helper.H9, DbType.Decimal, entity.H9);
            dbProvider.AddInParameter(command, helper.H8, DbType.Decimal, entity.H8);
            dbProvider.AddInParameter(command, helper.H7, DbType.Decimal, entity.H7);
            dbProvider.AddInParameter(command, helper.H6, DbType.Decimal, entity.H6);
            dbProvider.AddInParameter(command, helper.H5, DbType.Decimal, entity.H5);
            dbProvider.AddInParameter(command, helper.H4, DbType.Decimal, entity.H4);
            dbProvider.AddInParameter(command, helper.H3, DbType.Decimal, entity.H3);
            dbProvider.AddInParameter(command, helper.H2, DbType.Decimal, entity.H2);
            dbProvider.AddInParameter(command, helper.Medifecha, DbType.DateTime, entity.Medifecha);
            dbProvider.AddInParameter(command, helper.H1, DbType.Decimal, entity.H1);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int recurcodi, int srestcodi, int topcodi, DateTime medifecha)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Recurcodi, DbType.Int32, recurcodi);
            dbProvider.AddInParameter(command, helper.Srestcodi, DbType.Int32, srestcodi);
            dbProvider.AddInParameter(command, helper.Topcodi, DbType.Int32, topcodi);
            dbProvider.AddInParameter(command, helper.Medifecha, DbType.DateTime, medifecha);

            dbProvider.ExecuteNonQuery(command);
        }

        public CpMedicion48DTO GetById(int recurcodi, int srestcodi, int topcodi, DateTime medifecha)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Recurcodi, DbType.Int32, recurcodi);
            dbProvider.AddInParameter(command, helper.Srestcodi, DbType.Int32, srestcodi);
            dbProvider.AddInParameter(command, helper.Topcodi, DbType.Int32, topcodi);
            dbProvider.AddInParameter(command, helper.Medifecha, DbType.DateTime, medifecha);
            CpMedicion48DTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr, 1);
                }
            }

            return entity;
        }

        public List<CpMedicion48DTO> List(int topcodi)
        {
            List<CpMedicion48DTO> entitys = new List<CpMedicion48DTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlList);
            dbProvider.AddInParameter(command, helper.Topcodi, DbType.Int32, topcodi);
            int iCatcodi, iRecurnombre;
            int iCatnombre;
            int iSrestnombre;
            CpMedicion48DTO entity = new CpMedicion48DTO();
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = helper.Create(dr,1);
                    iCatcodi = dr.GetOrdinal(helper.Catcodi);
                    if (!dr.IsDBNull(iCatcodi)) entity.Catcodi = Convert.ToInt32(dr.GetValue(iCatcodi));
                    iCatnombre = dr.GetOrdinal(helper.Catnombre);
                    if (!dr.IsDBNull(iCatnombre)) entity.Catnombre = dr.GetString(iCatnombre);
                    iSrestnombre = dr.GetOrdinal(helper.Srestnombre);
                    if (!dr.IsDBNull(iSrestnombre)) entity.Srestnombre = dr.GetString(iSrestnombre);
                    iRecurnombre = dr.GetOrdinal(helper.Recurnombre);
                    if (!dr.IsDBNull(iRecurnombre)) entity.Recurnombre = dr.GetString(iRecurnombre);
                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<CpMedicion48DTO> GetByCriteria(string topcodi, DateTime medifecha, string srestcodi)
        {
            List<CpMedicion48DTO> entitys = new List<CpMedicion48DTO>();
            CpMedicion48DTO entity;
            string query = string.Format(helper.SqlGetByCriteria, medifecha.ToString(ConstantesBase.FormatoFecha), topcodi, srestcodi);

            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = helper.Create(dr, 1);

                    int iRecurcodisicoes = dr.GetOrdinal(helper.Recurcodisicoes);
                    if (!dr.IsDBNull(iRecurcodisicoes)) entity.Recurcodisicoes = Convert.ToInt32(dr.GetValue(iRecurcodisicoes));
                    int iFamcodi = dr.GetOrdinal(helper.Famcodi);
                    if (!dr.IsDBNull(iFamcodi)) entity.Famcodi = Convert.ToInt32(dr.GetValue(iFamcodi));

                    int iRecurnombre = dr.GetOrdinal(helper.Recurnombre);
                    if (!dr.IsDBNull(iRecurnombre)) entity.Recurnombre = dr.GetString(iRecurnombre);
                    int iCatcodi = dr.GetOrdinal(helper.Catcodi);
                    if (!dr.IsDBNull(iCatcodi)) entity.Catcodi = Convert.ToInt32(dr.GetValue(iCatcodi));

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<CpMedicion48DTO> ObtenerDatosModelo(string topcodi, DateTime medifecha, string srestcodi, int sinrsf)
        {
            List<CpMedicion48DTO> entitys = new List<CpMedicion48DTO>();
            CpMedicion48DTO entity;
            string query = string.Format(helper.SqlObtenerDatosModelo, medifecha.ToString(ConstantesBase.FormatoFecha), topcodi, srestcodi);
            int[] tipos = { 97, 30, 29, 70, 71, 40, 39, 51, 50, 38, 37, 42, 41, 53, 52, 44, 43, 46, 45, 55, 54 , 69 };

            int indicador = 1;

            if (sinrsf > 0 && tipos.Contains(int.Parse(srestcodi)))
            {
                indicador = 0;
            }


            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = helper.Create(dr, indicador);

                    int iRecurnombre = dr.GetOrdinal(helper.Recurnombre);
                    if (!dr.IsDBNull(iRecurnombre)) entity.Recurnombre = dr.GetString(iRecurnombre);

                    int iRecurcodicoes = dr.GetOrdinal(this.helper.Recurcodisicoes);
                    if (!dr.IsDBNull(iRecurcodicoes)) entity.Recurcodisicoes = Convert.ToInt32(dr.GetValue(iRecurcodicoes));

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<CpMedicion48DTO> ListByTipoYSubrestriccion(int toptipo, int srestcodi, DateTime fechaIni, DateTime fechaFin)
        {
            List<CpMedicion48DTO> entitys = new List<CpMedicion48DTO>();
            CpMedicion48DTO entity;
            string query = string.Format(helper.SqlListByTipoYSubrestriccion, toptipo, srestcodi, fechaIni.ToString(ConstantesBase.FormatoFecha), fechaFin.ToString(ConstantesBase.FormatoFecha));

            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = helper.Create(dr, 1);

                    int iRecurnombre = dr.GetOrdinal(helper.Recurnombre);
                    if (!dr.IsDBNull(iRecurnombre)) entity.Recurnombre = dr.GetString(iRecurnombre);

                    int iEquicodi = dr.GetOrdinal(helper.Equicodi);
                    if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

                    int iEquinomb = dr.GetOrdinal(helper.Equinomb);
                    if (!dr.IsDBNull(iEquinomb)) entity.Equinomb = dr.GetString(iEquinomb);

                    int iFamcodi = dr.GetOrdinal(helper.Famcodi);
                    if (!dr.IsDBNull(iFamcodi)) entity.Famcodi = Convert.ToInt32(dr.GetValue(iFamcodi));

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<CpMedicion48DTO> GetByCriteriaRecurso(int pTopcodi, string pSubRestriccion, int recurcodi)
        {
            string query = string.Format(helper.SqlGetByCriteriaRecurso, pTopcodi, recurcodi, pSubRestriccion);
            List<CpMedicion48DTO> entitys = new List<CpMedicion48DTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            CpMedicion48DTO entity = new CpMedicion48DTO();
            int iRecurCodi, iRecurconsideragams, iRecurcodisicoes;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = helper.Create(dr, 1);

                    iRecurCodi = dr.GetOrdinal(helper.Recurcodi);
                    if (!dr.IsDBNull(iRecurCodi)) entity.Recurcodi = Convert.ToInt32(dr.GetValue(iRecurCodi));

                    iRecurconsideragams = dr.GetOrdinal(helper.Recurconsideragams);
                    if (!dr.IsDBNull(iRecurconsideragams)) entity.Recurconsideragams = Convert.ToInt32(dr.GetValue(iRecurconsideragams));

                    iRecurcodisicoes = dr.GetOrdinal(helper.Recurcodisicoes);
                    if (!dr.IsDBNull(iRecurcodisicoes)) entity.Recurcodisicoes = dr.GetInt32(iRecurcodisicoes);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<CpMedicion48DTO> ObtieneRegistrosToDespacho(int topcodi, short srestcodi, int origlectcodi)
        {
            List<CpMedicion48DTO> entitys = new List<CpMedicion48DTO>();
            string query = string.Format(helper.SqlObtieneRegistrosToDespacho, topcodi, srestcodi, origlectcodi);
            DbCommand command = dbProvider.GetSqlStringCommand(query);
            CpMedicion48DTO entity = null;
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = helper.Create(dr, 1);
                    int iRecurcodicoes = dr.GetOrdinal(this.helper.Recurcodisicoes);
                    if (!dr.IsDBNull(iRecurcodicoes)) entity.Recurcodisicoes = Convert.ToInt32(dr.GetValue(iRecurcodicoes));
                    int iPtomedicodi = dr.GetOrdinal(this.helper.Ptomedicodi);
                    if (!dr.IsDBNull(iPtomedicodi)) entity.Ptomedicodi = Convert.ToInt32(dr.GetValue(iPtomedicodi));
                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<CpMedicion48DTO> ObtieneRegistrosToDespachoPTermica1(int topcodi, short srestcodi)
        {
            List<CpMedicion48DTO> entitys = new List<CpMedicion48DTO>();
            string query = string.Format(helper.SqlObtieneRegistrosToDespachoPTermicas1, topcodi, srestcodi);
            DbCommand command = dbProvider.GetSqlStringCommand(query);
            CpMedicion48DTO entity = null;
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = new CpMedicion48DTO();

                    int iPtomedicodi = dr.GetOrdinal(this.helper.Ptomedicodi);
                    if (!dr.IsDBNull(iPtomedicodi)) entity.Ptomedicodi = Convert.ToInt32(dr.GetValue(iPtomedicodi));

                    int iMedifecha = dr.GetOrdinal(helper.Medifecha);
                    if (!dr.IsDBNull(iMedifecha)) entity.Medifecha = dr.GetDateTime(iMedifecha);

                    for (int i = 1; i <= 48; i++)
                    {
                        int iOrdinal = dr.GetOrdinal("H" + i);
                        if (!dr.IsDBNull(iOrdinal))
                            entity.GetType().GetProperty("H" + i).SetValue(entity, dr.GetDecimal(iOrdinal));
                    }
                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<CpMedicion48DTO> ObtieneRegistrosToDespachoPTermica2(int topcodi, short srestcodi)
        {
            List<CpMedicion48DTO> entitys = new List<CpMedicion48DTO>();
            string query = string.Format(helper.SqlObtieneRegistrosToDespachoPTermicas2, topcodi, srestcodi);
            DbCommand command = dbProvider.GetSqlStringCommand(query);
            CpMedicion48DTO entity = null;
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = new CpMedicion48DTO();

                    int iPtomedicodi = dr.GetOrdinal(this.helper.Ptomedicodi);
                    if (!dr.IsDBNull(iPtomedicodi)) entity.Ptomedicodi = Convert.ToInt32(dr.GetValue(iPtomedicodi));

                    int iGrupocodi = dr.GetOrdinal(this.helper.Grupocodi);
                    if (!dr.IsDBNull(iGrupocodi)) entity.Grupocodi = Convert.ToInt32(dr.GetValue(iGrupocodi));

                    int iMedifecha = dr.GetOrdinal(helper.Medifecha);
                    if (!dr.IsDBNull(iMedifecha)) entity.Medifecha = dr.GetDateTime(iMedifecha);

                    for (int i = 1; i <= 48; i++)
                    {
                        int iOrdinal = dr.GetOrdinal("H" + i);
                        if (!dr.IsDBNull(iOrdinal))
                            entity.GetType().GetProperty("H" + i).SetValue(entity, dr.GetDecimal(iOrdinal));
                    }
                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<CpMedicion48DTO> ObtieneRegistrosToDespachoPrGrupo(int topcodi, string srestcodi)
        {
            List<CpMedicion48DTO> entitys = new List<CpMedicion48DTO>();
            string query = string.Format(helper.SqlObtieneRegistrosToDespachoPGrupo, topcodi, srestcodi);
            DbCommand command = dbProvider.GetSqlStringCommand(query);
            CpMedicion48DTO entity = null;
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = helper.Create(dr, 1);
                    int iPtomedicodi = dr.GetOrdinal(this.helper.Ptomedicodi);
                    if (!dr.IsDBNull(iPtomedicodi)) entity.Ptomedicodi = Convert.ToInt32(dr.GetValue(iPtomedicodi));
                    int iGrupocodi = dr.GetOrdinal(this.helper.Grupocodi);
                    if (!dr.IsDBNull(iGrupocodi)) entity.Grupocodi = Convert.ToInt32(dr.GetValue(iGrupocodi));
                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<CpMedicion48DTO> ObtieneRegistrosToDespachoRerPrGrupo(int topcodi, string srestcodi)
        {
            List<CpMedicion48DTO> entitys = new List<CpMedicion48DTO>();
            string query = string.Format(helper.SqlObtieneRegistrosToDespachoRerPGrupo, topcodi, srestcodi);
            DbCommand command = dbProvider.GetSqlStringCommand(query);
            CpMedicion48DTO entity = null;
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = helper.Create(dr, 1);
                    int iPtomedicodi = dr.GetOrdinal(this.helper.Ptomedicodi);
                    if (!dr.IsDBNull(iPtomedicodi)) entity.Ptomedicodi = Convert.ToInt32(dr.GetValue(iPtomedicodi));
                    int iGrupocodi = dr.GetOrdinal(this.helper.Grupocodi);
                    if (!dr.IsDBNull(iGrupocodi)) entity.Grupocodi = Convert.ToInt32(dr.GetValue(iGrupocodi));

                    int iGrupointegrante = dr.GetOrdinal(helper.Grupointegrante);
                    if (!dr.IsDBNull(iGrupointegrante)) entity.Grupointegrante = dr.GetString(iGrupointegrante);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<CpMedicion48DTO> ObtieneRegistrosPHToDespacho(int topcodi, short srestcodi, int origlectcodi)
        {
            List<CpMedicion48DTO> entitys = new List<CpMedicion48DTO>();
            string query = string.Format(helper.SqlObtieneRegistrosPHToDespacho, topcodi, srestcodi, origlectcodi);
            DbCommand command = dbProvider.GetSqlStringCommand(query);
            CpMedicion48DTO entity = null;
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = helper.Create(dr, 1);
                    int iRecurcodicoes = dr.GetOrdinal(this.helper.Recurcodisicoes);
                    if (!dr.IsDBNull(iRecurcodicoes)) entity.Recurcodisicoes = Convert.ToInt32(dr.GetValue(iRecurcodicoes));
                    int iPtomedicodi = dr.GetOrdinal(this.helper.Ptomedicodi);
                    if (!dr.IsDBNull(iPtomedicodi)) entity.Ptomedicodi = Convert.ToInt32(dr.GetValue(iPtomedicodi));
                    int iPtomedicalculado = dr.GetOrdinal(this.helper.Ptomedicalculado);
                    if (!dr.IsDBNull(iPtomedicalculado)) entity.Ptomedicalculado = dr.GetString(iPtomedicalculado);

                    int iRecurfamsic = dr.GetOrdinal(helper.Recurfamsic);
                    if (!dr.IsDBNull(iRecurfamsic)) entity.Recurfamsic = Convert.ToInt32(dr.GetValue(iRecurfamsic));

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<CpMedicion48DTO> ObtieneCostoMarginalBarraEscenario(int topcodi, short srestcodi, DateTime fecha)
        {
            List<CpMedicion48DTO> entitys = new List<CpMedicion48DTO>();
            string query = string.Format(helper.SqlObtieneCostoMarginalBarraEscenario, srestcodi, topcodi, fecha.ToString(ConstantesBase.FormatoFecha));
            DbCommand command = dbProvider.GetSqlStringCommand(query);
            CpMedicion48DTO entity = null;
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = helper.Create(dr, 1);
                    int iCnfbarcodi = dr.GetOrdinal(this.helper.Cnfbarcodi);
                    if (!dr.IsDBNull(iCnfbarcodi)) entity.Cnfbarcodi = Convert.ToInt32(dr.GetValue(iCnfbarcodi));
                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<CpMedicion48DTO> ObtieneRegistrosToBarra(int topcodi, short srestcodi, int origlectcodi)
        {
            List<CpMedicion48DTO> entitys = new List<CpMedicion48DTO>();
            string query = string.Format(helper.SqlObtieneRegistrosToBarra, topcodi, srestcodi, origlectcodi);
            DbCommand command = dbProvider.GetSqlStringCommand(query);
            CpMedicion48DTO entity = null;
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = helper.Create(dr, 1);
                    int iRecurcodicoes = dr.GetOrdinal(this.helper.Recurcodisicoes);
                    if (!dr.IsDBNull(iRecurcodicoes)) entity.Recurcodisicoes = Convert.ToInt32(dr.GetValue(iRecurcodicoes));
                    int iPtomedicodi = dr.GetOrdinal(this.helper.Ptomedicodi);
                    if (!dr.IsDBNull(iPtomedicodi)) entity.Ptomedicodi = Convert.ToInt32(dr.GetValue(iPtomedicodi));
                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        //Yupana
        public List<CpMedicion48DTO> ListaRestricion(int pTopcodi, short pSubRestriccion)
        {
            List<CpMedicion48DTO> entitys = new List<CpMedicion48DTO>();
            CpMedicion48DTO entity = null;
            string strSql = string.Format(helper.SqlListaRestriccion, pTopcodi, pSubRestriccion);
            DbCommand command = dbProvider.GetSqlStringCommand(strSql);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = helper.Create(dr, 1);
                    int iRecurcodi = dr.GetOrdinal(this.helper.Recurcodi);
                    if (!dr.IsDBNull(iRecurcodi)) entity.Recurcodi = Convert.ToInt32(dr.GetValue(iRecurcodi));
                    int iRecurnomb = dr.GetOrdinal(this.helper.Recurnombre);
                    if (!dr.IsDBNull(iRecurnomb)) entity.Recurnombre = dr.GetString(iRecurnomb);
                    int iCatcodi = dr.GetOrdinal(this.helper.Catcodi);
                    if (!dr.IsDBNull(iCatcodi)) entity.Catcodi = Convert.ToInt32(dr.GetValue(iCatcodi));
                    int iRecurconsideragams = dr.GetOrdinal(helper.Recurconsideragams);
                    entity.Recurconsideragams = Convert.ToInt32(dr.GetValue(iRecurconsideragams));
                    int iMeditotal = dr.GetOrdinal(this.helper.Meditotal);
                    if (!dr.IsDBNull(iMeditotal)) entity.Meditotal = dr.GetDecimal(iMeditotal);
                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        //- Cambio movisoft 19032021
        public List<CpMedicion48DTO> ObtenerCongestionProgramada(int topcodi)
        {
            List<CpMedicion48DTO> entitys = new List<CpMedicion48DTO>();

            string query = string.Format(helper.SqlObtenerCongestionProgramada, topcodi);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    CpMedicion48DTO entity = new CpMedicion48DTO();

                    int iEquicodi = dr.GetOrdinal(helper.Equicodi);
                    if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

                    int iFamcodi = dr.GetOrdinal(helper.Famcodi);
                    if (!dr.IsDBNull(iFamcodi)) entity.Famcodi = Convert.ToInt32(dr.GetValue(iFamcodi));

                    for (int i = 1; i <= 48; i++)
                    {
                        decimal valor = 0;
                        int indice = dr.GetOrdinal("H" + i);
                        if (!dr.IsDBNull(indice)) valor = dr.GetDecimal(indice);
                        entity.GetType().GetProperty("H" + i).SetValue(entity, valor);
                    }

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        //- Fin cambio movisoft 19032021

            // Yupana Continuo
        public List<CpMedicion48DTO> ListaSubRestriccionGams(int pTopcodi)
        {
            List<CpMedicion48DTO> entitys = new List<CpMedicion48DTO>();
            CpMedicion48DTO entity = null;
            string strSql = string.Format(helper.SqlListaSubRestriccionGams, pTopcodi);
            DbCommand command = dbProvider.GetSqlStringCommand(strSql);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = helper.Create(dr,1);
                    int iCatcodi = dr.GetOrdinal(this.helper.Catcodi);
                    if (!dr.IsDBNull(iCatcodi)) entity.Catcodi = Convert.ToInt32(dr.GetValue(iCatcodi));
                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public void CrearCopia(int topcodi1, int topcodi2, DateTime fecha1, DateTime fecha2, int signo)
        {
            string query = string.Format(helper.SqlCrearCopia, topcodi1, topcodi2, fecha1.ToString(ConstantesBase.FormatoFecha), fecha2.ToString(ConstantesBase.FormatoFecha), signo);
            DbCommand command = dbProvider.GetSqlStringCommand(query);
            dbProvider.ExecuteNonQuery(command);
        }

        public void DeleteTopSubrestric(int topcodi, string lsurest)
        {
            string query = string.Format(helper.SqlDeleteTopSubrest, topcodi, lsurest);
            DbCommand command = dbProvider.GetSqlStringCommand(query);
            dbProvider.ExecuteNonQuery(command);
        }

        #region Mejoras CMgN

        public List<CpMedicion48DTO> ObtenerProgramaPorRecurso(string topcodi, int recurcodi, string propcodi)
        {
            List<CpMedicion48DTO> entitys = new List<CpMedicion48DTO>();

            string query = string.Format(helper.SqlObtenerProgramaPorRecurso, topcodi, recurcodi, propcodi);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    CpMedicion48DTO entity = new CpMedicion48DTO();

                    int iTopcodi = dr.GetOrdinal(helper.Topcodi);
                    if (!dr.IsDBNull(iTopcodi)) entity.Topcodi = Convert.ToInt32(dr.GetValue(iTopcodi));

                    for (int i = 1; i <= 48; i++)
                    {
                        decimal valor = 0;
                        int indice = dr.GetOrdinal("H" + i);
                        if (!dr.IsDBNull(indice)) valor = dr.GetDecimal(indice);
                        entity.GetType().GetProperty("H" + i).SetValue(entity, valor);
                    }

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        #endregion

        #region SIOSEIN-PRIE-2021
        public List<CpMedicion48DTO> ObtieneCostoMarginalBarraEscenarioParaUnaBarra(int barrcodi, int topcodi, short srestcodi, DateTime fecha)
        {
            List<CpMedicion48DTO> entitys = new List<CpMedicion48DTO>();
            string query = string.Format(helper.SqlObtieneCostoMarginalBarraEscenarioParaUnaBarra, srestcodi, topcodi, fecha.ToString(ConstantesBase.FormatoFecha), barrcodi);
            DbCommand command = dbProvider.GetSqlStringCommand(query);
            CpMedicion48DTO entity = null;
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = helper.Create(dr, 1);

                    int iCnfbarcodi = dr.GetOrdinal(this.helper.Cnfbarcodi);
                    if (!dr.IsDBNull(iCnfbarcodi)) entity.Cnfbarcodi = Convert.ToInt32(dr.GetValue(iCnfbarcodi));
                    entitys.Add(entity);

                    int iOsinergcodi = dr.GetOrdinal(this.helper.Osinergcodi);
                    if (!dr.IsDBNull(iOsinergcodi)) entity.Osinergcodi = Convert.ToString(dr.GetValue(iOsinergcodi));
                    entitys.Add(entity);

                    int iCnfbarnombre = dr.GetOrdinal(this.helper.Cnfbarnombre);
                    if (!dr.IsDBNull(iCnfbarnombre)) entity.Cnfbarnombre = Convert.ToString(dr.GetValue(iCnfbarnombre));
                    entitys.Add(entity);
                }
            }

            return entitys;
        }
        #endregion

        #region Intervenciones

        public List<CpMedicion48DTO> ObtenerCapacidadNominal(int topcodi)
        {
            List<CpMedicion48DTO> entitys = new List<CpMedicion48DTO>();

            string query = string.Format(helper.SqlObtenerCapacidadNominal, topcodi);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    CpMedicion48DTO entity = new CpMedicion48DTO();

                    int iEquicodi = dr.GetOrdinal(helper.Equicodi);
                    if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

                    int iFamcodi = dr.GetOrdinal(helper.Famcodi);
                    if (!dr.IsDBNull(iFamcodi)) entity.Famcodi = Convert.ToInt32(dr.GetValue(iFamcodi));

                    int iAreanomb = dr.GetOrdinal("AREANOMB");
                    if (!dr.IsDBNull(iAreanomb)) entity.Areanomb = dr.GetString(iAreanomb);

                    int iEquiabrev = dr.GetOrdinal("EQUIABREV");
                    if (!dr.IsDBNull(iEquiabrev)) entity.Equiabrev = dr.GetString(iEquiabrev);

                    int iEmprnomb = dr.GetOrdinal("EMPRNOMB");
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    for (int i = 1; i <= 48; i++)
                    {
                        decimal valor = 0;
                        int indice = dr.GetOrdinal("H" + i);
                        if (!dr.IsDBNull(indice)) valor = dr.GetDecimal(indice);
                        entity.GetType().GetProperty("H" + i).SetValue(entity, valor);
                    }

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<CpMedicion48DTO> ObtenerConsumoGasNatural(int topcodi,int srestcodi)
        {
            List<CpMedicion48DTO> entitys = new List<CpMedicion48DTO>();

            string query = string.Format(helper.SqlObtenerConsumoGasNatural, topcodi, srestcodi);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    CpMedicion48DTO entity = new CpMedicion48DTO();

                    int iMeditotal = dr.GetOrdinal(helper.Meditotal);
                    if (!dr.IsDBNull(iMeditotal)) entity.Meditotal = dr.GetDecimal(iMeditotal);

                    int iMedifecha = dr.GetOrdinal(helper.Medifecha);
                    if (!dr.IsDBNull(iMedifecha)) entity.Medifecha = dr.GetDateTime(iMedifecha);

                    int iEquicodi = dr.GetOrdinal(helper.Equicodi);
                    if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

                    int iFamcodi = dr.GetOrdinal(helper.Famcodi);
                    if (!dr.IsDBNull(iFamcodi)) entity.Famcodi = Convert.ToInt32(dr.GetValue(iFamcodi));

                    int iEmprnomb = dr.GetOrdinal("EMPRNOMB");
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    int iEquiabrev = dr.GetOrdinal("EQUIABREV");
                    if (!dr.IsDBNull(iEquiabrev)) entity.Equiabrev = dr.GetString(iEquiabrev);

                    for (int i = 1; i <= 48; i++)
                    {
                        decimal valor = 0;
                        int indice = dr.GetOrdinal("H" + i);
                        if (!dr.IsDBNull(indice)) valor = dr.GetDecimal(indice);
                        entity.GetType().GetProperty("H" + i).SetValue(entity, valor);
                    }

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        #endregion

    }
}
