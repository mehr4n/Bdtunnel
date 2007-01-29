// -----------------------------------------------------------------------------
// BoutDuTunnel
// Sebastien LEBRETON
// sebastien.lebreton[-at-]free.fr
// -----------------------------------------------------------------------------

#region " Inclusions "
using System;
using System.Net;
using System.Reflection;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Http;
#endregion

namespace Bdt.Shared.Protocol
{

    /// -----------------------------------------------------------------------------
    /// <summary>
    /// Interfance indiquant la compatibilité du protocole avec un proxy pour
    /// le canal de communication côté client
    /// </summary>
    /// -----------------------------------------------------------------------------
    public interface IProxyCompatible
    {

        #region " Proprietes "
        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Le proxy utilisé par le canal de communication côté client
        /// </summary>
        /// -----------------------------------------------------------------------------
        IWebProxy Proxy
        {
            get;
            set;
        }
        #endregion

    }

}

