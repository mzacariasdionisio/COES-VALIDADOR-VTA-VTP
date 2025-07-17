using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COES.Base.Core;
using COES.Dominio.DTO.Sic;
using COES.Dominio.Interfaces.Sic;
using COES.Infraestructura.Datos.Helper.Sic;
using System.Globalization;

namespace COES.Infraestructura.Datos.Repositorio.Sic
{
    public class DpoProcesoPronosticoRepository : RepositoryBase, IDpoProcesoPronosticoRepository
    {
        public DpoProcesoPronosticoRepository(string strConn) : base(strConn)
        {
        }

        DpoProcesoPronosticoHelper helper = new DpoProcesoPronosticoHelper();
        DpoEstimadorRawHelper helperEstRaw = new DpoEstimadorRawHelper();
        
        public List<MeMedicion48DTO> ObtenerGeneracionPorFechas(string str)
        {
            List<MeMedicion48DTO> entitys = new List<MeMedicion48DTO>();
            string query = string.Format(helper.SqlObtenerGeneracionPorFechas, str);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    MeMedicion48DTO entity = new MeMedicion48DTO();

                    int iPtomedicodi = dr.GetOrdinal(helper.Ptomedicodi);
                    if (!dr.IsDBNull(iPtomedicodi)) entity.Ptomedicodi = Convert.ToInt32(dr.GetValue(iPtomedicodi));

                    int iMedifecha = dr.GetOrdinal(helper.Medifecha);
                    if (!dr.IsDBNull(iMedifecha)) entity.Medifecha = dr.GetDateTime(iMedifecha);

                    for (int i = 1; i <= 48; i++)
                    {
                        int iOrdinal = dr.GetOrdinal($"H{i}");
                        if (!dr.IsDBNull(iOrdinal))
                            entity.GetType().GetProperty($"H{i}")
                                .SetValue(entity, dr.GetDecimal(iOrdinal));
                    }
                    entitys.Add(entity);
                }
            }
            return entitys;
        }

        public List<DpoFormulaDTO> ObtenerFormulas()
        {
            List<DpoFormulaDTO> entitys = new List<DpoFormulaDTO>();
            string query = string.Format(helper.SqlObtenerFormulas);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    DpoFormulaDTO entity = new DpoFormulaDTO();

                    int iPtomedibarranomb = dr.GetOrdinal(helper.Ptomedibarranomb);
                    if (!dr.IsDBNull(iPtomedibarranomb)) entity.Ptomedibarranomb = dr.GetString(iPtomedibarranomb);

                    int iNombre_P = dr.GetOrdinal(helper.Nombre_P);
                    if (!dr.IsDBNull(iNombre_P)) entity.Nombre_P = dr.GetString(iNombre_P);

                    int iFormula_P = dr.GetOrdinal(helper.Formula_P);
                    if (!dr.IsDBNull(iFormula_P)) entity.Formula_P = dr.GetString(iFormula_P);

                    int iNombre_S = dr.GetOrdinal(helper.Nombre_S);
                    if (!dr.IsDBNull(iNombre_S)) entity.Nombre_S = dr.GetString(iNombre_S);

                    int iFormula_S = dr.GetOrdinal(helper.Formula_S);
                    if (!dr.IsDBNull(iFormula_S)) entity.Formula_S = dr.GetString(iFormula_S);

                    entitys.Add(entity);
                }
            }
            return entitys;
        }

        public List<DpoEstimadorRawDTO> ObtenerDemandaPorFechas(
            string nomTabla, string fecIni, string fecFin,
            string listaIds)
        {
            List<DpoEstimadorRawDTO> entitys = new List<DpoEstimadorRawDTO>();
            string query = string.Format(helper.SqlObtenerDemandaPorFechas,
                nomTabla, fecIni, fecFin, listaIds);
            DbCommand command = dbProvider.GetSqlStringCommand(query);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    DpoEstimadorRawDTO entity = new DpoEstimadorRawDTO();

                    int iPtomedicodi = dr.GetOrdinal(helperEstRaw.Ptomedicodi);
                    if (!dr.IsDBNull(iPtomedicodi)) entity.Ptomedicodi = Convert.ToInt32(dr.GetValue(iPtomedicodi));

                    int iDpovarcodi = dr.GetOrdinal(helperEstRaw.Prnvarcodi);
                    if (!dr.IsDBNull(iDpovarcodi)) entity.Prnvarcodi = Convert.ToInt32(dr.GetValue(iDpovarcodi));

                    int iDporawfuente = dr.GetOrdinal(helperEstRaw.Dporawfuente);
                    if (!dr.IsDBNull(iDporawfuente)) entity.Dporawfuente = Convert.ToInt32(dr.GetValue(iDporawfuente));

                    int iDporawtipomedi = dr.GetOrdinal(helperEstRaw.Dporawtipomedi);
                    if (!dr.IsDBNull(iDporawtipomedi)) entity.Dporawtipomedi = Convert.ToInt32(dr.GetValue(iDporawtipomedi));

                    int iDporawfecha = dr.GetOrdinal(helperEstRaw.Dporawfecha);
                    if (!dr.IsDBNull(iDporawfecha)) entity.Dporawfecha = dr.GetDateTime(iDporawfecha);

                    int ih1 = dr.GetOrdinal(helperEstRaw.Dporawvalorh1);
                    if (!dr.IsDBNull(ih1)) entity.Dporawvalorh1 = dr.GetDecimal(ih1);

                    int ih2 = dr.GetOrdinal(helperEstRaw.Dporawvalorh2);
                    if (!dr.IsDBNull(ih2)) entity.Dporawvalorh2 = dr.GetDecimal(ih2);

                    int ih3 = dr.GetOrdinal(helperEstRaw.Dporawvalorh3);
                    if (!dr.IsDBNull(ih3)) entity.Dporawvalorh3 = dr.GetDecimal(ih3);

                    int ih4 = dr.GetOrdinal(helperEstRaw.Dporawvalorh4);
                    if (!dr.IsDBNull(ih4)) entity.Dporawvalorh4 = dr.GetDecimal(ih4);

                    int ih5 = dr.GetOrdinal(helperEstRaw.Dporawvalorh5);
                    if (!dr.IsDBNull(ih5)) entity.Dporawvalorh5 = dr.GetDecimal(ih5);

                    int ih6 = dr.GetOrdinal(helperEstRaw.Dporawvalorh6);
                    if (!dr.IsDBNull(ih6)) entity.Dporawvalorh6 = dr.GetDecimal(ih6);

                    int ih7 = dr.GetOrdinal(helperEstRaw.Dporawvalorh7);
                    if (!dr.IsDBNull(ih7)) entity.Dporawvalorh7 = dr.GetDecimal(ih7);

                    int ih8 = dr.GetOrdinal(helperEstRaw.Dporawvalorh8);
                    if (!dr.IsDBNull(ih8)) entity.Dporawvalorh8 = dr.GetDecimal(ih8);

                    int ih9 = dr.GetOrdinal(helperEstRaw.Dporawvalorh9);
                    if (!dr.IsDBNull(ih9)) entity.Dporawvalorh9 = dr.GetDecimal(ih9);

                    int ih10 = dr.GetOrdinal(helperEstRaw.Dporawvalorh10);
                    if (!dr.IsDBNull(ih10)) entity.Dporawvalorh10 = dr.GetDecimal(ih10);

                    int ih11 = dr.GetOrdinal(helperEstRaw.Dporawvalorh11);
                    if (!dr.IsDBNull(ih11)) entity.Dporawvalorh11 = dr.GetDecimal(ih11);

                    int ih12 = dr.GetOrdinal(helperEstRaw.Dporawvalorh12);
                    if (!dr.IsDBNull(ih12)) entity.Dporawvalorh12 = dr.GetDecimal(ih12);

                    int ih13 = dr.GetOrdinal(helperEstRaw.Dporawvalorh13);
                    if (!dr.IsDBNull(ih13)) entity.Dporawvalorh13 = dr.GetDecimal(ih13);

                    int ih14 = dr.GetOrdinal(helperEstRaw.Dporawvalorh14);
                    if (!dr.IsDBNull(ih14)) entity.Dporawvalorh14 = dr.GetDecimal(ih14);

                    int ih15 = dr.GetOrdinal(helperEstRaw.Dporawvalorh15);
                    if (!dr.IsDBNull(ih15)) entity.Dporawvalorh15 = dr.GetDecimal(ih15);

                    int ih16 = dr.GetOrdinal(helperEstRaw.Dporawvalorh16);
                    if (!dr.IsDBNull(ih16)) entity.Dporawvalorh16 = dr.GetDecimal(ih16);

                    int ih17 = dr.GetOrdinal(helperEstRaw.Dporawvalorh17);
                    if (!dr.IsDBNull(ih17)) entity.Dporawvalorh17 = dr.GetDecimal(ih17);

                    int ih18 = dr.GetOrdinal(helperEstRaw.Dporawvalorh18);
                    if (!dr.IsDBNull(ih18)) entity.Dporawvalorh18 = dr.GetDecimal(ih18);

                    int ih19 = dr.GetOrdinal(helperEstRaw.Dporawvalorh19);
                    if (!dr.IsDBNull(ih19)) entity.Dporawvalorh19 = dr.GetDecimal(ih19);

                    int ih20 = dr.GetOrdinal(helperEstRaw.Dporawvalorh20);
                    if (!dr.IsDBNull(ih20)) entity.Dporawvalorh20 = dr.GetDecimal(ih20);

                    int ih21 = dr.GetOrdinal(helperEstRaw.Dporawvalorh21);
                    if (!dr.IsDBNull(ih21)) entity.Dporawvalorh21 = dr.GetDecimal(ih21);

                    int ih22 = dr.GetOrdinal(helperEstRaw.Dporawvalorh22);
                    if (!dr.IsDBNull(ih22)) entity.Dporawvalorh22 = dr.GetDecimal(ih22);

                    int ih23 = dr.GetOrdinal(helperEstRaw.Dporawvalorh23);
                    if (!dr.IsDBNull(ih23)) entity.Dporawvalorh23 = dr.GetDecimal(ih23);

                    int ih24 = dr.GetOrdinal(helperEstRaw.Dporawvalorh24);
                    if (!dr.IsDBNull(ih24)) entity.Dporawvalorh24 = dr.GetDecimal(ih24);

                    int ih25 = dr.GetOrdinal(helperEstRaw.Dporawvalorh25);
                    if (!dr.IsDBNull(ih25)) entity.Dporawvalorh25 = dr.GetDecimal(ih25);

                    int ih26 = dr.GetOrdinal(helperEstRaw.Dporawvalorh26);
                    if (!dr.IsDBNull(ih26)) entity.Dporawvalorh26 = dr.GetDecimal(ih26);

                    int ih27 = dr.GetOrdinal(helperEstRaw.Dporawvalorh27);
                    if (!dr.IsDBNull(ih27)) entity.Dporawvalorh27 = dr.GetDecimal(ih27);

                    int ih28 = dr.GetOrdinal(helperEstRaw.Dporawvalorh28);
                    if (!dr.IsDBNull(ih28)) entity.Dporawvalorh28 = dr.GetDecimal(ih28);

                    int ih29 = dr.GetOrdinal(helperEstRaw.Dporawvalorh29);
                    if (!dr.IsDBNull(ih29)) entity.Dporawvalorh29 = dr.GetDecimal(ih29);

                    int ih30 = dr.GetOrdinal(helperEstRaw.Dporawvalorh30);
                    if (!dr.IsDBNull(ih30)) entity.Dporawvalorh30 = dr.GetDecimal(ih30);

                    int ih31 = dr.GetOrdinal(helperEstRaw.Dporawvalorh31);
                    if (!dr.IsDBNull(ih31)) entity.Dporawvalorh31 = dr.GetDecimal(ih31);

                    int ih32 = dr.GetOrdinal(helperEstRaw.Dporawvalorh32);
                    if (!dr.IsDBNull(ih32)) entity.Dporawvalorh32 = dr.GetDecimal(ih32);

                    int ih33 = dr.GetOrdinal(helperEstRaw.Dporawvalorh33);
                    if (!dr.IsDBNull(ih33)) entity.Dporawvalorh33 = dr.GetDecimal(ih33);

                    int ih34 = dr.GetOrdinal(helperEstRaw.Dporawvalorh34);
                    if (!dr.IsDBNull(ih34)) entity.Dporawvalorh34 = dr.GetDecimal(ih34);

                    int ih35 = dr.GetOrdinal(helperEstRaw.Dporawvalorh35);
                    if (!dr.IsDBNull(ih35)) entity.Dporawvalorh35 = dr.GetDecimal(ih35);

                    int ih36 = dr.GetOrdinal(helperEstRaw.Dporawvalorh36);
                    if (!dr.IsDBNull(ih36)) entity.Dporawvalorh36 = dr.GetDecimal(ih36);

                    int ih37 = dr.GetOrdinal(helperEstRaw.Dporawvalorh37);
                    if (!dr.IsDBNull(ih37)) entity.Dporawvalorh37 = dr.GetDecimal(ih37);

                    int ih38 = dr.GetOrdinal(helperEstRaw.Dporawvalorh38);
                    if (!dr.IsDBNull(ih38)) entity.Dporawvalorh38 = dr.GetDecimal(ih38);

                    int ih39 = dr.GetOrdinal(helperEstRaw.Dporawvalorh39);
                    if (!dr.IsDBNull(ih39)) entity.Dporawvalorh39 = dr.GetDecimal(ih39);

                    int ih40 = dr.GetOrdinal(helperEstRaw.Dporawvalorh40);
                    if (!dr.IsDBNull(ih40)) entity.Dporawvalorh40 = dr.GetDecimal(ih40);

                    int ih41 = dr.GetOrdinal(helperEstRaw.Dporawvalorh41);
                    if (!dr.IsDBNull(ih41)) entity.Dporawvalorh41 = dr.GetDecimal(ih41);

                    int ih42 = dr.GetOrdinal(helperEstRaw.Dporawvalorh42);
                    if (!dr.IsDBNull(ih42)) entity.Dporawvalorh42 = dr.GetDecimal(ih42);

                    int ih43 = dr.GetOrdinal(helperEstRaw.Dporawvalorh43);
                    if (!dr.IsDBNull(ih43)) entity.Dporawvalorh43 = dr.GetDecimal(ih43);

                    int ih44 = dr.GetOrdinal(helperEstRaw.Dporawvalorh44);
                    if (!dr.IsDBNull(ih44)) entity.Dporawvalorh44 = dr.GetDecimal(ih44);

                    int ih45 = dr.GetOrdinal(helperEstRaw.Dporawvalorh45);
                    if (!dr.IsDBNull(ih45)) entity.Dporawvalorh45 = dr.GetDecimal(ih45);

                    int ih46 = dr.GetOrdinal(helperEstRaw.Dporawvalorh46);
                    if (!dr.IsDBNull(ih46)) entity.Dporawvalorh46 = dr.GetDecimal(ih46);

                    int ih47 = dr.GetOrdinal(helperEstRaw.Dporawvalorh47);
                    if (!dr.IsDBNull(ih47)) entity.Dporawvalorh47 = dr.GetDecimal(ih47);

                    int ih48 = dr.GetOrdinal(helperEstRaw.Dporawvalorh48);
                    if (!dr.IsDBNull(ih48)) entity.Dporawvalorh48 = dr.GetDecimal(ih48);

                    int ih49 = dr.GetOrdinal(helperEstRaw.Dporawvalorh49);
                    if (!dr.IsDBNull(ih49)) entity.Dporawvalorh49 = dr.GetDecimal(ih49);

                    int ih50 = dr.GetOrdinal(helperEstRaw.Dporawvalorh50);
                    if (!dr.IsDBNull(ih50)) entity.Dporawvalorh50 = dr.GetDecimal(ih50);

                    int ih51 = dr.GetOrdinal(helperEstRaw.Dporawvalorh51);
                    if (!dr.IsDBNull(ih51)) entity.Dporawvalorh51 = dr.GetDecimal(ih51);

                    int ih52 = dr.GetOrdinal(helperEstRaw.Dporawvalorh52);
                    if (!dr.IsDBNull(ih52)) entity.Dporawvalorh52 = dr.GetDecimal(ih52);

                    int ih53 = dr.GetOrdinal(helperEstRaw.Dporawvalorh53);
                    if (!dr.IsDBNull(ih53)) entity.Dporawvalorh53 = dr.GetDecimal(ih53);

                    int ih54 = dr.GetOrdinal(helperEstRaw.Dporawvalorh54);
                    if (!dr.IsDBNull(ih54)) entity.Dporawvalorh54 = dr.GetDecimal(ih54);

                    int ih55 = dr.GetOrdinal(helperEstRaw.Dporawvalorh55);
                    if (!dr.IsDBNull(ih55)) entity.Dporawvalorh55 = dr.GetDecimal(ih55);

                    int ih56 = dr.GetOrdinal(helperEstRaw.Dporawvalorh56);
                    if (!dr.IsDBNull(ih56)) entity.Dporawvalorh56 = dr.GetDecimal(ih56);

                    int ih57 = dr.GetOrdinal(helperEstRaw.Dporawvalorh57);
                    if (!dr.IsDBNull(ih57)) entity.Dporawvalorh57 = dr.GetDecimal(ih57);

                    int ih58 = dr.GetOrdinal(helperEstRaw.Dporawvalorh58);
                    if (!dr.IsDBNull(ih58)) entity.Dporawvalorh58 = dr.GetDecimal(ih58);

                    int ih59 = dr.GetOrdinal(helperEstRaw.Dporawvalorh59);
                    if (!dr.IsDBNull(ih59)) entity.Dporawvalorh59 = dr.GetDecimal(ih59);

                    int ih60 = dr.GetOrdinal(helperEstRaw.Dporawvalorh60);
                    if (!dr.IsDBNull(ih60)) entity.Dporawvalorh60 = dr.GetDecimal(ih60);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<DpoRelacionScoIeod> RelacionScoIeod()
        {
            List<DpoRelacionScoIeod> entitys = new List<DpoRelacionScoIeod>();
            string query = string.Format(helper.SqlRelacionScoIeod);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    DpoRelacionScoIeod entity = new DpoRelacionScoIeod();

                    int iPtomedicodi_Sco = dr.GetOrdinal(helper.Ptomedicodi_Sco);
                    if (!dr.IsDBNull(iPtomedicodi_Sco)) entity.Ptomedicodi_Sco = Convert.ToInt32(dr.GetValue(iPtomedicodi_Sco));
                    
                    int iPtomedicodi_Ieod = dr.GetOrdinal(helper.Ptomedicodi_Ieod);
                    if (!dr.IsDBNull(iPtomedicodi_Ieod)) entity.Ptomedicodi_Ieod = Convert.ToInt32(dr.GetValue(iPtomedicodi_Ieod));

                    entitys.Add(entity);
                }
            }
            return entitys;
        }

        public List<MePtomedicionDTO> ListaBarras()
        {
            List<MePtomedicionDTO> entitys = new List<MePtomedicionDTO>();
            string query = string.Format(helper.SqlListaBarras);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    MePtomedicionDTO entity = new MePtomedicionDTO();

                    int iPtomedicodi = dr.GetOrdinal(helper.Ptomedicodi);
                    if (!dr.IsDBNull(iPtomedicodi)) entity.Ptomedicodi = Convert.ToInt32(dr.GetValue(iPtomedicodi));

                    int iGrupocodi = dr.GetOrdinal(helper.Grupocodi);
                    if (!dr.IsDBNull(iGrupocodi)) entity.Grupocodi = Convert.ToInt32(dr.GetValue(iGrupocodi));

                    int iPtomedibarranomb = dr.GetOrdinal(helper.Ptomedibarranomb);
                    if (!dr.IsDBNull(iPtomedibarranomb)) entity.Ptomedibarranomb = dr.GetString(iPtomedibarranomb);

                    int iAreacodi = dr.GetOrdinal(helper.Areacodi);
                    if (!dr.IsDBNull(iAreacodi)) entity.Areacodi = Convert.ToInt32(dr.GetValue(iAreacodi));

                    entitys.Add(entity);
                }
            }
            return entitys;
        }

        public List<PrnMedicion48DTO> ObtenerDemandaSRP(
            string fecIni, string fecFin, string prnmgrtipo,
            string vergrpcodi)
        {
            List<PrnMedicion48DTO> entitys = new List<PrnMedicion48DTO>();
            string query = string.Format(helper.SqlObtenerDemandaSRP, 
                fecIni, fecFin, prnmgrtipo, vergrpcodi);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    PrnMedicion48DTO entity = new PrnMedicion48DTO();

                    int iMedifecha = dr.GetOrdinal(helper.Medifecha);
                    if (!dr.IsDBNull(iMedifecha)) entity.Medifecha = dr.GetDateTime(iMedifecha);

                    int iPtomedibarranomb = dr.GetOrdinal(helper.Ptomedibarranomb);
                    if (!dr.IsDBNull(iPtomedibarranomb)) entity.Gruponomb = dr.GetString(iPtomedibarranomb);

                    for (int i = 1; i <= 48; i++)
                    {
                        int iOrdinal = dr.GetOrdinal($"H{i}");
                        if (!dr.IsDBNull(iOrdinal))
                            entity.GetType().GetProperty($"H{i}")
                                .SetValue(entity, dr.GetDecimal(iOrdinal));
                    }
                    entitys.Add(entity);
                }
            }
            return entitys;
        }

        public List<DpoEstimadorRawDTO> ObtenerDemandaUltimaHora(
            string diaHora, string hora24, string dia, string diaHoraSig, 
            string listaIds)
        {
            List<DpoEstimadorRawDTO> entitys = new List<DpoEstimadorRawDTO>();
            string query = string.Format(helper.SqlObtenerDemandaUltimaHora,
                diaHora, hora24, dia, diaHoraSig, listaIds);
            DbCommand command = dbProvider.GetSqlStringCommand(query);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    DpoEstimadorRawDTO entity = new DpoEstimadorRawDTO();

                    int iPtomedicodi = dr.GetOrdinal(helperEstRaw.Ptomedicodi);
                    if (!dr.IsDBNull(iPtomedicodi)) entity.Ptomedicodi = Convert.ToInt32(dr.GetValue(iPtomedicodi));

                    int iDpovarcodi = dr.GetOrdinal(helperEstRaw.Prnvarcodi);
                    if (!dr.IsDBNull(iDpovarcodi)) entity.Prnvarcodi = Convert.ToInt32(dr.GetValue(iDpovarcodi));

                    int iDporawfuente = dr.GetOrdinal(helperEstRaw.Dporawfuente);
                    if (!dr.IsDBNull(iDporawfuente)) entity.Dporawfuente = Convert.ToInt32(dr.GetValue(iDporawfuente));

                    int iDporawtipomedi = dr.GetOrdinal(helperEstRaw.Dporawtipomedi);
                    if (!dr.IsDBNull(iDporawtipomedi)) entity.Dporawtipomedi = Convert.ToInt32(dr.GetValue(iDporawtipomedi));

                    int iDporawfecha = dr.GetOrdinal(helperEstRaw.Dporawfecha);
                    if (!dr.IsDBNull(iDporawfecha)) entity.Dporawfecha = dr.GetDateTime(iDporawfecha);

                    int ih1 = dr.GetOrdinal(helperEstRaw.Dporawvalorh1);
                    if (!dr.IsDBNull(ih1)) entity.Dporawvalorh1 = dr.GetDecimal(ih1);

                    int ih2 = dr.GetOrdinal(helperEstRaw.Dporawvalorh2);
                    if (!dr.IsDBNull(ih2)) entity.Dporawvalorh2 = dr.GetDecimal(ih2);

                    int ih3 = dr.GetOrdinal(helperEstRaw.Dporawvalorh3);
                    if (!dr.IsDBNull(ih3)) entity.Dporawvalorh3 = dr.GetDecimal(ih3);

                    int ih4 = dr.GetOrdinal(helperEstRaw.Dporawvalorh4);
                    if (!dr.IsDBNull(ih4)) entity.Dporawvalorh4 = dr.GetDecimal(ih4);

                    int ih5 = dr.GetOrdinal(helperEstRaw.Dporawvalorh5);
                    if (!dr.IsDBNull(ih5)) entity.Dporawvalorh5 = dr.GetDecimal(ih5);

                    int ih6 = dr.GetOrdinal(helperEstRaw.Dporawvalorh6);
                    if (!dr.IsDBNull(ih6)) entity.Dporawvalorh6 = dr.GetDecimal(ih6);

                    int ih7 = dr.GetOrdinal(helperEstRaw.Dporawvalorh7);
                    if (!dr.IsDBNull(ih7)) entity.Dporawvalorh7 = dr.GetDecimal(ih7);

                    int ih8 = dr.GetOrdinal(helperEstRaw.Dporawvalorh8);
                    if (!dr.IsDBNull(ih8)) entity.Dporawvalorh8 = dr.GetDecimal(ih8);

                    int ih9 = dr.GetOrdinal(helperEstRaw.Dporawvalorh9);
                    if (!dr.IsDBNull(ih9)) entity.Dporawvalorh9 = dr.GetDecimal(ih9);

                    int ih10 = dr.GetOrdinal(helperEstRaw.Dporawvalorh10);
                    if (!dr.IsDBNull(ih10)) entity.Dporawvalorh10 = dr.GetDecimal(ih10);

                    int ih11 = dr.GetOrdinal(helperEstRaw.Dporawvalorh11);
                    if (!dr.IsDBNull(ih11)) entity.Dporawvalorh11 = dr.GetDecimal(ih11);

                    int ih12 = dr.GetOrdinal(helperEstRaw.Dporawvalorh12);
                    if (!dr.IsDBNull(ih12)) entity.Dporawvalorh12 = dr.GetDecimal(ih12);

                    int ih13 = dr.GetOrdinal(helperEstRaw.Dporawvalorh13);
                    if (!dr.IsDBNull(ih13)) entity.Dporawvalorh13 = dr.GetDecimal(ih13);

                    int ih14 = dr.GetOrdinal(helperEstRaw.Dporawvalorh14);
                    if (!dr.IsDBNull(ih14)) entity.Dporawvalorh14 = dr.GetDecimal(ih14);

                    int ih15 = dr.GetOrdinal(helperEstRaw.Dporawvalorh15);
                    if (!dr.IsDBNull(ih15)) entity.Dporawvalorh15 = dr.GetDecimal(ih15);

                    int ih16 = dr.GetOrdinal(helperEstRaw.Dporawvalorh16);
                    if (!dr.IsDBNull(ih16)) entity.Dporawvalorh16 = dr.GetDecimal(ih16);

                    int ih17 = dr.GetOrdinal(helperEstRaw.Dporawvalorh17);
                    if (!dr.IsDBNull(ih17)) entity.Dporawvalorh17 = dr.GetDecimal(ih17);

                    int ih18 = dr.GetOrdinal(helperEstRaw.Dporawvalorh18);
                    if (!dr.IsDBNull(ih18)) entity.Dporawvalorh18 = dr.GetDecimal(ih18);

                    int ih19 = dr.GetOrdinal(helperEstRaw.Dporawvalorh19);
                    if (!dr.IsDBNull(ih19)) entity.Dporawvalorh19 = dr.GetDecimal(ih19);

                    int ih20 = dr.GetOrdinal(helperEstRaw.Dporawvalorh20);
                    if (!dr.IsDBNull(ih20)) entity.Dporawvalorh20 = dr.GetDecimal(ih20);

                    int ih21 = dr.GetOrdinal(helperEstRaw.Dporawvalorh21);
                    if (!dr.IsDBNull(ih21)) entity.Dporawvalorh21 = dr.GetDecimal(ih21);

                    int ih22 = dr.GetOrdinal(helperEstRaw.Dporawvalorh22);
                    if (!dr.IsDBNull(ih22)) entity.Dporawvalorh22 = dr.GetDecimal(ih22);

                    int ih23 = dr.GetOrdinal(helperEstRaw.Dporawvalorh23);
                    if (!dr.IsDBNull(ih23)) entity.Dporawvalorh23 = dr.GetDecimal(ih23);

                    int ih24 = dr.GetOrdinal(helperEstRaw.Dporawvalorh24);
                    if (!dr.IsDBNull(ih24)) entity.Dporawvalorh24 = dr.GetDecimal(ih24);

                    int ih25 = dr.GetOrdinal(helperEstRaw.Dporawvalorh25);
                    if (!dr.IsDBNull(ih25)) entity.Dporawvalorh25 = dr.GetDecimal(ih25);

                    int ih26 = dr.GetOrdinal(helperEstRaw.Dporawvalorh26);
                    if (!dr.IsDBNull(ih26)) entity.Dporawvalorh26 = dr.GetDecimal(ih26);

                    int ih27 = dr.GetOrdinal(helperEstRaw.Dporawvalorh27);
                    if (!dr.IsDBNull(ih27)) entity.Dporawvalorh27 = dr.GetDecimal(ih27);

                    int ih28 = dr.GetOrdinal(helperEstRaw.Dporawvalorh28);
                    if (!dr.IsDBNull(ih28)) entity.Dporawvalorh28 = dr.GetDecimal(ih28);

                    int ih29 = dr.GetOrdinal(helperEstRaw.Dporawvalorh29);
                    if (!dr.IsDBNull(ih29)) entity.Dporawvalorh29 = dr.GetDecimal(ih29);

                    int ih30 = dr.GetOrdinal(helperEstRaw.Dporawvalorh30);
                    if (!dr.IsDBNull(ih30)) entity.Dporawvalorh30 = dr.GetDecimal(ih30);

                    int ih31 = dr.GetOrdinal(helperEstRaw.Dporawvalorh31);
                    if (!dr.IsDBNull(ih31)) entity.Dporawvalorh31 = dr.GetDecimal(ih31);

                    int ih32 = dr.GetOrdinal(helperEstRaw.Dporawvalorh32);
                    if (!dr.IsDBNull(ih32)) entity.Dporawvalorh32 = dr.GetDecimal(ih32);

                    int ih33 = dr.GetOrdinal(helperEstRaw.Dporawvalorh33);
                    if (!dr.IsDBNull(ih33)) entity.Dporawvalorh33 = dr.GetDecimal(ih33);

                    int ih34 = dr.GetOrdinal(helperEstRaw.Dporawvalorh34);
                    if (!dr.IsDBNull(ih34)) entity.Dporawvalorh34 = dr.GetDecimal(ih34);

                    int ih35 = dr.GetOrdinal(helperEstRaw.Dporawvalorh35);
                    if (!dr.IsDBNull(ih35)) entity.Dporawvalorh35 = dr.GetDecimal(ih35);

                    int ih36 = dr.GetOrdinal(helperEstRaw.Dporawvalorh36);
                    if (!dr.IsDBNull(ih36)) entity.Dporawvalorh36 = dr.GetDecimal(ih36);

                    int ih37 = dr.GetOrdinal(helperEstRaw.Dporawvalorh37);
                    if (!dr.IsDBNull(ih37)) entity.Dporawvalorh37 = dr.GetDecimal(ih37);

                    int ih38 = dr.GetOrdinal(helperEstRaw.Dporawvalorh38);
                    if (!dr.IsDBNull(ih38)) entity.Dporawvalorh38 = dr.GetDecimal(ih38);

                    int ih39 = dr.GetOrdinal(helperEstRaw.Dporawvalorh39);
                    if (!dr.IsDBNull(ih39)) entity.Dporawvalorh39 = dr.GetDecimal(ih39);

                    int ih40 = dr.GetOrdinal(helperEstRaw.Dporawvalorh40);
                    if (!dr.IsDBNull(ih40)) entity.Dporawvalorh40 = dr.GetDecimal(ih40);

                    int ih41 = dr.GetOrdinal(helperEstRaw.Dporawvalorh41);
                    if (!dr.IsDBNull(ih41)) entity.Dporawvalorh41 = dr.GetDecimal(ih41);

                    int ih42 = dr.GetOrdinal(helperEstRaw.Dporawvalorh42);
                    if (!dr.IsDBNull(ih42)) entity.Dporawvalorh42 = dr.GetDecimal(ih42);

                    int ih43 = dr.GetOrdinal(helperEstRaw.Dporawvalorh43);
                    if (!dr.IsDBNull(ih43)) entity.Dporawvalorh43 = dr.GetDecimal(ih43);

                    int ih44 = dr.GetOrdinal(helperEstRaw.Dporawvalorh44);
                    if (!dr.IsDBNull(ih44)) entity.Dporawvalorh44 = dr.GetDecimal(ih44);

                    int ih45 = dr.GetOrdinal(helperEstRaw.Dporawvalorh45);
                    if (!dr.IsDBNull(ih45)) entity.Dporawvalorh45 = dr.GetDecimal(ih45);

                    int ih46 = dr.GetOrdinal(helperEstRaw.Dporawvalorh46);
                    if (!dr.IsDBNull(ih46)) entity.Dporawvalorh46 = dr.GetDecimal(ih46);

                    int ih47 = dr.GetOrdinal(helperEstRaw.Dporawvalorh47);
                    if (!dr.IsDBNull(ih47)) entity.Dporawvalorh47 = dr.GetDecimal(ih47);

                    int ih48 = dr.GetOrdinal(helperEstRaw.Dporawvalorh48);
                    if (!dr.IsDBNull(ih48)) entity.Dporawvalorh48 = dr.GetDecimal(ih48);

                    int ih49 = dr.GetOrdinal(helperEstRaw.Dporawvalorh49);
                    if (!dr.IsDBNull(ih49)) entity.Dporawvalorh49 = dr.GetDecimal(ih49);

                    int ih50 = dr.GetOrdinal(helperEstRaw.Dporawvalorh50);
                    if (!dr.IsDBNull(ih50)) entity.Dporawvalorh50 = dr.GetDecimal(ih50);

                    int ih51 = dr.GetOrdinal(helperEstRaw.Dporawvalorh51);
                    if (!dr.IsDBNull(ih51)) entity.Dporawvalorh51 = dr.GetDecimal(ih51);

                    int ih52 = dr.GetOrdinal(helperEstRaw.Dporawvalorh52);
                    if (!dr.IsDBNull(ih52)) entity.Dporawvalorh52 = dr.GetDecimal(ih52);

                    int ih53 = dr.GetOrdinal(helperEstRaw.Dporawvalorh53);
                    if (!dr.IsDBNull(ih53)) entity.Dporawvalorh53 = dr.GetDecimal(ih53);

                    int ih54 = dr.GetOrdinal(helperEstRaw.Dporawvalorh54);
                    if (!dr.IsDBNull(ih54)) entity.Dporawvalorh54 = dr.GetDecimal(ih54);

                    int ih55 = dr.GetOrdinal(helperEstRaw.Dporawvalorh55);
                    if (!dr.IsDBNull(ih55)) entity.Dporawvalorh55 = dr.GetDecimal(ih55);

                    int ih56 = dr.GetOrdinal(helperEstRaw.Dporawvalorh56);
                    if (!dr.IsDBNull(ih56)) entity.Dporawvalorh56 = dr.GetDecimal(ih56);

                    int ih57 = dr.GetOrdinal(helperEstRaw.Dporawvalorh57);
                    if (!dr.IsDBNull(ih57)) entity.Dporawvalorh57 = dr.GetDecimal(ih57);

                    int ih58 = dr.GetOrdinal(helperEstRaw.Dporawvalorh58);
                    if (!dr.IsDBNull(ih58)) entity.Dporawvalorh58 = dr.GetDecimal(ih58);

                    int ih59 = dr.GetOrdinal(helperEstRaw.Dporawvalorh59);
                    if (!dr.IsDBNull(ih59)) entity.Dporawvalorh59 = dr.GetDecimal(ih59);

                    int ih60 = dr.GetOrdinal(helperEstRaw.Dporawvalorh60);
                    if (!dr.IsDBNull(ih60)) entity.Dporawvalorh60 = dr.GetDecimal(ih60);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }
    }
}
