namespace MunicipalityRegistry.Municipality
{
    using System.Collections.Generic;
    using Be.Vlaanderen.Basisregisters.GrAr.Provenance;
    using Be.Vlaanderen.Basisregisters.Utilities.HexByteConvertor;
    using Events;

    public partial class Municipality
    {
        private MunicipalityId _municipalityId;
        private NisCode _nisCode;
        private MunicipalityStatus? _status;
        private RetirementDate _retiredDate;

        private readonly Dictionary<Language, MunicipalityName> _names
            = new Dictionary<Language, MunicipalityName>();

        private readonly List<Language> _officialLanguages = new List<Language>();
        private readonly List<Language> _facilitiesLanguages = new List<Language>();

        private ExtendedWkbGeometry _geometry;

        public Modification LastModificationBasedOnCrab { get; private set; }

        private Municipality()
        {
            Register<MunicipalityWasRegistered>(When);
            Register<MunicipalityNisCodeWasDefined>(When);
            Register<MunicipalityNisCodeWasCorrected>(When);

            Register<MunicipalityWasNamed>(When);
            Register<MunicipalityNameWasCorrected>(When);
            Register<MunicipalityNameWasCleared>(When);
            Register<MunicipalityNameWasCorrectedToCleared>(When);

            Register<MunicipalityOfficialLanguageWasAdded>(When);
            Register<MunicipalityOfficialLanguageWasRemoved>(When);

            Register<MunicipalityFacilityLanguageWasAdded>(When);
            Register<MunicipalityFacilitiesLanguageWasRemoved>(When);

            Register<MunicipalityWasDrawn>(When);
            Register<MunicipalityGeometryWasCorrected>(When);
            Register<MunicipalityGeometryWasCleared>(When);
            Register<MunicipalityGeometryWasCorrectedToCleared>(When);

            Register<MunicipalityBecameCurrent>(When);
            Register<MunicipalityWasCorrectedToCurrent>(When);
            Register<MunicipalityWasRetired>(When);
            Register<MunicipalityWasCorrectedToRetired>(When);

            Register<MunicipalityWasImportedFromCrab>(@event => WhenCrabEventApplied());
            Register<MunicipalityNameWasImportedFromCrab>(@event => WhenCrabEventApplied());

            Register<MunicipalitySnapshot>(When);
        }

        private void WhenCrabEventApplied()
        {
            switch (LastModificationBasedOnCrab)
            {
                case Modification.Unknown:
                    LastModificationBasedOnCrab = Modification.Insert;
                    break;

                case Modification.Insert:
                    LastModificationBasedOnCrab = Modification.Update;
                    break;
            }
        }

        private void When(MunicipalityWasRegistered @event)
        {
            _municipalityId = new MunicipalityId(@event.MunicipalityId);
            _nisCode = new NisCode(@event.NisCode);
        }

        private void When(MunicipalityNisCodeWasDefined @event)
        {
            _nisCode = new NisCode(@event.NisCode);
        }

        private void When(MunicipalityNisCodeWasCorrected @event)
        {
            _nisCode = new NisCode(@event.NisCode);
        }

        private void When(MunicipalityWasNamed @event)
        {
            _names[@event.Language] = new MunicipalityName(@event.Name, @event.Language);
        }

        private void When(MunicipalityNameWasCorrected @event)
        {
            _names[@event.Language] = new MunicipalityName(@event.Name, @event.Language);
        }

        private void When(MunicipalityNameWasCleared @event)
        {
            _names.Remove(@event.Language);
        }

        private void When(MunicipalityNameWasCorrectedToCleared @event)
        {
            _names.Remove(@event.Language);
        }

        private void When(MunicipalityOfficialLanguageWasAdded @event)
        {
            _officialLanguages.Add(@event.Language);
        }

        private void When(MunicipalityOfficialLanguageWasRemoved @event)
        {
            _officialLanguages.Remove(@event.Language);
        }

        private void When(MunicipalityFacilityLanguageWasAdded @event)
        {
            _facilitiesLanguages.Add(@event.Language);
        }

        private void When(MunicipalityFacilitiesLanguageWasRemoved @event)
        {
            _facilitiesLanguages.Remove(@event.Language);
        }

        private void When(MunicipalityWasDrawn @event)
        {
            _geometry = new ExtendedWkbGeometry(@event.ExtendedWkbGeometry.ToByteArray());
        }

        private void When(MunicipalityGeometryWasCorrected @event)
        {
            _geometry = new ExtendedWkbGeometry(@event.ExtendedWkbGeometry.ToByteArray());
        }

        private void When(MunicipalityGeometryWasCleared @event)
        {
            _geometry = null;
        }

        private void When(MunicipalityGeometryWasCorrectedToCleared @event)
        {
            _geometry = null;
        }

        private void When(MunicipalityBecameCurrent @event)
        {
            _status = MunicipalityStatus.Current;
        }

        private void When(MunicipalityWasRetired @event)
        {
            _status = MunicipalityStatus.Retired;
            _retiredDate = new RetirementDate(@event.RetirementDate);
        }

        private void When(MunicipalityWasCorrectedToRetired @event)
        {
            _status = MunicipalityStatus.Retired;
            _retiredDate = new RetirementDate(@event.RetirementDate);
        }

        private void When(MunicipalityWasCorrectedToCurrent @event)
        {
            _status = MunicipalityStatus.Current;
        }

        private void When(MunicipalitySnapshot snapshot)
        {
            _municipalityId = new MunicipalityId(snapshot.MunicipalityId);
            _nisCode = new NisCode(snapshot.NisCode);
            _status = snapshot.Status;

            foreach (var name in snapshot.Names)
                _names.Add(name.Key, new MunicipalityName(name.Value, name.Key));

            _officialLanguages.AddRange(snapshot.OfficialLanguages);
            _facilitiesLanguages.AddRange(snapshot.FacilitiesLanguages);

            _geometry = new ExtendedWkbGeometry(snapshot.Geometry.ToByteArray());

            LastModificationBasedOnCrab = snapshot.LastModificationBasedOnCrab;
        }
    }
}
