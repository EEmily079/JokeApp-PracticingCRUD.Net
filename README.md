# JokeApp-PracticingCRUD.Net
-Practicing CRUD with .Net core (MVC)


Jokes Application >>>

1. To able to talk to Data Base>>>> SQL Server (MSSQL)_Entity Framework Core
	go to Dependencies > Right Click > click on Manage NuGet Packages 
 
	- Search this in the package
	Microsoft.EntityFrameworkcore.sqlServer
	- Install this sqlServer package (must be 6.0.6) same as .Net version

	To do Data Migration with SQL need to install 2nd Package 
	- Search 2nd Package 
	Microsoft.EntityframeworkCore.Tools 
	- Install this package (must be 6.0.6)


2.  create Jokes Model 
	(Model file > add new folder (Domain)) then add class Joke
	Add few properties for this Joke class
	Sample code >>>>
		public class Joke
   	 {
       	 	public int Id { get; set; }
        	public string JokeQuestion { get; set; }
        	public string JokeAnswer { get; set; }


        	public Joke()
        	{

        	}


    	}


3. Create Database >>>>>

	To create New Database - Create DBContext Class (can name it like JokeDbContext)
	- add File in project then create class called DBContext.cs 
	that will be inherit from base class called DBContext
	using Microsoft.EntityFrameworkCore; 
	(add this as Library in DBContext.cs)
	- create Constructor (click on class name and press Ctl+.) 
	generate constructor with (option) 

	-create Properties (shoutcut > type prop and double click) 
	change property Dbtype of DbSet<> of <Joke> class (This come from Domain Entity)

	Sample Code>>>>
	public class JokesDbContext : DbContext
   	 {
        	public JokesDbContext(DbContextOptions options) : base(options)
       	 	{
        	}

        	public DbSet <Joke> Jokes{ get; set; }
   	 }


4. Inject it into Program.cs file > to let application know about DbContext and properties.
	code sample >>>> 
	// Add services to the container.
	builder.Services.AddControllersWithViews();
	builder.Services.AddDbContext<JokesDbContext>(options =>
    	options.UseSqlServer(builder.Configuration.
	GetConnectionString("MvcDemoConnectionString")));


5. appsetting.jason (need to add ConnectionString Property) JSON file 
	"MvcDemoConnectionString": "server=;database=;Trusted_connection=true"

	sample code >>>
	"ConnectionStrings": {
    	"MvcDemoConnectionString": "Server=localhost;Database=JokesDb;Trusted_Connection=True;"
  	}

	Database name = JokesDb will create inside MSSQL server. 

5. to create Database inside MSSQL server.
	Tools > Nuget package manager > Console 
	1.	Add-Migration "name something" 
	2.	Update-Database


FOR CRUD Functions >>>>>>>>>>>>>>>>>>>>>>>>>
Controller (JokesController) 

Need to inject DbContext , which is injected in sercvices (program.cs)
	inside JokesController on top of actions. create constructor 
	short key (ctor)
	then call JokesDbContext > give name of that model(this readonly will talk to database.)
	then right click on it and choose > create and assign field..

	Sample code >>>
         public class JokesController : Controller
    	{
        private readonly JokesDbContext jokesDbContext;

        public JokesController(JokesDbContext jokesDbContext)
        {
            this.jokesDbContext = jokesDbContext;
        }
	......... this will be the rest of the code...........
	}


6.Create Add Method with [HttpGet], when user call Add need to show (Add.cshtml)
	sample code >>
	[httpGet]
	  public IActionResult Add()
        {
            return View();
        }

7. when user click Add from navigation, we will show Add.cshtml
	- Create AddJokeViewModel.cs(to tigger action Add) 
		that will be same property as Employee model but no need Id (Id will define database itself)

8. Create Add Mehtod (to add model into database) that will return index
	sample code >> 
	[HttpPost]
        public async Task<IActionResult> Add(AddJokeViewModel addJokeRequest)
        {
            var joke = new Joke()
            {

                JokeQuestion = addJokeRequest.JokeQuestion,
                JokeAnswer = addJokeRequest.JokeAnswer
            };

            await jokesDbContext.Jokes.AddAsync(joke);
            await jokesDbContext.SaveChangesAsync();
            return RedirectToAction("Index");

        }

9. Navigate Page link navigation for Add Jokes, into layout.cshtml 
	Add new Joke is under JokesController(Controller) and action will be (Add) 
	then nav bar name is Add Employee 
	sample code >>> 
 		<li class="nav-item">
 		 <a class="nav-link text-dark" asp-area="" asp-controller="Employees" asp-action="Add">Add Employee</a>
 		</li>
10. Create Add Joke Web Page
	bootstarp link = https://getbootstrap.com/docs/4.1/components/forms/
	inside the form structure add method ="post" action="Add" (this add will trigger Models)
	when user key in input box, we need to tigger and tie to Jokes moedel.
	@model JokesWebApp.Models.AddJokeViewModel
	need to define asp-for="property"

	sample code >> 
	<h1>Add Jokes</h1>
	<form method="post" action="Add" class="mb-5">
    	<div class="mb-3">
        <label for="" class=" = " form-label">Joke Question</label>
        <input type="text" class="form-control" asp-for="JokeQuestion">
    	</div>
    	<div class="mb-3">
        <label for="" class=" = " form-label">Joke Answer</label>
        <input type="text" class="form-control" asp-for="JokeAnswer">
    	/div>
    	<button type="submit" class="btn btn-primary">Submit</button>
	</form>
   
