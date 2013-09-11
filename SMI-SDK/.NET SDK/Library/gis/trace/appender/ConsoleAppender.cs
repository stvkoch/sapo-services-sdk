namespace pt.sapo.gis.trace.appender
{
    using System;
    using System.Collections.Generic;
    using System.IO;

    public class ConsoleAppender : TextWriterAppender
    {
        public ConsoleAppender(pt.sapo.gis.trace.configuration.appender config)
            : base(config, Console.Out)
        {
        }        
    }    
}
