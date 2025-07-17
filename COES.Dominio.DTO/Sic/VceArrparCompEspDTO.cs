// =======================================================================================
//
// (c) COES Sinac. Todos los derechos reservados. 
//
// Autor: Henry Manuel Díaz Tuesta
// Acronimo: HDT
// Requerimiento: compensaciones
//
// Fecha creacion: 29/03/2017
// Descripcion: Archivo para la atencion del requerimiento.
//
// Historial de cambios:
// 
// Correlativo	Fecha		Requerimiento		Comentario
//
// =======================================================================================


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.DTO.Sic
{

    public class VceArrparCompEspDTO
    {
        public int PecaCodi { get; set; }

        public int Grupocodi { get; set; }

        public DateTime Apespfecha { get; set; }
            
        public string Apstocodi { get; set; }

        public decimal? Apespcargafinal { get; set; }

        public decimal? Apespenergprod { get; set; }

        public decimal? Apesprendvigente { get; set; }

        public decimal? Apesppreciocomb { get; set; }

        public decimal? Apespcombbase { get; set; }

        public decimal? Apespcombrampa { get; set; }

        public decimal? Apespcombreconocxtransf { get; set; }

        public decimal? Apesppreciocombalt { get; set; }

        public decimal? Apespcombbasealt { get; set; }

        public decimal? Apespcombrampaalt { get; set; }

        public decimal? Apespcombreconocxtransfalt { get; set; }

        public decimal? Apespcompensacion { get; set; }

        //- Complementarios

        public string Gruponomb { get; set; }

        public string Aptopcodi { get; set; }

        public string Aptopnombre { get; set; }

        public string Apstonombre { get; set; }

        public string Apespfechadesc { get; set; }

        public string Apesptipodesc { get; set; }

    }

}