In this action, when user submit new jokes it will trigger Add functions from JokesController
and Route back to index.cshtml after adding this. 

11. Create Index.cshtml >>> Create table in this webpage. 
	model will be list of Joke
	sample code>>>>>
	@model List<JokesWebApp.Models.Domain.Joke>
	@{
    
	}
	<h1>See All Jokes</h1>
	<table class="table">
   	 <thead>
        <tr>
            <th>ID</th>
            <th>Joke Question</th>
            <th>Joke Answer</th>
            <th> </th>
        </tr>
   	 </thead>
    	<tbody>
        @foreach (var joke in Model)
        {
          
            <tr>
                <td>@joke.Id</td>
                <td>@joke.JokeQuestion</td>
                <td>@joke.JokeAnswer</td>
                <td><a href="Jokes/View/@joke.Id">view</a></td>  (this is to view Detail of jokes)
            </tr>
        }
    	</tbody>
	</table>
	
to show above list give directory route in Layout............. 
	 <li class="nav-item">
          <a class="nav-link text-dark" asp-area="" asp-controller="Jokes" asp-action="Index">Jokes</a>
         </li>


12. when user click view from Jokes table, we want to show view.cshtml 
	- Create UpdateJokeViewModel.cs(to tigger form action view) 
	that will be same property as Employee model.(will include ID too)

13. to show this View/Update Joke Create another Page View.cshtml
	This view html is using UpdateJokeViewModel.
	sample code >>> (ID not able to change)
	@model JokesWebApp.Models.UpdateJokeViewModel
	@{
	}
	<h1>View Jokes Details </h1>
	<form method="post" action="Add" class="mb-5">
    	<div class="mb-3">
        <label for="" class=" = " form-label">ID</label>
        <input type="text" class="form-control" asp-for="Id" readonly>
    	</div>
    	<div class="mb-3">
        <label for="" class=" = " form-label">Joke Question</label>
        <input type="text" class="form-control" asp-for="JokeQuestion">
    	</div>
    	<div class="mb-3">
        <label for="" class=" = " form-label">Joke Answer</label>
        <input type="text" class="form-control" asp-for="JokeAnswer">
    	</div>

    	<button type="submit" class="btn btn-primary">Update</button>

when user update data and click submit need to trigger update funciton in View action....

14. Create [HttpGet] in JokesController (view action)
	sample code >>>>
	[HttpGet]
        public async Task<IActionResult> View(int id)
        {
            var joke = await jokesDbContext.Jokes.FirstOrDefaultAsync(x => x.Id == id);

            if (joke != null)
            {
                var veiwModel = new UpdateJokeViewModel()
                {
                    Id = joke.Id,
                    JokeQuestion = joke.JokeQuestion,
                    JokeAnswer = joke.JokeAnswer
                };
                return await Task.Run(() => View("View", veiwModel));
            }
            return RedirectToAction("Index");

        }
	
when user click view from table it will call ID and trigger this function with input id. 
then find joke by id using inner functions. like Jokes.FirstOrDefalutAsync(x => x.id == id);
If found we add new jokeupdatemodel by updating it with data retrieve from the Jokes. 
thenk return it into View but showing found Model. Task.Run (()=> View ("View", viewModel))
if cannot find >  Redirect to action Index


15. to Update Joke > Create [HttpPost] Method in JokesController (View action)
	this action will receive (UpdateJokeViewModel model) 
	and find data in dbContext by using model.Id 
	when it found > update existing data with model data 

	Sample Code >>>
	[HttpPost]
        public async Task<IActionResult> View (UpdateJokeViewModel model)
        {
            var joke = await jokesDbContext.Jokes.FindAsync(model.Id);
            if (joke != null)
            {
                joke.JokeQuestion = model.JokeQuestion;
                joke.JokeAnswer = model.JokeAnswer;

                await jokesDbContext.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");

        }
After Updating data need to SaveChangesAsync and return to Index. 
if cannot find data just redirect to Action Index.

16. Delete Function >>> create one more button inside View html page 
	btn is danger and need to add asp-action , asp-controller
	 <button type="submit" class="btn btn-danger"
        asp-action="Delete" 
        asp-controller="Jokes">Delete</button>
	
	asp-action = "Delete" this will trigger action so > create [httpPost] Delete Action inside JokesController
	sample code >>>>
	
	[HttpPost]
        public async Task<IActionResult> Delete( UpdateJokeViewModel model)
        {
            var joke = await jokesDbContext.Jokes.FindAsync (model.Id);
            if(joke != null)
            {
                jokesDbContext.Jokes.Remove(joke);
                jokesDbContext.SaveChanges();

                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }

	
After user delete route it back to Index action. 		
