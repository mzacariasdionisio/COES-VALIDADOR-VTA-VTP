using COES.Base.Core;
using COES.Dominio.DTO.Sic;
using System;
using System.Data;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla AF_EVENTO
    /// </summary>
    public class AfEventoHelper : HelperBase
    {
        public AfEventoHelper() : base(Consultas.AfEventoSql)
        {
        }

        public AfEventoDTO Create(IDataReader dr)
        {
            AfEventoDTO entity = new AfEventoDTO();

            int iAferacmt = dr.GetOrdinal(this.Aferacmt);
            if (!dr.IsDBNull(iAferacmt)) entity.Aferacmt = dr.GetString(iAferacmt);

            int iAfeeracmf = dr.GetOrdinal(this.Afeeracmf);
            if (!dr.IsDBNull(iAfeeracmf)) entity.Afeeracmf = dr.GetString(iAfeeracmf);

            int iAfermc = dr.GetOrdinal(this.Afermc);
            if (!dr.IsDBNull(iAfermc)) entity.Afermc = dr.GetString(iAfermc);

            int iAfecorr = dr.GetOrdinal(this.Afecorr);
            if (!dr.IsDBNull(iAfecorr)) entity.Afecorr = Convert.ToInt32(dr.GetValue(iAfecorr));

            int iAfeanio = dr.GetOrdinal(this.Afeanio);
            if (!dr.IsDBNull(iAfeanio)) entity.Afeanio = Convert.ToInt32(dr.GetValue(iAfeanio));

            int iEvencodi = dr.GetOrdinal(this.Evencodi);
            if (!dr.IsDBNull(iEvencodi)) entity.Evencodi = Convert.ToInt32(dr.GetValue(iEvencodi));

            int iAfecodi = dr.GetOrdinal(this.Afecodi);
            if (!dr.IsDBNull(iAfecodi)) entity.Afecodi = Convert.ToInt32(dr.GetValue(iAfecodi));

            int iAfefzamayor = dr.GetOrdinal(this.Afefzamayor);
            if (!dr.IsDBNull(iAfefzamayor)) entity.Afefzamayor = dr.GetString(iAfefzamayor);

            int iAfeestadomotivo = dr.GetOrdinal(this.Afeestadomotivo);
            if (!dr.IsDBNull(iAfeestadomotivo)) entity.Afeestadomotivo = dr.GetString(iAfeestadomotivo);

            int iAfeestado = dr.GetOrdinal(this.Afeestado);
            if (!dr.IsDBNull(iAfeestado)) entity.Afeestado = dr.GetString(iAfeestado);

            int iAfeempcompninguna = dr.GetOrdinal(this.Afeempcompninguna);
            if (!dr.IsDBNull(iAfeempcompninguna)) entity.Afeempcompninguna = dr.GetString(iAfeempcompninguna);

            int iAfeemprespninguna = dr.GetOrdinal(this.Afeemprespninguna);
            if (!dr.IsDBNull(iAfeemprespninguna)) entity.Afeemprespninguna = dr.GetString(iAfeemprespninguna);

            int iAfeitpitffecha = dr.GetOrdinal(this.Afeitpitffecha);
            if (!dr.IsDBNull(iAfeitpitffecha)) entity.Afeitpitffecha = dr.GetDateTime(iAfeitpitffecha);

            int iAfefzadecisfechasist = dr.GetOrdinal(this.Afefzadecisfechasist);
            if (!dr.IsDBNull(iAfefzadecisfechasist)) entity.Afefzadecisfechasist = dr.GetDateTime(iAfefzadecisfechasist);

            int iAfefzafechasist = dr.GetOrdinal(this.Afefzafechasist);
            if (!dr.IsDBNull(iAfefzafechasist)) entity.Afefzafechasist = dr.GetDateTime(iAfefzafechasist);

            int iAfeitdecfechaelab = dr.GetOrdinal(this.Afeitdecfechaelab);
            if (!dr.IsDBNull(iAfeitdecfechaelab)) entity.Afeitdecfechaelab = dr.GetDateTime(iAfeitdecfechaelab);

            int iAfeitdecfechanominal = dr.GetOrdinal(this.Afeitdecfechanominal);
            if (!dr.IsDBNull(iAfeitdecfechanominal)) entity.Afeitdecfechanominal = dr.GetDateTime(iAfeitdecfechanominal);

            int iAfecompfechasist = dr.GetOrdinal(this.Afecompfechasist);
            if (!dr.IsDBNull(iAfecompfechasist)) entity.Afecompfechasist = dr.GetDateTime(iAfecompfechasist);

            int iAfecompfecha = dr.GetOrdinal(this.Afecompfecha);
            if (!dr.IsDBNull(iAfecompfecha)) entity.Afecompfecha = dr.GetDateTime(iAfecompfecha);

            int iAfeitpdecisffechasist = dr.GetOrdinal(this.Afeitpdecisffechasist);
            if (!dr.IsDBNull(iAfeitpdecisffechasist)) entity.Afeitpdecisffechasist = dr.GetDateTime(iAfeitpdecisffechasist);

            int iAfeitpitffechasist = dr.GetOrdinal(this.Afeitpitffechasist);
            if (!dr.IsDBNull(iAfeitpitffechasist)) entity.Afeitpitffechasist = dr.GetDateTime(iAfeitpitffechasist);

            int iAfeconvcitacionfecha = dr.GetOrdinal(this.Afeconvcitacionfecha);
            if (!dr.IsDBNull(iAfeconvcitacionfecha)) entity.Afeconvcitacionfecha = dr.GetDateTime(iAfeconvcitacionfecha);

            int iAferctaeinformefecha = dr.GetOrdinal(this.Aferctaeinformefecha);
            if (!dr.IsDBNull(iAferctaeinformefecha)) entity.Aferctaeinformefecha = dr.GetDateTime(iAferctaeinformefecha);

            int iAferctaeactafecha = dr.GetOrdinal(this.Aferctaeactafecha);
            if (!dr.IsDBNull(iAferctaeactafecha)) entity.Aferctaeactafecha = dr.GetDateTime(iAferctaeactafecha);

            int iAfeimpugna = dr.GetOrdinal(this.Afeimpugna);
            if (!dr.IsDBNull(iAfeimpugna)) entity.Afeimpugna = dr.GetString(iAfeimpugna);

            int iLastdate = dr.GetOrdinal(this.Lastdate);
            if (!dr.IsDBNull(iLastdate)) entity.Lastdate = dr.GetDateTime(iLastdate);

            int iLastuser = dr.GetOrdinal(this.Lastuser);
            if (!dr.IsDBNull(iLastuser)) entity.Lastuser = dr.GetString(iLastuser);

            int iAfeitrdofecharecep = dr.GetOrdinal(this.Afeitrdofecharecep);
            if (!dr.IsDBNull(iAfeitrdofecharecep)) entity.Afeitrdofecharecep = dr.GetDateTime(iAfeitrdofecharecep);

            int iAfeitrdofechaenvio = dr.GetOrdinal(this.Afeitrdofechaenvio);
            if (!dr.IsDBNull(iAfeitrdofechaenvio)) entity.Afeitrdofechaenvio = dr.GetDateTime(iAfeitrdofechaenvio);

            int iAfeitrdoestado = dr.GetOrdinal(this.Afeitrdoestado);
            if (!dr.IsDBNull(iAfeitrdoestado)) entity.Afeitrdoestado = dr.GetString(iAfeitrdoestado);

            int iAfeitrdjrfecharecep = dr.GetOrdinal(this.Afeitrdjrfecharecep);
            if (!dr.IsDBNull(iAfeitrdjrfecharecep)) entity.Afeitrdjrfecharecep = dr.GetDateTime(iAfeitrdjrfecharecep);

            int iAfeitrdjrfechaenvio = dr.GetOrdinal(this.Afeitrdjrfechaenvio);
            if (!dr.IsDBNull(iAfeitrdjrfechaenvio)) entity.Afeitrdjrfechaenvio = dr.GetDateTime(iAfeitrdjrfechaenvio);

            int iAfeitrdjrestado = dr.GetOrdinal(this.Afeitrdjrestado);
            if (!dr.IsDBNull(iAfeitrdjrestado)) entity.Afeitrdjrestado = dr.GetString(iAfeitrdjrestado);

            int iAfeitfechaelab = dr.GetOrdinal(this.Afeitfechaelab);
            if (!dr.IsDBNull(iAfeitfechaelab)) entity.Afeitfechaelab = dr.GetDateTime(iAfeitfechaelab);

            int iAfeitfechanominal = dr.GetOrdinal(this.Afeitfechanominal);
            if (!dr.IsDBNull(iAfeitfechanominal)) entity.Afeitfechanominal = dr.GetDateTime(iAfeitfechanominal);

            int iAferctaeobservacion = dr.GetOrdinal(this.Aferctaeobservacion);
            if (!dr.IsDBNull(iAferctaeobservacion)) entity.Aferctaeobservacion = dr.GetString(iAferctaeobservacion);

            int iAfeconvtiporeunion = dr.GetOrdinal(this.Afeconvtiporeunion);
            if (!dr.IsDBNull(iAfeconvtiporeunion)) entity.Afeconvtiporeunion = dr.GetString(iAfeconvtiporeunion);

            int iAfereuhoraprog = dr.GetOrdinal(this.Afereuhoraprog);
            if (!dr.IsDBNull(iAfereuhoraprog)) entity.Afereuhoraprog = dr.GetString(iAfereuhoraprog);

            int iAfereufechaprog = dr.GetOrdinal(this.Afereufechaprog);
            if (!dr.IsDBNull(iAfereufechaprog)) entity.Afereufechaprog = dr.GetDateTime(iAfereufechaprog);

            int iAfereufechanominal = dr.GetOrdinal(this.Afereufechanominal);
            if (!dr.IsDBNull(iAfereufechanominal)) entity.Afereufechanominal = dr.GetDateTime(iAfereufechanominal);

            int iAfecitfechaelab = dr.GetOrdinal(this.Afecitfechaelab);
            if (!dr.IsDBNull(iAfecitfechaelab)) entity.Afecitfechaelab = dr.GetDateTime(iAfecitfechaelab);

            int iAfecitfechanominal = dr.GetOrdinal(this.Afecitfechanominal);
            if (!dr.IsDBNull(iAfecitfechanominal)) entity.Afecitfechanominal = dr.GetDateTime(iAfecitfechanominal);

            int iAferesponsable = dr.GetOrdinal(this.Aferesponsable);
            if (!dr.IsDBNull(iAferesponsable)) entity.Aferesponsable = dr.GetString(iAferesponsable);

            int iAfeedagsf = dr.GetOrdinal(this.Afeedagsf);
            if (!dr.IsDBNull(iAfeedagsf)) entity.Afeedagsf = dr.GetString(iAfeedagsf);

            int iAfeplazofecmodificacion = dr.GetOrdinal(this.Afeplazofecmodificacion);
            if (!dr.IsDBNull(iAfeplazofecmodificacion)) entity.Afeplazofecmodificacion = dr.GetDateTime(iAfeplazofecmodificacion);

            int iAfeplazousumodificacion = dr.GetOrdinal(this.Afeplazousumodificacion);
            if (!dr.IsDBNull(iAfeplazousumodificacion)) entity.Afeplazousumodificacion = dr.GetString(iAfeplazousumodificacion);

            int iAfeplazofechaampl = dr.GetOrdinal(this.Afeplazofechaampl);
            if (!dr.IsDBNull(iAfeplazofechaampl)) entity.Afeplazofechaampl = dr.GetDateTime(iAfeplazofechaampl);

            int iAfeplazofecha = dr.GetOrdinal(this.Afeplazofecha);
            if (!dr.IsDBNull(iAfeplazofecha)) entity.Afeplazofecha = dr.GetDateTime(iAfeplazofecha);

            int iAfefechainterr = dr.GetOrdinal(this.Afefechainterr);
            if (!dr.IsDBNull(iAfefechainterr)) entity.Afefechainterr = dr.GetDateTime(iAfefechainterr);

            return entity;
        }


        #region Mapeo de Campos

        public string Aferacmt = "AFERACMT";
        public string Afeeracmf = "AFEERACMF";
        public string Afermc = "AFERMC";
        public string Afecorr = "AFECORR";
        public string Afeanio = "AFEANIO";
        public string Evencodi = "EVENCODI";
        public string Afecodi = "AFECODI";
        public string Afefzamayor = "AFEFZAMAYOR";
        public string Afeestadomotivo = "AFEESTADOMOTIVO";
        public string Afeestado = "AFEESTADO";
        public string Afeempcompninguna = "AFEEMPCOMPNINGUNA";
        public string Afeemprespninguna = "AFEEMPRESPNINGUNA";
        public string Afeitpitffecha = "AFEITPITFFECHA";
        public string Afefzadecisfechasist = "AFEFZADECISFECHASIST";
        public string Afefzafechasist = "AFEFZAFECHASIST";
        public string Afeitdecfechaelab = "AFEITDECFECHAELAB";
        public string Afeitdecfechanominal = "AFEITDECFECHANOMINAL";
        public string Afecompfechasist = "AFECOMPFECHASIST";
        public string Afecompfecha = "AFECOMPFECHA";
        public string Afeitpdecisffechasist = "AFEITPDECISFFECHASIST";
        public string Afeitpitffechasist = "AFEITPITFFECHASIST";
        public string Afeconvcitacionfecha = "AFECONVCITACIONFECHA";
        public string Aferctaeinformefecha = "AFERCTAEINFORMEFECHA";
        public string Aferctaeactafecha = "AFERCTAEACTAFECHA";
        public string Afeimpugna = "AFEIMPUGNA";
        public string Lastdate = "LASTDATE";
        public string Lastuser = "LASTUSER";
        public string Afeitrdofecharecep = "AFEITRDOFECHARECEP";
        public string Afeitrdofechaenvio = "AFEITRDOFECHAENVIO";
        public string Afeitrdoestado = "AFEITRDOESTADO";
        public string Afeitrdjrfecharecep = "AFEITRDJRFECHARECEP";
        public string Afeitrdjrfechaenvio = "AFEITRDJRFECHAENVIO";
        public string Afeitrdjrestado = "AFEITRDJRESTADO";
        public string Afeitfechaelab = "AFEITFECHAELAB";
        public string Afeitfechanominal = "AFEITFECHANOMINAL";
        public string Aferctaeobservacion = "AFERCTAEOBSERVACION";
        public string Afeconvtiporeunion = "AFECONVTIPOREUNION";
        public string Afereuhoraprog = "AFEREUHORAPROG";
        public string Afereufechaprog = "AFEREUFECHAPROG";
        public string Afereufechanominal = "AFEREUFECHANOMINAL";
        public string Afecitfechaelab = "AFECITFECHAELAB";
        public string Afecitfechanominal = "AFECITFECHANOMINAL";
        public string Aferesponsable = "AFERESPONSABLE";
        public string Afeedagsf = "AFEEDAGSF";
        public string Afeplazofecmodificacion = "AFEPLAZOFECMODIFICACION";
        public string Afeplazousumodificacion = "AFEPLAZOUSUMODIFICACION";
        public string Afeplazofechaampl = "AFEPLAZOFECHAAMPL";
        public string Afeplazofecha = "AFEPLAZOFECHA";
        public string Afefechainterr = "AFEFECHAINTERR";

        #endregion
    }
}
