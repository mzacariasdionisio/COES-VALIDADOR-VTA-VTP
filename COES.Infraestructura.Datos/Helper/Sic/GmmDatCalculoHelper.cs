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

    /// <summary>
    /// Clase que contiene el mapeo de la tabla GMM_DATCALCULO
    /// </summary>
    public class GmmDatCalculoHelper : HelperBase
    {
        public GmmDatCalculoHelper()
            : base(Consultas.GmmDatCalculoSql)
        {
        }
        #region Mapeo de Campos
        public string Dcalcodi = "DCALCODI";
        public string DCALCODI = "DCALCODI";
        public string TINSCODI = "TINSCODI";
        public string EMPGCODI = "EMPGCODI";
        public string PERICODI = "PERICODI";

        public string DCALVALOR = "DCALVALOR";
        public string EMPRCODI = "EMPRCODI";
        public string BARRCODI = "BARRCODI";
        public string Pericodi = "PERICODI";
        public string Dcalvalor = "DCALVALOR";
        public string PTOMEDICODI = "PTOMEDICODI";
        public string TPTOMEDICODI = "TPTOMEDICODI";
        public string TIPOINFOCODI = "TIPOINFOCODI";
        public string MEDIFECHA = "MEDIFECHA";
        public string LECTCODI = "LECTCODI";

        public string DCALANIO = "DCALANIO";
        public string DCALMES = "DCALMES";

        public string DCALUSUCREACION = "DCALUSUCREACION";
        public string DCALFECCREACION = "DCALFECCREACION";

        #endregion
      

        #region Campos para resultados
        public string EMPRESA = "EMPRESA";
        // Energia Mes 1
        public string RCODI = "RCODI";
        public string RPMES = "RPMES";
        public string RRETIRO = "RRETIRO";
        public string RENTREGA = "RENTREGA";

        // Energia Mes 2
        public string RLVTEA = "RLVTEA";
        public string RVD10 = "RVD10";
        public string RVD11 = "RVD11";
        public string RVPROY = "RVPROY";
        public string RENERGIA = "RENERGIA";
        public string RUSUCREACION = "RUSUCREACION";
        // Potencia mes 1
        public string RPFIRME = "RPFIRME";
        public string RPFIRME1MR = "RPFIRME1MR";
        public string RDMES1 = "RDMES1";
        public string RDMES2 = "RDMES2";
        public string RDMES3 = "RDMES3";
        public string RCLVTP = "RCLVTP";
        public string RMCM2 = "RMCM2";
        public string RMCM3 = "RMCM3";
        // Potencia mes 2
        public string RPLVTP = "RPLVTP";
        public string RMPM2 = "RMPM2";
        public string RMPM3 = "RMPM3";
        public string RCAPACIDAD = "RCAPACIDAD";
        public string RPEAJE = "RPEAJE";

        public string RPREPOT = "RPREPOT";
        public string RPEAJEU = "RPEAJEU";
        public string RMARGENR = "RMARGENR";

        public string RMES1 = "RMES1";
        public string RMES2 = "RMES2";
        public string RMES3 = "RMES3";
        public string RSCOMPLE = "RSCOMPLE";

        public string REPRM1 = "REPRM1";
        public string RPARM1 = "RPARM1";
        public string REPRM2 = "REPRM2";
        public string RPARM2 = "RPARM2";
        public string REPRM3 = "REPRM3";
        public string RPARM3 = "RPARM3";

        public string RER10 = "RER10";
        public string REPR11 = "REPR11";
        //public string REPRM2 = "REPRM2";
        public string RVMERM1 = "RVMERM1";
        public string RCOL1 = "RCOL1";
        public string RCOL2 = "RCOL2";
        public string REREACTIVA = "REREACTIVA";

        public string RLSCIO = "RLSCIO";
        public string RINFLEXOP = "RINFLEXOP";

        public string ENRGN = "ENRGN";
        public string ENRGN1 = "ENRGN1";

        public string RDEMM1 = "RDEMM1";
        public string RDEMM2 = "RDEMM2";
        public string RDEMM3 = "RDEMM3";
        public string RINFLEX = "RINFLEX";


        public string TOTALGARANTIA = "TOTALGARANTIA";
        public string GARANTIADEP = "GARANTIADEP";
        public string FACTOR = "FACTOR";
        public string COMENTARIO = "COMENTARIO";

        public string anio = "anio";
        public string mes = "mes";
        public string periodo = "periodo";
        public string empresa = "empresa";
        public string insumo = "insumo";
        public string valor = "valor";

        public string usuario = "usuario";
        public string fecha = "fecha";

        public string barra = "barra";


        public string energia_t1 = "energia_t1";
        public string energia_t2 = "energia_t2";
        public string energia_t3 = "energia_t3";
        public string energia_t4 = "energia_t4";
        public string energia_t5 = "energia_t5";
        public string energia_t6 = "energia_t6";
        public string energia_t7 = "energia_t7";
        public string energia_t8 = "energia_t8";
        public string energia_t9 = "energia_t9";
        public string energia_t10 = "energia_t10";
        public string energia_t11 = "energia_t11";
        public string energia_t12 = "energia_t12";
        public string energia_t13 = "energia_t13";
        public string energia_t14 = "energia_t14";
        public string energia_t15 = "energia_t15";
        public string energia_t16 = "energia_t16";
        public string energia_t17 = "energia_t17";
        public string energia_t18 = "energia_t18";
        public string energia_t19 = "energia_t19";
        public string energia_t20 = "energia_t20";
        public string energia_t21 = "energia_t21";
        public string energia_t22 = "energia_t22";
        public string energia_t23 = "energia_t23";
        public string energia_t24 = "energia_t24";
        public string energia_t25 = "energia_t25";
        public string energia_t26 = "energia_t26";
        public string energia_t27 = "energia_t27";
        public string energia_t28 = "energia_t28";
        public string energia_t29 = "energia_t29";
        public string energia_t30 = "energia_t30";
        public string energia_t31 = "energia_t31";
        public string energia_t32 = "energia_t32";
        public string energia_t33 = "energia_t33";
        public string energia_t34 = "energia_t34";
        public string energia_t35 = "energia_t35";
        public string energia_t36 = "energia_t36";
        public string energia_t37 = "energia_t37";
        public string energia_t38 = "energia_t38";
        public string energia_t39 = "energia_t39";
        public string energia_t40 = "energia_t40";
        public string energia_t41 = "energia_t41";
        public string energia_t42 = "energia_t42";
        public string energia_t43 = "energia_t43";
        public string energia_t44 = "energia_t44";
        public string energia_t45 = "energia_t45";
        public string energia_t46 = "energia_t46";
        public string energia_t47 = "energia_t47";
        public string energia_t48 = "energia_t48";
        public string energia_t49 = "energia_t49";
        public string energia_t50 = "energia_t50";
        public string energia_t51 = "energia_t51";
        public string energia_t52 = "energia_t52";
        public string energia_t53 = "energia_t53";
        public string energia_t54 = "energia_t54";
        public string energia_t55 = "energia_t55";
        public string energia_t56 = "energia_t56";
        public string energia_t57 = "energia_t57";
        public string energia_t58 = "energia_t58";
        public string energia_t59 = "energia_t59";
        public string energia_t60 = "energia_t60";
        public string energia_t61 = "energia_t61";
        public string energia_t62 = "energia_t62";
        public string energia_t63 = "energia_t63";
        public string energia_t64 = "energia_t64";
        public string energia_t65 = "energia_t65";
        public string energia_t66 = "energia_t66";
        public string energia_t67 = "energia_t67";
        public string energia_t68 = "energia_t68";
        public string energia_t69 = "energia_t69";
        public string energia_t70 = "energia_t70";
        public string energia_t71 = "energia_t71";
        public string energia_t72 = "energia_t72";
        public string energia_t73 = "energia_t73";
        public string energia_t74 = "energia_t74";
        public string energia_t75 = "energia_t75";
        public string energia_t76 = "energia_t76";
        public string energia_t77 = "energia_t77";
        public string energia_t78 = "energia_t78";
        public string energia_t79 = "energia_t79";
        public string energia_t80 = "energia_t80";
        public string energia_t81 = "energia_t81";
        public string energia_t82 = "energia_t82";
        public string energia_t83 = "energia_t83";
        public string energia_t84 = "energia_t84";
        public string energia_t85 = "energia_t85";
        public string energia_t86 = "energia_t86";
        public string energia_t87 = "energia_t87";
        public string energia_t88 = "energia_t88";
        public string energia_t89 = "energia_t89";
        public string energia_t90 = "energia_t90";
        public string energia_t91 = "energia_t91";
        public string energia_t92 = "energia_t92";
        public string energia_t93 = "energia_t93";
        public string energia_t94 = "energia_t94";
        public string energia_t95 = "energia_t95";
        public string energia_t96 = "energia_t96";
        public string cm_t1 = "cm_t1";
        public string cm_t2 = "cm_t2";
        public string cm_t3 = "cm_t3";
        public string cm_t4 = "cm_t4";
        public string cm_t5 = "cm_t5";
        public string cm_t6 = "cm_t6";
        public string cm_t7 = "cm_t7";
        public string cm_t8 = "cm_t8";
        public string cm_t9 = "cm_t9";
        public string cm_t10 = "cm_t10";
        public string cm_t11 = "cm_t11";
        public string cm_t12 = "cm_t12";
        public string cm_t13 = "cm_t13";
        public string cm_t14 = "cm_t14";
        public string cm_t15 = "cm_t15";
        public string cm_t16 = "cm_t16";
        public string cm_t17 = "cm_t17";
        public string cm_t18 = "cm_t18";
        public string cm_t19 = "cm_t19";
        public string cm_t20 = "cm_t20";
        public string cm_t21 = "cm_t21";
        public string cm_t22 = "cm_t22";
        public string cm_t23 = "cm_t23";
        public string cm_t24 = "cm_t24";
        public string cm_t25 = "cm_t25";
        public string cm_t26 = "cm_t26";
        public string cm_t27 = "cm_t27";
        public string cm_t28 = "cm_t28";
        public string cm_t29 = "cm_t29";
        public string cm_t30 = "cm_t30";
        public string cm_t31 = "cm_t31";
        public string cm_t32 = "cm_t32";
        public string cm_t33 = "cm_t33";
        public string cm_t34 = "cm_t34";
        public string cm_t35 = "cm_t35";
        public string cm_t36 = "cm_t36";
        public string cm_t37 = "cm_t37";
        public string cm_t38 = "cm_t38";
        public string cm_t39 = "cm_t39";
        public string cm_t40 = "cm_t40";
        public string cm_t41 = "cm_t41";
        public string cm_t42 = "cm_t42";
        public string cm_t43 = "cm_t43";
        public string cm_t44 = "cm_t44";
        public string cm_t45 = "cm_t45";
        public string cm_t46 = "cm_t46";
        public string cm_t47 = "cm_t47";
        public string cm_t48 = "cm_t48";
        public string cm_t49 = "cm_t49";
        public string cm_t50 = "cm_t50";
        public string cm_t51 = "cm_t51";
        public string cm_t52 = "cm_t52";
        public string cm_t53 = "cm_t53";
        public string cm_t54 = "cm_t54";
        public string cm_t55 = "cm_t55";
        public string cm_t56 = "cm_t56";
        public string cm_t57 = "cm_t57";
        public string cm_t58 = "cm_t58";
        public string cm_t59 = "cm_t59";
        public string cm_t60 = "cm_t60";
        public string cm_t61 = "cm_t61";
        public string cm_t62 = "cm_t62";
        public string cm_t63 = "cm_t63";
        public string cm_t64 = "cm_t64";
        public string cm_t65 = "cm_t65";
        public string cm_t66 = "cm_t66";
        public string cm_t67 = "cm_t67";
        public string cm_t68 = "cm_t68";
        public string cm_t69 = "cm_t69";
        public string cm_t70 = "cm_t70";
        public string cm_t71 = "cm_t71";
        public string cm_t72 = "cm_t72";
        public string cm_t73 = "cm_t73";
        public string cm_t74 = "cm_t74";
        public string cm_t75 = "cm_t75";
        public string cm_t76 = "cm_t76";
        public string cm_t77 = "cm_t77";
        public string cm_t78 = "cm_t78";
        public string cm_t79 = "cm_t79";
        public string cm_t80 = "cm_t80";
        public string cm_t81 = "cm_t81";
        public string cm_t82 = "cm_t82";
        public string cm_t83 = "cm_t83";
        public string cm_t84 = "cm_t84";
        public string cm_t85 = "cm_t85";
        public string cm_t86 = "cm_t86";
        public string cm_t87 = "cm_t87";
        public string cm_t88 = "cm_t88";
        public string cm_t89 = "cm_t89";
        public string cm_t90 = "cm_t90";
        public string cm_t91 = "cm_t91";
        public string cm_t92 = "cm_t92";
        public string cm_t93 = "cm_t93";
        public string cm_t94 = "cm_t94";
        public string cm_t95 = "cm_t95";
        public string cm_t96 = "cm_t96";


        #endregion


      
        public string SqlGetValorDC
        {
            get { return base.GetSqlXml("GetValorDC"); }
        }
        public string SqlUpsertDatos
        {
            get { return base.GetSqlXml("UpsertDatos"); }
        }
        public string SqlDeleteValoresEnergia
        {
            get { return base.GetSqlXml("DeleteValoresEnergia"); }
        }
        public string SqlDeleteValoresEnergiaEntrega
        {
            get { return base.GetSqlXml("DeleteValoresEnergiaEntrega"); }
        }
        public string SqlDeleteValoresDemanda
        {
            get { return base.GetSqlXml("DeleteValoresDemanda"); }
        }
        public string SqlSetPFR
        {
            get { return base.GetSqlXml("SetPFR"); }
        }
        public string SqlSetMEEN10
        {
            get { return base.GetSqlXml("SetMEEN10"); }
        }
        public string SqlSetLVTEA
        {
            get { return base.GetSqlXml("SetLVTEA"); }
        }
        public string SqlSetVTP
        {
            get { return base.GetSqlXml("SetVTP"); }
        }
        public string SqlSetPPO
        {
            get { return base.GetSqlXml("SetPPO"); }
        }
        public string SqlSetValoresAdicionales
        {
            get { return base.GetSqlXml("SetValoresAdicionales"); }
        }
        
        public string SqlSetValoresAdicionalesInsumos
        {
            get { return base.GetSqlXml("SetValoresAdicionalesInsumos"); }
        }
        public string SqlListadoDatosCalculo
        {
            get { return base.GetSqlXml("GetUpsertDatos"); }
        }
        public string SqlActualizarDatosCalculo
        {
            get { return base.GetSqlXml("UpdateDatCalculo"); }
        }
        public string SqlEliminarDatosCalculo
        {
            get { return base.GetSqlXml("DeleteDatCalculo"); }
        }
        public string SqlConsultaParticipanteExistente
        {
            get { return base.GetSqlXml("ConsultaParticipanteExistente"); }
        }
        public string SqlConsultaEstadoPeriodo
        {
            get { return base.GetSqlXml("ConsultaEstadoPeriodo"); }
        }
        public string SqlSetPREG
        {
            get { return base.GetSqlXml("SetPREG"); }
        }
        public string SqlSetDemandaCOES
        {
            get { return base.GetSqlXml("SetDemandaCOES"); }
        }
        public string SqlSetVDIO10
        {
            get { return base.GetSqlXml("SetVDIO10"); }
        }
        public string SqlSetVDSC10
        {
            get { return base.GetSqlXml("SetVDSC10"); }
        }
        public string SqlSetVDER10
        {
            get { return base.GetSqlXml("SetVDER10"); }
        }
        public string SqlSetMPE
        {
            get { return base.GetSqlXml("SetMPE"); }
        }
        public string SqlSetPEAR
        {
            get { return base.GetSqlXml("SetPEAR"); }
        }
        public string SqlDelENRG10
        {
            get { return base.GetSqlXml("DelENRG10"); }
        }
        public string SqlDelENRG11
        {
            get { return base.GetSqlXml("DelENRG11"); }
        }
        public string SqlDelENRGN1
        {
            get { return base.GetSqlXml("DelENRGN1"); }
        }
        public string SqlDelENTPRE
        {
            get { return base.GetSqlXml("DelENTPRE"); }
        }
        public string SqlSetENTPRE
        {
            get { return base.GetSqlXml("SetENTPRE"); }
        }
        public string SqlSetENRG10
        {
            get { return base.GetSqlXml("SetENRG10"); }
        }
        public string SqlSetENRG11
        {
            get { return base.GetSqlXml("SetENRG11"); }
        }
        public string SqlSetENRGN1
        {
            get { return base.GetSqlXml("SetENRGN1"); }
        }
        public string SqlSetPerimsjpaso1
        {
            get { return base.GetSqlXml("SetPerimsjpaso1"); }
        }
        public string SqlSetPerimsjpaso2
        {
            get { return base.GetSqlXml("SetPerimsjpaso2"); }
        }
        public string SqlSetPerimsjpaso3
        {
            get { return base.GetSqlXml("SetPerimsjpaso3"); }
        }
        public string SqlSetPeriEstado
        {
            get { return base.GetSqlXml("SetPeriEstado"); }
        }
        public GmmDatCalculoDTO CreateListaValores1Originales(IDataReader dr)
        {
            GmmDatCalculoDTO entity = new GmmDatCalculoDTO();

            int iDCALVALOR = dr.GetOrdinal(this.DCALVALOR);
            if (!dr.IsDBNull(iDCALVALOR)) entity.DCALVALOR = Convert.ToDecimal(dr.GetValue(iDCALVALOR));
            int iEMPRCODI = dr.GetOrdinal(this.EMPRCODI);
            if (!dr.IsDBNull(iEMPRCODI)) entity.EMPRCODI = Convert.ToInt32(dr.GetValue(iEMPRCODI));
            int iBARRCODI = dr.GetOrdinal(this.BARRCODI);
            if (!dr.IsDBNull(iBARRCODI)) entity.BARRCODI = Convert.ToInt32(dr.GetValue(iBARRCODI));
            int iPTOMEDICODI = dr.GetOrdinal(this.PTOMEDICODI);
            if (!dr.IsDBNull(iPTOMEDICODI)) entity.PTOMEDICODI = Convert.ToInt32(dr.GetValue(iPTOMEDICODI));
            int iTPTOMEDICODI = dr.GetOrdinal(this.TPTOMEDICODI);
            if (!dr.IsDBNull(iTPTOMEDICODI)) entity.TPTOMEDICODI = Convert.ToInt32(dr.GetValue(iTPTOMEDICODI));
            int iTIPOINFOCODI = dr.GetOrdinal(this.TIPOINFOCODI);
            if (!dr.IsDBNull(iPTOMEDICODI)) entity.TIPOINFOCODI = Convert.ToInt32(dr.GetValue(iTIPOINFOCODI));
            int iEMedifecha = dr.GetOrdinal(this.MEDIFECHA);
            if (!dr.IsDBNull(iEMedifecha)) entity.MEDIFECHA = dr.GetDateTime(iEMedifecha);
            int iLECTCODI = dr.GetOrdinal(this.LECTCODI);
            if (!dr.IsDBNull(iLECTCODI)) entity.LECTCODI = Convert.ToInt32(dr.GetValue(iLECTCODI));
            int iDCALANIO = dr.GetOrdinal(this.DCALANIO);
            if (!dr.IsDBNull(iDCALANIO)) entity.DCALANIO = Convert.ToInt32(dr.GetValue(iDCALANIO));
            int iDCALMES = dr.GetOrdinal(this.DCALMES);
            if (!dr.IsDBNull(iDCALMES)) entity.DCALMES = Convert.ToInt32(dr.GetValue(iDCALMES));

            return entity;

        }
        public string SqlCleanGarantia
        {
            get { return base.GetSqlXml("CleanGarantia"); }
        }
        public string SqlCleanPotencia
        {
            get { return base.GetSqlXml("CleanPotencia"); }
        }
        public string SqlCleanServicioComp
        {
            get { return base.GetSqlXml("CleanServicioComp"); }
        }
        public string SqlCleaninflexibilidadOpe
        {
            get { return base.GetSqlXml("CleaninflexibilidadOpe"); }
        }
        public string SqlCleanEnergiaReactiva
        {
            get { return base.GetSqlXml("CleanEnergiaReactiva"); }
        }
        public string SqlSetGarantiaEnergia1mes
        {
            get { return base.GetSqlXml("SetGarantiaEnergia1mes"); }
        }
        public string SqlSetGarantiaEnergia1mesComp
        {
            get { return base.GetSqlXml("SetGarantiaEnergia1mesComp"); }
        }
        public string SqlSetGarantiaEnergiaComp
        {
            get { return base.GetSqlXml("SetGarantiaEnergiaComp"); }
        }
        public string SqlSetGarantiaEnergia
        {
            get { return base.GetSqlXml("SetGarantiaEnergia"); }
        }
        public string SqlSetGarantiaPotenciaPeaje1mes
        {
            get { return base.GetSqlXml("SetGarantiaPotenciaPeaje1mes"); }
        }
        public string SqlSetGarantiaPotenciaPeaje1mesComp
        {
            get { return base.GetSqlXml("SetGarantiaPotenciaPeaje1mesComp"); }
        }
        public string SqlSetGarantiaPotenciaPeaje
        {
            get { return base.GetSqlXml("SetGarantiaPotenciaPeaje"); }
        }
        public string SqlSetGarantiaPotenciaPeajeComp
        {
            get { return base.GetSqlXml("SetGarantiaPotenciaPeajeComp"); }
        }
        public string SqlSetGarantiaServiciosCompComp
        {
            get { return base.GetSqlXml("SetGarantiaServiciosCompComp"); }
        }
        public string SqlSetGarantiaServiciosComp1mes
        {
            get { return base.GetSqlXml("SetGarantiaServiciosComp1mes"); }
        }
        public string SqlSetGarantiaServiciosComp
        {
            get { return base.GetSqlXml("SetGarantiaServiciosComp"); }
        }
        public string SqlSetGarantiaEnergiaReactiva1mes
        {
            get { return base.GetSqlXml("SetGarantiaEnergiaReactiva1mes"); }
        }
        public string SqlSetGarantiaEnergiaReactiva1mesComp
        {
            get { return base.GetSqlXml("SetGarantiaEnergiaReactiva1mesComp"); }
        }
        public string SqlSetGarantiaEnergiaReactiva
        {
            get { return base.GetSqlXml("SetGarantiaEnergiaReactiva"); }
        }
        public string SqlSetGarantiaEnergiaReactivaComp
        {
            get { return base.GetSqlXml("SetGarantiaEnergiaReactivaComp"); }
        }

        public string SqlSetGarantiainflexibilidadOpe1mes
        {
            get { return base.GetSqlXml("SetGarantiainflexibilidadOpe1mes"); }
        }
        public string SqlSetGarantiainflexibilidadOpe1mesComp
        {
            get { return base.GetSqlXml("SetGarantiainflexibilidadOpe1mesComp"); }
        }
        public string SqlSetGarantiainflexibilidadOpe
        {
            get { return base.GetSqlXml("SetGarantiainflexibilidadOpe"); }
        }
        public string SqlSetGarantiainflexibilidadOpeComp
        {
            get { return base.GetSqlXml("SetGarantiainflexibilidadOpeComp"); }
        }
        public string SqllistarRptEnergia
        {
            get { return base.GetSqlXml("listarRptEnergia"); }
        }
        public string SqllistarRptInsumo
        {
            get { return base.GetSqlXml("listarRptInsumo"); }
        }
        public string SqllistarRpt1
        {
            get { return base.GetSqlXml("listarRpt1"); }
        }
        public string SqllistarRpt2
        {
            get { return base.GetSqlXml("listarRpt2"); }
        }
        public string SqllistarRpt3
        {
            get { return base.GetSqlXml("listarRpt3"); }
        }
        public string SqllistarRpt4
        {
            get { return base.GetSqlXml("listarRpt4"); }
        }
        public string SqllistarRpt5
        {
            get { return base.GetSqlXml("listarRpt5"); }
        }
        public string SqllistarRpt6
        {
            get { return base.GetSqlXml("listarRpt6"); }
        }
        public GmmDatCalculoDTO CreateListaRptEnergia(IDataReader dr)
        {
            GmmDatCalculoDTO entity = new GmmDatCalculoDTO();

            int iempresa = dr.GetOrdinal(this.empresa);
            if (!dr.IsDBNull(iempresa)) entity.empresa = dr.GetString(iempresa);
            int ianio = dr.GetOrdinal(this.anio);
            if (!dr.IsDBNull(ianio)) entity.anio = dr.GetDecimal(ianio);
            int imes = dr.GetOrdinal(this.mes);
            if (!dr.IsDBNull(imes)) entity.mes = dr.GetString(imes);
            int ibarra = dr.GetOrdinal(this.barra);
            if (!dr.IsDBNull(ibarra)) entity.barra = dr.GetString(ibarra);
            int ifecha = dr.GetOrdinal(this.fecha);
            if (!dr.IsDBNull(ifecha)) entity.fecha = dr.GetDateTime(ifecha);

            int ienergia_t1 = dr.GetOrdinal(this.energia_t1);
            if (!dr.IsDBNull(ienergia_t1)) entity.energia_t1 = dr.GetDecimal(ienergia_t1);
            int ienergia_t2 = dr.GetOrdinal(this.energia_t2);
            if (!dr.IsDBNull(ienergia_t2)) entity.energia_t2 = dr.GetDecimal(ienergia_t2);
            int ienergia_t3 = dr.GetOrdinal(this.energia_t3);
            if (!dr.IsDBNull(ienergia_t3)) entity.energia_t3 = dr.GetDecimal(ienergia_t3);
            int ienergia_t4 = dr.GetOrdinal(this.energia_t4);
            if (!dr.IsDBNull(ienergia_t4)) entity.energia_t4 = dr.GetDecimal(ienergia_t4);
            int ienergia_t5 = dr.GetOrdinal(this.energia_t5);
            if (!dr.IsDBNull(ienergia_t5)) entity.energia_t5 = dr.GetDecimal(ienergia_t5);
            int ienergia_t6 = dr.GetOrdinal(this.energia_t6);
            if (!dr.IsDBNull(ienergia_t6)) entity.energia_t6 = dr.GetDecimal(ienergia_t6);
            int ienergia_t7 = dr.GetOrdinal(this.energia_t7);
            if (!dr.IsDBNull(ienergia_t7)) entity.energia_t7 = dr.GetDecimal(ienergia_t7);
            int ienergia_t8 = dr.GetOrdinal(this.energia_t8);
            if (!dr.IsDBNull(ienergia_t8)) entity.energia_t8 = dr.GetDecimal(ienergia_t8);
            int ienergia_t9 = dr.GetOrdinal(this.energia_t9);
            if (!dr.IsDBNull(ienergia_t9)) entity.energia_t9 = dr.GetDecimal(ienergia_t9);
            int ienergia_t10 = dr.GetOrdinal(this.energia_t10);
            if (!dr.IsDBNull(ienergia_t10)) entity.energia_t10 = dr.GetDecimal(ienergia_t10);
            int ienergia_t11 = dr.GetOrdinal(this.energia_t11);
            if (!dr.IsDBNull(ienergia_t11)) entity.energia_t11 = dr.GetDecimal(ienergia_t11);
            int ienergia_t12 = dr.GetOrdinal(this.energia_t12);
            if (!dr.IsDBNull(ienergia_t12)) entity.energia_t12 = dr.GetDecimal(ienergia_t12);
            int ienergia_t13 = dr.GetOrdinal(this.energia_t13);
            if (!dr.IsDBNull(ienergia_t13)) entity.energia_t13 = dr.GetDecimal(ienergia_t13);
            int ienergia_t14 = dr.GetOrdinal(this.energia_t14);
            if (!dr.IsDBNull(ienergia_t14)) entity.energia_t14 = dr.GetDecimal(ienergia_t14);
            int ienergia_t15 = dr.GetOrdinal(this.energia_t15);
            if (!dr.IsDBNull(ienergia_t15)) entity.energia_t15 = dr.GetDecimal(ienergia_t15);
            int ienergia_t16 = dr.GetOrdinal(this.energia_t16);
            if (!dr.IsDBNull(ienergia_t16)) entity.energia_t16 = dr.GetDecimal(ienergia_t16);
            int ienergia_t17 = dr.GetOrdinal(this.energia_t17);
            if (!dr.IsDBNull(ienergia_t17)) entity.energia_t17 = dr.GetDecimal(ienergia_t17);
            int ienergia_t18 = dr.GetOrdinal(this.energia_t18);
            if (!dr.IsDBNull(ienergia_t18)) entity.energia_t18 = dr.GetDecimal(ienergia_t18);
            int ienergia_t19 = dr.GetOrdinal(this.energia_t19);
            if (!dr.IsDBNull(ienergia_t19)) entity.energia_t19 = dr.GetDecimal(ienergia_t19);
            int ienergia_t20 = dr.GetOrdinal(this.energia_t20);
            if (!dr.IsDBNull(ienergia_t20)) entity.energia_t20 = dr.GetDecimal(ienergia_t20);
            int ienergia_t21 = dr.GetOrdinal(this.energia_t21);
            if (!dr.IsDBNull(ienergia_t21)) entity.energia_t21 = dr.GetDecimal(ienergia_t21);
            int ienergia_t22 = dr.GetOrdinal(this.energia_t22);
            if (!dr.IsDBNull(ienergia_t22)) entity.energia_t22 = dr.GetDecimal(ienergia_t22);
            int ienergia_t23 = dr.GetOrdinal(this.energia_t23);
            if (!dr.IsDBNull(ienergia_t23)) entity.energia_t23 = dr.GetDecimal(ienergia_t23);
            int ienergia_t24 = dr.GetOrdinal(this.energia_t24);
            if (!dr.IsDBNull(ienergia_t24)) entity.energia_t24 = dr.GetDecimal(ienergia_t24);
            int ienergia_t25 = dr.GetOrdinal(this.energia_t25);
            if (!dr.IsDBNull(ienergia_t25)) entity.energia_t25 = dr.GetDecimal(ienergia_t25);
            int ienergia_t26 = dr.GetOrdinal(this.energia_t26);
            if (!dr.IsDBNull(ienergia_t26)) entity.energia_t26 = dr.GetDecimal(ienergia_t26);
            int ienergia_t27 = dr.GetOrdinal(this.energia_t27);
            if (!dr.IsDBNull(ienergia_t27)) entity.energia_t27 = dr.GetDecimal(ienergia_t27);
            int ienergia_t28 = dr.GetOrdinal(this.energia_t28);
            if (!dr.IsDBNull(ienergia_t28)) entity.energia_t28 = dr.GetDecimal(ienergia_t28);
            int ienergia_t29 = dr.GetOrdinal(this.energia_t29);
            if (!dr.IsDBNull(ienergia_t29)) entity.energia_t29 = dr.GetDecimal(ienergia_t29);
            int ienergia_t30 = dr.GetOrdinal(this.energia_t30);
            if (!dr.IsDBNull(ienergia_t30)) entity.energia_t30 = dr.GetDecimal(ienergia_t30);
            int ienergia_t31 = dr.GetOrdinal(this.energia_t31);
            if (!dr.IsDBNull(ienergia_t31)) entity.energia_t31 = dr.GetDecimal(ienergia_t31);
            int ienergia_t32 = dr.GetOrdinal(this.energia_t32);
            if (!dr.IsDBNull(ienergia_t32)) entity.energia_t32 = dr.GetDecimal(ienergia_t32);
            int ienergia_t33 = dr.GetOrdinal(this.energia_t33);
            if (!dr.IsDBNull(ienergia_t33)) entity.energia_t33 = dr.GetDecimal(ienergia_t33);
            int ienergia_t34 = dr.GetOrdinal(this.energia_t34);
            if (!dr.IsDBNull(ienergia_t34)) entity.energia_t34 = dr.GetDecimal(ienergia_t34);
            int ienergia_t35 = dr.GetOrdinal(this.energia_t35);
            if (!dr.IsDBNull(ienergia_t35)) entity.energia_t35 = dr.GetDecimal(ienergia_t35);
            int ienergia_t36 = dr.GetOrdinal(this.energia_t36);
            if (!dr.IsDBNull(ienergia_t36)) entity.energia_t36 = dr.GetDecimal(ienergia_t36);
            int ienergia_t37 = dr.GetOrdinal(this.energia_t37);
            if (!dr.IsDBNull(ienergia_t37)) entity.energia_t37 = dr.GetDecimal(ienergia_t37);
            int ienergia_t38 = dr.GetOrdinal(this.energia_t38);
            if (!dr.IsDBNull(ienergia_t38)) entity.energia_t38 = dr.GetDecimal(ienergia_t38);
            int ienergia_t39 = dr.GetOrdinal(this.energia_t39);
            if (!dr.IsDBNull(ienergia_t39)) entity.energia_t39 = dr.GetDecimal(ienergia_t39);
            int ienergia_t40 = dr.GetOrdinal(this.energia_t40);
            if (!dr.IsDBNull(ienergia_t40)) entity.energia_t40 = dr.GetDecimal(ienergia_t40);
            int ienergia_t41 = dr.GetOrdinal(this.energia_t41);
            if (!dr.IsDBNull(ienergia_t41)) entity.energia_t41 = dr.GetDecimal(ienergia_t41);
            int ienergia_t42 = dr.GetOrdinal(this.energia_t42);
            if (!dr.IsDBNull(ienergia_t42)) entity.energia_t42 = dr.GetDecimal(ienergia_t42);
            int ienergia_t43 = dr.GetOrdinal(this.energia_t43);
            if (!dr.IsDBNull(ienergia_t43)) entity.energia_t43 = dr.GetDecimal(ienergia_t43);
            int ienergia_t44 = dr.GetOrdinal(this.energia_t44);
            if (!dr.IsDBNull(ienergia_t44)) entity.energia_t44 = dr.GetDecimal(ienergia_t44);
            int ienergia_t45 = dr.GetOrdinal(this.energia_t45);
            if (!dr.IsDBNull(ienergia_t45)) entity.energia_t45 = dr.GetDecimal(ienergia_t45);
            int ienergia_t46 = dr.GetOrdinal(this.energia_t46);
            if (!dr.IsDBNull(ienergia_t46)) entity.energia_t46 = dr.GetDecimal(ienergia_t46);
            int ienergia_t47 = dr.GetOrdinal(this.energia_t47);
            if (!dr.IsDBNull(ienergia_t47)) entity.energia_t47 = dr.GetDecimal(ienergia_t47);
            int ienergia_t48 = dr.GetOrdinal(this.energia_t48);
            if (!dr.IsDBNull(ienergia_t48)) entity.energia_t48 = dr.GetDecimal(ienergia_t48);
            int ienergia_t49 = dr.GetOrdinal(this.energia_t49);
            if (!dr.IsDBNull(ienergia_t49)) entity.energia_t49 = dr.GetDecimal(ienergia_t49);
            int ienergia_t50 = dr.GetOrdinal(this.energia_t50);
            if (!dr.IsDBNull(ienergia_t50)) entity.energia_t50 = dr.GetDecimal(ienergia_t50);
            int ienergia_t51 = dr.GetOrdinal(this.energia_t51);
            if (!dr.IsDBNull(ienergia_t51)) entity.energia_t51 = dr.GetDecimal(ienergia_t51);
            int ienergia_t52 = dr.GetOrdinal(this.energia_t52);
            if (!dr.IsDBNull(ienergia_t52)) entity.energia_t52 = dr.GetDecimal(ienergia_t52);
            int ienergia_t53 = dr.GetOrdinal(this.energia_t53);
            if (!dr.IsDBNull(ienergia_t53)) entity.energia_t53 = dr.GetDecimal(ienergia_t53);
            int ienergia_t54 = dr.GetOrdinal(this.energia_t54);
            if (!dr.IsDBNull(ienergia_t54)) entity.energia_t54 = dr.GetDecimal(ienergia_t54);
            int ienergia_t55 = dr.GetOrdinal(this.energia_t55);
            if (!dr.IsDBNull(ienergia_t55)) entity.energia_t55 = dr.GetDecimal(ienergia_t55);
            int ienergia_t56 = dr.GetOrdinal(this.energia_t56);
            if (!dr.IsDBNull(ienergia_t56)) entity.energia_t56 = dr.GetDecimal(ienergia_t56);
            int ienergia_t57 = dr.GetOrdinal(this.energia_t57);
            if (!dr.IsDBNull(ienergia_t57)) entity.energia_t57 = dr.GetDecimal(ienergia_t57);
            int ienergia_t58 = dr.GetOrdinal(this.energia_t58);
            if (!dr.IsDBNull(ienergia_t58)) entity.energia_t58 = dr.GetDecimal(ienergia_t58);
            int ienergia_t59 = dr.GetOrdinal(this.energia_t59);
            if (!dr.IsDBNull(ienergia_t59)) entity.energia_t59 = dr.GetDecimal(ienergia_t59);
            int ienergia_t60 = dr.GetOrdinal(this.energia_t60);
            if (!dr.IsDBNull(ienergia_t60)) entity.energia_t60 = dr.GetDecimal(ienergia_t60);
            int ienergia_t61 = dr.GetOrdinal(this.energia_t61);
            if (!dr.IsDBNull(ienergia_t61)) entity.energia_t61 = dr.GetDecimal(ienergia_t61);
            int ienergia_t62 = dr.GetOrdinal(this.energia_t62);
            if (!dr.IsDBNull(ienergia_t62)) entity.energia_t62 = dr.GetDecimal(ienergia_t62);
            int ienergia_t63 = dr.GetOrdinal(this.energia_t63);
            if (!dr.IsDBNull(ienergia_t63)) entity.energia_t63 = dr.GetDecimal(ienergia_t63);
            int ienergia_t64 = dr.GetOrdinal(this.energia_t64);
            if (!dr.IsDBNull(ienergia_t64)) entity.energia_t64 = dr.GetDecimal(ienergia_t64);
            int ienergia_t65 = dr.GetOrdinal(this.energia_t65);
            if (!dr.IsDBNull(ienergia_t65)) entity.energia_t65 = dr.GetDecimal(ienergia_t65);
            int ienergia_t66 = dr.GetOrdinal(this.energia_t66);
            if (!dr.IsDBNull(ienergia_t66)) entity.energia_t66 = dr.GetDecimal(ienergia_t66);
            int ienergia_t67 = dr.GetOrdinal(this.energia_t67);
            if (!dr.IsDBNull(ienergia_t67)) entity.energia_t67 = dr.GetDecimal(ienergia_t67);
            int ienergia_t68 = dr.GetOrdinal(this.energia_t68);
            if (!dr.IsDBNull(ienergia_t68)) entity.energia_t68 = dr.GetDecimal(ienergia_t68);
            int ienergia_t69 = dr.GetOrdinal(this.energia_t69);
            if (!dr.IsDBNull(ienergia_t69)) entity.energia_t69 = dr.GetDecimal(ienergia_t69);
            int ienergia_t70 = dr.GetOrdinal(this.energia_t70);
            if (!dr.IsDBNull(ienergia_t70)) entity.energia_t70 = dr.GetDecimal(ienergia_t70);
            int ienergia_t71 = dr.GetOrdinal(this.energia_t71);
            if (!dr.IsDBNull(ienergia_t71)) entity.energia_t71 = dr.GetDecimal(ienergia_t71);
            int ienergia_t72 = dr.GetOrdinal(this.energia_t72);
            if (!dr.IsDBNull(ienergia_t72)) entity.energia_t72 = dr.GetDecimal(ienergia_t72);
            int ienergia_t73 = dr.GetOrdinal(this.energia_t73);
            if (!dr.IsDBNull(ienergia_t73)) entity.energia_t73 = dr.GetDecimal(ienergia_t73);
            int ienergia_t74 = dr.GetOrdinal(this.energia_t74);
            if (!dr.IsDBNull(ienergia_t74)) entity.energia_t74 = dr.GetDecimal(ienergia_t74);
            int ienergia_t75 = dr.GetOrdinal(this.energia_t75);
            if (!dr.IsDBNull(ienergia_t75)) entity.energia_t75 = dr.GetDecimal(ienergia_t75);
            int ienergia_t76 = dr.GetOrdinal(this.energia_t76);
            if (!dr.IsDBNull(ienergia_t76)) entity.energia_t76 = dr.GetDecimal(ienergia_t76);
            int ienergia_t77 = dr.GetOrdinal(this.energia_t77);
            if (!dr.IsDBNull(ienergia_t77)) entity.energia_t77 = dr.GetDecimal(ienergia_t77);
            int ienergia_t78 = dr.GetOrdinal(this.energia_t78);
            if (!dr.IsDBNull(ienergia_t78)) entity.energia_t78 = dr.GetDecimal(ienergia_t78);
            int ienergia_t79 = dr.GetOrdinal(this.energia_t79);
            if (!dr.IsDBNull(ienergia_t79)) entity.energia_t79 = dr.GetDecimal(ienergia_t79);
            int ienergia_t80 = dr.GetOrdinal(this.energia_t80);
            if (!dr.IsDBNull(ienergia_t80)) entity.energia_t80 = dr.GetDecimal(ienergia_t80);
            int ienergia_t81 = dr.GetOrdinal(this.energia_t81);
            if (!dr.IsDBNull(ienergia_t81)) entity.energia_t81 = dr.GetDecimal(ienergia_t81);
            int ienergia_t82 = dr.GetOrdinal(this.energia_t82);
            if (!dr.IsDBNull(ienergia_t82)) entity.energia_t82 = dr.GetDecimal(ienergia_t82);
            int ienergia_t83 = dr.GetOrdinal(this.energia_t83);
            if (!dr.IsDBNull(ienergia_t83)) entity.energia_t83 = dr.GetDecimal(ienergia_t83);
            int ienergia_t84 = dr.GetOrdinal(this.energia_t84);
            if (!dr.IsDBNull(ienergia_t84)) entity.energia_t84 = dr.GetDecimal(ienergia_t84);
            int ienergia_t85 = dr.GetOrdinal(this.energia_t85);
            if (!dr.IsDBNull(ienergia_t85)) entity.energia_t85 = dr.GetDecimal(ienergia_t85);
            int ienergia_t86 = dr.GetOrdinal(this.energia_t86);
            if (!dr.IsDBNull(ienergia_t86)) entity.energia_t86 = dr.GetDecimal(ienergia_t86);
            int ienergia_t87 = dr.GetOrdinal(this.energia_t87);
            if (!dr.IsDBNull(ienergia_t87)) entity.energia_t87 = dr.GetDecimal(ienergia_t87);
            int ienergia_t88 = dr.GetOrdinal(this.energia_t88);
            if (!dr.IsDBNull(ienergia_t88)) entity.energia_t88 = dr.GetDecimal(ienergia_t88);
            int ienergia_t89 = dr.GetOrdinal(this.energia_t89);
            if (!dr.IsDBNull(ienergia_t89)) entity.energia_t89 = dr.GetDecimal(ienergia_t89);
            int ienergia_t90 = dr.GetOrdinal(this.energia_t90);
            if (!dr.IsDBNull(ienergia_t90)) entity.energia_t90 = dr.GetDecimal(ienergia_t90);
            int ienergia_t91 = dr.GetOrdinal(this.energia_t91);
            if (!dr.IsDBNull(ienergia_t91)) entity.energia_t91 = dr.GetDecimal(ienergia_t91);
            int ienergia_t92 = dr.GetOrdinal(this.energia_t92);
            if (!dr.IsDBNull(ienergia_t92)) entity.energia_t92 = dr.GetDecimal(ienergia_t92);
            int ienergia_t93 = dr.GetOrdinal(this.energia_t93);
            if (!dr.IsDBNull(ienergia_t93)) entity.energia_t93 = dr.GetDecimal(ienergia_t93);
            int ienergia_t94 = dr.GetOrdinal(this.energia_t94);
            if (!dr.IsDBNull(ienergia_t94)) entity.energia_t94 = dr.GetDecimal(ienergia_t94);
            int ienergia_t95 = dr.GetOrdinal(this.energia_t95);
            if (!dr.IsDBNull(ienergia_t95)) entity.energia_t95 = dr.GetDecimal(ienergia_t95);
            int ienergia_t96 = dr.GetOrdinal(this.energia_t96);
            if (!dr.IsDBNull(ienergia_t96)) entity.energia_t96 = dr.GetDecimal(ienergia_t96);

            int icm_t1 = dr.GetOrdinal(this.cm_t1);
            if (!dr.IsDBNull(icm_t1)) entity.cm_t1 = dr.GetDecimal(icm_t1);
            int icm_t2 = dr.GetOrdinal(this.cm_t2);
            if (!dr.IsDBNull(icm_t2)) entity.cm_t2 = dr.GetDecimal(icm_t2);
            int icm_t3 = dr.GetOrdinal(this.cm_t3);
            if (!dr.IsDBNull(icm_t3)) entity.cm_t3 = dr.GetDecimal(icm_t3);
            int icm_t4 = dr.GetOrdinal(this.cm_t4);
            if (!dr.IsDBNull(icm_t4)) entity.cm_t4 = dr.GetDecimal(icm_t4);
            int icm_t5 = dr.GetOrdinal(this.cm_t5);
            if (!dr.IsDBNull(icm_t5)) entity.cm_t5 = dr.GetDecimal(icm_t5);
            int icm_t6 = dr.GetOrdinal(this.cm_t6);
            if (!dr.IsDBNull(icm_t6)) entity.cm_t6 = dr.GetDecimal(icm_t6);
            int icm_t7 = dr.GetOrdinal(this.cm_t7);
            if (!dr.IsDBNull(icm_t7)) entity.cm_t7 = dr.GetDecimal(icm_t7);
            int icm_t8 = dr.GetOrdinal(this.cm_t8);
            if (!dr.IsDBNull(icm_t8)) entity.cm_t8 = dr.GetDecimal(icm_t8);
            int icm_t9 = dr.GetOrdinal(this.cm_t9);
            if (!dr.IsDBNull(icm_t9)) entity.cm_t9 = dr.GetDecimal(icm_t9);
            int icm_t10 = dr.GetOrdinal(this.cm_t10);
            if (!dr.IsDBNull(icm_t10)) entity.cm_t10 = dr.GetDecimal(icm_t10);
            int icm_t11 = dr.GetOrdinal(this.cm_t11);
            if (!dr.IsDBNull(icm_t11)) entity.cm_t11 = dr.GetDecimal(icm_t11);
            int icm_t12 = dr.GetOrdinal(this.cm_t12);
            if (!dr.IsDBNull(icm_t12)) entity.cm_t12 = dr.GetDecimal(icm_t12);
            int icm_t13 = dr.GetOrdinal(this.cm_t13);
            if (!dr.IsDBNull(icm_t13)) entity.cm_t13 = dr.GetDecimal(icm_t13);
            int icm_t14 = dr.GetOrdinal(this.cm_t14);
            if (!dr.IsDBNull(icm_t14)) entity.cm_t14 = dr.GetDecimal(icm_t14);
            int icm_t15 = dr.GetOrdinal(this.cm_t15);
            if (!dr.IsDBNull(icm_t15)) entity.cm_t15 = dr.GetDecimal(icm_t15);
            int icm_t16 = dr.GetOrdinal(this.cm_t16);
            if (!dr.IsDBNull(icm_t16)) entity.cm_t16 = dr.GetDecimal(icm_t16);
            int icm_t17 = dr.GetOrdinal(this.cm_t17);
            if (!dr.IsDBNull(icm_t17)) entity.cm_t17 = dr.GetDecimal(icm_t17);
            int icm_t18 = dr.GetOrdinal(this.cm_t18);
            if (!dr.IsDBNull(icm_t18)) entity.cm_t18 = dr.GetDecimal(icm_t18);
            int icm_t19 = dr.GetOrdinal(this.cm_t19);
            if (!dr.IsDBNull(icm_t19)) entity.cm_t19 = dr.GetDecimal(icm_t19);
            int icm_t20 = dr.GetOrdinal(this.cm_t20);
            if (!dr.IsDBNull(icm_t20)) entity.cm_t20 = dr.GetDecimal(icm_t20);
            int icm_t21 = dr.GetOrdinal(this.cm_t21);
            if (!dr.IsDBNull(icm_t21)) entity.cm_t21 = dr.GetDecimal(icm_t21);
            int icm_t22 = dr.GetOrdinal(this.cm_t22);
            if (!dr.IsDBNull(icm_t22)) entity.cm_t22 = dr.GetDecimal(icm_t22);
            int icm_t23 = dr.GetOrdinal(this.cm_t23);
            if (!dr.IsDBNull(icm_t23)) entity.cm_t23 = dr.GetDecimal(icm_t23);
            int icm_t24 = dr.GetOrdinal(this.cm_t24);
            if (!dr.IsDBNull(icm_t24)) entity.cm_t24 = dr.GetDecimal(icm_t24);
            int icm_t25 = dr.GetOrdinal(this.cm_t25);
            if (!dr.IsDBNull(icm_t25)) entity.cm_t25 = dr.GetDecimal(icm_t25);
            int icm_t26 = dr.GetOrdinal(this.cm_t26);
            if (!dr.IsDBNull(icm_t26)) entity.cm_t26 = dr.GetDecimal(icm_t26);
            int icm_t27 = dr.GetOrdinal(this.cm_t27);
            if (!dr.IsDBNull(icm_t27)) entity.cm_t27 = dr.GetDecimal(icm_t27);
            int icm_t28 = dr.GetOrdinal(this.cm_t28);
            if (!dr.IsDBNull(icm_t28)) entity.cm_t28 = dr.GetDecimal(icm_t28);
            int icm_t29 = dr.GetOrdinal(this.cm_t29);
            if (!dr.IsDBNull(icm_t29)) entity.cm_t29 = dr.GetDecimal(icm_t29);
            int icm_t30 = dr.GetOrdinal(this.cm_t30);
            if (!dr.IsDBNull(icm_t30)) entity.cm_t30 = dr.GetDecimal(icm_t30);
            int icm_t31 = dr.GetOrdinal(this.cm_t31);
            if (!dr.IsDBNull(icm_t31)) entity.cm_t31 = dr.GetDecimal(icm_t31);
            int icm_t32 = dr.GetOrdinal(this.cm_t32);
            if (!dr.IsDBNull(icm_t32)) entity.cm_t32 = dr.GetDecimal(icm_t32);
            int icm_t33 = dr.GetOrdinal(this.cm_t33);
            if (!dr.IsDBNull(icm_t33)) entity.cm_t33 = dr.GetDecimal(icm_t33);
            int icm_t34 = dr.GetOrdinal(this.cm_t34);
            if (!dr.IsDBNull(icm_t34)) entity.cm_t34 = dr.GetDecimal(icm_t34);
            int icm_t35 = dr.GetOrdinal(this.cm_t35);
            if (!dr.IsDBNull(icm_t35)) entity.cm_t35 = dr.GetDecimal(icm_t35);
            int icm_t36 = dr.GetOrdinal(this.cm_t36);
            if (!dr.IsDBNull(icm_t36)) entity.cm_t36 = dr.GetDecimal(icm_t36);
            int icm_t37 = dr.GetOrdinal(this.cm_t37);
            if (!dr.IsDBNull(icm_t37)) entity.cm_t37 = dr.GetDecimal(icm_t37);
            int icm_t38 = dr.GetOrdinal(this.cm_t38);
            if (!dr.IsDBNull(icm_t38)) entity.cm_t38 = dr.GetDecimal(icm_t38);
            int icm_t39 = dr.GetOrdinal(this.cm_t39);
            if (!dr.IsDBNull(icm_t39)) entity.cm_t39 = dr.GetDecimal(icm_t39);
            int icm_t40 = dr.GetOrdinal(this.cm_t40);
            if (!dr.IsDBNull(icm_t40)) entity.cm_t40 = dr.GetDecimal(icm_t40);
            int icm_t41 = dr.GetOrdinal(this.cm_t41);
            if (!dr.IsDBNull(icm_t41)) entity.cm_t41 = dr.GetDecimal(icm_t41);
            int icm_t42 = dr.GetOrdinal(this.cm_t42);
            if (!dr.IsDBNull(icm_t42)) entity.cm_t42 = dr.GetDecimal(icm_t42);
            int icm_t43 = dr.GetOrdinal(this.cm_t43);
            if (!dr.IsDBNull(icm_t43)) entity.cm_t43 = dr.GetDecimal(icm_t43);
            int icm_t44 = dr.GetOrdinal(this.cm_t44);
            if (!dr.IsDBNull(icm_t44)) entity.cm_t44 = dr.GetDecimal(icm_t44);
            int icm_t45 = dr.GetOrdinal(this.cm_t45);
            if (!dr.IsDBNull(icm_t45)) entity.cm_t45 = dr.GetDecimal(icm_t45);
            int icm_t46 = dr.GetOrdinal(this.cm_t46);
            if (!dr.IsDBNull(icm_t46)) entity.cm_t46 = dr.GetDecimal(icm_t46);
            int icm_t47 = dr.GetOrdinal(this.cm_t47);
            if (!dr.IsDBNull(icm_t47)) entity.cm_t47 = dr.GetDecimal(icm_t47);
            int icm_t48 = dr.GetOrdinal(this.cm_t48);
            if (!dr.IsDBNull(icm_t48)) entity.cm_t48 = dr.GetDecimal(icm_t48);
            int icm_t49 = dr.GetOrdinal(this.cm_t49);
            if (!dr.IsDBNull(icm_t49)) entity.cm_t49 = dr.GetDecimal(icm_t49);
            int icm_t50 = dr.GetOrdinal(this.cm_t50);
            if (!dr.IsDBNull(icm_t50)) entity.cm_t50 = dr.GetDecimal(icm_t50);
            int icm_t51 = dr.GetOrdinal(this.cm_t51);
            if (!dr.IsDBNull(icm_t51)) entity.cm_t51 = dr.GetDecimal(icm_t51);
            int icm_t52 = dr.GetOrdinal(this.cm_t52);
            if (!dr.IsDBNull(icm_t52)) entity.cm_t52 = dr.GetDecimal(icm_t52);
            int icm_t53 = dr.GetOrdinal(this.cm_t53);
            if (!dr.IsDBNull(icm_t53)) entity.cm_t53 = dr.GetDecimal(icm_t53);
            int icm_t54 = dr.GetOrdinal(this.cm_t54);
            if (!dr.IsDBNull(icm_t54)) entity.cm_t54 = dr.GetDecimal(icm_t54);
            int icm_t55 = dr.GetOrdinal(this.cm_t55);
            if (!dr.IsDBNull(icm_t55)) entity.cm_t55 = dr.GetDecimal(icm_t55);
            int icm_t56 = dr.GetOrdinal(this.cm_t56);
            if (!dr.IsDBNull(icm_t56)) entity.cm_t56 = dr.GetDecimal(icm_t56);
            int icm_t57 = dr.GetOrdinal(this.cm_t57);
            if (!dr.IsDBNull(icm_t57)) entity.cm_t57 = dr.GetDecimal(icm_t57);
            int icm_t58 = dr.GetOrdinal(this.cm_t58);
            if (!dr.IsDBNull(icm_t58)) entity.cm_t58 = dr.GetDecimal(icm_t58);
            int icm_t59 = dr.GetOrdinal(this.cm_t59);
            if (!dr.IsDBNull(icm_t59)) entity.cm_t59 = dr.GetDecimal(icm_t59);
            int icm_t60 = dr.GetOrdinal(this.cm_t60);
            if (!dr.IsDBNull(icm_t60)) entity.cm_t60 = dr.GetDecimal(icm_t60);
            int icm_t61 = dr.GetOrdinal(this.cm_t61);
            if (!dr.IsDBNull(icm_t61)) entity.cm_t61 = dr.GetDecimal(icm_t61);
            int icm_t62 = dr.GetOrdinal(this.cm_t62);
            if (!dr.IsDBNull(icm_t62)) entity.cm_t62 = dr.GetDecimal(icm_t62);
            int icm_t63 = dr.GetOrdinal(this.cm_t63);
            if (!dr.IsDBNull(icm_t63)) entity.cm_t63 = dr.GetDecimal(icm_t63);
            int icm_t64 = dr.GetOrdinal(this.cm_t64);
            if (!dr.IsDBNull(icm_t64)) entity.cm_t64 = dr.GetDecimal(icm_t64);
            int icm_t65 = dr.GetOrdinal(this.cm_t65);
            if (!dr.IsDBNull(icm_t65)) entity.cm_t65 = dr.GetDecimal(icm_t65);
            int icm_t66 = dr.GetOrdinal(this.cm_t66);
            if (!dr.IsDBNull(icm_t66)) entity.cm_t66 = dr.GetDecimal(icm_t66);
            int icm_t67 = dr.GetOrdinal(this.cm_t67);
            if (!dr.IsDBNull(icm_t67)) entity.cm_t67 = dr.GetDecimal(icm_t67);
            int icm_t68 = dr.GetOrdinal(this.cm_t68);
            if (!dr.IsDBNull(icm_t68)) entity.cm_t68 = dr.GetDecimal(icm_t68);
            int icm_t69 = dr.GetOrdinal(this.cm_t69);
            if (!dr.IsDBNull(icm_t69)) entity.cm_t69 = dr.GetDecimal(icm_t69);
            int icm_t70 = dr.GetOrdinal(this.cm_t70);
            if (!dr.IsDBNull(icm_t70)) entity.cm_t70 = dr.GetDecimal(icm_t70);
            int icm_t71 = dr.GetOrdinal(this.cm_t71);
            if (!dr.IsDBNull(icm_t71)) entity.cm_t71 = dr.GetDecimal(icm_t71);
            int icm_t72 = dr.GetOrdinal(this.cm_t72);
            if (!dr.IsDBNull(icm_t72)) entity.cm_t72 = dr.GetDecimal(icm_t72);
            int icm_t73 = dr.GetOrdinal(this.cm_t73);
            if (!dr.IsDBNull(icm_t73)) entity.cm_t73 = dr.GetDecimal(icm_t73);
            int icm_t74 = dr.GetOrdinal(this.cm_t74);
            if (!dr.IsDBNull(icm_t74)) entity.cm_t74 = dr.GetDecimal(icm_t74);
            int icm_t75 = dr.GetOrdinal(this.cm_t75);
            if (!dr.IsDBNull(icm_t75)) entity.cm_t75 = dr.GetDecimal(icm_t75);
            int icm_t76 = dr.GetOrdinal(this.cm_t76);
            if (!dr.IsDBNull(icm_t76)) entity.cm_t76 = dr.GetDecimal(icm_t76);
            int icm_t77 = dr.GetOrdinal(this.cm_t77);
            if (!dr.IsDBNull(icm_t77)) entity.cm_t77 = dr.GetDecimal(icm_t77);
            int icm_t78 = dr.GetOrdinal(this.cm_t78);
            if (!dr.IsDBNull(icm_t78)) entity.cm_t78 = dr.GetDecimal(icm_t78);
            int icm_t79 = dr.GetOrdinal(this.cm_t79);
            if (!dr.IsDBNull(icm_t79)) entity.cm_t79 = dr.GetDecimal(icm_t79);
            int icm_t80 = dr.GetOrdinal(this.cm_t80);
            if (!dr.IsDBNull(icm_t80)) entity.cm_t80 = dr.GetDecimal(icm_t80);
            int icm_t81 = dr.GetOrdinal(this.cm_t81);
            if (!dr.IsDBNull(icm_t81)) entity.cm_t81 = dr.GetDecimal(icm_t81);
            int icm_t82 = dr.GetOrdinal(this.cm_t82);
            if (!dr.IsDBNull(icm_t82)) entity.cm_t82 = dr.GetDecimal(icm_t82);
            int icm_t83 = dr.GetOrdinal(this.cm_t83);
            if (!dr.IsDBNull(icm_t83)) entity.cm_t83 = dr.GetDecimal(icm_t83);
            int icm_t84 = dr.GetOrdinal(this.cm_t84);
            if (!dr.IsDBNull(icm_t84)) entity.cm_t84 = dr.GetDecimal(icm_t84);
            int icm_t85 = dr.GetOrdinal(this.cm_t85);
            if (!dr.IsDBNull(icm_t85)) entity.cm_t85 = dr.GetDecimal(icm_t85);
            int icm_t86 = dr.GetOrdinal(this.cm_t86);
            if (!dr.IsDBNull(icm_t86)) entity.cm_t86 = dr.GetDecimal(icm_t86);
            int icm_t87 = dr.GetOrdinal(this.cm_t87);
            if (!dr.IsDBNull(icm_t87)) entity.cm_t87 = dr.GetDecimal(icm_t87);
            int icm_t88 = dr.GetOrdinal(this.cm_t88);
            if (!dr.IsDBNull(icm_t88)) entity.cm_t88 = dr.GetDecimal(icm_t88);
            int icm_t89 = dr.GetOrdinal(this.cm_t89);
            if (!dr.IsDBNull(icm_t89)) entity.cm_t89 = dr.GetDecimal(icm_t89);
            int icm_t90 = dr.GetOrdinal(this.cm_t90);
            if (!dr.IsDBNull(icm_t90)) entity.cm_t90 = dr.GetDecimal(icm_t90);
            int icm_t91 = dr.GetOrdinal(this.cm_t91);
            if (!dr.IsDBNull(icm_t91)) entity.cm_t91 = dr.GetDecimal(icm_t91);
            int icm_t92 = dr.GetOrdinal(this.cm_t92);
            if (!dr.IsDBNull(icm_t92)) entity.cm_t92 = dr.GetDecimal(icm_t92);
            int icm_t93 = dr.GetOrdinal(this.cm_t93);
            if (!dr.IsDBNull(icm_t93)) entity.cm_t93 = dr.GetDecimal(icm_t93);
            int icm_t94 = dr.GetOrdinal(this.cm_t94);
            if (!dr.IsDBNull(icm_t94)) entity.cm_t94 = dr.GetDecimal(icm_t94);
            int icm_t95 = dr.GetOrdinal(this.cm_t95);
            if (!dr.IsDBNull(icm_t95)) entity.cm_t95 = dr.GetDecimal(icm_t95);
            int icm_t96 = dr.GetOrdinal(this.cm_t96);
            if (!dr.IsDBNull(icm_t96)) entity.cm_t96 = dr.GetDecimal(icm_t96);

            return entity;
        }
        public GmmDatCalculoDTO CreateListaRptInsumo(IDataReader dr)
        {
            GmmDatCalculoDTO entity = new GmmDatCalculoDTO();

            int iempresa = dr.GetOrdinal(this.empresa);
            if (!dr.IsDBNull(iempresa)) entity.empresa = dr.GetString(iempresa);
            int iinsumo = dr.GetOrdinal(this.insumo);
            if (!dr.IsDBNull(iinsumo)) entity.insumo = dr.GetString(iinsumo);

            int ianio = dr.GetOrdinal(this.anio);
            if (!dr.IsDBNull(ianio)) entity.anio = dr.GetDecimal(ianio);
            
            int imes = dr.GetOrdinal(this.mes);
            if (!dr.IsDBNull(imes)) entity.mes = dr.GetString(imes);
            

            int iperiodo  = dr.GetOrdinal(this.periodo);
            if (!dr.IsDBNull(iperiodo)) entity.periodo = dr.GetDecimal(iperiodo);
            int ivalor = dr.GetOrdinal(this.valor);
            if (!dr.IsDBNull(ivalor)) entity.valor = dr.GetDecimal(ivalor);

            int iusuario = dr.GetOrdinal(this.usuario);
            if (!dr.IsDBNull(iusuario)) entity.usuario = dr.GetString(iusuario);

            int ifecha = dr.GetOrdinal(this.fecha);
            if (!dr.IsDBNull(ifecha)) entity.fecha = dr.GetDateTime(ifecha);

            return entity;
        }
        public GmmDatCalculoDTO CreateListaRpt1(IDataReader dr)
        {
            GmmDatCalculoDTO entity = new GmmDatCalculoDTO();

            int iEMPRESA = dr.GetOrdinal(this.EMPRESA);
            if (!dr.IsDBNull(iEMPRESA)) entity.EMPRESA = dr.GetString(iEMPRESA);

            //int iEMPGCODI = dr.GetOrdinal(this.EMPGCODI);
            //if (!dr.IsDBNull(iEMPGCODI)) entity.EMPGCODI = dr.GetInt16(iEMPGCODI);
            //int iEMPRCODI = dr.GetOrdinal(this.EMPRCODI);
            //if (!dr.IsDBNull(iEMPRCODI)) entity.EMPRCODI = dr.GetInt16(iEMPRCODI);
            //int iPERICODI = dr.GetOrdinal(this.PERICODI);
            //if (!dr.IsDBNull(iPERICODI)) entity.PERICODI = dr.GetInt16(iPERICODI);

            int iRENERGIA = dr.GetOrdinal(this.RENERGIA);
            if (!dr.IsDBNull(iRENERGIA)) entity.RENERGIA = dr.GetDecimal(iRENERGIA);
            int iRCAPACIDAD = dr.GetOrdinal(this.RCAPACIDAD);
            if (!dr.IsDBNull(iRCAPACIDAD)) entity.RCAPACIDAD = dr.GetDecimal(iRCAPACIDAD);
            int iRPEAJE = dr.GetOrdinal(this.RPEAJE);
            if (!dr.IsDBNull(iRPEAJE)) entity.RPEAJE = dr.GetDecimal(iRPEAJE);
            int iRSCOMPLE = dr.GetOrdinal(this.RSCOMPLE);
            if (!dr.IsDBNull(iRSCOMPLE)) entity.RSCOMPLE = dr.GetDecimal(iRSCOMPLE);
            int iRINFLEXOP = dr.GetOrdinal(this.RINFLEXOP);
            if (!dr.IsDBNull(iRINFLEXOP)) entity.RINFLEXOP = dr.GetDecimal(iRINFLEXOP);
            int iREREACTIVA = dr.GetOrdinal(this.REREACTIVA);
            if (!dr.IsDBNull(iREREACTIVA)) entity.REREACTIVA = dr.GetDecimal(iREREACTIVA);
            int iTOTALGARANTIA = dr.GetOrdinal(this.TOTALGARANTIA);
            if (!dr.IsDBNull(iTOTALGARANTIA)) entity.TOTALGARANTIA = dr.GetDecimal(iTOTALGARANTIA);
            int iGARANTIADEP = dr.GetOrdinal(this.GARANTIADEP);
            if (!dr.IsDBNull(iGARANTIADEP)) entity.GARANTIADEP = dr.GetDecimal(iGARANTIADEP);
            int iFACTOR = dr.GetOrdinal(this.FACTOR);
            if (!dr.IsDBNull(iFACTOR)) entity.FACTOR = dr.GetDecimal(iFACTOR);
            int iCOMENTARIO = dr.GetOrdinal(this.COMENTARIO);
            if (!dr.IsDBNull(iCOMENTARIO)) entity.COMENTARIO = dr.GetString(iCOMENTARIO);

            return entity;
        }

        public GmmDatCalculoDTO CreateListaRpt2(IDataReader dr)
        {
            GmmDatCalculoDTO entity = new GmmDatCalculoDTO();

            int iEMPRESA = dr.GetOrdinal(this.EMPRESA);
            if (!dr.IsDBNull(iEMPRESA)) entity.EMPRESA = dr.GetString(iEMPRESA);

            int iRPMES = dr.GetOrdinal(this.RPMES);
            if (!dr.IsDBNull(iRPMES)) entity.RPMES  = dr.GetString(iRPMES);

            int iEMPGCODI = dr.GetOrdinal(this.EMPGCODI);
            if (!dr.IsDBNull(iEMPGCODI)) entity.EMPGCODI = dr.GetInt16(iEMPGCODI);
            int iEMPRCODI = dr.GetOrdinal(this.EMPRCODI);
            if (!dr.IsDBNull(iEMPRCODI)) entity.EMPRCODI = dr.GetInt16(iEMPRCODI);
            int iPERICODI = dr.GetOrdinal(this.PERICODI);
            if (!dr.IsDBNull(iPERICODI)) entity.PERICODI = dr.GetInt16(iPERICODI);

            int iRENERGIA = dr.GetOrdinal(this.RENERGIA);
            if (!dr.IsDBNull(iRENERGIA)) entity.RENERGIA = dr.GetDecimal(iRENERGIA);
            int iRVPROY = dr.GetOrdinal(this.RVPROY);
            if (!dr.IsDBNull(iRVPROY)) entity.RVPROY = dr.GetDecimal(iRVPROY);
            int iRVD11 = dr.GetOrdinal(this.RVD11);
            if (!dr.IsDBNull(iRVD11)) entity.RVD11 = dr.GetDecimal(iRVD11);
            int iRVD10 = dr.GetOrdinal(this.RVD10);
            if (!dr.IsDBNull(iRVD10)) entity.RVD10 = dr.GetDecimal(iRVD10);
            int iRLVTEA = dr.GetOrdinal(this.RLVTEA);
            if (!dr.IsDBNull(iRLVTEA)) entity.RLVTEA = dr.GetDecimal(iRLVTEA);
            int iRENTREGA = dr.GetOrdinal(this.RENTREGA);
            if (!dr.IsDBNull(iRENTREGA)) entity.RENTREGA = dr.GetDecimal(iRENTREGA);
            int iRRETIRO = dr.GetOrdinal(this.RRETIRO);
            if (!dr.IsDBNull(iRRETIRO)) entity.RRETIRO = dr.GetDecimal(iRRETIRO);
            int iRCODI = dr.GetOrdinal(this.RCODI);
            if (!dr.IsDBNull(iRCODI)) entity.RCODI = dr.GetDecimal(iRCODI);

            return entity;
        }

        public GmmDatCalculoDTO CreateListaRpt3(IDataReader dr)
        {
            GmmDatCalculoDTO entity = new GmmDatCalculoDTO();

            int iEMPRESA = dr.GetOrdinal(this.EMPRESA);
            if (!dr.IsDBNull(iEMPRESA)) entity.EMPRESA = dr.GetString(iEMPRESA);

            int iRPMES = dr.GetOrdinal(this.RPMES);
            if (!dr.IsDBNull(iRPMES)) entity.RPMES = dr.GetString(iRPMES);

            int iEMPGCODI = dr.GetOrdinal(this.EMPGCODI);
            if (!dr.IsDBNull(iEMPGCODI)) entity.EMPGCODI = dr.GetInt16(iEMPGCODI);
            int iEMPRCODI = dr.GetOrdinal(this.EMPRCODI);
            if (!dr.IsDBNull(iEMPRCODI)) entity.EMPRCODI = dr.GetInt16(iEMPRCODI);
            int iPERICODI = dr.GetOrdinal(this.PERICODI);
            if (!dr.IsDBNull(iPERICODI)) entity.PERICODI = dr.GetInt16(iPERICODI);

            int iRPEAJE = dr.GetOrdinal(this.RPEAJE);
            if (!dr.IsDBNull(iRPEAJE)) entity.RPEAJE = dr.GetDecimal(iRPEAJE);
            int iRCAPACIDAD = dr.GetOrdinal(this.RCAPACIDAD);
            if (!dr.IsDBNull(iRCAPACIDAD)) entity.RCAPACIDAD = dr.GetDecimal(iRCAPACIDAD);

            
            int iRPREPOT = dr.GetOrdinal(this.RPREPOT);
            if (!dr.IsDBNull(iRPREPOT)) entity.RPREPOT = dr.GetDecimal(iRPREPOT);
            int iRPEAJEU = dr.GetOrdinal(this.RPEAJEU);
            if (!dr.IsDBNull(iRPEAJEU)) entity.RPEAJEU = dr.GetDecimal(iRPEAJEU);
            int iRMARGENR = dr.GetOrdinal(this.RMARGENR);
            if (!dr.IsDBNull(iRMARGENR)) entity.RMARGENR = dr.GetDecimal(iRMARGENR);


            int iRMPM3 = dr.GetOrdinal(this.RMPM3);
            if (!dr.IsDBNull(iRMPM3)) entity.RMPM3 = dr.GetDecimal(iRMPM3);
            int iRMPM2 = dr.GetOrdinal(this.RMPM2);
            if (!dr.IsDBNull(iRMPM2)) entity.RMPM2 = dr.GetDecimal(iRMPM2);
            int iRPLVTP = dr.GetOrdinal(this.RPLVTP);
            if (!dr.IsDBNull(iRPLVTP)) entity.RPLVTP = dr.GetDecimal(iRPLVTP);
            int iRMCM3 = dr.GetOrdinal(this.RMCM3);
            if (!dr.IsDBNull(iRMCM3)) entity.RMCM3 = dr.GetDecimal(iRMCM3);
            int iRMCM2 = dr.GetOrdinal(this.RMCM2);
            if (!dr.IsDBNull(iRMCM2)) entity.RMCM2 = dr.GetDecimal(iRMCM2);
            int iRCLVTP = dr.GetOrdinal(this.RCLVTP);
            if (!dr.IsDBNull(iRCLVTP)) entity.RCLVTP = dr.GetDecimal(iRCLVTP);
            int iRDMES3 = dr.GetOrdinal(this.RDMES3);
            if (!dr.IsDBNull(iRDMES3)) entity.RDMES3 = dr.GetDecimal(iRDMES3);
            int iRDMES2 = dr.GetOrdinal(this.RDMES2);
            if (!dr.IsDBNull(iRDMES2)) entity.RDMES2 = dr.GetDecimal(iRDMES2);
            int iRDMES1 = dr.GetOrdinal(this.RDMES1);
            if (!dr.IsDBNull(iRDMES1)) entity.RDMES1 = dr.GetDecimal(iRDMES1);
            int iRPFIRME1MR = dr.GetOrdinal(this.RPFIRME1MR);
            if (!dr.IsDBNull(iRPFIRME1MR)) entity.RPFIRME1MR = dr.GetDecimal(iRPFIRME1MR);
            int iRPFIRME = dr.GetOrdinal(this.RPFIRME);
            if (!dr.IsDBNull(iRPFIRME)) entity.RPFIRME = dr.GetDecimal(iRPFIRME);
            int iRCODI = dr.GetOrdinal(this.RCODI);
            if (!dr.IsDBNull(iRCODI)) entity.RCODI = dr.GetDecimal(iRCODI);

            return entity;
        }
        public GmmDatCalculoDTO CreateListaRpt4(IDataReader dr)
        {
            GmmDatCalculoDTO entity = new GmmDatCalculoDTO();

            int iEMPRESA = dr.GetOrdinal(this.EMPRESA);
            if (!dr.IsDBNull(iEMPRESA)) entity.EMPRESA = dr.GetString(iEMPRESA);

            int iRPMES = dr.GetOrdinal(this.RPMES);
            if (!dr.IsDBNull(iRPMES)) entity.RPMES = dr.GetString(iRPMES);

            int iEMPGCODI = dr.GetOrdinal(this.EMPGCODI);
            if (!dr.IsDBNull(iEMPGCODI)) entity.EMPGCODI = dr.GetInt16(iEMPGCODI);
            int iEMPRCODI = dr.GetOrdinal(this.EMPRCODI);
            if (!dr.IsDBNull(iEMPRCODI)) entity.EMPRCODI = dr.GetInt16(iEMPRCODI);
            int iPERICODI = dr.GetOrdinal(this.PERICODI);
            if (!dr.IsDBNull(iPERICODI)) entity.PERICODI = dr.GetInt16(iPERICODI);

            int iRSCOMPLE = dr.GetOrdinal(this.RSCOMPLE);
            if (!dr.IsDBNull(iRSCOMPLE)) entity.RSCOMPLE = dr.GetDecimal(iRSCOMPLE);
            int iRMES3 = dr.GetOrdinal(this.RMES3);
            if (!dr.IsDBNull(iRMES3)) entity.RMES3 = dr.GetDecimal(iRMES3);
            int iRMES2 = dr.GetOrdinal(this.RMES2);
            if (!dr.IsDBNull(iRMES2)) entity.RMES2 = dr.GetDecimal(iRMES2);
            int iRMES1 = dr.GetOrdinal(this.RMES1);
            if (!dr.IsDBNull(iRMES1)) entity.RMES1 = dr.GetDecimal(iRMES1);
            int iRCODI = dr.GetOrdinal(this.RCODI);
            if (!dr.IsDBNull(iRCODI)) entity.RCODI = dr.GetDecimal(iRCODI);

            int iENRGN1 = dr.GetOrdinal(this.ENRGN1);
            if (!dr.IsDBNull(iENRGN1)) entity.ENRGN1 = dr.GetDecimal(iENRGN1);
            int iENRGN = dr.GetOrdinal(this.ENRGN);
            if (!dr.IsDBNull(iENRGN)) entity.ENRGN = dr.GetDecimal(iENRGN);


            return entity;
        }
        public GmmDatCalculoDTO CreateListaRpt5(IDataReader dr)
        {
            GmmDatCalculoDTO entity = new GmmDatCalculoDTO();

            int iEMPRESA = dr.GetOrdinal(this.EMPRESA);
            if (!dr.IsDBNull(iEMPRESA)) entity.EMPRESA = dr.GetString(iEMPRESA);

            int iRPMES = dr.GetOrdinal(this.RPMES);
            if (!dr.IsDBNull(iRPMES)) entity.RPMES = dr.GetString(iRPMES);

            int iEMPGCODI = dr.GetOrdinal(this.EMPGCODI);
            if (!dr.IsDBNull(iEMPGCODI)) entity.EMPGCODI = dr.GetInt16(iEMPGCODI);
            int iEMPRCODI = dr.GetOrdinal(this.EMPRCODI);
            if (!dr.IsDBNull(iEMPRCODI)) entity.EMPRCODI = dr.GetInt16(iEMPRCODI);
            int iPERICODI = dr.GetOrdinal(this.PERICODI);
            if (!dr.IsDBNull(iPERICODI)) entity.PERICODI = dr.GetInt16(iPERICODI);


            int iREREACTIVA = dr.GetOrdinal(this.REREACTIVA);
            if (!dr.IsDBNull(iREREACTIVA)) entity.REREACTIVA = dr.GetDecimal(iREREACTIVA);
            int iRCOL2 = dr.GetOrdinal(this.RCOL2);
            if (!dr.IsDBNull(iRCOL2)) entity.RCOL2 = dr.GetDecimal(iRCOL2);
            int iRCOL1 = dr.GetOrdinal(this.RCOL1);
            if (!dr.IsDBNull(iRCOL1)) entity.RCOL1 = dr.GetDecimal(iRCOL1);
            int iRVMERM1 = dr.GetOrdinal(this.RVMERM1);
            if (!dr.IsDBNull(iRVMERM1)) entity.RVMERM1 = dr.GetDecimal(iRVMERM1);
            int iREPR11 = dr.GetOrdinal(this.REPR11);
            if (!dr.IsDBNull(iREPR11)) entity.REPR11 = dr.GetDecimal(iREPR11);
            int iRER10 = dr.GetOrdinal(this.RER10);
            if (!dr.IsDBNull(iRER10)) entity.RER10 = dr.GetDecimal(iRER10);
            int iRPARM3 = dr.GetOrdinal(this.RPARM3);
            if (!dr.IsDBNull(iRPARM3)) entity.RPARM3 = dr.GetDecimal(iRPARM3);
            int iREPRM3 = dr.GetOrdinal(this.REPRM3);
            if (!dr.IsDBNull(iREPRM3)) entity.REPRM3 = dr.GetDecimal(iREPRM3);
            int iRPARM2 = dr.GetOrdinal(this.RPARM2);
            if (!dr.IsDBNull(iRPARM2)) entity.RPARM2 = dr.GetDecimal(iRPARM2);
            int iREPRM2 = dr.GetOrdinal(this.REPRM2);
            if (!dr.IsDBNull(iREPRM2)) entity.REPRM2 = dr.GetDecimal(iREPRM2);
            int iRPARM1 = dr.GetOrdinal(this.RPARM1);
            if (!dr.IsDBNull(iRPARM1)) entity.RPARM1 = dr.GetDecimal(iRPARM1);
            int iREPRM1 = dr.GetOrdinal(this.REPRM1);
            if (!dr.IsDBNull(iREPRM1)) entity.REPRM1 = dr.GetDecimal(iREPRM1);
            int iRCODI = dr.GetOrdinal(this.RCODI);
            if (!dr.IsDBNull(iRCODI)) entity.RCODI = dr.GetDecimal(iRCODI);


            return entity;
        }
        public GmmDatCalculoDTO CreateListaRpt6(IDataReader dr)
        {
            GmmDatCalculoDTO entity = new GmmDatCalculoDTO();

            int iEMPRESA = dr.GetOrdinal(this.EMPRESA);
            if (!dr.IsDBNull(iEMPRESA)) entity.EMPRESA = dr.GetString(iEMPRESA);

            int iRPMES = dr.GetOrdinal(this.RPMES);
            if (!dr.IsDBNull(iRPMES)) entity.RPMES = dr.GetString(iRPMES);

            int iEMPGCODI = dr.GetOrdinal(this.EMPGCODI);
            if (!dr.IsDBNull(iEMPGCODI)) entity.EMPGCODI = dr.GetInt16(iEMPGCODI);
            int iEMPRCODI = dr.GetOrdinal(this.EMPRCODI);
            if (!dr.IsDBNull(iEMPRCODI)) entity.EMPRCODI = dr.GetInt16(iEMPRCODI);
            int iPERICODI = dr.GetOrdinal(this.PERICODI);
            if (!dr.IsDBNull(iPERICODI)) entity.PERICODI = dr.GetInt16(iPERICODI);



            int iRINFLEXOP = dr.GetOrdinal(this.RINFLEXOP);
            if (!dr.IsDBNull(iRINFLEXOP)) entity.RINFLEXOP = dr.GetDecimal(iRINFLEXOP);
            int iRCOL2 = dr.GetOrdinal(this.RCOL2);
            if (!dr.IsDBNull(iRCOL2)) entity.RCOL2 = dr.GetDecimal(iRCOL2);
            int iRCOL1 = dr.GetOrdinal(this.RCOL1);
            if (!dr.IsDBNull(iRCOL1)) entity.RCOL1 = dr.GetDecimal(iRCOL1);
            int iRLSCIO = dr.GetOrdinal(this.RLSCIO);
            if (!dr.IsDBNull(iRLSCIO)) entity.RLSCIO = dr.GetDecimal(iRLSCIO);
            int iREPR11 = dr.GetOrdinal(this.REPR11);
            if (!dr.IsDBNull(iREPR11)) entity.REPR11 = dr.GetDecimal(iREPR11);
            int iRER10 = dr.GetOrdinal(this.RER10);
            if (!dr.IsDBNull(iRER10)) entity.RER10 = dr.GetDecimal(iRER10);
            int iRPARM3 = dr.GetOrdinal(this.RPARM3);
            if (!dr.IsDBNull(iRPARM3)) entity.RPARM3 = dr.GetDecimal(iRPARM3);
            int iREPRM3 = dr.GetOrdinal(this.REPRM3);
            if (!dr.IsDBNull(iREPRM3)) entity.REPRM3 = dr.GetDecimal(iREPRM3);
            int iRPARM2 = dr.GetOrdinal(this.RPARM2);
            if (!dr.IsDBNull(iRPARM2)) entity.RPARM2 = dr.GetDecimal(iRPARM2);
            int iREPRM2 = dr.GetOrdinal(this.REPRM2);
            if (!dr.IsDBNull(iREPRM2)) entity.REPRM2 = dr.GetDecimal(iREPRM2);
            int iRPARM1 = dr.GetOrdinal(this.RPARM1);
            if (!dr.IsDBNull(iRPARM1)) entity.RPARM1 = dr.GetDecimal(iRPARM1);
            int iREPRM1 = dr.GetOrdinal(this.REPRM1);
            if (!dr.IsDBNull(iREPRM1)) entity.REPRM1 = dr.GetDecimal(iREPRM1);
            int iRCODI = dr.GetOrdinal(this.RCODI);
            if (!dr.IsDBNull(iRCODI)) entity.RCODI = dr.GetDecimal(iRCODI);

            int iRDEMM1 = dr.GetOrdinal(this.RDEMM1);
            if (!dr.IsDBNull(iRDEMM1)) entity.RDEMM1 = dr.GetDecimal(iRDEMM1);
            int iRDEMM2 = dr.GetOrdinal(this.RDEMM2);
            if (!dr.IsDBNull(iRDEMM2)) entity.RDEMM2 = dr.GetDecimal(iRDEMM2);
            int iRDEMM3 = dr.GetOrdinal(this.RDEMM3);
            if (!dr.IsDBNull(iRDEMM3)) entity.RDEMM3 = dr.GetDecimal(iRDEMM3);
            int iRINFLEX = dr.GetOrdinal(this.RINFLEX);
            if (!dr.IsDBNull(iRINFLEX)) entity.RINFLEX = dr.GetDecimal(iRINFLEX);

            return entity;
        }
        public GmmDatInsumoDTO CreateListaDatosCalculo(IDataReader dr)
        {
            GmmDatInsumoDTO entity = new GmmDatInsumoDTO();

            int iDcalCodi = dr.GetOrdinal(this.Dcalcodi);
            if (!dr.IsDBNull(iDcalCodi)) entity.DCALCODI = dr.GetInt32(iDcalCodi);

            int iPeriCodi = dr.GetOrdinal(this.Pericodi);
            if (!dr.IsDBNull(iPeriCodi)) entity.PERICODI = dr.GetInt32(iPeriCodi);

            int iCalvalor = dr.GetOrdinal(this.Dcalvalor);
            if (!dr.IsDBNull(iCalvalor)) entity.DCALVALOR = dr.GetDecimal(iCalvalor);


            return entity;
        }
    }
}