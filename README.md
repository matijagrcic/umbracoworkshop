#Intro

Welcome to **Extending the Backoffice** Umbraco workshop. The goal of this workshop is to get you full up to speed on how to build your own custom panels and sections so you can extend and bend Umbraco to your will.

In addition to Umbraco, we are going to be using TypeScript, npm, Grunt and Chrome DevTools. Don’t worry if you’re aren’t familiar with any of these: that’s the point of this workshop. Since this workshop is about Umbraco, we will not be deep diving into anything else but feel free to ask, we love discussions.

Questions? Feel free to tweet at me **@matijagrcic**. Corrections or concerns about this page? Please file an issue or make a pull request on the repo.

##Objective

- Learn from Umbraco source code but DON'T touch or change it
- Learn some Chrome DevTool tricks
- **Have fun**

##Value

Ability to modify, replace any part of Umbraco built-in functionality.

#Setup
In order to be able to complete this entire workshop, we need to go ahead and get some tools downloaded.

**Umbraco** - Create an empty project and install it using Nuget ``Install-Package UmbracoCms`` or by downloading from https://our.umbraco.org/download  
**Node and NPM** - https://nodejs.org/, https://www.npmjs.com/package/npm-windows-upgrade or by using Chocolatey (https://chocolatey.org/) and running a  `choco install nodejs.install` command  
**Chrome Canary** - https://www.google.com/chrome/browser/canary.html 

##Extensions

**Web Extension Pack** - https://visualstudiogallery.msdn.microsoft.com/f3b504c6-0095-42f1-a989-51d5fc2a8459  
**Task Runner Explorer** - https://visualstudiogallery.msdn.microsoft.com/8e1b4368-4afb-467a-bc13-9650572db708  
**Snippet Essentials** - https://visualstudiogallery.msdn.microsoft.com/2758202e-f99a-4163-9a73-254e64917632  
**BuildVision** - https://visualstudiogallery.msdn.microsoft.com/23d3c821-ca2d-4e1a-a005-4f70f12f77ba  
**Bootstrap Snippet Pack** - https://visualstudiogallery.msdn.microsoft.com/e82e7862-f731-4183-a27a-3a44b261bfe5

###Extra
**DuoTone Dark Space** - https://studiostyl.es/schemes/duotone-dark-space  
**Nova** - https://studiostyl.es/schemes/nova


#Let's start

We will use .NET WebAPI to comunicate and provide data to our plugins, datatypes and sections so we will need to modify the `Global.asax` by registering the WebAPI routes by adding `WebApiConfig` in the `App_Start` and then adding to the global configuration.

 `GlobalConfiguration.Configure(WebApiConfig.Register);`


##TaskRunner

Before we can examine and test our TypeScript files we would need a task runner running to compile them. For the purpose of simplicity we've used Grunt for that task but you can use Gulp or Webpack it really doesn't matter. Copy the `gruntfile.js` and `package.json` to your solution.


You can run Grunt manually from the command line or use the previously mentioned Task Runner Explorer extension which is integrated inside VS 2105. Or use Task Runner Explorer for VS 2013 available at https://visualstudiogallery.msdn.microsoft.com/8e1b4368-4afb-467a-bc13-9650572db708.

##App_Plugins


Next add everything under the `App_Plugins` folder to your own solution.

You'll see there's a folder named **tweaks** and it's where all the magic happens. Putting a 
**package.manifest** inside tweaks allows us to load any CSS or JS and modify Umbraco look and feel, even the login page (https://twitter.com/matijagrcic/status/694827805910142976).

To be able to build our custom panels we'll leverage Angular `$http interceptors` https://docs.angularjs.org/api/ng/service/$http#interceptors

> For purposes of global error handling, authentication, or any kind of synchronous or asynchronous pre-processing of request or postprocessing of responses, it is desirable to be able to intercept requests before they are handed to the server and responses before they are handed over to the application code that initiated these requests. 


##WebAPI controller
Copy the `MediaApiController` along with **Models**, **Providers**, **Sections** and **Settings** folder to your solution.

##*.config
Copy and include `UmbracoCustomSettings.config` to your solution. To read the `UmbracoCustomSettings` we would need to create a custom section in the `web.config`

    <section name="UmbracoCustomSettings" type="ExtendingUmbraco.Providers.XmlSection`1[[ExtendingUmbraco.Settings.UmbracoCustomSettings, ExtendingUmbraco, Version=4.0.0.0, Culture=neutral]], ExtendingUmbraco" />

and then use that section to point to our file

    <UmbracoCustomSettings configSource="UmbracoCustomSettings.config" />

You can notice that we are using previously copied **Providers** to read the settings.


##Sections

To hook and new custom section we will need to modify several files so that Umbraco knows to display our section.
The first file is **applications.config**. Add the following line to it.

    <add alias="utilities" name="Utilities" icon="icon-utilities" sortOrder="99" />

Then we would need to modify the **Dashboard.config** by adding our own **StartupUtilitiesDashboardSection** by
which we describe our section.

Last file to modify is called **trees.config** in which we'll include the following

     <add initialize="true" sortOrder="0" alias="mediaMaintenance" application="utilities" title="Media Maintenance" iconClosed="icon-media" iconOpen="icon-media color-blue" type="ExtendingUmbraco.Sections.MediaMaintenanceTreeController, ExtendingUmbraco" />


## What's next

Let's dive into code and discuss.

