namespace MunicipalityRegistry.Municipality.Events
{
    using System;
    using Be.Vlaanderen.Basisregisters.EventHandling;
    using Be.Vlaanderen.Basisregisters.GrAr.Provenance;
    using Newtonsoft.Json;

    [EventName("MunicipalityWasNamed")]
    [EventDescription("De gemeente kreeg een naam toegewezen in een bepaalde taal.")]
    public class MunicipalityWasNamed : IHasProvenance, ISetProvenance
    {
        public Guid MunicipalityId { get; }
        public string Name { get; }
        public Language Language { get; }
        public ProvenanceData Provenance { get; private set; }

        public MunicipalityWasNamed(
            MunicipalityId municipalityId,
            MunicipalityName municipalityName)
        {
            MunicipalityId = municipalityId;
            Language = municipalityName.Language;
            Name = municipalityName.Name;
        }

        [JsonConstructor]
        private MunicipalityWasNamed(
            Guid municipalityId,
            string name,
            Language language,
            ProvenanceData provenance) :
            this(
                new MunicipalityId(municipalityId),
                new MunicipalityName(name, language)) => ((ISetProvenance)this).SetProvenance(provenance.ToProvenance());

        void ISetProvenance.SetProvenance(Provenance provenance) => Provenance = new ProvenanceData(provenance);
    }
}
