package pt.sapo.gis.trace;

/**
 * @author GIS Team
 * Enum for entry type codes
 */
public enum EntryType {
    UNKNOWN("0");
    
    private String code;
    
    private EntryType(String code){
        this.code = code;
    }
            
    @Override
    public String toString() {
        return code;
    }
}