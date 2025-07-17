using COES.Dominio.DTO.Sic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.Eventos.Models
{
    public class EnviarCorreoModel
    {

        public List<EveSubcausaeventoDTO> ListaEvensubcausa { get; set; }
        public List<SiPersonaDTO> ListaProgramador { get; set; }
        public EveMailsDTO EveMail { get; set; }
        public int Accion { get; set; }
        public int Mailcodi { get; set; }
        public int Subcausacodi { get; set; }
        public int MailTurnoNum { get; set; }
        public string MailReprogCausa { get; set; }
        public string MailCheck1 { get; set; }
        public string MailHoja { get; set; }
        public string MailProgramador { get; set; }
        public int MailBloqueHorario { get; set; }
        public string MailFecha { get; set; }
        public string MailCheck2 { get; set; }
        public string MailEmitido { get; set; }
        public int Equicodi { get; set; }
        public string MailFechaIni { get; set; }
        public string MailFechaFin { get; set; }
        public string LastUserProc { get; set; }
        public string MailEspecialista { get; set; }
        public string EspecialistaSME { get; set; }
        public int MailTipoPrograma { get; set; }
        public int Lastuser { get; set; }
        public int Lastdate { get; set; }
        public string Equinomb { get; set; }
        public bool Grabar { get; set; }
        public string Mailhora { get; set; }
        public string Mailconsecuencia { get; set; }
        public string CoordinadorTurno { get; set; }

        //lista de reprogramas
        public List<CpReprogramaDTO> ListaReprogramas { get; set; }
        public int Topcodi { get; set; }
        public List<String> ListaCoordinadores { get; set; }
        public List<SiPersonaDTO> ListaEspecialistaSME { get; set; }
        
    }

    public class BusquedaEnviarCorreoModel
    {
        public List<EveMailsDTO> ListaEveMail { get; set; }
        public List<EveSubcausaeventoDTO> ListaEvensubcausa { get; set; }
        public string Fechaini { get; set; }
        public string Fechafin { get; set; }
        public int NroPaginas { get; set; }
        public int NroMostrar { get; set; }
        public bool IndicadorPagina { get; set; }
        public EveSubcausaeventoDTO eveSubcausa { get; set; }
        public int IdSubCausa { get; set; }
        public bool AccionNuevo { get; set; }
        public bool AccionEditar { get; set; }
        public bool AccionEliminar { get; set; }
    }


    public class FormatoCorreoModel
    {
        public string From { get; set; }
        public string To { get; set; }
        public string CC { get; set; }
        public string BCC { get; set; }
        public string Asunto { get; set; }

        public string ruta { get; set; }

        [AllowHtml]
        public string Contenido { get; set; }
        public int Plantcodi { get; set; }
        public string Archivo { get; set; }

        public string LinkArchivo { get; set; }
        public string pathAlternativo { get; set; }
    }
}