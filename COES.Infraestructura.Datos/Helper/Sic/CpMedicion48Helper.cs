using COES.Base.Core;
using COES.Dominio.DTO.Sic;
using System;
using System.Data;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla CP_MEDICION48
    /// </summary>
    public class CpMedicion48Helper : HelperBase
    {
        public CpMedicion48Helper() : base(Consultas.CpMedicion48Sql)
        {
        }

        public CpMedicion48DTO Create(IDataReader dr, int indicador)
        {
            CpMedicion48DTO entity = new CpMedicion48DTO();

            int iRecurcodi = dr.GetOrdinal(this.Recurcodi);
            if (!dr.IsDBNull(iRecurcodi)) entity.Recurcodi = Convert.ToInt32(dr.GetValue(iRecurcodi));

            int iSrestcodi = dr.GetOrdinal(this.Srestcodi);
            if (!dr.IsDBNull(iSrestcodi)) entity.Srestcodi = Convert.ToInt32(dr.GetValue(iSrestcodi));

            int iTopcodi = dr.GetOrdinal(this.Topcodi);
            if (!dr.IsDBNull(iTopcodi)) entity.Topcodi = Convert.ToInt32(dr.GetValue(iTopcodi));
                     
            int iMedifecha = dr.GetOrdinal(this.Medifecha);
            if (!dr.IsDBNull(iMedifecha)) entity.Medifecha = dr.GetDateTime(iMedifecha);

            
            for (int i = 1; i <= 48; i++)
            {
                decimal valor = 0;

                if (indicador == 1)
                {
                    int indice = dr.GetOrdinal("H" + i);
                    if(!dr.IsDBNull(indice))valor = dr.GetDecimal(indice);                   
                }

                entity.GetType().GetProperty("H" + i).SetValue(entity, valor);
            }


            return entity;
        }


        #region Mapeo de Campos

        public string Recurcodi = "RECURCODI";
        public string Srestcodi = "SRESTCODI";
        public string Topcodi = "TOPCODI";
        public string H48 = "H48";
        public string H47 = "H47";
        public string H46 = "H46";
        public string H45 = "H45";
        public string H44 = "H44";
        public string H43 = "H43";
        public string H42 = "H42";
        public string H41 = "H41";
        public string H40 = "H40";
        public string H39 = "H39";
        public string H38 = "H38";
        public string H37 = "H37";
        public string H36 = "H36";
        public string H35 = "H35";
        public string H34 = "H34";
        public string H33 = "H33";
        public string H32 = "H32";
        public string H31 = "H31";
        public string H30 = "H30";
        public string H29 = "H29";
        public string H28 = "H28";
        public string H27 = "H27";
        public string H26 = "H26";
        public string H25 = "H25";
        public string H24 = "H24";
        public string H23 = "H23";
        public string H22 = "H22";
        public string H21 = "H21";
        public string H20 = "H20";
        public string H19 = "H19";
        public string H18 = "H18";
        public string H17 = "H17";
        public string H16 = "H16";
        public string H15 = "H15";
        public string H14 = "H14";
        public string H13 = "H13";
        public string H12 = "H12";
        public string H11 = "H11";
        public string H10 = "H10";
        public string H9 = "H9";
        public string H8 = "H8";
        public string H7 = "H7";
        public string H6 = "H6";
        public string H5 = "H5";
        public string H4 = "H4";
        public string H3 = "H3";
        public string H2 = "H2";
        public string Medifecha = "MEDIFECHA";
        public string H1 = "H1";
        public string Recurconsideragams = "RECURCONSIDERAGAMS";
        public string Cnfbarcodi = "CNFBARCODI";
        public string Ptomedicalculado = "Ptomedicalculado";
        public string Ptomedicodi = "PTOMEDICODI";
        public string Grupocodi = "GRUPOCODI";
        //Yupana Continuo
        public string Catnombre = "CATNOMBRE";
        public string Srestnombre = "SRESTNOMBRE";
        public string Meditotal = "MEDITOTAL";
        public string Grupointegrante = "Grupointegrante";

        #region MDCOES
        public string Recurcodisicoes = "RECURCODISICOES";
        public string Recurnombre = "RECURNOMBRE";
        #endregion

        #region Yupana
        public string Catcodi = "CATCODI";
        public string Recurfamsic = "RECURFAMSIC";
        #endregion

        #region SIOSEIN-PRIE-2021
        public string Osinergcodi = "OSINERGCODI";
        public string Cnfbarnombre = "CNFBARNOMBRE";
        #endregion

        public string SqlObtenerDatosModelo
        {
            get { return base.GetSqlXml("ObtenerDatosModelo"); }
        }

        public string SqlGetByCriteriaRecurso
        {
            get { return base.GetSqlXml("GetByCriteriaRecurso"); }
        }

        public string SqlObtieneRegistrosToDespacho
        {
            get { return base.GetSqlXml("ObtieneRegistrosToDespacho"); }
        }

        public string SqlObtieneRegistrosToDespachoPTermicas1
        {
            get { return base.GetSqlXml("ObtieneRegistrosToDespachoPTermicas1"); }
        }

        public string SqlObtieneRegistrosToDespachoPTermicas2
        {
            get { return base.GetSqlXml("ObtieneRegistrosToDespachoPTermicas2"); }
        }

        public string SqlObtieneRegistrosToDespachoPGrupo
        {
            get { return base.GetSqlXml("ObtieneRegistrosToDespachoPGrupo"); }
        }

        public string SqlObtieneRegistrosToDespachoRerPGrupo
        {
            get { return base.GetSqlXml("ObtieneRegistrosToDespachoRerPGrupo"); }
        }

        public string SqlObtieneRegistrosPHToDespacho
        {
            get { return base.GetSqlXml("ObtieneRegistrosPHToDespacho"); }
        }

        public string SqlObtieneCostoMarginalBarraEscenario
        {
            get { return base.GetSqlXml("ObtieneCostoMarginalBarraEscenario"); }
        }

        public string SqlObtieneRegistrosToBarra
        {
            get { return base.GetSqlXml("ObtieneRegistrosToBarra"); }
        }

        #endregion

        #region SIOSEIN2

        public string Famcodi = "FAMCODI";
        public string Equicodi = "EQUICODI";
        public string Equinomb = "EQUINOMB";

        public string SqlListByTipoYSubrestriccion
        {
            get { return base.GetSqlXml("ListByTipoYSubrestriccion"); }
        }

        #endregion

        #region Yupana
        public string SqlListaRestriccion
        {
            get { return GetSqlXml("ListaRestriccion"); }
        }
        #endregion

        //- Cambio movisoft 19032021
        public string SqlObtenerCongestionProgramada
        {
            get { return base.GetSqlXml("ObtenerCongestionProgramada"); }
        }

        //- Fin cambio movisoft 19032021

        // Yupana Continuo
        public string SqlListaSubRestriccionGams
        {
            get { return base.GetSqlXml("ListaSubRestriccionGams"); }
        }

        public string SqlCrearCopia
        {
            get { return base.GetSqlXml("CrearCopia"); }
        }

        public string SqlDeleteTopSubrest
        {
            get { return base.GetSqlXml("DeleteTopSubrest"); }
        }

        #region Mejoras CMgN

        public string SqlObtenerProgramaPorRecurso
        {
            get { return base.GetSqlXml("ObtenerProgramaPorRecurso"); }
        }

        #endregion

        #region SIOSEIN-PRIE-2021
        public string SqlObtieneCostoMarginalBarraEscenarioParaUnaBarra
        {
            get { return base.GetSqlXml("ObtieneCostoMarginalBarraEscenarioParaUnaBarra"); }
        }
        #endregion

        public string SqlObtenerCapacidadNominal
        {
            get { return base.GetSqlXml("ObtenerCapacidadNominal"); }
        }

        public string SqlObtenerConsumoGasNatural
        {
            get { return base.GetSqlXml("ObtenerConsumoGasNatural"); }
        }


    }
}
