using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.DTO.Transferencias
{
    public class ReporteLogDTO
    {
        public string NombreBarra;
        public int Dia;
        public decimal ValorCostoMarginal;
        //ASSETEC 202002
        public List<string> ListaFecFaltantes;

    }
}

