using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bogus;
using Bogus.Extensions;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Milabowl.Business.DTOs.Import;
using Milabowl.Business.Import;
using Milabowl.Business.Mappers;
using Milabowl.Infrastructure.Models;
using Milabowl.Test.BaseTestClasses;
using NUnit.Framework;

namespace Milabowl.Test.Business.Import
{
    [TestFixture, Rollback]
    public class DataImportBusinessTest: DbBaseTest
    {
        private readonly IDataImportBusiness _dataImportBusiness;

        public DataImportBusinessTest()
        {
            this._dataImportBusiness = new DataImportBusiness(new FantasyMapper());
        }

        [Test]
        public async Task ShouldImportEvents()
        {
            var bootstrapDTO = new BootstrapRootDTO
            {
                Events = new List<EventDTO> { this.GetEventDto() }
            };
            var eventsFromDb = new List<Event>();

            var events = await this._dataImportBusiness.ImportEvents(this.FantasyContext, bootstrapDTO, eventsFromDb);
            await this.FantasyContext.SaveChangesAsync();

            var eventFromDb = this.FantasyContext.Events.First(e => e.EventId == events[0].EventId);
            this.AssertEventAndEventDtoMatches(eventFromDb, bootstrapDTO.Events[0]);
        }

        [Test]
        public async Task ShouldUpdateEventIfAlreadyInDb()
        {
            var bootstrapDTO = new BootstrapRootDTO
            {
                Events = new List<EventDTO> { this.GetEventDto() }
            };
            var eventInDb = new Event {FantasyEventId = bootstrapDTO.Events[0].Id};
            await this.FantasyContext.Events.AddAsync(eventInDb);
            await this.FantasyContext.SaveChangesAsync(); 
            this.FantasyContext.Entry(eventInDb).State = EntityState.Detached;
            var eventsFromDb = new List<Event>{ eventInDb };

            var events = await this._dataImportBusiness.ImportEvents(this.FantasyContext, bootstrapDTO, eventsFromDb);
            await this.FantasyContext.SaveChangesAsync();

            var eventFromDb = this.FantasyContext.Events.First(e => e.EventId == events[0].EventId);
            this.AssertEventAndEventDtoMatches(eventFromDb, bootstrapDTO.Events[0]);
        }

        private void AssertEventAndEventDtoMatches(Event evt, EventDTO eventDto)
        {
            evt.FantasyEventId.Should().Be(eventDto.Id);
            evt.Name.Should().Be(eventDto.Name);
            evt.Deadline.Should().Be(eventDto.DeadlineTime);
            evt.Finished.Should().Be(eventDto.Finished);
            evt.DataChecked.Should().Be(eventDto.DataChecked);
            evt.GameWeek.Should().Be(1);
        }


        private EventDTO GetEventDto()
        {
            var faker = new Faker<EventDTO>();

            faker.RuleForType(typeof(bool), r => r.Random.Bool());
            faker.RuleForType(typeof(int), r => r.Random.Int());
            faker.RuleForType(typeof(int?), r => r.Random.Int().OrNull(r));
            faker.RuleFor(r => r.Name, r => "Gameweek 1");
            faker.RuleFor(r => r.DeadlineTime, r => r.Date.Soon());

            return faker.Generate();
        }

    }
}
