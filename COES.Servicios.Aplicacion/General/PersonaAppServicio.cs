using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;
using COES.Servicios.Aplicacion.Factory;
using COES.Servicios.Aplicacion.Helper;
using log4net;

namespace COES.Servicios.Aplicacion.General
{
    /// <summary>
    /// Clases con métodos del módulo Persona
    /// </summary>
    public class PersonaAppServicio : AppServicioBase
    {
        /// <summary>
        /// Instancia para el manejo de logs
        /// </summary>
        private static readonly ILog Logger = LogManager.GetLogger(typeof(PersonaAppServicio));

        #region Métodos Tabla SI_PERSONA

        /// <summary>
        /// Inserta un registro de la tabla SI_PERSONA
        /// </summary>
        public void SaveSiPersona(SiPersonaDTO entity)
        {
            try
            {
                FactorySic.GetSiPersonaRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla SI_PERSONA
        /// </summary>
        public void UpdateSiPersona(SiPersonaDTO entity)
        {
            try
            {
                FactorySic.GetSiPersonaRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla SI_PERSONA
        /// </summary>
        public void DeleteSiPersona(int percodi, string username)
        {
            try
            {
                FactorySic.GetSiPersonaRepository().Delete(percodi);
                FactorySic.GetSiPersonaRepository().Delete_UpdateAuditoria(percodi, username);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla SI_PERSONA
        /// </summary>
        public SiPersonaDTO GetByIdSiPersona(int percodi)
        {
            var reg = FactorySic.GetSiPersonaRepository().GetById(percodi);
            if (reg != null)
            {
                reg.Percargo = reg.Percargo ?? "";
                reg.Pertelefono = reg.Pertelefono ?? "";
            }

            return reg;
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla SI_PERSONA
        /// </summary>
        public List<SiPersonaDTO> ListSiPersonas()
        {
            return FactorySic.GetSiPersonaRepository().List();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla SiPersona
        /// </summary>
        public List<SiPersonaDTO> GetByCriteriaAreaSiPersonas(int areacodi)
        {
            return FactorySic.GetSiPersonaRepository().GetByCriteriaArea(areacodi);
        }

        /// <summary>
        /// Permite obtener el cargo de la persona
        /// </summary>
        public string GetCargo(string Nombre)
        {
            return FactorySic.GetSiPersonaRepository().GetCargo(Nombre);
        }


        /// <summary>
        /// Permite obtener el area de la persona
        /// </summary>
        public string GetArea(string Nombre)
        {
            return FactorySic.GetSiPersonaRepository().GetArea(Nombre);
        }


        /// <summary>
        /// Permite obtener el telédono de la persona
        /// </summary>
        public string GetTelefono(string Nombre)
        {
            return FactorySic.GetSiPersonaRepository().GetTelefono(Nombre);
        }

        /// <summary>
        /// Permite obtener el mail de la persona
        /// </summary>
        public string GetMail(string Nombre)
        {
            return FactorySic.GetSiPersonaRepository().GetMail(Nombre);
        }

        public List<SiPersonaDTO> ListaEspecialistasSME()
        {
            return FactorySic.GetSiPersonaRepository().ListaEspecialistasSME();
        }

        #endregion

    }
}
