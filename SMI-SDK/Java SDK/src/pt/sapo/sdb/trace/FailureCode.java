package pt.sapo.sdb.trace;

public enum FailureCode {
    UNKNOWN("0");

    private String code;
    
    private FailureCode(String code){
        this.code = code;
    }
            
    @Override
    public String toString() {
        return code;
    }
    
    
}
