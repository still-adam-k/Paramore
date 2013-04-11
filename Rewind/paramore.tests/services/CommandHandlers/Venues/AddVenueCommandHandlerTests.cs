﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FakeItEasy;
using Machine.Specifications;
using Paramore.Adapters.Infrastructure.Repositories;
using Paramore.Adapters.Tests.UnitTests.fakes;
using Paramore.Domain.Venues;
using Paramore.Ports.Services.Commands.Venue;
using Paramore.Ports.Services.Handlers.Venues;

namespace Paramore.Adapters.Tests.UnitTests.services.CommandHandlers.Venues
{
    [Subject("A call to the add new venue handler should result in a new venue being added")]
    public class When_adding_a_new_venue
    {
        static AddVenueCommandHandler addVenueCommandHandler;
        static AddVenueCommand addVenueCommand;
        static FakeRepository<Venue, VenueDocument> venueRepository; 
        static IAmAUnitOfWorkFactory uoWFactory;
        static IUnitOfWork uow;
          
        Establish context = () =>
        {
            venueRepository = FakeRepository<Venue, VenueDocument>>();
            uoWFactory = A.Fake<IAmAUnitOfWorkFactory>();
            uow = A.Fake<IUnitOfWork>();

            A.CallTo(() => uoWFactory.CreateUnitOfWork()).Returns(uow);

            addVenueCommand = new AddVenueCommand(
                id: Guid.NewGuid(), 
                venueName: "My Venue Name", 
                address:"Street : MyStreet, City : London, PostCode : SW1 1PL",
                mapURN: "http://www.mysite.com/maps/12345",
                contact: "Name : Mary Alice, EmailAddress : mary.alice@foobar.com: , PhoneNumber : 0111 111 1111");

            addVenueCommandHandler = new AddVenueCommandHandler(venueRepository, uoWFactory);
        };

        Because of = () => addVenueCommandHandler.Handle(addVenueCommand);

        It should_add_a_meeting_to_the_repository = () => A.CallTo(() => venueRepository.Add(A<Venue>.Ignored)).MustHaveHappened();
        It should_ask_the_session_factory_for_a_unit_of_work = () => A.CallTo(() => uoWFactory.CreateUnitOfWork()).MustHaveHappened();
        It should_commit_the_unit_of_work = () => A.CallTo(() => uow.Commit()).MustHaveHappened();


    }
}
