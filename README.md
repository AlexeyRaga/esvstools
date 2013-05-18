EventStore tools for Visual Studio
=========


**To build**
________

1. Make sure you have VS2012 and VS2102 SDK installed
2. Open the solution and build it.
3. The result package can be found as `bin\Debug\EventStore.VSTools.vsix` or `bin\Release\EventStore.VSTools.vsix`


**To install**
__________

1. Build `EventStore.VSTools.vsix`
2. Double-click on this file.
3. Agree to install.
4. Restart Visual Studio


**To create an EventStore projections project**
___________________________________________

1. In Visual Studio
2. Go to File -> New -> Project
3. Find and select the EventStore -> Projections Project template
4. Click OK.

**To connect with the EventStore**
______________________________

1. Right-click on the projections project node and open Properties
2. Go to Configuration Properties -> EventStore Connection
3. Type in the connection string field: `localhost:2113` (or whatever host name you have the EventStore on)
4. Click OK.
5. Click Save All on the VS toolbar (just in case).

**To run projection as a query**
____________________________

1. Be sure that the Event Store connection is specified
2. Right-click on the projection file you want to run as a query
3. Click on the "Run as a Query" menu.
4. The Query Tool window will pop up and will show you the result of the query. The result will be automatically updated until it the query is done.

**To deploy all the projections into the EventStore**
_________________________________________________

1. Be sure that the Event Store connection is specified
2. Right click on the projections project
3. Click on the "Deploy to EventStore" menu
4. You may watch what is happening in the Output window (the EventStore category)

**To make it better**
_________________

1. Go to https://github.com/AlexeyRaga/esvstools
2. Fork the repository
3. Make it better
4. Do a pull request
5. Have your "Thank you very much" from me, my friend :)
