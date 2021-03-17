namespace MunicipalityRegistry.Municipality.Events
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Be.Vlaanderen.Basisregisters.EventHandling;
    using Be.Vlaanderen.Basisregisters.GrAr.Provenance;
    using Be.Vlaanderen.Basisregisters.Utilities.HexByteConvertor;
    using Newtonsoft.Json;

    [EventName("MunicipalitySnapshot")]
    public class MunicipalitySnapshot
    {
        public Guid MunicipalityId { get; }
        public string NisCode { get; }
        public MunicipalityStatus? Status { get; }
        public Dictionary<Language, string> Names { get; }
        public List<Language> OfficialLanguages { get; }
        public List<Language> FacilitiesLanguages { get; }
        public string Geometry { get; }
        public Modification LastModificationBasedOnCrab { get; }

        public MunicipalitySnapshot(
            MunicipalityId municipalityId,
            NisCode nisCode,
            MunicipalityStatus? status,
            Dictionary<Language, MunicipalityName> names,
            List<Language> officialLanguages,
            List<Language> facilitiesLanguages,
            ExtendedWkbGeometry? geometry,
            Modification lastModificationBasedOnCrab)
        {
            MunicipalityId = municipalityId;
            NisCode = nisCode;
            Status = status;
            Names = names.ToDictionary(name => name.Key, nameValue => nameValue.Value.Name);
            OfficialLanguages = officialLanguages;
            FacilitiesLanguages = facilitiesLanguages;
            Geometry = geometry ?? string.Empty;
            LastModificationBasedOnCrab = lastModificationBasedOnCrab;
        }
        [JsonConstructor]
        private MunicipalitySnapshot(
            Guid municipalityId,
            string nisCode,
            MunicipalityStatus? status,
            Dictionary<Language, string> names,
            List<Language> officialLanguages,
            List<Language> facilitiesLanguages,
            string geometry,
            Modification lastModificationBasedOnCrab)
            : this(
                new MunicipalityId(municipalityId),
                new NisCode(nisCode),
                status,
                names.ToDictionary(nameForKey => nameForKey.Key, nameForValue => new MunicipalityName(nameForValue.Value, nameForValue.Key)),
                officialLanguages,
                facilitiesLanguages,
                string.IsNullOrEmpty(geometry) ? null : new ExtendedWkbGeometry(geometry.ToByteArray()),
                lastModificationBasedOnCrab)
        { }
    }
}
