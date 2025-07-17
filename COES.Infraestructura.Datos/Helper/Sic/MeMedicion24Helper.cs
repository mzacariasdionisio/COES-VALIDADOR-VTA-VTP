using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla ME_MEDICION24
    /// </summary>
    public class MeMedicion24Helper : HelperBase
    {
        public MeMedicion24Helper(): base(Consultas.MeMedicion24Sql)
        {
        }

        public MeMedicion24DTO Create(IDataReader dr)
        {
            MeMedicion24DTO entity = new MeMedicion24DTO();

            int iLectcodi = dr.GetOrdinal(this.Lectcodi);
            if (!dr.IsDBNull(iLectcodi)) entity.Lectcodi = Convert.ToInt32(dr.GetValue(iLectcodi));

            int iMedifecha = dr.GetOrdinal(this.Medifecha);
            if (!dr.IsDBNull(iMedifecha)) entity.Medifecha = dr.GetDateTime(iMedifecha);

            int iTipoinfocodi = dr.GetOrdinal(this.Tipoinfocodi);
            if (!dr.IsDBNull(iTipoinfocodi)) entity.Tipoinfocodi = Convert.ToInt32(dr.GetValue(iTipoinfocodi));

            int iPtomedicodi = dr.GetOrdinal(this.Ptomedicodi);
            if (!dr.IsDBNull(iPtomedicodi)) entity.Ptomedicodi = Convert.ToInt32(dr.GetValue(iPtomedicodi));

            int iMeditotal = dr.GetOrdinal(this.Meditotal);
            if (!dr.IsDBNull(iMeditotal)) entity.Meditotal = dr.GetDecimal(iMeditotal);

            int iMediestado = dr.GetOrdinal(this.Mediestado);
            if (!dr.IsDBNull(iMediestado)) entity.Mediestado = dr.GetString(iMediestado);

            int iH1 = dr.GetOrdinal(this.H1);
            if (!dr.IsDBNull(iH1)) entity.H1 = dr.GetDecimal(iH1);

            int iH2 = dr.GetOrdinal(this.H2);
            if (!dr.IsDBNull(iH2)) entity.H2 = dr.GetDecimal(iH2);

            int iH3 = dr.GetOrdinal(this.H3);
            if (!dr.IsDBNull(iH3)) entity.H3 = dr.GetDecimal(iH3);

            int iH4 = dr.GetOrdinal(this.H4);
            if (!dr.IsDBNull(iH4)) entity.H4 = dr.GetDecimal(iH4);

            int iH5 = dr.GetOrdinal(this.H5);
            if (!dr.IsDBNull(iH5)) entity.H5 = dr.GetDecimal(iH5);

            int iH6 = dr.GetOrdinal(this.H6);
            if (!dr.IsDBNull(iH6)) entity.H6 = dr.GetDecimal(iH6);

            int iH7 = dr.GetOrdinal(this.H7);
            if (!dr.IsDBNull(iH7)) entity.H7 = dr.GetDecimal(iH7);

            int iH8 = dr.GetOrdinal(this.H8);
            if (!dr.IsDBNull(iH8)) entity.H8 = dr.GetDecimal(iH8);

            int iH9 = dr.GetOrdinal(this.H9);
            if (!dr.IsDBNull(iH9)) entity.H9 = dr.GetDecimal(iH9);

            int iH10 = dr.GetOrdinal(this.H10);
            if (!dr.IsDBNull(iH10)) entity.H10 = dr.GetDecimal(iH10);

            int iH11 = dr.GetOrdinal(this.H11);
            if (!dr.IsDBNull(iH11)) entity.H11 = dr.GetDecimal(iH11);

            int iH12 = dr.GetOrdinal(this.H12);
            if (!dr.IsDBNull(iH12)) entity.H12 = dr.GetDecimal(iH12);

            int iH13 = dr.GetOrdinal(this.H13);
            if (!dr.IsDBNull(iH13)) entity.H13 = dr.GetDecimal(iH13);

            int iH14 = dr.GetOrdinal(this.H14);
            if (!dr.IsDBNull(iH14)) entity.H14 = dr.GetDecimal(iH14);

            int iH15 = dr.GetOrdinal(this.H15);
            if (!dr.IsDBNull(iH15)) entity.H15 = dr.GetDecimal(iH15);

            int iH16 = dr.GetOrdinal(this.H16);
            if (!dr.IsDBNull(iH16)) entity.H16 = dr.GetDecimal(iH16);

            int iH17 = dr.GetOrdinal(this.H17);
            if (!dr.IsDBNull(iH17)) entity.H17 = dr.GetDecimal(iH17);

            int iH18 = dr.GetOrdinal(this.H18);
            if (!dr.IsDBNull(iH18)) entity.H18 = dr.GetDecimal(iH18);

            int iH19 = dr.GetOrdinal(this.H19);
            if (!dr.IsDBNull(iH19)) entity.H19 = dr.GetDecimal(iH19);

            int iH20 = dr.GetOrdinal(this.H20);
            if (!dr.IsDBNull(iH20)) entity.H20 = dr.GetDecimal(iH20);

            int iH21 = dr.GetOrdinal(this.H21);
            if (!dr.IsDBNull(iH21)) entity.H21 = dr.GetDecimal(iH21);

            int iH22 = dr.GetOrdinal(this.H22);
            if (!dr.IsDBNull(iH22)) entity.H22 = dr.GetDecimal(iH22);

            int iH23 = dr.GetOrdinal(this.H23);
            if (!dr.IsDBNull(iH23)) entity.H23 = dr.GetDecimal(iH23);

            int iH24 = dr.GetOrdinal(this.H24);
            if (!dr.IsDBNull(iH24)) entity.H24 = dr.GetDecimal(iH24);

            int iLastuser = dr.GetOrdinal(this.Lastuser);
            if (!dr.IsDBNull(iLastuser)) entity.Lastuser = dr.GetString(iLastuser);

            int iLastdate = dr.GetOrdinal(this.Lastdate);
            if (!dr.IsDBNull(iLastdate)) entity.Lastdate = dr.GetDateTime(iLastdate);

            int iEmprcodi = dr.GetOrdinal(this.Emprcodi);
            if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));
            entity.Emprcodi = entity.Emprcodi > 0 ? entity.Emprcodi : -1;

            return entity;
        }

        public void CreateTipo(IDataReader dr, MeMedicion24DTO entity)
        {
            int iH1 = dr.GetOrdinal(this.T1);
            if (!dr.IsDBNull(iH1)) entity.T1 = Convert.ToInt32(dr.GetValue(iH1));

            int iH2 = dr.GetOrdinal(this.T2);
            if (!dr.IsDBNull(iH2)) entity.T2 = Convert.ToInt32(dr.GetValue(iH2));

            int iH3 = dr.GetOrdinal(this.T3);
            if (!dr.IsDBNull(iH3)) entity.T3 = Convert.ToInt32(dr.GetValue(iH3));

            int iH4 = dr.GetOrdinal(this.T4);
            if (!dr.IsDBNull(iH4)) entity.T4 = Convert.ToInt32(dr.GetValue(iH4));

            int iH5 = dr.GetOrdinal(this.T5);
            if (!dr.IsDBNull(iH5)) entity.T5 = Convert.ToInt32(dr.GetValue(iH5));

            int iH6 = dr.GetOrdinal(this.T6);
            if (!dr.IsDBNull(iH6)) entity.T6 = Convert.ToInt32(dr.GetValue(iH6));

            int iH7 = dr.GetOrdinal(this.T7);
            if (!dr.IsDBNull(iH7)) entity.T7 = Convert.ToInt32(dr.GetValue(iH7));

            int iH8 = dr.GetOrdinal(this.T8);
            if (!dr.IsDBNull(iH8)) entity.T8 = Convert.ToInt32(dr.GetValue(iH8));

            int iH9 = dr.GetOrdinal(this.T9);
            if (!dr.IsDBNull(iH9)) entity.T9 = Convert.ToInt32(dr.GetValue(iH9));

            int iH10 = dr.GetOrdinal(this.T10);
            if (!dr.IsDBNull(iH10)) entity.T10 = Convert.ToInt32(dr.GetValue(iH10));

            int iH11 = dr.GetOrdinal(this.T11);
            if (!dr.IsDBNull(iH11)) entity.T11 = Convert.ToInt32(dr.GetValue(iH11));

            int iH12 = dr.GetOrdinal(this.T12);
            if (!dr.IsDBNull(iH12)) entity.T12 = Convert.ToInt32(dr.GetValue(iH12));

            int iH13 = dr.GetOrdinal(this.T13);
            if (!dr.IsDBNull(iH13)) entity.T13 = Convert.ToInt32(dr.GetValue(iH13));

            int iH14 = dr.GetOrdinal(this.T14);
            if (!dr.IsDBNull(iH14)) entity.T14 = Convert.ToInt32(dr.GetValue(iH14));

            int iH15 = dr.GetOrdinal(this.T15);
            if (!dr.IsDBNull(iH15)) entity.T15 = Convert.ToInt32(dr.GetValue(iH15));

            int iH16 = dr.GetOrdinal(this.T16);
            if (!dr.IsDBNull(iH16)) entity.T16 = Convert.ToInt32(dr.GetValue(iH16));

            int iH17 = dr.GetOrdinal(this.T17);
            if (!dr.IsDBNull(iH17)) entity.T17 = Convert.ToInt32(dr.GetValue(iH17));

            int iH18 = dr.GetOrdinal(this.T18);
            if (!dr.IsDBNull(iH18)) entity.T18 = Convert.ToInt32(dr.GetValue(iH18));

            int iH19 = dr.GetOrdinal(this.T19);
            if (!dr.IsDBNull(iH19)) entity.T19 = Convert.ToInt32(dr.GetValue(iH19));

            int iH20 = dr.GetOrdinal(this.T20);
            if (!dr.IsDBNull(iH20)) entity.T20 = Convert.ToInt32(dr.GetValue(iH20));

            int iH21 = dr.GetOrdinal(this.T21);
            if (!dr.IsDBNull(iH21)) entity.T21 = Convert.ToInt32(dr.GetValue(iH21));

            int iH22 = dr.GetOrdinal(this.T22);
            if (!dr.IsDBNull(iH22)) entity.T22 = Convert.ToInt32(dr.GetValue(iH22));

            int iH23 = dr.GetOrdinal(this.T23);
            if (!dr.IsDBNull(iH23)) entity.T23 = Convert.ToInt32(dr.GetValue(iH23));

            int iH24 = dr.GetOrdinal(this.T24);
            if (!dr.IsDBNull(iH24)) entity.T24 = Convert.ToInt32(dr.GetValue(iH24));
        }

        #region Mapeo de Campos

        public string Lectcodi = "LECTCODI";
        public string Medifecha = "MEDIFECHA";
        public string Tipoinfocodi = "TIPOINFOCODI";
        public string Ptomedicodi = "PTOMEDICODI";
        public string Meditotal = "MEDITOTAL";
        public string Mediestado = "MEDIESTADO";
        public string H1 = "H1";
        public string H2 = "H2";
        public string H3 = "H3";
        public string H4 = "H4";
        public string H5 = "H5";
        public string H6 = "H6";
        public string H7 = "H7";
        public string H8 = "H8";
        public string H9 = "H9";
        public string H10 = "H10";
        public string H11 = "H11";
        public string H12 = "H12";
        public string H13 = "H13";
        public string H14 = "H14";
        public string H15 = "H15";
        public string H16 = "H16";
        public string H17 = "H17";
        public string H18 = "H18";
        public string H19 = "H19";
        public string H20 = "H20";
        public string H21 = "H21";
        public string H22 = "H22";
        public string H23 = "H23";
        public string H24 = "H24";
        public string Lastuser = "LASTUSER";
        public string Lastdate = "LASTDATE";
        public string T1 = "T1";
        public string T2 = "T2";
        public string T3 = "T3";
        public string T4 = "T4";
        public string T5 = "T5";
        public string T6 = "T6";
        public string T7 = "T7";
        public string T8 = "T8";
        public string T9 = "T9";
        public string T10 = "T10";
        public string T11 = "T11";
        public string T12 = "T12";
        public string T13 = "T13";
        public string T14 = "T14";
        public string T15 = "T15";
        public string T16 = "T16";
        public string T17 = "T17";
        public string T18 = "T18";
        public string T19 = "T19";
        public string T20 = "T20";
        public string T21 = "T21";
        public string T22 = "T22";
        public string T23 = "T23";
        public string T24 = "T24";

        public string Emprnomb = "EMPRNOMB";
        public string Equinomb = "Equinomb";
        public string Cuenca = "Cuenca";
        public string Tipoptomedinomb = "Tptomedinomb";
        public string Tipoinfoabrev = "Tipoinfoabrev";
        public string Ptomedibarranomb = "Ptomedibarranomb";
        public string Tipoptomedicodi = "Tptomedicodi";
        public string Famcodi = "Famcodi";
        public string Famabrev = "Famabrev";

        #region SIOSEIN
        public string Emprcodi = "Emprcodi";
        public string Osinergcodi = "Osinergcodi";
        public string Osicodi = "Osicodi"; // Movisoft 2022-03-07
        #endregion

        #region PR5
        public string Equicodi = "Equicodi";
        public string Ptomedielenomb = "Ptomedielenomb";
        public string Equipadre = "Equipadre";
        public string Equipopadre = "Equipopadre";
        public string Central = "CENTRAL";
        #endregion

        #region MigracionSGOCOES-GrupoB
        public string Grupocodi = "Grupocodi";
        public string Grupoabrev = "Grupoabrev";
        public string Gruponomb = "Gruponomb";
        public string Grupotipo = "Grupotipo";
        public string Equiabrev = "Equiabrev";
        public string Minimo = "Minimo";
        public string PotenciaEfectiva = "PotenciaEfectiva";
        public string Digsilent = "Digsilent";
        public string TableName = "ME_MEDICION24";
        public string FechapropequiMin = "FECHAPROPEQUIMIN";
        public string FechapropequiPotefec = "FECHAPROPEQUIPEFEC";
        #endregion

        #region Mejoras RDO
        public string Enviocodi = "ENVIOCODI";
        public string E1 = "E1";
        public string E2 = "E2";
        public string E3 = "E3";
        public string E4 = "E4";
        public string E5 = "E5";
        public string E6 = "E6";
        public string E7 = "E7";
        public string E8 = "E8";
        public string E9 = "E9";
        public string E10 = "E10";
        public string E11 = "E11";
        public string E12 = "E12";
        public string E13 = "E13";
        public string E14 = "E14";
        public string E15 = "E15";
        public string E16 = "E16";
        public string E17 = "E17";
        public string E18 = "E18";
        public string E19 = "E19";
        public string E20 = "E20";
        public string E21 = "E21";
        public string E22 = "E22";
        public string E23 = "E23";
        public string E24 = "E24";
        #endregion

        #endregion

        public string SqlDeleteEnvioArchivo
        {
            get { return base.GetSqlXml("DeleteEnvioArchivo"); }
        }

        public string SqlGetEnvioArchivo
        {
            get { return base.GetSqlXml("GetEnvioArchivo"); }
        }

        public string SqlGetHidrologia
        {
            get { return base.GetSqlXml("GetHidrologia"); }
        }

        public string SqlGetHidrologiaTiempoReal
        {
            get { return base.GetSqlXml("GetHidrologiaTiempoReal"); }
        }

        public string SqlGetInterconexiones
        {
            get { return base.GetSqlXml("GetInterconexiones"); }
        }

        public string SqlGetDataFormatoSec
        {
            get { return base.GetSqlXml("GetDataFormatoSec"); }
        }

        public string SqlGetLista24PresionGas
        {
            get { return base.GetSqlXml("GetLista24PresionGas"); }
        }
        
        public string SqlGetLista24TemperaturaAmbiente
        {
            get { return base.GetSqlXml("GetLista24TemperaturaAmbiente"); }
        }

        public string SqlGetMedicionHistoricoHidrologia
        {
            get { return base.GetSqlXml("GetMedicionHistoricoHidrologia"); }
        }

        //inicio modificado
        public string SqlObtenerMedicion24
        {
            get { return base.GetSqlXml("ObtenerMedicion24"); }
        }
        //fin modificado

        #region SIOSEIN

        public string SqlGetHidrologiaSioSein
        {
            get { return base.GetSqlXml("GetHidrologiaSioSein"); }
        }

        #endregion

        #region MigracionSGOCOES-GrupoB
        public string SqlListaGeneracionOpera
        {
            get { return base.GetSqlXml("ListaGeneracionOpera"); }
        }

        public string SqlListaDemandaDigsilent
        {
            get { return base.GetSqlXml("ListaDemandaDigsilent"); }
        }

        public string SqlDeleteMasivo
        {
            get { return base.GetSqlXml("DeleteMasivo"); }
        }
        #endregion

        #region Siosein2
        public string SqlObtenerVolumenUtil
        {
            get { return base.GetSqlXml("ObtenerVolumenUtil"); }
        }
        #endregion

        #region Mejoras RDO
        public string SqlGetEnvioArchivoEjecutados
        {
            get { return base.GetSqlXml("GetEnvioArchivoEjecutado"); }
        }
        public MeMedicion24DTO CreateEjecutados(IDataReader dr)
        {
            MeMedicion24DTO entity = new MeMedicion24DTO();

            int iLectcodi = dr.GetOrdinal(this.Lectcodi);
            if (!dr.IsDBNull(iLectcodi)) entity.Lectcodi = Convert.ToInt32(dr.GetValue(iLectcodi));

            int iMedifecha = dr.GetOrdinal(this.Medifecha);
            if (!dr.IsDBNull(iMedifecha)) entity.Medifecha = dr.GetDateTime(iMedifecha);

            int iTipoinfocodi = dr.GetOrdinal(this.Tipoinfocodi);
            if (!dr.IsDBNull(iTipoinfocodi)) entity.Tipoinfocodi = Convert.ToInt32(dr.GetValue(iTipoinfocodi));

            int iPtomedicodi = dr.GetOrdinal(this.Ptomedicodi);
            if (!dr.IsDBNull(iPtomedicodi)) entity.Ptomedicodi = Convert.ToInt32(dr.GetValue(iPtomedicodi));

            int iMeditotal = dr.GetOrdinal(this.Meditotal);
            if (!dr.IsDBNull(iMeditotal)) entity.Meditotal = dr.GetDecimal(iMeditotal);

            int iMediestado = dr.GetOrdinal(this.Mediestado);
            if (!dr.IsDBNull(iMediestado)) entity.Mediestado = dr.GetString(iMediestado);

            //int iCuenca = dr.GetOrdinal(this.Cuenca);
            //if (!dr.IsDBNull(iCuenca)) entity.Cuenca = dr.GetString(iCuenca);

            int iH1 = dr.GetOrdinal(this.H1);
            if (!dr.IsDBNull(iH1)) entity.H1 = dr.GetDecimal(iH1);

            int iH2 = dr.GetOrdinal(this.H2);
            if (!dr.IsDBNull(iH2)) entity.H2 = dr.GetDecimal(iH2);

            int iH3 = dr.GetOrdinal(this.H3);
            if (!dr.IsDBNull(iH3)) entity.H3 = dr.GetDecimal(iH3);

            int iH4 = dr.GetOrdinal(this.H4);
            if (!dr.IsDBNull(iH4)) entity.H4 = dr.GetDecimal(iH4);

            int iH5 = dr.GetOrdinal(this.H5);
            if (!dr.IsDBNull(iH5)) entity.H5 = dr.GetDecimal(iH5);

            int iH6 = dr.GetOrdinal(this.H6);
            if (!dr.IsDBNull(iH6)) entity.H6 = dr.GetDecimal(iH6);

            int iH7 = dr.GetOrdinal(this.H7);
            if (!dr.IsDBNull(iH7)) entity.H7 = dr.GetDecimal(iH7);

            int iH8 = dr.GetOrdinal(this.H8);
            if (!dr.IsDBNull(iH8)) entity.H8 = dr.GetDecimal(iH8);

            int iH9 = dr.GetOrdinal(this.H9);
            if (!dr.IsDBNull(iH9)) entity.H9 = dr.GetDecimal(iH9);

            int iH10 = dr.GetOrdinal(this.H10);
            if (!dr.IsDBNull(iH10)) entity.H10 = dr.GetDecimal(iH10);

            int iH11 = dr.GetOrdinal(this.H11);
            if (!dr.IsDBNull(iH11)) entity.H11 = dr.GetDecimal(iH11);

            int iH12 = dr.GetOrdinal(this.H12);
            if (!dr.IsDBNull(iH12)) entity.H12 = dr.GetDecimal(iH12);

            int iH13 = dr.GetOrdinal(this.H13);
            if (!dr.IsDBNull(iH13)) entity.H13 = dr.GetDecimal(iH13);

            int iH14 = dr.GetOrdinal(this.H14);
            if (!dr.IsDBNull(iH14)) entity.H14 = dr.GetDecimal(iH14);

            int iH15 = dr.GetOrdinal(this.H15);
            if (!dr.IsDBNull(iH15)) entity.H15 = dr.GetDecimal(iH15);

            int iH16 = dr.GetOrdinal(this.H16);
            if (!dr.IsDBNull(iH16)) entity.H16 = dr.GetDecimal(iH16);

            int iH17 = dr.GetOrdinal(this.H17);
            if (!dr.IsDBNull(iH17)) entity.H17 = dr.GetDecimal(iH17);

            int iH18 = dr.GetOrdinal(this.H18);
            if (!dr.IsDBNull(iH18)) entity.H18 = dr.GetDecimal(iH18);

            int iH19 = dr.GetOrdinal(this.H19);
            if (!dr.IsDBNull(iH19)) entity.H19 = dr.GetDecimal(iH19);

            int iH20 = dr.GetOrdinal(this.H20);
            if (!dr.IsDBNull(iH20)) entity.H20 = dr.GetDecimal(iH20);

            int iH21 = dr.GetOrdinal(this.H21);
            if (!dr.IsDBNull(iH21)) entity.H21 = dr.GetDecimal(iH21);

            int iH22 = dr.GetOrdinal(this.H22);
            if (!dr.IsDBNull(iH22)) entity.H22 = dr.GetDecimal(iH22);

            int iH23 = dr.GetOrdinal(this.H23);
            if (!dr.IsDBNull(iH23)) entity.H23 = dr.GetDecimal(iH23);

            int iH24 = dr.GetOrdinal(this.H24);
            if (!dr.IsDBNull(iH24)) entity.H24 = dr.GetDecimal(iH24);

            int iE1 = dr.GetOrdinal(this.E1);
            if (!dr.IsDBNull(iE1)) entity.E1 = dr.GetString(iE1);

            int iE2 = dr.GetOrdinal(this.E2);
            if (!dr.IsDBNull(iE2)) entity.E2 = dr.GetString(iE2);

            int iE3 = dr.GetOrdinal(this.E3);
            if (!dr.IsDBNull(iE3)) entity.E3 = dr.GetString(iE3);

            int iE4 = dr.GetOrdinal(this.E4);
            if (!dr.IsDBNull(iE4)) entity.E4 = dr.GetString(iE4);

            int iE5 = dr.GetOrdinal(this.E5);
            if (!dr.IsDBNull(iE5)) entity.E5 = dr.GetString(iE5);

            int iE6 = dr.GetOrdinal(this.E6);
            if (!dr.IsDBNull(iE6)) entity.E6 = dr.GetString(iE6);

            int iE7 = dr.GetOrdinal(this.E7);
            if (!dr.IsDBNull(iE7)) entity.E7 = dr.GetString(iE7);

            int iE8 = dr.GetOrdinal(this.E8);
            if (!dr.IsDBNull(iE8)) entity.E8 = dr.GetString(iE8);

            int iE9 = dr.GetOrdinal(this.E9);
            if (!dr.IsDBNull(iE9)) entity.E9 = dr.GetString(iE9);

            int iE10 = dr.GetOrdinal(this.E10);
            if (!dr.IsDBNull(iE10)) entity.E10 = dr.GetString(iE10);

            int iE11 = dr.GetOrdinal(this.E11);
            if (!dr.IsDBNull(iE11)) entity.E11 = dr.GetString(iE11);

            int iE12 = dr.GetOrdinal(this.E12);
            if (!dr.IsDBNull(iE12)) entity.E12 = dr.GetString(iE12);

            int iE13 = dr.GetOrdinal(this.E13);
            if (!dr.IsDBNull(iE13)) entity.E13 = dr.GetString(iE13);

            int iE14 = dr.GetOrdinal(this.E14);
            if (!dr.IsDBNull(iE14)) entity.E14 = dr.GetString(iE14);

            int iE15 = dr.GetOrdinal(this.E15);
            if (!dr.IsDBNull(iE15)) entity.E15 = dr.GetString(iE15);

            int iE16 = dr.GetOrdinal(this.E16);
            if (!dr.IsDBNull(iE16)) entity.E16 = dr.GetString(iE16);

            int iE17 = dr.GetOrdinal(this.E17);
            if (!dr.IsDBNull(iE17)) entity.E17 = dr.GetString(iE17);

            int iE18 = dr.GetOrdinal(this.E18);
            if (!dr.IsDBNull(iE18)) entity.E18 = dr.GetString(iE18);

            int iE19 = dr.GetOrdinal(this.E19);
            if (!dr.IsDBNull(iE19)) entity.E19 = dr.GetString(iE19);

            int iE20 = dr.GetOrdinal(this.E20);
            if (!dr.IsDBNull(iE20)) entity.E20 = dr.GetString(iE20);

            int iE21 = dr.GetOrdinal(this.E21);
            if (!dr.IsDBNull(iE21)) entity.E21 = dr.GetString(iE21);

            int iE22 = dr.GetOrdinal(this.E22);
            if (!dr.IsDBNull(iE22)) entity.E22 = dr.GetString(iE22);

            int iE23 = dr.GetOrdinal(this.E23);
            if (!dr.IsDBNull(iE23)) entity.E23 = dr.GetString(iE23);

            int iE24 = dr.GetOrdinal(this.E24);
            if (!dr.IsDBNull(iE24)) entity.E24 = dr.GetString(iE24);

            int iLastuser = dr.GetOrdinal(this.Lastuser);
            if (!dr.IsDBNull(iLastuser)) entity.Lastuser = dr.GetString(iLastuser);

            int iLastdate = dr.GetOrdinal(this.Lastdate);
            if (!dr.IsDBNull(iLastdate)) entity.Lastdate = dr.GetDateTime(iLastdate);

            int iEmprcodi = dr.GetOrdinal(this.Emprcodi);
            if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));
            entity.Emprcodi = entity.Emprcodi > 0 ? entity.Emprcodi : -1;

            return entity;
        }
        public string SqlGetEnvioArchivoIntranet
        {
            get { return base.GetSqlXml("GetEnvioArchivoIntranet"); }
        }
        public string SqlGetEnvioMeMedicion24Intranet
        {
            get { return base.GetSqlXml("GetEnvioMeMedicion24Intranet"); }
        }
        #endregion
    }
}
