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

## Nuget Package
AutoService includes both the generic repository and a generic service for you to inherit your services from if you wish. https://www.nuget.org/packages/AutoClutch.AutoService/
	
	Install-Package AutoClutch.AutoService
Or if you want just the repository bit
	
	Install-Package AutoClutch.AutoRepo

## Data structure
Following the N-Tier course, Part 1 and Part 2, in PluralSight,
Onion Architecture Domain Driven Design was referenced for the layout
of this generic repository.

## Features Included in the Latest Update
### Audit Logs 
Includes audit support from the Tracker-Enabled-DbContext project (https://github.com/bilal-fazlani/tracker-enabled-dbcontext) (http://bilal-fazlani.github.io/tracker-enabled-dbcontext/).

### String Querying
Includes support for querying the data by fluent or with strings.  String support provided by the Dynamic Linq project
(http://jasonwatmore.com/post/2014/07/16/Dynamic-LINQ-Using-strings-to-sort-by-properties-and-child-object-properties.aspx) (https://github.com/kahanu/System.Linq.Dynamic).

## Dependency Injection Example
Here is a simple example of how to use structuremap with AutoRepo.  Please note the 'TrackerEnabledDbContext.TrackerContext' class inherits from the 'DbContext' class.  So you will have to specify this 'TrackerEnabledDbContext.TrackerContext' for your dependency injection engine to find a concrete class for.

	public class DefaultRegistry : Registry {
	        public DefaultRegistry() {
	            Scan(
	                scan => {
	                    scan.TheCallingAssembly();
	                    scan.WithDefaultConventions();
	                });

	            For<TrackerEnabledDbContext.TrackerContext>().HybridHttpOrThreadLocalScoped().Use<MyDbContext>();
	
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

## Your Code-First Context Class
Please note that your context class has to inherit from 'TrackerEnabledDbContext.TrackerContext'.

		namespace LiteratureAssistant.Data
		{
		    using System;
		    using System.Data.Entity;
		    using System.ComponentModel.DataAnnotations.Schema;
		    using System.Linq;
		    using LiteratureAssistant.Core.Models;
		
		    public partial class MyDbContext : TrackerEnabledDbContext.TrackerContext
		    {
		        public MyDbContext()
		            : base("name=MyDbContext")
		        {
		        }
		
		        public virtual DbSet<changeOrder> changeOrders { get; set; }
		        public virtual DbSet<changeOrderType> changeOrderTypes { get; set; }
		        public virtual DbSet<contract> contracts { get; set; }
		        public virtual DbSet<contractCategory> contractCategories { get; set; }
		        public virtual DbSet<contractor> contractors { get; set; }
		        :
		        :
		        :
		
		        protected override void OnModelCreating(DbModelBuilder modelBuilder)
		        {
		            modelBuilder.Entity<changeOrder>()
		                .Property(e => e.engineerEstimate)
		                .HasPrecision(19, 4);
		
		            modelBuilder.Entity<changeOrder>()
		                .Property(e => e.proposalAmount)
		                .HasPrecision(19, 4);
		
		            modelBuilder.Entity<changeOrder>()
		                .Property(e => e.bwtApprovalAmount)
		                .HasPrecision(19, 4);
		
		            modelBuilder.Entity<changeOrder>()
		                .Property(e => e.registeredAmount)
		                .HasPrecision(19, 4);
		
		            :
		            :
		            :
		        }
		    }
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
	
Or, if your service has inherited IService<item>...

	_itemService.Update(item);


#Entire Object Graph Updating
In previous versions developers using AutoClutch.would have to break apart their disconnected object graph and update each child separately.  This has been done away with in versions 2.0 and later.  Now you as the developer no longer has to worry about updating each child element.  In the example above, 
	_itemRepository.Update(item);
	_itemService.Update(item);
If the disconnected object "item" has a child element in it "itemChild" and "itemChild" has changed then the above command will update "itemChild" in the database.  In previous versions of this library you would have to call "_itemService.update(itemChild)" as a separate call, so this subsequent call is now no longer needed.


##Important
Please note that in order to take advantage of the entire object graph update feature you must follow the table, primary key naming convention like this: "[Table Name]Id". So if your table name is "Employee" then your primary key name should be "EmployeeId".  

One of planned changes, is to include the naming convention for a primary key "Id", "[Table Name]id", or any other Regex match in an array of matches.  If you want to make use of these features right away create a issue and I will try to prioritize this for you.  Or better yet, create a pull request!

#Using the Get method and passing a fluent Func to it.
	var items = _itemRepository.Get(filter: i => i.itemId == firstItemAttribute.itemId).ToList();

Or, if your service has inherited IService<item>...

	var items = _itemService.Get(filter: i => i.itemId == firstItemAttribute.itemId).ToList();
