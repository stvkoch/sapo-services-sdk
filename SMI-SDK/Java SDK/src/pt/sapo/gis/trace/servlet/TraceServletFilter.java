package pt.sapo.gis.trace.servlet;

import java.io.IOException;
import java.net.InetAddress;
import javax.servlet.Filter;
import javax.servlet.FilterChain;
import javax.servlet.FilterConfig;
import javax.servlet.ServletException;
import javax.servlet.ServletRequest;
import javax.servlet.ServletResponse;
import javax.servlet.http.HttpServletRequest;
import pt.sapo.gis.trace.Trace;
import pt.sapo.gis.trace.TraceManager;
import pt.sapo.gis.trace.appender.SMI.SMILoggerAppender;

public class TraceServletFilter implements Filter{
    
    @Override
    public void init(FilterConfig filterConfig) throws ServletException {
    }

    @Override
    public void doFilter(ServletRequest request, ServletResponse response, FilterChain chain) throws IOException, ServletException {
        String traceContext = ((HttpServletRequest)request).getHeader("ESBTraceContext");
        String activityId = ((HttpServletRequest)request).getHeader("ESBActivityId");
        
        Trace trace = TraceManager.BeginTrace(null, traceContext);
        trace.setProperty(SMILoggerAppender.REPORT_ACTIVITY_ID_PROPERTY, activityId);
        
        trace.setServerInfo(InetAddress.getLocalHost());
        
        trace.setClientInfo(InetAddress.getByName(request.getRemoteAddr()));
        
        chain.doFilter(request, response);
        
        TraceManager.EndTrace();
                
    }

    @Override
    public void destroy() {
    }
}
