﻿using COES.Dominio.DTO.Transferencias;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COES.MVC.Extranet.Areas.Transferencias.Models
{
    public class TransferenciaRetiroModel
    {
        public List<TransferenciaRetiroDTO> ListaTransferenciaRetiro { get; set; }
        public TransferenciaRetiroDTO Entidad { get; set; }
        public int idTransferenciaRetiro { get; set; }
    }
}