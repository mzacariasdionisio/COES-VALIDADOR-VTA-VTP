using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.DTO.Sic
{
    public class EprEquipoReporteDTO
    {
        public int Codigo_Id { get; set; }
        public string Codigo { get; set; }
        public string Ubicacion { get; set; }
        public string Empresa { get; set; }
        public string Area { get; set; }
        public string Longitud { get; set; }
        public string Longitud_Coment { get; set; }
        public string Tension { get; set; }
        public string Tension_Coment { get; set; }
        public string Capacidad_A { get; set; }
        public string Capacidad_A_Coment { get; set; }
        public string Capacidad_Mva { get; set; }
        public string Capacidad_Mva_Coment { get; set; }
        public string Id_Celda_1 { get; set; }
        public string Nombre_Celda_1 { get; set; }
        public string Ubicacion_Celda_1 { get; set; }
        public string Posicion_Nucleo_Tc_Celda_1 { get; set; }
        public string Pick_Up_Celda_1 { get; set; }

        public string Id_Celda_2 { get; set; }
        public string Nombre_Celda_2 { get; set; }
        public string Ubicacion_Celda_2 { get; set; }
        public string Posicion_Nucleo_Tc_Celda_2 { get; set; }
        public string Pick_Up_Celda_2 { get; set; }

        public string Id_Banco_Condensador { get; set; }
        public string Nombre_Banco_Condensador { get; set; }
        public string Ubicacion_Banco_Condensador { get; set; }
        public string Capacidad_A_Banco { get; set; }
        public string Capacidad_Mvar_Banco { get; set; }
        public string Capac_Trans_Cond_1_Porcen { get; set; }
        public string Capac_Trans_Cond_1_Porcen_Coment { get; set; }
        public string Capac_Trans_Cond_1_Min { get; set; }
        public string Capac_Trans_Cond_1_Min_Coment { get; set; }
        public string Capac_Trans_Corr_1_A { get; set; }
        public string Capac_Trans_Corr_1_A_Coment { get; set; }
        public string Capac_Trans_Cond_2_Porcen { get; set; }
        public string Capac_Trans_Cond_2_Porcen_Coment { get; set; }
        public string Capac_Trans_Cond_2_Min { get; set; }
        public string Capac_Trans_Cond_2_Min_Coment { get; set; }
        public string Capac_Trans_Corr_2_A { get; set; }
        public string Capac_Trans_Corr_2_A_Coment { get; set; }
        public string Capacidad_Transmision_A { get; set; }
        public string Capacidad_Transmision_A_Coment { get; set; }
        public string Capacidad_Transmision_Mva { get; set; }
        public string Capacidad_Transmision_Mva_Coment { get; set; }
        public string Limite_Seg_Coes { get; set; }
        public string Limite_Seg_Coes_Coment { get; set; }
        public string Factor_Limitante_Calc { get; set; }
        public string Factor_Limitante_Calc_Coment { get; set; }
        public string Factor_Limitante_Final { get; set; }
        public string Factor_Limitante_Final_Coment { get; set; }
        public string Observaciones { get; set; }
        public string Usuario_Auditoria { get; set; }
        public string Fecha_Modificacion { get; set; }

        public string Motivo { get; set; }
        public string CapacidadABancoComent { get; set; }
        public string CapacidadMvarBancoComent { get; set; }
    }

    public class EprEquipoReactorReporteDTO
    {
        public string Codigo_Id { get; set; }
        public string Codigo { get; set; }
        public string Ubicacion { get; set; }
        public string Empresa { get; set; }
        public string Area { get; set; }       
        public string Tension { get; set; }
        public string Tension_Coment { get; set; }
        public string Capacidad_A { get; set; }
        public string Capacidad_A_Coment { get; set; }
        public string Capacidad_Mvar { get; set; }
        public string Capacidad_Mvar_Coment { get; set; }
        public string Id_Celda_1 { get; set; }
        public string Nombre_Celda_1 { get; set; }
        public string Ubicacion_Celda_1 { get; set; }
        public string Posicion_Nucleo_Tc_Celda_1 { get; set; }
        public string Pick_Up_Celda_1 { get; set; }

        public string Id_Celda_2 { get; set; }
        public string Nombre_Celda_2 { get; set; }
        public string Ubicacion_Celda_2 { get; set; }
        public string Posicion_Nucleo_Tc_Celda_2 { get; set; }
        public string Pick_Up_Celda_2 { get; set; }
               
        public string Capacidad_Transmision_A { get; set; }
        public string Capacidad_Transmision_A_Coment { get; set; }
        public string Capacidad_Transmision_Mvar { get; set; }
        public string Capacidad_Transmision_Mvar_Coment { get; set; }
        public string Limite_Seg_Coes { get; set; }
        public string Limite_Seg_Coes_Coment { get; set; }
        public string Factor_Limitante_Calc { get; set; }
        public string Factor_Limitante_Calc_Coment { get; set; }
        public string Factor_Limitante_Final { get; set; }
        public string Factor_Limitante_Final_Coment { get; set; }
        public string Observaciones { get; set; }
        public string Usuario_Auditoria { get; set; }
        public string Fecha_Modificacion { get; set; }

        public string Motivo { get; set; }
    }

    public class EprEquipoCeldaAcoplamientoReporteDTO
    {
        public string Codigo_Id { get; set; }
        public string Nombre { get; set; }
        public string Ubicacion { get; set; }    
        public string Area { get; set; }
        public string Posicion_Nucleo_Tc { get; set; }
        public string Pick_Up { get; set; }
        public string Codigo_Id_Interruptor { get; set; }
        public string Nombre_Interruptor { get; set; }
        public string Ubicacion_Interruptor { get; set; }
        public string Empresa_Interruptor { get; set; }
        public string Tension_Interruptor { get; set; }
        public string Capacidad_A_Interruptor { get; set; }
        public string Capacidad_A_Interruptor_Coment { get; set; }
        public string Capacidad_Mva_Interruptor { get; set; }
        public string Capacidad_Mva_Interruptor_Coment { get; set; }


        public string Capacidad_Transmision_A { get; set; }
        public string Capacidad_Transmision_A_Coment { get; set; }
        public string Capacidad_Transmision_Mva { get; set; }
        public string Capacidad_Transmision_Mva_Coment { get; set; }
       
        public string Factor_Limitante_Calc { get; set; }
        public string Factor_Limitante_Calc_Coment { get; set; }
        public string Factor_Limitante_Final { get; set; }
        public string Factor_Limitante_Final_Coment { get; set; }
        public string Observaciones { get; set; }
        public string Usuario_Auditoria { get; set; }
        public string Fecha_Modificacion { get; set; }

        public string Motivo { get; set; }
    }

    public class EprEquipoTransformadoresReporteDTO
    {
        public string Codigo_Id { get; set; }
        public string Codigo { get; set; }
        public string Ubicacion { get; set; }
        public string Empresa { get; set; }
        public string Area { get; set; }
        public int Famcodi { get; set; }
        public string D1_Id_Celda { get; set; }
        public string D1_Codigo_Celda { get; set; }
        public string D1_Ubicacion_Celda { get; set; }
        public string D1_Tension { get; set; }
        public string D1_Tension_Coment { get; set; }
        public string D1_Capacidad_Onan_Mva { get; set; }
        public string D1_Capacidad_Onan_Mva_Coment { get; set; }
        public string D1_Capacidad_Onaf_Mva { get; set; }
        public string D1_Capacidad_Onaf_Mva_Coment { get; set; }
        public string D1_Capacidad_Mva { get; set; }
        public string D1_Capacidad_Mva_Coment { get; set; }
        public string D1_Capacidad_A { get; set; }
        public string D1_Capacidad_A_Coment { get; set; }
        public string D1_Posicion_Nucleo_Tc { get; set; }
        public string D1_Pick_Up { get; set; }
        public string D1_Factor_Limitante_Calc { get; set; }
        public string D1_Factor_Limitante_Calc_Coment { get; set; }
        public string D1_Factor_Limitante_Final { get; set; }
        public string D1_Factor_Limitante_Final_Coment { get; set; }

        public string D2_Id_Celda { get; set; }
        public string D2_Codigo_Celda { get; set; }
        public string D2_Ubicacion_Celda { get; set; }
        public string D2_Tension { get; set; }
        public string D2_Tension_Coment { get; set; }
        public string D2_Capacidad_Onan_Mva { get; set; }
        public string D2_Capacidad_Onan_Mva_Coment { get; set; }
        public string D2_Capacidad_Onaf_Mva { get; set; }
        public string D2_Capacidad_Onaf_Mva_Coment { get; set; }
        public string D2_Capacidad_Mva { get; set; }
        public string D2_Capacidad_Mva_Coment { get; set; }
        public string D2_Capacidad_A { get; set; }
        public string D2_Capacidad_A_Coment { get; set; }
        public string D2_Posicion_Nucleo_Tc { get; set; }
        public string D2_Pick_Up { get; set; }
        public string D2_Factor_Limitante_Calc { get; set; }
        public string D2_Factor_Limitante_Calc_Coment { get; set; }
        public string D2_Factor_Limitante_Final { get; set; }
        public string D2_Factor_Limitante_Final_Coment { get; set; }

        public string D3_Id_Celda { get; set; }
        public string D3_Codigo_Celda { get; set; }
        public string D3_Ubicacion_Celda { get; set; }
        public string D3_Tension { get; set; }
        public string D3_Tension_Coment { get; set; }
        public string D3_Capacidad_Onan_Mva { get; set; }
        public string D3_Capacidad_Onan_Mva_Coment { get; set; }
        public string D3_Capacidad_Onaf_Mva { get; set; }
        public string D3_Capacidad_Onaf_Mva_Coment { get; set; }
        public string D3_Capacidad_Mva { get; set; }
        public string D3_Capacidad_Mva_Coment { get; set; }
        public string D3_Capacidad_A { get; set; }
        public string D3_Capacidad_A_Coment { get; set; }
        public string D3_Posicion_Nucleo_Tc { get; set; }
        public string D3_Pick_Up { get; set; }
        public string D3_Factor_Limitante_Calc { get; set; }
        public string D3_Factor_Limitante_Calc_Coment { get; set; }
        public string D3_Factor_Limitante_Final { get; set; }
        public string D3_Factor_Limitante_Final_Coment { get; set; }

        public string D4_Id_Celda { get; set; }
        public string D4_Codigo_Celda { get; set; }
        public string D4_Ubicacion_Celda { get; set; }
        public string D4_Tension { get; set; }
        public string D4_Tension_Coment { get; set; }
        public string D4_Capacidad_Onan_Mva { get; set; }
        public string D4_Capacidad_Onan_Mva_Coment { get; set; }
        public string D4_Capacidad_Onaf_Mva { get; set; }
        public string D4_Capacidad_Onaf_Mva_Coment { get; set; }
        public string D4_Capacidad_Mva { get; set; }
        public string D4_Capacidad_Mva_Coment { get; set; }
        public string D4_Capacidad_A { get; set; }
        public string D4_Capacidad_A_Coment { get; set; }
        public string D4_Posicion_Nucleo_Tc { get; set; }
        public string D4_Pick_Up { get; set; }
        public string D4_Factor_Limitante_Calc { get; set; }
        public string D4_Factor_Limitante_Calc_Coment { get; set; }
        public string D4_Factor_Limitante_Final { get; set; }
        public string D4_Factor_Limitante_Final_Coment { get; set; }

        public string Observaciones { get; set; }
        public string Usuario_Auditoria { get; set; }
        public string Fecha_Modificacion { get; set; }
        public string Motivo { get; set; }
    }
}
