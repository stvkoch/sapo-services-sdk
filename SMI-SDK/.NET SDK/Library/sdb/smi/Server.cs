using System;

namespace pt.sapo.sdb.smi
{

    /**
     *
     * @author GIS Team
     */
    public class Server
    {
        public String name;
        public String address;
        public DateTime? deployDate;
        public String version;

        public Server() { }

        public Server(String name, String address) : this(name, address, null, null) {}

        public Server(String name, String address, DateTime? deployDate, String version)
        {
            this.name = name;
            this.address = address;
            this.deployDate = deployDate;
            this.version = version;
        }
    }
}