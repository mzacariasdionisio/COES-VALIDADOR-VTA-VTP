using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COES.Dominio.DTO.Sic;

namespace COES.Servicios.Aplicacion.Indisponibilidades.Helper
{
    public class IndFormatoExcel
    {
        public string[][] DatosGenerales { get; set; }
        public string EmpresaNombre { get; set; }
        public string TituloCuadroA1 { get; set; }
        public string TituloCuadroA2 { get; set; }
        public string Subtitulo1 { get; set; }
        public string NombreLibroCuadroA1 { get; set; }
        public string NombreLibroCuadroA2 { get; set; }
        public int NumeroRegistros { get; set; }
        public int DiasPerido { get; set; }
        public List<string[]> Reservada { get; set; }
        public List<string[]> Transportada { get; set; }
        public List<string[]> Contratada { get; set; }
        public List<string[]> Cuadro2CTG { get; set; }
        public List<IndExcelHeader> NestedHeader1 { get; set; }
        public List<IndExcelHeader> NestedHeader2 { get; set; }
        public List<IndExcelHeader> NestedHeader3 { get; set; }
        public List<IndExcelHeader> NestedHeader4 { get; set; }
        public List<IndExcelHeader> NestedHeader5 { get; set; }
        public List<IndExcelHeader> NestedHeader6 { get; set; }
        public List<IndExcelHeader> NestedHeader7 { get; set; }
        public List<IndExcelHeader> NestedHeader8 { get; set; }
        public List<IndExcelHeader> NestedHeader9 { get; set; }
        public List<IndExcelHeader> NestedHeader10 { get; set; }
        public string[][] ColorByCells { get; set; }
        public List<IndRelacionEmpresaDTO> HabilitadasCuadro1 { get; set; }
        public string FechaInicioCDU { get; set; }
        public string FechaFinCDU { get; set; }
        public string FechaInicioCCD { get; set; }
        public string FechaFinCCD { get; set; }
        public bool HabilitadoCuadro1 { get; set; }
        public bool HabilitadoCuadro2 { get; set; }

        //adicionales
        public string Titulo { get; set; }
        public string Subtitulo2 { get; set; }
        public string[] Cabecera1 { get; set; }
        public string[] Cabecera2 { get; set; }
        public string[] Contenido1 { get; set; }
        public string[] Contenido2 { get; set; }
        public int[] AnchoColumnas1 { get; set; }
        public int[] AnchoColumnas2 { get; set; }
        public string NombreLibro { get; set; }
    }

    public class IndExcelHeader
    {
        public int Columnas { get; set; }
        public string Etiqueta { get; set; }
    }

    public class IndValidacionFormato
    {
        public string Cuadro { get; set; }
        public string TipoCuadro { get; set; }
        public string Descripcion { get; set; }
    }

    public class IndExcelGeneralData
    {
        public string Empresa { get; set; }
        public string Anio { get; set; }
        public string Mes { get; set; }
        public string Centrales { get; set; }
    }
}
