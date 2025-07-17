using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla AGC_CONTROL
    /// </summary>
    public class AnalisisFallaDTO : EntityBase
    {
        public int AFECODI { get; set; }
        public int AFRREC { get; set; }
        public int EVENCODI { get; set; }
        public int AFEANIO { get; set; }
        public int AFECORR { get; set; }
        public string AFERMC { get; set; }
        public string ANIO { get; set; }
        public string AFERMCchk
        {
            get
            {
                if (AFERMC == "S")
                {
                    return "checked";
                }
                return "";
            }
        }
        public string AFEERACMF { get; set; }
        public string AFEERACMFchk
        {
            get
            {
                if (AFEERACMF == "S")
                {
                    return "checked";
                }
                return "";
            }
        }

        public string AFERACMT { get; set; }
        public string AFERACMTchk
        {
            get
            {
                if (AFERACMT == "S")
                {
                    return "checked";
                }
                return "";
            }
        }
        public string AFEEDAGSF { get; set; }
        public string AFEEDAGSFchk
        {
            get
            {
                if (AFERACMT == "S")
                {
                    return "checked";
                }
                return "";
            }
        }
        public string AFERESPONSABLE { get; set; }
        public DateTime? AFECITFECHANOMINAL { get; set; }

        public string AFECITFECHANOMINALstr
        {
            get
            {
                if (AFECITFECHANOMINAL.HasValue)
                {
                    return AFECITFECHANOMINAL.Value.ToString("dd/MM/yyyy");
                }
                return "";
            }
        }

        public DateTime? AFECITFECHAELAB { get; set; }
        public string AFECITFECHAELABstr
        {
            get
            {
                if (AFECITFECHAELAB.HasValue)
                {
                    return AFECITFECHAELAB.Value.ToString("dd/MM/yyyy");
                }
                return "";
            }
        }

        public string AFECITFECHAELABU { get; set; }

        public DateTime? AFEREUFECHANOMINAL { get; set; }
        public string AFEREUFECHANOMINALstr
        {
            get
            {
                if (AFEREUFECHANOMINAL.HasValue)
                {
                    return AFEREUFECHANOMINAL.Value.ToString("dd/MM/yyyy");
                }
                return "";
            }
        }

        public DateTime? AFEREUFECHAPROG { get; set; }
        public string AFEREUFECHAPROGstr
        {
            get
            {
                if (AFEREUFECHAPROG.HasValue)
                {
                    return AFEREUFECHAPROG.Value.ToString("dd/MM/yyyy");
                }
                return "";
            }
        }
        public string AFEREUFECHAPROGU { get; set; }
        public string AFEREUHORAPROG { get; set; }
        public string AFECONVTIPOREUNION { get; set; }
        public string AFERCTAEOBSERVACION { get; set; }
        public DateTime? AFEITFECHANOMINAL { get; set; }
        public string AFEITFECHANOMINALstr
        {
            get
            {
                if (AFEITFECHANOMINAL.HasValue)
                {
                    return AFEITFECHANOMINAL.Value.ToString("dd/MM/yyyy");
                }
                return "";
            }
        }

        public DateTime? AFEITFECHAELAB { get; set; }
        public string AFEITFECHAELABU { get; set; }
        public string AFEITFECHAELABstr
        {
            get
            {
                if (AFEITFECHAELAB.HasValue)
                {
                    return AFEITFECHAELAB.Value.ToString("dd/MM/yyyy");
                }
                return "";
            }
        }

        public string AFEITRDJRESTADO { get; set; }
        public DateTime? AFEITRDJRFECHAENVIO { get; set; }
        public string AFEITRDJRFECHAENVIOU { get; set; }
        public string AFEITRDJRFECHAENVIOstr
        {
            get
            {
                if (AFEITRDJRFECHAENVIO.HasValue)
                {
                    return AFEITRDJRFECHAENVIO.Value.ToString("dd/MM/yyyy");
                }
                return "";
            }
        }

        public DateTime? AFEITRDJRFECHARECEP { get; set; }
        public string AFEITRDJRFECHARECEPU { get; set; }
        public string AFEITRDJRFECHARECEPstr
        {
            get
            {
                if (AFEITRDJRFECHARECEP.HasValue)
                {
                    return AFEITRDJRFECHARECEP.Value.ToString("dd/MM/yyyy");
                }
                return "";
            }
        }

        public string AFEITRDOESTADO { get; set; }
        public DateTime? AFEITRDOFECHAENVIO { get; set; }
        public string AFEITRDOFECHAENVIOU { get; set; }
        public string AFEITRDOFECHAENVIOstr
        {
            get
            {
                if (AFEITRDOFECHAENVIO.HasValue)
                {
                    return AFEITRDOFECHAENVIO.Value.ToString("dd/MM/yyyy");
                }
                return "";
            }
        }

        public DateTime? AFEITRDOFECHARECEP { get; set; }
        public string AFEITRDOFECHARECEPU { get; set; }
        public string AFEITRDOFECHARECEPstr
        {
            get
            {
                if (AFEITRDOFECHARECEP.HasValue)
                {
                    return AFEITRDOFECHARECEP.Value.ToString("dd/MM/yyyy");
                }
                return "";
            }
        }

        public string LASTUSER { get; set; }
        public DateTime? LASTDATE { get; set; }
        public string LASTDATEU { get; set; }
        public string LASTDATEstr
        {
            get
            {
                if (LASTDATE.HasValue)
                {
                    return LASTDATE.Value.ToString("dd/MM/yyyy HH:mm:ss");
                }
                return "";
            }
        }

        public string AFEIMPUGNA { get; set; }
        public DateTime? AFERCTAEACTAFECHA { get; set; }
        public string AFERCTAEACTAFECHAstr
        {
            get
            {
                if (AFERCTAEACTAFECHA.HasValue)
                {
                    return AFERCTAEACTAFECHA.Value.ToString("dd/MM/yyyy hh:mm:ss");
                }
                return "";
            }
        }

        public DateTime? AFERCTAEINFORMEFECHA { get; set; }
        public string AFERCTAEINFORMEFECHAstr
        {
            get
            {
                if (AFERCTAEINFORMEFECHA.HasValue)
                {
                    return AFERCTAEINFORMEFECHA.Value.ToString("dd/MM/yyyy hh:mm:ss");
                }
                return "";
            }
        }

        public DateTime? AFECONVCITACIONFECHA { get; set; }
        public string AFECONVCITACIONFECHAstr
        {
            get
            {
                if (AFECONVCITACIONFECHA.HasValue)
                {
                    return AFECONVCITACIONFECHA.Value.ToString("dd/MM/yyyy hh:mm:ss");
                }
                return "";
            }
        }

        public DateTime? AFEITPITFFECHASIST { get; set; }
        public string AFEITPITFFECHASISTstr
        {
            get
            {
                if (AFEITPITFFECHASIST.HasValue)
                {
                    return AFEITPITFFECHASIST.Value.ToString("dd/MM/yyyy hh:mm:ss");
                }
                return "";
            }
        }

        public string AFEITPITFFECHASISTFecha { get; set; }
        public string AFEITPITFFECHAFecha { get; set; }

        public DateTime? AFEITPDECISFFECHASIST { get; set; }
        public string AFEITPDECISFFECHASISTstr
        {
            get
            {
                if (AFEITPDECISFFECHASIST.HasValue)
                {
                    return AFEITPDECISFFECHASIST.Value.ToString("dd/MM/yyyy hh:mm:ss");
                }
                return "";
            }
        }

        public DateTime? AFECOMPFECHA { get; set; }
        public string AFECOMPFECHAstr
        {
            get
            {
                if (AFECOMPFECHA.HasValue)
                {
                    return AFECOMPFECHA.Value.ToString("dd/MM/yyyy");
                }
                return "";
            }
        }

        public DateTime? AFECOMPFECHASIST { get; set; }
        public string AFECOMPFECHASISTstr
        {
            get
            {
                if (AFECOMPFECHASIST.HasValue)
                {
                    return AFECOMPFECHASIST.Value.ToString("dd/MM/yyyy hh:mm:ss");
                }
                return "";
            }
        }

        public DateTime? AFEITDECFECHANOMINAL { get; set; }
        public string AFEITDECFECHANOMINALU { get; set; }
        public string AFEITDECFECHANOMINALstr
        {
            get
            {
                if (AFEITDECFECHANOMINAL.HasValue)
                {
                    return AFEITDECFECHANOMINAL.Value.ToString("dd/MM/yyyy");
                }
                return "";
            }
        }

        public DateTime? AFEITDECFECHAELAB { get; set; }
        public string AFEITDECFECHAELABU { get; set; }
        public string AFEITDECFECHAELABstr
        {
            get
            {
                if (AFEITDECFECHAELAB.HasValue)
                {
                    return AFEITDECFECHAELAB.Value.ToString("dd/MM/yyyy");
                }
                return "";
            }
        }

        public DateTime? AFEFZAFECHASIST { get; set; }
        public string AFEFZAFECHASISTstr
        {
            get
            {
                if (AFEFZAFECHASIST.HasValue)
                {
                    return AFEFZAFECHASIST.Value.ToString("dd/MM/yyyy");
                }
                return "";
            }
        }

        public DateTime? AFEFZADECISFECHASIST { get; set; }
        public string AFEFZADECISFECHASISTstr
        {
            get
            {
                if (AFEFZADECISFECHASIST.HasValue)
                {
                    return AFEFZADECISFECHASIST.Value.ToString("dd/MM/yyyy");
                }
                return "";
            }
        }

        public DateTime? AFEITPITFFECHA { get; set; }
        public string AFEITPITFFECHAstr
        {
            get
            {
                if (AFEITPITFFECHA.HasValue)
                {
                    return AFEITPITFFECHA.Value.ToString("dd/MM/yyyy");
                }
                return "";
            }
        }

        public string AFEEMPRESPNINGUNA { get; set; }
        public string AFEEMPCOMPNINGUNA { get; set; }
        public string AFEESTADO { get; set; }
        public string AFEESTADOMOTIVO { get; set; }
        public string AFEFZAMAYOR { get; set; }

        public string EVENASUNTO { get; set; }
        public string EMPRNOMB { get; set; }
        public string AFRRECOMEND { get; set; }
        public DateTime? AFRPUBLICAFECHA { get; set; }
        public string AFRPUBLICAFECHAU { get; set; }
        public DateTime? AFRMEDADOPFECHA { get; set; }

        public string AFRMEDADOPFECHAU { get; set; }

        public string AFRMEDADOPFECHAstr
        {
            get
            {
                if (AFRMEDADOPFECHA.HasValue)
                {
                    return AFRMEDADOPFECHA.Value.ToString("dd/MM/yyyy");
                }
                return "";
            }
        }
        public string AFRMEDADOPMEDIDA { get; set; }
        public string AFRMEDADOPNIVCUMP { get; set; }
        public string LASTUSERRPTA { get; set; }
        public DateTime? LASTDATERPTA { get; set; }
        public string INDIMPORTANTE { get; set; }
        public string NROREGRESPUESTA { get; set; }
        public string CODIGO { get; set; }        
        public string FECHA_EVENTO { get; set; }       
        public DateTime? EVENINI { get; set; }

        public string CodigoEvento { get; set; }

        //AFECODI,EVENCODI,AFEANIO,AFECORR,AFERMC,AFEERACMF,AFERACMT,AFEEDAGSF,AFERESPONSABLE


        //, AFECITFECHANOMINAL,AFECITFECHAELAB,AFEREUFECHANOMINAL
        //,AFEREUFECHAPROG,AFEREUHORAPROG,AFECONVTIPOREUNION
        //,AFERCTAEOBSERVACION
        //,AFEITFECHANOMINAL,AFEITFECHAELAB,AFEITRDJRESTADO,AFEITRDJRFECHAENVIO


        //  ,AFEITRDJRFECHARECEP,AFEITRDOESTADO
        //,AFEITRDOFECHAENVIO,AFEITRDOFECHARECEP
        //,LASTUSER,LASTDATE,AFEIMPUGNA

        //,AFERCTAEACTAFECHA
        //,AFERCTAEINFORMEFECHA--OK
        //,AFECONVCITACIONFECHA,AFEITPITFFECHASIST
        //,AFEITPDECISFFECHASIST
        //,AFECOMPFECHA,AFECOMPFECHASIST
        //,AFEITDECFECHANOMINAL,AFEITDECFECHAELAB

        //,AFERCTAEACTAFECHA
        //,AFERCTAEINFORMEFECHA--OK
        //,AFECONVCITACIONFECHA,AFEITPITFFECHASIST
        //,AFEITPDECISFFECHASIST
        //,AFECOMPFECHA,AFECOMPFECHASIST
        //,AFEITDECFECHANOMINAL,AFEITDECFECHAELAB

        //,AFEFZAFECHASIST
        //,AFEFZADECISFECHASIST,AFEITPITFFECHA
        //,AFEEMPRESPNINGUNA,AFEEMPCOMPNINGUNA
        //,AFEESTADO,AFEESTADOMOTIVO,AFEFZAMAYOR
        public int AFECODISCO { get; set; }
        public int AFSALA { get; set; }
        public string EVENDESCCTAF { get; set; }
        public string AFEREUHORINI { get; set; }
        public string AFEREUHORFIN { get; set; }

        public string AFELIMATENCOMENU { get; set; }
        public DateTime? AFELIMATENCOMEN { get; set; }
        public string AFELIMATENCOMENstr
        {
            get
            {
                if (AFELIMATENCOMEN.HasValue)
                {
                    return AFELIMATENCOMEN.Value.ToString("dd/MM/yyyy");
                }
                return "";
            }
        }
    }

    public class ReporteSemanalItemDTO
    {
        public int AFECODI { get; set; }
        public string CODIGO { get; set; }
        public string FECHA { get; set; }
    }
}
