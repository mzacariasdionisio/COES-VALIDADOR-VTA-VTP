using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;
using COES.Framework.Base.Core;
using COES.Framework.Base.Tools;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla EPR_PROYECTO_ACTEQP
    /// </summary>
    public class EprRepLimitCapHelper : HelperBase
    {
        public EprRepLimitCapHelper() : base(Consultas.EprRepLimitCapSql)
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
        public EprRepLimitCapDTO Create(IDataReader dr)
        {
            EprRepLimitCapDTO entity = new EprRepLimitCapDTO();
            int iNumero = dr.GetOrdinal(this.Numero);
            if (!dr.IsDBNull(iNumero)) entity.Numero = Convert.ToInt32(dr.GetValue(iNumero));
            int iEmpresa = dr.GetOrdinal(this.Empresa);
            if (!dr.IsDBNull(iEmpresa)) entity.Empresa = Convert.ToString(dr.GetValue(iEmpresa));
            int iSubestacion = dr.GetOrdinal(this.Subestacion);
            if (!dr.IsDBNull(iSubestacion)) entity.Subestacion = Convert.ToString(dr.GetValue(iSubestacion));
            int iCodigo = dr.GetOrdinal(this.Codigo);
            if (!dr.IsDBNull(iCodigo)) entity.Codigo = Convert.ToString(dr.GetValue(iCodigo));
            int iTension = dr.GetOrdinal(this.Tension);
            if (!dr.IsDBNull(iTension)) entity.Tension = Convert.ToString(dr.GetValue(iTension));
            int iCapacidadTransmisionA = dr.GetOrdinal(this.CapacidadTransmisionA);
            if (!dr.IsDBNull(iCapacidadTransmisionA)) entity.CapacidadTransmisionA = Convert.ToString(dr.GetValue(iCapacidadTransmisionA));
            int iCapacidadTransmisionMVA = dr.GetOrdinal(this.CapacidadTransmisionMVA);
            if (!dr.IsDBNull(iCapacidadTransmisionMVA)) entity.CapacidadTransmisionMVA = Convert.ToString(dr.GetValue(iCapacidadTransmisionMVA));

            int iCapacidadTransmisionCondPorcen = dr.GetOrdinal(this.CapacidadTransmisionCondPorcen);
            if (!dr.IsDBNull(iCapacidadTransmisionCondPorcen)) entity.CapacidadTransmisionCondPorcen = Convert.ToString(dr.GetValue(iCapacidadTransmisionCondPorcen));
            int iCapacidadTransmisionCondMin = dr.GetOrdinal(this.CapacidadTransmisionCondMin);
            if (!dr.IsDBNull(iCapacidadTransmisionCondMin)) entity.CapacidadTransmisionCondMin = Convert.ToString(dr.GetValue(iCapacidadTransmisionCondMin));

            int iCapacidad_transmision_a_coment = dr.GetOrdinal(this.Capacidad_transmision_a_coment);
            if (!dr.IsDBNull(iCapacidad_transmision_a_coment)) entity.Capacidad_transmision_a_coment = Convert.ToString(dr.GetValue(iCapacidad_transmision_a_coment));

            int iCapacidad_transmision_mva_coment = dr.GetOrdinal(this.Capacidad_transmision_mva_coment);
            if (!dr.IsDBNull(iCapacidad_transmision_mva_coment)) entity.Capacidad_transmision_mva_coment = Convert.ToString(dr.GetValue(iCapacidad_transmision_mva_coment));

            int iFactor_limitante_final_coment = dr.GetOrdinal(this.Factor_limitante_final_coment);
            if (!dr.IsDBNull(iFactor_limitante_final_coment)) entity.Factor_limitante_final_coment = Convert.ToString(dr.GetValue(iFactor_limitante_final_coment));

            int iFactorLimitanteFinal = dr.GetOrdinal(this.FactorLimitanteFinal);
            if (!dr.IsDBNull(iFactorLimitanteFinal)) entity.FactorLimitanteFinal = Convert.ToString(dr.GetValue(iFactorLimitanteFinal));

            int iEquicodi = dr.GetOrdinal(this.Equicodi);
            if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToString(dr.GetValue(iEquicodi));
            return entity;
        }

        public EprRepLimitCapDTO CreateTransformador(IDataReader dr)
        {
            EprRepLimitCapDTO entity = new EprRepLimitCapDTO();
            int iNumero = dr.GetOrdinal(this.Numero);
            if (!dr.IsDBNull(iNumero)) entity.Numero = Convert.ToInt32(dr.GetValue(iNumero));
            int iEmpresa = dr.GetOrdinal(this.Empresa);
            if (!dr.IsDBNull(iEmpresa)) entity.Empresa = Convert.ToString(dr.GetValue(iEmpresa));
            int iCodigo = dr.GetOrdinal(this.Codigo);
            if (!dr.IsDBNull(iCodigo)) entity.Codigo = Convert.ToString(dr.GetValue(iCodigo));
            int iSubestacion = dr.GetOrdinal(this.Subestacion);
            if (!dr.IsDBNull(iSubestacion)) entity.Subestacion = Convert.ToString(dr.GetValue(iSubestacion));

            int iEquicodi = dr.GetOrdinal(this.Equicodi);
            if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToString(dr.GetValue(iEquicodi));
            int iFamcodi = dr.GetOrdinal(this.Famcodi);
            if (!dr.IsDBNull(iFamcodi)) entity.Famcodi = Convert.ToInt32(dr.GetValue(iFamcodi));

            int iD1_capacidad_transmision_mva_coment = dr.GetOrdinal(this.D1_capacidad_transmision_mva_coment);
            if (!dr.IsDBNull(iD1_capacidad_transmision_mva_coment)) entity.D1_capacidad_transmision_mva_coment = Convert.ToString(dr.GetValue(iD1_capacidad_transmision_mva_coment));

            int iD2_capacidad_transmision_mva_coment = dr.GetOrdinal(this.D2_capacidad_transmision_mva_coment);
            if (!dr.IsDBNull(iD2_capacidad_transmision_mva_coment)) entity.D2_capacidad_transmision_mva_coment = Convert.ToString(dr.GetValue(iD2_capacidad_transmision_mva_coment));

            int iD3_capacidad_transmision_mva_coment = dr.GetOrdinal(this.D3_capacidad_transmision_mva_coment);
            if (!dr.IsDBNull(iD3_capacidad_transmision_mva_coment)) entity.D3_capacidad_transmision_mva_coment = Convert.ToString(dr.GetValue(iD3_capacidad_transmision_mva_coment));

            int iD4_capacidad_transmision_mva_coment = dr.GetOrdinal(this.D4_capacidad_transmision_mva_coment);
            if (!dr.IsDBNull(iD4_capacidad_transmision_mva_coment)) entity.D4_capacidad_transmision_mva_coment = Convert.ToString(dr.GetValue(iD4_capacidad_transmision_mva_coment));

            int iD1_capacidad_transmision_a_coment = dr.GetOrdinal(this.D1_capacidad_transmision_a_coment);
            if (!dr.IsDBNull(iD1_capacidad_transmision_a_coment)) entity.D1_capacidad_transmision_a_coment = Convert.ToString(dr.GetValue(iD1_capacidad_transmision_a_coment));

            int iD2_capacidad_transmision_a_coment = dr.GetOrdinal(this.D2_capacidad_transmision_a_coment);
            if (!dr.IsDBNull(iD2_capacidad_transmision_a_coment)) entity.D2_capacidad_transmision_a_coment = Convert.ToString(dr.GetValue(iD2_capacidad_transmision_a_coment));

            int iD3_capacidad_transmision_a_coment = dr.GetOrdinal(this.D3_capacidad_transmision_a_coment);
            if (!dr.IsDBNull(iD3_capacidad_transmision_a_coment)) entity.D3_capacidad_transmision_a_coment = Convert.ToString(dr.GetValue(iD3_capacidad_transmision_a_coment));

            int iD4_capacidad_transmision_a_coment = dr.GetOrdinal(this.D4_capacidad_transmision_a_coment);
            if (!dr.IsDBNull(iD4_capacidad_transmision_a_coment)) entity.D4_capacidad_transmision_a_coment = Convert.ToString(dr.GetValue(iD4_capacidad_transmision_a_coment));

            int iD1_factor_limitante_final_coment = dr.GetOrdinal(this.D1_factor_limitante_final_coment);
            if (!dr.IsDBNull(iD1_factor_limitante_final_coment)) entity.D1_factor_limitante_final_coment = Convert.ToString(dr.GetValue(iD1_factor_limitante_final_coment));

            int iD2_factor_limitante_final_coment = dr.GetOrdinal(this.D2_factor_limitante_final_coment);
            if (!dr.IsDBNull(iD2_factor_limitante_final_coment)) entity.D2_factor_limitante_final_coment = Convert.ToString(dr.GetValue(iD2_factor_limitante_final_coment));

            int iD3_factor_limitante_final_coment = dr.GetOrdinal(this.D3_factor_limitante_final_coment);
            if (!dr.IsDBNull(iD3_factor_limitante_final_coment)) entity.D3_factor_limitante_final_coment = Convert.ToString(dr.GetValue(iD3_factor_limitante_final_coment));

            int iD4_factor_limitante_final_coment = dr.GetOrdinal(this.D4_factor_limitante_final_coment);
            if (!dr.IsDBNull(iD4_factor_limitante_final_coment)) entity.D4_factor_limitante_final_coment = Convert.ToString(dr.GetValue(iD4_factor_limitante_final_coment));

            int iD1_tension = dr.GetOrdinal(this.D1_tension);
            if (!dr.IsDBNull(iD1_tension)) entity.D1_tension = Convert.ToString(dr.GetValue(iD1_tension));

            int iD2_tension = dr.GetOrdinal(this.D2_tension);
            if (!dr.IsDBNull(iD2_tension)) entity.D2_tension = Convert.ToString(dr.GetValue(iD2_tension));

            int iD3_tension = dr.GetOrdinal(this.D3_tension);
            if (!dr.IsDBNull(iD3_tension)) entity.D3_tension = Convert.ToString(dr.GetValue(iD3_tension));

            int iD4_tension = dr.GetOrdinal(this.D4_tension);
            if (!dr.IsDBNull(iD4_tension)) entity.D4_tension = Convert.ToString(dr.GetValue(iD4_tension));


            return entity;
        }

        public EprRepLimitCapDTO CreateAcoplamiento(IDataReader dr)
        {
            EprRepLimitCapDTO entity = new EprRepLimitCapDTO();
            int iNumero = dr.GetOrdinal(this.Numero);
            if (!dr.IsDBNull(iNumero)) entity.Numero = Convert.ToInt32(dr.GetValue(iNumero));
            int iEmpresa = dr.GetOrdinal(this.Empresa);
            if (!dr.IsDBNull(iEmpresa)) entity.Empresa = Convert.ToString(dr.GetValue(iEmpresa));
            int iSubestacion = dr.GetOrdinal(this.Subestacion);
            if (!dr.IsDBNull(iSubestacion)) entity.Subestacion = Convert.ToString(dr.GetValue(iSubestacion));
            int iTension = dr.GetOrdinal(this.Tension);
            if (!dr.IsDBNull(iTension)) entity.Tension = Convert.ToString(dr.GetValue(iTension));
            int iCapacidadTransmisionA = dr.GetOrdinal(this.CapacidadTransmisionA);
            if (!dr.IsDBNull(iCapacidadTransmisionA)) entity.CapacidadTransmisionA = Convert.ToString(dr.GetValue(iCapacidadTransmisionA));
            int iCapacidadTransmisionMVA = dr.GetOrdinal(this.CapacidadTransmisionMVA);
            if (!dr.IsDBNull(iCapacidadTransmisionMVA)) entity.CapacidadTransmisionMVA = Convert.ToString(dr.GetValue(iCapacidadTransmisionMVA));
            int iFactorLimitanteFinal = dr.GetOrdinal(this.FactorLimitanteFinal);
            if (!dr.IsDBNull(iFactorLimitanteFinal)) entity.FactorLimitanteFinal = Convert.ToString(dr.GetValue(iFactorLimitanteFinal));
            int iObservaciones = dr.GetOrdinal(this.Observaciones);
            if (!dr.IsDBNull(iObservaciones)) entity.Observaciones = Convert.ToString(dr.GetValue(iObservaciones));

            int iCapacidad_transmision_a_coment = dr.GetOrdinal(this.Capacidad_transmision_a_coment);
            if (!dr.IsDBNull(iCapacidad_transmision_a_coment)) entity.Capacidad_transmision_a_coment = Convert.ToString(dr.GetValue(iCapacidad_transmision_a_coment));

            int iCapacidad_transmision_mva_coment = dr.GetOrdinal(this.Capacidad_transmision_mva_coment);
            if (!dr.IsDBNull(iCapacidad_transmision_mva_coment)) entity.Capacidad_transmision_mva_coment = Convert.ToString(dr.GetValue(iCapacidad_transmision_mva_coment));

            int iFactor_limitante_final_coment = dr.GetOrdinal(this.Factor_limitante_final_coment);
            if (!dr.IsDBNull(iFactor_limitante_final_coment)) entity.Factor_limitante_final_coment = Convert.ToString(dr.GetValue(iFactor_limitante_final_coment));


            int iEquicodi = dr.GetOrdinal(this.Equicodi);
            if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToString(dr.GetValue(iEquicodi));

            return entity;
        }

        public EprRepLimitCapDTO CreateEpresaSigla(IDataReader dr)
        {
            EprRepLimitCapDTO entity = new EprRepLimitCapDTO();
            int iSiglas = dr.GetOrdinal(this.Siglas);
            if (!dr.IsDBNull(iSiglas)) entity.Siglas = Convert.ToString(dr.GetValue(iSiglas));
            int iDescripcion = dr.GetOrdinal(this.Descripcion);
            if (!dr.IsDBNull(iDescripcion)) entity.Descripcion = Convert.ToString(dr.GetValue(iDescripcion));
           
            return entity;
        }


        //GESPROTEC
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
        public string Numero = "NUMERO";
        public string Empresa = "EMPRESA";
        public string Codigo = "CODIGO";
        public string Tension = "TENSION";
        public string Subestacion = "SUBESTACIONES";
        public string CapacidadTransmisionA = "CAPACIDAD_TRANSMISION_A";
        public string CapacidadTransmisionMVA = "CAPACIDAD_TRANSMISION_MVA";
        public string CapacidadTransmisionCondPorcen = "CAPAC_TRANS_COND_PORCEN";
        public string CapacidadTransmisionCondMin = "CAPAC_TRANS_COND_MIN";
        public string FactorLimitanteFinal = "FACTOR_LIMITANTE_FINAL";
        public string Observaciones = "OBSERVACIONES";
        public string Eprtlcfecemision = "EPRTLCFECEMISION";
        public string Eprtlcrevision = "EPRTLCREVISION";
        public string Eprtlcdescripcion = "EPRTLCDESCRIPCION";
        public string Eprtlcusuelabora = "EPRTLCUSUELABORA";
        public string Eprtlcusurevisa = "EPRTLCUSUREVISA";
        public string Eprtlcusuaprueba = "EPRTLCUSUAPRUEBA";
        public string Siglas = "SIGLAS";
        public string Descripcion = "DESCRIPCION";
        public string Equicodi = "EQUICODI";
        public string Famcodi = "FAMCODI";

        public string Capac_trans_cond_1_porcen = "CAPAC_TRANS_COND_1_PORCEN";
        public string Capac_trans_cond_2_porcen = "capac_trans_cond_2_porcen";
        public string Capac_trans_cond_1_min = "capac_trans_cond_1_min";
        public string Capac_trans_cond_2_min = "capac_trans_cond_2_min";

        public string Capacidad_transmision_a_coment = "capacidad_transmision_a_coment";
        public string Capacidad_transmision_mva_coment = "capacidad_transmision_mva_coment";
        public string Capac_trans_cond_1_porcen_coment = "capac_trans_cond_1_porcen_coment";
        public string Capac_trans_cond_2_porcen_coment = "capac_trans_cond_2_porcen_coment";
        public string Capac_trans_cond_1_min_coment = "capac_trans_cond_1_min_coment";
        public string Capac_trans_cond_2_min_coment = "capac_trans_cond_2_min_coment";
        public string Factor_limitante_final_coment = "factor_limitante_final_coment";

        public string D1_capacidad_transmision_mva_coment = "D1_capacidad_transmision_mva_coment";
        public string D2_capacidad_transmision_mva_coment = "D2_capacidad_transmision_mva_coment";
        public string D3_capacidad_transmision_mva_coment = "D3_capacidad_transmision_mva_coment";
        public string D4_capacidad_transmision_mva_coment = "D4_capacidad_transmision_mva_coment";

        public string D1_capacidad_transmision_a_coment = "D1_capacidad_transmision_a_coment";
        public string D2_capacidad_transmision_a_coment = "D2_capacidad_transmision_a_coment";
        public string D3_capacidad_transmision_a_coment = "D3_capacidad_transmision_a_coment";
        public string D4_capacidad_transmision_a_coment = "D4_capacidad_transmision_a_coment";

        public string D1_factor_limitante_final_coment = "D1_factor_limitante_final_coment";
        public string D2_factor_limitante_final_coment = "D2_factor_limitante_final_coment";
        public string D3_factor_limitante_final_coment = "D3_factor_limitante_final_coment";
        public string D4_factor_limitante_final_coment = "D4_factor_limitante_final_coment";

        public string D1_capacidad_transmision_mva = "D1_capacidad_transmision_mva";
        public string D2_capacidad_transmision_mva = "D2_capacidad_transmision_mva";
        public string D3_capacidad_transmision_mva = "D3_capacidad_transmision_mva";
        public string D4_capacidad_transmision_mva = "D4_capacidad_transmision_mva";

        public string D1_capacidad_transmision_a = "D1_capacidad_transmision_a";
        public string D2_capacidad_transmision_a = "D2_capacidad_transmision_a";
        public string D3_capacidad_transmision_a = "D3_capacidad_transmision_a";
        public string D4_capacidad_transmision_a = "D4_capacidad_transmision_a";

        public string D1_factor_limitante_final = "D1_factor_limitante_final";
        public string D2_factor_limitante_final = "D2_factor_limitante_final";
        public string D3_factor_limitante_final = "D3_factor_limitante_final";
        public string D4_factor_limitante_final = "D4_factor_limitante_final";

        public string D1_tension = "D1_tension";
        public string D2_tension = "D2_tension";
        public string D3_tension = "D3_tension";
        public string D4_tension = "D4_tension";

        //Parametros
        public string IdAreaExcel = "ID_AREA_EXCEL";
        public string EpProyNomb = "EPPROYNOMB";
        #endregion

        #region Campos Paginacion
        public static string MaxRowToFetch = "MAXROWTOFETCH";
        public static string MinRowToFetch = "MINROWTOFETCH";
        #endregion

        public string ListCapacidadTransmision
        {
            get { return base.GetSqlXml("SqlListCapacidadTransmision"); }
        }
        public string ListCapacidadTransformador
        {
            get { return base.GetSqlXml("SqlListCapacidadTransformador"); }
        }
        public string ListCapacidadAcoplaminento
        {
            get { return base.GetSqlXml("SqlListCapacidadAcoplaminento"); }
        }
        public string ListActualizaciones
        {
            get { return base.GetSqlXml("SqlListActualizaciones"); }
        }

        public string ListRevisiones
        {
            get { return base.GetSqlXml("SqlListRevisiones"); }
        }
        public string ListaEmpresaSigla
        {
            get { return base.GetSqlXml("SqlListaEmpresaSigla"); }
        }
        
    }
}
