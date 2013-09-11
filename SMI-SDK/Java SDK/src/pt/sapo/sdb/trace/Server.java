/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */
package pt.sapo.sdb.trace;

import java.util.Date;

/**
 *
 * @author GIS Team
 */
public class Server {    
    public String name ;
    public String address ;
    public Date deployDate ;
    public String version ;
    
    public Server(){}
    
    public Server(String name, String address){
        this(name, address, null, null);
    }
    
    public Server(String name, String address, Date deployDate, String version){
        this.name = name;
        this.address = address;
        this.deployDate = deployDate;
        this.version = version;               
    }
}
