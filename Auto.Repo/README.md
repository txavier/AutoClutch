AutoClutch
==========

# The simple generic repository
AutoClutch is a tool for getting data. A few years ago I created a
much larger much more complicated generic repository for use in 
the enterprise. While it worked well in .NET 3.5 and Entity Framework
4 adding on more features and extending it to work with the newest
versions of .NET and Entity Framework became a laborious task. I 
decided to create a simple repository with the knowledge I had.
The idea was to target the repository for the most current versions of 
.NET and Entity Framework.  With no restrictions on backwards 
compatibility the idea was to create this simple repository and then 
add on top of it over time other libraries that would give me 
the functionality I need (i.e. Auditing, Item Tracking, etc.,). I 
believed a simple core was the right place to start.

## Data structure
Following the N-Tier course, Part 1 and Part 2, in PluralSight,
Onion Architecture Domain Driven Design was referenced for the layout
of this generic repository.

## Dependency Injection Example
Here is a simple example of how to use structuremap with AutoRepo.

public class DefaultRegistry : Registry {
        public DefaultRegistry() {
            Scan(
                scan => {
                    scan.TheCallingAssembly();
                    scan.WithDefaultConventions();
                });

            For<DbContext>().HybridHttpOrThreadLocalScoped().Use<MyDbContext>();

			// The below line is only needed if you are going to use the generic service 
			// abstraction layer 'AutoClutch.AutoService'.
            For(typeof(IService<>)).Use(typeof(Service<>));		

            For(typeof(IRepository<>)).Use(typeof(Repository<>));

            For<IItemService>().Use<ItemService>();

            For<IUserService>().Use<UserService>();

            Policies.SetAllProperties(prop => prop.OfType<IService<item>>());

            Policies.SetAllProperties(prop => prop.OfType<IItemService>());

            Policies.SetAllProperties(prop => prop.OfType<IUserService>());
        }

## Core Service Constructor
To initialize the repository in your Service class use the following example.

namespace LiteratureAssistant.Core.Services
{
    public class ItemService : Service<item>, IItemService
    {
        private readonly IRepository<item> _itemRepository;
        
        private readonly IService<itemAttribute> _itemAttributeService;

        private readonly IService<templateAttribute> _templateAttributeService;

        public int ItemTemplateId { get; set; }

        public ItemService(IRepository<item> itemRepository, IService<itemAttribute> itemAttributeService,
            IService<templateAttribute> templateAttributeService) :
            base(itemRepository)
        {
            _itemRepository = itemRepository;

            _itemAttributeService = itemAttributeService;

            _templateAttributeService = templateAttributeService;
        }

		:
		:
		:

#Calling the repostory in a method in your core service.
_itemRepository.Update(item);

#Using the Get method and passing a fluent Func to it.
var items = _itemRepository.Get(filter: i => i.itemId == firstItemAttribute.itemId).ToList();

#Check out the project located here for a working project using the suggestions above.
https://github.com/txavier/LiteratureAssistant