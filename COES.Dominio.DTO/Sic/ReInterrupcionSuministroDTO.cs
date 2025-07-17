using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla RE_INTERRUPCION_SUMINISTRO
    /// </summary>
    public class ReInterrupcionSuministroDTO : EntityBase
    {
        public int Reintcodi { get; set; }
        public int? Repercodi { get; set; }
        public int? Reintpadre { get; set; }
        public string Reintfinal { get; set; }
        public int? Emprcodi { get; set; }
        public string Reintestado { get; set; }
        public string Reintmotivoanulacion { get; set; }
        public string Reintusueliminacion { get; set; }
        public DateTime? Reintfecanulacion { get; set; }
        public int? Reintcorrelativo { get; set; }
        public string Reinttipcliente { get; set; }
        public int? Reintcliente { get; set; }
        public int? Repentcodi { get; set; }
        public string Reintptoentrega { get; set; }
        public string Reintnrosuministro { get; set; }
        public int? Rentcodi { get; set; }
        public int? Reintaplicacionnumeral { get; set; }
        public decimal? Reintenergiasemestral { get; set; }
        public string Reintinctolerancia { get; set; }
        public int? Retintcodi { get; set; }
        public int? Recintcodi { get; set; }
        public decimal? Reintni { get; set; }
        public decimal? Reintki { get; set; }
        public DateTime? Reintfejeinicio { get; set; }
        public DateTime? Reintfejefin { get; set; }
        public DateTime? Reintfproginicio { get; set; }
        public DateTime? Reintfprogfin { get; set; }
        public string Reintcausaresumida { get; set; }
        public decimal? Reinteie { get; set; }
        public decimal? Reintresarcimiento { get; set; }
        public string Reintevidencia { get; set; }
        public string Reintdescontroversia { get; set; }
        public string Reintcomentario { get; set; }
        public string Reintusucreacion { get; set; }
        public DateTime? Reintfeccreacion { get; set; }
        public int? Emprcodi1 { get; set; }
        public int? Emprcodi2 { get; set; }
        public int? Emprcodi3 { get; set; }
        public int? Emprcodi4 { get; set; }
        public int? Emprcodi5 { get; set; }
        public decimal? Porcentaje1 { get; set; }
        public decimal? Porcentaje2 { get; set; }
        public decimal? Porcentaje3 { get; set; }
        public decimal? Porcentaje4 { get; set; }
        public decimal? Porcentaje5 { get; set; }
        public string Emprnomb { get; set; }
        public int? Emprresponsable { get; set; }

        //- Campos para consolidado

        public string Cliente { get; set; }
        public string NivelTension { get; set; }
        public string TipoInterrupcion { get; set; }
        public string CausaInterrupcion { get; set; }
        public int OrdenDetalle { get; set; }
        public string Responsable { get; set; }
        public int Reintdcodi { get; set; }
        public decimal? Reintdorcentaje { get; set; }
        public string Reintdconformidadresp { get; set; }
        public string Reintdobservacionresp { get; set; }
        public string Reintddetalleresp { get; set; }
        public string Reintdcomentarioresp { get; set; }
        public string Reintdevidenciaresp { get; set; }
        public string Reintdconformidadsumi { get; set; }
        public string Reintdcomentariosumi { get; set; }
        public string Reintdevidenciasumi { get; set; }
        public decimal Horasdiferencia { get; set; }

        public decimal Factorcalculado { get; set; }
        public decimal Resarcimientocalculado { get; set; }
        public string DiferenciaCalculo { get; set; }
        public string Reintreftrimestral { get; set; }

        public int CampoOrden { get; set; }

        public string Reintddispocision { get; set; }
        public string Reintdcompceo { get; set; }
        public int OrdenRegistro { get; set; }

    }
}
