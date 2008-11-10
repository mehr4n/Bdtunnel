// -----------------------------------------------------------------------------
// BoutDuTunnel
// Sebastien LEBRETON
// sebastien.lebreton[-at-]free.fr
// -----------------------------------------------------------------------------

#region " Inclusions "
using System;
using System.Globalization;
using System.IO;
using System.Web;

using Bdt.Server.Service;
using Bdt.Server.Runtime;
using Bdt.Shared.Configuration;
using Bdt.Shared.Logs;
using Bdt.Shared.Runtime;
#endregion

namespace Bdt.WebServer.Runtime
{

    /// -----------------------------------------------------------------------------
    /// <summary>
    /// Programme côté serveur du tunnel de communication
    /// </summary>
    /// -----------------------------------------------------------------------------
    public class BdtWebServer : Program
    {

        #region " Attributs "
        HttpServerUtility m_server;
        #endregion

        #region " Proprietes "
        public override string ConfigFile
        {
            get
            {
                return m_server.MapPath(string.Format("App_Data" + Path.DirectorySeparatorChar + "{0}Cfg.xml", typeof(BdtServer).Assembly.GetName().Name));
            }
        }
        #endregion
        
        #region " Méthodes "
        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Constructeur
        /// </summary>
        /// <param name="server">l'utilitaire serveur (mappage)</param>
        /// -----------------------------------------------------------------------------
        public BdtWebServer(HttpServerUtility server)
            : base()
        {
            m_server = server;
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Fixe la culture courante
        /// </summary>
        /// <param name="name">le nom de la culture</param>
        /// -----------------------------------------------------------------------------
        public override void SetCulture(String name)
        {
            base.SetCulture(name);
            if ((name != null) && (name != String.Empty))
            {
                Bdt.Server.Resources.Strings.Culture = new CultureInfo(name);
            }
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Chargement des données de configuration
        /// </summary>
        /// <param name="args">Arguments de la ligne de commande</param>
        /// -----------------------------------------------------------------------------
        public override void LoadConfiguration(string[] args)
        {
            m_args = args;

            LoggedObject.GlobalLogger = CreateLoggers();
            Log(Bdt.Shared.Resources.Strings.LOADING_CONFIGURATION, ESeverity.DEBUG);
            SharedConfig cfg = new SharedConfig(m_config);
            // unneeded in IIs web hosting model, see web.config
            // m_protocol = GenericProtocol.GetInstance(cfg);
            SetCulture(cfg.ServiceCulture);
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Démarrage du serveur
        /// </summary>
        /// -----------------------------------------------------------------------------
        public void Start()
        {
            LoadConfiguration(new String[] { });

            Log(string.Format(Bdt.Server.Resources.Strings.SERVER_TITLE, this.GetType().Assembly.GetName().Version.ToString(3)), ESeverity.INFO);
            Log(Program.FrameworkVersion(), ESeverity.INFO);

            Tunnel.Configuration = m_config;
            Tunnel.Logger = LoggedObject.GlobalLogger;

            // unneeded in IIs web hosting model, see web.config
            // server.Protocol.ConfigureServer(typeof(Tunnel));
            Log(Bdt.Server.Resources.Strings.SERVER_STARTED, ESeverity.INFO);
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Initialisation des loggers
        /// </summary>
        /// <returns>un MultiLogger lié à une source fichier et console</returns>
        /// -----------------------------------------------------------------------------
        public override BaseLogger CreateLoggers()
        {
            XMLConfig xmlConfig = new XMLConfig(ConfigFile, 1);
            m_config = new ConfigPackage();
            m_config.AddSource(xmlConfig);

            // Map the path to the current Web Application
            String key = CFG_FILE + Bdt.Shared.Configuration.BaseConfig.SOURCE_ITEM_ATTRIBUTE + FileLogger.CONFIG_FILENAME;
            String filename = xmlConfig.Value(key, null);
            if ((filename != null) && (!Path.IsPathRooted(filename))) {
                xmlConfig.SetValue(key, m_server.MapPath(filename)); 
            }

            MultiLogger log = new MultiLogger();
            m_consoleLogger = new ConsoleLogger(CFG_CONSOLE, m_config);
            m_fileLogger = new FileLogger(CFG_FILE, m_config);
            log.AddLogger(m_consoleLogger);
            log.AddLogger(m_fileLogger);

            return log;
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Arrêt du serveur
        /// </summary>
        /// -----------------------------------------------------------------------------
        public void Stop()
        {
            Tunnel.DisableChecking();
            UnLoadConfiguration();
        }
        #endregion

    }
}
