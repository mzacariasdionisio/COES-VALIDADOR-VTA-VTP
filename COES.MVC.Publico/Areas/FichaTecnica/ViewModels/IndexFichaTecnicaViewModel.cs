using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using COES.Dominio.DTO.Sic;

namespace COES.MVC.Publico.Areas.FichaTecnica.ViewModels
{
    public class IndexFichaTecnicaViewModel
    {
        public IList<EqFamiliaDTO> TipoCentrales = new List<EqFamiliaDTO>();
        public int CodigoTipoEquipo;
        public int CodigoTipoEquipo2;
        public string CodEmpresa;
        public IList<SiEmpresaDTO> Empresas = new List<SiEmpresaDTO>();
        public string NombreEquipo;
    }
}