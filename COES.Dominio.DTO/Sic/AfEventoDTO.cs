using COES.Base.Core;
using System;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla AF_EVENTO
    /// </summary>
    public class AfEventoDTO : EntityBase
    {
        public string Aferacmt { get; set; }
        public string Afeeracmf { get; set; }
        public string Afermc { get; set; }
        public int? Afecorr { get; set; }
        public int? Afeanio { get; set; }
        public int? Evencodi { get; set; }
        public int Afecodi { get; set; }
        public string Afefzamayor { get; set; }
        public string Afeestadomotivo { get; set; }
        public string Afeestado { get; set; }
        public string Afeempcompninguna { get; set; }
        public string Afeemprespninguna { get; set; }
        public DateTime? Afeitpitffecha { get; set; }
        public DateTime? Afefzadecisfechasist { get; set; }
        public DateTime? Afefzafechasist { get; set; }
        public DateTime? Afeitdecfechaelab { get; set; }
        public DateTime? Afeitdecfechanominal { get; set; }
        public DateTime? Afecompfechasist { get; set; }
        public DateTime? Afecompfecha { get; set; }
        public DateTime? Afeitpdecisffechasist { get; set; }
        public DateTime? Afeitpitffechasist { get; set; }
        public DateTime? Afeconvcitacionfecha { get; set; }
        public DateTime? Aferctaeinformefecha { get; set; }
        public DateTime? Aferctaeactafecha { get; set; }
        public string Afeimpugna { get; set; }
        public DateTime? Lastdate { get; set; }
        public string Lastuser { get; set; }
        public DateTime? Afeitrdofecharecep { get; set; }
        public DateTime? Afeitrdofechaenvio { get; set; }
        public string Afeitrdoestado { get; set; }
        public DateTime? Afeitrdjrfecharecep { get; set; }
        public DateTime? Afeitrdjrfechaenvio { get; set; }
        public string Afeitrdjrestado { get; set; }
        public DateTime? Afeitfechaelab { get; set; }
        public DateTime? Afeitfechanominal { get; set; }
        public string Aferctaeobservacion { get; set; }
        public string Afeconvtiporeunion { get; set; }
        public string Afereuhoraprog { get; set; }
        public DateTime? Afereufechaprog { get; set; }
        public DateTime? Afereufechanominal { get; set; }
        public DateTime? Afecitfechaelab { get; set; }
        public DateTime? Afecitfechanominal { get; set; }
        public string Aferesponsable { get; set; }
        public string Afeedagsf { get; set; }
        public DateTime? Afeplazofecmodificacion { get; set; }
        public string Afeplazousumodificacion { get; set; }
        public DateTime? Afeplazofechaampl { get; set; }
        public DateTime? Afeplazofecha { get; set; }
        public DateTime? Afefechainterr { get; set; }
    }
}
