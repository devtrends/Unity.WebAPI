Unity.WebAPI
============

Unity.WebAPI is a library that allows the simple integration of Microsoft's Unity IoC container with ASP.NET Web API.


Getting started with Unity.WebAPI
---------------------------------

The easiest way to add Unity.WebApi to your project is via the NuGet package. You can search for unity.webapi using the GUI or type the following into the package manager console in Visual Studio:

> install-package Unity.WebApi

Once installed, just add a call to UnityConfig.RegisterComponents() in the Application_Start method of Global.asax.cs 
and the Web API framework will then use the Unity.WebAPI DependencyResolver to resolve your components.

e.g.
 
    public class WebApiApplication : System.Web.HttpApplication
    {
      protected void Application_Start()
      {
        AreaRegistration.RegisterAllAreas();
        UnityConfig.RegisterComponents();                           // <----- Add this line
        GlobalConfiguration.Configure(WebApiConfig.Register);
        FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
        RouteConfig.RegisterRoutes(RouteTable.Routes);
        BundleConfig.RegisterBundles(BundleTable.Bundles);
      }           
    }  

Add your Unity registrations in the RegisterComponents method of the UnityConfig class. All components that implement IDisposable should be 
registered with the HierarchicalLifetimeManager to ensure that they are properly disposed at the end of the request.

More Information
----------------

You can find out more about what has changed in this release by visiting the <a href="http://blog.feedbackhound.com/taking-ownership-of-unity.mvc-and-unity.webapi">FeedbackHound Blog</a>.
