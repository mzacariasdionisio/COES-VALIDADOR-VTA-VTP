using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.DTO.Sic
{
    public class CrEventoDTO
    {
        public int CREVENCODI { get; set; }
        public int AFECODI { get; set; }
        public int CRESPECIALCODI { get; set; }
        public DateTime LASTDATE { get; set; }
        public string LASTUSER { get; set; }
        public int NroPagina { get; set; }
        public int NroRegistros { get; set; }

        //------------------------------------------------------

        public int COD_CRITERIO { get; set; }
        public string DESCRIPCION_EVE_EVENTO { get; set; }
        public DateTime? FECHA_EVE_EVENTO { get; set; }
        public string DESCRITERIO { get; set; }
        public string DESCASOESPECIAL { get; set; }
        public int CODIGOIMPUGNACION { get; set; }
        public DateTime AFEITDECFECHAELAB { get; set; }


        //filtro de la consulta
        public string EmpresaPropietaria { get; set; }
        public string EmpresaInvolucrada { get; set; }
        public string CriterioDecision { get; set; }
        public string CasosEspeciales { get; set; }
        public string Impugnacionc { get; set; }
        public string CriteriosImpugnacion { get; set; }
        public string DI { get; set; }
        public string DF { get; set; }

        
        //resultado de la consulta

        //EVENTO
        
        public string CODIGO { get; set; }  
        public string FECHA_EVENTO { get; set; }
        public string NOMBRE_EVENTO { get; set; }
        public string CASOS_ESPECIAL { get; set; }
        public string IMPUGNACION { get; set; }
        public string EVENDESCCTAF { get; set; }
        //ETAPA 1
        public string FECHA_DECISION { get; set; }
        public int CODIGO_ETAPA { get; set; }
        public int ETAPA { get; set; }
        public string DESCRIPCION_EVENTO_DECISION { get; set; }
        public string RESUMEN_DECISION { get; set; }
        public string RESPONSABLE_DECISION { get; set; }
        public string COMENTARIO_EMPRESA_DECISION { get; set; }
        public string CRITERIO_DECISION { get; set; }
        //ETAPA2
        public string EMPR_SOLI_RECONSIDERACION { get; set; }
        public string ARGUMENTO_RECONCIDERACION { get; set; }
        public string DECISION_RECONCIDERACION { get; set; }
        public string RESPONSABLE_RECONCIDERACION { get; set; }
        public string COMENTARIOS_RECONCIDERACION { get; set; }
        public string CRITERIOS_RECONSIDERACION { get; set; }
        //ETAPA3
        public string EMPR_SOLI_APELACION { get; set; }
        public string ARGUMENTO_APELACION { get; set; }
        public string DECISION_APELACION { get; set; }
        public string RESPONSABLE_APELACION { get; set; }
        public string COMENTARIOS_APELACION { get; set; }
        public string CRITERIOS_APELACION { get; set; }
        //ETAPA4
        public string EMPR_SOLI_ARBITRAJE { get; set; }
        public string ARGUMENTO_ARBITRAJE { get; set; }
        public string DECISION_ARBITRAJE { get; set; }
        public string RESPONSABLE_ARBITRAJE { get; set; }
        public string COMENTARIOS_ARBITRAJE { get; set; }
        public string CRITERIOS_ARBITRAJE { get; set; }

        public int CRETAPACODI { get; set; }
        public int EMPRCODI { get; set; }
        public string EMPRNOMB { get; set; }
        public string CRARGUMENTO { get; set; }
        public string CRDECISION { get; set; }

        public string CRCRITERIOCODIDESICION { get; set; }
        public string CRCRITERIOCODIRECONSIDERACION { get; set; }
        public string CRCRITERIOCODIAPELACION { get; set; }
        public string CRCRITERIOCODIARBITRAJE { get; set; }
        
    }

    public class Responsables {
        public int CREVENCODI { get; set; }
        public int CRETAPACODI { get; set; }
        public int EMPRCODI { get; set; }
        public string EMPRNOMB { get; set; }
    }
    public class Solicitantes {
        public int CREVENCODI { get; set; }
        public int CRETAPACODI { get; set; }
        public int EMPRCODI { get; set; }
        public string EMPRNOMB { get; set; }
        public string CRARGUMENTO { get; set; }
        public string CRDECISION { get; set; }
    }
}
