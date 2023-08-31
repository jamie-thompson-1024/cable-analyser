namespace CableAnalyser
{
    public enum PortType
    {
        DB9_MALE, DB9_FEMALE,
        RJ45_MALE, RJ45_FEMALE 
    }

    class PortTypeMethods
    {
        public static string ToString(PortType portType)
        {
            switch(portType)
            {
                case PortType.DB9_MALE:
                    return "DB9 Male";
                case PortType.DB9_FEMALE:
                    return "DB9 Female";
                case PortType.RJ45_MALE:
                    return "RJ45 Male";
                case PortType.RJ45_FEMALE:
                    return "RJ45 Female";
                default:
                    return "N/A";
            }
        }
    }
}