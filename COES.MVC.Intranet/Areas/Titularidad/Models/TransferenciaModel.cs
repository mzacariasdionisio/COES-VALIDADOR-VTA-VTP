using COES.Dominio.DTO.Sic;
using COES.Servicios.Aplicacion.Titularidad;
using System.Collections.Generic;

namespace COES.MVC.Intranet.Areas.Titularidad.Models
{
    public class TransferenciaModel
    {
        public List<SiMigracionDTO> ListadoTransferencias { get; set; }
        public List<SiEmpresaDTO> ListaEmpresaOrigen { get; set; }
        public int IndiceEmpresaOrigen { get; set; }
        public List<SiEmpresaDTO> ListaEmpresaDestino { get; set; }
        public int IndiceEmpresaDestino { get; set; }
        public List<SiTipoempresaDTO> ListaTipoEmpresa { get; set; }
        public int IndiceTipoEmpresa { get; set; }
        public string Descripcion { get; set; }
        public List<SiMigraemprOrigenDTO> ListaTransferencias { get; set; }
        public string EnableNuevo { get; set; }
        public string EnableEditar { get; set; }
        public List<SiEmpresaDTO> ListaEmpresaOrigenAnidada { get; set; }
        public int IndiceEmpresaOrigenAnidada { get; set; }
        public string Fecha { get; set; }

        public SiEmpresaDTO Empresa { get; set; }

        public List<SiTipomigraoperacionDTO> ListaTipoOperacion { get; set; }
        public List<SiEmpresaDTO> ListaEmpresaAnidada { get; set; }

        public SiMigracionDTO Migracion { get; set; }
        public List<EqAreaDTO> ListaArea { get; set; }
        public List<EqEquipoDTO> ListaEquipo { get; set; }
        public List<EqFamiliaDTO> ListaFamilia { get; set; }
        public List<MeOrigenlecturaDTO> ListaOrigLectura { get; set; }
        public List<MePtomedicionDTO> ListaPtoMedicion { get; set; }
        public List<PrGrupoDTO> ListaGrupo { get; set; }
        public List<SiLogDTO> ListaLog { get; set; }
        public List<TTIEDetalleAdicional> ListaDetalleAdicional { get; set; }

        public List<SiMigralogdbaDTO> ListQuerysEjecutDba { get; set; }

        public SiMigraquerybaseDTO Query { get; set; }
        public SiMigraqueryplantillaDTO Plantilla { get; set; }
        public List<SiMigraParametroDTO> ListaParametro { get; set; }
        public List<SiMigraqueryplantillaDTO> ListaPlantilla { get; set; }
        public SiMigraParametroDTO Parametro { get; set; }

        public bool EsRegQueAnulaOtroReg { get; set; }
        public bool TieneFechaCorte { get; set; }
        public bool TieneProcesoStrPendiente { get; set; }
        public bool TienePermisoLogDBA { get; set; }

        public int Migracodi { get; set; }
        public string Mensaje { get; set; }
        public string Resultado { get; set; }
        public string Resultado2 { get; set; }
        public string Resultado3 { get; set; }
        public string Detalle { get; set; }

        //Permisos
        public bool AccionNuevo { get; set; }
    }

    public class TransferenciaParametroModel
    {
        public int Tmopercodi { get; set; }
        public int EmprcodiOrigen { get; set; }
        public int Emprcodi { get; set; }
        public string StrFechaCorte { get; set; }
        public string Descripcion { get; set; }
        public List<int> ListaEquicodi { get; set; }
        public List<int> ListaPtomedicodi { get; set; }
        public List<int> ListaGrupocodi { get; set; }
        public bool RegHistoricoTransf { get; set; }
        public int RegStrTransf { get; set; }
        //ASSETEC 202108 TIEE
        public int RegPM { get; set; }
    }

}