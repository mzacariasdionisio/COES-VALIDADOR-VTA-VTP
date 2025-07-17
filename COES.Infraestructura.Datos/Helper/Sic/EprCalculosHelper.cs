using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla 
    /// </summary>
    public class EprCalculosHelper : HelperBase
    {
        public EprCalculosHelper(): base(Consultas.EprCalculosSql)
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



        #region Mapeo de Campos Equipo
        public string IdLinea = "ID_LINEA";
        public string IdArea = "ID_AREA";
        public string CapacidadA = "CAPACIDAD_A";
        public string CapacidadMva = "CAPACIDAD_MVA";
        public string IdCelda = "ID_CELDA_1";
        public string IdCelda2 = "ID_CELDA_2";
        public string IdBancoCondensador = "ID_BANCO_CONDENSADOR";
        public string CapacTransCond1Porcen = "CAPAC_TRANS_COND_1_PORCEN";
        public string CapacTransCond1Min = "CAPAC_TRANS_COND_1_MIN";

        public string CapacTransCond1A = "CAPAC_TRANS_COND_1_A";
        public string CapacTransCond2Porcen = "CAPAC_TRANS_COND_2_PORCEN";
        public string CapacTransCond2Min = "CAPAC_TRANS_COND_2_MIN";
        public string CapacTransCond2A = "CAPAC_TRANS_COND_2_A";

        public string CapacidadTransmisionA = "CAPACIDAD_TRANSMISION_A";
        public string CapacidadTransmisionMVA = "CAPACIDAD_TRANSMISION_MVA";
        public string LimiteSegCoes = "LIMITE_SEG_COES";
        public string FactorLimitanteCalc = "FACTOR_LIMITANTE_CALC";
        public string FactorLimitanteFinal = "FACTOR_LIMITANTE_FINAL";
        

        public string CapacidadMvar = "CAPACIDAD_MVAR";
        public string CapacidadTransmisionMvar = "CAPACIDAD_TRANSMISION_MVAR";
        public string IdInterruptor = "ID_INTERRUPTOR";

        public string Equicodi = "EQUICODI";
        public string CapacTransCorr1A = "CAPAC_TRANS_CORR_1_A";
        public string CapacTransCorr2A = "CAPAC_TRANS_CORR_2_A";

        public string Tension = "TENSION";
        public string NivelTension = "NIVEL_TENSION";
        public string PropTipo = "PROPTIPO";

        #endregion

        #region Mapeo Campos Formulas
        public string Propcodi = "PROPCODI";
        public string Identificador = "IDENTIFICADOR";
        public string Abreviatura = "ABREVIATURA";
        public string ValorIdentificador = "VALOR";
        public string Formula = "FORMULA";

        #endregion

        #region Transformador
        public string IdTransformador = "ID_TRANSFORMADOR";
        public string D1IdCelda = "D1_ID_CELDA";
        public string D1CapacidadOnanMva = "D1_CAPACIDAD_ONAN_MVA";
        public string D1CapacidadOnanMvaComent = "D1_CAPACIDAD_ONAN_MVA_COMENT";
        public string D1CapacidadOnafMva = "D1_CAPACIDAD_ONAF_MVA";
        public string D1CapacidadOnafMvaComent = "D1_CAPACIDAD_ONAF_MVA_COMENT";
        public string D1CapacidadMva = "D1_CAPACIDAD_MVA";
        public string D1CapacidadMvaComent = "D1_CAPACIDAD_MVA_COMENT";
        public string D1CapacidadA = "D1_CAPACIDAD_A";
        public string D1CapacidadAComent = "D1_CAPACIDAD_A_COMENT";
        public string D1PosicionTcA = "D1_POSICION_TC_A";
        public string D1PosicionPickUpA = "D1_POSICION_PICK_UP_A";
        public string D1CapacidadTransmisionA = "D1_CAPACIDAD_TRANSMISION_A";
        public string D1CapacidadTransmisionAComent = "D1_CAPACIDAD_TRANSMISION_A_COMENT";
        public string D1CapacidadTransmisionMva = "D1_CAPACIDAD_TRANSMISION_MVA";
        public string D1CapacidadTransmisionMvaComent = "D1_CAPACIDAD_TRANSMISION_MVA_COMENT";
        public string D1FactorLimitanteCalc = "D1_FACTOR_LIMITANTE_CALC";
        public string D1FactorLimitanteCalcComent = "D1_FACTOR_LIMITANTE_CALC_COMENT";
        public string D1FactorLimitanteFinal = "D1_FACTOR_LIMTANTE_FINAL";
        public string D1FactorLimitanteFinalComent = "D1_FACTOR_LIMTANTE_FINAL_COMENT";
        public string D2IdCelda = "D2_ID_CELDA";
        public string D2CapacidadOnanMva = "D2_CAPACIDAD_ONAN_MVA";
        public string D2CapacidadOnanMvaComent = "D2_CAPACIDAD_ONAN_MVA_COMENT";
        public string D2CapacidadOnafMva = "D2_CAPACIDAD_ONAF_MVA";
        public string D2CapacidadOnafMvaComent = "D2_CAPACIDAD_ONAF_MVA_COMENT";
        public string D2CapacidadMva = "D2_CAPACIDAD_MVA";
        public string D2CapacidadMvaComent = "D2_CAPACIDAD_MVA_COMENT";
        public string D2CapacidadA = "D2_CAPACIDAD_A";
        public string D2CapacidadAComent = "D2_CAPACIDAD_A_COMENT";
        public string D2PosicionTcA = "D2_POSICION_TC_A";
        public string D2PosicionPickUpA = "D2_POSICION_PICK_UP_A";
        public string D2CapacidadTransmisionA = "D2_CAPACIDAD_TRANSMISION_A";
        public string D2CapacidadTransmisionAComent = "D2_CAPACIDAD_TRANSMISION_A_COMENT";
        public string D2CapacidadTransmisionMva = "D2_CAPACIDAD_TRANSMISION_MVA";
        public string D2CapacidadTransmisionMvaComent = "D2_CAPACIDAD_TRANSMISION_MVA_COMENT";
        public string D2FactorLimitanteCalc = "D2_FACTOR_LIMITANTE_CALC";
        public string D2FactorLimitanteCalcComent = "D2_FACTOR_LIMITANTE_CALC_COMENT";
        public string D2FactorLimitanteFinal = "D2_FACTOR_LIMITANTE_FINAL";
        public string D2FactorLimitanteFinalComent = "D2_FACTOR_LIMITANTE_FINAL_COMENT";
        public string D3IdCelda = "D3_ID_CELDA";
        public string D3CapacidadOnanMva = "D3_CAPACIDAD_ONAN_MVA";
        public string D3CapacidadOnanMvaComent = "D3_CAPACIDAD_ONAN_MVA_COMENT";
        public string D3CapacidadOnafMva = "D3_CAPACIDAD_ONAF_MVA";
        public string D3CapacidadOnafMvaComent = "D3_CAPACIDAD_ONAF_MVA_COMENT";
        public string D3CapacidadMva = "D3_CAPACIDAD_MVA";
        public string D3CapacidadMvaComent = "D3_CAPACIDAD_MVA_COMENT";
        public string D3CapacidadA = "D3_CAPACIDAD_A";
        public string D3CapacidadAComent = "D3_CAPACIDAD_A_COMENT";
        public string D3PosicionTcA = "D3_POSICION_TC_A";
        public string D3PosicionPickUpA = "D3_POSICION_PICK_UP_A";
        public string D3CapacidadTransmisionA = "D3_CAPACIDAD_TRANSMISION_A";
        public string D3CapacidadTransmisionAComent = "D3_CAPACIDAD_TRANSMISION_A_COMENT";
        public string D3CapacidadTransmisionMva = "D3_CAPACIDAD_TRANSMISION_MVA";
        public string D3CapacidadTransmisionMvaComent = "D3_CAPACIDAD_TRANSMISION_MVA_COMENT";
        public string D3FactorLimitanteCalc = "D3_FACTOR_LIMITANTE_CALC";
        public string D3FactorLimitanteCalcComent = "D3_FACTOR_LIMITANTE_CALC_COMENT";
        public string D3FactorLimitanteFinal = "D3_FACTOR_LIMITANTE_FINAL";
        public string D3FactorLimitanteFinalComent = "D3_FACTOR_LIMITANTE_FINAL_COMENT";

        public string D4IdCelda = "D4_ID_CELDA";
        public string D4CapacidadOnanMva = "D4_CAPACIDAD_ONAN_MVA";
        public string D4CapacidadOnanMvaComent = "D4_CAPACIDAD_ONAN_MVA_COMENT";
        public string D4CapacidadOnafMva = "D4_CAPACIDAD_ONAF_MVA";
        public string D4CapacidadOnafMvaComent = "D4_CAPACIDAD_ONAF_MVA_COMENT";
        public string D4CapacidadMva = "D4_CAPACIDAD_MVA";
        public string D4CapacidadMvaComent = "D4_CAPACIDAD_MVA_COMENT";
        public string D4CapacidadA = "D4_CAPACIDAD_A";
        public string D4CapacidadAComent = "D4_CAPACIDAD_A_COMENT";
        public string D4PosicionTcA = "D4_POSICION_TC_A";
        public string D4PosicionPickUpA = "D4_POSICION_PICK_UP_A";
        public string D4CapacidadTransmisionA = "D4_CAPACIDAD_TRANSMISION_A";
        public string D4CapacidadTransmisionAComent = "D4_CAPACIDAD_TRANSMISION_A_COMENT";
        public string D4CapacidadTransmisionMva = "D4_CAPACIDAD_TRANSMISION_MVA";
        public string D4CapacidadTransmisionMvaComent = "D4_CAPACIDAD_TRANSMISION_MVA_COMENT";
        public string D4FactorLimitanteCalc = "D4_FACTOR_LIMITANTE_CALC";
        public string D4FactorLimitanteCalcComent = "D4_FACTOR_LIMITANTE_CALC_COMENT";
        public string D4FactorLimitanteFinal = "D4_FACTOR_LIMITANTE_FINAL";
        public string D4FactorLimitanteFinalComent = "D4_FACTOR_LIMITANTE_FINAL_COMENT";

        public string D1Tension = "d1_nivel_tension";
        public string D2Tension = "d2_nivel_tension";
        public string D3Tension = "d3_nivel_tension";
        public string D4Tension = "d4_nivel_tension";

        #endregion

        public string TipoPropiedad = "TIPOPROPIEDAD";
        public string Famcodi = "FAMCODI";

        #region Campos Paginacion
        public static string MaxRowToFetch = "MAXROWTOFETCH";
        public static string MinRowToFetch = "MINROWTOFETCH";
        #endregion

        public string ListCalculoLinea
        {
            get { return base.GetSqlXml("ListCalculoLinea"); }
        }

        public string ListCalculoReactor
        {
            get { return base.GetSqlXml("ListCalculoReactor"); }
        }

        public string ListCalculoCelda
        {
            get { return base.GetSqlXml("ListCalculoCelda"); }
        }

        public string ListCalculoTransformadorDosDevanados
        {
            get { return base.GetSqlXml("ListCalculoTransformadorDosDevanados"); }
        }
        public string ListCalculoTransformadorTresDevanados
        {
            get { return base.GetSqlXml("ListCalculoTransformadorTresDevanados"); }
        }
        public string ListCalculoTransformadorCuatroDevanados
        {
            get { return base.GetSqlXml("ListCalculoTransformadorCuatroDevanados"); }
        }

        public string EvaluarCeldaPosicionNucleo
        {
            get { return base.GetSqlXml("EvaluarCeldaPosicionNucleo"); }
        }
        public string EvaluarCeldaPickUp
        {
            get { return base.GetSqlXml("EvaluarCeldaPickUp"); }
        }

        public string EvaluarPropiedadEquipo
        {
            get { return base.GetSqlXml("EvaluarPropiedadEquipo"); }
        }

        public string EvaluarTensionEquipo
        {
            get { return base.GetSqlXml("EvaluarTensionEquipo"); }
        }

        public string ListFunciones
        {
            get { return base.GetSqlXml("ListFunciones"); }
        }

        public string ListPropiedades
        {
            get { return base.GetSqlXml("ListPropiedades"); }
        }

        public string ListValidarFormula
        {
            get { return base.GetSqlXml("ListValidarFormula"); }
        }

        public string ListCalculosLineaMasivo
        {
            get { return base.GetSqlXml("ListCalculosLineaMasivo"); }
        }

        public string ListCalculosCeldaMasivo
        {
            get { return base.GetSqlXml("ListCalculosCeldaMasivo"); }
        }

        public string ListCalculosReactorMasivo
        {
            get { return base.GetSqlXml("ListCalculosReactorMasivo"); }
        }

        public string ListCalculosTransformadorDosDevanadosMasivo
        {
            get { return base.GetSqlXml("ListCalculosTransformadorDosDevanadosMasivo"); }
        }

        public string ListCalculosTransformadorTresDevanadosMasivo
        {
            get { return base.GetSqlXml("ListCalculosTransformadorTresDevanadosMasivo"); }
        }
        
        public string ListCalculosTransformadorCuatroDevanadosMasivo
        {
            get { return base.GetSqlXml("ListCalculosTransformadorCuatroDevanadosMasivo"); }
        }

        public string ListCalculoInterruptor
        {
            get { return base.GetSqlXml("ListCalculoInterruptor"); }
        }
    }
}
