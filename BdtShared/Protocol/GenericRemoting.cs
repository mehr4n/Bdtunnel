// -----------------------------------------------------------------------------
// BoutDuTunnel
// Sebastien LEBRETON
// sebastien.lebreton[-at-]free.fr
// -----------------------------------------------------------------------------

#region " Inclusions "
using System;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;

using Bdt.Shared.Resources;
using Bdt.Shared.Service;
using Bdt.Shared.Logs;
#endregion

namespace Bdt.Shared.Protocol
{

    /// -----------------------------------------------------------------------------
    /// <summary>
    /// Protocole de communication basé sur le remoting .NET
    /// </summary>
    /// -----------------------------------------------------------------------------
    public abstract class GenericRemoting : GenericProtocol
    {

        #region " Attributs "
        protected IChannel m_clientchannel;
        protected IChannel m_serverchannel;
        #endregion

        #region " Proprietes "
        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Le canal de communication côté serveur
        /// </summary>
        /// -----------------------------------------------------------------------------
        protected abstract IChannel ServerChannel
        {
            get;
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Le canal de communication côté client
        /// </summary>
        /// -----------------------------------------------------------------------------
        public abstract IChannel ClientChannel
        {
            get;
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// L'URL nécessaire pour se connecter au serveur
        /// </summary>
        /// -----------------------------------------------------------------------------
        protected abstract string ServerURL
        {
            get;
        }
        #endregion

        #region " Méthodes "
        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Configuration côté client
        /// </summary>
        /// -----------------------------------------------------------------------------
        public override void ConfigureClient()
        {
            Log(string.Format(Strings.CONFIGURING_CLIENT, this.GetType().Name, ServerURL), ESeverity.DEBUG);
            ChannelServices.RegisterChannel(ClientChannel, false);
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Dé-configuration côté client
        /// </summary>
        /// -----------------------------------------------------------------------------
        public override void UnConfigureClient()
        {
            Log(string.Format(Strings.UNCONFIGURING_CLIENT, this.GetType().Name), ESeverity.DEBUG);
            ChannelServices.UnregisterChannel(ClientChannel);
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Configuration côté serveur
        /// </summary>
        /// <param name="type">le type d'objet à rendre distant</param>
        /// -----------------------------------------------------------------------------
        public override void ConfigureServer(Type type)
        {
            Log(string.Format(Strings.CONFIGURING_SERVER, this.GetType().Name, Port), ESeverity.INFO);
            ChannelServices.RegisterChannel(ServerChannel, false);
            WellKnownServiceTypeEntry wks = new WellKnownServiceTypeEntry(type, Name, WellKnownObjectMode.Singleton);
            RemotingConfiguration.RegisterWellKnownServiceType(wks);
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Retourne une instance de tunnel
        /// </summary>
        /// <returns>une instance de tunnel</returns>
        /// -----------------------------------------------------------------------------
        public override Service.ITunnel GetTunnel()
        {
            return ((ITunnel)Activator.GetObject(typeof(ITunnel), ServerURL));
        }
        #endregion

    }

}

