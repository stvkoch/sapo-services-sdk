namespace pt.sapo.gis.trace
{
    using System;

    /**
     *
     * @author GIS Team
     */
    public class Summary
    {
        public bool Sucess { get; private set; }
        public String Result { get; private set; }

        public Summary(bool sucess) : this(sucess, null) { }

        public Summary(bool sucess, String result)
        {
            this.Sucess = sucess;
            this.Result = result;
        }
    }
}
