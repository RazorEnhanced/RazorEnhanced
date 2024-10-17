using System;
using System.Collections.Generic;
using System.Linq;
using Assistant;

namespace RazorEnhanced
{
    /// <summary>
    /// Contains all buff information
    /// </summary>
    public class BuffInfo
    {
        private readonly Assistant.BuffInfo m_BuffInfo;
        internal BuffInfo(Assistant.BuffInfo buffInfo)
        {
            m_BuffInfo = buffInfo;
            Name = Player.GetBuffDescription(buffInfo.Icon);
            
            if (buffInfo.TitleCliloc > 0)
            {
                Title = Language.ClilocFormat((int)buffInfo.TitleCliloc, ParseArgs(buffInfo.TitleArgs));
            }
            
            if (buffInfo.DescriptionCliloc > 0)
            {
                Description = Language.ClilocFormat((int)buffInfo.DescriptionCliloc, ParseArgs(buffInfo.DescriptionArgs));
            }
            
            if (buffInfo.ExtraInfoCliloc > 0)
            {
                ExtraInfo = Language.ClilocFormat((int)buffInfo.ExtraInfoCliloc, ParseArgs(buffInfo.ExtraInfoArgs) );
            }
        }

        /// <summary>
        /// Buff name
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Buff Icon type
        /// </summary>
        public int Icon => (int)m_BuffInfo.Icon;
        
        /// <summary>
        /// Buff Started time (datetime ticks)
        /// </summary>
        public long StartTime => m_BuffInfo.StartTime.Ticks;
        
        /// <summary>
        /// Buff Total duration in milliseconds
        /// </summary>
        public int Duration => (m_BuffInfo.Duration * 1000);
        
        /// <summary>
        /// Buff time remaining in milliseconds
        /// </summary>
        public int Remaining => (Duration - Elapsed);
        
        /// <summary>
        /// If buff has expired
        /// </summary>
        public bool HasExpired => (Duration > 0 && Elapsed > Duration);

        /// <summary>
        /// Buff time elapsed in milliseconds
        /// </summary>
        public int Elapsed => (int)(DateTime.Now - m_BuffInfo.StartTime).TotalMilliseconds;
        
        /// <summary>
        /// Buff Title
        /// </summary>
        public string Title { get; private set; }
        /// <summary>
        /// Buff description (with all args/clilocs parsed)
        /// </summary>
        public string Description { get; private set; }
        /// <summary>
        /// Extra information
        /// </summary>
        public string ExtraInfo { get; private set; }
        /// <summary>
        /// Title cliloc id
        /// </summary>
        public uint TitleCliloc => m_BuffInfo.TitleCliloc;
        /// <summary>
        /// Title cliloc args 
        /// </summary>
        public List<string> TitleArgs => ParseArgs(m_BuffInfo.TitleArgs).Split('\t').ToList();
        /// <summary>
        /// Description cliloc id
        /// </summary>
        public uint DescriptionCliloc => m_BuffInfo.DescriptionCliloc;
        /// <summary>
        /// Title cliloc args (useful to look up some values like Enemy of one dmg bonus, poison damage, etc...)
        /// </summary>
        public List<string> DescriptionArgs => ParseArgs(m_BuffInfo.TitleArgs).Split('\t').ToList();
        
        public uint ExtraInfoCliloc => m_BuffInfo.ExtraInfoCliloc;
        
        public List<string> ExtraInfoArgs => ParseArgs(m_BuffInfo.TitleArgs).Split('\t').ToList();

        /// <summary>
        /// Parse arguments from the server to a valid arguments to the standard logic
        /// some arguments are send with \t even if not needed, and it broke the all logic, or missing ones, order, etc
        /// </summary>
        /// <param name="args">Arguments to fix</param>
        /// <returns></returns>
        private string ParseArgs(string args)
        {
            string result = args;

            if (string.IsNullOrEmpty(args))
            {
                result = m_BuffInfo.TitleArgs;
            }

            if (!string.IsNullOrEmpty(result))
            {
                if (result.StartsWith("\t"))
                {
                    result = result.Substring(1);
                }

                result = Language.ParseSubCliloc(result);
            }

            return result;
        } 
    }
}