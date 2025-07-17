namespace COES.Dominio.DTO.Enum
{
    public enum ETipoProcesoAuditoriaVTP : int
    {
        GestionCodigosEnvioInformacion = 1,
        GenerarReporteSaldosSobrantes = 3,
        BarrasDeTransferencia = 4,
        CargaInformacionVTPExtranet = 9,
        CargaInformacionVTPIntranet = 11,
        ProcesarValorizacionProcesar = 13,
        ProcesarValorizacionBorrar = 14
    }
}
