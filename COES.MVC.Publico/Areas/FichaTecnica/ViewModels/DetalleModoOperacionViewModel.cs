using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COES.MVC.Publico.Areas.FichaTecnica.ViewModels
{
    public class DetalleModoOperacionViewModel
    {
        public string NombreCentral;
        public string NombreEmpresa;

        public string CodCentral;
        public string CodModo;
        public string Pe;
        public string Pmin;
        public string Pmax;
        public string V_tomacarga;
        public string V_TC_frio1;
        public string V_TC_frio2;
        public string V_TC_intca;
        public string V_TC_calie;
        public string V_descarga;
        public string T_sinc;
        public string T_sincronizacionFrio1;
        public string T_sinc_F2;
        public string T_sincronizaciónintermedio;
        public string T_sincronizacióncaliente;
        public string T_PC_Sinc;
        public string T_PC_F1;
        public string T_PC_F2;
        public string T_CargaIntermedio;
        public string T_PC_Cal;
        public string T_SFSP;
        public string T_PC_pm;
        public string T_ArrNegr;
        public string T_fuera_sinc;
        public string T_sinc_par;
        public string T_min_Arr;
        public string T_min_Arr_eme;
        public string MaxPotMin;
        public string Tmin_op;
        public string Ene_sinc;
        public string Ene_sinc_F1;
        public string Ene_sinc_F2;
        public string Ene_sinc_int;
        public string Ene_sinc_cal;
        public string Ene_PC_sinc;
        public string TipComb;
        public string HHV;
        public string LHV;
        public string ge;
        public string TempComb;
        public string a;
        public string b;
        public string c;
        public string CombATM;
        public string Comb_arr_sinc;
        public string Comb_arr_sinc_F1;
        public string Comb_arr_sinc_F2;
        public string Comb_arr_sinc_int;
        public string Comb_arr_sinc_cal;
        public string Comb_sinc_PC;
        public string Comb_sinc_PC_F1;
        public string Comb_sinc_PC_F2;
        public string Comb_sinc_PC_int;
        public string Comb_sinc_PC_cal;
        public string CombPRD;
        public string Comb_PC_sinc;
        public string Comb_sinc_par;
        public string EficTerm;
        public string EficBTUKWh;
        public string CVC;
        public string PrecioCComb;
        public string CostoTransCComb;
        public string CostoTratMecCComb;
        public string CostoTratQuiCComb;
        public string CostoFinCComb;
        public string CostoTotalComb;
        public string CVNC;
        public string CVONC;
        public string CVM;
        public string CMarr;
        public string SSAA;
        public string FDP;
        public string Densidad;
        //Unidades
        public string HHV_uni;
        public string LHV_uni;
        public string CombATM_uni;
        public string Comb_arr_sinc_uni;
        public string Comb_arr_sinc_F1_uni;
        public string Comb_arr_sinc_int_uni;
        public string Comb_arr_sinc_cal_uni;
        public string Comb_sinc_PC_uni;
        public string Comb_sinc_PC_F1_uni;
        public string Comb_sinc_PC_int_uni;
        public string Comb_sinc_PC_cal_uni;
        public string CombPRD_uni;
        public string Comb_PC_sinc_uni;
        public string Comb_sinc_par_uni;
        public string EficTerm_uni;
        public string EficBTUKWh_uni;
        public string PrecioCComb_uni;
        public string CostoTratMecCComb_uni;
        public string CostoTratQuiCComb_uni;
        public string CostoTransCComb_uni;
        public string Consumo_uni;
        public string Densidad_uni;
        public string CostoTotalComb_uni;
        //Abreviatura
        public string HHV_abrev;
        public string LHV_abrev;

        public bool bEsCasoIlo;
        public string sRowSpanVelocidad;
        public string sRowSpanTiempo;
        public string sRowSpanEnergia;
        public string sRowSpanCombustible;
        public string sRowSpanArranque;
        public string sRowSpanCCBEF;

        public string sNumeral1;
        public string sNumeral2;

        //Nuevos Campos Mejoras FFTT
        public string C_Arr_Par_f1;
        public string C_Arr_Par_f2;
        public string C_Arr_Par_int;
        public string C_Arr_Par_cal;
        public string Ccbef;
        public string Ccbef_f1;
        public string Ccbef_f2;
        public string Ccbef_int;
        public string Ccbef_cal;
        
        public string CombPE;
        public string Pot_punto1;
        public string Comb_punto1;
        public string Pot_punto2;
        public string Comb_punto2;
        public string Pot_punto3;
        public string Comb_punto3;
        public string Pot_punto4;
        public string Comb_punto4;

        public string C_Arr_Par;

        //Campos de textos a mostrar
        public string TextoCostoTransporte;
        public string TextoCostoTratamientoMecanico;
        public string TextoCostoTratamientoQuimico;

        #region Caso Modo Operacion ILO2CARB
        public bool EsIlo2;
        public string UnidadIlo2;
        public string CombustibleIlo2;
        #endregion
    }
}