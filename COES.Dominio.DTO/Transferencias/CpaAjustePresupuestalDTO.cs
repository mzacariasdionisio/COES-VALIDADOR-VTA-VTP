using System;

/// <summary>
/// Clase que mapea la tabla CPA_AJUSTEPRESUPUESTAL
/// </summary>
public class CpaAjustePresupuestalDTO
{
    public int Cpaapcodi { get; set; }        // Mapeo de CPAAPCODI
    public int Cpaapanio { get; set; }        // Mapeo de CPAAPANIO
    public string Cpaapajuste { get; set; }   // Mapeo de CPAAPAJUSTE
    public int Cpaapanioejercicio { get; set; } // Mapeo de CPAAPANIOEJERCICIO
    public string Cpaapusucreacion { get; set; } // Mapeo de CPAAPUSUCREACION
    public DateTime Cpaapfeccreacion { get; set; } // Mapeo de CPAAPFECCREACION
}
