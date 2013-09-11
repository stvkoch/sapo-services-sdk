/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */
package pt.sapo.gis.trace;

/**
 *
 * @author GIS Team
 */
public class SummaryTraceProperty {
    public static final String SUMMARY_TRACE_PROPERTY_NAME = "summary";
    
    public boolean sucess;
    public String result;

    SummaryTraceProperty(boolean sucess){
        this(sucess, null);
    }
            
    SummaryTraceProperty(boolean sucess, String result) {
        this.sucess = sucess;
        this.result = result;
    }
}
