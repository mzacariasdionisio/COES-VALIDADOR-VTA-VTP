using COES.Base.Core;
using COES.Dominio.DTO.Sic;
using COES.Dominio.DTO.Transferencias;
using COES.Servicios.Aplicacion.Factory;
using COES.Servicios.Aplicacion.Helper;
using log4net;
using System;
using System.Collections.Generic;

namespace COES.Servicios.Aplicacion.Auditoria
{
    public class AuditoriaAppServicio : AppServicioBase
    {

        /// <summary>
        /// Instancia para el manejo de logs
        /// </summary>
        private static readonly ILog Logger = LogManager.GetLogger(typeof(AuditoriaAppServicio));

        #region Métodos Tabla AUDITABLE

        public List<SiTablaAuditableDTO> List()
        {
            return FactorySic.GetSiTablaAuditableRepository().List();
        }

       


                

        public SiTablaAuditableDTO GetByIdAuditoria(int TauditCodi)
        {
            return FactorySic.GetSiTablaAuditableRepository().GetById(TauditCodi);
        }

 

        public int InsertNewAuditoria(SiTablaAuditableDTO entity)
        {
            try
            {
                return FactorySic.GetSiTablaAuditableRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }


        public void Update(SiTablaAuditableDTO model)
        {
            try
            {
                FactorySic.GetSiTablaAuditableRepository().Update(model);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        public void DeleteAuditoria(int tauditcodi)
        {
            try
            {
                FactorySic.GetSiTablaAuditableRepository().Delete(tauditcodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }
        #endregion

        #region AseguramientoOperacionSev
        public List<fwUserDTO> ListUserRol(int rolcode)
        {
            return FactorySic.GetSiTablaAuditableRepository().ListUserRol(rolcode);
        }
        #endregion
    }
}
