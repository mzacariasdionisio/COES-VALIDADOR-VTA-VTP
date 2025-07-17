using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla GMM_EMPRESA
    /// </summary>
    public partial class GmmEmpresaDTO : EntityBase
    {
        public int EMPGCODI { get; set; }
        public DateTime? EMPGFECINGRESO { get; set; }
        public string EMPGTIPOPART { get; set; }
        public string EMPGESTADO { get; set; }
        public int EMPRCODI { get; set; }
        public string EMPGCOMENTARIO { get; set; }
        public string EMPGUSUCREACION { get; set; }
        public DateTime? EMPGFECCREACION { get; set; }
        public string EMPGUSUMODIFICACION { get; set; }
        public DateTime? EMPGFECMODIFICACION { get; set; }
        public int PERICODI { get; set; }
        public string EMPGFASECAL { get; set; }
        public string PERIESTADO { get; set; }
        public int TIPOEMPRCODI { get; set; }

    }

    public partial class GmmEmpresaDTO : EntityBase
    {
        // Tabla de resultados para las búsquedas de agentes
        public string Emprestado { get; set; }
        public string Emprnombrecomercial { get; set; }
        public string Emprtipoparticipante { get; set; }
        public DateTime? EmprFechaIngreso { get; set; }
        public string EmprmodalidadBusqueda { get; set; }
        public string EmprMontoGarantia { get; set; }
        public int Emprnumeincumplimientos { get; set; }

    }

    public partial class GmmEmpresaDTO : EntityBase
    {
        // Histórico de modalidades de la empresa 
        public int EmprGaraCodi { get; set; }
        public DateTime? EmprFechaInicio { get; set; }
        public DateTime? EmprFechaFin { get; set; }
        public string EmprModalidad { get; set; }
        public decimal EmprMonto { get; set; }
        public string EmprArchivo { get; set; }
        public int EmprTrienio { get; set; }
        public int EmprTotalIncM { get; set; }
        public string EmprCertifica { get; set; }

    }

    public partial class GmmEmpresaDTO : EntityBase
    {
        // Estados que ha tenido la empresa
        public DateTime? EmprFechaRegistro { get; set; }
        public string EmprEstado { get; set; }
        public string EmprUsuario { get; set; }
    }

    public partial class GmmEmpresaDTO : EntityBase
    {
        // Incumplimientos de la empresa
        public int EmprTriSecuencia { get; set; }
        public DateTime? EmprFecIniTrienio { get; set; }
        public DateTime? EmprFecFinTrienio { get; set; }
        public int EmprTotalInc { get; set; }
    }

    public partial class GmmEmpresaDTO : EntityBase
    {
        // Datos de la empresa para editar
        public int EmpCodiEdit { get; set; }
        public string EmpNombreEdit { get; set; }
        public string EmpRucEdit { get; set; }
        public DateTime? EmpFecIngEdit { get; set; }
        public string EmptpartEdit { get; set; }
        public string EmpestadoEdit { get; set; }
        public string EmpComentarioEdit { get; set; }
    }

    public partial class GmmEmpresaDTO : EntityBase
    {
        // Datos de la empresa para editar
        public bool flagcambioEstado { get; set; }
    }

    public partial class GmmEmpresaDTO : EntityBase
    {
            // Datos del maestro de empresas
            public string Emprrazsocial { get; set; }
            public string EmprRuc { get; set; }
    }

    public partial class GmmEmpresaDTO : EntityBase
    {
        // Campos para el cálculo de garantías
        public int PericodiCal { get; set; }
        public int EmprcodiCal { get; set; }
        public int EmpgcodiCal { get; set; }
        public bool EmpgPrimerMes { get; set; }
        //Garantías por Energía
        public decimal CMgCPt { get; set; }

        // Valores de energía
        public List<GmmValEnergiaDTO> gmmValEnergiaDTOs { get; set; }
        public List<GmmValEnergiaEntregaDTO> gmmValEnergiaEntregaDTOs { get; set; }

    }
}