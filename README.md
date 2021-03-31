# LocalParks
Find what's going on in parks around you! 

```
Hosted on Azure: https://localparks.azurewebsites.net/
```

Written by Owen Steele

# Features

* ASP.NET Core, using MVC design and Razor page views

* MSSQL database built with Entity framework Core. With IdentityDBContext

* Has Angular 11 components to build the shopfront  

* RESTful API with JWT token Authentication
  * full functionalilty in API for querying parks, clubs, staff and events
  * CRUD capability for park events
  * Tokens generated in API and in-site.

* Parks, Sports Club, Events and Staff views can be sorted and filtered
  * custom jQuery and AJAX used for empty field validation
  * Events can be created, edited and deleted with the site

* ChartJS used to show report data
  * ChartBuilder built from ground up to automatically create JSON object for the JS chart, in C#

* jQuery used for client-side validation, alongside server-side validation

## UserStore and Identity 

* Can Create, Edit and Delete Users on the site

* Users have an account domain for:
  * API Token generation
  * Seeing Orders from the Angular shopfront
  * Inspect user events scheduled, CRUD support
  * Can alter and edit User information

* User Claims and Roles employed for Authorisation and Authentication Sitewide
