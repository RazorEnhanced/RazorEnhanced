using System;

namespace Assistant
{
    /// <summary>
    /// Contains all buff information like icon, time, text, description, cliloc, etc...
    /// </summary>
    internal class BuffInfo
    {
        /// <summary>
        /// Buff type
        /// </summary>
        public BuffIcon Icon { get; set; }
        /// <summary>
        /// Total duration
        /// </summary>
        public int Duration { get; set; }
        /// <summary>
        /// Started time
        /// </summary>
        public DateTime StartTime { get; set; }
        /// <summary>
        /// Cliloc title ID
        /// </summary>
        public uint TitleCliloc { get; set; }
        /// <summary>
        /// Cliloc title args
        /// </summary>
        public string TitleArgs { get; set; }
        /// <summary>
        /// Cliloc description ID
        /// </summary>
        public uint DescriptionCliloc { get; set; }
        /// <summary>
        /// Cliloc description args
        /// </summary>
        public string DescriptionArgs { get; set; }
        /// <summary>
        /// Cliloc extra information ID
        /// </summary>
        public uint ExtraInfoCliloc { get; set; }
        /// <summary>
        /// Cliloc extra information args
        /// </summary>
        public string ExtraInfoArgs { get; set; }
    }
}