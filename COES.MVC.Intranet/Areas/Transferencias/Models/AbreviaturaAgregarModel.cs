using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace COES.MVC.Intranet.Areas.Transferencias.Models
{
    public class AbreviaturaAgregarModel
    {
        public int Emprcodi { get; set; }
        public string Emprnomb { get; set; }

        [Required]
        public string EmprAbrevCodi { get; set; }
    }
}