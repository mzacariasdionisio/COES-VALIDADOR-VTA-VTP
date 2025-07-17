using COES.Base.Core;
using COES.Dominio.DTO.Sic;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    public class VceDatcalculoHelper : HelperBase
    {
        public VceDatcalculoHelper() : base(Consultas.VceDatcalculoSql)
        {
        }

        public VceDatcalculoDTO Create(IDataReader dr)
        {
            VceDatcalculoDTO entity = new VceDatcalculoDTO();

            int iCrdcgcmarrDol = dr.GetOrdinal(this.CrdcgcmarrDol);
            if (!dr.IsDBNull(iCrdcgcmarrDol)) entity.Crdcgcmarr_dol = dr.GetDecimal(iCrdcgcmarrDol);

            int iCrdcgcmarrSol = dr.GetOrdinal(this.CrdcgcmarrSol);
            if (!dr.IsDBNull(iCrdcgcmarrSol)) entity.Crdcgcmarr_sol = dr.GetDecimal(iCrdcgcmarrSol);

            int iCrdcgccbefparrampa = dr.GetOrdinal(this.Crdcgccbefparrampa);
            if (!dr.IsDBNull(iCrdcgccbefparrampa)) entity.Crdcgccbefparrampa = dr.GetDecimal(iCrdcgccbefparrampa);

            int iCrdcgccbefpar = dr.GetOrdinal(this.Crdcgccbefpar);
            if (!dr.IsDBNull(iCrdcgccbefpar)) entity.Crdcgccbefpar = dr.GetDecimal(iCrdcgccbefpar);

            int iCrdcgccbefarrtoma = dr.GetOrdinal(this.Crdcgccbefarrtoma);
            if (!dr.IsDBNull(iCrdcgccbefarrtoma)) entity.Crdcgccbefarrtoma = dr.GetDecimal(iCrdcgccbefarrtoma);

            int iCrdcgccbefarr = dr.GetOrdinal(this.Crdcgccbefarr);
            if (!dr.IsDBNull(iCrdcgccbefarr)) entity.Crdcgccbefarr = dr.GetDecimal(iCrdcgccbefarr);

            int iCrdcgpotmin = dr.GetOrdinal(this.Crdcgpotmin);
            if (!dr.IsDBNull(iCrdcgpotmin)) entity.Crdcgpotmin = dr.GetDecimal(iCrdcgpotmin);

            int iCrdcgconcompp4 = dr.GetOrdinal(this.Crdcgconcompp4);
            if (!dr.IsDBNull(iCrdcgconcompp4)) entity.Crdcgconcompp4 = dr.GetDecimal(iCrdcgconcompp4);

            int iCrdcgpotpar4 = dr.GetOrdinal(this.Crdcgpotpar4);
            if (!dr.IsDBNull(iCrdcgpotpar4)) entity.Crdcgpotpar4 = dr.GetDecimal(iCrdcgpotpar4);

            int iCrdcgconcompp3 = dr.GetOrdinal(this.Crdcgconcompp3);
            if (!dr.IsDBNull(iCrdcgconcompp3)) entity.Crdcgconcompp3 = dr.GetDecimal(iCrdcgconcompp3);

            int iCrdcgpotpar3 = dr.GetOrdinal(this.Crdcgpotpar3);
            if (!dr.IsDBNull(iCrdcgpotpar3)) entity.Crdcgpotpar3 = dr.GetDecimal(iCrdcgpotpar3);

            int iCrdcgconcompp2 = dr.GetOrdinal(this.Crdcgconcompp2);
            if (!dr.IsDBNull(iCrdcgconcompp2)) entity.Crdcgconcompp2 = dr.GetDecimal(iCrdcgconcompp2);

            int iCrdcgpotpar2 = dr.GetOrdinal(this.Crdcgpotpar2);
            if (!dr.IsDBNull(iCrdcgpotpar2)) entity.Crdcgpotpar2 = dr.GetDecimal(iCrdcgpotpar2);

            int iCrdcgconcompp1 = dr.GetOrdinal(this.Crdcgconcompp1);
            if (!dr.IsDBNull(iCrdcgconcompp1)) entity.Crdcgconcompp1 = dr.GetDecimal(iCrdcgconcompp1);

            int iCrdcgpotpar1 = dr.GetOrdinal(this.Crdcgpotpar1);
            if (!dr.IsDBNull(iCrdcgpotpar1)) entity.Crdcgpotpar1 = dr.GetDecimal(iCrdcgpotpar1);

            int iCrdcgccpotefe = dr.GetOrdinal(this.Crdcgccpotefe);
            if (!dr.IsDBNull(iCrdcgccpotefe)) entity.Crdcgccpotefe = dr.GetDecimal(iCrdcgccpotefe);

            int iCrdcgpotefe = dr.GetOrdinal(this.Crdcgpotefe);
            if (!dr.IsDBNull(iCrdcgpotefe)) entity.Crdcgpotefe = dr.GetDecimal(iCrdcgpotefe);

            int iCrdcgnumarrpar = dr.GetOrdinal(this.Crdcgnumarrpar);
            if (!dr.IsDBNull(iCrdcgnumarrpar)) entity.Crdcgnumarrpar = dr.GetDecimal(iCrdcgnumarrpar);

            int iCrdcgprecioaplicunid = dr.GetOrdinal(this.Crdcgprecioaplicunid);
            if (!dr.IsDBNull(iCrdcgprecioaplicunid)) entity.Crdcgprecioaplicunid = dr.GetString(iCrdcgprecioaplicunid);

            int iCrdcgprecioaplic = dr.GetOrdinal(this.Crdcgprecioaplic);
            if (!dr.IsDBNull(iCrdcgprecioaplic)) entity.Crdcgprecioaplic = dr.GetDecimal(iCrdcgprecioaplic);

            int iCrdcgprecombunid = dr.GetOrdinal(this.Crdcgprecombunid);
            if (!dr.IsDBNull(iCrdcgprecombunid)) entity.Crdcgprecombunid = dr.GetString(iCrdcgprecombunid);

            int iCrdcgprecomb = dr.GetOrdinal(this.Crdcgprecomb);
            if (!dr.IsDBNull(iCrdcgprecomb)) entity.Crdcgprecomb = dr.GetDecimal(iCrdcgprecomb);

            int iCrdcgcvncsol = dr.GetOrdinal(this.Crdcgcvncsol);
            if (!dr.IsDBNull(iCrdcgcvncsol)) entity.Crdcgcvncsol = dr.GetDecimal(iCrdcgcvncsol);

            int iCrdcgcvncdol = dr.GetOrdinal(this.Crdcgcvncdol);
            if (!dr.IsDBNull(iCrdcgcvncdol)) entity.Crdcgcvncdol = dr.GetDecimal(iCrdcgcvncdol);

            int iCrdcgtratquim = dr.GetOrdinal(this.Crdcgtratquim);
            if (!dr.IsDBNull(iCrdcgtratquim)) entity.Crdcgtratquim = dr.GetDecimal(iCrdcgtratquim);

            int iCrdcgtratmec = dr.GetOrdinal(this.Crdcgtratmec);
            if (!dr.IsDBNull(iCrdcgtratmec)) entity.Crdcgtratmec = dr.GetDecimal(iCrdcgtratmec);

            int iCrdcgtranspor = dr.GetOrdinal(this.Crdcgtranspor);
            if (!dr.IsDBNull(iCrdcgtranspor)) entity.Crdcgtranspor = dr.GetDecimal(iCrdcgtranspor);

            int iCrdcglhv = dr.GetOrdinal(this.Crdcglhv);
            if (!dr.IsDBNull(iCrdcglhv)) entity.Crdcglhv = dr.GetDecimal(iCrdcglhv);

            int iCrdcgtipcom = dr.GetOrdinal(this.Crdcgtipcom);
            if (!dr.IsDBNull(iCrdcgtipcom)) entity.Crdcgtipcom = dr.GetString(iCrdcgtipcom);

            int iCrdcgfecmod = dr.GetOrdinal(this.Crdcgfecmod);
            if (!dr.IsDBNull(iCrdcgfecmod)) entity.Crdcgfecmod = dr.GetDateTime(iCrdcgfecmod);

            int iGrupocodi = dr.GetOrdinal(this.Grupocodi);
            if (!dr.IsDBNull(iGrupocodi)) entity.Grupocodi = Convert.ToInt32(dr.GetValue(iGrupocodi));

            int iPecacodi = dr.GetOrdinal(this.Pecacodi);
            if (!dr.IsDBNull(iPecacodi)) entity.PecaCodi = Convert.ToInt32(dr.GetValue(iPecacodi));

            int iCrdcgdiasfinanc = dr.GetOrdinal(this.Crdcgdiasfinanc);
            if (!dr.IsDBNull(iCrdcgdiasfinanc)) entity.Crdcgdiasfinanc = Convert.ToInt32(dr.GetValue(iCrdcgdiasfinanc));

            int iCrdcgtiempo = dr.GetOrdinal(this.Crdcgtiempo);
            if (!dr.IsDBNull(iCrdcgtiempo)) entity.Crdcgtiempo = dr.GetDecimal(iCrdcgtiempo);

            int iCrdcgenergia = dr.GetOrdinal(this.Crdcgenergia);
            if (!dr.IsDBNull(iCrdcgenergia)) entity.Crdcgenergia = dr.GetDecimal(iCrdcgenergia);

            int iCrdcgconsiderapotnom = dr.GetOrdinal(this.Crdcgconsiderapotnom);
            if (!dr.IsDBNull(iCrdcgconsiderapotnom)) entity.Crdcgconsiderapotnom = Convert.ToInt32(dr.GetValue(iCrdcgconsiderapotnom));

            int iBarrcodi = dr.GetOrdinal(this.Barrcodi);
            if (!dr.IsDBNull(iBarrcodi)) entity.Barrcodi = Convert.ToInt32(dr.GetValue(iBarrcodi));

            //- compensaciones.HDT - Inicio 16/03/2017: Cambio para atender el requerimiento.

            int iCrdcgconspotefearr = dr.GetOrdinal(this.Crdcgconspotefearr);
            if (!dr.IsDBNull(iCrdcgconspotefearr)) entity.Crdcgconspotefearr = dr.GetDecimal(iCrdcgconspotefearr);

            int iCrdcgconspotefepar = dr.GetOrdinal(this.Crdcgconspotefepar);
            if (!dr.IsDBNull(iCrdcgconspotefearr)) entity.Crdcgconspotefepar = dr.GetDecimal(iCrdcgconspotefepar);

            int iCrdcgprecioaplicxarr = dr.GetOrdinal(this.Crdcgprecioaplicxarr);
            if (!dr.IsDBNull(iCrdcgprecioaplicxarr)) entity.Crdcgprecioaplicxarr = dr.GetDecimal(iCrdcgprecioaplicxarr);

            int iCrdcgprecioaplicxpar = dr.GetOrdinal(this.Crdcgprecioaplicxpar);
            if (!dr.IsDBNull(iCrdcgprecioaplicxpar)) entity.Crdcgprecioaplicxpar = dr.GetDecimal(iCrdcgprecioaplicxpar);

            int iCrdcgprecioaplicxincgen = dr.GetOrdinal(this.Crdcgprecioaplicxincgen);
            if (!dr.IsDBNull(iCrdcgprecioaplicxincgen)) entity.Crdcgprecioaplicxincgen = dr.GetDecimal(iCrdcgprecioaplicxincgen);

            int iCrdcgprecioaplicxdisgen = dr.GetOrdinal(this.Crdcgprecioaplicxdisgen);
            if (!dr.IsDBNull(iCrdcgprecioaplicxdisgen)) entity.Crdcgprecioaplicxdisgen = dr.GetDecimal(iCrdcgprecioaplicxdisgen);

            //- HDT Fin

            return entity;
        }

        #region Mapeo de Campos

        public string CrdcgcmarrDol = "CRDCGCMARR_DOL";
        public string CrdcgcmarrSol = "CRDCGCMARR_SOL";
        public string Crdcgccbefparrampa = "CRDCGCCBEFPARRAMPA";
        public string Crdcgccbefpar = "CRDCGCCBEFPAR";
        public string Crdcgccbefarrtoma = "CRDCGCCBEFARRTOMA";
        public string Crdcgccbefarr = "CRDCGCCBEFARR";
        public string Crdcgpotmin = "CRDCGPOTMIN";
        public string Crdcgconcompp4 = "CRDCGCONCOMPP4";
        public string Crdcgpotpar4 = "CRDCGPOTPAR4";
        public string Crdcgconcompp3 = "CRDCGCONCOMPP3";
        public string Crdcgpotpar3 = "CRDCGPOTPAR3";
        public string Crdcgconcompp2 = "CRDCGCONCOMPP2";
        public string Crdcgpotpar2 = "CRDCGPOTPAR2";
        public string Crdcgconcompp1 = "CRDCGCONCOMPP1";
        public string Crdcgpotpar1 = "CRDCGPOTPAR1";
        public string Crdcgccpotefe = "CRDCGCCPOTEFE";
        public string Crdcgpotefe = "CRDCGPOTEFE";
        public string Crdcgnumarrpar = "CRDCGNUMARRPAR";
        public string Crdcgprecioaplicunid = "CRDCGPRECIOAPLICUNID";
        public string Crdcgprecioaplic = "CRDCGPRECIOAPLIC";
        public string Crdcgprecombunid = "CRDCGPRECOMBUNID";
        public string Crdcgprecomb = "CRDCGPRECOMB";
        public string Crdcgcvncsol = "CRDCGCVNCSOL";
        public string Crdcgcvncdol = "CRDCGCVNCDOL";
        public string Crdcgtratquim = "CRDCGTRATQUIM";
        public string Crdcgtratmec = "CRDCGTRATMEC";
        public string Crdcgtranspor = "CRDCGTRANSPOR";
        public string Crdcglhv = "CRDCGLHV";
        public string Crdcgtipcom = "CRDCGTIPCOM";
        public string Crdcgfecmod = "CRDCGFECMOD";
        public string Grupocodi = "GRUPOCODI";
        public string Pecacodi = "PECACODI";
        public string Crdcgdiasfinanc = "CRDCGDIASFINANC";
        public string Crdcgtiempo = "CRDCGTIEMPO";
        public string Crdcgenergia = "CRDCGENERGIA";
        public string Crdcgconsiderapotnom = "CRDCGCONSIDERAPOTNOM";
        public string Barrcodi = "BARRCODI";

        //Adicionales
        public string Gruponomb = "GRUPONOMB";
        public string Fenergnomb = "FENERGNOMB";
        public string Barradiaper = "BARRADIAPER";
        public string Considerarpotnominal = "CONSIDERARPOTNOMINAL";
        public string VceDCMEnergia = "VCEDCMENERGIA";
        public string VceDCMTiempo = "VCEDCMTIEMPO";
        public string Edit = "EDIT";
        public string Periodo = "PERIODO";
        public string VceDCMConsideraPotNom = "VCEDCMCONSIDERAPOTNOM";
        //- compensaciones.HDT - Inicio 27/02/2017: Cambio para atender el requerimiento.
        public string Emprnomb = "EMPRNOMB";
        //- HDT Fin

        //- compensaciones.HDT - Inicio 03/03/2017: Cambio para atender el requerimiento.
        public string Crdcgconspotefearr = "CRDCGCONSPOTEFEARR";
        public string Crdcgconspotefepar = "CRDCGCONSPOTEFEPAR";
        public string Crdcgprecioaplicxarr = "CRDCGPRECIOAPLICXARR";
        public string Crdcgprecioaplicxpar = "CRDCGPRECIOAPLICXPAR";
        public string Crdcgprecioaplicxincgen = "CRDCGPRECIOAPLICXINCGEN";
        public string Crdcgprecioaplicxdisgen = "CRDCGPRECIOAPLICXDISGEN";
        //- HDT Fin
        

        #endregion

        public string SqlGetTipoCambioFecha
        {
            get { return base.GetSqlXml("GetTipoCambioFecha"); }
        }

        public string SqlInsertRegistros
        {
            get { return base.GetSqlXml("InsertRegistros"); }
        }

        public string SqlListGrupo
        {
            get { return base.GetSqlXml("ListGrupo"); }
        }

        //- compensaciones.HDT - 02/03/2017: Cambio para atender el requerimiento. 
        /// <summary>
        /// Permite obtener la cadena SQL para realizar la consulta de VceDatCalculo por periodo.
        /// </summary>
        public string SqlListVceDatCalculoPorPeriodo
        {
            get { return base.GetSqlXml("ListVceDatCalculoPorPeriodo"); }
        }

        //- compensaciones.HDT - 16/03/2017: Cambio para atender el requerimiento. 
        /// <summary>
        /// Permite obtener la cadena SQL para realizar la consulta de VceDatCalculo por periodo.
        /// </summary>
        public string SqlListVceDatCalculoPorPeriodoPotenciaEfectiva
        {
            get { return base.GetSqlXml("ListVceDatCalculoPorPeriodoPotenciaEfectiva"); }
        }

        //- compensaciones.HDT - 02/03/2017: Cambio para atender el requerimiento. 
        /// <summary>
        /// Permite obtener la cadena SQL para las distintas fechas de modificación por periodo.
        /// </summary>
        public string SqlListDistinctFecVceDatCalculoPorPeriodo
        {
            get { return base.GetSqlXml("ListDistinctFecVceDatCalculoPorPeriodo"); }
        }

        //- compensaciones.HDT - 16/03/2017: Cambio para atender el requerimiento. 
        /// <summary>
        /// Permite obtener la cadena SQL para las distintas fechas de modificación por periodo.
        /// </summary>
        public string SqlListDistinctFecVceDatCalculoPorPeriodoPotenciaEfectiva
        {
            get { return base.GetSqlXml("ListDistinctFecVceDatCalculoPorPeriodoPotenciaEfectiva"); }
        }

        //- compensaciones.HDT - 03/03/2017: Cambio para atender el requerimiento. 
        /// <summary>
        /// Permite obtener la cadena SQL para consultar los distintos grupos para un periodo dado.
        /// </summary>
        public string SqlListDistinctIdGrupo
        {
            get { return base.GetSqlXml("ListDistinctIdGrupo"); }
        }

        //- compensaciones.HDT - 03/03/2017: Cambio para atender el requerimiento. 
        /// <summary>
        /// Permite obtener la cadena SQL para consultar los distintos grupos para un periodo dado relacionados
        /// con la potencia efectiva.
        /// </summary>
        public string SqlListDistinctIdGrupoPotEfectiva
        {
            get { return base.GetSqlXml("ListDistinctIdGrupoPotEfectiva"); }
        }

        //- compensaciones.HDT - 02/03/2017: Cambio para atender el requerimiento. 
        /// <summary>
        /// Permite obtener la cadena SQL para la configuración de los campos.
        /// </summary>
        public string SqlListVceCfgDatCalculo
        {
            get { return base.GetSqlXml("ListVceCfgDatCalculo"); }
        }

        //- compensaciones.HDT - 16/03/2017: Cambio para atender el requerimiento. 
        /// <summary>
        /// Permite obtener la cadena SQL para la actualización del tipo de combustible.
        /// </summary>
        public string SqlActualizarTipoCombustible
        {
            get { return base.GetSqlXml("ActualizarTipoCombustible"); }
        }


        public string SqlListCampo
        {
            get { return base.GetSqlXml("ListCampo"); }
        }

        public string SqlListParametrosGenerador
        {
            get { return base.GetSqlXml("ListParametrosGenerador"); }
        }

        public string SqlUpdateArrPar
        {
            get { return base.GetSqlXml("UpdateArrPar"); }
        }

        public string SqlUpdateConsComb
        {
            get { return base.GetSqlXml("UpdateConsComb"); }
        }

        public string SqlListByFiltro
        {
            get { return base.GetSqlXml("ListByFiltro"); }
        }

        public string SqlGetByIdGrupo
        {
            get { return base.GetSqlXml("GetByIdGrupo"); }
        }

        public string SqlUpdateCalculo
        {
            get { return base.GetSqlXml("UpdateCalculo"); }
        }
        public string SqlDeleteCalculo
        {
            get { return base.GetSqlXml("DeleteCalculo"); }
        }

        public string SqlListCursorFechas
        {
            get { return base.GetSqlXml("ListCursorFechas"); }
        }

        public string SqlListCursorGrupos
        {
            get { return base.GetSqlXml("ListCursorGrupos"); }
        }

        public string SqlGetParametrosGenerales
        {
            get { return base.GetSqlXml("GetParametrosGenerales"); }
        }

        public string SqlGetCabAgrupado
        {
            get { return base.GetSqlXml("GetCabAgrupado"); }
        }


        public string SqlListCurModoOperacion
        {
            get { return base.GetSqlXml("ListCurModoOperacion"); }
        }

        public string SqlListCurMedicion
        {
            get { return base.GetSqlXml("ListCurMedicion"); }
        }

        public string SqlSaveFromOtherVersion
        {
            get { return base.GetSqlXml("SaveFromOtherVersion"); }
        }

    }
}
