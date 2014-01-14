using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace pt.sapo.gis.trace.configuration{

    /// <summary>
    /// Configuration Object to FileLogAppender
    /// </summary>    
    public class FileAppenderConfigProperties : AppenderConfigProperties{
        public FileAppenderConfigProperties(IDictionary<String, String> properties) 
            : base(properties) { }

        /// <summary>
        /// File name to create log files
        /// </summary>
        public String Filename { get { return this["filename"]; } }

        /// <summary>
        /// Path to write log files
        /// Default: current directory
        /// </summary>
        public String Path { get { return ContainsKey("path") ? this["path"] : ""; } }        
        
    }
}
