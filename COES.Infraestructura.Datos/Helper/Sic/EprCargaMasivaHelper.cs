using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla EPR_CARGA_MASIVA
    /// </summary>
    public class EprCargaMasivaHelper : HelperBase
    {
        public EprCargaMasivaHelper(): base(Consultas.EprCargaMasivaSql)
        {
        }

        public void ObtenerMetaDatos(IDataReader dr, ref Dictionary<int, MetadataDTO> metadatos)
        {
            for (int i = 0; i < dr.FieldCount; i++)
            {
                metadatos.Add(i, new MetadataDTO
                {
                    FieldName = dr.GetName(i),
                    TipoDato = dr.GetFieldType(i)
                });
            }
        }
        public EprCargaMasivaDTO Create(IDataReader dr)
        {
            EprCargaMasivaDTO entity = new EprCargaMasivaDTO();

            int iEpcamacodi = dr.GetOrdinal(this.Epcamacodi);
            if (!dr.IsDBNull(iEpcamacodi)) entity.Epcamacodi = Convert.ToInt32(dr.GetValue(iEpcamacodi));

            int iEpcamatipuso = dr.GetOrdinal(this.Epcamatipuso);
            if (!dr.IsDBNull(iEpcamatipuso)) entity.Epcamatipuso = Convert.ToInt32(dr.GetValue(iEpcamatipuso));

            int iEpcamafeccarga = dr.GetOrdinal(this.Epcamafeccarga);
            if (!dr.IsDBNull(iEpcamafeccarga)) entity.Epcamafeccarga = Convert.ToDateTime(dr.GetValue(iEpcamafeccarga));

            int iEpcamausucreacion = dr.GetOrdinal(this.Epcamausucreacion);
            if (!dr.IsDBNull(iEpcamausucreacion)) entity.Epcamausucreacion = dr.GetValue(iEpcamausucreacion).ToString();

            int iEpcamafeccreacion = dr.GetOrdinal(this.Epcamafeccreacion);
            if (!dr.IsDBNull(iEpcamafeccreacion)) entity.Epcamafeccreacion = Convert.ToDateTime(dr.GetValue(iEpcamafeccarga));

            int iEpcamausumodificacion = dr.GetOrdinal(this.Epcamausumodificacion);
            if (!dr.IsDBNull(iEpcamausumodificacion)) entity.Epcamausumodificacion = dr.GetValue(iEpcamausumodificacion).ToString();

            int iEpcamafecmodificacion = dr.GetOrdinal(this.Epcamafecmodificacion);
            if (!dr.IsDBNull(iEpcamafecmodificacion)) entity.Epcamafecmodificacion = Convert.ToDateTime(dr.GetValue(iEpcamafecmodificacion));

            return entity;
        }

        //GESPROTEC - 20241029
        #region GESPROTECT
        bool validaColumna(IDataReader dr, string columnName)
        {
            for (int i = 0; i < dr.FieldCount; i++)
            {
                if (dr.GetName(i).Equals(columnName, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }
            return false;
        }
        #endregion

        #region Mapeo de Campos
        public string Epcamacodi = "EPCAMACODI";
        public string Epcamatipuso = "EPCAMATIPUSO";
        public string Epcamatipusonombre = "EQCATDDESCRIPCION";
        public string Epcamafeccarga = "EPCAMAFECCARGA";
        public string Epcamausucreacion = "EPCAMAUSUCREACION";
        public string Epcamafeccreacion = "EPCAMAFECCREACION";

        public string Epcamausumodificacion = "EPCAMAUSUMODIFICACION";
        public string Epcamafecmodificacion = "EPCAMAFECMODIFICACION";

        public string Epcamatotalregistro = "EPCAMATOTALREGISTRO";
        #endregion

        #region Campos Paginacion
        public static string MaxRowToFetch = "MAXROWTOFETCH";
        public static string MinRowToFetch = "MINROWTOFETCH";
        #endregion

        #region ListEquipoLineaReporte

        public string Codigo_Id = "Codigo_Id";
        public string Codigo = "Codigo";
        public string Ubicacion = "Ubicacion";
        public string Empresa = "Empresa";
        public string Area = "Area";
        public string Longitud_Reporte = "Longitud";
        public string Longitud_Coment = "Longitud_Coment";
        public string Tension_Reporte = "Tension";
        public string Tension_Coment = "Tension_Coment";
        public string Capacidad_A = "Capacidad_A";
        public string Capacidad_A_Coment = "Capacidad_A_Coment";
        public string Capacidad_Mva = "Capacidad_Mva";
        public string Capacidad_Mva_Coment = "Capacidad_Mva_Coment";
        public string Id_Celda_1 = "Id_Celda_1";
        public string Nombre_Celda_1 = "Nombre_Celda_1";
        public string Ubicacion_Celda_1 = "Ubicacion_Celda_1";
        public string Posicion_Nucleo_Tc_Celda_1 = "Posicion_Nucleo_Tc_Celda_1";
        public string Pick_Up_Celda_1 = "Pick_Up_Celda_1";

        public string Id_Celda_2 = "Id_Celda_2";
        public string Nombre_Celda_2 = "Nombre_Celda_2";
        public string Ubicacion_Celda_2 = "Ubicacion_Celda_2";
        public string Posicion_Nucleo_Tc_Celda_2 = "Posicion_Nucleo_Tc_Celda_2";
        public string Pick_Up_Celda_2 = "Pick_Up_Celda_2";

        public string Id_Banco_Condensador = "Id_Banco_Condensador";
        public string Nombre_Banco_Condensador = "Nombre_Banco_Condensador";
        public string Ubicacion_Banco_Condensador = "Ubicacion_Banco_Condensador";
        public string Capacidad_A_Banco = "Capacidad_A_Banco";
        public string Capacidad_Mvar_Banco = "Capacidad_Mvar_Banco";
        public string Capac_Trans_Cond_1_Porcen = "Capac_Trans_Cond_1_Porcen";
        public string Capac_Trans_Cond_1_Porcen_Coment = "Capac_Trans_Cond_1_Porcen_Coment";
        public string Capac_Trans_Cond_1_Min = "Capac_Trans_Cond_1_Min";
        public string Capac_Trans_Cond_1_Min_Coment = "Capac_Trans_Cond_1_Min_Coment";
        public string Capac_Trans_Corr_1_A = "Capac_Trans_Corr_1_A";
        public string Capac_Trans_Corr_1_A_Coment = "Capac_Trans_Corr_1_A_Coment";
        public string Capac_Trans_Cond_2_Porcen = "Capac_Trans_Cond_2_Porcen";
        public string Capac_Trans_Cond_2_Porcen_Coment = "Capac_Trans_Cond_2_Porcen_Coment";
        public string Capac_Trans_Cond_2_Min = "Capac_Trans_Cond_2_Min";
        public string Capac_Trans_Cond_2_Min_Coment = "Capac_Trans_Cond_2_Min_Coment";
        public string Capac_Trans_Corr_2_A = "Capac_Trans_Corr_2_A";
        public string Capac_Trans_Corr_2_A_Coment = "Capac_Trans_Corr_2_A_Coment";
        public string Capacidad_Transmision_A = "Capacidad_Transmision_A";
        public string Capacidad_Transmision_A_Coment = "Capacidad_Transmision_A_Coment";
        public string Capacidad_Transmision_Mva = "Capacidad_Transmision_Mva";
        public string Capacidad_Transmision_Mva_Coment = "Capacidad_Transmision_Mva_Coment";
        public string Limite_Seg_Coes = "Limite_Seg_Coes";
        public string Limite_Seg_Coes_Coment = "Limite_Seg_Coes_Coment";
        public string Factor_Limitante_Calc = "Factor_Limitante_Calc";
        public string Factor_Limitante_Calc_Coment = "Factor_Limitante_Calc_Coment";
        public string Factor_Limitante_Final = "Factor_Limitante_Final";
        public string Factor_Limitante_Final_Coment = "Factor_Limitante_Final_Coment";
        public string Observaciones_Reporte = "Observaciones";
        public string Usuario_Auditoria = "Usuario_Auditoria";
        public string Fecha_Modificacion = "Fecha_Modificacion";
        public string Motivo = "Motivo";

        public string Capacidad_Mvar = "Capacidad_Mvar";
        public string Capacidad_Mvar_Coment = "Capacidad_Mvar_Coment";
        public string Capacidad_Transmision_Mvar = "Capacidad_Transmision_Mvar";
        public string Capacidad_Transmision_Mvar_Coment = "Capacidad_Transmision_Mvar_Coment";

        public string Capacidad_A_Banco_Coment = "Capacidad_A_Banco_Coment";
        public string Capacidad_Mvar_Banco_Coment = "Capacidad_Mvar_Banco_Coment";


        #endregion

        #region ListEquiposCeldaAcoplamiento

        public string Nombre = "Nombre";
        public string Posicion_Nucleo_Tc = "Posicion_Nucleo_Tc";
        public string Pick_Up = "Pick_Up";
        public string Codigo_Id_Interruptor = "Codigo_Id_Interruptor";
        public string Nombre_Interruptor = "Nombre_Interruptor";
        public string Ubicacion_Interruptor = "Ubicacion_Interruptor";
        public string Empresa_Interruptor = "Empresa_Interruptor";
        public string Tension_Interruptor = "Tension_Interruptor";
        public string Capacidad_A_Interruptor = "Capacidad_A_Interruptor";
        public string Capacidad_A_Interruptor_Coment = "Capacidad_A_Interruptor_Coment";
        public string Capacidad_Mva_Interruptor = "Capacidad_Mva_Interruptor";
        public string Capacidad_Mva_Interruptor_Coment = "Capacidad_Mva_Interruptor_Coment";

        #endregion

        #region ListTransformadoresReporte

        public string Famcodi = "FAMCODI";
        public string D1_Id_Celda = "D1_Id_Celda";
        public string D1_Codigo_Celda = "D1_Codigo_Celda";
        public string D1_Ubicacion_Celda = "D1_Ubicacion_Celda";
        public string D1_Tension = "D1_Tension";
        public string D1_Tension_Coment = "D1_Tension_Coment";
        public string D1_Capacidad_Onan_Mva = "D1_Capacidad_Onan_Mva";
        public string D1_Capacidad_Onan_Mva_Coment = "D1_Capacidad_Onan_Mva_Coment";
        public string D1_Capacidad_Onaf_Mva = "D1_Capacidad_Onaf_Mva";
        public string D1_Capacidad_Onaf_Mva_Coment = "D1_Capacidad_Onaf_Mva_Coment";
        public string D1_Capacidad_Mva = "D1_Capacidad_Mva";
        public string D1_Capacidad_Mva_Coment = "D1_Capacidad_Mva_Coment";
        public string D1_Capacidad_A = "D1_Capacidad_A";
        public string D1_Capacidad_A_Coment = "D1_Capacidad_A_Coment";
        public string D1_Posicion_Nucleo_Tc = "D1_Posicion_Nucleo_Tc";
        public string D1_Pick_Up = "D1_Pick_Up";
        public string D1_Factor_Limitante_Calc = "D1_Factor_Limitante_Calc";
        public string D1_Factor_Limitante_Calc_Coment = "D1_Factor_Limitante_Calc_Coment";
        public string D1_Factor_Limitante_Final = "D1_Factor_Limitante_Final";
        public string D1_Factor_Limitante_Final_Coment = "D1_Factor_Limitante_Final_Coment";

        public string D2_Id_Celda = "D2_Id_Celda";
        public string D2_Codigo_Celda = "D2_Codigo_Celda";
        public string D2_Ubicacion_Celda = "D2_Ubicacion_Celda";
        public string D2_Tension = "D2_Tension";
        public string D2_Tension_Coment = "D2_Tension_Coment";
        public string D2_Capacidad_Onan_Mva = "D2_Capacidad_Onan_Mva";
        public string D2_Capacidad_Onan_Mva_Coment = "D2_Capacidad_Onan_Mva_Coment";
        public string D2_Capacidad_Onaf_Mva = "D2_Capacidad_Onaf_Mva";
        public string D2_Capacidad_Onaf_Mva_Coment = "D2_Capacidad_Onaf_Mva_Coment";
        public string D2_Capacidad_Mva = "D2_Capacidad_Mva";
        public string D2_Capacidad_Mva_Coment = "D2_Capacidad_Mva_Coment";
        public string D2_Capacidad_A = "D2_Capacidad_A";
        public string D2_Capacidad_A_Coment = "D2_Capacidad_A_Coment";
        public string D2_Posicion_Nucleo_Tc = "D2_Posicion_Nucleo_Tc";
        public string D2_Pick_Up = "D2_Pick_Up";
        public string D2_Factor_Limitante_Calc = "D2_Factor_Limitante_Calc";
        public string D2_Factor_Limitante_Calc_Coment = "D2_Factor_Limitante_Calc_Coment";
        public string D2_Factor_Limitante_Final = "D2_Factor_Limitante_Final";
        public string D2_Factor_Limitante_Final_Coment = "D2_Factor_Limitante_Final_Coment";

        public string D3_Id_Celda = "D3_Id_Celda";
        public string D3_Codigo_Celda = "D3_Codigo_Celda";
        public string D3_Ubicacion_Celda = "D3_Ubicacion_Celda";
        public string D3_Tension = "D3_Tension";
        public string D3_Tension_Coment = "D3_Tension_Coment";
        public string D3_Capacidad_Onan_Mva = "D3_Capacidad_Onan_Mva";
        public string D3_Capacidad_Onan_Mva_Coment = "D3_Capacidad_Onan_Mva_Coment";
        public string D3_Capacidad_Onaf_Mva = "D3_Capacidad_Onaf_Mva";
        public string D3_Capacidad_Onaf_Mva_Coment = "D3_Capacidad_Onaf_Mva_Coment";
        public string D3_Capacidad_Mva = "D3_Capacidad_Mva";
        public string D3_Capacidad_Mva_Coment = "D3_Capacidad_Mva_Coment";
        public string D3_Capacidad_A = "D3_Capacidad_A";
        public string D3_Capacidad_A_Coment = "D3_Capacidad_A_Coment";
        public string D3_Posicion_Nucleo_Tc = "D3_Posicion_Nucleo_Tc";
        public string D3_Pick_Up = "D3_Pick_Up";
        public string D3_Factor_Limitante_Calc = "D3_Factor_Limitante_Calc";
        public string D3_Factor_Limitante_Calc_Coment = "D3_Factor_Limitante_Calc_Coment";
        public string D3_Factor_Limitante_Final = "D3_Factor_Limitante_Final";
        public string D3_Factor_Limitante_Final_Coment = "D3_Factor_Limitante_Final_Coment";

        public string D4_Id_Celda = "D4_Id_Celda";
        public string D4_Codigo_Celda = "D4_Codigo_Celda";
        public string D4_Ubicacion_Celda = "D4_Ubicacion_Celda";
        public string D4_Tension = "D4_Tension";
        public string D4_Tension_Coment = "D4_Tension_Coment";
        public string D4_Capacidad_Onan_Mva = "D4_Capacidad_Onan_Mva";
        public string D4_Capacidad_Onan_Mva_Coment = "D4_Capacidad_Onan_Mva_Coment";
        public string D4_Capacidad_Onaf_Mva = "D4_Capacidad_Onaf_Mva";
        public string D4_Capacidad_Onaf_Mva_Coment = "D4_Capacidad_Onaf_Mva_Coment";
        public string D4_Capacidad_Mva = "D4_Capacidad_Mva";
        public string D4_Capacidad_Mva_Coment = "D4_Capacidad_Mva_Coment";
        public string D4_Capacidad_A = "D4_Capacidad_A";
        public string D4_Capacidad_A_Coment = "D4_Capacidad_A_Coment";
        public string D4_Posicion_Nucleo_Tc = "D4_Posicion_Nucleo_Tc";
        public string D4_Pick_Up = "D4_Pick_Up";
        public string D4_Factor_Limitante_Calc = "D4_Factor_Limitante_Calc";
        public string D4_Factor_Limitante_Calc_Coment = "D4_Factor_Limitante_Calc_Coment";
        public string D4_Factor_Limitante_Final = "D4_Factor_Limitante_Final";
        public string D4_Factor_Limitante_Final_Coment = "D4_Factor_Limitante_Final_Coment";


        #endregion

        public string SqlListFiltroCargaMasiva
        {
            get { return GetSqlXml("ListFiltroCargaMasiva"); }
        }

        public string SqlValidarProteccionesUsoGeneral
        {
            get { return GetSqlXml("ValidarProteccionesUsoGeneral"); }
        }

        public string SqlSaveProteccionesUsoGeneral
        {
            get { return GetSqlXml("SaveProteccionesUsoGeneral"); }
        }

        public string SqlValidarProteccionesMandoSincronizado
        {
            get { return GetSqlXml("ValidarProteccionesMandoSincronizado"); }
        }
        public string SqlSaveProteccionesMandoSincronizado
        {
            get { return GetSqlXml("SaveProteccionesMandoSincronizado"); }
        }

        public string SqlValidarProteccionesTorsional
        {
            get { return GetSqlXml("ValidarProteccionesTorsional"); }
        }

        public string SqlSaveProteccionesTorsional
        {
            get { return GetSqlXml("SaveProteccionesTorsional"); }
        }

        public string SqlValidarProteccionesPmu
        {
            get { return GetSqlXml("ValidarProteccionesPmu"); }
        }

        public string SqlSaveProteccionesPmu
        {
            get { return GetSqlXml("SaveProteccionesPmu"); }
        }

        #region GESPROTECT - 20250206

        public string SqlValidarProteccionesLinea
        {
            get { return GetSqlXml("ValidarProteccionesLinea"); }
        }

        public string SqlSaveProteccionesLinea
        {
            get { return GetSqlXml("SaveProteccionesLinea"); }
        }

        public string SqlValidarProteccionesReactor
        {
            get { return GetSqlXml("ValidarProteccionesReactor"); }
        }

        public string SqlSaveProteccionesReactor
        {
            get { return GetSqlXml("SaveProteccionesReactor"); }
        }

        public string SqlValidarProteccionesCeldaAcoplamiento
        {
            get { return GetSqlXml("ValidarProteccionesCeldaAcoplamiento"); }
        }

        public string SqlSaveProteccionesCeldaAcoplamiento
        {
            get { return GetSqlXml("SaveProteccionesCeldaAcoplamiento"); }
        }

        public string SqlValidarProteccionesTransformador
        {
            get { return GetSqlXml("ValidarProteccionesTransformador"); }
        }

        public string SqlSaveProteccionesTransformador
        {
            get { return GetSqlXml("SaveProteccionesTransformador"); }
        }

        public string ListLineaEvaluacionReporte
        {
            get { return GetSqlXml("ListLineaEvaluacionReporte"); }
        }

        public string ListReactorEvaluacionReporte
        {
            get { return GetSqlXml("ListReactorEvaluacionReporte"); }
        }

        public string ListCeldasAcoplamientoReporte
        {
            get { return GetSqlXml("ListCeldasAcoplamientoReporte"); }
        }

        public string ListTransformadoresReporte
        {
            get { return GetSqlXml("ListTransformadoresReporte"); }
        }

        #endregion
    }
}
