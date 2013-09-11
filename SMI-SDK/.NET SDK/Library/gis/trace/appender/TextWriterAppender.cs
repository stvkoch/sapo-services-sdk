using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace pt.sapo.gis.trace.appender
{
    public abstract class TextWriterAppender : Appender
    {
        protected TextWriter writer;

        public TextWriterAppender(configuration.appender appender, TextWriter writer)
            : base(appender)
        {
            this.writer = writer;
        }

        public virtual void Write(Trace t) {

            writer.Write("\n--------------\n Trace from {0:yyyy-MM-dd HH:mm:ss.fff} to {1:yyyy-MM-dd HH:mm:ss.fff} \n entries:", t.Start, t.End);
            foreach (Entry e in t.Entries)
            {
                Write(e, 0);
            }
            writer.Write("\n--------------\n");
            
        }

        public virtual void Write(Entry e, int deep)
        {
            writer.WriteLine();
            for (int i = 0; i < deep; ++i)
                writer.Write("\t");
            var str = new StringWriter();
            var jsonSer = new Newtonsoft.Json.JsonSerializer();
            jsonSer.Serialize(str, e.Properties);
            writer.WriteLine("{{ description: '{0}', offset: {1:0.000}, duration: {2:0.000}, severity: {3}, properties: {4}, entries: [", e.Description, e.Offset, e.Duration, e.Severity, str.ToString());
            foreach (Entry s in e.Entries)
            {
                Write(s, deep + 1);
            }
            writer.WriteLine("]}}");
        }

        public override void OnEntry(Entry e)
        {            
        }

        public override void OnTrace(Trace t)
        {
            Write(t);
        }
    }
}

