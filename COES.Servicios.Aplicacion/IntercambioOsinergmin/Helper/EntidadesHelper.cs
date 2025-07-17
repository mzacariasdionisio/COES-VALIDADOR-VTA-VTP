using System;
using System.Collections.Generic;

namespace COES.Servicios.Aplicacion.IntercambioOsinergmin.Helper
{
    public class EntidadesHelper
    {

        public static string GetFamiliaMaestro(EntidadSincroniza entidad)
        {
            if (!Enum.IsDefined(typeof(EntidadSincroniza), entidad)) throw new ArgumentOutOfRangeException("entidad");

            switch (entidad)
            {
                case EntidadSincroniza.Barra:
                    return ConstantesIio.FamNombBarra;
                case EntidadSincroniza.CentralGeneracion:
                    return ConstantesIio.FamNombCentral;
                case EntidadSincroniza.GrupoGeneracion:
                    return ConstantesIio.FamNombGenerador;
                case EntidadSincroniza.ModoOperacion:
                    return ConstantesIio.FamNombModoOperacion;
                case EntidadSincroniza.Cuenca:
                    return ConstantesIio.FamNombCuenca;
                case EntidadSincroniza.Embalse:
                    return ConstantesIio.FamNombEmbalse;
                case EntidadSincroniza.Lago:
                    return ConstantesIio.FamNombLago;

                default:
                    throw new ArgumentOutOfRangeException("entidad", entidad, null);
            }
        }

        public static string GetEntidadDisplayName(EntidadSincroniza entidad)
        {
            if (!Enum.IsDefined(typeof(EntidadSincroniza), entidad)) throw new ArgumentOutOfRangeException("entidad");

            string nombre;

            switch (entidad)
            {
                case EntidadSincroniza.Empresa:
                    nombre = ConstantesIio.EmpresaDisplayName;
                    break;
                case EntidadSincroniza.UsuarioLibre:
                    nombre = ConstantesIio.UsuarioLibreDisplayName;
                    break;
                case EntidadSincroniza.Suministro:
                    nombre = ConstantesIio.SuministroDisplayName;
                    break;
                case EntidadSincroniza.Barra:
                    nombre = ConstantesIio.BarraDisplayName;
                    break;
                case EntidadSincroniza.CentralGeneracion:
                    nombre = ConstantesIio.CentralDisplayName;
                    break;
                case EntidadSincroniza.GrupoGeneracion:
                    nombre = ConstantesIio.GrupoGeneracionDisplayName;
                    break;
                case EntidadSincroniza.ModoOperacion:
                    nombre = ConstantesIio.ModoOperacionDisplayName;
                    break;
                case EntidadSincroniza.Cuenca:
                    nombre = ConstantesIio.CuencaDisplayName;
                    break;
                case EntidadSincroniza.Embalse:
                    nombre = ConstantesIio.EmbalseDisplayName;
                    break;
                case EntidadSincroniza.Lago:
                    nombre = ConstantesIio.LagoDisplayName;
                    break;
                case EntidadSincroniza.Ninguno:
                    nombre = ConstantesIio.ConAsignacionPendienteDisplayName;
                    break;
                default:
                    throw new ArgumentOutOfRangeException("entidad");

            }
            return nombre;
        }
        public static string GetEntidadTableName(EntidadSincroniza entidad)
        {
            if (!Enum.IsDefined(typeof(EntidadSincroniza), entidad)) throw new ArgumentOutOfRangeException("entidad");

            string nombre;

            switch (entidad)
            {
                case EntidadSincroniza.Empresa:
                    nombre = ConstantesIio.EmpresaDisplayName;
                    break;
                case EntidadSincroniza.UsuarioLibre:
                    nombre = ConstantesIio.UsuarioLibreDisplayName;
                    break;
                case EntidadSincroniza.Suministro:
                    nombre = ConstantesIio.SuministroDisplayName;
                    break;
                case EntidadSincroniza.Barra:
                    nombre = ConstantesIio.BarraDisplayName;
                    break;
                case EntidadSincroniza.CentralGeneracion:
                    nombre = ConstantesIio.CentralDisplayName;
                    break;
                case EntidadSincroniza.GrupoGeneracion:
                    nombre = ConstantesIio.GrupoGeneracionDisplayName;
                    break;
                case EntidadSincroniza.ModoOperacion:
                    nombre = ConstantesIio.ModoOperacionDisplayName;
                    break;
                case EntidadSincroniza.Cuenca:
                    nombre = ConstantesIio.CuencaDisplayName;
                    break;
                case EntidadSincroniza.Embalse:
                    nombre = ConstantesIio.EmbalseDisplayName;
                    break;
                case EntidadSincroniza.Lago:
                    nombre = ConstantesIio.LagoDisplayName;
                    break;
                case EntidadSincroniza.Ninguno:
                    nombre = ConstantesIio.ConAsignacionPendienteDisplayName;
                    break;
                default:
                    throw new ArgumentOutOfRangeException("entidad");

            }

            return nombre;
        }

        public static bool IsEntidadConAsignacionesPendiente(EntidadSincroniza entidad)
        {
            if (!Enum.IsDefined(typeof(EntidadSincroniza), entidad)) throw new ArgumentOutOfRangeException("entidad");

            return entidad == EntidadSincroniza.Ninguno;
        }

        public static List<EntidadSincroniza> GetEntidadesPorProceso(ProcesoEnum proceso)
        {
            if (!Enum.IsDefined(typeof (ProcesoEnum), proceso)) throw new ArgumentOutOfRangeException("proceso");

            switch (proceso)
            {
                case ProcesoEnum.Maestros:
                    return new List<EntidadSincroniza>
                    {
                        EntidadSincroniza.Empresa,
                        EntidadSincroniza.UsuarioLibre,
                        EntidadSincroniza.Suministro,
                        EntidadSincroniza.Barra,
                        EntidadSincroniza.CentralGeneracion,
                        EntidadSincroniza.GrupoGeneracion,
                        EntidadSincroniza.ModoOperacion,
                        EntidadSincroniza.Cuenca,
                        EntidadSincroniza.Embalse,
                        EntidadSincroniza.Lago,                        
                        EntidadSincroniza.Ninguno
                    };
                default:
                    throw new ArgumentOutOfRangeException("proceso", proceso, null);
            }
        }
    }
}