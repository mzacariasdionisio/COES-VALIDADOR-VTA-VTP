
using COES.Dominio.DTO.Campania;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace COES.MVC.Extranet.Areas.Campanias.Models
{
    public class ProyectoModel
    {
        public int ProyCodi { get; set; }

        public int Secccodi { get; set; }
        public TransmisionProyectoDTO TransmisionProyectoDTO { get; set; }
        public int CodPlanTransmision { get; set; }
        public string Comentarios { get; set; }
        public string Modo { get; set; }
        public RegHojaADTO RegHojaADTO {  get; set; }
        public RegHojaBDTO RegHojaBDTO { get; set; }
        public RegHojaCDTO RegHojaCDTO { get; set; }
        public List<DetRegHojaCDTO> DetRegHojaCDTO { get; set; }

        public List<RegHojaDDTO> ListRegHojaD { get; set; }

        public List<TransmisionProyectoDTO> listaProyecto {  get; set; }

        public RegHojaCCTTADTO RegHojaCCTTADTO { get; set; }
        public RegHojaCCTTBDTO RegHojaCCTTBDTO { get; set; }
        public RegHojaCCTTCDTO RegHojaCCTTCDTO { get; set; }
        public List<Det1RegHojaCCTTCDTO> Det1RegHojaCCTTCDTO { get; set; }
        public List<Det2RegHojaCCTTCDTO> Det2RegHojaCCTTCDTO { get; set; }

        public RegHojaASubestDTO RegHojaASubestDTO { get; set; }
        public List<DetRegHojaASubestDTO> DetRegHojaASubestDTO { get; set; }
        public RegHojaEolADTO RegHojaEolADTO { get; set; }
        public List<RegHojaEolADetDTO> RegHojaEolADetDTOs { get; set; }
        public RegHojaEolBDTO RegHojaEolBDTO { get; set; }
        public RegHojaEolCDTO RegHojaEolCDTO { get; set; }
        public List<DetRegHojaEolCDTO> DetRegHojaEolCDTO { get; set; }        
        public SolHojaADTO SolHojaADTO { get; set; }
        public SolHojaBDTO SolHojaBDTO { get; set; }

        public SolHojaCDTO SolHojaCDTO { get; set; }    

        public List<DetSolHojaCDTO> ListaDetSolHojaCDTO { get; set; }

        public BioHojaADTO BioHojaADTO { get; set; }
        public BioHojaBDTO BioHojaBDTO { get; set; }

        public BioHojaCDTO BioHojaCDTO { get; set; }

        public List<DetBioHojaCDTO> ListaDetBioHojaCDTO { get; set; }

        public ITCFE01DTO ITCFE01DTO { get; set; }

        public List<Itcdf104DTO> Itcdf104DTOs { get; set; }
        public List<Itcdf108DTO> Itcdf108DTOs { get; set; }
        public List<Itcdf110DTO> Itcdf110DTOs { get; set; }
        public List<Itcdf116DTO> Itcdf116DTOs { get; set; }
        public List<Itcdf121DTO> Itcdf121DTOs { get; set; }
        public List<Itcdf123DTO> Itcdf123DTOs { get; set; }
        public Itcdfp011DTO Itcdfp011DTO { get; set; }
        public List<Itcdfp012DTO> Itcdfp012DTOs { get; set; }
        public List<Itcdfp013DTO> Itcdfp013DTOs { get; set; }
        public List<Itcdf116DetDTO> ListaItcdf116DetDTO { get; set; }
        public List<Itcdf121DetDTO> ListaItcdf121DetDTO { get; set; }
        public List<Itcdfp011DetDTO> ListaItcdfp011DetDTO { get; set; }
        public List<Itcdfp013DetDTO> ListaItcdfp013DetDTO { get; set; }

        public List<ItcPrm1Dto> ListaItcPrm1DTO { get; set; }

        public List<ItcPrm2Dto> ListaItcPrm2DTO { get; set; }

        public List<ItcRed1Dto> ListaItcRed1DTO { get; set; }

        public List<ItcRed2Dto> ListaItcRed2DTO { get; set; }

        public List<ItcRed3Dto> ListaItcRed3DTO { get; set; }

        public List<ItcRed4Dto> ListaItcRed4DTO { get; set; }

        public List<ItcRed5Dto> ListaItcRed5DTO { get; set; }
        public CuestionarioH2VADTO CuestionarioH2VADTO { get; set; }
        public List<CuestionarioH2VADet1DTO> ListaCH2VADet1DTOs { get; set; }   
        public List<CuestionarioH2VADet2DTO> ListaCH2VADet2DTOs { get; set; }

        public CuestionarioH2VBDTO CuestionarioH2VBDTO { get; set; }
        public CuestionarioH2VFDTO cuestionarioH2VFDTO { get; set; }

        public CCGDADTO CcgdaDTO { get; set; }

        public List<CCGDBDTO> Ccgdbdtos { get; set; }
        public List<CCGDDDTO> Ccgdddtos { get; set; }

        public List<CCGDCOptDTO> CCGDCOptDTOs { get; set; }

        public List<CCGDCPesDTO> CCGDCPesDTOs { get; set; }

        public CCGDEDTO CcgdeDTO { get; set; }

        public List<CCGDFDTO> CcgdfDTOs { get; set; }

        public CuestionarioH2VFDTO Ch2vfDTO {  get; set; }

        public List<CuestionarioH2VCDTO> Ch2vcDTOs { get; set; }

        public List<CuestionarioH2VGDTO> Ch2vgDTOs { get; set; }

        public List<CuestionarioH2VEDTO> Ch2veDTOs { get; set; }

        public CroFicha1DTO CroFicha1DTO { get; set; }

        public List<CroFicha1DetDTO> ListaCroFicha1DetDTO { get; set; }

        public LineasFichaADTO lineasFichaADTO { get; set; }
        public List<LineasFichaADet1DTO> listaLineasFichaADet1DTO { get; set; }
        public List<LineasFichaADet2DTO> listaLineasFichaADet2DTO { get; set; }
        public LineasFichaBDTO LineasFichaBDTO { get; set; }
        public List<LineasFichaBDetDTO> ListLineasFichaBDetDTO { get; set; }

        public SubestFicha1DTO subestFicha1DTO { get; set; }
        public List<SubestFicha1Det1DTO> listaSubestFicha1Det1DTO { get; set; }
        public List<SubestFicha1Det2DTO> listaSubestFicha1Det2DTO { get; set; }
        public List<SubestFicha1Det3DTO> listaSubestFicha1Det3DTO { get; set; }


        public T2SubestFicha1DTO t2SubestFicha1DTO { get; set; }
        public List<T2SubestFicha1Det1DTO> listaT2SubestFicha1Det1DTO { get; set; }
        public List<T2SubestFicha1Det2DTO> listaT2SubestFicha1Det2DTO { get; set; }
        public List<T2SubestFicha1Det3DTO> listaT2SubestFicha1Det3DTO { get; set; }
        public List<FormatoD1DDTO> listaFormatoDs {  get; set; }

        public FormatoD1CDTO FormatoD1CDTO { get; set; } 

        public List<FormatoD1CDetDTO> ListaFormatoDCDet1 {  get; set; }

        public FormatoD1BDTO FormatoD1BDTO  { get; set; }

        public List<FormatoD1BDetDTO> ListaFormatoDet1B {  get; set; }

        public FormatoD1ADTO FormatoD1ADTO { get; set; }

        public List<FormatoD1ADet1DTO> ListaFormatoDet1A { get; set; }
        public List<FormatoD1ADet2DTO> ListaFormatoDet2A { get; set; }
        public List<FormatoD1ADet3DTO> ListaFormatoDet3A { get; set; }
        public List<FormatoD1ADet4DTO> ListaFormatoDet4A { get; set; }
        public List<FormatoD1ADet5DTO> ListaFormatoDet5A { get; set; }

        public T1LinFichaADTO t1LinFichaADTO { get; set; }


        public T1LinFichaADTO t1lineasFichaADTO { get; set; }
        public List<T1LinFichaADet1DTO> listaT1LineasFichaADet1DTO { get; set; }
        public List<T1LinFichaADet2DTO> listaT1LineasFichaADet2DTO { get; set; }

        public RespuestaObsDTO respuestaObsDTO { get; set; }
        public int CodPlanTransmisionN { get; set; }
    }
}