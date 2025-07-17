using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla EPR_PROYECTO_ACTEQP
    /// </summary>
    [DataContract]
    [Serializable]
    public class EprRepLimitCapDTO : EntityBase
    {
        [DataMember]
        public int Numero { get; set; }
        [DataMember]
        public string Empresa { get; set; }
        [DataMember]
        public string Codigo { get; set; }
        [DataMember]
        public string Tension { get; set; }
        [DataMember]
        public string Subestacion { get; set; }
        [DataMember]
        public string CapacidadTransmisionA { get; set; }
        [DataMember]
        public string CapacidadTransmisionMVA { get; set; }
        [DataMember]
        public string CapacidadTransmisionCondPorcen { get; set; }
        [DataMember]
        public string CapacidadTransmisionCondMin { get; set; }
        [DataMember]
        public string FactorLimitanteFinal { get; set; }
        [DataMember]
        public string EpProyNomb { get; set; }

        [DataMember]
        public string Eprtlcfecemision { get; set; }
        [DataMember]
        public string Eprtlcrevision { get; set; }
        [DataMember]
        public string Eprtlcdescripcion { get; set; }
        [DataMember]
        public string Eprtlcusuelabora { get; set; }
        [DataMember]
        public string Eprtlcusurevisa { get; set; }
        [DataMember]
        public string Eprtlcusuaprueba { get; set; }
        [DataMember]
        public string Observaciones { get; set; }
        [DataMember]
        public string Siglas { get; set; }
        [DataMember]
        public string Descripcion { get; set; }

        [DataMember]
        public string Equicodi {  get; set; }
        [DataMember]
        public int Famcodi { get; set; }
        [DataMember]
        public string Capac_trans_cond_1_porcen { get; set; }
        [DataMember]
        public string Capac_trans_cond_2_porcen { get; set; }
        [DataMember]
        public string Capac_trans_cond_1_min { get; set; }
        [DataMember]
        public string Capac_trans_cond_2_min { get; set; }
        [DataMember]
        public string Capacidad_transmision_a_coment { get; set; }
        [DataMember]
        public string Capacidad_transmision_mva_coment { get; set; }
        [DataMember]
        public string Capac_trans_cond_1_porcen_coment { get; set; }
        [DataMember]
        public string Capac_trans_cond_2_porcen_coment { get; set; }
        [DataMember]
        public string Capac_trans_cond_1_min_coment { get; set; }
        [DataMember]
        public string Capac_trans_cond_2_min_coment { get; set; }
        [DataMember]
        public string Factor_limitante_final_coment { get; set; }


        public string D1_capacidad_transmision_mva_coment { get; set; }
        public string D2_capacidad_transmision_mva_coment { get; set; }
        public string D3_capacidad_transmision_mva_coment { get; set; }
        public string D4_capacidad_transmision_mva_coment { get; set; }

        public string D1_capacidad_transmision_a_coment { get; set; }
        public string D2_capacidad_transmision_a_coment { get; set; }
        public string D3_capacidad_transmision_a_coment { get; set; }
        public string D4_capacidad_transmision_a_coment { get; set; }

        public string D1_factor_limitante_final_coment { get; set; }
        public string D2_factor_limitante_final_coment { get; set; }
        public string D3_factor_limitante_final_coment { get; set; }
        public string D4_factor_limitante_final_coment { get; set; }

        public string D1_capacidad_transmision_mva { get; set; }
        public string D2_capacidad_transmision_mva { get; set; }
        public string D3_capacidad_transmision_mva { get; set; }
        public string D4_capacidad_transmision_mva { get; set; }

        public string D1_capacidad_transmision_a { get; set; }
        public string D2_capacidad_transmision_a { get; set; }
        public string D3_capacidad_transmision_a { get; set; }
        public string D4_capacidad_transmision_a { get; set; }

        public string D1_factor_limitante_final { get; set; }
        public string D2_factor_limitante_final { get; set; }
        public string D3_factor_limitante_final { get; set; }
        public string D4_factor_limitante_final { get; set; }

        public string D1_tension { get; set; }
        public string D2_tension { get; set; }
        public string D3_tension { get; set; }
        public string D4_tension { get; set; }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
