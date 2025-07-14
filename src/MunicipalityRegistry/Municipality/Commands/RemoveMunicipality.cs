namespace MunicipalityRegistry.Municipality.Commands
{
    public class RemoveMunicipality
    {
        public NisCode NisCode { get; }

        public RemoveMunicipality(NisCode nisCode)
        {
            NisCode = nisCode;
        }
    }
}
