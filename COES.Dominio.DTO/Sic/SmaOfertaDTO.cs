using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla SMA_OFERTA
    /// </summary>
    public partial class SmaOfertaDTO : EntityBase
    {
        public int Ofertipo { get; set; }
        public DateTime? Oferfechainicio { get; set; }
        public DateTime? Oferfechafin { get; set; }
        public string Ofercodenvio { get; set; }
        public string Oferestado { get; set; }
        public string Oferusucreacion { get; set; }
        public DateTime? Oferfeccreacion { get; set; }
        public string Oferusumodificacion { get; set; }
        public DateTime? Oferfecmodificacion { get; set; }
        public DateTime? Oferfechaenvio { get; set; }
        public int Ofercodi { get; set; }
        public int Usercode { get; set; }
        public string Oferfuente { get; set; }
    }

    public partial class SmaOfertaDTO
    {
        public int Ofdecodi { get; set; }
        public string Ofdehorainicio { get; set; }
        public string Ofdehorafin { get; set; }
        public int Grupocodi { get; set; }
        public string OferlistMO { get; set; }
        public string OferlistMODes { get; set; }

        // REPORTE//
        public string Username { get; set; }
        public string Gruponomb { get; set; }
        public string Grupoabrev { get; set; }
        public int Grupocodincp { get; set; }
        public DateTime? Repofecha { get; set; }
        public int Repointvnum { get; set; } //Numero del Intervalo
        public int Repointvhini { get; set; } //Rango  30 mins
        public int Repointvmini { get; set; } //Rango  30 mins
        public int Repointvhfin { get; set; } // Rango 30 mins
        public int Repointvmfin { get; set; } // Rango 30 mins
        public string Repohoraini { get; set; } // Hora Inicio
        public string Repohorafin { get; set; } // Hora Fin
        public int Urscodi { get; set; }
        public string Ursnomb { get; set; }
        //public decimal Repopotmaxofer { get; set; } //CAMPO deprecated, usar BandaDisponible
        public decimal Repopotofer { get; set; }
        public string Repoprecio { get; set; }
        public string Repomoneda { get; set; }
        public int? Reponrounit { get; set; }
        public string Grupotipo { get; set; }
        public string Urstipo { get; set; }
        public string Emprnomb { get; set; }
        public int Emprcodi { get; set; }
        public int Ofdetipo { get; set; }
        public decimal BandaCalificada { get; set; }
        public decimal BandaDisponible { get; set; }

        public List<int> ListarUrscodi { get; set; }
        public List<SmaOfertaDetalleDTO> ListaDetalle { get; set; }
        //adicionales
        public string OferfechainicioDesc { get; set; }
        public string OferfechafinDesc { get; set; }
        public string RepopotoferDesc { get; set; }
        public string BandaCalificadaDesc { get; set; }
        public string OferfechaenvioDesc { get; set; }
        public int Rowspan { get; set; }
        public int Rowspan2 { get; set; }
        public bool TineAgrupFuentes { get; set; }
        public bool TineAgrupFuentes2 { get; set; }

        //Prueba para hacer Independent Copy 
        public SmaOfertaDTO Copy()
        {
            return (SmaOfertaDTO)this.MemberwiseClone();
        }

        public int? Smapaccodi { get; set; }
        public List<SmaOfertaDetalleDTO> ListaDetalles { get; set; }
    }
}
