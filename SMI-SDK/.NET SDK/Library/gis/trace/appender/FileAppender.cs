using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using pt.sapo.gis.trace.appender;
using System.IO;
using pt.sapo.gis.trace;
using pt.sapo.gis.trace.configuration;

namespace pt.sapo.gis.trace.appender {

    /// <summary>
    /// Trace SDK Appender that logs all traces in files.
    /// </summary>   
    public class FileAppender : pt.sapo.gis.trace.appender.Appender{
        
        private FileAppenderConfigProperties properties;
        private StreamWriter file;
        private string currentFileDate;
        private object lockObj = new object();

        public FileAppender(pt.sapo.gis.trace.configuration.appender config)
            : base(config) {                
                properties = new FileAppenderConfigProperties(base.Properties);                
        }        

        private void Prepare(){            
            if(file != null && currentFileDate != DateTime.Now.ToString("yyyyMMdd")){
                file.Close();
                file = null;
            }
            if (file == null) {
                currentFileDate = DateTime.Now.ToString("yyyyMMdd");
                int dotIdx = properties.Filename.LastIndexOf('.');
                file = new StreamWriter(
                    Path.Combine(
                        properties.Path, 
                        string.Format("{0}_{1}{2}", 
                            dotIdx == -1 ? properties.Filename : properties.Filename.Substring(0, dotIdx),
                            currentFileDate,
                            dotIdx == -1 ? ".log" : properties.Filename.Substring(dotIdx))
                        ), true);
            }            
        }
        
        protected virtual void Write(Trace t) {            
            file.WriteLine("TRACE ({2}|{3})[{0:yyyy-MM-dd HH:mm:ss.fff} - {1:yyyy-MM-dd HH:mm:ss.fff}] {4:0.00}ms", t.Start, t.End, t.Id, t.ContextId, (t.End-t.Start).TotalMilliseconds);
            foreach (Entry e in t.Entries) {
                Write(e, 0);
            }
        }

        protected virtual void Write(Entry e, int deep) {
            for (int i = -1; i < deep; ++i)
                file.Write("..");            
            file.WriteLine("ENTRY: '{0}', offset: {1:0.000}, duration: {2:0.000}, severity: {3}, properties: [{4}]", 
                    e.Description, 
                    e.Offset, 
                    e.Duration, 
                    e.Severity, 
                    string.Join(";", e.Properties.Select((k,v) => k+":"+v.ToString())));
            if (deep != -1) {
                foreach (Entry se in e.Entries) {
                    Write(se, deep + 1);
                }
            }
        }

        public override void OnEntry(Entry e) {
            lock (lockObj) {
                Prepare();
                Write(e, -1);
                file.Flush();
            }
        }

        public override void OnTrace(Trace t) {
            lock (lockObj) {
                Prepare();            
                Write(t);
                file.Flush();
            }
        }
    }
}
