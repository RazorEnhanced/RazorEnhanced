using Assistant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace RazorEnhanced
{
    /// <summary>
    /// The Sound class provides an api to manipulate Sounds. For now it just turns logging for sounds on / off
    /// </summary>

    public class Sound
    {

        static internal bool m_logActive = false;
        static internal bool Logging
            {
            get {return m_logActive; }
            set { m_logActive = value; }
            }

        /// <summary>
        /// Enables/Disables logging of incoming sound requests
        /// </summary>
        /// <param name="activateLogging"> True= activate sound logging/ False Deactivate sound logging</param>
        public static void Log(bool activateLogging)
        {
            Logging = activateLogging;
        }
	}
}
