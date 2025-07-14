namespace MunicipalityRegistry.Municipality.Events
{
    using System;
    using Be.Vlaanderen.Basisregisters.EventHandling;
    using Be.Vlaanderen.Basisregisters.GrAr.Provenance;
    using Newtonsoft.Json;

    [EventName("MunicipalityWasRemoved")]
    [EventDescription("De gemeente werd verwijderd.")]
    public class MunicipalityWasRemoved : IHasProvenance, ISetProvenance
    {
        public Guid MunicipalityId { get; }
        public ProvenanceData Provenance { get; private set; }

        public MunicipalityWasRemoved(MunicipalityId municipalityId)
        {
            MunicipalityId = municipalityId;
        }

        [JsonConstructor]
        private MunicipalityWasRemoved(Guid municipalityId, ProvenanceData provenance)
            : this(new MunicipalityId(municipalityId))
        {
            ((ISetProvenance)this).SetProvenance(provenance.ToProvenance());
        }

        void ISetProvenance.SetProvenance(Provenance provenance) => Provenance = new ProvenanceData(provenance);
    }
}
