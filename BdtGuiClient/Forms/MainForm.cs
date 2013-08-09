/// -----------------------------------------------------------------------------
/// BoutDuTunnel
/// Sebastien LEBRETON
/// sebastien.lebreton[-at-]free.fr
/// -----------------------------------------------------------------------------

#region " Inclusions "
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Net;
using System.Text;
using System.Windows.Forms;

using Bdt.GuiClient.Resources;
using Bdt.Client.Runtime;
using Bdt.Shared.Logs;
using Bdt.Shared.Protocol;
#endregion

namespace Bdt.GuiClient.Forms
{

    /// -----------------------------------------------------------------------------
    /// <summary>
    /// Fen�tre principale de l'application, se r�duit � une ic�ne de notification
    /// </summary>
    /// -----------------------------------------------------------------------------
    public partial class MainForm : Form
    {

        #region " Enumerations "
        public enum EClientState
        {
            CHANGING = 0,
            STARTED = 1,
            STOPPED = 2,
        }
        #endregion

        #region " Attributs "
        protected BdtClient m_client = null;
        protected EClientState m_clientState = EClientState.STOPPED;
        #endregion

        #region " Methodes "
        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Constructeur
        /// </summary>
        /// <param name="client">le client Bdt associ�</param>
        /// -----------------------------------------------------------------------------
        public MainForm (BdtClient client)
        {
            m_client = client;
            InitializeComponent();
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Configuration du client
        /// </summary>
        /// <param name="sender">l'appelant</param>
        /// <param name="e">les parametres</param>
        /// -----------------------------------------------------------------------------
        private void ConfigureItem_Click (object sender, EventArgs e)
        {
            EClientState previous = m_clientState;
            m_clientState = EClientState.CHANGING;
            using (SetupForm setup = new SetupForm(m_client.ClientConfig))
            {
                if (setup.ShowDialog() == DialogResult.OK)
                {
                    /*
                     * Petite subtilite: a ce moment la configuration m_client contient de faux loggers
                     * utiles uniquement pour sauvegarder le parametrage dans le fichier de configuration.
                     * 
                     * Par contre les vrais instances des loggers sont conserves dans l'ancetre BdtClient.
                     * UnLoadConfiguration va fermer ces loggers. LoadConfiguration va recharger le fichier
                     * de configuration prealablement sauvegarde pour remettre a jour les instances ->
                     * rechargement de la section logs uniquement
                     * 
                     */
                    m_clientState = previous;
                    WaitThenStopClientIfNeeded();
                    try
                    {
                        m_client.ClientConfig.SaveToFile(Path.GetDirectoryName(Application.ExecutablePath) + Path.DirectorySeparatorChar + m_client.ConfigFile);
                        m_client.UnLoadConfiguration();
                        m_client.LoadConfiguration();
                        StartItem_Click(sender, e);
                    }
                    catch (Exception ex)
                    {
                        m_client.Log(ex.Message, ESeverity.ERROR);
                        m_client.Log(ex.ToString(), ESeverity.DEBUG);
                    }
                }
                else
                {
                    m_clientState = previous;
                }
            }
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Saisie des informations d'authentification sur le proxy
        /// </summary>
        /// <param name="proxyProtocol">le protocol IProxyCompatible � alterer</param>
        /// <param name="retry">pour permettre les essais multiples</param>
        /// -----------------------------------------------------------------------------
        public void InputProxyCredentials (IProxyCompatible proxyProtocol, ref bool retry)
        {
            using (ProxyForm proxy = new ProxyForm(m_client.ClientConfig))
            {
                if (proxy.ShowDialog() == DialogResult.OK)
                {
                    proxyProtocol.Proxy.Credentials = new NetworkCredential(m_client.ClientConfig.ProxyUserName, m_client.ClientConfig.ProxyPassword, m_client.ClientConfig.ProxyDomain);
                    retry = true;
                }
                else
                {
                    retry = false;
                }
            }
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// D�marrage du client
        /// </summary>
        /// <param name="sender">l'appelant</param>
        /// <param name="e">les parametres</param>
        /// -----------------------------------------------------------------------------
        private void StartItem_Click (object sender, EventArgs e)
        {
            m_clientState = EClientState.CHANGING;
            UpdateNotifyIcon(Strings.MAINFORM_CLIENT_STARTING, false);
            System.Threading.ThreadPool.QueueUserWorkItem(StartClient);
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Arr�t du client
        /// </summary>
        /// <param name="sender">l'appelant</param>
        /// <param name="e">les parametres</param>
        /// -----------------------------------------------------------------------------
        private void StopItem_Click (object sender, EventArgs e)
        {
            m_clientState = EClientState.CHANGING;
            UpdateNotifyIcon(Strings.MAINFORM_CLIENT_STOPPING, false);
            System.Threading.ThreadPool.QueueUserWorkItem(StopClient);
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// D�marrage effectif du client
        /// </summary>
        /// <param name="state">les parametres</param>
        /// -----------------------------------------------------------------------------
        private void StartClient (Object state)
        {
            try
            {
                m_client.StartClient();
                m_clientState = EClientState.STARTED;
                // On utilise un Invoke car le thread courant est diff�rent du thread cr�ateur du contr�le (mod�le STA)
                this.Invoke(new UpdateNotifyIconDelegate(UpdateNotifyIcon), new object[] { Strings.MAINFORM_CLIENT_STARTED, true });
            }
            catch (Exception e)
            {
                m_client.Log(e.Message, ESeverity.ERROR);
                m_client.Log(e.ToString(), ESeverity.DEBUG);
                StopClient(null);
            }
            NotifyContextMenu_Opened(this, EventArgs.Empty);
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Arr�t effectif du client
        /// </summary>
        /// <param name="state">les parametres</param>
        /// -----------------------------------------------------------------------------
        private void StopClient (Object state)
        {
            try
            {
                m_client.StopClient();
                m_clientState = EClientState.STOPPED;
                // On utilise un Invoke car le thread courant est diff�rent du thread cr�ateur du contr�le (mod�le STA)
                this.Invoke(new UpdateNotifyIconDelegate(UpdateNotifyIcon), new object[] { Strings.MAINFORM_CLIENT_STOPPED, false });
            }
            catch (Exception e)
            {
                m_client.Log(e.Message, ESeverity.ERROR);
                m_client.Log(e.ToString(), ESeverity.DEBUG);
            }
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// D�l�gu� pour l'appel inter-thread de UpdateNotifyIcon
        /// </summary>
        /// <param name="text">le texte a fixer (null si inutile)</param>
        /// -----------------------------------------------------------------------------
        private delegate void UpdateNotifyIconDelegate (string text, bool useBalloon);

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Mise � jour de l'ic�ne de notification
        /// </summary>
        /// <param name="text">le texte a fixer (null si inutile)</param>
        /// -----------------------------------------------------------------------------
        private void UpdateNotifyIcon (string text, bool useBalloon)
        {
            if (text != null)
            {
                NotifyIcon.Text = text;
            }
            StartItem.Enabled = (m_clientState == EClientState.STOPPED);
            StopItem.Enabled = (m_clientState == EClientState.STARTED);
            ConfigureItem.Enabled = (m_clientState != EClientState.CHANGING);
            QuitItem.Enabled = (m_clientState != EClientState.CHANGING);
            if (m_clientState == EClientState.CHANGING)
            {
                InfoItem.Text = Strings.MAINFORM_PLEASE_WAIT;
            }
            else
            {
                InfoItem.Text = string.Format(Strings.MAINFORM_CLIENT_TITLE, this.GetType().Assembly.GetName().Version.ToString(3));
            }
            LogsItem.Enabled = m_client.ClientConfig.FileLogger.Enabled;
            if (useBalloon)
            {
                NotifyIcon.ShowBalloonTip(0, InfoItem.Text, text, ToolTipIcon.Info);
            }
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Arr�t du client si n�cessaire
        /// </summary>
        /// <param name="state">les parametres</param>
        /// -----------------------------------------------------------------------------
        private void WaitThenStopClientIfNeeded ()
        {
            // On attends que le traitement en cours s'ach�ve
            while (m_clientState == EClientState.CHANGING)
            {
                Application.DoEvents();
            }

            // Arr�t du client si n�cessaire
            if (m_clientState == EClientState.STARTED)
            {
                StopClient(null);
            }
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Fin de l'application
        /// </summary>
        /// <param name="sender">l'appelant</param>
        /// <param name="e">les parametres</param>
        /// -----------------------------------------------------------------------------
        private void QuitItem_Click (object sender, EventArgs e)
        {
            WaitThenStopClientIfNeeded();
            Close();
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Visualisation des logs
        /// </summary>
        /// <param name="sender">l'appelant</param>
        /// <param name="e">les parametres</param>
        /// -----------------------------------------------------------------------------
        private void LogsItem_Click (object sender, EventArgs e)
        {
            ProcessStartInfo info = new ProcessStartInfo(m_client.ClientConfig.FileLogger.Filename);
            Process.Start(info);
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Ouverture du menu contextuel
        /// </summary>
        /// <param name="sender">l'appelant</param>
        /// <param name="e">les parametres</param>
        /// -----------------------------------------------------------------------------
        private void NotifyContextMenu_Opened (object sender, EventArgs e)
        {
            UpdateNotifyIcon(null, false);
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Chargement de la page
        /// </summary>
        /// <param name="sender">l'appelant</param>
        /// <param name="e">les parametres</param>
        /// -----------------------------------------------------------------------------
        private void MainForm_Load (object sender, EventArgs e)
        {
            /*
             * Le choix de la culture se produit apr�s la cr�ation du form
             * principal. Il est donc n�cessaire de rafraichir les objets pr�sents
             * en fonction de la culture active.
             * 
             */
            RefreshLocalizedItems();
            StartItem_Click(sender, e);
        }
        #endregion

    }
}